using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("b3a7a6f648db423c94a79e76bd96cd4b")]
public class ActionReturnToGolarion : IsekaiGameAction
{
	public override string GetCaption()
	{
		return "Return to Golarion";
	}

	protected override void RunSafely()
	{
		PlanarColiseumHubManager.ReturnToGolarion();
	}
}
