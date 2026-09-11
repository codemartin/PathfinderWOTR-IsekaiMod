using IsekaiMod.Content.Classes.Deathsnatcher;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.Deathsnatcher
{
	internal class DeathsnatcherCommandUndead
	{
		private static readonly BlueprintAbility CommandUndeadAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("0b101dd5618591e478f825f0eef155b4");

		private static readonly BlueprintBuff CommandUndeadIntelligentBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("07f4f8d2000a91c459c23c7fff8c74fb");

		private static readonly BlueprintBuff CommandUndeadBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("7cd727ddd4cc4be498720e45f0c1f6f4");

		private static readonly BlueprintFeature UndeadType = BlueprintTools.GetBlueprint<BlueprintFeature>("734a29b693e9ec346ba2951b27987e33");

		public static void Add()
		{
			BlueprintAbilityResource DeathsnatcherCommandUndeadResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherCommandUndeadResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 10,
					IncreasedByLevel = false,
					LevelIncrease = 1,
					IncreasedByLevelStartPlusDivStep = false,
					StartingLevel = 0,
					StartingIncrease = 0,
					LevelStep = 0,
					PerStepIncrease = 0,
					MinClassLevelIncrease = 0,
					OtherClassesModifier = 0f,
					IncreasedByStat = false,
					ResourceBonusStat = StatType.Unknown
				};
			});
			BlueprintAbility DeathsnatcherCommandUndeadAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherCommandUndeadAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(((BlueprintUnitFact)CommandUndeadAbility).m_DisplayName);
				bp.SetDescription(((BlueprintUnitFact)CommandUndeadAbility).m_Description);
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)CommandUndeadAbility).m_Icon;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.SavingThrowType = SavingThrowType.Will;
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved contextActionConditionalSaved)
					{
						contextActionConditionalSaved.Succeed = ActionFlow.DoNothing();
						contextActionConditionalSaved.Failed = ActionFlow.DoSingle(delegate(Conditional conditional)
						{
							conditional.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionStatValue contextConditionStatValue)
							{
								contextConditionStatValue.Not = false;
								contextConditionStatValue.N = 1;
								contextConditionStatValue.Stat = StatType.Intelligence;
							});
							conditional.IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
							{
								contextActionApplyBuff.m_Buff = CommandUndeadIntelligentBuff.ToReference<BlueprintBuffReference>();
								contextActionApplyBuff.Permanent = false;
								contextActionApplyBuff.UseDurationSeconds = false;
								contextActionApplyBuff.DurationSeconds = 0f;
								contextActionApplyBuff.IsFromSpell = true;
								contextActionApplyBuff.IsNotDispelable = false;
								contextActionApplyBuff.ToCaster = false;
								contextActionApplyBuff.AsChild = false;
								contextActionApplyBuff.SameDuration = false;
								contextActionApplyBuff.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Rounds,
									DiceType = DiceType.Zero,
									DiceCountValue = 0,
									BonusValue = Values.CreateContextRankValue(AbilityRankType.DamageBonus),
									m_IsExtendable = true
								};
							});
							conditional.IfFalse = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
							{
								contextActionApplyBuff.m_Buff = CommandUndeadBuff.ToReference<BlueprintBuffReference>();
								contextActionApplyBuff.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Rounds,
									DiceType = DiceType.Zero,
									DiceCountValue = 0,
									BonusValue = Values.CreateContextRankValue(AbilityRankType.DamageBonus),
									m_IsExtendable = true
								};
								contextActionApplyBuff.IsFromSpell = true;
							});
						});
					});
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Necromancy;
				});
				bp.AddComponent(delegate(SpellDescriptorComponent c)
				{
					c.Descriptor = SpellDescriptor.UndeadControl;
				});
				bp.AddComponent(delegate(AbilitySpawnFx c)
				{
					c.PrefabLink = new PrefabLink
					{
						AssetId = "cbfe312cb8e63e240a859efaad8e467c"
					};
					c.Time = AbilitySpawnFxTime.OnApplyEffect;
					c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
				});
				bp.AddComponent(delegate(AbilityTargetHasFact c)
				{
					c.m_CheckedFacts = new BlueprintUnitFactReference[1] { UndeadType.ToReference<BlueprintUnitFactReference>() };
					c.Inverted = false;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 1;
					c.m_Class = new BlueprintCharacterClassReference[1] { DeathsnatcherClass.GetReference() };
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = DeathsnatcherCommandUndeadResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
				});
				bp.Type = AbilityType.SpellLike;
				bp.Range = AbilityRange.Close;
				bp.CanTargetEnemies = true;
				bp.SpellResistance = true;
				bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
				bp.EffectOnAlly = AbilityEffectOnUnit.None;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = CommandUndeadAbility.AvailableMetamagic;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = StaticReferences.Strings.SavingThrow.WillNegatesSaveEachRound;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherCommandUndeadFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(((BlueprintUnitFact)CommandUndeadAbility).m_DisplayName);
				bp.SetDescription(Main.IsekaiContext, "At 1st level, the Deathsnatcher gains Command Undead as a spell-like ability 10 times per day.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)CommandUndeadAbility).m_Icon;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = DeathsnatcherCommandUndeadResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { DeathsnatcherCommandUndeadAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
