using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.PhantomBlade;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

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
		BlueprintFeature modBlueprint14 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiSpontaneousMetamagic");
		BlueprintFeature modBlueprint15 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SummonHaremFeature");
		BlueprintFeature modBlueprint16 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OtherworldlyStamina");
		BlueprintFeature modBlueprint17 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SignatureAbility");
		BlueprintFeature modBlueprint18 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiFighterTraining");
		BlueprintFeature modBlueprint19 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Afterimage");
		BlueprintFeature modBlueprint20 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiQuickFooted");
		BlueprintFeature modBlueprint21 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
		BlueprintFeature modBlueprint22 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
		BlueprintFeature modBlueprint23 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DarkAuraFeature");
		BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DivineAuraFeature");
		BlueprintFeature modBlueprint24 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
		BlueprintFeature modBlueprint25 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicTokensFeature");
		BlueprintFeature modBlueprint26 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicSponsorshipExchangeFeature");
		BlueprintFeature modBlueprint27 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicBarrenEncountersFeature");
		BlueprintFeature modBlueprint28 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DimensionalRiftsFeature");
		BlueprintFeature modBlueprint29 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AppraisalFeature");
		BlueprintFeature modBlueprint30 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
		BlueprintProgression modBlueprint31 = BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "IsekaiCapstoneProgression");
		BlueprintFeature modBlueprint32 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PhantomBladeProficiencies");
		BlueprintFeature modBlueprint33 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PhantomFlurry");
		BlueprintFeature modBlueprint34 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PhantomDimensionalBlinkFeature");
		BlueprintFeature modBlueprint35 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PhantomVelocityFeature");
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
		List<BlueprintFeatureBase> list = new List<BlueprintFeatureBase>
		{
			modBlueprint,
			modBlueprint2,
			modBlueprint3,
			modBlueprint94,
			modBlueprint13,
			modBlueprint14,
			modBlueprint5,
			modBlueprint4,
			modBlueprint22,
			LegacySelection.GetClassFeature(),
			modBlueprint28,
			modBlueprint29,
			modBlueprint30,
			modBlueprint31,
			IsekaiLegendAscension.GetWatcher()
		};
		if (Main.IsekaiContext.AddedContent.EnableConstellations)
		{
			list.Add(modBlueprint26);
		}
		if (Main.IsekaiContext.AddedContent.EnableDimensionalArena)
		{
			list.Add(modBlueprint27);
		}
		if (Main.IsekaiContext.AddedContent.EnableConstellations || Main.IsekaiContext.AddedContent.EnableDimensionalArena)
		{
			list.Add(modBlueprint25);
		}
		RegressionSanityCheck.ReportMissingLevelFeatures("IsekaiProtagonistProgression", 1, list);
		list.RemoveAll((BlueprintFeatureBase f) => f == null);
		blueprintProgression.LevelEntries = new LevelEntry[20]
		{
			CreateLevelEntry(1, list.ToArray()),
			CreateLevelEntry(2, modBlueprint3),
			CreateLevelEntry(3, modBlueprint18, modBlueprint90, modBlueprint21),
			CreateLevelEntry(4, modBlueprint3),
			CreateLevelEntry(5, modBlueprint94),
			CreateLevelEntry(6, modBlueprint3, modBlueprint10, modBlueprint11),
			CreateLevelEntry(7, modBlueprint90),
			CreateLevelEntry(8, modBlueprint3, modBlueprint19),
			CreateLevelEntry(9, modBlueprint90),
			CreateLevelEntry(10, modBlueprint3, modBlueprint94, modBlueprint12, modBlueprint6),
			CreateLevelEntry(11, modBlueprint90),
			CreateLevelEntry(12, modBlueprint3, modBlueprint7, modBlueprint8),
			CreateLevelEntry(13, modBlueprint90, modBlueprint16),
			CreateLevelEntry(14, modBlueprint3),
			CreateLevelEntry(15, modBlueprint94, modBlueprint20, modBlueprint24),
			CreateLevelEntry(16, modBlueprint3),
			CreateLevelEntry(17, modBlueprint90, modBlueprint15),
			CreateLevelEntry(18, modBlueprint3),
			CreateLevelEntry(19, modBlueprint90),
			CreateLevelEntry(20, modBlueprint3, modBlueprint94, modBlueprint9)
		};
		blueprintProgression.UIGroups = new UIGroup[19]
		{
			CreateSafeUIGroup(modBlueprint13, modBlueprint18, modBlueprint17, modBlueprint10, modBlueprint15, modBlueprint12, modBlueprint37, modBlueprint23, modBlueprint52, modBlueprint19, modBlueprint20, modBlueprint44, modBlueprint65, modBlueprint7, modBlueprint16, modBlueprint9, modBlueprint59, modBlueprint66, modBlueprint71),
			CreateSafeUIGroup(modBlueprint21, modBlueprint22, modBlueprint11, modBlueprint6, modBlueprint8, modBlueprint24),
			CreateSafeUIGroup(modBlueprint33, modBlueprint34, modBlueprint35),
			CreateSafeUIGroup(modBlueprint45, modBlueprint46, modBlueprint36, modBlueprint40, modBlueprint38, modBlueprint39, modBlueprint41, modBlueprint42, modBlueprint47, modBlueprint48, modBlueprint49, modBlueprint50, modBlueprint51),
			CreateSafeUIGroup(modBlueprint54, modBlueprint92, modBlueprint55, modBlueprint56, modBlueprint57, modBlueprint58, modBlueprint60, modBlueprint61, modBlueprint62),
			CreateSafeUIGroup(modBlueprint95, blueprint, modBlueprint67),
			CreateSafeUIGroup(modBlueprint64, blueprint3, blueprint4, blueprint5),
			CreateSafeUIGroup(modBlueprint96, modBlueprint93, modBlueprint69, modBlueprint70, modBlueprint72, modBlueprint73, modBlueprint74, modBlueprint75, modBlueprint76, modBlueprint77),
			CreateSafeUIGroup(modBlueprint79, modBlueprint80, modBlueprint81, modBlueprint82, modBlueprint83),
			CreateSafeUIGroup(modBlueprint85, modBlueprint86, modBlueprint87, modBlueprint88, modBlueprint89),
			CreateSafeUIGroup(modBlueprint94, modBlueprint90, modBlueprint91),
			CreateSafeUIGroup(LegacySelection.GetClassFeature()),
			CreateSafeUIGroup(HeroLegacySelection.getClassFeature()),
			CreateSafeUIGroup(GodEmperorLegacySelection.getClassFeature()),
			CreateSafeUIGroup(MastermindLegacySelection.getClassFeature()),
			CreateSafeUIGroup(OverlordLegacySelection.getClassFeature()),
			CreateSafeUIGroup(PhantomBladeLegacySelection.getClassFeature()),
			CreateSafeUIGroup(DevourerLegacySelection.getClassFeature()),
			CreateSafeUIGroup(ShadowMonarchLegacySelection.getClassFeature())
		}.Where((UIGroup g) => g != null).ToArray();
		blueprintProgression.m_UIDeterminatorsGroup = (from f in new BlueprintFeatureBase[12]
			{
				modBlueprint2, modBlueprint5, modBlueprint4, modBlueprint, modBlueprint32, modBlueprint43, modBlueprint53, modBlueprint63, modBlueprint68, modBlueprint78,
				modBlueprint84, blueprint2
			}
			where f != null
			select f.ToReference<BlueprintFeatureBaseReference>()).ToArray();
		IsekaiProtagonistClass.SetProgression(blueprintProgression);
	}

	private static LevelEntry CreateLevelEntry(int level, params BlueprintFeatureBase[] features)
	{
		RegressionSanityCheck.ReportMissingLevelFeatures("IsekaiProtagonistProgression", level, features);
		List<BlueprintFeatureBaseReference> features2 = (from f in features
			where f != null
			select f.ToReference<BlueprintFeatureBaseReference>()).ToList();
		return new LevelEntry
		{
			Level = level,
			m_Features = features2
		};
	}

	private static UIGroup CreateSafeUIGroup(params BlueprintFeatureBase[] features)
	{
		List<BlueprintFeatureBaseReference> list = (from f in features
			where f != null
			select f.ToReference<BlueprintFeatureBaseReference>()).ToList();
		if (list.Count == 0)
		{
			return null;
		}
		return new UIGroup
		{
			m_Features = list
		};
	}
}
