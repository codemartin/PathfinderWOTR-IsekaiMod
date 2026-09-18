using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace IsekaiMod.Content.Narrative
{
	public class SceneRewards
	{
		public int Gold { get; set; }

		public int CosmicCoins { get; set; }

		public string[] ItemGuids { get; set; }

		public BlueprintBuff Buff { get; set; }

		public string RecruitUnitGuid { get; set; }

		public int RecruitCount { get; set; }

		public string RecruitArmyName { get; set; }
	}
}
