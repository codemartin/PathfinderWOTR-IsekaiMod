using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.KingmakerIntegration;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.QA.Statistics;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace IsekaiMod.Content.Arenas
{
	public class DimensionalArenaManager : IUnitHandler, ISubscriber, IGlobalSubscriber, IUnitSpawnHandler, IAreaHandler
	{
		private static bool _initialized = false;

		private static DimensionalArenaManager _instance;

		private readonly HashSet<string> _activeEnemyIds = new HashSet<string>();

		private readonly List<string> _spawnedEnemyIds = new List<string>();

		private string _activeCameoId;

		private Vector3 _arenaCenter;

		private static readonly BlueprintUnit SatyrArcher = BlueprintTools.GetBlueprint<BlueprintUnit>("6114adae73a1338498412b3c63c946f8");

		private static readonly BlueprintUnit SatyrSwordsman = BlueprintTools.GetBlueprint<BlueprintUnit>("a6dceccaf84af1a42843e58412265388");

		private static readonly BlueprintUnit Dryad = BlueprintTools.GetBlueprint<BlueprintUnit>("20660a3d7ef5ec54a9c1f08b0b58d753");

		private static readonly BlueprintUnit Redcap = BlueprintTools.GetBlueprint<BlueprintUnit>("99286885f5be6b948baf3de77471d6ca");

		private static readonly BlueprintUnit Nymph = BlueprintTools.GetBlueprint<BlueprintUnit>("0cc7a2526e4557945b1d8eb277d1fb3a");

		private static readonly BlueprintUnit RedcapBarbarian = BlueprintTools.GetBlueprint<BlueprintUnit>("2e4a9f6fa84651148bc065c5330817fe");

		private static readonly BlueprintUnit HamadryadQueen = BlueprintTools.GetBlueprint<BlueprintUnit>("b8972cfe36e3cd945bbd2c4c320d5237");

		private static readonly BlueprintUnit LanternKingAvatar = BlueprintTools.GetBlueprint<BlueprintUnit>("8e74850f1bb94808b3c46822f6edd739");

		private static readonly BlueprintFaction NeutralsFaction = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");

		private static readonly BlueprintUnit GhoulStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("2c0fd840d87c3b4418ae958c9a813fe0");

		private static readonly BlueprintUnit GhoulCreeper = BlueprintTools.GetBlueprint<BlueprintUnit>("492e9c74335b7b140bb5cab8413f7a42");

		private static readonly BlueprintUnit WightWizard = BlueprintTools.GetBlueprint<BlueprintUnit>("5e02d1f447e542758a9dd9a80777d5b0");

		private static readonly BlueprintUnit WightFighter = BlueprintTools.GetBlueprint<BlueprintUnit>("09737a3ed9c248d5ba8697a4eb7452b3");

		private static readonly BlueprintUnit SkeletalChampion = BlueprintTools.GetBlueprint<BlueprintUnit>("622b8701c4f2468479b9f533c2cc24e5");

		private static readonly BlueprintUnit SkeletalChampionArcher = BlueprintTools.GetBlueprint<BlueprintUnit>("a24e752e4748bd548936020938effee1");

		private static readonly BlueprintUnit GhostDruidess = BlueprintTools.GetBlueprint<BlueprintUnit>("d0b3eb1737dd433b9d7c39544acb6e13");

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

		private static readonly BlueprintUnit InevitableKolyarutMiniboss = BlueprintTools.GetBlueprint<BlueprintUnit>("3b4b1b05783044f0b75f6f3afd9e6d7f") ?? BalorMythicBloodrager ?? BalorStandard;

		private static readonly BlueprintUnit WildHuntMonarch = BlueprintTools.GetBlueprint<BlueprintUnit>("573009c2f6493514188a2844ba53bdf8") ?? HamadryadQueen ?? SatyrSwordsman;

		private static readonly BlueprintUnit WildHuntScout = BlueprintTools.GetBlueprint<BlueprintUnit>("6f5ff0f1e359ee042ba49a746a507190") ?? SatyrSwordsman;

		private static readonly BlueprintUnit WildHuntArcher = BlueprintTools.GetBlueprint<BlueprintUnit>("e21b6536b40aaad4e9c9cd6c216778a3") ?? SatyrArcher;

		private static readonly BlueprintUnit Ankou = BlueprintTools.GetBlueprint<BlueprintUnit>("24fcf04e625a5b945bad7f3978072836") ?? Nymph ?? Redcap;

		private static readonly BlueprintUnit PrimalTreant = BlueprintTools.GetBlueprint<BlueprintUnit>("b2c86a184c5f9c942aeb77e52ea1d17d") ?? RedcapBarbarian;

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

		public ArenaType CurrentArena { get; private set; }

		public int CurrentWave { get; private set; }

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

		public void OnAreaDidLoad()
		{
			ResetArenaState();
		}

		public void OnAreaBeginUnloading()
		{
			ResetArenaState();
		}

		public void ResetArenaState()
		{
			IsActive = false;
			CurrentWave = 0;
			ClearSpawnedUnits();
		}

		public void ClearSpawnedUnits(bool clearCameo = true)
		{
			try
			{
				IEnumerable<UnitEntityData> enumerable = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>();
				if (enumerable != null)
				{
					foreach (string id in _spawnedEnemyIds)
					{
						enumerable.FirstOrDefault((UnitEntityData x) => x.UniqueId == id)?.FadeOutViewAndDestroy();
					}
					if (clearCameo && !string.IsNullOrEmpty(_activeCameoId))
					{
						enumerable.FirstOrDefault((UnitEntityData x) => x.UniqueId == _activeCameoId)?.FadeOutViewAndDestroy();
						_activeCameoId = null;
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error clearing spawned arena units: " + ex);
			}
			_spawnedEnemyIds.Clear();
			_activeEnemyIds.Clear();
		}

		public static void StartArena(ArenaType type)
		{
			Instance.Begin(type);
		}

		public void Begin(ArenaType type)
		{
			UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
			if (unitEntityData == null)
			{
				return;
			}
			BlueprintArea currentlyLoadedArea = Game.Instance.CurrentlyLoadedArea;
			if (currentlyLoadedArea != null && currentlyLoadedArea.IsGlobalMap)
			{
				PostLog("<color=#DC143C><b>[Planar Coliseum]</b></color> You cannot initiate a tournament while traversing the Worldwound overland! Enter a local area or the Planar Coliseum.");
				return;
			}
			if (type == ArenaType.StolenLandsLegacyCup && !KingmakerCrossSaveManager.IsKingmakerInstalled)
			{
				PostLog("<color=#DC143C><b>[Planar Coliseum]</b></color> The Stolen Lands Legacy Cup requires a detected Pathfinder: Kingmaker installation.");
				return;
			}
			Game instance = Game.Instance;
			if (instance != null && instance.Player?.IsInCombat == true)
			{
				PostLog("<color=#DC143C><b>[Planar Coliseum]</b></color> You cannot initiate a tournament while already engaged in battle!");
				return;
			}
			if (IsActive)
			{
				PostLog("<color=#DC143C><b>[Planar Coliseum]</b></color> An active tournament is already underway!");
				return;
			}
			ClearSpawnedUnits();
			IsActive = true;
			CurrentArena = type;
			CurrentWave = 0;
			_activeEnemyIds.Clear();
			_arenaCenter = unitEntityData.Position;
			PostLog($"<color=#9400D3><b>[Planar Coliseum]</b></color> <b>Rifts tear open across the arena sands! The trial of {type} has commenced!</b>");
			SpawnNextWave();
		}

		private int GetMaxWaves(ArenaType type)
		{
			switch (type)
			{
			case ArenaType.BronzeCup:
			case ArenaType.SilverCup:
			case ArenaType.GoldCup:
			case ArenaType.PlatinumCup:
			case ArenaType.StolenLandsLegacyCup:
				return 3;
			case ArenaType.DiamondCup:
			case ArenaType.AstralCup:
			case ArenaType.GenesisCup:
			case ArenaType.Act1FaeGrove:
			case ArenaType.Act2LostLegions:
			case ArenaType.Act3MidnightColosseum:
			case ArenaType.Act4VoidRift:
			case ArenaType.Act5MultiverseThreshold:
				return 4;
			case ArenaType.TheCosmicChallenge:
				return 5;
			default:
				return 3;
			}
		}

		private void SpawnNextWave()
		{
			CurrentWave++;
			_activeEnemyIds.Clear();
			switch (CurrentArena)
			{
			case ArenaType.BronzeCup:
			case ArenaType.Act1FaeGrove:
				SpawnBronzeWave(CurrentWave);
				break;
			case ArenaType.SilverCup:
			case ArenaType.Act2LostLegions:
				SpawnSilverWave(CurrentWave);
				break;
			case ArenaType.GoldCup:
			case ArenaType.Act3MidnightColosseum:
				SpawnGoldWave(CurrentWave);
				break;
			case ArenaType.PlatinumCup:
			case ArenaType.Act4VoidRift:
				SpawnPlatinumWave(CurrentWave);
				break;
			case ArenaType.DiamondCup:
			case ArenaType.Act5MultiverseThreshold:
				SpawnDiamondWave(CurrentWave);
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
				SpawnBronzeWave(CurrentWave);
				break;
			}
			if (_activeEnemyIds.Count == 0 && IsActive)
			{
				PostLog("<color=#DC143C><b>[Planar Coliseum]</b></color> No adversaries answered the summons.");
				FinishArena();
			}
		}

		private void SpawnBronzeWave(int wave)
		{
			switch (wave)
			{
			case 1:
				PostLog("<color=#7CFC00><b>[Constellation - Desna]</b></color>: <i>\"Swift arrows and playful laughter! First World scouts test your footwork!\"</i>");
				SpawnEnemy(SatyrArcher, GetOffset(5f, 0f));
				SpawnEnemy(SatyrSwordsman, GetOffset(-5f, 2f));
				SpawnEnemy(Dryad, GetOffset(0f, 6f));
				break;
			case 2:
				PostLog("<color=#FF69B4><b>[Constellation - Calistria]</b></color>: <i>\"Cruel little Redcaps and alluring Nymphs. Paint the dirt red, darling.\"</i>");
				SpawnEnemy(Redcap, GetOffset(-6f, 3f));
				SpawnEnemy(Redcap, GetOffset(6f, -3f));
				SpawnEnemy(Nymph, GetOffset(0f, 7f));
				break;
			case 3:
			case 4:
				PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The Hamadryad Queen steps forward to defend her sacred grove.\"</i>");
				SpawnEnemy(HamadryadQueen, GetOffset(0f, 7f));
				SpawnEnemy(RedcapBarbarian, GetOffset(-5f, 4f));
				SpawnEnemy(RedcapBarbarian, GetOffset(5f, 4f));
				break;
			default:
				FinishArena();
				break;
			}
		}

		private void SpawnSilverWave(int wave)
		{
			switch (wave)
			{
			case 1:
				PostLog("<color=#708090><b>[Constellation - Pharasma]</b></color>: <i>\"Restless ghouls crawl from ancient barrows. Put them to eternal sleep.\"</i>");
				SpawnEnemy(GhoulStandard, GetOffset(4f, 2f));
				SpawnEnemy(GhoulStandard, GetOffset(-4f, -2f));
				SpawnEnemy(GhoulCreeper, GetOffset(0f, 5f));
				break;
			case 2:
				PostLog("<color=#FF4500><b>[Constellation - Gorum]</b></color>: <i>\"Wight fighters and crypt sorcerers! Strike them down without hesitation!\"</i>");
				SpawnEnemy(WightFighter, GetOffset(5f, 0f));
				SpawnEnemy(WightFighter, GetOffset(-5f, 0f));
				SpawnEnemy(WightWizard, GetOffset(0f, 6f));
				break;
			case 3:
			case 4:
				PostLog("<color=#FFD700><b>[Constellation - Iomedae]</b></color>: <i>\"The spectral warden of the fallen legions manifests with her skeletal champions!\"</i>");
				SpawnEnemy(GhostDruidess, GetOffset(0f, 6f));
				SpawnEnemy(SkeletalChampion, GetOffset(-5f, 2f));
				SpawnEnemy(SkeletalChampionArcher, GetOffset(5f, -2f));
				break;
			default:
				FinishArena();
				break;
			}
		}

		private void SpawnGoldWave(int wave)
		{
			switch (wave)
			{
			case 1:
				PostLog("<color=#FF69B4><b>[Constellation - Calistria]</b></color>: <i>\"Demonic gladiators eager to feast on your flesh! Show them agony!\"</i>");
				SpawnEnemy(DerakniStandard, GetOffset(-5f, 0f));
				SpawnEnemy(SchirAdvancedMelee, GetOffset(5f, 0f));
				break;
			case 2:
				PostLog("<color=#00BFFF><b>[Constellation - Nethys]</b></color>: <i>\"Coloxus spellcasters bend the arcane winds of the colosseum!\"</i>");
				SpawnEnemy(ColoxusStandard, GetOffset(0f, 6f));
				SpawnEnemy(DerakniAdvanced, GetOffset(-6f, -3f));
				SpawnEnemy(DerakniAdvanced, GetOffset(6f, -3f));
				break;
			case 3:
			case 4:
				PostLog("<color=#DC143C><b>[Constellation - Asmodeus]</b></color>: <i>\"The four-armed Glabrezu apex titan enters the ring. Execute it with cold precision.\"</i>");
				SpawnEnemy(GlabrezuAdvanced, GetOffset(0f, 7f));
				SpawnEnemy(ColoxusEmissary, GetOffset(-5f, 0f));
				SpawnEnemy(ColoxusEmissary, GetOffset(5f, 0f));
				break;
			default:
				FinishArena();
				break;
			}
		}

		private void SpawnPlatinumWave(int wave)
		{
			switch (wave)
			{
			case 1:
				PostLog("<color=#FF4500><b>[Constellation - Gorum]</b></color>: <i>\"Six-bladed Mariliths and serpentine Vavakia dreadnoughts! FORGE YOUR LEGEND!\"</i>");
				SpawnEnemy(MarilithStandard, GetOffset(-5f, 0f));
				SpawnEnemy(VavakiaStandard, GetOffset(5f, 0f));
				break;
			case 2:
				PostLog("<color=#7CFC00><b>[Constellation - Desna]</b></color>: <i>\"The void winds howl! Advanced abyssal war-beasts descend!\"</i>");
				SpawnEnemy(MarilithAdvanced, GetOffset(0f, 6f));
				SpawnEnemy(VavakiaAdvanced, GetOffset(0f, -6f));
				break;
			case 3:
			case 4:
				PostLog("<color=#DC143C><b>[Constellation - Asmodeus]</b></color>: <i>\"Balor hellfire and an axiomatic Kolyarut enforcer! Maintain discipline and prevail!\"</i>");
				SpawnEnemy(BalorStandard, GetOffset(0f, 7f));
				SpawnEnemy(InevitableKolyarutStandard, GetOffset(0f, -7f));
				SpawnEnemy(MarilithMythic, GetOffset(-6f, 0f));
				break;
			default:
				FinishArena();
				break;
			}
		}

		private void SpawnDiamondWave(int wave)
		{
			switch (wave)
			{
			case 1:
				PostLog("<color=#9400D3><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"Multiversal rifts fracture reality. Mythic horrors manifest.\"</i>");
				SpawnEnemy(MythicDerakniFighter, GetOffset(-5f, 2f));
				SpawnEnemy(MythicDerakniFighter, GetOffset(5f, 2f));
				SpawnEnemy(ColoxusCaster, GetOffset(0f, 6f));
				break;
			case 2:
				PostLog("<color=#FFD700><b>[Constellation - Iomedae]</b></color>: <i>\"The Balor Bloodrager marches forth! Hold the line with unshakable resolve!\"</i>");
				SpawnEnemy(BalorMythicBloodrager, GetOffset(0f, 7f));
				SpawnEnemy(ColoxusCaster, GetOffset(-5f, -3f));
				break;
			case 3:
			{
				PostLog("<color=#00BFFF><b>[Constellation - Nethys]</b></color>: <i>\"A high-tier Inevitable enforcer intervenes to enforce multiversal equilibrium!\"</i>");
				BlueprintUnit unitBp3 = InevitableKolyarutEpic ?? InevitableKolyarutStandard;
				SpawnEnemy(unitBp3, GetOffset(0f, 7f));
				SpawnEnemy(BalorMythicBloodrager, GetOffset(0f, -7f));
				break;
			}
			case 4:
			{
				PostLog("<color=#FF8C00><b>[Constellation - The Lantern King]</b></color>: <i>\"FINALE! The supreme Inevitable and apex Balor team up to test your godly might!\"</i>");
				BlueprintUnit unitBp = InevitableKolyarutMiniboss ?? InevitableKolyarutStandard;
				BlueprintUnit unitBp2 = InevitableKolyarutEpic ?? InevitableKolyarutStandard;
				SpawnEnemy(unitBp, GetOffset(0f, 7f));
				SpawnEnemy(BalorMythicBloodrager, GetOffset(-6f, 0f));
				SpawnEnemy(unitBp2, GetOffset(0f, -7f));
				break;
			}
			default:
				FinishArena();
				break;
			}
		}

		private void SpawnAstralWave(int wave)
		{
			switch (wave)
			{
			case 1:
				PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"Astral currents converge upon the colosseum.\"</i>");
				SpawnEnemy(MarilithMythic, GetOffset(-5f, 0f));
				SpawnEnemy(VavakiaAdvanced, GetOffset(5f, 0f));
				break;
			case 2:
				PostLog("<color=#00BFFF><b>[Constellation - Nethys]</b></color>: <i>\"Cosmic Inevitables enter the arena in pairs!\"</i>");
				SpawnEnemy(InevitableKolyarutStandard, GetOffset(-4f, 4f));
				SpawnEnemy(InevitableKolyarutStandard, GetOffset(4f, -4f));
				SpawnEnemy(ColoxusCaster, GetOffset(0f, 6f));
				break;
			case 3:
				PostLog("<color=#FF4500><b>[Constellation - Gorum]</b></color>: <i>\"Dual Balor warlords! Crush their iron spines!\"</i>");
				SpawnEnemy(BalorMythicBloodrager, GetOffset(-5f, 0f));
				SpawnEnemy(BalorMythicBloodrager, GetOffset(5f, 0f));
				break;
			case 4:
			{
				PostLog("<color=#9400D3><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The Astral Monarch descends with the apex enforcers of the cosmos!\"</i>");
				BlueprintUnit unitBp = InevitableKolyarutMiniboss ?? InevitableKolyarutStandard;
				BlueprintUnit unitBp2 = InevitableKolyarutEpic ?? InevitableKolyarutStandard;
				SpawnEnemy(unitBp, GetOffset(0f, 7f));
				SpawnEnemy(unitBp2, GetOffset(0f, -7f));
				SpawnEnemy(GlabrezuAdvanced, GetOffset(6f, 0f));
				break;
			}
			default:
				FinishArena();
				break;
			}
		}

		private void SpawnGenesisWave(int wave)
		{
			switch (wave)
			{
			case 1:
				PostLog("<color=#9400D3><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The primordial genesis flame ignites. Pre-cosmic terrors emerge.\"</i>");
				SpawnEnemy(MythicDerakniFighter, GetOffset(-5f, 2f));
				SpawnEnemy(InevitableKolyarutStandard, GetOffset(5f, 2f));
				SpawnEnemy(ColoxusCaster, GetOffset(0f, 6f));
				break;
			case 2:
				PostLog("<color=#DC143C><b>[Constellation - Asmodeus]</b></color>: <i>\"Balor warlords accompanied by mythic Marilith executioners.\"</i>");
				SpawnEnemy(BalorMythicBloodrager, GetOffset(0f, 7f));
				SpawnEnemy(MarilithMythic, GetOffset(-6f, 0f));
				SpawnEnemy(VavakiaAdvanced, GetOffset(6f, 0f));
				break;
			case 3:
			{
				PostLog("<color=#00BFFF><b>[Constellation - Nethys]</b></color>: <i>\"Reality tears at the seams! Epic Axiomatic Guardians engage!\"</i>");
				BlueprintUnit unitBp3 = InevitableKolyarutEpic ?? InevitableKolyarutStandard;
				SpawnEnemy(unitBp3, GetOffset(0f, 7f));
				SpawnEnemy(GlabrezuAdvanced, GetOffset(0f, -7f));
				break;
			}
			case 4:
			{
				PostLog("<color=#FF8C00><b>[Constellation - The Lantern King]</b></color>: <i>\"GENESIS APEX! The grand overseer manifests with legion vanguards!\"</i>");
				BlueprintUnit unitBp = InevitableKolyarutMiniboss ?? InevitableKolyarutStandard;
				BlueprintUnit unitBp2 = InevitableKolyarutEpic ?? InevitableKolyarutStandard;
				SpawnEnemy(unitBp, GetOffset(0f, 7f));
				SpawnEnemy(BalorMythicBloodrager, GetOffset(-6f, 0f));
				SpawnEnemy(unitBp2, GetOffset(0f, -7f));
				break;
			}
			default:
				FinishArena();
				break;
			}
		}

		private void SpawnCosmicChallengeWave(int wave)
		{
			switch (wave)
			{
			case 1:
				PostLog("<color=#FFD700><b>[The Cosmic Challenge - Round 1/5]</b></color> <b>The Sylvan & Spectral Wardens: Ghost Druidess & Hamadryad Queen!</b>");
				SpawnEnemy(GhostDruidess, GetOffset(-4f, 4f));
				SpawnEnemy(HamadryadQueen, GetOffset(4f, 4f));
				break;
			case 2:
				PostLog("<color=#FFD700><b>[The Cosmic Challenge - Round 2/5]</b></color> <b>The Abyssal Behemoths: Glabrezu Advanced & Vavakia Dreadnought!</b>");
				SpawnEnemy(GlabrezuAdvanced, GetOffset(-5f, 0f));
				SpawnEnemy(VavakiaAdvanced, GetOffset(5f, 0f));
				break;
			case 3:
				PostLog("<color=#FFD700><b>[The Cosmic Challenge - Round 3/5]</b></color> <b>The Warlords of Ruin: Balor Mythic Bloodrager & Marilith Mythic!</b>");
				SpawnEnemy(BalorMythicBloodrager, GetOffset(0f, 7f));
				SpawnEnemy(MarilithMythic, GetOffset(0f, -7f));
				break;
			case 4:
			{
				PostLog("<color=#FFD700><b>[The Cosmic Challenge - Round 4/5]</b></color> <b>The Axiomatic Enforcers: Dual Inevitable Kolyaruts!</b>");
				BlueprintUnit unitBp3 = InevitableKolyarutMiniboss ?? InevitableKolyarutStandard;
				BlueprintUnit unitBp4 = InevitableKolyarutEpic ?? InevitableKolyarutStandard;
				SpawnEnemy(unitBp3, GetOffset(-5f, 0f));
				SpawnEnemy(unitBp4, GetOffset(5f, 0f));
				break;
			}
			case 5:
			{
				PostLog("<color=#FFD700><b>[The Cosmic Challenge - FINAL PINNACLE]</b></color> <b>THE COSMIC ARBITER AND APEX WARLORDS MANIFEST!</b>");
				BlueprintUnit unitBp = InevitableKolyarutMiniboss ?? InevitableKolyarutStandard;
				BlueprintUnit unitBp2 = InevitableKolyarutEpic ?? InevitableKolyarutStandard;
				SpawnEnemy(unitBp, GetOffset(0f, 7f));
				SpawnEnemy(BalorMythicBloodrager, GetOffset(-6f, 0f));
				SpawnEnemy(BalorMythicBloodrager, GetOffset(6f, 0f));
				SpawnEnemy(unitBp2, GetOffset(0f, -7f));
				break;
			}
			default:
				FinishArena();
				break;
			}
		}

		private void SpawnStolenLandsWave(int wave)
		{
			string rulerName = KingmakerCrossSaveManager.GetRulerName();
			switch (wave)
			{
			case 1:
				PostLog("<color=#32CD32><b>[Stolen Lands Legacy Cup - Round 1/3]</b></color> <i>Envoys of the ancient wilderness ruled by <b>" + rulerName + "</b> enter the arena!</i>");
				SpawnEnemy(RedcapBarbarian, GetOffset(-5f, 2f));
				SpawnEnemy(SatyrSwordsman, GetOffset(5f, 2f));
				SpawnEnemy(Dryad, GetOffset(0f, 6f));
				break;
			case 2:
				PostLog("<color=#32CD32><b>[Stolen Lands Legacy Cup - Round 2/3]</b></color> <b>The Primal Forest Awakens: Bloom Treants & Wild Hunt Scouts!</b>");
				SpawnEnemy(PrimalTreant, GetOffset(0f, 7f));
				SpawnEnemy(WildHuntScout, GetOffset(-5f, 0f));
				SpawnEnemy(WildHuntArcher, GetOffset(5f, 0f));
				break;
			case 3:
				PostLog("<color=#32CD32><b>[Stolen Lands Legacy Cup - FINAL]</b></color> <b>THE WILD HUNT MONARCH & THE SHADOW ANKOU STEP FORTH!</b>");
				SpawnEnemy(WildHuntMonarch, GetOffset(0f, 7f));
				SpawnEnemy(Ankou, GetOffset(-5f, 2f));
				SpawnEnemy(HamadryadQueen, GetOffset(5f, 2f));
				break;
			default:
				FinishArena();
				break;
			}
		}

		private Vector3 GetOffset(float x, float z)
		{
			return new Vector3(_arenaCenter.x + x, _arenaCenter.y, _arenaCenter.z + z);
		}

		private void SpawnEnemy(BlueprintUnit unitBp, Vector3 position)
		{
			if (unitBp == null || Game.Instance?.LoadedAreaState?.MainState == null)
			{
				return;
			}
			try
			{
				if (NavMesh.SamplePosition(position, out var hit, 15f, -1))
				{
					position = hit.position;
				}
				UnitEntityData unitEntityData = Game.Instance.EntityCreator.SpawnUnit(unitBp, position, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
				if (unitEntityData != null)
				{
					BlueprintFaction blueprintFaction = BlueprintRoot.Instance?.Cheats?.Enemy?.Faction;
					if (blueprintFaction != null)
					{
						unitEntityData.SwitchFactions(blueprintFaction, resetAttackFactions: true);
					}
					UnitEntityData unitEntityData2 = BlueprintSafetyExtensions.SafeGetMainCharacter();
					if (unitEntityData2 != null)
					{
						unitEntityData.CombatState?.Engage(unitEntityData2);
					}
					_activeEnemyIds.Add(unitEntityData.UniqueId);
					_spawnedEnemyIds.Add(unitEntityData.UniqueId);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error spawning coliseum enemy: " + ex);
			}
		}

		public void OnUnitDie(UnitEntityData unit)
		{
			if (IsActive && !(unit == null) && _activeEnemyIds.Remove(unit.UniqueId) && _activeEnemyIds.Count == 0)
			{
				int maxWaves = GetMaxWaves(CurrentArena);
				if (CurrentWave >= maxWaves)
				{
					FinishArena();
					return;
				}
				PostLog($"<color=#32CD32><b>[Planar Coliseum]</b></color> <b>Wave {CurrentWave} Cleared!</b> The arena sand trembles as greater challenges prepare to enter...");
				SpawnNextWave();
			}
		}

		private void FinishArena()
		{
			IsActive = false;
			_activeEnemyIds.Clear();
			PostLog("<color=#FFD700><b>=======================================================</b></color>");
			PostLog($"<color=#FFD700><b>[Planar Coliseum] VICTORY! All waves of {CurrentArena} defeated!</b></color>");
			if (LanternKingAvatar != null)
			{
				try
				{
					UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
					if (unitEntityData != null && Game.Instance?.LoadedAreaState?.MainState != null)
					{
						Vector3 vector = unitEntityData.Position + unitEntityData.OrientationDirection * 5f;
						if (NavMesh.SamplePosition(vector, out var hit, 10f, -1))
						{
							vector = hit.position;
						}
						if (!string.IsNullOrEmpty(_activeCameoId))
						{
							(Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>()?.FirstOrDefault((UnitEntityData x) => x.UniqueId == _activeCameoId))?.FadeOutViewAndDestroy();
							_activeCameoId = null;
						}
						UnitEntityData unitEntityData2 = Game.Instance.EntityCreator.SpawnUnit(LanternKingAvatar, vector, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
						if (unitEntityData2 != null)
						{
							unitEntityData2.SwitchFactions(NeutralsFaction);
							_activeCameoId = unitEntityData2.UniqueId;
						}
					}
				}
				catch (Exception ex)
				{
					Main.IsekaiContext.Logger.LogError("Error manifesting Lantern King cameo: " + ex);
				}
			}
			PostLog("<color=#FF8C00><b>[The Lantern King]</b></color>: <i>\"Tell me, otherworlder... does this crusade feel at all familiar to you? BRAVO! The roar of the coliseum reaches across the cosmos! The constellations are singing your praises!\"</i>");
			PostConstellationVictoryReactions(CurrentArena);
			int num = 8000;
			int num2 = 3000;
			int num3 = 200;
			switch (CurrentArena)
			{
			case ArenaType.BronzeCup:
			case ArenaType.Act1FaeGrove:
				num = 8000;
				num2 = 3000;
				num3 = 200;
				break;
			case ArenaType.SilverCup:
			case ArenaType.Act2LostLegions:
				num = 20000;
				num2 = 8000;
				num3 = 400;
				break;
			case ArenaType.GoldCup:
			case ArenaType.Act3MidnightColosseum:
				num = 45000;
				num2 = 15000;
				num3 = 750;
				break;
			case ArenaType.PlatinumCup:
			case ArenaType.Act4VoidRift:
				num = 90000;
				num2 = 30000;
				num3 = 1200;
				break;
			case ArenaType.DiamondCup:
			case ArenaType.Act5MultiverseThreshold:
				num = 150000;
				num2 = 50000;
				num3 = 2000;
				break;
			case ArenaType.AstralCup:
				num = 250000;
				num2 = 80000;
				num3 = 3000;
				break;
			case ArenaType.GenesisCup:
				num = 400000;
				num2 = 120000;
				num3 = 5000;
				break;
			case ArenaType.TheCosmicChallenge:
				num = 1000000;
				num2 = 250000;
				num3 = 10000;
				break;
			case ArenaType.StolenLandsLegacyCup:
				num = 60000;
				num2 = 25000;
				num3 = 1500;
				break;
			}
			Player player = Game.Instance?.Player;
			if (player != null)
			{
				player.GainPartyExperience(num, ExperienceGainStatistic.GainType.Mob);
				player.GainMoney(num2);
			}
			DivineTokens.AddCoins(num3, $"Planar Coliseum Victory ({CurrentArena})");
			TimelineManager.Data.ArenaCompleted = true;
			string item = $"DimensionalArena_{CurrentArena}";
			if (!TimelineManager.Data.CompletedSideQuests.Contains(item))
			{
				TimelineManager.Data.CompletedSideQuests.Add(item);
			}
			TimelineManager.Save();
			PostLog($"<color=#FFD700><b>[Tournament Spoils]</b></color> Awarded <b>+{num} XP</b>, <b>+{num2} Gold</b>, and <b>+{num3} Cosmic Coins</b>!");
			PostLog("<color=#FFD700><b>=======================================================</b></color>");
			ClearSpawnedUnits(clearCameo: false);
		}

		private void PostConstellationVictoryReactions(ArenaType cup)
		{
			switch (cup)
			{
			case ArenaType.BronzeCup:
			case ArenaType.Act1FaeGrove:
				PostLog("<color=#7CFC00><b>[Constellation - Desna]</b></color>: <i>\"A dazzling debut! You danced through the First World's briars with laughter in your heart! The Song of the Spheres echoes your triumph!\"</i>");
				PostLog("<color=#FFD700><b>[Constellation - Cayden Cailean]</b></color>: <i>\"Bwahaha! Now that's what I call a tavern cellar rumble! The next round of celestial ale is on me!\"</i>");
				break;
			case ArenaType.SilverCup:
			case ArenaType.Act2LostLegions:
				PostLog("<color=#708090><b>[Constellation - Pharasma]</b></color>: <i>\"The restless dead have been returned to eternal rest. The scales of fate remain balanced by your hand.\"</i>");
				PostLog("<color=#FF4500><b>[Constellation - Gorum]</b></color>: <i>\"IRON AND BLOOD! You shattered their bones and crushed their crypts! A true warrior's glory!\"</i>");
				break;
			case ArenaType.GoldCup:
			case ArenaType.Act3MidnightColosseum:
				PostLog("<color=#FFD700><b>[Constellation - Iomedae]</b></color>: <i>\"Valiant Commander! The demonic terrors of the Abyss crumble before your righteous resolve. Drezen stands unbroken!\"</i>");
				PostLog("<color=#DC143C><b>[Constellation - Asmodeus]</b></color>: <i>\"Ruthless discipline and total domination. A textbook demonstration of superior order.\"</i>");
				break;
			case ArenaType.StolenLandsLegacyCup:
			{
				string rulerName = KingmakerCrossSaveManager.GetRulerName();
				PostLog("<color=#32CD32><b>[Constellation - Sylvan Court]</b></color>: <i>\"The memory of " + rulerName + "'s kingdom burns eternal! The First World salutes the inheritor of the Stolen Lands!\"</i>");
				break;
			}
			case ArenaType.PlatinumCup:
			case ArenaType.Act4VoidRift:
				PostLog("<color=#FF69B4><b>[Constellation - Calistria]</b></color>: <i>\"Mmm, delicious agony! Watching those arrogant abyssal blademasters grovel in the dirt was sheer ecstasy, darling.\"</i>");
				PostLog("<color=#00BFFF><b>[Constellation - Nethys]</b></color>: <i>\"Fascinating... the magic tearing through planar space resonates with pure arcane brilliance!\"</i>");
				break;
			case ArenaType.DiamondCup:
			case ArenaType.Act5MultiverseThreshold:
				PostLog("<color=#FFD700><b>[Constellation - Iomedae]</b></color>: <i>\"The threshold of mortality has been breached! You stand as a beacon of transcendent power across the multiverse!\"</i>");
				PostLog("<color=#FF4500><b>[Constellation - Gorum]</b></color>: <i>\"APEX GLORY! Even the heavens shake when your war-cry sounds!\"</i>");
				break;
			case ArenaType.AstralCup:
				PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The cosmic spheres turn in reverence. Beyond the veil of time, your feats are etched into eternity.\"</i>");
				break;
			case ArenaType.GenesisCup:
				PostLog("<color=#9400D3><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"Primordial genesis acknowledged. The fabric of reality bends to your awakened sovereignty.\"</i>");
				break;
			case ArenaType.TheCosmicChallenge:
				PostLog("<color=#FFD700><b>[THE CONSTELLATIONS ASSEMBLE]</b></color>: <i>\"BEHOLD THE TRANSCENDENT VICTOR! Desna, Gorum, Iomedae, Calistria, Nethys, Asmodeus, and Yog-Sothoth unite their divine voices in celestial chorus! A mortal who has conquered the pinnacle of all creation!\"</i>");
				break;
			}
		}

		private static void PostLog(string message)
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(message);
			});
		}
	}
}
