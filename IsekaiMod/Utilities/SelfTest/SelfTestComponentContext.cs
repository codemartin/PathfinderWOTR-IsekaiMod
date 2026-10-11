using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.EntitySystem;

namespace IsekaiMod.Utilities.SelfTest;

[HarmonyPatch]
internal static class SelfTestComponentContext
{
	[ThreadStatic]
	private static List<EntityFactComponent> t_Running;

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return IsekaiSelfTest.Enabled;
	}

	internal static string Current()
	{
		try
		{
			List<EntityFactComponent> list = t_Running;
			if (list == null || list.Count == 0)
			{
				return null;
			}
			EntityFactComponent entityFactComponent = list[list.Count - 1];
			if (entityFactComponent == null)
			{
				return null;
			}
			string text = entityFactComponent.Fact?.Blueprint?.name ?? "(no fact)";
			string text2 = entityFactComponent.SourceBlueprintComponent?.GetType().Name ?? entityFactComponent.GetType().Name;
			string sourceBlueprintComponentName = entityFactComponent.SourceBlueprintComponentName;
			return string.IsNullOrEmpty(sourceBlueprintComponentName) ? (text + " (" + text2 + ")") : (text + " (" + text2 + " " + sourceBlueprintComponentName + ")");
		}
		catch
		{
			return null;
		}
	}

	private static void Push(EntityFactComponent component)
	{
		(t_Running ?? (t_Running = new List<EntityFactComponent>())).Add(component);
	}

	private static void Pop()
	{
		List<EntityFactComponent> list = t_Running;
		if (list != null && list.Count > 0)
		{
			list.RemoveAt(list.Count - 1);
		}
	}

	[HarmonyPatch(typeof(EntityFactComponent), "Activate")]
	[HarmonyPrefix]
	private static void ActivatePrefix(EntityFactComponent __instance)
	{
		Push(__instance);
	}

	[HarmonyPatch(typeof(EntityFactComponent), "Activate")]
	[HarmonyFinalizer]
	private static void ActivateFinalizer()
	{
		Pop();
	}

	[HarmonyPatch(typeof(EntityFactComponent), "Deactivate")]
	[HarmonyPrefix]
	private static void DeactivatePrefix(EntityFactComponent __instance)
	{
		Push(__instance);
	}

	[HarmonyPatch(typeof(EntityFactComponent), "Deactivate")]
	[HarmonyFinalizer]
	private static void DeactivateFinalizer()
	{
		Pop();
	}

	[HarmonyPatch(typeof(EntityFactComponent), "TurnOn")]
	[HarmonyPrefix]
	private static void TurnOnPrefix(EntityFactComponent __instance)
	{
		Push(__instance);
	}

	[HarmonyPatch(typeof(EntityFactComponent), "TurnOn")]
	[HarmonyFinalizer]
	private static void TurnOnFinalizer()
	{
		Pop();
	}

	[HarmonyPatch(typeof(EntityFactComponent), "TurnOff")]
	[HarmonyPrefix]
	private static void TurnOffPrefix(EntityFactComponent __instance)
	{
		Push(__instance);
	}

	[HarmonyPatch(typeof(EntityFactComponent), "TurnOff")]
	[HarmonyFinalizer]
	private static void TurnOffFinalizer()
	{
		Pop();
	}
}
