using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;

namespace IsekaiMod.Utilities;

[HarmonyPatch]
internal static class LegacySelectionNestingPatch
{
	private const int MaxChainDepth = 32;

	private static readonly ConditionalWeakTable<FeatureSelectionState, FeatureSelectionState> s_CreatedBy = new ConditionalWeakTable<FeatureSelectionState, FeatureSelectionState>();

	private static readonly ConditionalWeakTable<SelectFeature, StrongBox<LevelUpActionPriority>> s_ActionPriority = new ConditionalWeakTable<SelectFeature, StrongBox<LevelUpActionPriority>>();

	[ThreadStatic]
	private static List<FeatureSelectionState> t_Applying;

	internal static FeatureSelectionState CreatorOf(FeatureSelectionState state)
	{
		if (state == null)
		{
			return null;
		}
		if (!s_CreatedBy.TryGetValue(state, out var value))
		{
			return null;
		}
		return value;
	}

	internal static void RecordCreator(FeatureSelectionState created, FeatureSelectionState creator)
	{
		if (!(created == null) && !(creator == null) && (object)created != creator)
		{
			s_CreatedBy.Remove(created);
			s_CreatedBy.Add(created, creator);
		}
	}

	internal static void EnterApplying(FeatureSelectionState picked)
	{
		if (t_Applying == null)
		{
			t_Applying = new List<FeatureSelectionState>();
		}
		t_Applying.Add(picked);
	}

	internal static void ExitApplying()
	{
		if (t_Applying != null && t_Applying.Count > 0)
		{
			t_Applying.RemoveAt(t_Applying.Count - 1);
		}
	}

	private static FeatureSelectionState CurrentlyApplying()
	{
		if (t_Applying != null && t_Applying.Count != 0)
		{
			return t_Applying[t_Applying.Count - 1];
		}
		return null;
	}

	internal static bool IsLegacyList(FeatureSelectionState state)
	{
		if (state?.Selection is SimpleBlueprint simpleBlueprint)
		{
			return LegacySelectionOrderPatch.IsLegacySelection(simpleBlueprint.AssetGuid);
		}
		return false;
	}

	internal static bool ChainHasLegacyList(FeatureSelectionState state)
	{
		int num = 0;
		while (state != null && num < 32)
		{
			if (IsLegacyList(state))
			{
				return true;
			}
			state = CreatorOf(state);
			num++;
		}
		return false;
	}

	internal static FeatureSelectionState LegacyHostOf(FeatureSelectionState state)
	{
		FeatureSelectionState featureSelectionState = CreatorOf(state);
		if (featureSelectionState == null || !ChainHasLegacyList(featureSelectionState))
		{
			return null;
		}
		FeatureSelectionState featureSelectionState2 = featureSelectionState;
		int num = 0;
		while (featureSelectionState2.Parent != null && num < 32)
		{
			featureSelectionState2 = featureSelectionState2.Parent;
			num++;
		}
		if ((object)featureSelectionState2 != state)
		{
			return featureSelectionState2;
		}
		return null;
	}

	internal static LevelUpActionPriority NestedPriority(LevelUpActionPriority own, bool chainHasLegacyList, LevelUpActionPriority? creator)
	{
		if (!chainHasLegacyList || !creator.HasValue)
		{
			return own;
		}
		if (creator.Value <= own)
		{
			return own;
		}
		return creator.Value;
	}

	internal static LevelUpActionPriority EffectivePriorityOf(FeatureSelectionState state)
	{
		return EffectivePriorityOf(state, 0);
	}

	private static LevelUpActionPriority EffectivePriorityOf(FeatureSelectionState state, int depth)
	{
		LevelUpActionPriority levelUpActionPriority = SelectFeature.CalculatePriority(state.Selection);
		FeatureSelectionState featureSelectionState = CreatorOf(state);
		if (featureSelectionState == null || depth >= 32)
		{
			return levelUpActionPriority;
		}
		if (!IsLegacyList(state) && !ChainHasLegacyList(featureSelectionState))
		{
			return levelUpActionPriority;
		}
		return NestedPriority(levelUpActionPriority, chainHasLegacyList: true, EffectivePriorityOf(featureSelectionState, depth + 1));
	}

	private static FeatureSelectionState StateOf(SelectFeature action, LevelUpState state)
	{
		if (action == null || state?.Selections == null)
		{
			return null;
		}
		foreach (FeatureSelectionState selection in state.Selections)
		{
			if (selection != null && selection.Selection == action.Selection && selection.Index == action.SelectionIndex)
			{
				return selection;
			}
		}
		return null;
	}

	[HarmonyPatch(typeof(SelectFeature), "Apply")]
	[HarmonyPrefix]
	private static void Apply_Prefix(SelectFeature __instance, LevelUpState state)
	{
		FeatureSelectionState picked = null;
		try
		{
			picked = StateOf(__instance, state);
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[LegacySelectionNestingPatch] could not tell which selection a pick belongs to. {arg}");
		}
		EnterApplying(picked);
	}

	[HarmonyPatch(typeof(SelectFeature), "Apply")]
	[HarmonyFinalizer]
	private static void Apply_Finalizer()
	{
		ExitApplying();
	}

	[HarmonyPatch(typeof(LevelUpState), "AddSelection")]
	[HarmonyPostfix]
	private static void AddSelection_Postfix(FeatureSelectionState parent, FeatureSelectionState __result)
	{
		try
		{
			RecordCreator(__result, parent ?? CurrentlyApplying());
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[LegacySelectionNestingPatch] could not record where a selection came from. {arg}");
		}
	}

	[HarmonyPatch(typeof(SelectFeature), MethodType.Constructor, new Type[]
	{
		typeof(FeatureSelectionState),
		typeof(IFeatureSelectionItem)
	})]
	[HarmonyPostfix]
	private static void Constructor_Postfix(SelectFeature __instance, FeatureSelectionState selection)
	{
		try
		{
			if (__instance != null && selection?.Selection != null)
			{
				LevelUpActionPriority levelUpActionPriority = SelectFeature.CalculatePriority(selection.Selection);
				LevelUpActionPriority levelUpActionPriority2 = EffectivePriorityOf(selection);
				if (levelUpActionPriority2 != levelUpActionPriority)
				{
					s_ActionPriority.Remove(__instance);
					s_ActionPriority.Add(__instance, new StrongBox<LevelUpActionPriority>(levelUpActionPriority2));
				}
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[LegacySelectionNestingPatch] could not order a nested legacy pick; using the engine's own priority. {arg}");
		}
	}

	[HarmonyPatch(typeof(SelectFeature), "Priority", MethodType.Getter)]
	[HarmonyPostfix]
	private static void Priority_Postfix(SelectFeature __instance, ref LevelUpActionPriority __result)
	{
		if (__instance != null && s_ActionPriority.TryGetValue(__instance, out var value))
		{
			__result = value.Value;
		}
	}
}
