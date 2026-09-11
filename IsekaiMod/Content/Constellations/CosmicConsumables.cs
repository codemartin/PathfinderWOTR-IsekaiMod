using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
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

namespace IsekaiMod.Content.Constellations
{
	public static class CosmicConsumables
	{
		public static BlueprintItemEquipmentUsable ItemPrimordialAmbrosiaMight;

		public static BlueprintItemEquipmentUsable ItemPrimordialAmbrosiaGrace;

		public static BlueprintItemEquipmentUsable ItemPrimordialAmbrosiaEndurance;

		public static BlueprintItemEquipmentUsable ItemPrimordialAmbrosiaIntellect;

		public static BlueprintItemEquipmentUsable ItemPrimordialAmbrosiaInsight;

		public static BlueprintItemEquipmentUsable ItemPrimordialAmbrosiaMajesty;

		public static BlueprintItemEquipmentUsable ItemPrimordialAmbrosiaCelerity;

		public static BlueprintItemEquipmentUsable ItemPrimordialAmbrosiaEternalRenewal;

		public static BlueprintItemEquipmentUsable ItemNectarEndlessLife;

		public static BlueprintItemEquipmentUsable ItemNectarVoidStalker;

		public static BlueprintItemEquipmentUsable ItemNectarWorldBreaker;

		public static BlueprintItemEquipmentUsable ItemNectarDiamondSoul;

		public static BlueprintItemEquipmentUsable ItemNectarUnbrokenAegis;

		public static BlueprintItemEquipmentUsable ItemDraughtCosmicVelocity;

		public static BlueprintItemEquipmentUsable ItemDraughtImmortalTitan;

		public static BlueprintItemEquipmentUsable ItemDraughtArcaneOmniscience;

		public static BlueprintItemEquipmentUsable ItemDraughtApexPredator;

		public static BlueprintItemEquipmentUsable ItemDraughtAbsoluteSoulAnchor;

		public static BlueprintItemEquipmentUsable ItemCosmicWishstone;

