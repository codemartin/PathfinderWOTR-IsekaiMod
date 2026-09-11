using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class ExtraBloodlineSelection
	{
		private static BlueprintFeatureSelection IsekaiSorcererSelection;

		public static void Configure()
		{
			BlueprintFeatureSelection IsekaiBloodlineSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiBloodlineSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Bloodline");
				bp.SetDescription(Main.IsekaiContext, "You can pick additional bloodlines as your chimera blood becomes stronger.");
				bp.Ranks = 4;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = true;
				bp.m_AllFeatures = StaticReferences.SorcererBloodlineSelection.m_AllFeatures;
				bp.m_Features = bp.m_AllFeatures;
			});
			MirroredSelections.Register(IsekaiBloodlineSelection, StaticReferences.SorcererBloodlineSelection);
			IsekaiSorcererSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiSorcererSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Bloodline Evolution");
				bp.SetDescription(Main.IsekaiContext, "As your chimera blood evolves you can pick a new bloodline feat or a new bloodline.");
				bp.Ranks = 7;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[3]
				{
					IsekaiBloodlineSelection.ToReference<BlueprintFeatureReference>(),
					FeatTools.Selections.SorcererBonusFeat.ToReference<BlueprintFeatureReference>(),
					FeatTools.Selections.SorcererFeatSelection.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}

		public static BlueprintFeatureSelection Get()
		{
			if (IsekaiSorcererSelection != null)
			{
				return IsekaiSorcererSelection;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiSorcererSelection");
		}

		public static BlueprintFeatureReference GetReference()
		{
			return Get().ToReference<BlueprintFeatureReference>();
		}
	}
}
