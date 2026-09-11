using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;

namespace IsekaiMod.Components
{
	[AllowMultipleComponents]
	[TypeId("f9cdd93e34a149349a64dfec5b0b6deb")]
	public class SetAttackerAutoMiss : BlueprintComponent, IRuntimeEntityFactComponentProvider
	{
		public class Runtime : EntityFactComponent<UnitEntityData, SetAttackerAutoMiss>, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber
		{
			public override void OnTurnOn()
			{
			}

			public override void OnTurnOff()
			{
			}

			public void OnEventAboutToTrigger(RuleAttackRoll evt)
			{
				if (evt.Target != base.Owner)
				{
					return;
				}
				using (base.Fact.MaybeContext?.GetDataScope(evt.Initiator))
				{
					evt.AutoMiss = true;
				}
			}

			public void OnEventDidTrigger(RuleAttackRoll evt)
			{
			}
		}

		public EntityFactComponent CreateRuntimeFactComponent()
		{
			return new Runtime();
		}
	}
}
