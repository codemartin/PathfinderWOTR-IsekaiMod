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
		int objectives = 0;
		int fields = 0;
		ResourcesLibrary.BlueprintsCache.ForEachLoaded(delegate(BlueprintGuid guid, SimpleBlueprint bp)
		{
			if (bp is BlueprintQuestObjective objective)
			{
				int num = Fill(objective);
				if (num > 0)
				{
					objectives++;
					fields += num;
				}
			}
			else if (bp is BlueprintQuest { ComponentsArray: null } blueprintQuest)
			{
				blueprintQuest.ComponentsArray = new BlueprintComponent[0];
				fields++;
			}
		});
		if (objectives > 0)
		{
			((ModContextBase)Main.IsekaiContext).Logger.Log($"QuestObjectiveListSafety: filled {fields} missing list(s) on {objectives} quest objective(s).");
		}
	}
}
