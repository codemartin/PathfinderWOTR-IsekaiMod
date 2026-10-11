using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Quests;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints.Area;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas;

public static class BossSites
{
	internal sealed class SiteDefinition
	{
		internal BossSiteKind Kind;

		internal int Act;

		internal string Stem;

		internal string AreaGuid;

		internal string EnterPointGuid;

		internal string HostAreaGuid;

		internal string HostEnterPointGuid;

		internal string FallbackHostAreaGuid;

		internal string FallbackHostEnterPointGuid;

		internal string AreaName => Stem + "Area";

		internal string EnterPointName => Stem + "EnterPoint";
	}

	internal enum AccessPoint
	{
		None,
		KenabresMap,
		WorldWoundMap,
		AlushinyrraEmissary,
		ThresholdEmissary
	}

	internal const string KenabresStreetsHostAreaGuid = "9cccc3f5dd88e00448d500fbe954769c";

	internal const string KenabresStreetsHostEnterPointGuid = "ebec4fa949aae2b48a3973b61aeba107";

	internal const string KenabresStreets02HostAreaGuid = "ad3aa89f0d3f463478d25f62991e82b2";

	internal const string KenabresStreets02HostEnterPointGuid = "295dc4efb57f6674ab09df54245cc95a";

	internal const string KenabresStreets03HostAreaGuid = "851d755cdab9b9a49ae533fd6add5d52";

	internal const string KenabresStreets03HostEnterPointGuid = "62d964e3d294d374b8326f740ef7debd";

	internal const string KenabresStreets04HostAreaGuid = "80b9a3ae6d4c5f24a9fd5a3968c4003f";

	internal const string KenabresStreets04HostEnterPointGuid = "8358e79beca657649a59589ae98f659d";

	internal const string KenabresStreets05HostAreaGuid = "97657db76559e6242b13b7ea922714a9";

	internal const string KenabresStreets05HostEnterPointGuid = "94a1a320038a462449dd46d4e3863d42";

	internal const string KareliaCanyonHostAreaGuid = "03fca664fc43fb040aae42bb0f41c23c";

	internal const string KareliaCanyonHostEnterPointGuid = "b7b729eb8b3ff074a89d6a10e3ae4b49";

	internal const string KareliaRuinsHostAreaGuid = "b732856564ff898419dfc2431a01efce";

	internal const string KareliaRuinsHostEnterPointGuid = "3861a9ccea5bdb0438ab76cca6de0754";

	internal const string SnowCliffsHostAreaGuid = "fa42b091997a6914f89a1b2e2e4b3257";

	internal const string SnowCliffsHostEnterPointGuid = "a5903ee26d5dab54ca295ba44f6a2661";

	internal const string SnowHillsHostAreaGuid = "a188ac8c8acc065459ce1ca68b41e861";

	internal const string SnowHillsHostEnterPointGuid = "a23f83fadb2c2af42a177abc09da6d9a";

	internal const string WinterForestHostAreaGuid = "50581d70db957d64b90d06cd1cd197e9";

	internal const string WinterForestHostEnterPointGuid = "f1ac59dbf579a87479411e0656136481";

	internal const string WoundedForestHostAreaGuid = "da702e8d79c6046439c17e7e9cbfd758";

	internal const string WoundedForestHostEnterPointGuid = "ef93f4358d1e1be41a9a396712601a5a";

	internal const string WoundCracks2HostAreaGuid = "08ef1831c8de3274c88d9e2016ea2997";

	internal const string WoundCracks2HostEnterPointGuid = "fe90614de7418a84c9317a5931e2591d";

	internal const string SarkorisDisasterHostAreaGuid = "fd10ce9065ac3ce4f9955b21819e1aec";

	internal const string SarkorisDisasterHostEnterPointGuid = "f674344b94eb6a04a8036be585abfc10";

	internal const string WorldwoundHostAreaGuid = "2a2b91bad89a73b4f9e86b219ed74ce3";

	internal const string WorldwoundHostEnterPointGuid = "a0548f2e54eae6e4b9e729982514f0f7";

	internal const string RiftAct1Stem = "IsekaiRiftSiteAct1";

