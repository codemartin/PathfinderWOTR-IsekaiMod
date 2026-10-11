using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[TypeId("5a55db142ba54dc28705d74c9ee78b40")]
[ComponentName("Add stat bonus that stacks with every bonus of its type")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class AddStackingStatBonus : UnitFactComponentDelegate
{
	public ModifierDescriptor Descriptor;

	public StatType Stat;

	public int Value;

	internal static int Amount(int value, int rank)
	{
		return value * Math.Max(1, rank);
	}

	internal static ModifiableValue.Modifier CreateModifier(int value, ModifierDescriptor descriptor, EntityFact source, string sourceComponent)
	{
		return new ModifiableValue.Modifier
		{
			ModValue = value,
			ModDescriptor = descriptor,
			StackMode = ModifiableValue.StackMode.ForceStack,
			Source = source,
			SourceComponent = sourceComponent,
			ItemSource = source?.MaybeContext?.ParentContext?.SourceItem
		};
	}

	private static bool AlreadyApplied(ModifiableValue stat, EntityFact source, string sourceComponent)
	{
		return (stat.Modifiers ?? Enumerable.Empty<ModifiableValue.Modifier>()).Any((ModifiableValue.Modifier m) => m != null && m.Source == source && m.SourceComponent == sourceComponent);
	}

	public override void OnTurnOn()
	{
		ModifiableValue modifiableValue = base.Owner?.Stats?.GetStat(Stat);
		if (modifiableValue != null && base.Fact != null)
		{
			string sourceComponent = base.Runtime?.SourceBlueprintComponentName;
			if (!AlreadyApplied(modifiableValue, base.Fact, sourceComponent))
			{
				modifiableValue.AddModifier(CreateModifier(Amount(Value, base.Fact.GetRank()), Descriptor, base.Fact, sourceComponent));
			}
		}
	}

	public override void OnTurnOff()
	{
		base.Owner?.Stats?.GetStat(Stat)?.RemoveModifiersFrom(base.Runtime);
	}
}
