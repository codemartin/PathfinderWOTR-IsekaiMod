using IsekaiMod.Components;
using IsekaiMod.Content.Classes.Deathsnatcher;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.Deathsnatcher;

internal class DeathsnatcherAnimateDead
{
	private static readonly BlueprintAbility AnimateDeadAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("4b76d32feb089ad4499c3a1ce8e1ac27");

	private static readonly BlueprintUnit SummonedSkeletonChampion = BlueprintTools.GetBlueprint<BlueprintUnit>("53e228ba7fe18104c93dc4b7294a1b30");

	private static readonly BlueprintBuff SummonedCreatureSpawnMonsterIV_VI = BlueprintTools.GetBlueprint<BlueprintBuff>("50d51854cf6a3434d96a87d050e1d09a");

	public static void Add()
	{
		BlueprintSummonPool DeathsnatcherUndeadPool = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherUndeadPool", delegate(BlueprintSummonPool bp)
		{
			bp.Limit = 6;
			bp.DoNotRemoveDeadUnits = false;
		});
		BlueprintAbilityResource DeathsnatcherAnimateDeadResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherAnimateDeadResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
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
		BlueprintAbility DeathsnatcherAnimateDeadAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherAnimateDeadAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(((BlueprintUnitFact)AnimateDeadAbility).m_DisplayName);
			bp.SetDescription(((BlueprintUnitFact)AnimateDeadAbility).m_Description);
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)AnimateDeadAbility).m_Icon;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				ContextActionSpawnMonster contextActionSpawnMonster = new ContextActionSpawnMonster
				{
					m_Blueprint = SummonedSkeletonChampion.ToReference<BlueprintUnitReference>(),
					m_SummonPool = DeathsnatcherUndeadPool.ToReference<BlueprintSummonPoolReference>(),
					UseLimitFromSummonPool = true,
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
					},
					CountValue = new ContextDiceValue
					{
						DiceType = DiceType.D4,
						DiceCountValue = 1,
						BonusValue = 2
					},
					LevelValue = 0,
					AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
					{
						a.Permanent = true;
						a.m_Buff = SummonedCreatureSpawnMonsterIV_VI.ToReference<BlueprintBuffReference>();
						a.DurationValue = Values.Duration.Zero;
						a.IsNotDispelable = true;
					})
				};
				c.Actions = Helpers.CreateActionList(contextActionSpawnMonster, new ContextActionEnforceShadowExtractionCap
				{
					m_SummonPool = DeathsnatcherUndeadPool.ToReference<BlueprintSummonPoolReference>(),
					m_ShadowUnits = new BlueprintUnitReference[1] { SummonedSkeletonChampion.ToReference<BlueprintUnitReference>() },
					FixedCap = 6,
					AfterSpawn = true
				});
			});
			bp.AddComponent(delegate(SpellComponent c)
			{
				((BlueprintComponent)c).m_Flags = (BlueprintComponent.Flags)0;
				c.School = SpellSchool.Necromancy;
			});
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.Evil;
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
				c.m_RequiredResource = DeathsnatcherAnimateDeadResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
			});
			bp.Type = AbilityType.SpellLike;
			bp.Range = AbilityRange.Close;
			bp.CanTargetPoint = true;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AvailableMetamagic = AnimateDeadAbility.AvailableMetamagic;
			bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
			bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherAnimateDeadFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(((BlueprintUnitFact)AnimateDeadAbility).m_DisplayName);
			bp.SetDescription(Main.IsekaiContext, "At 7th level, the Deathsnatcher gains Animate Dead as a spell-like ability 3 times per day. It can have at most 6 of its own skeletons raised at once.");
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)AnimateDeadAbility).m_Icon;
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = DeathsnatcherAnimateDeadResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { DeathsnatcherAnimateDeadAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherAnimateDeadAdditionalUse", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Animate Dead - Additional Uses");
			bp.SetDescription(Main.IsekaiContext, "At 10th level, the Deathsnatcher gains 2 additional uses of Animate Dead per day.");
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)AnimateDeadAbility).m_Icon;
			bp.AddComponent(delegate(IncreaseResourceAmount c)
			{
				c.m_Resource = DeathsnatcherAnimateDeadResource.ToReference<BlueprintAbilityResourceReference>();
				c.Value = 2;
			});
		});
	}
}