	internal const string RiftAct2Stem = "IsekaiRiftSiteAct2";

	internal const string RiftAct3Stem = "IsekaiRiftSiteAct3";

	internal const string RiftAct4Stem = "IsekaiRiftSiteAct4";

	internal const string RiftAct5Stem = "IsekaiRiftSiteAct5";

	internal const string RiftAct6Stem = "IsekaiRiftSiteAct6";

	internal const string AnomalyAct1Stem = "IsekaiAnomalySiteAct1";

	internal const string AnomalyAct2Stem = "IsekaiAnomalySiteAct2";

	internal const string AnomalyAct3Stem = "IsekaiAnomalySiteAct3";

	internal const string AnomalyAct4Stem = "IsekaiAnomalySiteAct4";

	internal const string AnomalyAct5Stem = "IsekaiAnomalySiteAct5";

	internal static readonly SiteDefinition[] Sites = new SiteDefinition[11]
	{
		Site(BossSiteKind.Rift, 1, "IsekaiRiftSiteAct1", "45691335077a56e0aaaf8bb6c1944dca", "6c7cbcdee14655779b5ff64920646a98", "9cccc3f5dd88e00448d500fbe954769c", "ebec4fa949aae2b48a3973b61aeba107", "ad3aa89f0d3f463478d25f62991e82b2", "295dc4efb57f6674ab09df54245cc95a"),
		Site(BossSiteKind.Rift, 2, "IsekaiRiftSiteAct2", "a4e4499ad8eb534eb296c34e6f6b82bd", "20c8cda1ef095088b85d4d4fc3069935", "b732856564ff898419dfc2431a01efce", "3861a9ccea5bdb0438ab76cca6de0754", "50581d70db957d64b90d06cd1cd197e9", "f1ac59dbf579a87479411e0656136481"),
		Site(BossSiteKind.Rift, 3, "IsekaiRiftSiteAct3", "ed26fafc46405b1993db72d37ecd724d", "df7f57d21f0757e4a903ef3f61ae2282", "03fca664fc43fb040aae42bb0f41c23c", "b7b729eb8b3ff074a89d6a10e3ae4b49", "08ef1831c8de3274c88d9e2016ea2997", "fe90614de7418a84c9317a5931e2591d"),
		Site(BossSiteKind.Rift, 4, "IsekaiRiftSiteAct4", "c72e477754df576eb90a91f736b8cf18", "b8b52ab8c50d58feadcbcfd77f51eaad", "08ef1831c8de3274c88d9e2016ea2997", "fe90614de7418a84c9317a5931e2591d", "fa42b091997a6914f89a1b2e2e4b3257", "a5903ee26d5dab54ca295ba44f6a2661"),
		Site(BossSiteKind.Rift, 5, "IsekaiRiftSiteAct5", "8f8a13a2db52570db098a13d2d09236d", "02aa527584575374a2d76a67a4742690", "2a2b91bad89a73b4f9e86b219ed74ce3", "a0548f2e54eae6e4b9e729982514f0f7", "fd10ce9065ac3ce4f9955b21819e1aec", "f674344b94eb6a04a8036be585abfc10"),
		Site(BossSiteKind.Rift, 6, "IsekaiRiftSiteAct6", "0678efac198f5b57a073b2b8a7b56855", "c9617688931a50d384dc1927a9b32f54", "97657db76559e6242b13b7ea922714a9", "94a1a320038a462449dd46d4e3863d42", "80b9a3ae6d4c5f24a9fd5a3968c4003f", "8358e79beca657649a59589ae98f659d"),
		Site(BossSiteKind.Anomaly, 1, "IsekaiAnomalySiteAct1", "b8f999946c1c5083965bd6f6cd185948", "f5e418615da958488e243c7f24442444", "50581d70db957d64b90d06cd1cd197e9", "f1ac59dbf579a87479411e0656136481", "da702e8d79c6046439c17e7e9cbfd758", "ef93f4358d1e1be41a9a396712601a5a"),
		Site(BossSiteKind.Anomaly, 2, "IsekaiAnomalySiteAct2", "bab9943dd3a250ecb09e6580f502e3ce", "a25f97cfb35652078774452e508841d8", "a188ac8c8acc065459ce1ca68b41e861", "a23f83fadb2c2af42a177abc09da6d9a", "ad3aa89f0d3f463478d25f62991e82b2", "295dc4efb57f6674ab09df54245cc95a"),
		Site(BossSiteKind.Anomaly, 3, "IsekaiAnomalySiteAct3", "6a75984b16b05bf78fd0e0ff71d55c2a", "3939847b06b15106b01c8c311e16a74e", "fa42b091997a6914f89a1b2e2e4b3257", "a5903ee26d5dab54ca295ba44f6a2661", "80b9a3ae6d4c5f24a9fd5a3968c4003f", "8358e79beca657649a59589ae98f659d"),
		Site(BossSiteKind.Anomaly, 4, "IsekaiAnomalySiteAct4", "c5923f21e97a5cdf99d53a3ade2a886a", "564a2096d71b5109bd87ca73793f60a6", "fd10ce9065ac3ce4f9955b21819e1aec", "f674344b94eb6a04a8036be585abfc10", "da702e8d79c6046439c17e7e9cbfd758", "ef93f4358d1e1be41a9a396712601a5a"),
		Site(BossSiteKind.Anomaly, 5, "IsekaiAnomalySiteAct5", "367aac36052e525790fa2ef2d42e329b", "5d6efeb893e05d1493b448547083c40c", "851d755cdab9b9a49ae533fd6add5d52", "62d964e3d294d374b8326f740ef7debd", "ad3aa89f0d3f463478d25f62991e82b2", "295dc4efb57f6674ab09df54245cc95a")
	};

