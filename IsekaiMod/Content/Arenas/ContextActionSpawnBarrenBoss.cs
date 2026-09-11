using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Arenas
{
	public class ContextActionSpawnBarrenBoss : ContextAction
	{
		public BarrenBossType BossType;

		public override string GetCaption()
		{
			return $"Spawns Barren Area Boss: {BossType}";
		}

		public override void RunAction()
		{
			CosmicBarrenBosses.SpawnBoss(BossType);
		}
	}
}
