using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[AllowMultipleComponents]
[TypeId("0879aaa34709436599999fa66a5cf60e")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class IsekaiHealingBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleHealDamage>, IRulebookHandler<RuleHealDamage>, ISubscriber, IInitiatorRulebookSubscriber
{
	public int Value;

	internal static bool Applies(bool fromAbility)
	{
		return fromAbility;
	}

	public void OnEventAboutToTrigger(RuleHealDamage evt)
	{
		if (Applies(evt.Reason?.Context?.SourceAbility != null))
		{
			evt.AddModifierBonus(Value, base.Fact);
		}
	}

	public void OnEventDidTrigger(RuleHealDamage evt)
	{
	}
}
