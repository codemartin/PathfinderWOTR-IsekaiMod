using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Quests;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.GlobalMap
{
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

		public static void LinkObjectiveToMapPoint(BlueprintQuestObjective objective, BlueprintGlobalMapPoint point)
		{
			if (objective != null && point != null)
			{
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
		}

		public static void LinkObjectiveToMultiEntrance(BlueprintQuestObjective objective, BlueprintMultiEntranceEntry entry)
		{
			if (objective != null && entry != null)
			{
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
	}
}
