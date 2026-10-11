using HarmonyLib;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(AbilityTargetsAround), "Select")]
internal static class TargetsAroundConditionSafety
{
	private static readonly AccessTools.FieldRef<AbilityTargetsAround, ConditionsChecker> conditionRef = AccessTools.FieldRefAccess<AbilityTargetsAround, ConditionsChecker>("m_Condition");

	[HarmonyPrefix]
	private static void Prefix(AbilityTargetsAround __instance)
	{
		if (__instance != null)
		{
			ref ConditionsChecker reference = ref conditionRef(__instance);
			if (reference == null)
			{
				reference = ActionFlow.EmptyCondition();
			}
			else if (reference.Conditions == null)
			{
				reference.Conditions = new Condition[0];
			}
		}
	}
}