	private static readonly Dictionary<string, BlueprintAreaEnterPoint> s_EnterPoints = new Dictionary<string, BlueprintAreaEnterPoint>(StringComparer.OrdinalIgnoreCase);

	internal static SiteDefinition SiteFor(BossSiteKind kind, int act)
	{
		return Sites.FirstOrDefault((SiteDefinition s) => s.Kind == kind && s.Act == act);
	}

	internal static string SiteEnterPointGuidFor(BossSiteKind kind, int act)
	{
		return SiteFor(kind, act)?.EnterPointGuid;
	}

	internal static SiteDefinition SiteForArea(string areaGuid, string areaName)
	{
		SiteDefinition[] sites = Sites;
		foreach (SiteDefinition siteDefinition in sites)
		{
			if (!string.IsNullOrEmpty(areaGuid) && string.Equals(siteDefinition.AreaGuid, areaGuid, StringComparison.OrdinalIgnoreCase))
			{
				return siteDefinition;
			}
			if (!string.IsNullOrEmpty(areaName) && string.Equals(siteDefinition.AreaName, areaName, StringComparison.Ordinal))
			{
				return siteDefinition;
			}
		}
		return null;
	}

	internal static bool IsSiteArea(string areaGuid, string areaName)
	{
		return SiteForArea(areaGuid, areaName) != null;
	}

	internal static bool IsSiteArea(BlueprintArea area)
	{
		if (area != null)
		{
			return IsSiteArea(area.AssetGuid.ToString(), area.name);
		}
		return false;
	}

	internal static SiteDefinition SiteForEnterPointGuid(string enterPointGuid)
	{
		if (string.IsNullOrEmpty(enterPointGuid))
		{
			return null;
		}
		return Sites.FirstOrDefault((SiteDefinition s) => string.Equals(s.EnterPointGuid, enterPointGuid, StringComparison.OrdinalIgnoreCase));
	}

	internal static SiteDefinition CurrentSite()
	{
		BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
		if (blueprintArea != null)
		{
			return SiteForArea(blueprintArea.AssetGuid.ToString(), blueprintArea.name);
		}
		return null;
	}

	internal static int AccessibleRiftAct(int chapter, IList<QuestObjectiveState> actStates)
	{
		int num = DimensionalRiftIncursions.ResolveAct(chapter, actStates);
		if (num == 6 && chapter < 6)
		{
			return 0;
		}
		return num;
	}

	internal static string RiftSiteEnterPointGuid(int chapter, IList<QuestObjectiveState> actStates)
	{
		int num = AccessibleRiftAct(chapter, actStates);
		if (num != 0)
		{
			return SiteEnterPointGuidFor(BossSiteKind.Rift, num);
		}
		return null;
	}

