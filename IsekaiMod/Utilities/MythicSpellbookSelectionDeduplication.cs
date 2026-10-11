using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;

namespace IsekaiMod.Utilities;

// Retain the first choice per blueprint while preserving choices without a blueprint.
[HarmonyPatch(typeof(BlueprintFeatureSelectMythicSpellbook), "ExtractSelectionItems")]
internal static class MythicSpellbookSelectionDeduplication
{
    [HarmonyPostfix]
    private static void Postfix(ref IEnumerable<IFeatureSelectionItem> __result)
    {
        if (__result == null) return;
        var seen = new HashSet<BlueprintScriptableObject>();
        __result = __result.Where(item => item?.Param?.Blueprint == null || seen.Add(item.Param.Blueprint)).ToList();
    }
}
