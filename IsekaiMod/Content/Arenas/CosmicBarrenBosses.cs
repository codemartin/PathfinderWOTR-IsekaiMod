using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers;
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

namespace IsekaiMod.Content.Arenas;

public class CosmicBarrenBosses : IUnitHandler, ISubscriber, IGlobalSubscriber, IUnitSpawnHandler, IAreaHandler
{
	internal enum SummonRefusal
	{
		None,
		GlobalMap,
		InCombat,
		ArmyBattle,
		Dialogue,
		Cutscene,
		Coliseum,
		Prologue,
		TooEarly
	}

	private static bool _initialized = false;

	private static CosmicBarrenBosses _instance;

	public static BlueprintFeature CosmicBarrenEncountersFeature;

	internal const string ModPaysExperienceKey = "ModPaysExperience";

	internal const string DefeatStorageName = "IsekaiMod.BarrenAnomalies";

	internal const string BossTypeMarkerKey = "BossType";

	private static readonly List<UnitEntityData> _spawnedThisVisit = new List<UnitEntityData>();

	public static CosmicBarrenBosses Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CosmicBarrenBosses();
			}
			return _instance;
		}
	}

	public static void CreateBlueprints()
	{
		Sprite Icon_Boss = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		CreateSummonAbility("SummonBarrenBossAct1Ability", "Barren Anomaly: The Outer World Stray (Act 1)", "Summons a satyr archer (CR 4) that strayed out of the First World into Kenabres. Defeating it the first time in a playthrough awards 300 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins.", BarrenBossType.Act1OuterWorldStray);
		CreateSummonAbility("SummonBarrenBossAct2Ability", "Barren Anomaly: Chronos Exile (Act 2)", "Summons an ash giant (CR 11) cast adrift in time across the Worldwound. Requires Chapter 2. Defeating it the first time in a playthrough awards 600 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins.", BarrenBossType.Act2ChronosExile);
		CreateSummonAbility("SummonBarrenBossAct3Ability", "Barren Anomaly: The Transdimensional Behemoth (Act 3)", "Summons a towering glabrezu (CR 14), a four-armed demon of treachery dragged through a planar fissure. Requires Chapter 3. Defeating it the first time in a playthrough awards 1,000 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins.", BarrenBossType.Act3TransdimensionalBehemoth);
		CreateSummonAbility("SummonBarrenBossAct4Ability", "Barren Anomaly: Abyssal Void Vavakia (Act 4)", "Summons an advanced vavakia demon (CR 19) warped by the Midnight Fane and the Abyss. Requires Chapter 4. Defeating it the first time in a playthrough awards 1,500 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins.", BarrenBossType.Act4AbyssalVoidDragon);
		CreateSummonAbility("SummonBarrenBossAct5Ability", "Barren Anomaly: Cosmic Arbiter of Ruin (Act 5)", "Summons a balor (CR 20), a general of the Abyss sent to pass judgment upon your transmigrated soul. Requires Chapter 5. Defeating it the first time in a playthrough awards 2,500 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins.", BarrenBossType.Act5CosmicArbiterOfRuin);
		CosmicBarrenEncountersFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicBarrenEncountersFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Barren Encounters");
			bp.SetDescription(Main.IsekaiContext, "Five challenging optional cosmic anomalies, one per chapter of the crusade, wait in places of their own: a Barren Anomaly on the world map for each, and the Otherworldly Emissary in the Midnight Isles and at the Threshold. Each of them pays its Cosmic Coin bounty once per playthrough; a rematch is always available and pays a tenth of the normal experience and no coins.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Boss;
			bp.Ranks = 1;
			bp.IsClassFeature = true;
		});
		BlueprintAbility CreateSummonAbility(string name, string displayName, string description, BarrenBossType bossType)
		{
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintUnitFact)bp).m_Icon = Icon_Boss;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpawnBarrenBoss a)
					{
						a.BossType = bossType;
					});
				});
			});
		}
	}

	public static void Add()
	{
		Init();
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
	}

	public void HandleUnitSpawned(UnitEntityData unit)
	{
	}

	internal static int RewardForBoss(BarrenBossType type)
	{
		return type switch
		{
			BarrenBossType.Act1OuterWorldStray => 300, 
			BarrenBossType.Act2ChronosExile => 600, 
			BarrenBossType.Act3TransdimensionalBehemoth => 1000, 
			BarrenBossType.Act4AbyssalVoidDragon => 1500, 
			BarrenBossType.Act5CosmicArbiterOfRuin => 2500, 
			_ => 300, 
		};
	}

	internal static int MinimumChapterForBoss(BarrenBossType type)
	{
		return type switch
		{
			BarrenBossType.Act1OuterWorldStray => 1, 
			BarrenBossType.Act2ChronosExile => 2, 
			BarrenBossType.Act3TransdimensionalBehemoth => 3, 
			BarrenBossType.Act4AbyssalVoidDragon => 4, 
			BarrenBossType.Act5CosmicArbiterOfRuin => 5, 
			_ => 1, 
		};
	}

	internal static BarrenBossType BossTypeForAct(int act)
	{
		return act switch
		{
			2 => BarrenBossType.Act2ChronosExile, 
			3 => BarrenBossType.Act3TransdimensionalBehemoth, 
			4 => BarrenBossType.Act4AbyssalVoidDragon, 
			5 => BarrenBossType.Act5CosmicArbiterOfRuin, 
			_ => BarrenBossType.Act1OuterWorldStray, 
		};
	}

	internal static int ActForBoss(BarrenBossType type)
	{
		return MinimumChapterForBoss(type);
	}

	internal static int DecideReward(int baseReward, bool alreadyDefeated)
	{
		if (baseReward <= 0)
		{
			return 0;
		}
		if (!alreadyDefeated)
		{
			return baseReward;
		}
		return 0;
	}

	internal static string UnitGuidForBoss(BarrenBossType type)
	{
		return type switch
		{
			BarrenBossType.Act2ChronosExile => "4ddefbd20c9ab204c902de574d83fdb8", 
			BarrenBossType.Act3TransdimensionalBehemoth => "d1822e208ff314846a5cd1443a9db45b", 
			BarrenBossType.Act4AbyssalVoidDragon => "370039781f3a4004ab4e3ac6c7032573", 
			BarrenBossType.Act5CosmicArbiterOfRuin => "eb0b4e6a01f6b30449f40cd03feb3c77", 
			_ => "6114adae73a1338498412b3c63c946f8", 
		};
	}

	internal static int ExperienceForDefeat(int fullExperience, bool firstDefeat)
	{
		return ColiseumRewards.For(fullExperience, 0, 0, firstDefeat).Experience;
	}

	internal static SummonRefusal RefusalFor(bool globalMap, bool inCombat, bool armyBattle, bool dialogue, bool cutscene, bool coliseum, int chapter, int minimumChapter)
	{
		if (globalMap)
		{
			return SummonRefusal.GlobalMap;
		}
		if (armyBattle)
		{
			return SummonRefusal.ArmyBattle;
		}
		if (inCombat)
		{
			return SummonRefusal.InCombat;
		}
		if (cutscene)
		{
			return SummonRefusal.Cutscene;
		}
		if (dialogue)
		{
			return SummonRefusal.Dialogue;
		}
		if (coliseum)
		{
			return SummonRefusal.Coliseum;
		}
		if (chapter < 1)
		{
			return SummonRefusal.Prologue;
		}
		if (chapter < minimumChapter)
		{
			return SummonRefusal.TooEarly;
		}
		return SummonRefusal.None;
	}

	internal static string SpawnEntry(string saveId, string uniqueId, BarrenBossType type)
	{
		return saveId + "|" + uniqueId + "|" + type;
	}

	internal static string DefeatEntry(string saveId, BarrenBossType type)
	{
		return saveId + "|" + type;
	}

	internal static string BossTypeForUnit(List<string> activeEntries, string saveId, string uniqueId)
	{
		if (activeEntries == null || string.IsNullOrEmpty(uniqueId))
		{
			return null;
		}
		string text = saveId + "|" + uniqueId + "|";
		for (int i = 0; i < activeEntries.Count; i++)
		{
			string text2 = activeEntries[i];
			if (text2 != null && text2.StartsWith(text, StringComparison.Ordinal))
			{
				return text2.Substring(text.Length);
			}
		}
		return null;
	}

	internal static string DefeatedKey(BarrenBossType type)
	{
		return "Defeated." + type;
	}

	internal static int SettleDefeat(IDictionary<string, string> saveRecord, BarrenBossType type, bool paidByOlderVersion)
	{
		if (saveRecord == null)
		{
			return 0;
		}
		string key = DefeatedKey(type);
		bool alreadyDefeated = paidByOlderVersion || SaveScopedStorage.GetFlag(saveRecord, key);
		SaveScopedStorage.SetFlag(saveRecord, key, value: true);
		return DecideReward(RewardForBoss(type), alreadyDefeated);
	}

	private static bool TryParseBoss(string token, out BarrenBossType type)
	{
		type = BarrenBossType.Act1OuterWorldStray;
		if (string.IsNullOrEmpty(token))
		{
			return false;
		}
		foreach (BarrenBossType value in Enum.GetValues(typeof(BarrenBossType)))
		{
			if (string.Equals(value.ToString(), token, StringComparison.Ordinal))
			{
				type = value;
				return true;
			}
		}
		return false;
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

	private static bool PaidByOlderVersion(string saveId, BarrenBossType type)
	{
		return TimelineManager.Data.DefeatedBarrenBosses?.Contains(DefeatEntry(saveId, type)) ?? false;
	}

	private static bool IsDefeatedInThisSave(BarrenBossType type)
	{
		return SaveScopedStorage.GetFlag(SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.BarrenAnomalies"), DefeatedKey(type));
	}

	private static void Log(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, message, null, ConstellationCategory.Combat, "Cosmic Anomaly");
	}

	private static void Quiet(string message)
	{
		ConstellationChatManager.PostCombatLog(message);
	}

	private static string RefusalMessage(SummonRefusal refusal, int minimumChapter)
	{
		return refusal switch
		{
			SummonRefusal.GlobalMap => "<color=#7A1010><b>[Cosmic Anomaly]</b></color> Cosmic entities cannot manifest while traversing the Worldwound overland. Enter a local area first!", 
			SummonRefusal.InCombat => "<color=#7A1010><b>[Cosmic Anomaly]</b></color> You cannot summon a cosmic anomaly while already engaged in battle!", 
			SummonRefusal.ArmyBattle => "<color=#7A1010><b>[Cosmic Anomaly]</b></color> The anomaly will not answer while the crusade's armies are in the field.", 
			SummonRefusal.Dialogue => "<color=#7A1010><b>[Cosmic Anomaly]</b></color> The anomaly will not answer in the middle of a conversation.", 
			SummonRefusal.Cutscene => "<color=#7A1010><b>[Cosmic Anomaly]</b></color> The anomaly will not answer while a scene is playing.", 
			SummonRefusal.Coliseum => "<color=#7A1010><b>[Cosmic Anomaly]</b></color> The Planar Coliseum runs its own trials; summon the anomaly out in the world.", 
			SummonRefusal.Prologue => "<color=#7A1010><b>[Cosmic Anomaly]</b></color> The anomalies do not reach Kenabres before the crusade begins. They answer from Chapter 1 onward.", 
			SummonRefusal.TooEarly => "<color=#7A1010><b>[Cosmic Anomaly]</b></color> " + ConstellationText.LocalizedFormat("Constellation.Anomaly", "This anomaly does not reach into the current era. It answers from Chapter {0} onward.", minimumChapter), 
			_ => null, 
		};
	}

	public static void SpawnBoss(BarrenBossType bossType)
	{
		try
		{
			Game instance = Game.Instance;
			UnitEntityData unitEntityData = ((instance?.Player != null) ? instance.Player.MainCharacter.Value : null);
			if (unitEntityData == null)
			{
				return;
			}
			int minimumChapter = MinimumChapterForBoss(bossType);
			SummonRefusal summonRefusal = RefusalFor(instance.CurrentlyLoadedArea?.IsGlobalMap ?? false, instance.Player.IsInCombat, CampaignEncounterGuard.IsArmyBattle(null), CampaignEncounterGuard.IsDialogue(), CampaignEncounterGuard.IsCutscene(), CampaignEncounterGuard.IsColiseum(null), instance.Player.Chapter, minimumChapter);
			if (summonRefusal != SummonRefusal.None)
			{
				Quiet(RefusalMessage(summonRefusal, minimumChapter));
				return;
			}
			UnitEntityData unitEntityData2 = SpawnBossAt(bossType, unitEntityData.Position + unitEntityData.OrientationDirection * 8f);
			if (unitEntityData2 != null)
			{
				_spawnedThisVisit.Add(unitEntityData2);
				unitEntityData2.CombatState?.Engage(unitEntityData);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in SpawnBoss: " + ex);
		}
	}

	internal static UnitEntityData SpawnBossAt(BarrenBossType bossType, Vector3 position)
	{
		Game instance = Game.Instance;
		bool flag = IsDefeatedInThisSave(bossType) || PaidByOlderVersion(CurrentSaveId(), bossType);
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>(UnitGuidForBoss(bossType));
		if (blueprint == null || instance?.LoadedAreaState?.MainState == null)
		{
			return null;
		}
		UnitEntityData unitEntityData = instance.Player?.MainCharacter.Value;
		position = IsekaiNpcUnit.PlaceWithinReach(position, unitEntityData?.OrientationDirection ?? Vector3.forward, unitEntityData?.Position ?? position);
		UnitEntityData unitEntityData2 = instance.EntityCreator.SpawnUnit(blueprint, position, Quaternion.identity, instance.LoadedAreaState.MainState);
		if (unitEntityData2 == null)
		{
			return null;
		}
		unitEntityData2.SwitchFactions(BlueprintRoot.Instance.Cheats.Enemy.Faction, resetAttackFactions: true);
		unitEntityData2.GiveExperienceOnDeath = false;
		SaveScopedStorage.MarkSpawned(unitEntityData2, "BarrenBoss");
		Dictionary<string, string> dictionary = SaveScopedStorage.SpawnMarker(unitEntityData2);
		if (dictionary != null)
		{
			dictionary["BossType"] = bossType.ToString();
			SaveScopedStorage.SetFlag(dictionary, "ModPaysExperience", value: true);
		}
		DimensionalReinforcements.RegisterExternalSpawnedMob(unitEntityData2.UniqueId);
		Log(ConstellationText.LocalizedFormat("Constellation.Anomaly", "<color=#7A1010><b>[Cosmic Anomaly]</b></color> <b>{0}</b> has emerged from the planar fissure!", blueprint.CharacterName));
		if (flag)
		{
			Quiet(ConstellationText.Localized("Constellation.Anomaly", "<color=#3F3F3F><i>[Cosmic Anomaly] The constellations have already paid out this bounty in this cycle. This rematch pays a tenth of the experience and no coins.</i></color>"));
		}
		Main.IsekaiContext.Logger.Log($"[Cosmic Barren Bosses] Spawned {bossType} (bounty already paid: {flag}).");
		return unitEntityData2;
	}

	internal static BarrenBossType? BossTypeFromMarker(IDictionary<string, string> marker)
	{
		if (SaveScopedStorage.Get(marker, "Kind") != "BarrenBoss")
		{
			return null;
		}
		if (!TryParseBoss(SaveScopedStorage.Get(marker, "BossType"), out var type))
		{
			return null;
		}
		return type;
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
			Main.IsekaiContext.Logger.LogError("Error removing a barren anomaly: " + ex);
		}
	}

	public void OnAreaBeginUnloading()
	{
		foreach (UnitEntityData item in _spawnedThisVisit)
		{
			Despawn(item);
		}
		_spawnedThisVisit.Clear();
	}

	public void OnAreaDidLoad()
	{
		_spawnedThisVisit.Clear();
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
				if (item != null && SaveScopedStorage.SpawnKindOf(item) == "BarrenBoss")
				{
					Despawn(item);
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error clearing barren anomalies left in the area: " + ex);
		}
	}

	private static int FullExperienceFor(UnitEntityData unit)
	{
		int num = 0;
		BlueprintComponent[] array = unit?.Blueprint?.ComponentsArray;
		if (array == null)
		{
			return 0;
		}
		foreach (Experience item in array.OfType<Experience>())
		{
			if (!item.PlayerGainsNoExp)
			{
				num += Mathf.RoundToInt((float)ExperienceHelper.GetXp(item.Encounter, item.CR, item.Modifier, item.Count) * (1f + unit.BonusExperience));
			}
		}
		return num;
	}

	public void OnUnitDie(UnitEntityData unit)
	{
		if (unit == null)
		{
			return;
		}
		try
		{
			string saveId = CurrentSaveId();
			Dictionary<string, string> storage = SaveScopedStorage.SpawnMarker(unit);
			string text = ((SaveScopedStorage.Get(storage, "Kind") == "BarrenBoss") ? SaveScopedStorage.Get(storage, "BossType") : null);
			List<string> activeBarrenBosses = TimelineManager.Data.ActiveBarrenBosses;
			string text2 = BossTypeForUnit(activeBarrenBosses, saveId, unit.UniqueId);
			if (text == null)
			{
				text = text2;
			}
			if (text == null || !TryParseBoss(text, out var type))
			{
				return;
			}
			_spawnedThisVisit.Remove(unit);
			if (text2 != null && activeBarrenBosses.Remove(SpawnEntry(saveId, unit.UniqueId, type)))
			{
				TimelineManager.Save();
			}
			int num = SettleDefeat(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.BarrenAnomalies"), type, PaidByOlderVersion(saveId, type));
			int num2 = 0;
			if (SaveScopedStorage.GetFlag(storage, "ModPaysExperience"))
			{
				SaveScopedStorage.SetFlag(storage, "ModPaysExperience", value: false);
				num2 = ExperienceForDefeat(FullExperienceFor(unit), num > 0);
				if (num2 > 0)
				{
					GameHelper.GainExperience(num2, null, ExperienceGainStatistic.GainType.Mob);
				}
			}
			if (num > 0)
			{
				DivineTokens.AddClaimedCoins(num, ConstellationText.Localized("CosmicCoins.Sponsor", "Barren Anomaly Victory"));
				Log(ConstellationText.LocalizedFormat("Constellation.Anomaly", "<color=#5B2A86><b>[Cosmic Anomaly Defeated]</b></color> You triumphed over <b>{0}</b> and earned <b>+{1} Cosmic Coins</b>!", unit.CharacterName, num));
			}
			else
			{
				Log(ConstellationText.LocalizedFormat("Constellation.Anomaly", "<color=#5B2A86><b>[Cosmic Anomaly Defeated]</b></color> You triumphed over <b>{0}</b> once more. The bounty was already claimed in this cycle; the rematch paid <b>{1} experience</b>.", unit.CharacterName, num2));
			}
			Main.IsekaiContext.Logger.Log($"[Cosmic Barren Bosses] {type} defeated! Awarded {num} Cosmic Coins and {num2} experience.");
			IsekaiBarrenAnomaliesQuest.Refresh();
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in CosmicBarrenBosses.OnUnitDie: " + ex);
		}
	}
}
