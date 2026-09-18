using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace IsekaiMod.Content.Constellations
{
	public class ConstellationBounty
	{
		public string Id { get; set; }

		public string Title { get; set; }

		public string Sponsor { get; set; }

		public string AnnouncementText { get; set; }

		public string SuperChatText { get; set; }

		public int CoinReward { get; set; }

		public BlueprintBuff BuffReward { get; set; }

		public string MilestoneKey { get; set; }
	}
}
