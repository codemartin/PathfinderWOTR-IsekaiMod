using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using TabletopTweaks.Core.ModLogic;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(CriticalConfirmationBonus), "OnEventAboutToTrigger", new Type[] { typeof(RuleAttackRoll) })]
internal static class CriticalConfirmationBonusValueSafety
{
	private static readonly HashSet<string> reported = new HashSet<string>();

	[HarmonyPrefix]
	private static void Prefix(CriticalConfirmationBonus __instance)
	{
		if (__instance != null && __instance.Value == null)
		{
			__instance.Value = 0;
			string text = __instance.OwnerBlueprint?.name ?? "(unknown blueprint)";
			if (reported.Add(text))
			{
				((ModContextBase)Main.IsekaiContext).Logger.Log($"CriticalConfirmationBonus on {text} had no Value; using 0 so its flat bonus of {__instance.Bonus} applies.");
			}
		}
	}
}
