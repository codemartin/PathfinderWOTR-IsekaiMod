using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class IsekaiMinagho
{
	public static void Add()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("e778129f817a5fa4286e64b061df84a5");
		BlueprintUnit blueprint2 = BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765");
		BlueprintUnit blueprint3 = BlueprintTools.GetBlueprint<BlueprintUnit>("397b090721c41044ea3220445300e1b8");
		BlueprintCue blueprint4 = BlueprintTools.GetBlueprint<BlueprintCue>("3bd9a4263d8064b49a9d1eec365807b9");
		BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("5acd8001d9f7d2443bd57fb1291a03e4");
		if (blueprint == null || blueprint2 == null || blueprint3 == null || blueprint4 == null || blueprint5 == null)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiMinagho: a vanilla blueprint was not found, skipping.");
			return;
		}
		BlueprintCue blueprint6 = GraftedCue.Named("IsekaiDialogueMinaghoSeelahReply").Says("{n}Seelah grips her longsword with renewed conviction, nodding firmly at your resolve.{/n} \"Well said! Kenabres will not bend to you again, demon!\"").SpokenBy(blueprint2)
			.ThenContinueTo(blueprint4)
			.Blueprint;
		BlueprintCue blueprint7 = GraftedCue.Named("IsekaiDialogueMinaghoCamelliaReply").Says("{n}Camellia tilts her head, giving a quiet, amused smirk behind her buckler.{/n} \"Such bold words... let us see if your blade can back them up.\"").SpokenBy(blueprint3)
			.ThenContinueTo(blueprint6, blueprint4)
			.Blueprint;
		BlueprintCue blueprint8 = GraftedCue.Named("IsekaiDialogueMinaghoIrabethReply").Says("{n}Irabeth straightens her bruised shoulders, drawing courage from your defiance.{/n} \"Our mysterious ally is right, Minagho. Your reign of terror ends tonight!\"").SpokenBy(blueprint)
			.Configure(delegate(BlueprintCue bp)
			{
				bp.TurnSpeaker = false;
			})
			.ThenContinueTo(blueprint7, blueprint6, blueprint4)
			.Blueprint;
		GraftedAnswer.Named("IsekaiDialogueMinagho").At(DialogueBeats.IrabethNamesMinagho).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"So you are the infamous commander behind Kenabres's slaughter. You look remarkably complacent for someone whose army is about to be driven out of the garrison.\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.ShownWhen(new HasFact
			{
				Unit = new PlayerCharacter(),
				m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "IsekaiProficiencies")
			})
			.ContinuesStraightTo(blueprint8, blueprint7, blueprint6, blueprint4)
			.Graft();
	}
}
