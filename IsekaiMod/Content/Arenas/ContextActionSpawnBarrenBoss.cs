using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("d3b788c33512436c9f0f48dd29542a65")]
public class ContextActionSpawnBarrenBoss : IsekaiContextAction
{
	public BarrenBossType BossType;

	public override string GetCaption()
	{
		return $"Spawns Barren Area Boss: {BossType}";
	}

	protected override void RunSafely()
	{
		CosmicBarrenBosses.SpawnBoss(BossType);
	}
}
