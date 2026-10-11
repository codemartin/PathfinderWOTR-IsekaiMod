using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class IsekaiFinnean
{
	public static void Add()
	{
		AddExoticFormsDialogue();
	}

	private static void AddExoticFormsDialogue()
	{
		BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("615ef80243cfc184ba42286395880b0e");
		BlueprintUnitReference blueprintReference = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>("ea8034769ab7d584e97b5227cbc03296");
		if (blueprint == null)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiFinnean: AnswersList_0003 was not found, skipping.");
			return;
		}
		GraftedAnswer.Named("IsekaiDialogueFinnean").At(DialogueBeats.FinneanHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"A sentient soul bound within an ever-shifting armament? You are essentially an enchanted legendary relic. Can you take the shape of other exotic weapon forms?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.RepliedToBy("IsekaiDialogueFinneanReply", "\"Exotic forms? Well, I try my best! Swords, bows, polearms... you name it, and I'll do everything I can to keep you alive out there, partner!\"")
			.SpokenWithPortraitOf(blueprintReference)
			.ThenBackToTheList()
			.Graft();
	}
}