		public static void Add()
		{
			Sprite iconCoin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			ItemPrimordialAmbrosiaMight = CreateStatAmbrosia("ItemPrimordialAmbrosiaMight", "Might", StatType.Strength);
			ItemPrimordialAmbrosiaGrace = CreateStatAmbrosia("ItemPrimordialAmbrosiaGrace", "Grace", StatType.Dexterity);
			ItemPrimordialAmbrosiaEndurance = CreateStatAmbrosia("ItemPrimordialAmbrosiaEndurance", "Endurance", StatType.Constitution);
			ItemPrimordialAmbrosiaIntellect = CreateStatAmbrosia("ItemPrimordialAmbrosiaIntellect", "Intellect", StatType.Intelligence);
			ItemPrimordialAmbrosiaInsight = CreateStatAmbrosia("ItemPrimordialAmbrosiaInsight", "Insight", StatType.Wisdom);
			ItemPrimordialAmbrosiaMajesty = CreateStatAmbrosia("ItemPrimordialAmbrosiaMajesty", "Majesty", StatType.Charisma);
			BlueprintFeature celerityFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemPrimordialAmbrosiaCelerityFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Primordial Ambrosia: Celerity");
				bp.SetDescription(Main.IsekaiContext, "Infused with primordial swiftness. Each bottle consumed permanently grants a +10 ft Untyped Stackable bonus to base movement speed.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Ranks = 999;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Multiplier = 10;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
					c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
				});
			});
			BlueprintAbility celerityAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemPrimordialAmbrosiaCelerityAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Primordial Ambrosia of Celerity");
				bp.SetDescription(Main.IsekaiContext, "Drink or administer this quicksilver nectar. Permanently increases base movement speed by +10 ft (Untyped Stackable). Stacks with Haste, Boots of Speed, and class abilities!");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionConsumeStackableConsumable
					{
						m_Feature = celerityFeature.ToReference<BlueprintFeatureReference>(),
						ItemName = "Primordial Ambrosia of Celerity",
						ValuePerRank = 10,
						StatDisplayName = "Base Speed"
					});
				});
			});
			ItemPrimordialAmbrosiaCelerity = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemPrimordialAmbrosiaCelerity", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Primordial Ambrosia of Celerity");
				bp.SetDescription(Main.IsekaiContext, "Quicksilver nectar distilled from shooting stars. Permanently grants +10 ft base movement speed per bottle. Untyped and infinitely stackable!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 3000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = celerityAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintFeature eternalRenewalFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemPrimordialAmbrosiaEternalRenewalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Primordial Ambrosia: Eternal Renewal");
				bp.SetDescription(Main.IsekaiContext, "Infused with immortal phoenix fire. Each bottle consumed permanently grants Fast Healing 5 (Untyped Stackable). Stacks infinitely with itself!");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Ranks = 999;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 0;
					c.Bonus = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
					c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
					c.m_Progression = ContextRankProgression.MultiplyByModifier;
					c.m_StepLevel = 5;
				});
			});
			BlueprintAbility eternalRenewalAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemPrimordialAmbrosiaEternalRenewalAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Primordial Ambrosia of Eternal Renewal");
				bp.SetDescription(Main.IsekaiContext, "Permanently grants Fast Healing 5 (Untyped Stackable). Stacks infinitely with itself (drink 2 = FH 10, drink 4 = FH 20, drink 10 = FH 50 every round)!");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionConsumeStackableConsumable
					{
						m_Feature = eternalRenewalFeature.ToReference<BlueprintFeatureReference>(),
						ItemName = "Primordial Ambrosia of Eternal Renewal",
						ValuePerRank = 5,
						StatDisplayName = "Fast Healing"
					});
				});
			});
			ItemPrimordialAmbrosiaEternalRenewal = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemPrimordialAmbrosiaEternalRenewal", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Primordial Ambrosia of Eternal Renewal");
				bp.SetDescription(Main.IsekaiContext, "The apex elixir of godly immortality. Permanently grants Fast Healing 5 per bottle consumed. Untyped, uncapped, and stacks with itself infinitely. The ultimate high-roller prestige trophy!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 10000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = eternalRenewalAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintFeature endlessLifeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarEndlessLifeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar: Endless Life");
				bp.SetDescription(Main.IsekaiContext, "Permanently grants +25 Maximum Hit Points per bottle consumed (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Ranks = 999;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Multiplier = 25;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
					c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
				});
			});
			BlueprintAbility endlessLifeAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarEndlessLifeAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Nectar of Endless Life");
				bp.SetDescription(Main.IsekaiContext, "Permanently increases Maximum Hit Points by +25 (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionConsumeStackableConsumable
					{
						m_Feature = endlessLifeFeature.ToReference<BlueprintFeatureReference>(),
						ItemName = "Nectar of Endless Life",
						ValuePerRank = 25,
						StatDisplayName = "Max HP"
					});
				});
			});
			ItemNectarEndlessLife = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarEndlessLife", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar of Endless Life");
				bp.SetDescription(Main.IsekaiContext, "Golden nectar teeming with primal vitality. Permanently grants +25 Max Hit Points per bottle. Untyped and infinitely stackable!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 1500;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = endlessLifeAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintFeature voidStalkerFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarVoidStalkerFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar: Void Stalker");
				bp.SetDescription(Main.IsekaiContext, "Permanently increases Spell DC by +1 and Spell Penetration by +2 per bottle consumed (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Ranks = 999;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 1;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
			});
			BlueprintAbility voidStalkerAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarVoidStalkerAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Nectar of the Void Stalker");
				bp.SetDescription(Main.IsekaiContext, "Permanently increases Spell DC by +1 and Spell Penetration by +2 (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionConsumeStackableConsumable
					{
						m_Feature = voidStalkerFeature.ToReference<BlueprintFeatureReference>(),
						ItemName = "Nectar of the Void Stalker",
						ValuePerRank = 1,
						StatDisplayName = "Spell DC (+1) & Spell Pen (+2)"
					});
				});
			});
			ItemNectarVoidStalker = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarVoidStalker", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar of the Void Stalker");
				bp.SetDescription(Main.IsekaiContext, "Deep violet nectar that attunes your soul to the void. Permanently grants +1 Spell DC and +2 Spell Penetration per bottle. Untyped and infinitely stackable!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 4000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = voidStalkerAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintFeature worldBreakerFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarWorldBreakerFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar: World Breaker");
				bp.SetDescription(Main.IsekaiContext, "Permanently increases Attack Bonus by +2 and Weapon Damage by +2 per bottle consumed (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Ranks = 999;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
					c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
				});
			});
			BlueprintAbility worldBreakerAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarWorldBreakerAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Nectar of the World Breaker");
				bp.SetDescription(Main.IsekaiContext, "Permanently increases Attack Bonus by +2 and Weapon Damage by +2 (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionConsumeStackableConsumable
					{
						m_Feature = worldBreakerFeature.ToReference<BlueprintFeatureReference>(),
						ItemName = "Nectar of the World Breaker",
						ValuePerRank = 2,
						StatDisplayName = "Attack & Damage"
					});
				});
			});
			ItemNectarWorldBreaker = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarWorldBreaker", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar of the World Breaker");
				bp.SetDescription(Main.IsekaiContext, "Crimson nectar brimming with tectonic wrath. Permanently grants +2 Attack Bonus and +2 Weapon Damage per bottle. Untyped and infinitely stackable!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 4000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = worldBreakerAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintFeature diamondSoulFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarDiamondSoulFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar: Diamond Soul");
				bp.SetDescription(Main.IsekaiContext, "Permanently grants a +2 Untyped Stackable bonus to all Saving Throws (Fortitude, Reflex, Will) per bottle consumed.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Ranks = 999;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
					c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
				});
			});
			BlueprintAbility diamondSoulAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarDiamondSoulAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Nectar of the Diamond Soul");
				bp.SetDescription(Main.IsekaiContext, "Permanently increases all Saving Throws by +2 (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionConsumeStackableConsumable
					{
						m_Feature = diamondSoulFeature.ToReference<BlueprintFeatureReference>(),
						ItemName = "Nectar of the Diamond Soul",
						ValuePerRank = 2,
						StatDisplayName = "All Saving Throws"
					});
				});
			});
			ItemNectarDiamondSoul = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarDiamondSoul", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar of the Diamond Soul");
				bp.SetDescription(Main.IsekaiContext, "Pristine crystalline nectar that hardens the spirit into unbreakable diamond. Permanently grants +2 to all Saving Throws per bottle. Untyped and infinitely stackable!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 3500;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = diamondSoulAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintFeature unbrokenAegisFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarUnbrokenAegisFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar: Unbroken Aegis");
				bp.SetDescription(Main.IsekaiContext, "Permanently grants a +1 Untyped Stackable bonus to Armor Class per bottle consumed.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Ranks = 999;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AC;
					c.Multiplier = 1;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
					c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
				});
			});
			BlueprintAbility unbrokenAegisAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarUnbrokenAegisAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Nectar of the Unbroken Aegis");
				bp.SetDescription(Main.IsekaiContext, "Permanently increases Armor Class by +1 (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionConsumeStackableConsumable
					{
						m_Feature = unbrokenAegisFeature.ToReference<BlueprintFeatureReference>(),
						ItemName = "Nectar of the Unbroken Aegis",
						ValuePerRank = 1,
						StatDisplayName = "Armor Class"
					});
				});
			});
			ItemNectarUnbrokenAegis = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNectarUnbrokenAegis", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Nectar of the Unbroken Aegis");
				bp.SetDescription(Main.IsekaiContext, "Dense platinum nectar that crystallizes across your skin. Permanently grants +1 Armor Class per bottle. Untyped and infinitely stackable!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 3500;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = unbrokenAegisAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintBuff velocityBuff = TTCoreExtensions.CreateBuff("DraughtCosmicVelocityBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of Cosmic Velocity");
				bp.SetDescription(Main.IsekaiContext, "Grants a +20 ft Untyped Stackable bonus to movement speed and +1 extra attack on a full attack. Stacks with Haste and all equipment.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
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
			BlueprintAbility velocityDraughtAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtCosmicVelocityAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Draught of Cosmic Velocity");
				bp.SetDescription(Main.IsekaiContext, "Grants +20 ft movement speed and +1 extra attack on full attack for 24 hours (Untyped Stackable).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = velocityBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Hours,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 24
							}
						},
						Permanent = false
					});
				});
			});
			ItemDraughtCosmicVelocity = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtCosmicVelocity", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of Cosmic Velocity");
				bp.SetDescription(Main.IsekaiContext, "A potent 24-hour combat stimulant. Grants an untyped +20 ft speed and +1 extra attack on full attack for 24 hours. Stacks directly with Haste!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 2500;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = velocityDraughtAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintBuff titanBuff = TTCoreExtensions.CreateBuff("DraughtImmortalTitanBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of the Immortal Titan");
				bp.SetDescription(Main.IsekaiContext, "Grants +150 Temporary Hit Points, DR 10/--, and Fast Healing 10 for 24 hours.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
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
			BlueprintAbility titanAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtImmortalTitanAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Draught of the Immortal Titan");
				bp.SetDescription(Main.IsekaiContext, "Grants +150 Temporary Hit Points, DR 10/--, and Fast Healing 10 for 24 hours.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = titanBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Hours,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 24
							}
						},
						Permanent = false
					});
				});
			});
			ItemDraughtImmortalTitan = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtImmortalTitan", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of the Immortal Titan");
				bp.SetDescription(Main.IsekaiContext, "Titan blood infused with adamant wards. Grants +150 Temp HP, DR 10/--, and Fast Healing 10 for 24 hours.");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 2500;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = titanAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintBuff arcaneBuff = TTCoreExtensions.CreateBuff("DraughtArcaneOmniscienceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of Arcane Omniscience");
				bp.SetDescription(Main.IsekaiContext, "Grants a +4 bonus to Caster Level, and all spells are automatically cast as though affected by Extend Spell and Empower Spell without raising spell level.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
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
			BlueprintAbility arcaneAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtArcaneOmniscienceAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Draught of Arcane Omniscience");
				bp.SetDescription(Main.IsekaiContext, "Grants +4 Caster Level check bonus and automatically extends and empowers all spells for 24 hours.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = arcaneBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Hours,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 24
							}
						},
						Permanent = false
					});
				});
			});
			ItemDraughtArcaneOmniscience = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtArcaneOmniscience", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of Arcane Omniscience");
				bp.SetDescription(Main.IsekaiContext, "Liquid ley-line mana. Grants +4 Spell Penetration/CL bonus and auto-extends and auto-empowers all cast spells for 24 hours.");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 3000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = arcaneAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintBuff apexBuff = TTCoreExtensions.CreateBuff("DraughtApexPredatorBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of the Apex Predator");
				bp.SetDescription(Main.IsekaiContext, "Increases critical multiplier by +1 and causes all weapon attacks to completely bypass Damage Reduction and Concealment for 24 hours.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Value = 6;
				});
				bp.AddComponent<IgnoreConcealment>();
				bp.AddComponent<IgnoreDamageReductionOnAttack>();
			});
			BlueprintAbility apexAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtApexPredatorAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Draught of the Apex Predator");
				bp.SetDescription(Main.IsekaiContext, "Grants +6 critical confirmation, bypasses all Damage Reduction, and ignores concealment for 24 hours.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = apexBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Hours,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 24
							}
						},
						Permanent = false
					});
				});
			});
			ItemDraughtApexPredator = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtApexPredator", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of the Apex Predator");
				bp.SetDescription(Main.IsekaiContext, "Primal predator adrenaline. Enhances critical confirmations by +6 and bypasses all DR and concealment for 24 hours.");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 3000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = apexAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintBuff soulAnchorBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "DraughtOfAbsoluteSoulAnchorBuff");
			BlueprintAbility soulAnchorAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtAbsoluteSoulAnchorAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drink Draught of Absolute Soul Anchor");
				bp.SetDescription(Main.IsekaiContext, "Immunity to death effects, negative levels, energy drain, and compulsion for 24 hours.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetSelf = true;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					if (soulAnchorBuff != null)
					{
						c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
						{
							m_Buff = soulAnchorBuff.ToReference<BlueprintBuffReference>(),
							DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Hours,
								DiceType = DiceType.Zero,
								BonusValue = new ContextValue
								{
									ValueType = ContextValueType.Simple,
									Value = 24
								}
							},
							Permanent = false
						});
					}
				});
			});
			ItemDraughtAbsoluteSoulAnchor = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemDraughtAbsoluteSoulAnchor", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Draught of Absolute Soul Anchor");
				bp.SetDescription(Main.IsekaiContext, "Anchors your soul to the outer cosmos. Grants complete immunity to death effects, negative levels, and mental compulsion for 24 hours.");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 500;
				((BlueprintItem)bp).m_Weight = 0.5f;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = soulAnchorAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintAbility wishRestoration = Helpers.CreateBlueprint(Main.IsekaiContext, "WishstoneMiracleRestorationAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wishstone: Miracle of Restoration");
				bp.SetDescription(Main.IsekaiContext, "True Resurrection on all fallen party members at 100% HP, restores all spell slots and class resources to maximum, and cleanses all conditions and Abyssal Corruption.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionWishstoneRestoration());
				});
			});
			BlueprintBuff fateBuff = TTCoreExtensions.CreateBuff("WishstoneAbsoluteFateBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Miracle of Absolute Fate");
				bp.SetDescription(Main.IsekaiContext, "All attack rolls, saving throws, and skill checks automatically result in Natural 20s for 3 rounds!");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(ModifyD20 c)
				{
					c.Rule = RuleType.All;
					c.Replace = true;
					c.RollResult = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 20
					};
				});
			});
			BlueprintAbility wishFate = Helpers.CreateBlueprint(Main.IsekaiContext, "WishstoneMiracleAbsoluteFateAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wishstone: Miracle of Absolute Fate");
				bp.SetDescription(Main.IsekaiContext, "Warp causality itself: all party members roll automatic Natural 20s on every d20 roll for 3 rounds!");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = fateBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 3
							}
						},
						Permanent = false
					});
				});
			});
			BlueprintAbility wishRuin = Helpers.CreateBlueprint(Main.IsekaiContext, "WishstoneMiracleRuinAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wishstone: Miracle of Ruin");
				bp.SetDescription(Main.IsekaiContext, "Unleashes 350 unresistable divine force damage to all enemies in a 60-ft radius and dispels all enemy buffs with no saving throw or spell resistance.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
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
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Direct,
							Common = new DamageTypeDescription.CommomData(),
							Physical = new DamageTypeDescription.PhysicalData()
						},
						Duration = new ContextDurationValue(),
						Value = new ContextDiceValue
						{
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 350
							}
						}
					}, new ContextActionDispelMagic
					{
						OnlyTargetEnemyBuffs = true,
						OneRollForAll = true,
						CheckBonus = 100
					});
				});
			});
			BlueprintAbility wishWealth = Helpers.CreateBlueprint(Main.IsekaiContext, "WishstoneMiracleWealthAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wishstone: Miracle of Wealth");
				bp.SetDescription(Main.IsekaiContext, "Manifests 500,000 Gold directly into your party treasury.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionWishstoneWealth());
				});
			});
			BlueprintAbility wishAkashic = Helpers.CreateBlueprint(Main.IsekaiContext, "WishstoneMiracleAkashicDuplicationAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wishstone: Miracle of Akashic Duplication");
				bp.SetDescription(Main.IsekaiContext, "Manifests scrolls of Mass Heal, Heroic Invocation, Overwhelming Presence, Tsunami, Weird, and Foresight directly into your pack.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionWishstoneAkashicDuplication());
				});
			});
			BlueprintAbility wishstoneMasterAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "WishstoneMasterAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Invoke Bottled Djinn Wishstone");
				bp.SetDescription(Main.IsekaiContext, "Crush the bottled djinn wishstone to rewrite reality. Offers a choice of five miracles: Restoration, Absolute Fate (Nat 20s), Ruin (350 AOE Nuke), Wealth (500,000 Gold), or Akashic Duplication (9th-level Scrolls).");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityVariants c)
				{
					c.m_Variants = new BlueprintAbilityReference[5]
					{
						wishRestoration.ToReference<BlueprintAbilityReference>(),
						wishFate.ToReference<BlueprintAbilityReference>(),
						wishRuin.ToReference<BlueprintAbilityReference>(),
						wishWealth.ToReference<BlueprintAbilityReference>(),
						wishAkashic.ToReference<BlueprintAbilityReference>()
					};
				});
			});
			ItemCosmicWishstone = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemCosmicWishstone", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Bottled Djinn Wishstone");
				bp.SetDescription(Main.IsekaiContext, "An ancient crystalline vessel sealing a multiversal djinn of absolute cosmic authority. Can be equipped in quickslot by any party member. When activated, offers 5 reality-warping miracles: Full Team Restoration, 3 rounds of Natural 20s, 350 Divine Force AOE Nuke, 500,000 Gold, or 9th-level Scroll Duplication!");
				((BlueprintItem)bp).m_Icon = iconCoin;
				((BlueprintItem)bp).m_Cost = 4000;
				((BlueprintItem)bp).m_Weight = 1f;
				bp.Type = UsableItemType.Other;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = wishstoneMasterAbility.ToReference<BlueprintAbilityReference>();
			});
			BlueprintItemEquipmentUsable CreateStatAmbrosia(string id, string statName, StatType stat, int cost = 3000)
			{
				BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, id + "Feature", delegate(BlueprintFeature bp)
				{
					bp.SetName(Main.IsekaiContext, "Primordial Ambrosia: " + statName);
					bp.SetDescription(Main.IsekaiContext, "Infused with untyped primordial divine essence. Each bottle consumed permanently grants a +2 Untyped Stackable bonus to " + statName + ".");
					((BlueprintUnitFact)bp).m_Icon = iconCoin;
					bp.Ranks = 999;
					bp.AddComponent(delegate(AddContextStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = stat;
						c.Multiplier = 2;
						c.Value = new ContextValue
						{
							ValueType = ContextValueType.Rank,
							ValueRank = AbilityRankType.Default
						};
					});
					bp.AddComponent(delegate(ContextRankConfig c)
					{
						c.m_Type = AbilityRankType.Default;
						c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
						c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
					});
				});
				BlueprintAbility ability = Helpers.CreateBlueprint(Main.IsekaiContext, id + "Ability", delegate(BlueprintAbility bp)
				{
					bp.SetName(Main.IsekaiContext, "Drink Primordial Ambrosia of " + statName);
					bp.SetDescription(Main.IsekaiContext, "Drink or administer this shimmering nectar. Permanently increases " + statName + " by +2 (Untyped Stackable). Stacks infinitely with itself, gear, and all other bonuses!");
					((BlueprintUnitFact)bp).m_Icon = iconCoin;
					bp.Type = AbilityType.Special;
					bp.Range = AbilityRange.Touch;
					bp.CanTargetSelf = true;
					bp.CanTargetFriends = true;
					bp.ActionType = UnitCommand.CommandType.Standard;
					bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Potion;
					bp.AddComponent(delegate(AbilityEffectRunAction c)
					{
						c.Actions = Helpers.CreateActionList(new ContextActionConsumeStackableConsumable
						{
							m_Feature = feature.ToReference<BlueprintFeatureReference>(),
							ItemName = "Primordial Ambrosia of " + statName,
							ValuePerRank = 2,
							StatDisplayName = statName
						});
					});
				});
				return Helpers.CreateBlueprint(Main.IsekaiContext, id, delegate(BlueprintItemEquipmentUsable bp)
				{
					bp.SetName(Main.IsekaiContext, "Primordial Ambrosia of " + statName);
					bp.SetDescription(Main.IsekaiContext, "A crystallized vial of starlight nectar brewed from the tears of the Outer Gods. Permanently increases " + statName + " by +2. It is an untyped bonus with no upper limit--the only limit is how many coins you spend!");
					((BlueprintItem)bp).m_Icon = iconCoin;
					((BlueprintItem)bp).m_Cost = cost;
					((BlueprintItem)bp).m_Weight = 0.5f;
					bp.Type = UsableItemType.Potion;
					bp.SpendCharges = true;
					bp.Charges = 1;
					bp.RestoreChargesOnRest = false;
					((BlueprintItemEquipment)bp).m_Ability = ability.ToReference<BlueprintAbilityReference>();
				});
			}
		}
	}
}
