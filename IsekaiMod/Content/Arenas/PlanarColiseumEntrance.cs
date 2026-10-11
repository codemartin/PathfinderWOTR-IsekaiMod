using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Interaction;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas;

public static class PlanarColiseumEntrance
{
	private const string AlushinyrraMultiEntranceGuid = "5f531d59bbf5b754cb82439c2376aea3";

	private const string NexusArkMarkGuid = "060d4982238ba604aac2ded721d67e09";

	private const string MonadicDevaStandardGuid = "7bcffd91514f489469a6fb6dea0a50d2";

	private const string JoranAnswersListGuid = "38adb0a43a5ac3043ad0fe59fdcba6b8";

	private const string JoranExitAnswerGuid = "a6cd2920bff16b84697dd5614139977b";

	private const string GemylAnswersListGuid = "6c6ca2d78b121504ca4bedf0a1c6f07f";

	private const string GemylExitAnswerGuid = "d95335b734c642541b6448e42ac9132f";

	private const string JernaughAnswersListGuid = "e9355a3b814dbc24ea94193b093ba8e1";

	private const string JernaughExitAnswerGuid = "de6dd0792cd487c4aa33c1054e5f05d2";

	public static BlueprintDialog ColiseumHeraldDialog;

	public static BlueprintUnit ColiseumHeraldUnit;

	public static BlueprintAnswersList HeraldMainAnswers;

	private static BlueprintAnswer s_HeraldLeaveAnswer;

	internal const string JernaughVillageObjectiveGuid = "83ef582fb9a4f1f4b9da02dd3973a71e";

	public static void Add()
	{
		CreateColiseumDialogue();
		HookDefendersHeartEntrance();
		HookWarCampEntrance();
	}

