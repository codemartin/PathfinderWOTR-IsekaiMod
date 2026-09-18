using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionLoopBreakerInsight : ContextAction
	{
		public override string GetCaption()
		{
			return "Awards 1,000 Cosmic Coins and weakens Yog-Sothoth's causal anchors";
		}

		public override void RunAction()
		{
			DivineTokens.AddCoins(1000, "The Laughing King");
			ConstellationChatManager.PostLog("<color=#FFD700><b>[Loop-Breaker Conviction]</b></color> <i>You have defied both celestial dogma and abyssal temptation! The cosmic anchors of Yog-Sothoth tremble before your mortal resolve!</i>", "The Laughing King", ConstellationCategory.MetaLoop, 0, "Loop-Breaker Conviction");
			if (!ConstellationChatManager.HasTriggeredMilestone("Act5DivineSummit"))
			{
				new ContextActionGoddessesChatBanter().RunAction();
			}
		}
	}
}
