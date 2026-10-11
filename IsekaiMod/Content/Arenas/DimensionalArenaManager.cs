using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.KingmakerIntegration;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Blueprints.Root;
using Kingmaker.DLC;
using Kingmaker.Designers;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.QA.Statistics;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

public class DimensionalArenaManager : IUnitHandler, ISubscriber, IGlobalSubscriber, IUnitSpawnHandler, IAreaHandler, IUnitLifeStateChanged
{
	private static bool _initialized = false;

	private static DimensionalArenaManager _instance;

	private readonly HashSet<string> _activeEnemyIds = new HashSet<string>();

	private readonly List<string> _spawnedEnemyIds = new List<string>();

	private string _activeCameoId;

	private Vector3 _arenaAnchor;

	private Vector3 _arenaForward = new Vector3(0f, 0f, 1f);

	private static HashSet<string> _rosterGuids;

	private static readonly BlueprintUnit SatyrArcher = BlueprintTools.GetBlueprint<BlueprintUnit>("6114adae73a1338498412b3c63c946f8");

	private static readonly BlueprintUnit SatyrSwordsman = BlueprintTools.GetBlueprint<BlueprintUnit>("a6dceccaf84af1a42843e58412265388");

	private static readonly BlueprintUnit Dryad = BlueprintTools.GetBlueprint<BlueprintUnit>("20660a3d7ef5ec54a9c1f08b0b58d753");

	private static readonly BlueprintUnit Redcap = BlueprintTools.GetBlueprint<BlueprintUnit>("99286885f5be6b948baf3de77471d6ca");

	private static readonly BlueprintUnit Nymph = BlueprintTools.GetBlueprint<BlueprintUnit>("0cc7a2526e4557945b1d8eb277d1fb3a");

	private static readonly BlueprintUnit RedcapBarbarian = BlueprintTools.GetBlueprint<BlueprintUnit>("2e4a9f6fa84651148bc065c5330817fe");

	private static readonly BlueprintUnit HamadryadQueen = BlueprintTools.GetBlueprint<BlueprintUnit>("b8972cfe36e3cd945bbd2c4c320d5237");

	private static readonly BlueprintUnit LanternKingAvatar = BlueprintTools.GetBlueprint<BlueprintUnit>("8e74850f1bb94808b3c46822f6edd739");

	private static readonly BlueprintUnit GhoulStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("2c0fd840d87c3b4418ae958c9a813fe0");

	private static readonly BlueprintUnit GhoulCreeper = BlueprintTools.GetBlueprint<BlueprintUnit>("492e9c74335b7b140bb5cab8413f7a42");

	private static readonly BlueprintUnit SkeletalChampion = BlueprintTools.GetBlueprint<BlueprintUnit>("622b8701c4f2468479b9f533c2cc24e5");

	private static readonly BlueprintUnit SkeletalChampionArcher = BlueprintTools.GetBlueprint<BlueprintUnit>("a24e752e4748bd548936020938effee1");

	internal const string WightWizardGuid = "5e02d1f447e542758a9dd9a80777d5b0";

	internal const string WightFighterGuid = "09737a3ed9c248d5ba8697a4eb7452b3";

	internal const string GhostDruidessGuid = "d0b3eb1737dd433b9d7c39544acb6e13";

	internal const string SpectreStandardGuid = "2f91d7337b60e3b4b9b137198a8c8745";

	internal const string WightFighterBaseGuid = "4ae78e4aa82d37a4cb6cbfbcdcc93195";

	internal const string GhostlyGuardStandardGuid = "d61f011e2ceb27f42878ff77502f1163";

	private static readonly BlueprintUnit WightWizard = BlueprintTools.GetBlueprint<BlueprintUnit>("5e02d1f447e542758a9dd9a80777d5b0");

	private static readonly BlueprintUnit WightFighter = BlueprintTools.GetBlueprint<BlueprintUnit>("09737a3ed9c248d5ba8697a4eb7452b3");

	private static readonly BlueprintUnit GhostDruidess = BlueprintTools.GetBlueprint<BlueprintUnit>("d0b3eb1737dd433b9d7c39544acb6e13");

	private static readonly BlueprintUnit SpectreStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("2f91d7337b60e3b4b9b137198a8c8745");

	private static readonly BlueprintUnit WightFighterBase = BlueprintTools.GetBlueprint<BlueprintUnit>("4ae78e4aa82d37a4cb6cbfbcdcc93195");

	private static readonly BlueprintUnit GhostlyGuardStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("d61f011e2ceb27f42878ff77502f1163");

	private static readonly BlueprintUnit DerakniStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("1f8c87fde2d7be342b90a58d0180884b");

	private static readonly BlueprintUnit SchirAdvancedMelee = BlueprintTools.GetBlueprint<BlueprintUnit>("dbe2e6ca69a41984383093c9ae7bd159");

	private static readonly BlueprintUnit ColoxusStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("d5a01b84a52ed0e4f88dc06828360e2d");

	private static readonly BlueprintUnit DerakniAdvanced = BlueprintTools.GetBlueprint<BlueprintUnit>("43233c4df8cbbde4c8168ad7d70dc639");

	private static readonly BlueprintUnit GlabrezuStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("d13545bd34bf2c3438f909417b9c5314");

	private static readonly BlueprintUnit ColoxusEmissary = BlueprintTools.GetBlueprint<BlueprintUnit>("ebfc4f9af4ec9b84da9682cb46cd1d45");

	private static readonly BlueprintUnit GlabrezuAdvanced = BlueprintTools.GetBlueprint<BlueprintUnit>("d1822e208ff314846a5cd1443a9db45b");

	private static readonly BlueprintUnit MarilithStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("4addf73a9a68c694f9919137f800be50");

	private static readonly BlueprintUnit VavakiaStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("143bf780bdf425a4ca14ab0e5df20232");

	private static readonly BlueprintUnit MarilithAdvanced = BlueprintTools.GetBlueprint<BlueprintUnit>("e350e84a381e17748aa663bd033f8ab1");

	private static readonly BlueprintUnit VavakiaAdvanced = BlueprintTools.GetBlueprint<BlueprintUnit>("370039781f3a4004ab4e3ac6c7032573");

	private static readonly BlueprintUnit BalorStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("eb0b4e6a01f6b30449f40cd03feb3c77");

	private static readonly BlueprintUnit MarilithMythic = BlueprintTools.GetBlueprint<BlueprintUnit>("a4738fdde19441b45873ca290e7bf14b");

	private static readonly BlueprintUnit InevitableKolyarutStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("995fd185611986c4ca02fcac280b03fa") ?? BalorStandard;

	private static readonly BlueprintUnit MythicDerakniFighter = BlueprintTools.GetBlueprint<BlueprintUnit>("bd78f369ef376ad478061cd73555efc2");

	private static readonly BlueprintUnit ColoxusCaster = BlueprintTools.GetBlueprint<BlueprintUnit>("5e0ec427990647f6bfe5345669315ed8");

	private static readonly BlueprintUnit BalorMythicBloodrager = BlueprintTools.GetBlueprint<BlueprintUnit>("138deed3343f207488cb85edc9e0c145");

	private static readonly BlueprintUnit InevitableKolyarutEpic = BlueprintTools.GetBlueprint<BlueprintUnit>("04fa5fcad264414cbfd6af49f93aa565") ?? BalorMythicBloodrager ?? BalorStandard;

	private static readonly BlueprintUnit InevitableKolyarutMiniboss = BlueprintTools.GetBlueprint<BlueprintUnit>("3b4b1b05783044f0b75f6f3afd9e6d7f");

