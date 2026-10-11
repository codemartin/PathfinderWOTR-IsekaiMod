using System;

namespace IsekaiMod.Content.Quests;

internal sealed class PersonalQuestAct
{
	internal int Act;

	internal string ObjectiveName;

	internal int Chapter;

	internal PersonalQuestTrigger Trigger = PersonalQuestTrigger.None;

	internal string[] TriggerIds = Array.Empty<string>();

	internal PersonalQuestTrigger Fallback = PersonalQuestTrigger.None;

	internal string[] FallbackIds = Array.Empty<string>();

	internal bool FallbackRequiresAutoCrusade;

	internal string TrialId;

	internal string BonusObjectiveId;

	internal string MarkerPointGuid;

	internal bool MarkerIsColiseumTile;

	internal bool CatchUpByChapter
	{
		get
		{
			if (Trigger != PersonalQuestTrigger.Trial)
			{
				return Trigger != PersonalQuestTrigger.DevourerDivergence;
			}
			return false;
		}
	}

	internal int ExperienceCR => PersonalQuestRules.ExperienceCRForChapter(Chapter);
}
