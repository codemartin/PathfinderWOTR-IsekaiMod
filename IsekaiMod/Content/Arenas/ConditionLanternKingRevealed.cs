using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("a7d3e59c1b2f4c68905e7b4d3c2a1f80")]
public class ConditionLanternKingRevealed : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return LanternKingReveal.IsRevealed(ColiseumProgress.Find());
	}

	public override string GetConditionCaption()
	{
		return "The Lantern King has revealed himself in this save";
	}
}
