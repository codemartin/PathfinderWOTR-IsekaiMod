using System;
using System.Collections.Generic;
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
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.PhantomBlade;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;
using IsekaiMod.Content.Features.IsekaiProtagonist.SoloMastery;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Content.Features.IsekaiProtagonist.TrainingEpisode;
using IsekaiMod.Content.GlobalMap;
using IsekaiMod.Content.Guardians;
using IsekaiMod.Content.Heritages;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content;

internal class ContentAdder
{
	[HarmonyPatch(typeof(BlueprintsCache), "Init")]
	private static class BlueprintsCache_Init_Patch
	{
		private static bool Initialized;

		private static readonly AddedContent AddedContent = Main.IsekaiContext.AddedContent;

		private const string Phase = "CreateNewBlueprints";

		[HarmonyPriority(800)]
		[HarmonyPostfix]
		public static void CreateNewBlueprints()
		{
			if (!Initialized)
			{
				Initialized = true;
				SafeStep("PerPlaythroughState", delegate
				{
					PerPlaythroughState.Init();
				});
				SafeStep("ExtraWings.Add", ExtraWings.Add);
				SafeStep("ExoticWeaponProficiency.Add", ExoticWeaponProficiency.Add);
				SafeStep("AddExceptionalFeats", AddExceptionalFeats);
				SafeStep("AddIsekaiBackgrounds", AddIsekaiBackgrounds);
				SafeStep("AddIsekaiDeities", AddIsekaiDeities);
				SafeStep("AddIsekaiHeritages", AddIsekaiHeritages);
				SafeStep("ConstellationBlueprints", CreateConstellationBlueprints);
				if (ContentOffering.ShouldRestrictExceptionalFeats(ContentOffering.IsOffered(OfferedContent.IsekaiProtagonist), AddedContent.RestrictExceptionalFeats))
				{
					SafeStep("RestrictExceptionalFeats", RestrictExceptionalFeats);
				}
				SafeStep("AddIsekaiProtagonistClass", AddIsekaiProtagonistClass);
				SafeStep("AddConstellations", AddConstellations);
				SafeStep("AddIsekaiDialogue", AddIsekaiDialogue);
				SafeStep("BlueprintTextDefaults", BlueprintTextDefaultsPatch.FillComponents);
				SafeStep("ElementDefaults", ElementDefaults.FillAll);
				SafeStep("RegressionSanityCheck", RegressionSanityCheck.Run);
				RegressionSanityCheck.LogStepFailureSummary("CreateNewBlueprints");
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
				RegressionSanityCheck.RecordStepFailure("CreateNewBlueprints", name);
				Main.IsekaiContext.Logger.LogError($"[Startup Warning] Subsystem '{name}' threw an exception: {arg}");
			}
		}

		public static void CreateConstellationBlueprints()
		{
			SafeStep("DivineTokens.CreateBlueprints", DivineTokens.CreateBlueprints);
			SafeStep("DivineSponsorshipStore.CreateBlueprints", DivineSponsorshipStore.CreateBlueprints);
			SafeStep("CosmicBarrenBosses.CreateBlueprints", CosmicBarrenBosses.CreateBlueprints);
			SafeStep("DimensionalRiftIncursions.CreateBlueprints", DimensionalRiftIncursions.CreateBlueprints);
			SafeStep("EpicPrestigeClasses.CreateBlueprints", EpicPrestigeClasses.CreateBlueprints);
		}

