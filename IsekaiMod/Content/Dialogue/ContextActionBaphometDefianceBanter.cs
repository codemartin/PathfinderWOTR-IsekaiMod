using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionBaphometDefianceBanter : ContextAction
	{
		public override string GetCaption()
		{
			return "Awards 500 Cosmic Coins for humiliating Baphomet";
		}

		public override void RunAction()
		{
			if (ConstellationChatManager.HasTriggeredMilestone("Act4BaphometConfrontation"))
			{
				return;
			}
			ConstellationChatManager.MarkMilestoneTriggered("Act4BaphometConfrontation");
			List<string> act4BaphometConfrontationBanter = ConstellationDialogueBanter.GetAct4BaphometConfrontationBanter(TimelineManager.GetCurrentCycle(), out var coinsAwarded, out var primarySponsor);
			if (act4BaphometConfrontationBanter != null)
			{
				foreach (string item in act4BaphometConfrontationBanter)
				{
					ConstellationChatManager.PostLog(item, primarySponsor, ConstellationCategory.Quest, 0, "Defying the Lord of the Labyrinth");
				}
			}
			if (coinsAwarded > 0)
			{
				DivineTokens.AddCoins(coinsAwarded, primarySponsor);
			}
		}
	}
}
