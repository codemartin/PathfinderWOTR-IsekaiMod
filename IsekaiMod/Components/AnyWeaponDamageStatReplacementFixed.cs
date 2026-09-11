using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components
{
	[AllowMultipleComponents]
	[ComponentName("Replace damage stat for weapon")]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	[TypeId("5701cdf3df1845a29ab74c21c3747747")]
	public class AnyWeaponDamageStatReplacementFixed : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
	{
		public StatType Stat;

		public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
		{
			if (evt.Initiator != base.Owner)
			{
				return;
			}
			ModifiableValueAttributeStat modifiableValueAttributeStat = (evt.DamageBonusStat.HasValue ? (evt.Initiator.Descriptor.Stats.GetStat(evt.DamageBonusStat.Value) as ModifiableValueAttributeStat) : null);
			if (evt.Initiator.Descriptor.Stats.GetStat(Stat) is ModifiableValueAttributeStat modifiableValueAttributeStat2 && (modifiableValueAttributeStat == null || modifiableValueAttributeStat2.Bonus > modifiableValueAttributeStat.Bonus))
			{
				evt.OverrideDamageBonusStat(Stat);
				evt.TwoHandedStatReplacement = true;
				if (Stat != StatType.Strength && Stat != StatType.Dexterity && (evt.Weapon.HoldInTwoHands || (evt.SlotToInsert != null && evt.Weapon.CanTakeTwoHands() && (evt.SlotToInsert.HandsEquipmentSet.GripType == GripType.TwoHanded || !evt.Weapon.CanTakeOneHand()))))
				{
					evt.OverrideDamageBonusStatMultiplier(1.5f);
				}
			}
		}

		public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
		{
		}
	}
}
