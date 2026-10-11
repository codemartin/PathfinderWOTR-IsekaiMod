using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Quests;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Kingdom;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.GlobalMap;

public static class GlobalMapVariationManager
{
	public static BlueprintGlobalMapPointVariation AddPointVariation(BlueprintGlobalMapPoint point, string variationName, LocalizedString displayName, LocalizedString description, ConditionsChecker conditions, BlueprintAreaEnterPoint areaEntrance = null, BlueprintDialog bookEvent = null, BlueprintMultiEntrance multiEntrance = null)
	{
		if (point == null)
		{
			Main.IsekaiContext.Logger.LogError("[GlobalMapVariation] Cannot add variation '" + variationName + "' to null point.");
			return null;
		}
		BlueprintGlobalMapPointVariation blueprintGlobalMapPointVariation = Helpers.CreateBlueprint(Main.IsekaiContext, variationName, delegate(BlueprintGlobalMapPointVariation bp)
		{
			bp.Name = displayName;
			bp.Description = description;
			bp.Conditions = conditions ?? new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
			if (areaEntrance != null)
			{
				bp.m_AreaEntrance = areaEntrance.ToReference<BlueprintAreaEnterPointReference>();
			}
			if (bookEvent != null)
			{
				bp.m_BookEvent = bookEvent.ToReference<BlueprintDialogReference>();
			}
			if (multiEntrance != null)
			{
				bp.m_Entrances = multiEntrance.ToReference<BlueprintMultiEntranceReference>();
			}
		});
		List<BlueprintGlobalMapPointVariation.Reference> list = point.LocationVariations?.ToList() ?? new List<BlueprintGlobalMapPointVariation.Reference>();
		BlueprintGlobalMapPointVariation.Reference newRef = blueprintGlobalMapPointVariation.ToReference<BlueprintGlobalMapPointVariation.Reference>();
		if (!list.Any((BlueprintGlobalMapPointVariation.Reference r) => ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)newRef).deserializedGuid))
		{
			list.Add(newRef);
			point.LocationVariations = list.ToArray();
			Main.IsekaiContext.Logger.Log("[GlobalMapVariation] Added variation '" + variationName + "' to map point '" + point.name + "'.");
		}
		return blueprintGlobalMapPointVariation;
	}

	public static BlueprintGlobalMapPoint AddMapPoint(BlueprintGlobalMap map, string pointName, LocalizedString displayName, LocalizedString description, BlueprintAreaEnterPoint areaEntrance, Action<BlueprintGlobalMapPoint> init = null)
	{
		if (map == null)
		{
			Main.IsekaiContext.Logger.LogError("[GlobalMapVariation] Cannot add point '" + pointName + "' to a null map.");
			return null;
		}
		BlueprintGlobalMapPoint blueprintGlobalMapPoint = Helpers.CreateBlueprint(Main.IsekaiContext, pointName, delegate(BlueprintGlobalMapPoint bp)
		{
			bp.m_GlobalMap = map.ToReference<BlueprintGlobalMap.Reference>();
			bp.Type = GlobalMapPointType.Location;
			bp.ExploreOnEnter = true;
			bp.Name = displayName;
			bp.Description = description;
			if (areaEntrance != null)
			{
				bp.m_AreaEntrance = areaEntrance.ToReference<BlueprintAreaEnterPointReference>();
			}
			bp.ResourceStats = new KingdomStats.Changes();
			init?.Invoke(bp);
			BlueprintFieldDefaults.FillNullFields(bp);
		});
		BlueprintGlobalMapPoint.Reference reference = blueprintGlobalMapPoint.ToReference<BlueprintGlobalMapPoint.Reference>();
		BlueprintGlobalMapPoint.Reference[] array = map.Points ?? Array.Empty<BlueprintGlobalMapPoint.Reference>();
		if (!array.Any((BlueprintGlobalMapPoint.Reference r) => r != null && ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)reference).deserializedGuid))
		{
			map.Points = array.Concat(new BlueprintGlobalMapPoint.Reference[1] { reference }).ToArray();
			Main.IsekaiContext.Logger.Log("[GlobalMapVariation] Added point '" + pointName + "' to map '" + map.name + "'.");
		}
		return blueprintGlobalMapPoint;
	}

	public static BlueprintGlobalMapEdge AddMapEdge(BlueprintGlobalMap map, string edgeName, BlueprintGlobalMapPoint point1, BlueprintGlobalMapPoint point2, GlobalMapEdgeType type, GlobalMapEdgePriority priority, float length)
	{
		if (map == null || point1 == null || point2 == null || point1.GlobalMap != map || point2.GlobalMap != map)
		{
			Main.IsekaiContext.Logger.LogError("[GlobalMapVariation] Cannot add edge '" + edgeName + "': both points must belong to the map.");
			return null;
		}
		BlueprintGlobalMapEdge blueprintGlobalMapEdge = Helpers.CreateBlueprint(Main.IsekaiContext, edgeName, delegate(BlueprintGlobalMapEdge bp)
		{
			bp.Type = type;
			bp.Priority = priority;
			bp.m_Point1 = point1.ToReference<BlueprintGlobalMapPoint.Reference>();
			bp.m_Point2 = point2.ToReference<BlueprintGlobalMapPoint.Reference>();
			bp.Length = length;
			BlueprintFieldDefaults.FillNullFields(bp);
		});
		BlueprintGlobalMapEdge.Reference reference = blueprintGlobalMapEdge.ToReference<BlueprintGlobalMapEdge.Reference>();
		if (map.Edges == null)
		{
			map.Edges = new List<BlueprintGlobalMapEdge.Reference>();
		}
		if (!map.Edges.Any((BlueprintGlobalMapEdge.Reference r) => r != null && ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)reference).deserializedGuid))
		{
			map.Edges.Add(reference);
			Main.IsekaiContext.Logger.Log("[GlobalMapVariation] Added edge '" + edgeName + "' to map '" + map.name + "'.");
		}
		return blueprintGlobalMapEdge;
	}

	public static void LinkObjectiveToMapPoint(BlueprintQuestObjective objective, BlueprintGlobalMapPoint point)
	{
		if (objective == null || point == null)
		{
			Main.IsekaiContext.Logger.LogError("[GlobalMapVariation] Cannot link null objective or point.");
			return;
		}
		if (objective.Locations == null)
		{
			objective.Locations = new List<BlueprintGlobalMapPoint.Reference>();
		}
		BlueprintGlobalMapPoint.Reference pointRef = point.ToReference<BlueprintGlobalMapPoint.Reference>();
		if (!objective.Locations.Any((BlueprintGlobalMapPoint.Reference r) => ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)pointRef).deserializedGuid))
		{
			objective.Locations.Add(pointRef);
			Main.IsekaiContext.Logger.Log("[GlobalMapVariation] Linked objective '" + objective.name + "' to map point '" + point.name + "'.");
		}
	}

	public static void LinkObjectiveToMultiEntrance(BlueprintQuestObjective objective, BlueprintMultiEntranceEntry entry)
	{
		if (objective == null || entry == null)
		{
			Main.IsekaiContext.Logger.LogError("[GlobalMapVariation] Cannot link null objective or multi-entrance entry.");
			return;
		}
		if (objective.MultiEntranceEntries == null)
		{
			objective.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
		}
		BlueprintMultiEntranceEntry.Reference entryRef = entry.ToReference<BlueprintMultiEntranceEntry.Reference>();
		if (!objective.MultiEntranceEntries.Any((BlueprintMultiEntranceEntry.Reference r) => ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)entryRef).deserializedGuid))
		{
			objective.MultiEntranceEntries.Add(entryRef);
			Main.IsekaiContext.Logger.Log("[GlobalMapVariation] Linked objective '" + objective.name + "' to multi-entrance entry '" + entry.name + "'.");
		}
	}
}
