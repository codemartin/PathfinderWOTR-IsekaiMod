using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[ComponentName("Add Extra Off-Hand Attack")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[TypeId("327faf39b9a643c2bbca12c7e8f81ccb")]
public class AddExtraOffHandAttack : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, ISubscriber, IInitiatorRulebookSubscriber
{
	public int Number = 1;

	internal static bool WieldsTwoWeapons(RuleCalculateAttacksCount evt)
	{
		if (evt?.Initiator?.Body == null)
		{
			return false;
		}
		HandSlot primaryHand = evt.Initiator.Body.PrimaryHand;
		HandSlot secondaryHand = evt.Initiator.Body.SecondaryHand;
		if (primaryHand == null || secondaryHand == null || !primaryHand.HasWeapon || !secondaryHand.HasWeapon)
		{
			return false;
		}
		if (primaryHand.Weapon?.Blueprint == null || secondaryHand.Weapon?.Blueprint == null)
		{
			return false;
		}
		if (primaryHand.Weapon.Blueprint.IsNatural || secondaryHand.Weapon.Blueprint.IsNatural)
		{
			return false;
		}
		if (primaryHand.Weapon == evt.Initiator.Body.EmptyHandWeapon || secondaryHand.Weapon == evt.Initiator.Body.EmptyHandWeapon)
		{
			return false;
		}
		return evt.Result?.SecondaryHand != null;
	}

	public void OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
	{
		if (!(evt.Initiator != base.Owner) && WieldsTwoWeapons(evt))
		{
			evt.Result.SecondaryHand.AdditionalAttacks += Number * base.Fact.GetRank();
		}
	}

	public void OnEventDidTrigger(RuleCalculateAttacksCount evt)
	{
	}
}
