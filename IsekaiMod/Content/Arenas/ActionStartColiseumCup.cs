using Kingmaker.ElementsSystem;

namespace IsekaiMod.Content.Arenas
{
	public class ActionStartColiseumCup : GameAction
	{
		public ArenaType Cup;

		public override string GetCaption()
		{
			return $"Start Planar Coliseum Cup: {Cup}";
		}

		public override void RunAction()
		{
			DimensionalArenaManager.StartArena(Cup);
		}
	}
}
