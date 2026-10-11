using System.Collections.Generic;
using IsekaiMod.Content.GlobalMap;
using Kingmaker.AreaLogic.QuestSystem;

namespace IsekaiMod.Content.Arenas;

internal static class BossSiteDelivery
{
	internal static string[] ChapterTowns(int chapter)
	{
		switch (chapter)
		{
		case 1:
			return new string[1] { "DefendersHeart" };
		case 2:
			return new string[1] { "WarCamp" };
		case 3:
		case 5:
			return new string[1] { "DrezenCapital" };
		case 4:
			return (string[])ActionEnterColiseum.AlushinyrraAreaNames.Clone();
		case 6:
			return new string[1] { "ThresholdOutdoor" };
		default:
			return new string[0];
		}
	}

	internal static BossSiteMapPoints.MapId? ChapterMap(int chapter)
	{
		return BossSites.AccessPointForChapter(chapter) switch
		{
			BossSites.AccessPoint.KenabresMap => BossSiteMapPoints.MapId.Kenabres, 
			BossSites.AccessPoint.WorldWoundMap => BossSiteMapPoints.MapId.WorldWound, 
			_ => null, 
		};
	}

	internal static QuestObjectiveState[] StatesWithActOpen(int act)
	{
		QuestObjectiveState[] array = new QuestObjectiveState[6];
		for (int i = 0; i < 6; i++)
		{
			array[i] = ((i < act - 1) ? QuestObjectiveState.Completed : ((i == act - 1) ? QuestObjectiveState.Started : QuestObjectiveState.None));
		}
		return array;
	}

	internal static bool IsOpen(BossSiteKind kind, int act, int chapter)
	{
		if (kind == BossSiteKind.Anomaly)
		{
			return BossSites.AnomalyOffered(act, chapter, ownsEncounters: true);
		}
		if (BossSites.RiftOffered(QuestState.Started, chapter, StatesWithActOpen(act)))
		{
			return BossSites.AccessibleRiftAct(chapter, StatesWithActOpen(act)) == act;
		}
		return false;
	}

	internal static List<Delivery> For(BossSiteKind kind, int act, int chapter)
	{
		List<Delivery> list = new List<Delivery>();
		if (!IsOpen(kind, act, chapter))
		{
			return list;
		}
		BossSiteMapPoints.MapId? mapId = ChapterMap(chapter);
		if (mapId.HasValue)
		{
			bool num = ((kind == BossSiteKind.Anomaly) ? BossSiteMapPoints.AnomalyTileShown(mapId.Value, act, chapter, ownsEncounters: true) : BossSiteMapPoints.BreachShown(mapId.Value, QuestState.Started, chapter, StatesWithActOpen(act)));
			string text = BossSiteMapPoints.AnchorFor(kind, act, mapId.Value);
			if (num && text != null)
			{
				list.Add(new Delivery
				{
					Route = DeliveryRoute.MapTile,
					Map = mapId.Value,
					AnchorGuid = text
				});
			}
		}
		string[] array = ChapterTowns(chapter);
		foreach (string text2 in array)
		{
			if ((kind == BossSiteKind.Anomaly) ? PlanarGatewayEmissary.OffersAnomaly(text2, chapter, anomalyOffered: true, siteBuilt: true) : PlanarGatewayEmissary.OffersRift(text2, chapter, riftOffered: true))
			{
				list.Add(new Delivery
				{
					Route = DeliveryRoute.Emissary,
					TownAreaName = text2
				});
			}
		}
		return list;
	}
}
