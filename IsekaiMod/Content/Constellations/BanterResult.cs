using System.Collections.Generic;

namespace IsekaiMod.Content.Constellations
{
	public class BanterResult
	{
		public List<string> Lines { get; set; }

		public int Coins { get; set; }

		public string Sponsor { get; set; }

		public ConstellationCategory Category { get; set; }

		public string SceneContext { get; set; }

		public BanterResult(List<string> lines, int coins, string sponsor, string sceneContext, ConstellationCategory category = ConstellationCategory.Quest)
		{
			Lines = lines ?? new List<string>();
			Coins = coins;
			Sponsor = sponsor;
			SceneContext = sceneContext;
			Category = category;
		}
	}
}
