using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using Kingmaker;
using System;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Second pass of the Isekai spell-list merge.
    ///
    /// The main merge runs during BlueprintsCache.Init. Some mods (Mystical Mayhem for one)
    /// add their spells to the canon class lists later, from GameStarter.FixTMPAssets, so the
    /// first merge never sees them. This postfix runs after those mods (low Harmony priority)
    /// and merges again; RegisterSpell skips anything already present, so it is safe to repeat.
    /// </summary>
    [HarmonyPatch(typeof(GameStarter), nameof(GameStarter.FixTMPAssets))]
    internal static class LateSpellListMergePatch {
        private static bool ran;

        [HarmonyPriority(Priority.Low)]
        [HarmonyPostfix]
        private static void Postfix() {
            if (ran) return;
            ran = true;
            if (IsekaiContext?.AddedContent == null || !IsekaiContext.AddedContent.MergeIsekaiSpellList) return;
            try {
                var timer = System.Diagnostics.Stopwatch.StartNew();
                int before = IsekaiProtagonistSpellList.CountSpells();
                IsekaiProtagonistSpellList.MergeSpellLists();
                MastermindSpellList.PatchMastermindSpellList();
                int added = IsekaiProtagonistSpellList.CountSpells() - before;
                timer.Stop();
                IsekaiContext.Logger.Log($"Late spell list merge added {added} spell(s) from late-loading mods in {timer.ElapsedMilliseconds} ms");
            } catch (Exception ex) {
                IsekaiContext.Logger.LogError($"Late spell list merge failed: {ex}");
            }
        }
    }
}
