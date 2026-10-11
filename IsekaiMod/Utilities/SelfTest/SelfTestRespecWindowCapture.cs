using HarmonyLib;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Utilities.SelfTest;

[HarmonyPatch(typeof(LevelUpConfig), "OpenUI")]
internal static class SelfTestRespecWindowCapture
{
	internal static bool Armed;

	internal static LevelUpConfig Captured;

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return IsekaiSelfTest.Enabled;
	}

	[HarmonyPrefix]
	private static bool Prefix(LevelUpConfig __instance)
	{
		if (!Armed)
		{
			return true;
		}
		Captured = __instance;
		return false;
	}
}
