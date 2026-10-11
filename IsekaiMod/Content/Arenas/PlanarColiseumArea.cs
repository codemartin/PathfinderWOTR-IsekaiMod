using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.RandomEncounters.Settings;
using Kingmaker.View;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas;

public static class PlanarColiseumArea
{
	internal sealed class ArenaDefinition
	{
		internal int Act;

		internal string Stem;

		internal string AreaGuid;

		internal string EnterPointGuid;

		internal string HostAreaGuid;

		internal string HostEnterPointGuid;

		internal string AreaName => Stem + "Area";

		internal string EnterPointName => Stem + "EnterPoint";
	}

	[HarmonyPatch(typeof(AreaEnterPoint), "FindAreaEnterPointOnScene")]
	internal static class AreaEnterPoint_FindAreaEnterPointOnScene_Patch
	{
		[HarmonyPostfix]
		private static void Postfix(BlueprintAreaEnterPoint blueprint, ref AreaEnterPoint __result)
		{
			if (__result != null || blueprint == null)
			{
				return;
			}
			try
			{
				BlueprintAreaEnterPoint blueprintAreaEnterPoint = HostEnterPointFor(blueprint);
				if (blueprintAreaEnterPoint != null)
				{
					__result = AreaEnterPoint.FindAreaEnterPointOnScene(blueprintAreaEnterPoint);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Enter point fallback failed: " + ex);
			}
		}
	}

	internal const string FaeGroveHostAreaGuid = "147f91771a79f584492c40eba4cb603f";

	internal const string FaeGroveHostEnterPointGuid = "41d090f733ced2049a94985e46bd3714";

	internal const string LostLegionsHostAreaGuid = "c7125176689d3814199266e2ad2c3077";

	internal const string LostLegionsHostEnterPointGuid = "67254758f3eb1c14c8e538b72ac0eea4";

	internal const string HostAreaGuid = "b99ed02cbe28dfe40ac1299cf971784a";

	internal const string HostEnterPointGuid = "19eea6fee7243eb4bb116ea5b751c34a";

	internal const string VoidRiftHostAreaGuid = "b99ed02cbe28dfe40ac1299cf971784a";

	internal const string VoidRiftHostEnterPointGuid = "19eea6fee7243eb4bb116ea5b751c34a";

	internal const string MultiverseThresholdHostAreaGuid = "784f113d0dc6d0c4bbac0d91abeb7f32";

	internal const string MultiverseThresholdHostEnterPointGuid = "922c67f78dfdcff4082da5a9b7a80fd1";

	internal const string FaeGroveArenaName = "PlanarColiseumFaeGrove";

	internal const string FaeGroveAreaGuid = "e661b603477a5b84bde2de2f53f076a8";

	internal const string FaeGroveEnterPointGuid = "32d12698bba65ce28609b1911818ed90";

	internal const string LostLegionsArenaName = "PlanarColiseumLostLegions";

	internal const string LostLegionsAreaGuid = "07f731a4043b52f097245309e00fe2a1";

	internal const string LostLegionsEnterPointGuid = "2c0793d8ea115a92bbccbd1d6d620f5a";

	internal const string ColiseumArenaName = "PlanarColiseum";

	internal const string ColiseumAreaGuid = "05f68f97b98e5988b8e079033bc5752b";

	internal const string ColiseumEnterPointGuid = "128fddc8f8b6587cb22a592b8a07a904";

	internal const string VoidRiftArenaName = "PlanarColiseumVoidRift";

	internal const string VoidRiftAreaGuid = "9195dd67d5d450248f6f808a989bec9c";

	internal const string VoidRiftEnterPointGuid = "5da71035ee4d5a94971d18c5991b4f6e";

	internal const string MultiverseThresholdArenaName = "PlanarColiseumMultiverseThreshold";

	internal const string MultiverseThresholdAreaGuid = "4240fda390e055ceb515442bb9f59351";

	internal const string MultiverseThresholdEnterPointGuid = "748abd19e5dc565b9c4e87ddcaa885bc";

