using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;

namespace IsekaiMod.Components;

[TypeId("3f5c1e4d8a0b46c6a2d1f9b7c4e05a83")]
public class ContextConditionImmuneToDeathEffect : IsekaiContextCondition
{
	public override string GetConditionCaption()
	{
		return "Target is immune to death effects";
	}

	protected override bool CheckSafely()
	{
		return IsImmuneToDeathEffect(base.Target?.Unit);
	}

	internal static bool IsImmuneToDeathEffect(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		if (HasInfallible(unit))
		{
			return true;
		}
		return HasDeathDescriptorImmunity(unit);
	}

	private static bool HasInfallible(UnitEntityData unit)
	{
		UnitMechanicFeatures unitMechanicFeatures = unit.Descriptor?.State?.Features;
		if (unitMechanicFeatures != null)
		{
			return unitMechanicFeatures.Infallible;
		}
		return false;
	}

	internal static bool HasDeathDescriptorImmunity(UnitEntityData unit)
	{
		List<EntityFact> list = unit.Descriptor?.Facts?.List;
		if (list == null)
		{
			return false;
		}
		for (int i = 0; i < list.Count; i++)
		{
			EntityFact entityFact = list[i];
			if (entityFact != null)
			{
				SpellImmunityToSpellDescriptor component = entityFact.GetComponent<SpellImmunityToSpellDescriptor>();
				if (component != null && component.Descriptor.HasAnyFlag(SpellDescriptor.Death))
				{
					return true;
				}
				BuffDescriptorImmunity component2 = entityFact.GetComponent<BuffDescriptorImmunity>();
				if (component2 != null && component2.Descriptor.HasAnyFlag(SpellDescriptor.Death))
				{
					return true;
				}
			}
		}
		return false;
	}
}
