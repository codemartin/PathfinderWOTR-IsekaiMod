using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UI.MVVM._VM.CharGen.Phases;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;

namespace IsekaiMod.Utilities;

[HarmonyPatch]
internal static class LegacySelectionOrderPatch
{
	internal const LevelUpActionPriority LegacyPriority = LevelUpActionPriority.Heritage;

	internal static readonly LevelUpActionPriority[] NestedSelectionPriorities = new LevelUpActionPriority[7]
	{
		LevelUpActionPriority.ReplaceSpellbook,
		LevelUpActionPriority.Background,
		LevelUpActionPriority.MythicAbility,
		LevelUpActionPriority.Deity,
		LevelUpActionPriority.Path,
		LevelUpActionPriority.Features,
		LevelUpActionPriority.MythicAdditionalProgressions
	};

	private static readonly HashSet<BlueprintGuid> s_LegacySelections = new HashSet<BlueprintGuid>();

	private static readonly HashSet<BlueprintGuid> s_LeadingSelections = new HashSet<BlueprintGuid>();

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.ResolvesAll(typeof(LegacySelectionOrderPatch), (typeof(CharGenFeatureSelectorPhaseVM), "get_OrderPriority"));
	}

	internal static void Register(BlueprintFeatureSelection selection)
	{
		if (selection != null)
		{
			Register(selection.AssetGuid);
		}
	}

	internal static void Register(BlueprintGuid guid)
	{
		s_LegacySelections.Add(guid);
	}

	internal static bool IsLegacySelection(BlueprintGuid guid)
	{
		return s_LegacySelections.Contains(guid);
	}

	internal static void RegisterLeading(BlueprintFeatureSelection selection)
	{
		if (selection != null)
		{
			RegisterLeading(selection.AssetGuid);
		}
	}

	internal static void RegisterLeading(BlueprintGuid guid)
	{
		s_LeadingSelections.Add(guid);
	}

	internal static bool IsLeadingSelection(BlueprintGuid guid)
	{
		return s_LeadingSelections.Contains(guid);
	}

	internal static LevelUpActionPriority PriorityOf(bool isLegacySelection, LevelUpActionPriority vanilla)
	{
		if (!isLegacySelection)
		{
			return vanilla;
		}
		return LevelUpActionPriority.Heritage;
	}

	[HarmonyPatch(typeof(SelectFeature), "CalculatePriority")]
	[HarmonyPostfix]
	public static void CalculatePriority_Postfix(IFeatureSelection selection, ref LevelUpActionPriority __result)
	{
		try
		{
			if (selection is SimpleBlueprint simpleBlueprint)
			{
				__result = PriorityOf(IsLegacySelection(simpleBlueprint.AssetGuid) || IsLeadingSelection(simpleBlueprint.AssetGuid), __result);
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[LegacySelectionOrderPatch] could not order a selection; using the engine's own priority. {arg}");
		}
	}

	internal static int ChargenOrderOf(bool isLegacySelection, int vanilla, int indexInLevelUpState, int classPageOrder, int firstSelectionPageOrder)
	{
		if (!isLegacySelection)
		{
			return vanilla;
		}
		int num = (firstSelectionPageOrder - classPageOrder) / 2;
		if (num < 1)
		{
			return vanilla;
		}
		int num2 = classPageOrder + num;
		if (indexInLevelUpState < 0)
		{
			indexInLevelUpState = 0;
		}
		if (indexInLevelUpState >= num)
		{
			indexInLevelUpState = num - 1;
		}
		return num2 + indexInLevelUpState;
	}

	internal static int ChargenOrderOfLeading(int vanilla, int indexInLevelUpState, int classPageOrder, int firstSelectionPageOrder)
	{
		int num = (firstSelectionPageOrder - classPageOrder) / 4;
		if (num < 1)
		{
			return vanilla;
		}
		if (indexInLevelUpState < 0)
		{
			indexInLevelUpState = 0;
		}
		if (indexInLevelUpState >= num)
		{
			indexInLevelUpState = num - 1;
		}
		return classPageOrder + num + indexInLevelUpState;
	}

	internal static int ChargenOrderBehindHost(int vanilla, int indexInLevelUpState, int hostOrder, int hostIndex)
	{
		if (vanilla > hostOrder)
		{
			return vanilla;
		}
		return hostOrder + Math.Max(1, indexInLevelUpState - hostIndex);
	}

	internal static int WindowPageOrder(CharGenFeatureSelectorPhaseVM phase, LevelUpController controller, FeatureSelectionState state, int vanilla, int indexInLevelUpState)
	{
		SimpleBlueprint simpleBlueprint = state?.Selection as SimpleBlueprint;
		if (phase == null || simpleBlueprint == null)
		{
			return vanilla;
		}
		int baseOrderPriority = phase.GetBaseOrderPriority(CharGenPhaseBaseVM.ChargenPhasePriority.Class);
		int baseOrderPriority2 = phase.GetBaseOrderPriority(CharGenPhaseBaseVM.ChargenPhasePriority.ClassFeatures);
		if (IsLegacySelection(simpleBlueprint.AssetGuid))
		{
			return ChargenOrderOf(isLegacySelection: true, vanilla, indexInLevelUpState, baseOrderPriority, baseOrderPriority2);
		}
		if (IsLeadingSelection(simpleBlueprint.AssetGuid))
		{
			return ChargenOrderOfLeading(vanilla, indexInLevelUpState, baseOrderPriority, baseOrderPriority2);
		}
		FeatureSelectionState featureSelectionState = LegacySelectionNestingPatch.LegacyHostOf(state);
		LevelUpState levelUpState = controller?.State;
		if (featureSelectionState == null || levelUpState == null)
		{
			return vanilla;
		}
		int num = levelUpState.Selections.IndexOf(featureSelectionState);
		if (num < 0)
		{
			return vanilla;
		}
		int vanilla2 = phase.GetBaseOrderPriority(phase.GetFeaturePriority(featureSelectionState)) + num;
		int hostOrder = ChargenOrderOf(featureSelectionState.Selection is SimpleBlueprint simpleBlueprint2 && IsLegacySelection(simpleBlueprint2.AssetGuid), vanilla2, num, baseOrderPriority, baseOrderPriority2);
		return ChargenOrderBehindHost(vanilla, indexInLevelUpState, hostOrder, num);
	}

	[HarmonyPatch(typeof(CharGenFeatureSelectorPhaseVM), "OrderPriority", MethodType.Getter)]
	[HarmonyPostfix]
	public static void OrderPriority_Postfix(CharGenFeatureSelectorPhaseVM __instance, ref int __result)
	{
		try
		{
			FeatureSelectionState featureSelectionState = __instance?.FeatureSelectorStateVM?.SelectionState;
			if (!(featureSelectionState == null))
			{
				__result = WindowPageOrder(__instance, ((CharGenPhaseBaseVM)__instance).LevelUpController, featureSelectionState, __result, __instance.IndexInLevelupState);
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[LegacySelectionOrderPatch] could not order a legacy page; using the engine's own order. {arg}");
		}
	}
}
