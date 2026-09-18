using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionAreeluLoopTruthBanter : ContextAction
	{
		public override string GetCaption()
		{
			return "Awards 500 Cosmic Coins and triggers Constellation reaction to the Loop Truth reveal";
		}

		public override void RunAction()
		{
			if (ConstellationChatManager.HasTriggeredMilestone("Act3AreeluLabTruth"))
			{
				return;
			}
			ConstellationChatManager.MarkMilestoneTriggered("Act3AreeluLabTruth");
			List<string> act3AreeluLabTruthBanter = ConstellationDialogueBanter.GetAct3AreeluLabTruthBanter(TimelineManager.GetCurrentCycle(), out var coinsAwarded, out var primarySponsor);
			if (act3AreeluLabTruthBanter != null)
			{
				foreach (string item in act3AreeluLabTruthBanter)
				{
					ConstellationChatManager.PostLog(item, primarySponsor, ConstellationCategory.Quest, 0, "Areelu's Laboratory: The Unscripted Soul");
				}
			}
			if (coinsAwarded > 0)
			{
				DivineTokens.AddCoins(coinsAwarded, primarySponsor);
			}
		}
	}
}
