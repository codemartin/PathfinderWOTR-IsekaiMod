using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class ShadowMonarchArchetype
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "ShadowMonarchArchetype.Name", "Shadow Monarch");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "ShadowMonarchArchetype.Description", "The sovereign commander of the dead. Rather than raising shambling corpses, the Shadow Monarch extracts the lingering shadows of fallen warriors, binding them into an eternal, loyal army. Commanding shadows, dimensional steps, and dark authority, they stand as the ultimate hunter.");

		public static void Add()
		{
			BlueprintFeature ShadowMonarchProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies");
			BlueprintFeature ShadowHunterInstincts = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowHunterInstincts");
			BlueprintFeature ShadowExtractionFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowExtractionFeature");
			BlueprintFeature ShadowMonarchFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchFeature");
			BlueprintFeature ShadowStepFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowStepFeature");
			BlueprintFeature ShadowArmorFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowArmorFeature");
			BlueprintFeature MonarchDomainFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MonarchDomainFeature");
			BlueprintFeature ShadowExchangeFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowExchangeFeature");
			BlueprintFeature IsekaiProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			BlueprintFeature ReleaseEnergy = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
			BlueprintFeature Gifted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
			BlueprintFeature SecondReincarnation = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
			BlueprintFeatureSelection HaxSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
			BlueprintFeatureSelection SecretPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
			BlueprintFeatureSelection SignatureMoveBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveBonusSelection");
			BlueprintFeatureSelection TrainingEpisodeBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			BlueprintFeature ChronicleOtherworldFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
			BlueprintFeature HuntersLogFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HuntersLogFeature");
			IsekaiProtagonistClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchArchetype", delegate(BlueprintArchetype bp)
			{
				bp.LocalizedName = Name;
				bp.LocalizedDescription = Description;
				bp.LocalizedDescriptionShort = Description;
				bp.IsArcaneCaster = true;
				bp.IsDivineCaster = true;
				bp.RemoveFeatures = new LevelEntry[7]
				{
					Helpers.CreateLevelEntry(1, IsekaiProficiencies, Gifted, LegacySelection.GetClassFeature(), ChronicleOtherworldFeature),
					Helpers.CreateLevelEntry(3, ReleaseEnergy),
					Helpers.CreateLevelEntry(6, SignatureMoveBonusSelection),
					Helpers.CreateLevelEntry(10, SecretPowerSelection),
					Helpers.CreateLevelEntry(12, TrainingEpisodeBonusSelection),
					Helpers.CreateLevelEntry(15, SecondReincarnation),
					Helpers.CreateLevelEntry(20, HaxSelection)
				};
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowEssenceAbsorptionFeature");
				BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "RulersAuthorityFeature");
				BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowDaggerRushFeature");
				BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowCloakFeature");
				BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MonarchDragonsFearFeature");
				bp.AddFeatures = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, ShadowMonarchProficiencies, ShadowHunterInstincts, ShadowExtractionFeature, modBlueprint, HuntersLogFeature, ShadowMonarchLegacySelection.getClassFeature()),
					Helpers.CreateLevelEntry(3, ShadowStepFeature, modBlueprint2),
					Helpers.CreateLevelEntry(5, modBlueprint3),
					Helpers.CreateLevelEntry(7, ShadowArmorFeature, modBlueprint4),
					Helpers.CreateLevelEntry(10, MonarchDomainFeature),
					Helpers.CreateLevelEntry(11, modBlueprint5),
					Helpers.CreateLevelEntry(15, ShadowExchangeFeature),
					Helpers.CreateLevelEntry(20, ShadowMonarchFeature)
				};
				bp.OverrideAttributeRecommendations = true;
				bp.RecommendedAttributes = new StatType[2]
				{
					StatType.Charisma,
					StatType.Dexterity
				};
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "ShadowMonarch";
				});
				bp.m_ReplaceSpellbook = ShadowMonarchSpellbook.GetReference();
			}));
		}

		public static BlueprintArchetype Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "ShadowMonarchArchetype");
		}

		public static BlueprintArchetypeReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "ShadowMonarchArchetype");
		}
	}
}
