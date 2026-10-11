using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("c2e8f4a61d7b4b3a8e5f90c1d6a7b2e4")]
public class ConditionLanternKingPrizeAvailable : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return LanternKingReveal.IsPrizeAvailable(ColiseumProgress.Find());
	}

	public override string GetConditionCaption()
	{
		return "The Lantern King's prize is still unclaimed";
	}
}
