using System.Linq;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;

namespace IsekaiMod.Content.Arenas
{
	public class ActionStartTradeWithSmuggler : GameAction
	{
		public override string GetCaption()
		{
			return "Start trade with Void Market Smuggler";
		}

		public override void RunAction()
		{
			UnitEntityData smuggler = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>()?.FirstOrDefault((UnitEntityData u) => u.Blueprint?.name == "VoidMarketSmugglerUnit") ?? Game.Instance?.DialogController?.CurrentSpeaker;
			if (smuggler != null)
			{
				EventBus.RaiseEvent(delegate(IVendorUIHandler h)
				{
					h.HandleTradeStarted(smuggler);
				});
			}
		}
	}
}
