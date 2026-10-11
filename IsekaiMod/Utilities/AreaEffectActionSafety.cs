using HarmonyLib;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(AbilityAreaEffectRunAction))]
internal static class AreaEffectActionSafety
{
	private static void Fill(AbilityAreaEffectRunAction instance)
	{
		if (instance != null)
		{
			instance.UnitEnter = instance.UnitEnter ?? new ActionList();
			instance.UnitExit = instance.UnitExit ?? new ActionList();
			instance.UnitMove = instance.UnitMove ?? new ActionList();
			instance.Round = instance.Round ?? new ActionList();
		}
	}

	[HarmonyPatch("OnUnitEnter")]
	[HarmonyPrefix]
	private static void EnterPrefix(AbilityAreaEffectRunAction __instance)
	{
		Fill(__instance);
	}

	[HarmonyPatch("OnUnitExit")]
	[HarmonyPrefix]
	private static void ExitPrefix(AbilityAreaEffectRunAction __instance)
	{
		Fill(__instance);
	}

	[HarmonyPatch("OnUnitMove")]
	[HarmonyPrefix]
	private static void MovePrefix(AbilityAreaEffectRunAction __instance)
	{
		Fill(__instance);
	}

	[HarmonyPatch("OnRound")]
	[HarmonyPrefix]
	private static void RoundPrefix(AbilityAreaEffectRunAction __instance)
	{
		Fill(__instance);
	}
}
