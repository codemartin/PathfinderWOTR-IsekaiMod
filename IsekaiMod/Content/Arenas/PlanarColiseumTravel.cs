using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.View;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

public static class PlanarColiseumTravel
{
	internal enum ExitRoute
	{
		Unchanged,
		ArrivalMap,
		ReturnPoint
	}

	[HarmonyPatch(typeof(Game), "LoadArea", new Type[]
	{
		typeof(BlueprintAreaEnterPoint),
		typeof(AutoSaveMode),
		typeof(Action)
	})]
	internal static class Game_LoadArea_Patch
	{
		[HarmonyPrefix]
		private static void Prefix(ref BlueprintAreaEnterPoint areaEnterPoint)
		{
			if (areaEnterPoint == null)
			{
				return;
			}
			try
			{
				BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
				bool flag = blueprintArea != null && IsPlanarArea(blueprintArea.AssetGuid.ToString(), blueprintArea.name);
				if (PlanarColiseumArea.IsArenaEnterPoint(areaEnterPoint))
				{
					areaEnterPoint = PlanarColiseumArea.CurrentArenaEnterPoint() ?? areaEnterPoint;
					if (!flag)
					{
						RecordArrival(blueprintArea);
					}
					return;
				}
				string text = BossSites.ResolveSiteEntry(areaEnterPoint.AssetGuid.ToString(), (Game.Instance?.Player?.Chapter).GetValueOrDefault(), IsekaiTransmigrationQuest.ActStates());
				if (text != null)
				{
					areaEnterPoint = BossSites.CreatedEnterPoint(text) ?? areaEnterPoint;
					if (!flag)
					{
						RecordArrival(blueprintArea);
					}
				}
				else if (flag)
				{
					BlueprintArea area = areaEnterPoint.Area;
					bool flag2 = area?.IsGlobalMap ?? false;
					string destinationMapGuid = ((!flag2) ? null : (BlueprintRoot.Instance?.GlobalMap?.GetGlobalMap(areaEnterPoint) ?? BlueprintRoot.Instance?.GlobalMap?.GetGlobalMap(area))?.AssetGuid.ToString());
					string text2 = ReadArrivalMapGuid();
					BlueprintAreaEnterPoint blueprintAreaEnterPoint = null;
					switch (DecideExit(leavingArena: true, flag2, destinationMapGuid, text2))
					{
					case ExitRoute.Unchanged:
						return;
					case ExitRoute.ArrivalMap:
						blueprintAreaEnterPoint = ArrivalMap()?.GlobalMapEnterPoint ?? ReturnPoint();
						break;
					case ExitRoute.ReturnPoint:
						blueprintAreaEnterPoint = ReturnPoint();
						break;
					}
					if (blueprintAreaEnterPoint != null)
					{
						Main.IsekaiContext.Logger.Log("[PlanarColiseum] The arena's exit led to " + areaEnterPoint.name + "; the party came from " + (string.IsNullOrEmpty(text2) ? "a rift" : ("global map " + text2)) + " and goes to " + blueprintAreaEnterPoint.name + ".");
						areaEnterPoint = blueprintAreaEnterPoint;
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Exit guard failed; the load continues unchanged: " + ex);
			}
		}
	}

	internal static ExitRoute DecideExit(bool leavingArena, bool destinationIsGlobalMap, string destinationMapGuid, string arrivalMapGuid)
	{
		if (!leavingArena || !destinationIsGlobalMap)
		{
			return ExitRoute.Unchanged;
		}
		if (string.IsNullOrEmpty(arrivalMapGuid))
		{
			return ExitRoute.ReturnPoint;
		}
		if (!string.Equals(arrivalMapGuid, destinationMapGuid, StringComparison.OrdinalIgnoreCase))
		{
			return ExitRoute.ArrivalMap;
		}
		return ExitRoute.Unchanged;
	}

	internal static bool IsPlanarArea(string areaGuid, string areaName)
	{
		if (!PlanarColiseumArea.IsArenaArea(areaGuid, areaName))
		{
			return BossSites.IsSiteArea(areaGuid, areaName);
		}
		return true;
	}

	internal static string ResolveReturnEnterPointGuid(string arrivalMapEnterPointGuid, string recordedEnterPoint, int? recordedChapter, int chapter)
	{
		if (!string.IsNullOrEmpty(arrivalMapEnterPointGuid))
		{
			return arrivalMapEnterPointGuid;
		}
		return ColiseumProgress.ResolveReturnPoint(recordedEnterPoint, recordedChapter, chapter);
	}

	internal static void RecordArrival(BlueprintArea from)
	{
		Dictionary<string, string> dictionary = ColiseumProgress.Ensure();
		if (dictionary != null)
		{
			BlueprintGlobalMap blueprintGlobalMap = ((from == null || !from.IsGlobalMap) ? null : BlueprintRoot.Instance?.GlobalMap?.GetGlobalMap(from));
			if (blueprintGlobalMap != null)
			{
				dictionary["ArrivalMap"] = blueprintGlobalMap.AssetGuid.ToString();
			}
			else
			{
				dictionary.Remove("ArrivalMap");
			}
		}
	}

	internal static string ReadArrivalMapGuid()
	{
		return SaveScopedStorage.Get(ColiseumProgress.Find(), "ArrivalMap");
	}

	private static BlueprintGlobalMap ArrivalMap()
	{
		string text = ReadArrivalMapGuid();
		if (!string.IsNullOrEmpty(text))
		{
			return BlueprintTools.GetBlueprint<BlueprintGlobalMap>(text);
		}
		return null;
	}

	internal static int ChooseAnchor(IList<(float X, float Z, bool InCurrentPart)> candidates, (float X, float Z) party)
	{
		if (candidates == null)
		{
			return -1;
		}
		int result = -1;
		double num = double.MaxValue;
		for (int i = 0; i < candidates.Count; i++)
		{
			if (candidates[i].InCurrentPart)
			{
				double num2 = candidates[i].X - party.X;
				double num3 = candidates[i].Z - party.Z;
				double num4 = num2 * num2 + num3 * num3;
				if (num4 < num)
				{
					num = num4;
					result = i;
				}
			}
		}
		return result;
	}

	internal static BlueprintAreaEnterPoint FindNearestEnterPoint()
	{
		try
		{
			Game instance = Game.Instance;
			BlueprintArea blueprintArea = instance?.CurrentlyLoadedArea;
			UnitEntityData unitEntityData = instance?.Player?.MainCharacter.Value;
			if (blueprintArea == null || blueprintArea.IsGlobalMap || unitEntityData == null)
			{
				return null;
			}
			BlueprintAreaPart blueprintAreaPart = instance.CurrentlyLoadedAreaPart ?? blueprintArea;
			List<BlueprintAreaEnterPoint> list = new List<BlueprintAreaEnterPoint>();
			List<(float, float, bool)> list2 = new List<(float, float, bool)>();
			foreach (AreaEnterPoint item in AreaEnterPoint.Instances.Where((AreaEnterPoint v) => v != null && v.m_Blueprint != null))
			{
				try
				{
					BlueprintAreaEnterPoint blueprint = item.Blueprint;
					if (blueprint != null && blueprint.Area == blueprintArea && !PlanarColiseumArea.IsArenaEnterPoint(blueprint))
					{
						BlueprintAreaPart blueprintAreaPart2 = blueprint.AreaPart ?? blueprint.Area;
						Vector3 position = item.transform.position;
						list.Add(blueprint);
						list2.Add((position.x, position.z, blueprintAreaPart2 == blueprintAreaPart));
					}
				}
				catch (Exception ex)
				{
					Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Skipped an enter point while anchoring a rift: " + ex.Message);
				}
			}
			Vector3 position2 = unitEntityData.Position;
			int num = ChooseAnchor(list2, (X: position2.x, Z: position2.z));
			return (num < 0) ? null : list[num];
		}
		catch (Exception ex2)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not anchor the rift on an entrance: " + ex2);
			return null;
		}
	}

