using System.Collections.Generic;

namespace IsekaiMod.Content.Constellations;

public class TimelineData
{
	public int TotalRuns = 1;

	public int DeathCount;

	public int CosmicCoins;

	public bool RecentDeath;

	public string LastArea = "";

	public string RecentDeathGameId = "";

	public List<string> RunOwners = new List<string>();

	public List<string> LoopEndReasons = new List<string>();

	public bool LoopShattered;

	public bool LoopBreakerAchieved;

	public bool ShardRelicGranted;

	public List<string> ShardGrantedGames = new List<string>();

	public string ActiveRunArchetype = "";

	public List<string> RunArchetypes = new List<string>();

	public int GachaPityCount;

	public List<string> ActiveBarrenBosses = new List<string>();

	public List<string> DefeatedBarrenBosses = new List<string>();

	public string ActiveArenaSaveId = "";

	public string ActiveArenaCup = "";

	public int ActiveArenaWave;

	public List<string> ActiveArenaEnemies = new List<string>();

	public List<string> ActiveArenaSpawned = new List<string>();

	public List<string> ReinforcedBosses = new List<string>();

	public List<string> ImperialTitheDays = new List<string>();

	public List<string> PastChoices = new List<string>();

	public List<string> PastMythicPaths = new List<string>();

	public List<string> PastLegacies = new List<string>();

	public List<string> ClearedEndings = new List<string>();

	public List<PastCycleRecord> PastCycles = new List<PastCycleRecord>();

	public List<string> ClosedCycles = new List<string>();

	public int LoopCount
	{
		get
		{
			return TotalRuns;
		}
		set
		{
			TotalRuns = value;
		}
	}
}
