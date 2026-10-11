using System;
using System.Collections.Generic;
using IsekaiMod.Config;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using IsekaiMod.Content.GlobalMap;
using Kingmaker;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Quests;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Quests;

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

	public static BlueprintQuestObjective ObjectiveEnding_ShadowSyndicate;

	public static BlueprintQuestObjective ObjectiveEnding_NpcSimulator;

	public static BlueprintQuestObjective ObjectiveEnding_InfiniteHorizons;

	public static BlueprintQuestObjective ObjectiveEnding_BoundaryHermit;

	public static BlueprintQuestObjective ObjectiveEnding_SeveredHorizon;

	internal static readonly string[,] RomanceActiveEtudes = new string[7, 2]
	{
		{ "6a3fdd0758fe78d4aa2c3b26d7614fbc", "Arueshalae" },
		{ "33c4c2f66f2461e4993df21566252079", "Wenduag" },
		{ "af394a43ba7c1314bb31aea74a5e4c0e", "Lann" },
		{ "8541453b31379964e834cf2309444388", "Daeran" },
		{ "efb130b8a22c9534ca40a1e41ef8e931", "Sosiel" },
		{ "c9358d866e0b3844b8d72536ca60e4b4", "Queen Galfrey" },
		{ "14650554734f4adc81af61e442fd628b", "Ulbrig" }
	};

	public static void Add()
	{
		TransmigrationQuest = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiTransmigrationQuest", delegate(BlueprintQuest bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, "IsekaiTransmigrationQuest.Title", "The Outer Threshold: Transmigration Mystery");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "IsekaiTransmigrationQuest.Description", "You were pulled across planar dimensions and time by Yog-Sothoth, the Key and the Gate.\nAcross each phase of your crusade, tears in the dimensional fabric manifest in the form of otherworld anomalies and deadly incursions, and each chapter opens a new arena of the Planar Coliseum.\nUnravel the truth behind your transmigration, the watching deities, and the closed temporal loop holding Golarion hostage. Only a champion who has sealed every rift and won every arena's cup can break it.");
			bp.CompletionText = Helpers.CreateString(Main.IsekaiContext, "IsekaiTransmigrationQuest.CompletionText", "Your choice at the Threshold has closed this cycle, and the Timeline has recorded how it ended.");
			bp.m_Group = QuestGroupId.None;
			bp.m_Type = QuestType.Normal;
			bp.m_LastChapter = 6;
		});
		ObjectiveAct1 = CreateObjective("IsekaiQuestObjective_Act1", "A Flaw in the Script: Investigate the Kenabres Rift Anomaly", "A dimensional rift has cracked open reality near Kenabres. Follow the Dimensional Breach on the map beside Defender's Heart, or ask the Otherworldly Emissary to open the way, and confront the entity emerging from the void. Notably, the observing deities talk as if they've seen this play out before.");
		ObjectiveAct2 = CreateObjective("IsekaiQuestObjective_Act2", "The Discarded Iteration: Confront Kenji the Exiled Ronin", "Another transmigrated warrior, Kenji, a masterless swordsman from an eastern realm, has crossed into the Worldwound. The Dimensional Breach beside Kenabres on the Worldwound map leads to him, and the Otherworldly Emissary can open the way from any town. Defeat him to uncover his echo journal detailing previous forgotten crusades.");
		ObjectiveAct3 = CreateObjective("IsekaiQuestObjective_Act3", "Behind the Velvet Curtain: The Shadow at the Rift Nexus", "A shadowy planar horror named Malakor is drawing power from the rift nexus. The Dimensional Breach on the Worldwound map leads to him, and the Otherworldly Emissary can open the way from any town. Destroy him and learn what the rifts have to do with your transmigration.");
		ObjectiveAct4 = CreateObjective("IsekaiQuestObjective_Act4", "The Key of Causal Rupture: Slay the Abyssal Rift Titan", "Deep in the Abyss, the rift disruption has manifested an immense titan. In the Midnight Isles the Otherworldly Emissary opens the breach from Alushinyrra's cities; back on Golarion, the Dimensional Breach on the Worldwound map leads there. Claim the Key of Causal Rupture, rejecting Yog-Sothoth's whisper to remain comfortably within the endless loop.");
		ObjectiveAct5 = CreateObjective("IsekaiQuestObjective_Act5", "Breach the Perimeter: Defeat the Herald of the Closed Threshold", "At the precipice of the Threshold, the final guardian of the multiversal barrier awaits. The Dimensional Breach on the Worldwound map leads there, and at the Threshold the Otherworldly Emissary opens it. Defeat the Herald to unlock the core mechanism of the temporal cycle.");
		ObjectiveAct6 = CreateObjective("IsekaiQuestObjective_Act6", "Shatter the Infinite Loop: The Unwritten Dawn", "Using the Key of Causal Rupture, confront the Grand Arbiter of the Loop. Only at the Threshold does the last breach reach him: the Otherworldly Emissary opens it there once the loop's requirements are met. Decide whether to remain the adored immortal star of the celestial stream, or shatter the loop forever to claim true unscripted freedom.");
		LinkLocation(ObjectiveAct1, BossSiteMapPoints.KenabresBreach);
		for (int num = 1; num <= 5; num++)
		{
			LinkLocation(ObjectiveForAct(num), BossSiteMapPoints.WorldWoundBreach);
		}
		ObjectiveEnding_UnwrittenDawn = CreateEndingObjective("ObjectiveEnding_UnwrittenDawn", "The Unwritten Dawn (True Loop-Breaker)", "Shatter Yog-Sothoth's causal anchors. Break the loop forever and open an interdimensional gateway between Golarion and Earth.");
		ObjectiveEnding_InterdimensionalTraveler = CreateEndingObjective("ObjectiveEnding_InterdimensionalTraveler", "The Interdimensional Traveler", "Leave the Worldwound in mortal hands and step into the dimensional slipstream to explore the Great Beyond.");
		ObjectiveEnding_EternalActor = CreateEndingObjective("ObjectiveEnding_EternalActor", "The Eternal Actor", "Surrender to the Constellation broadcast, choosing comfortable eternal celebrity over unknown reality.");
		ObjectiveEnding_PaxSepulchrum = CreateEndingObjective("ObjectiveEnding_PaxSepulchrum", "Pax Sepulchrum - Guardian of the Great Tomb", "Extend an impartial, unyielding undead protectorate over mortal subjects where the dead labor and the living thrive.");
		ObjectiveEnding_IronCitadel = CreateEndingObjective("ObjectiveEnding_IronCitadel", "Dominion of the Iron Citadel", "Fortify Sarkoris behind obsidian ramparts, declaring an isolated, sovereign necropolis.");
		ObjectiveEnding_NecroTyrant = CreateEndingObjective("ObjectiveEnding_NecroTyrant", "The Eternal Necro-Tyrant", "Deem mortal life a chaotic inefficiency; harvest all living souls into the disciplined undead legion.");
		ObjectiveEnding_TempestFederation = CreateEndingObjective("ObjectiveEnding_TempestFederation", "The Tempest Multiverse Federation", "Metabolize the Worldwound rift into fertile mana, founding a multiversal haven of beasts, monsters, and mortals.");
		ObjectiveEnding_PrimevalMarsh = CreateEndingObjective("ObjectiveEnding_PrimevalMarsh", "The Primeval Marsh", "Restore the Worldwound into an untamed, primeval wildland, sleeping beneath the World Tree as Golarion's guardian beast-god.");
		ObjectiveEnding_AbyssalSingularity = CreateEndingObjective("ObjectiveEnding_AbyssalSingularity", "The Abyssal Singularity", "Plunge into the Abyss and devour demon lords until stirring Azathoth, forcing the causal loop to reset.");
		ObjectiveEnding_EternalHope = CreateEndingObjective("ObjectiveEnding_EternalHope", "The Sovereign Beacon of Eternal Hope", "Channel the collective hope of all mortals into an undying celestial nova, ascending as the eternal protector.");
		ObjectiveEnding_RetiredLegend = CreateEndingObjective("ObjectiveEnding_RetiredLegend", "The Retired Legend", "Refuse godhood and political thrones, opening a training guild and tavern in Kenabres to live as a peaceful mortal.");
		ObjectiveEnding_CorruptedChampion = CreateEndingObjective("ObjectiveEnding_CorruptedChampion", "The Corrupted Champion", "Live long enough to become the villain; consumed by paranoia, blood on your hands, and madness.");
		ObjectiveEnding_PaxSolaris = CreateEndingObjective("ObjectiveEnding_PaxSolaris", "Pax Solaris - The Golden Renaissance", "Establish the Golden Throne of Sarkoris, becoming the eternal God-King of humanity and the multiverse.");
		ObjectiveEnding_SarkorianImperium = CreateEndingObjective("ObjectiveEnding_SarkorianImperium", "The Sarkorian Imperium", "Fortify northern Avistan into an unassailable imperial domain prioritizing defense, trade dominance, and order.");
		ObjectiveEnding_DualThrone = CreateEndingObjective("ObjectiveEnding_DualThrone", "The Dual Throne of Sun and Abyss", "March solar legions into the Abyss, conquering both mortals and fiends under absolute solar fealty.");
		ObjectiveEnding_SilentAegis = CreateEndingObjective("ObjectiveEnding_SilentAegis", "The Silent Aegis of Shadows", "Command the infinite Legion of Shadows to guard Golarion from the dark without oppressing mortals.");
		ObjectiveEnding_ObsidianBastion = CreateEndingObjective("ObjectiveEnding_ObsidianBastion", "The Obsidian Bastion", "Raise the Shadow Gate at the boundary between the planes and sit upon its threshold as the eternal gatekeeper, so neither demon nor conqueror crosses.");
		ObjectiveEnding_NetherworldCataclysm = CreateEndingObjective("ObjectiveEnding_NetherworldCataclysm", "The Netherworld Cataclysm", "Open a permanent portal to the Shadow Plane where the Worldwound used to be.");
		ObjectiveEnding_GrandArchitect = CreateEndingObjective("ObjectiveEnding_GrandArchitect", "The Immortal Philanthropist", "Use discovered immortality and foresight to do genuine good across centuries from behind the curtain.");
		ObjectiveEnding_ShadowSyndicate = CreateEndingObjective("ObjectiveEnding_ShadowSyndicate", "The Shadow Syndicate", "Erase personal records from history, becoming an elusive global information broker.");
		ObjectiveEnding_NpcSimulator = CreateEndingObjective("ObjectiveEnding_NpcSimulator", "The NPC Simulator", "Strip all emotion from calculation, viewing companions, crusaders, and demons as mere NPCs in a private game.");
		ObjectiveEnding_InfiniteHorizons = CreateEndingObjective("ObjectiveEnding_InfiniteHorizons", "The Edge of Infinite Horizons", "Slice the Worldwound away from reality, ascending into the Astral Plane as the God of the Sword.");
		ObjectiveEnding_BoundaryHermit = CreateEndingObjective("ObjectiveEnding_BoundaryHermit", "The Boundary Hermit", "Retire to a tranquil pocket dimension to perfect the transcendent strike until the loop resets.");
		ObjectiveEnding_SeveredHorizon = CreateEndingObjective("ObjectiveEnding_SeveredHorizon", "The Severed Horizon", "Slice reality too deeply, accidentally severing the metaphysical thread anchoring Golarion to existence.");
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
			ObjectiveEnding_ShadowSyndicate.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveEnding_NpcSimulator.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveEnding_InfiniteHorizons.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveEnding_BoundaryHermit.ToReference<BlueprintQuestObjectiveReference>(),
			ObjectiveEnding_SeveredHorizon.ToReference<BlueprintQuestObjectiveReference>()
		};
	}

	private static void LinkLocation(BlueprintQuestObjective objective, BlueprintGlobalMapPoint tile)
	{
		if (objective != null && tile != null)
		{
			GlobalMapVariationManager.LinkObjectiveToMapPoint(objective, tile);
		}
	}

	private static BlueprintQuestObjective CreateObjective(string name, string title, string description)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
			bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			bp.m_Quest = TransmigrationQuest.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = false;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
		});
	}

	private static BlueprintQuestObjective CreateEndingObjective(string name, string title, string description)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
			bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			bp.m_Quest = TransmigrationQuest.ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = true;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
		});
	}

	internal static bool CanStartTransmigration(bool hasRiftFeature, int isekaiClassLevel)
	{
		if (!hasRiftFeature)
		{
			return isekaiClassLevel > 0;
		}
		return true;
	}

	internal static bool PlayerCanStartTransmigration()
	{
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (unitEntityData?.Descriptor == null)
		{
			return false;
		}
		BlueprintFeature blueprintFeature = DimensionalRiftIncursions.DimensionalRiftsFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DimensionalRiftsFeature");
		bool hasRiftFeature = blueprintFeature != null && unitEntityData.Descriptor.Progression.Features.HasFact(blueprintFeature);
		int isekaiClassLevel = 0;
		BlueprintCharacterClass modBlueprint = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "IsekaiProtagonistClass");
		if (modBlueprint != null && unitEntityData.Descriptor.Progression != null)
		{
			isekaiClassLevel = unitEntityData.Descriptor.Progression.GetClassLevel(modBlueprint);
		}
		return CanStartTransmigration(hasRiftFeature, isekaiClassLevel);
	}

	public static void StartQuest()
	{
		try
		{
			if (Game.Instance?.Player?.QuestBook != null && PlayerCanStartTransmigration())
			{
				QuestBook questBook = Game.Instance.Player.QuestBook;
				if (questBook.GetQuestState(TransmigrationQuest) == QuestState.None)
				{
					questBook.GiveObjective(ObjectiveAct1);
					PostLog(ConstellationText.Localized("Constellation.Transmigration", "<color=#5B2A86><b>[New Quest]</b></color> <b>The Outer Threshold: Transmigration Mystery</b> has begun!"));
					ConstellationChatManager.OnNewRun(TimelineManager.Data.TotalRuns);
				}
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
		string[] array = new string[5] { "Act 1 Rift: The Kenabres Void Horror", "Act 2 Rift: Kenji the Exiled Ronin", "Act 3 Rift: Malakor the Shadow Weaver", "Act 4 Rift: The Abyssal Rift Titan", "Act 5 Rift: Herald of the Closed Threshold" };
		for (int i = 1; i <= array.Length; i++)
		{
			BlueprintQuestObjective blueprintQuestObjective = ObjectiveForAct(i);
			if (questBook == null || blueprintQuestObjective == null || questBook.GetObjectiveState(blueprintQuestObjective) != QuestObjectiveState.Completed)
			{
				missingQuests.Add(array[i - 1]);
			}
		}
		missingQuests.AddRange(IsekaiColiseumQuest.MissingArenaRequirements());
		return missingQuests.Count == 0;
	}

	public static BlueprintQuestObjective ObjectiveForAct(int act)
	{
		return act switch
		{
			1 => ObjectiveAct1, 
			2 => ObjectiveAct2, 
			3 => ObjectiveAct3, 
			4 => ObjectiveAct4, 
			5 => ObjectiveAct5, 
			6 => ObjectiveAct6, 
			_ => null, 
		};
	}

	public static QuestObjectiveState[] ActStates()
	{
		QuestObjectiveState[] array = new QuestObjectiveState[6];
		QuestBook questBook = Game.Instance?.Player?.QuestBook;
		if (questBook == null)
		{
			return array;
		}
		for (int i = 1; i <= array.Length; i++)
		{
			BlueprintQuestObjective blueprintQuestObjective = ObjectiveForAct(i);
			array[i - 1] = ((blueprintQuestObjective != null) ? questBook.GetObjectiveState(blueprintQuestObjective) : QuestObjectiveState.None);
		}
		return array;
	}

	public static QuestState QuestStateInSave()
	{
		QuestBook questBook = Game.Instance?.Player?.QuestBook;
		if (questBook == null || TransmigrationQuest == null)
		{
			return QuestState.None;
		}
		return questBook.GetQuestState(TransmigrationQuest);
	}

	public static bool CanSettleAct(int act)
	{
		try
		{
			if (ObjectiveForAct(act) == null)
			{
				return false;
			}
			return DimensionalRiftIncursions.ShouldSettleKill(QuestStateInSave(), ActStates()[act - 1]);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error reading rift act state: " + ex);
			return false;
		}
	}

	public static void CompleteAct(int act)
	{
		try
		{
			if (Game.Instance?.Player?.QuestBook != null)
			{
				QuestBook questBook = Game.Instance.Player.QuestBook;
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
					PostLog(ConstellationText.LocalizedFormat("Constellation.Transmigration", "<color=#004604><b>[Quest Completed]</b></color> <b>{0}</b>!", blueprintQuestObjective.Title));
					ConstellationChatManager.OnActProgression(act);
				}
				if (blueprintQuestObjective2 != null)
				{
					questBook.GiveObjective(blueprintQuestObjective2);
					PostLog(ConstellationText.LocalizedFormat("Constellation.Transmigration", "<color=#5B2A86><b>[Quest Updated]</b></color> <b>{0}</b>!", blueprintQuestObjective2.Title));
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error completing act in IsekaiTransmigrationQuest: " + ex);
		}
	}

	public static BlueprintQuestObjective ObjectiveForEnding(string endingId)
	{
		return endingId switch
		{
			"UnwrittenDawn" => ObjectiveEnding_UnwrittenDawn, 
			"InterdimensionalTraveler" => ObjectiveEnding_InterdimensionalTraveler, 
			"EternalActor" => ObjectiveEnding_EternalActor, 
			"PaxSepulchrum" => ObjectiveEnding_PaxSepulchrum, 
			"IronCitadel" => ObjectiveEnding_IronCitadel, 
			"NecroTyrant" => ObjectiveEnding_NecroTyrant, 
			"TempestFederation" => ObjectiveEnding_TempestFederation, 
			"PrimevalMarsh" => ObjectiveEnding_PrimevalMarsh, 
			"AbyssalSingularity" => ObjectiveEnding_AbyssalSingularity, 
			"EternalHope" => ObjectiveEnding_EternalHope, 
			"RetiredLegend" => ObjectiveEnding_RetiredLegend, 
			"CorruptedChampion" => ObjectiveEnding_CorruptedChampion, 
			"PaxSolaris" => ObjectiveEnding_PaxSolaris, 
			"SarkorianImperium" => ObjectiveEnding_SarkorianImperium, 
			"DualThrone" => ObjectiveEnding_DualThrone, 
			"SilentAegis" => ObjectiveEnding_SilentAegis, 
			"ObsidianBastion" => ObjectiveEnding_ObsidianBastion, 
			"NetherworldCataclysm" => ObjectiveEnding_NetherworldCataclysm, 
			"GrandArchitect" => ObjectiveEnding_GrandArchitect, 
			"ShadowSyndicate" => ObjectiveEnding_ShadowSyndicate, 
			"NpcSimulator" => ObjectiveEnding_NpcSimulator, 
			"InfiniteHorizons" => ObjectiveEnding_InfiniteHorizons, 
			"BoundaryHermit" => ObjectiveEnding_BoundaryHermit, 
			"SeveredHorizon" => ObjectiveEnding_SeveredHorizon, 
			_ => null, 
		};
	}

	internal static bool IsLoopBreakingEnding(string endingId)
	{
		switch (endingId)
		{
		case "PaxSepulchrum":
		case "UnwrittenDawn":
		case "EternalHope":
		case "SilentAegis":
		case "TempestFederation":
		case "PaxSolaris":
		case "GrandArchitect":
		case "InfiniteHorizons":
			return true;
		default:
			return false;
		}
	}

	internal static string CycleRecordMessage(string endingTitle, string finalFrame, int totalRuns)
	{
		return ConstellationText.LocalizedFormat("Constellation.Transmigration", "<color=#5B2A86><b>[CYCLE RESOLUTION]</b></color> <b>{0}</b>! Whatever the histories of this era went on to say, this is the frame the recording stops on: {1} The reel ends there. In the deep void Yog-Sothoth's causal hourglass flips, this cycle is shelved with all the others, and the era begins again with nothing of it kept. (Cycle #{2} begins upon new playthrough).", endingTitle, finalFrame, totalRuns);
	}

	internal static string CycleResolutionMessage(string endingId, string endingTitle, int totalRuns)
	{
		return endingId switch
		{
			"SeveredHorizon" => CycleRecordMessage(endingTitle, ConstellationText.Localized("Constellation.Transmigration", "the promised cut lands, the metaphysical thread anchoring Golarion parts, and the world comes apart like silk into glowing iridescent static, with no one left standing anywhere to remember your name."), totalRuns), 
			"AbyssalSingularity" => CycleRecordMessage(endingTitle, ConstellationText.Localized("Constellation.Transmigration", "you go down into the Abyss and eat it to its last demon lord, and the timeline folds inward exactly as you intended, until there is nothing further left to collapse."), totalRuns), 
			"NetherworldCataclysm" => CycleRecordMessage(endingTitle, ConstellationText.Localized("Constellation.Transmigration", "the Netherworld opens across northern Avistan, the sun does not rise again, and the continent goes silent under a sovereign dark where only your shadows walk."), totalRuns), 
			_ => ConstellationText.LocalizedFormat("Constellation.Transmigration", "<color=#5B2A86><b>[CYCLE RESOLUTION]</b></color> <b>{0}</b>! Your legend echoes across the era. Yet in the deep void, Yog-Sothoth's causal hourglass flips once more... (Cycle #{1} begins upon new playthrough).", endingTitle, totalRuns), 
		};
	}

	public static void TriggerEnding(string endingId, string endingTitle)
	{
		try
		{
			BlueprintQuestObjective blueprintQuestObjective = ObjectiveForEnding(endingId);
			if (blueprintQuestObjective == null)
			{
				Main.IsekaiContext.Logger.LogError("[IsekaiTransmigrationQuest] Unknown ending id: " + endingId);
				return;
			}
			TimelineData data = TimelineManager.Data;
			string gameId = TimelineManager.CurrentSaveId();
			bool flag = IsLoopBreakingEnding(endingId);
			bool shatterRequirementsMet = AreAllSideQuestsCompleted(out var missingQuests);
			CycleClosure cycleClosure = TimelineManager.DecideCycleClosure(data.ClosedCycles, gameId, flag, shatterRequirementsMet);
			if (cycleClosure == CycleClosure.AlreadyClosed)
			{
				TimelineManager.TryGetClosedCycle(data.ClosedCycles, gameId, out var endingId2, out var shattered);
				BlueprintQuestObjective blueprintQuestObjective2 = ObjectiveForEnding(endingId2) ?? blueprintQuestObjective;
				Quiet(ConstellationText.LocalizedFormat("Constellation.Transmigration", "<color=#3F3F3F><b>[CYCLE ALREADY CLOSED]</b></color> This playthrough's cycle was already closed with <b>{0}</b>. The Timeline keeps that ending and records nothing twice.", (string)blueprintQuestObjective2.Title));
				FinaleVoice(ConstellationDialogueBanter.AlreadyClosedRemark());
				CompleteEndingInJournal(blueprintQuestObjective2, shattered);
				return;
			}
			UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
			string characterName = unitEntityData?.CharacterName ?? "The Transmigrator";
			string archetype = ArchetypeOrigin.DisplayName(TimelineManager.LookupRunArchetype(data.RunArchetypes, gameId));
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
			List<string> list = HeldLegacyNames(unitEntityData);
			TimelineManager.CloseCycle(gameId, cycleClosure, endingId, endingTitle, archetype, mythicPath, activeRomanceName, characterName, (list.Count > 0) ? list[0] : "", (list.Count > 1) ? list[1] : "");
			if (cycleClosure == CycleClosure.Shattered)
			{
				string text = ((endingId == "UnwrittenDawn") ? ConstellationText.Localized("Constellation.Transmigration", " Golarion and Earth are connected!") : "");
				PostLog(ConstellationText.LocalizedFormat("Constellation.Transmigration", "<color=#5B2A86><b>[TRUE ENDING: LOOP SHATTERED]</b></color> <b>{0}</b>! The causal anchors holding Golarion in the repeating cycle shatter into cosmic stardust. The timeline is unchained forever!{1}", endingTitle, text));
			}
			else
			{
				if (flag)
				{
					AddedContent addedContent = Main.IsekaiContext.AddedContent;
					Quiet((addedContent == null || addedContent.EnableDimensionalArena) ? ConstellationText.Localized("Constellation.Transmigration", "<color=#7A1010><b>[THE LOOP HOLDS]</b></color> The causal anchors resist: the loop can only be shattered once every rift act is sealed and every arena of the Planar Coliseum is won.") : ConstellationText.Localized("Constellation.Transmigration", "<color=#7A1010><b>[THE LOOP HOLDS]</b></color> The causal anchors resist: the loop can only be shattered once every rift act is sealed."));
					foreach (string item in missingQuests)
					{
						Quiet(ConstellationText.LocalizedFormat("Constellation.Transmigration", "  <color=#7A1010>- {0}</color>", item));
					}
				}
				PostLog(CycleResolutionMessage(endingId, endingTitle, TimelineManager.Data.TotalRuns));
			}
			PostEndingBroadcast(endingId, endingTitle, flag, cycleClosure == CycleClosure.Shattered);
			CompleteEndingInJournal(blueprintQuestObjective, cycleClosure == CycleClosure.Shattered);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error triggering ending in IsekaiTransmigrationQuest: " + ex);
		}
	}

	private static void CompleteEndingInJournal(BlueprintQuestObjective endingObjective, bool shattered)
	{
		QuestBook questBook = Game.Instance?.Player?.QuestBook;
		if (questBook == null || endingObjective == null)
		{
			return;
		}
		QuestState questState = QuestStateInSave();
		if (questState != QuestState.Completed && questState != QuestState.Failed)
		{
			if (shattered && ObjectiveAct6 != null && questBook.GetObjectiveState(ObjectiveAct6) == QuestObjectiveState.Started)
			{
				questBook.CompleteObjective(ObjectiveAct6);
			}
			questBook.GiveObjective(endingObjective);
			questBook.CompleteObjective(endingObjective);
		}
	}

	internal static string RomanceName(Func<string, bool> romanceActive)
	{
		if (romanceActive == null)
		{
			return "None";
		}
		for (int i = 0; i < RomanceActiveEtudes.GetLength(0); i++)
		{
			if (romanceActive(RomanceActiveEtudes[i, 0]))
			{
				return RomanceActiveEtudes[i, 1];
			}
		}
		return "None";
	}

	internal static List<string> HeldLegacyNames(UnitEntityData unit)
	{
		List<string> list = new List<string>();
		try
		{
			BlueprintFeatureSelection classFeature = LegacySelection.GetClassFeature();
			if (unit?.Descriptor?.Progression == null || classFeature == null)
			{
				return list;
			}
			HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
			foreach (BlueprintFeature allFeature in classFeature.AllFeatures)
			{
				if (allFeature != null)
				{
					hashSet.Add(allFeature.AssetGuid);
				}
			}
			foreach (Feature item in unit.Descriptor.Progression.Features.Enumerable)
			{
				BlueprintFeature blueprintFeature = item?.Blueprint;
				if (blueprintFeature != null && hashSet.Contains(blueprintFeature.AssetGuid))
				{
					string name = blueprintFeature.Name;
					if (string.IsNullOrEmpty(name))
					{
						name = blueprintFeature.name;
					}
					if (!list.Contains(name))
					{
						list.Add(name);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[IsekaiTransmigrationQuest] could not read the character's legacies for the timeline: " + ex);
		}
		return list;
	}

	private static string GetActiveRomanceName()
	{
		try
		{
			EtudesSystem es = Game.Instance?.Player?.EtudesSystem;
			if (es == null)
			{
				return "None";
			}
			return RomanceName(delegate(string guid)
			{
				BlueprintEtude blueprint = BlueprintTools.GetBlueprint<BlueprintEtude>(guid);
				return blueprint != null && es.EtudeIsStarted(blueprint);
			});
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[IsekaiTransmigrationQuest] Could not read the romance for the cycle record: " + ex);
		}
		return "None";
	}

	private static void PostLog(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, message, null, ConstellationCategory.Quest, "The Outer Threshold");
	}

	private static void PostEndingBroadcast(string endingId, string endingTitle, bool loopBreaking, bool shattered)
	{
		try
		{
			EndingLoopOutcome outcome = ConstellationDialogueBanter.DecideEndingOutcome(endingId, loopBreaking, shattered);
			string patronSeat = ConstellationChatManager.SponsorForPlayerDeity();
			List<string> list = new List<string>();
			foreach (string key in ConstellationDisfavor.SeatAlignment.Keys)
			{
				if (ConstellationChatManager.SeatIsOffended(key))
				{
					list.Add(key);
				}
			}
			foreach (string item in ConstellationDialogueBanter.GetEndingBanter(endingId, endingTitle, outcome, patronSeat, list))
			{
				FinaleVoice(item);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error posting the ending broadcast in IsekaiTransmigrationQuest: " + ex);
		}
	}

	private static void FinaleVoice(string message)
	{
		ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, message, null, ConstellationCategory.MetaLoop, "The Grand Final", 60f);
	}

	private static void Quiet(string message)
	{
		ConstellationChatManager.PostCombatLog(message);
	}
}
