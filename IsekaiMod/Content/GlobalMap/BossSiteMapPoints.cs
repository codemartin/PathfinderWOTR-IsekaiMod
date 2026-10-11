using System;
using System.Collections.Generic;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Quests;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints.Area;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.GlobalMap;

public static class BossSiteMapPoints
{
	internal enum MapId
	{
		Kenabres,
		WorldWound
	}

	internal const string KenabresMapGuid = "5a903425adbaa02439df63ee129a6fb8";

	internal const string WorldWoundMapGuid = "38ef391d8b3ee9742ad713ada0b62bed";

	internal const string KenabresAnchorGuid = "40b6a5c759b584343987a1dfcf919a06";

	internal const string WorldWoundKenabresAnchorGuid = "7e1ba32e81ab3e94c9030aeb0dfd14ac";

	internal const string WorldWoundDrezenAnchorGuid = "70990a0f7010447478e68b8e16ae52b2";

	internal const string KenabresBreachPointName = "IsekaiDimensionalBreachMapPoint_Kenabres";

	internal const string KenabresBreachEdgeName = "IsekaiDimensionalBreachMapEdge_Kenabres";

	internal const string WorldWoundBreachPointName = "IsekaiDimensionalBreachMapPoint_WorldWound";

	internal const string WorldWoundBreachEdgeName = "IsekaiDimensionalBreachMapEdge_WorldWound";

	internal const string KenabresAnomaly1PointName = "IsekaiAnomalyMapPoint_Act1_Kenabres";

	internal const string KenabresAnomaly1EdgeName = "IsekaiAnomalyMapEdge_Act1_Kenabres";

	internal const string WorldWoundAnomaly1PointName = "IsekaiAnomalyMapPoint_Act1_WorldWound";

	internal const string WorldWoundAnomaly1EdgeName = "IsekaiAnomalyMapEdge_Act1_WorldWound";

	internal const string WorldWoundAnomaly2PointName = "IsekaiAnomalyMapPoint_Act2_WorldWound";

	internal const string WorldWoundAnomaly2EdgeName = "IsekaiAnomalyMapEdge_Act2_WorldWound";

	internal const string WorldWoundAnomaly3PointName = "IsekaiAnomalyMapPoint_Act3_WorldWound";

	internal const string WorldWoundAnomaly3EdgeName = "IsekaiAnomalyMapEdge_Act3_WorldWound";

	internal const string WorldWoundAnomaly4PointName = "IsekaiAnomalyMapPoint_Act4_WorldWound";

	internal const string WorldWoundAnomaly4EdgeName = "IsekaiAnomalyMapEdge_Act4_WorldWound";

	internal const string WorldWoundAnomaly5PointName = "IsekaiAnomalyMapPoint_Act5_WorldWound";

	internal const string WorldWoundAnomaly5EdgeName = "IsekaiAnomalyMapEdge_Act5_WorldWound";

	public static BlueprintGlobalMapPoint KenabresBreach;

	public static BlueprintGlobalMapPoint WorldWoundBreach;

	private static readonly Dictionary<int, List<BlueprintGlobalMapPoint>> s_AnomalyTiles = new Dictionary<int, List<BlueprintGlobalMapPoint>>();

	public static IEnumerable<BlueprintGlobalMapPoint> AnomalyTiles(int act)
	{
		if (!s_AnomalyTiles.TryGetValue(act, out var value))
		{
			return Array.Empty<BlueprintGlobalMapPoint>();
		}
		return value;
	}

	internal static string MapGuid(MapId map)
	{
		if (map != MapId.Kenabres)
		{
			return "38ef391d8b3ee9742ad713ada0b62bed";
		}
		return "5a903425adbaa02439df63ee129a6fb8";
	}

	internal static string AnchorFor(BossSiteKind kind, int act, MapId map)
	{
		if (kind == BossSiteKind.Rift)
		{
			if (act < 1 || act > 5)
			{
				return null;
			}
			if (map != MapId.Kenabres)
			{
				return "7e1ba32e81ab3e94c9030aeb0dfd14ac";
			}
			return "40b6a5c759b584343987a1dfcf919a06";
		}
		if (act < 1 || act > 5)
		{
			return null;
		}
		if (map == MapId.Kenabres)
		{
			if (act != 1)
			{
				return null;
			}
			return "40b6a5c759b584343987a1dfcf919a06";
		}
		if (act > 2)
		{
			return "70990a0f7010447478e68b8e16ae52b2";
		}
		return "7e1ba32e81ab3e94c9030aeb0dfd14ac";
	}

	internal static bool BreachShown(MapId map, QuestState questState, int chapter, IList<QuestObjectiveState> actStates)
	{
		if (!BossSites.RiftOffered(questState, chapter, actStates))
		{
			return false;
		}
		int num = BossSites.AccessibleRiftAct(chapter, actStates);
		if (map != MapId.Kenabres)
		{
			if (num >= 1)
			{
				return num <= 5;
			}
			return false;
		}
		return num == 1;
	}

	internal static bool AnomalyTileShown(MapId map, int act, int chapter, bool ownsEncounters)
	{
		if (map == MapId.Kenabres && act != 1)
		{
			return false;
		}
		return BossSites.AnomalyOffered(act, chapter, ownsEncounters);
	}

