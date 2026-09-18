using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class StatusWindow
	{
		private static readonly Sprite Icon_Appraisal = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_GIFTED.png");

		public static void Add()
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "StatusWindowFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Status Window");
				bp.SetDescription(Main.IsekaiContext, "You project a translucent RPG system interface over reality, granting real-time appraisal of enemy attributes, incoming trajectories, and structural weak spots.\nBenefit: You gain permanent True Seeing and See Invisibility, complete immunity to blindness, a +4 insight bonus to Armor Class, attack rolls, damage rolls, and saving throws, a +5 competence bonus on all skill checks, and your weapon critical threat range increases by 1 (which stacks with Improved Critical).\nNote: Mutually exclusive with Omniscient Mimicry (Great Sage) and Paradox Sovereign (Temporal Inevitable).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Appraisal;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffAllSkillsBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Value = 5;
					c.Multiplier = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 1
					};
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.TrueSeeing;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.SeeInvisibility;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Blindness;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Blindness;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Blindness;
				});
				bp.AddComponent(delegate(WeaponCriticalEdgeIncreaseStackable c)
				{
					c.Value = 1;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "OmniscientMimicryFeature");
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "ParadoxSovereignFeature");
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AllAccordingToPlanFeature");
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
