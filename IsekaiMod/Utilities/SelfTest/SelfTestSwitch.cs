using System;
using System.Collections.Generic;
using System.Globalization;

namespace IsekaiMod.Utilities.SelfTest;

public static class SelfTestSwitch
{
	public const string Argument = "-isekai-selftest";

	public const string TimeoutArgumentPrefix = "-isekai-selftest-timeout=";

	public const string EnvironmentVariable = "ISEKAI_SELFTEST";

	public const int DefaultTimeoutSeconds = 1080;

	public const int MinimumTimeoutSeconds = 30;

	public const int MaximumTimeoutSeconds = 3600;

	public static bool IsRequested(IEnumerable<string> args, string environmentValue)
	{
		if (args != null)
		{
			foreach (string arg in args)
			{
				if (arg != null)
				{
					string a = arg.Trim();
					if (string.Equals(a, "-isekai-selftest", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
					if (string.Equals(a, "--isekai-selftest", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
		}
		string a2 = environmentValue?.Trim();
		if (!string.Equals(a2, "1", StringComparison.Ordinal))
		{
			return string.Equals(a2, "true", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public static int TimeoutSeconds(IEnumerable<string> args)
	{
		int result = 1080;
		if (args == null)
		{
			return result;
		}
		foreach (string arg in args)
		{
			if (arg != null)
			{
				string text = arg.Trim();
				if (text.StartsWith("--", StringComparison.Ordinal))
				{
					text = text.Substring(1);
				}
				if (text.StartsWith("-isekai-selftest-timeout=", StringComparison.OrdinalIgnoreCase) && int.TryParse(text.Substring("-isekai-selftest-timeout=".Length), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
				{
					result = Math.Max(30, Math.Min(3600, result2));
				}
			}
		}
		return result;
	}

	public static bool IsRequestedByThisProcess()
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
			environmentValue = Environment.GetEnvironmentVariable("ISEKAI_SELFTEST");
		}
		catch
		{
			environmentValue = null;
		}
		return IsRequested(args, environmentValue);
	}
}
