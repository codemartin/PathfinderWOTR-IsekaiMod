using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class IsekaiHorgus
{
	public static void Add()
	{
		BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("12e42316950f8c9498afb8b0fb2baaae");
		BlueprintAnswer blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswer>("3ab564082485b034a9d0a7b550e1a3e2");
		BlueprintAnswer blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswer>("df6f2843e88b0c746934b3ff68bc8008");
		BlueprintAnswer blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswer>("95fa78f8e532ea942a2f78750a183d18");
		BlueprintAnswer blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswer>("ecece59fae7527b478d1cc706ba7000a");
		BlueprintUnit blueprint6 = BlueprintTools.GetBlueprint<BlueprintUnit>("c02e641bf8cf0984fb49604afa224563");
		BlueprintUnlockableFlag Horgus_GetMeOut_price = BlueprintTools.GetBlueprint<BlueprintUnlockableFlag>("ddfedbdeab95ed941b6968b06162c921");
		if (blueprint == null || blueprint2 == null || blueprint3 == null || blueprint4 == null || blueprint5 == null || blueprint6 == null || Horgus_GetMeOut_price == null)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiHorgus: a vanilla blueprint was not found, skipping.");
			return;
		}
		BlueprintAnswer blueprintAnswer = GraftedAnswer.Named("IsekaiDialogueHorgus").At(DialogueBeats.HorgusNamesHisPrice).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"A thousand gold to escort a nobleman through monster-infested caverns? That barely covers hazard pay, Sir Gwerm. Make it two thousand and we have an agreement.\"")
			.OnSelect(new GainExp
			{
				Encounter = EncounterType.SkillCheck,
				CR = 2,
				Modifier = 1f
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.RepliedToBy("IsekaiDialogueHorgusReply", "\"Hazard pay?!\" {n}Horgus scowls, his mustache twitching with indignation.{/n} \"Such impudence from a stranger! But fine... seeing as we are trapped under a collapsed city, two thousand gold it is. Just keep those subterranean beasts away from us!\"")
			.SpokenBy(blueprint6)
			.ConfigureReply(delegate(GraftedCue reply)
			{
				reply.OnStop(new UnlockFlag
				{
					m_flag = Horgus_GetMeOut_price.ToReference<BlueprintUnlockableFlagReference>(),
					flagValue = 2000
				});
			})
			.ThenBackToTheList()
			.Graft();
		if (blueprintAnswer != null)
		{
			BlueprintAnswer[] array = new BlueprintAnswer[4] { blueprint2, blueprint3, blueprint4, blueprint5 };
			foreach (BlueprintAnswer right in array)
			{
				ExtensionMethods.HideEachOther(blueprintAnswer, right);
			}
		}
	}
}
