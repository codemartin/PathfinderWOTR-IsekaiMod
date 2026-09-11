using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.ExceptionalFeats
{
	internal class ExceptionalFeatSelection
	{
		private static readonly BlueprintFeatureSelection MythicFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9ee0f6745f555484299b0a1563b99d81");

		private static readonly BlueprintFeatureSelection ExtraFeatMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("e10c4f18a6c8b4342afe6954bde0587b");

		private static readonly BlueprintFeatureSelection ExtraMythicAbilityMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");

		private static readonly Sprite Icon_ExceptionalFeat = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_EXCEPTIONAL_FEAT.png");

		public static void Add()
		{
			BlueprintFeatureReference[] ExceptionalFeatures = (MythicFeatSelection?.m_AllFeatures ?? new BlueprintFeatureReference[0]).Where((BlueprintFeatureReference f) => f != null).ToArray();
			if (ExtraFeatMythicFeat != null)
			{
				ExceptionalFeatures = ExceptionalFeatures.RemoveFromArray(ExtraFeatMythicFeat.ToReference<BlueprintFeatureReference>());
			}
			if (ExtraMythicAbilityMythicFeat != null)
			{
				ExceptionalFeatures = ExceptionalFeatures.RemoveFromArray(ExtraMythicAbilityMythicFeat.ToReference<BlueprintFeatureReference>());
			}
			LocalizedString exceptionalFeatDescription = Helpers.CreateString(Main.IsekaiContext, "ExceptionalFeatSelection.Description", "Exceptional feats are feats that no ordinary NPC possess.\nSource: Isekai Mod");
			BlueprintFeatureSelection feature = Helpers.CreateBlueprint(Main.IsekaiContext, "ExceptionalFeatSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Exceptional Feats");
				bp.SetDescription(exceptionalFeatDescription);
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_ExceptionalFeat;
				bp.AddComponent(delegate(PureRecommendation c)
				{
					c.Priority = RecommendationPriority.Good;
				});
				bp.m_AllFeatures = ExceptionalFeatures;
				bp.m_Features = ExceptionalFeatures;
			});
			BlueprintFeatureSelection bonusFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ExceptionalFeatBonusSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Exceptional Feats");
				bp.SetDescription(exceptionalFeatDescription);
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_ExceptionalFeat;
				bp.AddComponent(delegate(PureRecommendation c)
				{
					c.Priority = RecommendationPriority.Good;
				});
				bp.m_AllFeatures = ExceptionalFeatures;
				bp.m_Features = ExceptionalFeatures;
			});
			// Keep both Exceptional Feat lists in sync with the mythic feat list after other mods
			// have added their own mythic feats (MirroredSelections.Sync runs in FinalPatcher).
			BlueprintFeatureReference[] excludedMythicFeats = new BlueprintFeatureReference[]
			{
				ExtraFeatMythicFeat?.ToReference<BlueprintFeatureReference>(),
				ExtraMythicAbilityMythicFeat?.ToReference<BlueprintFeatureReference>()
			};
			MirroredSelections.Register(feature, excludedMythicFeats, MythicFeatSelection);
			MirroredSelections.Register(bonusFeature, excludedMythicFeats, MythicFeatSelection);
			if (Main.IsekaiContext.AddedContent.Other.IsEnabled("Exceptional Feats"))
			{
				FeatTools.Selections.BasicFeatSelection.AddToFirst(feature);
			}
		}

		public static void AddToSelection(BlueprintFeatureSelection selection, BlueprintFeatureSelection bonusSelection)
		{
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "ExceptionalFeatSelection");
			BlueprintFeatureSelection modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "ExceptionalFeatBonusSelection");
			if (modBlueprint != null && selection != null)
			{
				modBlueprint.AddToSelection(selection);
			}
			if (modBlueprint2 != null && bonusSelection != null)
			{
				modBlueprint2.AddToSelection(bonusSelection);
			}
		}

		public static BlueprintFeatureSelection Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "ExceptionalFeatSelection");
		}
	}
}
