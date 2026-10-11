using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;

namespace IsekaiMod.Components;

[AllowMultipleComponents]
[ComponentName("Increase spell damage per die")]
[TypeId("5d0f6b7e2c8a4e39a1b7f4c3d2e6a918")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class IncreaseSpellDamagePerDie : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
{
	public int BonusPerDie = 1;

	internal static int BonusFor(int rolls, DiceType dice, int bonusPerDie)
	{
		if (rolls <= 0 || dice == DiceType.Zero)
		{
			return 0;
		}
		return rolls * bonusPerDie;
	}

	public void OnEventAboutToTrigger(RuleCalculateDamage evt)
	{
		if (evt?.Initiator != base.Owner || evt.DamageBundle == null)
		{
			return;
		}
		AbilityData abilityData = evt.Reason?.Ability;
		if (abilityData == null || abilityData.Blueprint == null || !abilityData.Blueprint.IsSpell)
		{
			return;
		}
		foreach (BaseDamage item in evt.DamageBundle)
		{
			if (item?.Dice != null)
			{
				DiceFormula modifiedValue = item.Dice.ModifiedValue;
				int num = BonusFor(modifiedValue.Rolls, modifiedValue.Dice, BonusPerDie);
				if (num != 0)
				{
					item.AddModifier(num, base.Fact);
				}
			}
		}
	}

	public void OnEventDidTrigger(RuleCalculateDamage evt)
	{
	}
}
