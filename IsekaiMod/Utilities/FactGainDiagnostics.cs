using HarmonyLib;
using Kingmaker.EntitySystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic for slow fact grants. Buying a boon in the Cosmic Store (a plain AddFact) has
    /// been seen to block the game for 7 to 33 seconds, and Avatar ascension for 24 seconds. Three things
    /// are timed so the cost can be attributed:
    ///   - EntityFact.Attach and EntityFact.TurnOn, by the fact's blueprint (covers the fact's own components);
    ///   - EntityFactComponent.Activate and TurnOn, by component type and owning blueprint, when slow;
    ///   - every IUnitGainFactHandler.HandleUnitGainFact implementation in any loaded assembly, by class and
    ///     assembly (this is where other mods react to a fact being gained).
    /// Calls above the thresholds are logged. All patched methods are plain non-generic instance methods.
    /// </summary>
    internal static class FactGainDiagnostics {
        private const double FactMs = 100.0;
        private const double ComponentMs = 100.0;
        private const double HandlerMs = 50.0;

        internal static void Install(Harmony harmony) {
            int patched = 0, failed = 0;
            try {
                foreach (string name in new[] { "Attach", "TurnOn" }) {
                    MethodInfo m = typeof(EntityFact).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .FirstOrDefault(x => x.Name == name && x.DeclaringType == typeof(EntityFact));
                    if (m != null) { try { harmony.Patch(m, prefix: new HarmonyMethod(typeof(FactGainDiagnostics), nameof(TimedPrefix)), postfix: new HarmonyMethod(typeof(FactGainDiagnostics), nameof(FactPostfix))); patched++; } catch { failed++; } }
                }
                foreach (string name in new[] { "Activate", "TurnOn" }) {
                    MethodInfo m = typeof(EntityFactComponent).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .FirstOrDefault(x => x.Name == name && x.DeclaringType == typeof(EntityFactComponent) && x.GetParameters().Length == 0);
                    if (m != null) { try { harmony.Patch(m, prefix: new HarmonyMethod(typeof(FactGainDiagnostics), nameof(TimedPrefix)), postfix: new HarmonyMethod(typeof(FactGainDiagnostics), nameof(ComponentPostfix))); patched++; } catch { failed++; } }
                }

                HarmonyMethod hPrefix = new HarmonyMethod(typeof(FactGainDiagnostics), nameof(TimedPrefix));
                HarmonyMethod hPostfix = new HarmonyMethod(typeof(FactGainDiagnostics), nameof(HandlerPostfix));
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    if (assembly.IsDynamic) continue;
                    Type[] types;
                    try { types = assembly.GetTypes(); } catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); } catch (Exception) { continue; }
                    foreach (Type type in types) {
                        if (type == null || type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition) continue;
                        bool relevant;
                        try { relevant = type.GetInterfaces().Any(i => i.Name == "IUnitGainFactHandler" || i.Name == "IUnitLostFactHandler"); } catch (Exception) { continue; }
                        if (!relevant) continue;
                        foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)) {
                            if (method.IsAbstract || method.IsGenericMethodDefinition || method.GetParameters().Length != 1) continue;
                            if (method.Name != "HandleUnitGainFact" && method.Name != "HandleUnitLostFact") continue;
                            try { harmony.Patch(method, prefix: hPrefix, postfix: hPostfix); patched++; } catch (Exception) { failed++; }
                        }
                    }
                }
                IsekaiContext.Logger.Log($"Fact gain diagnostics: {patched} method(s) timed ({failed} could not be patched).");
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Fact gain diagnostics could not be installed: {ex.Message}");
            }
        }

        private static void TimedPrefix(out long __state) { __state = Stopwatch.GetTimestamp(); }

        private static double Ms(long start) => (Stopwatch.GetTimestamp() - start) * 1000.0 / Stopwatch.Frequency;

        private static void FactPostfix(EntityFact __instance, MethodBase __originalMethod, long __state) {
            double ms = Ms(__state);
            if (ms < FactMs) return;
            string what = $"fact {__instance?.Blueprint?.name ?? "?"} on {(__instance?.Owner as Kingmaker.EntitySystem.Entities.UnitEntityData)?.CharacterName ?? "?"}.{__originalMethod?.Name} took {ms:F0}ms";
            IsekaiContext.Logger.Log("Fact timing: " + what);
            FrameStallDiagnostics.Note(what);
        }

        private static void ComponentPostfix(EntityFactComponent __instance, MethodBase __originalMethod, long __state) {
            double ms = Ms(__state);
            if (ms < ComponentMs) return;
            string what = $"component {__instance?.SourceBlueprintComponent?.GetType().Name ?? __instance?.GetType().Name ?? "?"} of {__instance?.Fact?.Blueprint?.name ?? "?"}.{__originalMethod?.Name} took {ms:F0}ms";
            IsekaiContext.Logger.Log("Fact timing: " + what);
            FrameStallDiagnostics.Note(what);
        }

        private static void HandlerPostfix(object __instance, MethodBase __originalMethod, object[] __args, long __state) {
            double ms = Ms(__state);
            if (ms < HandlerMs) return;
            EntityFact fact = __args != null && __args.Length > 0 ? __args[0] as EntityFact : null;
            Type type = __instance?.GetType() ?? __originalMethod?.DeclaringType;
            string what = $"handler {type?.FullName ?? "?"} ({type?.Assembly?.GetName().Name ?? "?"}).{__originalMethod?.Name} for {fact?.Blueprint?.name ?? "?"} took {ms:F0}ms";
            IsekaiContext.Logger.Log("Fact timing: " + what);
            FrameStallDiagnostics.Note(what);
        }
    }
}
