using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("1fe09f2d47f9403b9b54122b313e8563")]
public class ConditionInColiseumArenaAct : IsekaiCondition
{
	public int Act;

	protected override bool CheckSafely()
	{
		return PlanarColiseumArea.CurrentArenaAct() == Act;
	}

	public override string GetConditionCaption()
	{
		return "The party stands in the Planar Coliseum's Act " + Act + " arena";
	}
}
