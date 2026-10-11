using IsekaiMod.Content.KingmakerIntegration;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("0d6e4b2a8c9f4a1e7b3d5c6f8e2a1b90")]
public class ConditionLanternKingKnowsYourRuler : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return KingmakerCrossSaveManager.IsKingmakerInstalled;
	}

	public override string GetConditionCaption()
	{
		return "Pathfinder: Kingmaker is detected";
	}
}
