using System;
using System.Linq;

namespace IsekaiMod.Utilities.SelfTest;

internal static class ClassDescriptionRules
{
	internal const int MinDescriptionLength = 100;

	internal const int MinShortDescriptionLength = 60;

	internal const int MaxDescriptionLength = 2000;

	internal static readonly string[] PlaceholderMarkers = new string[10] { "TODO", "TBD", "placeholder", "lorem ipsum", "coming soon", "not yet implemented", "description here", "<null>", "[unknown key", "milestones progression for the" };

	internal static string Problem(string name, string text, int minLength)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return "is empty";
		}
		string trimmed = text.Trim();
		if (trimmed.Length < minLength)
		{
			return $"is only {trimmed.Length} characters (at least {minLength} expected)";
		}
		if (trimmed.Length > 2000)
		{
			return $"is {trimmed.Length} characters (at most {2000})";
		}
		if (!string.IsNullOrEmpty(name) && string.Equals(trimmed, name.Trim(), StringComparison.OrdinalIgnoreCase))
		{
			return "only repeats the name";
		}
		string text2 = PlaceholderMarkers.FirstOrDefault((string m) => trimmed.IndexOf(m, StringComparison.OrdinalIgnoreCase) >= 0);
		if (text2 != null)
		{
			return "reads as a placeholder (\"" + text2 + "\")";
		}
		return null;
	}
}
