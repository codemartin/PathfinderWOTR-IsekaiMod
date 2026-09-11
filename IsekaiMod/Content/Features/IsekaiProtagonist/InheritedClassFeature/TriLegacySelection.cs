using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class TriLegacySelection
	{
		private static readonly Sprite Icon_AllSkilled = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("f3bc6f9c855b2fb4e9aea364b8163aca"))?.m_Icon;

		private static BlueprintFeatureSelection Selection;

		public static void Configure()
		{
			if (Selection == null)
			{
				Selection = Helpers.CreateBlueprint(Main.IsekaiContext, "TriLegacySelection", delegate(BlueprintFeatureSelection bp)
				{
					bp.SetName(Main.IsekaiContext, "Tri-Legacy");
					bp.SetDescription(Main.IsekaiContext, "Your reincarnated soul weaves memories across multiple lifetimes. Having already awakened Dual Legacy, you now unlock mastery over a third legacy class progression.\nWhatever your past incarnations were, their distinct powers converge seamlessly within your otherworldly vessel.");
					((BlueprintUnitFact)bp).m_Icon = Icon_AllSkilled;
					bp.Ranks = 1;
					bp.IsClassFeature = true;
					bp.IgnorePrerequisites = false;
					bp.m_AllFeatures = new BlueprintFeatureReference[0];
					bp.m_Features = new BlueprintFeatureReference[0];
				});
				Selection.AddComponent(delegate(PrerequisiteClassLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.Level = 10;
				});
				Selection.AddComponent(delegate(PrerequisiteFeature c)
				{
					c.m_Feature = LegacySelection.GetOverwhelmingFeature().ToReference<BlueprintFeatureReference>();
				});
				Selection.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = Selection.ToReference<BlueprintFeatureReference>();
				});
			}
		}

		public static void Finish()
		{
			if (Selection != null)
			{
				BlueprintFeatureSelection overwhelmingFeature = LegacySelection.GetOverwhelmingFeature();
				if (overwhelmingFeature != null && overwhelmingFeature.m_AllFeatures != null)
				{
					Selection.SetFeatures(overwhelmingFeature.m_AllFeatures);
				}
				OverpoweredAbilitySelection.AddToSelection(Selection);
			}
		}

		public static BlueprintFeatureSelection Get()
		{
			if (Selection != null)
			{
				return Selection;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TriLegacySelection");
		}
	}
}
