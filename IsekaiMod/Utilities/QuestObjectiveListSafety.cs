using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Globalmap.Blueprints;
using TabletopTweaks.Core.ModLogic;

namespace IsekaiMod.Utilities;

internal static class QuestObjectiveListSafety
{
	public static int Fill(BlueprintQuestObjective objective)
	{
		if (objective == null)
		{
			return 0;
		}
		int num = 0;
		if (objective.Locations == null)
		{
			objective.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			num++;
		}
		if (objective.MultiEntranceEntries == null)
		{
			objective.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
			num++;
		}
		if (objective.m_Areas == null)
		{
			objective.m_Areas = new List<BlueprintAreaReference>();
			num++;
		}
		if (objective.m_Addendums == null)
		{
			objective.m_Addendums = new List<BlueprintQuestObjectiveReference>();
			num++;
		}
		if (objective.m_NextObjectives == null)
		{
			objective.m_NextObjectives = new List<BlueprintQuestObjectiveReference>();
			num++;
		}
		if (objective.ComponentsArray == null)
		{
			objective.ComponentsArray = new BlueprintComponent[0];
			num++;
		}
		if (objective.Quest != null && objective.Quest.ComponentsArray == null)
		{
			objective.Quest.ComponentsArray = new BlueprintComponent[0];
			num++;
		}
		return num;
	}

	public static void FillAllLoaded()
	{
		// Fill() reads objective.Quest, which loads the quest blueprint into the cache. Doing that while
		// ForEachLoaded is enumerating the cache throws "Collection was modified" and the whole step was
		// skipped (seen in Player.log on the 6.5 build). Collect first, then fill.
		List<BlueprintQuestObjective> objectiveList = new List<BlueprintQuestObjective>();
		List<BlueprintQuest> questList = new List<BlueprintQuest>();
		ResourcesLibrary.BlueprintsCache.ForEachLoaded(delegate(BlueprintGuid guid, SimpleBlueprint bp)
		{
			if (bp is BlueprintQuestObjective objective)
			{
				objectiveList.Add(objective);
			}
			else if (bp is BlueprintQuest { ComponentsArray: null } blueprintQuest)
			{
				questList.Add(blueprintQuest);
			}
		});
		int objectives = 0;
		int fields = 0;
		foreach (BlueprintQuestObjective objective in objectiveList)
		{
			int num = Fill(objective);
			if (num > 0)
			{
				objectives++;
				fields += num;
			}
		}
		foreach (BlueprintQuest blueprintQuest in questList)
		{
			blueprintQuest.ComponentsArray = new BlueprintComponent[0];
			fields++;
		}
		if (objectives > 0)
		{
			((ModContextBase)Main.IsekaiContext).Logger.Log($"QuestObjectiveListSafety: filled {fields} missing list(s) on {objectives} quest objective(s).");
		}
	}
}
