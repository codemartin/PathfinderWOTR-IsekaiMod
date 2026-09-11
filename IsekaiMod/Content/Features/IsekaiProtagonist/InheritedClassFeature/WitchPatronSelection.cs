using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class WitchPatronSelection
	{
		private static BlueprintFeatureSelection myfeat;

		public static void Configure()
		{
			myfeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiWitchSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Patrons Blessing");
				bp.SetDescription(Main.IsekaiContext, "As you grow so does your ability to make new pacts with otherworldy beings other gain other benefits from your existing ones.");
				bp.Ranks = 4;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
		}

		public static BlueprintFeatureSelection Get()
		{
			if (myfeat != null)
			{
				return myfeat;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiWitchSelection");
		}
	}
}
