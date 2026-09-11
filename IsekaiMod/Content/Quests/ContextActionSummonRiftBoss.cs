using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Quests
{
	public class ContextActionSummonRiftBoss : ContextAction
	{
		public int Act;

		public override string GetCaption()
		{
			return $"Summons Act {Act} Dimensional Rift Boss";
		}

		public override void RunAction()
		{
			DimensionalRiftIncursions.SpawnRiftBoss(Act);
		}
	}
}
