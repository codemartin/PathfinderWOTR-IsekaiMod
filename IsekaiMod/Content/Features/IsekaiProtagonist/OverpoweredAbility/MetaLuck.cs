using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class MetaLuck
	{
		private static readonly Sprite Icon_Fortune = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("eaf7077a8ff35644883df6d4f7b2084c")).m_Icon;

		public static void Add()
		{
			BlueprintBuff MetaLuckDebuff = TTCoreExtensions.CreateBuff("MetaLuckDebuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Meta Luck (Debuff)");
				bp.SetDescription(Main.IsekaiContext, "This creature takes the lower of two d20 rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Fortune;
				bp.AddComponent(delegate(ModifyD20 c)
				{
					c.Rule = RuleType.All;
					c.RollsAmount = 1;
				});
				bp.Stacking = StackingType.Replace;
				bp.IsClassFeature = true;
			});
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("MetaLuck", "Overpowered Ability - Meta Luck", "Everyone mistakes you for a prodigal genius, treating every action you make as a calculated move in your 1000-year plan. Enemies tremble in fear as they meet you, hallucinating a universe-sized gap between them and your power level. Guided by karmic providence and chaotic fortune (echoing Desna's boundless luck), reality bends to your whims.\nBenefit: You always take the higher of two d20 rolls. Enemies that attack you take the lower of two d20 rolls for one round.\nNote: Mutually exclusive with Perfect Roll (Axis Determinism) and Otherworldly Gacha (Merchant's Gamble).", "Meta Luck", "This character always takes the higher of two d20 rolls.", Icon_Fortune, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(ModifyD20 c)
				{
					c.Rule = RuleType.All;
					c.RollsAmount = 1;
					c.TakeBest = true;
				});
				bp.AddComponent(delegate(AddTargetBeforeAttackRollTrigger c)
				{
					c.ActionsOnAttacker = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = MetaLuckDebuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.DurationValue = Values.Duration.OneRound;
					});
					c.ActionOnSelf = ActionFlow.DoNothing();
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "PerfectRollFeature");
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "MerchantsGambleFeature");
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
