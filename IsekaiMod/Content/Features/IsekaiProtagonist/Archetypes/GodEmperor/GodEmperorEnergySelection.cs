using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class GodEmperorEnergySelection
	{
		public static void Add()
		{
			BlueprintFeature IsekaiChannelPositiveEnergyFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiChannelPositiveEnergyFeature");
			BlueprintFeature IsekaiChannelNegativeEnergyFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiChannelNegativeEnergyFeature");
			Helpers.CreateBlueprint(Main.IsekaiContext, "GodEmperorEnergySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Channel Energy");
				bp.SetDescription(Main.IsekaiContext, "At 3rd level, the God Emperor is able to choose between channeling positive energy or negative energy.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)IsekaiChannelPositiveEnergyFeature)?.m_Icon;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					IsekaiChannelPositiveEnergyFeature.ToReference<BlueprintFeatureReference>(),
					IsekaiChannelNegativeEnergyFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}
	}
}
