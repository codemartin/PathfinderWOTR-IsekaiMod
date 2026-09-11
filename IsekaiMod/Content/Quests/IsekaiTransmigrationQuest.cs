using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Quests;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Quests
{
	public static class IsekaiTransmigrationQuest
	{
		public static BlueprintQuest TransmigrationQuest;

		public static BlueprintQuestObjective ObjectiveAct1;

		public static BlueprintQuestObjective ObjectiveAct2;

		public static BlueprintQuestObjective ObjectiveAct3;

		public static BlueprintQuestObjective ObjectiveAct4;

		public static BlueprintQuestObjective ObjectiveAct5;

		public static BlueprintQuestObjective ObjectiveAct6;

		public static BlueprintQuestObjective ObjectiveEnding_UnwrittenDawn;

		public static BlueprintQuestObjective ObjectiveEnding_InterdimensionalTraveler;

		public static BlueprintQuestObjective ObjectiveEnding_EternalActor;

		public static BlueprintQuestObjective ObjectiveEnding_PaxSepulchrum;

		public static BlueprintQuestObjective ObjectiveEnding_IronCitadel;

		public static BlueprintQuestObjective ObjectiveEnding_NecroTyrant;

		public static BlueprintQuestObjective ObjectiveEnding_TempestFederation;

		public static BlueprintQuestObjective ObjectiveEnding_PrimevalMarsh;

		public static BlueprintQuestObjective ObjectiveEnding_AbyssalSingularity;

		public static BlueprintQuestObjective ObjectiveEnding_EternalHope;

		public static BlueprintQuestObjective ObjectiveEnding_RetiredLegend;

		public static BlueprintQuestObjective ObjectiveEnding_CorruptedChampion;

		public static BlueprintQuestObjective ObjectiveEnding_PaxSolaris;

		public static BlueprintQuestObjective ObjectiveEnding_SarkorianImperium;

		public static BlueprintQuestObjective ObjectiveEnding_DualThrone;

		public static BlueprintQuestObjective ObjectiveEnding_SilentAegis;

		public static BlueprintQuestObjective ObjectiveEnding_ObsidianBastion;

		public static BlueprintQuestObjective ObjectiveEnding_NetherworldCataclysm;

		public static BlueprintQuestObjective ObjectiveEnding_GrandArchitect;

		public static BlueprintQuestObjective ObjectiveEnding_PuppetParliament;

		public static BlueprintQuestObjective ObjectiveEnding_ChessboardApocalypse;

		public static BlueprintQuestObjective ObjectiveEnding_InfiniteHorizons;

		public static BlueprintQuestObjective ObjectiveEnding_BoundaryHermit;

		public static BlueprintQuestObjective ObjectiveEnding_SeveredHorizon;

		public static void Add()
		{
			TransmigrationQuest = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiTransmigrationQuest", delegate(BlueprintQuest bp)
			{
				bp.Title = Helpers.CreateString(Main.IsekaiContext, "IsekaiTransmigrationQuest.Title", "The Outer Threshold: Reincarnation Mystery");
				bp.Description = Helpers.CreateString(Main.IsekaiContext, "IsekaiTransmigrationQuest.Description", "You were pulled across planar dimensions and time by Yog-Sothoth, the Key and the Gate.\nAcross each phase of your crusade, tears in the dimensional fabric manifest in the form of otherworld anomalies and deadly incursions.\nUnravel the truth behind your reincarnation, the watching deities, and the closed temporal loop holding Golarion hostage.");
				bp.m_Group = QuestGroupId.None;
				bp.m_Type = QuestType.Normal;
				bp.m_LastChapter = 5;
			});
			ObjectiveAct1 = CreateObjective("IsekaiQuestObjective_Act1", "A Flaw in the Script: Investigate the Kenabres Rift Anomaly", "A dimensional rift has cracked open reality near Kenabres. Confront the entity emerging from the void. Noticeably, the observing deities talk as if they've seen this play out before.");
			ObjectiveAct2 = CreateObjective("IsekaiQuestObjective_Act2", "The Discarded Iteration: Confront Kenji the Exiled Ronin", "Another reincarnated warrior, Kenji, a masterless swordsman from an eastern realm, has crossed into the Worldwound. Defeat him to uncover his echo journal detailing previous forgotten crusades.");
			ObjectiveAct3 = CreateObjective("IsekaiQuestObjective_Act3", "Behind the Velvet Curtain: The Truth in Areelu's Lab", "A shadowy planar horror named Malakor is drawing power from the rift nexus. Unravel the revelation from Areelu's lab: her ritual did not summon her child, but Yog-Sothoth siphoned your reincarnated soul into this closed playback cycle.");
			ObjectiveAct4 = CreateObjective("IsekaiQuestObjective_Act4", "The Key of Causal Rupture: Slay the Abyssal Rift Titan", "Deep in the Abyss, the rift disruption has manifested an immense titan. Claim the Key of Causal Rupture, rejecting Yog-Sothoth's whisper to remain comfortably within the endless loop.");
			ObjectiveAct5 = CreateObjective("IsekaiQuestObjective_Act5", "Breach the Perimeter: Defeat the Herald of the Closed Threshold", "At the precipice of the Threshold, the final guardian of the multiversal barrier awaits. Defeat the Herald to unlock the core mechanism of the temporal cycle.");
			ObjectiveAct6 = CreateObjective("IsekaiQuestObjective_Act6", "Shatter the Infinite Loop: The Unwritten Dawn", "Using the assembled Keys of Causal Rupture, confront the Grand Arbiter of the Loop. Decide whether to remain the adored immortal star of the celestial stream, or shatter the loop forever to claim true unscripted freedom.");
			ObjectiveEnding_UnwrittenDawn = CreateObjective("ObjectiveEnding_UnwrittenDawn", "The Unwritten Dawn (True Loop-Breaker)", "Shatter Yog-Sothoth's causal anchors. Break the loop forever and open an interdimensional gateway between Golarion and Earth.");
			ObjectiveEnding_InterdimensionalTraveler = CreateObjective("ObjectiveEnding_InterdimensionalTraveler", "The Interdimensional Traveler", "Seal the Worldwound and step into the dimensional slipstream to explore the Great Beyond.");
			ObjectiveEnding_EternalActor = CreateObjective("ObjectiveEnding_EternalActor", "The Eternal Actor", "Surrender to the Constellation broadcast, choosing comfortable eternal celebrity over unknown reality.");
			ObjectiveEnding_PaxSepulchrum = CreateObjective("ObjectiveEnding_PaxSepulchrum", "Pax Sepulchrum - Guardian of the Great Tomb", "Extend an impartial, unyielding undead protectorate over mortal subjects where the dead labor and the living thrive.");
			ObjectiveEnding_IronCitadel = CreateObjective("ObjectiveEnding_IronCitadel", "Dominion of the Iron Citadel", "Fortify Sarkoris behind obsidian ramparts, declaring an isolated, sovereign necropolis.");
			ObjectiveEnding_NecroTyrant = CreateObjective("ObjectiveEnding_NecroTyrant", "The Eternal Necro-Tyrant", "Deem mortal life a chaotic inefficiency; harvest all living souls into the disciplined undead legion.");
			ObjectiveEnding_TempestFederation = CreateObjective("ObjectiveEnding_TempestFederation", "The Tempest Multiverse Federation", "Metabolize the Worldwound rift into fertile mana, founding a multiversal haven of beasts, monsters, and mortals.");
			ObjectiveEnding_PrimevalMarsh = CreateObjective("ObjectiveEnding_PrimevalMarsh", "The Primeval Marsh", "Restore the Worldwound into an untamed, primeval wildland, sleeping beneath the World Tree as Golarion's guardian beast-god.");
			ObjectiveEnding_AbyssalSingularity = CreateObjective("ObjectiveEnding_AbyssalSingularity", "The Abyssal Singularity", "Plunge into the Abyss and devour demon lords until stirring Azathoth, forcing the causal loop to reset.");
			ObjectiveEnding_EternalHope = CreateObjective("ObjectiveEnding_EternalHope", "The Sovereign Beacon of Eternal Hope", "Channel the collective hope of all mortals into an undying celestial nova, ascending as the eternal protector.");
			ObjectiveEnding_RetiredLegend = CreateObjective("ObjectiveEnding_RetiredLegend", "The Retired Legend", "Refuse godhood and political thrones, opening a training guild and tavern in Kenabres to live as a peaceful mortal.");
			ObjectiveEnding_CorruptedChampion = CreateObjective("ObjectiveEnding_CorruptedChampion", "The Corrupted Champion", "Live long enough to become the villain; consumed by paranoia, blood on your hands, and madness.");
			ObjectiveEnding_PaxSolaris = CreateObjective("ObjectiveEnding_PaxSolaris", "Pax Solaris - The Golden Renaissance", "Establish the Golden Throne of Sarkoris, becoming the eternal God-King of humanity and the multiverse.");
			ObjectiveEnding_SarkorianImperium = CreateObjective("ObjectiveEnding_SarkorianImperium", "The Sarkorian Imperium", "Fortify northern Avistan into an unassailable imperial domain prioritizing defense, trade dominance, and order.");
			ObjectiveEnding_DualThrone = CreateObjective("ObjectiveEnding_DualThrone", "The Dual Throne of Sun and Abyss", "March solar legions into the Abyss, conquering both mortals and fiends under absolute solar fealty.");
			ObjectiveEnding_SilentAegis = CreateObjective("ObjectiveEnding_SilentAegis", "The Silent Aegis of Shadows", "Command the infinite Legion of Shadows to guard Golarion from the dark without oppressing mortals.");
			ObjectiveEnding_ObsidianBastion = CreateObjective("ObjectiveEnding_ObsidianBastion", "The Obsidian Bastion", "Transform the Worldwound into the Shadow Gate, sealing the Abyss permanently from Golarion.");
			ObjectiveEnding_NetherworldCataclysm = CreateObjective("ObjectiveEnding_NetherworldCataclysm", "The Netherworld Cataclysm", "Open a permanent portal to the Netherworld / Shadow Plane where the Worldwound used to be.");
			ObjectiveEnding_GrandArchitect = CreateObjective("ObjectiveEnding_GrandArchitect", "The Grand Architect", "Weave a flawless global web of statecraft and enlightenment from the shadows, bending nations toward prosperity without ever revealing your hand.");
			ObjectiveEnding_PuppetParliament = CreateObjective("ObjectiveEnding_PuppetParliament", "The Puppet Parliament", "Install figureheads across every throne in Avistan, pulling the geopolitical strings of kings and archbishops as a secret shadow government.");
			ObjectiveEnding_ChessboardApocalypse = CreateObjective("ObjectiveEnding_ChessboardApocalypse", "The Chessboard Apocalypse", "Treat the mortal and planar realms as expendable pawns on a cosmic chessboard, plunging civilizations into engineered ruin for amusement.");
			ObjectiveEnding_InfiniteHorizons = CreateObjective("ObjectiveEnding_InfiniteHorizons", "The Edge of Infinite Horizons", "Sever the Worldwound away from reality with your sovereign Ki strike, slicing cleanly through the causal loop and ascending to infinite martial frontiers.");
			ObjectiveEnding_BoundaryHermit = CreateObjective("ObjectiveEnding_BoundaryHermit", "The Boundary Hermit", "Retire to a tranquil pocket dimension to meditate upon the eternal Dao of Ki until the causal loop resets.");
			ObjectiveEnding_SeveredHorizon = CreateObjective("ObjectiveEnding_SeveredHorizon", "The Severed Horizon", "Unleash a catastrophic eruption of unrestrained Ki that cleaves reality too deeply, collapsing the dimensional boundaries of Golarion.");
			TransmigrationQuest.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveAct3.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveAct4.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveAct5.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveAct6.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_UnwrittenDawn.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_InterdimensionalTraveler.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_EternalActor.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_PaxSepulchrum.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_IronCitadel.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_NecroTyrant.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_TempestFederation.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_PrimevalMarsh.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_AbyssalSingularity.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_EternalHope.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_RetiredLegend.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_CorruptedChampion.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_PaxSolaris.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_SarkorianImperium.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_DualThrone.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_SilentAegis.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_ObsidianBastion.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_NetherworldCataclysm.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_GrandArchitect.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_PuppetParliament.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_ChessboardApocalypse.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_InfiniteHorizons.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_BoundaryHermit.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveEnding_SeveredHorizon.ToReference<BlueprintQuestObjectiveReference>()
			};
		}

		private static BlueprintQuestObjective CreateObjective(string name, string title, string description)
		{
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
			{
				bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
				bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
				bp.m_Quest = TransmigrationQuest.ToReference<BlueprintQuestReference>();
				bp.m_Type = BlueprintQuestObjective.Type.Objective;
			});
		}

		public static void StartQuest()
		{
			try
			{
				QuestBook questBook = Game.Instance?.Player?.QuestBook;
				if (questBook != null && questBook.GetQuestState(TransmigrationQuest) == QuestState.None)
				{
					questBook.GiveObjective(ObjectiveAct1);
					PostLog("<color=#9400D3><b>[New Quest]</b></color> <b>The Outer Threshold: Reincarnation Mystery</b> has begun!");
					ConstellationChatManager.OnNewRun(TimelineManager.Data.TotalRuns);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error starting IsekaiTransmigrationQuest: " + ex);
			}
		}

		public static bool AreAllSideQuestsCompleted(out List<string> missingQuests)
		{
			missingQuests = new List<string>();
			QuestBook questBook = Game.Instance?.Player?.QuestBook;
			if (questBook != null)
			{
				if (questBook.GetObjectiveState(ObjectiveAct1) != QuestObjectiveState.Completed)
				{
					missingQuests.Add("Act 1 Rift: The Kenabres Void Horror");
				}
				if (questBook.GetObjectiveState(ObjectiveAct2) != QuestObjectiveState.Completed)
				{
					missingQuests.Add("Act 2 Rift: Kenji the Exiled Ronin");
				}
				if (questBook.GetObjectiveState(ObjectiveAct3) != QuestObjectiveState.Completed)
				{
					missingQuests.Add("Act 3 Rift: Malakor the Shadow Weaver");
				}
				if (questBook.GetObjectiveState(ObjectiveAct4) != QuestObjectiveState.Completed)
				{
					missingQuests.Add("Act 4 Rift: The Abyssal Rift Titan");
				}
				if (questBook.GetObjectiveState(ObjectiveAct5) != QuestObjectiveState.Completed)
				{
					missingQuests.Add("Act 5 Rift: Herald of the Closed Threshold");
				}
				if (questBook.GetObjectiveState(ObjectiveAct6) != QuestObjectiveState.Completed && !TimelineManager.Data.GrandArbiterDefeated)
				{
					missingQuests.Add("Act 6 Rift: Grand Arbiter of the Loop");
				}
			}
			else
			{
				for (int i = 1; i <= 6; i++)
				{
					if (!TimelineManager.Data.CompletedSideQuests.Contains($"TransmigrationQuest_Act{i}"))
					{
						missingQuests.Add($"Act {i} Rift Incursion");
					}
				}
			}
			if (!TimelineManager.Data.GrandArbiterDefeated && !TimelineManager.Data.CompletedSideQuests.Contains("TransmigrationQuest_Act6") && !missingQuests.Contains("Act 6 Rift: Grand Arbiter of the Loop"))
			{
				missingQuests.Add("Act 6 Rift: Grand Arbiter of the Loop");
			}
			if (!TimelineManager.Data.ArenaCompleted && !TimelineManager.Data.CompletedSideQuests.Contains("DimensionalArena_Act1FaeGrove"))
			{
				missingQuests.Add("Dimensional Arena: Primal Fae Grove (The Lantern King's Revelation)");
			}
			return missingQuests.Count == 0;
		}

		public static void CompleteAct(int act)
		{
			try
			{
				QuestBook questBook = Game.Instance?.Player?.QuestBook;
				if (questBook == null)
				{
					return;
				}
				BlueprintQuestObjective blueprintQuestObjective = null;
				BlueprintQuestObjective blueprintQuestObjective2 = null;
				switch (act)
				{
				case 1:
					blueprintQuestObjective = ObjectiveAct1;
					blueprintQuestObjective2 = ObjectiveAct2;
					break;
				case 2:
					blueprintQuestObjective = ObjectiveAct2;
					blueprintQuestObjective2 = ObjectiveAct3;
					break;
				case 3:
					blueprintQuestObjective = ObjectiveAct3;
					blueprintQuestObjective2 = ObjectiveAct4;
					break;
				case 4:
					blueprintQuestObjective = ObjectiveAct4;
					blueprintQuestObjective2 = ObjectiveAct5;
					break;
				case 5:
					blueprintQuestObjective = ObjectiveAct5;
					blueprintQuestObjective2 = ObjectiveAct6;
					break;
				case 6:
					blueprintQuestObjective = ObjectiveAct6;
					break;
				}
				if (blueprintQuestObjective != null)
				{
					questBook.CompleteObjective(blueprintQuestObjective);
					PostLog($"<color=#32CD32><b>[Quest Completed]</b></color> <b>{blueprintQuestObjective.Title}</b>!");
					if (!TimelineManager.Data.CompletedSideQuests.Contains($"TransmigrationQuest_Act{act}"))
					{
						TimelineManager.Data.CompletedSideQuests.Add($"TransmigrationQuest_Act{act}");
						TimelineManager.Save();
					}
				}
				if (blueprintQuestObjective2 != null)
				{
					questBook.GiveObjective(blueprintQuestObjective2);
					PostLog($"<color=#9400D3><b>[Quest Updated]</b></color> <b>{blueprintQuestObjective2.Title}</b>!");
					ConstellationChatManager.OnActProgression(act + 1);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error completing act in IsekaiTransmigrationQuest: " + ex);
			}
		}

		public static void TriggerEnding(string endingId, string endingTitle, bool shatterLoop = false)
		{
			try
			{
				UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
				string characterName = unitEntityData?.CharacterName ?? "The Otherworlder";
				string activeRunArchetype = TimelineManager.Data.ActiveRunArchetype;
				string mythicPath = "Legend";
				if (unitEntityData?.Progression?.Classes != null)
				{
					foreach (ClassData @class in unitEntityData.Progression.Classes)
					{
						if (@class?.CharacterClass != null && @class.CharacterClass.IsMythic)
						{
							mythicPath = @class.CharacterClass.LocalizedName?.ToString() ?? @class.CharacterClass.name;
							break;
						}
					}
				}
				string activeRomanceName = GetActiveRomanceName();
				TimelineManager.RecordCycleCompletion(endingId, endingTitle, activeRunArchetype, mythicPath, activeRomanceName, characterName);
				if (shatterLoop)
				{
					if (TimelineManager.Data.LoopShattered)
					{
						int totalRuns = TimelineManager.Data.TotalRuns;
						int num = 500 * totalRuns;
						DivineTokens.AddCoins(num, $"Multiverse Branch #{totalRuns}");
						PostLog($"<color=#00FFFF><b>[MULTIVERSE BRANCH #{totalRuns}]</b></color> You forge a new dimensional branch across the infinite cosmos! Awarded <b>+{num} Cosmic Coins</b> (500 * branchNumber)!");
					}
					TimelineManager.Data.LoopShattered = true;
					TimelineManager.Data.LoopBreakerAchieved = true;
					TimelineManager.Save();
					PostLog("<color=#F5C542><b>[TRUE ENDING: LOOP SHATTERED]</b></color> <b>" + endingTitle + "</b>! The causal anchors holding Golarion in the repeating cycle shatter into cosmic stardust. The timeline is unchained forever! Golarion and Earth are connected!");
				}
				else
				{
					if (endingId.IndexOf("Ascension", StringComparison.OrdinalIgnoreCase) >= 0 || endingTitle.IndexOf("Ascension", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						PostLog("<color=#FFD700><b>[ASCENSION: THE GOLDEN CAGE]</b></color> You ascended to godhood with your companions, reigning over a magnificent new pantheon. Yet without the 4 Keys of Causal Rupture, your divinity remains ensnared inside Yog-Sothoth's closed temporal loop. The celestial audience applauds your glorious, cyclical divinity!");
					}
					PostLog($"<color=#9400D3><b>[CYCLE RESOLUTION]</b></color> <b>{endingTitle}</b>! Your reign echoes across the era. Yet in the deep void, Yog-Sothoth's causal hourglass flips once more... (Cycle #{TimelineManager.Data.TotalRuns + 1} begins upon new playthrough).");
					TimelineManager.AdvanceLoop(endingTitle);
				}
				QuestBook questBook = Game.Instance?.Player?.QuestBook;
				if (questBook == null)
				{
					return;
				}
				questBook.CompleteObjective(ObjectiveAct6);
				if (TransmigrationQuest?.m_Objectives == null)
				{
					return;
				}
				foreach (BlueprintQuestObjectiveReference objective in TransmigrationQuest.m_Objectives)
				{
					BlueprintQuestObjective blueprintQuestObjective = objective?.Get();
					if (blueprintQuestObjective != null && blueprintQuestObjective.name.IndexOf(endingId, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						questBook.GiveObjective(blueprintQuestObjective);
						questBook.CompleteObjective(blueprintQuestObjective);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error triggering ending in IsekaiTransmigrationQuest: " + ex);
			}
		}

		private static string GetActiveRomanceName()
		{
			try
			{
				EtudesSystem etudesSystem = Game.Instance?.Player?.EtudesSystem;
				if (etudesSystem == null)
				{
					return "None";
				}
				foreach (BlueprintEtude startedEtude in etudesSystem.GetStartedEtudes())
				{
					string text = startedEtude?.name ?? "";
					if (text.IndexOf("CamelliaRomance", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return "Camellia";
					}
					if (text.IndexOf("ArueshalaeRomance", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return "Arueshalae";
					}
					if (text.IndexOf("WenduagRomance", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return "Wenduag";
					}
					if (text.IndexOf("LannRomance", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return "Lann";
					}
					if (text.IndexOf("DaeranRomance", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return "Daeran";
					}
					if (text.IndexOf("SosielRomance", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return "Sosiel";
					}
					if (text.IndexOf("GalfreyRomance", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return "Queen Galfrey";
					}
				}
			}
			catch
			{
			}
			return "None";
		}

		private static void PostLog(string message)
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(message);
			});
		}
	}
}
