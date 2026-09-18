namespace IsekaiMod.Content.Constellations
{
	public readonly struct DialogueContext
	{
		public readonly string PlayerDeity;

		public readonly bool HasAvatar;

		public readonly int Cycle;

		public readonly string DialogName;

		public readonly string AnswerText;

		public readonly string AnswerGuid;

		public readonly string AnswerName;

		public readonly string SpeakerName;

		public readonly string AreaName;

		public DialogueContext(string playerDeity, bool hasAvatar, int cycle, string dialogName, string answerText, string answerGuid, string answerName, string speakerName, string areaName)
		{
			PlayerDeity = playerDeity;
			HasAvatar = hasAvatar;
			Cycle = cycle;
			DialogName = dialogName;
			AnswerText = answerText;
			AnswerGuid = answerGuid;
			AnswerName = answerName;
			SpeakerName = speakerName;
			AreaName = areaName;
		}
	}
}
