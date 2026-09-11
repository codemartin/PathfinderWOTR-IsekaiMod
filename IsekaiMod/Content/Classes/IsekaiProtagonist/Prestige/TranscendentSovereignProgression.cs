using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige
{
	internal class TranscendentSovereignProgression
	{
		public static void Add()
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TranscendentAuthority");
			BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PlotTwistFeature");
			BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TranscendentDomainFeature");
			BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AbsoluteCounterFeature");
			BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "RealityRewriteFeature");
			BlueprintFeatureSelection modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
			BlueprintFeatureSelection modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
			BlueprintProgression blueprintProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "TranscendentSovereignProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendent Sovereign");
				bp.SetDescription(Main.IsekaiContext, "Having mastered the boundaries between mortal existence and the infinite multiverse, the Transcendent Sovereign commands authority over time, space, and reality itself.");
				((BlueprintUnitFact)bp).m_AllowNonContextActions = false;
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = TranscendentSovereignClass.GetReference(),
						AdditionalLevel = 0
					}
				};
			});
			blueprintProgression.LevelEntries = new LevelEntry[10]
			{
				Helpers.CreateLevelEntry(1, modBlueprint),
				Helpers.CreateLevelEntry(2, modBlueprint6),
				Helpers.CreateLevelEntry(3, modBlueprint2),
				Helpers.CreateLevelEntry(4, modBlueprint6),
				Helpers.CreateLevelEntry(5, modBlueprint3),
				Helpers.CreateLevelEntry(6, modBlueprint6),
				Helpers.CreateLevelEntry(7, modBlueprint4),
				Helpers.CreateLevelEntry(8, modBlueprint6),
				Helpers.CreateLevelEntry(9, modBlueprint7),
				Helpers.CreateLevelEntry(10, modBlueprint5)
			};
			blueprintProgression.UIGroups = new UIGroup[1] { Helpers.CreateUIGroup(modBlueprint, modBlueprint2, modBlueprint3, modBlueprint4, modBlueprint5) };
			TranscendentSovereignClass.SetProgression(blueprintProgression);
		}
	}
}
