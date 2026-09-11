using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class OtherworldScavenger
	{
		private static readonly Sprite Icon_Scavenger = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");

		public static void Add()
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldScavengerFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Otherworld Scavenger");
				bp.SetDescription(Main.IsekaiContext, "Whether caught in the burning ashes of Kenabres or stranded in desolate icy wastes, you adapt with uncanny resourcefulness.\nBenefit: You gain a +4 competence bonus to Perception, Lore (Nature), and Athletics checks, Damage Reduction 5/-, and complete immunity to the fatigued and exhausted conditions.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Scavenger;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillLoreNature;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillAthletics;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Fatigued;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Exhausted;
				});
			});
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
			if (modBlueprint != null)
			{
				modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(blueprintFeature.ToReference<BlueprintFeatureReference>());
				modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(blueprintFeature.ToReference<BlueprintFeatureReference>());
			}
			FeatTools.Selections.BasicFeatSelection.AddToSelection(blueprintFeature);
		}
	}
}
