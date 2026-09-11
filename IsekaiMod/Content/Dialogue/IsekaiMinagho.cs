using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiMinagho
	{
		public static void Add()
		{
			BlueprintUnit IrabethTirabladeGG = BlueprintTools.GetBlueprint<BlueprintUnit>("e778129f817a5fa4286e64b061df84a5");
			BlueprintUnit Seelah = BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765");
			BlueprintUnit Camellia = BlueprintTools.GetBlueprint<BlueprintUnit>("397b090721c41044ea3220445300e1b8");
			BlueprintCue ThatsNotVeryNiceCue = BlueprintTools.GetBlueprint<BlueprintCue>("3bd9a4263d8064b49a9d1eec365807b9");
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("5acd8001d9f7d2443bd57fb1291a03e4");
			if (blueprint != null && IrabethTirabladeGG != null && Seelah != null && Camellia != null && ThatsNotVeryNiceCue != null)
			{
				BlueprintCue IsekaiDialogueMinaghoSeelahReply = TTCoreExtensions.CreateCue("IsekaiDialogueMinaghoSeelahReply", delegate(BlueprintCue blueprintCue)
				{
					blueprintCue.SetText(Main.IsekaiContext, "{n}Seelah grips her longsword with renewed conviction, nodding firmly at your resolve.{/n} \"Well said! Kenabres will not bend to you again, demon!\"");
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = Seelah.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
					blueprintCue.Continue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { ThatsNotVeryNiceCue.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
				});
				BlueprintCue IsekaiDialogueMinaghoCamelliaReply = TTCoreExtensions.CreateCue("IsekaiDialogueMinaghoCamelliaReply", delegate(BlueprintCue blueprintCue)
				{
					blueprintCue.SetText(Main.IsekaiContext, "{n}Camellia tilts her head, giving a quiet, amused smirk behind her buckler.{/n} \"Such bold words... let us see if your blade can back them up.\"");
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = Camellia.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
					blueprintCue.Continue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueMinaghoSeelahReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
				});
				BlueprintCue IsekaiDialogueMinaghoIrabethReply = TTCoreExtensions.CreateCue("IsekaiDialogueMinaghoIrabethReply", delegate(BlueprintCue blueprintCue)
				{
					blueprintCue.SetText(Main.IsekaiContext, "{n}Irabeth straightens her bruised shoulders, drawing courage from your defiance.{/n} \"The Commander is right, Minagho. Your reign of terror ends tonight!\"");
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = IrabethTirabladeGG.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
					blueprintCue.Continue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueMinaghoCamelliaReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintCue.TurnSpeaker = false;
				});
				BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiDialogueMinagho", delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"So you are the infamous commander behind Kenabres's slaughter. You look remarkably complacent for someone whose army is about to be driven out of the garrison.\"");
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueMinaghoIrabethReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintAnswer.RequirePlotArmor();
				});
				blueprint.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
			}
		}
	}
}
