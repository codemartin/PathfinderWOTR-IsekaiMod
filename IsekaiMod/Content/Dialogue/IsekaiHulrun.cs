using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiHulrun
	{
		public static void Add()
		{
			BlueprintCue DontRememberCue = BlueprintTools.GetBlueprint<BlueprintCue>("ba9c82193a32275408973a8aebdb3a6d");
			BlueprintEtude DontRememberEtude = BlueprintTools.GetBlueprint<BlueprintEtude>("d6c6161d2cf0ac44786f9df67fca5ce9");
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e27807b731f3b1a4eb19c1a04fdfcf53");
			if (blueprint == null || DontRememberCue == null || DontRememberEtude == null)
			{
				return;
			}
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiDialogueHulrun", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Other than the blinding lights of an out-of-control vehicle and waking up on these cobblestones, my memories before the festival are completely hazy...\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { DontRememberCue.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.OnSelect = ActionFlow.DoSingle(delegate(StartEtude c)
				{
					c.Etude = DontRememberEtude.ToReference<BlueprintEtudeReference>();
					c.Evaluate = false;
				});
				blueprintAnswer.RequirePlotArmor();
			});
			blueprint.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}
	}
}
