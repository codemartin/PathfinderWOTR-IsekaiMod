using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
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
	internal class DevourerArchetype
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "DevourerArchetype.Name", "Slime");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "DevourerArchetype.Description", "Reincarnated not as a standard mortal or dragon lord, but as an amorphous, fluid slime entity, the Slime masters consumption and mimicry. Able to assume a humanoid form or shift into its true resilient slime shape at will, it devours defeated monsters and demons, assimilating their biological traits, stealing elemental immunities, and swallowing incoming hostile magic into an infinite spatial stomach.");

		public static void Add()
		{
			BlueprintFeature DevourerProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies");
			BlueprintFeature PredatorInstincts = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PredatorInstincts");
			BlueprintFeature PredatorMawFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PredatorMawFeature");
			BlueprintFeature EssenceAssimilationFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "EssenceAssimilationFeature");
			BlueprintFeature InfiniteStomachFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "InfiniteStomachFeature");
			BlueprintFeature DevourMagicFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourMagicFeature");
			BlueprintFeature BeelzebubLordOfDevourersFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "BeelzebubLordOfDevourersFeature");
			BlueprintFeature SlimeFormFeature = SlimeForm.Get();
			BlueprintFeature IsekaiProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "StartingWeaponSelection");
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiPetSelection");
			BlueprintFeature ReleaseEnergy = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ReleaseEnergy");
			BlueprintFeature Gifted = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Gifted");
			BlueprintFeature SecondReincarnation = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SecondReincarnation");
			BlueprintFeatureSelection HaxSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
			BlueprintFeatureSelection SecretPowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
			BlueprintFeatureSelection SignatureMoveBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SignatureMoveBonusSelection");
			BlueprintFeatureSelection TrainingEpisodeBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TrainingEpisodeBonusSelection");
			BlueprintFeature ChronicleOtherworldFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChronicleOtherworldFeature");
			BlueprintFeature GluttonyCompendiumFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GluttonyCompendiumFeature");
			IsekaiProtagonistClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "DevourerArchetype", delegate(BlueprintArchetype bp)
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
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PredatorArtStickyThreadFeature");
				BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PredatorArtBodyArmorFeature");
				BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PredatorArtBlackFlameFeature");
				BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PredatorArtThoughtAccelerationFeature");
				BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PredatorArtDragonBreathFeature");
				bp.AddFeatures = new LevelEntry[7]
				{
					Helpers.CreateLevelEntry(1, DevourerProficiencies, PredatorInstincts, PredatorMawFeature, SlimeFormFeature, GluttonyCompendiumFeature, DevourerLegacySelection.getClassFeature()),
					Helpers.CreateLevelEntry(3, EssenceAssimilationFeature, modBlueprint),
					Helpers.CreateLevelEntry(5, modBlueprint2),
					Helpers.CreateLevelEntry(7, InfiniteStomachFeature, modBlueprint3),
					Helpers.CreateLevelEntry(9, modBlueprint4),
					Helpers.CreateLevelEntry(11, DevourMagicFeature, modBlueprint5),
					Helpers.CreateLevelEntry(20, BeelzebubLordOfDevourersFeature)
				};
				BlueprintFeature VampireHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiVampireHeritage");
				BlueprintFeature DragonHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiDragonLordHeritage");
				BlueprintFeature WerewolfHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiWerewolfHeritage");
				if (VampireHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = VampireHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
				if (DragonHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = DragonHeritage.ToReference<BlueprintFeatureReference>();
					});
				}
				if (WerewolfHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = WerewolfHeritage.ToReference<BlueprintFeatureReference>();
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
				bp.AddComponent(delegate(PrerequisiteCycleOriginLock c)
				{
					c.AllowedOrigin = "Devourer";
				});
				bp.OverrideAttributeRecommendations = true;
				bp.RecommendedAttributes = new StatType[2]
				{
					StatType.Constitution,
					StatType.Strength
				};
			}));
		}

		public static BlueprintArchetype Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "DevourerArchetype");
		}

		public static BlueprintArchetypeReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "DevourerArchetype");
		}
	}
}
