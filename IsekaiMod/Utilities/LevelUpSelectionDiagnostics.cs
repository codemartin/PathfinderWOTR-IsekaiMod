using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using System;
using System.Linq;
using System.Text;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic for level-up choices that look selectable but do not stick (Cleric Legacy domains and
    /// channel energy, and earlier reports of the same for other inherited features). The level-up screen drops a
    /// choice silently when SelectFeature.Check fails, and the game's own "Invalid action" line does not reach
    /// Player.log. This logs every failed Check with the reason it would fail: not in the selection's list, a
    /// prerequisite (named), already at max rank, selection mode, or the selection being already filled. When the
    /// choice is dropped later, during a preview recalculation, the same line is written from that path.
    /// </summary>
    internal static class LevelUpSelectionDiagnostics {

        [HarmonyPatch(typeof(SelectFeature), nameof(SelectFeature.Check))]
        private static class CheckTrace {
            private static void Postfix(SelectFeature __instance, LevelUpState state, UnitDescriptor unit, bool __result) {
                if (__result) return;
                try {
                    IsekaiContext.Logger.Log(Describe(__instance, state, unit));
                } catch (Exception ex) {
                    IsekaiContext.Logger.Log("Level-up selection diagnostics failed: " + ex.Message);
                }
            }
        }

        private static string Describe(SelectFeature action, LevelUpState state, UnitDescriptor unit) {
            StringBuilder sb = new StringBuilder();
            BlueprintFeature feature = action.Item?.Feature;
            BlueprintFeatureSelection selection = action.Selection as BlueprintFeatureSelection;
            sb.Append($"Level-up choice rejected: {feature?.name ?? "?"} in {(action.Selection as Kingmaker.Blueprints.BlueprintScriptableObject)?.name ?? action.Selection?.GetType().Name ?? "?"}#{action.SelectionIndex} on {unit?.CharacterName ?? "?"}: ");
            if (action.Item == null) return sb.Append("no item").ToString();
            FeatureSelectionState selectionState = state?.Selections?.FirstOrDefault(s => s.Selection == action.Selection && s.Index == action.SelectionIndex);
            if (selectionState == null) return sb.Append("selection state not found in this level-up").ToString();
            if (selectionState.Selected) return sb.Append($"selection already holds {selectionState.SelectedItem?.Feature?.name ?? "?"}").ToString();
            if (selection == null) return sb.Append("selection is not a BlueprintFeatureSelection; its CanSelect said no").ToString();
            if (feature == null) return sb.Append("item has no feature").ToString();
            bool any = false;
            if (!(feature is BlueprintLevelUpPlanFeaturesList) && !selection.AllFeatures.Contains(feature)) { sb.Append("not in the selection's feature list; "); any = true; }
            bool ignore = selection.IsIgnorePrerequisites() || selectionState.IgnorePrerequisites;
            if (!ignore) {
                foreach (Prerequisite p in feature.ComponentsArray.OfType<Prerequisite>()) {
                    bool ok;
                    try { ok = p.Check(selectionState, unit, state); } catch (Exception ex) { ok = false; sb.Append($"prerequisite {p.GetType().Name} threw {ex.GetType().Name}; "); any = true; continue; }
                    if (!ok) {
                        string text;
                        try { text = p.GetUIText(unit); } catch { text = "?"; }
                        sb.Append($"prerequisite {p.GetType().Name} [{text}] failed; ");
                        any = true;
                    }
                }
            }
            int rank = unit.Progression.Features.GetRank(feature);
            if (!(feature is IFeatureSelection) && rank >= feature.Ranks) { sb.Append($"already has it (rank {rank}/{feature.Ranks}); "); any = true; }
            if (selection.Mode == SelectionMode.OnlyNew && rank > 0) { sb.Append("mode OnlyNew and rank > 0; "); any = true; }
            if (selection.Mode == SelectionMode.OnlyRankUp && rank <= 0) { sb.Append("mode OnlyRankUp and rank 0; "); any = true; }
            if (!any) {
                try {
                    if (NestedFeatureSelectionUtils.AllNestedFeaturesUnavailable(unit, state, selectionState, feature)) { sb.Append("every nested choice is unavailable; "); any = true; }
                } catch (Exception ex) { sb.Append("nested check threw " + ex.GetType().Name + "; "); any = true; }
            }
            if (!any) sb.Append("no reason found by this trace (a patch on CanSelect or MeetsPrerequisites may be refusing it)");
            return sb.ToString();
        }
    }
}
