using System.Linq;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;

namespace IsekaiMod.Content.Arenas
{
	public class ActionStartTradeWithEnvoy : GameAction
	{
		public override string GetCaption()
		{
			return "Start trade with Astral Coin Envoy";
		}

		public override void RunAction()
		{
			UnitEntityData envoy = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>()?.FirstOrDefault((UnitEntityData u) => u.Blueprint?.name == "AstralCoinEnvoyUnit") ?? Game.Instance?.DialogController?.CurrentSpeaker;
			if (envoy != null)
			{
				EventBus.RaiseEvent(delegate(IVendorUIHandler h)
				{
					h.HandleTradeStarted(envoy);
				});
			}
		}
	}
}
