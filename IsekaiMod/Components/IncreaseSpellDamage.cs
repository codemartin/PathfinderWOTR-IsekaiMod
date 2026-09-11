using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace IsekaiMod.Components
{
	[AllowMultipleComponents]
	[ComponentName("Increase spell damage")]
	[TypeId("35ca7b8440554218931bdee77ead0b92")]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	public class IncreaseSpellDamage : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
	{
		public ContextValue DamageBonus;

		public void OnEventAboutToTrigger(RuleCalculateDamage evt)
		{
			if (!(evt.Initiator != base.Owner) && !(evt.Reason.Ability == null))
			{
				evt.DamageBundle.First?.AddModifier(DamageBonus.Calculate(base.Context), base.Fact);
			}
		}

		public void OnEventDidTrigger(RuleCalculateDamage evt)
		{
		}
	}
}
