using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace IsekaiMod.Components
{
	[TypeId("9174128509ca4eb4a99182301cde9941")]
	public class ContextConditionCasterLevel : ContextCondition
	{
		public int MinLevel = 1;

		public override string GetConditionCaption()
		{
			return $"Caster character level >= {MinLevel}";
		}

		public override bool CheckCondition()
		{
			UnitEntityData unitEntityData = base.Context?.MaybeCaster;
			if (unitEntityData == null)
			{
				return false;
			}
			return (unitEntityData.Progression?.CharacterLevel ?? 1) >= MinLevel;
		}
	}
}
