using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionDesnaNatureCommunion : ContextAction
	{
		public int Amount = 250;

		public override string GetCaption()
		{
			return "Awards 250 Cosmic Coins and triggers Elysian Nature Communion";
		}

		public override void RunAction()
		{
			DivineTokens.AddCoins(Amount, "The Song of the Spheres");
			ConstellationChatManager.PostLog("<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"The spirits of ancient Sarkoris and the beasts of Elysium heed the gentle rhythm of your living jelly core.\"</i>", "The Song of the Spheres", ConstellationCategory.Subclass, 0, "Elysian Nature Communion");
		}
	}
}
