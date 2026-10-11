using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Utilities.SelfTest;

[HarmonyPatch]
internal static class SelfTestForeignSpellbookRecorder
{
	private static readonly object Gate = new object();

	private static readonly List<string> Demands = new List<string>();

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return IsekaiSelfTest.Enabled;
	}

	internal static void Reset()
	{
		lock (Gate)
		{
			Demands.Clear();
		}
	}

	internal static List<string> Recorded()
	{
		lock (Gate)
		{
			return Demands.Distinct().OrderBy((string d) => d, StringComparer.Ordinal).ToList();
		}
	}

	private static void Record(string what)
	{
		lock (Gate)
		{
			if (Demands.Count < 200)
			{
				Demands.Add("'" + what + "' demanded by " + Caller());
			}
		}
	}

	[HarmonyPatch(typeof(UnitDescriptor), "DemandSpellbook", new Type[] { typeof(BlueprintCharacterClass) })]
	[HarmonyPrefix]
	private static void ByClass(UnitDescriptor __instance, BlueprintCharacterClass characterClass)
	{
		try
		{
			if (__instance != null && characterClass != null && __instance.GetSpellbook(characterClass) == null)
			{
				Record(characterClass.name);
			}
		}
		catch
		{
		}
	}

	[HarmonyPatch(typeof(UnitDescriptor), "DemandSpellbook", new Type[] { typeof(BlueprintSpellbook) })]
	[HarmonyPrefix]
	private static void ByBlueprint(UnitDescriptor __instance, BlueprintSpellbook blueprint)
	{
		try
		{
			if (__instance != null && blueprint != null && __instance.GetSpellbook(blueprint) == null)
			{
				Record(blueprint.name);
			}
		}
		catch
		{
		}
	}

	private static Type OuterType(Type type)
	{
		Type type2 = type;
		while (type2.DeclaringType != null && type2.Name.StartsWith("<", StringComparison.Ordinal))
		{
			type2 = type2.DeclaringType;
		}
		return type2;
	}

	private static string Caller()
	{
		try
		{
			StackTrace stackTrace = new StackTrace(2, fNeedFileInfo: false);
			for (int i = 0; i < stackTrace.FrameCount && i < 20; i++)
			{
				MethodBase methodBase = stackTrace.GetFrame(i)?.GetMethod();
				Type type = methodBase?.DeclaringType;
				if (!(type == null) && !(methodBase == null) && !(type == typeof(UnitDescriptor)) && !(type == typeof(Spellbook)) && !(type == typeof(SelfTestForeignSpellbookRecorder)))
				{
					string text = type.Namespace ?? "";
					if (!text.StartsWith("MonoMod", StringComparison.Ordinal) && !text.StartsWith("HarmonyLib", StringComparison.Ordinal) && !text.StartsWith("System", StringComparison.Ordinal) && methodBase.Name.IndexOf("DemandSpellbook", StringComparison.Ordinal) < 0 && type.Name.IndexOf("DemandSpellbook", StringComparison.Ordinal) < 0)
					{
						return OuterType(type).Name + "." + methodBase.Name;
					}
				}
			}
			return "(the caller could not be read from the stack)";
		}
		catch
		{
			return "(the caller could not be read from the stack)";
		}
	}
}
