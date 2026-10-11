using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Config;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.QA.Statistics;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Quests;

public class DimensionalRiftIncursions : IUnitHandler, ISubscriber, IGlobalSubscriber, IUnitSpawnHandler, IAreaHandler
{
	private static bool _initialized = false;

	private static DimensionalRiftIncursions _instance;

	internal static readonly string[] RetiredSummonAbilityNames = new string[6] { "SummonRiftBossAbility", "SummonBarrenBossAct1Ability", "SummonBarrenBossAct2Ability", "SummonBarrenBossAct3Ability", "SummonBarrenBossAct4Ability", "SummonBarrenBossAct5Ability" };

	private static readonly List<UnitEntityData> _calledHereThisVisit = new List<UnitEntityData>();

	internal const string VoidHorrorBaseGuid = "ffc75a2dc5cd08f4093a4898b6db3efe";

	internal const string KenjiBaseGuid = "4fb235855e3ea6b47b9ef1295a4f5bdb";

	internal const string MalakorBaseGuid = "d9b4d98ff9c519c4bb2c164d4352a587";

	internal const string RiftTitanBaseGuid = "eb0b4e6a01f6b30449f40cd03feb3c77";

	internal const string ThresholdHeraldBaseGuid = "520cbe63260f488ebf883b0f6c047b55";

	internal const string ArbiterBaseGuid = "8de6736828d308242b5500afd8db5819";

	private static BlueprintUnit BossAct1_VoidHorror;

	private static BlueprintUnit BossAct2_Kenji;

	private static BlueprintUnit BossAct3_Malakor;

	private static BlueprintUnit BossAct4_RiftTitan;

	private static BlueprintUnit BossAct5_Herald;

	private static BlueprintUnit BossAct6_Arbiter;

	internal const string ActMarkerKey = "Act";

	public static BlueprintAbility SummonRiftBossAbility;

	public static BlueprintFeature DimensionalRiftsFeature;

