using System;
using System.Collections.Generic;
using System.Globalization;

namespace IsekaiMod.Utilities.SelfTest;

public static class SaveHarnessSwitch
{
	public const string SaveArgumentPrefix = "-isekai-selftest-save=";

	public const string SaveTimeoutArgumentPrefix = "-isekai-selftest-save-timeout=";

	public const string SaveEnvironmentVariable = "ISEKAI_SELFTEST_SAVE";

	public const int DefaultSaveTimeoutSeconds = 900;

	public const int MinimumSaveTimeoutSeconds = 60;

	public const int MaximumSaveTimeoutSeconds = 2400;

	public static string RequestedSave(IEnumerable<string> args, string environmentValue)
	{
		string text = null;
		if (args != null)
		{
			foreach (string arg in args)
			{
				string text2 = ValueOf(arg, "-isekai-selftest-save=");
				if (text2 != null)
				{
					text = text2;
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		string text3 = Unquote(environmentValue);
		if (!string.IsNullOrEmpty(text3))
		{
			return text3;
		}
		return null;
	}

	public static bool IsRequested(IEnumerable<string> args, string environmentValue)
	{
		return RequestedSave(args, environmentValue) != null;
	}

	public static int SaveTimeoutSeconds(IEnumerable<string> args)
	{
		int result = 900;
		if (args == null)
		{
			return result;
		}
		foreach (string arg in args)
		{
			string text = ValueOf(arg, "-isekai-selftest-save-timeout=");
			if (text != null && int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
			{
				result = Math.Max(60, Math.Min(2400, result2));
			}
		}
		return result;
	}

	private static string ValueOf(string raw, string prefix)
	{
		if (raw == null)
		{
			return null;
		}
		string text = raw.Trim();
		if (text.StartsWith("--", StringComparison.Ordinal))
		{
			text = text.Substring(1);
		}
		if (!text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		string text2 = Unquote(text.Substring(prefix.Length));
		if (!string.IsNullOrEmpty(text2))
		{
			return text2;
		}
		return null;
	}

	public static string Unquote(string text)
	{
		string text2 = (text ?? "").Trim();
		if (text2.Length >= 2 && text2[0] == '"' && text2[text2.Length - 1] == '"')
		{
			text2 = text2.Substring(1, text2.Length - 2).Trim();
		}
		return text2;
	}

	public static string RequestedSaveForThisProcess()
	{
		string[] args;
		try
		{
			args = Environment.GetCommandLineArgs();
		}
		catch
		{
			args = new string[0];
		}
		string environmentValue;
		try
		{
			environmentValue = Environment.GetEnvironmentVariable("ISEKAI_SELFTEST_SAVE");
		}
		catch
		{
			environmentValue = null;
		}
		return RequestedSave(args, environmentValue);
	}

	public static int SaveTimeoutSecondsForThisProcess()
	{
		try
		{
			return SaveTimeoutSeconds(Environment.GetCommandLineArgs());
		}
		catch
		{
			return 900;
		}
	}
}
