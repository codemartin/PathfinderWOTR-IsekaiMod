using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("e91b6d2f3a4c4f0b8d7e5a6c1b2f3d49")]
public class ConditionLanternKingDiscountAnnouncementPending : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return LanternKingReveal.IsDiscountAnnouncementPending(ColiseumProgress.Find(), LanternKingReveal.StolenLandsAvailableSafe());
	}

	public override string GetConditionCaption()
	{
		return "Every coliseum cup is won and the Lantern King has not announced his discount";
	}
}
