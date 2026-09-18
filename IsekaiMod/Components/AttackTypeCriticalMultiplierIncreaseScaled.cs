using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components
{
	[ComponentName("Scaled Critical Multiplier Increase")]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	[TypeId("6d34e10b2848411b98f2441ca4615a13")]
	public class AttackTypeCriticalMultiplierIncreaseScaled : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
	{
		public WeaponRangeType Type;

		public int AdditionalMultiplier = 1;

		public int MinCharacterLevel = 10;

		public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
		{
			if (evt != null && !(evt.Initiator != base.Owner) && (base.Owner?.Progression?.CharacterLevel ?? 1) >= MinCharacterLevel && evt.Weapon != null && Type.IsSuitableWeapon(evt.Weapon))
			{
				evt.AdditionalCriticalMultiplier.Add(new Modifier(AdditionalMultiplier, base.Fact, ModifierDescriptor.None));
			}
		}

		public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
		{
		}
	}
}