		public static void AddConstellations()
		{
			SafeStep("DeityAvatarAscension", delegate
			{
				DeityAvatarAscension.Add();
			});
			SafeStep("CosmicRelics", delegate
			{
				CosmicRelics.Add();
			});
			SafeStep("DivineSponsorshipStore", delegate
			{
				DivineSponsorshipStore.Add();
			});
			SafeStep("CosmicConsumables", delegate
			{
				CosmicConsumables.Add();
			});
			SafeStep("DivineTokens", delegate
			{
				DivineTokens.Init();
			});
			SafeStep("ForbiddenCheatRecord", delegate
			{
				ForbiddenCheatRecord.Init();
			});
			SafeStep("KeptRewards", delegate
			{
				KeptRewards.Init();
			});
			SafeStep("CosmicExchangeGrant", delegate
			{
				CosmicExchangeGrant.Init();
			});
			SafeStep("TimelineManager", delegate
			{
				TimelineManager.Init();
			});
			SafeStep("ConstellationChatManager", delegate
			{
				ConstellationChatManager.Init();
			});
			SafeStep("CodexOfReincarnation", delegate
			{
				CodexOfReincarnation.Add();
			});
			SafeStep("CodexOfReincarnationDelivery", delegate
			{
				CodexOfReincarnation.Init();
			});
			SafeStep("DimensionalArenaManager", delegate
			{
				DimensionalArenaManager.Init();
			});
			SafeStep("PlanarColiseumArea", delegate
			{
				PlanarColiseumArea.Add();
			});
			SafeStep("PlanarColiseumMapPoint", delegate
			{
				PlanarColiseumMapPoint.Add();
			});
			SafeStep("BossSites", delegate
			{
				BossSites.Add();
			});
			SafeStep("BossSiteMapPoints", delegate
			{
				BossSiteMapPoints.Add();
			});
			SafeStep("BossSiteSpawner", delegate
			{
				BossSiteSpawner.Add();
			});
			SafeStep("PlanarColiseumEntrance", delegate
			{
				PlanarColiseumEntrance.Add();
			});
			SafeStep("CosmicRelicsVendorTable", delegate
			{
				CosmicRelicsVendorTable.Add();
			});
			SafeStep("OtherworldLuxuryVendorTable", delegate
			{
				OtherworldLuxuryVendorTable.Add();
			});
			SafeStep("PlanarGatewayEmissary", delegate
			{
				PlanarGatewayEmissary.Add();
			});
			SafeStep("PlanarColiseumHubManager", delegate
			{
				PlanarColiseumHubManager.Init();
			});
			SafeStep("ColiseumUiStateGuard", delegate
			{
				ColiseumUiStateGuard.Init();
			});
			SafeStep("ItemRenderGuard", delegate
			{
				ItemRenderGuard.Init();
			});
			SafeStep("LevelUncapperRecords", delegate
			{
				LevelUncapperPatch.Init();
			});
			SafeStep("HumanHeritageRepair", delegate
			{
				HumanHeritageSelection.InitRepair();
			});
			SafeStep("GuardianRoles.Init", delegate
			{
				GuardianRoles.Init();
			});
			SafeStep("IsekaiColiseumQuest", delegate
			{
				IsekaiColiseumQuest.Add();
			});
			SafeStep("ShardOfTheShatteredLoop", delegate
			{
				ShardOfTheShatteredLoop.Add();
			});
			SafeStep("EchoOfPastSovereigns", delegate
			{
				EchoOfPastSovereigns.Add();
			});
			SafeStep("IsekaiTransmigrationQuest", delegate
			{
				IsekaiTransmigrationQuest.Add();
			});
			SafeStep("DimensionalRiftIncursions", delegate
			{
				DimensionalRiftIncursions.Init();
			});
			SafeStep("IsekaiBarrenAnomaliesQuest", delegate
			{
				IsekaiBarrenAnomaliesQuest.Add();
			});
			SafeStep("SubclassPersonalQuests", delegate
			{
				SubclassPersonalQuests.Add();
			});
			SafeStep("PersonalQuestTrialDialog", delegate
			{
				PersonalQuestTrialDialog.Add();
			});
			SafeStep("SylvanFeyAllianceQuest", delegate
			{
				SylvanFeyAllianceQuest.Add();
			});
			SafeStep("MythicQuestSynergies", delegate
			{
				MythicQuestSynergies.Add();
			});
			SafeStep("IsekaiKingdomProjects", delegate
			{
				IsekaiKingdomProjects.Add();
			});
			SafeStep("IsekaiSettlementBuildings", delegate
			{
				IsekaiSettlementBuildings.Add();
			});
			SafeStep("SubclassRespecManager", delegate
			{
				SubclassRespecManager.Init();
			});
			SafeStep("CosmicThreatScaling", delegate
			{
				CosmicThreatScaling.Add();
			});
			SafeStep("RivalReincarnators", delegate
			{
				RivalReincarnators.Add();
			});
			SafeStep("DimensionalReinforcements", delegate
			{
				DimensionalReinforcements.Add();
			});
			SafeStep("ConstellationCombatReactions", delegate
			{
				ConstellationCombatReactions.Init();
			});
			SafeStep("CosmicBarrenBosses", delegate
			{
				CosmicBarrenBosses.Add();
			});
		}

