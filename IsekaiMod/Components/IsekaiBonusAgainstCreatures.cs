using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;

namespace IsekaiMod.Components;

[AllowMultipleComponents]
[TypeId("2b8a67b4a0a34875947ae187ea200c2f")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class IsekaiBonusAgainstCreatures : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, IInitiatorRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>
{
	public BlueprintUnitFactReference m_RequiredFact;

	public bool RequireEvil;

	public bool RequireChaotic;

	public int AttackBonus;

	public int DamageBonus;

	public int ACBonus;

	public int SaveBonus;

	public bool SaveOnlyAgainstSpells;

	public int CasterLevelBonus;

	public ModifierDescriptor Bonus = ModifierDescriptor.Sacred;

	internal static bool CreatureMatches(bool factRequired, bool hasFact, bool requireEvil, bool isEvil, bool requireChaotic, bool isChaotic)
	{
		if (factRequired && !hasFact)
		{
			return false;
		}
		if (requireEvil && !isEvil)
		{
			return false;
		}
		if (requireChaotic && !isChaotic)
		{
			return false;
		}
		return true;
	}

	private bool Matches(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		BlueprintUnitFact blueprintUnitFact = m_RequiredFact?.Get();
		bool num = blueprintUnitFact != null;
		bool hasFact = num && unit.Descriptor.HasFact(blueprintUnitFact);
		Alignment valueVisible = unit.Descriptor.Alignment.ValueVisible;
		return CreatureMatches(num, hasFact, RequireEvil, valueVisible.HasComponent(AlignmentComponent.Evil), RequireChaotic, valueVisible.HasComponent(AlignmentComponent.Chaotic));
	}

	public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
	{
		if (AttackBonus != 0 && Matches(evt.Target))
		{
			evt.AddModifier(AttackBonus, base.Fact, Bonus);
		}
	}

	public void OnEventDidTrigger(RuleCalculateAttackBonus evt)
	{
	}

	public void OnEventAboutToTrigger(RuleCalculateDamage evt)
	{
		if (DamageBonus != 0 && Matches(evt.Target))
		{
			evt.DamageBundle.First?.AddModifier(DamageBonus, base.Fact);
		}
	}

	public void OnEventDidTrigger(RuleCalculateDamage evt)
	{
	}

	public void OnEventAboutToTrigger(RuleCalculateAC evt)
	{
		if (ACBonus != 0 && Matches(evt.Initiator))
		{
			evt.AddModifier(ACBonus, base.Fact, Bonus);
		}
	}

	public void OnEventDidTrigger(RuleCalculateAC evt)
	{
	}

	public void OnEventAboutToTrigger(RuleSavingThrow evt)
	{
		if (SaveBonus != 0)
		{
			MechanicsContext mechanicsContext = evt.Reason?.Context;
			if ((!SaveOnlyAgainstSpells || (mechanicsContext != null && mechanicsContext.SourceAbility?.Type == AbilityType.Spell)) && Matches(mechanicsContext?.MaybeCaster))
			{
				evt.AddModifier(SaveBonus, base.Fact, Bonus);
			}
		}
	}

	public void OnEventDidTrigger(RuleSavingThrow evt)
	{
	}

	public void OnEventAboutToTrigger(RuleCastSpell evt)
	{
		if (CasterLevelBonus != 0 && evt.Context?.Params != null)
		{
			AbilityData spell = evt.Spell;
			if ((object)spell != null && spell.Blueprint?.Type == AbilityType.Spell && Matches(evt.SpellTarget?.Unit))
			{
				evt.Context.Params.CasterLevel += CasterLevelBonus;
			}
		}
	}

	public void OnEventDidTrigger(RuleCastSpell evt)
	{
	}
}
