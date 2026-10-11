using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Quests;

[TypeId("38607665964f4d0f9372bf7bbbd97395")]
public class ContextActionSummonRiftBoss : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Summons the next Dimensional Rift Boss";
	}

	protected override void RunSafely()
	{
		DimensionalRiftIncursions.SpawnRiftBoss();
	}
}
