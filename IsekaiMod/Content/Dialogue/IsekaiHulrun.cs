using IsekaiMod.Utilities;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class IsekaiHulrun
{
	public static void Add()
	{
		BlueprintCue blueprint = BlueprintTools.GetBlueprint<BlueprintCue>("ba9c82193a32275408973a8aebdb3a6d");
		BlueprintEtude blueprint2 = BlueprintTools.GetBlueprint<BlueprintEtude>("d6c6161d2cf0ac44786f9df67fca5ce9");
		BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e27807b731f3b1a4eb19c1a04fdfcf53");
		if (blueprint == null || blueprint2 == null || blueprint3 == null)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiHulrun: a vanilla blueprint was not found, skipping.");
			return;
		}
		GraftedAnswer.Named("IsekaiDialogueHulrun").At(DialogueBeats.HulrunAsksWhoYouAre).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Other than the blinding lights of an out-of-control vehicle and waking up on these cobblestones, my memories before the festival are completely hazy...\"")
			.OnSelect(new StartEtude
			{
				Etude = blueprint2.ToReference<BlueprintEtudeReference>(),
				Evaluate = false
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.ShownWhen(new HasFact
			{
				Unit = new PlayerCharacter(),
				m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "IsekaiProficiencies")
			})
			.ContinuesStraightTo(blueprint)
			.Graft();
	}
}
