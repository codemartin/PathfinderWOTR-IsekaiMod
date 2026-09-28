using HarmonyLib;
using Kingmaker.AreaLogic.SummonPool;
using Kingmaker.EntitySystem.Entities;
using System;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic: Shadow Extraction soldiers vanish shortly after being summoned. Every removal
    /// from the ShadowExtractionPool is logged with the unit, its remaining summon time and the call stack,
    /// so the log says who unsummoned it.
    /// </summary>
    [HarmonyPatch(typeof(SummonPool), nameof(SummonPool.Unregister))]
    internal static class ShadowSummonDiagnostics {
        private static void Prefix(SummonPool __instance, UnitEntityData unit) {
            try {
                if (__instance?.Blueprint?.name != "ShadowExtractionPool" || unit == null) return;
                var summoned = unit.Descriptor?.Buffs?.GetBuff(Kingmaker.Blueprints.Root.BlueprintRoot.Instance.SystemMechanics.SummonedUnitBuff);
                string remaining = summoned == null ? "no summon buff" : $"summon buff {summoned.TimeLeft.TotalSeconds:F0}s left";
                IsekaiContext.Logger.Log($"Shadow soldier {unit.CharacterName} removed from ShadowExtractionPool (dead={unit.Descriptor?.State?.IsDead}, {remaining}). Stack:\n{Environment.StackTrace}");
            } catch (Exception ex) {
                IsekaiContext.Logger.Log("Shadow summon diagnostics failed: " + ex.Message);
            }
        }
    }
}
