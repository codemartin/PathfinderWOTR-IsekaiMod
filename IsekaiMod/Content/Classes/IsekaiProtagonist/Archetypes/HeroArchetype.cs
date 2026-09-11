using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class HeroArchetype
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "HeroArchetype.Name", "Hero");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "HeroArchetype.Description", "Heroes use their newfound powers for good. After realising the suffering and despair of the inhabitants of the new world, the hero sets out to bring knowledge from their old world in order to save them.");

		public static void Add()
		{
			BlueprintFeature HeroProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroProficiencies");
			BlueprintFeature GracefulCombat = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GracefulCombat");
			BlueprintFeature IsekaiChannelPositiveEnergyFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiChannelPositiveEnergyFeature");
			BlueprintFeature HandsOfSalvation = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HandsOfSalvation");
			BlueprintFeature GoldBarrierFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GoldBarrierFeature");
			BlueprintFeature GoldBarrierHeroism = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GoldBarrierHeroism");
			BlueprintFeature GoldBarrierFastHealing = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GoldBarrierFastHealing");
			BlueprintFeature GoldBarrierResistance = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GoldBarrierResistance");
			BlueprintFeature DeusExMachinaFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeusExMachinaFeature");
			BlueprintFeatureSelection HeroAuraSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HeroAuraSelection");
			BlueprintFeature IsekaiProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			BlueprintFeature ReleaseEnergy = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
			BlueprintFeature Gifted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
			BlueprintFeature SecondReincarnation = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
			BlueprintFeatureSelection IsekaiAuraSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiAuraSelection");
			BlueprintFeatureSelection SecretPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
			BlueprintFeatureSelection SignatureMoveBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveBonusSelection");
			BlueprintFeatureSelection TrainingEpisodeBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			BlueprintFeatureSelection SpecialPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
			BlueprintFeatureSelection HaxSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
			IsekaiProtagonistClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "HeroArchetype", delegate(BlueprintArchetype bp)
			{
				bp.LocalizedName = Name;
				bp.LocalizedDescription = Description;
				bp.LocalizedDescriptionShort = Description;
				bp.IsArcaneCaster = true;
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
				BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroChronicleFeature");
				bp.RemoveFeatures = new LevelEntry[9]
				{
					Helpers.CreateLevelEntry(1, IsekaiProficiencies, Gifted, LegacySelection.GetClassFeature(), modBlueprint),
					Helpers.CreateLevelEntry(3, ReleaseEnergy),
					Helpers.CreateLevelEntry(6, SignatureMoveBonusSelection),
					Helpers.CreateLevelEntry(9, SpecialPowerSelection),
					Helpers.CreateLevelEntry(10, IsekaiAuraSelection, SecretPowerSelection),
					Helpers.CreateLevelEntry(11, SpecialPowerSelection),
					Helpers.CreateLevelEntry(12, TrainingEpisodeBonusSelection),
					Helpers.CreateLevelEntry(15, SecondReincarnation),
					Helpers.CreateLevelEntry(20, HaxSelection)
				};
				BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "BondsOfFellowshipFeature");
				BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "RetributionMiracleFeature");
				BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TriforceCourageFeature");
				BlueprintFeature modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TriforcePowerFeature");
				BlueprintFeature modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TriforceWisdomFeature");
				BlueprintFeature modBlueprint8 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SongOfTimeFeature");
				BlueprintFeature modBlueprint9 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SacredTriadFeature");
				bp.AddFeatures = new LevelEntry[9]
				{
					Helpers.CreateLevelEntry(1, HeroProficiencies, GracefulCombat, HeroLegacySelection.getClassFeature(), modBlueprint5, modBlueprint2),
					Helpers.CreateLevelEntry(3, modBlueprint3, IsekaiChannelPositiveEnergyFeature),
					Helpers.CreateLevelEntry(4, HandsOfSalvation),
					Helpers.CreateLevelEntry(7, GoldBarrierFeature, modBlueprint6),
					Helpers.CreateLevelEntry(10, HeroAuraSelection, GoldBarrierHeroism, modBlueprint8),
					Helpers.CreateLevelEntry(12, modBlueprint4, GoldBarrierFastHealing),
					Helpers.CreateLevelEntry(13, modBlueprint7),
					Helpers.CreateLevelEntry(15, GoldBarrierResistance),
					Helpers.CreateLevelEntry(20, DeusExMachinaFeature, modBlueprint9)
				};
				bp.OverrideAttributeRecommendations = true;
				bp.RecommendedAttributes = new StatType[1] { StatType.Charisma };
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "Hero";
				});
				bp.RemoveSpellbook = Main.IsekaiContext.AddedContent.DisableSpellbookHero;
			}));
			PatchSpecialPowers();
		}

		public static void PatchSpecialPowers()
		{
			BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ExcaliburFeature").AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.Group = Prerequisite.GroupType.Any;
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = GetReference();
				c.Level = 1;
			});
		}

		public static BlueprintArchetype Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "HeroArchetype");
		}

		public static BlueprintArchetypeReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "HeroArchetype");
		}
	}
}
