using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class OverlordArchetype
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "OverlordArchetype.Name", "Overlord");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "OverlordArchetype.Description", "After obtaining ungodly amounts of power, some protagonists become Overlords. They view the new world as theirs to play with, and the new inhabitants as theirs to torment. Overlords seek to increase their power even further, often by establishing their own kingdom.");

		public static void Add()
		{
			BlueprintFeature OverlordProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies");
			BlueprintFeature DarkAuraFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DarkAuraFeature");
			BlueprintFeature OverpoweredAbilitySelectionOverlord = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverpoweredAbilitySelectionOverlord");
			BlueprintFeature CorruptAuraFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CorruptAuraFeature");
			BlueprintFeature SecondPhaseFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondPhaseFeature");
			BlueprintFeature IsekaiChannelNegativeEnergyFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiChannelNegativeEnergyFeature");
			BlueprintFeature SiphoningAuraFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SiphoningAuraFeature");
			BlueprintFeature IsekaiProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			BlueprintFeature IsekaiAuraSelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiAuraSelection");
			BlueprintFeature ReleaseEnergy = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
			BlueprintFeature Gifted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
			BlueprintFeature SecretPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecretPowerSelection");
			BlueprintFeature HaxSelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HaxSelection");
			BlueprintFeature SignatureMoveBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SignatureMoveBonusSelection");
			BlueprintFeatureSelection TrainingEpisodeBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			BlueprintFeature SecondReincarnation = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
			BlueprintFeatureSelection SpecialPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
			IsekaiProtagonistClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordArchetype", delegate(BlueprintArchetype bp)
			{
				bp.LocalizedName = Name;
				bp.LocalizedDescription = Description;
				bp.LocalizedDescriptionShort = Description;
				bp.IsArcaneCaster = true;
				bp.IsDivineCaster = true;
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
				BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TombAnnalsFeature");
				BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TranscendentProtagonistFeature");
				BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AnimeFinalFormFeature");
				BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordSupremeRuler");
				BlueprintFeature modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordTrueDominion");
				bp.RemoveFeatures = new LevelEntry[19]
				{
					Helpers.CreateLevelEntry(1, IsekaiProficiencies, Gifted, LegacySelection.GetClassFeature(), modBlueprint),
					Helpers.CreateLevelEntry(3, ReleaseEnergy),
					Helpers.CreateLevelEntry(6, SignatureMoveBonusSelection),
					Helpers.CreateLevelEntry(9, SpecialPowerSelection),
					Helpers.CreateLevelEntry(10, IsekaiAuraSelection, SecretPowerSelection),
					Helpers.CreateLevelEntry(11, SpecialPowerSelection),
					Helpers.CreateLevelEntry(12, TrainingEpisodeBonusSelection),
					Helpers.CreateLevelEntry(15, SecondReincarnation),
					Helpers.CreateLevelEntry(20, HaxSelection),
					Helpers.CreateLevelEntry(21, SpecialPowerSelection),
					Helpers.CreateLevelEntry(23, SpecialPowerSelection),
					Helpers.CreateLevelEntry(27, SpecialPowerSelection),
					Helpers.CreateLevelEntry(29, SpecialPowerSelection),
					Helpers.CreateLevelEntry(30, SecretPowerSelection, modBlueprint3),
					Helpers.CreateLevelEntry(31, SpecialPowerSelection),
					Helpers.CreateLevelEntry(33, SpecialPowerSelection),
					Helpers.CreateLevelEntry(37, SpecialPowerSelection),
					Helpers.CreateLevelEntry(39, SpecialPowerSelection),
					Helpers.CreateLevelEntry(40, HaxSelection, modBlueprint4)
				};
				BlueprintFeature modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GraspHeartFeature");
				BlueprintFeature modBlueprint8 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DespairAuraFeature");
				BlueprintFeature modBlueprint9 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CreateDeathKnightFeature");
				BlueprintFeature modBlueprint10 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SupremeBuffRoutineFeature");
				BlueprintFeature modBlueprint11 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TheGoalOfAllLifeIsDeathFeature");
				BlueprintFeature modBlueprint12 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SuperTierMagicFeature");
				BlueprintFeature blueprintFeature = SkeletalOverlordForm.Get();
				BlueprintFeature blueprintFeature2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordChannelEnergyFeature") ?? IsekaiChannelNegativeEnergyFeature;
				BlueprintFeature modBlueprint13 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AuraOfRighteousMajestyFeature");
				bp.AddFeatures = new LevelEntry[15]
				{
					Helpers.CreateLevelEntry(1, OverlordProficiencies, modBlueprint7, modBlueprint8, blueprintFeature, modBlueprint2, OverlordLegacySelection.getClassFeature()),
					Helpers.CreateLevelEntry(3, modBlueprint9, blueprintFeature2),
					Helpers.CreateLevelEntry(5, OverpoweredAbilitySelectionOverlord),
					Helpers.CreateLevelEntry(7, modBlueprint10, CorruptAuraFeature, modBlueprint13),
					Helpers.CreateLevelEntry(10, DarkAuraFeature),
					Helpers.CreateLevelEntry(11, SiphoningAuraFeature),
					Helpers.CreateLevelEntry(12, modBlueprint11),
					Helpers.CreateLevelEntry(15, OverpoweredAbilitySelectionOverlord),
					Helpers.CreateLevelEntry(20, modBlueprint12, SecondPhaseFeature),
					Helpers.CreateLevelEntry(23, OverpoweredAbilitySelectionOverlord),
					Helpers.CreateLevelEntry(25, modBlueprint9),
					Helpers.CreateLevelEntry(28, OverpoweredAbilitySelectionOverlord),
					Helpers.CreateLevelEntry(30, modBlueprint5),
					Helpers.CreateLevelEntry(35, modBlueprint12),
					Helpers.CreateLevelEntry(40, modBlueprint6)
				};
				BlueprintFeature VampireHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiVampireHeritage");
				BlueprintFeature DragonHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiDragonLordHeritage");
				BlueprintFeature WerewolfHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiWerewolfHeritage");
				if (VampireHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = VampireHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
				if (DragonHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = DragonHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
				if (WerewolfHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = WerewolfHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
				BlueprintFeature SlimeHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeReincarnateHeritage");
				if (SlimeHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = SlimeHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "Overlord";
					c.HideInUI = true;
				});
				bp.m_ReplaceSpellbook = OverlordSpellbook.GetReference();
				bp.RemoveSpellbook = Main.IsekaiContext.AddedContent.DisableSpellbookOverlord;
			}));
		}

		public static BlueprintArchetype Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "OverlordArchetype");
		}

		public static BlueprintArchetypeReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "OverlordArchetype");
		}
	}
}
