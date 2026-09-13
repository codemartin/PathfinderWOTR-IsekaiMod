using HarmonyLib;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Reports slow level-up feature refreshes and shows which features spent the
    /// most time in a full reapply. Every feature that goes through
    /// <see cref="EntityFact.Reapply"/> during the refresh is timed, so the report
    /// covers heritages, guardians and any other feature flagged with
    /// ReapplyOnLevelUp without needing a hand-maintained list.
    /// Logging happens only for slow refreshes and is throttled so the diagnostic
    /// stays cheap.
    /// </summary>
    [HarmonyPatch(typeof(UnitProgressionData), nameof(UnitProgressionData.ReapplyFeaturesOnLevelUp))]
    internal static class LevelUpPerformanceDiagnostics {
        private const double SlowRefreshMilliseconds = 25.0;
        private const int ReportedFeatureCount = 10;
        private static readonly TimeSpan LogThrottle = TimeSpan.FromSeconds(1);

        [ThreadStatic]
        private static RefreshProfile currentProfile;

        [ThreadStatic]
        private static DateTime lastLogUtc;

        [HarmonyPrefix]
        private static void Prefix() {
            currentProfile = new RefreshProfile();
        }

        [HarmonyPostfix]
        private static void Postfix(UnitProgressionData __instance) {
            var profile = currentProfile;
            currentProfile = null;
            if (profile == null) return;

            profile.Total.Stop();
            if (profile.Total.Elapsed.TotalMilliseconds < SlowRefreshMilliseconds) return;

            var now = DateTime.UtcNow;
            if (now - lastLogUtc < LogThrottle) return;
            lastLogUtc = now;

            try {
                int reapplied = profile.Features.Values.Sum(timing => timing.Count);
                string details = profile.Features.Count == 0
                    ? "no feature went through a full reapply"
                    : string.Join(", ", profile.Features
                        .OrderByDescending(entry => entry.Value.ElapsedTicks)
                        .Take(ReportedFeatureCount)
                        .Select(entry => $"{entry.Key}={TicksToMilliseconds(entry.Value.ElapsedTicks):F1}ms/{entry.Value.Count}x"));
                string owner = __instance?.Owner?.CharacterName ?? "unknown unit";
                IsekaiContext.Logger.Log(
                    $"Level-up feature refresh for {owner} took {profile.Total.Elapsed.TotalMilliseconds:F1}ms across {reapplied} reapplied feature(s); slowest: {details}.");
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Level-up diagnostics could not build a report: {ex.Message}");
            }
        }

        internal static long BeginFeature(EntityFact fact) {
            if (currentProfile == null || !(fact is Feature)) return 0;
            return Stopwatch.GetTimestamp();
        }

        internal static void EndFeature(EntityFact fact, long startedAt) {
            if (startedAt == 0 || currentProfile == null || !(fact is Feature feature)) return;
            string name = feature.Blueprint?.name;
            if (string.IsNullOrEmpty(name)) name = "(unnamed feature)";

            long elapsed = Stopwatch.GetTimestamp() - startedAt;
            if (!currentProfile.Features.TryGetValue(name, out var timing)) {
                timing = new FeatureTiming();
                currentProfile.Features.Add(name, timing);
            }
            timing.ElapsedTicks += elapsed;
            timing.Count++;
        }

        private static double TicksToMilliseconds(long ticks) {
            return ticks * 1000.0 / Stopwatch.Frequency;
        }

        private sealed class RefreshProfile {
            internal readonly Stopwatch Total = Stopwatch.StartNew();
            internal readonly Dictionary<string, FeatureTiming> Features = new Dictionary<string, FeatureTiming>(StringComparer.Ordinal);
        }

        private sealed class FeatureTiming {
            internal long ElapsedTicks;
            internal int Count;
        }
    }

    [HarmonyPatch(typeof(EntityFact), nameof(EntityFact.Reapply))]
    internal static class IsekaiFeatureReapplyTimingPatch {
        [HarmonyPrefix]
        private static void Prefix(EntityFact __instance, out long __state) {
            __state = LevelUpPerformanceDiagnostics.BeginFeature(__instance);
        }

        [HarmonyPostfix]
        private static void Postfix(EntityFact __instance, long __state) {
            LevelUpPerformanceDiagnostics.EndFeature(__instance, __state);
        }
    }
}
