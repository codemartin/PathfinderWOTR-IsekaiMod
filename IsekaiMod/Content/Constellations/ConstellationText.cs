using System;
using System.Collections.Generic;
using System.Globalization;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using TabletopTweaks.Core.Localization;

namespace IsekaiMod.Content.Constellations;

internal static class ConstellationText
{
	private static readonly Dictionary<string, string> s_Memo = new Dictionary<string, string>(StringComparer.Ordinal);

	private static object s_MemoPack;

	private static Locale s_MemoLocale;

	internal static string Localized(string simpleName, string english)
	{
		return LocalizeIfKnown(english);
	}

	internal static string LocalizeIfKnown(string english)
	{
		if (string.IsNullOrEmpty(english))
		{
			return english;
		}
		try
		{
			MultiLocalizationPack multiLocalizationPack = Main.IsekaiContext?.ModLocalizationPack;
			if (multiLocalizationPack == null)
			{
				return english;
			}
			Locale currentLocale = LocalizationManager.CurrentLocale;
			lock (s_Memo)
			{
				if (s_MemoPack != multiLocalizationPack || s_MemoLocale != currentLocale)
				{
					s_Memo.Clear();
					s_MemoPack = multiLocalizationPack;
					s_MemoLocale = currentLocale;
				}
				if (s_Memo.TryGetValue(english, out var value))
				{
					return value;
				}
			}
			string text = english;
			if (multiLocalizationPack.TryGetText(english, out var result) && result != null)
			{
				string text2 = result.LocalizedString?.ToString();
				if (!string.IsNullOrEmpty(text2))
				{
					text = text2;
				}
			}
			lock (s_Memo)
			{
				s_Memo[english] = text;
			}
			return text;
		}
		catch
		{
			return english;
		}
	}

	internal static void ClearMemo()
	{
		lock (s_Memo)
		{
			s_Memo.Clear();
			s_MemoPack = null;
		}
	}

	internal static string LocalizedFormat(string simpleName, string englishFormat, params object[] args)
	{
		if (string.IsNullOrEmpty(englishFormat))
		{
			return englishFormat;
		}
		if (args == null || args.Length == 0)
		{
			return Localized(simpleName, englishFormat);
		}
		string format = Localized(simpleName, englishFormat);
		try
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}
		catch (FormatException)
		{
			try
			{
				return string.Format(CultureInfo.InvariantCulture, englishFormat, args);
			}
			catch (FormatException)
			{
				return englishFormat;
			}
		}
	}
}
