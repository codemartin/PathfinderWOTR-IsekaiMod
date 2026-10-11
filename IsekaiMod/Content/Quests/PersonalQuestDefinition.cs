using System;
using System.Linq;

namespace IsekaiMod.Content.Quests;

internal sealed class PersonalQuestDefinition
{
	internal string Id;

	internal string ArchetypeToken;

	internal string QuestName;

	internal string DormantObjectiveName;

	internal string RelicItemName;

	internal PersonalQuestAct[] Acts = Array.Empty<PersonalQuestAct>();

	internal bool RelicCountsAgainstCap => ArchetypeToken != null;

	internal PersonalQuestAct ActAt(int act)
	{
		return Acts.FirstOrDefault((PersonalQuestAct a) => a.Act == act);
	}

	internal PersonalQuestAct ActNamed(string objectiveName)
	{
		return Acts.FirstOrDefault((PersonalQuestAct a) => string.Equals(a.ObjectiveName, objectiveName, StringComparison.Ordinal));
	}
}
