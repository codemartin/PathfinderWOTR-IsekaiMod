using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionSlimeRevealBanter : ContextAction
	{
		public override string GetCaption()
		{
			return "Awards 250 Cosmic Coins and triggers Slime Reveal Banter";
		}

		public override void RunAction()
		{
			DivineTokens.AddCoins(250, "The Laughing King");
			foreach (string line in ConstellationDialogueBanter.GetSlimeRevealBanter())
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(line);
				});
			}
		}
	}
}
