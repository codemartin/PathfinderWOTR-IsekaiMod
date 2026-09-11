using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionBuyGachaJackpot : ContextAction
	{
		public int Cost = 5000;

		public BlueprintUnitFactReference m_Fact;

		public override string GetCaption()
		{
			return "Buys Gacha Jackpot cheat and manifests wealth";
		}

		public override void RunAction()
		{
			UnitEntityData player = DivineTokens.GetPlayer();
			BlueprintUnitFact blueprintUnitFact = m_Fact?.Get();
			if (player == null || blueprintUnitFact == null)
			{
				return;
			}
			if (player.Descriptor.Progression.Features.HasFact(blueprintUnitFact))
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#DC143C>[Cosmic Store]</color> You already possess this permanent cosmic boon!");
				});
			}
			else if (DivineTokens.SpendCoins(Cost))
			{
				player.Descriptor.AddFact(blueprintUnitFact);
				Game.Instance?.Player?.GainMoney(250000L);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#FFD700><b>[Gacha Jackpot!]</b></color> You acquired the Gacha Jackpot cheat and 250,000 Gold has manifested directly into your purse!");
				});
			}
			else
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#DC143C>[Cosmic Store]</color> Insufficient Cosmic Coins! Required: {Cost}. Current: {DivineTokens.GetBalance()}.");
				});
			}
		}
	}
}
