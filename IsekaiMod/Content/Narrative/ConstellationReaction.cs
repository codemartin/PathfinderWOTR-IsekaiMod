using System.Collections.Generic;
using IsekaiMod.Content.Constellations;

namespace IsekaiMod.Content.Narrative
{
	public class ConstellationReaction
	{
		public string Sponsor { get; set; } = "The Observer";

		public ConstellationCategory Category { get; set; } = ConstellationCategory.Subclass;

		public List<string> Lines { get; set; } = new List<string>();

		public int CosmicCoins { get; set; }

		public string SceneContext { get; set; }

		public List<CycleVariation> CycleOverrides { get; set; } = new List<CycleVariation>();
	}
}