		public static void AddIsekaiProtagonistClass()
		{
			LegacySelection.ConfigureStep1();
			IsekaiProtagonistSpellList.Add();
			IsekaiProtagonistSpellsPerDay.Add();
			IsekaiProtagonistSpellsKnown.Add();
			IsekaiProtagonistSpellbook.Add();
			IsekaiProtagonistClass.Add();
			SafeStep("TrainingEpisode", delegate
			{
				TrainingEpisodeSelection.Add();
				HealthyBody.Add();
				InnerPower.Add();
				MasterSelf.Add();
				Tenacious.Add();
			});
			SafeStep("SpecialPower", delegate
			{
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
				ArchetypeExclusivePowers.Add();
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
			});
			SafeStep("EchoOfPastLegacies", EchoOfPastLegacies.Add);
			SafeStep("OverpoweredAbility", delegate
			{
				OverpoweredAbilitySelection.Add();
				AutoMetamagicSelection.Add();
				Instakill.Add();
				MerchantsGamble.Add();
				PerfectRoll.Add();
				SuperBuff.Add();
				UnlimitedPower.Add();
				MindControl.Add();
				SummonCalamityPool.Add();
				SummonCalamity.Add();
				InfiniteSpace.Add();
				TrueResurrection.Add();
				SupremeBeing.Add();
				MetaLuck.Add();
				PowerLeveling.Add();
				MasterSummoner.Add();
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
				ChronoSurge.Add();
				Port62ForbiddenCheats.Add();
				BlessingOfTheMythic.Configure();
			});
			SafeStep("IsekaiProtagonistFeatures", delegate
			{
				IsekaiProficiencies.Add();
				SubclassCantrips.Add();
				IsekaiCantrips.Add();
				IsekaiBonusFeatSelection.Add();
				OtherworldScavenger.Add();
				IsekaiTalentSelection.Add();
				GuardianCompanionClass.Add();
				GuardianProgressions.Add();
				GuardianBarks.Add();
				IsekaiGuardians.Add();
				SoloSovereign.Add();
				SafeStep("Guardians.TwinAvatarCompanionUnit.Add", TwinAvatarCompanionUnit.Add);
				SafeStep("Features.IsekaiProtagonist.TwinAvatarCompanion.Add", TwinAvatarCompanion.Add);
				IsekaiPetSelection.Add();
				PlotArmor.Add();
				IsekaiSpontaneousMetamagic.Add();
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
			});
			SafeStep("GodEmperorArchetype", delegate
			{
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
			});
			SafeStep("MartialGodArchetype", delegate
			{
				PhantomBladeProficiencies.Add();
				PhantomFlurry.Add();
				PhantomDimensionalBlink.Add();
				PhantomVelocity.Add();
				ExtraSpecialPowerSelection.Add();
				MartialGodArchetype.Add();
			});
			SafeStep("HeroArchetype", delegate
			{
				HeroProficiencies.Add();
				GracefulCombat.Add();
				HandsOfSalvation.Add();
				DeusExMachina.Add();
				BondsOfFellowship.Add();
				RetributionMiracle.Add();
				HeroOfTimeFeatures.Add();
				HeroArchetype.Add();
			});
			SafeStep("MastermindArchetype", delegate
			{
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
			});
			SafeStep("ArchetypeHeritageTraits", ArchetypeHeritageTraits.Add);
			SafeStep("OverlordArchetype", delegate
			{
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
			});
			SafeStep("DevourerArchetype", delegate
			{
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
				GluttonyPredator.Add();
			});
			SafeStep("ShadowMonarchArchetype", delegate
			{
				ShadowMonarchSummonPool.Add();
				ShadowMonarchProficiencies.Add();
				ShadowMonarchSpellbook.Add();
				ShadowHunterInstincts.Add();
				ShadowExtraction.Add();
				ShadowStep.Add();
				ShadowArmor.Add();
				MonarchDomain.Add();
				ShadowExchange.Add();
				ShadowMonarchArts.Add();
				ShadowMonarch.Add();
				ShadowMonarchArchetype.Add();
			});
			SafeStep("StartingWeaponProficiencyFix", StartingWeaponProficiencyFix.Add);
			SafeStep("PrebuildIsekaiProtagonistFeatureList", PrebuildIsekaiProtagonistFeatureList.Add);
			SafeStep("IsekaiProtagonistProgression", IsekaiProtagonistProgression.Add);
			SafeStep("ArchetypeExclusivePowers.Lock", ArchetypeExclusivePowers.Lock);
			SafeStep("PrestigeClassReplaceSpellbook", PrestigeClassReplaceSpellbook.Patch);
			SafeStep("IsekaiAscendantClass", delegate
			{
				IsekaiAscendantClass.Add();
				PatchTools.RegisterClassLevelContinuation(IsekaiProtagonistClass.GetReference(), IsekaiAscendantClass.GetReference());
				IsekaiAscendantProgression.Add();
				IsekaiAscendantArchetypes.Add();
			});
			SafeStep("Deathsnatcher", delegate
			{
				DeathsnatcherClass.Add();
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
			});
			SafeStep("SubclassGuardians", delegate
			{
				SubclassGuardianProgressions.Add();
				SubclassGuardians.Add();
				GuardianArchetypes.Add();
				OtherworldGuardianPactSelection.Add();
			});
			SafeStep("GuardianRoles", delegate
			{
				GuardianRoles.Add();
			});
			SafeStep("TranscendentSovereign", delegate
			{
				TranscendentSovereignClass.Add();
				TranscendentSovereignFeatures.Add();
				TranscendentSovereignProgression.Add();
				TranscendentSovereignArchetypes.Add();
			});
			SafeStep("EpicPrestigeClasses", EpicPrestigeClasses.Add);
			SafeStep("OtherworldRetinuePrestige", OtherworldRetinuePrestige.Add);
			SafeStep("LegacySelection.ConfigureStep2", LegacySelection.ConfigureStep2);
			SafeStep("ExtraSpecialPowerSelection.Resync", ExtraSpecialPowerSelection.Resync);
		}

