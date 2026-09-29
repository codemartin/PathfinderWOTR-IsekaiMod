using HarmonyLib;
using Kingmaker.AreaLogic.SummonPool;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using System;
using System.Linq;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic: Shadow Extraction soldiers vanish shortly after being summoned. The first
    /// round of logging showed the unit being destroyed by the destruction controller with ~600 s of
    /// summon time left, which only says the unit had been marked for destruction earlier. This traces the
    /// mark itself: MarkForDestroy on any unit in the ShadowExtractionPool is logged with the call stack.
    /// The pool-removal hook is kept, but now only for units that are in the pool.
    /// </summary>
    internal static class ShadowSummonDiagnostics {
        private static bool InShadowPool(UnitEntityData unit) {
            try {
                if (unit == null) return false;
                return unit.Get<SummonPool.PooledPart>()?.Pools?.Any(bp => bp != null && bp.name == "ShadowExtractionPool") == true;
            } catch { return false; }
        }

        [HarmonyPatch(typeof(EntityDataBase), nameof(EntityDataBase.MarkForDestroy))]
        private static class MarkForDestroyTrace {
            private static void Prefix(EntityDataBase __instance) {
                try {
                    if (!(__instance is UnitEntityData unit) || !InShadowPool(unit)) return;
                    var summoned = unit.Descriptor?.Buffs?.GetBuff(Kingmaker.Blueprints.Root.BlueprintRoot.Instance.SystemMechanics.SummonedUnitBuff);
                    string remaining = summoned == null ? "no summon buff" : $"summon buff {summoned.TimeLeft.TotalSeconds:F0}s left";
                    IsekaiContext.Logger.Log($"Shadow soldier {unit.CharacterName} marked for destruction (dead={unit.Descriptor?.State?.IsDead}, {remaining}). Stack:\n{Environment.StackTrace}");
                } catch (Exception ex) {
                    IsekaiContext.Logger.Log("Shadow summon diagnostics failed: " + ex.Message);
                }
            }
        }

        [HarmonyPatch(typeof(SummonPool), nameof(SummonPool.Unregister))]
        private static class UnregisterTrace {
            private static void Prefix(SummonPool __instance, UnitEntityData unit) {
                try {
                    if (__instance?.Blueprint?.name != "ShadowExtractionPool" || unit == null) return;
                    if (!__instance.Units.Contains(unit)) return;
                    var summoned = unit.Descriptor?.Buffs?.GetBuff(Kingmaker.Blueprints.Root.BlueprintRoot.Instance.SystemMechanics.SummonedUnitBuff);
                    string remaining = summoned == null ? "no summon buff" : $"summon buff {summoned.TimeLeft.TotalSeconds:F0}s left";
                    IsekaiContext.Logger.Log($"Shadow soldier {unit.CharacterName} removed from ShadowExtractionPool (dead={unit.Descriptor?.State?.IsDead}, {remaining}). Stack:\n{Environment.StackTrace}");
                } catch (Exception ex) {
                    IsekaiContext.Logger.Log("Shadow summon diagnostics failed: " + ex.Message);
                }
            }
        }
    }
}
