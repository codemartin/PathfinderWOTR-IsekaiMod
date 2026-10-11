using System;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace IsekaiMod.Content.Constellations;

[TypeId("5af49d0248de48fb887ee48602936938")]
public class ContextActionSpendCoinsAndApplyBuff : IsekaiContextAction
{
	public int Cost;

	public BlueprintBuffReference m_Buff;

	public int DurationHours = 1;

	public override string GetCaption()
	{
		return $"Spends {Cost} Cosmic Coins and applies buff for {DurationHours} hours";
	}

	protected override void RunSafely()
	{
		UnitEntityData unitEntityData = DivineTokens.ResolveDialogTarget(base.Target, DivineTokens.GetPlayer);
		BlueprintBuff blueprintBuff = m_Buff?.Get();
		if (!(unitEntityData == null) && blueprintBuff != null && DivineTokens.SpendCoins(Cost))
		{
			if (base.Context != null)
			{
				unitEntityData.Descriptor.Buffs.AddBuff(blueprintBuff, base.Context, new TimeSpan(DurationHours, 0, 0));
			}
			else
			{
				unitEntityData.Descriptor.Buffs.AddBuff(blueprintBuff, unitEntityData, new TimeSpan(DurationHours, 0, 0));
			}
			DivineTokens.Log(CosmicStoreText.BlessingActivated, blueprintBuff.Name, DurationHours);
		}
	}
}