	public static void AddHeraldAnswer(BlueprintAnswer answer)
	{
		if (answer == null || HeraldMainAnswers == null)
		{
			return;
		}
		BlueprintAnswerBaseReference reference = answer.ToReference<BlueprintAnswerBaseReference>();
		if (!HeraldMainAnswers.Answers.Any((BlueprintAnswerBaseReference r) => ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)reference).deserializedGuid))
		{
			int num = ((s_HeraldLeaveAnswer == null) ? (-1) : HeraldMainAnswers.Answers.FindIndex((BlueprintAnswerBaseReference r) => ((BlueprintReferenceBase)r).deserializedGuid == s_HeraldLeaveAnswer.AssetGuid));
			if (num < 0)
			{
				HeraldMainAnswers.Answers.Add(reference);
			}
			else
			{
				HeraldMainAnswers.Answers.Insert(num, reference);
			}
		}
	}

	internal static DialogSpeaker CreateHeraldSpeaker(BlueprintUnit herald)
	{
		return new DialogSpeaker
		{
			m_Blueprint = null,
			m_SpeakerPortrait = herald?.ToReference<BlueprintUnitReference>(),
			MoveCamera = false
		};
	}

	private static CueSelection Next(BlueprintCueBase cue)
	{
		return new CueSelection
		{
			Cues = new List<BlueprintCueBaseReference> { cue.ToReference<BlueprintCueBaseReference>() }
		};
	}

	private static void CreateColiseumDialogue()
	{
		BlueprintUnit baseDeva = BlueprintTools.GetBlueprint<BlueprintUnit>("7bcffd91514f489469a6fb6dea0a50d2");
		ColiseumHeraldUnit = Helpers.CreateBlueprint(Main.IsekaiContext, "PlanarColiseumHeraldUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Herald of the Planar Coliseum");
			if (baseDeva != null)
			{
				bp.m_Portrait = baseDeva.m_Portrait;
				bp.Prefab = baseDeva.Prefab;
				bp.Visual = baseDeva.Visual;
				bp.Gender = baseDeva.Gender;
				bp.Size = baseDeva.Size;
				bp.Alignment = Alignment.TrueNeutral;
			}
			IsekaiNpcUnit.MakeServiceNpc(bp);
			bp.AddComponent(delegate(DialogOnClick c)
			{
				c.m_Dialog = BlueprintTools.GetModBlueprintReference<BlueprintDialogReference>(Main.IsekaiContext, "PlanarColiseumHeraldDialog");
				c.Conditions = ActionFlow.EmptyCondition();
				c.NoDialogActions = ActionFlow.DoNothing();
			});
		});
		DialogSpeaker heraldSpeaker = CreateHeraldSpeaker(ColiseumHeraldUnit);
		ColiseumHeraldDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "PlanarColiseumHeraldDialog", delegate(BlueprintDialog bp)
		{
			bp.Type = DialogType.Common;
			bp.Conditions = new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
			List<BlueprintCue> returnsToMain = new List<BlueprintCue>();
			List<BlueprintCue> source = new List<BlueprintCue>
			{
				CreateIntro("ColiseumHerald_CueIntro_Act1", 1, "{n}Briars of luminous thorn have overgrown streets that belong to no city on Golarion, and fey lanterns drift above the cobbles. A herald in starlit robes bows with theatrical grace.{/n}\n\n\"Welcome to the Fae Grove, the first stage of the Planar Coliseum, where the First World's tricksters test newcomers before the watching constellations. What is your desire, champion?\""),
				CreateIntro("ColiseumHerald_CueIntro_Act2", 2, "{n}Broken banners of crusades long lost stand in the grey dust, and the wind carries the clatter of old armor. A herald in starlit robes waits among the cairns.{/n}\n\n\"Welcome to the Lost Legions, where the fallen of forgotten crusades still march for the constellations. Honor them, or join them. What is your desire, champion?\""),
				CreateIntro("ColiseumHerald_CueIntro_Act3", 3, "{n}Cracks of red light split the scorched earth beneath a sky that is always midnight. A herald in starlit robes bows from the edge of the ring.{/n}\n\n\"Welcome to the Midnight Colosseum, where demons fight for the favor of the stars and champions fight for their lives. What is your desire?\""),
				CreateIntro("ColiseumHerald_CueIntro_Act4", 4, "{n}The rock here is too quiet. Past the canyon's rim the sky simply ends, and the dark beyond it drinks the light. A herald in starlit robes stands at the lip of nothing.{/n}\n\n\"Welcome to the Void Rift, a shard of the coliseum torn loose and adrift between the Abyss and the stars. Whatever falls from here is not missed. What is your desire, champion?\""),
				CreateIntro("ColiseumHerald_CueIntro_Act5", 5, "{n}The canyon walls shimmer with reflections of other skies, other wars and other versions of this very moment. A herald in starlit robes waits where the reflections meet.{/n}\n\n\"Welcome to the Multiverse Threshold, the last stage of the Planar Coliseum, where champions of every timeline meet. Win here, and cups older than the gods open to you. What is your desire?\""),
				CreateIntro("ColiseumHerald_CueIntro", 0, "{n}A rift of swirling astral light coalesces into a gilded amphitheater platform. Before you stands an otherworldly herald clad in starlit robes, bowing with theatrical grace.{/n}\n\n\"Greetings, champion beyond worlds! Welcome to the Planar Coliseum, where travelers from across the multiverse test their mettle before the observing constellations. What is your desire?\"")
			};
			List<BlueprintAnswer> cupAnswers = new List<BlueprintAnswer>();
			CreateCupAnswer("Ans_Cup_FaeGrove", "Challenge the Fae Grove Cup (Act 1, recommended level 4).", "Act1.FaeGrove", "\"A bold choice! Step into the ring and face the Fae Grove Cup!\"");
			CreateCupAnswer("Ans_Cup_LostLegions", "Challenge the Lost Legions Cup (Act 2, recommended level 7).", "Act2.LostLegions", "\"A bold choice! Step into the ring and face the Lost Legions Cup!\"");
			CreateCupAnswer("Ans_Cup_MidnightColosseum", "Challenge the Midnight Colosseum Cup (Act 3, recommended level 11).", "Act3.MidnightColosseum", "\"A bold choice! Step into the ring and face the Midnight Colosseum Cup!\"");
			CreateCupAnswer("Ans_Cup_StolenLands", "Challenge the Stolen Lands Legacy Cup (Act 3, recommended level 12).", "Act3.StolenLands", "\"Ah, memories of the River Kingdoms! The beasts and fey of the Stolen Lands answer your challenge!\"");
			CreateCupAnswer("Ans_Cup_VoidRift", "Challenge the Void Rift Cup (Act 4, recommended level 15).", "Act4.VoidRift", "\"A bold choice! Step to the edge of the void and face the Void Rift Cup!\"");
			CreateCupAnswer("Ans_Cup_MultiverseThreshold", "Challenge the Multiverse Threshold Cup (Act 5, recommended level 19).", "Act5.MultiverseThreshold", "\"A bold choice! Step across the threshold and face the Multiverse Threshold Cup!\"");
			CreateCupAnswer("Ans_Cup_Astral", "Challenge the Astral Cup (recommended level 25).", "Act5.AstralCup", "\"A bold choice! Step into the ring and face the Astral Cup!\"");
			CreateCupAnswer("Ans_Cup_Genesis", "Challenge the Genesis Cup (recommended level 32).", "Act5.GenesisCup", "\"A bold choice! Step into the ring and face the Genesis Cup!\"");
			CreateCupAnswer("Ans_Cup_Cosmic", "Enter The Cosmic Challenge (level 40 pinnacle boss rush).", "Act5.CosmicChallenge", "\"A bold choice! Step into the ring and face The Cosmic Challenge!\"");
			BlueprintCue cueCupBack = null;
			BlueprintAnswer ansCupBack = TTCoreExtensions.CreateAnswer("Ans_Coliseum_CupBack", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "I need more time to prepare.");
				cueCupBack = TTCoreExtensions.CreateCue("Cue_Coliseum_CupBack", delegate(BlueprintCue cb)
				{
					cb.SetText(Main.IsekaiContext, "\"The sands are patient, champion. What else is your desire?\"");
					cb.Speaker = heraldSpeaker;
				});
				a.NextCue = Next(cueCupBack);
			});
			returnsToMain.Add(cueCupBack);
			BlueprintAnswer ansRegisterCups = TTCoreExtensions.CreateAnswer("Ans_Coliseum_RegisterCups", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "I wish to register for a Tournament Cup.");
				a.AddShowCondition<ConditionPartyIsInPlanarColiseum>();
				a.AddShowCondition(delegate(ConditionColiseumTrialActive c)
				{
					c.Not = true;
				});
				BlueprintCue cue = TTCoreExtensions.CreateCue("Cue_Coliseum_CupList", delegate(BlueprintCue cc)
				{
					cc.SetText(Main.IsekaiContext, "\"Which cup do you wish to conquer? A cup pays its full purse of gold, experience and Cosmic Coins the first time you win it. Each victory after that pays a tenth of the gold and experience and a quarter of the coins, never fewer than 25.\"");
					cc.Speaker = heraldSpeaker;
					BlueprintAnswersList bp3 = Helpers.CreateBlueprint(Main.IsekaiContext, "Coliseum_CupsAnswersList", delegate(BlueprintAnswersList cal)
					{
						cal.Answers = cupAnswers.Select((BlueprintAnswer ans) => ans.ToReference<BlueprintAnswerBaseReference>()).ToList();
						cal.Answers.Add(ansCupBack.ToReference<BlueprintAnswerBaseReference>());
					});
					cc.Answers = new List<BlueprintAnswerBaseReference> { bp3.ToReference<BlueprintAnswerBaseReference>() };
				});
				a.NextCue = Next(cue);
			});
			BlueprintAnswer ansStore = TTCoreExtensions.CreateAnswer("Ans_Coliseum_Store", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Browse the Cosmic Constellation Exchange (Trade Cosmic Coins).");
				a.AddShowCondition(delegate(ConditionColiseumTrialActive c)
				{
					c.Not = true;
				});
				BlueprintCue cue = TTCoreExtensions.CreateCue("Cue_Coliseum_Store", delegate(BlueprintCue cs)
				{
					cs.SetText(Main.IsekaiContext, "\"Certainly! Step right this way to the relic vault.\"");
					cs.Speaker = heraldSpeaker;
					cs.OnStop = Helpers.CreateActionList(new ActionOpenCosmicStoreFromColiseum());
				});
				a.NextCue = Next(cue);
			});
			BlueprintAnswer ansArenaLore = TTCoreExtensions.CreateAnswer("Ans_Coliseum_ArenaLore", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "What is this place?");
				BlueprintCue[] source2 = new BlueprintCue[6]
				{
					CreateArenaLore("Cue_Coliseum_ArenaLore_Act1", 1, "\"A grove the fey grew over the memory of a burning city. The First World remembers what mortals forget, and it dresses its memories in thorns. Satyrs, redcaps and a Hamadryad Queen keep the Fae Grove Cup, and they fight to amuse, not to kill. Mostly.\""),
					CreateArenaLore("Cue_Coliseum_ArenaLore_Act2", 2, "\"Every crusade that ever marched into the Worldwound left someone behind, and the constellations gathered the ones nobody buried. Ghouls, wights and the champions of dead legions hold the Lost Legions Cup. Beat them, and they rest a while.\""),
					CreateArenaLore("Cue_Coliseum_ArenaLore_Act3", 3, "\"The coliseum's heart. Demon gladiators from a hundred Abyssal pits bargain for a place on these sands, and the Midnight Colosseum Cup is theirs to defend.\""),
					CreateArenaLore("Cue_Coliseum_ArenaLore_Act4", 4, "\"A piece of the coliseum that broke away and fell toward the Abyss, and never finished falling. Mariliths, vavakia and balors climb out of the dark to test anyone who stands at its edge. Hold the Void Rift Cup, and the void itself takes notice.\""),
					CreateArenaLore("Cue_Coliseum_ArenaLore_Act5", 5, "\"The place where every timeline's coliseum touches. The Multiverse Threshold Cup is fought against the mightiest things the constellations can borrow from other worlds. Conquer it, and the Astral Cup, the Genesis Cup and the Cosmic Challenge open beyond it.\""),
					CreateArenaLore("Cue_Coliseum_ArenaLore", 0, "\"One stage of the Planar Coliseum. Each chapter of your crusade has its own, and each keeps its own cup.\"")
				};
				a.NextCue = new CueSelection
				{
					Cues = source2.Select((BlueprintCue c) => c.ToReference<BlueprintCueBaseReference>()).ToList()
				};
			});
			BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Coliseum_Lore", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Tell me of this coliseum. Who observes these battles?");
				BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("Cue_Coliseum_Lore", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"The coliseum stands anchored in the dimensional nexus between Golarion and divergent timelines. The constellations (Desna, Gorum, Iomedae, Calistria, Nethys, Asmodeus, the Lantern King, and Yog-Sothoth himself) watch your feats from the heavens, rewarding mortal champions who push past the boundaries of destiny.\"");
					cl.Speaker = heraldSpeaker;
				});
				returnsToMain.Add(blueprintCue);
				a.NextCue = Next(blueprintCue);
			});
			BlueprintAnswer ansReturn = TTCoreExtensions.CreateAnswer("Ans_Coliseum_ReturnGolarion", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Step through the planar rift to return to Golarion.");
				a.AddShowCondition<ConditionPartyIsInPlanarColiseum>();
				BlueprintCue cue = TTCoreExtensions.CreateCue("Cue_Coliseum_ReturnGolarion", delegate(BlueprintCue cr)
				{
					cr.SetText(Main.IsekaiContext, "\"May the constellations preserve your strength. Until next time, champion!\"");
					cr.Speaker = heraldSpeaker;
					cr.OnStop = Helpers.CreateActionList(new ActionReturnToGolarion());
				});
				a.NextCue = Next(cue);
			});
			BlueprintAnswer ansForfeit = TTCoreExtensions.CreateAnswer("Ans_Coliseum_Forfeit", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "I withdraw from the trial underway. Clear the sands.");
				a.AddShowCondition<ConditionPartyIsInPlanarColiseum>();
				a.AddShowCondition<ConditionColiseumTrialActive>();
				BlueprintCue cue = TTCoreExtensions.CreateCue("Cue_Coliseum_Forfeit", delegate(BlueprintCue cf)
				{
					cf.SetText(Main.IsekaiContext, "\"A withdrawal is no disgrace, champion. Some adversaries refuse to fall by any honest means.\" The herald claps once, and the ring empties.");
					cf.Speaker = heraldSpeaker;
					cf.OnStop = Helpers.CreateActionList(new ActionForfeitColiseumCup());
				});
				a.NextCue = Next(cue);
			});
			BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Coliseum_Leave", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "I will return later.");
				BlueprintCue cue = TTCoreExtensions.CreateCue("Cue_Coliseum_Leave", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"May the constellations illuminate your journey, champion. The coliseum shall await your return.\"");
					cl.Speaker = heraldSpeaker;
				});
				a.NextCue = Next(cue);
			});
			s_HeraldLeaveAnswer = ansLeave;
			BlueprintAnswersList bp2 = (HeraldMainAnswers = Helpers.CreateBlueprint(Main.IsekaiContext, "Coliseum_MainAnswersList", delegate(BlueprintAnswersList al)
			{
				al.Answers = new List<BlueprintAnswerBaseReference>
				{
					ansRegisterCups.ToReference<BlueprintAnswerBaseReference>(),
					ansStore.ToReference<BlueprintAnswerBaseReference>(),
					ansArenaLore.ToReference<BlueprintAnswerBaseReference>(),
					ansLore.ToReference<BlueprintAnswerBaseReference>(),
					ansReturn.ToReference<BlueprintAnswerBaseReference>(),
					ansForfeit.ToReference<BlueprintAnswerBaseReference>(),
					ansLeave.ToReference<BlueprintAnswerBaseReference>()
				};
			}));
			foreach (BlueprintCue item in returnsToMain)
			{
				item.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			}
			bp.FirstCue = new CueSelection
			{
				Cues = source.Select((BlueprintCue c) => c.ToReference<BlueprintCueBaseReference>()).ToList()
			};
			BlueprintCue CreateArenaLore(string name, int act, string text)
			{
				BlueprintCue blueprintCue = TTCoreExtensions.CreateCue(name, delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, text);
					c.Speaker = heraldSpeaker;
					if (act > 0)
					{
						c.Conditions = ActionFlow.IfSingle(delegate(ConditionInColiseumArenaAct cond)
						{
							cond.Act = act;
						});
					}
				});
				returnsToMain.Add(blueprintCue);
				return blueprintCue;
			}
			BlueprintAnswer CreateCupAnswer(string answerName, string text, string cupId, string startText)
			{
				BlueprintAnswer blueprintAnswer = TTCoreExtensions.CreateAnswer(answerName, delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, text);
					a.AddShowCondition(delegate(ConditionColiseumCupOffered c)
					{
						c.CupId = cupId;
					});
					a.AddShowCondition(delegate(ConditionColiseumTrialActive c)
					{
						c.Not = true;
					});
					BlueprintCue cue = TTCoreExtensions.CreateCue(answerName + "_Cue", delegate(BlueprintCue sc)
					{
						sc.SetText(Main.IsekaiContext, startText);
						sc.Speaker = heraldSpeaker;
						sc.OnStop = Helpers.CreateActionList(new ActionStartColiseumCup
						{
							CupId = cupId
						});
					});
					a.NextCue = Next(cue);
				});
				cupAnswers.Add(blueprintAnswer);
				return blueprintAnswer;
			}
			BlueprintCue CreateIntro(string name, int act, string text)
			{
				BlueprintCue blueprintCue = TTCoreExtensions.CreateCue(name, delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, text);
					c.Speaker = heraldSpeaker;
					if (act > 0)
					{
						c.Conditions = ActionFlow.IfSingle(delegate(ConditionInColiseumArenaAct cond)
						{
							cond.Act = act;
						});
					}
				});
				returnsToMain.Add(blueprintCue);
				return blueprintCue;
			}
		});
	}

	private static void HookDefendersHeartEntrance()
	{
		try
		{
			GraftedAnswer.Named("Ans_DH_PlanarColiseum").At(DialogueBeats.JoranHub, DialogueBeats.GemylHub).Repeatable()
				.Says("{n}A faint, ethereal humming rises from somewhere beneath the tavern.{/n} [Follow the humming]")
				.ShownWhen(new ConditionPlanarColiseumEnabled())
				.Configure(delegate(BlueprintAnswer a)
				{
					ColiseumGates.AddDefendersHeartSiegeGate(a);
				})
				.RepliedToBy("Cue_DH_PlanarColiseum", "{n}Below the tavern a stone archway glows with soft starlight. As you step closer the veil between planes parts, and somewhere beyond it a crowd roars for the champions of the Planar Coliseum.{/n}")
				.ConfigureReply(delegate(GraftedCue c)
				{
					c.OnStop(new ActionEnterColiseum());
				})
				.ThenEndTheConversation()
				.Graft();
			Main.IsekaiContext.Logger.Log("[PlanarColiseum] Successfully hooked Planar Coliseum into Defender's Heart Cellar.");
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] Error hooking Defender's Heart Entrance: {arg}");
		}
	}

	private static void HookWarCampEntrance()
	{
		try
		{
			GraftedAnswer.Named("Ans_WarCamp_PlanarColiseum").At(DialogueBeats.JernaughHub).Repeatable()
				.Says("{n}A thin rift of starlight pulses beside the supply wagons at the edge of the camp.{/n} [Step into the starlight]")
				.ShownWhen(new ConditionPlanarColiseumEnabled(), JernaughHandedOverTheVillage())
				.RepliedToBy("Cue_WarCamp_PlanarColiseum", "{n}The starlight enfolds you and lifts you beyond the Worldwound's blighted soil. Somewhere ahead, a crowd roars for the champions of the Planar Coliseum.{/n}")
				.ConfigureReply(delegate(GraftedCue c)
				{
					c.OnStop(new ActionEnterColiseum());
				})
				.ThenEndTheConversation()
				.Graft();
			Main.IsekaiContext.Logger.Log("[PlanarColiseum] Successfully hooked Planar Coliseum into Crusader War Camp.");
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] Error hooking War Camp Entrance: {arg}");
		}
	}

	private static Condition JernaughHandedOverTheVillage()
	{
		BlueprintQuestObjective blueprint = BlueprintTools.GetBlueprint<BlueprintQuestObjective>("83ef582fb9a4f1f4b9da02dd3973a71e");
		if (blueprint == null)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Objective_01_GotoVillage was not found; the war camp rift answer is left out.");
			return null;
		}
		return new ObjectiveStatus
		{
			m_QuestObjective = blueprint.ToReference<BlueprintQuestObjectiveReference>(),
			State = QuestObjectiveState.None,
			Not = true
		};
	}
}
