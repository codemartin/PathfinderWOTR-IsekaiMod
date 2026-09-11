using System.Collections.Generic;

namespace IsekaiMod.Content.Constellations
{
	public class TimelineData
	{
		public int TotalRuns = 1;

		public int DeathCount;

		public bool RecentDeath;

		public string LastArea = "";

		public bool LoopShattered;

		public bool LoopBreakerAchieved;

		public bool ShardRelicGranted;

		public bool ArenaCompleted;

		public bool GrandArbiterDefeated;

		public string ActiveRunArchetype = "";

		public int GachaPityCount;

		public List<string> CompletedSideQuests = new List<string>();

		public List<string> PastChoices = new List<string>();

		public List<string> PastMythicPaths = new List<string>();

		public List<string> ClearedEndings = new List<string>();

		public List<string> PastLegacies = new List<string>();

		public List<PastCycleRecord> PastCycles = new List<PastCycleRecord>();

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
}
