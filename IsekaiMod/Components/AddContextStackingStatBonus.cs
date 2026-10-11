using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace IsekaiMod.Components;

[TypeId("2234c1b3e6f24b78a0b68ada5ab3b17f")]
[ComponentName("Add context stat bonus that stacks with every bonus of its type")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class AddContextStackingStatBonus : UnitFactComponentDelegate
{
	public ModifierDescriptor Descriptor;

	public StatType Stat;

	public int Multiplier = 1;

	public ContextValue Value;

	public override void OnTurnOn()
	{
		ModifiableValue modifiableValue = base.Owner?.Stats?.GetStat(Stat);
		if (modifiableValue == null || base.Fact == null || Value == null)
		{
			return;
		}
		MechanicsContext maybeContext = base.Fact.MaybeContext;
		if (maybeContext != null)
		{
			string component = base.Runtime?.SourceBlueprintComponentName;
			if (!(modifiableValue.Modifiers ?? Enumerable.Empty<ModifiableValue.Modifier>()).Any((ModifiableValue.Modifier m) => m != null && m.Source == base.Fact && m.SourceComponent == component))
			{
				modifiableValue.AddModifier(AddStackingStatBonus.CreateModifier(Value.Calculate(maybeContext) * Multiplier, Descriptor, base.Fact, component));
			}
		}
	}

	public override void OnTurnOff()
	{
		base.Owner?.Stats?.GetStat(Stat)?.RemoveModifiersFrom(base.Runtime);
	}
}
