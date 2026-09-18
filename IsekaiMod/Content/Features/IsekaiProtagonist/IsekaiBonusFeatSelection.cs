using IsekaiMod.Content.Features.ExceptionalFeats;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class IsekaiBonusFeatSelection
	{
		private static readonly BlueprintFeatureSelection BasicFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");

		public static void Add()
		{
			PatchExceptionalFeatSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiBonusFeatSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Bonus Feat");
				bp.SetDescription(Main.IsekaiContext, "At 1st level, and at every even level thereafter, you gain a bonus {g|Encyclopedia:Feat}feat{/g} in addition to those gained from normal advancement.");
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.Group = FeatureGroup.Feat;
				bp.Group2 = FeatureGroup.TricksterFeat;
				bp.m_AllFeatures = BasicFeatSelection.m_AllFeatures;
				bp.m_Features = BasicFeatSelection.m_AllFeatures;
			}));
		}

		private static void PatchExceptionalFeatSelection(BlueprintFeatureSelection blueprintFeatureSelection)
		{
			BlueprintFeatureSelection blueprintFeatureSelection2 = ExceptionalFeatSelection.Get();
			BlueprintFeatureSelection bonus = ExceptionalFeatSelection.GetBonus();
			if (blueprintFeatureSelection2 != null && bonus != null)
			{
				blueprintFeatureSelection.RemoveFromSelection(blueprintFeatureSelection2);
				blueprintFeatureSelection.AddToFirst(bonus);
			}
			// Mirror the basic feat list so feats other mods add later show up here too.
			MirroredSelections.Register(blueprintFeatureSelection,
				blueprintFeatureSelection2 == null ? null : new BlueprintFeatureReference[] { blueprintFeatureSelection2.ToReference<BlueprintFeatureReference>() },
				BasicFeatSelection);
		}
	}
}