	internal static void RecordReturnPoint(string enterPointGuid)
	{
		int chapter = Game.Instance?.Player?.Chapter ?? 1;
		ColiseumProgress.WriteReturnPoint(ColiseumProgress.Ensure(), enterPointGuid, chapter);
	}

	private static BlueprintAreaEnterPoint ReturnPoint()
	{
		int num = Game.Instance?.Player?.Chapter ?? 1;
		return BlueprintTools.GetBlueprint<BlueprintAreaEnterPoint>(ColiseumProgress.ReadReturnPoint(ColiseumProgress.Find(), num)) ?? BlueprintTools.GetBlueprint<BlueprintAreaEnterPoint>(ActionEnterColiseum.ResolveReturnEnterPoint(null, num));
	}

	internal static BlueprintAreaEnterPoint ResolveReturn()
	{
		int chapter = Game.Instance?.Player?.Chapter ?? 1;
		string arrivalMapEnterPointGuid = ArrivalMap()?.GlobalMapEnterPoint?.AssetGuid.ToString();
		ColiseumProgress.ReadRecordedReturnPoint(ColiseumProgress.Find(), out var enterPointGuid, out var chapter2);
		return BlueprintTools.GetBlueprint<BlueprintAreaEnterPoint>(ResolveReturnEnterPointGuid(arrivalMapEnterPointGuid, enterPointGuid, chapter2, chapter)) ?? ReturnPoint();
	}
}
