using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
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
			double num = ConstellationDialogueBanter.RollMultiplier("The Laughing King", out var shoutout);
			DivineTokens.AddCoins((int)(500.0 * num), "The Laughing King");
			if (!string.IsNullOrEmpty(shoutout))
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(shoutout);
				});
			}
			foreach (string line in ConstellationDialogueBanter.GetWardstoneMythicAwakeningBanter("The Laughing King"))
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(line);
				});
			}
		}
	}
}
