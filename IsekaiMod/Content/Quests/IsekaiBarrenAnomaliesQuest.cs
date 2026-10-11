using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.GlobalMap;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Enums;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Quests;

public static class IsekaiBarrenAnomaliesQuest
{
	internal enum ObjectiveUpdate
	{
		Nothing,
		Give,
		Complete
	}

	private sealed class AnomaliesQuestHandler : IAreaActivationHandler, ISubscriber, IGlobalSubscriber, IChapterChangeHandler
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

	public static BlueprintQuest AnomaliesQuest;

	private static readonly BlueprintQuestObjective[] s_Objectives = new BlueprintQuestObjective[5];

	public static BlueprintQuestObjective ObjectiveAllSealed;

	private static bool s_Subscribed;

	public static void Add()
	{
		AnomaliesQuest = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiBarrenAnomaliesQuest", delegate(BlueprintQuest bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "IsekaiBarrenAnomaliesQuest.Title", "Barren Anomalies: Strays Between Worlds");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "IsekaiBarrenAnomaliesQuest.Description", "The same fissures that carried you across the planes drag other things through. Each chapter of the crusade, one more cosmic anomaly settles somewhere it does not belong. The Otherworldly Emissary opens the way to every anomaly in this journal from each chapter's town: Defender's Heart, the crusade's war camp, Drezen, Alushinyrra's cities and the Threshold. On the Kenabres city map and the Worldwound map, Barren Anomaly tiles lead there too.\nEach pays its Cosmic Coin bounty the first time it falls. It can be fought again, for a tenth of the experience and no coins.");
			bp.CompletionText = Helpers.CreateString(Main.IsekaiContext, "IsekaiBarrenAnomaliesQuest.CompletionText", "All five anomalies have fallen. Their fissures stay open, should you want a rematch.");
			bp.m_Group = QuestGroupId.None;
			bp.m_Type = QuestType.Normal;
			bp.m_LastChapter = 0;
		});
		s_Objectives[0] = CreateObjective("IsekaiBarrenAnomalyObjective_Act1", 1, "Act 1: The Outer World Stray", "A satyr archer has strayed out of the First World into a frozen wood. In Kenabres, the Otherworldly Emissary in Defender's Heart opens the way, and a Barren Anomaly tile waits beside Defender's Heart on the Kenabres city map. Later in the crusade, the emissary in each chapter's town still opens it, and its tile waits beside Kenabres on the Worldwound map.");
		s_Objectives[1] = CreateObjective("IsekaiBarrenAnomalyObjective_Act2", 2, "Act 2: Chronos Exile", "An ash giant cast adrift in time walks the ashen hills. The Otherworldly Emissary at the crusade's war camp opens the way, and its Barren Anomaly tile waits beside Kenabres on the Worldwound map.");
		s_Objectives[2] = CreateObjective("IsekaiBarrenAnomalyObjective_Act3", 3, "Act 3: The Transdimensional Behemoth", "A glabrezu dragged through a planar fissure haunts the snowbound cliffs. The Otherworldly Emissary in Drezen opens the way, and its Barren Anomaly tile waits beside Drezen on the Worldwound map.");
		s_Objectives[3] = CreateObjective("IsekaiBarrenAnomalyObjective_Act4", 4, "Act 4: Abyssal Void Vavakia", "A vavakia warped by the Abyss has found its way to demon-blasted Sarkoris. In Alushinyrra's cities the Otherworldly Emissary opens the way; back on Golarion the emissary in Drezen does too, and its Barren Anomaly tile waits beside Drezen on the Worldwound map.");
		s_Objectives[4] = CreateObjective("IsekaiBarrenAnomalyObjective_Act5", 5, "Act 5: Cosmic Arbiter of Ruin", "A balor sent to judge your transmigrated soul waits in an echo of burning Kenabres. The Otherworldly Emissary in Drezen opens the way, and at the Threshold he does too; its Barren Anomaly tile waits beside Drezen on the Worldwound map.");
		ObjectiveAllSealed = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiBarrenAnomalyObjective_AllSealed", delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "IsekaiBarrenAnomalyObjective_AllSealed.Title", "Every Anomaly Has Fallen");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "IsekaiBarrenAnomalyObjective_AllSealed.Description", "All five Barren anomalies are defeated.");
			bp.m_Quest = AnomaliesQuest.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = true;
			bp.m_Hidden = true;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
		});
		AnomaliesQuest.m_Objectives = (from o in s_Objectives.Where((BlueprintQuestObjective o) => o != null).Concat(new BlueprintQuestObjective[1] { ObjectiveAllSealed })
			select o.ToReference<BlueprintQuestObjectiveReference>()).ToList();
		if (!s_Subscribed)
		{
			s_Subscribed = true;
			EventBus.Subscribe(new AnomaliesQuestHandler());
		}
	}

	private static BlueprintQuestObjective CreateObjective(string name, int act, string title, string description)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
			bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			bp.m_Quest = AnomaliesQuest.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = false;
			bp.Locations = (from t in BossSiteMapPoints.AnomalyTiles(act)
				select t.ToReference<BlueprintGlobalMapPoint.Reference>()).ToList();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
		});
	}

	public static BlueprintQuestObjective ObjectiveForAct(int act)
	{
		if (act < 1 || act > 5)
		{
			return null;
		}
		return s_Objectives[act - 1];
	}

	internal static ObjectiveUpdate DecideObjective(int act, int chapter, bool ownsEncounters, QuestObjectiveState state, bool defeatedInSave)
	{
		if (state == QuestObjectiveState.Completed || state == QuestObjectiveState.Failed)
		{
			return ObjectiveUpdate.Nothing;
		}
		if (defeatedInSave)
		{
			return ObjectiveUpdate.Complete;
		}
		if (state == QuestObjectiveState.None && BossSites.AnomalyOffered(act, chapter, ownsEncounters))
		{
			return ObjectiveUpdate.Give;
		}
		return ObjectiveUpdate.Nothing;
	}

	internal static bool ShouldClose(IList<QuestObjectiveState> states, QuestObjectiveState closerState)
	{
		if (states == null || states.Count < 5)
		{
			return false;
		}
		if (closerState == QuestObjectiveState.Completed || closerState == QuestObjectiveState.Failed)
		{
			return false;
		}
		return states.Take(5).All((QuestObjectiveState s) => s == QuestObjectiveState.Completed);
	}

	public static void Refresh()
	{
		try
		{
			Player player = Game.Instance?.Player;
			QuestBook questBook = player?.QuestBook;
			if (questBook == null || AnomaliesQuest == null)
			{
				return;
			}
			QuestState questState = questBook.GetQuestState(AnomaliesQuest);
			if (questState == QuestState.Completed || questState == QuestState.Failed)
			{
				return;
			}
			bool ownsEncounters = IsekaiTransmigrationQuest.PlayerCanStartTransmigration();
			Dictionary<string, string> storage = SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.BarrenAnomalies");
			bool flag = questState != QuestState.None;
			QuestObjectiveState[] array = new QuestObjectiveState[5];
			for (int i = 1; i <= 5; i++)
			{
				BlueprintQuestObjective blueprintQuestObjective = ObjectiveForAct(i);
				if (blueprintQuestObjective == null)
				{
					continue;
				}
				bool flag2 = SaveScopedStorage.GetFlag(storage, CosmicBarrenBosses.DefeatedKey(CosmicBarrenBosses.BossTypeForAct(i)));
				switch (DecideObjective(i, player.Chapter, ownsEncounters, questBook.GetObjectiveState(blueprintQuestObjective), flag2))
				{
				case ObjectiveUpdate.Give:
					questBook.GiveObjective(blueprintQuestObjective);
					if (!flag)
					{
						flag = true;
						PostLog(ConstellationText.Localized("Constellation.AnomaliesQuest", "<color=#5B2A86><b>[New Quest]</b></color> <b>Barren Anomalies: Strays Between Worlds</b> has begun!"));
					}
					break;
				case ObjectiveUpdate.Complete:
					questBook.CompleteObjective(blueprintQuestObjective);
					flag = true;
					PostLog(ConstellationText.LocalizedFormat("Constellation.AnomaliesQuest", "<color=#004604><b>[Quest Updated]</b></color> Barren Anomalies: <b>{0}</b> defeated.", blueprintQuestObjective.Title));
					break;
				}
				array[i - 1] = questBook.GetObjectiveState(blueprintQuestObjective);
			}
			if (ObjectiveAllSealed != null && ShouldClose(array, questBook.GetObjectiveState(ObjectiveAllSealed)))
			{
				questBook.GiveObjective(ObjectiveAllSealed);
				questBook.CompleteObjective(ObjectiveAllSealed);
				PostLog(ConstellationText.Localized("Constellation.AnomaliesQuest", "<color=#5B2A86><b>[Quest Completed]</b></color> <b>Barren Anomalies: Strays Between Worlds</b>! Every anomaly has fallen."));
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[IsekaiBarrenAnomaliesQuest] Could not update the anomalies quest: " + ex);
		}
	}

	private static void PostLog(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, message, null, ConstellationCategory.Quest, "Barren Anomalies");
	}
}
