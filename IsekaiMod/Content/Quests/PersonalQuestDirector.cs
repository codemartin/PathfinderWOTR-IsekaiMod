using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Constellations;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Quests;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.Settings;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Quests;

public static class PersonalQuestDirector
{
	private sealed class PersonalQuestHandler : IAreaActivationHandler, ISubscriber, IGlobalSubscriber, IChapterChangeHandler, IQuestObjectiveHandler
	{
		public void OnAreaActivated()
		{
			Reconcile();
		}

		public void HandleChapterChanged()
		{
			Reconcile();
		}

		public void HandleQuestObjectiveStarted(QuestObjective objective)
		{
			Reconcile();
		}

		public void HandleQuestObjectiveCompleted(QuestObjective objective)
		{
			Reconcile();
		}

		public void HandleQuestObjectiveFailed(QuestObjective objective)
		{
			Reconcile();
		}

		public void HandleQuestObjectiveBecameVisible(QuestObjective objective)
		{
		}
	}

	private static bool s_Subscribed;

	private static bool s_Evaluating;

	public static void Init()
	{
		if (!s_Subscribed)
		{
			s_Subscribed = true;
			EventBus.Subscribe(new PersonalQuestHandler());
		}
	}

	private static UnitEntityData MainCharacter()
	{
		return Game.Instance?.Player?.MainCharacter.Value;
	}

	private static QuestBook Book()
	{
		return Game.Instance?.Player?.QuestBook;
	}

	private static int Chapter()
	{
		return Game.Instance?.Player?.Chapter ?? 1;
	}

	internal static string ActiveArchetypeToken()
	{
		return SubclassRespecManager.DetectArchetypeOf(MainCharacter());
	}

	internal static bool IsEligible()
	{
		UnitEntityData unitEntityData = MainCharacter();
		if (unitEntityData?.Descriptor == null)
		{
			return false;
		}
		BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
		int isekaiClassLevel = ((reference != null) ? (unitEntityData.Progression?.GetClassLevel(reference) ?? 0) : 0);
		BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
		bool hasIsekaiFact = modBlueprint != null && unitEntityData.Descriptor.HasFact(modBlueprint);
		return PersonalQuestRules.IsEligible(isMainCharacter: true, isekaiClassLevel, hasIsekaiFact);
	}

	private static QuestObjectiveState StateOf(string objectiveName)
	{
		QuestBook questBook = Book();
		BlueprintQuestObjective blueprintQuestObjective = SubclassPersonalQuests.ObjectiveByName(objectiveName);
		if (questBook != null && blueprintQuestObjective != null)
		{
			return questBook.GetObjectiveState(blueprintQuestObjective);
		}
		return QuestObjectiveState.None;
	}

	private static QuestState QuestStateOf(PersonalQuestDefinition quest)
	{
		QuestBook questBook = Book();
		BlueprintQuest blueprintQuest = SubclassPersonalQuests.QuestByName(quest?.QuestName);
		if (questBook != null && blueprintQuest != null)
		{
			return questBook.GetQuestState(blueprintQuest);
		}
		return QuestState.None;
	}

	private static QuestObjectiveState VanillaState(string objectiveGuid)
	{
		try
		{
			QuestBook questBook = Book();
			BlueprintQuestObjective blueprint = BlueprintTools.GetBlueprint<BlueprintQuestObjective>(objectiveGuid);
			return (questBook != null && blueprint != null) ? questBook.GetObjectiveState(blueprint) : QuestObjectiveState.None;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not read vanilla objective " + objectiveGuid + ": " + ex);
			return QuestObjectiveState.None;
		}
	}

	private static bool AreaVisited(string areaGuid)
	{
		try
		{
			BlueprintGuid guid = BlueprintGuid.Parse(areaGuid);
			BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
			if (blueprintArea != null && blueprintArea.AssetGuid == guid)
			{
				return true;
			}
			return (Game.Instance?.Player?.VisitedAreas)?.Any((BlueprintArea a) => a != null && a.AssetGuid == guid) ?? false;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not read the visited areas: " + ex);
			return false;
		}
	}

