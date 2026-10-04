using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic for the level-up screen freezing 8 to 17 seconds per click on a character with
    /// ~2700 features. The feature reapply at the end of each preview rebuild is already timed (about 0.45 s),
    /// so the rest of the rebuild is where the time goes. This times the whole LevelUpController.UpdatePreview,
    /// and inside it: disposing the old preview unit, creating the new one (a full copy of the character),
    /// each level-up action's Apply (by action type and feature), and every IUnitLevelUpHandler in any loaded
    /// assembly (where other mods react to a level-up). One summary line per rebuild. All patched methods are
    /// plain non-generic instance methods.
    /// </summary>
    internal static class LevelUpPreviewDiagnostics {
        private const double ReportMs = 250.0;
        private const int Top = 12;

        [ThreadStatic] private static Profile current;

        internal static void Install(Harmony harmony) {
            int patched = 0, failed = 0;
            try {
                HarmonyMethod pre = new HarmonyMethod(typeof(LevelUpPreviewDiagnostics), nameof(TimedPrefix));
                HarmonyMethod post = new HarmonyMethod(typeof(LevelUpPreviewDiagnostics), nameof(HandlerPostfix));
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    if (assembly.IsDynamic) continue;
                    Type[] types;
                    try { types = assembly.GetTypes(); } catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); } catch (Exception) { continue; }
                    foreach (Type type in types) {
                        if (type == null || type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition) continue;
                        bool relevant;
                        try { relevant = type.GetInterfaces().Any(i => i.Name == "IUnitLevelUpHandler" || i.Name == "ILevelUpCompleteUIHandler" || i.Name == "IUnitGainLevelHandler"); } catch (Exception) { continue; }
                        if (!relevant) continue;
                        foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)) {
                            if (method.IsAbstract || method.IsGenericMethodDefinition) continue;
                            if (!method.Name.StartsWith("HandleUnit") && !method.Name.StartsWith("HandleLevelUp")) continue;
                            try { harmony.Patch(method, prefix: pre, postfix: post); patched++; } catch (Exception) { failed++; }
                        }
                    }
                }
                IsekaiContext.Logger.Log($"Level-up preview diagnostics: {patched} level-up handler method(s) timed ({failed} could not be patched).");
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Level-up preview diagnostics could not be installed: {ex.Message}");
            }
        }

        private static void TimedPrefix(out long __state) { __state = Stopwatch.GetTimestamp(); }

        private static void HandlerPostfix(object __instance, MethodBase __originalMethod, long __state) {
            Type type = __instance?.GetType() ?? __originalMethod?.DeclaringType;
            Record("handler " + (type?.Name ?? "?") + " (" + (type?.Assembly?.GetName().Name ?? "?") + ")." + __originalMethod?.Name, __state);
        }

        private static double Ms(long start) => (Stopwatch.GetTimestamp() - start) * 1000.0 / Stopwatch.Frequency;

        private static void Record(string key, long start) {
            Profile p = current;
            if (p == null) return;
            long elapsed = Stopwatch.GetTimestamp() - start;
            if (!p.Items.TryGetValue(key, out Item item)) { item = new Item(); p.Items[key] = item; }
            item.Ticks += elapsed;
            item.Count++;
        }

        [HarmonyPatch(typeof(LevelUpController), "UpdatePreview")]
        private static class UpdatePreviewTrace {
            private static void Prefix(out Profile __state) {
                __state = null;
                try {
                    // Only a rebuild (m_RecalculatePreview set) does real work; a plain call returns at once.
                    __state = new Profile();
                    current = __state;
                } catch { }
            }

            private static void Postfix(LevelUpController __instance, Profile __state) {
                if (__state == null) return;
                current = null;
                try {
                    double total = Ms(__state.Start);
                    if (total < ReportMs) return;
                    string details = string.Join(", ", __state.Items.OrderByDescending(e => e.Value.Ticks).Take(Top)
                        .Select(e => $"{e.Key}={e.Value.Ticks * 1000.0 / Stopwatch.Frequency:F0}ms/{e.Value.Count}x"));
                    string what = $"Level-up preview rebuild for {__instance?.Unit?.CharacterName ?? "?"} took {total:F0}ms ({__instance?.LevelUpActions?.Count ?? 0} actions); {details}";
                    IsekaiContext.Logger.Log(what);
                    FrameStallDiagnostics.Note(what);
                } catch (Exception ex) {
                    IsekaiContext.Logger.Log("Level-up preview diagnostics failed: " + ex.Message);
                }
            }
        }

        [HarmonyPatch(typeof(Kingmaker.UnitLogic.UnitHelper), "CreatePreview", typeof(UnitEntityData), typeof(bool))]
        private static class CreatePreviewTrace {
            private static void Prefix(out long __state) { __state = Stopwatch.GetTimestamp(); }
            private static void Postfix(long __state) { RecordTop("create preview unit (copy)", __state); }
        }

        [HarmonyPatch(typeof(Kingmaker.EntitySystem.EntityDataBase), "Dispose")]
        private static class DisposeTrace {
            private static void Prefix(out long __state) { __state = Stopwatch.GetTimestamp(); }
            private static void Postfix(UnitEntityData __instance, long __state) { if (current != null) RecordTop("dispose unit", __state); }
        }

        [HarmonyPatch(typeof(LevelUpController), nameof(LevelUpController.ApplyLevelUpActions))]
        private static class ApplyActionsTrace {
            private static void Prefix(out long __state) { __state = Stopwatch.GetTimestamp(); }
            private static void Postfix(long __state) { RecordTop("apply all actions", __state); }
        }

        [HarmonyPatch]
        private static class ActionApplyTrace {
            private static IEnumerable<MethodBase> TargetMethods() {
                foreach (Type t in new[] { typeof(SelectFeature), typeof(SelectClass), typeof(ApplyClassMechanics), typeof(ApplySpellbook), typeof(SelectSpell), typeof(AddArchetype), typeof(ApplySkillPoints), typeof(SelectRace), typeof(SelectAlignment) }) {
                    MethodInfo m = t.GetMethod("Apply", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly, null, new[] { typeof(LevelUpState), typeof(Kingmaker.UnitLogic.UnitDescriptor) }, null);
                    if (m != null) yield return m;
                }
            }
            private static void Prefix(out long __state) { __state = Stopwatch.GetTimestamp(); }
            private static void Postfix(object __instance, long __state) {
                if (current == null) return;
                string key = "action " + __instance.GetType().Name;
                if (__instance is SelectFeature sf) key += " " + (sf.Item?.Feature?.name ?? "?");
                else if (__instance is SelectClass sc) key += " " + (sc.CharacterClass?.name ?? "?");
                Record(key, __state);
            }
        }

        private static void RecordTop(string key, long start) {
            Profile p = current;
            if (p == null) return;
            Record(key, start);
            p.Items[key].TopLevel = true;
        }

        private sealed class Profile {
            internal readonly long Start = Stopwatch.GetTimestamp();
            internal readonly Dictionary<string, Item> Items = new Dictionary<string, Item>(StringComparer.Ordinal);
        }

        private sealed class Item { internal long Ticks; internal int Count; internal bool TopLevel; }
    }
}
