using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[AllowMultipleComponents]
[TypeId("70138442252a499c90c75436afdcdd7e")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class IsekaiUnarmedAndNaturalBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>
{
	public int AttackBonus;

	public int DamageBonus;

	public ModifierDescriptor Bonus = ModifierDescriptor.Sacred;

	internal static bool Applies(bool isNatural, bool isUnarmed)
	{
		return isNatural | isUnarmed;
	}

	private static bool Counts(ItemEntityWeapon weapon)
	{
		BlueprintItemWeapon blueprintItemWeapon = weapon?.Blueprint;
		if (blueprintItemWeapon != null)
		{
			return Applies(blueprintItemWeapon.IsNatural, blueprintItemWeapon.IsUnarmed);
		}
		return false;
	}

	public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
	{
		if (AttackBonus != 0 && Counts(evt.Weapon))
		{
			evt.AddModifier(AttackBonus, base.Fact, Bonus);
		}
	}

	public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
	{
	}

	public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
	{
		if (DamageBonus != 0 && Counts(evt.Weapon))
		{
			evt.AddDamageModifier(DamageBonus, base.Fact, Bonus);
		}
	}

	public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
	{
	}
}
