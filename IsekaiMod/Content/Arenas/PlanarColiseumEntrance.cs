using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.KingmakerIntegration;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas
{
	public static class PlanarColiseumEntrance
	{
		private const string DrezenMultiEntranceGuid = "a179d214cbab4ee40b0ea3f7f312fb57";

		private const string MonadicDevaStandardGuid = "7bcffd91514f489469a6fb6dea0a50d2";

		public static BlueprintDialog ColiseumHeraldDialog;

		public static BlueprintUnit ColiseumHeraldUnit;

		public static void Add()
		{
			CreateColiseumDialogue();
			HookDrezenMultiEntrance();
			HookDefendersHeartEntrance();
			HookWarCampEntrance();
		}

		private static void CreateColiseumDialogue()
		{
			BlueprintUnit baseDeva = BlueprintTools.GetBlueprint<BlueprintUnit>("7bcffd91514f489469a6fb6dea0a50d2");
			ColiseumHeraldUnit = HubUnitFactory.Create(baseDeva, "PlanarColiseumHeraldUnit", delegate(BlueprintUnit bp)
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
				bp.AddComponent<AddImmortality>();
			});
			DialogSpeaker heraldSpeaker = new DialogSpeaker
			{
				m_Blueprint = ColiseumHeraldUnit.ToReference<BlueprintUnitReference>(),
				MoveCamera = false
			};
			ColiseumHeraldDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "PlanarColiseumHeraldDialog", delegate(BlueprintDialog bp)
			{
				bp.Type = DialogType.Common;
				bp.Conditions = new ConditionsChecker
				{
					Conditions = Array.Empty<Condition>()
				};
				BlueprintCue cueIntro = TTCoreExtensions.CreateCue("ColiseumHerald_CueIntro", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "{n}A rift of swirling astral light coalesces into a gilded amphitheater platform. Before you stands an otherworldly herald clad in starlit robes, bowing with theatrical grace.{/n}\n\n\"Greetings, champion beyond worlds! Welcome to the Planar Coliseum, where travelers from across the multiverse test their mettle before the observing constellations. What is your desire?\"");
					c.Speaker = heraldSpeaker;
				});
				BlueprintAnswer ansBronze = CreateCupAnswer("Ans_Cup_Bronze", "Register for the Bronze Cup (Recommended Level 4 - Act 1)", ArenaType.BronzeCup);
				BlueprintAnswer ansSilver = CreateCupAnswer("Ans_Cup_Silver", "Register for the Silver Cup (Recommended Level 7 - Act 2)", ArenaType.SilverCup);
				BlueprintAnswer ansGold = CreateCupAnswer("Ans_Cup_Gold", "Register for the Gold Cup (Recommended Level 11 - Act 3)", ArenaType.GoldCup);
				BlueprintAnswer ansPlatinum = CreateCupAnswer("Ans_Cup_Platinum", "Register for the Platinum Cup (Recommended Level 15 - Act 4)", ArenaType.PlatinumCup);
				BlueprintAnswer ansDiamond = CreateCupAnswer("Ans_Cup_Diamond", "Register for the Diamond Cup (Recommended Level 19 - Act 5)", ArenaType.DiamondCup);
				BlueprintAnswer ansAstral = CreateCupAnswer("Ans_Cup_Astral", "Register for the Astral Cup (Recommended Level 25 - Uncapped)", ArenaType.AstralCup);
				BlueprintAnswer ansGenesis = CreateCupAnswer("Ans_Cup_Genesis", "Register for the Genesis Cup (Recommended Level 32 - Uncapped)", ArenaType.GenesisCup);
				BlueprintAnswer ansCosmic = CreateCupAnswer("Ans_Cup_Cosmic", "Enter The Cosmic Challenge (Level 40 Pinnacle Boss Rush)", ArenaType.TheCosmicChallenge);
				BlueprintAnswer ansStolenLands = TTCoreExtensions.CreateAnswer("Ans_Cup_StolenLands", delegate(BlueprintAnswer a)
				{
					string rulerName = KingmakerCrossSaveManager.GetRulerName();
					a.SetText(Main.IsekaiContext, "Challenge the Stolen Lands Legacy Cup (Recommended Level 12 - Act 3, Echoes of " + rulerName + "'s Realm)");
					BlueprintCue bp2 = TTCoreExtensions.CreateCue("Ans_Cup_StolenLands_Cue", delegate(BlueprintCue sc)
					{
						sc.SetText(Main.IsekaiContext, "\"Ah, memories of the River Kingdoms! The beasts and fey of the Stolen Lands answer your challenge!\"");
						sc.Speaker = heraldSpeaker;
						sc.OnStop = Helpers.CreateActionList(new ActionStartColiseumCup
						{
							Cup = ArenaType.StolenLandsLegacyCup
						});
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp2.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansRegisterCups = TTCoreExtensions.CreateAnswer("Ans_Coliseum_RegisterCups", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "I wish to register for a Tournament Cup.");
					BlueprintAnswer ansBackFromCups = TTCoreExtensions.CreateAnswer("Ans_Cup_Back", delegate(BlueprintAnswer blueprintAnswer)
					{
						blueprintAnswer.SetText(Main.IsekaiContext, "[Back] Return to the previous menu.");
						blueprintAnswer.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { cueIntro.ToReference<BlueprintCueBaseReference>() }
						};
					});
					BlueprintCue bp2 = TTCoreExtensions.CreateCue("Cue_Coliseum_CupList", delegate(BlueprintCue cc)
					{
						cc.SetText(Main.IsekaiContext, "\"Which division of the coliseum do you wish to conquer? Each cup awards substantial gold, experience, and Cosmic Coins upon complete victory.\"");
						cc.Speaker = heraldSpeaker;
						BlueprintAnswersList answersList2 = Helpers.CreateBlueprint(Main.IsekaiContext, "Coliseum_CupsAnswersList", delegate(BlueprintAnswersList cal)
						{
							cal.Answers = new List<BlueprintAnswerBaseReference>
							{
								ansBronze.ToReference<BlueprintAnswerBaseReference>(),
								ansSilver.ToReference<BlueprintAnswerBaseReference>(),
								ansGold.ToReference<BlueprintAnswerBaseReference>(),
								ansPlatinum.ToReference<BlueprintAnswerBaseReference>(),
								ansDiamond.ToReference<BlueprintAnswerBaseReference>(),
								ansAstral.ToReference<BlueprintAnswerBaseReference>(),
								ansGenesis.ToReference<BlueprintAnswerBaseReference>(),
								ansCosmic.ToReference<BlueprintAnswerBaseReference>()
							};
							if (KingmakerCrossSaveManager.IsKingmakerInstalled)
							{
								cal.InsertAnswer(ansStolenLands, cal.Answers.Count);
							}
							cal.Answers.Add(ansBackFromCups.ToReference<BlueprintAnswerBaseReference>());
						});
						cc.SetAnswersList(answersList2);
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp2.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansStore = TTCoreExtensions.CreateAnswer("Ans_Coliseum_Store", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Browse the Cosmic Relic Exchange (Trade Cosmic Coins).");
					BlueprintCue bp2 = TTCoreExtensions.CreateCue("Cue_Coliseum_Store", delegate(BlueprintCue cs)
					{
						cs.SetText(Main.IsekaiContext, "\"Certainly! Step right this way to the relic vault.\"");
						cs.Speaker = heraldSpeaker;
						cs.OnStop = Helpers.CreateActionList(new ActionOpenCosmicStoreFromColiseum());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp2.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintCue cueLore = TTCoreExtensions.CreateCue("Cue_Coliseum_Lore", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"The coliseum stands anchored in the dimensional nexus between Golarion and divergent timelines. The constellations: Desna, Gorum, Iomedae, Calistria, Nethys, Asmodeus, the Lantern King, and Yog-Sothoth himself: watch your feats from the heavens, rewarding mortal champions who push past the boundaries of destiny.\"");
					cl.Speaker = heraldSpeaker;
				});
				BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Coliseum_Lore", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Tell me of this coliseum. Who observes these battles?");
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { cueLore.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansReturn = TTCoreExtensions.CreateAnswer("Ans_Coliseum_ReturnGolarion", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Step through the planar rift to return to Golarion.");
					BlueprintCue bp2 = TTCoreExtensions.CreateCue("Cue_Coliseum_ReturnGolarion", delegate(BlueprintCue cr)
					{
						cr.SetText(Main.IsekaiContext, "\"May the constellations preserve your strength. Until next time, champion!\"");
						cr.Speaker = heraldSpeaker;
						cr.OnStop = Helpers.CreateActionList(new ActionReturnToGolarion());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp2.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Coliseum_Leave", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "I will return later.");
					BlueprintCue bp2 = TTCoreExtensions.CreateCue("Cue_Coliseum_Leave", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"May the constellations illuminate your journey, otherworlder. The coliseum shall await your return.\"");
						cl.Speaker = heraldSpeaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp2.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswersList answersList = Helpers.CreateBlueprint(Main.IsekaiContext, "Coliseum_MainAnswersList", delegate(BlueprintAnswersList al)
				{
					al.Answers = new List<BlueprintAnswerBaseReference>
					{
						ansRegisterCups.ToReference<BlueprintAnswerBaseReference>(),
						ansStore.ToReference<BlueprintAnswerBaseReference>(),
						ansLore.ToReference<BlueprintAnswerBaseReference>(),
						ansReturn.ToReference<BlueprintAnswerBaseReference>(),
						ansLeave.ToReference<BlueprintAnswerBaseReference>()
					};
				});
				cueLore.SetAnswersList(answersList);
				cueIntro.SetAnswersList(answersList);
				bp.FirstCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { cueIntro.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer CreateCupAnswer(string id, string text, ArenaType cup)
			{
				return TTCoreExtensions.CreateAnswer(id, delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, text);
					BlueprintCue bp = TTCoreExtensions.CreateCue(id + "_Cue", delegate(BlueprintCue sc)
					{
						sc.SetText(Main.IsekaiContext, $"\"A bold choice! Step forward into the ring and face the trials of the {cup}!\"");
						sc.Speaker = heraldSpeaker;
						sc.OnStop = Helpers.CreateActionList(new ActionStartColiseumCup
						{
							Cup = cup
						});
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp.ToReference<BlueprintCueBaseReference>() }
					};
				});
			}
		}

		private static void HookDrezenMultiEntrance()
		{
			try
			{
				BlueprintMultiEntrance blueprint = BlueprintTools.GetBlueprint<BlueprintMultiEntrance>("a179d214cbab4ee40b0ea3f7f312fb57");
				if (blueprint != null)
				{
					BlueprintMultiEntranceEntry bp = Helpers.CreateBlueprint(Main.IsekaiContext, "DrezenColiseumMultiEntranceEntry", delegate(BlueprintMultiEntranceEntry blueprintMultiEntranceEntry)
					{
						blueprintMultiEntranceEntry.Name = Helpers.CreateString(Main.IsekaiContext, "DrezenColiseumMultiEntranceEntry.Name", "Planar Coliseum");
						blueprintMultiEntranceEntry.m_Condition = new ConditionsChecker
						{
							Conditions = Array.Empty<Condition>()
						};
						blueprintMultiEntranceEntry.m_Actions = Helpers.CreateActionList(new ActionTriggerColiseumHeraldDialogue());
					});
					List<BlueprintMultiEntranceEntryReference> list = blueprint.m_Entries?.ToList() ?? new List<BlueprintMultiEntranceEntryReference>();
					BlueprintMultiEntranceEntryReference newRef = bp.ToReference<BlueprintMultiEntranceEntryReference>();
					if (!list.Any((BlueprintMultiEntranceEntryReference r) => ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)newRef).deserializedGuid))
					{
						list.Add(newRef);
						blueprint.m_Entries = list.ToArray();
						Main.IsekaiContext.Logger.Log("[PlanarColiseum] Successfully hooked Planar Coliseum into Drezen Multi-Entrance.");
					}
				}
			}
			catch (Exception arg)
			{
				Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] Error hooking Drezen Multi-Entrance: {arg}");
			}
		}

		private static void HookDefendersHeartEntrance()
		{
			try
			{
				BlueprintAnswerBaseReference answerRef = TTCoreExtensions.CreateAnswer("Ans_DH_PlanarColiseum", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "{n}An ethereal humming reverberates from the tavern cellar.{/n} \"There is a strange planar resonance echoing below. I wish to enter the Planar Coliseum.\"");
					BlueprintCue bp = TTCoreExtensions.CreateCue("Cue_DH_PlanarColiseum", delegate(BlueprintCue c)
					{
						c.SetText(Main.IsekaiContext, "{n}The cellar's stone archway glows with soft cosmic light as the veil between planes parts before you.{/n}");
						c.OnStop = Helpers.CreateActionList(new ActionTriggerColiseumHeraldDialogue());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp.ToReference<BlueprintCueBaseReference>() }
					};
				}).ToReference<BlueprintAnswerBaseReference>();
				BlueprintTools.GetBlueprint<BlueprintAnswersList>("38adb0a43a5ac3043ad0fe59fdcba6b8")?.InsertAnswer(answerRef);
				BlueprintTools.GetBlueprint<BlueprintAnswersList>("6c6ca2d78b121504ca4bedf0a1c6f07f")?.InsertAnswer(answerRef);
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
				BlueprintAnswerBaseReference answerRef = TTCoreExtensions.CreateAnswer("Ans_WarCamp_PlanarColiseum", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "{n}A starlit rift pulses gently near the supply wagons at the camp perimeter.{/n} \"Step through the rift to the Planar Coliseum.\"");
					BlueprintCue bp = TTCoreExtensions.CreateCue("Cue_WarCamp_PlanarColiseum", delegate(BlueprintCue c)
					{
						c.SetText(Main.IsekaiContext, "{n}The starlight enfolds you, lifting you beyond the Worldwound's blighted soil to the planar arena.{/n}");
						c.OnStop = Helpers.CreateActionList(new ActionTriggerColiseumHeraldDialogue());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp.ToReference<BlueprintCueBaseReference>() }
					};
				}).ToReference<BlueprintAnswerBaseReference>();
				BlueprintTools.GetBlueprint<BlueprintAnswersList>("e9355a3b814dbc24ea94193b093ba8e1")?.InsertAnswer(answerRef);
				Main.IsekaiContext.Logger.Log("[PlanarColiseum] Successfully hooked Planar Coliseum into Crusader War Camp.");
			}
			catch (Exception arg)
			{
				Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] Error hooking War Camp Entrance: {arg}");
			}
		}
	}
}
