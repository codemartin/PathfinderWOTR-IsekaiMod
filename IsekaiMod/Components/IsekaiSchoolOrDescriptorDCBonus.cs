using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace IsekaiMod.Components;

[AllowMultipleComponents]
[TypeId("ec9642e726f9491298c8625e1f41d1e1")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class IsekaiSchoolOrDescriptorDCBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
{
	public SpellSchool School;

	public SpellDescriptorWrapper Descriptor;

	public int Value;

	public ModifierDescriptor Bonus = ModifierDescriptor.Sacred;

	internal static bool Applies(SpellSchool wantedSchool, SpellSchool school, SpellDescriptor wanted, SpellDescriptor descriptor)
	{
		if (wantedSchool != SpellSchool.None && school == wantedSchool)
		{
			return true;
		}
		return (wanted & descriptor) != 0;
	}

	public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
	{
		BlueprintAbility spell = evt.Spell;
		if (spell != null && Applies(School, spell.School, Descriptor.Value, spell.SpellDescriptor))
		{
			evt.AddBonusDC(Value, Bonus);
		}
	}

	public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
	{
	}
}