	internal static readonly ArenaDefinition[] Arenas = new ArenaDefinition[5]
	{
		new ArenaDefinition
		{
			Act = 1,
			Stem = "PlanarColiseumFaeGrove",
			AreaGuid = "e661b603477a5b84bde2de2f53f076a8",
			EnterPointGuid = "32d12698bba65ce28609b1911818ed90",
			HostAreaGuid = "147f91771a79f584492c40eba4cb603f",
			HostEnterPointGuid = "41d090f733ced2049a94985e46bd3714"
		},
		new ArenaDefinition
		{
			Act = 2,
			Stem = "PlanarColiseumLostLegions",
			AreaGuid = "07f731a4043b52f097245309e00fe2a1",
			EnterPointGuid = "2c0793d8ea115a92bbccbd1d6d620f5a",
			HostAreaGuid = "c7125176689d3814199266e2ad2c3077",
			HostEnterPointGuid = "67254758f3eb1c14c8e538b72ac0eea4"
		},
		new ArenaDefinition
		{
			Act = 3,
			Stem = "PlanarColiseum",
			AreaGuid = "05f68f97b98e5988b8e079033bc5752b",
			EnterPointGuid = "128fddc8f8b6587cb22a592b8a07a904",
			HostAreaGuid = "b99ed02cbe28dfe40ac1299cf971784a",
			HostEnterPointGuid = "19eea6fee7243eb4bb116ea5b751c34a"
		},
		new ArenaDefinition
		{
			Act = 4,
			Stem = "PlanarColiseumVoidRift",
			AreaGuid = "9195dd67d5d450248f6f808a989bec9c",
			EnterPointGuid = "5da71035ee4d5a94971d18c5991b4f6e",
			HostAreaGuid = "b99ed02cbe28dfe40ac1299cf971784a",
			HostEnterPointGuid = "19eea6fee7243eb4bb116ea5b751c34a"
		},
		new ArenaDefinition
		{
			Act = 5,
			Stem = "PlanarColiseumMultiverseThreshold",
			AreaGuid = "4240fda390e055ceb515442bb9f59351",
			EnterPointGuid = "748abd19e5dc565b9c4e87ddcaa885bc",
			HostAreaGuid = "784f113d0dc6d0c4bbac0d91abeb7f32",
			HostEnterPointGuid = "922c67f78dfdcff4082da5a9b7a80fd1"
		}
	};

	internal static readonly string[] ArenaAreaGuids = Arenas.Select((ArenaDefinition a) => a.AreaGuid).ToArray();

	internal static readonly string[] ArenaAreaNames = Arenas.Select((ArenaDefinition a) => a.AreaName).ToArray();

	internal static readonly string[] ArenaEnterPointGuids = Arenas.Select((ArenaDefinition a) => a.EnterPointGuid).ToArray();

	public static BlueprintArea ColiseumArea;

	public static BlueprintAreaEnterPoint ColiseumEnterPoint;

	private static readonly Dictionary<int, BlueprintAreaEnterPoint> s_EnterPointsByAct = new Dictionary<int, BlueprintAreaEnterPoint>();

	private static readonly Dictionary<BlueprintGuid, BlueprintAreaEnterPoint> s_HostEnterPoints = new Dictionary<BlueprintGuid, BlueprintAreaEnterPoint>();

	private static readonly Dictionary<string, BlueprintAreaEnterPoint> s_EnterPointsByGuid = new Dictionary<string, BlueprintAreaEnterPoint>(StringComparer.OrdinalIgnoreCase);

	private const BindingFlags DeclaredInstance = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

	private static readonly MethodInfo MemberwiseCloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

	public static void Add()
	{
		AddArena(1, () => CreateArena("PlanarColiseumFaeGrove", "147f91771a79f584492c40eba4cb603f", "41d090f733ced2049a94985e46bd3714", Helpers.CreateString(Main.IsekaiContext, "PlanarColiseumFaeGroveArea.Name", "Planar Coliseum: The Fae Grove"), out var _));
		AddArena(2, () => CreateArena("PlanarColiseumLostLegions", "c7125176689d3814199266e2ad2c3077", "67254758f3eb1c14c8e538b72ac0eea4", Helpers.CreateString(Main.IsekaiContext, "PlanarColiseumLostLegionsArea.Name", "Planar Coliseum: The Lost Legions"), out var _));
		AddArena(3, delegate
		{
			BlueprintAreaEnterPoint blueprintAreaEnterPoint = CreateArena("PlanarColiseum", "b99ed02cbe28dfe40ac1299cf971784a", "19eea6fee7243eb4bb116ea5b751c34a", Helpers.CreateString(Main.IsekaiContext, "PlanarColiseumArea.Name", "Planar Coliseum: The Midnight Colosseum"), out var area);
			ColiseumArea = area;
			ColiseumEnterPoint = blueprintAreaEnterPoint;
			return blueprintAreaEnterPoint;
		});
		AddArena(4, () => CreateArena("PlanarColiseumVoidRift", "b99ed02cbe28dfe40ac1299cf971784a", "19eea6fee7243eb4bb116ea5b751c34a", Helpers.CreateString(Main.IsekaiContext, "PlanarColiseumVoidRiftArea.Name", "Planar Coliseum: The Void Rift"), out var _));
		AddArena(5, () => CreateArena("PlanarColiseumMultiverseThreshold", "784f113d0dc6d0c4bbac0d91abeb7f32", "922c67f78dfdcff4082da5a9b7a80fd1", Helpers.CreateString(Main.IsekaiContext, "PlanarColiseumMultiverseThresholdArea.Name", "Planar Coliseum: The Multiverse Threshold"), out var _));
	}

