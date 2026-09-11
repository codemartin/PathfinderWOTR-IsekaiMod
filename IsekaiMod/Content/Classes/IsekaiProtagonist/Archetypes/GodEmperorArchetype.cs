using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class GodEmperorArchetype
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "GodEmperorArchetype.Name", "God Emperor");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "GodEmperorArchetype.Description", "Rather than wandering aimlessly or seeking fleeting worldly glory, some otherworldly champions ascend toward true divinity. They sacrifice mundane versatility to cultivate overwhelming divine auras, planar authority, and an inexorable path toward godhood.");

		public static void Add()
		{
			BlueprintFeature GodEmperorProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies");
			BlueprintFeature NascentApotheosis = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "NascentApotheosis");
			BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "LightEnergyCondensation");
			BlueprintFeature GodEmperorQuickFooted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorQuickFooted");
			BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MajesticAuraFeature");
			BlueprintFeature Godhood = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Godhood");
			BlueprintFeature GodlyVessel = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodlyVessel");
			BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SiphoningAuraFeature");
			BlueprintFeature ArmorSaint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ArmorSaint");
			BlueprintFeatureSelection GodEmperorEnergySelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "GodEmperorEnergySelection");
			BlueprintFeatureSelection GodEmperorAuraSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "GodEmperorAuraSelection");
			BlueprintFeatureSelection EnergyCondensationSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "EnergyCondensationSelection");
			BlueprintFeatureSelection BarrierSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "BarrierSelection");
			BlueprintFeatureSelection BodyMindAlterSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "BodyMindAlterSelection");
			BlueprintFeatureSelection PathSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "PathSelection");
			BlueprintFeatureSelection RealmSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "RealmSelection");
			BlueprintFeature IsekaiProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			BlueprintFeature IsekaiQuickFooted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiQuickFooted");
			BlueprintFeature ReleaseEnergy = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
			BlueprintFeature Gifted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
			BlueprintFeature SecondReincarnation = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
			BlueprintFeatureSelection HaxSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
			BlueprintFeatureSelection SecretPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
			BlueprintFeatureSelection IsekaiAuraSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiAuraSelection");
			BlueprintFeatureSelection SignatureMoveBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveBonusSelection");
			BlueprintFeatureSelection TrainingEpisodeSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeSelection");
			BlueprintFeatureSelection TrainingEpisodeBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			BlueprintFeatureSelection SpecialPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection");
			BlueprintFeature ChronicleOtherworldFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
			BlueprintFeature ImperialEdictsFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialEdictsFeature");
			IsekaiProtagonistClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "GodEmperorArchetype", delegate(BlueprintArchetype bp)
			{
				bp.LocalizedName = Name;
				bp.LocalizedDescription = Description;
				bp.LocalizedDescriptionShort = Description;
				bp.IsArcaneCaster = true;
				bp.IsDivineCaster = true;
				bp.RemoveFeatures = new LevelEntry[13]
				{
					Helpers.CreateLevelEntry(1, IsekaiProficiencies, Gifted, LegacySelection.GetClassFeature(), ChronicleOtherworldFeature),
					Helpers.CreateLevelEntry(3, SpecialPowerSelection, ReleaseEnergy),
					Helpers.CreateLevelEntry(6, SignatureMoveBonusSelection),
					Helpers.CreateLevelEntry(7, SpecialPowerSelection),
					Helpers.CreateLevelEntry(9, SpecialPowerSelection),
					Helpers.CreateLevelEntry(10, IsekaiAuraSelection, SecretPowerSelection),
					Helpers.CreateLevelEntry(11, SpecialPowerSelection),
					Helpers.CreateLevelEntry(12, TrainingEpisodeSelection, TrainingEpisodeBonusSelection),
					Helpers.CreateLevelEntry(13, SpecialPowerSelection),
					Helpers.CreateLevelEntry(15, IsekaiQuickFooted, SecondReincarnation),
					Helpers.CreateLevelEntry(17, SpecialPowerSelection),
					Helpers.CreateLevelEntry(19, SpecialPowerSelection),
					Helpers.CreateLevelEntry(20, HaxSelection)
				};
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialWorshipFeature");
				BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialSovereigntyFeature");
				BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialTitheFeature");
				bp.AddFeatures = new LevelEntry[9]
				{
					Helpers.CreateLevelEntry(1, NascentApotheosis, GodEmperorProficiencies, modBlueprint, modBlueprint2, modBlueprint3, ImperialEdictsFeature, GodEmperorLegacySelection.getClassFeature()),
					Helpers.CreateLevelEntry(3, GodEmperorEnergySelection, ArmorSaint),
					Helpers.CreateLevelEntry(5, EnergyCondensationSelection),
					Helpers.CreateLevelEntry(7, BarrierSelection),
					Helpers.CreateLevelEntry(10, GodEmperorAuraSelection, BodyMindAlterSelection),
					Helpers.CreateLevelEntry(12, PathSelection),
					Helpers.CreateLevelEntry(15, RealmSelection, GodEmperorQuickFooted),
					Helpers.CreateLevelEntry(17, GodlyVessel),
					Helpers.CreateLevelEntry(20, Godhood)
				};
				bp.OverrideAttributeRecommendations = true;
				bp.m_ReplaceSpellbook = GodEmperorSpellbook.GetReference();
				bp.RecommendedAttributes = new StatType[1] { StatType.Wisdom };
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "GodEmperor";
				});
				bp.RemoveSpellbook = Main.IsekaiContext.AddedContent.DisableSpellbookGodEmperor;
			}));
		}

		public static BlueprintArchetype Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "GodEmperorArchetype");
		}

		public static BlueprintArchetypeReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "GodEmperorArchetype");
		}
	}
}
