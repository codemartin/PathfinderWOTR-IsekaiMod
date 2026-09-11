using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige
{
	internal class TranscendentSovereignClass
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "TranscendentSovereignClass.Name", "Transcendent Sovereign");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "TranscendentSovereignClass.Description", "Having achieved mastery beyond ordinary dimensional boundaries, the Transcendent Sovereign is an otherworldly entity whose authority bends space, time, and destiny itself. With absolute protagonist certainty, they command temporal halts, manifest supreme domain auras, and rewrite reality at their whim.");

		private static readonly LocalizedString DescriptionShort = Helpers.CreateString(Main.IsekaiContext, "TranscendentSovereignClass.DescriptionShort", "A supreme prestige class for otherworldly protagonists capable of halting time, manifesting domain auras, and rewriting reality.");

		private static BlueprintCharacterClass transcendentSovereignClass;

		private static readonly BlueprintStatProgression BABFull = BlueprintTools.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");

		private static readonly BlueprintStatProgression SavesHigh = BlueprintTools.GetBlueprint<BlueprintStatProgression>("ff4662bde9e75f145853417313842751");

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
			transcendentSovereignClass = Helpers.CreateBlueprint(Main.IsekaiContext, "TranscendentSovereignClass", delegate(BlueprintCharacterClass bp)
			{
				bp.LocalizedName = Name;
				bp.LocalizedDescription = Description;
				bp.LocalizedDescriptionShort = DescriptionShort;
				bp.HitDie = DiceType.D12;
				bp.PrestigeClass = true;
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
				bp.m_Archetypes = new BlueprintArchetypeReference[0];
				bp.AddComponent(delegate(PrerequisiteStatValue c)
				{
					c.Stat = StatType.BaseAttackBonus;
					c.Value = 7;
				});
				bp.AddComponent(delegate(PrerequisiteClassLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.Level = 10;
				});
			});
			TTCoreExtensions.RegisterClass(transcendentSovereignClass);
		}

		public static void RegisterArchetype(BlueprintArchetype archetype)
		{
			BlueprintCharacterClass blueprintCharacterClass = Get();
			blueprintCharacterClass.m_Archetypes = blueprintCharacterClass.m_Archetypes.AppendToArray(archetype.ToReference<BlueprintArchetypeReference>());
		}

		public static void SetProgression(BlueprintProgression progression)
		{
			Get().m_Progression = progression.ToReference<BlueprintProgressionReference>();
		}

		public static BlueprintCharacterClass Get()
		{
			if (transcendentSovereignClass != null)
			{
				return transcendentSovereignClass;
			}
			return BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "TranscendentSovereignClass");
		}

		public static BlueprintCharacterClassReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "TranscendentSovereignClass");
		}
	}
}
