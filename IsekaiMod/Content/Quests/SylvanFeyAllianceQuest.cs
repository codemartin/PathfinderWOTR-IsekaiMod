using System;
using System.Collections.Generic;
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
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Quests
{
	public static class SylvanFeyAllianceQuest
	{
		public static BlueprintQuest QuestSylvanAlliance;

		public static BlueprintQuestObjective ObjectiveBloomingScar;

		public static BlueprintQuestObjective ObjectiveWildHuntTrial;

		public static BlueprintQuestObjective ObjectiveSylvanCrusade;

		private const string ArmagsBladeGuid = "ce0842546b73aa34b8fcf40a970ede68";

		private const string RobeOfArcaneAnnihilationGuid = "f389db9e641c45a2b3f7ab9cca0f76ce";

		private const string ArmagsBreastplateGuid = "cb2809036dc17fd42a391e5a2a318282";

		private const string AngelFlowersGuid = "f8c66d7875baa2b40aeb667f4d7257d5";

		private const string WildHuntMonarchGuid = "573009c2f6493514188a2844ba53bdf8";

		private const string WildHuntScoutGuid = "6f5ff0f1e359ee042ba49a746a507190";

		private const string WildHuntArcherGuid = "e21b6536b40aaad4e9c9cd6c216778a3";

		private const string AnkouGuid = "58ed91a92b8d70248aa884d303954469";

		private const string HamadryadGuid = "b8972cfe36e3cd945bbd2c4c320d5237";

		private const string PrimalTreantGuid = "b2c86a184c5f9c942aeb77e52ea1d17d";

		public static void Add()
		{
			try
			{
				if (!KingmakerCrossSaveManager.IsKingmakerInstalled)
				{
					Main.IsekaiContext.Logger.Log("[SylvanFeyAllianceQuest] Kingmaker not detected; Sylvan Fey Alliance quest remains dormant.");
					return;
				}
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
				bp.Description = Helpers.CreateString(Main.IsekaiContext, "QuestSylvanAlliance.Description", "A bizarre First World Bloom Portal has pierced the western perimeter of the Worldwound. Interdimensional echoes suggest an ancient treaty forged in the Stolen Lands may now alter the course of the Fifth Crusade.");
			});
			ObjectiveBloomingScar = Helpers.CreateBlueprint(Main.IsekaiContext, "ObjectiveBloomingScar", delegate(BlueprintQuestObjective bp)
			{
				bp.m_Quest = QuestSylvanAlliance.ToReference<BlueprintQuestReference>();
				bp.m_Type = BlueprintQuestObjective.Type.Objective;
				bp.Title = Helpers.CreateString(Main.IsekaiContext, "ObjectiveBloomingScar.Title", "Investigate the Blooming Scar");
				bp.Description = Helpers.CreateString(Main.IsekaiContext, "ObjectiveBloomingScar.Description", "Traverse to the western forest boundary near the ruins of Sarkoris to investigate reports of luminescent flora, fey mists, and demonic slaughter.");
			});
			ObjectiveWildHuntTrial = Helpers.CreateBlueprint(Main.IsekaiContext, "ObjectiveWildHuntTrial", delegate(BlueprintQuestObjective bp)
			{
				bp.m_Quest = QuestSylvanAlliance.ToReference<BlueprintQuestReference>();
				bp.m_Type = BlueprintQuestObjective.Type.Objective;
				bp.Title = Helpers.CreateString(Main.IsekaiContext, "ObjectiveWildHuntTrial.Title", "Parley with the Wild Hunt");
				bp.Description = Helpers.CreateString(Main.IsekaiContext, "ObjectiveWildHuntTrial.Description", "Step through the Bloom Rift and confront the Wild Hunt Monarch. Prove the Crusade's worth through skill or steel to forge a planar alliance.");
			});
			ObjectiveSylvanCrusade = Helpers.CreateBlueprint(Main.IsekaiContext, "ObjectiveSylvanCrusade", delegate(BlueprintQuestObjective bp)
			{
				bp.m_Quest = QuestSylvanAlliance.ToReference<BlueprintQuestReference>();
				bp.m_Type = BlueprintQuestObjective.Type.Objective;
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
			string ruler = KingmakerCrossSaveManager.GetRulerName();
			Helpers.CreateBlueprint(Main.IsekaiContext, "SylvanBloomBookEvent", delegate(BlueprintDialog bp)
			{
				bp.Type = DialogType.Book;
				bp.Conditions = new ConditionsChecker
				{
					Conditions = Array.Empty<Condition>()
				};
				BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("SylvanBloom_CueIntro", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "{n}You push through dense briars that have erupted from the cracked, abyssal basalt. Giant phosphorescent flowers exhale luminous fey pollen, painting the grim gray sky in swirling shades of violet and emerald. Littered around the glade lie the carcasses of an entire demonic vanguard, slain not with crusader steel, but pierced by crystalline arrows and shredded by thornwood claws.{/n}\n\nA royal sigil stamped into a silver spear shaft catches your eye, bearing the insignia of the River Kingdoms realm ruled by <b>" + ruler + "</b>. Before you stands a swirling vortex of emerald mist: a living Bloom Portal connecting the Worldwound to the First World border.");
				});
				BlueprintAnswer answerInvestigate = TTCoreExtensions.CreateAnswer("SylvanBloom_AnsInvestigate", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "[Lore: Nature] Analyze the bloom spores and read the residual First World magic.");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("SylvanBloom_CueNature", delegate(BlueprintCue c)
					{
						c.SetText(Main.IsekaiContext, "{n}You recognize the telltale signs of a planar bloom. The fabric of reality here is unusually thin. The First World is not merely invading: it is retaliating against the demonic corruption bleeding through the Worldwound's western frontier.{/n}");
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer answerEnter = TTCoreExtensions.CreateAnswer("SylvanBloom_AnsEnter", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Draw your weapons and step through the Bloom Portal to meet whoever commands this incursion.");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("SylvanBloom_CueEnter", delegate(BlueprintCue c)
					{
						c.SetText(Main.IsekaiContext, "{n}The world twists into a blur of blinding green and autumn gold. The suffocating sulfur of the Worldwound vanishes, replaced by the scent of damp earth, crushed pine needles, and ozone. Standing atop a towering mossy hillock is a towering figure crowned in stag antlers: the <b>Wild Hunt Monarch</b>.{/n}\n\n\"Mortal Commander...\" his voice rumbles like distant thunder. \"The rot of the Abyss encroaches upon our ancient preserves. The rulers of the Stolen Lands spoke of a champion beyond worlds. Show us if your blade matches your legend!\"");
						c.OnStop = Helpers.CreateActionList(new ActionTriggerSylvanWildHunt());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "SylvanBloom_AnswersList", delegate(BlueprintAnswersList al)
				{
					al.Answers = new List<BlueprintAnswerBaseReference>
					{
						answerInvestigate.ToReference<BlueprintAnswerBaseReference>(),
						answerEnter.ToReference<BlueprintAnswerBaseReference>()
					};
				});
				blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
				bp.FirstCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { blueprintCue.ToReference<BlueprintCueBaseReference>() }
				};
			});
		}

		private static void HookGlobalMapVariation()
		{
			BlueprintGlobalMapPoint blueprint = BlueprintTools.GetBlueprint<BlueprintGlobalMapPoint>("f8c66d7875baa2b40aeb667f4d7257d5");
			if (blueprint != null)
			{
				BlueprintDialog modBlueprint = BlueprintTools.GetModBlueprint<BlueprintDialog>(Main.IsekaiContext, "SylvanBloomBookEvent");
				GlobalMapVariationManager.AddPointVariation(blueprint, "PointVariation_SylvanBloomScar", Helpers.CreateString(Main.IsekaiContext, "PointVariation_SylvanBloomScar.Name", "The Blooming Scar: First World Incursion"), Helpers.CreateString(Main.IsekaiContext, "PointVariation_SylvanBloomScar.Desc", "A vibrant rift of First World bloom and mist has ruptured the desolate Worldwound stone, guarded by eerie fey sentinels."), new ConditionsChecker
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
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveBloomingScar, blueprint);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveWildHuntTrial, blueprint);
			}
		}

		public static void CheckAndActivateSylvanQuest()
		{
			try
			{
				if (!KingmakerCrossSaveManager.IsKingmakerInstalled)
				{
					return;
				}
				Player player = Game.Instance?.Player;
				if (player?.QuestBook != null && player.Chapter >= 3 && player.QuestBook.GetQuestState(QuestSylvanAlliance) == QuestState.None)
				{
					player.QuestBook.GiveObjective(ObjectiveBloomingScar);
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#32CD32><b>[The Bloom and the Blade]</b></color> Begun: Scouts report First World Bloom Portals emerging in the western Worldwound!");
					});
				}
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
				if (player == null)
				{
					return;
				}
				QuestObjective objective = player.QuestBook.GetObjective(ObjectiveBloomingScar);
				if (objective != null && objective.State == QuestObjectiveState.Started)
				{
					player.QuestBook.CompleteObjective(ObjectiveBloomingScar);
					player.QuestBook.GiveObjective(ObjectiveWildHuntTrial);
				}
				TryGrant("ce0842546b73aa34b8fcf40a970ede68", "Ovinrbaane, Enemy of All Enemies");
				TryGrant("f389db9e641c45a2b3f7ab9cca0f76ce", "Robe of Arcane Annihilation");
				TryGrant("cb2809036dc17fd42a391e5a2a318282", "Armag's Breastplate");
				QuestObjective objective2 = player.QuestBook.GetObjective(ObjectiveWildHuntTrial);
				if (objective2 != null && objective2.State == QuestObjectiveState.Started)
				{
					player.QuestBook.CompleteObjective(ObjectiveWildHuntTrial);
					player.QuestBook.GiveObjective(ObjectiveSylvanCrusade);
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#32CD32><b>[Sylvan Alliance Forged]</b></color> The Wild Hunt Monarch pledges his horns to the Fifth Crusade!");
					});
				}
				void TryGrant(string guid, string itemName)
				{
					BlueprintItem item = BlueprintTools.GetBlueprint<BlueprintItem>(guid);
					if (item != null && !player.Inventory.Contains(item))
					{
						player.Inventory.Add(item, 1);
						EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
						{
							h.HandleLogMessage("<color=#FFD700>[Relic of the Stolen Lands Acquired]</color> Obtained: <b>" + item.Name + "</b>!");
						});
					}
				}
			}
			catch (Exception arg)
			{
				Main.IsekaiContext.Logger.LogError($"[SylvanFeyAllianceQuest] Error in TriggerWildHuntEncounter: {arg}");
			}
		}
	}
}
