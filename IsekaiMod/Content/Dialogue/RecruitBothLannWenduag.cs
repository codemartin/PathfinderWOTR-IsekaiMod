using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class RecruitBothLannWenduag
{
	private const string LannContinuationCueGuid = "f850c07546728ad43a037504e60eee88";

	private const string WenduagRecruitCueGuid = "4e9564474c2755e40b44944ed75d81f0";

	private const string WenduagUnitGuid = "ae766624c03058440a036de90a7f2009";

	private const string LannUnitGuid = "cb29621d99b902e4da6f5d232352fbda";

	private const string VenduagChosenEtudeGuid = "285ae03094c379045b7cb3106e3f8abf";

	public static void Add()
	{
		BlueprintCue blueprint = BlueprintTools.GetBlueprint<BlueprintCue>("f850c07546728ad43a037504e60eee88");
		if (blueprint == null || blueprint.OnStop == null || !blueprint.OnStop.HasActions)
		{
			Main.IsekaiContext.Logger.LogError("RecruitBothLannWenduag: vanilla cue f850c07546728ad43a037504e60eee88 or its OnStop actions were not found. Skipping rather than adding an answer that never recruits Lann or brings Sull.");
			return;
		}
		GameAction[] actions = BuildOutcome(blueprint);
		BlueprintCue blueprint2 = GraftedCue.Named("IsekaiRecruitBothLannCue").Says("{n}Lann glances at Wenduag, then lets out a long breath.{/n} \"Well, crazier plans have worked. Not many, mind you. Wait here, I'll fetch the Chief and the others. Somebody has to explain this to him, and it had better be me.\"").SpokenBy(BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda"))
			.OnStop(actions)
			.Blueprint;
		BlueprintCue blueprint3 = GraftedCue.Named("IsekaiRecruitBothLannCueNoSpeaker").Says("{n}Lann glances at Wenduag, then lets out a long breath.{/n} \"Well, crazier plans have worked. Not many, mind you. Wait here, I'll fetch the Chief and the others. Somebody has to explain this to him, and it had better be me.\"").Narrated()
			.OnStop(actions)
			.Blueprint;
		GraftedCue reply = GraftedCue.Named("IsekaiRecruitBothCue").Says("{n}Lann and Wenduag stare in utter shock, their bitter feud halted in its tracks by the sheer weight of your command.{/n}\n\n\"You... you cannot be serious,\" {n}Lann stammers, lowering his bow in disbelief.{/n} \"Us? Together?\"\n\n{n}Wenduag shivers, a mixture of predatory awe and subservience gleaming in her eyes.{/n} \"Power dictates the law of the caves. If you have the strength to bend us both to your will, {mf|master|mistress}... then neither of us can refuse.\"").ThenContinueTo(blueprint2, blueprint3);
		GraftedAnswer.Named("IsekaiRecruitBothLannWenduag").At(DialogueBeats.LannOrWenduag, DialogueBeats.LannOrWenduagDecision, DialogueBeats.WoundedWenduagOrLann).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) [Subjugate the Maze] \"I am taking you both. The underground belongs to me now, and anyone who argues will join Hosilla in the dirt.\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.RepliedWith(reply)
			.Graft();
	}

	private static GameAction[] BuildOutcome(BlueprintCue lannContinuation)
	{
		List<GameAction> list = new List<GameAction>(lannContinuation.OnStop.Actions);
		list.Add(BuildWenduagRecruit());
		if (BlueprintTools.GetBlueprint<BlueprintEtude>("285ae03094c379045b7cb3106e3f8abf") != null)
		{
			list.Add(new StartEtude
			{
				Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("285ae03094c379045b7cb3106e3f8abf")
			});
		}
		else
		{
			Main.IsekaiContext.Logger.LogError("RecruitBothLannWenduag: Venduag_Chosen 285ae03094c379045b7cb3106e3f8abf was not found; Wenduag is recruited without her prologue outcome marker.");
		}
		return list.ToArray();
	}

	private static GameAction BuildWenduagRecruit()
	{
		Recruit recruit = BlueprintTools.GetBlueprint<BlueprintCue>("4e9564474c2755e40b44944ed75d81f0")?.OnStop?.Actions?.OfType<Recruit>().FirstOrDefault();
		if (recruit != null)
		{
			return new Recruit
			{
				AddToParty = recruit.AddToParty,
				MatchPlayerXpExactly = recruit.MatchPlayerXpExactly,
				Recruited = recruit.Recruited,
				OnRecruit = Helpers.CreateActionList(),
				OnRecruitImmediate = Helpers.CreateActionList()
			};
		}
		Main.IsekaiContext.Logger.LogWarning("RecruitBothLannWenduag: could not read the Recruit action from vanilla cue 4e9564474c2755e40b44944ed75d81f0; falling back to a blueprint-only recruit.");
		Recruit recruit2 = new Recruit();
		recruit2.AddToParty = true;
		recruit2.MatchPlayerXpExactly = true;
		recruit2.Recruited = new Recruit.RecruitData[1]
		{
			new Recruit.RecruitData
			{
				m_CompanionBlueprint = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>("ae766624c03058440a036de90a7f2009"),
				MustBeInParty = false
			}
		};
		recruit2.OnRecruit = Helpers.CreateActionList();
		recruit2.OnRecruitImmediate = Helpers.CreateActionList();
		return recruit2;
	}
}
