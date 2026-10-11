using System;
using System.Text.RegularExpressions;

namespace IsekaiMod.Utilities.SelfTest;

public static class SelfTestLogScan
{
	private static readonly Regex ExceptionWord = new Regex("\\w*exception\\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	public static bool IsModExceptionLine(string line)
	{
		if (string.IsNullOrEmpty(line))
		{
			return false;
		}
		if (!line.StartsWith("[IsekaiMod]", StringComparison.Ordinal))
		{
			return false;
		}
		return ExceptionWord.IsMatch(line);
	}

	public static bool IsModException(string condition, string stackTrace, bool isExceptionType, bool isErrorType)
	{
		if ((condition + "\n" + stackTrace).IndexOf("IsekaiMod", StringComparison.Ordinal) < 0)
		{
			return false;
		}
		if (isExceptionType)
		{
			return true;
		}
		if (isErrorType)
		{
			return ExceptionWord.IsMatch(condition ?? "");
		}
		return false;
	}
}
