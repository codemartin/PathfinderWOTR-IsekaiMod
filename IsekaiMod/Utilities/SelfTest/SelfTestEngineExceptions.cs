using System;
using System.Collections.Generic;
using HarmonyLib;
using Owlcat.Runtime.Core.Logging;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

[HarmonyPatch(typeof(LogChannel), "Exception", new Type[]
{
	typeof(UnityEngine.Object),
	typeof(Exception),
	typeof(string),
	typeof(object[])
})]
internal static class SelfTestEngineExceptions
{
	private const int MaxPerCapture = 50;

	[ThreadStatic]
	private static List<Exception> t_Capture;

	private static readonly string[] OwnRoots = new string[8] { "Kingmaker.", "IsekaiMod.", "Owlcat.", "System.", "UnityEngine.", "Newtonsoft.", "TabletopTweaks.Core.NewComponents.", "TabletopTweaks.Core.NewActions." };

	private static readonly string[] PatchMethods = new string[3] { ".Prefix", ".Postfix", ".Finalizer" };

	[HarmonyPrepare]
	private static bool Prepare()
	{
		if (IsekaiSelfTest.Enabled)
		{
			return PatchGuard.Resolves(typeof(SelfTestEngineExceptions), typeof(LogChannel), "Exception", new Type[4]
			{
				typeof(UnityEngine.Object),
				typeof(Exception),
				typeof(string),
				typeof(object[])
			});
		}
		return false;
	}

	[HarmonyPrefix]
	private static void Prefix(Exception ex)
	{
		List<Exception> list = t_Capture;
		if (list != null && ex != null && list.Count < 50)
		{
			list.Add(ex);
		}
	}

	internal static void Begin()
	{
		t_Capture = new List<Exception>();
	}

	internal static List<Exception> End()
	{
		List<Exception> list = t_Capture;
		t_Capture = null;
		if (list == null)
		{
			list = new List<Exception>();
		}
		return list;
	}

	internal static string RaisedIn(string stackTrace)
	{
		if (string.IsNullOrEmpty(stackTrace))
		{
			return null;
		}
		string[] array = stackTrace.Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.StartsWith("at ", StringComparison.Ordinal))
			{
				text = text.Substring(3).Trim();
			}
			if (text.Length != 0)
			{
				if (text.StartsWith("(wrapper dynamic-method) ", StringComparison.Ordinal))
				{
					text = text.Substring("(wrapper dynamic-method) ".Length);
				}
				if (text.StartsWith("MonoMod.Utils.DynamicMethodDefinition.", StringComparison.Ordinal))
				{
					text = text.Substring("MonoMod.Utils.DynamicMethodDefinition.".Length);
				}
				int num = text.IndexOf('(');
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
				return text.Trim();
			}
		}
		return null;
	}

	internal static bool IsCounted(string raisedIn)
	{
		if (string.IsNullOrEmpty(raisedIn))
		{
			return true;
		}
		bool flag = false;
		string[] ownRoots = OwnRoots;
		foreach (string value in ownRoots)
		{
			if (raisedIn.StartsWith(value, StringComparison.Ordinal))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (raisedIn.StartsWith("IsekaiMod.", StringComparison.Ordinal))
		{
			return true;
		}
		ownRoots = PatchMethods;
		foreach (string value2 in ownRoots)
		{
			if (raisedIn.EndsWith(value2, StringComparison.Ordinal))
			{
				return false;
			}
		}
		return true;
	}

	internal static string Describe(string factName, Exception ex)
	{
		if (ex == null)
		{
			return factName + ": the engine caught an exception.";
		}
		Exception ex2 = ex.InnerException ?? ex;
		string text = RaisedIn(ex2.StackTrace) ?? RaisedIn(ex.StackTrace) ?? "an unknown frame";
		return factName + ": the engine caught " + ex2.GetType().Name + " in " + text + " and carried on without it: " + SelfTestReport.Clean(ex2.Message);
	}
}
