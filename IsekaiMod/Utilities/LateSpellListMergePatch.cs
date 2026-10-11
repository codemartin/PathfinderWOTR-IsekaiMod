using System;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using Kingmaker;

namespace IsekaiMod.Utilities;

// Catch spells added after the normal FinalPatcher pass. RegisterSpell deduplicates entries.
[HarmonyPatch(typeof(GameStarter), "FixTMPAssets")]
internal static class LateSpellListMergePatch
{
    private static bool ran;

    [HarmonyPriority(Priority.Low)]
    [HarmonyAfter("MysticalMayhem", "TabletopTweaks-Base", "TomeOfTheFirebird")]
    [HarmonyPostfix]
    private static void Postfix()
    {
        if (ran || Main.IsekaiContext?.AddedContent == null || !Main.IsekaiContext.AddedContent.MergeIsekaiSpellList) return;
        // Do not consume the one-shot if blueprint creation has not completed yet.
        if (IsekaiProtagonistSpellList.Get()?.SpellsByLevel == null || MastermindSpellList.Get() == null) return;
        try
        {
            int before = CountSpells();
            IsekaiProtagonistSpellList.MergeSpellLists();
            MastermindSpellList.PatchMastermindSpellList();
            ran = true;
            Main.Log($"Late spell list merge added {CountSpells() - before} spell(s) from late-loading mods");
        }
        catch (Exception ex)
        {
            Main.LogError($"Late spell list merge failed: {ex}");
        }
    }

    private static int CountSpells() => IsekaiProtagonistSpellList.Get().SpellsByLevel.Sum(level => level?.m_Spells?.Count ?? 0);
}
