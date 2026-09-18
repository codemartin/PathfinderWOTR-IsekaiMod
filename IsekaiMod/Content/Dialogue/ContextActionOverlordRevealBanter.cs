using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
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
			List<string> overlordSkeletonRevealBanter = ConstellationDialogueBanter.GetOverlordSkeletonRevealBanter();
			if (overlordSkeletonRevealBanter == null)
			{
				return;
			}
			foreach (string item in overlordSkeletonRevealBanter)
			{
				ConstellationChatManager.PostLog(item, "Urgathoa", ConstellationCategory.Subclass, 0, "Overlord Reveal");
			}
		}
	}
}
