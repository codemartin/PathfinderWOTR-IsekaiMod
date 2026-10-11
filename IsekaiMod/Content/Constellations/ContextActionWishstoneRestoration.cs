using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;

namespace IsekaiMod.Content.Constellations;

[TypeId("9d3e2c61b0a44f7e8c5a1b2d3e4f5a71")]
public class ContextActionWishstoneRestoration : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Miracle of Restoration: party true resurrection, full spell slots and resources, condition cleanse";
	}

	protected override void RunSafely()
	{
		Player player = Game.Instance?.Player;
		if (player == null)
		{
			return;
		}
		foreach (UnitEntityData item in player.Party.ToList())
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
			item.Descriptor.Resources.FullRestoreAll(ContextActionRejuvenation.RejuvenationExcludedResources());
			List<Buff> list = new List<Buff>();
			foreach (Buff buff in item.Descriptor.Buffs)
			{
				if (buff.Blueprint.Harmful)
				{
					list.Add(buff);
				}
			}
			foreach (Buff item2 in list)
			{
				item2.Remove();
			}
		}
		player.Corruption?.Clear();
		EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
		{
			h.HandleLogMessage(ConstellationText.Localized("CosmicWishstone.Miracle", "<color=#5B2A86><b>[Cosmic Wishstone] Miracle of Restoration!</b></color> Divine starlight floods your entire company: all fallen allies rise fully restored, all spell slots and class abilities return to maximum (Merchant's Gamble pulls excepted), and all corruption dissolves into the cosmos!"));
		});
	}
}
