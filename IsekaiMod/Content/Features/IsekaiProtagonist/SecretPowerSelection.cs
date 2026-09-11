using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal static class SecretPowerSelection
	{
		private static readonly Sprite Icon_SecretPower = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SECRET_POWER.png");

		public static void Add()
		{
			BlueprintFeatureSelection AutoMetamagicSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "AutoMetamagicSelection");
			Helpers.CreateBlueprint(Main.IsekaiContext, "SecretPowerSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Secret Power");
				bp.SetDescription(Main.IsekaiContext, "On the verge of defeat, you were somehow able to draw out your secret power...");
				((BlueprintUnitFact)bp).m_Icon = Icon_SecretPower;
				bp.Ranks = 2;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = ((AutoMetamagicSelection == null) ? new BlueprintFeatureReference[0] : new BlueprintFeatureReference[1] { AutoMetamagicSelection.ToReference<BlueprintFeatureReference>() });
				bp.m_Features = bp.m_AllFeatures;
			});
		}

		public static void AddToSelection(BlueprintFeature feature)
		{
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection").AddToSelection(feature);
		}
	}
}
