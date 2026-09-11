using System.Collections.Generic;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	public class DevourerLegacySelection
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "DevourerLegacySelection.Name", "Slime Legacy");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "DevourerLegacySelection.Description", "You recall memories of a past life of predation and boundless consumption as an otherworldly slime, absorbing the essence of creatures across dimensions.");

		private static BlueprintFeatureSelection ClassFeature;

		private static BlueprintProgression[] registered = new BlueprintProgression[0];

		private static HashSet<BlueprintFeature> prohibited = new HashSet<BlueprintFeature>();

		public static void Configure()
		{
			if (ClassFeature == null)
			{
				ClassFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "DevourerLegacySelection", delegate(BlueprintFeatureSelection bp)
				{
					bp.SetName(Name);
					bp.SetDescription(Description);
					bp.m_AllFeatures = new BlueprintFeatureReference[0];
					bp.m_Features = new BlueprintFeatureReference[0];
					bp.IsClassFeature = true;
				});
			}
		}

		public static BlueprintFeatureSelection getClassFeature()
		{
			if (ClassFeature == null)
			{
				Configure();
			}
			return ClassFeature;
		}

		public static void Register(BlueprintProgression prog)
		{
			registered = registered.AppendToArray(prog);
		}

		public static void Prohibit(BlueprintProgression prog)
		{
			prohibited.Add(prog);
		}

		public static void Finish()
		{
			BlueprintFeatureSelection classFeature = getClassFeature();
			BlueprintProgression[] array = registered;
			foreach (BlueprintProgression blueprintProgression in array)
			{
				classFeature.AddFeatures(blueprintProgression);
			}
			BlueprintFeatureSelection classFeature2 = LegacySelection.GetClassFeature();
			if (classFeature2 == null || classFeature2.m_AllFeatures == null)
			{
				return;
			}
			BlueprintFeatureReference[] allFeatures = classFeature2.m_AllFeatures;
			for (int i = 0; i < allFeatures.Length; i++)
			{
				BlueprintFeature blueprintFeature = allFeatures[i].Get();
				if (blueprintFeature != null && !prohibited.Contains(blueprintFeature))
				{
					classFeature.AddFeatures(blueprintFeature);
				}
			}
		}
	}
}
