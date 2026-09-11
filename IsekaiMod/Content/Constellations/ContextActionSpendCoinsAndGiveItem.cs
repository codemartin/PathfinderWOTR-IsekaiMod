using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionSpendCoinsAndGiveItem : ContextAction
	{
		public int Cost;

		public BlueprintItemReference m_Item;

		public int Amount = 1;

		public override string GetCaption()
		{
			return $"Spends {Cost} coins and gives {Amount} items";
		}

		public override void RunAction()
		{
			if (DivineTokens.SpendCoins(Cost))
			{
				BlueprintItem item = m_Item?.Get();
				if (item != null)
				{
					Game.Instance?.Player?.Inventory?.Add(item, Amount);
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage($"<color=#FFD700>[Cosmic Store]</color> Purchased {item.Name} x{Amount}!");
					});
				}
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
