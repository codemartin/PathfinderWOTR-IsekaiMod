using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;

namespace IsekaiMod.Components
{
	[AllowMultipleComponents]
	[TypeId("ffa9f2123e7f4b949250ca3f1ae9aea0")]
	public class SetFlatFootedOnAttack : EntityFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
	{
		public void OnEventAboutToTrigger(RuleAttackRoll evt)
		{
			if (evt.Initiator != base.Owner)
			{
				return;
			}
			using (base.Fact.MaybeContext?.GetDataScope(evt.Target))
			{
				evt.ForceFlatFooted = true;
			}
		}

		public void OnEventDidTrigger(RuleAttackRoll evt)
		{
		}
	}
}
