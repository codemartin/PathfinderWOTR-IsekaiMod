using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Quests;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

public class AnimeStoryMilestonesHandler : IUnitFinallyDeadHandler, ISubscriber, IGlobalSubscriber, IAreaHandler, IQuestObjectiveHandler, IUnitChangedAfterRespecHandler
{
	internal sealed class MilestoneGate
	{
		public string FeatureName;

		public string BookItemName;

		public string Announcement;

		public string ObjectiveGuid;

		public bool AcceptStarted;

		public string AreaGuid;

		public string UnitGuid;
	}

	internal enum MilestoneAction
	{
		Nothing,
		Award,
		Restore,
		RecordOnly
	}

	internal const string ChronicleTagName = "AnimeMilestone.ChronicleTag";

	internal const string AnnouncementName = "AnimeMilestone.Announcement";

	internal static readonly MilestoneGate[] Gates = new MilestoneGate[12]
	{
		new MilestoneGate
		{
			FeatureName = "MilestoneShieldMazeFeature",
			BookItemName = "ItemSkillBookFirstCrusaders",
			Announcement = "Shield Maze Conquered",
			ObjectiveGuid = "1992ddc7d1b615748a0652a964846d4c"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneWaterElementalFeature",
			BookItemName = null,
			Announcement = "Shield Maze Apex Predator Slayed",
			UnitGuid = "4518955732be42ecbe41221001163c2a"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneGrayGarrisonFeature",
			BookItemName = "ItemSkillBookRadiantWard",
			Announcement = "Gray Garrison Liberated",
			ObjectiveGuid = "388790cbbec808e4487f547e7d805cc9"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneVescavorQueenFeature",
			BookItemName = "ItemSkillBookPrimalBeastmaster",
			Announcement = "Leper's Smile Purged",
			ObjectiveGuid = "ef480c0bdbb133e4b8aca81a71720021",
			AcceptStarted = true
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneLostChapelFeature",
			BookItemName = "ItemSkillBookUnbrokenSoul",
			Announcement = "Lost Chapel Reclaimed",
			ObjectiveGuid = "1c234d7992082f14aadd349e49e989b1"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneDrezenCitadelFeature",
			BookItemName = "ItemSkillBookSwarmTransmutation",
			Announcement = "Drezen Citadel Captured",
			ObjectiveGuid = "2882c4f49ca2def48b466cfcfd6a2aad"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneIvorySanctumFeature",
			BookItemName = "ItemSkillBookPlanarInfiltration",
			Announcement = "Ivory Sanctum Infiltrated",
			AreaGuid = "982abcee3e7b25f459bef22ea22b3ab5"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneMidnightFaneFeature",
			BookItemName = "ItemSkillBookAreeluAxiom",
			Announcement = "Midnight Fane Breached",
			ObjectiveGuid = "bcf5a9f544fd8bb46a1562dcde735893"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneRedDragonFeature",
			BookItemName = null,
			Announcement = "Ancient Dragon Devarra Hunted",
			UnitGuid = "b01da68dab56e004f952d7ad0e83cc46"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneColyphyrFeature",
			BookItemName = "ItemSkillBookMythicMineralogy",
			Announcement = "Colyphyr Mines Exploited",
			ObjectiveGuid = "f04f3fb286872ec42a575f3c231302cf"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneBaphometFeature",
			BookItemName = "ItemSkillBookRunelordLexicon",
			Announcement = "Demon Lord Confronted",
			ObjectiveGuid = "419f0f0ad936d3b4b842e94a4ced2902"
		},
		new MilestoneGate
		{
			FeatureName = "MilestoneThresholdFeature",
			BookItemName = null,
			Announcement = "Threshold of the Multiverse Reached",
			ObjectiveGuid = "5f0fa112e73203947a3717db16d3106e",
			AcceptStarted = true
		}
	};

	internal const string MilestoneStorageName = "IsekaiMod.StoryMilestones";

	private static readonly string[] OriginMilestoneNames = new string[28]
	{
		"MilestoneSlime_ShieldMazeFeature", "MilestoneSlime_WaterElementalFeature", "MilestoneSlime_GrayGarrisonFeature", "MilestoneSlime_DrezenCitadelFeature", "MilestoneOverlord_ShieldMazeFeature", "MilestoneOverlord_WaterElementalFeature", "MilestoneOverlord_GrayGarrisonFeature", "MilestoneOverlord_DrezenCitadelFeature", "MilestoneShadow_ShieldMazeFeature", "MilestoneShadow_WaterElementalFeature",
		"MilestoneShadow_GrayGarrisonFeature", "MilestoneShadow_DrezenCitadelFeature", "MilestoneHero_ShieldMazeFeature", "MilestoneHero_WaterElementalFeature", "MilestoneHero_GrayGarrisonFeature", "MilestoneHero_DrezenCitadelFeature", "MilestoneGod_ShieldMazeFeature", "MilestoneGod_WaterElementalFeature", "MilestoneGod_GrayGarrisonFeature", "MilestoneGod_DrezenCitadelFeature",
		"MilestoneMastermind_ShieldMazeFeature", "MilestoneMastermind_WaterElementalFeature", "MilestoneMastermind_GrayGarrisonFeature", "MilestoneMastermind_DrezenCitadelFeature", "MilestonePhantom_ShieldMazeFeature", "MilestonePhantom_WaterElementalFeature", "MilestonePhantom_GrayGarrisonFeature", "MilestonePhantom_DrezenCitadelFeature"
	};

