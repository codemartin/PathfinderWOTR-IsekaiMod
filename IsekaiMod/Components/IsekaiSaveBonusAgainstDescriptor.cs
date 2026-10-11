using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[AllowMultipleComponents]
[TypeId("ddccd2c0d0c14c569e5ca1deda87eb03")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class IsekaiSaveBonusAgainstDescriptor : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
{
	public SavingThrowType Save = SavingThrowType.Will;

	public SpellDescriptorWrapper Descriptor;

	public int Value;

	public ModifierDescriptor Bonus = ModifierDescriptor.Sacred;

	internal static bool Applies(SavingThrowType wanted, SavingThrowType actual, SpellDescriptor wantedDescriptors, SpellDescriptor effect)
	{
		if (wanted != SavingThrowType.Unknown && wanted != actual)
		{
			return false;
		}
		return (wantedDescriptors & effect) != 0;
	}

	public void OnEventAboutToTrigger(RuleSavingThrow evt)
	{
		SpellDescriptor valueOrDefault = (evt.Reason?.Context?.SpellDescriptor).GetValueOrDefault();
		if (Applies(Save, evt.Type, Descriptor.Value, valueOrDefault))
		{
			evt.AddModifier(Value, base.Fact, Bonus);
		}
	}

	public void OnEventDidTrigger(RuleSavingThrow evt)
	{
	}
}
