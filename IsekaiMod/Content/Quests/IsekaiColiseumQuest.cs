using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Config;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.GlobalMap;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Enums;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Quests;

public static class IsekaiColiseumQuest
{
	internal enum ActUpdate
	{
		Nothing,
		Give,
		Complete
	}

	private sealed class ColiseumQuestHandler : IAreaActivationHandler, ISubscriber, IGlobalSubscriber, IChapterChangeHandler
	{
		public void OnAreaActivated()
		{
			Refresh();
		}

		public void HandleChapterChanged()
		{
			Refresh();
		}
	}

	public static BlueprintQuest ColiseumQuest;

	public static BlueprintQuestObjective ObjectiveAct1FaeGrove;

	public static BlueprintQuestObjective ObjectiveAct2LostLegions;

	public static BlueprintQuestObjective ObjectiveAct3MidnightColosseum;

	public static BlueprintQuestObjective ObjectiveAct4VoidRift;

	public static BlueprintQuestObjective ObjectiveAct5MultiverseThreshold;

	public static BlueprintQuestObjective ObjectiveChampion;

	private static bool s_Subscribed;

	public static void Add()
	{
		ColiseumQuest = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiColiseumQuest", delegate(BlueprintQuest bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "IsekaiColiseumQuest.Title", "The Planar Coliseum: Champion Beyond Worlds");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "IsekaiColiseumQuest.Description", "A coliseum anchored outside linear time holds a stage for every chapter of the crusade, and the constellations watch every bout. Its rifts open beside Defender's Heart and Drezen on the world map, in the Defender's Heart tavern and the crusader war camp, and wherever the Otherworldly Emissary is waiting to open one.\nWin each arena's cup to be named the coliseum's champion.");
			bp.CompletionText = Helpers.CreateString(Main.IsekaiContext, "IsekaiColiseumQuest.CompletionText", "Every arena of the Planar Coliseum has crowned you. The constellations will be talking about this for an age.");
			bp.m_Group = QuestGroupId.MythicQuests;
			bp.m_Type = QuestType.Normal;
			bp.m_LastChapter = 0;
		});
		ObjectiveAct1FaeGrove = CreateActObjective("ColiseumObjective_Act1FaeGrove", "e661b603477a5b84bde2de2f53f076a8", PlanarColiseumMapPoint.KenabresPoint, "Act 1: The Fae Grove", "Win the Fae Grove Cup in the Planar Coliseum's first arena. Its rift waits on the Kenabres map beside Defender's Heart, and in the Defender's Heart tavern.");
		ObjectiveAct2LostLegions = CreateActObjective("ColiseumObjective_Act2LostLegions", "07f731a4043b52f097245309e00fe2a1", PlanarColiseumMapPoint.WorldWoundPoint, "Act 2: The Lost Legions", "Win the Lost Legions Cup in the arena of forgotten crusades. Its rift waits on the Worldwound map and at the crusader war camp.");
		ObjectiveAct3MidnightColosseum = CreateActObjective("ColiseumObjective_Act3MidnightColosseum", "05f68f97b98e5988b8e079033bc5752b", PlanarColiseumMapPoint.WorldWoundPoint, "Act 3: The Midnight Colosseum", "Win the Midnight Colosseum Cup, where demon gladiators defend the coliseum's heart. Its rift waits on the Worldwound map beside Drezen.");
		ObjectiveAct4VoidRift = CreateActObjective("ColiseumObjective_Act4VoidRift", "9195dd67d5d450248f6f808a989bec9c", null, "Act 4: The Void Rift", "Win the Void Rift Cup at the edge of nothing. No road leads there from the Midnight Isles: the Otherworldly Emissary carries the rift, and waits in Alushinyrra's lower, middle and upper city. The Act 5 arena replays its cup.");
		ObjectiveAct5MultiverseThreshold = CreateActObjective("ColiseumObjective_Act5MultiverseThreshold", "4240fda390e055ceb515442bb9f59351", PlanarColiseumMapPoint.WorldWoundPoint, "Act 5: The Multiverse Threshold", "Win the Multiverse Threshold Cup, where every timeline's coliseum touches. Its rift waits on the Worldwound map beside Drezen.");
		ObjectiveChampion = Helpers.CreateBlueprint(Main.IsekaiContext, "ColiseumObjective_Champion", delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "ColiseumObjective_Champion.Title", "Champion of the Planar Coliseum");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "ColiseumObjective_Champion.Description", "Every arena's cup is yours.");
			bp.m_Quest = ColiseumQuest.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = true;
			bp.m_Hidden = true;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
		});
		ColiseumQuest.m_Objectives = new List<BlueprintQuestObjectiveReference>
		{
			ObjectiveAct1FaeGrove.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveAct2LostLegions.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveAct3MidnightColosseum.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveAct4VoidRift.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveAct5MultiverseThreshold.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveChampion.ToReference<BlueprintQuestObjectiveReference>()
		};
		if (!s_Subscribed)
		{
			s_Subscribed = true;
			EventBus.Subscribe(new ColiseumQuestHandler());
		}
	}

	private static BlueprintQuestObjective CreateActObjective(string name, string arenaAreaGuid, BlueprintGlobalMapPoint tile, string title, string description)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
			bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			bp.m_Quest = ColiseumQuest.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = false;
			bp.m_Areas = new List<BlueprintAreaReference> { BlueprintTools.GetBlueprintReference<BlueprintAreaReference>(arenaAreaGuid) };
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			if (tile != null)
			{
				bp.Locations.Add(tile.ToReference<BlueprintGlobalMapPoint.Reference>());
			}
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
		});
	}

	public static BlueprintQuestObjective ObjectiveForAct(int act)
	{
		return act switch
		{
			1 => ObjectiveAct1FaeGrove, 
			2 => ObjectiveAct2LostLegions, 
			3 => ObjectiveAct3MidnightColosseum, 
			4 => ObjectiveAct4VoidRift, 
			5 => ObjectiveAct5MultiverseThreshold, 
			_ => null, 
		};
	}

	internal static int HighestActGiven(int chapter)
	{
		if (chapter < 1)
		{
			return 0;
		}
		return Math.Min(chapter, 5);
	}

	internal static ActUpdate DecideAct(int act, int chapter, QuestObjectiveState state, bool cupClearedInSave)
	{
		if (state == QuestObjectiveState.Completed || state == QuestObjectiveState.Failed)
		{
			return ActUpdate.Nothing;
		}
		if (cupClearedInSave)
		{
			return ActUpdate.Complete;
		}
		if (state == QuestObjectiveState.None && act <= HighestActGiven(chapter))
		{
			return ActUpdate.Give;
		}
		return ActUpdate.Nothing;
	}

	internal static bool ShouldCrownChampion(IList<QuestObjectiveState> actStates, QuestObjectiveState championState)
	{
		if (actStates == null || actStates.Count < 5)
		{
			return false;
		}
		if (championState == QuestObjectiveState.Completed || championState == QuestObjectiveState.Failed)
		{
			return false;
		}
		return actStates.Take(5).All((QuestObjectiveState s) => s == QuestObjectiveState.Completed);
	}

	internal static List<string> MissingArenaRequirements(bool arenaEnabled, IList<QuestObjectiveState> actStates)
	{
		List<string> list = new List<string>();
		if (!arenaEnabled)
		{
			return list;
		}
		for (int i = 1; i <= 5; i++)
		{
			if (((actStates != null && actStates.Count >= i) ? actStates[i - 1] : QuestObjectiveState.None) != QuestObjectiveState.Completed)
			{
				list.Add(ConstellationText.LocalizedFormat("Constellation.Transmigration", "Planar Coliseum, Act {0}: {1}", i, ConstellationText.LocalizeIfKnown(ColiseumCups.ForAct(i)?.DisplayName)));
			}
		}
		return list;
	}

	public static QuestObjectiveState[] ActStates()
	{
		QuestObjectiveState[] array = new QuestObjectiveState[5];
		QuestBook questBook = Game.Instance?.Player?.QuestBook;
		if (questBook == null)
		{
			return array;
		}
		for (int i = 1; i <= 5; i++)
		{
			BlueprintQuestObjective blueprintQuestObjective = ObjectiveForAct(i);
			array[i - 1] = ((blueprintQuestObjective != null) ? questBook.GetObjectiveState(blueprintQuestObjective) : QuestObjectiveState.None);
		}
		return array;
	}

	public static List<string> MissingArenaRequirements()
	{
		return MissingArenaRequirements(Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true, ActStates());
	}

	public static void OnCupCleared(string cupId)
	{
		Refresh();
	}

	public static void Refresh()
	{
		try
		{
			Player player = Game.Instance?.Player;
			QuestBook questBook = player?.QuestBook;
			if (questBook == null || ColiseumQuest == null)
			{
				return;
			}
			AddedContent addedContent = Main.IsekaiContext.AddedContent;
			if (addedContent != null && !addedContent.EnableDimensionalArena)
			{
				return;
			}
			QuestState questState = questBook.GetQuestState(ColiseumQuest);
			if (questState == QuestState.Completed || questState == QuestState.Failed)
			{
				return;
			}
			Dictionary<string, string> storage = ColiseumProgress.Find();
			bool flag = questState != QuestState.None;
			for (int i = 1; i <= 5; i++)
			{
				BlueprintQuestObjective blueprintQuestObjective = ObjectiveForAct(i);
				if (blueprintQuestObjective == null)
				{
					continue;
				}
				ColiseumCup coliseumCup = ColiseumCups.ForAct(i);
				switch (DecideAct(i, player.Chapter, questBook.GetObjectiveState(blueprintQuestObjective), ColiseumProgress.IsCleared(storage, coliseumCup?.Id)))
				{
				case ActUpdate.Give:
					questBook.GiveObjective(blueprintQuestObjective);
					if (!flag)
					{
						flag = true;
						PostLog(ConstellationText.Localized("Constellation.ColiseumQuest", "<color=#5B2A86><b>[New Quest]</b></color> <b>The Planar Coliseum: Champion Beyond Worlds</b> has begun!"));
					}
					break;
				case ActUpdate.Complete:
					questBook.CompleteObjective(blueprintQuestObjective);
					flag = true;
					PostLog(ConstellationText.LocalizedFormat("Constellation.ColiseumQuest", "<color=#004604><b>[Quest Updated]</b></color> The Planar Coliseum: <b>{0}</b> won.", coliseumCup?.DisplayName));
					break;
				}
			}
			if (ObjectiveChampion != null && ShouldCrownChampion(ActStates(), questBook.GetObjectiveState(ObjectiveChampion)))
			{
				questBook.GiveObjective(ObjectiveChampion);
				questBook.CompleteObjective(ObjectiveChampion);
				PostLog(ConstellationText.Localized("Constellation.ColiseumQuest", "<color=#5B2A86><b>[Quest Completed]</b></color> <b>The Planar Coliseum: Champion Beyond Worlds</b>! Every arena's cup is yours."));
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[IsekaiColiseumQuest] Could not update the coliseum quest: " + ex);
		}
	}

	private static void PostLog(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, message, null, ConstellationCategory.Quest, "Planar Coliseum");
	}
}
