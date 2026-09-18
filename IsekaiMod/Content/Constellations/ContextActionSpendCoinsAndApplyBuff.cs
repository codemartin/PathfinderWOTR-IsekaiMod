using System;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionSpendCoinsAndApplyBuff : ContextAction
	{
		public int Cost;

		public BlueprintBuffReference m_Buff;

		public int DurationHours = 1;

		public override string GetCaption()
		{
			return $"Spends {Cost} Cosmic Coins and applies buff for {DurationHours} hours";
		}

		public override void RunAction()
		{
			UnitEntityData unitEntityData = base.Target?.Unit ?? DivineTokens.GetPlayer();
			BlueprintBuff buff = m_Buff?.Get();
			if (unitEntityData == null || buff == null)
			{
				return;
			}
			if (DivineTokens.SpendCoins(Cost))
			{
				if (base.Context != null)
				{
					unitEntityData.Descriptor.Buffs.AddBuff(buff, base.Context, new TimeSpan(DurationHours, 0, 0));
				}
				else
				{
					unitEntityData.Descriptor.Buffs.AddBuff(buff, unitEntityData, new TimeSpan(DurationHours, 0, 0));
				}
				Main.Log(System.Text.RegularExpressions.Regex.Replace($"<color=#F5C542>[Cosmic Store]</color> Activated blessing: {buff.Name} ({DurationHours}h)!", "<[^>]+>", ""));
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#F5C542>[Cosmic Store]</color> Activated blessing: {buff.Name} ({DurationHours}h)!");
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
