using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiHorgus
	{
		public static void Add()
		{
			BlueprintAnswersList AnswersList_0009 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("12e42316950f8c9498afb8b0fb2baaae");
			BlueprintAnswer Answer_0011 = BlueprintTools.GetBlueprint<BlueprintAnswer>("3ab564082485b034a9d0a7b550e1a3e2");
			BlueprintUnit Horgus = BlueprintTools.GetBlueprint<BlueprintUnit>("c02e641bf8cf0984fb49604afa224563");
			BlueprintUnlockableFlag Horgus_GetMeOut_price = BlueprintTools.GetBlueprint<BlueprintUnlockableFlag>("ddfedbdeab95ed941b6968b06162c921");
			if (AnswersList_0009 == null || Answer_0011 == null || Horgus == null || Horgus_GetMeOut_price == null)
			{
				return;
			}
			BlueprintCue IsekaiDialogueHorgusReply = TTCoreExtensions.CreateCue("IsekaiDialogueHorgusReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "\"Hazard pay?!\" {n}Horgus scowls, his mustache twitching with indignation as he glances at Camellia.{/n} \"Such impudence from a stranger! But fine... seeing as we are trapped under a collapsed city, two thousand gold it is. Just keep those subterranean beasts away from us!\"");
				blueprintCue.Speaker = new DialogSpeaker
				{
					m_Blueprint = Horgus.ToReference<BlueprintUnitReference>(),
					MoveCamera = true
				};
				blueprintCue.OnStop = ActionFlow.DoSingle(delegate(UnlockFlag c)
				{
					c.m_flag = Horgus_GetMeOut_price.ToReference<BlueprintUnlockableFlagReference>();
					c.flagValue = 2000;
				});
				blueprintCue.Answers = AnswersList_0009.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiDialogueHorgus", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Two thousand gold to escort a high nobleman through monster-infested caverns? That barely covers hazard pay, Sir Gwerm, but considering your daughter's safety is on the line, we have an agreement.\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { IsekaiDialogueHorgusReply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.OnSelect = ActionFlow.DoSingle(delegate(GainExp c)
				{
					c.Encounter = EncounterType.SkillCheck;
					c.CR = 2;
					c.Modifier = 1f;
				});
				blueprintAnswer.AddShowCondition(delegate(AnswerSelected c)
				{
					c.Not = true;
					c.m_Answer = Answer_0011.ToReference<BlueprintAnswerReference>();
				});
				blueprintAnswer.RequirePlotArmor();
			});
			Answer_0011.ShowConditions.Conditions = Answer_0011.ShowConditions.Conditions.AppendToArray(new AnswerSelected
			{
				Not = true,
				m_Answer = bp.ToReference<BlueprintAnswerReference>()
			});
			AnswersList_0009.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}
	}
}
