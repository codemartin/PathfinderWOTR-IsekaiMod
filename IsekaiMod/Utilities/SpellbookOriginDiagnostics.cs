using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using System;
using System.Linq;
using System.Collections.Generic;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Reports who asks for a class spellbook on a unit that has no levels in that class. The game
    /// answers such a request with the class's default spellbook, which then shows up as a level 0
    /// class row on the character sheet. Logged once per unit and class, with the call stack, so
    /// the responsible component can be redirected like the ones in IsekaiSpellbookRedirect.
    /// </summary>
    [HarmonyPatch(typeof(UnitDescriptor), nameof(UnitDescriptor.DemandSpellbook), new Type[] { typeof(BlueprintCharacterClass) })]
    internal static class SpellbookOriginDiagnostics {
        private static readonly HashSet<string> reported = new HashSet<string>(StringComparer.Ordinal);

        [HarmonyPrefix]
        private static void Prefix(UnitDescriptor __instance, BlueprintCharacterClass characterClass) {
            try {
                if (__instance == null || characterClass == null) return;
                if (__instance.Progression.GetClassData(characterClass) != null) return;
                string key = __instance.CharacterName + "|" + characterClass.name;
                if (!reported.Add(key)) return;
                IsekaiContext.Logger.Log(
                    $"Spellbook origin: {__instance.CharacterName} was asked for the {characterClass.name} spellbook without having that class. Stack:\n{Environment.StackTrace}");
            } catch (Exception) {
                // diagnostics only
            }
        }
    }
}

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Collapses duplicate rows in the mythic spellbook selection and logs what it offered.
    ///
    /// Other mods that gather "every modded spellbook" into the mythic spellbook selections add one entry
    /// per class and archetype that references a book, so the Isekai base book showed up eight times and
    /// the Shadow Monarch book twice (96 entries in the Demon selection's allowed list). The game builds
    /// one row per allowed-list entry the unit carries, hence the wall of identical rows. Rows are now
    /// reduced to one per spellbook blueprint; the standalone "(default)" row is untouched.
    /// </summary>
    [HarmonyPatch(typeof(Kingmaker.Blueprints.Classes.BlueprintFeatureSelectMythicSpellbook), "ExtractSelectionItems")]
    internal static class MythicSpellbookSelectionDiagnostics {
        private static readonly HashSet<string> reported = new HashSet<string>(StringComparer.Ordinal);

        [HarmonyPostfix]
        private static void Postfix(Kingmaker.Blueprints.Classes.BlueprintFeatureSelectMythicSpellbook __instance, UnitDescriptor previewUnit, ref IEnumerable<Kingmaker.Blueprints.Classes.Selection.IFeatureSelectionItem> __result) {
            try {
                if (previewUnit == null || __result == null) return;
                var seen = new HashSet<Kingmaker.Blueprints.BlueprintScriptableObject>();
                var items = __result.Where(item => item?.Param?.Blueprint == null || seen.Add(item.Param.Blueprint)).ToList();
                __result = items;
                string key = __instance.name + "|" + previewUnit.CharacterName;
                if (!reported.Add(key)) return;
                var offered = items.Select(item => item?.Param?.Blueprint).Select(bp => bp == null ? "(default)" : bp.name + "/" + bp.AssetGuid);
                var carried = previewUnit.Spellbooks.Select(book => book.Blueprint.name + "/" + book.Blueprint.AssetGuid + " (" + (book.Blueprint.CharacterClass?.name ?? "no class") + ", level " + book.BaseLevel + ")");
                var allowed = __instance.AllowedSpellbooks.Where(sb => sb != null).Select(sb => sb.name + "/" + sb.AssetGuid);
                IsekaiContext.Logger.Log($"Mythic spellbook selection {__instance.name} for {previewUnit.CharacterName}: offered [{string.Join(", ", offered)}]; unit carries [{string.Join(", ", carried)}]; allowed list has {allowed.Count()} entries: [{string.Join(", ", allowed)}]");
            } catch (Exception ex) {
                IsekaiContext.Logger.Log($"Mythic spellbook selection diagnostics skipped: {ex.Message}");
            }
        }
    }
}