	private static void AddArena(int act, Func<BlueprintAreaEnterPoint> create)
	{
		try
		{
			BlueprintAreaEnterPoint blueprintAreaEnterPoint = create();
			if (blueprintAreaEnterPoint != null)
			{
				s_EnterPointsByAct[act] = blueprintAreaEnterPoint;
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] The arena for Act {act} failed: {arg}");
		}
	}

	public static BlueprintAreaEnterPoint AnyArenaEnterPoint()
	{
		return ColiseumEnterPoint ?? (from p in s_EnterPointsByAct
			orderby p.Key
			select p.Value).FirstOrDefault();
	}

	public static BlueprintAreaEnterPoint EnterPointForAct(int act)
	{
		if (!s_EnterPointsByAct.TryGetValue(act, out var value))
		{
			return null;
		}
		return value;
	}

	internal static BlueprintAreaEnterPoint CreateArena(string arenaName, string hostAreaGuid, string hostEnterPointGuid, LocalizedString displayName, out BlueprintArea area)
	{
		area = null;
		BlueprintArea hostArea = BlueprintTools.GetBlueprint<BlueprintArea>(hostAreaGuid);
		BlueprintAreaEnterPoint blueprint = BlueprintTools.GetBlueprint<BlueprintAreaEnterPoint>(hostEnterPointGuid);
		if (hostArea == null || blueprint == null)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Arena " + arenaName + ": host area " + hostAreaGuid + " or enter point " + hostEnterPointGuid + " not found.");
			return null;
		}
		if (blueprint.Area != hostArea)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Arena " + arenaName + ": " + blueprint.name + " is not an enter point of " + hostArea.name + ".");
			return null;
		}
		if (hostArea.HasParts || hostArea.IsGlobalMap)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Arena " + arenaName + ": " + hostArea.name + " has area parts or is a global map and cannot host an arena.");
			return null;
		}
		BlueprintArea arenaArea = Helpers.CreateBlueprint(Main.IsekaiContext, arenaName + "Area", delegate(BlueprintArea bp)
		{
			CopyAreaFields(hostArea, bp);
			bp.AreaName = displayName ?? new LocalizedString();
			bp.AreaLocalName = new LocalizedString();
			bp.ExcludeFromSave = true;
			if (bp.RandomEncounterSettings == null)
			{
				bp.RandomEncounterSettings = new RandomEncounterSettings();
			}
			bp.RandomEncounterSettings.AreRandomEncountersEnabled = false;
			bp.AddComponent(delegate(CombatRandomEncounterAreaSettings c)
			{
				c.m_DefaultEnterPoint = new BlueprintAreaEnterPointReference();
				c.m_GoodAvoidanceEnterPoint = new BlueprintAreaEnterPointReference();
				c.AllowedNaturalSettings = Array.Empty<GlobalMapZone>();
				c.Formations = Array.Empty<CombatRandomEncounterAreaSettings.Formation>();
			});
		});
		BlueprintAreaEnterPoint blueprintAreaEnterPoint = Helpers.CreateBlueprint(Main.IsekaiContext, arenaName + "EnterPoint", delegate(BlueprintAreaEnterPoint bp)
		{
			bp.m_Area = arenaArea.ToReference<BlueprintAreaReference>();
			BlueprintFieldDefaults.FillNullFields(bp);
		});
		s_HostEnterPoints[blueprintAreaEnterPoint.AssetGuid] = blueprint;
		s_EnterPointsByGuid[blueprintAreaEnterPoint.AssetGuid.ToString()] = blueprintAreaEnterPoint;
		area = arenaArea;
		return blueprintAreaEnterPoint;
	}

	internal static void CopyAreaFields(BlueprintArea host, BlueprintArea target)
	{
		if (host == null || target == null)
		{
			return;
		}
		Type[] array = new Type[2]
		{
			typeof(BlueprintAreaPart),
			typeof(BlueprintArea)
		};
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo[] fields = array[i].GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsInitOnly && !fieldInfo.IsLiteral)
				{
					fieldInfo.SetValue(target, CopyValue(fieldInfo.GetValue(host)));
				}
			}
		}
	}

	private static object CopyValue(object value)
	{
		if (value != null)
		{
			if (!(value is Array array))
			{
				if (!(value is BlueprintReferenceBase blueprintReferenceBase))
				{
					if (value is CampingSettings || value is RandomEncounterSettings)
					{
						return MemberwiseCloneMethod.Invoke(value, null);
					}
					Type type = value.GetType();
					if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
					{
						IList list = (IList)Activator.CreateInstance(type);
						{
							foreach (object item in (IList)value)
							{
								list.Add(CopyValue(item));
							}
							return list;
						}
					}
					return value;
				}
				BlueprintReferenceBase obj = (BlueprintReferenceBase)Activator.CreateInstance(blueprintReferenceBase.GetType());
				obj.ReadGuidFromGuid(blueprintReferenceBase.Guid);
				return obj;
			}
			return array.Clone();
		}
		return null;
	}

	internal static int ArenaActForChapter(int chapter)
	{
		if (chapter <= 1)
		{
			return 1;
		}
		if (chapter < 5)
		{
			return chapter;
		}
		return 5;
	}

	internal static string ArenaEnterPointGuidForChapter(int chapter)
	{
		int act = ArenaActForChapter(chapter);
		return Arenas.First((ArenaDefinition a) => a.Act == act).EnterPointGuid;
	}

	internal static int ArenaActForArea(string areaGuid, string areaName)
	{
		ArenaDefinition[] arenas = Arenas;
		foreach (ArenaDefinition arenaDefinition in arenas)
		{
			if (!string.IsNullOrEmpty(areaGuid) && string.Equals(arenaDefinition.AreaGuid, areaGuid, StringComparison.OrdinalIgnoreCase))
			{
				return arenaDefinition.Act;
			}
			if (!string.IsNullOrEmpty(areaName) && string.Equals(arenaDefinition.AreaName, areaName, StringComparison.Ordinal))
			{
				return arenaDefinition.Act;
			}
		}
		return 0;
	}

	public static int CurrentArenaAct()
	{
		BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
		if (blueprintArea != null)
		{
			return ArenaActForArea(blueprintArea.AssetGuid.ToString(), blueprintArea.name);
		}
		return 0;
	}

	public static BlueprintAreaEnterPoint CurrentArenaEnterPoint()
	{
		string key = ArenaEnterPointGuidForChapter(Game.Instance?.Player?.Chapter ?? 1);
		if (!s_EnterPointsByGuid.TryGetValue(key, out var value))
		{
			return AnyArenaEnterPoint();
		}
		return value;
	}

	internal static bool IsArenaArea(string areaGuid, string areaName)
	{
		if (!string.IsNullOrEmpty(areaGuid) && ArenaAreaGuids.Any((string g) => string.Equals(g, areaGuid, StringComparison.OrdinalIgnoreCase)))
		{
			return true;
		}
		if (!string.IsNullOrEmpty(areaName))
		{
			return ArenaAreaNames.Any((string n) => string.Equals(n, areaName, StringComparison.Ordinal));
		}
		return false;
	}

	internal static bool IsArenaArea(BlueprintArea area)
	{
		if (area != null)
		{
			return IsArenaArea(area.AssetGuid.ToString(), area.name);
		}
		return false;
	}

	internal static bool IsArenaEnterPointGuid(string enterPointGuid)
	{
		if (!string.IsNullOrEmpty(enterPointGuid))
		{
			return ArenaEnterPointGuids.Any((string g) => string.Equals(g, enterPointGuid, StringComparison.OrdinalIgnoreCase));
		}
		return false;
	}

	internal static bool IsArenaEnterPoint(BlueprintAreaEnterPoint enterPoint)
	{
		if (enterPoint != null)
		{
			return IsArenaEnterPointGuid(enterPoint.AssetGuid.ToString());
		}
		return false;
	}

	internal static BlueprintAreaEnterPoint HostEnterPointFor(BlueprintAreaEnterPoint enterPoint)
	{
		if (enterPoint == null)
		{
			return null;
		}
		if (!s_HostEnterPoints.TryGetValue(enterPoint.AssetGuid, out var value))
		{
			return null;
		}
		return value;
	}
}