	internal static string ResolveSiteEntry(string enterPointGuid, int chapter, IList<QuestObjectiveState> actStates)
	{
		SiteDefinition siteDefinition = SiteForEnterPointGuid(enterPointGuid);
		if (siteDefinition == null)
		{
			return null;
		}
		if (siteDefinition.Kind == BossSiteKind.Anomaly)
		{
			return siteDefinition.EnterPointGuid;
		}
		return RiftSiteEnterPointGuid(chapter, actStates) ?? siteDefinition.EnterPointGuid;
	}

	internal static AccessPoint AccessPointForChapter(int chapter)
	{
		if (chapter < 1)
		{
			return AccessPoint.None;
		}
		if (chapter == 1)
		{
			return AccessPoint.KenabresMap;
		}
		if (chapter == 4)
		{
			return AccessPoint.AlushinyrraEmissary;
		}
		if (chapter >= 6)
		{
			return AccessPoint.ThresholdEmissary;
		}
		return AccessPoint.WorldWoundMap;
	}

	internal static bool ChapterReachesSitesThroughEmissary(int chapter)
	{
		AccessPoint accessPoint = AccessPointForChapter(chapter);
		if (accessPoint != AccessPoint.AlushinyrraEmissary)
		{
			return accessPoint == AccessPoint.ThresholdEmissary;
		}
		return true;
	}

	internal static bool RiftOffered(QuestState questState, int chapter, IList<QuestObjectiveState> actStates)
	{
		if (AccessPointForChapter(chapter) == AccessPoint.None)
		{
			return false;
		}
		if (questState == QuestState.Started)
		{
			return AccessibleRiftAct(chapter, actStates) != 0;
		}
		return false;
	}

	internal static bool AnomalyOffered(int act, int chapter, bool ownsEncounters)
	{
		if (!ownsEncounters || act < 1 || act > 5 || chapter < 1)
		{
			return false;
		}
		return chapter >= CosmicBarrenBosses.MinimumChapterForBoss(CosmicBarrenBosses.BossTypeForAct(act));
	}

	internal static int CurrentAccessibleRiftAct()
	{
		return AccessibleRiftAct((Game.Instance?.Player?.Chapter).GetValueOrDefault(), IsekaiTransmigrationQuest.ActStates());
	}

	internal static bool CurrentRiftOffered()
	{
		int valueOrDefault = (Game.Instance?.Player?.Chapter).GetValueOrDefault();
		return RiftOffered(IsekaiTransmigrationQuest.QuestStateInSave(), valueOrDefault, IsekaiTransmigrationQuest.ActStates());
	}

	internal static bool CurrentAnomalyOffered(int act)
	{
		int valueOrDefault = (Game.Instance?.Player?.Chapter).GetValueOrDefault();
		return AnomalyOffered(act, valueOrDefault, IsekaiTransmigrationQuest.PlayerCanStartTransmigration());
	}

