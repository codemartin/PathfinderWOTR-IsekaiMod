using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
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
			DivineTokens.AddCoins(500, "The Laughing King");
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Did the Commander just threaten to turn the Lord of the Labyrinth into soup and drinking cups?! Absolute legendary performance!\"</i>");
			});
		}
	}
}
