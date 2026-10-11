using System;
using System.Collections.Generic;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

internal static class ArchetypeOrigin
{
	internal const string Base = "Base";

	internal const string GodEmperor = "GodEmperor";

	internal const string MartialGod = "MartialGod";

	internal const string Hero = "Hero";

	internal const string Mastermind = "Mastermind";

	internal const string Overlord = "Overlord";

	internal const string Devourer = "Devourer";

	internal const string ShadowMonarch = "ShadowMonarch";

	private static readonly Dictionary<string, string> Display = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{ "Base", "Isekai Protagonist (Base)" },
		{ "GodEmperor", "God Emperor" },
		{ "MartialGod", "Martial God" },
		{ "Hero", "Hero" },
		{ "Mastermind", "Mastermind" },
		{ "Overlord", "Overlord" },
		{ "Devourer", "Slime" },
		{ "ShadowMonarch", "Shadow Monarch" }
	};

	private static readonly Dictionary<string, string> Aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{ "Base", "Base" },
		{ "Base Protagonist", "Base" },
		{ "Isekai Protagonist (Base)", "Base" },
		{ "GodEmperor", "GodEmperor" },
		{ "God Emperor", "GodEmperor" },
		{ "MartialGod", "MartialGod" },
		{ "Martial God", "MartialGod" },
		{ "PhantomBlade", "MartialGod" },
		{ "Phantom Blade", "MartialGod" },
		{ "EdgeLord", "MartialGod" },
		{ "Hero", "Hero" },
		{ "Mastermind", "Mastermind" },
		{ "Overlord", "Overlord" },
		{ "Devourer", "Devourer" },
		{ "Slime", "Devourer" },
		{ "ShadowMonarch", "ShadowMonarch" },
		{ "Shadow Monarch", "ShadowMonarch" }
	};

	internal static string Normalize(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return string.Empty;
		}
		string text = value.Trim();
		if (text.Length == 0)
		{
			return string.Empty;
		}
		if (!Aliases.TryGetValue(text, out var value2))
		{
			return text;
		}
		return value2;
	}

	internal static string DisplayName(string token)
	{
		string text = Normalize(token);
		if (text.Length == 0)
		{
			return string.Empty;
		}
		if (!Display.TryGetValue(text, out var value))
		{
			return text;
		}
		return value;
	}

	internal static bool Matches(string a, string b)
	{
		return string.Equals(Normalize(a), Normalize(b), StringComparison.OrdinalIgnoreCase);
	}
}
