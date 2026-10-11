using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.GlobalMap;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Globalmap.State;
using Kingmaker.Globalmap.View;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestWorldMapChecks
{
	internal sealed class MarkerFacts
	{
		public string Label;

		public bool Offered;

		public bool AnchorRevealed;

		public bool HasView;

		public bool Revealed;

		public bool VisualShown;

		public bool Reachable;

		public bool ClearOfAnchor = true;

		public string Note;
	}

	internal static BlueprintGlobalMap ChapterWorldMap(int chapter)
	{
		BossSiteMapPoints.MapId? mapId = BossSiteDelivery.ChapterMap(chapter);
		if (!mapId.HasValue)
		{
			return null;
		}
		return BlueprintTools.GetBlueprint<BlueprintGlobalMap>(BossSiteMapPoints.MapGuid(mapId.Value));
	}

	internal static List<string> MarkerProblems(IEnumerable<MarkerFacts> markers)
	{
		List<string> list = new List<string>();
		foreach (MarkerFacts item in markers ?? Enumerable.Empty<MarkerFacts>())
		{
			if (item == null)
			{
				continue;
			}
			if (!item.Offered)
			{
				if (item.HasView)
				{
					list.Add("World-map marker " + item.Label + " is not offered in this save, but the map drew it.");
				}
			}
			else if (item.AnchorRevealed)
			{
				List<string> list2 = new List<string>();
				if (!item.HasView)
				{
					list2.Add("has no view on the map");
				}
				if (item.HasView && !item.Revealed)
				{
					list2.Add("is not revealed");
				}
				if (item.HasView && !item.VisualShown)
				{
					list2.Add("has no visible marker");
				}
				if (item.HasView && !item.ClearOfAnchor)
				{
					list2.Add("is buried inside its anchor's model");
				}
				if (!item.Reachable)
				{
					list2.Add("has no road the party can travel from where it stands");
				}
				if (list2.Count > 0)
				{
					list.Add("World-map marker " + item.Label + " is offered and its anchor is revealed, but it " + string.Join(", ", list2) + ".");
				}
			}
		}
		return list;
	}

	internal static List<MarkerFacts> ReadMarkers()
	{
		List<MarkerFacts> list = new List<MarkerFacts>();
		GlobalMapView instance = GlobalMapView.Instance;
		BlueprintGlobalMap blueprintGlobalMap = ((instance != null) ? instance.Blueprint : null);
		if (blueprintGlobalMap == null)
		{
			return list;
		}
		GlobalMapState state = instance.State;
		bool flag = PlanarColiseumMapPoint.SavedPosition(state.Player) != null || state.Player?.TravelData != null;
		foreach (PlanarColiseumMapPoint.Tile tile in PlanarColiseumMapPoint.Tiles)
		{
			if (tile?.Point == null || tile.Map != blueprintGlobalMap)
			{
				continue;
			}
			MarkerFacts markerFacts = new MarkerFacts
			{
				Label = tile.Point.name
			};
			try
			{
				markerFacts.Offered = PlanarColiseumMapPoint.IsTileOffered(tile);
				markerFacts.AnchorRevealed = state.GetPointState(tile.Anchor).IsRevealed;
				GlobalMapPointView pointView = instance.GetPointView(tile.Point);
				GlobalMapPointView pointView2 = instance.GetPointView(tile.Anchor);
				markerFacts.HasView = pointView != null;
				markerFacts.Revealed = state.GetPointState(tile.Point).IsRevealed;
				markerFacts.VisualShown = pointView != null && pointView.LocationVisual != null && pointView.LocationVisual.activeInHierarchy;
				markerFacts.Reachable = state.Player?.Location == tile.Point || (flag && state.PathManager.CalculatePlayerPathToLocation(tile.Point) != null);
				if (pointView != null && pointView2 != null)
				{
					Vector3 position = pointView2.transform.position;
					Vector3 position2 = pointView.transform.position;
					markerFacts.ClearOfAnchor = Vector3.Distance(new Vector3(position.x, 0f, position.z), new Vector3(position2.x, 0f, position2.z)) > PlanarColiseumMapPoint.AnchorFootprint(pointView2);
				}
				GlobalMapEdgeState edgeState = state.GetEdgeState(tile.Edge);
				markerFacts.Note = "beside " + tile.Anchor?.name + ((pointView != null && pointView2 != null) ? $", {Vector3.Distance(pointView2.transform.position, pointView.transform.position):0.00} from it" : "") + string.Format(", road explored {0:0.00}, party at {1}", edgeState.Explored1 + edgeState.Explored2, state.Player?.Location?.name ?? "(on a road)");
			}
			catch (Exception ex)
			{
				markerFacts.Note = "Reading the tile threw " + ex.GetType().Name + ": " + ex.Message;
			}
			list.Add(markerFacts);
		}
		return list;
	}

	internal static SelfTestChecks.Outcome MarkersOnLoadedMap(int chapter)
	{
		GlobalMapView instance = GlobalMapView.Instance;
		if (instance == null || instance.Blueprint == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "No world map is loaded, so the markers could not be read.");
		}
		List<MarkerFacts> list = ReadMarkers();
		List<string> list2 = new List<string> { $"Chapter {chapter}, map '{instance.Blueprint.name}', {list.Count} mod tile(s) on it." };
		foreach (MarkerFacts item in list)
		{
			list2.Add($"{item.Label}: offered={item.Offered} anchorRevealed={item.AnchorRevealed} view={item.HasView} revealed={item.Revealed} visible={item.VisualShown} reachable={item.Reachable} clearOfAnchor={item.ClearOfAnchor}; {item.Note}");
		}
		List<string> list3 = MarkerProblems(list);
		list2.AddRange(list3);
		if (list.Count == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "No mod tile is registered on '" + instance.Blueprint.name + "'; the tiles were never built.", list2);
		}
		return SelfTestChecks.Outcome.Of(list3.Count == 0, (list3.Count == 0) ? "Every marker the mod offers on this map is drawn, revealed and reachable, or waits for an anchor the chapter has not revealed yet." : "A marker the mod offers on this map cannot be seen or reached.", list2);
	}
}
