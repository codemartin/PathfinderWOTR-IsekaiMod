using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;

namespace IsekaiMod.Components;

[AllowMultipleComponents]
[TypeId("73903c351e76444996de31568471dfb8")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class IsekaiDescriptorSpellDamageBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
{
	public SpellDescriptorWrapper Descriptor;

	public int Value;

	internal static bool Applies(bool isSpell, SpellDescriptor wanted, SpellDescriptor effect)
	{
		if (isSpell)
		{
			return (wanted & effect) != 0;
		}
		return false;
	}

	public void OnEventAboutToTrigger(RuleCalculateDamage evt)
	{
		MechanicsContext mechanicsContext = evt.Reason?.Context;
		if (!Applies(mechanicsContext != null && mechanicsContext.SourceAbility?.Type == AbilityType.Spell, Descriptor.Value, mechanicsContext?.SpellDescriptor ?? SpellDescriptor.None))
		{
			return;
		}
		foreach (BaseDamage item in evt.DamageBundle)
		{
			item.AddModifier(Value, base.Fact);
		}
	}

	public void OnEventDidTrigger(RuleCalculateDamage evt)
	{
	}
}
