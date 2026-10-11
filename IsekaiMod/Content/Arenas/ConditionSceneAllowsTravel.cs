using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("4c01cc9d2dcd479a8795213a56ec4b6b")]
public class ConditionSceneAllowsTravel : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return ColiseumGates.CurrentRefusal() == ColiseumGates.TravelRefusal.None;
	}

	public override string GetConditionCaption()
	{
		return "The party may leave this scene for the Planar Coliseum or a boss site";
	}
}
