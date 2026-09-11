using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	internal class IsekaiProtagonistProgression
	{
		public static void Add()
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiCantrips");
			BlueprintFeatureSelection modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
			BlueprintFeatureSelection modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiPetSelection");
			BlueprintFeatureSelection modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "StartingWeaponSelection");
			BlueprintFeatureSelection modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
			BlueprintFeatureSelection modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeSelection");
			BlueprintFeatureSelection modBlueprint8 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			BlueprintFeatureSelection modBlueprint9 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
			BlueprintFeatureSelection modBlueprint10 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveSelection");
			BlueprintFeatureSelection modBlueprint11 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveBonusSelection");
			BlueprintFeatureSelection modBlueprint12 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiAuraSelection");
			BlueprintFeature modBlueprint13 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PlotArmor");
			BlueprintFeature modBlueprint14 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SummonHaremFeature");
			BlueprintFeature modBlueprint15 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OtherworldlyStamina");
			BlueprintFeature modBlueprint16 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SignatureAbility");
			BlueprintFeature modBlueprint17 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiFighterTraining");
			BlueprintFeature modBlueprint18 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Afterimage");
			BlueprintFeature modBlueprint19 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiQuickFooted");
			BlueprintFeature modBlueprint20 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
			BlueprintFeature modBlueprint21 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
			BlueprintFeature modBlueprint22 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DarkAuraFeature");
			BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DivineAuraFeature");
			BlueprintFeature modBlueprint23 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
			BlueprintFeature modBlueprint24 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicTokensFeature");
			BlueprintFeature modBlueprint25 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicSponsorshipExchangeFeature");
			BlueprintFeature modBlueprint26 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DimensionalArenaFeature");
			BlueprintFeature modBlueprint27 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DimensionalRiftsFeature");
			BlueprintFeature modBlueprint28 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AppraisalFeature");
			BlueprintFeature modBlueprint29 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
			BlueprintFeature modBlueprint30 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TranscendentProtagonistFeature");
			BlueprintFeature modBlueprint31 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AnimeFinalFormFeature");
			BlueprintFeature modBlueprint32 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialGodProficiencies");
			BlueprintFeature modBlueprint33 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialFlurry");
			BlueprintFeature modBlueprint34 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialDimensionalBlinkFeature");
			BlueprintFeature modBlueprint35 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialVelocityFeature");
			BlueprintFeatureSelection modBlueprint36 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "GodEmperorEnergySelection");
			BlueprintFeatureSelection modBlueprint37 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "GodEmperorAuraSelection");
			BlueprintFeatureSelection modBlueprint38 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "EnergyCondensationSelection");
			BlueprintFeatureSelection modBlueprint39 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "BarrierSelection");
			BlueprintFeatureSelection modBlueprint40 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "BodyMindAlterSelection");
			BlueprintFeatureSelection modBlueprint41 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "PathSelection");
			BlueprintFeatureSelection modBlueprint42 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "RealmSelection");
			BlueprintFeature modBlueprint43 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies");
			BlueprintFeature modBlueprint44 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorQuickFooted");
			BlueprintFeature modBlueprint45 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "NascentApotheosis");
			BlueprintFeature modBlueprint46 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "LightEnergyCondensation");
			BlueprintFeature modBlueprint47 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodlyVessel");
			BlueprintFeature modBlueprint48 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Godhood");
			BlueprintFeature modBlueprint49 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialWorshipFeature");
			BlueprintFeature modBlueprint50 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialTitheFeature");
			BlueprintFeature modBlueprint51 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ImperialSovereigntyFeature");
			BlueprintFeatureSelection modBlueprint52 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HeroAuraSelection");
			BlueprintFeature modBlueprint53 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroProficiencies");
			BlueprintFeature modBlueprint54 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GracefulCombat");
			BlueprintFeature modBlueprint55 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HandsOfSalvation");
			BlueprintFeature modBlueprint56 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GoldBarrierFeature");
			BlueprintFeature modBlueprint57 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GoldBarrierHeroism");
			BlueprintFeature modBlueprint58 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GoldBarrierFastHealing");
			BlueprintFeature modBlueprint59 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeusExMachinaFeature");
			BlueprintFeature modBlueprint60 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GoldBarrierResistance");
			BlueprintFeature modBlueprint61 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "BondsOfFellowshipFeature");
			BlueprintFeature modBlueprint62 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "RetributionMiracleFeature");
			BlueprintFeature modBlueprint63 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindProficiencies");
			BlueprintFeature modBlueprint64 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindConsumeSpells");
			BlueprintFeature modBlueprint65 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindQuickFooted");
			BlueprintFeature modBlueprint66 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MasterplanFeature");
			BlueprintFeature modBlueprint67 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TacticalAmbushFeature");
			BlueprintFeatureSelection blueprint = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b8bf3d5023f2d8c428fdf6438cecaea7");
			BlueprintFeature blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeature>("55db1859bd72fd04f9bd3fe1f10e4cbb");
			BlueprintFeature blueprint3 = BlueprintTools.GetBlueprint<BlueprintFeature>("644c0e9618e417947bd0a1252a5e6ecf");
			BlueprintFeature blueprint4 = BlueprintTools.GetBlueprint<BlueprintFeature>("718fe8e143d38cc4899ae798dd098b6e");
			BlueprintFeature blueprint5 = BlueprintTools.GetBlueprint<BlueprintFeature>("685ee64e43fcb6546b65436a3deb98bd");
			BlueprintFeature modBlueprint68 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies");
			BlueprintFeature modBlueprint69 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CorruptAuraFeature");
			BlueprintFeature modBlueprint70 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SiphoningAuraFeature");
			BlueprintFeature modBlueprint71 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondPhaseFeature");
			BlueprintFeature modBlueprint72 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GraspHeartFeature");
			BlueprintFeature modBlueprint73 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DespairAuraFeature");
			BlueprintFeature modBlueprint74 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CreateDeathKnightFeature");
			BlueprintFeature modBlueprint75 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SupremeBuffRoutineFeature");
			BlueprintFeature modBlueprint76 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TheGoalOfAllLifeIsDeathFeature");
			BlueprintFeature modBlueprint77 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SuperTierMagicFeature");
			BlueprintFeature modBlueprint78 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies");
			BlueprintFeature modBlueprint79 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PredatorMawFeature");
			BlueprintFeature modBlueprint80 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "EssenceAssimilationFeature");
			BlueprintFeature modBlueprint81 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "InfiniteStomachFeature");
			BlueprintFeature modBlueprint82 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourMagicFeature");
			BlueprintFeature modBlueprint83 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "BeelzebubLordOfDevourersFeature");
			BlueprintFeature modBlueprint84 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies");
			BlueprintFeature modBlueprint85 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowExtractionFeature");
			BlueprintFeature modBlueprint86 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowStepFeature");
			BlueprintFeature modBlueprint87 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowArmorFeature");
			BlueprintFeature modBlueprint88 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MonarchDomainFeature");
			BlueprintFeature modBlueprint89 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowExchangeFeature");
			BlueprintFeatureSelection modBlueprint90 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
			BlueprintFeature modBlueprint91 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ArmorSaint");
			BlueprintFeature modBlueprint92 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiChannelPositiveEnergyFeature");
			BlueprintFeature modBlueprint93 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiChannelNegativeEnergyFeature");
			BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Supermassive");
			BlueprintFeatureSelection modBlueprint94 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection");
			BlueprintFeatureSelection modBlueprint95 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "AutoMetamagicSelectionMastermind");
			BlueprintFeatureSelection modBlueprint96 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelectionOverlord");
			BlueprintProgression blueprintProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(StaticReferences.Strings.Null);
				bp.SetDescription(Main.IsekaiContext, "Isekai protagonists are otherworldly entities who have been reincarnated into the world of Golarion with extraordinary abilities. As their story progresses, they gain more unexplained and overpowered abilities to overcome every challenge they face.");
				((BlueprintUnitFact)bp).m_AllowNonContextActions = false;
				bp.IsClassFeature = true;
				bp.m_FeaturesRankIncrease = null;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = IsekaiProtagonistClass.GetReference(),
						AdditionalLevel = 0
					}
				};
			});
			blueprintProgression.LevelEntries = new LevelEntry[40]
			{
				Helpers.CreateLevelEntry(1, modBlueprint, modBlueprint2, modBlueprint3, modBlueprint94, modBlueprint13, modBlueprint5, modBlueprint4, modBlueprint21, LegacySelection.GetClassFeature(), modBlueprint24, modBlueprint25, modBlueprint26, modBlueprint27, modBlueprint28, modBlueprint29),
				Helpers.CreateLevelEntry(2, modBlueprint3),
				Helpers.CreateLevelEntry(3, modBlueprint17, modBlueprint90, modBlueprint20),
				Helpers.CreateLevelEntry(4, modBlueprint3),
				Helpers.CreateLevelEntry(5, modBlueprint94),
				Helpers.CreateLevelEntry(6, modBlueprint3, modBlueprint10, modBlueprint11),
				Helpers.CreateLevelEntry(7, modBlueprint90),
				Helpers.CreateLevelEntry(8, modBlueprint3, modBlueprint18),
				Helpers.CreateLevelEntry(9, modBlueprint90),
				Helpers.CreateLevelEntry(10, modBlueprint3, modBlueprint94, modBlueprint12, modBlueprint6),
				Helpers.CreateLevelEntry(11, modBlueprint90),
				Helpers.CreateLevelEntry(12, modBlueprint3, modBlueprint7, modBlueprint8),
				Helpers.CreateLevelEntry(13, modBlueprint90, modBlueprint15),
				Helpers.CreateLevelEntry(14, modBlueprint3),
				Helpers.CreateLevelEntry(15, modBlueprint94, modBlueprint19, modBlueprint23),
				Helpers.CreateLevelEntry(16, modBlueprint3),
				Helpers.CreateLevelEntry(17, modBlueprint90, modBlueprint14),
				Helpers.CreateLevelEntry(18, modBlueprint3),
				Helpers.CreateLevelEntry(19, modBlueprint90),
				Helpers.CreateLevelEntry(20, modBlueprint3, modBlueprint94, modBlueprint9),
				Helpers.CreateLevelEntry(21, modBlueprint90),
				Helpers.CreateLevelEntry(22, modBlueprint3),
				Helpers.CreateLevelEntry(23, modBlueprint90),
				Helpers.CreateLevelEntry(24, modBlueprint3),
				Helpers.CreateLevelEntry(25, modBlueprint94, modBlueprint90),
				Helpers.CreateLevelEntry(26, modBlueprint3),
				Helpers.CreateLevelEntry(27, modBlueprint90),
				Helpers.CreateLevelEntry(28, modBlueprint3),
				Helpers.CreateLevelEntry(29, modBlueprint90),
				Helpers.CreateLevelEntry(30, modBlueprint3, modBlueprint94, modBlueprint6, modBlueprint30),
				Helpers.CreateLevelEntry(31, modBlueprint90),
				Helpers.CreateLevelEntry(32, modBlueprint3),
				Helpers.CreateLevelEntry(33, modBlueprint90),
				Helpers.CreateLevelEntry(34, modBlueprint3),
				Helpers.CreateLevelEntry(35, modBlueprint94, modBlueprint90),
				Helpers.CreateLevelEntry(36, modBlueprint3),
				Helpers.CreateLevelEntry(37, modBlueprint90),
				Helpers.CreateLevelEntry(38, modBlueprint3),
				Helpers.CreateLevelEntry(39, modBlueprint90),
				Helpers.CreateLevelEntry(40, modBlueprint3, modBlueprint94, modBlueprint9, modBlueprint31)
			};
			blueprintProgression.UIGroups = new UIGroup[18]
			{
				Helpers.CreateUIGroup(modBlueprint13, modBlueprint17, modBlueprint16, modBlueprint10, modBlueprint14, modBlueprint12, modBlueprint37, modBlueprint22, modBlueprint52, modBlueprint18, modBlueprint19, modBlueprint44, modBlueprint65, modBlueprint7, modBlueprint15, modBlueprint9, modBlueprint59, modBlueprint66, modBlueprint71),
				Helpers.CreateUIGroup(modBlueprint20, modBlueprint21, modBlueprint11, modBlueprint6, modBlueprint8, modBlueprint23),
				Helpers.CreateUIGroup(modBlueprint33, modBlueprint34, modBlueprint35),
				Helpers.CreateUIGroup(modBlueprint45, modBlueprint46, modBlueprint36, modBlueprint40, modBlueprint38, modBlueprint39, modBlueprint41, modBlueprint42, modBlueprint47, modBlueprint48, modBlueprint49, modBlueprint50, modBlueprint51),
				Helpers.CreateUIGroup(modBlueprint54, modBlueprint92, modBlueprint55, modBlueprint56, modBlueprint57, modBlueprint58, modBlueprint60, modBlueprint61, modBlueprint62),
				Helpers.CreateUIGroup(modBlueprint95, blueprint, modBlueprint67),
				Helpers.CreateUIGroup(modBlueprint64, blueprint3, blueprint4, blueprint5),
				Helpers.CreateUIGroup(modBlueprint96, modBlueprint93, modBlueprint69, modBlueprint70, modBlueprint72, modBlueprint73, modBlueprint74, modBlueprint75, modBlueprint76, modBlueprint77),
				Helpers.CreateUIGroup(modBlueprint79, modBlueprint80, modBlueprint81, modBlueprint82, modBlueprint83),
				Helpers.CreateUIGroup(modBlueprint85, modBlueprint86, modBlueprint87, modBlueprint88, modBlueprint89),
				Helpers.CreateUIGroup(modBlueprint94, modBlueprint90, modBlueprint91),
				Helpers.CreateUIGroup(LegacySelection.GetClassFeature()),
				Helpers.CreateUIGroup(HeroLegacySelection.getClassFeature()),
				Helpers.CreateUIGroup(MastermindLegacySelection.getClassFeature()),
				Helpers.CreateUIGroup(OverlordLegacySelection.getClassFeature()),
				Helpers.CreateUIGroup(MartialGodLegacySelection.getClassFeature()),
				Helpers.CreateUIGroup(DevourerLegacySelection.getClassFeature()),
				Helpers.CreateUIGroup(ShadowMonarchLegacySelection.getClassFeature())
			};
			blueprintProgression.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[12]
			{
				modBlueprint2.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint5.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint4.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint32.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint43.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint53.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint63.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint68.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint78.ToReference<BlueprintFeatureBaseReference>(),
				modBlueprint84.ToReference<BlueprintFeatureBaseReference>(),
				blueprint2.ToReference<BlueprintFeatureBaseReference>()
			};
			IsekaiProtagonistClass.SetProgression(blueprintProgression);
		}
	}
}
