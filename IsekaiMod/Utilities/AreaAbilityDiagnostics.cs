using HarmonyLib;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary. Logs what point-targeted area abilities select and whether their action
    /// list runs, so an ability that "does nothing" can be traced without the game log.
    /// Limited to a few lines per minute.
    /// </summary>
    internal static class AreaAbilityDiagnostics {
        private const int MaxPerMinute = 30;
        private static DateTime windowStart = DateTime.MinValue;
        private static int count;

        internal static bool Allow() {
            var now = DateTime.UtcNow;
            if (now - windowStart > TimeSpan.FromMinutes(1)) {
                windowStart = now;
                count = 0;
            }
            return ++count <= MaxPerMinute;
        }

        [HarmonyPatch(typeof(AbilityTargetsAround), nameof(AbilityTargetsAround.Select))]
        internal static class SelectPostfix {
            [HarmonyPostfix]
            private static void Postfix(AbilityTargetsAround __instance, AbilityExecutionContext context, TargetWrapper anchor, ref IEnumerable<TargetWrapper> __result) {
                try {
                    var list = (__result ?? Enumerable.Empty<TargetWrapper>()).ToList();
                    __result = list;
                    if (!Allow()) return;
                    string ability = context?.AbilityBlueprint?.name ?? "(no ability)";
                    string where = anchor == null ? "no anchor" : anchor.IsUnit ? "unit " + anchor.Unit?.CharacterName : "point " + anchor.Point;
                    IsekaiContext.Logger.Log($"Area select: {ability} at {where}, radius {__instance?.m_Radius.Value:F0} ft, type {__instance?.m_TargetType}, caster {context?.MaybeCaster?.CharacterName ?? "none"} -> {list.Count} target(s)");
                } catch (Exception ex) {
                    IsekaiContext.Logger.Log($"Area select diagnostic failed: {ex.Message}");
                }
            }
        }

        [HarmonyPatch(typeof(AbilityEffectRunAction), nameof(AbilityEffectRunAction.Apply))]
        internal static class RunActionPrefix {
            [HarmonyPrefix]
            private static void Prefix(AbilityEffectRunAction __instance, AbilityExecutionContext context, TargetWrapper target) {
                try {
                    string ability = context?.AbilityBlueprint?.name ?? "";
                    if (ability.IndexOf("Breath", StringComparison.Ordinal) < 0 && ability.IndexOf("Hellfire", StringComparison.Ordinal) < 0) return;
                    if (!Allow()) return;
                    int actions = __instance?.Actions?.Actions?.Length ?? -1;
                    string who = target == null ? "null" : target.IsUnit ? target.Unit?.CharacterName : "point";
                    IsekaiContext.Logger.Log($"Area run action: {ability} on {who}, {actions} action(s), savingThrow {__instance?.SavingThrowType}");
                } catch (Exception ex) {
                    IsekaiContext.Logger.Log($"Area run action diagnostic failed: {ex.Message}");
                }
            }
        }
    }
}
