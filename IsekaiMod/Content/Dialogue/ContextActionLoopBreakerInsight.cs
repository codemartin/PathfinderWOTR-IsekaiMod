using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
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
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#FFD700><b>[Loop-Breaker Conviction]</b></color> <i>You have defied both celestial dogma and abyssal temptation! The cosmic anchors of Yog-Sothoth tremble before your mortal resolve!</i>");
			});
		}
	}
}
