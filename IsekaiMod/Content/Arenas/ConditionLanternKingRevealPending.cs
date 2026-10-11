using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("4f1c2b8e9a6d4e57b3c0d8a1f2e6b947")]
public class ConditionLanternKingRevealPending : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return LanternKingReveal.IsRevealPending(ColiseumProgress.Find());
	}

	public override string GetConditionCaption()
	{
		return "The Lantern King has yet to reveal himself in this save";
	}
}