	public static void Add()
	{
		AddSite("IsekaiRiftSiteAct1", Helpers.CreateString(Main.IsekaiContext, "IsekaiRiftSiteAct1Area.Name", "Dimensional Breach: The Kenabres Rift"));
		AddSite("IsekaiRiftSiteAct2", Helpers.CreateString(Main.IsekaiContext, "IsekaiRiftSiteAct2Area.Name", "Dimensional Breach: The Ronin's Ruins"));
		AddSite("IsekaiRiftSiteAct3", Helpers.CreateString(Main.IsekaiContext, "IsekaiRiftSiteAct3Area.Name", "Dimensional Breach: The Shadow Nexus"));
		AddSite("IsekaiRiftSiteAct4", Helpers.CreateString(Main.IsekaiContext, "IsekaiRiftSiteAct4Area.Name", "Dimensional Breach: The Abyssal Wound"));
		AddSite("IsekaiRiftSiteAct5", Helpers.CreateString(Main.IsekaiContext, "IsekaiRiftSiteAct5Area.Name", "Dimensional Breach: The Closed Threshold"));
		AddSite("IsekaiRiftSiteAct6", Helpers.CreateString(Main.IsekaiContext, "IsekaiRiftSiteAct6Area.Name", "Dimensional Breach: Where the Loop Began"));
		AddSite("IsekaiAnomalySiteAct1", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalySiteAct1Area.Name", "Barren Anomaly: The Frozen Wood"));
		AddSite("IsekaiAnomalySiteAct2", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalySiteAct2Area.Name", "Barren Anomaly: The Ashen Hills"));
		AddSite("IsekaiAnomalySiteAct3", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalySiteAct3Area.Name", "Barren Anomaly: The Behemoth's Cliffs"));
		AddSite("IsekaiAnomalySiteAct4", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalySiteAct4Area.Name", "Barren Anomaly: The Void-Warped Waste"));
		AddSite("IsekaiAnomalySiteAct5", Helpers.CreateString(Main.IsekaiContext, "IsekaiAnomalySiteAct5Area.Name", "Barren Anomaly: The Arbiter's Street"));
	}

	private static void AddSite(string stem, LocalizedString displayName)
	{
		SiteDefinition siteDefinition = Sites.FirstOrDefault((SiteDefinition s) => s.Stem == stem);
		if (siteDefinition == null)
		{
			return;
		}
		try
		{
			BlueprintAreaEnterPoint blueprintAreaEnterPoint = PlanarColiseumArea.CreateArena(stem, siteDefinition.HostAreaGuid, siteDefinition.HostEnterPointGuid, displayName, out var area);
			if (blueprintAreaEnterPoint == null)
			{
				Main.IsekaiContext.Logger.LogError("[BossSites] " + stem + ": its host " + siteDefinition.HostAreaGuid + " cannot be used; trying " + siteDefinition.FallbackHostAreaGuid + ".");
				blueprintAreaEnterPoint = PlanarColiseumArea.CreateArena(stem, siteDefinition.FallbackHostAreaGuid, siteDefinition.FallbackHostEnterPointGuid, displayName, out area);
			}
			if (blueprintAreaEnterPoint != null)
			{
				s_EnterPoints[siteDefinition.EnterPointGuid] = blueprintAreaEnterPoint;
			}
			else
			{
				Main.IsekaiContext.Logger.LogError("[BossSites] " + siteDefinition.Stem + ": no host could be used; the emissary will call this fight where the party stands.");
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[BossSites] The site {siteDefinition.Stem} failed: {arg}");
		}
	}

	internal static BlueprintAreaEnterPoint SiteEnterPointFor(BossSiteKind kind, int act)
	{
		string text = SiteEnterPointGuidFor(kind, act);
		if (text == null || !s_EnterPoints.TryGetValue(text, out var value))
		{
			return null;
		}
		return value;
	}

	internal static BlueprintAreaEnterPoint CreatedEnterPoint(string enterPointGuid)
	{
		if (string.IsNullOrEmpty(enterPointGuid) || !s_EnterPoints.TryGetValue(enterPointGuid, out var value))
		{
			return null;
		}
		return value;
	}

	internal static BlueprintAreaEnterPoint AnyRiftSiteEnterPoint()
	{
		return (from s in Sites
			where s.Kind == BossSiteKind.Rift
			select SiteEnterPointFor(s.Kind, s.Act)).FirstOrDefault((BlueprintAreaEnterPoint e) => e != null);
	}

	internal static BlueprintAreaEnterPoint CurrentRiftSiteEnterPoint()
	{
		int num = CurrentAccessibleRiftAct();
		if (num != 0)
		{
			return SiteEnterPointFor(BossSiteKind.Rift, num);
		}
		return null;
	}

	private static SiteDefinition Site(BossSiteKind kind, int act, string stem, string areaGuid, string enterPointGuid, string host, string hostEnter, string fallback, string fallbackEnter)
	{
		return new SiteDefinition
		{
			Kind = kind,
			Act = act,
			Stem = stem,
			AreaGuid = areaGuid,
			EnterPointGuid = enterPointGuid,
			HostAreaGuid = host,
			HostEnterPointGuid = hostEnter,
			FallbackHostAreaGuid = fallback,
			FallbackHostEnterPointGuid = fallbackEnter
		};
	}
}
