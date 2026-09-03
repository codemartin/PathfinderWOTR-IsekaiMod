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
    /// Reports slow level-up feature refreshes and attributes time spent in the
    /// Isekai features that still require full reapplication. Logging is limited
    /// to slow calls and throttled so the diagnostic does not become the problem.
    /// </summary>
    [HarmonyPatch(typeof(UnitProgressionData), nameof(UnitProgressionData.ReapplyFeaturesOnLevelUp))]
    internal static class LevelUpPerformanceDiagnostics {
        private const double SlowRefreshMilliseconds = 25.0;
        private static readonly TimeSpan LogThrottle = TimeSpan.FromSeconds(1);

        private static readonly HashSet<string> TrackedBlueprintNames = new HashSet<string>(StringComparer.Ordinal) {
            "IsekaiAngelHeritage",
            "IsekaiWoodElfHeritage",
            "IsekaiVampireHeritage",
            "IsekaiSuccubusHeritage",
            "IsekaiHighElfHeritage",
            "IsekaiDarkElfHeritage",
            "IsekaiFurryHeritage",
            "DeathsnatcherFeature",
            "MightySummoningFeature",
            "MagicalSummoningFeature",
            "ForbiddenSummoningFeature",
            "FerociousSummoningFeature",
            "NascentApotheosis",
            "SignatureStrike",
            "AutoElementalAcidFeature",
            "AutoElementalColdFeature",
            "AutoElementalElectricityFeature",
            "AutoElementalFireFeature",
        };

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

            string details = profile.Features.Count == 0
                ? "no tracked Isekai feature used full reapply"
                : string.Join(", ", profile.Features
                    .OrderByDescending(entry => entry.Value.ElapsedTicks)
                    .Select(entry => $"{entry.Key}={TicksToMilliseconds(entry.Value.ElapsedTicks):F1}ms/{entry.Value.Count}x"));
            string owner = __instance?.Owner?.CharacterName ?? "unknown unit";
            IsekaiContext.Logger.Log(
                $"Level-up feature refresh for {owner} took {profile.Total.Elapsed.TotalMilliseconds:F1}ms; {details}.");
        }

        internal static long BeginFeature(EntityFact fact) {
            if (currentProfile == null || !(fact is Feature feature)) return 0;
            string name = feature.Blueprint?.name;
            return name != null && TrackedBlueprintNames.Contains(name)
                ? Stopwatch.GetTimestamp()
                : 0;
        }

        internal static void EndFeature(EntityFact fact, long startedAt) {
            if (startedAt == 0 || currentProfile == null || !(fact is Feature feature)) return;
            string name = feature.Blueprint?.name;
            if (name == null) return;

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
            internal readonly Dictionary<string, FeatureTiming> Features = new Dictionary<string, FeatureTiming>();
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
