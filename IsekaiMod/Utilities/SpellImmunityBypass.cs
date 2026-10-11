using System;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal static class SpellImmunityBypass
{
	public static readonly SpellDescriptor AllDescriptors = UnionOfAllDescriptors();

	internal static SpellDescriptor UnionOfAllDescriptors()
	{
		long num = 0L;
		foreach (object value in Enum.GetValues(typeof(SpellDescriptor)))
		{
			num |= Convert.ToInt64(value);
		}
		return (SpellDescriptor)num;
	}

	public static void Add(BlueprintFact fact)
	{
		fact.AddComponent(delegate(IgnoreSpellImmunity c)
		{
			c.SpellDescriptor = AllDescriptors;
		});
	}
}
