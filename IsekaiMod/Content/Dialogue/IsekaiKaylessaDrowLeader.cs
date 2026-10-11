using IsekaiMod.Utilities;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class IsekaiKaylessaDrowLeader
{
	public static void Add()
	{
		BlueprintCue blueprint = BlueprintTools.GetBlueprint<BlueprintCue>("a8cc736feec11024eb6a5d3dbcb69f5c");
		BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("b8e680587a76f064fac9a01034c02391");
		if (blueprint == null || blueprint2 == null)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiKaylessaDrowLeader: a vanilla blueprint was not found, skipping.");
			return;
		}
		GraftedAnswer.Named("IsekaiKaylessaDrowLeader").At(DialogueBeats.DrowDisguisesDrop).Repeatable()
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequireIsekaiClass();
			})
			.Says("(Isekai Protagonist) [Attack] \"A convincing disguise, but the masquerade is over. Draw your steel and let us see if your blade matches your deceit.\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.ContinuesStraightTo(blueprint)
			.Graft();
	}
}
