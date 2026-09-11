using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionOverlordRevealBanter : ContextAction
	{
		public override string GetCaption()
		{
			return "Awards 250 Cosmic Coins and triggers Overlord Reveal Banter";
		}

		public override void RunAction()
		{
			DivineTokens.AddCoins(250, "Urgathoa");
			foreach (string line in ConstellationDialogueBanter.GetOverlordSkeletonRevealBanter())
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(line);
				});
			}
		}
	}
}
