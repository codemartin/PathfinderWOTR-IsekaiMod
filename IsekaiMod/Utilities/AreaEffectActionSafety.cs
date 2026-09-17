using HarmonyLib;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.EntitySystem.Entities;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// AbilityAreaEffectRunAction assumes all four of its action lists exist; blueprints loaded from the
    /// game's JSON always have them, but a list left unset in code is null and the game then throws (and
    /// swallows) a NullReferenceException on every round tick or unit move of that aura. Several of this
    /// mod's toggle auras only set enter and exit actions. Empty lists are supplied on first use.
    /// </summary>
    [HarmonyPatch(typeof(AbilityAreaEffectRunAction))]
    internal static class AreaEffectActionSafety {
        private static void Fill(AbilityAreaEffectRunAction instance) {
            if (instance == null) return;
            instance.UnitEnter = instance.UnitEnter ?? new ActionList();
            instance.UnitExit = instance.UnitExit ?? new ActionList();
            instance.UnitMove = instance.UnitMove ?? new ActionList();
            instance.Round = instance.Round ?? new ActionList();
        }

        [HarmonyPatch("OnUnitEnter")]
        [HarmonyPrefix]
        private static void EnterPrefix(AbilityAreaEffectRunAction __instance) { Fill(__instance); }

        [HarmonyPatch("OnUnitExit")]
        [HarmonyPrefix]
        private static void ExitPrefix(AbilityAreaEffectRunAction __instance) { Fill(__instance); }

        [HarmonyPatch("OnUnitMove")]
        [HarmonyPrefix]
        private static void MovePrefix(AbilityAreaEffectRunAction __instance) { Fill(__instance); }

        [HarmonyPatch("OnRound")]
        [HarmonyPrefix]
        private static void RoundPrefix(AbilityAreaEffectRunAction __instance) { Fill(__instance); }
    }

    /// <summary>
    /// AbilityTargetsAround dereferences its condition checker unconditionally, so a component created in
    /// code without one throws when the ability is delivered and the ability appears to do nothing. Most of
    /// this mod's point-targeted area abilities (Dragon Breath, Abyssal Hellfire and others) were built that
    /// way. An empty checker is supplied before the selection runs.
    /// </summary>
    [HarmonyPatch(typeof(Kingmaker.UnitLogic.Abilities.Components.AbilityTargetsAround), "Select")]
    internal static class TargetsAroundConditionSafety {
        private static readonly AccessTools.FieldRef<Kingmaker.UnitLogic.Abilities.Components.AbilityTargetsAround, ConditionsChecker> conditionRef =
            AccessTools.FieldRefAccess<Kingmaker.UnitLogic.Abilities.Components.AbilityTargetsAround, ConditionsChecker>("m_Condition");

        [HarmonyPrefix]
        private static void Prefix(Kingmaker.UnitLogic.Abilities.Components.AbilityTargetsAround __instance) {
            if (__instance == null) return;
            ref ConditionsChecker condition = ref conditionRef(__instance);
            if (condition == null) {
                condition = ActionFlow.EmptyCondition();
            } else if (condition.Conditions == null) {
                condition.Conditions = new Condition[0];
            }
        }
    }
}
