using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
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
				bp.ChangeCasterType = true;
				bp.RemoveFeatures = new LevelEntry[16]
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
					Helpers.CreateLevelEntry(20, IsekaiBonusFeatSelection, HaxSelection)
				};
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TacticalAmbushFeature");
				BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindGrandmasterForesight");
				BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindPersuasionTake20");
				BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindCheckmateGambit");
				BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindZugzwang");
				BlueprintFeature modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindCalculatedSacrifice");
				BlueprintFeature modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindGeassOfAbsoluteCommand");
				bp.AddFeatures = new LevelEntry[12]
				{
					Helpers.CreateLevelEntry(1, MastermindProficiencies, AutoMetamagicSelectionMastermind, ArcanistArcaneReservoirFeature, MastermindConsumeSpells, GrandStrategyFeature, modBlueprint2, modBlueprint3, MastermindLegacySelection.getClassFeature()),
					Helpers.CreateLevelEntry(3, modBlueprint, ArcanistExploitSelection, EldritchFontEldritchSurge),
					Helpers.CreateLevelEntry(5, AutoMetamagicSelectionMastermind, modBlueprint4),
					Helpers.CreateLevelEntry(6, SignatureAbility),
					Helpers.CreateLevelEntry(7, modBlueprint, ArcanistExploitSelection, EldritchFontImprovedSurge),
					Helpers.CreateLevelEntry(9, AutoMetamagicSelectionMastermind, modBlueprint5),
					Helpers.CreateLevelEntry(11, modBlueprint, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(13, AutoMetamagicSelectionMastermind, EldritchFontGreaterSurge, modBlueprint6),
					Helpers.CreateLevelEntry(15, modBlueprint, ArcanistExploitSelection, MastermindQuickFooted),
					Helpers.CreateLevelEntry(17, AutoMetamagicSelectionMastermind, modBlueprint7),
					Helpers.CreateLevelEntry(19, modBlueprint, ArcanistExploitSelection),
					Helpers.CreateLevelEntry(20, MasterplanFeature)
				};
				bp.OverrideAttributeRecommendations = true;
				bp.m_ReplaceSpellbook = MastermindSpellbook.GetReference();
				bp.RecommendedAttributes = new StatType[1] { StatType.Intelligence };
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "Mastermind";
				});
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
