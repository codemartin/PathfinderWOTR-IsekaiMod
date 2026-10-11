using System;
using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace IsekaiMod.Components;

[TypeId("51c463cf4610465883eb61dd3f67f50c")]
public class AegisUndyingTrigger : UnitFactComponentDelegate, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, ITargetRulebookSubscriber
{
	public int Heal = 150;

	public int ThresholdPercent = 25;

	public int Rounds = 3;

	public BlueprintBuffReference m_Buff;

	public int CooldownSeconds = 6;

	private readonly Dictionary<string, TimeSpan> m_LastTrigger = new Dictionary<string, TimeSpan>();

	internal static bool ShouldTrigger(int maxHp, int hpLeft, bool dead, int thresholdPercent, int damageDealt, TimeSpan now, TimeSpan? lastTrigger, TimeSpan cooldown)
	{
		if (damageDealt <= 0)
		{
			return false;
		}
		if (!ShouldTrigger(maxHp, hpLeft, dead, thresholdPercent))
		{
			return false;
		}
		if (lastTrigger.HasValue)
		{
			return now - lastTrigger.Value >= cooldown;
		}
		return true;
	}

	internal static bool ShouldTrigger(int maxHp, int hpLeft, bool dead, int thresholdPercent)
	{
		if (dead || maxHp <= 0 || hpLeft <= 0)
		{
			return false;
		}
		return (long)hpLeft * 100L < (long)maxHp * (long)thresholdPercent;
	}

	public void OnEventAboutToTrigger(RuleDealDamage evt)
	{
	}

	public void OnEventDidTrigger(RuleDealDamage evt)
	{
		try
		{
			UnitEntityData owner = base.Owner;
			if (evt == null || owner == null || evt.Target != owner)
			{
				return;
			}
			TimeSpan timeSpan = Game.Instance?.TimeController?.GameTime ?? TimeSpan.Zero;
			string key = owner.UniqueId ?? "";
			TimeSpan? lastTrigger = (m_LastTrigger.TryGetValue(key, out var value) ? new TimeSpan?(value) : ((TimeSpan?)null));
			if (ShouldTrigger(owner.MaxHP, owner.HPLeft, owner.State?.IsDead ?? false, ThresholdPercent, evt.Result, timeSpan, lastTrigger, TimeSpan.FromSeconds(CooldownSeconds)))
			{
				m_LastTrigger[key] = timeSpan;
				Rulebook.Trigger(new RuleHealDamage(owner, owner, Heal));
				BlueprintBuff blueprintBuff = m_Buff?.Get();
				if (blueprintBuff != null)
				{
					owner.Descriptor.Buffs.AddBuff(blueprintBuff, owner, TimeSpan.FromSeconds(6 * Rounds));
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in AegisUndyingTrigger: " + ex);
		}
	}
}
