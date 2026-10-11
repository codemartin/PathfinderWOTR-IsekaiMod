using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.AreaLogic.QuestSystem;

namespace IsekaiMod.Content.Quests;

internal static class PersonalQuestRules
{
	internal enum ActAction
	{
		None,
		Give,
		GiveAndComplete,
		Complete
	}

	internal enum DormancyAction
	{
		None,
		Sleep,
		Wake
	}

	internal enum RelicGrant
	{
		Grant,
		AlreadyPaid,
		AlreadyOwned,
		ArchetypeCapReached
	}

	internal static bool IsEligible(bool isMainCharacter, int isekaiClassLevel, bool hasIsekaiFact)
	{
		if (isMainCharacter)
		{
			return (isekaiClassLevel > 0) | hasIsekaiFact;
		}
		return false;
	}

	internal static int ExperienceCRForChapter(int chapter)
	{
		switch (chapter)
		{
		case 1:
			return 4;
		case 2:
			return 8;
		case 3:
			return 12;
		case 4:
			return 15;
		default:
			if (chapter > 0)
			{
				return 18;
			}
			return 4;
		}
	}

	internal static int CoinsForAct(int chapter, bool isTrial, bool bonusEarned)
	{
		int num = Math.Max(1, Math.Min(6, chapter));
		int num2 = 100 * num;
		if (isTrial)
		{
			num2 += num2 / 2;
		}
		if (bonusEarned)
		{
			num2 += num2 / 2;
		}
		return num2;
	}

	internal static bool ActIsSatisfied(bool catchUpByChapter, bool mainTriggerMet, bool fallbackMet, bool chapterPassed)
	{
		if (!catchUpByChapter)
		{
			return mainTriggerMet;
		}
		return mainTriggerMet | fallbackMet | chapterPassed;
	}

	internal static bool CatchesUpByChapter(bool catchUpByChapter, bool arenaEnabled)
	{
		if (!catchUpByChapter)
		{
			return !arenaEnabled;
		}
		return true;
	}

	internal static bool ChapterPassed(int actChapter, int chapter)
	{
		return chapter > actChapter;
	}

	internal static ActAction[] PlanActs(IList<QuestObjectiveState> states, IList<bool> satisfied)
	{
		int num = states?.Count ?? 0;
		ActAction[] array = new ActAction[num];
		for (int i = 0; i < num; i++)
		{
			QuestObjectiveState questObjectiveState = states[i];
			if (questObjectiveState == QuestObjectiveState.Completed || questObjectiveState == QuestObjectiveState.Failed)
			{
				array[i] = ActAction.None;
				continue;
			}
			bool flag = satisfied != null && i < satisfied.Count && satisfied[i];
			if (questObjectiveState == QuestObjectiveState.None)
			{
				array[i] = ((!flag) ? ActAction.Give : ActAction.GiveAndComplete);
			}
			else
			{
				array[i] = (flag ? ActAction.Complete : ActAction.None);
			}
			if (!flag)
			{
				break;
			}
		}
		return array;
	}

	internal static bool AllActsFinished(IList<QuestObjectiveState> states)
	{
		if (states != null && states.Count > 0)
		{
			return states.All((QuestObjectiveState s) => s == QuestObjectiveState.Completed || s == QuestObjectiveState.Failed);
		}
		return false;
	}

	internal static DormancyAction DecideDormancy(bool isActiveArchetypeQuest, QuestState questState, QuestObjectiveState dormantState)
	{
		if (questState != QuestState.Started)
		{
			return DormancyAction.None;
		}
		if (isActiveArchetypeQuest)
		{
			if (dormantState != QuestObjectiveState.Started)
			{
				return DormancyAction.None;
			}
			return DormancyAction.Wake;
		}
		if (dormantState != QuestObjectiveState.None)
		{
			return DormancyAction.None;
		}
		return DormancyAction.Sleep;
	}

	internal static bool IsTrialOffered(bool isActiveArchetypeQuest, bool dormant, QuestObjectiveState actState, int chapter, int trialChapter, bool trialOrCupRunning)
	{
		if (isActiveArchetypeQuest && !dormant && actState == QuestObjectiveState.Started && chapter >= trialChapter)
		{
			return !trialOrCupRunning;
		}
		return false;
	}

	internal static RelicGrant DecideRelic(bool countsAgainstCap, bool alreadyPaid, bool ownedByParty, bool archetypeRelicClaimed)
	{
		if (alreadyPaid)
		{
			return RelicGrant.AlreadyPaid;
		}
		if (ownedByParty)
		{
			return RelicGrant.AlreadyOwned;
		}
		if (countsAgainstCap & archetypeRelicClaimed)
		{
			return RelicGrant.ArchetypeCapReached;
		}
		return RelicGrant.Grant;
	}

	internal static bool ShouldPayCoins(bool alreadyPaidForAct)
	{
		return !alreadyPaidForAct;
	}

	internal static string DevourerAct3ObjectiveName(string path)
	{
		if (!string.Equals(path, "Void", StringComparison.OrdinalIgnoreCase))
		{
			return PersonalQuestCatalog.Devourer.ActAt(3).ObjectiveName;
		}
		return "ObjectiveDevourerAct3Void";
	}

	internal static bool IsDivergenceOffered(bool isActiveArchetypeQuest, bool dormant, QuestObjectiveState act2State, int chapter, string chosenPath)
	{
		if (isActiveArchetypeQuest && !dormant && act2State == QuestObjectiveState.Started && chapter >= 2)
		{
			return string.IsNullOrEmpty(chosenPath);
		}
		return false;
	}
}
