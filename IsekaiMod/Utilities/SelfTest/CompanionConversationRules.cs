namespace IsekaiMod.Utilities.SelfTest;

internal static class CompanionConversationRules
{
	internal sealed class Probe
	{
		public bool ConditionsPass;

		public bool FirstCueFound;

		public bool FirstCueIsPlainCue;

		public bool FirstCueHasText;

		public bool Continues;

		public int SelectableAnswers;

		public int SkippedShowChecks;
	}

	internal static string Problem(Probe probe)
	{
		if (probe == null)
		{
			return "the conversation could not be played";
		}
		if (!probe.ConditionsPass)
		{
			return null;
		}
		if (!probe.FirstCueFound)
		{
			return "no first cue can show, so clicking the companion opens nothing";
		}
		if (!probe.FirstCueIsPlainCue)
		{
			return null;
		}
		if (!probe.FirstCueHasText)
		{
			return "the first cue has no text, so the window comes up blank";
		}
		if (probe.Continues)
		{
			return null;
		}
		if (probe.SelectableAnswers == 0 && probe.SkippedShowChecks == 0)
		{
			return "the first cue offers no answer the player can pick, so the window holds a greeting and nothing but Exit";
		}
		return null;
	}
}
