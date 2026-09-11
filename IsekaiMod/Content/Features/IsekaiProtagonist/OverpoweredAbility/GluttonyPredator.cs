using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class GluttonyPredator
	{
		private static readonly Sprite Icon_Gluttony = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("52b5df2a97df18242aec67610616ded0")).m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource PredatorMawResource = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorMawResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 5,
					StartingIncrease = 1,
					LevelStep = 5,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 6;
			});
			BlueprintBuff PredatorResonanceBuff = TTCoreExtensions.CreateBuff("PredatorResonanceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator's Essence Absorption");
				bp.SetDescription(Main.IsekaiContext, "You have devoured the essence of a worthy foe. Grants +2 profane bonus to Attack and Damage rolls, +10 energy resistance to all elements, and Fast Healing 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Gluttony;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
				});
			});
			BlueprintAbility PredatorMawAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorMawAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Predator's Maw");
				bp.SetDescription(Main.IsekaiContext, "Swift action bite against an adjacent enemy dealing 1d6 per character level divine/unholy damage. If the strike slays the target, you devour their essence: instantly restores 20% max HP, refreshes all lower spell slots, and grants Predator's Essence Absorption for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Gluttony;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetEnemies = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Energy,
							Energy = DamageEnergyType.Unholy
						},
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
							BonusValue = 0
						},
						HalfIfSaved = false,
						IsAoE = false
					}, new ContextActionApplyBuff
					{
						m_Buff = PredatorResonanceBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Minutes,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 1
							}
						}
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = PredatorMawResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "GluttonyPredatorFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Gluttony (Predator)");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to the Slime archetype. Like a primordial slime from another dimension, your body can ingest and process the spiritual essence of anything you slay. \nBenefit: Grants the swift-action Predator's Maw ability (3/day) to deal devastating unholy bite damage and absorb enemy essence.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Gluttony;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { PredatorMawAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = PredatorMawResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
				bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.m_Archetype = DevourerArchetype.GetReference();
					c.Level = 1;
				});
			}));
		}
	}
}
