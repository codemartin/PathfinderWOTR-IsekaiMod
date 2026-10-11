using System;
using System.Collections.Generic;
using HarmonyLib;

namespace IsekaiMod.Utilities;

internal static class HarmonyStartup
{
	internal static List<string> PatchEachClass(Harmony harmony, IEnumerable<Type> types, Action<string> reportError)
	{
		List<string> list = new List<string>();
		if (harmony == null || types == null)
		{
			return list;
		}
		foreach (Type type in types)
		{
			if (type == null)
			{
				continue;
			}
			try
			{
				harmony.CreateClassProcessor(type).Patch();
			}
			catch (Exception ex)
			{
				list.Add(type.FullName);
				try
				{
					reportError?.Invoke(FailureLine(type, ex));
				}
				catch
				{
				}
			}
		}
		return list;
	}

	internal static List<string> PatchModAssembly(Harmony harmony)
	{
		return PatchEachClass(harmony, AccessTools.GetTypesFromAssembly(typeof(HarmonyStartup).Assembly), Main.LogError);
	}

	internal static string FailureLine(Type patchClass, Exception ex)
	{
		string text = ((ex == null) ? "unknown error" : (ex.GetType().Name + ": " + (ex.InnerException?.Message ?? ex.Message)));
		return "[HarmonyStartup] Patch class " + (patchClass?.FullName ?? "(unknown)") + " could not be applied and is skipped; the rest of the mod is patched. " + text;
	}
}
