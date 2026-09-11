using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiHumanCrossbreedLegacy
	{
		private static BlueprintFeature ourHeritage;

		public static void Add()
		{
			ourHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHumanCrossbreedLegacy", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Crossbreed Human");
				bp.SetDescription(Main.IsekaiContext, "Humanity's remarkable adaptability has long intertwined with planar entities, fey, and ancient bloodlines. \nIn you, dormant ancestral lineages have awakened in vibrant harmony, granting you versatile racial heritage options.");
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.ReapplyOnLevelUp = true;
			});
			HumanHeritageSelection.Register(ourHeritage);
		}

		public static void Patch()
		{
			List<BlueprintFeatureReference> list = new BlueprintFeatureReference[9]
			{
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("b7f02ba92b363064fb873963bec275ee"),
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("64e8b7d5f1ae91d45bbf1e56a3fdff01"),
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("c4faf439f0e70bd40b5e36ee80d06be7"),
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("25a5878d125338244896ebd3238226c8"),
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("ef35a22c9a27da345a4528f0d5889157"),
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("1dc20e195581a804890ddc74218bfd8e"),
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("fd188bb7bb0002e49863aec93bfb9d99"),
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("4d4555326b9b7144f93be1ea61337cd7"),
				BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("5c4e42124dc2b4647af6e36cf2590500")
			}.ToList();
			if (ourHeritage == null)
			{
				return;
			}
			BlueprintFeatureReference[] allFeatures = FeatTools.Selections.BasicFeatSelection.m_AllFeatures;
			foreach (BlueprintFeatureReference blueprintFeatureReference in allFeatures)
			{
				if (blueprintFeatureReference == null || blueprintFeatureReference.Get() == null)
				{
					continue;
				}
				BlueprintFeature blueprintFeature = blueprintFeatureReference.Get();
				if (blueprintFeature == null)
				{
					continue;
				}
				bool flag = false;
				if (((BlueprintScriptableObject)blueprintFeature).Components != null && ((BlueprintScriptableObject)blueprintFeature).Components.Length != 0)
				{
					BlueprintComponent[] components = ((BlueprintScriptableObject)blueprintFeature).Components;
					foreach (BlueprintComponent blueprintComponent in components)
					{
						if (blueprintComponent != null && blueprintComponent is PrerequisiteFeature { m_Feature: not null } prerequisiteFeature && list.Contains(prerequisiteFeature.m_Feature))
						{
							flag = true;
							if (prerequisiteFeature.Group.Equals(Prerequisite.GroupType.All))
							{
								prerequisiteFeature.Group = Prerequisite.GroupType.Any;
							}
						}
					}
				}
				if (flag)
				{
					blueprintFeature.AddComponent(delegate(PrerequisiteFeature c)
					{
						c.Group = Prerequisite.GroupType.Any;
						c.CheckInProgression = false;
						c.HideInUI = false;
						c.m_Feature = ourHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
			}
		}
	}
}