	public static void Add()
	{
		BlueprintAreaEnterPoint blueprintAreaEnterPoint = BossSites.AnyRiftSiteEnterPoint();
		if (blueprintAreaEnterPoint != null)
		{
			LocalizedString name = Helpers.CreateString(Main.IsekaiContext, "IsekaiDimensionalBreachMapPoint.Name", "Dimensional Breach");
			LocalizedString description = Helpers.CreateString(Main.IsekaiContext, "IsekaiDimensionalBreachMapPoint.Description", "A wound in the dimensional fabric, humming with the next incursion of The Outer Threshold. Whichever rift act is open waits on its other side.");
			KenabresBreach = PlanarColiseumMapPoint.AddTile("5a903425adbaa02439df63ee129a6fb8", AnchorFor(BossSiteKind.Rift, 1, MapId.Kenabres), "IsekaiDimensionalBreachMapPoint_Kenabres", "IsekaiDimensionalBreachMapEdge_Kenabres", blueprintAreaEnterPoint, name, description, () => BreachShownInSave(MapId.Kenabres));
			WorldWoundBreach = PlanarColiseumMapPoint.AddTile("38ef391d8b3ee9742ad713ada0b62bed", AnchorFor(BossSiteKind.Rift, 1, MapId.WorldWound), "IsekaiDimensionalBreachMapPoint_WorldWound", "IsekaiDimensionalBreachMapEdge_WorldWound", blueprintAreaEnterPoint, name, description, () => BreachShownInSave(MapId.WorldWound));
		}
		else
		{
			Main.IsekaiContext.Logger.LogError("[BossSites] No rift site was created; no breach tile. The emissary calls the rift bosses where the party stands.");
		}
		LocalizedString name2 = Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act1.Name", "Barren Anomaly: The Outer World Stray");
		LocalizedString description2 = Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act1.Description", "A frozen wood where a satyr archer (CR 4) strayed out of the First World. Its first defeat in a playthrough awards 300 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins.");
		AddAnomalyTile(1, MapId.Kenabres, "IsekaiAnomalyMapPoint_Act1_Kenabres", "IsekaiAnomalyMapEdge_Act1_Kenabres", name2, description2);
		AddAnomalyTile(1, MapId.WorldWound, "IsekaiAnomalyMapPoint_Act1_WorldWound", "IsekaiAnomalyMapEdge_Act1_WorldWound", name2, description2);
		AddAnomalyTile(2, MapId.WorldWound, "IsekaiAnomalyMapPoint_Act2_WorldWound", "IsekaiAnomalyMapEdge_Act2_WorldWound", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act2.Name", "Barren Anomaly: Chronos Exile"), Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act2.Description", "Ashen hills where an ash giant (CR 11) walks, cast adrift in time. Its first defeat in a playthrough awards 600 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins."));
		AddAnomalyTile(3, MapId.WorldWound, "IsekaiAnomalyMapPoint_Act3_WorldWound", "IsekaiAnomalyMapEdge_Act3_WorldWound", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act3.Name", "Barren Anomaly: The Transdimensional Behemoth"), Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act3.Description", "Snowbound cliffs where a towering glabrezu (CR 14) was dragged through a planar fissure. Its first defeat in a playthrough awards 1,000 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins."));
		AddAnomalyTile(4, MapId.WorldWound, "IsekaiAnomalyMapPoint_Act4_WorldWound", "IsekaiAnomalyMapEdge_Act4_WorldWound", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act4.Name", "Barren Anomaly: Abyssal Void Vavakia"), Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act4.Description", "Demon-blasted Sarkoris, where an advanced vavakia (CR 19) warped by the Abyss waits. Its first defeat in a playthrough awards 1,500 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins."));
		AddAnomalyTile(5, MapId.WorldWound, "IsekaiAnomalyMapPoint_Act5_WorldWound", "IsekaiAnomalyMapEdge_Act5_WorldWound", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act5.Name", "Barren Anomaly: Cosmic Arbiter of Ruin"), Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalyMapPoint_Act5.Description", "An echo of burning Kenabres, where a balor (CR 20) waits to pass judgment on your transmigrated soul. Its first defeat in a playthrough awards 2,500 Cosmic Coins; a rematch pays a tenth of the normal experience and no coins."));
	}

	private static void AddAnomalyTile(int act, MapId map, string pointName, string edgeName, LocalizedString name, LocalizedString description)
	{
		BlueprintAreaEnterPoint blueprintAreaEnterPoint = BossSites.SiteEnterPointFor(BossSiteKind.Anomaly, act);
		if (blueprintAreaEnterPoint == null)
		{
			Main.IsekaiContext.Logger.LogError($"[BossSites] Anomaly {act} has no site; no tile {pointName}. The emissary calls it where the party stands.");
			return;
		}
		BlueprintGlobalMapPoint blueprintGlobalMapPoint = PlanarColiseumMapPoint.AddTile(MapGuid(map), AnchorFor(BossSiteKind.Anomaly, act, map), pointName, edgeName, blueprintAreaEnterPoint, name, description, () => AnomalyTileShownInSave(map, act));
		if (blueprintGlobalMapPoint != null)
		{
			if (!s_AnomalyTiles.TryGetValue(act, out var value))
			{
				value = (s_AnomalyTiles[act] = new List<BlueprintGlobalMapPoint>());
			}
			value.Add(blueprintGlobalMapPoint);
		}
	}

	private static bool BreachShownInSave(MapId map)
	{
		int valueOrDefault = (Game.Instance?.Player?.Chapter).GetValueOrDefault();
		if (BossSites.CurrentRiftSiteEnterPoint() == null)
		{
			return false;
		}
		return BreachShown(map, IsekaiTransmigrationQuest.QuestStateInSave(), valueOrDefault, IsekaiTransmigrationQuest.ActStates());
	}

	private static bool AnomalyTileShownInSave(MapId map, int act)
	{
		int valueOrDefault = (Game.Instance?.Player?.Chapter).GetValueOrDefault();
		return AnomalyTileShown(map, act, valueOrDefault, IsekaiTransmigrationQuest.PlayerCanStartTransmigration());
	}
}
