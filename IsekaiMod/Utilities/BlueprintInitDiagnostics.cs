using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic for the startup stall. The frame that runs BlueprintsCache.Init took 170 s on one
    /// start and 490 s on the next; every installed mod builds its blueprints in a patch on that method, and the
    /// stall trace cannot say which. This runs as the very first prefix of Init: it looks up every other patch on
    /// Init that Harmony knows about and wraps each one in a stopwatch, then logs each patch that took more than
    /// a second with its owner id and assembly, and the whole Init with the total. The wrapped methods are the
    /// other mods' plain static patch methods; nothing about their order or behaviour changes.
    /// </summary>
    [HarmonyPatch(typeof(BlueprintsCache), "Init")]
    internal static class BlueprintInitDiagnostics {
        private const double ReportMs = 1000.0;
        private static readonly Harmony timer = new Harmony("IsekaiMod.BlueprintInitDiagnostics");
        private static readonly List<string> slow = new List<string>();
        private static long initStart;
        private static bool installed;

        [HarmonyPriority(Priority.First)]
        [HarmonyPrefix]
        private static void Prefix() {
            initStart = Stopwatch.GetTimestamp();
            if (installed) return;
            installed = true;
            int wrapped = 0, failed = 0;
            try {
                MethodBase init = AccessTools.Method(typeof(BlueprintsCache), "Init");
                Patches info = Harmony.GetPatchInfo(init);
                if (info == null) { IsekaiContext.Logger.Log("Blueprint init diagnostics: no patch info."); return; }
                HarmonyMethod pre = new HarmonyMethod(typeof(BlueprintInitDiagnostics), nameof(TimedPrefix));
                HarmonyMethod post = new HarmonyMethod(typeof(BlueprintInitDiagnostics), nameof(TimedPostfix));
                foreach (Patch patch in info.Prefixes.Concat(info.Postfixes).Concat(info.Finalizers)) {
                    MethodInfo m = patch.PatchMethod;
                    if (m == null || m.DeclaringType == typeof(BlueprintInitDiagnostics)) continue;
                    try { timer.Patch(m, prefix: pre, postfix: post); wrapped++; } catch (Exception) { failed++; }
                }
                IsekaiContext.Logger.Log($"Blueprint init diagnostics: timing {wrapped} Init patch method(s) from other mods ({failed} could not be wrapped).");
            } catch (Exception ex) {
                IsekaiContext.Logger.Log("Blueprint init diagnostics could not be installed: " + ex.Message);
            }
        }

        [HarmonyPriority(Priority.Last)]
        [HarmonyPostfix]
        private static void Postfix() {
            try {
                double total = (Stopwatch.GetTimestamp() - initStart) * 1000.0 / Stopwatch.Frequency;
                IsekaiContext.Logger.Log($"Blueprint init: {total / 1000.0:F1} s in total; patches over {ReportMs / 1000.0:F0} s: " + (slow.Count == 0 ? "none" : string.Join("; ", slow)));
                slow.Clear();
            } catch (Exception ex) {
                IsekaiContext.Logger.Log("Blueprint init diagnostics failed: " + ex.Message);
            }
        }

        private static void TimedPrefix(out long __state) { __state = Stopwatch.GetTimestamp(); }

        private static void TimedPostfix(MethodBase __originalMethod, long __state) {
            double ms = (Stopwatch.GetTimestamp() - __state) * 1000.0 / Stopwatch.Frequency;
            if (ms < ReportMs) return;
            Type t = __originalMethod?.DeclaringType;
            string what = $"{t?.FullName ?? "?"}.{__originalMethod?.Name} ({t?.Assembly?.GetName().Name ?? "?"}) {ms / 1000.0:F1} s";
            slow.Add(what);
            IsekaiContext.Logger.Log("Blueprint init patch: " + what);
        }
    }
}
