using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class AdaptiveArmor
	{
		private static readonly Sprite Icon_Armor = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d7741c08ccf699e4a8a8f8ab2ed345f8"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0a5ddfbcfb3989543ac7c936fc256889"))?.m_Icon;

		public static BlueprintFeature AdaptiveArmorFeature;

		public static void Add()
		{
			BlueprintBuff Buff = Helpers.CreateBlueprint(Main.IsekaiContext, "AdaptiveArmorBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Adaptive Armor Adaptation");
				bp.SetDescription(Main.IsekaiContext, "Your otherworldly armor and skin dynamically adapt to incoming trauma. Each stack grants damage reduction 3/- and 5 energy resistance against acid, cold, electricity, fire, and sonic (stacking up to 5 times for DR 15/- and 25 energy resistance).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Armor;
				bp.IsClassFeature = true;
				bp.Ranks = 5;
				bp.Stacking = StackingType.Rank;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.TargetBuffRank;
					c.m_Buff = bp.ToReference<BlueprintBuffReference>();
					c.m_Progression = ContextRankProgression.MultiplyByModifier;
					c.m_StepLevel = 3;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Sonic;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.TargetBuffRank;
					c.m_Buff = bp.ToReference<BlueprintBuffReference>();
					c.m_Progression = ContextRankProgression.MultiplyByModifier;
					c.m_StepLevel = 5;
				});
			});
			AdaptiveArmorFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AdaptiveArmorFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power: Adaptive Armor");
				bp.SetDescription(Main.IsekaiContext, "Your otherworldly armor and skin possess reactive evolutionary memory, hardening against hostile damage frequencies.\nBenefit: Whenever you take damage from an attack or hostile effect, you gain a stacking layer of Adaptive Armor lasting 1 minute. Each stack grants damage reduction 3/- and 5 resistance against all energy types (acid, cold, electricity, fire, sonic). This effect stacks up to 5 times for a maximum of DR 15/- and 25 energy resistance.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Armor;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddIncomingDamageTrigger c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = Buff.ToReference<BlueprintBuffReference>();
						b.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Minutes,
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = 1
						};
						b.AsChild = false;
					});
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			});
			SpecialPowerSelection.AddToSelection(AdaptiveArmorFeature);
		}
	}
}
