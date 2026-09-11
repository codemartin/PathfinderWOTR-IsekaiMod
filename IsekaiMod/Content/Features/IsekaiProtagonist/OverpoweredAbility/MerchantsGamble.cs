using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class MerchantsGamble
	{
		private const string Name = "Overpowered Ability - Otherworldly Gacha";

		private static readonly BlueprintItem GoldCoins = BlueprintTools.GetBlueprint<BlueprintItem>("f2bc0997c24e573448c6c91d2be88afa");

		public static void Add()
		{
			LocalizedString GachaDesc = Helpers.CreateString(Main.IsekaiContext, "MerchantsGamble.Description", "Tap into interdimensional gacha algorithms to manifest wealth, cosmic tokens, and legendary relics across the multiverse:\n• 3-Star (Common - 81.8%): 1,500 to 4,000 Gold plus vital consumables (Diamond Dust, Restoration scrolls, or Elixirs).\n• 4-Star (SR - 15.0%): 15,000 to 35,000 Gold, 50 to 150 Cosmic Coins, and powerful metamagic rods.\n• 5-Star (SSR - 3.0%): 100,000 to 250,000 Gold, 500 to 1,000 Cosmic Coins, legendary relics, and patron deity blessings.\n• 6-Star (Transcendent UR - 0.2% or 100-pull pity): 1,000,000 Gold, 5,000 Cosmic Coins, 5 Cosmic Wish charges, and 'The Omnipotent Sovereign's Die' (+4 sacred bonus to all d20 rolls, True Seeing, and 20% reality-bending advantage).\n");
			Sprite Icon_Merchants_Gamble = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_DUPE_GOLD.png");
			BlueprintAbilityResource resource = CreateResource();
			BlueprintFeature OmnipotentSovereignsDieFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "OmnipotentSovereignsDieFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "The Omnipotent Sovereign's Die");
				bp.SetDescription(Main.IsekaiContext, "The impossible 6-Star Transcendent UR prize of the Otherworldly Gacha. This twenty-sided geometric relic vibrates with the authority of infinite parallel realities. You gain a +4 sacred bonus to all d20 rolls (attack rolls, saving throws, skill checks), permanent True Seeing, and a 20% chance on any d20 roll to bend reality and roll twice, taking the better result.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Merchants_Gamble;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffAllSkillsBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Value = 4;
					c.Multiplier = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 1
					};
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.TrueSeeing;
				});
				bp.AddComponent(delegate(ModifyD20 c)
				{
					c.Rule = RuleType.All;
					c.TakeBest = true;
					c.RollsAmount = 1;
					c.WithChance = true;
					c.Chance = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 20
					};
					c.AddBonus = true;
					c.Bonus = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 4
					};
					c.BonusDescriptor = ModifierDescriptor.Sacred;
				});
			});
			BlueprintAbility MerchantsGambleAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MerchantsGambleAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Otherworldly Gacha");
				bp.SetDescription(GachaDesc);
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new CustomRollLogic
					{
						GoldCoins = GoldCoins.ToReference<BlueprintItemReference>(),
						OmnipotentDie = OmnipotentSovereignsDieFeature.ToReference<BlueprintFeatureReference>()
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_Merchants_Gamble;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.LocalizedDuration = StaticReferences.Strings.Null;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "MerchantsGambleFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Otherworldly Gacha");
				bp.SetDescription(GachaDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_Merchants_Gamble;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { MerchantsGambleAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = resource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			}));
		}

		private static BlueprintAbilityResource CreateResource()
		{
			return Helpers.CreateBlueprint(Main.IsekaiContext, "MerchantsGambleResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = true,
					m_Class = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_Archetypes = new BlueprintArchetypeReference[0],
					LevelIncrease = 1,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Charisma
				};
				bp.m_UseMax = false;
			});
		}
	}
}