	private static bool AutoCrusadeOn()
	{
		try
		{
			return SettingsRoot.Difficulty.AutoCrusade.GetValue();
		}
		catch
		{
			return false;
		}
	}

	private static bool TriggerMet(PersonalQuestTrigger trigger, IEnumerable<string> ids, IDictionary<string, string> record)
	{
		if (ids == null)
		{
			return false;
		}
		return trigger switch
		{
			PersonalQuestTrigger.VanillaObjectiveCompleted => ids.Any((string id) => VanillaState(id) == QuestObjectiveState.Completed), 
			PersonalQuestTrigger.VanillaObjectiveFinished => ids.Any(delegate(string id)
			{
				QuestObjectiveState questObjectiveState = VanillaState(id);
				return questObjectiveState == QuestObjectiveState.Completed || questObjectiveState == QuestObjectiveState.Failed;
			}), 
			PersonalQuestTrigger.VanillaObjectiveStarted => ids.Any((string id) => VanillaState(id) != QuestObjectiveState.None), 
			PersonalQuestTrigger.AreaVisited => ids.Any(AreaVisited), 
			PersonalQuestTrigger.CupCleared => ids.Any(ColiseumProgress.IsClearedInSave), 
			PersonalQuestTrigger.Decree => ids.Any((string id) => PersonalQuestRecord.DecreeDone(record, id)), 
			PersonalQuestTrigger.DevourerDivergence => PersonalQuestRecord.DevourerPath(record) != null, 
			_ => false, 
		};
	}

	private static bool ArenaEnabled()
	{
		return Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true;
	}

	private static bool ActSatisfied(PersonalQuestAct act, IDictionary<string, string> record, int chapter)
	{
		bool mainTriggerMet = ((act.Trigger == PersonalQuestTrigger.Trial) ? PersonalQuestRecord.TrialWon(record, act.TrialId) : TriggerMet(act.Trigger, act.TriggerIds, record));
		bool fallbackMet = act.Fallback != PersonalQuestTrigger.None && (!act.FallbackRequiresAutoCrusade || AutoCrusadeOn()) && TriggerMet(act.Fallback, act.FallbackIds, record);
		bool chapterPassed = PersonalQuestRules.ChapterPassed(act.Chapter, chapter);
		return PersonalQuestRules.ActIsSatisfied(PersonalQuestRules.CatchesUpByChapter(act.CatchUpByChapter, ArenaEnabled()), mainTriggerMet, fallbackMet, chapterPassed);
	}

	internal static string ObjectiveNameFor(PersonalQuestDefinition quest, PersonalQuestAct act, IDictionary<string, string> record)
	{
		if (quest != null && act != null && quest.Id == "Devourer" && act.Act == 3)
		{
			return PersonalQuestRules.DevourerAct3ObjectiveName(PersonalQuestRecord.DevourerPath(record));
		}
		return act?.ObjectiveName;
	}

