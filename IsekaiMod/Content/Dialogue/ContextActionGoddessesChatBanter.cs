using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionGoddessesChatBanter : ContextAction
	{
		public override string GetCaption()
		{
			return "Triggers Constellation Banter at the Goddesses Summit";
		}

		public override void RunAction()
		{
			if (ConstellationChatManager.HasTriggeredMilestone("Act5DivineSummit"))
			{
				return;
			}
			ConstellationChatManager.MarkMilestoneTriggered("Act5DivineSummit");
			List<string> act5DivineSummitBanter = ConstellationDialogueBanter.GetAct5DivineSummitBanter(TimelineManager.GetCurrentCycle(), out var coinsAwarded, out var primarySponsor);
			if (act5DivineSummitBanter != null)
			{
				foreach (string item in act5DivineSummitBanter)
				{
					ConstellationChatManager.PostLog(item, primarySponsor, ConstellationCategory.Quest, 0, "The Goddesses Summit");
				}
			}
			if (coinsAwarded > 0)
			{
				DivineTokens.AddCoins(coinsAwarded, primarySponsor);
			}
		}
	}
}
