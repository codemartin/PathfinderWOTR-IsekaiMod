using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("191995bf33b048248b78658a4a0eecad")]
public class ConditionPlanarColiseumEnabled : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true;
	}

	public override string GetConditionCaption()
	{
		return "The Planar Coliseum is enabled";
	}
}
