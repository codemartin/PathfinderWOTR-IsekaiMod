using System;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;

namespace IsekaiMod.Utilities;

public static class ElementSafety
{
	private static readonly object Gate = new object();

	private static readonly Dictionary<string, int> FailuresBySite = new Dictionary<string, int>(StringComparer.Ordinal);

	public static void Run(Element site, Action body)
	{
		if (body == null)
		{
			return;
		}
		try
		{
			body();
		}
		catch (Exception e)
		{
			Report(site, "RunAction", e);
		}
	}

	public static bool Check(Element site, Func<bool> body, bool safeResult)
	{
		if (body == null)
		{
			return safeResult;
		}
		try
		{
			return body();
		}
		catch (Exception e)
		{
			Report(site, "CheckCondition", e);
			return safeResult;
		}
	}

	internal static bool BeforeNot(bool wanted, bool not)
	{
		if (!not)
		{
			return wanted;
		}
		return !wanted;
	}

	internal static bool ShouldLog(int count)
	{
		if (count <= 1)
		{
			return count == 1;
		}
		int num = count;
		while (num % 10 == 0)
		{
			num /= 10;
		}
		return num == 1;
	}

	internal static string Describe(Element site)
	{
		if (site == null)
		{
			return "(an element that could not be read)";
		}
		string fullName = site.GetType().FullName;
		string text = "";
		string text2 = "";
		try
		{
			text = site.name ?? "";
		}
		catch
		{
		}
		try
		{
			SimpleBlueprint owner = site.Owner;
			if (owner != null)
			{
				text2 = $"{owner.name} ({owner.AssetGuid})";
			}
		}
		catch
		{
		}
		string text3 = fullName;
		if (text.Length > 0)
		{
			text3 = text3 + " '" + text + "'";
		}
		if (text2.Length > 0)
		{
			text3 = text3 + " on " + text2;
		}
		return text3;
	}

	private static void Report(Element site, string phase, Exception e)
	{
		try
		{
			string text = Describe(site);
			string key = text + "|" + phase;
			int value;
			lock (Gate)
			{
				FailuresBySite.TryGetValue(key, out value);
				value++;
				FailuresBySite[key] = value;
			}
			if (ShouldLog(value))
			{
				string text2 = ((value == 1) ? "" : $" (failure {value} at this site)");
				Main.IsekaiContext?.Logger?.LogError("[ElementSafety] " + text + " threw during " + phase + text2 + " and was stopped so the game keeps running; " + $"what it was meant to do did not happen. {e}");
				if (Main.IsekaiContext?.Logger == null)
				{
					Main.Log($"ERROR: [ElementSafety] {text} threw during {phase}{text2}. {e}");
				}
			}
		}
		catch
		{
		}
	}
}
