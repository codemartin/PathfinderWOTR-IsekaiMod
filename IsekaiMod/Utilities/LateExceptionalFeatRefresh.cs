using System;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Features.ExceptionalFeats;
using Kingmaker;

namespace IsekaiMod.Utilities;

// Run independently of the optional spell-list merge, after late-loading mods.
[HarmonyPatch(typeof(GameStarter), "FixTMPAssets")]
internal static class LateExceptionalFeatRefresh
{
    [HarmonyPriority(Priority.Last)]
    [HarmonyPostfix]
    private static void Postfix()
    {
        try
        {
            ExceptionalFeatSelection.RefreshLateFeats();
        }
        catch (Exception ex)
        {
            Main.LogError($"Late Exceptional feat refresh failed: {ex}");
        }
    }
}