	public static DimensionalRiftIncursions Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DimensionalRiftIncursions();
			}
			return _instance;
		}
	}

	public static void Init()
	{
		if (!_initialized)
		{
			_initialized = true;
			EventBus.Subscribe(Instance);
		}
	}

	public void HandleUnitDeath(UnitEntityData unit)
	{
		OnUnitDie(unit);
	}

	public void HandleUnitDestroyed(UnitEntityData unit)
	{
		_calledHereThisVisit.Remove(unit);
	}

	public void HandleUnitSpawned(UnitEntityData unit)
	{
	}

	internal static int RemoveRetiredSummons()
	{
		int num = 0;
		try
		{
			List<UnitEntityData> list = Game.Instance?.Player?.AllCharacters;
			if (list == null)
			{
				return 0;
			}
			string[] retiredSummonAbilityNames = RetiredSummonAbilityNames;
			foreach (string name in retiredSummonAbilityNames)
			{
				BlueprintAbility modBlueprint = BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, name);
				if (modBlueprint == null)
				{
					continue;
				}
				foreach (UnitEntityData item in list.ToList())
				{
					if (item?.Descriptor != null && item.Descriptor.HasFact(modBlueprint))
					{
						item.Descriptor.RemoveFact(modBlueprint);
						num++;
					}
				}
			}
			if (num > 0)
			{
				Main.IsekaiContext.Logger.Log($"[DimensionalRift] Removed {num} retired summon abilities from the party.");
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[DimensionalRift] Could not remove the retired summon abilities: " + ex);
		}
		return num;
	}

	private static void Despawn(UnitEntityData unit)
	{
		try
		{
			if (!(unit == null) && !unit.IsDisposed && !unit.Destroyed)
			{
				UnitDescriptor descriptor = unit.Descriptor;
				if (descriptor == null || descriptor.State?.IsDead != true)
				{
					unit.MarkForDestroy();
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[DimensionalRift] Error removing a rift boss: " + ex);
		}
	}

	public void OnAreaBeginUnloading()
	{
		foreach (UnitEntityData item in _calledHereThisVisit.ToList())
		{
			Despawn(item);
		}
		_calledHereThisVisit.Clear();
	}

	public void OnAreaDidLoad()
	{
		_calledHereThisVisit.Clear();
		RemoveRetiredSummons();
		try
		{
			Game instance = Game.Instance;
			if ((instance != null && instance.Player?.IsInCombat == true) || BossSites.CurrentSite() != null)
			{
				return;
			}
			IEnumerable<UnitEntityData> enumerable = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>();
			if (enumerable == null)
			{
				return;
			}
			foreach (UnitEntityData item in enumerable.ToList())
			{
				if (item != null && SaveScopedStorage.SpawnKindOf(item) == "RiftIncursionBoss")
				{
					Despawn(item);
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[DimensionalRift] Error clearing rift bosses left in the area: " + ex);
		}
	}

	private static string RefusalMessage(CosmicBarrenBosses.SummonRefusal refusal)
	{
		return refusal switch
		{
			CosmicBarrenBosses.SummonRefusal.GlobalMap => ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[Dimensional Rift]</b></color> No breach can open while you travel the world map. Enter an area first."), 
			CosmicBarrenBosses.SummonRefusal.InCombat => ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[Dimensional Rift]</b></color> The breach will not open while you are already fighting."), 
			CosmicBarrenBosses.SummonRefusal.ArmyBattle => ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[Dimensional Rift]</b></color> The breach will not open while the crusade's armies are in the field."), 
			CosmicBarrenBosses.SummonRefusal.Dialogue => ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[Dimensional Rift]</b></color> The breach will not open in the middle of a conversation."), 
			CosmicBarrenBosses.SummonRefusal.Cutscene => ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[Dimensional Rift]</b></color> The breach will not open while a scene is playing."), 
			CosmicBarrenBosses.SummonRefusal.Coliseum => ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[Dimensional Rift]</b></color> The Planar Coliseum and the boss sites keep their own breaches. Call the rift out in the world."), 
			_ => null, 
		};
	}

	private static BlueprintUnit CreateBossCopy(string vanillaGuid, string name, string displayName)
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>(vanillaGuid);
		if (blueprint == null)
		{
			Main.IsekaiContext.Logger.LogError("[DimensionalRiftIncursions] Rift boss " + name + ": vanilla unit " + vanillaGuid + " not found.");
			return null;
		}
		return blueprint.CreateCopy(Main.IsekaiContext, name, delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, displayName);
		});
	}

	public static void CreateBlueprints()
	{
		Sprite Icon_Rift = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("dd3dacafcf40a0145a5824c838e2698d"))?.m_Icon ?? AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		BossAct1_VoidHorror = CreateBossCopy("ffc75a2dc5cd08f4093a4898b6db3efe", "RiftBossAct1VoidHorrorUnit", "The Void Horror");
		BossAct2_Kenji = CreateBossCopy("4fb235855e3ea6b47b9ef1295a4f5bdb", "RiftBossAct2KenjiUnit", "Kenji the Exiled Ronin");
		BossAct3_Malakor = CreateBossCopy("d9b4d98ff9c519c4bb2c164d4352a587", "RiftBossAct3MalakorUnit", "Malakor the Shadow Weaver");
		BossAct4_RiftTitan = CreateBossCopy("eb0b4e6a01f6b30449f40cd03feb3c77", "RiftBossAct4RiftTitanUnit", "The Abyssal Rift Titan");
		BossAct5_Herald = CreateBossCopy("520cbe63260f488ebf883b0f6c047b55", "RiftBossAct5ThresholdHeraldUnit", "Herald of the Closed Threshold");
		BossAct6_Arbiter = CreateBossCopy("8de6736828d308242b5500afd8db5819", "RiftBossAct6ArbiterUnit", "The Grand Arbiter of the Loop");
		SummonRiftBossAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonRiftBossAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Open Dimensional Rift: Incursion Boss");
			bp.SetDescription(Main.IsekaiContext, "Tears open an unstable dimensional anomaly to draw forth the planar incursion entity matching your current progression (Acts 1-6).\nDefeating them advances 'The Outer Threshold' questline and uncovers the reality of the temporal loop.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Rift;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle<ContextActionSummonRiftBoss>();
			});
		});
		DimensionalRiftsFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "DimensionalRiftsFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Dimensional Incursions");
			bp.SetDescription(Main.IsekaiContext, "You sense the rifts of The Outer Threshold wherever they tear open. Each open rift act waits behind the Dimensional Breach on the world map; where no road leads, in the Midnight Isles and at the Threshold, the Otherworldly Emissary opens the way. Defeating each incursion entity advances the questline.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Rift;
			bp.IsClassFeature = true;
		});
	}

	public static void SpawnRiftBoss()
	{
		Instance.Spawn();
	}

	internal static int ResolveAct(int chapter, IList<QuestObjectiveState> actStates)
	{
		if (actStates == null || actStates.Count < 6)
		{
			return 0;
		}
		int num = Math.Min(Math.Max(chapter, 1), 5);
		for (int i = 1; i <= num; i++)
		{
			switch (actStates[i - 1])
			{
			default:
				return i;
			case QuestObjectiveState.Failed:
				return 0;
			case QuestObjectiveState.Completed:
				break;
			}
		}
		if (num < 5)
		{
			return 0;
		}
		if (actStates[5] != QuestObjectiveState.Started)
		{
			return 0;
		}
		return 6;
	}

	internal static bool ShouldSettleKill(QuestState questState, QuestObjectiveState actState)
	{
		if (questState == QuestState.Completed || questState == QuestState.Failed)
		{
			return false;
		}
		if (actState != QuestObjectiveState.None)
		{
			return actState == QuestObjectiveState.Started;
		}
		return true;
	}

	internal static int RiftActFromMarker(IDictionary<string, string> marker)
	{
		if (SaveScopedStorage.Get(marker, "Kind") != "RiftIncursionBoss")
		{
			return 0;
		}
		int num = SaveScopedStorage.GetInt(marker, "Act");
		if (num < 1 || num > 6)
		{
			return 0;
		}
		return num;
	}

	public void Spawn()
	{
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (unitEntityData == null || Game.Instance.Player.QuestBook == null)
		{
			return;
		}
		Game instance = Game.Instance;
		CosmicBarrenBosses.SummonRefusal summonRefusal = CosmicBarrenBosses.RefusalFor(instance.CurrentlyLoadedArea?.IsGlobalMap ?? false, instance.Player.IsInCombat, CampaignEncounterGuard.IsArmyBattle(null), CampaignEncounterGuard.IsDialogue(), CampaignEncounterGuard.IsCutscene(), CampaignEncounterGuard.IsColiseum(null) || BossSites.CurrentSite() != null, Math.Max(1, instance.Player.Chapter), 1);
		if (summonRefusal != CosmicBarrenBosses.SummonRefusal.None)
		{
			Quiet(RefusalMessage(summonRefusal));
			return;
		}
		QuestState questState = IsekaiTransmigrationQuest.QuestStateInSave();
		if (questState == QuestState.Completed || questState == QuestState.Failed)
		{
			Quiet(ConstellationText.Localized("Constellation.Rift", "<color=#3F3F3F><i>[Dimensional Rift] The Outer Threshold is resolved in this cycle. No breach answers the call.</i></color>"));
			return;
		}
		QuestObjectiveState[] array = IsekaiTransmigrationQuest.ActStates();
		int num = ResolveAct(Game.Instance.Player.Chapter, array);
		switch (num)
		{
		case 0:
			Quiet((array[5] == QuestObjectiveState.Completed) ? ConstellationText.Localized("Constellation.Rift", "<color=#3F3F3F><i>[Dimensional Rift] The Grand Arbiter has already fallen in this cycle. No breach answers the call.</i></color>") : ConstellationText.Localized("Constellation.Rift", "<color=#3F3F3F><i>[Dimensional Rift] Every breach this chapter can reach is sealed. The next incursion stirs in a later chapter.</i></color>"));
			return;
		case 6:
		{
			if (!IsekaiTransmigrationQuest.AreAllSideQuestsCompleted(out var missingQuests))
			{
				ReportArbiterRequirements(missingQuests);
				return;
			}
			break;
		}
		}
		try
		{
			if (Game.Instance?.LoadedAreaState?.MainState != null)
			{
				UnitEntityData unitEntityData2 = SpawnRiftBossAt(num, unitEntityData.Position + unitEntityData.OrientationDirection * 8f);
				if (unitEntityData2 != null)
				{
					_calledHereThisVisit.Add(unitEntityData2);
				}
				unitEntityData2?.CombatState?.Engage(unitEntityData);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error spawning dimensional rift boss: " + ex);
		}
	}

	internal static BlueprintUnit BossForAct(int act)
	{
		return act switch
		{
			1 => BossAct1_VoidHorror, 
			2 => BossAct2_Kenji, 
			3 => BossAct3_Malakor, 
			4 => BossAct4_RiftTitan, 
			5 => BossAct5_Herald, 
			6 => BossAct6_Arbiter, 
			_ => null, 
		};
	}

	private static string ArrivalLine(int act)
	{
		return act switch
		{
			1 => "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"A residual sliver of the space between dimensions clings to your passage. Cleanse it.\"</i>", 
			2 => "<color=#5B2A86><b>[Constellation - Cayden Cailean]</b></color>: <i>\"Another wandering soul with a blade? Let's see whose martial prowess is real!\"</i>", 
			3 => "<color=#5B2A86><b>[Constellation - Calistria]</b></color>: <i>\"A creature of darkness feasting on the rift's power. Tear his shadows apart, champion.\"</i>", 
			4 => "<color=#7A1010><b>[Constellation - Asmodeus]</b></color>: <i>\"An uncontrolled eruption of abyssal brutality. Slay it with unyielding authority.\"</i>", 
			5 => "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The guardian at the final gate stirs. Claim dominion over the threshold.\"</i>", 
			6 => "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The temporal apparatus trembles. The Arbiter emerges to preserve the closed circle. Will you submit, or attempt the impossible?\"</i>", 
			_ => null, 
		};
	}

	internal static UnitEntityData SpawnRiftBossAt(int act, Vector3 position)
	{
		BlueprintUnit blueprintUnit = BossForAct(act);
		if (blueprintUnit == null || Game.Instance?.LoadedAreaState?.MainState == null)
		{
			return null;
		}
		UnitEntityData unitEntityData = Game.Instance.Player?.MainCharacter.Value;
		position = IsekaiNpcUnit.PlaceWithinReach(position, unitEntityData?.OrientationDirection ?? Vector3.forward, unitEntityData?.Position ?? position);
		UnitEntityData unitEntityData2 = Game.Instance.EntityCreator.SpawnUnit(blueprintUnit, position, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
		if (unitEntityData2 == null)
		{
			return null;
		}
		unitEntityData2.SwitchFactions(BlueprintRoot.Instance.Cheats.Enemy.Faction, resetAttackFactions: true);
		SaveScopedStorage.MarkSpawned(unitEntityData2, "RiftIncursionBoss");
		Dictionary<string, string> dictionary = SaveScopedStorage.SpawnMarker(unitEntityData2);
		if (dictionary != null)
		{
			dictionary["Act"] = act.ToString();
		}
		DimensionalReinforcements.RegisterExternalSpawnedMob(unitEntityData2.UniqueId);
		string text = ArrivalLine(act);
		if (text != null)
		{
			Voice(text);
		}
		PostLog(ConstellationText.LocalizedFormat("Constellation.Rift", "<color=#5B2A86><b>[Dimensional Rift]</b></color> <b>{0}</b> has breached the threshold into reality!", unitEntityData2.CharacterName));
		return unitEntityData2;
	}

	internal static void ReportArbiterRequirements(IEnumerable<string> missingQuests)
	{
		Quiet("<color=#7A1010><b>=======================================================</b></color>");
		Quiet(ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[Loop Boundary Enforcement] THE TEMPORAL NEXUS REJECTS YOUR FREQUENCY!</b></color>"));
		AddedContent addedContent = Main.IsekaiContext.AddedContent;
		Quiet((addedContent == null || addedContent.EnableDimensionalArena) ? ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>The Grand Arbiter will not answer until every rift act is sealed and every arena of the Planar Coliseum is won.</b></color>") : ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>The Grand Arbiter will not answer until every rift act is sealed.</b></color>"));
		Quiet(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>Still unresolved in this cycle:</b></color>"));
		foreach (string item in missingQuests ?? Array.Empty<string>())
		{
			Quiet(ConstellationText.LocalizedFormat("Constellation.Rift", "  <color=#7A1010>- {0}</color>", item));
		}
		Voice(ConstellationText.Localized("Constellation.Rift", "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"Incomplete vectors cannot pierce the threshold. Return when the paradoxes are resolved.\"</i>"));
		Voice(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Constellation - The Lantern King]</b></color>: <i>\"Missed a cue, kid! Go finish the scene you skipped, then knock again!\"</i>"));
		Quiet("<color=#7A1010><b>=======================================================</b></color>");
	}

	public void OnUnitDie(UnitEntityData unit)
	{
		if (unit == null)
		{
			return;
		}
		int num;
		try
		{
			num = RiftActFromMarker(SaveScopedStorage.SpawnMarker(unit));
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error reading a rift boss marker: " + ex);
			return;
		}
		if (num == 0)
		{
			return;
		}
		Quiet("<color=#5B2A86><b>=======================================================</b></color>");
		PostLog(ConstellationText.LocalizedFormat("Constellation.Rift", "<color=#5B2A86><b>[Dimensional Incursion] Rift Entity for Act {0} DEFEATED!</b></color>", num));
		if (!IsekaiTransmigrationQuest.CanSettleAct(num))
		{
			Quiet(ConstellationText.Localized("Constellation.Rift", "<color=#3F3F3F><i>[Dimensional Incursion] This breach was already sealed in this cycle. The Outer Court has nothing further to pay for it.</i></color>"));
			Quiet("<color=#5B2A86><b>=======================================================</b></color>");
			return;
		}
		IsekaiTransmigrationQuest.CompleteAct(num);
		switch (num)
		{
		case 1:
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The anomaly collapses. The cosmic tether binds tighter to your vessel.\"</i>"));
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Echo Fragment]</b></color> A strange prickle of deja vu washes over you... as if your blade has sliced this exact geometry a hundred times before."));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Constellation - Cayden Cailean]</b></color>: <i>\"Whew, nice slice! Wait... didn't you parry to the left last time?\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#133E71><b>[Constellation - Desna]</b></color>: <i>\"Cayden, hush! Don't confuse the poor mortal.\"</i>"));
			break;
		case 2:
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Echo Journal Recovered]</b></color> Kenji whispers with his dying breath: <i>\"You don't remember, do you...? How many times we've bled on these stones... how many crusades ended in dust... Wake up... break the circle...\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[Constellation - Asmodeus]</b></color>: <i>\"A discarded pawn from an earlier run. His failure was catalogued. Yours is currently entertaining.\"</i>"));
			break;
		case 3:
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[The Curtain Falls]</b></color> Malakor's shadowy form violently disintegrates, hissing: <i>\"Fools! The Fifth Crusade already ended in the Prime Timeline centuries ago! You are inside an artificial playback loop engineered by the Outer God for celestial entertainment!\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#133E71><b>[Constellation - Iomedae (Parallel Echo)]</b></color>: <i>\"Silence, abyssal shadow! Knight-Commander, your valor is real! Do not falter before planar deception!\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Constellation - The Lantern King]</b></color>: <i>\"BWAHAHA! Oops! Someone leaked the script! Don't look behind the curtain, kid, it ruins the comedy!\"</i>"));
			break;
		case 4:
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Key of Causal Rupture Claimed]</b></color> The immense rift titan dissolves into pure cosmic energy."));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#133E71><b>[Yog-Sothoth Whispers In Your Mind]</b></color>: <i>\"All moments on the sphere are eternal. You have died, won, ascended, and restarted ten thousand times. It is perfection. Why would you ever desire an unwritten future?\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Constellation - Cayden Cailean]</b></color>: <i>\"Don't listen to the tentacle-sphere, kid! A loop ain't living, it's just being stuck in a taproom that never pours fresh brew!\"</i>"));
			break;
		case 5:
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Perimeter Breached]</b></color> The Herald of the Closed Threshold collapses. The mechanism maintaining the temporal loop stands completely exposed!"));
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Cosmic Revelation]</b></color> <b>The Final Quest Objective Unlocked: Shatter the Infinite Loop!</b> At the Threshold, the Otherworldly Emissary opens the last breach, to the Grand Arbiter!"));
			break;
		case 6:
			Quiet("<color=#5B2A86><b>=======================================================</b></color>");
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[THE TEMPORAL NEXUS SHATTERS!]</b></color>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#3F3F3F><b>[CONSTELLATION STREAM GLITCHING...]</b></color>"));
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#7A1010><b>[ALERT: LOOP GUARDIAN DESTROYED - THE FINAL CHOICE IS OPEN]</b></color>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The Arbiter is unmade. The circle stands open, and only the choice you make at the Threshold can break it or close it again.\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Constellation - The Lantern King]</b></color>: <i>\"YOU DID IT! You punched the referee! Now go pick your ending, kid, the whole audience is watching!\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Constellation - Cayden Cailean]</b></color>: <i>\"RAISE YOUR HORNS, GODS AND MORTALS! To freedom! To an unwritten dawn!\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Rift", "<color=#133E71><b>[Constellation - Iomedae (Parallel Echo)]</b></color>: <i>\"May the unwritten footsteps of your own destiny guide Golarion forever forward.\"</i>"));
			PostLog(ConstellationText.Localized("Constellation.Rift", "<color=#5B2A86><b>[Arbiter Defeated]</b></color> Choose a loop-breaking ending at the Grand Final to rise as <b>Sovereign of the Unwritten Dawn</b>!"));
			break;
		}
		int num2 = num * 3000;
		int num3 = num * 200;
		if (num == 6)
		{
			num2 = 20000;
			num3 = 2500;
		}
		Game.Instance.Player.GainPartyExperience(num2, ExperienceGainStatistic.GainType.Mob);
		DivineTokens.AddClaimedCoins(num3, (num == 6) ? ConstellationText.Localized("CosmicCoins.Sponsor", "The Free Multiverse") : ConstellationText.Localized("CosmicCoins.Sponsor", "Yog-Sothoth & The Outer Court"));
		Quiet(ConstellationText.LocalizedFormat("Constellation.Rift", "<color=#5B2A86><b>[Rewards]</b></color> Awarded <b>+{0} Experience</b> and <b>+{1} Cosmic Coins</b>!", num2, num3));
		Quiet("<color=#5B2A86><b>=======================================================</b></color>");
	}

	private static void PostLog(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, message, null, ConstellationCategory.Combat, "Dimensional Rift");
	}

	private static void Voice(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, message, null, ConstellationCategory.Combat, "Dimensional Rift");
	}

	private static void Quiet(string message)
	{
		ConstellationChatManager.PostCombatLog(message);
	}
}
