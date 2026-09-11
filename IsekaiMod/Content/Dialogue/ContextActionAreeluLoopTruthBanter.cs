using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
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
			DivineTokens.AddCoins(500, "The Key and the Gate");
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#9400D3><b>[The Key and the Gate]</b></color>: <i>\"The veil of causality is pierced. The architect of the wound gazes into the infinite recurring corridors of the All-in-One.\"</i>");
			});
		}
	}
}
