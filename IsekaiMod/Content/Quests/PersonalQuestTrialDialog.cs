using System;
using System.Collections.Generic;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Quests;

public static class PersonalQuestTrialDialog
{
	public static void Add()
	{
		AddTrialAnswers();
		AddDivergenceAnswers();
	}

	private static CueSelection Next(BlueprintCueBase cue)
	{
		return new CueSelection
		{
			Cues = new List<BlueprintCueBaseReference> { cue.ToReference<BlueprintCueBaseReference>() }
		};
	}

	private static void AddTrialAnswers()
	{
		try
		{
			DialogSpeaker herald = PlanarColiseumEntrance.CreateHeraldSpeaker(PlanarColiseumEntrance.ColiseumHeraldUnit);
			AddTrial(herald, "Ans_Trial_PhantomSwordSaint", "Trial.PhantomBlade.Act2", "(Personal Trial) I formally challenge the Sarkorian sword-saint.", "\"A formal challenge, entered and accepted. He has been waiting a long time for somebody to do it properly.\"");
			AddTrial(herald, "Ans_Trial_BladeSpirit", "Trial.PhantomBlade.Act3", "(Personal Trial) Call out the Blade Spirit.", "\"The Blade Spirit does not need calling. It has been watching you fight since you walked in.\"");
			AddTrial(herald, "Ans_Trial_BoneCourt", "Trial.Overlord.Act2", "(Personal Trial) Convene the bone court on these sands.", "\"Then the court is convened, and you are not the one being tried. Not exactly.\"");
			AddTrial(herald, "Ans_Trial_CorrosiveWyrm", "Trial.Devourer.Act1", "(Personal Trial) Bring me something corrosive to devour.", "\"An unusual request, and one this coliseum can meet. Mind the fumes, champion.\"");
			AddTrial(herald, "Ans_Trial_PrimordialBehemoth", "Trial.Devourer.Act3", "(Personal Trial) Put the Primordial Behemoth in the ring.", "\"The far gate is wide enough. Barely. Step back, champion.\"");
			AddTrial(herald, "Ans_Trial_FallenVanguard", "Trial.ShadowMonarch.Act2", "(Personal Trial) Summon the shadows of the fallen vanguard.", "\"They were crusaders. They will fight you as crusaders. Take what is left of them afterwards, if you can.\"");
			AddTrial(herald, "Ans_Trial_ThroneOfShadows", "Trial.ShadowMonarch.Act3", "(Personal Trial) Set the Throne of Shadows on the sands.", "\"The throne is already here. It has always been here. Whatever sits on it will want to keep it.\"");
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not add the trial answers to the herald: " + ex);
		}
	}

	private static void AddTrial(DialogSpeaker herald, string answerName, string trialId, string answerText, string heraldReply)
	{
		PlanarColiseumEntrance.AddHeraldAnswer(TTCoreExtensions.CreateAnswer(answerName, delegate(BlueprintAnswer a)
		{
			a.SetText(Main.IsekaiContext, answerText);
			a.AddShowCondition<ConditionPlanarColiseumEnabled>();
			a.AddShowCondition<ConditionPartyIsInPlanarColiseum>();
			a.AddShowCondition(delegate(ConditionColiseumTrialActive c)
			{
				c.Not = true;
			});
			a.AddShowCondition(delegate(ConditionPersonalTrialOffered c)
			{
				c.TrialId = trialId;
			});
			BlueprintCue cue = TTCoreExtensions.CreateCue(answerName + "_Cue", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, heraldReply);
				c.Speaker = herald;
				c.OnStop = Helpers.CreateActionList(new ActionStartColiseumCup
				{
					CupId = trialId
				});
			});
			a.NextCue = Next(cue);
			a.RequireValidCue = true;
		}));
	}

	private static void AddDivergenceAnswers()
	{
		try
		{
			DialogSpeaker oracle = PlanarColiseumHubManager.CreateOracleSpeaker();
			AddDivergence(oracle, "Ans_Oracle_DevourerTempest", "Tempest", "(The Sovereign Divergence) I will be the Benevolent Tempest. What I take, I will keep alive.", "\"The Tempest, then. The constellations write it down, and they will hold you to it. What you devour, you will answer for.\"");
			AddDivergence(oracle, "Ans_Oracle_DevourerVoid", "Void", "(The Sovereign Divergence) I will be the Abyssal Void. What I take is gone.", "\"The Void. The basin does not show me the end of that one, and the constellations have stopped arguing about it. It is written.\"");
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PersonalQuests] Could not add the divergence answers to the Oracle: " + ex);
		}
	}

	private static void AddDivergence(DialogSpeaker oracle, string answerName, string path, string answerText, string oracleReply)
	{
		PlanarColiseumHubManager.AddOracleAnswer(TTCoreExtensions.CreateAnswer(answerName, delegate(BlueprintAnswer a)
		{
			a.SetText(Main.IsekaiContext, answerText);
			a.AddShowCondition<ConditionPlanarColiseumEnabled>();
			a.AddShowCondition<ConditionDevourerDivergenceOpen>();
			a.OnSelect = Helpers.CreateActionList(new ActionChooseDevourerPath
			{
				Path = path
			});
			BlueprintCue cue = TTCoreExtensions.CreateCue(answerName + "_Cue", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, oracleReply);
				c.Speaker = oracle;
				if (PlanarColiseumHubManager.OracleMainAnswers != null)
				{
					c.Answers = new List<BlueprintAnswerBaseReference> { PlanarColiseumHubManager.OracleMainAnswers.ToReference<BlueprintAnswerBaseReference>() };
				}
			});
			a.NextCue = Next(cue);
			a.RequireValidCue = true;
		}));
	}
}
