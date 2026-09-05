using HarmonyLib;
using Kingmaker.Blueprints.Classes;
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
        /// PatchTools copies inherited special-list spells into the Isekai spellbook as known spells.
        /// Suppress the original component for a class the unit does not own so its unconditional
        /// DemandSpellbook call cannot leave an empty source-class spellbook in the character UI.
        /// </summary>
        [HarmonyPatch(typeof(AddSpecialSpellList), "OnActivate")]
        private static class AddSpecialSpellListPatcher {
            [HarmonyPrefix]
            private static bool Prefix(AddSpecialSpellList __instance) {
                return !TryGetSpellbook(__instance.Owner?.Descriptor, __instance.CharacterClass, out _);
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
