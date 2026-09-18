using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionRejuvenation : ContextAction
	{
		public int Cost = 400;

		public override string GetCaption()
		{
			return "Restores all spell slots, resources, and cures negative conditions";
		}

		public override void RunAction()
		{
			UnitEntityData unitEntityData = base.Target?.Unit ?? DivineTokens.GetPlayer();
			if (unitEntityData == null)
			{
				return;
			}
			if (DivineTokens.SpendCoins(Cost))
			{
				foreach (Spellbook spellbook in unitEntityData.Descriptor.Spellbooks)
				{
					spellbook.Rest();
				}
				unitEntityData.Descriptor.Resources.FullRestoreAll();
				unitEntityData.Descriptor.Damage = 0;
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#FFD700>[Cosmic Elixir]</color> Rejuvenated body, mind, and soul! All spell slots and class abilities fully restored!");
				});
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
