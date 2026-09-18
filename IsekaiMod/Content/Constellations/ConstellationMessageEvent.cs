using System;

namespace IsekaiMod.Content.Constellations
{
	public class ConstellationMessageEvent
	{
		public DateTime Timestamp { get; set; }

		public string Sponsor { get; set; }

		public ConstellationCategory Category { get; set; }

		public string Message { get; set; }

		public int CoinsAwarded { get; set; }

		public string SceneContext { get; set; }

		public ConstellationMessageEvent(string message, string sponsor, ConstellationCategory category, int coins = 0, string sceneContext = null)
		{
			Timestamp = DateTime.Now;
			Message = message ?? string.Empty;
			Sponsor = (string.IsNullOrEmpty(sponsor) ? "The Constellations" : sponsor);
			Category = category;
			CoinsAwarded = coins;
			SceneContext = sceneContext ?? string.Empty;
		}
	}
}
