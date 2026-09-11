using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class ChronoDomain
	{
		private static readonly Sprite Icon_TimeStop = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98")).m_Icon;

		private static readonly BlueprintBuff StaggeredBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3");

		public static BlueprintFeature CosmicChronoDomainFeature;

		public static void Add()
		{
			BlueprintAbilityResource ChronoDomainResource = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoDomainResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 15,
					StartingIncrease = 1,
					LevelStep = 5,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 2;
			});
			BlueprintBuff TemporalStasisBuff = TTCoreExtensions.CreateBuff("TemporalStasisBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Temporal Stasis");
				bp.SetDescription(Main.IsekaiContext, "Frozen in time. The creature cannot take any actions, react, or defend itself for 2 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TimeStop;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Paralyzed;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.CantAct;
				});
				bp.m_Flags = BlueprintBuff.Flags.Harmful;
			});
			BlueprintUnitProperty ChronoDomainDCProperty = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoDomainDCProperty", delegate(BlueprintUnitProperty bp)
			{
				bp.name = "ChronoDomainDCProperty";
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
			BlueprintAbility ChronoDomainAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoDomainAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Chrono Domain (Time Stop)");
				bp.SetDescription(Main.IsekaiContext, "You declare dominion over the flow of time itself. As a swift action, force all enemies within 60 ft to make a Will saving throw (DC starts at 10 and scales with your level and Charisma modifier). On a failure, they are frozen in absolute temporal stasis for 2 rounds. On a success, they are staggered for 1 round by the temporal distortion. Usable once per day (gains a second use at level 15).");
				((BlueprintUnitFact)bp).m_Icon = Icon_TimeStop;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.LocalizedSavingThrow = Helpers.CreateString(Main.IsekaiContext, "ChronoDomain.SavingThrow", "Will negates stasis (staggered on save)");
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = 60.Feet();
					c.m_TargetType = TargetType.Enemy;
					c.m_IncludeDead = false;
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.Add10ToDC = false;
					c.DC = Values.CreateContextCasterCustomPropertyValue(ChronoDomainDCProperty);
					c.CasterLevel = -1;
					c.Concentration = -1;
					c.SpellLevel = 9;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.SavingThrowType = SavingThrowType.Will;
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSavingThrow s)
					{
						s.Type = SavingThrowType.Will;
						s.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0];
						s.Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved cs)
						{
							cs.Succeed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
							{
								b.m_Buff = StaggeredBuff.ToReference<BlueprintBuffReference>();
								b.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Rounds,
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
								b.m_Buff = TemporalStasisBuff.ToReference<BlueprintBuffReference>();
								b.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Rounds,
									DiceType = DiceType.Zero,
									BonusValue = new ContextValue
									{
										ValueType = ContextValueType.Simple,
										Value = 2
									}
								};
								b.IsNotDispelable = true;
							});
						});
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ChronoDomainResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoDomainFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Chrono Domain (Time Stop)");
				bp.SetDescription(Main.IsekaiContext, "Time is not a cage that binds you; it is a tapestry you can pause at will. \nBenefit: Grants the Chrono Domain ability (1/day, 2/day at level 15) to freeze or stagger all nearby enemies in time as a swift action.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TimeStop;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ChronoDomainAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = ChronoDomainResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
			BlueprintAbility CosmicChronoDomainAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicChronoDomainAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Chronos Absolute Stasis");
				bp.SetDescription(Main.IsekaiContext, "You declare absolute dominion over the flow of time itself. As a swift action, freeze all enemies within 60 ft in absolute temporal stasis for 2 rounds with NO saving throw and NO spell resistance. Affected foes cannot act or evade attacks. Usable once per day (gains a second use at level 15).");
				((BlueprintUnitFact)bp).m_Icon = Icon_TimeStop;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = 60.Feet();
					c.m_TargetType = TargetType.Enemy;
					c.m_IncludeDead = false;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
					{
						a.m_Buff = TemporalStasisBuff.ToReference<BlueprintBuffReference>();
						a.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 2
							}
						};
						a.IsNotDispelable = true;
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ChronoDomainResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			CosmicChronoDomainFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicChronoDomainFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chronos Absolute Stasis");
				bp.SetDescription(Main.IsekaiContext, "Time is not a cage that binds you; it is a tapestry you can pause at will.\nBenefit: Grants the Chronos Absolute Stasis ability (1/day, 2/day at level 15) to freeze all nearby enemies in time for 2 rounds as a swift action with no saving throw.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TimeStop;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { CosmicChronoDomainAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = ChronoDomainResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
		}
	}
}