	internal const string GhostBarbarianSarkorianGuid = "9bd447f9353249e490c6921cb0e6e890";

	internal const string GhostStandardGuid = "655ac57b330918c4aadc78a00fb2ccaf";

	internal const string GhostFighterSarkorianGuid = "b95b40741207418294a72bdfc409c9c1";

	internal const string BloodyBonesBarbarianGuid = "f68391521a2175647ab311fc64108574";

	internal const string RiftDrakeStandardGuid = "f1b34ef3c473a394b9b82004fa6e1699";

	internal const string NalfeshneeStandardGuid = "a8a88d3dfea41894597969cda1772f65";

	internal const string GibrilethEliteGuid = "0d618058a02c6e843b094a3727d6edd7";

	internal const string SkeletalChampionGuardGuid = "eaad6ebec57b4d3e8cff044ec633a10a";

	internal const string CorpulentSpectreGuid = "e4993034a5ef4c5468a4959a6765314a";

	internal const string DeepShadowDemonToughGuid = "dd779b0f75bf954428d9d0c9569587e0";

	internal const string ShadowcasterGuid = "bad7a4e16dc48d64bb162c95109fd4d2";

	private static readonly BlueprintUnit GhostBarbarianSarkorian = BlueprintTools.GetBlueprint<BlueprintUnit>("9bd447f9353249e490c6921cb0e6e890");

	private static readonly BlueprintUnit GhostStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("655ac57b330918c4aadc78a00fb2ccaf");

	private static readonly BlueprintUnit GhostFighterSarkorian = BlueprintTools.GetBlueprint<BlueprintUnit>("b95b40741207418294a72bdfc409c9c1");

	private static readonly BlueprintUnit BloodyBonesBarbarian = BlueprintTools.GetBlueprint<BlueprintUnit>("f68391521a2175647ab311fc64108574");

	private static readonly BlueprintUnit RiftDrakeStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("f1b34ef3c473a394b9b82004fa6e1699");

	private static readonly BlueprintUnit NalfeshneeStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("a8a88d3dfea41894597969cda1772f65");

	private static readonly BlueprintUnit GibrilethElite = BlueprintTools.GetBlueprint<BlueprintUnit>("0d618058a02c6e843b094a3727d6edd7");

	private static readonly BlueprintUnit SkeletalChampionGuard = BlueprintTools.GetBlueprint<BlueprintUnit>("eaad6ebec57b4d3e8cff044ec633a10a");

	private static readonly BlueprintUnit CorpulentSpectre = BlueprintTools.GetBlueprint<BlueprintUnit>("e4993034a5ef4c5468a4959a6765314a");

	private static readonly BlueprintUnit DeepShadowDemonTough = BlueprintTools.GetBlueprint<BlueprintUnit>("dd779b0f75bf954428d9d0c9569587e0");

	private static readonly BlueprintUnit Shadowcaster = BlueprintTools.GetBlueprint<BlueprintUnit>("bad7a4e16dc48d64bb162c95109fd4d2");

	private static readonly BlueprintUnit WildHuntMonarch = BlueprintTools.GetBlueprint<BlueprintUnit>("573009c2f6493514188a2844ba53bdf8") ?? HamadryadQueen ?? SatyrSwordsman;

	private static readonly BlueprintUnit WildHuntScout = BlueprintTools.GetBlueprint<BlueprintUnit>("6f5ff0f1e359ee042ba49a746a507190") ?? SatyrSwordsman;

	private static readonly BlueprintUnit WildHuntArcher = BlueprintTools.GetBlueprint<BlueprintUnit>("e21b6536b40aaad4e9c9cd6c216778a3") ?? SatyrArcher;

	private static readonly BlueprintUnit Ankou = BlueprintTools.GetBlueprint<BlueprintUnit>("58ed91a92b8d70248aa884d303954469") ?? Nymph ?? Redcap;

	private static readonly BlueprintUnit PrimalTreant = BlueprintTools.GetBlueprint<BlueprintUnit>("b2c86a184c5f9c942aeb77e52ea1d17d") ?? RedcapBarbarian;

	internal const string Dlc1Guid = "8576a633c8fe4ce78530b55c1f0d14e5";

	internal const string Dlc4Guid = "35b89606cfe9405085a35b02cf15017f";

	internal const string Dlc6Guid = "c2340df3fdaf403baffe824ae7a0a547";

	private bool _isSpawningWave;

	private readonly List<Vector3> _placedThisWave = new List<Vector3>();

