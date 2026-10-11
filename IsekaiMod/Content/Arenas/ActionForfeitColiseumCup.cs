using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("3f74ba10b7b8450c897df0655d07b3f6")]
public class ActionForfeitColiseumCup : IsekaiGameAction
{
	public override string GetCaption()
	{
		return "Forfeit the Planar Coliseum trial in progress";
	}

	protected override void RunSafely()
	{
		DimensionalArenaManager.ForfeitArena();
	}
}
