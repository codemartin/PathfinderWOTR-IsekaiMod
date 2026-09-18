using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
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
			List<string> slimeRevealBanter = ConstellationDialogueBanter.GetSlimeRevealBanter();
			if (slimeRevealBanter == null)
			{
				return;
			}
			foreach (string item in slimeRevealBanter)
			{
				ConstellationChatManager.PostLog(item, "The Laughing King", ConstellationCategory.Subclass, 0, "Slime Reveal");
			}
		}
	}
}
