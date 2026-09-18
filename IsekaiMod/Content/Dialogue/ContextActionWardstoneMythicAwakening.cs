using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionWardstoneMythicAwakening : ContextAction
	{
		public override string GetCaption()
		{
			return "Awards 500 Cosmic Coins and triggers Wardstone Mythic Banter";
		}

		public override void RunAction()
		{
			if (ConstellationChatManager.HasTriggeredMilestone("Act1WardstoneClimax"))
			{
				return;
			}
			ConstellationChatManager.MarkMilestoneTriggered("Act1WardstoneClimax");
			List<string> wardstoneMythicAwakeningBanter = ConstellationDialogueBanter.GetWardstoneMythicAwakeningBanter(TimelineManager.GetCurrentCycle(), out var coinsAwarded, out var primarySponsor);
			double num = ConstellationDialogueBanter.RollMultiplier(primarySponsor, out var shoutoutMsg);
			int amount = (int)Math.Round((double)coinsAwarded * num);
			if (wardstoneMythicAwakeningBanter != null)
			{
				foreach (string item in wardstoneMythicAwakeningBanter)
				{
					ConstellationChatManager.PostLog(item, primarySponsor, ConstellationCategory.Quest, 0, "Wardstone Mythic Awakening");
				}
			}
			if (!string.IsNullOrEmpty(shoutoutMsg))
			{
				ConstellationChatManager.PostLog(shoutoutMsg, primarySponsor, ConstellationCategory.Quest, 0, "Wardstone Mythic Awakening");
			}
			DivineTokens.AddCoins(amount, primarySponsor);
		}
	}
}
