using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionWishWealth : ContextAction
	{
		public override string GetCaption()
		{
			return "Grants 500,000 gold";
		}

		public override void RunAction()
		{
			Game.Instance?.Player?.GainMoney(500000L);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#FFD700>[Cosmic Wish]</color> Granted 500,000 Gold from the cosmos!");
			});
		}
	}
}
