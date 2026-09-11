using System;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace IsekaiMod.Components
{
	[TypeId("31889a286c02440da2565b130a1cddc7")]
	public class ContextConditionTargetHPLessThanPercent : ContextCondition
	{
		public int Percent = 25;

		public override string GetConditionCaption()
		{
			return $"Target HP is less than or equal to {Percent}%";
		}

		public override bool CheckCondition()
		{
			UnitEntityData unitEntityData = base.Target?.Unit ?? base.Context?.MaybeCaster ?? base.Context?.MaybeOwner;
			if (unitEntityData == null)
			{
				return false;
			}
			return unitEntityData.HPLeft * 100 / Math.Max(1, unitEntityData.MaxHP) <= Percent;
		}
	}
}
