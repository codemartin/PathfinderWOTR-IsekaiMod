using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod
{
	public class MartialGodLegacySelection
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "MartialGodLegacySelection.Name", "Martial God Legacy");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "MartialGodLegacySelection.Description", "You recall memories of a past life of supreme martial cultivation, wielding sovereign Ki that shatters physical boundaries.");

		private static BlueprintFeatureSelection ClassFeature;

		public static void Configure()
		{
			if (ClassFeature == null)
			{
				ClassFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodLegacySelection", delegate(BlueprintFeatureSelection bp)
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
			getClassFeature();
			ClassFeature.m_AllFeatures = ClassFeature.m_AllFeatures.AddToArray(prog.ToReference<BlueprintFeatureReference>());
			ClassFeature.m_Features = ClassFeature.m_Features.AddToArray(prog.ToReference<BlueprintFeatureReference>());
		}

		public static void Prohibit(BlueprintProgression prog)
		{
		}

		public static void Finish()
		{
		}
	}
}
