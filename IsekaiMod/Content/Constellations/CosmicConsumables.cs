using System;
using System.Collections.Generic;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations;

public static class CosmicConsumables
{
	private static readonly Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");

	public static readonly Dictionary<string, BlueprintItem> Items = new Dictionary<string, BlueprintItem>(StringComparer.Ordinal);

	public static BlueprintBuff SoulAnchorBuff;

	public static BlueprintItemEquipmentUsable ItemCosmicWishstone;

	private static Sprite IconFor(string abilityName)
	{
		string text = CosmicConsumableCatalog.IconSourceOf(abilityName);
		if (text == null)
		{
			return Icon_Coin;
		}
		SimpleBlueprint blueprint = BlueprintTools.GetBlueprint<SimpleBlueprint>(text);
		Sprite sprite = null;
		if (blueprint is BlueprintUnitFact blueprintUnitFact)
		{
			sprite = blueprintUnitFact.m_Icon;
		}
		else if (blueprint is BlueprintItem blueprintItem)
		{
			sprite = blueprintItem.m_Icon;
		}
		return sprite ?? Icon_Coin;
	}

	private static ContextValue Rank(AbilityRankType type = AbilityRankType.Default)
	{
		return new ContextValue
		{
			ValueType = ContextValueType.Rank,
			ValueRank = type
		};
	}

	private static void AddRankConfig(BlueprintFeature feature, AbilityRankType type, int perBottle)
	{
		feature.AddComponent(delegate(ContextRankConfig c)
		{
			c.m_Type = type;
			c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
			c.m_Feature = feature.ToReference<BlueprintFeatureReference>();
			if (perBottle == 1)
			{
				c.m_Progression = ContextRankProgression.AsIs;
			}
			else
			{
				c.m_Progression = ContextRankProgression.MultiplyByModifier;
				c.m_StepLevel = perBottle;
			}
		});
	}

	private static void AddUntypedStat(BlueprintFeature feature, StatType stat)
	{
		feature.AddComponent(delegate(AddContextStatBonus c)
		{
			c.Descriptor = ModifierDescriptor.UntypedStackable;
			c.Stat = stat;
			c.Multiplier = 1;
			c.Value = Rank();
		});
	}

	private static ContextDurationValue Hours(int hours)
	{
		return new ContextDurationValue
		{
			Rate = DurationRate.Hours,
			DiceType = DiceType.Zero,
			DiceCountValue = 0,
			BonusValue = hours
		};
	}

