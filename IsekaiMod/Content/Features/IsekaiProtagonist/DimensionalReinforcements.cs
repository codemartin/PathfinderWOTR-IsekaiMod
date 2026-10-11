using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.View;
using Pathfinding;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

public class DimensionalReinforcements : IDamageHandler, ISubscriber, IGlobalSubscriber, IAreaHandler, IAreaActivationHandler, IUnitHandler, IUnitSpawnHandler, IUnitCombatHandler, IPartyCombatHandler, IUnitFactionHandler, IUnitChangeAttackFactionsHandler
{
	internal struct ReinforcementUnit
	{
		public string Guid;

		public int CR;

		public string Name;
	}

	private static bool _initialized = false;

	private static DimensionalReinforcements _instance;

	private static readonly HashSet<string> _externalSpawnIds = new HashSet<string>();

	private static readonly HashSet<string> _legacyTriggeredIds = new HashSet<string>();

	private static readonly HashSet<string> _triggeredIds = new HashSet<string>();

	private static readonly HashSet<string> _bossesWithWaves = new HashSet<string>();

	private static readonly List<UnitEntityData> _activeReinforcements = new List<UnitEntityData>();

	internal const string LegacySatyrGuid = "6114adae73a1338498412b3c63c946f8";

	internal const string BossStorageName = "IsekaiMod.DimensionalReinforcements";

	internal const string TriggeredKey = "Triggered";

	internal const string SummonerKey = "Boss";

	internal const int WaveSize = 2;

	internal const int ActiveCap = 4;

	internal const int MultipliedWaveSize = 4;

	internal const int MultipliedActiveCap = 8;

	internal const string AegisBuffName = "DimensionalAegisBuff";

	internal const int AegisResistance = 999;

	internal const int AegisRounds = 1;

	internal const float MaxRiftSnapDistance = 2f;

	internal const string ArrivalSponsor = "The Key and the Gate";

