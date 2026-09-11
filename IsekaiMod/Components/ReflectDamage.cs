using System;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace IsekaiMod.Components
{
	[TypeId("823bcefa4de241ed88a61db6886e16fb")]
	public class ReflectDamage : UnitFactComponentDelegate, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, ITargetRulebookSubscriber
	{
		[ThreadStatic]
		private static bool m_IsReflecting;

		public void TryRunAction(RuleDealDamage e)
		{
			if (m_IsReflecting || e.Target != base.Owner || e.Initiator == null || e.Initiator == base.Owner || e.Reason.Fact == base.Fact || !(base.Fact is IFactContextOwner factContextOwner))
			{
				return;
			}
			try
			{
				m_IsReflecting = true;
				int reflected = ((base.Owner.Progression != null && base.Owner.Progression.CharacterLevel >= 15) ? Math.Max(1, e.Result * 3 / 4) : Math.Max(1, e.Result / 2));
				ActionList action = ActionFlow.DealDamage(delegate(ContextActionDealDamage c)
				{
					c.DamageType = new DamageTypeDescription
					{
						Type = DamageType.Direct,
						Physical = new DamageTypeDescription.PhysicalData(),
						Common = new DamageTypeDescription.CommomData()
					};
					c.Value = new ContextDiceValue
					{
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = reflected
					};
				});
				factContextOwner.RunActionInContext(action, e.Initiator);
			}
			finally
			{
				m_IsReflecting = false;
			}
		}

		public void OnEventAboutToTrigger(RuleDealDamage evt)
		{
		}

		public void OnEventDidTrigger(RuleDealDamage evt)
		{
			TryRunAction(evt);
		}
	}
}
