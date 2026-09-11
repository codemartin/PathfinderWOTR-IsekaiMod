using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
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
	internal class WorldBreak
	{
		private static readonly Sprite Icon_WorldBreak = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e4450dd9c06dc034fb7c0c08abcc202b"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource WorldBreakResource = Helpers.CreateBlueprint(Main.IsekaiContext, "WorldBreakResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevelStartPlusDivStep = false,
					StartingLevel = 0,
					StartingIncrease = 0,
					LevelStep = 0,
					PerStepIncrease = 0,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[0],
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 3;
			});
			BlueprintAbility WorldBreakAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "WorldBreakAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - World Break");
				bp.SetDescription(Main.IsekaiContext, "Inspired by the ultimate technique of the World Champion, you cleave through the very fabric of space and time. \nBenefit: As a swift action, strike a target within melee reach to deal irresistible spatial force damage equal to 10 times your character level. This damage ignores all damage reduction, hardness, and concealment. Usable 3 times per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_WorldBreak;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetEnemies = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Reach;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.Instantaneous;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionDealDamage d)
					{
						d.DamageType = new DamageTypeDescription
						{
							Type = DamageType.Force,
							Common = new DamageTypeDescription.CommomData(),
							Physical = new DamageTypeDescription.PhysicalData()
						};
						d.Duration = Values.Duration.Zero;
						d.Value = new ContextDiceValue
						{
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = Values.CreateContextRankValue(AbilityRankType.DamageBonus)
						};
						d.IgnoreCritical = true;
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.DamageBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.MultiplyByModifier;
					c.m_StepLevel = 10;
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = WorldBreakResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "WorldBreakFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - World Break");
				bp.SetDescription(Main.IsekaiContext, "The pinnacle of martial reality manipulation. As a swift action, unleash World Break to deal 10 x Character Level irresistible force damage to a target within touch reach (3/day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_WorldBreak;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { WorldBreakAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = WorldBreakResource.ToReference<BlueprintAbilityResourceReference>();
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
