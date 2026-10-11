using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;

namespace IsekaiMod.Components;

[TypeId("9c4e0b7a35d84d1ab6f2c0e5a71d3f28")]
public class ContextConditionTargetIsLiving : IsekaiContextCondition
{
	public override string GetConditionCaption()
	{
		return "Target is a living creature";
	}

	protected override bool CheckSafely()
	{
		return IsLiving(base.Target?.Unit);
	}

	internal static bool IsLiving(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		return !ContextConditionImmuneToDeathEffect.HasDeathDescriptorImmunity(unit);
	}
}