	public static DimensionalReinforcements Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DimensionalReinforcements();
			}
			return _instance;
		}
	}

	private static BlueprintFaction EnemyFaction
	{
		get
		{
			try
			{
				return BlueprintRoot.Instance?.Cheats?.Enemy?.Faction;
			}
			catch
			{
				return null;
			}
		}
	}

	public static void RegisterExternalSpawnedMob(string unitUniqueId)
	{
		if (!string.IsNullOrEmpty(unitUniqueId))
		{
			_externalSpawnIds.Add(unitUniqueId);
		}
	}

	public static void Add()
	{
		TTCoreExtensions.CreateBuff("DimensionalAegisBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Phase Gate: Dimensional Aegis");
			bp.SetDescription(Main.IsekaiContext, "Protected by impenetrable dimensional stasis for 1 round while reinforcements breach reality. Incoming physical and energy damage is completely negated.");
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("247a4068296e8be42890143f451b4b45"))?.m_Icon;
			bp.m_Flags = (BlueprintBuff.Flags)0;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 999;
				c.BypassedByMaterial = false;
				c.BypassedByAlignment = false;
				c.BypassedByForm = false;
				c.BypassedByMagic = false;
				c.BypassedByReality = false;
			});
			foreach (DamageEnergyType energy in Enum.GetValues(typeof(DamageEnergyType)))
			{
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = energy;
					c.Value = 999;
				});
			}
		});
		Init();
	}

	private static void ApplyAegis(UnitEntityData boss)
	{
		BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "DimensionalAegisBuff");
		if (modBlueprint != null && boss?.Descriptor != null)
		{
			boss.Descriptor.Buffs.AddBuff(modBlueprint, boss, TimeSpan.FromSeconds(6.0));
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

	internal static string TriggerEntry(string saveId, string uniqueId)
	{
		return saveId + "|" + uniqueId;
	}

	private static string CurrentSaveId()
	{
		try
		{
			return Game.Instance?.Player?.GameId ?? "";
		}
		catch
		{
			return "";
		}
	}

	internal static ReinforcementUnit ReinforcementForChapter(int chapter)
	{
		return Math.Max(1, Math.Min(5, chapter)) switch
		{
			1 => new ReinforcementUnit
			{
				Guid = "5abf050c62112ee46b3f4851c458c3da",
				CR = 3,
				Name = "Units/Monsters/Demon Vermlek/CR3_VermlekStandard"
			}, 
			2 => new ReinforcementUnit
			{
				Guid = "b80f9014b742ea04ba27f897177bafc4",
				CR = 6,
				Name = "Units/Monsters/Demon Babau/CR6_BabauStandard"
			}, 
			3 => new ReinforcementUnit
			{
				Guid = "5440e7be8185f2d4eafbb89f5925feab",
				CR = 9,
				Name = "Units/Monsters/Demon Vrock/CR9_VrockStandard"
			}, 
			4 => new ReinforcementUnit
			{
				Guid = "7f05bfa834ef53445bffd6da7ea29c7b",
				CR = 12,
				Name = "Units/Monsters/Demon Nabasu/CR12_NabasuAdvanced"
			}, 
			_ => new ReinforcementUnit
			{
				Guid = "de516b18abf742339988b11d00ba1363",
				CR = 15,
				Name = "Units/Monsters/Demon Babau/CR15_BabauSpecial"
			}, 
		};
	}

	internal static int[] TableChapters()
	{
		return new int[5] { 1, 2, 3, 4, 5 };
	}

	internal static bool IsEligibleBoss(bool bossEncounter, bool plotProtected, bool hostile, bool summoned, bool modSpawned)
	{
		if (((bossEncounter && !plotProtected) & hostile) && !summoned)
		{
			return !modSpawned;
		}
		return false;
	}

	internal static bool Qualifies(bool eligibleBoss, int maxHp, int currentHp)
	{
		if (!eligibleBoss)
		{
			return false;
		}
		if (maxHp <= 0 || currentHp <= 0)
		{
			return false;
		}
		return currentHp <= maxHp / 2;
	}

	internal static int WaveSizeFor(bool encounterMultiplier)
	{
		if (!encounterMultiplier)
		{
			return 2;
		}
		return 4;
	}

	internal static int ActiveCapFor(bool encounterMultiplier)
	{
		if (!encounterMultiplier)
		{
			return 4;
		}
		return 8;
	}

	internal static string EncounterMultiplierLabel()
	{
		return $"Double Dimensional Reinforcements: a boss calls {WaveSizeFor(encounterMultiplier: true)} instead of {WaveSizeFor(encounterMultiplier: false)}, " + $"and up to {ActiveCapFor(encounterMultiplier: true)} instead of {ActiveCapFor(encounterMultiplier: false)} can stand at once. " + "Does nothing unless Dimensional Reinforcements is on. Off: waves already called stay until their fight ends. Applies at once.";
	}

	internal static bool CountsAsActiveReinforcement(string spawnKind, string blueprintGuid, bool isEnemyFaction, bool isDead, bool isInGame)
	{
		if (isDead || !isInGame)
		{
			return false;
		}
		if (spawnKind == "DimensionalReinforcement")
		{
			return true;
		}
		if (spawnKind != null)
		{
			return false;
		}
		if (isEnemyFaction)
		{
			return string.Equals(blueprintGuid, "6114adae73a1338498412b3c63c946f8", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	internal static bool ShouldSweepCorpse(string spawnKind, bool isDead, bool hasLoot, bool isDestroyed)
	{
		if (((spawnKind == "DimensionalReinforcement") & isDead) && !hasLoot)
		{
			return !isDestroyed;
		}
		return false;
	}

	internal static bool IsStillActive(bool isDisposed, bool isDestroyed, bool isMarkedForDestroy, bool isDead, bool isInGame)
	{
		return (!isDisposed && !isDestroyed && !isMarkedForDestroy && !isDead) & isInGame;
	}

	internal static bool IsModSpawned(string spawnKind, bool registeredThisSession)
	{
		return (spawnKind != null) | registeredThisSession;
	}

	internal static bool AlreadyTriggered(IDictionary<string, string> bossStorage, bool inLegacyRecord)
	{
		if (!inLegacyRecord)
		{
			return SaveScopedStorage.GetFlag(bossStorage, "Triggered");
		}
		return true;
	}

	internal static bool BelongsTo(IDictionary<string, string> reinforcementMarker, string bossUniqueId)
	{
		if (string.IsNullOrEmpty(bossUniqueId))
		{
			return false;
		}
		return string.Equals(SaveScopedStorage.Get(reinforcementMarker, "Boss"), bossUniqueId, StringComparison.Ordinal);
	}

	internal static int SpawnCountFor(int activeCount, int maxActive, int desired)
	{
		if (desired <= 0 || maxActive <= 0)
		{
			return 0;
		}
		int num = maxActive - Math.Max(0, activeCount);
		if (num <= 0)
		{
			return 0;
		}
		return Math.Min(num, desired);
	}

	internal static bool ShouldRecordCall(int allowed, int spawned)
	{
		if (allowed > 0)
		{
			return spawned > 0;
		}
		return true;
	}

	internal static bool IsUsableRiftPoint(bool hasNode, float offsetX, float offsetZ, uint nodeArea, uint bossArea)
	{
		if (!hasNode)
		{
			return false;
		}
		if (offsetX * offsetX + offsetZ * offsetZ > 4f)
		{
			return false;
		}
		return nodeArea == bossArea;
	}

	private static void RebuildFromLoadedArea()
	{
		_externalSpawnIds.Clear();
		_legacyTriggeredIds.Clear();
		_triggeredIds.Clear();
		_bossesWithWaves.Clear();
		_activeReinforcements.Clear();
		try
		{
			List<string> reinforcedBosses = TimelineManager.Data.ReinforcedBosses;
			if (reinforcedBosses != null && reinforcedBosses.Count > 0)
			{
				string text = CurrentSaveId() + "|";
				foreach (string item in reinforcedBosses)
				{
					if (item != null && item.StartsWith(text, StringComparison.Ordinal))
					{
						string text2 = item.Substring(text.Length);
						if (text2.Length > 0)
						{
							_legacyTriggeredIds.Add(text2);
						}
					}
				}
			}
			IEnumerable<UnitEntityData> enumerable = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>();
			if (enumerable == null)
			{
				return;
			}
			BlueprintFaction enemyFaction = EnemyFaction;
			Game instance = Game.Instance;
			bool flag = instance != null && instance.Player?.IsInCombat == true;
			List<UnitEntityData> list = new List<UnitEntityData>();
			foreach (UnitEntityData item2 in enumerable)
			{
				if (item2 == null)
				{
					continue;
				}
				if (!flag)
				{
					string spawnKind = SaveScopedStorage.SpawnKindOf(item2);
					UnitDescriptor descriptor = item2.Descriptor;
					if (ShouldSweepCorpse(spawnKind, descriptor != null && descriptor.State?.IsDead == true, item2.IsDeadAndHasLoot, item2.Destroyed))
					{
						list.Add(item2);
						continue;
					}
				}
				string spawnKind2 = SaveScopedStorage.SpawnKindOf(item2);
				string blueprintGuid = ((item2.Blueprint != null) ? item2.Blueprint.AssetGuid.ToString() : null);
				bool isEnemyFaction = enemyFaction != null && item2.Faction == enemyFaction && item2.Master == null;
				UnitDescriptor descriptor2 = item2.Descriptor;
				if (CountsAsActiveReinforcement(spawnKind2, blueprintGuid, isEnemyFaction, descriptor2 != null && descriptor2.State?.IsDead == true, item2.IsInGame))
				{
					_activeReinforcements.Add(item2);
					string text3 = SaveScopedStorage.Get(SaveScopedStorage.SpawnMarker(item2), "Boss");
					if (!string.IsNullOrEmpty(text3))
					{
						_bossesWithWaves.Add(text3);
					}
				}
			}
			foreach (UnitEntityData item3 in list)
			{
				Despawn(item3);
			}
			if (_activeReinforcements.Count > 0)
			{
				Game instance2 = Game.Instance;
				if (instance2 == null || instance2.Player?.IsInCombat != true)
				{
					DespawnAll();
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error restoring dimensional reinforcement state: " + ex);
		}
	}

	private static int CountActiveReinforcements()
	{
		_activeReinforcements.RemoveAll(delegate(UnitEntityData u)
		{
			if (!(u == null))
			{
				bool isDisposed = u.IsDisposed;
				bool destroyed = u.Destroyed;
				bool shouldFadeOutViewAndDestroy = u.ShouldFadeOutViewAndDestroy;
				UnitDescriptor descriptor = u.Descriptor;
				return !IsStillActive(isDisposed, destroyed, shouldFadeOutViewAndDestroy, descriptor != null && descriptor.State?.IsDead == true, u.IsInGame);
			}
			return true;
		});
		return _activeReinforcements.Count;
	}

	private static void RecordTriggeredBoss(UnitEntityData boss)
	{
		SaveScopedStorage.SetFlag(SaveScopedStorage.Ensure(boss, "IsekaiMod.DimensionalReinforcements"), "Triggered", value: true);
	}

	private static void Despawn(UnitEntityData unit)
	{
		try
		{
			if (!(unit == null) && !unit.IsDisposed && !unit.Destroyed)
			{
				unit.MarkForDestroy();
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error removing a dimensional reinforcement: " + ex);
		}
	}

	private static void DespawnAll()
	{
		if (_activeReinforcements.Count == 0)
		{
			return;
		}
		foreach (UnitEntityData item in _activeReinforcements.ToList())
		{
			if (item != null)
			{
				UnitDescriptor descriptor = item.Descriptor;
				if (descriptor == null || descriptor.State?.IsDead != true)
				{
					Despawn(item);
				}
			}
		}
		_activeReinforcements.Clear();
		_bossesWithWaves.Clear();
	}

	private static void DespawnWaveOf(UnitEntityData boss)
	{
		if (boss == null || string.IsNullOrEmpty(boss.UniqueId) || !_bossesWithWaves.Remove(boss.UniqueId))
		{
			return;
		}
		foreach (UnitEntityData item in _activeReinforcements.ToList())
		{
			if (!(item == null) && BelongsTo(SaveScopedStorage.SpawnMarker(item), boss.UniqueId))
			{
				Despawn(item);
				_activeReinforcements.Remove(item);
			}
		}
	}

	public void OnAreaDidLoad()
	{
		RebuildFromLoadedArea();
	}

	public void OnAreaBeginUnloading()
	{
		DespawnAll();
		_externalSpawnIds.Clear();
		_triggeredIds.Clear();
	}

	public void OnAreaActivated()
	{
		RebuildFromLoadedArea();
	}

	public void HandleUnitDeath(UnitEntityData unit)
	{
		if (!(unit == null))
		{
			_activeReinforcements.Remove(unit);
			DespawnWaveOf(unit);
		}
	}

	public void HandleUnitDestroyed(UnitEntityData unit)
	{
		if (!(unit == null))
		{
			_activeReinforcements.Remove(unit);
			DespawnWaveOf(unit);
		}
	}

	public void HandleUnitSpawned(UnitEntityData unit)
	{
	}

	public void HandleUnitJoinCombat(UnitEntityData unit)
	{
	}

	public void HandleUnitLeaveCombat(UnitEntityData unit)
	{
		DespawnWaveOf(unit);
	}

	public void HandlePartyCombatStateChanged(bool inCombat)
	{
		if (!inCombat)
		{
			DespawnAll();
		}
	}

	public void HandleFactionChanged(UnitEntityData unit)
	{
		DespawnWaveIfNoLongerHostile(unit);
	}

	public void HandleUnitChangeAttackFactions(UnitEntityData unit)
	{
		DespawnWaveIfNoLongerHostile(unit);
	}

	private static void DespawnWaveIfNoLongerHostile(UnitEntityData unit)
	{
		if (!(unit == null) && !string.IsNullOrEmpty(unit.UniqueId) && _bossesWithWaves.Contains(unit.UniqueId) && !CampaignEncounterGuard.IsPartysEnemy(unit))
		{
			DespawnWaveOf(unit);
		}
	}

	public void HandleDamageDealt(RuleDealDamage dealDamage)
	{
		if (!Main.IsekaiContext.AddedContent.EnableDimensionalReinforcements)
		{
			return;
		}
		UnitEntityData unitEntityData = dealDamage?.Target;
		if (unitEntityData == null || string.IsNullOrEmpty(unitEntityData.UniqueId) || _triggeredIds.Contains(unitEntityData.UniqueId))
		{
			return;
		}
		UnitEntityData unitEntityData2 = Game.Instance?.Player?.MainCharacter.Value;
		if (unitEntityData2 == null || !CampaignEncounterGuard.IsPartysEnemy(unitEntityData))
		{
			return;
		}
		Game instance = Game.Instance;
		if (instance == null || instance.Player?.IsInCombat != true || !unitEntityData.IsInCombat)
		{
			return;
		}
		Game instance2 = Game.Instance;
		if ((instance2 != null && instance2.CurrentlyLoadedArea?.IsGlobalMap == true) || CampaignEncounterGuard.Excludes(unitEntityData) || !CampaignEncounterGuard.IsBoss(unitEntityData.Blueprint) || !Qualifies(eligibleBoss: true, unitEntityData.MaxHP, unitEntityData.HPLeft) || !CampaignEncounterGuard.AppliesToParty(Main.IsekaiContext.AddedContent.EnableDimensionalReinforcements, CampaignEncounterGuard.ProtagonistsInParty()))
		{
			return;
		}
		if (!IsEligibleBoss(bossEncounter: true, CampaignEncounterGuard.IsPlotProtected(unitEntityData), hostile: true, summoned: false, IsModSpawned(SaveScopedStorage.SpawnKindOf(unitEntityData), _externalSpawnIds.Contains(unitEntityData.UniqueId))))
		{
			_triggeredIds.Add(unitEntityData.UniqueId);
			return;
		}
		if (AlreadyTriggered(SaveScopedStorage.Find(unitEntityData, "IsekaiMod.DimensionalReinforcements"), _legacyTriggeredIds.Contains(unitEntityData.UniqueId)))
		{
			_triggeredIds.Add(unitEntityData.UniqueId);
			return;
		}
		_triggeredIds.Add(unitEntityData.UniqueId);
		bool enableIsekaiEncounterMultiplier = Main.IsekaiContext.AddedContent.EnableIsekaiEncounterMultiplier;
		int num = SpawnCountFor(CountActiveReinforcements(), ActiveCapFor(enableIsekaiEncounterMultiplier), WaveSizeFor(enableIsekaiEncounterMultiplier));
		int num2 = ((num > 0) ? SpawnReinforcements(unitEntityData, unitEntityData2, num) : 0);
		if (ShouldRecordCall(num, num2))
		{
			RecordTriggeredBoss(unitEntityData);
		}
		if (num2 > 0)
		{
			AnnounceArrival(unitEntityData.CharacterName, num2);
		}
		if (num2 > 0)
		{
			ApplyAegis(unitEntityData);
		}
	}

	internal static string ArrivalLine(string bossName, int count)
	{
		if (count != 1)
		{
			return ConstellationText.LocalizedFormat("Constellation.DimensionalReinforcements.Arrival", "<color=#7A1010><b>[Dimensional Reinforcements]</b></color> <b>{0}</b> tears open a rift, and {1} demons answer the call!", bossName, count);
		}
		return ConstellationText.LocalizedFormat("Constellation.DimensionalReinforcements.Arrival", "<color=#7A1010><b>[Dimensional Reinforcements]</b></color> <b>{0}</b> tears open a rift, and {1} demon answers the call!", bossName, count);
	}

	internal static string ArrivalVoiceLine()
	{
		return "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color>: <i>\"A door, opened in haste by something losing. I know every door. This one leads nowhere kind.\"</i>";
	}

	private static void AnnounceArrival(string bossName, int count)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, ArrivalLine(bossName, count), null, ConstellationCategory.Combat, "Dimensional Reinforcements");
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, ArrivalVoiceLine(), "The Key and the Gate", ConstellationCategory.Combat, "Dimensional Reinforcements");
	}

	private static List<Vector3> SpawnPoints(Vector3 bossPos, Vector3 partyPos, int count, float unitRadius)
	{
		List<Vector3> list = new List<Vector3>();
		if (count <= 0 || AstarPath.active == null)
		{
			return list;
		}
		NNInfo nearestNode = ObstacleAnalyzer.GetNearestNode(bossPos);
		if (nearestNode.node == null)
		{
			return list;
		}
		uint area = nearestNode.node.Area;
		Vector3 vector = bossPos - partyPos;
		vector.y = 0f;
		if (vector.sqrMagnitude < 0.01f)
		{
			vector = Vector3.forward;
		}
		vector.Normalize();
		Vector3 position = nearestNode.position;
		bool flag = false;
		float[] array = new float[9] { 0f, 35f, -35f, 70f, -70f, 105f, -105f, 140f, -140f };
		float[] array2 = new float[2] { 4f, 6f };
		foreach (float num in array2)
		{
			float[] array3 = array;
			foreach (float y in array3)
			{
				Vector3 pos = bossPos + Quaternion.Euler(0f, y, 0f) * vector * num;
				NNInfo nearestNode2 = ObstacleAnalyzer.GetNearestNode(pos);
				bool flag2 = nearestNode2.node != null;
				if (IsUsableRiftPoint(flag2, nearestNode2.position.x - pos.x, nearestNode2.position.z - pos.z, flag2 ? nearestNode2.node.Area : 0u, area))
				{
					position = nearestNode2.position;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		FreePlaceSelector.PlaceSpawnPlaces(count, unitRadius, position);
		for (int k = 0; k < count; k++)
		{
			list.Add(FreePlaceSelector.GetRelaxedPosition(k, projectOnGround: true));
		}
		return list;
	}

	private static int SpawnReinforcements(UnitEntityData boss, UnitEntityData player, int spawnCount)
	{
		int num = 0;
		try
		{
			if (Game.Instance?.LoadedAreaState?.MainState == null)
			{
				return 0;
			}
			ReinforcementUnit reinforcementUnit = ReinforcementForChapter(Game.Instance.Player?.Chapter ?? 1);
			BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>(reinforcementUnit.Guid);
			if (blueprint == null)
			{
				return 0;
			}
			UnitEntityView unitEntityView = blueprint.Prefab?.Load();
			float unitRadius = ((unitEntityView != null) ? unitEntityView.Corpulence : 0.5f);
			List<Vector3> list = SpawnPoints(boss.Position, player.Position, spawnCount, unitRadius);
			if (list.Count == 0)
			{
				Main.IsekaiContext.Logger.Log("[Dimensional Reinforcements] No pathfinding ground near " + boss.CharacterName + "; the rift stays shut.");
				return 0;
			}
			Main.IsekaiContext.Logger.Log($"[Dimensional Reinforcements] Rift opened near {boss.CharacterName} at below 50% HP: {list.Count} x {reinforcementUnit.Name}.");
			BlueprintFaction enemyFaction = EnemyFaction;
			foreach (Vector3 item in list)
			{
				UnitEntityData unitEntityData = Game.Instance.EntityCreator.SpawnUnit(blueprint, item, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
				if (!(unitEntityData == null))
				{
					if (enemyFaction != null)
					{
						unitEntityData.SwitchFactions(enemyFaction, resetAttackFactions: true);
					}
					unitEntityData.CombatState?.Engage(player);
					SaveScopedStorage.MarkSpawned(unitEntityData, "DimensionalReinforcement");
					Dictionary<string, string> dictionary = SaveScopedStorage.SpawnMarker(unitEntityData);
					if (dictionary != null)
					{
						dictionary["Boss"] = boss.UniqueId;
					}
					_externalSpawnIds.Add(unitEntityData.UniqueId);
					_activeReinforcements.Add(unitEntityData);
					_bossesWithWaves.Add(boss.UniqueId);
					num++;
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error spawning dimensional reinforcements: " + ex);
		}
		return num;
	}
}
