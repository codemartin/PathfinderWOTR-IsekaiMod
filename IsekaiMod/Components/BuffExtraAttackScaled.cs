using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components
{
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	[TypeId("2b370d88ab7b485d927cb7a53b9b471a")]
	public class BuffExtraAttackScaled : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, ISubscriber, IInitiatorRulebookSubscriber
	{
		public int Number = 1;

		public bool Haste;

		public bool Penalized;

		public int MinCharacterLevel = 10;

		public void OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
		{
			if (evt != null && !(evt.Initiator != base.Owner) && (base.Context?.MaybeCaster?.Descriptor?.Progression?.CharacterLevel ?? base.Owner?.Progression?.CharacterLevel ?? 1) >= MinCharacterLevel)
			{
				evt.AddExtraAttacks(Number, Haste, Penalized);
			}
		}

		public void OnEventDidTrigger(RuleCalculateAttacksCount evt)
		{
		}
	}
}