	public static DimensionalArenaManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DimensionalArenaManager();
			}
			return _instance;
		}
	}

	public bool IsActive { get; private set; }

	internal ColiseumCup CurrentCup { get; private set; }

	public int CurrentWave { get; private set; }

	public string CurrentCupName => CurrentCup?.DisplayName ?? "the trial";

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

	private static HashSet<string> RosterGuids
	{
		get
		{
			if (_rosterGuids == null)
			{
				_rosterGuids = new HashSet<string>(from u in WaveRoster()
					where u != null
					select u.AssetGuid.ToString(), StringComparer.OrdinalIgnoreCase);
			}
			return _rosterGuids;
		}
	}

	private static BlueprintUnit KolyarutMiniboss => PreferOwned(IsDlcOwned("8576a633c8fe4ce78530b55c1f0d14e5"), InevitableKolyarutMiniboss, InevitableKolyarutEpic ?? InevitableKolyarutStandard);

	private static BlueprintUnit KolyarutEpic => InevitableKolyarutEpic ?? InevitableKolyarutStandard;

	internal static event Action<string, bool> CupCleared;

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
		OnUnitDie(unit);
	}

	public void HandleUnitSpawned(UnitEntityData unit)
	{
	}

	internal static bool ShouldFinishOff(bool trialActive, bool trackedContestant, UnitLifeState lifeState)
	{
		if (trialActive & trackedContestant)
		{
			return lifeState == UnitLifeState.Unconscious;
		}
		return false;
	}

	public void HandleUnitLifeStateChanged(UnitEntityData unit, UnitLifeState prevLifeState)
	{
		try
		{
			if (!(unit == null) && unit.Descriptor?.State != null && ShouldFinishOff(IsActive && CurrentCup != null, _activeEnemyIds.Contains(unit.UniqueId), unit.Descriptor.State.LifeState))
			{
				Quiet(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#3F3F3F><i>[Planar Coliseum] <b>{0}</b> is down and cannot rise before the count. The herald calls the bout.</i></color>", unit.CharacterName));
				GameHelper.KillUnit(unit);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not settle a contestant that was knocked out: " + ex);
		}
	}

	private void FinishOffKnockedOutContestants()
	{
		try
		{
			List<UnitEntityData> list = LoadedUnits();
			if (list == null)
			{
				return;
			}
			foreach (UnitEntityData item in list)
			{
				if (!item.Destroyed && item.Descriptor?.State != null)
				{
					HandleUnitLifeStateChanged(item, UnitLifeState.Conscious);
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not settle knocked-out contestants on load: " + ex);
		}
	}

	public void OnAreaDidLoad()
	{
		RestoreOrCleanUpTournament();
	}

	public void OnAreaBeginUnloading()
	{
		if (IsDisposingGameState())
		{
			ForgetRuntimeState();
		}
		else
		{
			ResetArenaState();
		}
	}

	private static bool IsDisposingGameState()
	{
		try
		{
			return Game.Instance?.IsUnloading ?? false;
		}
		catch
		{
			return false;
		}
	}

	public int ResetArenaState()
	{
		IsActive = false;
		CurrentWave = 0;
		int result = 0;
		try
		{
			ColiseumTournamentRecord coliseumTournamentRecord = ReadRecord() ?? new ColiseumTournamentRecord();
			coliseumTournamentRecord.Spawned = coliseumTournamentRecord.Spawned.Concat(coliseumTournamentRecord.Enemies).Concat(_spawnedEnemyIds).Concat(_activeEnemyIds)
				.ToList();
			coliseumTournamentRecord.Enemies = new List<string>();
			result = SweepLeftovers(coliseumTournamentRecord, out var _);
			RemoveCameo();
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error clearing the coliseum trial: " + ex);
		}
		_spawnedEnemyIds.Clear();
		_activeEnemyIds.Clear();
		ClearPersistedTournament(clearCameo: true);
		ClearLegacyRecord();
		return result;
	}

	private void FixArenaFrame()
	{
		if (!PlanarColiseumHubManager.TryGetArenaAnchor(out _arenaAnchor, out _arenaForward))
		{
			_arenaAnchor = Vector3.zero;
			_arenaForward = new Vector3(0f, 0f, 1f);
		}
	}

	private void ForgetRuntimeState()
	{
		IsActive = false;
		CurrentWave = 0;
		CurrentCup = null;
		_activeEnemyIds.Clear();
		_spawnedEnemyIds.Clear();
		_activeCameoId = null;
	}

	private static List<UnitEntityData> LoadedUnits()
	{
		return Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>().Where((UnitEntityData u) => u != null).ToList();
	}

	public int ClearSpawnedUnits(bool clearCameo = true)
	{
		int num = 0;
		try
		{
			List<UnitEntityData> list = LoadedUnits();
			if (list != null)
			{
				HashSet<string> spawned = new HashSet<string>(_spawnedEnemyIds, StringComparer.Ordinal);
				BlueprintFaction enemyFaction = EnemyFaction;
				List<UnitEntityData> list2 = list.Where((UnitEntityData u) => spawned.Contains(u.UniqueId) && !u.Destroyed && !u.ShouldFadeOutViewAndDestroy).ToList();
				HashSet<string> hashSet = new HashSet<string>(ColiseumTournamentPlanner.SweepSpawned(list2.Select((UnitEntityData u) => Snapshot(u, enemyFaction))), StringComparer.Ordinal);
				foreach (UnitEntityData item in list2)
				{
					if (hashSet.Contains(item.UniqueId))
					{
						item.FadeOutViewAndDestroy();
						num++;
					}
				}
			}
			if (clearCameo)
			{
				RemoveCameo();
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error clearing spawned arena units: " + ex);
		}
		_spawnedEnemyIds.Clear();
		_activeEnemyIds.Clear();
		return num;
	}

	private void RemoveCameo()
	{
		if (!string.IsNullOrEmpty(_activeCameoId))
		{
			(LoadedUnits()?.FirstOrDefault((UnitEntityData x) => x.UniqueId == _activeCameoId))?.FadeOutViewAndDestroy();
		}
		_activeCameoId = null;
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

	private static ColiseumTournamentRecord ReadRecord()
	{
		return ColiseumTournamentRecord.Read(SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.ColiseumTournament"));
	}

	private void PersistTournament()
	{
		try
		{
			Dictionary<string, string> dictionary = SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.ColiseumTournament");
			if (dictionary != null)
			{
				ColiseumTournamentRecord coliseumTournamentRecord = new ColiseumTournamentRecord();
				coliseumTournamentRecord.Cup = CurrentCup?.Id ?? "";
				coliseumTournamentRecord.Wave = CurrentWave;
				coliseumTournamentRecord.Enemies = new List<string>(_activeEnemyIds);
				coliseumTournamentRecord.Spawned = new List<string>(_spawnedEnemyIds);
				coliseumTournamentRecord.Write(dictionary);
				PersistCameo(dictionary);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error persisting coliseum tournament state: " + ex);
		}
	}

	private void PersistCameo(Dictionary<string, string> storage)
	{
		if (storage != null)
		{
			if (string.IsNullOrEmpty(_activeCameoId))
			{
				storage.Remove("Cameo");
			}
			else
			{
				storage["Cameo"] = _activeCameoId;
			}
		}
	}

	private void ClearPersistedTournament(bool clearCameo)
	{
		try
		{
			EntityDataBase entity = SaveScopedStorage.CurrentPlayer();
			Dictionary<string, string> dictionary = SaveScopedStorage.Find(entity, "IsekaiMod.ColiseumTournament");
			if (dictionary != null)
			{
				ColiseumTournamentRecord.Clear(dictionary);
				if (clearCameo)
				{
					dictionary.Remove("Cameo");
				}
				if (dictionary.Count == 0)
				{
					SaveScopedStorage.Drop(entity, "IsekaiMod.ColiseumTournament");
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error clearing coliseum tournament state: " + ex);
		}
	}

	private static ColiseumTournamentRecord ReadLegacyRecord()
	{
		TimelineData data = TimelineManager.Data;
		if (string.IsNullOrEmpty(data.ActiveArenaSaveId) || !string.Equals(data.ActiveArenaSaveId, CurrentSaveId(), StringComparison.Ordinal))
		{
			return null;
		}
		return new ColiseumTournamentRecord
		{
			Cup = (data.ActiveArenaCup ?? ""),
			Wave = data.ActiveArenaWave,
			Enemies = new List<string>(data.ActiveArenaEnemies ?? new List<string>()),
			Spawned = new List<string>(data.ActiveArenaSpawned ?? new List<string>())
		};
	}

	private static void ClearLegacyRecord()
	{
		try
		{
			TimelineData data = TimelineManager.Data;
			if (!string.IsNullOrEmpty(data.ActiveArenaSaveId) && string.Equals(data.ActiveArenaSaveId, CurrentSaveId(), StringComparison.Ordinal))
			{
				data.ActiveArenaSaveId = "";
				data.ActiveArenaCup = "";
				data.ActiveArenaWave = 0;
				data.ActiveArenaEnemies = new List<string>();
				data.ActiveArenaSpawned = new List<string>();
				TimelineManager.Save();
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error clearing the old coliseum tournament record: " + ex);
		}
	}

	internal static BlueprintUnit[] WaveRoster()
	{
		return new BlueprintUnit[52]
		{
			SatyrArcher, SatyrSwordsman, Dryad, Redcap, Nymph, RedcapBarbarian, HamadryadQueen, GhoulStandard, GhoulCreeper, WightWizard,
			WightFighter, SkeletalChampion, SkeletalChampionArcher, GhostDruidess, SpectreStandard, WightFighterBase, GhostlyGuardStandard, DerakniStandard, SchirAdvancedMelee, ColoxusStandard,
			DerakniAdvanced, GlabrezuStandard, ColoxusEmissary, GlabrezuAdvanced, MarilithStandard, VavakiaStandard, MarilithAdvanced, VavakiaAdvanced, BalorStandard, MarilithMythic,
			InevitableKolyarutStandard, MythicDerakniFighter, ColoxusCaster, BalorMythicBloodrager, InevitableKolyarutEpic, InevitableKolyarutMiniboss, WildHuntMonarch, WildHuntScout, WildHuntArcher, Ankou,
			PrimalTreant, GhostBarbarianSarkorian, GhostStandard, GhostFighterSarkorian, BloodyBonesBarbarian, RiftDrakeStandard, NalfeshneeStandard, GibrilethElite, SkeletalChampionGuard, CorpulentSpectre,
			DeepShadowDemonTough, Shadowcaster
		};
	}

	private static bool CarriesLoot(UnitEntityData unit)
	{
		try
		{
			return unit.IsDeadAndHasLoot;
		}
		catch
		{
			return false;
		}
	}

	private static ColiseumUnitSnapshot Snapshot(UnitEntityData unit, BlueprintFaction enemyFaction)
	{
		ColiseumUnitSnapshot result = new ColiseumUnitSnapshot
		{
			UniqueId = unit.UniqueId,
			BlueprintGuid = ((unit.Blueprint != null) ? unit.Blueprint.AssetGuid.ToString() : null),
			IsEnemyFaction = (enemyFaction != null && unit.Faction == enemyFaction && !unit.IsPlayerFaction && unit.Master == null)
		};
		UnitDescriptor descriptor = unit.Descriptor;
		result.IsDead = descriptor != null && descriptor.State?.IsDead == true;
		result.CarriesLoot = CarriesLoot(unit);
		result.SpawnKind = SaveScopedStorage.SpawnKindOf(unit);
		return result;
	}

	private int SweepLeftovers(ColiseumTournamentRecord record, out List<string> resume)
	{
		resume = new List<string>();
		List<UnitEntityData> list = LoadedUnits();
		if (list == null)
		{
			return 0;
		}
		list = list.Where((UnitEntityData u) => !u.Destroyed && !u.ShouldFadeOutViewAndDestroy).ToList();
		BlueprintFaction enemyFaction = EnemyFaction;
		ColiseumTournamentRecord coliseumTournamentRecord = ReadLegacyRecord();
		List<string> legacySpawnedIds = coliseumTournamentRecord?.Spawned.Concat(coliseumTournamentRecord.Enemies).ToList();
		ColiseumTournamentPlanner.Plan(record, list.Select((UnitEntityData u) => Snapshot(u, enemyFaction)), PlanarColiseumHubManager.IsInColiseum(), RosterGuids, legacySpawnedIds, out resume, out var destroy);
		HashSet<string> hashSet = new HashSet<string>(destroy, StringComparer.Ordinal);
		int num = 0;
		foreach (UnitEntityData item in list)
		{
			if (hashSet.Contains(item.UniqueId))
			{
				item.FadeOutViewAndDestroy();
				num++;
			}
		}
		return num;
	}

	private void RestoreOrCleanUpTournament()
	{
		ForgetRuntimeState();
		try
		{
			Dictionary<string, string> storage = SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.ColiseumTournament");
			ColiseumTournamentRecord coliseumTournamentRecord = ColiseumTournamentRecord.Read(storage) ?? ReadLegacyRecord();
			_activeCameoId = SaveScopedStorage.Get(storage, "Cameo");
			ColiseumCup coliseumCup = ((coliseumTournamentRecord == null) ? null : ColiseumCups.ById(coliseumTournamentRecord.Cup));
			if (coliseumTournamentRecord != null && coliseumCup == null)
			{
				coliseumTournamentRecord.Spawned = coliseumTournamentRecord.Spawned.Concat(coliseumTournamentRecord.Enemies).ToList();
				coliseumTournamentRecord.Enemies = new List<string>();
			}
			SweepLeftovers(coliseumTournamentRecord, out var resume);
			ClearLegacyRecord();
			if (coliseumTournamentRecord != null)
			{
				if (resume.Count == 0 || coliseumCup == null)
				{
					ClearPersistedTournament(clearCameo: false);
					return;
				}
				CurrentCup = coliseumCup;
				CurrentWave = Math.Max(1, coliseumTournamentRecord.Wave);
				IsActive = true;
				_activeEnemyIds.UnionWith(resume);
				_spawnedEnemyIds.AddRange(resume);
				FixArenaFrame();
				PersistTournament();
				FinishOffKnockedOutContestants();
				Quiet((_activeEnemyIds.Count == 1) ? ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> The <b>{0}</b> resumes at wave {1}. {2} adversary still stand.", coliseumCup.DisplayName, CurrentWave, _activeEnemyIds.Count) : ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> The <b>{0}</b> resumes at wave {1}. {2} adversaries still stand.", coliseumCup.DisplayName, CurrentWave, _activeEnemyIds.Count));
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error restoring coliseum tournament state: " + ex);
		}
	}

	public static void ForfeitArena()
	{
		Instance.Forfeit();
	}

	public void Forfeit()
	{
		bool isActive = IsActive;
		string currentCupName = CurrentCupName;
		int num = ResetArenaState();
		CurrentCup = null;
		if (isActive)
		{
			Quiet(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> You withdraw from the <b>{0}</b>. The sands are swept clean and the herald will take your entry again whenever you are ready.", currentCupName));
		}
		else if (num > 0)
		{
			Quiet((num == 1) ? ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> No trial is underway, but {0} combatant was still standing from an old one. The herald sweeps the sands clean.", num) : ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> No trial is underway, but {0} combatants were still standing from an old one. The herald sweeps the sands clean.", num));
		}
		else
		{
			Quiet(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> There is no trial underway to withdraw from, and the sands are already clear."));
		}
	}

	private static bool IsDlcOwned(string dlcGuid)
	{
		try
		{
			return BlueprintTools.GetBlueprint<BlueprintDlc>(dlcGuid)?.IsAvailable ?? false;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not ask whether DLC " + dlcGuid + " is owned: " + ex);
			return false;
		}
	}

	internal static T PreferOwned<T>(bool dlcOwned, T dlcUnit, T fallback) where T : class
	{
		if (!dlcOwned || dlcUnit == null)
		{
			return fallback;
		}
		return dlcUnit;
	}

	private static BlueprintUnit Dlc4(BlueprintUnit dlcUnit, BlueprintUnit fallback)
	{
		return PreferOwned(IsDlcOwned("35b89606cfe9405085a35b02cf15017f"), dlcUnit, fallback);
	}

	public static void StartArena(string cupId)
	{
		Instance.Begin(cupId);
	}

	internal static bool IsCupOfferedHere(ColiseumCup cup)
	{
		if (cup != null && cup.Kind == ColiseumCupKind.PersonalTrial)
		{
			return PersonalQuestDirector.IsTrialOfferedNow(cup.Id);
		}
		int arenaAct = PlanarColiseumArea.CurrentArenaAct();
		return ColiseumCups.IsOffered(cup, arenaAct, StolenLandsCupAvailable(), ColiseumProgress.IsClearedInSave("Act5.MultiverseThreshold"));
	}

	internal static bool StolenLandsCupAvailable()
	{
		if (KingmakerCrossSaveManager.IsKingmakerInstalled)
		{
			return true;
		}
		try
		{
			BlueprintQuestObjective objectiveWildHuntTrial = SylvanFeyAllianceQuest.ObjectiveWildHuntTrial;
			QuestBook questBook = Game.Instance?.Player?.QuestBook;
			return objectiveWildHuntTrial != null && questBook != null && questBook.GetObjectiveState(objectiveWildHuntTrial) == QuestObjectiveState.Started;
		}
		catch
		{
			return false;
		}
	}

	public void Begin(string cupId)
	{
		if (Game.Instance?.Player?.MainCharacter.Value == null)
		{
			return;
		}
		ColiseumCup coliseumCup = ColiseumCups.ById(cupId);
		if (coliseumCup == null)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Unknown cup id: " + cupId);
			return;
		}
		BlueprintArea currentlyLoadedArea = Game.Instance.CurrentlyLoadedArea;
		if ((currentlyLoadedArea != null && currentlyLoadedArea.IsGlobalMap) || !PlanarColiseumHubManager.IsInColiseum())
		{
			Quiet(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> Tournament cups are fought only on the arena sands."));
			return;
		}
		if (!IsCupOfferedHere(coliseumCup))
		{
			Quiet(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> The <b>{0}</b> is not fought in this arena.", coliseumCup.DisplayName));
			return;
		}
		if (Game.Instance.Player.IsInCombat)
		{
			Quiet(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> You cannot initiate a tournament while already engaged in battle!"));
			return;
		}
		if (IsActive)
		{
			Quiet(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> An active tournament is already underway!"));
			return;
		}
		ClearSpawnedUnits();
		IsActive = true;
		CurrentCup = coliseumCup;
		CurrentWave = 0;
		_activeEnemyIds.Clear();
		FixArenaFrame();
		PostLog(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> <b>Rifts tear open across the arena sands! The {0} has commenced!</b>", coliseumCup.DisplayName));
		SpawnNextWave();
	}

	private void SpawnNextWave()
	{
		if (_isSpawningWave || CurrentCup == null)
		{
			return;
		}
		try
		{
			_isSpawningWave = true;
			CurrentWave++;
			_activeEnemyIds.Clear();
			_placedThisWave.Clear();
			switch (CurrentCup.Waves)
			{
			case ArenaType.Act1FaeGrove:
				SpawnFaeGroveWave(CurrentWave);
				break;
			case ArenaType.Act2LostLegions:
				SpawnLostLegionsWave(CurrentWave);
				break;
			case ArenaType.Act3MidnightColosseum:
				SpawnMidnightColosseumWave(CurrentWave);
				break;
			case ArenaType.Act4VoidRift:
				SpawnVoidRiftWave(CurrentWave);
				break;
			case ArenaType.Act5MultiverseThreshold:
				SpawnMultiverseThresholdWave(CurrentWave);
				break;
			case ArenaType.AstralCup:
				SpawnAstralWave(CurrentWave);
				break;
			case ArenaType.GenesisCup:
				SpawnGenesisWave(CurrentWave);
				break;
			case ArenaType.TheCosmicChallenge:
				SpawnCosmicChallengeWave(CurrentWave);
				break;
			case ArenaType.StolenLandsLegacyCup:
				SpawnStolenLandsWave(CurrentWave);
				break;
			default:
				SpawnPersonalTrialWave(CurrentCup.Waves);
				break;
			}
			if (_activeEnemyIds.Count == 0 && IsActive)
			{
				AbortEmptyWave();
			}
			else
			{
				PersistTournament();
			}
		}
		finally
		{
			_isSpawningWave = false;
		}
	}

	private void AbortEmptyWave()
	{
		string currentCupName = CurrentCupName;
		int currentWave = CurrentWave;
		ResetArenaState();
		CurrentCup = null;
		Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] Wave {currentWave} of {currentCupName} spawned no units; the trial was called off.");
		Quiet(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> No adversaries answered the summons for wave {0} of the <b>{1}</b>. The herald calls the trial off; nothing is won or lost.", currentWave, currentCupName));
	}

	private void SpawnFaeGroveWave(int wave)
	{
		switch (wave)
		{
		case 1:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#004604><b>[Constellation - Desna]</b></color>: <i>\"Swift arrows and playful laughter! First World scouts test your footwork!\"</i>"));
			SpawnEnemy(SatyrArcher, GetOffset(5f, 0f));
			SpawnEnemy(SatyrSwordsman, GetOffset(-5f, 2f));
			SpawnEnemy(Dryad, GetOffset(0f, 6f));
			break;
		case 2:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Calistria]</b></color>: <i>\"Cruel little Redcaps and alluring Nymphs. Paint the dirt red, darling.\"</i>"));
			SpawnEnemy(Redcap, GetOffset(-6f, 3f));
			SpawnEnemy(Redcap, GetOffset(6f, -3f));
			SpawnEnemy(Nymph, GetOffset(0f, 7f));
			break;
		case 3:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The Hamadryad Queen steps forward to defend her sacred grove.\"</i>"));
			SpawnEnemy(HamadryadQueen, GetOffset(0f, 7f));
			SpawnEnemy(RedcapBarbarian, GetOffset(-5f, 4f));
			SpawnEnemy(RedcapBarbarian, GetOffset(5f, 4f));
			break;
		}
	}

	private void SpawnLostLegionsWave(int wave)
	{
		switch (wave)
		{
		case 1:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#3F3F3F><b>[Constellation - Pharasma]</b></color>: <i>\"Restless ghouls crawl from the graves of forgotten crusaders. Put them to eternal sleep.\"</i>"));
			SpawnEnemy(GhoulStandard, GetOffset(4f, 2f));
			SpawnEnemy(GhoulStandard, GetOffset(-4f, -2f));
			SpawnEnemy(GhoulCreeper, GetOffset(0f, 6f));
			break;
		case 2:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Constellation - Gorum]</b></color>: <i>\"Wight fighters and the restless dead of their crypts! Strike them down without hesitation!\"</i>"));
			SpawnEnemy(Dlc4(WightFighter, WightFighterBase), GetOffset(5f, 0f));
			SpawnEnemy(Dlc4(WightFighter, WightFighterBase), GetOffset(-5f, 0f));
			SpawnEnemy(Dlc4(WightWizard, SpectreStandard), GetOffset(0f, 6f));
			break;
		case 3:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Iomedae]</b></color>: <i>\"The spectral warden of the fallen legions manifests with her skeletal champions!\"</i>"));
			SpawnEnemy(Dlc4(GhostDruidess, GhostlyGuardStandard), GetOffset(0f, 6f));
			SpawnEnemy(SkeletalChampion, GetOffset(-5f, 2f));
			SpawnEnemy(SkeletalChampionArcher, GetOffset(5f, -2f));
			break;
		}
	}

	private void SpawnMidnightColosseumWave(int wave)
	{
		switch (wave)
		{
		case 1:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Calistria]</b></color>: <i>\"Demonic gladiators eager to feast on your flesh! Show them agony!\"</i>"));
			SpawnEnemy(DerakniStandard, GetOffset(-5f, 0f));
			SpawnEnemy(SchirAdvancedMelee, GetOffset(5f, 0f));
			break;
		case 2:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Nethys]</b></color>: <i>\"Coloxus spellcasters bend the arcane winds of the colosseum!\"</i>"));
			SpawnEnemy(ColoxusStandard, GetOffset(0f, 6f));
			SpawnEnemy(DerakniAdvanced, GetOffset(-6f, -3f));
			SpawnEnemy(DerakniAdvanced, GetOffset(6f, -3f));
			break;
		case 3:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Constellation - Asmodeus]</b></color>: <i>\"The four-armed Glabrezu apex titan enters the ring. Execute it with cold precision.\"</i>"));
			SpawnEnemy(GlabrezuAdvanced, GetOffset(0f, 7f));
			SpawnEnemy(ColoxusEmissary, GetOffset(-5f, 0f));
			SpawnEnemy(ColoxusEmissary, GetOffset(5f, 0f));
			break;
		}
	}

	private void SpawnVoidRiftWave(int wave)
	{
		switch (wave)
		{
		case 1:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Nethys]</b></color>: <i>\"Mariliths and vavakia climb out of the dark beneath the rift. Nothing down there should still be alive!\"</i>"));
			SpawnEnemy(MarilithStandard, GetOffset(-5f, 0f));
			SpawnEnemy(VavakiaStandard, GetOffset(5f, 0f));
			break;
		case 2:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#004604><b>[Constellation - Desna]</b></color>: <i>\"The void winds howl! Greater abyssal war-beasts rise out of the nothing!\"</i>"));
			SpawnEnemy(MarilithAdvanced, GetOffset(0f, 6f));
			SpawnEnemy(VavakiaAdvanced, GetOffset(0f, -6f));
			break;
		case 3:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Constellation - Asmodeus]</b></color>: <i>\"Balor hellfire, and a Kolyarut sent to see that the void keeps its bargains. Maintain discipline and prevail!\"</i>"));
			SpawnEnemy(BalorStandard, GetOffset(0f, 7f));
			SpawnEnemy(InevitableKolyarutStandard, GetOffset(0f, -7f));
			SpawnEnemy(MarilithMythic, GetOffset(-6f, 0f));
			break;
		}
	}

	private void SpawnMultiverseThresholdWave(int wave)
	{
		switch (wave)
		{
		case 1:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"Multiversal rifts fracture reality. Mythic horrors manifest.\"</i>"));
			SpawnEnemy(MythicDerakniFighter, GetOffset(-5f, 2f));
			SpawnEnemy(MythicDerakniFighter, GetOffset(5f, 2f));
			SpawnEnemy(ColoxusCaster, GetOffset(0f, 6f));
			break;
		case 2:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Iomedae]</b></color>: <i>\"The Balor Bloodrager marches forth! Hold the line with unshakable resolve!\"</i>"));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(0f, 7f));
			SpawnEnemy(ColoxusCaster, GetOffset(-5f, -3f));
			break;
		case 3:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Nethys]</b></color>: <i>\"A high-tier Inevitable enforcer intervenes to enforce multiversal equilibrium!\"</i>"));
			SpawnEnemy(KolyarutEpic, GetOffset(0f, 7f));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(0f, -7f));
			break;
		case 4:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - The Lantern King]</b></color>: <i>\"FINALE! The supreme Inevitable and apex Balor team up to test your godly might!\"</i>"));
			SpawnEnemy(KolyarutMiniboss, GetOffset(0f, 7f));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(-6f, 0f));
			SpawnEnemy(KolyarutEpic, GetOffset(0f, -7f));
			break;
		}
	}

	private void SpawnAstralWave(int wave)
	{
		switch (wave)
		{
		case 1:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"Astral currents converge upon the colosseum.\"</i>"));
			SpawnEnemy(MarilithMythic, GetOffset(-5f, 0f));
			SpawnEnemy(VavakiaAdvanced, GetOffset(5f, 0f));
			break;
		case 2:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Nethys]</b></color>: <i>\"Cosmic Inevitables enter the arena in pairs!\"</i>"));
			SpawnEnemy(InevitableKolyarutStandard, GetOffset(-4f, 4f));
			SpawnEnemy(InevitableKolyarutStandard, GetOffset(4f, -4f));
			SpawnEnemy(ColoxusCaster, GetOffset(0f, 6f));
			break;
		case 3:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Constellation - Gorum]</b></color>: <i>\"Dual Balor warlords! Crush their iron spines!\"</i>"));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(-5f, 0f));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(5f, 0f));
			break;
		case 4:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The Astral Monarch descends with the apex enforcers of the cosmos!\"</i>"));
			SpawnEnemy(KolyarutMiniboss, GetOffset(0f, 7f));
			SpawnEnemy(KolyarutEpic, GetOffset(0f, -7f));
			SpawnEnemy(GlabrezuAdvanced, GetOffset(6f, 0f));
			break;
		}
	}

	private void SpawnGenesisWave(int wave)
	{
		switch (wave)
		{
		case 1:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The primordial genesis flame ignites. Pre-cosmic terrors emerge.\"</i>"));
			SpawnEnemy(MythicDerakniFighter, GetOffset(-5f, 2f));
			SpawnEnemy(InevitableKolyarutStandard, GetOffset(5f, 2f));
			SpawnEnemy(ColoxusCaster, GetOffset(0f, 6f));
			break;
		case 2:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Constellation - Asmodeus]</b></color>: <i>\"Balor warlords accompanied by mythic Marilith executioners.\"</i>"));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(0f, 7f));
			SpawnEnemy(MarilithMythic, GetOffset(-6f, 0f));
			SpawnEnemy(VavakiaAdvanced, GetOffset(6f, 0f));
			break;
		case 3:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Nethys]</b></color>: <i>\"Reality tears at the seams! Epic Axiomatic Guardians engage!\"</i>"));
			SpawnEnemy(KolyarutEpic, GetOffset(0f, 7f));
			SpawnEnemy(GlabrezuAdvanced, GetOffset(0f, -7f));
			break;
		case 4:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - The Lantern King]</b></color>: <i>\"GENESIS APEX! The grand overseer manifests with legion vanguards!\"</i>"));
			SpawnEnemy(KolyarutMiniboss, GetOffset(0f, 7f));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(-6f, 0f));
			SpawnEnemy(KolyarutEpic, GetOffset(0f, -7f));
			break;
		}
	}

	private void SpawnCosmicChallengeWave(int wave)
	{
		switch (wave)
		{
		case 1:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[The Cosmic Challenge - Round 1/5]</b></color> <b>The Sylvan & Spectral Wardens: a spectral warden & the Hamadryad Queen!</b>"));
			SpawnEnemy(Dlc4(GhostDruidess, GhostlyGuardStandard), GetOffset(-4f, 4f));
			SpawnEnemy(HamadryadQueen, GetOffset(4f, 4f));
			break;
		case 2:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[The Cosmic Challenge - Round 2/5]</b></color> <b>The Abyssal Behemoths: Glabrezu Advanced & Vavakia Dreadnought!</b>"));
			SpawnEnemy(GlabrezuAdvanced, GetOffset(-5f, 0f));
			SpawnEnemy(VavakiaAdvanced, GetOffset(5f, 0f));
			break;
		case 3:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[The Cosmic Challenge - Round 3/5]</b></color> <b>The Warlords of Ruin: Balor Mythic Bloodrager & Marilith Mythic!</b>"));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(0f, 7f));
			SpawnEnemy(MarilithMythic, GetOffset(0f, -7f));
			break;
		case 4:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[The Cosmic Challenge - Round 4/5]</b></color> <b>The Axiomatic Enforcers: Dual Inevitable Kolyaruts!</b>"));
			SpawnEnemy(KolyarutMiniboss, GetOffset(-5f, 0f));
			SpawnEnemy(KolyarutEpic, GetOffset(5f, 0f));
			break;
		case 5:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[The Cosmic Challenge - FINAL PINNACLE]</b></color> <b>THE COSMIC ARBITER AND APEX WARLORDS MANIFEST!</b>"));
			SpawnEnemy(KolyarutMiniboss, GetOffset(0f, 7f));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(-6f, 0f));
			SpawnEnemy(BalorMythicBloodrager, GetOffset(6f, 0f));
			SpawnEnemy(KolyarutEpic, GetOffset(0f, -7f));
			break;
		}
	}

	private void SpawnStolenLandsWave(int wave)
	{
		string rulerName = KingmakerCrossSaveManager.GetRulerName();
		switch (wave)
		{
		case 1:
			PostLog(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#004604><b>[Stolen Lands Legacy Cup - Round 1/3]</b></color> <i>Envoys of the ancient wilderness ruled by <b>{0}</b> enter the arena!</i>", rulerName));
			SpawnEnemy(RedcapBarbarian, GetOffset(-5f, 2f));
			SpawnEnemy(SatyrSwordsman, GetOffset(5f, 2f));
			SpawnEnemy(Dryad, GetOffset(0f, 6f));
			break;
		case 2:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#004604><b>[Stolen Lands Legacy Cup - Round 2/3]</b></color> <b>The Primal Forest Awakens: Bloom Treants & Wild Hunt Scouts!</b>"));
			SpawnEnemy(PrimalTreant, GetOffset(0f, 7f));
			SpawnEnemy(WildHuntScout, GetOffset(-5f, 0f));
			SpawnEnemy(WildHuntArcher, GetOffset(5f, 0f));
			break;
		case 3:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#004604><b>[Stolen Lands Legacy Cup - FINAL]</b></color> <b>THE WILD HUNT MONARCH & THE SHADOW ANKOU STEP FORTH!</b>"));
			SpawnEnemy(WildHuntMonarch, GetOffset(0f, 7f));
			SpawnEnemy(Ankou, GetOffset(-5f, 2f));
			SpawnEnemy(HamadryadQueen, GetOffset(5f, 2f));
			break;
		}
	}

	private void SpawnPersonalTrialWave(ArenaType trial)
	{
		switch (trial)
		{
		case ArenaType.TrialPhantomSwordSaint:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Planar Coliseum]</b></color> <i>\"The challenge is accepted.\" The mist over the sands takes the shape of a Sarkorian sword-saint, and two of his dead come with him.</i>"));
			SpawnEnemy(GhostBarbarianSarkorian, GetOffset(0f, 6f));
			SpawnEnemy(GhostStandard, GetOffset(-5f, 2f));
			SpawnEnemy(GhostStandard, GetOffset(5f, 2f));
			break;
		case ArenaType.TrialBladeSpirit:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Planar Coliseum]</b></color> <i>The Blade Spirit steps out of the mist without a sound, and the bones of two champions rise to keep the ring honest.</i>"));
			SpawnEnemy(GhostFighterSarkorian, GetOffset(0f, 7f));
			SpawnEnemy(SkeletalChampion, GetOffset(-5f, 2f));
			SpawnEnemy(SkeletalChampion, GetOffset(5f, 2f));
			break;
		case ArenaType.TrialBoneCourt:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#3F3F3F><b>[Planar Coliseum]</b></color> <i>The bone court convenes: a beast of knitted bone, and a champion who has not knelt to anything yet.</i>"));
			SpawnEnemy(BloodyBonesBarbarian, GetOffset(0f, 6f));
			SpawnEnemy(SkeletalChampion, GetOffset(-5f, 2f));
			break;
		case ArenaType.TrialCorrosiveWyrm:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#004604><b>[Planar Coliseum]</b></color> <i>A rift drake lands on the sand, and everything it breathes on begins to smoke.</i>"));
			SpawnEnemy(RiftDrakeStandard, GetOffset(0f, 6f));
			break;
		case ArenaType.TrialPrimordialBehemoth:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> <i>The behemoth comes through the far gate shoulder first, and the contaminator walks in its shadow.</i>"));
			SpawnEnemy(NalfeshneeStandard, GetOffset(0f, 7f));
			SpawnEnemy(GibrilethElite, GetOffset(-6f, 2f));
			break;
		case ArenaType.TrialFallenVanguard:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> <i>A crusader vanguard that never came home takes the sands: its guard captain, and two of the things grief made of the rest.</i>"));
			SpawnEnemy(SkeletalChampionGuard, GetOffset(0f, 6f));
			SpawnEnemy(CorpulentSpectre, GetOffset(-5f, 2f));
			SpawnEnemy(CorpulentSpectre, GetOffset(5f, 2f));
			break;
		case ArenaType.TrialThroneOfShadows:
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> <i>The light on the sand thins to nothing. What is sitting on the throne stands up, and its votaries stand with it.</i>"));
			SpawnEnemy(DeepShadowDemonTough, GetOffset(0f, 7f));
			SpawnEnemy(Shadowcaster, GetOffset(-6f, 2f));
			SpawnEnemy(Shadowcaster, GetOffset(6f, 2f));
			break;
		}
	}

	private Vector3 GetOffset(float x, float z)
	{
		return NpcLayout.FightPoint(_arenaAnchor, _arenaForward, x, z);
	}

	private void SpawnEnemy(BlueprintUnit unitBp, Vector3 position)
	{
		if (unitBp == null || Game.Instance?.LoadedAreaState?.MainState == null)
		{
			return;
		}
		try
		{
			position = IsekaiNpcUnit.PlaceCombatant(position, _arenaForward, _placedThisWave);
			_placedThisWave.Add(position);
			UnitEntityData unitEntityData = Game.Instance.EntityCreator.SpawnUnit(unitBp, position, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
			if (unitEntityData != null)
			{
				unitEntityData.SwitchFactions(BlueprintRoot.Instance.Cheats.Enemy.Faction, resetAttackFactions: true);
				UnitEntityData unitEntityData2 = Game.Instance?.Player?.MainCharacter.Value;
				if (unitEntityData2 != null)
				{
					unitEntityData.CombatState?.Engage(unitEntityData2);
				}
				_activeEnemyIds.Add(unitEntityData.UniqueId);
				_spawnedEnemyIds.Add(unitEntityData.UniqueId);
				SaveScopedStorage.MarkSpawned(unitEntityData, "ColiseumContestant");
				DimensionalReinforcements.RegisterExternalSpawnedMob(unitEntityData.UniqueId);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error spawning coliseum enemy: " + ex);
		}
	}

	public void OnUnitDie(UnitEntityData unit)
	{
		if (!IsActive || unit == null || _isSpawningWave || CurrentCup == null || !_activeEnemyIds.Remove(unit.UniqueId))
		{
			return;
		}
		if (_activeEnemyIds.Count == 0)
		{
			if (CurrentWave >= CurrentCup.MaxWaves)
			{
				FinishArena();
				return;
			}
			PostLog(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#004604><b>[Planar Coliseum]</b></color> <b>Wave {0} Cleared!</b> The arena sand trembles as greater challenges prepare to enter...", CurrentWave));
			SpawnNextWave();
		}
		else
		{
			PersistTournament();
		}
	}

	private void FinishArena()
	{
		ColiseumCup currentCup = CurrentCup;
		IsActive = false;
		_activeEnemyIds.Clear();
		if (currentCup == null)
		{
			ResetArenaState();
			return;
		}
		Quiet("<color=#5B2A86><b>=======================================================</b></color>");
		PostLog(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum] VICTORY! Every wave of the {0} has fallen!</b></color>", currentCup.DisplayName));
		SpawnLanternKingCameo();
		PostConstellationVictoryReactions(currentCup);
		bool num = currentCup.Kind == ColiseumCupKind.PersonalTrial;
		Dictionary<string, string> storage = (num ? null : ColiseumProgress.Ensure());
		bool flag = !num && ColiseumProgress.RecordClear(storage, currentCup.Id);
		if (!num)
		{
			bool flag2 = flag || ColiseumProgress.TryClaimPaidReplay(storage, currentCup.Id, GameCalendar.CurrentDay());
			ColiseumPurse coliseumPurse = ColiseumRewards.For(currentCup, flag, flag2);
			Game.Instance.Player.GainPartyExperience(coliseumPurse.Experience, ExperienceGainStatistic.GainType.Mob);
			Game.Instance.Player.GainMoney(coliseumPurse.Gold);
			DivineTokens.AddClaimedCoinsWithoutLoopMultiplier(coliseumPurse.Coins, ConstellationText.LocalizedFormat("Constellation.CosmicCoins.Sponsor.Coliseum", "Planar Coliseum Victory ({0})", ConstellationText.LocalizeIfKnown(currentCup.DisplayName)));
			PostLog(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Tournament Spoils]</b></color> Awarded <b>+{0} XP</b>, <b>+{1} Gold</b>, and <b>+{2} Cosmic Coins</b>!", coliseumPurse.Experience, coliseumPurse.Gold, coliseumPurse.Coins));
			if (!flag & flag2)
			{
				Quiet(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#3F3F3F><i>[Tournament Spoils] The constellations have seen you win the {0} before. Today's replay pays a tenth of its gold and experience and a quarter of its coins; a further replay today pays only a token purse.</i></color>", currentCup.DisplayName));
			}
			else if (!flag)
			{
				Quiet(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#3F3F3F><i>[Tournament Spoils] The audience has already had its {0} rematch today. The constellations toss you a token purse and wait for tomorrow.</i></color>", currentCup.DisplayName));
			}
		}
		Quiet("<color=#5B2A86><b>=======================================================</b></color>");
		OnCupCleared(currentCup, flag);
		ClearSpawnedUnits(clearCameo: false);
		ClearPersistedTournament(clearCameo: true);
		if (!string.IsNullOrEmpty(_activeCameoId))
		{
			PersistCameo(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.ColiseumTournament"));
		}
		CurrentCup = null;
	}

	private static void OnCupCleared(ColiseumCup cup, bool firstClear)
	{
		if (cup.Kind == ColiseumCupKind.PersonalTrial)
		{
			try
			{
				PersonalQuestDirector.OnTrialCleared(cup.Id);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] The personal quest could not record its trial: " + ex);
			}
			CupCleared?.Invoke(cup.Id, firstClear);
			return;
		}
		try
		{
			IsekaiColiseumQuest.OnCupCleared(cup.Id);
		}
		catch (Exception ex2)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] The coliseum quest could not record the clear: " + ex2);
		}
		try
		{
			PersonalQuestDirector.Reconcile();
		}
		catch (Exception ex3)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] The personal quests could not follow the clear: " + ex3);
		}
		if (cup.Kind == ColiseumCupKind.StolenLands)
		{
			SylvanFeyAllianceQuest.OnWildHuntDefeated();
		}
		CupCleared?.Invoke(cup.Id, firstClear);
	}

	private void SpawnLanternKingCameo()
	{
		BlueprintUnit cameo = LanternKingCameo.Unit;
		if (cameo == null)
		{
			return;
		}
		try
		{
			if (!(Game.Instance?.Player?.MainCharacter.Value != null) || Game.Instance?.LoadedAreaState?.MainState == null)
			{
				return;
			}
			LanternKingCameo.ApplyModel(LanternKingAvatar != null && IsDlcOwned("c2340df3fdaf403baffe824ae7a0a547"));
			RemoveCameo();
			foreach (UnitEntityData item in LoadedUnits()?.Where((UnitEntityData u) => u != null && !u.Destroyed && u.Blueprint == cameo).ToList() ?? new List<UnitEntityData>())
			{
				item.MarkForDestroy();
			}
			FixArenaFrame();
			List<Vector3> taken = IsekaiNpcUnit.WithParty(LoadedUnits()?.Where((UnitEntityData u) => u != null && !u.Destroyed && u.IsInGame && u.Blueprint != cameo).Select((UnitEntityData u) => u.Position));
			Vector3 position = PlanarColiseumHubManager.PlaceColiseumNpc("LanternKingCameoUnit", _arenaAnchor, _arenaForward, taken);
			UnitEntityData unitEntityData = Game.Instance.EntityCreator.SpawnUnit(cameo, position, Quaternion.LookRotation(-_arenaForward), Game.Instance.LoadedAreaState.MainState);
			if (unitEntityData != null)
			{
				IsekaiNpcUnit.Protect(unitEntityData);
				SaveScopedStorage.MarkSpawned(unitEntityData, "ColiseumCameo");
				_activeCameoId = unitEntityData.UniqueId;
				LanternKingReveal.OnCameoAppeared();
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error manifesting Lantern King cameo: " + ex);
		}
	}

	private void PostConstellationVictoryReactions(ColiseumCup cup)
	{
		Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[The Lantern King]</b></color>: <i>\"Tell me, transmigrator... does this crusade feel at all familiar to you? BRAVO! The roar of the coliseum reaches across the cosmos! The constellations are singing your praises!\"</i>"));
		switch (cup.Waves)
		{
		case ArenaType.Act1FaeGrove:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#004604><b>[Constellation - Desna]</b></color>: <i>\"A dazzling debut! You danced through the First World's briars with laughter in your heart! The Song of the Spheres echoes your triumph!\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Cayden Cailean]</b></color>: <i>\"Bwahaha! Now that's what I call a street brawl in a fairy garden! The next round of celestial ale is on me!\"</i>"));
			break;
		case ArenaType.Act2LostLegions:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#3F3F3F><b>[Constellation - Pharasma]</b></color>: <i>\"The restless dead have been returned to eternal rest. The scales of fate remain balanced by your hand.\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Constellation - Gorum]</b></color>: <i>\"IRON AND BLOOD! You shattered their bones and crushed their crypts! A true warrior's glory!\"</i>"));
			break;
		case ArenaType.Act3MidnightColosseum:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Iomedae]</b></color>: <i>\"Valiant Commander! The demonic terrors of the Abyss crumble before your righteous resolve. Drezen stands unbroken!\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Constellation - Asmodeus]</b></color>: <i>\"Ruthless discipline and total domination. A textbook demonstration of superior order.\"</i>"));
			break;
		case ArenaType.StolenLandsLegacyCup:
		{
			string rulerName = KingmakerCrossSaveManager.GetRulerName();
			Voice(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#004604><b>[Constellation - Sylvan Court]</b></color>: <i>\"The memory of {0}'s kingdom burns eternal! The First World salutes the inheritor of the Stolen Lands!\"</i>", rulerName));
			break;
		}
		case ArenaType.Act4VoidRift:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Calistria]</b></color>: <i>\"Mmm, delicious agony! Watching those arrogant abyssal blademasters fall back into the dark was sheer ecstasy, darling.\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Nethys]</b></color>: <i>\"Fascinating... the magic tearing through the void resonates with pure arcane brilliance!\"</i>"));
			break;
		case ArenaType.Act5MultiverseThreshold:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Iomedae]</b></color>: <i>\"The threshold of mortality has been breached! You stand as a beacon of transcendent power across the multiverse!\"</i>"));
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Constellation - Gorum]</b></color>: <i>\"APEX GLORY! Even the heavens shake when your war-cry sounds!\"</i>"));
			break;
		case ArenaType.AstralCup:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#133E71><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The cosmic spheres turn in reverence. Beyond the veil of time, your feats are etched into eternity.\"</i>"));
			break;
		case ArenaType.GenesisCup:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"Primordial genesis acknowledged. The fabric of reality bends to your awakened sovereignty.\"</i>"));
			break;
		case ArenaType.TheCosmicChallenge:
			Voice(ConstellationText.Localized("Constellation.Coliseum", "<color=#5B2A86><b>[THE CONSTELLATIONS ASSEMBLE]</b></color>: <i>\"BEHOLD THE TRANSCENDENT VICTOR! Desna, Gorum, Iomedae, Calistria, Nethys, Asmodeus, and Yog-Sothoth unite their divine voices in celestial chorus! A mortal who has conquered the pinnacle of all creation!\"</i>"));
			break;
		}
	}

	private static void PostLog(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, message, null, ConstellationCategory.Combat, "Planar Coliseum");
	}

	private static void Voice(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, message, null, ConstellationCategory.Combat, "Planar Coliseum");
	}

	private static void Quiet(string message)
	{
		ConstellationChatManager.PostCombatLog(message);
	}
}
