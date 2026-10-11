using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("5b8a0d3e7c1f4e2a9b6d4c8e0f1a2b37")]
public class ConditionLanternKingDiscountEarned : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return LanternKingReveal.DiscountEarnedInSave();
	}

	public override string GetConditionCaption()
	{
		return "Every Planar Coliseum cup is won (the Lantern King's Avatar discount)";
	}
}
