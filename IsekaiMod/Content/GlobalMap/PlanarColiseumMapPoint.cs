using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using IsekaiMod.Content.Arenas;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Globalmap.State;
using Kingmaker.Globalmap.View;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IsekaiMod.Content.GlobalMap;

public static class PlanarColiseumMapPoint
{
	internal sealed class Tile
	{
		internal BlueprintGlobalMap Map;

		internal BlueprintGlobalMapPoint Anchor;

		internal BlueprintGlobalMapPoint Point;

		internal BlueprintGlobalMapEdge Edge;

		internal Func<bool> Offered;
	}

	[HarmonyPatch(typeof(GlobalMapView), "OnAreaDidLoad")]
	internal static class GlobalMapView_OnAreaDidLoad_Patch
	{
		[HarmonyPrefix]
		private static void Prefix(GlobalMapView __instance)
		{
			if (__instance == null || s_Tiles.Count == 0)
			{
				return;
			}
			BlueprintGlobalMap blueprint;
			try
			{
				blueprint = __instance.Blueprint;
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not read the loaded world map: " + ex);
				return;
			}
			bool arenaEnabled = Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true;
			foreach (Tile s_Tile in s_Tiles)
			{
				if (s_Tile.Map != blueprint)
				{
					continue;
				}
				if (!IsOffered(s_Tile, arenaEnabled))
				{
					MovePartyOffBrokenTile(__instance, s_Tile);
					continue;
				}
				try
				{
					BuildTile(__instance, s_Tile);
				}
				catch (Exception arg)
				{
					Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] Could not place {s_Tile.Point?.name} on {blueprint?.name}: {arg}");
					MovePartyOffBrokenTile(__instance, s_Tile);
				}
			}
		}
	}

	private sealed class AnchorRevealHandler : ILocationHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnLocationRevealed(GlobalMapPointState location)
		{
			try
			{
				GlobalMapView instance = GlobalMapView.Instance;
				if (location?.Blueprint == null || instance == null || s_Tiles.Count == 0)
				{
					return;
				}
				BlueprintGlobalMap blueprint = instance.Blueprint;
				foreach (Tile s_Tile in s_Tiles)
				{
					if (s_Tile.Map == blueprint)
					{
						GlobalMapPointView pointView = instance.GetPointView(s_Tile.Point);
						GlobalMapPointState pointState = instance.State.GetPointState(s_Tile.Point);
						GlobalMapEdgeState edgeState = instance.State.GetEdgeState(s_Tile.Edge);
						if (TileFollowsReveal(location.Blueprint, s_Tile.Anchor, pointView != null, pointState.IsRevealed, edgeState.Revealed))
						{
							edgeState.Explored1 = 1f;
							instance.GetEdgeView(s_Tile.Edge)?.UpdateRenderers();
							instance.RevealLocation(pointView);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not reveal a tile beside a newly revealed location: " + ex);
			}
		}

		public void OnLocationHidden(GlobalMapPointState location)
		{
		}
	}

	internal const string KenabresMapGuid = "5a903425adbaa02439df63ee129a6fb8";

	internal const string KenabresAnchorGuid = "40b6a5c759b584343987a1dfcf919a06";

	internal const string WorldWoundMapGuid = "38ef391d8b3ee9742ad713ada0b62bed";

	internal const string WorldWoundAnchorGuid = "70990a0f7010447478e68b8e16ae52b2";

	internal const string KenabresPointName = "PlanarColiseumMapPoint_Kenabres";

	internal const string KenabresEdgeName = "PlanarColiseumMapEdge_Kenabres";

	internal const string WorldWoundPointName = "PlanarColiseumMapPoint_WorldWound";

	internal const string WorldWoundEdgeName = "PlanarColiseumMapEdge_WorldWound";

	internal const float TileFraction = 0.4f;

	private static readonly List<Tile> s_Tiles = new List<Tile>();

	private static readonly FieldInfo s_PlayerPositionField = AccessTools.Field(typeof(GlobalMapPlayerState), "m_Position");

	private static readonly FieldInfo s_PlayerTravelDataField = AccessTools.Field(typeof(GlobalMapPlayerState), "m_TravelData");

	public static BlueprintGlobalMapPoint KenabresPoint;

	public static BlueprintGlobalMapPoint WorldWoundPoint;

	internal const float MarkerClearance = 1f;

	private const double MinSeparation = Math.PI * 7.0 / 36.0;

	private const double MinClearance = 0.75;

	internal static readonly FieldInfo SavedPositionField = s_PlayerPositionField;

	private static bool s_Subscribed;

	internal static IReadOnlyList<Tile> Tiles => s_Tiles;

	internal static bool IsTileOffered(Tile tile)
	{
		if (tile != null)
		{
			return IsOffered(tile, Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true);
		}
		return false;
	}

	public static void Add()
	{
		BlueprintAreaEnterPoint blueprintAreaEnterPoint = PlanarColiseumArea.EnterPointForAct(1) ?? PlanarColiseumArea.AnyArenaEnterPoint();
		BlueprintAreaEnterPoint blueprintAreaEnterPoint2 = PlanarColiseumArea.EnterPointForAct(2) ?? PlanarColiseumArea.AnyArenaEnterPoint();
		if (blueprintAreaEnterPoint == null || blueprintAreaEnterPoint2 == null)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] No arena was created; no world-map tile.");
			return;
		}
		LocalizedString name = Helpers.CreateString(Main.IsekaiContext, "PlanarColiseumMapPoint.Name", "Planar Coliseum");
		LocalizedString description = Helpers.CreateString(Main.IsekaiContext, "PlanarColiseumMapPoint.Description", "A rift of starlight anchored beside the road, where champions from across the multiverse fight before the watching constellations. The tournament cups, the relic exchange and the Oracle of the Constellations wait on its sands.");
		KenabresPoint = AddTile("5a903425adbaa02439df63ee129a6fb8", "40b6a5c759b584343987a1dfcf919a06", "PlanarColiseumMapPoint_Kenabres", "PlanarColiseumMapEdge_Kenabres", blueprintAreaEnterPoint, name, description);
		WorldWoundPoint = AddTile("38ef391d8b3ee9742ad713ada0b62bed", "70990a0f7010447478e68b8e16ae52b2", "PlanarColiseumMapPoint_WorldWound", "PlanarColiseumMapEdge_WorldWound", blueprintAreaEnterPoint2, name, description);
	}

	internal static BlueprintGlobalMapPoint AddTile(string mapGuid, string anchorGuid, string pointName, string edgeName, BlueprintAreaEnterPoint enterPoint, LocalizedString name, LocalizedString description, Func<bool> offered = null)
	{
		try
		{
			BlueprintGlobalMap blueprint = BlueprintTools.GetBlueprint<BlueprintGlobalMap>(mapGuid);
			BlueprintGlobalMapPoint anchor = BlueprintTools.GetBlueprint<BlueprintGlobalMapPoint>(anchorGuid);
			if (blueprint == null || anchor == null || anchor.GlobalMap != blueprint)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] World-map tile " + pointName + ": map " + mapGuid + " or anchor " + anchorGuid + " is missing or mismatched.");
				return null;
			}
			HashSet<BlueprintGlobalMapEdge> modRoads = new HashSet<BlueprintGlobalMapEdge>(from t in s_Tiles
				where t?.Edge != null
				select t.Edge);
			float num = (from e in blueprint.GetEdges(anchor)
				where e != null && e.Length > 0f && !modRoads.Contains(e)
				select e.Length).DefaultIfEmpty(1f).Min();
			float length = 0.4f * num;
			BlueprintGlobalMapPoint blueprintGlobalMapPoint = GlobalMapVariationManager.AddMapPoint(blueprint, pointName, name, description, enterPoint, delegate(BlueprintGlobalMapPoint bp)
			{
				bp.GlobalMapZone = anchor.GlobalMapZone;
				bp.Region = anchor.Region;
				bp.OverrideRandomEncounterZoneSize = true;
				bp.NoRandomEncounterZoneSize = length * 1.1f;
			});
			BlueprintGlobalMapEdge blueprintGlobalMapEdge = GlobalMapVariationManager.AddMapEdge(blueprint, edgeName, anchor, blueprintGlobalMapPoint, GlobalMapEdgeType.PassableOnlyByPlayer, GlobalMapEdgePriority.Side, length);
			if (blueprintGlobalMapPoint == null || blueprintGlobalMapEdge == null)
			{
				return null;
			}
			s_Tiles.Add(new Tile
			{
				Map = blueprint,
				Anchor = anchor,
				Point = blueprintGlobalMapPoint,
				Edge = blueprintGlobalMapEdge,
				Offered = offered
			});
			SubscribeRevealHandler();
			return blueprintGlobalMapPoint;
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] World-map tile {pointName} failed: {arg}");
			return null;
		}
	}

	internal static float TileDistance(float roadDistance, float anchorFootprint, float markerClearance)
	{
		float val = 0.4f * ((roadDistance > 0f) ? roadDistance : 1f);
		float val2 = Math.Max(0f, anchorFootprint) + Math.Max(0f, markerClearance);
		return Math.Max(val, val2);
	}

	internal static int ShortestOwnRoad(IList<(float Length, bool ModRoad)> roads)
	{
		int num = -1;
		for (int i = 0; i < (roads?.Count ?? 0); i++)
		{
			if (!roads[i].ModRoad && (num < 0 || roads[i].Length < roads[num].Length))
			{
				num = i;
			}
		}
		return num;
	}

	private static float FootprintRadius(GlobalMapPointView anchorView, Vector3 anchorPosition)
	{
		float num = 0f;
		Renderer[] componentsInChildren = anchorView.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!(renderer == null))
			{
				Bounds bounds = renderer.bounds;
				if (!(bounds.size.x <= 0f) || !(bounds.size.z <= 0f))
				{
					num = Math.Max(num, Math.Max(Math.Max(Math.Abs(bounds.max.x - anchorPosition.x), Math.Abs(bounds.min.x - anchorPosition.x)), Math.Max(Math.Abs(bounds.max.z - anchorPosition.z), Math.Abs(bounds.min.z - anchorPosition.z))));
				}
			}
		}
		return num;
	}

	internal static float AnchorFootprint(GlobalMapPointView anchorView)
	{
		if (!(anchorView == null))
		{
			return FootprintRadius(anchorView, anchorView.transform.position);
		}
		return 0f;
	}

	internal static (float X, float Z) ChooseTilePosition((float X, float Z) anchor, IList<(float X, float Z)> neighbours, IList<(float X, float Z)> others, float distance)
	{
		neighbours = neighbours ?? Array.Empty<(float, float)>();
		others = others ?? Array.Empty<(float, float)>();
		if (distance <= 0f)
		{
			distance = 1f;
		}
		double num = 0.0;
		double num2 = 0.0;
		List<double> list = new List<double>();
		foreach (var neighbour in neighbours)
		{
			double num3 = neighbour.X - anchor.X;
			double num4 = neighbour.Z - anchor.Z;
			double num5 = Math.Sqrt(num3 * num3 + num4 * num4);
			if (!(num5 < 0.0001))
			{
				num += num3 / num5;
				num2 += num4 / num5;
				list.Add(Math.Atan2(num4, num3));
			}
		}
		double num6 = ((num * num + num2 * num2 > 1E-06) ? Math.Atan2(0.0 - num2, 0.0 - num) : 0.0);
		(float, float) result = (anchor.X + distance, anchor.Z);
		double num7 = double.MinValue;
		for (int i = 0; i <= 12; i++)
		{
			int num8 = (i + 1) / 2;
			double angle = num6 + (double)(((i % 2 == 1) ? 1 : (-1)) * num8) * Math.PI / 6.0;
			(float, float) candidate = ((float)((double)anchor.X + Math.Cos(angle) * (double)distance), (float)((double)anchor.Z + Math.Sin(angle) * (double)distance));
			double num9 = ((list.Count == 0) ? Math.PI : list.Min((double road) => Math.Abs(Math.Atan2(Math.Sin(angle - road), Math.Cos(angle - road)))));
			double num10 = ((others.Count == 0) ? double.MaxValue : others.Min(((float X, float Z) o) => Distance(o, candidate)));
			if (num9 >= Math.PI * 7.0 / 36.0 && num10 >= 0.75 * (double)distance)
			{
				return candidate;
			}
			double num11 = Math.Min(num10 / (double)distance, 10.0) + num9 / Math.PI;
			if (num11 > num7)
			{
				num7 = num11;
				result = candidate;
			}
		}
		return result;
	}

	private static double Distance((float X, float Z) a, (float X, float Z) b)
	{
		double num = a.X - b.X;
		double num2 = a.Z - b.Z;
		return Math.Sqrt(num * num + num2 * num2);
	}

	private static bool IsOffered(Tile tile, bool arenaEnabled)
	{
		if (tile.Offered == null)
		{
			return arenaEnabled;
		}
		try
		{
			return tile.Offered();
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] Could not decide whether {tile.Point?.name} is offered: {arg}");
			return false;
		}
	}

	internal static bool PartyIsStrandedOn(object location, object positionEdge, object travelTarget, IEnumerable<object> travelEdges, object tilePoint, object tileEdge)
	{
		if (tilePoint != null && location == tilePoint)
		{
			return true;
		}
		if (tilePoint != null && travelTarget == tilePoint)
		{
			return true;
		}
		if (tileEdge == null)
		{
			return false;
		}
		if (positionEdge == tileEdge)
		{
			return true;
		}
		if (travelTarget == tileEdge)
		{
			return true;
		}
		if (travelEdges != null && travelEdges.Any((object edge) => edge == tileEdge))
		{
			return true;
		}
		return false;
	}

	internal static GlobalMapPosition SavedPosition(GlobalMapPlayerState player)
	{
		if (player == null || SavedPositionField == null)
		{
			return null;
		}
		return SavedPositionField.GetValue(player) as GlobalMapPosition;
	}

	internal static bool ClearTravelSafely(Action finishTravel, Action clearStoredTravel)
	{
		try
		{
			finishTravel?.Invoke();
			return true;
		}
		catch (Exception ex)
		{
			SafeLog("[PlanarColiseum] FinishTravel could not run; the stored travel is cleared directly: " + ex);
		}
		try
		{
			clearStoredTravel?.Invoke();
			return true;
		}
		catch (Exception ex2)
		{
			SafeLog("[PlanarColiseum] The stored travel could not be cleared: " + ex2);
			return false;
		}
	}

	private static void SafeLog(string message)
	{
		try
		{
			Main.IsekaiContext.Logger.LogError(message);
		}
		catch (Exception)
		{
		}
	}

	private static void MovePartyOffBrokenTile(GlobalMapView mapView, Tile tile)
	{
		try
		{
			if (GlobalMapPointView.Instances.Any((GlobalMapPointView v) => v != null && v.Blueprint == tile.Point))
			{
				return;
			}
			GlobalMapPlayerState player = mapView.State?.Player;
			if (player == null)
			{
				return;
			}
			GlobalMapTravelData travelData = player.TravelData;
			List<BlueprintGlobalMapEdge> list = new List<BlueprintGlobalMapEdge>();
			if (travelData != null)
			{
				if (travelData.CurrentEdge?.Blueprint != null)
				{
					list.Add(travelData.CurrentEdge.Blueprint);
				}
				foreach (TravelEdge item in travelData.Path)
				{
					if (item?.Blueprint != null)
					{
						list.Add(item.Blueprint);
					}
				}
			}
			BlueprintGlobalMapEdge positionEdge = ((travelData != null) ? null : SavedPosition(player)?.Edge);
			if (!PartyIsStrandedOn(player.Location, positionEdge, travelData?.To?.Location, list, tile.Point, tile.Edge))
			{
				return;
			}
			player.SetCurrentPosition(new GlobalMapPosition(tile.Anchor));
			if (travelData != null)
			{
				ClearTravelSafely(player.FinishTravel, delegate
				{
					s_PlayerTravelDataField?.SetValue(player, null);
				});
			}
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] The party was on " + tile.Point?.name + " or its road; it was moved to " + tile.Anchor?.name + ".");
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not move the party off the coliseum tile: " + ex);
		}
	}

	private static void BuildTile(GlobalMapView mapView, Tile tile)
	{
		if (GlobalMapPointView.Instances.Any((GlobalMapPointView v) => v != null && v.Blueprint == tile.Point))
		{
			return;
		}
		GlobalMapPointView anchorView = GlobalMapPointView.Instances.FirstOrDefault((GlobalMapPointView v) => v != null && v.Blueprint == tile.Anchor);
		if (anchorView == null)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] " + tile.Anchor.name + " has no view on " + tile.Map.name + "; no coliseum tile.");
			MovePartyOffBrokenTile(mapView, tile);
			return;
		}
		Vector3 position = anchorView.transform.position;
		HashSet<BlueprintGlobalMapEdge> hashSet = new HashSet<BlueprintGlobalMapEdge>(from t in s_Tiles
			where t?.Edge != null
			select t.Edge);
		List<(GlobalMapEdgeView, float, float, bool, float, float)> list = new List<(GlobalMapEdgeView, float, float, bool, float, float)>();
		foreach (GlobalMapEdgeView instance in GlobalMapEdgeView.Instances)
		{
			BlueprintGlobalMapEdge blueprintGlobalMapEdge = ((instance != null) ? instance.Blueprint : null);
			if (blueprintGlobalMapEdge != null && blueprintGlobalMapEdge != tile.Edge && blueprintGlobalMapEdge.Contains(tile.Anchor))
			{
				BlueprintGlobalMapPoint other = ((blueprintGlobalMapEdge.Point1 == tile.Anchor) ? blueprintGlobalMapEdge.Point2 : blueprintGlobalMapEdge.Point1);
				GlobalMapPointView globalMapPointView = GlobalMapPointView.Instances.FirstOrDefault((GlobalMapPointView v) => v != null && v.Blueprint == other);
				if (!(globalMapPointView == null))
				{
					Vector3 position2 = globalMapPointView.transform.position;
					list.Add((instance, blueprintGlobalMapEdge.Length, Vector3.Distance(new Vector3(position.x, 0f, position.z), new Vector3(position2.x, 0f, position2.z)), hashSet.Contains(blueprintGlobalMapEdge), position2.x, position2.z));
				}
			}
		}
		List<(float, float)> neighbours = list.Select<(GlobalMapEdgeView, float, float, bool, float, float), (float, float)>(((GlobalMapEdgeView View, float Length, float Distance, bool ModRoad, float X, float Z) r) => (X: r.X, Z: r.Z)).ToList();
		int num = ShortestOwnRoad(list.Select<(GlobalMapEdgeView, float, float, bool, float, float), (float, bool)>(((GlobalMapEdgeView View, float Length, float Distance, bool ModRoad, float X, float Z) r) => (Length: r.Length, ModRoad: r.ModRoad)).ToList());
		GlobalMapEdgeView template = ((num >= 0) ? list[num].Item1 : null);
		float num2 = ((num >= 0) ? list[num].Item3 : 0f);
		List<(float, float)> others = (from v in GlobalMapPointView.Instances
			where v != null && v != anchorView
			select (x: v.transform.position.x, z: v.transform.position.z)).ToList();
		float num3 = FootprintRadius(anchorView, position);
		float num4 = TileDistance(num2, num3, 1f * Mathf.Abs(anchorView.transform.lossyScale.x));
		(float, float) tuple = ChooseTilePosition((X: position.x, Z: position.z), neighbours, others, num4);
		Vector3 vector = new Vector3(tuple.Item1, 0f, tuple.Item2);
		GlobalMapPointView globalMapPointView2 = CreatePointView(mapView, anchorView, tile.Point, vector);
		GlobalMapEdgeView globalMapEdgeView;
		try
		{
			globalMapEdgeView = CreateEdgeView(template, anchorView, tile.Edge, new Vector3(position.x, 0f, position.z), vector);
		}
		catch
		{
			Discard(globalMapPointView2.gameObject);
			throw;
		}
		GlobalMapState state = mapView.State;
		if (state.GetPointState(tile.Anchor).IsRevealed)
		{
			state.GetPointState(tile.Point).Reveal();
			state.GetEdgeState(tile.Edge).Explored1 = 1f;
		}
		Main.IsekaiContext.Logger.Log("[PlanarColiseum] Placed " + globalMapPointView2.name + " and " + globalMapEdgeView.name + " on " + tile.Map.name + " beside " + tile.Anchor.name + ", " + $"{num4:0.00} from it (road {num2:0.00}, model half-width {num3:0.00}).");
	}

	internal static bool TileFollowsReveal(object revealed, object tileAnchor, bool tileHasView, bool tileRevealed, bool roadExplored)
	{
		if (revealed == null || tileAnchor == null || revealed != tileAnchor)
		{
			return false;
		}
		if (tileHasView)
		{
			if (tileRevealed)
			{
				return !roadExplored;
			}
			return true;
		}
		return false;
	}

	private static void SubscribeRevealHandler()
	{
		if (!s_Subscribed)
		{
			s_Subscribed = true;
			EventBus.Subscribe(new AnchorRevealHandler());
		}
	}

	private static GlobalMapPointView CreatePointView(GlobalMapView mapView, GlobalMapPointView anchorView, BlueprintGlobalMapPoint point, Vector3 position)
	{
		GameObject gameObject = new GameObject(point.name);
		gameObject.SetActive(value: false);
		bool flag = false;
		try
		{
			SceneManager.MoveGameObjectToScene(gameObject, anchorView.gameObject.scene);
			gameObject.layer = anchorView.gameObject.layer;
			gameObject.transform.SetParent(anchorView.transform.parent, worldPositionStays: false);
			gameObject.transform.localScale = anchorView.transform.localScale;
			gameObject.transform.position = position;
			GlobalMapPointView globalMapPointView = gameObject.AddComponent<GlobalMapPointView>();
			globalMapPointView.m_Blueprint = point.ToReference<BlueprintGlobalMapPoint.Reference>();
			mapView.SetLocationVisual(globalMapPointView);
			gameObject.SetActive(value: true);
			flag = true;
			return globalMapPointView;
		}
		finally
		{
			if (!flag)
			{
				Discard(gameObject);
			}
		}
	}

	private static GlobalMapEdgeView CreateEdgeView(GlobalMapEdgeView template, GlobalMapPointView anchorView, BlueprintGlobalMapEdge edge, Vector3 from, Vector3 to)
	{
		GameObject gameObject = new GameObject(edge.name);
		gameObject.SetActive(value: false);
		bool flag = false;
		try
		{
			GameObject gameObject2 = ((template != null) ? template.gameObject : anchorView.gameObject);
			SceneManager.MoveGameObjectToScene(gameObject, gameObject2.scene);
			gameObject.layer = gameObject2.layer;
			gameObject.transform.SetParent(gameObject2.transform.parent, worldPositionStays: false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			GlobalMapEdgeView globalMapEdgeView = gameObject.AddComponent<GlobalMapEdgeView>();
			globalMapEdgeView.m_Blueprint = edge.ToReference<BlueprintGlobalMapEdge.Reference>();
			globalMapEdgeView.m_CustomEncounters = Array.Empty<BlueprintRandomEncounterReference>();
			Vector3 vector = gameObject.transform.InverseTransformPoint(from);
			Vector3 vector2 = gameObject.transform.InverseTransformPoint(to);
			if (!SetSplineNodes(gameObject, new List<Vector3>
			{
				vector,
				vector,
				(vector + vector2) * 0.5f,
				vector2,
				vector2
			}))
			{
				throw new InvalidOperationException("VectorSpline.SetNodes(IEnumerable<Vector3>) was not found on the edge.");
			}
			gameObject.SetActive(value: true);
			flag = true;
			return globalMapEdgeView;
		}
		finally
		{
			if (!flag)
			{
				Discard(gameObject);
			}
		}
	}

	private static void Discard(GameObject go)
	{
		if (!(go == null))
		{
			go.SetActive(value: false);
			UnityEngine.Object.Destroy(go);
		}
	}

	private static bool SetSplineNodes(GameObject edgeObject, List<Vector3> nodes)
	{
		Component component = edgeObject.GetComponent("VectorSpline");
		MethodInfo methodInfo = ((component != null) ? component.GetType().GetMethod("SetNodes", new Type[1] { typeof(IEnumerable<Vector3>) }) : null);
		if (methodInfo == null)
		{
			return false;
		}
		methodInfo.Invoke(component, new object[1] { nodes });
		return true;
	}
}
