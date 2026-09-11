using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Arenas
{
	public class ContextActionStartDimensionalArena : ContextAction
	{
		public ArenaType Arena;

		public override string GetCaption()
		{
			return $"Starts Dimensional Arena: {Arena}";
		}

		public override void RunAction()
		{
			DimensionalArenaManager.StartArena(Arena);
		}
	}
}
