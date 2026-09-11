using System;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	public class AnimeStoryMilestonesHandler : IUnitFinallyDeadHandler, ISubscriber, IGlobalSubscriber, IAreaHandler
	{
		public void HandleUnitBecameFinallyDead(UnitEntityData unit)
		{
			CheckDeadUnit(unit);
			CheckMilestones();
		}

		public void OnAreaDidLoad()
		{
			CheckMilestones();
		}

		public void OnAreaBeginUnloading()
		{
		}

		public static string GetActiveChroniclePrefix(UnitEntityData mainChar, out string chronicleTag)
		{
			chronicleTag = "[Chronicle]";
			if (mainChar?.Descriptor == null)
			{
				return null;
			}
			UnitDescriptor descriptor = mainChar.Descriptor;
			BlueprintFeature blueprintFeature = AnimeStoryMilestones.GluttonyCompendiumFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GluttonyCompendiumFeature");
			if (blueprintFeature != null && descriptor.HasFact(blueprintFeature))
			{
				chronicleTag = "[Predator's Compendium]";
				return "Slime";
			}
			BlueprintFeature blueprintFeature2 = AnimeStoryMilestones.TombAnnalsFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TombAnnalsFeature");
			if (blueprintFeature2 != null && descriptor.HasFact(blueprintFeature2))
			{
				chronicleTag = "[Grimoire of Conquest]";
				return "Overlord";
			}
			BlueprintFeature blueprintFeature3 = AnimeStoryMilestones.HuntersLogFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HuntersLogFeature");
			if (blueprintFeature3 != null && descriptor.HasFact(blueprintFeature3))
			{
				chronicleTag = "[Hunter's Log]";
				return "Shadow";
			}
			BlueprintFeature blueprintFeature4 = AnimeStoryMilestones.HeroChronicleFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroChronicleFeature");
			if (blueprintFeature4 != null && descriptor.HasFact(blueprintFeature4))
			{
				chronicleTag = "[Hero's Odyssey]";
				return "Hero";
			}
			BlueprintFeature blueprintFeature5 = AnimeStoryMilestones.ImperialEdictsFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialEdictsFeature");
			if (blueprintFeature5 != null && descriptor.HasFact(blueprintFeature5))
			{
				chronicleTag = "[Imperial Edicts]";
				return "God";
			}
			BlueprintFeature blueprintFeature6 = AnimeStoryMilestones.GrandStrategyFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GrandStrategyFeature");
			if (blueprintFeature6 != null && descriptor.HasFact(blueprintFeature6))
			{
				chronicleTag = "[Grand Strategy]";
				return "Mastermind";
			}
			BlueprintFeature blueprintFeature7 = AnimeStoryMilestones.SoulArchiveFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SoulArchiveFeature");
			if (blueprintFeature7 != null && descriptor.HasFact(blueprintFeature7))
			{
				chronicleTag = "[Soul Archive]";
				return "Phantom";
			}
			BlueprintFeature blueprintFeature8 = AnimeStoryMilestones.ChronicleOtherworldFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
			if (blueprintFeature8 != null && descriptor.HasFact(blueprintFeature8))
			{
				chronicleTag = "[Otherworld Chronicle]";
				return "";
			}
			return null;
		}

		private static void CheckDeadUnit(UnitEntityData unit)
		{
			if (unit == null)
			{
				return;
			}
			try
			{
				Player player = Game.Instance?.Player;
				if (player == null)
				{
					return;
				}
				UnitEntityData unitEntityData = player.SafeGetMainCharacter();
				if (!(unitEntityData == null) && GetActiveChroniclePrefix(unitEntityData, out var _) != null)
				{
					string text = unit.Blueprint?.name ?? "";
					string text2 = unit.CharacterName ?? "";
					if ((text.IndexOf("WaterElemental", StringComparison.OrdinalIgnoreCase) >= 0 || text2.IndexOf("Water Elemental", StringComparison.OrdinalIgnoreCase) >= 0) && (Game.Instance?.CurrentlyLoadedArea?.name ?? "").IndexOf("ShieldMaze", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						AwardMilestone("MilestoneWaterElementalFeature", null, "Shield Maze Apex Predator Slayed");
					}
					if (text.IndexOf("Devarra", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("RedDragon", StringComparison.OrdinalIgnoreCase) >= 0 || text2.IndexOf("Devarra", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						AwardMilestone("MilestoneRedDragonFeature", null, "Ancient Dragon Devarra Hunted");
					}
				}
			}
			catch (Exception ex)
			{
				Main.Log(ex.ToString());
			}
		}

		public static void AwardMilestone(string featureName, string bookItemName, string announcement)
		{
			Player player = Game.Instance?.Player;
			if (player == null)
			{
				return;
			}
			UnitEntityData unitEntityData = player.SafeGetMainCharacter();
			if (unitEntityData == null)
			{
				return;
			}
			string activeChroniclePrefix = GetActiveChroniclePrefix(unitEntityData, out var chronicleTag);
			if (activeChroniclePrefix == null)
			{
				return;
			}
			string name = featureName;
			if (!string.IsNullOrEmpty(activeChroniclePrefix))
			{
				string text = featureName.Replace("Milestone", "Milestone" + activeChroniclePrefix + "_");
				if (BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, text) != null)
				{
					name = text;
				}
			}
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, name);
			if (modBlueprint == null || unitEntityData.Descriptor.HasFact(modBlueprint))
			{
				return;
			}
			unitEntityData.Descriptor.AddFact(modBlueprint);
			if (!string.IsNullOrEmpty(bookItemName))
			{
				BlueprintItem modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, bookItemName);
				if (modBlueprint2 != null && !player.Inventory.Contains(modBlueprint2))
				{
					player.Inventory.Add(modBlueprint2, 1);
				}
			}
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#FFD700>" + chronicleTag + "</color> Milestone Recorded: <b>" + announcement + "</b>!");
			});
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
				UnitEntityData unitEntityData = player.SafeGetMainCharacter();
				if (!(unitEntityData == null) && GetActiveChroniclePrefix(unitEntityData, out var _) != null)
				{
					int chapter = player.Chapter;
					if (chapter >= 1)
					{
						AwardMilestone("MilestoneShieldMazeFeature", "ItemSkillBookFirstCrusaders", "Shield Maze Conquered");
					}
					if (chapter >= 2)
					{
						AwardMilestone("MilestoneWaterElementalFeature", null, "Shield Maze Apex Predator Slayed");
						AwardMilestone("MilestoneGrayGarrisonFeature", "ItemSkillBookRadiantWard", "Gray Garrison Liberated");
						AwardMilestone("MilestoneVescavorQueenFeature", "ItemSkillBookPrimalBeastmaster", "Leper's Smile Purged");
						AwardMilestone("MilestoneLostChapelFeature", "ItemSkillBookUnbrokenSoul", "Lost Chapel Reclaimed");
					}
					if (chapter >= 3)
					{
						AwardMilestone("MilestoneDrezenCitadelFeature", "ItemSkillBookSwarmTransmutation", "Drezen Citadel Captured");
						AwardMilestone("MilestoneIvorySanctumFeature", "ItemSkillBookPlanarInfiltration", "Ivory Sanctum Infiltrated");
						AwardMilestone("MilestoneMidnightFaneFeature", "ItemSkillBookAreeluAxiom", "Midnight Fane Breached");
					}
					if (chapter >= 4)
					{
						AwardMilestone("MilestoneRedDragonFeature", null, "Ancient Dragon Devarra Hunted");
						AwardMilestone("MilestoneColyphyrFeature", "ItemSkillBookMythicMineralogy", "Colyphyr Mines Exploited");
						AwardMilestone("MilestoneBaphometFeature", "ItemSkillBookRunelordLexicon", "Demon Lord Confronted");
					}
					if (chapter >= 5)
					{
						AwardMilestone("MilestoneThresholdFeature", null, "Threshold of the Multiverse Reached");
					}
				}
			}
			catch (Exception ex)
			{
				Main.Log(ex.ToString());
			}
		}
	}
}
