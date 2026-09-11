using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class PlotArmor
	{
		private static readonly Sprite Icon_EdictOfImpenetrableFortress = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d7741c08ccf699e4a8a8f8ab2ed345f8"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff PlotArmorSurgeBuff = TTCoreExtensions.CreateBuff("PlotArmorSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Plot Armor: Narrative Surge");
				bp.SetDescription(Main.IsekaiContext, "Brought low in battle, destiny refuses to let the protagonist fall. You gain damage reduction 20/-, +4 morale bonus to all saves, and immunity to death effects for 3 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_EdictOfImpenetrableFortress;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 20
					};
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "PlotArmor", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Plot Armor");
				bp.SetDescription(Main.IsekaiContext, "You gain a luck bonus to AC and all saving throws equal to half your character level (minimum 1). Furthermore, when reduced below zero hit points, narrative surge triggers, granting damage reduction 20/-, +4 morale bonus to all saves, and immunity to death effects for 3 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_EdictOfImpenetrableFortress;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
				bp.AddComponent(delegate(AddIncomingDamageTrigger c)
				{
					c.ReduceBelowZero = true;
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
					{
						a.m_Buff = PlotArmorSurgeBuff.ToReference<BlueprintBuffReference>();
						a.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							BonusValue = 3
						};
					});
				});
				bp.ReapplyOnLevelUp = true;
			});
		}
	}
}
