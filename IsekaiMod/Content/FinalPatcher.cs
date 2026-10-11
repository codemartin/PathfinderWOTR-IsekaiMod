using System;
using HarmonyLib;
using IsekaiMod.Config;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Content.Deities;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using IsekaiMod.Content.Heritages;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content;

[HarmonyPatch]
internal static class FinalPatcher
{
	private static bool Run;

	private const string Phase = "FinalPatcher";

	[HarmonyPriority(-100)]
	[HarmonyPatch(typeof(StartGameLoader), "LoadAllJson")]
	[HarmonyPostfix]
	private static void Postfix()
	{
		if (Run)
		{
			return;
		}
		Run = true;
		try
		{
			PatchAll();
		}
		finally
		{
			RegressionSanityCheck.LogStepFailureSummary("FinalPatcher");
		}
	}

	private static void PatchAll()
	{
		if (ContentOffering.IsOffered(OfferedContent.IsekaiDeities))
		{
			SafeStep("PatchDeitySelection", IsekaiDeitySelection.PatchDeitySelection);
		}
		if (Main.IsekaiContext.AddedContent.EnableConstellations)
		{
			SafeStep("MarkConstellationSponsors", IsekaiDeitySelection.MarkConstellationSponsors);
		}
		if (ModSupport.IsExpandedContentEnabled)
		{
			SafeStep("ModSupport.AddExpandedContentDrakes", delegate
			{
				ModSupport.AddExpandedContentDrakes(IsekaiProtagonistClass.Get());
			});
		}
		SafeStep("BonusFeatListRefresh", BonusFeatListRefresh.RefreshAll);
		SafeStep("ExceptionalFeatSelection.RefreshLateFeats", IsekaiMod.Content.Features.ExceptionalFeats.ExceptionalFeatSelection.RefreshLateFeats);
		SafeStep("FlushHeritageOffers", delegate
		{
			ContentOffering.FlushHeritageOffers();
		});
		SafeStep("PatchHeritages", PatchHeritages);
		if (ContentOffering.IsOffered(OfferedContent.IsekaiHeritages))
		{
			SafeStep("HumanHeritageSelection.Patch", HumanHeritageSelection.Patch);
		}
		SafeStep("LegacySelection.ConfigureStep3", LegacySelection.ConfigureStep3);
		SafeStep("LateSelectionCompatibility.Refresh", LateSelectionCompatibility.Refresh);
		SafeStep("PrebuildIsekaiProtagonistFeatureList.PatchLegacySelection", PrebuildIsekaiProtagonistFeatureList.PatchLegacySelection);
		SafeStep("ArcanistPatcher", delegate
		{
			PatchTools.ArcanistPatcher.Patch(IsekaiProtagonistClass.GetReference(), MastermindSpellbook.GetReference());
		});
		SafeStep("KineticistPatcher", delegate
		{
			PatchTools.KineticistPatcher.Patch(IsekaiProtagonistClass.GetReference());
		});
		SafeStep("ShifterPatcher", delegate
		{
			PatchTools.ShifterPatcher.Patch(IsekaiProtagonistClass.GetReference());
		});
		if (ContentOffering.IsOffered(OfferedContent.IsekaiProtagonist))
		{
			SafeStep("PatchWearyingStrike", PatchTools.PatchWearyingStrike);
		}
		if (Main.IsekaiContext.AddedContent.MergeIsekaiSpellList)
		{
			SafeStep("MergeSpellLists", IsekaiProtagonistSpellList.MergeSpellLists);
		}
		SafeStep("PatchMastermindSpellList", MastermindSpellList.PatchMastermindSpellList);
		if (ModSupport.IsTableTopTweakBaseEnabled)
		{
			SafeStep("PatchTableTopTweakCore", PatchTableTopTweakCore);
		}
		SafeStep("QuestObjectiveListSafety", QuestObjectiveListSafety.FillAllLoaded);
		SafeStep("RegressionSanityCheck.RunAfterFinalPatch", RegressionSanityCheck.RunAfterFinalPatch);
	}

	private static void SafeStep(string name, Action action)
	{
		try
		{
			action();
		}
		catch (Exception arg)
		{
			RegressionSanityCheck.RecordStepFailure("FinalPatcher", name);
			Main.IsekaiContext.Logger.LogError($"[FinalPatcher Warning] Step '{name}' threw an exception: {arg}");
		}
	}

	private static void PatchTableTopTweakCore()
	{
		BlueprintFeatureSelection blueprint = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c898b6e4918c41c3a351c9a882c65cea");
		BlueprintFeatureSelection blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("6e32488a2cec4ba586508db4f78b062d");
		BlueprintFeatureSelection blueprint3 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("40f13b4925c24e50bc8f3d5fe4d42a05");
		if (blueprint != null && OracleLegacy.Get() != null)
		{
			OracleLegacy.Get().LevelEntries = (OracleLegacy.Get().LevelEntries ?? new LevelEntry[0]).AddToArray(Helpers.CreateLevelEntry(20, blueprint));
		}
		if (blueprint2 != null && ShamanLegacy.Get() != null)
		{
			ShamanLegacy.Get().LevelEntries = (ShamanLegacy.Get().LevelEntries ?? new LevelEntry[0]).AddToArray(Helpers.CreateLevelEntry(20, blueprint2));
		}
		if (blueprint3 != null && SorcererLegacy.Get() != null)
		{
			SorcererLegacy.Get().LevelEntries = (SorcererLegacy.Get().LevelEntries ?? new LevelEntry[0]).AddToArray(Helpers.CreateLevelEntry(20, blueprint3));
		}
		MagusBasicLegacy.PatchForBroadStudy();
	}

	private static void PatchHeritages()
	{
		SafeStep("IsekaiHumanCrossbreedLegacy.Patch", IsekaiHumanCrossbreedLegacy.Patch);
		SafeStep("ElfHeritagePatcher.Patch", ElfHeritagePatcher.Patch);
		SafeStep("RaceStatSuppressor.Patch", RaceStatSuppressor.Patch);
		SafeStep("AlternateTraitExclusion.Patch", AlternateTraitExclusion.Patch);
	}
}
