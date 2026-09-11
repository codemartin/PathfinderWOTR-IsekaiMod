using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
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
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"The spirits of ancient Sarkoris and the beasts of Elysium heed the gentle rhythm of your living jelly core.\"</i>");
			});
		}
	}
}
