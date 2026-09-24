using HarmonyLib;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.Particles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic for multi-second freezes that the rulebook profiler does not see (for example a
    /// spell cast that stalls the game even though RuleCastSpell itself only took 200 ms).
    ///
    /// A hidden MonoBehaviour watches the frame time. When one frame takes longer than StallSeconds it writes
    /// one log entry with the frame length, the garbage collections that happened during it, and the last
    /// few things that were recorded before it: slow buff applications, slow action lists and FX spawns. Those are recorded by small timing patches that only write to a ring buffer, so the
    /// cost while nothing stalls is a few microseconds per call.
    ///
    /// Everything here is diagnostics only and can be removed once the freeze is understood.
    /// </summary>
    internal static class FrameStallDiagnostics {
        private const float StallSeconds = 0.4f;
        private const double NoteBuffMs = 25.0;
        private const double NoteActionListMs = 40.0;
        private const double NoteFxMs = 15.0;
        private const int RingSize = 40;

        private static readonly object gate = new object();
        private static readonly Queue<string> recent = new Queue<string>(RingSize);
        private static readonly Stopwatch clock = Stopwatch.StartNew();
        private static int lastGc0, lastGc1, lastGc2;

        internal static void Note(string what) {
            lock (gate) {
                if (recent.Count >= RingSize) recent.Dequeue();
                recent.Enqueue($"{clock.Elapsed.TotalSeconds:F2}s {what}");
            }
        }

        internal static void Install(Harmony harmony) {
            try {
                GameObject go = new GameObject("IsekaiMod.FrameStallWatcher");
                UnityEngine.Object.DontDestroyOnLoad(go);
                go.hideFlags = HideFlags.HideAndDontSave;
                go.AddComponent<FrameStallWatcher>();

                int patched = 0;
                patched += TryPatch(harmony, typeof(BuffCollection).GetMethod("AddBuff", new[] { typeof(BlueprintBuff), typeof(MechanicsContext), typeof(TimeSpan?) }),
                    nameof(TimedPrefix), nameof(AddBuffPostfix));
                patched += TryPatch(harmony, typeof(ActionList).GetMethod("Run", Type.EmptyTypes), nameof(TimedPrefix), nameof(ActionListPostfix));
                foreach (MethodInfo m in typeof(FxHelper).GetMethods(BindingFlags.Public | BindingFlags.Static)) {
                    if (!m.Name.StartsWith("SpawnFx", StringComparison.Ordinal) || m.GetParameters().Length == 0) continue;
                    if (m.GetParameters()[0].ParameterType == typeof(GameObject)) {
                        patched += TryPatch(harmony, m, nameof(TimedPrefix), nameof(FxPostfix));
                    }
                }
                // Deliberately NOT patched: WeakResourceLink<T>.Load. It is a generic method, and on Mono every
                // reference-type instantiation shares one body. Harmony re-emits the patched body specialised for
                // the instantiation it was given, so patching the GameObject version also rerouted the TextAsset
                // version the area loader uses for navmeshes; that asked the resource library for a GameObject under
                // a TextAsset id, got null, and every area transition failed with a NullReferenceException in
                // NavMeshManager.SetNavMeshInternal (r74). Never patch generic methods of that class.
                IsekaiContext.Logger.Log($"Frame stall diagnostics installed ({patched} timing patch(es)); frames above {StallSeconds * 1000:F0}ms are logged with recent activity.");
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Frame stall diagnostics could not be installed: {ex.Message}");
            }
        }

        private static int TryPatch(Harmony harmony, MethodInfo target, string prefix, string postfix) {
            if (target == null) return 0;
            try {
                harmony.Patch(target,
                    prefix: new HarmonyMethod(typeof(FrameStallDiagnostics), prefix),
                    postfix: new HarmonyMethod(typeof(FrameStallDiagnostics), postfix));
                return 1;
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Frame stall diagnostics: could not patch {target.DeclaringType?.Name}.{target.Name}: {ex.Message}");
                return 0;
            }
        }

        private static double Ms(long start) => (Stopwatch.GetTimestamp() - start) * 1000.0 / Stopwatch.Frequency;

        private static void TimedPrefix(out long __state) {
            __state = Stopwatch.GetTimestamp();
        }

        private static void AddBuffPostfix(BlueprintBuff blueprint, MechanicsContext parentContext, long __state) {
            double ms = Ms(__state);
            if (ms < NoteBuffMs) return;
            Note($"buff {blueprint?.name ?? "?"} on {parentContext?.MaybeOwner?.CharacterName ?? "?"} took {ms:F0}ms");
        }

        private static void ActionListPostfix(ActionList __instance, long __state) {
            double ms = Ms(__state);
            if (ms < NoteActionListMs) return;
            string actions = "?";
            try {
                actions = string.Join(",", (__instance?.Actions ?? new GameAction[0]).Where(a => a != null).Select(a => a.GetType().Name).Take(6));
            } catch { }
            Note($"action list [{actions}] took {ms:F0}ms");
        }

        private static void FxPostfix(GameObject prefab, long __state) {
            double ms = Ms(__state);
            if (ms < NoteFxMs) return;
            Note($"fx spawn {prefab?.name ?? "?"} took {ms:F0}ms");
        }

        private sealed class FrameStallWatcher : MonoBehaviour {
            private bool primed;

            private void Update() {
                float dt = Time.unscaledDeltaTime;
                int gc0 = GC.CollectionCount(0), gc1 = GC.CollectionCount(1), gc2 = GC.CollectionCount(2);
                if (!primed) {
                    primed = true;
                } else if (dt >= StallSeconds) {
                    string lines;
                    lock (gate) {
                        lines = recent.Count == 0 ? "  (nothing recorded)" : "  " + string.Join("\n  ", recent);
                        recent.Clear();
                    }
                    long mem = GC.GetTotalMemory(false) / (1024 * 1024);
                    IsekaiContext.Logger.Log(
                        $"Long frame: {dt * 1000:F0}ms at {clock.Elapsed.TotalSeconds:F2}s; GC gen0/1/2 during it: {gc0 - lastGc0}/{gc1 - lastGc1}/{gc2 - lastGc2}; managed heap {mem}MB; recent activity:\n{lines}");
                }
                lastGc0 = gc0; lastGc1 = gc1; lastGc2 = gc2;
            }
        }
    }
}