		public static void AddIsekaiDialogue()
		{
			List<BlueprintAnswer> list = (TTCoreExtensions.AnswerRecorder = new List<BlueprintAnswer>());
			try
			{
				AddIsekaiDialogueGrafts();
			}
			finally
			{
				TTCoreExtensions.AnswerRecorder = null;
			}
			foreach (BlueprintAnswer item in list)
			{
				ContentOffering.ShowOnlyWhenOffered(item, OfferedContent.IsekaiDialogue);
			}
		}

		private static void AddIsekaiDialogueGrafts()
		{
			SafeStep("Dialogue.Port62Rewards", Port62Rewards.Create);
			SafeStep("Dialogue.IsekaiPrologueExpansions", IsekaiPrologueExpansions.Add);
			SafeStep("Dialogue.IsekaiHulrun", IsekaiHulrun.Add);
			SafeStep("Dialogue.IsekaiRadiance", IsekaiRadiance.Add);
			SafeStep("Dialogue.IsekaiKaylessaDrowLeader", IsekaiKaylessaDrowLeader.Add);
			SafeStep("Dialogue.IsekaiHorgus", IsekaiHorgus.Add);
			SafeStep("Dialogue.IsekaiMinagho", IsekaiMinagho.Add);
			SafeStep("Dialogue.IsekaiRomanceDialogue", IsekaiRomanceDialogue.Add);
			SafeStep("Dialogue.SubclassDialogueReactions", SubclassDialogueReactions.Add);
			SafeStep("Dialogue.IsekaiFinnean", IsekaiFinnean.Add);
			SafeStep("Dialogue.IsekaiNenioStatue", IsekaiNenioStatue.Add);
			SafeStep("Dialogue.CompanionRetinueDialogue", CompanionRetinueDialogue.Add);
			SafeStep("Dialogue.RecruitBothLannWenduag", RecruitBothLannWenduag.Add);
			SafeStep("Dialogue.Port62Act2", Port62Act2.Add);
			SafeStep("Dialogue.Port62Act3", Port62Act3.Add);
			SafeStep("Dialogue.Port62CampHubs", Port62CampHubs.Add);
			SafeStep("Dialogue.Port62Act1", Port62Act1.Add);
			SafeStep("Dialogue.Port62Act4", Port62Act4.Add);
			SafeStep("Dialogue.Port62Act5", Port62Act5.Add);
			SafeStep("Dialogue.Port62Act6Dlc", Port62Act6Dlc.Add);
		}

