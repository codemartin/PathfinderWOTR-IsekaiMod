using HarmonyLib;
using Kingmaker;
using Kingmaker.Dungeon;
using Kingmaker.UI.MVVM._VM.MapIslands;
using System;
using System.Linq;
using System.Text;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Temporary diagnostic for the Midnight Isles (DLC3) island map showing no clickable island. Whenever the
    /// map opens, the dungeon map state and every island's flags are written to the log, together with the
    /// game's own MayBeVisitedNow verdict, so the reason nothing is selectable can be read off directly.
    /// Nothing is changed; the mod does not touch the expedition map anywhere else.
    /// </summary>
    [HarmonyPatch(typeof(MapIslandsVM), MethodType.Constructor, typeof(Action))]
    internal static class IslandMapDiagnostics {
        private static void Postfix(MapIslandsVM __instance) {
            try {
                DungeonState ds = Game.Instance?.Player?.DungeonState;
                DungeonMapState ms = ds?.MapState;
                StringBuilder sb = new StringBuilder();
                sb.Append("Island map opened: ");
                if (ms == null) {
                    sb.Append("no map state (DungeonState ").Append(ds == null ? "null" : "present").Append(")");
                    IsekaiContext.Logger.Log(sb.ToString());
                    return;
                }
                sb.Append($"stage={ds.StageIndex} travelling={ms.IsTravelling} travelDest={ms.TravelDestination?.Blueprint?.name ?? "none"} travelSrc={ms.TravelSource?.Blueprint?.name ?? "none"} current={ms.IslandCurrent?.Blueprint?.name ?? "none"} final={ms.IslandFinal?.Blueprint?.name ?? "none"} expedition={ms.Expedition?.name ?? "none"} islands={ms.Islands?.Length ?? 0} itemVMs={__instance.IslandsVm?.Count ?? 0}");
                foreach (DungeonIslandState island in ms.Islands ?? new DungeonIslandState[0]) {
                    if (island == null) continue;
                    sb.Append($"\n  {island.Blueprint?.name ?? "?"}: first={island.IsFirst} last={island.IsLast} current={island.IsCurrent} visited={island.IsVisited} finished={island.IsFinished} skipped={island.IsSkipped} prev={island.PreviousIslands?.Count ?? 0} next={island.NextIslands?.Count ?? 0} mayVisit={island.MayBeVisitedNow}");
                }
                IsekaiContext.Logger.Log(sb.ToString());
            } catch (Exception ex) {
                IsekaiContext.Logger.Log("Island map diagnostics failed: " + ex.Message);
            }
        }
    }
}
