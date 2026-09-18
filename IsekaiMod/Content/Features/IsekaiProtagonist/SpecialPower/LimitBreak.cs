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
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class LimitBreak
	{
		private static readonly Sprite Icon_LimitBreak = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("df4f34f7cac73ab40986bc33f87b1a3c")).m_Icon;

		public static void Add()
		{
			BlueprintBuff LimitBreakBuff = TTCoreExtensions.CreateBuff("LimitBreakBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Limit Break Awakening");
				bp.SetDescription(Main.IsekaiContext, "Pushed to the brink of death, your latent otherworldly power awakens! You gain Haste, a +4 untyped bonus to Attack and Damage rolls, and DR 20/- for 3 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_LimitBreak;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
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
			SpecialPowerSelection.AddToAuthoritySelection(Helpers.CreateBlueprint(Main.IsekaiContext, "LimitBreak", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power - Limit Break");
				bp.SetDescription(Main.IsekaiContext, "True protagonists grow strongest when their backs are against the wall. \nBenefit: When your hit points fall below 25%, you enter a Limit Break Awakening for 3 rounds, gaining an extra attack (Haste), +4 to attack and damage, and DR 20/-.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_LimitBreak;
				bp.AddComponent(delegate(AddIncomingDamageTrigger c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(Conditional cond)
					{
						cond.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionTargetHPLessThanPercent hp)
						{
							hp.Percent = 25;
						});
						cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
						{
							a.m_Buff = LimitBreakBuff.ToReference<BlueprintBuffReference>();
							a.DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Rounds,
								DiceType = DiceType.Zero,
								BonusValue = new ContextValue
								{
									ValueType = ContextValueType.Simple,
									Value = 3
								}
							};
						});
						cond.IfFalse = ActionFlow.DoNothing();
					});
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 13;
				});
			}));
		}
	}
}
