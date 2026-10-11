using HarmonyLib;
using IsekaiMod.Config;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;

internal class SpecialPowerSelection
{
	private static readonly Sprite Icon_ForetellAidBuff = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon;

	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "SpecialPowerSelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Special Power");
			bp.SetDescription(Main.IsekaiContext, "As you increase your level, you gain special powers that allow you to wreck your enemies more easily.");
			((BlueprintUnitFact)bp).m_Icon = Icon_ForetellAidBuff;
			bp.Ranks = 10;
			bp.IsClassFeature = true;
			bp.m_Features = new BlueprintFeatureReference[0];
			bp.m_AllFeatures = new BlueprintFeatureReference[0];
		});
		BlueprintFeatureSelection SpecialPowerMythicSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "SpecialPowerMythicSelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Mythic Special Power");
			bp.SetDescription(Main.IsekaiContext, "You use your mythic powers to gain an additional special power.\nSource: Isekai Reimagined");
			((BlueprintUnitFact)bp).m_Icon = Icon_ForetellAidBuff;
			bp.Ranks = 10;
			bp.IsClassFeature = true;
			bp.m_Features = new BlueprintFeatureReference[0];
			bp.m_AllFeatures = new BlueprintFeatureReference[0];
		});
		if (!Main.IsekaiContext.AddedContent.MultipleMythicSpecialPower)
		{
			SpecialPowerMythicSelection.AddPrerequisite(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = SpecialPowerMythicSelection.ToReference<BlueprintFeatureReference>();
			});
		}
		if (Main.IsekaiContext.AddedContent.RestrictMythicSpecialPower)
		{
			SpecialPowerMythicSelection.AddPrerequisite(delegate(PrerequisiteClassLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.Level = 1;
			});
		}
		ContentOffering.OfferInSelection(FeatTools.Selections.MythicAbilitySelection, SpecialPowerMythicSelection, OfferedContent.IsekaiProtagonist);
		ContentOffering.OfferInSelection(FeatTools.Selections.ExtraMythicAbilityMythicFeat, SpecialPowerMythicSelection, OfferedContent.IsekaiProtagonist);
	}

	public static void AddToSelection(BlueprintFeature feature)
	{
		BlueprintFeatureSelection[] array = new BlueprintFeatureSelection[2]
		{
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection"),
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerMythicSelection")
		};
		foreach (BlueprintFeatureSelection obj in array)
		{
			obj.m_Features = obj.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			obj.m_AllFeatures = obj.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
		}
	}

	public static void AddToNonMythicSelection(BlueprintFeature feature)
	{
		BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
		modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
		modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
	}

	public static BlueprintFeatureSelection Get()
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
	}
}
