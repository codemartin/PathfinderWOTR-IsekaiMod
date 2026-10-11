using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("9f5be09cb3bd4b2ea39ef1ff2c1f6a73")]
public class ConditionWarCampLevyStillOwed : IsekaiCondition
{
	public string ClaimKey = "WarCampLevy";

	protected override bool CheckSafely()
	{
		return ContextActionMobilizeSubclassArmy.LevyStillOwed(ConstellationChatManager.SceneRewardClaimed(ClaimKey), ContextActionMobilizeSubclassArmy.LastPickClaimed, ClaimKey);
	}

	public override string GetConditionCaption()
	{
		return "Checks whether the crusade has yet to take its levy of Isekai volunteers";
	}
}
