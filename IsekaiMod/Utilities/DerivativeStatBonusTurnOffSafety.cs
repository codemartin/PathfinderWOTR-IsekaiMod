using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using TabletopTweaks.Core.ModLogic;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(DerivativeStatBonus), "OnTurnOff")]
internal static class DerivativeStatBonusTurnOffSafety
{
	private static readonly HashSet<string> reported = new HashSet<string>();

	[HarmonyPrefix]
	private static bool Prefix(DerivativeStatBonus __instance)
	{
		if (((EntityFactComponentDelegate<UnitEntityData, EmptyComponentData>)__instance)?.Owner?.Stats == null)
		{
			return true;
		}
		if (((EntityFactComponentDelegate<UnitEntityData, EmptyComponentData>)__instance).Owner.Stats.GetStat(__instance.DerivativeStat) != null)
		{
			return true;
		}
		string text = __instance.OwnerBlueprint?.name ?? "(unknown blueprint)";
		if (reported.Add(text))
		{
			((ModContextBase)Main.IsekaiContext).Logger.Log($"DerivativeStatBonus on {text} points at {__instance.DerivativeStat}, which {((EntityFactComponentDelegate<UnitEntityData, EmptyComponentData>)__instance).Owner.CharacterName} does not have; skipped removal.");
		}
		return false;
	}
}
