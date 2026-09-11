using System;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
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
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class AbsoluteDefense
	{
		private static readonly Sprite Icon_AbsoluteDefense = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("62888999171921e4dafb46de83f4d67d"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource AbsoluteDefenseResource = Helpers.CreateBlueprint(Main.IsekaiContext, "AbsoluteDefenseResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 10,
					StartingIncrease = 1,
					LevelStep = 5,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 4;
			});
			BlueprintBuff AbsoluteDefenseBuff = TTCoreExtensions.CreateBuff("AbsoluteDefenseBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Defense");
				bp.SetDescription(Main.IsekaiContext, "For 1 round, you are invulnerable to mortal harm. Grants DR 100/-, immunity to all energy damage, and a +20 shield bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = Icon_AbsoluteDefense;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 100;
				});
				foreach (DamageEnergyType energyType in Enum.GetValues(typeof(DamageEnergyType)))
				{
					bp.AddComponent(delegate(AddDamageResistanceEnergy c)
					{
						c.Type = energyType;
						c.Value = 100;
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 20;
				});
			});
			BlueprintAbility AbsoluteDefenseAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "AbsoluteDefenseAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Absolute Defense");
				bp.SetDescription(Main.IsekaiContext, "Channeling the ultimate defensive technique from another realm, you create an impenetrable barrier around yourself for 1 round. \nBenefit: As a swift action, grant yourself DR 100/-, 100 resistance to all energy damage types, and +20 shield AC for 1 round. Usable 1 time per day (scaling up to 4 times per day by level 20).");
				((BlueprintUnitFact)bp).m_Icon = Icon_AbsoluteDefense;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRound;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = AbsoluteDefenseBuff.ToReference<BlueprintBuffReference>();
						b.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							BonusValue = 1
						};
						b.Permanent = false;
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = AbsoluteDefenseResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AbsoluteDefenseFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Absolute Defense");
				bp.SetDescription(Main.IsekaiContext, "Like the Legendary Shield Hero, your defenses cannot be breached by conventional attacks. \nBenefit: Gain the swift-action Absolute Defense ability, granting impenetrable DR 100/-, energy immunity, and +20 shield AC for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_AbsoluteDefense;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { AbsoluteDefenseAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = AbsoluteDefenseResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
