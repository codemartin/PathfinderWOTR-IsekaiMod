using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;

internal class LimitBreak
{
	private static readonly Sprite Icon_LimitBreak = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("df4f34f7cac73ab40986bc33f87b1a3c"))?.m_Icon;

	public static void Add()
	{
		BlueprintBuff LimitBreakBuff = TTCoreExtensions.CreateBuff("LimitBreakBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Limit Break Awakening");
			bp.SetDescription(Main.IsekaiContext, "Pushed to the brink of death, your latent otherworldly power awakens! You gain an extra attack, a +4 untyped bonus to Attack and Damage rolls, and DR 20/- for 3 rounds. The damage reduction does not stack with other damage reduction.");
			((BlueprintUnitFact)bp).m_Icon = Icon_LimitBreak;
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 20;
			});
			bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
		});
		BlueprintBuff LimitBreakSpentBuff = TTCoreExtensions.CreateBuff("LimitBreakSpentBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Limit Break Spent");
			bp.SetDescription(Main.IsekaiContext, "You have already drawn on your limit break. You cannot awaken again for 1 minute.");
			((BlueprintUnitFact)bp).m_Icon = Icon_LimitBreak;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
		});
		SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "LimitBreak", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Special Power - Limit Break");
			bp.SetDescription(Main.IsekaiContext, "True protagonists grow strongest when their backs are against the wall. \nBenefit: When your hit points fall below 25%, you enter a Limit Break Awakening for 3 rounds, gaining an extra attack, +4 to attack and damage, and DR 20/- that does not stack with other damage reduction.\nOnce it triggers, Limit Break cannot trigger again for 1 minute.\nRequires character level 5.");
			((BlueprintUnitFact)bp).m_Icon = Icon_LimitBreak;
			bp.AddComponent(delegate(AddIncomingDamageTrigger c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(Conditional cond)
				{
					cond.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionTargetHPLessThanPercent hp)
					{
						hp.Percent = 25;
					});
					cond.IfFalse = ActionFlow.DoNothing();
					cond.IfTrue = ActionFlow.DoSingle(delegate(Conditional guard)
					{
						guard.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionHasBuff chk)
						{
							chk.m_Buff = LimitBreakSpentBuff.ToReference<BlueprintBuffReference>();
							chk.Not = true;
						});
						guard.IfFalse = ActionFlow.DoNothing();
						guard.IfTrue = Helpers.CreateActionList(new ContextActionApplyBuff
						{
							m_Buff = LimitBreakBuff.ToReference<BlueprintBuffReference>(),
							DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Rounds,
								DiceType = DiceType.Zero,
								BonusValue = new ContextValue
								{
									ValueType = ContextValueType.Simple,
									Value = 3
								}
							}
						}, new ContextActionApplyBuff
						{
							m_Buff = LimitBreakSpentBuff.ToReference<BlueprintBuffReference>(),
							DurationValue = Values.Duration.OneMinute
						});
					});
				});
			});
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 5;
			});
		}));
	}
}