	public void HandleUnitBecameFinallyDead(UnitEntityData unit)
	{
		CheckDeadUnit(unit);
	}

	public void OnAreaDidLoad()
	{
		CheckMilestones();
	}

	public void OnAreaBeginUnloading()
	{
	}

	public void HandleQuestObjectiveStarted(QuestObjective objective)
	{
		CheckMilestones();
	}

	public void HandleQuestObjectiveBecameVisible(QuestObjective objective)
	{
	}

	public void HandleQuestObjectiveCompleted(QuestObjective objective)
	{
		CheckMilestones();
	}

	public void HandleQuestObjectiveFailed(QuestObjective objective)
	{
	}

	public void HandleUnitChangedAfterRespec(UnitEntityData unit)
	{
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (unit != null && unit == unitEntityData)
		{
			CheckMilestones();
		}
	}

	public static string GetActiveChroniclePrefix(UnitEntityData mainChar, out string chronicleTag)
	{
		chronicleTag = "[Chronicle]";
		if (mainChar?.Descriptor == null)
		{
			return null;
		}
		UnitDescriptor descriptor = mainChar.Descriptor;
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GluttonyCompendiumFeature")))
		{
			chronicleTag = "[Predator's Compendium]";
			return "Slime";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TombAnnalsFeature")))
		{
			chronicleTag = "[Grimoire of Conquest]";
			return "Overlord";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HuntersLogFeature")))
		{
			chronicleTag = "[Hunter's Log]";
			return "Shadow";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroChronicleFeature")))
		{
			chronicleTag = "[Hero's Odyssey]";
			return "Hero";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialEdictsFeature")))
		{
			chronicleTag = "[Imperial Edicts]";
			return "God";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GrandStrategyFeature")))
		{
			chronicleTag = "[Grand Strategy]";
			return "Mastermind";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SoulArchiveFeature")))
		{
			chronicleTag = "[Soul Archive]";
			return "Phantom";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature")))
		{
			chronicleTag = "[Otherworld Chronicle]";
			return "";
		}
		return null;
	}

	internal static bool IsObjectiveStateEnough(QuestObjectiveState state, bool acceptStarted)
	{
		if (state == QuestObjectiveState.Completed)
		{
			return true;
		}
		if (acceptStarted)
		{
			return state == QuestObjectiveState.Started;
		}
		return false;
	}

	internal static bool MatchesDeath(MilestoneGate gate, string unitBlueprintGuid)
	{
		if (gate == null || string.IsNullOrEmpty(gate.UnitGuid))
		{
			return false;
		}
		return string.Equals(gate.UnitGuid, unitBlueprintGuid, StringComparison.OrdinalIgnoreCase);
	}

	internal static string AwardedKey(string featureName)
	{
		return "Awarded." + featureName;
	}

	internal static string ReachedKey(string featureName)
	{
		return "Reached." + featureName;
	}

	internal static MilestoneAction DecideMilestone(bool reached, bool awarded, bool hasFeature)
	{
		if (awarded)
		{
			if (!hasFeature)
			{
				return MilestoneAction.Restore;
			}
			return MilestoneAction.Nothing;
		}
		if (hasFeature)
		{
			return MilestoneAction.RecordOnly;
		}
		if (!reached)
		{
			return MilestoneAction.Nothing;
		}
		return MilestoneAction.Award;
	}

	internal static bool GrantsBook(MilestoneAction action)
	{
		return action == MilestoneAction.Award;
	}

	private static void RecordReached(MilestoneGate gate)
	{
		SaveScopedStorage.SetFlag(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.StoryMilestones"), ReachedKey(gate.FeatureName), value: true);
	}

	private static void CheckDeadUnit(UnitEntityData unit)
	{
		if (unit == null)
		{
			return;
		}
		try
		{
			string unitBlueprintGuid = ((unit.Blueprint != null) ? unit.Blueprint.AssetGuid.ToString() : "");
			MilestoneGate[] gates = Gates;
			foreach (MilestoneGate gate in gates)
			{
				if (MatchesDeath(gate, unitBlueprintGuid))
				{
					RecordReached(gate);
					UnitEntityData mainChar = Game.Instance?.Player?.MainCharacter.Value;
					string activeChroniclePrefix = GetActiveChroniclePrefix(mainChar, out var chronicleTag);
					SettleMilestone(gate, reached: true, mainChar, activeChroniclePrefix, chronicleTag);
				}
			}
		}
		catch (Exception ex)
		{
			Main.Log(ex.ToString());
		}
	}

	private static string CurrentAreaGuid()
	{
		BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
		if (blueprintArea == null)
		{
			return "";
		}
		return blueprintArea.AssetGuid.ToString();
	}

	private static BlueprintFeature TargetFeature(string featureName, string prefix)
	{
		if (!string.IsNullOrEmpty(prefix))
		{
			string text = featureName.Replace("Milestone", "Milestone" + prefix + "_");
			string[] originMilestoneNames = OriginMilestoneNames;
			foreach (string text2 in originMilestoneNames)
			{
				if (!(text2 != text))
				{
					BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, text2);
					if (modBlueprint != null)
					{
						return modBlueprint;
					}
				}
			}
		}
		return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, featureName);
	}

	private static void SettleMilestone(MilestoneGate gate, bool reached, UnitEntityData mainChar, string prefix, string chronicleTag)
	{
		Player player = Game.Instance?.Player;
		if (player == null || mainChar == null || prefix == null)
		{
			return;
		}
		BlueprintFeature blueprintFeature = TargetFeature(gate.FeatureName, prefix);
		if (blueprintFeature == null)
		{
			return;
		}
		bool flag = SaveScopedStorage.GetFlag(SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.StoryMilestones"), AwardedKey(gate.FeatureName));
		MilestoneAction milestoneAction = DecideMilestone(reached, flag, mainChar.Descriptor.HasFact(blueprintFeature));
		switch (milestoneAction)
		{
		case MilestoneAction.Nothing:
			return;
		case MilestoneAction.Award:
		case MilestoneAction.RecordOnly:
			SaveScopedStorage.SetFlag(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.StoryMilestones"), AwardedKey(gate.FeatureName), value: true);
			break;
		}
		if (milestoneAction == MilestoneAction.RecordOnly)
		{
			return;
		}
		mainChar.Descriptor.AddFact(blueprintFeature);
		if (GrantsBook(milestoneAction) && !string.IsNullOrEmpty(gate.BookItemName))
		{
			BlueprintItem modBlueprint = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, gate.BookItemName);
			if (modBlueprint != null && !player.Inventory.Contains(modBlueprint))
			{
				player.Inventory.Add(modBlueprint, 1);
			}
		}
		if (milestoneAction == MilestoneAction.Award)
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(ConstellationText.LocalizedFormat("Constellation.AnimeMilestone.Recorded", "<color=#5B2A86>{0}</color> Milestone Recorded: <b>{1}</b>!", ConstellationText.Localized("AnimeMilestone.ChronicleTag", chronicleTag), ConstellationText.Localized("AnimeMilestone.Announcement", gate.Announcement)));
			});
		}
	}

	public static void CheckMilestones()
	{
		try
		{
			Player player = Game.Instance?.Player;
			if (player == null)
			{
				return;
			}
			UnitEntityData value = player.MainCharacter.Value;
			if (value == null)
			{
				return;
			}
			QuestBook questBook = player.QuestBook;
			string currentAreaGuid = CurrentAreaGuid();
			MilestoneGate[] gates = Gates;
			foreach (MilestoneGate gate in gates)
			{
				if (IsAreaReached(gate, currentAreaGuid))
				{
					RecordReached(gate);
				}
			}
			string activeChroniclePrefix = GetActiveChroniclePrefix(value, out var chronicleTag);
			if (activeChroniclePrefix != null)
			{
				Dictionary<string, string> storage = SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.StoryMilestones");
				gates = Gates;
				foreach (MilestoneGate milestoneGate in gates)
				{
					bool reached = SaveScopedStorage.GetFlag(storage, ReachedKey(milestoneGate.FeatureName)) || IsObjectiveGateOpen(milestoneGate, questBook);
					SettleMilestone(milestoneGate, reached, value, activeChroniclePrefix, chronicleTag);
				}
			}
		}
		catch (Exception ex)
		{
			Main.Log(ex.ToString());
		}
	}

	internal static bool IsAreaReached(MilestoneGate gate, string currentAreaGuid)
	{
		if (gate == null || string.IsNullOrEmpty(gate.AreaGuid) || string.IsNullOrEmpty(currentAreaGuid))
		{
			return false;
		}
		return string.Equals(gate.AreaGuid, currentAreaGuid, StringComparison.OrdinalIgnoreCase);
	}

	private static bool IsObjectiveGateOpen(MilestoneGate gate, QuestBook questBook)
	{
		if (string.IsNullOrEmpty(gate.ObjectiveGuid) || questBook == null)
		{
			return false;
		}
		BlueprintQuestObjective blueprint = BlueprintTools.GetBlueprint<BlueprintQuestObjective>(gate.ObjectiveGuid);
		if (blueprint == null)
		{
			return false;
		}
		return IsObjectiveStateEnough(questBook.GetObjectiveState(blueprint), gate.AcceptStarted);
	}
}
