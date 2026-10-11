using HarmonyLib;
using Kingmaker;

namespace IsekaiMod.Utilities.SelfTest;

[HarmonyPatch(typeof(MainMenu), "Start")]
internal static class SelfTestMainMenuHook
{
	[HarmonyPrepare]
	private static bool Prepare()
	{
		return IsekaiSelfTest.Enabled;
	}

	[HarmonyPostfix]
	private static void Postfix()
	{
		IsekaiSelfTest.OnMainMenu();
	}
}
