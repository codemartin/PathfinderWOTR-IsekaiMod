using IsekaiMod.Utilities;
using Kingmaker.DialogSystem.Blueprints;

namespace IsekaiMod.Content.Dialogue
{
	// 6.3 folded the old per-NPC dialogue classes (Hulrun, Horgus, Minagho, Finnean, Nenio's statue, Kaylessa)
	// into the reaction and narrative layers and stopped creating their answer and cue blueprints. Saves made on
	// 6.2 remember those answers in the dialogue history, and the loader refuses the save when a remembered
	// blueprint no longer exists. These stubs keep the old names alive under their registered GUIDs. Nothing
	// links to them, so they never show in a conversation.
	internal static class LegacyDialogueStubs
	{
		private static readonly string[] Answers = new string[]
		{
			"IsekaiDialogueHulrun",
			"IsekaiDialogueHorgus",
			"IsekaiDialogueMinagho",
			"IsekaiDialogueFinnean",
			"IsekaiDialogueNenioStatueAnswer1",
			"IsekaiDialogueNenioStatueAnswer2",
			"IsekaiDialogueNenioStatueAnswer3",
			"IsekaiDialogueNenioStatueAnswer4",
			"IsekaiDialogueNenioStatueAnswer5",
			"IsekaiDialogueNenioStatueAnswer6",
			"IsekaiKaylessaDrowLeader"
		};

		private static readonly string[] Cues = new string[]
		{
			"IsekaiDialogueHorgusReply",
			"IsekaiDialogueMinaghoCamelliaReply",
			"IsekaiDialogueMinaghoIrabethReply",
			"IsekaiDialogueMinaghoSeelahReply",
			"IsekaiDialogueFinneanReply",
			"IsekaiDialogueNenioStatueReply"
		};

		public static void Add()
		{
			foreach (string name in Answers)
			{
				TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
				{
					bp.SetText(Main.IsekaiContext, name);
					bp.ShowConditions = ActionFlow.IfAll(new Kingmaker.Designers.EventConditionActionSystem.Conditions.False());
				});
			}
			foreach (string name in Cues)
			{
				TTCoreExtensions.CreateCue(name, delegate(BlueprintCue bp)
				{
					bp.SetText(Main.IsekaiContext, name);
				});
			}
		}
	}
}
