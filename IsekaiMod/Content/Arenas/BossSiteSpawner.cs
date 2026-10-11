using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints.Area;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.PubSubSystem;
using Kingmaker.View;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

public sealed class BossSiteSpawner : IAreaHandler, ISubscriber, IGlobalSubscriber, IGameModeHandler, ICutsceneHandler
{
	internal enum SpawnDecision
	{
		Spawn,
		NotASite,
		AlreadyPresent,
		Refused,
		Closed
	}

	private static bool s_Subscribed;

	internal const float SpawnDistance = 14f;

	private static string s_PendingRetryArea;

	public static void Add()
	{
		if (!s_Subscribed)
		{
			s_Subscribed = true;
			EventBus.Subscribe(new BossSiteSpawner());
		}
	}

	internal static SpawnDecision Decide(bool isSite, bool markedBossPresent, bool armyBattle, bool cutscene, bool fightOpen)
	{
		if (!isSite)
		{
			return SpawnDecision.NotASite;
		}
		if (markedBossPresent)
		{
			return SpawnDecision.AlreadyPresent;
		}
		if (armyBattle | cutscene)
		{
			return SpawnDecision.Refused;
		}
		if (!fightOpen)
		{
			return SpawnDecision.Closed;
		}
		return SpawnDecision.Spawn;
	}

	internal static bool FightOpen(BossSiteKind kind, int act, QuestState questState, int chapter, IList<QuestObjectiveState> actStates, bool arbiterRequirementsMet, bool ownsEncounters)
	{
		if (kind == BossSiteKind.Anomaly)
		{
			return BossSites.AnomalyOffered(act, chapter, ownsEncounters);
		}
		if (!BossSites.RiftOffered(questState, chapter, actStates))
		{
			return false;
		}
		if (BossSites.AccessibleRiftAct(chapter, actStates) != act)
		{
			return false;
		}
		return (act != 6) | arbiterRequirementsMet;
	}

	internal static bool MarkerIsSiteBoss(IDictionary<string, string> marker, BossSiteKind kind, int act)
	{
		if (marker == null)
		{
			return false;
		}
		if (kind == BossSiteKind.Rift)
		{
			return DimensionalRiftIncursions.RiftActFromMarker(marker) == act;
		}
		BarrenBossType? barrenBossType = CosmicBarrenBosses.BossTypeFromMarker(marker);
		if (barrenBossType.HasValue)
		{
			return CosmicBarrenBosses.ActForBoss(barrenBossType.Value) == act;
		}
		return false;
	}

	internal static bool ShouldRetry(SpawnDecision decision)
	{
		return decision == SpawnDecision.Refused;
	}

	public void OnAreaDidLoad()
	{
		s_PendingRetryArea = null;
		TryPlaceBoss();
	}

	public void OnGameModeStop(GameModeType gameMode)
	{
		ScheduleRetry();
	}

	public void OnGameModeStart(GameModeType gameMode)
	{
		if (gameMode == GameModeType.Default)
		{
			ScheduleRetry();
		}
	}

	public void HandleCutsceneStopped(CutscenePlayerData cutscene)
	{
		ScheduleRetry();
	}

	public void HandleCutsceneStarted(CutscenePlayerData cutscene, bool queued)
	{
	}

	public void HandleCutsceneRestarted(CutscenePlayerData cutscene)
	{
	}

	public void HandleCutscenePaused(CutscenePlayerData cutscene, CutscenePauseReason reason)
	{
	}

	public void HandleCutsceneResumed(CutscenePlayerData cutscene)
	{
	}

	private static void ScheduleRetry()
	{
		try
		{
			string text = s_PendingRetryArea;
			if (string.IsNullOrEmpty(text) || Game.Instance == null)
			{
				return;
			}
			if (!string.Equals(Game.Instance.CurrentlyLoadedArea?.name, text, StringComparison.Ordinal))
			{
				s_PendingRetryArea = null;
				return;
			}
			Game.Instance.ScheduleAction(delegate
			{
				try
				{
					if (s_PendingRetryArea != null)
					{
						if (!string.Equals(Game.Instance?.CurrentlyLoadedArea?.name, s_PendingRetryArea, StringComparison.Ordinal))
						{
							s_PendingRetryArea = null;
						}
						else
						{
							TryPlaceBoss();
						}
					}
				}
				catch (Exception ex2)
				{
					Main.IsekaiContext.Logger.LogError("[BossSites] Retrying a refused site threw: " + ex2);
				}
			});
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[BossSites] Could not schedule a refused site's retry: " + ex);
		}
	}

	private static void TryPlaceBoss()
	{
		try
		{
			BossSites.SiteDefinition site = BossSites.CurrentSite();
			if (site == null)
			{
				return;
			}
			Game instance = Game.Instance;
			bool markedBossPresent = (instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>()?.ToList() ?? new List<UnitEntityData>()).Any((UnitEntityData u) => u != null && MarkerIsSiteBoss(SaveScopedStorage.SpawnMarker(u), site.Kind, site.Act));
			int valueOrDefault = (instance?.Player?.Chapter).GetValueOrDefault();
			QuestObjectiveState[] actStates = IsekaiTransmigrationQuest.ActStates();
			bool arbiterRequirementsMet = site.Kind == BossSiteKind.Rift && site.Act == 6 && IsekaiTransmigrationQuest.AreAllSideQuestsCompleted(out var _);
			bool fightOpen = FightOpen(site.Kind, site.Act, IsekaiTransmigrationQuest.QuestStateInSave(), valueOrDefault, actStates, arbiterRequirementsMet, IsekaiTransmigrationQuest.PlayerCanStartTransmigration());
			SpawnDecision spawnDecision = Decide(isSite: true, markedBossPresent, CampaignEncounterGuard.IsArmyBattle(null), CampaignEncounterGuard.IsCutscene(), fightOpen);
			s_PendingRetryArea = ((!ShouldRetry(spawnDecision)) ? null : instance?.CurrentlyLoadedArea?.name);
			if (spawnDecision != SpawnDecision.Spawn)
			{
				Main.IsekaiContext.Logger.Log($"[BossSites] {site.Stem}: {spawnDecision}; nothing is placed" + (ShouldRetry(spawnDecision) ? " yet, and it is tried again when that ends." : "."));
				return;
			}
			Vector3 position = SpawnPosition(site);
			if (((site.Kind == BossSiteKind.Rift) ? DimensionalRiftIncursions.SpawnRiftBossAt(site.Act, position) : CosmicBarrenBosses.SpawnBossAt(CosmicBarrenBosses.BossTypeForAct(site.Act), position)) == null)
			{
				Main.IsekaiContext.Logger.LogError("[BossSites] " + site.Stem + ": the boss could not be placed.");
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[BossSites] Could not place a site's boss: " + ex);
		}
	}

	private static Vector3 SpawnPosition(BossSites.SiteDefinition site)
	{
		BlueprintAreaEnterPoint blueprintAreaEnterPoint = BossSites.SiteEnterPointFor(site.Kind, site.Act);
		AreaEnterPoint areaEnterPoint = ((blueprintAreaEnterPoint != null) ? AreaEnterPoint.FindAreaEnterPointOnScene(blueprintAreaEnterPoint) : null);
		if (areaEnterPoint != null)
		{
			Transform transform = areaEnterPoint.transform;
			return transform.position + transform.forward * 14f;
		}
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (!(unitEntityData != null))
		{
			return Vector3.zero;
		}
		return unitEntityData.Position + unitEntityData.OrientationDirection * 14f;
	}

	public void OnAreaBeginUnloading()
	{
		s_PendingRetryArea = null;
	}
}
