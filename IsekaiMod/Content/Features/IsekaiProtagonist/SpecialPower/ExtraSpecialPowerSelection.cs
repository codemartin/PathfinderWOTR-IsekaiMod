using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal static class ExtraSpecialPowerSelection
	{
		private static readonly Sprite Icon_ForetellAidBuff = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon;

		public static void Add()
		{
			BlueprintFeatureSelection blueprintFeatureSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "ExtraSpecialPowerSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Extra Special Power");
				bp.SetDescription(Main.IsekaiContext, "You gain an additional Special Power.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ForetellAidBuff;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = SpecialPowerSelection.Get()?.m_AllFeatures ?? new BlueprintFeatureReference[0];
				bp.m_Features = bp.m_AllFeatures;
			});
			MirroredSelections.Register(blueprintFeatureSelection, SpecialPowerSelection.Get());
			if (blueprintFeatureSelection != null)
			{
				SecretPowerSelection.AddToSelection(blueprintFeatureSelection);
			}
		}
	}
}
