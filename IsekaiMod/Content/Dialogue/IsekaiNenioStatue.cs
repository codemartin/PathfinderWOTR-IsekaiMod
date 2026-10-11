using IsekaiMod.Utilities;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class IsekaiNenioStatue
{
	public static void Add()
	{
		BlueprintCue blueprint = BlueprintTools.GetBlueprint<BlueprintCue>("1701cb6cba55ed04cac7908e072563ac");
		BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("db068bf5b388cce4c9f828d389ca537d");
		if (blueprint == null || blueprint2 == null)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiNenioStatue: a vanilla blueprint was not found, skipping.");
			return;
		}
		GraftedCue IsekaiDialogueNenioStatueReply = GraftedCue.Named("IsekaiDialogueNenioStatueReply").Says("{n}The emptiness is bewildered, yet it cannot deny that what you say is the truth.{/n}").Narrated()
			.ThenContinueTo(blueprint);
		AddNenioAnswer("IsekaiDialogueNenioStatueAnswer1", "(Isekai Protagonist) \"I am the bone of my sword.\"");
		AddNenioAnswer("IsekaiDialogueNenioStatueAnswer2", "(Isekai Protagonist) \"I am the hope of the universe.\"");
		AddNenioAnswer("IsekaiDialogueNenioStatueAnswer3", "(Isekai Protagonist) \"I am just a guy who's a hero for fun.\"");
		AddNenioAnswer("IsekaiDialogueNenioStatueAnswer4", "(Isekai Protagonist) \"I am Atomic.\"");
		AddNenioAnswer("IsekaiDialogueNenioStatueAnswer5", "(Isekai Protagonist) \"I am a God.\"");
		AddNenioAnswer("IsekaiDialogueNenioStatueAnswer6", "(Isekai Protagonist) \"I am a wanderer cast across the rift between worlds, forging my own fate.\"");
		void AddNenioAnswer(string name, string text)
		{
			GraftedAnswer.Named(name).At(DialogueBeats.EmptinessAsksWhoYouAre).Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequireIsekaiClass();
			})
				.Says(text)
				.Configure(delegate(BlueprintAnswer bp)
				{
					bp.RequirePlotArmor();
				})
				.RepliedWith(IsekaiDialogueNenioStatueReply)
				.Graft();
		}
	}
}
