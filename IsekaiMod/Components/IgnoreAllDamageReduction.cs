using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[TypeId("8b2930b0feb64a2c915e6a0624028230")]
public class IgnoreAllDamageReduction : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, IInitiatorRulebookSubscriber
{
	public void OnEventAboutToTrigger(RuleDealDamage evt)
	{
		if (evt != null && !(evt.Initiator != base.Owner))
		{
			evt.IgnoreDamageReduction = true;
		}
	}

	public void OnEventDidTrigger(RuleDealDamage evt)
	{
	}
}
