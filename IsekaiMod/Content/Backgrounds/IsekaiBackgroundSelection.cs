using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class IsekaiBackgroundSelection
	{
		public static void Add()
		{
			BlueprintFeatureSelection feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiBackgroundSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai");
				bp.SetDescription(Main.IsekaiContext, "Before you were summoned across the cosmic rift into Golarion, in your past life you were a...");
				bp.HideInUI = true;
				bp.Groups = new FeatureGroup[1] { FeatureGroup.BackgroundSelection };
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
			});
			FeatTools.Selections.BackgroundsBaseSelection.AddToSelection(feature);
		}

		public static void AddToSelection(BlueprintFeature background)
		{
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBackgroundSelection").AddToSelection(background);
		}
	}
}
