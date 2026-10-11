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

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;

internal class TranscendentSovereignClass
{
	private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "TranscendentSovereignClass.Name", "Transcendent Sovereign");

	internal static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "TranscendentSovereignClass.Description", "The protagonist who has stopped reacting to the story and started ruling it. A Transcendent Sovereign's word becomes law a little more with every level: once a day time stops for everyone else, allies fight inside a domain of the sovereign's authority, and at the summit a single decree rewrites the battle, raising the fallen and unmaking the enemy in the same breath. Seven oaths, one for each Isekai archetype, bend the domain and the crown to that archetype's own story.\n\nRequires 10 levels of Isekai Protagonist and 10 ranks in Persuasion. Every level advances the Isekai spellbook you choose at 1st level. Transcendent Authority (1st): +2 to the DCs of your spells and abilities and +2 caster level. Sovereign Decree (2nd, 4th, 6th and 8th): +1 to attack rolls and Charisma per rank, up to +4. Temporal Inversion - Sovereign Domain (3rd): once a day, freeze every enemy within 60 feet for 2 rounds while you gain an extra attack, True Seeing, speed and +4 dodge AC. Transcendent Domain (5th): a 40-foot aura that gives allies +4 morale to attack rolls and saving throws and immunity to fear and mind-affecting effects. Absolute Counter (7th): +10 spell resistance and +4 dodge AC. Crown of Stories (9th): +2 to your spell save DCs and +2 to all saving throws. Reality Rewrite (10th): once a day, raise and fully restore every fallen ally within 60 feet and deal 200 damage that nothing reduces to every enemy in range. A Transcendent Sovereign archetype replaces the 5th- and 9th-level features with its own. You also gain an Isekai bonus feat at 2nd, 4th, 6th and 8th level and a Special Power at 9th.");

	private static readonly LocalizedString DescriptionShort = Helpers.CreateString(Main.IsekaiContext, "TranscendentSovereignClass.DescriptionShort", "Isekai prestige class for ruling the story: spell DC and caster level bonuses, a daily time stop, a domain aura and Reality Rewrite, with seven archetypes tuned to your Isekai archetype.");

	private static BlueprintCharacterClass transcendentSovereignClass;

	private static readonly BlueprintStatProgression BABFull = StaticReferences.StatProgressions.BABFull;

	private static readonly BlueprintStatProgression SavesPrestigeHigh = StaticReferences.StatProgressions.SavesPrestigeHigh;

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
			bp.m_FortitudeSave = SavesPrestigeHigh.ToReference<BlueprintStatProgressionReference>();
			bp.m_ReflexSave = SavesPrestigeHigh.ToReference<BlueprintStatProgressionReference>();
			bp.m_WillSave = SavesPrestigeHigh.ToReference<BlueprintStatProgressionReference>();
			bp.m_Difficulty = 1;
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
			bp.IsDivineCaster = false;
			bp.IsArcaneCaster = false;
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
				c.Stat = StatType.SkillPersuasion;
				c.Value = 10;
			});
			bp.AddComponent(delegate(PrerequisiteClassLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.Level = 10;
			});
		});
		ContentOffering.RegisterClass(transcendentSovereignClass, OfferedContent.IsekaiProtagonist);
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
