using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic for slow area loads. Every concrete class in any loaded assembly that implements
    /// one of the area event interfaces (IAreaHandler, IAreaLoadingStagesHandler, IAreaActivationHandler) gets
    /// its handler methods timed. A call over ReportMs is logged with the class, its assembly and the time, so
    /// a 90 second load can be attributed to the mod that spends it. Nothing here changes behaviour; the
    /// patches are plain non-generic instance methods, so the WeakResourceLink lesson from r74 does not apply.
    /// </summary>
    internal static class AreaLoadDiagnostics {
        private const double ReportMs = 50.0;
        private static readonly string[] MethodNames = { "OnAreaDidLoad", "OnAreaBeginUnloading", "OnAreaScenesLoaded", "OnAreaLoadingComplete", "OnAreaActivated" };
        private static readonly string[] InterfaceNames = { "IAreaHandler", "IAreaLoadingStagesHandler", "IAreaActivationHandler" };

        internal static void Install(Harmony harmony) {
            int patched = 0, failed = 0;
            try {
                HarmonyMethod prefix = new HarmonyMethod(typeof(AreaLoadDiagnostics), nameof(Prefix));
                HarmonyMethod postfix = new HarmonyMethod(typeof(AreaLoadDiagnostics), nameof(Postfix));
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    if (assembly.IsDynamic) continue;
                    Type[] types;
                    try { types = assembly.GetTypes(); } catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); } catch (Exception) { continue; }
                    foreach (Type type in types) {
                        if (type == null || type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition) continue;
                        bool relevant;
                        try { relevant = type.GetInterfaces().Any(i => InterfaceNames.Contains(i.Name)); } catch (Exception) { continue; }
                        if (!relevant) continue;
                        foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)) {
                            if (method.IsAbstract || method.IsGenericMethodDefinition || method.GetParameters().Length != 0) continue;
                            if (!MethodNames.Contains(method.Name)) continue;
                            try { harmony.Patch(method, prefix: prefix, postfix: postfix); patched++; } catch (Exception) { failed++; }
                        }
                    }
                }
                IsekaiContext.Logger.Log($"Area load diagnostics: timing {patched} area handler method(s) ({failed} could not be patched); calls over {ReportMs:F0}ms are logged.");
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Area load diagnostics could not be installed: {ex.Message}");
            }
        }

        private static void Prefix(out long __state) {
            __state = Stopwatch.GetTimestamp();
        }

        private static void Postfix(object __instance, MethodBase __originalMethod, long __state) {
            double ms = (Stopwatch.GetTimestamp() - __state) * 1000.0 / Stopwatch.Frequency;
            if (ms < ReportMs) return;
            Type type = __instance?.GetType() ?? __originalMethod?.DeclaringType;
            string what = $"{type?.FullName ?? "?"} ({type?.Assembly?.GetName().Name ?? "?"}).{__originalMethod?.Name ?? "?"} took {ms:F0}ms";
            IsekaiContext.Logger.Log("Area handler: " + what);
            FrameStallDiagnostics.Note("area handler " + what);
        }
    }
}
