using System;
using HarmonyLib;
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

namespace IsekaiMod.Content
{
	[HarmonyPatch]
	internal static class FinalPatcher
	{
		private static bool Run;

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
			SafeStep("PatchDeitySelection", IsekaiDeitySelection.PatchDeitySelection);
			if (Main.IsekaiContext.AddedContent.Isekai.IsEnabled("Isekai Heritages"))
			{
				SafeStep("PatchHeritages", PatchHeritages);
			}
			if (!Main.IsekaiContext.AddedContent.Isekai.IsDisabled("Isekai Protagonist"))
			{
				SafeStep("LegacySelection.ConfigureStep3", LegacySelection.ConfigureStep3);
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
				SafeStep("PatchWearyingStrike", PatchTools.PatchWearyingStrike);
				if (Main.IsekaiContext.AddedContent.MergeIsekaiSpellList)
				{
					SafeStep("MergeSpellLists", IsekaiProtagonistSpellList.MergeSpellLists);
				}
				SafeStep("PatchMastermindSpellList", MastermindSpellList.PatchMastermindSpellList);
				if (ModSupport.IsTableTopTweakBaseEnabled)
				{
					SafeStep("PatchTableTopTweakCore", PatchTableTopTweakCore);
				}
			}
			// Runs after every other mod's BlueprintsCache.Init postfix, so mirrored selections
			// (Exceptional Feats, Isekai Bonus Feat, Extra Special Power) pick up features those mods added.
			SafeStep("MirroredSelections.Sync", MirroredSelections.Sync);
			if (!Main.IsekaiContext.AddedContent.Isekai.IsDisabled("Isekai Protagonist"))
			{
				// Let Isekai wrapper selections satisfy feat and mythic prerequisites that expect the base-game selections.
				SafeStep("ShamanSelection.PatchPrerequisiteCompatibility", ShamanSelection.PatchPrerequisiteCompatibility);
				SafeStep("ExtraOracleSelection.PatchPrerequisiteCompatibility", ExtraOracleSelection.PatchPrerequisiteCompatibility);
				SafeStep("BloodragerChimeraLegacy.PatchPrerequisiteCompatibility", BloodragerChimeraLegacy.PatchPrerequisiteCompatibility);
				SafeStep("ExtraBloodlineSelection.PatchPrerequisiteCompatibility", ExtraBloodlineSelection.PatchPrerequisiteCompatibility);
				SafeStep("WitchPatronSelection.PatchPrerequisiteCompatibility", WitchPatronSelection.PatchPrerequisiteCompatibility);
				SafeStep("InquisitorTacticianLegacy.PatchPrerequisiteCompatibility", InquisitorTacticianLegacy.PatchPrerequisiteCompatibility);
				SafeStep("InquisitorDomainLordLegacy.PatchPrerequisiteCompatibility", InquisitorDomainLordLegacy.PatchPrerequisiteCompatibility);
			}
		}

		private static void SafeStep(string name, Action action)
		{
			try
			{
				action();
			}
			catch (Exception arg)
			{
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
			MagusArcherLegacy.PatchForBroadStudy();
			MagusDancerLegacy.PatchForBroadStudy();
			MagusSpellbladeLegacy.PatchForBroadStudy();
		}

		private static void PatchHeritages()
		{
			IsekaiHumanCrossbreedLegacy.Patch();
			HumanHeritageSelection.Patch();
		}
	}
}
