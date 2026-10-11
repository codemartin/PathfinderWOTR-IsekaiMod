using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("dfdfafd516964e579ce1c996034a033c")]
public class ContextActionReturnToGolarion : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Return to Golarion from the Planar Coliseum";
	}

	protected override void RunSafely()
	{
		PlanarColiseumHubManager.ReturnToGolarion();
	}
}
