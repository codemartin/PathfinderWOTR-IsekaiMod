using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Corruption;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionWishstoneRestoration : ContextAction
	{
		public override string GetCaption()
		{
			return "Miracle of Restoration: Party True Resurrection, full spell slots, and condition cleanse";
		}

		public override void RunAction()
		{
			List<UnitEntityData> list = Game.Instance?.Player?.Party;
			if (list == null)
			{
				return;
			}
			foreach (UnitEntityData item in list)
			{
				if (item == null)
				{
					continue;
				}
				if (item.Descriptor.State.IsDead)
				{
					item.Descriptor.ResurrectAndFullRestore(item);
				}
				item.Descriptor.Damage = 0;
				foreach (Spellbook spellbook in item.Descriptor.Spellbooks)
				{
					spellbook.Rest();
				}
				item.Descriptor.Resources.FullRestoreAll();
				List<Buff> list2 = new List<Buff>();
				foreach (Buff buff in item.Descriptor.Buffs)
				{
					if (buff.Blueprint.Harmful)
					{
						list2.Add(buff);
					}
				}
				foreach (Buff item2 in list2)
				{
					item.Descriptor.RemoveFact(item2);
				}
			}
			if (Game.Instance?.Player?.Corruption != null)
			{
				CorruptionManager corruptionManager = Game.Instance?.Player?.Corruption;
				if (corruptionManager != null)
				{
					corruptionManager.CurrentValue = 0;
				}
			}
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#FFD700><b>[Cosmic Wishstone] Miracle of Restoration!</b></color> Divine starlight floods your entire company: all fallen allies rise fully restored, all spell slots and class abilities return to maximum, and all corruption dissolves into the cosmos!");
			});
		}
	}
}
