using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionGoddessesChatBanter : ContextAction
	{
		public override string GetCaption()
		{
			return "Triggers Constellation Live Chat Banter at the Goddesses Summit";
		}

		public override void RunAction()
		{
			DivineTokens.AddCoins(500, "The Laughing King");
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"HAHAHA! Standing between the Inheritor and the Demon Queen and checking the chat poll! Maximum ratings unlocked!\"</i>");
			});
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"Freedom is never chosen from someone else's menu. Dance on your own stage, starlight!\"</i>");
			});
		}
	}
}
