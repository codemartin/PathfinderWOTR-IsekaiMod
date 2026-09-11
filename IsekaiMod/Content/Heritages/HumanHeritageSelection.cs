using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages
{
	internal class HumanHeritageSelection
	{
		private static BlueprintFeature[] ourHeritages;

		private static BlueprintFeature dummyBasicFeat;

		private static BlueprintFeature dummyNoFeat;

		private static BlueprintFeatureSelection ourHeritageSelection;

		public static void CreateDummy()
		{
			ourHeritages = new BlueprintFeature[0];
			ourHeritageSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHumanHeritageSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Alternate Racial Traits");
				bp.SetDescription(Main.IsekaiContext, "The following alternate traits are available.");
				bp.IsClassFeature = true;
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.Group = FeatureGroup.KitsuneHeritage;
				bp.m_Features = new BlueprintFeatureReference[0];
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
			});
			dummyBasicFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHumanHeritageDummyFeat", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, ((BlueprintUnitFact)FeatTools.Selections.BasicFeatSelection).m_DisplayName);
				bp.SetDescription(Main.IsekaiContext, ((BlueprintUnitFact)FeatTools.Selections.BasicFeatSelection).m_Description);
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)FeatTools.Selections.BasicFeatSelection).m_Icon;
				bp.IsClassFeature = true;
			});
			dummyNoFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHumanHeritageNoFeat", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "None");
				bp.SetDescription(Main.IsekaiContext, "No alternate trait");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)FeatTools.Selections.BasicFeatSelection).m_Icon;
				bp.IsClassFeature = true;
			});
		}

		public static void Register(BlueprintFeature feature)
		{
			ourHeritages = ourHeritages.AppendToArray(feature);
		}

		public static void Patch()
		{
			BlueprintRace blueprint = BlueprintTools.GetBlueprint<BlueprintRace>("0a5d473ead98b0646b94495af250fdc4");
			if (blueprint == null || blueprint.m_Features == null)
			{
				Main.IsekaiContext.Logger.LogWarning("Human race blueprint not found or has no features; skipping human heritage patch.");
				return;
			}
			BlueprintFeature blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeature>("3adf9274a210b164cb68f472dc1e4544");
			BlueprintFeatureSelection basicFeatSelection = FeatTools.Selections.BasicFeatSelection;
			BlueprintFeatureSelection blueprintFeatureSelection = null;
			BlueprintFeatureBaseReference[] features = blueprint.m_Features;
			foreach (BlueprintFeatureBaseReference blueprintFeatureBaseReference in features)
			{
				if (blueprintFeatureBaseReference != null && blueprintFeatureBaseReference.Get() is BlueprintFeatureSelection blueprintFeatureSelection2 && !blueprintFeatureSelection2.Equals(basicFeatSelection))
				{
					blueprintFeatureSelection = blueprintFeatureSelection2;
				}
			}
			BlueprintFeature[] array;
			if (blueprintFeatureSelection != null)
			{
				Main.IsekaiContext.Logger.Log("found a selection added by another mod, adding onto that rather than creating our own");
				if (ourHeritages == null)
				{
					return;
				}
				array = ourHeritages;
				foreach (BlueprintFeature blueprintFeature in array)
				{
					if (blueprintFeature != null)
					{
						blueprintFeatureSelection.AddFeatures(blueprintFeature);
					}
				}
				return;
			}
			Main.IsekaiContext.Logger.Log("patching our own alternate human heritage feats into the game because no other source was present");
			List<BlueprintFeatureBaseReference> list = new List<BlueprintFeatureBaseReference>();
			if (dummyBasicFeat != null)
			{
				list.Add(dummyBasicFeat.ToReference<BlueprintFeatureBaseReference>());
			}
			if (blueprint2 != null)
			{
				list.Add(blueprint2.ToReference<BlueprintFeatureBaseReference>());
			}
			if (ourHeritageSelection != null)
			{
				list.Add(ourHeritageSelection.ToReference<BlueprintFeatureBaseReference>());
			}
			blueprint.m_Features = list.ToArray();
			if (ourHeritageSelection == null)
			{
				return;
			}
			if (basicFeatSelection != null)
			{
				ourHeritageSelection.AddFeatures(basicFeatSelection);
			}
			if (dummyNoFeat != null)
			{
				ourHeritageSelection.AddFeatures(dummyNoFeat);
			}
			if (ourHeritages == null)
			{
				return;
			}
			array = ourHeritages;
			foreach (BlueprintFeature blueprintFeature2 in array)
			{
				if (blueprintFeature2 != null)
				{
					ourHeritageSelection.AddFeatures(blueprintFeature2);
				}
			}
		}
	}
}
