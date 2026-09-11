using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class ExtraOracleSelection
	{
		public static void Configure()
		{
			BlueprintFeatureSelection CurseSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiOracleCurseSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Divine Curse");
				bp.SetDescription(Main.IsekaiContext, "Gain a curse, but some curses are blessings in disguise.");
				bp.Ranks = 3;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = FeatTools.Selections.OracleCurseSelection.m_AllFeatures;
				bp.m_Features = bp.m_AllFeatures;
			});
			BlueprintFeatureSelection MysterySelection = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiOracleMysterySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Divine Mystery");
				bp.SetDescription(Main.IsekaiContext, "Master another part of reality...");
				bp.Ranks = 3;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = FeatTools.Selections.OracleMysterySelection.m_AllFeatures;
				bp.m_Features = bp.m_AllFeatures;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiOracleSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Divine Inheritance");
				bp.SetDescription(Main.IsekaiContext, "As you get closer and closer to the truth of divinity you gain a new Mystery, Revelation, or perhaps a curse from a jealous god?");
				bp.Ranks = 7;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[4]
				{
					CurseSelection.ToReference<BlueprintFeatureReference>(),
					MysterySelection.ToReference<BlueprintFeatureReference>(),
					FeatTools.Selections.OracleRevelationSelection.ToReference<BlueprintFeatureReference>(),
					FeatTools.Selections.OracleCureOrInflictSelection.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}

		public static BlueprintFeatureSelection Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiOracleSelection");
		}

		public static BlueprintFeatureReference GetReference()
		{
			return Get().ToReference<BlueprintFeatureReference>();
		}
	}
}
