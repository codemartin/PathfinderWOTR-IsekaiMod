using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[ComponentName("Phantom Flurry attacks")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[TypeId("5be9897e92ca40378252aec55664e554")]
public class PhantomFlurryAttacks : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, ISubscriber, IInitiatorRulebookSubscriber
{
	internal const int FullBonusRanks = 2;

	internal const int MaxRanks = 4;

	internal static int FullBonusAttacks(int rank)
	{
		return Math.Max(0, Math.Min(rank, 2));
	}

	internal static int PenalizedAttacks(int rank)
	{
		return Math.Max(0, Math.Min(rank, 4) - 2);
	}

	public void OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
	{
		if (evt == null || evt.Initiator != base.Owner)
		{
			return;
		}
		int rank = base.Fact?.GetRank() ?? 0;
		int num = FullBonusAttacks(rank);
		int num2 = PenalizedAttacks(rank);
		if (num <= 0 && num2 <= 0)
		{
			return;
		}
		if (num > 0)
		{
			evt.AddExtraAttacks(num, haste: false, penalized: false);
		}
		if (num2 > 0)
		{
			evt.AddExtraAttacks(num2, haste: false, penalized: true);
		}
		if (AddExtraOffHandAttack.WieldsTwoWeapons(evt))
		{
			UnitBody body = evt.Initiator.Body;
			if (body.PrimaryHand.MaybeWeapon != body.SecondaryHand.MaybeWeapon)
			{
				evt.Result.SecondaryHand.AdditionalAttacks += num;
				evt.Result.SecondaryHand.PenalizedAttacks += num2;
			}
		}
	}

	public void OnEventDidTrigger(RuleCalculateAttacksCount evt)
	{
	}
}
