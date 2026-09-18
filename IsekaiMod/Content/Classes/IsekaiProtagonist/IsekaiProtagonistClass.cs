using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	internal class IsekaiProtagonistClass
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "IsekaiProtagonistClass.Name", "Isekai Protagonist");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "IsekaiProtagonistClass.Description", "Isekai protagonists arrive in Golarion from beyond the veil of worlds, endowed with extraordinary cheat powers, transcendent talent, and reality-bending potential. Shielded by uncanny plot armor and an insatiable drive to master their new reality, they carve their own destiny through the Worldwound, commanding celestial favor and shattering cosmic expectations.");

		private static readonly LocalizedString DescriptionShort = Helpers.CreateString(Main.IsekaiContext, "IsekaiProtagonistClass.DescriptionShort", "Reincarnated travelers from another world, Isekai protagonists possess extraordinary cheat powers, uncanny plot armor, and an ever-expanding repertoire of legendary abilities.");

		private static BlueprintCharacterClass isekaiProtagonistClass;

		private static readonly Sprite Icon_AllSkilled = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("f3bc6f9c855b2fb4e9aea364b8163aca")).m_Icon;

		private static readonly Sprite Icon_ForetellAidBuff = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2")).m_Icon;

		private static readonly Sprite Icon_EdictOfImpenetrableFortress = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d7741c08ccf699e4a8a8f8ab2ed345f8")).m_Icon;

		private static readonly Sprite Icon_TrickFate = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd")).m_Icon;

		private static readonly Sprite Icon_BasicFeatSelection = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45")).m_Icon;

		private static readonly BlueprintStatProgression BABFull = BlueprintTools.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");

		private static readonly BlueprintStatProgression SavesHigh = BlueprintTools.GetBlueprint<BlueprintStatProgression>("ff4662bde9e75f145853417313842751");

		private static readonly BlueprintCharacterClass AnimalClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("4cd1757a0eea7694ba5c933729a53920");

		public static void Add()
		{
			BlueprintCharacterClass blueprintCharacterClass = ClassTools.Classes.SlayerClass ?? ClassTools.Classes.FighterClass;
			int num = StaticReferences.BaseClasses.IndexOf(blueprintCharacterClass);
			if (num < 0 && StaticReferences.BaseClasses.Length != 0)
			{
				num = 0;
			}
			int isekaiDefaultClothes = Main.IsekaiContext.AddedContent.IsekaiDefaultClothes;
			int num2 = StaticReferences.BaseClasses.Length;
			BlueprintCharacterClass clothesClass = ((isekaiDefaultClothes >= 0 && isekaiDefaultClothes < num2) ? StaticReferences.BaseClasses[isekaiDefaultClothes] : null);
			if (clothesClass == null)
			{
				clothesClass = ((num >= 0 && num < num2) ? StaticReferences.BaseClasses[num] : blueprintCharacterClass);
			}
			BlueprintFeature IsekaiProtagonistPlotArmorFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistPlotArmorFeat", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Plot Armor");
				bp.SetDescription(Main.IsekaiContext, "Isekai Protagonists gain a luck bonus to {g|Encyclopedia:Armor_Class}AC{/g} and all {g|Encyclopedia:Saving_Throw}saving throws{/g} equal to their character level.");
				((BlueprintUnitFact)bp).m_DescriptionShort = ((BlueprintUnitFact)bp).m_Description;
				((BlueprintUnitFact)bp).m_Icon = Icon_EdictOfImpenetrableFortress;
			});
			BlueprintFeature IsekaiProtagonistSpecialPowerFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistSpecialPowerFeat", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Powers");
				bp.SetDescription(Main.IsekaiContext, "Isekai Protagonists gain special powers as they increase their level. These are skills or abilities that greatly enhance the Isekai Protagonist's combat power.");
				((BlueprintUnitFact)bp).m_DescriptionShort = ((BlueprintUnitFact)bp).m_Description;
				((BlueprintUnitFact)bp).m_Icon = Icon_ForetellAidBuff;
			});
			BlueprintFeature IsekaiProtagonistOverpoweredAbilityFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistOverpoweredAbilityFeat", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Abilities");
				bp.SetDescription(Main.IsekaiContext, "Isekai Protagonists gain Overpowered Abilities as they increase their level. These are extremely powerful abiilities that surpass even gods.");
				((BlueprintUnitFact)bp).m_DescriptionShort = ((BlueprintUnitFact)bp).m_Description;
				((BlueprintUnitFact)bp).m_Icon = Icon_TrickFate;
			});
			BlueprintFeature IsekaiProtagonistBonusFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistBonusFeat", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Bonus Feat");
				bp.SetDescription(Main.IsekaiContext, "Isekai Protagonists gain twice as many {g|Encyclopedia:Feat}feats{/g} as the other classes.");
				((BlueprintUnitFact)bp).m_DescriptionShort = ((BlueprintUnitFact)bp).m_Description;
				((BlueprintUnitFact)bp).m_Icon = Icon_BasicFeatSelection;
			});
			BlueprintFeature IsekaiProtagonistLegacyFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistLegacyFeat", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Legacy");
				bp.SetDescription(Main.IsekaiContext, "Isekai Protagonists gain a limited set of features from other classes.");
				((BlueprintUnitFact)bp).m_DescriptionShort = ((BlueprintUnitFact)bp).m_Description;
				((BlueprintUnitFact)bp).m_Icon = Icon_AllSkilled;
			});
			isekaiProtagonistClass = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistClass", delegate(BlueprintCharacterClass bp)
			{
				bp.LocalizedName = Name;
				bp.LocalizedDescription = Description;
				bp.LocalizedDescriptionShort = DescriptionShort;
				bp.HitDie = DiceType.D12;
				bp.m_BaseAttackBonus = BABFull.ToReference<BlueprintStatProgressionReference>();
				bp.m_FortitudeSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				bp.m_ReflexSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				bp.m_WillSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				bp.m_Difficulty = 1;
				bp.m_Spellbook = IsekaiProtagonistSpellbook.GetReference();
				bp.RecommendedAttributes = new StatType[2]
				{
					StatType.Strength,
					StatType.Charisma
				};
				bp.NotRecommendedAttributes = new StatType[0];
				bp.m_EquipmentEntities = new KingmakerEquipmentEntityReference[0];
				bp.m_StartingItems = new BlueprintItemReference[0];
				bp.SkillPoints = 4;
				bp.ClassSkills = new StatType[11]
				{
					StatType.SkillAthletics,
					StatType.SkillMobility,
					StatType.SkillThievery,
					StatType.SkillStealth,
					StatType.SkillKnowledgeArcana,
					StatType.SkillKnowledgeWorld,
					StatType.SkillLoreNature,
					StatType.SkillLoreReligion,
					StatType.SkillPerception,
					StatType.SkillPersuasion,
					StatType.SkillUseMagicDevice
				};
				bp.IsDivineCaster = true;
				bp.IsArcaneCaster = true;
				bp.StartingGold = 69420;
				bp.PrimaryColor = 9;
				bp.SecondaryColor = 9;
				if (clothesClass != null)
				{
					bp.MaleEquipmentEntities = clothesClass.MaleEquipmentEntities;
					bp.FemaleEquipmentEntities = clothesClass.FemaleEquipmentEntities;
				}
				bp.m_SignatureAbilities = new BlueprintFeatureReference[5]
				{
					IsekaiProtagonistPlotArmorFeat.ToReference<BlueprintFeatureReference>(),
					IsekaiProtagonistSpecialPowerFeat.ToReference<BlueprintFeatureReference>(),
					IsekaiProtagonistOverpoweredAbilityFeat.ToReference<BlueprintFeatureReference>(),
					IsekaiProtagonistBonusFeat.ToReference<BlueprintFeatureReference>(),
					IsekaiProtagonistLegacyFeat.ToReference<BlueprintFeatureReference>()
				};
				bp.AddComponent(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = AnimalClass.ToReference<BlueprintCharacterClassReference>();
				});
				bp.AddComponent(delegate(PrerequisiteIsPet c)
				{
					c.Not = true;
					c.HideInUI = true;
				});
				if (Main.IsekaiContext.AddedContent.ExcludeCompanionsFromIsekaiClass)
				{
					bp.AddComponent<PrerequisiteIsMainCharacter>();
				}
				bp.m_Archetypes = new BlueprintArchetypeReference[0];
				bp.m_Progression = null;
				bp.m_DefaultBuild = null;
			});
			IsekaiProtagonistSpellbook.SetCharacterClass(isekaiProtagonistClass);
			TTCoreExtensions.RegisterClass(isekaiProtagonistClass);
		}

		public static void RegisterArchetype(BlueprintArchetype archetype)
		{
			BlueprintCharacterClass blueprintCharacterClass = Get();
			if (blueprintCharacterClass != null && archetype != null)
			{
				blueprintCharacterClass.m_Archetypes = (blueprintCharacterClass.m_Archetypes ?? new BlueprintArchetypeReference[0]).AppendToArray(archetype.ToReference<BlueprintArchetypeReference>());
			}
		}

		public static void SetProgression(BlueprintProgression progression)
		{
			BlueprintCharacterClass blueprintCharacterClass = Get();
			if (blueprintCharacterClass != null && progression != null)
			{
				blueprintCharacterClass.m_Progression = progression.ToReference<BlueprintProgressionReference>();
			}
		}

		public static void SetDefaultBuild(BlueprintUnitFact prebuildFeatureList)
		{
			BlueprintCharacterClass blueprintCharacterClass = Get();
			if (blueprintCharacterClass != null && prebuildFeatureList != null)
			{
				blueprintCharacterClass.m_DefaultBuild = prebuildFeatureList.ToReference<BlueprintUnitFactReference>();
			}
		}

		public static BlueprintCharacterClass Get()
		{
			if (isekaiProtagonistClass != null)
			{
				return isekaiProtagonistClass;
			}
			return BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "IsekaiProtagonistClass");
		}

		public static BlueprintCharacterClassReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
		}
	}
}
