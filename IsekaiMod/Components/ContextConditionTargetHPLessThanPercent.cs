using System;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;

namespace IsekaiMod.Components;

[TypeId("31889a286c02440da2565b130a1cddc7")]
public class ContextConditionTargetHPLessThanPercent : IsekaiContextCondition
{
	public int Percent = 25;

	public override string GetConditionCaption()
	{
		return $"Target HP is less than or equal to {Percent}%";
	}

	protected override bool CheckSafely()
	{
		UnitEntityData unitEntityData = base.Target?.Unit ?? base.Context?.MaybeCaster ?? base.Context?.MaybeOwner;
		if (unitEntityData == null)
		{
			return false;
		}
		return unitEntityData.HPLeft * 100 / Math.Max(1, unitEntityData.MaxHP) <= Percent;
	}
}
