using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Runtime profiler for combat stutter. Times every top-level rulebook event and counts the stat
    /// modifier churn (ModifiableValue.AddModifier / UpdateValue) that happens inside it, attributing
    /// modifiers to the fact that added them. A summary of the slowest event types and the busiest
    /// modifier sources is written to the log every ReportInterval while events are flowing, and any
    /// single event slower than SlowEventMilliseconds is reported on its own.
    ///
    /// Rulebook.TriggerEvent is generic; on Mono, reference-type instantiations share one native body,
    /// so patching the open method covers every event type. The patch is applied manually and any
    /// failure is logged instead of thrown so the mod keeps loading.
    /// </summary>
    internal static class CombatPerformanceDiagnostics {
        private const double SlowEventMilliseconds = 20.0;
        private static readonly TimeSpan ReportInterval = TimeSpan.FromSeconds(15);

        private sealed class Timing {
            internal long Ticks;
            internal int Count;
            internal long MaxTicks;
        }

        private static readonly Dictionary<Type, Timing> Events = new Dictionary<Type, Timing>();
        private static readonly Dictionary<string, int> ModifierSources = new Dictionary<string, int>(StringComparer.Ordinal);
        private static int depth;
        private static int modifierAdds;
        private static int valueUpdates;
        private static int eventModifierAdds;
        private static int eventValueUpdates;
        private static DateTime lastReportUtc = DateTime.UtcNow;

        // Per-handler timing: every OnEventAboutToTrigger / OnEventDidTrigger of a rulebook handler for the
        // rule types below is timed and attributed to the handler's type, so a slow rule can be traced to
        // the component that makes it slow.
        private static readonly Dictionary<string, Timing> Handlers = new Dictionary<string, Timing>(StringComparer.Ordinal);
        private static readonly string[] HandlerRuleTypes = {
            "RuleAttackWithWeapon", "RuleAttackRoll", "RuleCalculateAttackBonus", "RuleCalculateAC", "RuleCalculateDamage",
            "RuleDealDamage", "RuleCalculateWeaponStats", "RuleSavingThrow", "RuleCastSpell", "RuleRollD20", "RuleSkillCheck",
            "RuleCalculateAbilityParams", "RuleAttackWithWeaponResolve", "RuleCanApplyBuff",
        };

        // Rule types worth timing. The open generic TriggerEvent cannot be patched ("Specified method is not
        // supported"), so closed instantiations are patched one by one; on Mono they may share a body, in which
        // case the first patch already covers the rest and the later ones are no-ops.
        private static readonly string[] ProfiledRuleTypes = {
            "Kingmaker.RuleSystem.Rules.RuleAttackWithWeapon",
            "Kingmaker.RuleSystem.Rules.RuleAttackRoll",
            "Kingmaker.RuleSystem.Rules.RuleCalculateAttackBonus",
            "Kingmaker.RuleSystem.Rules.RuleCalculateAC",
            "Kingmaker.RuleSystem.Rules.RuleCalculateDamage",
            "Kingmaker.RuleSystem.Rules.RuleDealDamage",
            "Kingmaker.RuleSystem.Rules.RuleSavingThrow",
            "Kingmaker.RuleSystem.Rules.RuleSkillCheck",
            "Kingmaker.RuleSystem.Rules.RuleRollD20",
            "Kingmaker.RuleSystem.Rules.RuleRollDamage",
            "Kingmaker.RuleSystem.Rules.RuleInitiativeRoll",
            "Kingmaker.RuleSystem.Rules.RuleCheckConcentration",
            "Kingmaker.RuleSystem.Rules.RuleSpellResistanceCheck",
            "Kingmaker.RuleSystem.Rules.Abilities.RuleCastSpell",
            "Kingmaker.RuleSystem.Rules.Abilities.RuleCalculateAbilityParams",
            "Kingmaker.RuleSystem.Rules.RuleCalculateWeaponStats",
            "Kingmaker.RuleSystem.Rules.RuleCalculateCMB",
            "Kingmaker.RuleSystem.Rules.RuleCalculateCMD",
            "Kingmaker.RuleSystem.Rules.RuleAttackWithWeaponResolve",
            "Kingmaker.RuleSystem.Rules.RuleCombatManeuver",
            "Kingmaker.RuleSystem.Rules.Damage.RuleCalculateDamage",
            "Kingmaker.RuleSystem.Rules.Damage.RuleDealDamage",
        };

        internal static void Install(Harmony harmony) {
            try {
                MethodInfo trigger = typeof(Rulebook).GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == "TriggerEvent" && m.IsGenericMethodDefinition && m.GetParameters().Length == 1);
                if (trigger == null) {
                    IsekaiContext.Logger.Log("Combat diagnostics: Rulebook.TriggerEvent not found, profiler disabled");
                    return;
                }
                Assembly game = typeof(Rulebook).Assembly;
                int patched = 0;
                List<string> failed = new List<string>();
                foreach (string typeName in ProfiledRuleTypes) {
                    Type ruleType = game.GetType(typeName);
                    if (ruleType == null) continue;
                    try {
                        harmony.Patch(trigger.MakeGenericMethod(ruleType),
                            prefix: new HarmonyMethod(typeof(CombatPerformanceDiagnostics), nameof(TriggerPrefix)),
                            postfix: new HarmonyMethod(typeof(CombatPerformanceDiagnostics), nameof(TriggerPostfix)));
                        patched++;
                    } catch (Exception ex) {
                        failed.Add(ruleType.Name + " (" + ex.Message + ")");
                    }
                }

                MethodInfo addModifier = typeof(ModifiableValue).GetMethod("AddModifier", new[] { typeof(ModifiableValue.Modifier) });
                if (addModifier != null) {
                    harmony.Patch(addModifier, prefix: new HarmonyMethod(typeof(CombatPerformanceDiagnostics), nameof(AddModifierPrefix)));
                }
                MethodInfo updateValue = typeof(ModifiableValue).GetMethod("UpdateValue", Type.EmptyTypes);
                if (updateValue != null) {
                    harmony.Patch(updateValue, prefix: new HarmonyMethod(typeof(CombatPerformanceDiagnostics), nameof(UpdateValuePrefix)));
                }
                int handlerMethods = InstallHandlerProfiling(harmony);
                IsekaiContext.Logger.Log($"Combat diagnostics: rule timing profiler installed on {patched} rule type(s), {handlerMethods} handler method(s)" + (failed.Count == 0 ? "" : "; not patched: " + string.Join(", ", failed)));
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Combat diagnostics could not be installed: {ex.Message}");
            }
        }

        private static int InstallHandlerProfiling(Harmony harmony) {
            int patched = 0;
            HarmonyMethod prefix = new HarmonyMethod(typeof(CombatPerformanceDiagnostics), nameof(HandlerPrefix));
            HarmonyMethod postfix = new HarmonyMethod(typeof(CombatPerformanceDiagnostics), nameof(HandlerPostfix));
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                Type[] types;
                try { types = assembly.GetTypes(); } catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); } catch (Exception) { continue; }
                foreach (Type type in types) {
                    if (type == null || type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition) continue;
                    bool relevant;
                    try {
                        relevant = type.GetInterfaces().Any(i => i.IsGenericType && i.Name.StartsWith("IRulebookHandler", StringComparison.Ordinal)
                            && HandlerRuleTypes.Contains(i.GetGenericArguments()[0].Name));
                    } catch (Exception) { continue; }
                    if (!relevant) continue;
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)) {
                        if (method.IsAbstract || method.IsGenericMethodDefinition) continue;
                        if (method.Name != "OnEventAboutToTrigger" && method.Name != "OnEventDidTrigger") continue;
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length != 1 || !HandlerRuleTypes.Contains(parameters[0].ParameterType.Name)) continue;
                        try {
                            harmony.Patch(method, prefix: prefix, postfix: postfix);
                            patched++;
                        } catch (Exception) {
                            // a handler that cannot be patched is simply not timed
                        }
                    }
                }
            }
            return patched;
        }

        private static void HandlerPrefix(object __instance, out KeyValuePair<long, string> __state) {
            // The owning fact is only reachable while the component's runtime is current, i.e. inside the call.
            string owner = null;
            try {
                // Handlers are BlueprintComponents; the blueprint that carries the component is the stable name.
                owner = (__instance as Kingmaker.Blueprints.BlueprintComponent)?.OwnerBlueprint?.name
                    ?? (__instance as Kingmaker.EntitySystem.EntityFactComponent)?.Fact?.Blueprint?.name;
            } catch (Exception) {
                // some components have no runtime bound at this point
            }
            __state = new KeyValuePair<long, string>(Stopwatch.GetTimestamp(), owner);
        }

        private static void HandlerPostfix(object __instance, KeyValuePair<long, string> __state) {
            long elapsed = Stopwatch.GetTimestamp() - __state.Key;
            string key = (__instance?.GetType().Name ?? "(unknown)") + (__state.Value == null ? "" : "@" + __state.Value);
            if (!Handlers.TryGetValue(key, out Timing timing)) {
                timing = new Timing();
                Handlers.Add(key, timing);
            }
            timing.Ticks += elapsed;
            timing.Count++;
            if (elapsed > timing.MaxTicks) timing.MaxTicks = elapsed;
        }

        private static void TriggerPrefix(out long __state) {
            // An exception inside TriggerEvent skips the postfix; the rulebook resets its own context then, so do the same.
            if (depth > 40) depth = 0;
            depth++;
            if (depth == 1) {
                eventModifierAdds = 0;
                eventValueUpdates = 0;
            }
            __state = Stopwatch.GetTimestamp();
        }

        private static void TriggerPostfix(RulebookEvent evt, long __state) {
            long elapsed = Stopwatch.GetTimestamp() - __state;
            depth = Math.Max(0, depth - 1);
            if (depth != 0 || evt == null) return;

            try {
                Type type = evt.GetType();
                if (!Events.TryGetValue(type, out Timing timing)) {
                    timing = new Timing();
                    Events.Add(type, timing);
                }
                timing.Ticks += elapsed;
                timing.Count++;
                if (elapsed > timing.MaxTicks) timing.MaxTicks = elapsed;

                double ms = ToMilliseconds(elapsed);
                if (ms >= SlowEventMilliseconds) {
                    string owner = evt.Initiator?.CharacterName ?? "unknown unit";
                    IsekaiContext.Logger.Log(
                        $"Slow rule: {type.Name} for {owner} took {ms:F1}ms with {eventModifierAdds} stat modifier add(s) and {eventValueUpdates} stat update(s)");
                }

                DateTime now = DateTime.UtcNow;
                if (now - lastReportUtc >= ReportInterval) {
                    lastReportUtc = now;
                    Report();
                }
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Combat diagnostics could not record an event: {ex.Message}");
            }
        }

        private static int stackSamples;

        private static void AddModifierPrefix(ModifiableValue __instance, ModifiableValue.Modifier mod) {
            if (depth == 0) return;
            modifierAdds++;
            eventModifierAdds++;
            string source = mod?.Source?.Blueprint?.name ?? mod?.SourceComponent;
            if (source == null) {
                source = $"(no source) {mod?.ModDescriptor} on {__instance?.Type}";
                // A burst of source-less modifiers inside a rule means something is re-applying facts wholesale.
                // Log one call stack per report window so the trigger can be identified.
                if (stackSamples < 1 && eventModifierAdds > 50) {
                    stackSamples++;
                    string[] lines = Environment.StackTrace.Split('\n');
                    IsekaiContext.Logger.Log("Modifier burst sample (" + eventModifierAdds + " adds so far in this rule):\n" + string.Join("\n", lines.Skip(2).Take(45)));
                }
            }
            ModifierSources.TryGetValue(source, out int count);
            ModifierSources[source] = count + 1;
        }

        private static void UpdateValuePrefix() {
            if (depth == 0) return;
            valueUpdates++;
            eventValueUpdates++;
        }

        private static void Report() {
            if (Events.Count == 0) return;
            string slowest = string.Join(", ", Events
                .OrderByDescending(entry => entry.Value.Ticks)
                .Take(8)
                .Select(entry => $"{entry.Key.Name}={ToMilliseconds(entry.Value.Ticks):F0}ms/{entry.Value.Count}x(max {ToMilliseconds(entry.Value.MaxTicks):F1}ms)"));
            string sources = ModifierSources.Count == 0
                ? "none"
                : string.Join(", ", ModifierSources
                    .OrderByDescending(entry => entry.Value)
                    .Take(8)
                    .Select(entry => $"{entry.Key}={entry.Value}"));
            string handlers = Handlers.Count == 0
                ? "none"
                : string.Join(", ", Handlers
                    .OrderByDescending(entry => entry.Value.Ticks)
                    .Take(16)
                    .Select(entry => $"{entry.Key}={ToMilliseconds(entry.Value.Ticks):F0}ms/{entry.Value.Count}x(max {ToMilliseconds(entry.Value.MaxTicks):F1}ms)"));
            IsekaiContext.Logger.Log(
                $"Rule timing (last {ReportInterval.TotalSeconds:F0}s): {slowest}; {modifierAdds} stat modifier add(s), {valueUpdates} stat update(s); busiest modifier sources: {sources}; slowest handlers: {handlers}");
            Events.Clear();
            ModifierSources.Clear();
            Handlers.Clear();
            stackSamples = 0;
            modifierAdds = 0;
            valueUpdates = 0;
        }

        private static double ToMilliseconds(long ticks) {
            return ticks * 1000.0 / Stopwatch.Frequency;
        }
    }
}
