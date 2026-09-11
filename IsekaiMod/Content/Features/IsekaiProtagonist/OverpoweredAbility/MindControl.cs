using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class MindControl
	{
		public static void Add()
		{
			Sprite Icon_MindControl = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_MIND_CONTROL.png");
			Sprite Icon_MindControlImmune = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_MIND_CONTROL_IMMUNE.png");
			LocalizedString MindControlDesc = Helpers.CreateString(Main.IsekaiContext, "MindControl.Description", "Behold the power of a king! All will listen and obey!\nBenefit: You can make any creature fight on your side as if it was your ally if it fails a Will saving throw (DC starts at 10 and scales with your level and Charisma modifier). It will {g|Encyclopedia:Attack}attack{/g} your opponents to the best of its ability.\nOnce the domination effect ends or if the target successfully saves, it gains immunity to Mind Control for 1 minute.\nYou gain 3 uses per day at level 1, increasing up to 7 uses per day by level 20.");
			BlueprintBuff MindControlImmunity = TTCoreExtensions.CreateBuff("MindControlImmunity", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Mind Control Immunity");
				bp.SetDescription(Main.IsekaiContext, "This creature cannot be mind-controlled again for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_MindControlImmune;
				bp.AddComponent<IsPositiveEffect>();
			});
			BlueprintBuff MindControlBuff = TTCoreExtensions.CreateBuff("MindControlBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Mind Controlled");
				bp.SetDescription(Main.IsekaiContext, "This creature has been mind-controlled.");
				((BlueprintUnitFact)bp).m_Icon = Icon_MindControl;
				bp.AddComponent(delegate(ChangeFaction c)
				{
					c.m_Type = ChangeFaction.ChangeType.ToCaster;
				});
				bp.AddComponent(delegate(AddFactContextActions c)
				{
					c.Deactivated = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = MindControlImmunity.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Minutes,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 1
							}
						};
					});
				});
				bp.Stacking = StackingType.Ignore;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			});
			BlueprintAbilityResource MindControlResource = Helpers.CreateBlueprint(Main.IsekaiContext, "MindControlResource", delegate(BlueprintAbilityResource resource)
			{
				resource.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevelStartPlusDivStep = true,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					StartingLevel = 5,
					StartingIncrease = 1,
					LevelStep = 4,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					OtherClassesModifier = 0f
				};
				resource.m_UseMax = true;
				resource.m_Max = 7;
			});
			BlueprintUnitProperty MindControlDCProperty = Helpers.CreateBlueprint(Main.IsekaiContext, "MindControlDCProperty", delegate(BlueprintUnitProperty bp)
			{
				bp.name = "MindControlDCProperty";
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.Level;
				});
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.StatBonusCharisma;
				});
				bp.BaseValue = 10;
				bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
			});
			BlueprintAbility MindControlAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MindControlAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Mind Control");
				bp.SetDescription(MindControlDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_MindControl;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.SavingThrowType = SavingThrowType.Will;
					c.Actions = ActionFlow.DoSingle(delegate(Conditional conditional)
					{
						conditional.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionHasFact contextConditionHasFact)
						{
							contextConditionHasFact.m_Fact = MindControlImmunity.ToReference<BlueprintUnitFactReference>();
						});
						conditional.IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
						{
							contextActionApplyBuff.m_Buff = MindControlImmunity.ToReference<BlueprintBuffReference>();
							contextActionApplyBuff.DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Minutes,
								DiceType = DiceType.Zero,
								BonusValue = new ContextValue
								{
									ValueType = ContextValueType.Simple,
									Value = 1
								}
							};
						});
						conditional.IfFalse = ActionFlow.DoSingle(delegate(ContextActionSavingThrow s)
						{
							s.Type = SavingThrowType.Will;
							s.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0];
							s.Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved cs)
							{
								cs.Succeed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
								{
									b.m_Buff = MindControlImmunity.ToReference<BlueprintBuffReference>();
									b.DurationValue = new ContextDurationValue
									{
										Rate = DurationRate.Minutes,
										DiceType = DiceType.Zero,
										BonusValue = new ContextValue
										{
											ValueType = ContextValueType.Simple,
											Value = 1
										}
									};
								});
								cs.Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
								{
									b.m_Buff = MindControlBuff.ToReference<BlueprintBuffReference>();
									b.DurationValue = new ContextDurationValue
									{
										Rate = DurationRate.Rounds,
										DiceType = DiceType.Zero,
										DiceCountValue = 0,
										BonusValue = new ContextValue
										{
											ValueType = ContextValueType.Rank
										}
									};
								});
							});
						});
					});
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Enchantment;
				});
				bp.AddComponent(delegate(SpellDescriptorComponent c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion;
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.Add10ToDC = false;
					c.DC = Values.CreateContextCasterCustomPropertyValue(MindControlDCProperty);
					c.CasterLevel = -1;
					c.Concentration = -1;
					c.SpellLevel = 9;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = MindControlResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Medium;
				bp.CanTargetEnemies = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = Helpers.CreateString(Main.IsekaiContext, "MindControl.SavingThrow", "Will negates");
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "MindControlFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Mind Control");
				bp.SetDescription(MindControlDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_MindControl;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { MindControlAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = MindControlResource.ToReference<BlueprintAbilityResourceReference>();
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
