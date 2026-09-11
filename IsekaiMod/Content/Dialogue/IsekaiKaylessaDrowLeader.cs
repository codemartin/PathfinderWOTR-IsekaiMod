using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiKaylessaDrowLeader
	{
		public static void Add()
		{
			BlueprintCue Cue_0067 = BlueprintTools.GetBlueprint<BlueprintCue>("a8cc736feec11024eb6a5d3dbcb69f5c");
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("b8e680587a76f064fac9a01034c02391");
			if (Cue_0067 != null && blueprint != null)
			{
				BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiKaylessaDrowLeader", delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) [Attack] \"A convincing disguise, but your monologue gave away your stance. Draw your steel and let us see if your blade matches your deceit.\"");
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { Cue_0067.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					blueprintAnswer.RequirePlotArmor();
				});
				blueprint.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
			}
		}
	}
}
