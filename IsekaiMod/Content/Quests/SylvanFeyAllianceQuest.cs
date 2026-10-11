using System;
using System.Collections.Generic;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Crusade;
using IsekaiMod.Content.GlobalMap;
using IsekaiMod.Content.KingmakerIntegration;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Quests;

public static class SylvanFeyAllianceQuest
{
	internal enum SylvanSettlement
	{
		None,
		TrialWonAlready,
		EnvoyFindsYou,
		ParleyWithoutRing
	}

	public static BlueprintQuest QuestSylvanAlliance;

	public static BlueprintQuestObjective ObjectiveBloomingScar;

	public static BlueprintQuestObjective ObjectiveWildHuntTrial;

	public static BlueprintQuestObjective ObjectiveSylvanCrusade;

	private const string StolenLandsBladeGuid = "5f6b606de240fcf4d941343e6c055179";

	private const string RobeOfArcaneAnnihilationGuid = "f389db9e641c45a2b3f7ab9cca0f76ce";

	private const string ArmagsBreastplateGuid = "cb2809036dc17fd42a391e5a2a318282";

	internal static readonly string[] GrantedRelicGuids = new string[3] { "5f6b606de240fcf4d941343e6c055179", "f389db9e641c45a2b3f7ab9cca0f76ce", "cb2809036dc17fd42a391e5a2a318282" };

	public static void Add()
	{
		try
		{
			CreateQuestAndObjectives();
			CreateBookEventAndEncounter();
			HookGlobalMapVariation();
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Failed to initialize: {arg}");
		}
	}

