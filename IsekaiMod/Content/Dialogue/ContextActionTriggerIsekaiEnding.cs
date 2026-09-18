using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Quests;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionTriggerIsekaiEnding : ContextAction
	{
		public string EndingId;

		public string EndingTitle;

		public bool CheckLoopBreakerKeys;

		public bool ForceLoopShatter;

		public override string GetCaption()
		{
			return "Triggers Isekai Ending: " + EndingTitle;
		}

		public override void RunAction()
		{
			if (ConstellationChatManager.HasTriggeredMilestone("Act6ThresholdEnding"))
			{
				return;
			}
			ConstellationChatManager.MarkMilestoneTriggered("Act6ThresholdEnding");
			bool shatterLoop = ForceLoopShatter;
			if (CheckLoopBreakerKeys)
			{
				shatterLoop = TimelineManager.Data.LoopShattered || IsekaiTransmigrationQuest.AreAllSideQuestsCompleted(out var _);
			}
			try
			{
				List<string> act6ThresholdLoopBreakerBanter = ConstellationDialogueBanter.GetAct6ThresholdLoopBreakerBanter(EndingId, out var coinsAwarded, out var primarySponsor);
				if (act6ThresholdLoopBreakerBanter != null)
				{
					foreach (string item in act6ThresholdLoopBreakerBanter)
					{
						ConstellationChatManager.PostLog(item, primarySponsor, ConstellationCategory.MetaLoop, 0, "Grand Climax: " + EndingTitle);
					}
				}
				if (coinsAwarded > 0)
				{
					DivineTokens.AddCoins(coinsAwarded, primarySponsor);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error logging ending banter: " + ex);
			}
			IsekaiTransmigrationQuest.TriggerEnding(EndingId, EndingTitle, shatterLoop);
		}
	}
}