	private static BlueprintAbility DrinkAbility(string name, string displayName, string description, GameAction action)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintUnitFact)bp).m_Icon = IconFor(name);
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Touch;
			bp.CanTargetSelf = true;
			bp.CanTargetFriends = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(action);
			});
		});
	}

	private static BlueprintItemEquipmentUsable Bottle(string name, string displayName, string description, int coins, BlueprintAbility ability, UsableItemType type = UsableItemType.Other, float weight = 0.5f)
	{
		BlueprintItemEquipmentUsable blueprintItemEquipmentUsable = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintItemEquipmentUsable bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintItem)bp).m_Icon = ((BlueprintUnitFact)ability).m_Icon;
			((BlueprintItem)bp).m_Cost = CosmicEnvoyPrices.GoldValue(coins);
			((BlueprintItem)bp).m_Weight = weight;
			bp.Type = type;
			bp.SpendCharges = true;
			bp.Charges = 1;
			bp.RestoreChargesOnRest = false;
			((BlueprintItemEquipment)bp).m_Ability = ability.ToReference<BlueprintAbilityReference>();
		});
		Items[name] = blueprintItemEquipmentUsable;
		return blueprintItemEquipmentUsable;
	}

	private static void Permanent(string name, int coins, string featureName, string featureDescription, string abilityName, string abilityDescription, string itemName, string itemDescription, string effectPerBottle, Action<BlueprintFeature> addEffects)
	{
		BlueprintFeature bp = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature blueprintFeature)
		{
			blueprintFeature.SetName(Main.IsekaiContext, featureName);
			blueprintFeature.SetDescription(Main.IsekaiContext, featureDescription);
			((BlueprintUnitFact)blueprintFeature).m_Icon = IconFor(name + "Ability");
			blueprintFeature.Ranks = 999;
			blueprintFeature.IsClassFeature = false;
			addEffects(blueprintFeature);
		});
		BlueprintAbility ability = DrinkAbility(name + "Ability", abilityName, abilityDescription, new ContextActionConsumeStackableConsumable
		{
			m_Feature = bp.ToReference<BlueprintFeatureReference>(),
			ItemName = itemName,
			EffectPerBottle = effectPerBottle
		});
		Bottle(name, itemName, itemDescription, coins, ability);
	}

	private static void Draught(string name, int coins, BlueprintBuff buff, string abilityName, string abilityDescription, string itemName, string itemDescription)
	{
		BlueprintAbility ability = DrinkAbility(name + "Ability", abilityName, abilityDescription, new ContextActionApplyBuff
		{
			m_Buff = buff.ToReference<BlueprintBuffReference>(),
			DurationValue = Hours(24)
		});
		Bottle(name, itemName, itemDescription, coins, ability);
	}

	private static BlueprintBuff DraughtBuff(string name, string iconAbility, string displayName, string description, Action<BlueprintBuff> addEffects)
	{
		return TTCoreExtensions.CreateBuff(name, delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintUnitFact)bp).m_Icon = IconFor(iconAbility);
			bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			addEffects(bp);
		});
	}

	public static void Add()
	{
		AddAmbrosias();
		AddNectars();
		AddDraughts();
		AddWishstone();
		Main.IsekaiContext.Logger.Log($"[CosmicConsumables] {Items.Count} Cosmic consumables created.");
	}

	private static void AddStatAmbrosia(string name, StatType stat, string featureName, string featureDescription, string abilityName, string abilityDescription, string itemName, string itemDescription, string effectPerBottle)
	{
		Permanent(name, 3000, featureName, featureDescription, abilityName, abilityDescription, itemName, itemDescription, effectPerBottle, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = stat;
				c.Multiplier = 2;
				c.Value = Rank();
			});
			AddRankConfig(bp, AbilityRankType.Default, 1);
		});
	}

	private static void AddAmbrosias()
	{
		AddStatAmbrosia("ItemPrimordialAmbrosiaMight", StatType.Strength, "Primordial Ambrosia: Might", "Infused with untyped primordial divine essence. Each bottle consumed permanently grants a +2 Untyped Stackable bonus to Might.", "Drink Primordial Ambrosia of Might", "Drink or administer this shimmering nectar. Permanently increases Might by +2 (Untyped Stackable). Stacks infinitely with itself, gear, and all other bonuses!", "Primordial Ambrosia of Might", "A crystallized vial of starlight nectar brewed from the tears of the Outer Gods. Permanently increases Might by +2. It is an untyped bonus with no upper limit--the only limit is how many coins you spend!", "+2 Strength");
		AddStatAmbrosia("ItemPrimordialAmbrosiaGrace", StatType.Dexterity, "Primordial Ambrosia: Grace", "Infused with untyped primordial divine essence. Each bottle consumed permanently grants a +2 Untyped Stackable bonus to Grace.", "Drink Primordial Ambrosia of Grace", "Drink or administer this shimmering nectar. Permanently increases Grace by +2 (Untyped Stackable). Stacks infinitely with itself, gear, and all other bonuses!", "Primordial Ambrosia of Grace", "A crystallized vial of starlight nectar brewed from the tears of the Outer Gods. Permanently increases Grace by +2. It is an untyped bonus with no upper limit--the only limit is how many coins you spend!", "+2 Dexterity");
		AddStatAmbrosia("ItemPrimordialAmbrosiaEndurance", StatType.Constitution, "Primordial Ambrosia: Endurance", "Infused with untyped primordial divine essence. Each bottle consumed permanently grants a +2 Untyped Stackable bonus to Endurance.", "Drink Primordial Ambrosia of Endurance", "Drink or administer this shimmering nectar. Permanently increases Endurance by +2 (Untyped Stackable). Stacks infinitely with itself, gear, and all other bonuses!", "Primordial Ambrosia of Endurance", "A crystallized vial of starlight nectar brewed from the tears of the Outer Gods. Permanently increases Endurance by +2. It is an untyped bonus with no upper limit--the only limit is how many coins you spend!", "+2 Constitution");
		AddStatAmbrosia("ItemPrimordialAmbrosiaIntellect", StatType.Intelligence, "Primordial Ambrosia: Intellect", "Infused with untyped primordial divine essence. Each bottle consumed permanently grants a +2 Untyped Stackable bonus to Intellect.", "Drink Primordial Ambrosia of Intellect", "Drink or administer this shimmering nectar. Permanently increases Intellect by +2 (Untyped Stackable). Stacks infinitely with itself, gear, and all other bonuses!", "Primordial Ambrosia of Intellect", "A crystallized vial of starlight nectar brewed from the tears of the Outer Gods. Permanently increases Intellect by +2. It is an untyped bonus with no upper limit--the only limit is how many coins you spend!", "+2 Intelligence");
		AddStatAmbrosia("ItemPrimordialAmbrosiaInsight", StatType.Wisdom, "Primordial Ambrosia: Insight", "Infused with untyped primordial divine essence. Each bottle consumed permanently grants a +2 Untyped Stackable bonus to Insight.", "Drink Primordial Ambrosia of Insight", "Drink or administer this shimmering nectar. Permanently increases Insight by +2 (Untyped Stackable). Stacks infinitely with itself, gear, and all other bonuses!", "Primordial Ambrosia of Insight", "A crystallized vial of starlight nectar brewed from the tears of the Outer Gods. Permanently increases Insight by +2. It is an untyped bonus with no upper limit--the only limit is how many coins you spend!", "+2 Wisdom");
		AddStatAmbrosia("ItemPrimordialAmbrosiaMajesty", StatType.Charisma, "Primordial Ambrosia: Majesty", "Infused with untyped primordial divine essence. Each bottle consumed permanently grants a +2 Untyped Stackable bonus to Majesty.", "Drink Primordial Ambrosia of Majesty", "Drink or administer this shimmering nectar. Permanently increases Majesty by +2 (Untyped Stackable). Stacks infinitely with itself, gear, and all other bonuses!", "Primordial Ambrosia of Majesty", "A crystallized vial of starlight nectar brewed from the tears of the Outer Gods. Permanently increases Majesty by +2. It is an untyped bonus with no upper limit--the only limit is how many coins you spend!", "+2 Charisma");
		Permanent("ItemPrimordialAmbrosiaCelerity", 3000, "Primordial Ambrosia: Celerity", "Infused with primordial swiftness. Each bottle consumed permanently grants a +10 ft Untyped Stackable bonus to base movement speed.", "Drink Primordial Ambrosia of Celerity", "Drink or administer this quicksilver nectar. Permanently increases base movement speed by +10 ft (Untyped Stackable). Stacks with Haste, Boots of Speed, and class abilities!", "Primordial Ambrosia of Celerity", "Quicksilver nectar distilled from shooting stars. Permanently grants +10 ft base movement speed per bottle. Untyped and infinitely stackable!", "+10 ft speed", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Speed;
				c.Multiplier = 10;
				c.Value = Rank();
			});
			AddRankConfig(bp, AbilityRankType.Default, 1);
		});
		Permanent("ItemPrimordialAmbrosiaEternalRenewal", 10000, "Primordial Ambrosia: Eternal Renewal", "Infused with immortal phoenix fire. Each bottle consumed permanently grants Fast Healing 5 (Untyped Stackable). Stacks infinitely with itself!", "Drink Primordial Ambrosia of Eternal Renewal", "Permanently grants Fast Healing 5 (Untyped Stackable). Stacks infinitely with itself (drink 2 = FH 10, drink 4 = FH 20, drink 10 = FH 50 every round)!", "Primordial Ambrosia of Eternal Renewal", "The apex elixir of godly immortality. Permanently grants Fast Healing 5 per bottle consumed. Untyped, uncapped, and stacks with itself infinitely. The ultimate high-roller prestige trophy!", "Fast Healing +5", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 0;
				c.Bonus = Rank();
			});
			AddRankConfig(bp, AbilityRankType.Default, 5);
		});
	}

	private static void AddNectars()
	{
		Permanent("ItemNectarEndlessLife", 1500, "Nectar: Endless Life", "Permanently grants +25 Maximum Hit Points per bottle consumed (Untyped Stackable).", "Drink Nectar of Endless Life", "Permanently increases Maximum Hit Points by +25 (Untyped Stackable).", "Nectar of Endless Life", "Golden nectar teeming with primal vitality. Permanently grants +25 Max Hit Points per bottle. Untyped and infinitely stackable!", "+25 maximum hit points", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Multiplier = 25;
				c.Value = Rank();
			});
			AddRankConfig(bp, AbilityRankType.Default, 1);
		});
		Permanent("ItemNectarVoidStalker", 4000, "Nectar: Void Stalker", "Permanently increases Spell DC by +1 and Spell Penetration by +2 per bottle consumed (Untyped Stackable).", "Drink Nectar of the Void Stalker", "Permanently increases Spell DC by +1 and Spell Penetration by +2 (Untyped Stackable).", "Nectar of the Void Stalker", "Deep violet nectar that attunes your soul to the void. Permanently grants +1 Spell DC and +2 Spell Penetration per bottle. Untyped and infinitely stackable!", "+1 spell DC and +2 spell penetration", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = Rank();
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = Rank(AbilityRankType.DamageBonus);
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			AddRankConfig(bp, AbilityRankType.Default, 1);
			AddRankConfig(bp, AbilityRankType.DamageBonus, 2);
		});
		Permanent("ItemNectarWorldBreaker", 4000, "Nectar: World Breaker", "Permanently increases Attack Bonus by +2 and Weapon Damage by +2 per bottle consumed (Untyped Stackable).", "Drink Nectar of the World Breaker", "Permanently increases Attack Bonus by +2 and Weapon Damage by +2 (Untyped Stackable).", "Nectar of the World Breaker", "Crimson nectar brimming with tectonic wrath. Permanently grants +2 Attack Bonus and +2 Weapon Damage per bottle. Untyped and infinitely stackable!", "+2 attack and +2 weapon damage", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Multiplier = 2;
				c.Value = Rank();
			});
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Multiplier = 2;
				c.Value = Rank();
			});
			AddRankConfig(bp, AbilityRankType.Default, 1);
		});
		Permanent("ItemNectarDiamondSoul", 3500, "Nectar: Diamond Soul", "Permanently grants a +2 Untyped Stackable bonus to all Saving Throws (Fortitude, Reflex, Will) per bottle consumed.", "Drink Nectar of the Diamond Soul", "Permanently increases all Saving Throws by +2 (Untyped Stackable).", "Nectar of the Diamond Soul", "Pristine crystalline nectar that hardens the spirit into unbreakable diamond. Permanently grants +2 to all Saving Throws per bottle. Untyped and infinitely stackable!", "+2 to all saving throws", delegate(BlueprintFeature bp)
		{
			StatType[] array = new StatType[3]
			{
				StatType.SaveFortitude,
				StatType.SaveReflex,
				StatType.SaveWill
			};
			foreach (StatType save in array)
			{
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = save;
					c.Multiplier = 2;
					c.Value = Rank();
				});
			}
			AddRankConfig(bp, AbilityRankType.Default, 1);
		});
		Permanent("ItemNectarUnbrokenAegis", 3500, "Nectar: Unbroken Aegis", "Permanently grants a +1 Untyped Stackable bonus to Armor Class per bottle consumed.", "Drink Nectar of the Unbroken Aegis", "Permanently increases Armor Class by +1 (Untyped Stackable).", "Nectar of the Unbroken Aegis", "Dense platinum nectar that crystallizes across your skin. Permanently grants +1 Armor Class per bottle. Untyped and infinitely stackable!", "+1 Armor Class", delegate(BlueprintFeature bp)
		{
			AddUntypedStatPerBottle(bp, StatType.AC, 1);
			AddRankConfig(bp, AbilityRankType.Default, 1);
		});
	}

	private static void AddUntypedStatPerBottle(BlueprintFeature feature, StatType stat, int perBottle)
	{
		feature.AddComponent(delegate(AddContextStatBonus c)
		{
			c.Descriptor = ModifierDescriptor.UntypedStackable;
			c.Stat = stat;
			c.Multiplier = perBottle;
			c.Value = Rank();
		});
	}

	private static void AddDraughts()
	{
		BlueprintBuff buff = DraughtBuff("DraughtCosmicVelocityBuff", "ItemDraughtCosmicVelocityAbility", "Draught of Cosmic Velocity", "Grants a +20 ft Untyped Stackable bonus to movement speed and +1 extra attack on a full attack. Stacks with Haste and all equipment.", delegate(BlueprintBuff bp)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Speed;
				c.Value = 20;
			});
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
		});
		Draught("ItemDraughtCosmicVelocity", 2500, buff, "Drink Draught of Cosmic Velocity", "Grants +20 ft movement speed and +1 extra attack on full attack for 24 hours (Untyped Stackable).", "Draught of Cosmic Velocity", "A potent 24-hour combat stimulant. Grants an untyped +20 ft speed and +1 extra attack on full attack for 24 hours. Stacks directly with Haste!");
		BlueprintBuff buff2 = DraughtBuff("DraughtImmortalTitanBuff", "ItemDraughtImmortalTitanAbility", "Draught of the Immortal Titan", "Grants +150 Temporary Hit Points, DR 10/--, and Fast Healing 10 for 24 hours.", delegate(BlueprintBuff bp)
		{
			bp.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
			{
				c.Value = 150;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 10;
				c.Bonus = 0;
			});
		});
		Draught("ItemDraughtImmortalTitan", 2500, buff2, "Drink Draught of the Immortal Titan", "Grants +150 Temporary Hit Points, DR 10/--, and Fast Healing 10 for 24 hours.", "Draught of the Immortal Titan", "Titan blood infused with adamant wards. Grants +150 Temp HP, DR 10/--, and Fast Healing 10 for 24 hours.");
		BlueprintBuff buff3 = DraughtBuff("DraughtArcaneOmniscienceBuff", "ItemDraughtArcaneOmniscienceAbility", "Draught of Arcane Omniscience", "Grants a +4 bonus to Caster Level, and all spells are automatically cast as though affected by Extend Spell and Empower Spell without raising spell level.", delegate(BlueprintBuff bp)
		{
			bp.AddComponent(delegate(IncreaseCasterLevel c)
			{
				c.Value = 4;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 4;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			bp.AddComponent(delegate(AutoMetamagic c)
			{
				c.Metamagic = Metamagic.Empower | Metamagic.Extend;
				c.Abilities = new List<BlueprintAbilityReference>();
			});
		});
		Draught("ItemDraughtArcaneOmniscience", 3000, buff3, "Drink Draught of Arcane Omniscience", "Grants +4 Caster Level check bonus and automatically extends and empowers all spells for 24 hours.", "Draught of Arcane Omniscience", "Liquid ley-line mana. Grants +4 Spell Penetration/CL bonus and auto-extends and auto-empowers all cast spells for 24 hours.");
		BlueprintBuff buff4 = DraughtBuff("DraughtApexPredatorBuff", "ItemDraughtApexPredatorAbility", "Draught of the Apex Predator", "Increases critical multiplier by +1 and causes all weapon attacks to completely bypass Damage Reduction and Concealment for 24 hours.", delegate(BlueprintBuff bp)
		{
			bp.AddComponent(delegate(CriticalConfirmationBonus c)
			{
				c.Value = 6;
			});
			bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
			{
				c.Type = WeaponRangeType.Melee;
				c.AdditionalMultiplier = 1;
			});
			bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
			{
				c.Type = WeaponRangeType.Ranged;
				c.AdditionalMultiplier = 1;
			});
			bp.AddComponent<IgnoreConcealment>();
			bp.AddComponent<IgnoreDamageReductionOnAttack>();
		});
		Draught("ItemDraughtApexPredator", 3000, buff4, "Drink Draught of the Apex Predator", "Grants +6 critical confirmation, bypasses all Damage Reduction, and ignores concealment for 24 hours.", "Draught of the Apex Predator", "Primal predator adrenaline. Enhances critical confirmations by +6 and bypasses all DR and concealment for 24 hours.");
		SoulAnchorBuff = DraughtBuff("SoulAnchorBuff", "ItemDraughtAbsoluteSoulAnchorAbility", "Absolute Soul Anchor", "Your soul is anchored to the outer cosmos. Immune to death effects, negative levels, energy drain, and compulsion.", delegate(BlueprintBuff bp)
		{
			SpellDescriptor anchored = SpellDescriptor.Compulsion | SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = anchored;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = anchored;
			});
			bp.AddComponent<AddImmunityToEnergyDrain>();
		});
		Draught("ItemDraughtAbsoluteSoulAnchor", 500, SoulAnchorBuff, "Drink Draught of Absolute Soul Anchor", "Immunity to death effects, negative levels, energy drain, and compulsion for 24 hours.", "Draught of Absolute Soul Anchor", "Anchors your soul to the outer cosmos. Grants complete immunity to death effects, negative levels, and mental compulsion for 24 hours.");
	}

	private static BlueprintAbility Miracle(string name, string displayName, string description, Action<BlueprintAbility> configure)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintUnitFact)bp).m_Icon = IconFor(name);
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			configure(bp);
		});
	}

	private static void AddWishstone()
	{
		BlueprintAbility blueprintAbility = Miracle("WishstoneMiracleRestorationAbility", "Wishstone: Miracle of Restoration", "True Resurrection on all fallen party members at 100% HP, restores all spell slots and class resources to maximum (except Merchant's Gamble pulls), and cleanses all conditions and Abyssal Corruption.", delegate(BlueprintAbility bp)
		{
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionWishstoneRestoration());
			});
		});
		BlueprintBuff fateBuff = TTCoreExtensions.CreateBuff("WishstoneAbsoluteFateBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Miracle of Absolute Fate");
			bp.SetDescription(Main.IsekaiContext, "All attack rolls, saving throws, and skill checks automatically result in Natural 20s for 3 rounds!");
			((BlueprintUnitFact)bp).m_Icon = IconFor("WishstoneMiracleAbsoluteFateAbility");
			bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			bp.AddComponent(delegate(ModifyD20 c)
			{
				c.Rule = RuleType.All;
				c.Replace = true;
				c.RollResult = 20;
			});
		});
		BlueprintAbility blueprintAbility2 = Miracle("WishstoneMiracleAbsoluteFateAbility", "Wishstone: Miracle of Absolute Fate", "Warp causality itself: all party members roll automatic Natural 20s on every d20 roll for 3 rounds!", delegate(BlueprintAbility bp)
		{
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionPartyMembers p)
				{
					p.Action = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = fateBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = 3
						}
					});
				});
			});
		});
		BlueprintAbility blueprintAbility3 = Miracle("WishstoneMiracleRuinAbility", "Wishstone: Miracle of Ruin", "Unleashes 350 unresistable divine force damage to all enemies in a 60-ft radius and dispels all enemy buffs with no saving throw or spell resistance.", delegate(BlueprintAbility bp)
		{
			bp.AddComponent(delegate(AbilityTargetsAround c)
			{
				c.m_Radius = 60.Feet();
				c.m_TargetType = TargetType.Enemy;
				c.m_Condition = ActionFlow.EmptyCondition();
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
				{
					m_Type = ContextActionDealDamage.Type.Damage,
					DamageType = new DamageTypeDescription
					{
						Type = DamageType.Direct,
						Common = new DamageTypeDescription.CommomData(),
						Physical = new DamageTypeDescription.PhysicalData()
					},
					Duration = Values.Duration.Zero,
					Value = new ContextDiceValue
					{
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 350
					}
				}, new ContextActionDispelMagic
				{
					OnlyTargetEnemyBuffs = true,
					OneRollForAll = true,
					CheckBonus = 100,
					m_MaxSpellLevel = 0,
					m_MaxCasterLevel = 0,
					m_CountToRemove = 1,
					ContextBonus = 0,
					OnSuccess = ActionFlow.DoNothing(),
					OnFail = ActionFlow.DoNothing()
				});
			});
		});
		BlueprintAbility blueprintAbility4 = Miracle("WishstoneMiracleWealthAbility", "Wishstone: Miracle of Wealth", "Manifests 500,000 Gold directly into your party treasury.", delegate(BlueprintAbility bp)
		{
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionWishstoneWealth());
			});
		});
		BlueprintAbility blueprintAbility5 = Miracle("WishstoneMiracleAkashicDuplicationAbility", "Wishstone: Miracle of Akashic Duplication", "Manifests scrolls of Mass Heal, Heroic Invocation, Overwhelming Presence, Tsunami, Weird, and Foresight directly into your pack.", delegate(BlueprintAbility bp)
		{
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionWishstoneAkashicDuplication());
			});
		});
		BlueprintAbility[] miracles = new BlueprintAbility[5] { blueprintAbility, blueprintAbility2, blueprintAbility3, blueprintAbility4, blueprintAbility5 };
		BlueprintAbility blueprintAbility6 = Miracle("WishstoneMasterAbility", "Invoke Bottled Djinn Wishstone", "Crush the bottled djinn wishstone to rewrite reality. Offers a choice of five miracles: Restoration, Absolute Fate (Nat 20s), Ruin (350 AOE Nuke), Wealth (500,000 Gold), or Akashic Duplication (9th-level Scrolls).", delegate(BlueprintAbility bp)
		{
			bp.AddComponent(delegate(AbilityVariants c)
			{
				c.m_Variants = Array.ConvertAll(miracles, (BlueprintAbility m) => m.ToReference<BlueprintAbilityReference>());
			});
		});
		BlueprintAbility[] array = miracles;
		for (int num = 0; num < array.Length; num++)
		{
			array[num].m_Parent = blueprintAbility6.ToReference<BlueprintAbilityReference>();
		}
		ItemCosmicWishstone = Bottle("ItemCosmicWishstone", "Bottled Djinn Wishstone", "An ancient crystalline vessel sealing a multiversal djinn of absolute cosmic authority. Can be equipped in quickslot by any party member. When activated, offers 5 reality-warping miracles: Full Team Restoration, 3 rounds of Natural 20s, 350 Divine Force AOE Nuke, 500,000 Gold, or 9th-level Scroll Duplication!", 4000, blueprintAbility6, UsableItemType.Other, 1f);
	}
}
