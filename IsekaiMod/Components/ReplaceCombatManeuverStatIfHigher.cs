using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components
{
	[AllowMultipleComponents]
	[ComponentName("Replace CMB stat if replacement is higher")]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	[TypeId("6c01cdf3df1845a29ab74c21c3747748")]
	public class ReplaceCombatManeuverStatIfHigher : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateBaseCMB>, IRulebookHandler<RuleCalculateBaseCMB>, ISubscriber, IInitiatorRulebookSubscriber
	{
		public StatType StatType = StatType.Dexterity;

		public void OnEventAboutToTrigger(RuleCalculateBaseCMB evt)
		{
			if (!(evt?.Initiator == null) && base.Owner?.Stats != null && !(evt.Initiator != base.Owner))
			{
				ModifiableValueAttributeStat modifiableValueAttributeStat = base.Owner.Stats.GetStat(StatType.Strength) as ModifiableValueAttributeStat;
				if (base.Owner.Stats.GetStat(StatType) is ModifiableValueAttributeStat modifiableValueAttributeStat2 && modifiableValueAttributeStat != null && modifiableValueAttributeStat2.Bonus > modifiableValueAttributeStat.Bonus)
				{
					evt.ReplaceStrength = StatType;
				}
			}
		}

		public void OnEventDidTrigger(RuleCalculateBaseCMB evt)
		{
		}
	}
}
