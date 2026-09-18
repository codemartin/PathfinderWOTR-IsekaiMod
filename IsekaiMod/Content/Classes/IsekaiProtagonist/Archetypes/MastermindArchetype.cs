using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
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
	internal class MastermindArchetype
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "MastermindArchetype.Name", "Mastermind");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "MastermindArchetype.Description", "The mastermind has an unparalleled intellect in the new world. They are able to predict the enemies' movements four parallel universes in advance, outsmarting them with mind-boggling strategies using knowledge from their old world.\nYou cast spells like an Arcanist with a number of slots equal to your spells per day.");

		public static void Add()
		{
			BlueprintFeature MastermindProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindProficiencies");
			BlueprintFeature MastermindConsumeSpells = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindConsumeSpells");
			BlueprintFeature MastermindQuickFooted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindQuickFooted");
			BlueprintFeature SignatureAbility = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SignatureAbility");
			BlueprintFeature MasterplanFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MasterplanFeature");
			BlueprintFeature ArcanistArcaneReservoirFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("55db1859bd72fd04f9bd3fe1f10e4cbb");
			BlueprintFeature EldritchFontEldritchSurge = BlueprintTools.GetBlueprint<BlueprintFeature>("644c0e9618e417947bd0a1252a5e6ecf");
			BlueprintFeature EldritchFontImprovedSurge = BlueprintTools.GetBlueprint<BlueprintFeature>("718fe8e143d38cc4899ae798dd098b6e");
			BlueprintFeature EldritchFontGreaterSurge = BlueprintTools.GetBlueprint<BlueprintFeature>("685ee64e43fcb6546b65436a3deb98bd");
			BlueprintFeatureSelection ArcanistExploitSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b8bf3d5023f2d8c428fdf6438cecaea7");
			// Every greater exploit requires this marker, which the Arcanist progression grants at level 11.
			BlueprintFeature ArcanistGreaterExploitsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c7536b93f17c70d4fa3a8cf9aa76bfb7");
			BlueprintFeatureSelection AutoMetamagicSelectionMastermind = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "AutoMetamagicSelectionMastermind");
			BlueprintFeature IsekaiProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			BlueprintFeature ReleaseEnergy = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
			BlueprintFeature Gifted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
			BlueprintFeature Afterimage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Afterimage");
			BlueprintFeature IsekaiQuickFooted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiQuickFooted");
			BlueprintFeature SecondReincarnation = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
			BlueprintFeature OtherworldlyStamina = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OtherworldlyStamina");
			BlueprintFeature IsekaiFighterTraining = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiFighterTraining");
			BlueprintFeatureSelection StartingWeaponSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "StartingWeaponSelection");
			BlueprintFeatureSelection SpecialPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
			BlueprintFeatureSelection SecretPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
			BlueprintFeatureSelection HaxSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
			BlueprintFeatureSelection SignatureMoveBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveBonusSelection");
			BlueprintFeatureSelection SignatureMoveSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveSelection");
			BlueprintFeatureSelection TrainingEpisodeBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			BlueprintFeatureSelection IsekaiBonusFeatSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
			BlueprintFeature ChronicleOtherworldFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
			BlueprintFeature GrandStrategyFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GrandStrategyFeature");
			IsekaiProtagonistClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindArchetype", delegate(BlueprintArchetype bp)
			{
				bp.LocalizedName = Name;
				bp.LocalizedDescription = Description;
				bp.LocalizedDescriptionShort = Description;
				bp.IsArcaneCaster = true;
				bp.IsDivineCaster = true;
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TranscendentProtagonistFeature");
				BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AnimeFinalFormFeature");
				BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindGrandmasterForesightEpic");
				BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindAbsoluteCheckmate");
				bp.RemoveFeatures = new LevelEntry[36]
				{
					Helpers.CreateLevelEntry(1, IsekaiBonusFeatSelection, IsekaiProficiencies, StartingWeaponSelection, Gifted, LegacySelection.GetClassFeature(), ChronicleOtherworldFeature),
					Helpers.CreateLevelEntry(2, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(3, ReleaseEnergy, IsekaiFighterTraining),
					Helpers.CreateLevelEntry(4, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(6, IsekaiBonusFeatSelection, SignatureMoveSelection, SignatureMoveBonusSelection),
					Helpers.CreateLevelEntry(8, IsekaiBonusFeatSelection, Afterimage),
					Helpers.CreateLevelEntry(9, SpecialPowerSelection),
					Helpers.CreateLevelEntry(10, IsekaiBonusFeatSelection, SecretPowerSelection),
					Helpers.CreateLevelEntry(11, SpecialPowerSelection),
					Helpers.CreateLevelEntry(12, IsekaiBonusFeatSelection, TrainingEpisodeBonusSelection),
					Helpers.CreateLevelEntry(13, OtherworldlyStamina),
					Helpers.CreateLevelEntry(14, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(15, IsekaiQuickFooted, SecondReincarnation),
					Helpers.CreateLevelEntry(16, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(18, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(20, IsekaiBonusFeatSelection, HaxSelection),
					Helpers.CreateLevelEntry(21, SpecialPowerSelection),
					Helpers.CreateLevelEntry(22, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(23, SpecialPowerSelection),
					Helpers.CreateLevelEntry(24, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(25, SpecialPowerSelection),
					Helpers.CreateLevelEntry(26, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(27, SpecialPowerSelection),
					Helpers.CreateLevelEntry(28, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(29, SpecialPowerSelection),
					Helpers.CreateLevelEntry(30, IsekaiBonusFeatSelection, SecretPowerSelection, modBlueprint),
					Helpers.CreateLevelEntry(31, SpecialPowerSelection),
					Helpers.CreateLevelEntry(32, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(33, SpecialPowerSelection),
					Helpers.CreateLevelEntry(34, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(35, SpecialPowerSelection),
					Helpers.CreateLevelEntry(36, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(37, SpecialPowerSelection),
					Helpers.CreateLevelEntry(38, IsekaiBonusFeatSelection),
					Helpers.CreateLevelEntry(39, SpecialPowerSelection),
					Helpers.CreateLevelEntry(40, IsekaiBonusFeatSelection, HaxSelection, modBlueprint2)
				};
				BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TacticalAmbushFeature");
				BlueprintFeature modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindGrandmasterForesight");
				BlueprintFeature modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindPersuasionTake20");
				BlueprintFeature modBlueprint8 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindCheckmateGambit");
				BlueprintFeature modBlueprint9 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindZugzwang");
				BlueprintFeature modBlueprint10 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindCalculatedSacrifice");
				BlueprintFeature modBlueprint11 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindGeassOfAbsoluteCommand");
				bp.AddFeatures = new LevelEntry[23]
				{
					Helpers.CreateLevelEntry(1, MastermindProficiencies, AutoMetamagicSelectionMastermind, ArcanistArcaneReservoirFeature, MastermindConsumeSpells, GrandStrategyFeature, modBlueprint6, modBlueprint7, MastermindLegacySelection.getClassFeature()),
					Helpers.CreateLevelEntry(3, modBlueprint5, ArcanistExploitSelection, EldritchFontEldritchSurge),
					Helpers.CreateLevelEntry(5, AutoMetamagicSelectionMastermind, modBlueprint8),
					Helpers.CreateLevelEntry(6, SignatureAbility),
					Helpers.CreateLevelEntry(7, modBlueprint5, ArcanistExploitSelection, EldritchFontImprovedSurge),
					Helpers.CreateLevelEntry(9, AutoMetamagicSelectionMastermind, modBlueprint9),
					Helpers.CreateLevelEntry(11, modBlueprint5, ArcanistGreaterExploitsFeature, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(13, AutoMetamagicSelectionMastermind, EldritchFontGreaterSurge, modBlueprint10),
					Helpers.CreateLevelEntry(15, modBlueprint5, ArcanistExploitSelection, MastermindQuickFooted),
					Helpers.CreateLevelEntry(17, AutoMetamagicSelectionMastermind, modBlueprint11),
					Helpers.CreateLevelEntry(19, modBlueprint5, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(20, MasterplanFeature),
					Helpers.CreateLevelEntry(21, modBlueprint5, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(23, AutoMetamagicSelectionMastermind),
					Helpers.CreateLevelEntry(25, modBlueprint5, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(27, AutoMetamagicSelectionMastermind),
					Helpers.CreateLevelEntry(29, modBlueprint5, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(30, modBlueprint3),
					Helpers.CreateLevelEntry(33, AutoMetamagicSelectionMastermind),
					Helpers.CreateLevelEntry(35, modBlueprint5, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(37, AutoMetamagicSelectionMastermind),
					Helpers.CreateLevelEntry(39, modBlueprint5, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(40, modBlueprint4)
				};
				bp.OverrideAttributeRecommendations = true;
				bp.m_ReplaceSpellbook = MastermindSpellbook.GetReference();
				bp.RecommendedAttributes = new StatType[1] { StatType.Intelligence };
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "Mastermind";
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
				bp.RemoveSpellbook = Main.IsekaiContext.AddedContent.DisableSpellbookMastermind;
			}));
		}

		public static BlueprintArchetype Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "MastermindArchetype");
		}

		public static BlueprintArchetypeReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "MastermindArchetype");
		}
	}
}
