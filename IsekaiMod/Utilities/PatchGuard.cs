using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IsekaiMod.Utilities;

internal static class PatchGuard
{
	private const BindingFlags AnyMember = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	internal static bool MethodExists(Type declaringType, string methodName, Type[] argumentTypes = null)
	{
		if (declaringType == null || string.IsNullOrEmpty(methodName))
		{
			return false;
		}
		try
		{
			Type type = declaringType;
			while (type != null)
			{
				MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					if (!(methodInfo.Name != methodName))
					{
						if (argumentTypes == null)
						{
							return true;
						}
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (parameters.Length == argumentTypes.Length && parameters.Select((ParameterInfo p) => p.ParameterType).SequenceEqual(argumentTypes))
						{
							return true;
						}
					}
				}
				type = type.BaseType;
			}
		}
		catch
		{
			return false;
		}
		return false;
	}

	internal static bool Resolves(Type patchClass, Type declaringType, string methodName, Type[] argumentTypes = null)
	{
		if (MethodExists(declaringType, methodName, argumentTypes))
		{
			return true;
		}
		Report(patchClass, new string[1] { Describe(declaringType, methodName, argumentTypes) });
		return false;
	}

	internal static bool ResolvesAll(Type patchClass, params (Type DeclaringType, string MethodName)[] targets)
	{
		List<string> list = (from t in targets ?? new(Type, string)[0]
			where !MethodExists(t.DeclaringType, t.MethodName)
			select Describe(t.DeclaringType, t.MethodName, null)).ToList();
		if (list.Count == 0)
		{
			return true;
		}
		Report(patchClass, list);
		return false;
	}

	internal static string FailureLine(Type patchClass, IEnumerable<string> missing)
	{
		return "[PatchGuard] " + (patchClass?.FullName ?? "(unknown patch class)") + " was not applied: " + string.Join(", ", missing ?? new string[0]) + " no longer exists in the game or the mod it patches. Everything else still loads; only this patch is off.";
	}

	private static string Describe(Type declaringType, string methodName, Type[] argumentTypes)
	{
		string text = ((argumentTypes == null) ? "" : ("(" + string.Join(", ", argumentTypes.Select((Type a) => a?.Name ?? "?")) + ")"));
		return (declaringType?.FullName ?? "(missing type)") + "." + (methodName ?? "?") + text;
	}

	private static void Report(Type patchClass, IEnumerable<string> missing)
	{
		try
		{
			Main.LogError(FailureLine(patchClass, missing));
		}
		catch
		{
		}
	}
}
