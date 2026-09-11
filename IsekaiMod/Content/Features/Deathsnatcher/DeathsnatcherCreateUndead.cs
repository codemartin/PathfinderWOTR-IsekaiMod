using IsekaiMod.Content.Classes.Deathsnatcher;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.Deathsnatcher
{
	internal class DeathsnatcherCreateUndead
	{
		private static readonly BlueprintAbility CreateUndeadAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("76a11b460be25e44ca85904d6806e5a3");

		private static readonly BlueprintAbility CreateUndeadLivingArmor = BlueprintTools.GetBlueprint<BlueprintAbility>("43a1ea314c59c4a4eb2c193a1e17b805");

		private static readonly BlueprintAbility CreateUndeadGraveKnight = BlueprintTools.GetBlueprint<BlueprintAbility>("9b75cb3bd3108a24c81329a3734f2248");

		public static void Add()
		{
			BlueprintAbilityResource DeathsnatcherCreateUndeadResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherCreateUndeadResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
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
			BlueprintAbility DeathsnatcherCreateUndeadAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherCreateUndeadAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(((BlueprintUnitFact)CreateUndeadAbility).m_DisplayName);
				bp.SetDescription(((BlueprintUnitFact)CreateUndeadAbility).m_Description);
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)CreateUndeadAbility).m_Icon;
				bp.AddComponent(delegate(AbilityVariants c)
				{
					c.m_Variants = new BlueprintAbilityReference[2]
					{
						CreateUndeadLivingArmor.ToReference<BlueprintAbilityReference>(),
						CreateUndeadGraveKnight.ToReference<BlueprintAbilityReference>()
					};
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
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
					c.m_RequiredResource = DeathsnatcherCreateUndeadResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
				});
				bp.Type = AbilityType.SpellLike;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = CreateUndeadAbility.AvailableMetamagic;
				bp.m_IsFullRoundAction = true;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherCreateUndeadFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(((BlueprintUnitFact)CreateUndeadAbility).m_DisplayName);
				bp.SetDescription(Main.IsekaiContext, "At 13th level, the Deathsnatcher gains Create Undead as a spell-like ability once per day.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)CreateUndeadAbility).m_Icon;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = DeathsnatcherCreateUndeadResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { DeathsnatcherCreateUndeadAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