		public static void AddIsekaiHeritages()
		{
			SafeStep("Heritages.HumanHeritageSelection.CreateDummy", HumanHeritageSelection.CreateDummy);
			SafeStep("Heritages.IsekaiSuccubusHeritage", IsekaiSuccubusHeritage.Add);
			SafeStep("Heritages.IsekaiAngelHeritage", IsekaiAngelHeritage.Add);
			SafeStep("Heritages.IsekaiVampireHeritage", IsekaiVampireHeritage.Add);
			SafeStep("Heritages.IsekaiSprigganHeritage", IsekaiSprigganHeritage.Add);
			SafeStep("Heritages.IsekaiDarkElfHeritage", IsekaiDarkElfHeritage.Add);
			SafeStep("Heritages.IsekaiHighElfHeritage", IsekaiHighElfHeritage.Add);
			SafeStep("Heritages.IsekaiWoodElfHeritage", IsekaiWoodElfHeritage.Add);
			SafeStep("Heritages.IsekaiKitsuneHeritage", IsekaiKitsuneHeritage.Add);
			SafeStep("Heritages.IsekaiHighHumanHeritage", IsekaiHighHumanHeritage.Add);
			SafeStep("Heritages.IsekaiImmortalHumanHeritage", IsekaiImmortalHumanHeritage.Add);
			SafeStep("Heritages.GnomeOfTheFirstWorld", GnomeOfTheFirstWorld.Add);
			SafeStep("Heritages.DemonLordHeritage", DemonLordHeritage.Add);
			SafeStep("Heritages.IsekaiWerewolfHeritage", IsekaiWerewolfHeritage.Add);
			SafeStep("Heritages.IsekaiDragonLordHeritage", IsekaiDragonLordHeritage.Add);
			SafeStep("Heritages.IsekaiTitanDwarfHeritage", IsekaiTitanDwarfHeritage.Add);
			SafeStep("Heritages.IsekaiFateweaverHeritage", IsekaiFateweaverHeritage.Add);
			SafeStep("Heritages.IsekaiCataclysmBerserkerHeritage", IsekaiCataclysmBerserkerHeritage.Add);
			SafeStep("Heritages.IsekaiHumanCrossbreedLegacy", IsekaiHumanCrossbreedLegacy.Add);
			SafeStep("Heritages.HeteromorphicOverlordHeritage", HeteromorphicOverlordHeritage.Add);
			SafeStep("Heritages.SlimeReincarnateHeritage", SlimeReincarnateHeritage.Add);
		}

		public static void AddIsekaiBackgrounds()
		{
			SafeStep("Backgrounds.IsekaiBackgroundSelection", IsekaiBackgroundSelection.Add);
			SafeStep("Backgrounds.TabletopRPGPlayer", TabletopRPGPlayer.Add);
			SafeStep("Backgrounds.MartialArtist", MartialArtist.Add);
			SafeStep("Backgrounds.Salaryman", Salaryman.Add);
			SafeStep("Backgrounds.HighschoolStudent", HighschoolStudent.Add);
			SafeStep("Backgrounds.RebornDemonLord", RebornDemonLord.Add);
			SafeStep("Backgrounds.Otaku", Otaku.Add);
			SafeStep("Backgrounds.Gamer", Gamer.Add);
			SafeStep("Backgrounds.BetaTester", BetaTester.Add);
			SafeStep("Backgrounds.DemonicCultivator", DemonicCultivator.Add);
			SafeStep("Backgrounds.EnlightenedSage", EnlightenedSage.Add);
			SafeStep("Backgrounds.Musician", Musician.Add);
			SafeStep("Backgrounds.Rationalist", Rationalist.Add);
			SafeStep("Backgrounds.CollegeStudent", CollegeStudent.Add);
			SafeStep("Backgrounds.Speedrunner", Speedrunner.Add);
			SafeStep("Backgrounds.WebNovelAuthor", WebNovelAuthor.Add);
			SafeStep("Backgrounds.Surgeon", Surgeon.Add);
			SafeStep("Backgrounds.ModernSoldier", ModernSoldier.Add);
			SafeStep("Backgrounds.SoftwareEngineer", SoftwareEngineer.Add);
			SafeStep("Backgrounds.MasterChef", MasterChef.Add);
			SafeStep("Backgrounds.ChessGrandmaster", ChessGrandmaster.Add);
			SafeStep("Backgrounds.TruckDriver", TruckDriver.Add);
			SafeStep("Backgrounds.AnimeReferenceBackgrounds", AnimeReferenceBackgrounds.Add);
		}

		public static void AddIsekaiDeities()
		{
			SafeStep("Deities.IsekaiDeitySelection", IsekaiDeitySelection.Add);
			SafeStep("Deities.YogSothoth", YogSothoth.Add);
			SafeStep("Deities.BlackButterfly", BlackButterfly.Add);
			SafeStep("Deities.Milani", Milani.Add);
			SafeStep("Deities.LanternKing", LanternKing.Add);
			SafeStep("Deities.Besmara", Besmara.Add);
			SafeStep("Deities.Chaldira", Chaldira.Add);
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
			ExceptionalFeatSelection.Get().AddPrerequisite(delegate(PrerequisiteClassLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.Level = 1;
			});
		}
	}
}
