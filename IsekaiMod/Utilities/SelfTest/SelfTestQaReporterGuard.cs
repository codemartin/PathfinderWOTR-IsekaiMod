using System;
using System.Reflection;
using HarmonyLib;
using Kingmaker.Cheats;
using Kingmaker.QA;

namespace IsekaiMod.Utilities.SelfTest;

[HarmonyPatch(typeof(QAModeExceptionReporter), "MaybeShowError")]
internal static class SelfTestQaReporterGuard
{
	private static readonly PropertyInfo QaModeProperty = AccessTools.Property(typeof(CheatsJira), "QaMode");

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return IsekaiSelfTest.Enabled;
	}

	internal static bool MayRun(object qaModeVariable)
	{
		return qaModeVariable != null;
	}

	[HarmonyPrefix]
	private static bool Prefix()
	{
		try
		{
			return MayRun(QaModeProperty?.GetValue(null));
		}
		catch (Exception)
		{
			return false;
		}
	}
}
