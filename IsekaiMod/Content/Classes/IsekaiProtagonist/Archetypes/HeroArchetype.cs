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
				BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TranscendentProtagonistFeature");
				BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AnimeFinalFormFeature");
				BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroOfLegendTranscendentTriad");
				BlueprintFeature modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroMasterSwordAwakened");
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
				BlueprintFeature modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "BondsOfFellowshipFeature");
				BlueprintFeature modBlueprint8 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "RetributionMiracleFeature");
				BlueprintFeature modBlueprint9 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TriforceCourageFeature");
				BlueprintFeature modBlueprint10 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TriforcePowerFeature");
				BlueprintFeature modBlueprint11 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TriforceWisdomFeature");
				BlueprintFeature modBlueprint12 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SongOfTimeFeature");
				BlueprintFeature modBlueprint13 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SacredTriadFeature");
				bp.AddFeatures = new LevelEntry[14]
				{
					Helpers.CreateLevelEntry(1, HeroProficiencies, GracefulCombat, HeroLegacySelection.getClassFeature(), modBlueprint9, modBlueprint2),
					Helpers.CreateLevelEntry(3, modBlueprint7, IsekaiChannelPositiveEnergyFeature),
					Helpers.CreateLevelEntry(4, HandsOfSalvation),
					Helpers.CreateLevelEntry(7, GoldBarrierFeature, modBlueprint10),
					Helpers.CreateLevelEntry(10, HeroAuraSelection, GoldBarrierHeroism, modBlueprint12),
					Helpers.CreateLevelEntry(12, modBlueprint8, GoldBarrierFastHealing),
					Helpers.CreateLevelEntry(13, modBlueprint11),
					Helpers.CreateLevelEntry(15, GoldBarrierResistance),
					Helpers.CreateLevelEntry(20, DeusExMachinaFeature, modBlueprint13),
					Helpers.CreateLevelEntry(23, HeroAuraSelection),
					Helpers.CreateLevelEntry(25, modBlueprint7),
					Helpers.CreateLevelEntry(30, modBlueprint5),
					Helpers.CreateLevelEntry(35, HeroAuraSelection),
					Helpers.CreateLevelEntry(40, modBlueprint6)
				};
				bp.OverrideAttributeRecommendations = true;
				bp.RecommendedAttributes = new StatType[1] { StatType.Charisma };
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "Hero";
					c.HideInUI = true;
				});
				BlueprintFeature SlimeHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeReincarnateHeritage");
				if (SlimeHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = SlimeHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
				BlueprintFeature OverlordHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeteromorphicOverlordHeritage");
				if (OverlordHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = OverlordHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
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
