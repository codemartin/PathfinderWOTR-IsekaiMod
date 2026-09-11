using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.TrainingEpisode
{
	internal class TrainingEpisodeSelection
	{
		private static readonly LocalizedString TrainingEpisodeDesc = Helpers.CreateString(Main.IsekaiContext, "TrainingEpisode.Description", "At 12th level, you and your companions undertake intensive combat drills, tactical sparring, and physical conditioning, pushing your mortal limits to achieve newfound martial prowess.");

		private static readonly Sprite Icon_TrainingEpisode = ((BlueprintUnitFact)FeatTools.Selections.FighterFeatSelection).m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("da1b292d91ba37948893cdbe9ea89e28"))?.m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "TrainingEpisodeSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Training Episode");
				bp.SetDescription(TrainingEpisodeDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_TrainingEpisode;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_Features = new BlueprintFeatureReference[0];
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "TrainingEpisodeBonusSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Another Training Episode");
				bp.SetDescription(TrainingEpisodeDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_TrainingEpisode;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_Features = new BlueprintFeatureReference[0];
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
			});
		}

		public static void AddToSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeSelection");
			modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			BlueprintFeatureSelection modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			modBlueprint2.m_Features = modBlueprint2.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			modBlueprint2.m_AllFeatures = modBlueprint2.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
		}
	}
}
