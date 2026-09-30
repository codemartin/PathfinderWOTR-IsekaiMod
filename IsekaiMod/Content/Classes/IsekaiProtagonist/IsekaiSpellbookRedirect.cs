using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist {

    /// <summary>
    /// Sends spell grants that name an inherited class into the Isekai Protagonist spellbook.
    ///
    /// Features reached through a legacy (an Oracle mystery's "choose a shadow spell", a bonus spell
    /// list, a mythic feature copied from another class) still say "learn this into the Oracle
    /// spellbook". An Isekai Protagonist has no Oracle levels, so LearnSpellParametrized created a
    /// hidden, empty Oracle spellbook and learned the spell there, while AddKnownSpell silently did
    /// nothing. Both now land in the character's own spellbook (or the archetype's replacement).
    ///
    /// The redirect only applies when the unit has no levels in the requested class and does have
    /// Isekai Protagonist levels. Multiclassed characters that really own the requested class, and
    /// every non-Isekai character, take the unmodified game path.
    /// </summary>
    internal static class IsekaiSpellbookRedirect {

        internal static bool TryGetSpellbook(UnitDescriptor unit, BlueprintCharacterClass requestedClass, out Spellbook spellbook) {
            spellbook = null;
            if (unit == null || requestedClass == null) return false;
            if (unit.Progression.GetClassData(requestedClass) != null) return false;
            BlueprintCharacterClass isekaiClass = IsekaiProtagonistClass.Get();
            if (isekaiClass == null || isekaiClass == requestedClass) return false;
            ClassData isekai = unit.Progression.GetClassData(isekaiClass);
            if (isekai?.Spellbook == null) return false;
            spellbook = unit.DemandSpellbook(isekai.Spellbook);
            return spellbook != null;
        }

        /// <summary>
        /// The general case. UnitDescriptor.DemandSpellbook(class) answers a request for a class the unit does
        /// not have with that class's default spellbook, which then sits on the character as an empty book
        /// and shows as a level 0 class row. The level-up UI does this while listing a parametrized feature's
        /// spells (BlueprintParametrizedFeature.ExtractItemsFromSpellList), and other callers exist. For an
        /// Isekai Protagonist the answer is its own spellbook instead.
        /// </summary>
        [HarmonyPatch(typeof(UnitDescriptor), nameof(UnitDescriptor.DemandSpellbook), new System.Type[] { typeof(BlueprintCharacterClass) })]
        private static class DemandSpellbookPatcher {
            [HarmonyPrefix]
            private static bool Prefix(UnitDescriptor __instance, BlueprintCharacterClass characterClass, ref Spellbook __result) {
                if (!TryGetSpellbook(__instance, characterClass, out Spellbook spellbook)) return true;
                __result = spellbook;
                return false;
            }
        }

        /// <summary>
        /// Saves made before the redirect above can already carry such an empty stray book. Drop it on load
        /// when the unit has Isekai levels, no levels in the book's class, and nothing learned in the book.
        /// </summary>
        [HarmonyPatch(typeof(UnitDescriptor), nameof(UnitDescriptor.PostLoad))]
        private static class StraySpellbookCleanup {
            [HarmonyPostfix]
            private static void Postfix(UnitDescriptor __instance) {
                try {
                    BlueprintCharacterClass isekaiClass = IsekaiProtagonistClass.Get();
                    if (isekaiClass == null || __instance?.Progression?.GetClassData(isekaiClass) == null) return;
                    // Two kinds of stray: a book of a class the unit has no levels in, and the base Isekai
                    // Protagonist book on a character whose archetype replaced it (Shadow Monarch and the
                    // others). Both are only removed when nothing was ever learned into them.
                    var stray = __instance.Spellbooks
                        .Where(book => book?.Blueprint != null && book.Blueprint.CharacterClass != null
                            && __instance.Progression.Classes.All(cd => cd.Spellbook != book.Blueprint)
                            && (book.Blueprint.CharacterClass == isekaiClass
                                || __instance.Progression.GetClassData(book.Blueprint.CharacterClass) == null)
                            && !book.GetAllKnownSpells().Any())
                        .Select(book => book.Blueprint)
                        .ToList();
                    foreach (BlueprintSpellbook book in stray) {
                        __instance.DeleteSpellbook(book);
                        Main.IsekaiContext.Logger.Log($"Removed empty stray {book.name} from {__instance.CharacterName}");
                    }
                } catch (System.Exception ex) {
                    Main.IsekaiContext.Logger.Log($"Stray spellbook cleanup skipped for {__instance?.CharacterName}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// The spellbook overload. Callers that already hold a BlueprintSpellbook bypass the class overload above;
        /// the two seen in practice are TabletopTweaks' Spell Kenning (a Skald feature, reached through the Skald
        /// legacy, which demands the Skald spellbook to build its conversions) and SpellbookSync's prestige
        /// fail-safe, which demands the base Isekai Protagonist book on characters whose archetype replaced it.
        /// Both left an empty book that shows as a level 0 class row and a second spellbook tab.
        ///
        /// An Isekai Protagonist answers such a request with its own spellbook (or the archetype's replacement)
        /// when the requested book belongs to the Isekai class itself, or to a non-mythic class the unit has no
        /// levels in. Books already on the unit, mythic books, and requests on non-Isekai or multiclassed units
        /// take the unmodified path.
        /// </summary>
        [HarmonyPatch(typeof(UnitDescriptor), nameof(UnitDescriptor.DemandSpellbook), new System.Type[] { typeof(BlueprintSpellbook) })]
        private static class DemandSpellbookByBlueprintPatcher {
            private static readonly System.Collections.Generic.HashSet<string> reported = new System.Collections.Generic.HashSet<string>();

            [HarmonyPrefix]
            private static bool Prefix(UnitDescriptor __instance, BlueprintSpellbook blueprint, ref Spellbook __result) {
                try {
                    if (__instance == null || blueprint?.CharacterClass == null || __instance.GetSpellbook(blueprint) != null) return true;
                    BlueprintCharacterClass isekaiClass = IsekaiProtagonistClass.Get();
                    if (isekaiClass == null) return true;
                    ClassData isekai = __instance.Progression?.GetClassData(isekaiClass);
                    if (isekai?.Spellbook == null || isekai.Spellbook == blueprint) return true;
                    if (__instance.Progression.Classes.Any(cd => cd.Spellbook == blueprint)) return true;
                    if (blueprint.CharacterClass.IsMythic) return true;
                    if (blueprint.CharacterClass != isekaiClass && __instance.Progression.GetClassData(blueprint.CharacterClass) != null) return true;
                    Spellbook own = __instance.DemandSpellbook(isekai.Spellbook);
                    if (own == null) return true;
                    if (reported.Add(__instance.CharacterName + "|" + blueprint.name)) {
                        Main.IsekaiContext.Logger.Log($"Spellbook request for {blueprint.name} on {__instance.CharacterName} answered with {own.Blueprint.name}");
                    }
                    __result = own;
                    return false;
                } catch (System.Exception ex) {
                    Main.IsekaiContext.Logger.Log($"Spellbook redirect skipped for {__instance?.CharacterName}: {ex.Message}");
                    return true;
                }
            }
        }

        /// <summary>
        /// Redirect an inherited special list to the Isekai spellbook so the list continues to unlock
        /// spells at the spellbook's normal levels without creating an empty source-class spellbook.
        /// </summary>
        [HarmonyPatch(typeof(AddSpecialSpellList), "OnActivate")]
        private static class AddSpecialSpellListPatcher {
            [HarmonyPrefix]
            private static bool Prefix(AddSpecialSpellList __instance) {
                if (!TryGetSpellbook(__instance.Owner?.Descriptor, __instance.CharacterClass, out Spellbook spellbook)) return true;
                if (__instance.SpellList != null) spellbook.AddSpecialList(__instance.SpellList);
                return false;
            }
        }

        [HarmonyPatch(typeof(AddSpecialSpellList), "OnDeactivate")]
        private static class RemoveSpecialSpellListPatcher {
            [HarmonyPrefix]
            private static bool Prefix(AddSpecialSpellList __instance) {
                if (!TryGetSpellbook(__instance.Owner?.Descriptor, __instance.CharacterClass, out Spellbook spellbook)) return true;
                if (__instance.SpellList != null) spellbook.RemoveSpecialList(__instance.SpellList);
                return false;
            }
        }

        /// <summary>
        /// PatchTools adds an equivalent spontaneous-conversion component for the Isekai class.
        /// Suppress the original source-class component when the unit has no levels in that class so
        /// its unconditional DemandSpellbook calls cannot leave an empty spellbook in the UI.
        /// </summary>
        private static bool ShouldRunSourceConversion(SpontaneousSpellConversion instance) {
            return !TryGetSpellbook(instance.Owner?.Descriptor, instance.CharacterClass, out _);
        }

        [HarmonyPatch(typeof(SpontaneousSpellConversion), "OnTurnOn")]
        private static class SpontaneousSpellConversionTurnOnPatcher {
            [HarmonyPrefix]
            private static bool Prefix(SpontaneousSpellConversion __instance) {
                return ShouldRunSourceConversion(__instance);
            }
        }

        [HarmonyPatch(typeof(SpontaneousSpellConversion), "OnTurnOff")]
        private static class SpontaneousSpellConversionTurnOffPatcher {
            [HarmonyPrefix]
            private static bool Prefix(SpontaneousSpellConversion __instance) {
                return ShouldRunSourceConversion(__instance);
            }
        }

        /// <summary>
        /// Mirrors LearnSpellList.LearnList for an inherited class. The original method exits before
        /// granting anything when the unit has no levels in its configured source class.
        /// </summary>
        [HarmonyPatch(typeof(LearnSpellList), "LearnList")]
        private static class LearnSpellListPatcher {
            [HarmonyPrefix]
            private static bool Prefix(LearnSpellList __instance) {
                if (!TryGetSpellbook(__instance.Owner?.Descriptor, __instance.CharacterClass, out Spellbook spellbook)) return true;
                if (__instance.SpellList == null) return false;

                foreach (var spellLevel in __instance.SpellList.SpellsByLevel) {
                    if (spellLevel == null || spellLevel.SpellLevel > spellbook.MaxSpellLevel) continue;
                    foreach (BlueprintAbility spell in spellLevel.SpellsFiltered) {
                        if (spell == null) continue;
                        if (spellbook.GetKnownSpells(spellLevel.SpellLevel).Any(known => known.Blueprint == spell)) continue;
                        spellbook.AddKnown(spellLevel.SpellLevel, spell, true);
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Mirrors LearnSpellParametrized.OnActivate, with the spellbook resolved through the redirect.
        /// </summary>
        [HarmonyPatch(typeof(LearnSpellParametrized), "OnActivate")]
        private static class LearnSpellParametrizedPatcher {
            [HarmonyPrefix]
            private static bool Prefix(LearnSpellParametrized __instance) {
                if (!TryGetSpellbook(__instance.Owner?.Descriptor, __instance.SpellcasterClass, out Spellbook spellbook)) return true;
                if (!(__instance.Param?.Blueprint is BlueprintAbility spell)) return false;
                int level = __instance.SpecificSpellLevel
                    ? __instance.SpellLevel
                    : __instance.SpellList.GetLevel(spell) + __instance.SpellLevelPenalty;
                spellbook.AddKnown(level, spell);
                return false;
            }
        }

        /// <summary>
        /// Mirrors AddKnownSpell.AddSpell for the redirected case. Archetype-restricted grants are left to
        /// the game, since an Isekai Protagonist never owns the other class's archetype.
        /// </summary>
        [HarmonyPatch(typeof(AddKnownSpell), "AddSpell")]
        private static class AddKnownSpellPatcher {
            [HarmonyPrefix]
            private static bool Prefix(AddKnownSpell __instance) {
                if (__instance.Archetype != null) return true;
                if (!TryGetSpellbook(__instance.Owner?.Descriptor, __instance.CharacterClass, out Spellbook spellbook)) return true;
                BlueprintAbility spell = __instance.Spell;
                if (spell == null) return false;
                int level = __instance.SpellLevel;
                if (level > spellbook.MaxSpellLevel) return false;
                if (spellbook.GetKnownSpells(level).Any(known => known.Blueprint == spell)) return false;
                spellbook.AddKnown(level, spell);
                return false;
            }
        }
    }
}
