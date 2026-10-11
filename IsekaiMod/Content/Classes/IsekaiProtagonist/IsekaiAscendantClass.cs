using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

internal class IsekaiAscendantClass
{
	internal const string BlueprintName = "IsekaiAscendantClass";

	private static readonly BlueprintStatProgression BABFull = StaticReferences.StatProgressions.BABFull;

	internal const int RequiredIsekaiLevel = 20;

	internal const int MaxLevel = 20;

	private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "IsekaiAscendantClass.Name", "Isekai Ascendant");

	private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "IsekaiAscendantClass.Description", "Twenty levels into a borrowed world, the protagonist stops being its guest. An Isekai Ascendant continues the same story past the mortal ceiling: the same cheat powers, the same plot armor, the same spellbook, carried into the range where the Worldwound's own legends stop counting. It is not a new career but the second half of one, and it begins only where the Isekai Protagonist ends.\n\nBecause the spellbook is the one you already have, it is charted under Isekai Protagonist rather than here: this page shows no spellbook row of its own. Your caster level, slots and spells known all continue to rise, and the spell pages show them correctly.");

	private static readonly LocalizedString DescriptionShort = Helpers.CreateString(Main.IsekaiContext, "IsekaiAscendantClass.DescriptionShort", "The second half of the Isekai Protagonist's path, from character level 21 to 40. It advances the spellbook you already have rather than granting a new one.");

	private static BlueprintCharacterClass isekaiAscendantClass;

	private static BlueprintStatProgression EpicPrestigeSaves()
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiAscendantSavesPrestigeHigh", delegate(BlueprintStatProgression bp)
		{
			int[] array = new int[21];
			for (int i = 0; i <= 20; i++)
			{
				array[i] = (i + 1) / 2;
			}
			bp.Bonuses = array;
		});
	}

	public static void Add()
	{
		BlueprintStatProgression epicSaves = EpicPrestigeSaves();
		IsekaiProtagonistClass.Get();
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
		isekaiAscendantClass = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiAscendantClass", delegate(BlueprintCharacterClass bp)
		{
			bp.LocalizedName = Name;
			bp.LocalizedDescription = Description;
			bp.LocalizedDescriptionShort = DescriptionShort;
			bp.HitDie = DiceType.D12;
			bp.PrestigeClass = false;
			bp.m_BaseAttackBonus = BABFull.ToReference<BlueprintStatProgressionReference>();
			bp.m_FortitudeSave = epicSaves.ToReference<BlueprintStatProgressionReference>();
			bp.m_ReflexSave = epicSaves.ToReference<BlueprintStatProgressionReference>();
			bp.m_WillSave = epicSaves.ToReference<BlueprintStatProgressionReference>();
			bp.m_Difficulty = 1;
			bp.m_Spellbook = null;
			bp.RecommendedAttributes = new StatType[2]
			{
				StatType.Strength,
				StatType.Charisma
			};
			bp.NotRecommendedAttributes = new StatType[0];
			bp.m_EquipmentEntities = new KingmakerEquipmentEntityReference[0];
			bp.m_StartingItems = new BlueprintItemReference[0];
			bp.StartingGold = 0;
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
			bp.PrimaryColor = 9;
			bp.SecondaryColor = 9;
			if (clothesClass != null)
			{
				bp.MaleEquipmentEntities = clothesClass.MaleEquipmentEntities;
				bp.FemaleEquipmentEntities = clothesClass.FemaleEquipmentEntities;
			}
			bp.m_SignatureAbilities = SignatureAbilities();
			bp.AddComponent(delegate(PrerequisiteClassLevel c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.Level = 20;
			});
			bp.AddComponent(delegate(PrerequisiteIsPet c)
			{
				c.Not = true;
				c.HideInUI = true;
			});
			bp.AddComponent(delegate(PrerequisiteIsMainCharacter c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.RespectsIsekaiSettings = true;
			});
			bp.m_Archetypes = new BlueprintArchetypeReference[0];
			bp.HideIfRestricted = true;
			bp.m_Progression = null;
			bp.m_DefaultBuild = null;
		});
		ContentOffering.RegisterClass(isekaiAscendantClass, OfferedContent.IsekaiProtagonist);
	}

	private static BlueprintFeatureReference[] SignatureAbilities()
	{
		return (from feature in new BlueprintFeature[4]
			{
				BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProtagonistPlotArmorFeat"),
				BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProtagonistSpecialPowerFeat"),
				BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProtagonistOverpoweredAbilityFeat"),
				BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProtagonistBonusFeat")
			}
			where feature != null
			select feature.ToReference<BlueprintFeatureReference>()).ToArray();
	}

	public static void RegisterArchetype(BlueprintArchetype archetype)
	{
		BlueprintCharacterClass blueprintCharacterClass = Get();
		if (blueprintCharacterClass == null || archetype == null)
		{
			return;
		}
		if (!archetype.GetComponents<PrerequisiteIsMainCharacter>().Any())
		{
			archetype.AddComponent(delegate(PrerequisiteIsMainCharacter c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.RespectsIsekaiSettings = true;
			});
		}
		blueprintCharacterClass.m_Archetypes = (blueprintCharacterClass.m_Archetypes ?? new BlueprintArchetypeReference[0]).AppendToArray(archetype.ToReference<BlueprintArchetypeReference>());
	}

	public static void SetProgression(BlueprintProgression progression)
	{
		BlueprintCharacterClass blueprintCharacterClass = Get();
		if (blueprintCharacterClass != null && progression != null)
		{
			blueprintCharacterClass.m_Progression = progression.ToReference<BlueprintProgressionReference>();
		}
	}

	public static BlueprintCharacterClass Get()
	{
		if (isekaiAscendantClass != null)
		{
			return isekaiAscendantClass;
		}
		return BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "IsekaiAscendantClass");
	}

	public static BlueprintCharacterClassReference GetReference()
	{
		return BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiAscendantClass");
	}

	public static BlueprintCharacterClassReference[] ClassLevelScalingClasses()
	{
		BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
		BlueprintCharacterClassReference reference2 = GetReference();
		if (reference2 != null && !(reference2.Guid == BlueprintGuid.Empty))
		{
			return new BlueprintCharacterClassReference[2] { reference, reference2 };
		}
		return new BlueprintCharacterClassReference[1] { reference };
	}
}
