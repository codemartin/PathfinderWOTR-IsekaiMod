using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionWishstoneWealth : ContextAction
	{
		public override string GetCaption()
		{
			return "Miracle of Wealth: Grants 500,000 Gold";
		}

		public override void RunAction()
		{
			Game.Instance?.Player?.GainMoney(500000L);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#FFD700><b>[Cosmic Wishstone] Miracle of Wealth!</b></color> 500,000 Gold has manifested directly into your party treasury from the outer realms!");
			});
		}
	}
}