	private static void CreateQuestAndObjectives()
	{
		QuestSylvanAlliance = Helpers.CreateBlueprint(Main.IsekaiContext, "QuestSylvanAlliance", delegate(BlueprintQuest bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "QuestSylvanAlliance.Title", "The Bloom and the Blade: The Sylvan Crusade");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "QuestSylvanAlliance.Description", "A First World Bloom Portal has opened beside the Planar Coliseum's rift near Drezen. Interdimensional echoes suggest a treaty forged in the Stolen Lands may now alter the course of the Fifth Crusade.");
			bp.CompletionText = Helpers.CreateString(Main.IsekaiContext, "QuestSylvanAlliance.CompletionText", "The Treaty of the Stolen Lands is ratified, and the Sylvan Vanguard marches beside the Fifth Crusade.");
		});
		ObjectiveBloomingScar = Helpers.CreateBlueprint(Main.IsekaiContext, "ObjectiveBloomingScar", delegate(BlueprintQuestObjective bp)
		{
			bp.m_Quest = QuestSylvanAlliance.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = false;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "ObjectiveBloomingScar.Title", "Investigate the Blooming Scar");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "ObjectiveBloomingScar.Description", "Travel to the Planar Coliseum's rift beside Drezen on the Worldwound map and investigate reports of luminescent flora, fey mists and demonic slaughter.");
		});
		ObjectiveWildHuntTrial = Helpers.CreateBlueprint(Main.IsekaiContext, "ObjectiveWildHuntTrial", delegate(BlueprintQuestObjective bp)
		{
			bp.m_Quest = QuestSylvanAlliance.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = false;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "ObjectiveWildHuntTrial.Title", "Parley with the Wild Hunt");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "ObjectiveWildHuntTrial.Description", "Answer the Wild Hunt Monarch's challenge: defeat his court in the Stolen Lands Cup in the Midnight Colosseum (Act 3 arena). Every later arena of the Planar Coliseum offers the cup as well.");
		});
		ObjectiveSylvanCrusade = Helpers.CreateBlueprint(Main.IsekaiContext, "ObjectiveSylvanCrusade", delegate(BlueprintQuestObjective bp)
		{
			bp.m_Quest = QuestSylvanAlliance.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = true;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "ObjectiveSylvanCrusade.Title", "Unleash the Sylvan Vanguard");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "ObjectiveSylvanCrusade.Description", "Ratify the Treaty of the Stolen Lands at the Drezen War Table and march alongside the Fey against the demon horde.");
		});
		QuestSylvanAlliance.m_Objectives = new List<BlueprintQuestObjectiveReference>
		{
			ObjectiveBloomingScar.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveWildHuntTrial.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveSylvanCrusade.ToReference<BlueprintQuestObjectiveReference>()
		};
	}

	private static void CreateBookEventAndEncounter()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "SylvanBloomBookEvent", delegate(BlueprintDialog bp)
		{
			bp.Type = DialogType.Book;
			bp.Conditions = new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
			BlueprintCue cue = TTCoreExtensions.CreateCue("SylvanBloom_CueIntro", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "{n}You push through dense briars that have erupted from the cracked, abyssal basalt around the Planar Coliseum's rift. Giant phosphorescent flowers exhale luminous fey pollen, painting the grim gray sky in swirling shades of violet and emerald. Littered around the glade lie the carcasses of an entire demonic vanguard, slain not with crusader steel, but pierced by crystalline arrows and shredded by thornwood claws.{/n}");
			});
			BlueprintCue followOn = TTCoreExtensions.CreateCue("SylvanBloom_CueIntroPortal", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "{n}A royal sigil stamped into a silver spear shaft catches your eye: the insignia of a River Kingdoms realm in the Stolen Lands. Before you swirls a vortex of emerald mist, a living Bloom Portal connecting the Worldwound to the First World border.{/n}");
			});
			BlueprintCue cue2 = TTCoreExtensions.CreateCue("SylvanBloom_CueColiseum", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "{n}You leave the briars behind and step into the starlight of the Planar Coliseum's rift.{/n}");
				c.OnStop = Helpers.CreateActionList(new ActionEnterColiseum());
			});
			BlueprintBookPage pageColiseum = CreateBookPage("SylvanBloom_PageColiseum", cue2);
			BlueprintAnswer answerColiseum = TTCoreExtensions.CreateAnswer("SylvanBloom_AnsColiseum", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Step into the coliseum's rift instead.");
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { pageColiseum.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer answerLeave = TTCoreExtensions.CreateAnswer("SylvanBloom_AnsLeave", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Leave the glade for now.");
			});
			BlueprintCue cue3 = TTCoreExtensions.CreateCue("SylvanBloom_CueNature", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "{n}You recognize the telltale signs of a planar bloom. The fabric of reality here is unusually thin, worn through by the coliseum's rift. The First World is not merely invading: it is retaliating against the demonic corruption bleeding through the Worldwound.{/n}");
			});
			BlueprintBookPage pageNature = CreateBookPage("SylvanBloom_PageNature", cue3);
			BlueprintAnswer answerInvestigate = TTCoreExtensions.CreateAnswer("SylvanBloom_AnsInvestigate", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Analyze the bloom spores and read the residual First World magic.");
				a.ShowOnce = true;
				a.ShowOnceCurrentDialog = true;
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { pageNature.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer answerEnter = TTCoreExtensions.CreateAnswer("SylvanBloom_AnsEnter", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Draw your weapons and step through the Bloom Portal to meet whoever commands this incursion.");
				a.OnSelect = Helpers.CreateActionList(new ActionTriggerSylvanWildHunt());
				BlueprintCue cue4 = TTCoreExtensions.CreateCue("SylvanBloom_CueEnter", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "{n}The world twists into a blur of blinding green and autumn gold. The suffocating sulfur of the Worldwound vanishes, replaced by the scent of damp earth, crushed pine needles, and ozone. Standing atop a mossy hillock is a towering figure crowned in stag antlers: the <b>Wild Hunt Monarch</b>.{/n}");
				});
				BlueprintCue followOn2 = TTCoreExtensions.CreateCue("SylvanBloom_CueEnterChallenge", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "\"Commander of the Crusade...\" his voice rumbles like distant thunder. \"The rot of the Abyss encroaches upon our ancient preserves. The rulers of the Stolen Lands spoke of a champion of the Crusade. Show us if your blade matches your legend.\"\n\n\"But not here, where the rot can watch. My court will meet you in the ring the constellations keep: the Stolen Lands Cup of the Planar Coliseum.\" {n}The mist folds shut, and you stand once more beside the coliseum's rift.{/n}");
				});
				BlueprintAnswersList answers = Helpers.CreateBlueprint(Main.IsekaiContext, "SylvanBloom_EnterAnswersList", delegate(BlueprintAnswersList al)
				{
					al.Answers = new List<BlueprintAnswerBaseReference>
					{
						answerColiseum.ToReference<BlueprintAnswerBaseReference>(),
						answerLeave.ToReference<BlueprintAnswerBaseReference>()
					};
				});
				BlueprintBookPage bp3 = CreateBookPage("SylvanBloom_PageEnter", cue4, answers, followOn2);
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswersList blueprintAnswersList = Helpers.CreateBlueprint(Main.IsekaiContext, "SylvanBloom_AnswersList", delegate(BlueprintAnswersList al)
			{
				al.Answers = new List<BlueprintAnswerBaseReference>
				{
					answerInvestigate.ToReference<BlueprintAnswerBaseReference>(),
					answerEnter.ToReference<BlueprintAnswerBaseReference>(),
					answerColiseum.ToReference<BlueprintAnswerBaseReference>(),
					answerLeave.ToReference<BlueprintAnswerBaseReference>()
				};
			});
			pageNature.Answers.Add(blueprintAnswersList.ToReference<BlueprintAnswerBaseReference>());
			BlueprintBookPage bp2 = CreateBookPage("SylvanBloom_PageIntro", cue, blueprintAnswersList, followOn);
			bp.FirstCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { bp2.ToReference<BlueprintCueBaseReference>() }
			};
		});
	}

	private static BlueprintBookPage CreateBookPage(string name, BlueprintCue cue, BlueprintAnswersList answers = null, BlueprintCue followOn = null)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintBookPage page)
		{
			page.Conditions = ActionFlow.EmptyCondition();
			page.OnShow = ActionFlow.DoNothing();
			page.Cues = new List<BlueprintCueBaseReference> { cue.ToReference<BlueprintCueBaseReference>() };
			if (followOn != null)
			{
				page.Cues.Add(followOn.ToReference<BlueprintCueBaseReference>());
			}
			page.Answers = new List<BlueprintAnswerBaseReference>();
			if (answers != null)
			{
				page.Answers.Add(answers.ToReference<BlueprintAnswerBaseReference>());
			}
		});
	}

	private static void HookGlobalMapVariation()
	{
		BlueprintGlobalMapPoint worldWoundPoint = PlanarColiseumMapPoint.WorldWoundPoint;
		if (worldWoundPoint == null)
		{
			Main.IsekaiContext.Logger.LogError("[SylvanFeyAllianceQuest] The Planar Coliseum's Worldwound tile was not created; the Blooming Scar has no place on the map.");
			return;
		}
		BlueprintDialog modBlueprint = BlueprintTools.GetModBlueprint<BlueprintDialog>(Main.IsekaiContext, "SylvanBloomBookEvent");
		GlobalMapVariationManager.AddPointVariation(worldWoundPoint, "PointVariation_SylvanBloomScar", Helpers.CreateString(Main.IsekaiContext, "PointVariation_SylvanBloomScar.Name", "Planar Coliseum: The Blooming Scar"), Helpers.CreateString(Main.IsekaiContext, "PointVariation_SylvanBloomScar.Desc", "Luminous First World briars have overgrown the road to the coliseum's rift, and a Bloom Portal swirls among them, guarded by eerie fey sentinels."), new ConditionsChecker
		{
			Conditions = new Condition[1]
			{
				new ObjectiveStatus
				{
					m_QuestObjective = ObjectiveBloomingScar.ToReference<BlueprintQuestObjectiveReference>(),
					State = QuestObjectiveState.Started
				}
			}
		}, null, modBlueprint);
		GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveBloomingScar, worldWoundPoint);
		GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveWildHuntTrial, worldWoundPoint);
	}

	internal static bool ShouldStartSylvanQuest(bool kingmakerInstalled, int chapter, QuestState questState, bool arenaEnabled)
	{
		if ((kingmakerInstalled & arenaEnabled) && chapter >= 3 && chapter <= 5)
		{
			return questState == QuestState.None;
		}
		return false;
	}

	public static void CheckAndActivateSylvanQuest()
	{
		try
		{
			Player player = Game.Instance?.Player;
			if (player?.QuestBook == null || QuestSylvanAlliance == null)
			{
				return;
			}
			if (ShouldStartSylvanQuest(KingmakerCrossSaveManager.IsKingmakerInstalled, player.Chapter, player.QuestBook.GetQuestState(QuestSylvanAlliance), Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true))
			{
				player.QuestBook.GiveObjective(ObjectiveBloomingScar);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(ConstellationText.Localized("SylvanQuest.Begun", "<color=#004604><b>[The Bloom and the Blade]</b></color> Begun: Scouts report a First World Bloom Portal beside the Planar Coliseum's rift near Drezen!"));
				});
			}
			SettleOpenObjectives(player);
			CompleteSylvanCrusadeIfTreatyRatified(player);
			CompleteSylvanCrusadeByFallbackIfNeeded(player);
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Error in CheckAndActivateSylvanQuest: {arg}");
		}
	}

	public static void TriggerWildHuntEncounter()
	{
		try
		{
			Player player = Game.Instance?.Player;
			if (player?.QuestBook == null)
			{
				return;
			}
			QuestObjective objective = player.QuestBook.GetObjective(ObjectiveBloomingScar);
			if (objective != null && objective.State == QuestObjectiveState.Started)
			{
				player.QuestBook.CompleteObjective(ObjectiveBloomingScar);
				player.QuestBook.GiveObjective(ObjectiveWildHuntTrial);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(ConstellationText.Localized("SylvanQuest.WildHuntTerms", "<color=#004604><b>[The Bloom and the Blade]</b></color> The Wild Hunt Monarch names his terms: prove the Crusade's worth in the ring. His court awaits you in the <b>Stolen Lands Cup</b> in the Midnight Colosseum (Act 3 arena), and in every later arena of the Planar Coliseum."));
				});
				SettleOpenObjectives(player);
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Error in TriggerWildHuntEncounter: {arg}");
		}
	}

	internal static SylvanSettlement Settle(int chapter, bool arenaEnabled, QuestObjectiveState scar, QuestObjectiveState trial, bool stolenLandsCleared)
	{
		bool flag = scar == QuestObjectiveState.Started;
		bool flag2 = trial == QuestObjectiveState.Started;
		if (flag2 & stolenLandsCleared)
		{
			return SylvanSettlement.TrialWonAlready;
		}
		if (!flag && !flag2)
		{
			return SylvanSettlement.None;
		}
		if (!arenaEnabled)
		{
			return SylvanSettlement.ParleyWithoutRing;
		}
		if (flag && chapter >= 6)
		{
			return SylvanSettlement.EnvoyFindsYou;
		}
		return SylvanSettlement.None;
	}

	private static bool StolenLandsClearedInSave()
	{
		try
		{
			return ColiseumProgress.IsClearedInSave("Act3.StolenLands");
		}
		catch
		{
			return false;
		}
	}

	private static void SettleOpenObjectives(Player player)
	{
		try
		{
			if (player?.QuestBook == null || ObjectiveBloomingScar == null || ObjectiveWildHuntTrial == null)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				QuestObjectiveState questObjectiveState = player.QuestBook.GetObjective(ObjectiveBloomingScar)?.State ?? QuestObjectiveState.None;
				QuestObjectiveState trial = player.QuestBook.GetObjective(ObjectiveWildHuntTrial)?.State ?? QuestObjectiveState.None;
				switch (Settle(player.Chapter, Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true, questObjectiveState, trial, StolenLandsClearedInSave()))
				{
				default:
					return;
				case SylvanSettlement.TrialWonAlready:
					Log(ConstellationText.Localized("Constellation.Sylvan", "<color=#004604><b>[The Bloom and the Blade]</b></color> The Wild Hunt Monarch has already seen your court beaten on the Stolen Lands sands, and he does not ask twice."));
					OnWildHuntDefeated();
					break;
				case SylvanSettlement.EnvoyFindsYou:
					player.QuestBook.CompleteObjective(ObjectiveBloomingScar);
					player.QuestBook.GiveObjective(ObjectiveWildHuntTrial);
					Log(ConstellationText.Localized("Constellation.Sylvan", "<color=#004604><b>[The Bloom and the Blade]</b></color> The Blooming Scar is out of reach now, so the Wild Hunt Monarch's envoy finds you instead: his court awaits you in the <b>Stolen Lands Cup</b> of the Planar Coliseum. The Otherworldly Emissary opens the way."));
					break;
				case SylvanSettlement.ParleyWithoutRing:
				{
					if (questObjectiveState == QuestObjectiveState.Started)
					{
						player.QuestBook.CompleteObjective(ObjectiveBloomingScar);
					}
					QuestObjective objective = player.QuestBook.GetObjective(ObjectiveWildHuntTrial);
					if (objective == null || objective.State != QuestObjectiveState.Started)
					{
						player.QuestBook.GiveObjective(ObjectiveWildHuntTrial);
					}
					Log(ConstellationText.Localized("Constellation.Sylvan", "<color=#004604><b>[The Bloom and the Blade]</b></color> With the Planar Coliseum closed there is no ring to meet in, so the Wild Hunt Monarch comes to parley in person and judges the Crusade by its deeds instead."));
					OnWildHuntDefeated();
					break;
				}
				}
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Error settling the quest's open objectives: {arg}");
		}
	}

	private static void Log(string line)
	{
		ConstellationChatManager.PostCombatLog(line);
	}

	public static void OnWildHuntDefeated()
	{
		try
		{
			Player player = Game.Instance?.Player;
			if (player?.QuestBook == null || ObjectiveWildHuntTrial == null)
			{
				return;
			}
			QuestObjective objective = player.QuestBook.GetObjective(ObjectiveWildHuntTrial);
			if (objective != null && objective.State == QuestObjectiveState.Started)
			{
				player.QuestBook.CompleteObjective(ObjectiveWildHuntTrial);
				player.QuestBook.GiveObjective(ObjectiveSylvanCrusade);
				string forged = (IsekaiKingdomProjects.CanRatifyStolenLandsTreaty() ? ConstellationText.Localized("SylvanQuest.AllianceForgedWarTable", "<color=#004604><b>[Sylvan Alliance Forged]</b></color> The Wild Hunt Monarch pledges his horns to the Fifth Crusade! Ratify the Treaty of the Stolen Lands at the war table to march beside them.") : ConstellationText.Localized("SylvanQuest.AllianceForged", "<color=#004604><b>[Sylvan Alliance Forged]</b></color> The Wild Hunt Monarch pledges his horns to the Fifth Crusade!"));
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(forged);
				});
				GrantStolenLandsRelics(player);
				CompleteSylvanCrusadeIfTreatyRatified(player);
				CompleteSylvanCrusadeByFallbackIfNeeded(player);
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Error in OnWildHuntDefeated: {arg}");
		}
	}

	public static void OnTreatyRatified()
	{
		try
		{
			Player player = Game.Instance?.Player;
			if (player?.QuestBook != null && ObjectiveSylvanCrusade != null)
			{
				CompleteSylvanCrusade(player, treatyRatified: true);
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Error in OnTreatyRatified: {arg}");
		}
	}

	internal static bool ShouldCompleteSylvanCrusade(QuestObjectiveState objectiveState, bool treatyRatified)
	{
		if (treatyRatified)
		{
			return objectiveState == QuestObjectiveState.Started;
		}
		return false;
	}

	internal static bool TreatyRatifiedIn(IEnumerable<KingdomEventHistoryEntry> history, BlueprintKingdomEventBase treaty)
	{
		if (history == null || treaty == null)
		{
			return false;
		}
		foreach (KingdomEventHistoryEntry item in history)
		{
			if (item != null && item.Event == treaty && item.SolveResults != null && item.SolveResults.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsTreatyRatified()
	{
		BlueprintKingdomProject projectTreatyStolenLands = IsekaiKingdomProjects.ProjectTreatyStolenLands;
		if (projectTreatyStolenLands == null || !KingdomState.Founded)
		{
			return false;
		}
		return TreatyRatifiedIn(KingdomState.Instance.EventHistory, projectTreatyStolenLands);
	}

	private static void CompleteSylvanCrusadeIfTreatyRatified(Player player)
	{
		try
		{
			if (player?.QuestBook != null && ObjectiveSylvanCrusade != null)
			{
				QuestObjective objective = player.QuestBook.GetObjective(ObjectiveSylvanCrusade);
				if (objective != null && objective.State == QuestObjectiveState.Started)
				{
					CompleteSylvanCrusade(player, IsTreatyRatified());
				}
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Error checking the Treaty of the Stolen Lands: {arg}");
		}
	}

	internal static bool ShouldCompleteSylvanCrusadeByFallback(QuestObjectiveState objectiveState, bool treatyRatified, bool warTableCanRatify)
	{
		if (objectiveState == QuestObjectiveState.Started && !treatyRatified)
		{
			return !warTableCanRatify;
		}
		return false;
	}

	private static void CompleteSylvanCrusadeByFallbackIfNeeded(Player player)
	{
		try
		{
			if (player?.QuestBook == null || ObjectiveSylvanCrusade == null)
			{
				return;
			}
			QuestObjectiveState questObjectiveState = player.QuestBook.GetObjective(ObjectiveSylvanCrusade)?.State ?? QuestObjectiveState.None;
			if (questObjectiveState == QuestObjectiveState.Started && ShouldCompleteSylvanCrusadeByFallback(questObjectiveState, IsTreatyRatified(), IsekaiKingdomProjects.CanRatifyStolenLandsTreaty()))
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(ConstellationText.Localized("SylvanQuest.TreatyWithoutWarTable", "<color=#004604><b>[The Bloom and the Blade]</b></color> The envoy of the Stolen Lands does not wait for the war table: the treaty is sealed with the crusade's quartermasters."));
				});
				IsekaiKingdomProjects.PayStolenLandsTreatyWithoutWarTable();
				CompleteSylvanCrusade(player, treatyRatified: true);
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Error completing the Sylvan Vanguard without the war table: {arg}");
		}
	}

	private static void CompleteSylvanCrusade(Player player, bool treatyRatified)
	{
		QuestObjective objective = player.QuestBook.GetObjective(ObjectiveSylvanCrusade);
		if (objective != null && ShouldCompleteSylvanCrusade(objective.State, treatyRatified))
		{
			player.QuestBook.CompleteObjective(ObjectiveSylvanCrusade);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(ConstellationText.Localized("SylvanQuest.Complete", "<color=#004604><b>[The Bloom and the Blade: Complete]</b></color> The Sylvan Vanguard marches out of the First World and onto the Worldwound."));
			});
		}
	}

	private static void GrantStolenLandsRelics(Player player)
	{
		TryGrant("5f6b606de240fcf4d941343e6c055179");
		TryGrant("f389db9e641c45a2b3f7ab9cca0f76ce");
		TryGrant("cb2809036dc17fd42a391e5a2a318282");
		void TryGrant(string guid)
		{
			BlueprintItem item = BlueprintTools.GetBlueprint<BlueprintItem>(guid);
			if (item != null && !PersonalQuestDirector.PartyHolds(item))
			{
				player.Inventory.Add(item, 1);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(ConstellationText.LocalizedFormat("Constellation.SylvanQuest.Gift", "<color=#5B2A86>[Gift of the Wild Hunt]</color> Obtained: <b>{0}</b>!", item.Name));
				});
			}
		}
	}
}
