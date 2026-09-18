using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components
{
	[ComponentName("Add Extra Off-Hand Attack")]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	[TypeId("327faf39b9a643c2bbca12c7e8f81ccb")]
	public class AddExtraOffHandAttack : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, ISubscriber, IInitiatorRulebookSubscriber
	{
		public int Number = 1;

		public int MinCharacterLevel = 1;

		public void OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
		{
			if (!(evt.Initiator != base.Owner) && evt.Initiator?.Body != null && (evt.Initiator.Progression?.CharacterLevel ?? 1) >= MinCharacterLevel)
			{
				HandSlot primaryHand = evt.Initiator.Body.PrimaryHand;
				HandSlot secondaryHand = evt.Initiator.Body.SecondaryHand;
				if (primaryHand != null && secondaryHand != null && primaryHand.HasWeapon && secondaryHand.HasWeapon && primaryHand.Weapon?.Blueprint != null && secondaryHand.Weapon?.Blueprint != null && !primaryHand.Weapon.Blueprint.IsNatural && !secondaryHand.Weapon.Blueprint.IsNatural && primaryHand.Weapon != evt.Initiator.Body.EmptyHandWeapon && secondaryHand.Weapon != evt.Initiator.Body.EmptyHandWeapon && evt.Result?.SecondaryHand != null)
				{
					evt.Result.SecondaryHand.AdditionalAttacks += Number * (base.Fact?.GetRank() ?? 1);
				}
			}
		}

		public void OnEventDidTrigger(RuleCalculateAttacksCount evt)
		{
		}
	}
}