	public static void Reconcile()
	{
		if (s_Evaluating)
		{
			return;
		}
		try
		{
			s_Evaluating = true;
			if (Book() == null || !IsEligible())
			{
				return;
			}
			PersonalQuestDefinition personalQuestDefinition = PersonalQuestCatalog.ForArchetypeToken(ActiveArchetypeToken());
			foreach (PersonalQuestDefinition archetypeQuest in PersonalQuestCatalog.ArchetypeQuests)
			{
				ApplyDormancy(archetypeQuest, archetypeQuest == personalQuestDefinition);
			}
			Evaluate(PersonalQuestCatalog.Nostalgia);
			if (personalQuestDefinition != null)
			{
				Evaluate(personalQuestDefinition);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Reconcile failed: " + ex);
		}
		finally
		{
			s_Evaluating = false;
		}
	}

	private static void ApplyDormancy(PersonalQuestDefinition quest, bool isActive)
	{
		BlueprintQuestObjective blueprintQuestObjective = SubclassPersonalQuests.ObjectiveByName(quest.DormantObjectiveName);
		if (blueprintQuestObjective == null)
		{
			return;
		}
		QuestBook questBook = Book();
		if (questBook != null)
		{
			switch (PersonalQuestRules.DecideDormancy(isActive, QuestStateOf(quest), questBook.GetObjectiveState(blueprintQuestObjective)))
			{
			case PersonalQuestRules.DormancyAction.Sleep:
				questBook.GiveObjective(blueprintQuestObjective);
				Quiet(ConstellationText.LocalizedFormat("Constellation.PersonalQuest", "<color=#3F3F3F><b>[Personal Quest]</b></color> <b>{0}</b> goes quiet: you no longer walk that path. It waits in your journal.", TitleOf(quest)));
				break;
			case PersonalQuestRules.DormancyAction.Wake:
				questBook.ResetObjective(blueprintQuestObjective);
				Quiet(ConstellationText.LocalizedFormat("Constellation.PersonalQuest", "<color=#5B2A86><b>[Personal Quest]</b></color> <b>{0}</b> stirs again, and takes up where it left off.", TitleOf(quest)));
				break;
			}
		}
	}

	private static bool IsDormant(PersonalQuestDefinition quest)
	{
		BlueprintQuestObjective blueprintQuestObjective = SubclassPersonalQuests.ObjectiveByName(quest?.DormantObjectiveName);
		QuestBook questBook = Book();
		if (blueprintQuestObjective != null && questBook != null)
		{
			return questBook.GetObjectiveState(blueprintQuestObjective) == QuestObjectiveState.Started;
		}
		return false;
	}

	internal static void Evaluate(PersonalQuestDefinition quest)
	{
		QuestBook questBook = Book();
		if (quest == null || questBook == null || QuestStateOf(quest) == QuestState.Completed || QuestStateOf(quest) == QuestState.Failed || IsDormant(quest))
		{
			return;
		}
		Dictionary<string, string> record = PersonalQuestRecord.Find();
		int chapter = Chapter();
		List<string> list = quest.Acts.Select((PersonalQuestAct a) => ObjectiveNameFor(quest, a, record)).ToList();
		List<QuestObjectiveState> states = list.Select(StateOf).ToList();
		List<bool> satisfied = quest.Acts.Select((PersonalQuestAct a) => ActSatisfied(a, record, chapter)).ToList();
		PersonalQuestRules.ActAction[] array = PersonalQuestRules.PlanActs(states, satisfied);
		bool flag = QuestStateOf(quest) != QuestState.None;
		for (int num = 0; num < array.Length; num++)
		{
			PersonalQuestAct act = quest.Acts[num];
			BlueprintQuestObjective blueprintQuestObjective = SubclassPersonalQuests.ObjectiveByName(list[num]);
			if (blueprintQuestObjective == null)
			{
				continue;
			}
			if (array[num] == PersonalQuestRules.ActAction.Give || array[num] == PersonalQuestRules.ActAction.GiveAndComplete)
			{
				questBook.GiveObjective(blueprintQuestObjective);
				if (!flag)
				{
					flag = true;
					PostLog(ConstellationText.LocalizedFormat("Constellation.PersonalQuest", "<color=#5B2A86><b>[Personal Quest]</b></color> Begun: <b>{0}</b>!", TitleOf(quest)));
				}
				if (array[num] == PersonalQuestRules.ActAction.Give)
				{
					PostLog(ConstellationText.LocalizedFormat("Constellation.PersonalQuest", "<color=#5B2A86><b>[Quest Updated]</b></color> <b>{0}</b>", TitleOf(blueprintQuestObjective)));
				}
			}
			if (array[num] == PersonalQuestRules.ActAction.Complete || array[num] == PersonalQuestRules.ActAction.GiveAndComplete)
			{
				questBook.CompleteObjective(blueprintQuestObjective);
				PostLog(ConstellationText.LocalizedFormat("Constellation.PersonalQuest", "<color=#004604><b>[Quest Completed]</b></color> <b>{0}</b>", TitleOf(blueprintQuestObjective)));
				PayActCoins(quest, act);
			}
		}
		TryGrantRelic(quest);
	}

	private static void PayActCoins(PersonalQuestDefinition quest, PersonalQuestAct act)
	{
		try
		{
			Dictionary<string, string> storage = PersonalQuestRecord.Ensure();
			if (PersonalQuestRules.ShouldPayCoins(PersonalQuestRecord.CoinsPaid(storage, quest.Id, act.Act)))
			{
				bool flag = !string.IsNullOrEmpty(act.BonusObjectiveId) && VanillaState(act.BonusObjectiveId) == QuestObjectiveState.Completed;
				int amount = PersonalQuestRules.CoinsForAct(act.Chapter, act.Trigger == PersonalQuestTrigger.Trial, flag);
				PersonalQuestRecord.MarkCoinsPaid(storage, quest.Id, act.Act);
				DivineTokens.AddClaimedCoinsWithoutLoopMultiplier(amount, ConstellationText.LocalizedFormat("Constellation.CosmicCoins.Sponsor.PersonalQuest", "Personal Quest ({0}, Act {1})", TitleOf(quest), act.Act));
				if (flag)
				{
					PostLog(ConstellationText.Localized("Constellation.PersonalQuest", "<color=#5B2A86><b>[Personal Quest]</b></color> You answered when you did not have to, and the constellations noticed. The act pays half again."));
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not pay an act's coins: " + ex);
		}
	}

	internal static void TryGrantRelic(PersonalQuestDefinition quest)
	{
		try
		{
			QuestBook questBook = Book();
			if (quest == null || questBook == null || string.IsNullOrEmpty(quest.RelicItemName))
			{
				return;
			}
			Dictionary<string, string> record = PersonalQuestRecord.Find();
			if (!PersonalQuestRules.AllActsFinished(quest.Acts.Select((PersonalQuestAct a) => StateOf(ObjectiveNameFor(quest, a, record))).ToList()))
			{
				return;
			}
			BlueprintItem blueprintItem = SubclassPersonalQuests.RelicByName(quest.RelicItemName);
			if (blueprintItem != null)
			{
				switch (PersonalQuestRules.DecideRelic(quest.RelicCountsAgainstCap, PersonalQuestRecord.RelicPaid(record, quest.RelicItemName), PartyHolds(blueprintItem), PersonalQuestRecord.ArchetypeRelicClaimed(record)))
				{
				case PersonalQuestRules.RelicGrant.Grant:
					Game.Instance.Player.Inventory.Add(blueprintItem, 1);
					PersonalQuestRecord.MarkRelicPaid(PersonalQuestRecord.Ensure(), quest.RelicItemName, quest.RelicCountsAgainstCap);
					PostLog(ConstellationText.LocalizedFormat("Constellation.PersonalQuest", "<color=#5B2A86><b>[Relic Earned]</b></color> <b>{0}</b> is yours.", blueprintItem.Name));
					break;
				case PersonalQuestRules.RelicGrant.AlreadyOwned:
					PersonalQuestRecord.MarkRelicPaid(PersonalQuestRecord.Ensure(), quest.RelicItemName, quest.RelicCountsAgainstCap);
					break;
				case PersonalQuestRules.RelicGrant.ArchetypeCapReached:
					Quiet(ConstellationText.Localized("Constellation.PersonalQuest", "<color=#3F3F3F><b>[Personal Quest]</b></color> One relic answers to one soul in one lifetime. The one you already carry is the one you keep."));
					PersonalQuestRecord.MarkRelicPaid(PersonalQuestRecord.Ensure(), quest.RelicItemName, countsAgainstCap: false);
					break;
				case PersonalQuestRules.RelicGrant.AlreadyPaid:
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not hand over a relic: " + ex);
		}
	}

	internal static bool PartyHolds(BlueprintItem item)
	{
		try
		{
			Player player = Game.Instance?.Player;
			if (player == null || item == null)
			{
				return false;
			}
			if (player.Inventory != null && player.Inventory.Contains(item))
			{
				return true;
			}
			List<UnitEntityData> allCharacters = player.AllCharacters;
			if (allCharacters == null)
			{
				return false;
			}
			foreach (UnitEntityData item2 in allCharacters.ToList())
			{
				IEnumerable<ItemEntity> enumerable = item2?.Body?.Items;
				if (enumerable != null && enumerable.Any((ItemEntity i) => i != null && i.Blueprint == item))
				{
					return true;
				}
			}
			return false;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not check who holds a relic: " + ex);
			return false;
		}
	}

	internal static bool IsTrialOfferedNow(string trialId)
	{
		try
		{
			PersonalQuestDefinition personalQuestDefinition = PersonalQuestCatalog.QuestForTrial(trialId);
			PersonalQuestAct personalQuestAct = PersonalQuestCatalog.ActForTrial(trialId);
			if (personalQuestDefinition == null || personalQuestAct == null || Book() == null)
			{
				return false;
			}
			if (!ArenaEnabled())
			{
				return false;
			}
			Dictionary<string, string> dictionary = PersonalQuestRecord.Find();
			if (PersonalQuestRecord.TrialWon(dictionary, trialId))
			{
				return false;
			}
			return PersonalQuestRules.IsTrialOffered(PersonalQuestCatalog.ForArchetypeToken(ActiveArchetypeToken()) == personalQuestDefinition, IsDormant(personalQuestDefinition), StateOf(ObjectiveNameFor(personalQuestDefinition, personalQuestAct, dictionary)), Chapter(), personalQuestAct.Chapter, DimensionalArenaManager.Instance.IsActive);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not decide whether to offer trial " + trialId + ": " + ex);
			return false;
		}
	}

	internal static bool IsDivergenceOfferedNow()
	{
		try
		{
			PersonalQuestDefinition devourer = PersonalQuestCatalog.Devourer;
			if (Book() == null)
			{
				return false;
			}
			if (!ArenaEnabled())
			{
				return false;
			}
			Dictionary<string, string> storage = PersonalQuestRecord.Find();
			return PersonalQuestRules.IsDivergenceOffered(PersonalQuestCatalog.ForArchetypeToken(ActiveArchetypeToken()) == devourer, IsDormant(devourer), StateOf(devourer.ActAt(2).ObjectiveName), Chapter(), PersonalQuestRecord.DevourerPath(storage));
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not decide whether to offer the divergence: " + ex);
			return false;
		}
	}

	public static void OnTrialCleared(string trialId)
	{
		try
		{
			if (PersonalQuestCatalog.ActForTrial(trialId) != null)
			{
				PersonalQuestRecord.MarkTrialWon(PersonalQuestRecord.Ensure(), trialId);
				Reconcile();
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not record trial " + trialId + ": " + ex);
		}
	}

	public static void OnDivergenceChosen(string path)
	{
		try
		{
			Dictionary<string, string> storage = PersonalQuestRecord.Ensure();
			if (PersonalQuestRecord.DevourerPath(storage) == null)
			{
				PersonalQuestRecord.SetDevourerPath(storage, path);
				Reconcile();
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not record the Devourer's path: " + ex);
		}
	}

	public static void OnDecreeRatified(string decreeId)
	{
		try
		{
			PersonalQuestRecord.MarkDecreeDone(PersonalQuestRecord.Ensure(), decreeId);
			Reconcile();
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not record decree " + decreeId + ": " + ex);
		}
	}

	internal static bool IsImperialDaisDecreeOpen()
	{
		try
		{
			PersonalQuestDefinition godEmperor = PersonalQuestCatalog.GodEmperor;
			if (Book() == null)
			{
				return false;
			}
			if (PersonalQuestCatalog.ForArchetypeToken(ActiveArchetypeToken()) != godEmperor)
			{
				return false;
			}
			if (IsDormant(godEmperor))
			{
				return false;
			}
			if (PersonalQuestRecord.DecreeDone(PersonalQuestRecord.Find(), "ImperialDais"))
			{
				return false;
			}
			return StateOf(godEmperor.ActAt(3).ObjectiveName) == QuestObjectiveState.Started;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not decide whether to offer the Imperial Dais decree: " + ex);
			return false;
		}
	}

	private static string TitleOf(PersonalQuestDefinition quest)
	{
		return SubclassPersonalQuests.QuestByName(quest?.QuestName)?.Title?.ToString() ?? quest?.Id ?? "";
	}

	private static string TitleOf(BlueprintQuestObjective objective)
	{
		return objective?.Title?.ToString() ?? objective?.name ?? "";
	}

	private static void PostLog(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, message, null, ConstellationCategory.Quest, "Personal Quest");
	}

	private static void Quiet(string message)
	{
		ConstellationChatManager.PostCombatLog(message);
	}
}
