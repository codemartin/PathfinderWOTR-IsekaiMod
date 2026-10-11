using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Kingmaker.Blueprints.Items.Ecnchantments;
using IsekaiMod.Components;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62Rewards
{
	internal const string SilverDragonAegisBuff = "SilverDragonAegisBuff";

	internal const string StitchWardBuff = "StitchWardBuff";

	internal const string MindOfTheOtherworlderBuff = "MindOfTheOtherworlderBuff";

	internal const string EmberSparkBuff = "EmberSparkBuff";

	internal const string CrusaderLiberatorBuff = "CrusaderLiberatorBuff";

	internal const string TavernDefianceMoraleBuff = "TavernDefianceMoraleBuff";

	internal const string OtherworldEspressoBuff = "OtherworldEspressoBuff";

	internal const string LarielsVowFeature = "LarielsVowFeature";

	internal const string ResonantMythicSparkFeature = "ResonantMythicSparkFeature";

	internal const string SwordOfValorResonantFeature = "SwordOfValorResonantFeature";

	internal const string ItemSilverDragonShard = "ItemSilverDragonShard";

	internal const string ItemTearOfSilverDragon = "ItemTearOfSilverDragon";

	internal const string ItemLarielFeather = "ItemLarielFeather";

	internal const string ItemThieflingCharm = "ItemThieflingCharm";

	internal const string ItemRoyalMendevianSignet = "ItemRoyalMendevianSignet";

	internal const string ItemHellknightOfficerRegalia = "ItemHellknightOfficerRegalia";

	internal const string ItemVescavorPheromoneFlask = "ItemVescavorPheromoneFlask";

	internal const string ItemGwermAncestralRapier = "ItemGwermAncestralRapier";

	internal const string ItemRingOfTheAncientChronicler = "ItemRingOfTheAncientChronicler";

	internal const string ItemThieflingMasterLockpicks = "ItemThieflingMasterLockpicks";

	internal const string ItemNenioInscribedJournal = "ItemNenioInscribedJournal";

	internal const string ItemOtherworldEspresso = "ItemOtherworldEspresso";

	internal const string ItemTalismanOfTheCosmicTourist = "ItemTalismanOfTheCosmicTourist";

	internal const string ItemAmuletOfOtherworldEquilibrium = "ItemAmuletOfOtherworldEquilibrium";

	internal const string ItemRingOfTheGrandAuditor = "ItemRingOfTheGrandAuditor";

	internal const string ItemMartialGodFocusBand = "ItemMartialGodFocusBand";

	internal const string ItemSovereignsImperialSignet = "ItemSovereignsImperialSignet";

	internal const string ItemCodexOfReincarnation = "ItemCodexOfReincarnation";

	internal const string TearOfSilverDragonAbility = "TearOfSilverDragonAbility";

	internal const string TearOfSilverDragonEnduranceBuff = "TearOfSilverDragonEnduranceBuff";

	internal const string SilverDragonShardFeature = "ItemSilverDragonShardFeature";

	internal const string LarielFeatherFeature = "ItemLarielFeatherFeature";

	internal const string RoyalMendevianSignetFeature = "ItemRoyalMendevianSignetFeature";

	internal const string RoyalMendevianSignetAreaBuff = "RoyalMendevianSignetAuraAreaBuff";

	internal const string RoyalMendevianSignetArea = "RoyalMendevianSignetAuraArea";

	internal const string RoyalMendevianSignetEffectBuff = "RoyalMendevianSignetAuraEffectBuff";

	internal const string VescavorPheromoneAbility = "VescavorPheromoneFlaskAbility";

	internal const string VescavorPheromoneBuff = "VescavorPheromoneConfoundedBuff";

	internal const string AncientChroniclerFeature = "ItemRingOfTheAncientChroniclerFeature";

	internal const string MasterLockpicksFeature = "ItemThieflingMasterLockpicksFeature";

	internal const string OtherworldEspressoAbility = "OtherworldEspressoAbility";

	internal const string TalismanOfTheCosmicTouristFeature = "ItemTalismanOfTheCosmicTouristFeature";

	internal const string AmuletOfOtherworldEquilibriumFeature = "ItemAmuletOfOtherworldEquilibriumFeature";

	internal const string RingOfTheGrandAuditorFeature = "ItemRingOfTheGrandAuditorFeature";

	internal const string MartialGodFocusBandFeature = "ItemMartialGodFocusBandFeature";

	internal const string SovereignsImperialSignetFeature = "ItemSovereignsImperialSignetFeature";

	internal static readonly IReadOnlyDictionary<string, string> SponsorRemap = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "The Fading Dragon", "The Inheritor" },
		{ "The Dawnflower", "The Everbloom" },
		{ "The World Sovereign", "The Inheritor" },
		{ "The Grand Arbiter", "The All-Seeing Eye" },
		{ "The Hermit", "The All-Seeing Eye" },
		{ "The Dark Prince", "The Prince of Darkness" },
		{ "The Iron Warden", "Our Lord in Iron" }
	};

	internal static readonly IReadOnlyDictionary<string, string> GateRemap = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "MartialGodProficiencies", "PhantomBladeProficiencies" },
		{ "MasterAtArmsProficiencies", "PhantomBladeProficiencies" },
		{ "VillainProficiencies", "OverlordProficiencies" }
	};

	internal const string MythicAngel = "a5a9fe8f663d701488bd1db8ea40484e";

	internal const string MythicAeon = "15a85e67b7d69554cab9ed5830d0268e";

	internal const string MythicAzata = "9a3b2c63afa79744cbca46bea0da9a16";

	internal const string MythicDemon = "8e19495ea576a8641964102d177e34b7";

	internal const string MythicDevil = "211f49705f478b3468db6daa802452a2";

	internal const string MythicGoldDragon = "daf1235b6217787499c14e4e32142523";

	internal const string MythicLegend = "3d420403f3e7340499931324640efe96";

	internal const string MythicLich = "5d501618a28bdc24c80007a5c937dcb7";

	internal const string MythicSwarm = "5295b8e13c2303f4c88bdb3d7760a757";

	internal const string MythicTrickster = "8df873a8c6e48294abdb78c45834aa0a";

	internal static readonly IReadOnlyDictionary<string, string> SixTwoMythicClassIds = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "Angel", "a5a9fe8f663d701488bd1db8ea40484e" },
		{ "Aeon", "15a85e67b7d69554cab9ed5830d0268e" },
		{ "Azata", "9a3b2c63afa79744cbca46bea0da9a16" },
		{ "Demon", "8e19495ea576a8641964102d177e34b7" },
		{ "Devil", "211f49705f478b3468db6daa802452a2" },
		{ "GoldDragon", "daf1235b6217787499c14e4e32142523" },
		{ "Legend", "3d420403f3e7340499931324640efe96" },
		{ "Lich", "5d501618a28bdc24c80007a5c937dcb7" },
		{ "Swarm", "5295b8e13c2303f4c88bdb3d7760a757" },
		{ "Trickster", "8df873a8c6e48294abdb78c45834aa0a" }
	};

	private static readonly Dictionary<string, BlueprintScriptableObject> s_Created = new Dictionary<string, BlueprintScriptableObject>(StringComparer.Ordinal);

	private const string OutsiderType = "9054d3988d491d944ac144e27b6bc318";

	private const string SubtypeDemon = "dc960a234d365cb4f905bdc5937e623a";

	private const string VerminType = "09478937695300944a179530664e42ec";

	private const string SwarmDiminutive = "2e3e840ab458ce04c92064489f87ecc2";

	private const string SwarmTiny = "5a04735fd0e952142bfc8ecf995e2361";

	private const string HasteBuff = "03464790f40c3c24aa684b57155f3280";

	private const string ColdIronRapier = "ec731c55e657cf0408fd89c648ccc536";

	private const string Enhancement1 = "d42fc23b92c640846ac137dc26e000d4";

	private const string Keen = "102a9c8c9b7a75e4fb5844e79deaf4c0";

	private static readonly StatType[] Saves = new StatType[3]
	{
		StatType.SaveFortitude,
		StatType.SaveReflex,
		StatType.SaveWill
	};

	private static readonly StatType[] Skills = new StatType[11]
	{
		StatType.SkillMobility,
		StatType.SkillAthletics,
		StatType.SkillPerception,
		StatType.SkillThievery,
		StatType.SkillPersuasion,
		StatType.SkillKnowledgeArcana,
		StatType.SkillKnowledgeWorld,
		StatType.SkillLoreNature,
		StatType.SkillLoreReligion,
		StatType.SkillStealth,
		StatType.SkillUseMagicDevice
	};

	private static Sprite s_Coin;

	internal static string Seat(string sixTwoSponsor)
	{
		if (string.IsNullOrEmpty(sixTwoSponsor))
		{
			return sixTwoSponsor;
		}
		if (!SponsorRemap.TryGetValue(sixTwoSponsor, out var value))
		{
			return sixTwoSponsor;
		}
		return value;
	}

	internal static string GateName(string sixTwoProficiency)
	{
		if (string.IsNullOrEmpty(sixTwoProficiency) || sixTwoProficiency == "IsekaiProficiencies")
		{
			return null;
		}
		if (!GateRemap.TryGetValue(sixTwoProficiency, out var value))
		{
			return sixTwoProficiency;
		}
		return value;
	}

	internal static Condition Gate(string sixTwoProficiency)
	{
		string text = GateName(sixTwoProficiency);
		if (text == null)
		{
			return ExtensionMethods.PlotArmorCondition();
		}
		BlueprintFeature blueprintFeature = GateFeature(text);
		if (blueprintFeature == null)
		{
			return null;
		}
		return new HasFact
		{
			Unit = new PlayerCharacter(),
			m_Fact = blueprintFeature.ToReference<BlueprintUnitFactReference>()
		};
	}

	private static BlueprintFeature GateFeature(string name)
	{
		switch (name)
		{
		case "PhantomBladeProficiencies":
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PhantomBladeProficiencies");
		case "DevourerProficiencies":
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies");
		case "OverlordProficiencies":
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies");
		case "HeroProficiencies":
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroProficiencies");
		case "MastermindProficiencies":
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindProficiencies");
		case "GodEmperorProficiencies":
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies");
		case "ShadowMonarchProficiencies":
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies");
		default:
			Main.IsekaiContext.Logger.LogError("Port62Rewards: '" + name + "' is not an archetype proficiency feature an answer can be gated on.");
			return null;
		}
	}

	internal static Condition MythicGate(string mythicClassGuid)
	{
		BlueprintCharacterClass blueprint = BlueprintTools.GetBlueprint<BlueprintCharacterClass>(mythicClassGuid);
		if (blueprint == null)
		{
			return null;
		}
		return new UnitClass
		{
			Unit = new PlayerCharacter(),
			m_Class = blueprint.ToReference<BlueprintCharacterClassReference>()
		};
	}

	internal static string MythicClassFor(string sixTwoPath)
	{
		if (string.IsNullOrEmpty(sixTwoPath))
		{
			return null;
		}
		if (!SixTwoMythicClassIds.TryGetValue(sixTwoPath, out var value))
		{
			return null;
		}
		return value;
	}

	internal static BlueprintItem Item(string name)
	{
		return Lookup(name) as BlueprintItem;
	}

	internal static BlueprintBuff Buff(string name)
	{
		return Lookup(name) as BlueprintBuff;
	}

	internal static BlueprintFeature Feature(string name)
	{
		return Lookup(name) as BlueprintFeature;
	}

	private static BlueprintScriptableObject Lookup(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (s_Created.TryGetValue(name, out var value))
		{
			return value;
		}
		if (name == "ItemCodexOfReincarnation")
		{
			return CodexOfReincarnation.Item;
		}
		return null;
	}

    private static readonly AccessTools.FieldRef<BlueprintItemEquipmentSimple, BlueprintEquipmentEnchantmentReference[]> EquipmentEnchantments =
        AccessTools.FieldRefAccess<BlueprintItemEquipmentSimple, BlueprintEquipmentEnchantmentReference[]>("m_Enchantments");

	private static T Keep<T>(T blueprint) where T : BlueprintScriptableObject
	{
		if (blueprint != null)
		{
            // The engine enumerates this serialized array when creating an inventory item.
            // New reward equipment has no donor blueprint to initialize it for us.
            if (blueprint is BlueprintItemEquipmentSimple equipment)
                EquipmentEnchantments(equipment) ??= Array.Empty<BlueprintEquipmentEnchantmentReference>();
			s_Created[blueprint.name] = blueprint;
		}
		return blueprint;
	}

	internal static ActionGiveOtherworlderRewards Reward(string claimKey, int coins, string sixTwoSponsor, int gold, IEnumerable<string> vanillaItems, IEnumerable<string> modItems, string buff, int buffMinutes, string feature, string sponsorLine, int expCr = 0)
	{
		List<KeyValuePair<string, BlueprintItem>> source = (vanillaItems ?? new string[0]).Select((string id) => new KeyValuePair<string, BlueprintItem>(id, BlueprintTools.GetBlueprint<BlueprintItem>(id))).Concat((modItems ?? new string[0]).Select((string name) => new KeyValuePair<string, BlueprintItem>(name, Item(name)))).ToList();
		foreach (KeyValuePair<string, BlueprintItem> item in source.Where((KeyValuePair<string, BlueprintItem> e) => e.Value == null))
		{
			Main.IsekaiContext.Logger.LogError("Port62Rewards: reward '" + claimKey + "' lists item '" + item.Key + "', which does not resolve; it is left out.");
		}
		BlueprintItemReference[] items = (from e in source
			where e.Value != null
			select e.Value.ToReference<BlueprintItemReference>()).ToArray();
		KeptRewards.RegisterClaimReward(claimKey, Buff(buff), buffMinutes, Feature(feature));
		return new ActionGiveOtherworlderRewards
		{
			ClaimKey = claimKey,
			Coins = coins,
			Sponsor = Seat(sixTwoSponsor),
			Gold = gold,
			ExpCr = expCr,
			Items = items,
			Buff = Buff(buff)?.ToReference<BlueprintBuffReference>(),
			BuffMinutes = buffMinutes,
			Feature = Feature(feature)?.ToReference<BlueprintFeatureReference>(),
			SponsorLine = sponsorLine
		};
	}

	public static void Create()
	{
		s_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		CreatePrologueRewards();
		CreateBuffsAndFeatures();
		CreatePlainItems();
		CreateEquipment();
		CreateUsables();
	}

	private static Sprite IconOf<T>(string id) where T : BlueprintScriptableObject
	{
		T blueprint = BlueprintTools.GetBlueprint<T>(id);
		if (blueprint is BlueprintItem blueprintItem && blueprintItem.m_Icon != null)
		{
			return blueprintItem.m_Icon;
		}
		if (blueprint is BlueprintUnitFact blueprintUnitFact && blueprintUnitFact.m_Icon != null)
		{
			return blueprintUnitFact.m_Icon;
		}
		return s_Coin;
	}

	private static void StatBonus(BlueprintScriptableObject bp, StatType stat, int value, ModifierDescriptor descriptor)
	{
		bp.AddComponent(delegate(AddStatBonus c)
		{
			c.Stat = stat;
			c.Value = value;
			c.Descriptor = descriptor;
		});
	}

	private static void FearImmunity(BlueprintScriptableObject bp, SpellDescriptor extra = SpellDescriptor.None)
	{
		bp.AddComponent(delegate(AddConditionImmunity c)
		{
			c.Condition = UnitCondition.Frightened;
		});
		bp.AddComponent(delegate(AddConditionImmunity c)
		{
			c.Condition = UnitCondition.Shaken;
		});
		SpellDescriptor all = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened | extra;
		bp.AddComponent(delegate(BuffDescriptorImmunity c)
		{
			c.Descriptor = all;
		});
		bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
		{
			c.Descriptor = all;
		});
	}

	private static void CreatePrologueRewards()
	{
		Sprite scaleIcon = IconOf<BlueprintItem>("816f244523b5455a85ae06db452d4330");
		Sprite fearIcon = IconOf<BlueprintBuff>("c5c86809a1c834e42a2eb33133e90a28");
		Sprite wardIcon = IconOf<BlueprintBuff>("5274ddc289f4a7447b7ace68ad8bebb0");
		Sprite coldIcon = IconOf<BlueprintBuff>("dfedc0bf1d93f024d85546314c42b56a");
		BlueprintBuff aegis = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "SilverDragonAegisBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Silver Dragon's Aegis");
			bp.SetDescription(Main.IsekaiContext, "Terendelev's draconic starlight grants a +2 sacred bonus on all saving throws and absolute immunity to fear effects.");
			((BlueprintUnitFact)bp).m_Icon = fearIcon;
			StatType[] saves = Saves;
			foreach (StatType stat in saves)
			{
				StatBonus(bp, stat, 2, ModifierDescriptor.Sacred);
			}
			FearImmunity(bp, SpellDescriptor.None);
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "StitchWardBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Stitch's Scavenged Ward");
			bp.SetDescription(Main.IsekaiContext, "A hastily woven protective ward scavenged from Kenabres rubble, granting a +2 dodge bonus to Armor Class for 1 hour.");
			((BlueprintUnitFact)bp).m_Icon = wardIcon;
			StatBonus(bp, StatType.AC, 2, ModifierDescriptor.Dodge);
		}));
		BlueprintFeature shardFeature = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemSilverDragonShardFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Silver Dragon's Shard");
			bp.SetDescription(Main.IsekaiContext, "The shard's starlight shields you from dark sorceries: a +2 sacred bonus on saving throws against spells cast by evil creatures.");
			((BlueprintUnitFact)bp).m_Icon = scaleIcon;
			bp.HideInCharacterSheetAndLevelUp = true;
			bp.AddComponent(delegate(IsekaiBonusAgainstCreatures c)
			{
				c.RequireEvil = true;
				c.SaveBonus = 2;
				c.SaveOnlyAgainstSpells = true;
				c.Bonus = ModifierDescriptor.Sacred;
			});
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemSilverDragonShard", delegate(BlueprintItemEquipmentNeck bp)
		{
			bp.SetName(Main.IsekaiContext, "Silver Dragon's Shard");
			bp.SetDescription(Main.IsekaiContext, "A radiant silver crystal fragment imbued with Terendelev's sacred starlight. It pulses with gentle warmth, shielding its bearer from dark sorceries. Worn at the throat, it grants a +2 sacred bonus on saving throws against spells cast by evil creatures.");
			((BlueprintItem)bp).m_Icon = scaleIcon;
			((BlueprintItem)bp).m_Cost = 1000;
			((BlueprintItem)bp).m_Weight = 0.5f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.AddComponent(delegate(AddFactToEquipmentWielder c)
			{
				c.m_Fact = shardFeature.ToReference<BlueprintUnitFactReference>();
			});
		}));
		BlueprintBuff endurance = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "TearOfSilverDragonEnduranceBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Endurance of the Silver Dragon");
			bp.SetDescription(Main.IsekaiContext, "Terendelev's tear warms you against the freezing dark: you gain cold resistance 10.");
			((BlueprintUnitFact)bp).m_Icon = coldIcon;
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Cold;
				c.Value = 10;
			});
		}));
		BlueprintAbility tearAbility = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "TearOfSilverDragonAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Tear of the Silver Dragon");
			bp.SetDescription(Main.IsekaiContext, "You hold Terendelev's tear and her light answers: for 1 hour you gain Silver Dragon's Aegis (a +2 sacred bonus on all saving throws and immunity to fear effects) and cold resistance 10.");
			((BlueprintUnitFact)bp).m_Icon = fearIcon;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.CanTargetPoint = false;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(ApplyForAnHour(aegis), ApplyForAnHour(endurance));
			});
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemTearOfSilverDragon", delegate(BlueprintItemEquipmentUsable bp)
		{
			bp.SetName(Main.IsekaiContext, "Tear of the Silver Dragon");
			bp.SetDescription(Main.IsekaiContext, "A pearlescent silver teardrop gifted directly by Terendelev. Radiates ancient draconic grace, granting its bearer supernatural endurance against the freezing dark. Once per rest, its bearer can hold it to gain, for 1 hour, Silver Dragon's Aegis (a +2 sacred bonus on all saving throws and immunity to fear effects) and cold resistance 10.");
			((BlueprintItem)bp).m_Icon = scaleIcon;
			((BlueprintItem)bp).m_Cost = 3000;
			((BlueprintItem)bp).m_Weight = 0.5f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.Type = UsableItemType.Other;
			bp.SpendCharges = true;
			bp.Charges = 1;
			bp.RestoreChargesOnRest = true;
			((BlueprintItemEquipment)bp).m_Ability = tearAbility.ToReference<BlueprintAbilityReference>();
		}));
		static ContextActionApplyBuff ApplyForAnHour(BlueprintBuff buff)
		{
			return new ContextActionApplyBuff
			{
				m_Buff = buff.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Hours,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = 1
				}
			};
		}
	}

	private static void CreateBuffsAndFeatures()
	{
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "MindOfTheOtherworlderBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Mind of the Otherworlder");
			bp.IsClassFeature = false;
			bp.m_Flags |= BlueprintBuff.Flags.StayOnDeath;
			bp.SetDescription(Main.IsekaiContext, "Having traversed the cosmic void between dimensions, your mind is anchored beyond mortal terror, granting a permanent +2 sacred bonus on Will saving throws against fear and mind-affecting effects.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			bp.AddComponent(delegate(IsekaiSaveBonusAgainstDescriptor c)
			{
				c.Save = SavingThrowType.Will;
				c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;
				c.Value = 2;
				c.Bonus = ModifierDescriptor.Sacred;
			});
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "EmberSparkBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Ember's Saintly Spark");
			bp.IsClassFeature = false;
			bp.m_Flags |= BlueprintBuff.Flags.StayOnDeath;
			bp.Stacking = StackingType.Prolong;
			bp.SetDescription(Main.IsekaiContext, "Resonating with Ember's pure-hearted innocent flame, your fire spells and healing abilities receive a +2 sacred bonus: each damage roll of your fire spells deals 2 more damage, and your spells and abilities heal 2 more hit points. The spark also grants a +1 sacred bonus on Will saving throws.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			bp.AddComponent(delegate(IsekaiDescriptorSpellDamageBonus c)
			{
				c.Descriptor = SpellDescriptor.Fire;
				c.Value = 2;
			});
			bp.AddComponent(delegate(IsekaiHealingBonus c)
			{
				c.Value = 2;
			});
			StatBonus(bp, StatType.SaveWill, 1, ModifierDescriptor.Sacred);
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "LarielsVowFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Lariel's Vow");
			bp.SetDescription(Main.IsekaiContext, "The fallen angel Lariel honors your dual covenant of mercy and retribution, granting a +1 sacred bonus on attack rolls and +1 to caster level against chaotic evil outsiders. The caster level bonus applies to spells you cast at a chaotic evil outsider.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(IsekaiBonusAgainstCreatures c)
			{
				c.m_RequiredFact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("9054d3988d491d944ac144e27b6bc318");
				c.RequireEvil = true;
				c.RequireChaotic = true;
				c.AttackBonus = 1;
				c.CasterLevelBonus = 1;
				c.Bonus = ModifierDescriptor.Sacred;
			});
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ResonantMythicSparkFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Resonant Mythic Spark");
			bp.SetDescription(Main.IsekaiContext, "Cleansing and harmonizing the Kenabres Wardstone with otherworldly mana awakens your dormant mythic potential, granting a +1 sacred bonus to attack rolls, AC, and all saving throws.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			bp.IsClassFeature = false;
			StatBonus(bp, StatType.AdditionalAttackBonus, 1, ModifierDescriptor.Sacred);
			StatBonus(bp, StatType.AC, 1, ModifierDescriptor.Sacred);
			StatType[] saves = Saves;
			foreach (StatType stat in saves)
			{
				StatBonus(bp, stat, 1, ModifierDescriptor.Sacred);
			}
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "CrusaderLiberatorBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Crusader Liberator's Aegis");
			bp.IsClassFeature = false;
			bp.m_Flags |= BlueprintBuff.Flags.StayOnDeath;
			bp.SetDescription(Main.IsekaiContext, "Sovereign starlight channeled at the Lost Chapel cleanses all corruption, granting a +2 morale bonus on attack rolls and saving throws, and total immunity to fear and disease.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			StatBonus(bp, StatType.AdditionalAttackBonus, 2, ModifierDescriptor.Morale);
			StatType[] saves = Saves;
			foreach (StatType stat in saves)
			{
				StatBonus(bp, stat, 2, ModifierDescriptor.Morale);
			}
			FearImmunity(bp, SpellDescriptor.Disease);
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "SwordOfValorResonantFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Resonant Sword of Valor");
			bp.SetDescription(Main.IsekaiContext, "The liberated Sword of Valor resonates with otherworldly starlight, granting a +2 sacred bonus to attack rolls, armor class, and saving throws against evil outsiders.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(IsekaiBonusAgainstCreatures c)
			{
				c.m_RequiredFact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("9054d3988d491d944ac144e27b6bc318");
				c.RequireEvil = true;
				c.AttackBonus = 2;
				c.ACBonus = 2;
				c.SaveBonus = 2;
				c.Bonus = ModifierDescriptor.Sacred;
			});
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "TavernDefianceMoraleBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Tavern Defiance Morale");
			bp.SetDescription(Main.IsekaiContext, "Inspired by the Otherworlder Vanguard Doctrine during the defense of Defender's Heart, this unit gains a +2 morale bonus on attack rolls and Will saving throws.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			StatBonus(bp, StatType.AdditionalAttackBonus, 2, ModifierDescriptor.Morale);
			StatBonus(bp, StatType.SaveWill, 2, ModifierDescriptor.Morale);
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldEspressoBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Otherworld Espresso Rush");
			bp.SetDescription(Main.IsekaiContext, "A concentrated jolt of dimensional caffeine and alchemical sugar, granting Haste, a +2 morale bonus to attack rolls, saving throws, and skill checks, and absolute immunity to sleep and fatigue for 10 minutes.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			StatBonus(bp, StatType.AdditionalAttackBonus, 2, ModifierDescriptor.Morale);
			StatType[] saves = Saves;
			foreach (StatType stat in saves)
			{
				StatBonus(bp, stat, 2, ModifierDescriptor.Morale);
			}
			saves = Skills;
			foreach (StatType stat2 in saves)
			{
				StatBonus(bp, stat2, 2, ModifierDescriptor.Morale);
			}
			bp.AddComponent(delegate(BuffMovementSpeed c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Value = 30;
				c.ContextBonus = new ContextValue();
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Fatigued;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Sleeping;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Sleep;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Sleep;
			});
			BlueprintBuff haste = BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280");
			if (haste != null)
			{
				bp.AddComponent(delegate(AddFactContextActions c)
				{
					c.Activated = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = haste.ToReference<BlueprintBuffReference>(),
						AsChild = true,
						Permanent = true,
						DurationValue = new ContextDurationValue()
					});
					c.Deactivated = Helpers.CreateActionList();
					c.NewRound = Helpers.CreateActionList();
				});
			}
		}));
	}

	private static void CreatePlainItems()
	{
		Plain("ItemThieflingCharm", "Thiefling's Luck Charm", "A brass coin stamped with the mark of the Kenabres Thieflings, pledged by Woljif Jefto under an Otherworlder retainer contract.", 1500, 0.1f);
		Plain("ItemHellknightOfficerRegalia", "Hellknight Officer's Field Regalia", "A polished cold iron medallion stamped with the symbol of the Order of the Godclaw, conferred by Paralictor Regill Derenge in recognition of impeccable field triage and strategic logic.", 4000, 0.5f);
		Plain("ItemNenioInscribedJournal", "Nenio's Inscribed Research Journal", "A thick leather-bound compendium filled with Nenio's frantic diagrams, multiversal theorems, and cellular taxonomy notes. Inspires profound intellectual curiosity.", 3000, 1f);
		static BlueprintItem Plain(string name, string display, string description, int cost, float weight)
		{
			return Keep(Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintItem bp)
			{
				bp.SetName(Main.IsekaiContext, display);
				bp.SetDescription(Main.IsekaiContext, description);
				bp.m_Icon = s_Coin;
				bp.m_Cost = cost;
				bp.m_Weight = weight;
				bp.m_IsNotable = true;
				bp.m_Destructible = false;
			}));
		}
	}

	private static void Describe(BlueprintFeature bp, string display, string description, Sprite icon)
	{
		bp.SetName(Main.IsekaiContext, display);
		bp.SetDescription(Main.IsekaiContext, description);
		((BlueprintUnitFact)bp).m_Icon = icon;
		bp.HideInCharacterSheetAndLevelUp = true;
	}

	private static void Worn<T>(string name, string display, string description, Sprite icon, int cost, float weight, BlueprintFeature fact) where T : BlueprintItemEquipment, new()
	{
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(T bp)
		{
			bp.SetName(Main.IsekaiContext, display);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintItem)bp).m_Icon = icon;
			((BlueprintItem)bp).m_Cost = cost;
			((BlueprintItem)bp).m_Weight = weight;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.AddComponent(delegate(AddFactToEquipmentWielder c)
			{
				c.m_Fact = fact.ToReference<BlueprintUnitFactReference>();
			});
		}));
	}

	private static void CreateEquipment()
	{
		Sprite ringIcon = IconOf<BlueprintItemEquipmentRing>("f333bf86cd122974792162cbcd27c9ed");
		Sprite neckIcon = IconOf<BlueprintItemEquipmentNeck>("afd04948b8f211c448f1cc4bd9a67e3e");
		Sprite headIcon = IconOf<BlueprintItemEquipmentHead>("a3e8e907908ca7a40ac6da78e70bf33d");
		BlueprintFeature fact = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemLarielFeatherFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Lariel's Radiant Feather", "A pristine celestial feather plucked from the memory of the angel Lariel. Glows with unyielding righteousness against the demonic hordes. Worn at the throat, it grants a +2 sacred bonus on attack and damage rolls against demons.", s_Coin);
			bp.AddComponent(delegate(IsekaiBonusAgainstCreatures c)
			{
				c.m_RequiredFact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("dc960a234d365cb4f905bdc5937e623a");
				c.AttackBonus = 2;
				c.DamageBonus = 2;
				c.Bonus = ModifierDescriptor.Sacred;
			});
		}));
		Worn<BlueprintItemEquipmentNeck>("ItemLarielFeather", "Lariel's Radiant Feather", "A pristine celestial feather plucked from the memory of the angel Lariel. Glows with unyielding righteousness against the demonic hordes. Worn at the throat, it grants a +2 sacred bonus on attack and damage rolls against demons.", s_Coin, 2500, 0.1f, fact);
		BlueprintBuff signetEffect = Keep(TTCoreExtensions.CreateBuff("RoyalMendevianSignetAuraEffectBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Royal Mendevian Command");
			bp.SetDescription(Main.IsekaiContext, "Inspired by the Queen's signet on the commander's hand, this ally gains a +1 morale bonus on attack rolls and saving throws.");
			((BlueprintUnitFact)bp).m_Icon = ringIcon;
			StatBonus(bp, StatType.AdditionalAttackBonus, 1, ModifierDescriptor.Morale);
			StatType[] saves = Saves;
			foreach (StatType stat in saves)
			{
				StatBonus(bp, stat, 1, ModifierDescriptor.Morale);
			}
		}));
		BlueprintAbilityAreaEffect signetArea = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "RoyalMendevianSignetAuraArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddComponent(delegate(AbilityAreaEffectBuff c)
			{
				c.m_Buff = signetEffect.ToReference<BlueprintBuffReference>();
				c.Condition = new ConditionsChecker
				{
					Conditions = new Condition[1]
					{
						new ContextConditionIsCaster
						{
							Not = true
						}
					}
				};
			});
		}));
		BlueprintBuff signetAreaBuff = Keep(TTCoreExtensions.CreateBuff("RoyalMendevianSignetAuraAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Royal Mendevian Command");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +1 morale bonus on attack rolls and saving throws.");
			((BlueprintUnitFact)bp).m_Icon = ringIcon;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = signetArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		}));
		BlueprintFeature fact2 = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRoyalMendevianSignetFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Royal Mendevian Command Signet", "A gold signet ring engraved with the royal crest of Mendev, gifted directly by Queen Galfrey. Radiates sovereign authority, inspiring all companion vanguard forces. While it is worn, every ally within 30 feet of the wearer gains a +1 morale bonus on attack rolls and saving throws.", ringIcon);
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = signetAreaBuff.ToReference<BlueprintBuffReference>();
			});
		}));
		Worn<BlueprintItemEquipmentRing>("ItemRoyalMendevianSignet", "Royal Mendevian Command Signet", "A gold signet ring engraved with the royal crest of Mendev, gifted directly by Queen Galfrey. Radiates sovereign authority, inspiring all companion vanguard forces. While it is worn, every ally within 30 feet of the wearer gains a +1 morale bonus on attack rolls and saving throws.", ringIcon, 3500, 0.1f, fact2);
		BlueprintFeature fact3 = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRingOfTheAncientChroniclerFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Ring of the Ancient Chronicler", "A star-sapphire band engraved with the annals of forgotten realities, gifted by the Storyteller. Grants a +3 competence bonus to Knowledge (World) and Lore (Religion) checks.", ringIcon);
			StatBonus(bp, StatType.SkillKnowledgeWorld, 3, ModifierDescriptor.Competence);
			StatBonus(bp, StatType.SkillLoreReligion, 3, ModifierDescriptor.Competence);
		}));
		Worn<BlueprintItemEquipmentRing>("ItemRingOfTheAncientChronicler", "Ring of the Ancient Chronicler", "A star-sapphire band engraved with the annals of forgotten realities, gifted by the Storyteller. Grants a +3 competence bonus to Knowledge (World) and Lore (Religion) checks.", ringIcon, 3500, 0.1f, fact3);
		BlueprintFeature fact4 = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemThieflingMasterLockpicksFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Thiefling Master Lockpicks", "A set of finely-tempered cold iron tension wrenches and skeleton picks gifted by Sister Kerisme. Grants a +3 competence bonus to Trickery checks. The picks hang from a cold iron key ring and work while it is worn.", ringIcon);
			StatBonus(bp, StatType.SkillThievery, 3, ModifierDescriptor.Competence);
		}));
		Worn<BlueprintItemEquipmentRing>("ItemThieflingMasterLockpicks", "Thiefling Master Lockpicks", "A set of finely-tempered cold iron tension wrenches and skeleton picks gifted by Sister Kerisme. Grants a +3 competence bonus to Trickery checks. The picks hang from a cold iron key ring and work while it is worn.", ringIcon, 2500, 0.5f, fact4);
		BlueprintFeature fact5 = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemTalismanOfTheCosmicTouristFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Talisman of the Cosmic Tourist", "An otherworldly silver pendant shaped like a compass rose. Grants a +2 luck bonus to Armor Class and all saving throws.", neckIcon);
			StatBonus(bp, StatType.AC, 2, ModifierDescriptor.Luck);
			StatType[] saves = Saves;
			foreach (StatType stat in saves)
			{
				StatBonus(bp, stat, 2, ModifierDescriptor.Luck);
			}
		}));
		Worn<BlueprintItemEquipmentNeck>("ItemTalismanOfTheCosmicTourist", "Talisman of the Cosmic Tourist", "An otherworldly silver pendant shaped like a compass rose. Grants a +2 luck bonus to Armor Class and all saving throws.", neckIcon, 10000, 0.5f, fact5);
		BlueprintFeature fact6 = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemAmuletOfOtherworldEquilibriumFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Amulet of Otherworld Equilibrium", "A pristine crystal talisman that stabilizes the wearer's biological rhythms, granting a +4 enhancement bonus to Constitution and absolute immunity to fatigue and exhaustion.", neckIcon);
			StatBonus(bp, StatType.Constitution, 4, ModifierDescriptor.Enhancement);
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Fatigued;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Exhausted;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
			});
		}));
		Worn<BlueprintItemEquipmentNeck>("ItemAmuletOfOtherworldEquilibrium", "Amulet of Otherworld Equilibrium", "A pristine crystal talisman that stabilizes the wearer's biological rhythms, granting a +4 enhancement bonus to Constitution and absolute immunity to fatigue and exhaustion.", neckIcon, 15000, 0.5f, fact6);
		BlueprintFeature fact7 = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRingOfTheGrandAuditorFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Ring of the Grand Auditor", "A platinum signet ring engraved with analytical runes. Grants a +5 competence bonus to Persuasion, Lore (Religion), and Knowledge (World) checks.", ringIcon);
			StatBonus(bp, StatType.SkillPersuasion, 5, ModifierDescriptor.Competence);
			StatBonus(bp, StatType.SkillLoreReligion, 5, ModifierDescriptor.Competence);
			StatBonus(bp, StatType.SkillKnowledgeWorld, 5, ModifierDescriptor.Competence);
		}));
		Worn<BlueprintItemEquipmentRing>("ItemRingOfTheGrandAuditor", "Ring of the Grand Auditor", "A platinum signet ring engraved with analytical runes. Grants a +5 competence bonus to Persuasion, Lore (Religion), and Knowledge (World) checks.", ringIcon, 14000, 0.1f, fact7);
		BlueprintFeature fact8 = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemMartialGodFocusBandFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Martial God's Focus Band", "A woven headband imbued with focused spiritual prana. Grants a +2 sacred bonus to unarmed and natural attack and damage rolls and a +2 sacred bonus to Reflex saving throws.", headIcon);
			bp.AddComponent(delegate(IsekaiUnarmedAndNaturalBonus c)
			{
				c.AttackBonus = 2;
				c.DamageBonus = 2;
				c.Bonus = ModifierDescriptor.Sacred;
			});
			StatBonus(bp, StatType.SaveReflex, 2, ModifierDescriptor.Sacred);
		}));
		Worn<BlueprintItemEquipmentHead>("ItemMartialGodFocusBand", "Martial God's Focus Band", "A woven headband imbued with focused spiritual prana. Grants a +2 sacred bonus to unarmed and natural attack and damage rolls and a +2 sacred bonus to Reflex saving throws.", headIcon, 12000, 0.5f, fact8);
		BlueprintFeature fact9 = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemSovereignsImperialSignetFeature", delegate(BlueprintFeature bp)
		{
			Describe(bp, "Sovereign's Imperial Signet", "A gold signet ring radiating imperial authority. Grants a +2 sacred bonus to the Difficulty Class (DC) of all mind-affecting and enchantment spells and abilities, and a +2 sacred bonus on Will saving throws.", ringIcon);
			bp.AddComponent(delegate(IsekaiSchoolOrDescriptorDCBonus c)
			{
				c.School = SpellSchool.Enchantment;
				c.Descriptor = SpellDescriptor.MindAffecting;
				c.Value = 2;
				c.Bonus = ModifierDescriptor.Sacred;
			});
			StatBonus(bp, StatType.SaveWill, 2, ModifierDescriptor.Sacred);
		}));
		Worn<BlueprintItemEquipmentRing>("ItemSovereignsImperialSignet", "Sovereign's Imperial Signet", "A gold signet ring radiating imperial authority. Grants a +2 sacred bonus to the Difficulty Class (DC) of all mind-affecting and enchantment spells and abilities, and a +2 sacred bonus on Will saving throws.", ringIcon, 16000, 0.1f, fact9);
		BlueprintItemWeapon blueprint = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("ec731c55e657cf0408fd89c648ccc536");
		if (blueprint == null)
		{
			return;
		}
		Keep(blueprint.CreateCopy(Main.IsekaiContext, "ItemGwermAncestralRapier", delegate(BlueprintItemWeapon bp)
		{
			bp.SetName(Main.IsekaiContext, "Gwerm Ancestral Rapier");
			bp.SetDescription(Main.IsekaiContext, "An heirloom cold iron rapier passed down through the Gwerm bloodline, sharpened to a lethal edge. Deals +1 enhancement bonus and possesses the Keen property.");
			((BlueprintItem)bp).m_Cost = 4500;
			List<BlueprintWeaponEnchantmentReference> list = (bp.m_Enchantments ?? new BlueprintWeaponEnchantmentReference[0]).ToList();
			string[] array = new string[2] { "d42fc23b92c640846ac137dc26e000d4", "102a9c8c9b7a75e4fb5844e79deaf4c0" };
			foreach (string id in array)
			{
				BlueprintWeaponEnchantmentReference enchantment = BlueprintTools.GetBlueprintReference<BlueprintWeaponEnchantmentReference>(id);
				if (!list.Any((BlueprintWeaponEnchantmentReference e) => ((BlueprintReferenceBase)e).deserializedGuid == ((BlueprintReferenceBase)enchantment).deserializedGuid))
				{
					list.Add(enchantment);
				}
			}
			bp.m_Enchantments = list.ToArray();
		}));
	}

	private static void CreateUsables()
	{
		BlueprintBuff confounded = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "VescavorPheromoneConfoundedBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Confounded by Pheromones");
			bp.SetDescription(Main.IsekaiContext, "The Vescavor Queen's scent has overwhelmed this creature's senses: it is confused.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			bp.AddComponent(delegate(AddCondition c)
			{
				c.Condition = UnitCondition.Confusion;
			});
		}));
		BlueprintAbility flaskAbility = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "VescavorPheromoneFlaskAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Vescavor Pheromone Flask");
			bp.SetDescription(Main.IsekaiContext, "You uncork the flask. Every enemy vermin or swarm within 30 feet must succeed on a DC 15 Fortitude saving throw or be confused for 1d4+1 rounds.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			bp.Type = AbilityType.Extraordinary;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.CanTargetPoint = false;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			bp.AddComponent(delegate(AbilityTargetsAround c)
			{
				c.m_Radius = new Feet(30f);
				c.m_TargetType = TargetType.Enemy;
				c.m_Condition = new ConditionsChecker
				{
					Operation = Operation.Or,
					Conditions = new Condition[3]
					{
						Has("09478937695300944a179530664e42ec"),
						Has("2e3e840ab458ce04c92064489f87ecc2"),
						Has("5a04735fd0e952142bfc8ecf995e2361")
					}
				};
			});
			bp.AddComponent(delegate(ContextSetAbilityParams c)
			{
				c.DC = 15;
				c.Add10ToDC = false;
				c.CasterLevel = 5;
				c.SpellLevel = 1;
				c.Concentration = 0;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.SavingThrowType = SavingThrowType.Fortitude;
				c.Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
				{
					Succeed = Helpers.CreateActionList(),
					Failed = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = confounded.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.D4,
							DiceCountValue = 1,
							BonusValue = 1
						}
					})
				});
			});
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemVescavorPheromoneFlask", delegate(BlueprintItemEquipmentUsable bp)
		{
			bp.SetName(Main.IsekaiContext, "Vescavor Pheromone Flask");
			bp.SetDescription(Main.IsekaiContext, "A sealed crystal flask filled with concentrated Vescavor Queen pheromones confiscated from the saboteur Nurah. Its pungent scent can confound insectoid swarms and draw vermin away from friendly ranks. Once per rest, uncorking it forces every enemy vermin or swarm within 30 feet to succeed on a DC 15 Fortitude saving throw or be confused for 1d4+1 rounds.");
			((BlueprintItem)bp).m_Icon = s_Coin;
			((BlueprintItem)bp).m_Cost = 2000;
			((BlueprintItem)bp).m_Weight = 0.5f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.Type = UsableItemType.Other;
			bp.SpendCharges = true;
			bp.Charges = 1;
			bp.RestoreChargesOnRest = true;
			((BlueprintItemEquipment)bp).m_Ability = flaskAbility.ToReference<BlueprintAbilityReference>();
		}));
		BlueprintBuff rush = Buff("OtherworldEspressoBuff");
		BlueprintAbility espressoAbility = Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldEspressoAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Otherworld Triple-Shot Espresso");
			bp.SetDescription(Main.IsekaiContext, "You drink the espresso and gain Otherworld Espresso Rush for 10 minutes: Haste, a +2 morale bonus to attack rolls, saving throws, and skill checks, and immunity to sleep and fatigue.");
			((BlueprintUnitFact)bp).m_Icon = s_Coin;
			bp.Type = AbilityType.Extraordinary;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.CanTargetPoint = false;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
				{
					m_Buff = rush.ToReference<BlueprintBuffReference>(),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.TenMinutes,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 1
					}
				});
			});
		}));
		Keep(Helpers.CreateBlueprint(Main.IsekaiContext, "ItemOtherworldEspresso", delegate(BlueprintItemEquipmentUsable bp)
		{
			bp.SetName(Main.IsekaiContext, "Otherworld Triple-Shot Espresso");
			bp.SetDescription(Main.IsekaiContext, "A steaming porcelain thermos filled with concentrated Otherworld espresso brewed from celestial roasted beans. Grants the Otherworld Espresso Rush buff for 10 minutes.");
			((BlueprintItem)bp).m_Icon = s_Coin;
			((BlueprintItem)bp).m_Cost = 750;
			((BlueprintItem)bp).m_Weight = 0.5f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.Type = UsableItemType.Potion;
			bp.SpendCharges = true;
			bp.Charges = 1;
			((BlueprintItemEquipment)bp).m_Ability = espressoAbility.ToReference<BlueprintAbilityReference>();
		}));
		static ContextConditionHasFact Has(string id)
		{
			return new ContextConditionHasFact
			{
				m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(id)
			};
		}
	}
}
