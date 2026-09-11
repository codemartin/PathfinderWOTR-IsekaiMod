using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	public class MartialGodArchetype
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "MartialGodArchetype.Name", "Martial God");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "MartialGodArchetype.Description", "After reincarnating into Golarion, some protagonists master the absolute apex of martial perfection. Transcending the boundary between weapons, bare fists, and spiritual power, they flow across the battlefield with divine speed and lethal precision.");

		public static BlueprintArchetype Archetype { get; private set; }

		public static void Add()
		{
			MartialTranscendence.Add();
			MartialSoulAwakening.Add();
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialGodProficiencies");
			BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialTranscendenceFeature");
			BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialSoulAwakeningStage1");
			BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialSoulAwakeningStage2");
			BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialSoulAwakeningStage3");
			BlueprintFeature modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialSoulAwakeningStage4");
			BlueprintFeature modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialFlurry");
			BlueprintFeature modBlueprint8 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialDimensionalBlinkFeature");
			BlueprintFeature modBlueprint9 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialVelocityFeature");
			BlueprintFeatureSelection modBlueprint10 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "ExtraSpecialPowerSelection");
			BlueprintFeature modBlueprint11 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			BlueprintFeature modBlueprint12 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
			BlueprintFeature modBlueprint13 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
			BlueprintFeature modBlueprint14 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
			BlueprintFeatureSelection modBlueprint15 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
			BlueprintFeatureSelection modBlueprint16 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
			BlueprintFeatureSelection modBlueprint17 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveBonusSelection");
			BlueprintFeatureSelection modBlueprint18 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			BlueprintFeature modBlueprint19 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
			BlueprintFeature modBlueprint20 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SoulArchiveFeature");
			LevelEntry[] removeEntries = new LevelEntry[7]
			{
				Helpers.CreateLevelEntry(1, modBlueprint11, modBlueprint13, LegacySelection.GetClassFeature(), modBlueprint19),
				Helpers.CreateLevelEntry(3, modBlueprint12),
				Helpers.CreateLevelEntry(6, modBlueprint17),
				Helpers.CreateLevelEntry(10, modBlueprint16),
				Helpers.CreateLevelEntry(12, modBlueprint18),
				Helpers.CreateLevelEntry(15, modBlueprint14),
				Helpers.CreateLevelEntry(20, modBlueprint15)
			};
			LevelEntry[] addEntries = new LevelEntry[10]
			{
				Helpers.CreateLevelEntry(1, modBlueprint, modBlueprint2, modBlueprint3, modBlueprint20, MartialGodLegacySelection.getClassFeature()),
				Helpers.CreateLevelEntry(3, modBlueprint10),
				Helpers.CreateLevelEntry(5, modBlueprint7),
				Helpers.CreateLevelEntry(6, modBlueprint4),
				Helpers.CreateLevelEntry(7, modBlueprint8),
				Helpers.CreateLevelEntry(10, modBlueprint7),
				Helpers.CreateLevelEntry(12, modBlueprint5),
				Helpers.CreateLevelEntry(15, modBlueprint7),
				Helpers.CreateLevelEntry(18, modBlueprint6),
				Helpers.CreateLevelEntry(20, modBlueprint7, modBlueprint9)
			};
			Archetype = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodArchetype", delegate(BlueprintArchetype bp)
			{
				bp.LocalizedName = Name;
				bp.LocalizedDescription = Description;
				bp.LocalizedDescriptionShort = Description;
				bp.IsArcaneCaster = true;
				bp.IsDivineCaster = true;
				bp.RemoveFeatures = removeEntries;
				bp.AddFeatures = addEntries;
				bp.OverrideAttributeRecommendations = true;
				bp.RecommendedAttributes = new StatType[3]
				{
					StatType.Strength,
					StatType.Dexterity,
					StatType.Charisma
				};
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "MartialGod";
				});
				bp.RemoveSpellbook = Main.IsekaiContext.AddedContent.DisableSpellbookMartialGod;
			});
			IsekaiProtagonistClass.RegisterArchetype(Archetype);
		}

		public static BlueprintArchetype Get()
		{
			return Archetype ?? BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "MartialGodArchetype");
		}

		public static BlueprintArchetypeReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "MartialGodArchetype");
		}
	}
}
