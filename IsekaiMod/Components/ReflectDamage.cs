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

namespace IsekaiMod.Components;

[TypeId("823bcefa4de241ed88a61db6886e16fb")]
public class ReflectDamage : UnitFactComponentDelegate, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, ITargetRulebookSubscriber
{
	[ThreadStatic]
	private static bool m_IsReflecting;

	public bool FullCounter;

	public void TryRunAction(RuleDealDamage e)
	{
		if (e == null || e.Target == null || base.Owner == null || m_IsReflecting || e.Target != base.Owner || e.Initiator == null || e.Initiator == base.Owner || e.Reason.Fact == base.Fact || !(base.Fact is IFactContextOwner factContextOwner) || !ShouldReflect(e.Result, e.Initiator.IsEnemy(base.Owner)))
		{
			return;
		}
		try
		{
			m_IsReflecting = true;
			int characterLevel = base.Owner.Progression?.CharacterLevel ?? 0;
			int reflected = (FullCounter ? CalculateFullCounterDamage(e.Result, characterLevel) : CalculateReflectedDamage(e.Result, characterLevel));
			ActionList action = ActionFlow.DealDamage(delegate(ContextActionDealDamage c)
			{
				c.IgnoreCritical = true;
				c.UseWeaponDamageModifiers = false;
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

	internal static bool ShouldReflect(int incomingDamage, bool initiatorIsEnemy)
	{
		return (incomingDamage > 0) & initiatorIsEnemy;
	}

	internal static int CalculateReflectedDamage(int incomingDamage, int characterLevel)
	{
		if (incomingDamage <= 0)
		{
			return 0;
		}
		int val = ((characterLevel >= 15) ? (incomingDamage / 2) : (incomingDamage / 4));
		int val2 = 10 + Math.Max(0, characterLevel);
		return Math.Max(1, Math.Min(val2, val));
	}

	internal static int CalculateFullCounterDamage(int incomingDamage, int characterLevel)
	{
		if (incomingDamage <= 0)
		{
			return 0;
		}
		int val = ((characterLevel >= 15) ? (incomingDamage * 3 / 4) : (incomingDamage / 2));
		return Math.Max(1, val);
	}

	public void OnEventAboutToTrigger(RuleDealDamage evt)
	{
	}

	public void OnEventDidTrigger(RuleDealDamage evt)
	{
		TryRunAction(evt);
	}
}
