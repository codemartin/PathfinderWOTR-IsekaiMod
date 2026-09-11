namespace IsekaiMod.Content.Constellations
{
	public class PastCycleRecord
	{
		public int CycleNumber;

		public string EndingId = "";

		public string EndingTitle = "";

		public string CharacterName = "";

		public string Archetype = "";

		public string MythicPath = "";

		public string RomancedCompanion = "";

		public string CompletionDate = "";

		public string PrimaryLegacy = "";

		public string SecondaryLegacy = "";

		public string EndingAchieved
		{
			get
			{
				if (!string.IsNullOrEmpty(EndingTitle))
				{
					return EndingTitle;
				}
				return EndingId;
			}
		}
	}
}
