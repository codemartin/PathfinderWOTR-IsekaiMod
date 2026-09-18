using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Globalmap.Blueprints;
using System.Collections.Generic;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Quest objectives loaded from the game's JSON always carry their list fields, but an objective built in
    /// code leaves them null. GlobalMapPointState.ImportantObjectives walks Locations of every started objective
    /// without a null check, so a single started mod objective threw on every frame of the global map and the
    /// location names and icons never initialised. Every loaded objective gets empty lists here.
    /// </summary>
    internal static class QuestObjectiveListSafety {
        public static int Fill(BlueprintQuestObjective objective) {
            if (objective == null) return 0;
            int filled = 0;
            if (objective.Locations == null) { objective.Locations = new List<BlueprintGlobalMapPoint.Reference>(); filled++; }
            if (objective.MultiEntranceEntries == null) { objective.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>(); filled++; }
            if (objective.m_Areas == null) { objective.m_Areas = new List<BlueprintAreaReference>(); filled++; }
            if (objective.m_Addendums == null) { objective.m_Addendums = new List<BlueprintQuestObjectiveReference>(); filled++; }
            if (objective.m_NextObjectives == null) { objective.m_NextObjectives = new List<BlueprintQuestObjectiveReference>(); filled++; }
            return filled;
        }

        public static void FillAllLoaded() {
            int objectives = 0, fields = 0;
            ResourcesLibrary.BlueprintsCache.ForEachLoaded(delegate (BlueprintGuid guid, SimpleBlueprint bp) {
                if (bp is BlueprintQuestObjective objective) {
                    int n = Fill(objective);
                    if (n > 0) { objectives++; fields += n; }
                }
            });
            if (objectives > 0) {
                IsekaiContext.Logger.Log($"QuestObjectiveListSafety: filled {fields} missing list(s) on {objectives} quest objective(s).");
            }
        }
    }
}
