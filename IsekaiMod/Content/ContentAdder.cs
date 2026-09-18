using System;
using HarmonyLib;
using IsekaiMod.Config;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Backgrounds;
using IsekaiMod.Content.Classes.Deathsnatcher;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Crusade;
using IsekaiMod.Content.Deities;
using IsekaiMod.Content.Dialogue;
using IsekaiMod.Content.Features;
using IsekaiMod.Content.Features.Deathsnatcher;
using IsekaiMod.Content.Features.ExceptionalFeats;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;
using IsekaiMod.Content.Features.IsekaiProtagonist.SoloMastery;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Content.Features.IsekaiProtagonist.TrainingEpisode;
using IsekaiMod.Content.Guardians;
using IsekaiMod.Content.Heritages;
using IsekaiMod.Content.Narrative;
using IsekaiMod.Content.Quests;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content
{
	internal class ContentAdder
	{
		[HarmonyPatch(typeof(BlueprintsCache), "Init")]
		private static class BlueprintsCache_Init_Patch
		{
			private static bool Initialized;

			private static bool _constellationsAdded;

			private static AddedContent AddedContent => Main.IsekaiContext.AddedContent;

			private static SettingGroup Other => Main.IsekaiContext.AddedContent.Other;

			private static SettingGroup Isekai => Main.IsekaiContext.AddedContent.Isekai;

			[HarmonyPriority(800)]
			[HarmonyPostfix]
			public static void CreateNewBlueprints()
			{
				if (Initialized)
				{
					return;
				}
				Initialized = true;
				SafeStep("ExtraWings.Add", ExtraWings.Add);
				SafeStep("ExoticWeaponProficiency.Add", ExoticWeaponProficiency.Add);
				SafeStep("AddExceptionalFeats", AddExceptionalFeats);
				if (Isekai.IsEnabled("Isekai Backgrounds"))
				{
					SafeStep("AddIsekaiBackgrounds", AddIsekaiBackgrounds);
				}
				if (Isekai.IsEnabled("Isekai Deities"))
				{
					SafeStep("AddIsekaiDeities", AddIsekaiDeities);
				}
				if (Isekai.IsEnabled("Isekai Heritages"))
				{
					SafeStep("AddIsekaiHeritages", AddIsekaiHeritages);
				}
				if (Isekai.IsEnabled("Isekai Protagonist"))
				{
					if (AddedContent.RestrictExceptionalFeats)
					{
						SafeStep("RestrictExceptionalFeats", RestrictExceptionalFeats);
					}
					SafeStep("AddIsekaiProtagonistClass", AddIsekaiProtagonistClass);
				}
				SafeStep("AddConstellations", AddConstellations);
			// The class progression is built before the Constellation features exist; link them now.
			SafeStep("LinkLateProgressionFeatures", IsekaiProtagonistProgression.LinkLateFeatures);
				if (Isekai.IsEnabled("Isekai Dialogue"))
				{
					SafeStep("AddIsekaiDialogue", AddIsekaiDialogue);
				}
			}

			public static void SafeStep(string name, Action action)
			{
				try
				{
					action();
				}
				catch (Exception arg)
				{
					Main.IsekaiContext.Logger.LogError($"[Startup Warning] Subsystem '{name}' threw an exception: {arg}");
				}
			}

			public static void AddConstellations()
			{
				if (!_constellationsAdded)
				{
					DivineTokens.Add();
					DeityAvatarAscension.Add();
					CosmicRelics.Add();
					DivineSponsorshipStore.Add();
					CosmicConsumables.Add();
					CodexOfReincarnation.Add();
					TimelineManager.Init();
					ConstellationChatManager.Init();
					DimensionalArenaManager.Init();
					DimensionalArenaAbilities.Add();
					PlanarColiseumEntrance.Add();
					CosmicRelicsVendorTable.Add();
					OtherworldLuxuryVendorTable.Add();
					PlanarGatewayEmissary.Add();
					PlanarColiseumHubManager.Init();
					ShardOfTheShatteredLoop.Add();
					EchoOfPastSovereigns.Add();
					IsekaiTransmigrationQuest.Add();
					DimensionalRiftIncursions.Init();
					DimensionalRiftIncursions.Add();
					SubclassPersonalQuests.Add();
					SylvanFeyAllianceQuest.Add();
					MythicQuestSynergies.Add();
					IsekaiKingdomProjects.Add();
					IsekaiSettlementBuildings.Add();
					SubclassRespecManager.Init();
					CosmicThreatScaling.Add();
					RivalReincarnators.Add();
					DimensionalReinforcements.Add();
					CosmicBarrenBosses.Add();
					_constellationsAdded = true;
				}
			}

			public static void AddIsekaiProtagonistClass()
			{
				LegacySelection.ConfigureStep1();
				IsekaiProtagonistSpellList.Add();
				IsekaiProtagonistSpellsPerDay.Add();
				IsekaiProtagonistSpellsKnown.Add();
				IsekaiProtagonistSpellbook.Add();
				IsekaiProtagonistClass.Add();
				TrainingEpisodeSelection.Add();
				HealthyBody.Add();
				InnerPower.Add();
				MasterSelf.Add();
				Tenacious.Add();
				SpecialPowerSelection.Add();
				MundaneAura.Add();
				AlphaStrike.Add();
				BetaStrike.Add();
				GammaStrike.Add();
				OmegaStrike.Add();
				SigmaStrike.Add();
				Regeneration.Add();
				EnergyImmunitySelection.Add();
				TrainingMontage.Add();
				BodyStrengthening.Add();
				SpellNegation.Add();
				ExtremeSpeed.Add();
				SneakyMagic.Add();
				SpellMaster.Add();
				ArmorSaint.Add();
				ArmorOfStrength.Add();
				SummonBeast.Add();
				KillingIntent.Add();
				MagicalAmplification.Add();
				Reflect.Add();
				IsekaiChannelPositiveEnergy.Add();
				IsekaiChannelNegativeEnergy.Add();
				Supermassive.Add();
				Excalibur.Add();
				Unreactable.Add();
				Haggler.Add();
				VampiricDrain.Add();
				ApexPredator.Add();
				ManaShield.Add();
				LimitBreak.Add();
				ProtagonistPhilosophies.Add();
				DomainExpansion.Add();
				InfiniteArmory.Add();
				SecondaryAwakening.Add();
				AdaptiveArmor.Add();
				EchoOfPastLegacies.Add();
				OverpoweredAbilitySelection.Add();
				AutoMetamagicSelection.Add();
				Instakill.Add();
				MerchantsGamble.Add();
				PerfectRoll.Add();
				SuperBuff.Add();
				UnlimitedPower.Add();
				MindControl.Add();
				SummonCalamity.Add();
				InfiniteSpace.Add();
				TrueResurrection.Add();
				SupremeBeing.Add();
				MetaLuck.Add();
				PowerLeveling.Add();
				MasterSummoner.Add();
				ShadowMonarch.Add();
				GluttonyPredator.Add();
				ChronoDomain.Add();
				IsekaiLegendAscension.Add();
				OverpoweredAuraSelection.Add();
				UnderpoweredAbilitySelection.Add();
				AbsoluteDefense.Add();
				WorldBreak.Add();
				CheatDeath.Add();
				SovereignsDread.Add();
				OmniscientMimicry.Add();
				Epic21Abilities.Add();
				MartialGodTranscendence.Add();
				MandateOfHeaven.Add();
				PowerOfFriendship.Add();
				AllAccordingToPlan.Add();
				SovereignOfTheGreatTomb.Add();
				StatusWindow.Add();
				ProtagonistsWill.Add();
				OtherworldlyGourmet.Add();
				FullCounter.Add();
				EminenceInTheShadow.Add();
				CreationMagic.Add();
				AbyssalCorsair.Add();
				PrimalChimera.Add();
				ProtagonistsSpotlight.Add();
				ParadoxSovereign.Add();
				ParallelProcessing.Add();
				ParallelMetamagicResonance.Add();
				if (Other.IsEnabled("Mythic Class Feature"))
				{
					BlessingOfTheMythic.Configure();
				}
				IsekaiProficiencies.Add();
				SubclassCantrips.Add();
				IsekaiCantrips.Add();
				IsekaiBonusFeatSelection.Add();
				// Moved after IsekaiBonusFeatSelection: it registers itself into that selection.
				OtherworldScavenger.Add();
				IsekaiTalentSelection.Add();
				GuardianCompanionClass.Add();
				GuardianProgressions.Add();
				GuardianBarks.Add();
				IsekaiGuardians.Add();
				SoloSovereign.Add();
				IsekaiPetSelection.Add();
				PlotArmor.Add();
				StartingWeaponSelection.Add();
				IsekaiFighterTraining.Add();
				SignatureMoveSelection.Add();
				Afterimage.Add();
				OtherworldlyStamina.Add();
				IsekaiQuickFooted.Add();
				IsekaiAuraSelection.Add();
				SummonExaltedChoir.Add();
				SecondReincarnation.Add();
				ReleaseEnergy.Add();
				Gifted.Add();
				SecretPowerSelection.Add();
				HaxSelection.Add();
				Appraisal.Add();
				AnimeStoryMilestones.Add();
				EpicFeats.Add();
				EpicPrestigeClasses.AddCapstones();
				EpicArchetypeFeatures.Add();
				GodEmperorSpellbook.Add();
				GodEmperorProficiencies.Add();
				GodEmperorQuickFooted.Add();
				NascentApotheosis.Add();
				EnergyCondensationSelection.Add();
				GodEmperorEnergySelection.Add();
				BarrierSelection.Add();
				BodyMindAlterSelection.Add();
				PathSelection.Add();
				GodlyVessel.Add();
				RealmSelection.Add();
				Godhood.Add();
				ImperialWorship.Add();
				ImperialTithe.Add();
				ImperialSovereignty.Add();
				GodEmperorArchetype.Add();
				MartialGodProficiencies.Add();
				MartialFlurry.Add();
				MartialDimensionalBlink.Add();
				MartialVelocity.Add();
				ExtraSpecialPowerSelection.Add();
				MartialGodArchetype.Add();
				HeroProficiencies.Add();
				GracefulCombat.Add();
				HandsOfSalvation.Add();
				DeusExMachina.Add();
				BondsOfFellowship.Add();
				RetributionMiracle.Add();
				HeroOfTimeFeatures.Add();
				HeroArchetype.Add();
				MastermindConsumeSpells.Add();
				MastermindSpellList.Add();
				MastermindSpellsPerDay.Add();
				MastermindSpellbook.Add();
				MastermindProficiencies.Add();
				MastermindQuickFooted.Add();
				Masterplan.Add();
				TacticalAmbush.Add();
				MastermindOverhaulFeatures.Add();
				MastermindArchetype.Add();
				OverlordSpellbook.Add();
				OverlordProficiencies.Add();
				CorruptAuraFeature.Add();
				SecondPhaseFeature.Add();
				GraspHeart.Add();
				DespairAura.Add();
				CreateDeathKnight.Add();
				SupremeBuffRoutine.Add();
				TheGoalOfAllLifeIsDeath.Add();
				SuperTierMagic.Add();
				SkeletalOverlordForm.Add();
				OverlordBenevolentFeatures.Add();
				OverlordArchetype.Add();
				DevourerProficiencies.Add();
				PredatorInstincts.Add();
				PredatorMaw.Add();
				EssenceAssimilation.Add();
				InfiniteStomach.Add();
				DevourMagic.Add();
				BeelzebubLordOfDevourers.Add();
				SlimeForm.Add();
				SlimePredatorArts.Add();
				DevourerArchetype.Add();
				ShadowMonarchProficiencies.Add();
				ShadowMonarchSpellbook.Add();
				ShadowHunterInstincts.Add();
				ShadowExtraction.Add();
				ShadowStep.Add();
				ShadowArmor.Add();
				MonarchDomain.Add();
				ShadowExchange.Add();
				ShadowMonarchArts.Add();
				ShadowMonarchArchetype.Add();
				TranscendentSovereignClass.Add();
				TranscendentSovereignFeatures.Add();
				TranscendentSovereignProgression.Add();
				TranscendentSovereignArchetypes.Add();
				EpicPrestigeClasses.Add();
				OtherworldRetinuePrestige.Add();
				DeathsnatcherClass.Add();
				DeathsnatcherSpellLikeDC.Add();
				DeathsnatcherSizeBaby.Add();
				DeathsnatcherResistances.Add();
				DeathsnatcherCommandUndead.Add();
				DeathsnatcherAnimateDead.Add();
				DeathsnatcherCreateUndead.Add();
				DeathsnatcherFingerOfDeath.Add();
				DeathsnatcherFastHealing.Add();
				DeathsnatcherPoisonSting.Add();
				DeathsnatcherUndeadMaster.Add();
				DeathsnatcherProgression.Add();
				DeathsnatcherUnit.Add();
				SubclassGuardianProgressions.Add();
				SubclassGuardians.Add();
				OtherworldGuardianPactSelection.Add();
				PrebuildIsekaiProtagonistFeatureList.Add();
				IsekaiProtagonistProgression.Add();
				PrestigeClassReplaceSpellbook.Patch();
				LegacySelection.ConfigureStep2();
			}

			public static void AddIsekaiDialogue()
			{
				IsekaiHulrun.Add();
				LegacyDialogueStubs.Add();
				IsekaiRadiance.Add();
				IsekaiKaylessaDrowLeader.Add();
				IsekaiHorgus.Add();
				IsekaiMinagho.Add();
				IsekaiRomanceDialogue.Add();
				SubclassDialogueReactions.Add();
				IsekaiEpilogueSlides.Add();
				IsekaiFinnean.Add();
				IsekaiNenioStatue.Add();
				CompanionRetinueDialogue.Add();
				IsekaiCampaignExpansions.Add();
				NarrativeEngine.RegisterAllDefaultScenes();
				NarrativeEngine.CompileAll();
			}

			public static void AddIsekaiHeritages()
			{
				HumanHeritageSelection.CreateDummy();
				IsekaiSuccubusHeritage.Add();
				IsekaiAngelHeritage.Add();
				IsekaiVampireHeritage.Add();
				IsekaiSprigganHeritage.Add();
				IsekaiDarkElfHeritage.Add();
				IsekaiHighElfHeritage.Add();
				IsekaiWoodElfHeritage.Add();
				IsekaiKitsuneHeritage.Add();
				IsekaiHighHumanHeritage.Add();
				IsekaiImmortalHumanHeritage.Add();
				GnomeOfTheFirstWorld.Add();
				DemonLordHeritage.Add();
				IsekaiWerewolfHeritage.Add();
				IsekaiDragonLordHeritage.Add();
				IsekaiTitanDwarfHeritage.Add();
				IsekaiFateweaverHeritage.Add();
				IsekaiCataclysmBerserkerHeritage.Add();
				IsekaiHumanCrossbreedLegacy.Add();
				HeteromorphicOverlordHeritage.Add();
				SlimeReincarnateHeritage.Add();
				ElfHeritagePatcher.Patch();
			}

			public static void AddIsekaiBackgrounds()
			{
				IsekaiBackgroundSelection.Add();
				TabletopRPGPlayer.Add();
				MartialArtist.Add();
				Salaryman.Add();
				HighschoolStudent.Add();
				RebornDemonLord.Add();
				Otaku.Add();
				Gamer.Add();
				BetaTester.Add();
				DemonicCultivator.Add();
				EnlightenedSage.Add();
				Musician.Add();
				Rationalist.Add();
				CollegeStudent.Add();
				Speedrunner.Add();
				WebNovelAuthor.Add();
				Surgeon.Add();
				ModernSoldier.Add();
				SoftwareEngineer.Add();
				MasterChef.Add();
				ChessGrandmaster.Add();
				TruckDriver.Add();
				AnimeReferenceBackgrounds.Add();
			}

			public static void AddIsekaiDeities()
			{
				IsekaiDeitySelection.Add();
				YogSothoth.Add();
				BlackButterfly.Add();
				Milani.Add();
				LanternKing.Add();
				Besmara.Add();
				Chaldira.Add();
			}

			public static void AddExceptionalFeats()
			{
				ExceptionalFeatSelection.Add();
				EffectImmunitySelection.Add();
				ExceptionalSummoningSelection.Add();
				ExceptionalWeaponSelection.Add();
			}

			public static void RestrictExceptionalFeats()
			{
				ExceptionalFeatSelection.Get()?.AddPrerequisite(delegate(PrerequisiteClassLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.Level = 1;
				});
			}
		}
	}
}
