using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionBuyWishCharge : ContextAction
	{
		public int Cost = 4000;

		public BlueprintAbilityResourceReference m_WishResource;

		public override string GetCaption()
		{
			return $"Spends {Cost} Cosmic Coins and adds 1 Wish charge";
		}

		public override void RunAction()
		{
			UnitEntityData unitEntityData = base.Target?.Unit ?? DivineTokens.GetPlayer();
			BlueprintAbilityResource blueprintAbilityResource = m_WishResource?.Get();
			if (unitEntityData == null || blueprintAbilityResource == null)
			{
				return;
			}
			if (DivineTokens.SpendCoins(Cost))
			{
				unitEntityData.Descriptor.Resources.Restore(blueprintAbilityResource, 1);
				Main.Log(System.Text.RegularExpressions.Regex.Replace("<color=#FFD700>[Cosmic Store]</color> Purchased 1 Cosmic Wish charge! You may now cast the Cosmic Wish modular ability in the field.", "<[^>]+>", ""));
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#FFD700>[Cosmic Store]</color> Purchased 1 Cosmic Wish charge! You may now cast the Cosmic Wish modular ability in the field.");
				});
			}
			else
			{
				Main.Log(System.Text.RegularExpressions.Regex.Replace($"<color=#DC143C>[Cosmic Store]</color> Insufficient Cosmic Coins! Required: {Cost}. Current: {DivineTokens.GetBalance()}.", "<[^>]+>", ""));
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#DC143C>[Cosmic Store]</color> Insufficient Cosmic Coins! Required: {Cost}. Current: {DivineTokens.GetBalance()}.");
				});
			}
		}
	}
}
