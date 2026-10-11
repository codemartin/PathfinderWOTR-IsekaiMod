using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("db64a564576f49a3b42fe8645ec23625")]
public class ConditionPartyIsInPlanarColiseum : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return PlanarColiseumHubManager.IsInColiseum();
	}

	public override string GetConditionCaption()
	{
		return "The party is standing in the Planar Coliseum";
	}
}
