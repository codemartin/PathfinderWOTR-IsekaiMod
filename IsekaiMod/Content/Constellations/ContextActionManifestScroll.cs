using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionManifestScroll : ContextAction
	{
		public int Cost = 1500;

		public BlueprintItemReference m_Item;

		public override string GetCaption()
		{
			return $"Manifests scroll for {Cost} coins";
		}

		public override void RunAction()
		{
			if (DivineTokens.SpendCoins(Cost))
			{
				BlueprintItem item = m_Item?.Get();
				if (item != null)
				{
					Game.Instance?.Player?.Inventory?.Add(item, 1);
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#FFD700>[Cosmic Store]</color> Manifested " + item.Name + " into your inventory!");
					});
				}
			}
			else
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#DC143C>[Cosmic Store]</color> Insufficient Cosmic Coins! Required: {Cost}.");
				});
			}
		}
	}
}
