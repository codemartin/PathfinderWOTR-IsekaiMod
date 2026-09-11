using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiNenioStatue
	{
		public static void Add()
		{
			BlueprintCue BeholdTheTruthCue = BlueprintTools.GetBlueprint<BlueprintCue>("1701cb6cba55ed04cac7908e072563ac");
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("db068bf5b388cce4c9f828d389ca537d");
			if (blueprint != null && BeholdTheTruthCue != null)
			{
				BlueprintCue IsekaiDialogueNenioStatueReply = TTCoreExtensions.CreateCue("IsekaiDialogueNenioStatueReply", delegate(BlueprintCue blueprintCue)
				{
					blueprintCue.SetText(Main.IsekaiContext, "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}");
					blueprintCue.Continue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { BeholdTheTruthCue.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
				});
				BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiDialogueNenioStatueAnswer1", delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"I am the bone of my sword.\"");
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueNenioStatueReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintAnswer.ShowOnce = true;
					blueprintAnswer.RequirePlotArmor();
				});
				BlueprintAnswer bp2 = TTCoreExtensions.CreateAnswer("IsekaiDialogueNenioStatueAnswer2", delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"I am the hope of the universe.\"");
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueNenioStatueReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintAnswer.ShowOnce = true;
					blueprintAnswer.RequirePlotArmor();
				});
				BlueprintAnswer bp3 = TTCoreExtensions.CreateAnswer("IsekaiDialogueNenioStatueAnswer3", delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"I am just a guy who's a hero for fun.\"");
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueNenioStatueReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintAnswer.ShowOnce = true;
					blueprintAnswer.RequirePlotArmor();
				});
				BlueprintAnswer bp4 = TTCoreExtensions.CreateAnswer("IsekaiDialogueNenioStatueAnswer4", delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"I am Atomic.\"");
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueNenioStatueReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintAnswer.ShowOnce = true;
					blueprintAnswer.RequirePlotArmor();
				});
				BlueprintAnswer bp5 = TTCoreExtensions.CreateAnswer("IsekaiDialogueNenioStatueAnswer5", delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"I am a God.\"");
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueNenioStatueReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintAnswer.ShowOnce = true;
					blueprintAnswer.RequirePlotArmor();
				});
				BlueprintAnswer bp6 = TTCoreExtensions.CreateAnswer("IsekaiDialogueNenioStatueAnswer6", delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"I am a wanderer cast across the rift between worlds, forging my own fate.\"");
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueNenioStatueReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintAnswer.ShowOnce = true;
					blueprintAnswer.RequirePlotArmor();
				});
				blueprint.Answers.Insert(0, bp6.ToReference<BlueprintAnswerBaseReference>());
				blueprint.Answers.Insert(0, bp5.ToReference<BlueprintAnswerBaseReference>());
				blueprint.Answers.Insert(0, bp4.ToReference<BlueprintAnswerBaseReference>());
				blueprint.Answers.Insert(0, bp3.ToReference<BlueprintAnswerBaseReference>());
				blueprint.Answers.Insert(0, bp2.ToReference<BlueprintAnswerBaseReference>());
				blueprint.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
			}
		}
	}
}
