using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.Deathsnatcher
{
	internal class DeathsnatcherClass
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherClass.Name", "Deathsnatcher");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherClass.Description", "This bipedal jackal has vulture wings and a rat tail ending in a scorpion's stinger. Each of its four arms ends in a clawed hand.");

		private static readonly BlueprintFeature MonstrousHumanoidType = BlueprintTools.GetBlueprint<BlueprintFeature>("57614b50e8d86b24395931fffc5e409b");

		private static readonly BlueprintStatProgression BABFull = BlueprintTools.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");

		private static readonly BlueprintStatProgression SavesHigh = BlueprintTools.GetBlueprint<BlueprintStatProgression>("ff4662bde9e75f145853417313842751");

		private static readonly BlueprintStatProgression SavesLow = BlueprintTools.GetBlueprint<BlueprintStatProgression>("dc0c7c1aba755c54f96c089cdf7d14a3");

		private static BlueprintCharacterClass deathsnatcherClass;

		public static void Add()
		{
			deathsnatcherClass = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherClass", delegate(BlueprintCharacterClass blueprintCharacterClass)
			{
				blueprintCharacterClass.LocalizedName = Name;
				blueprintCharacterClass.LocalizedDescription = Description;
				blueprintCharacterClass.LocalizedDescriptionShort = Description;
				blueprintCharacterClass.HitDie = DiceType.D10;
				blueprintCharacterClass.m_BaseAttackBonus = BABFull.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass.m_FortitudeSave = SavesLow.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass.m_ReflexSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass.m_WillSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass.m_Difficulty = 1;
				blueprintCharacterClass.HideIfRestricted = true;
				blueprintCharacterClass.m_Archetypes = new BlueprintArchetypeReference[0];
				blueprintCharacterClass.RecommendedAttributes = new StatType[0];
				blueprintCharacterClass.NotRecommendedAttributes = new StatType[0];
				blueprintCharacterClass.m_EquipmentEntities = new KingmakerEquipmentEntityReference[0];
				blueprintCharacterClass.m_StartingItems = new BlueprintItemReference[0];
				blueprintCharacterClass.SkillPoints = 0;
				blueprintCharacterClass.ClassSkills = new StatType[4]
				{
					StatType.SkillAthletics,
					StatType.SkillMobility,
					StatType.SkillLoreNature,
					StatType.SkillPersuasion
				};
				blueprintCharacterClass.IsDivineCaster = false;
				blueprintCharacterClass.IsArcaneCaster = false;
				blueprintCharacterClass.StartingGold = 0;
				blueprintCharacterClass.PrimaryColor = 0;
				blueprintCharacterClass.SecondaryColor = 0;
				blueprintCharacterClass.MaleEquipmentEntities = new EquipmentEntityLink[0];
				blueprintCharacterClass.FemaleEquipmentEntities = new EquipmentEntityLink[0];
				blueprintCharacterClass.m_SignatureAbilities = new BlueprintFeatureReference[0];
				blueprintCharacterClass.AddComponent(delegate(PrerequisiteIsPet c)
				{
					c.Group = Prerequisite.GroupType.Any;
				});
				blueprintCharacterClass.AddComponent(delegate(PrerequisiteFeature c)
				{
					c.HideInUI = true;
					c.m_Feature = MonstrousHumanoidType.ToReference<BlueprintFeatureReference>();
				});
				blueprintCharacterClass.m_Archetypes = new BlueprintArchetypeReference[0];
				blueprintCharacterClass.m_Progression = null;
				blueprintCharacterClass.m_DefaultBuild = null;
			});
			BlueprintTools.GetBlueprint<BlueprintCharacterClass>("26b10d4340839004f960f9816f6109fe").AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.HideInUI = true;
				c.m_Feature = MonstrousHumanoidType.ToReference<BlueprintFeatureReference>();
			});
			BlueprintTools.GetBlueprint<BlueprintRoot>("2d77316c72b9ed44f888ceefc2a131f6");
			BlueprintRoot.Instance.Progression.m_PetClasses = BlueprintRoot.Instance.Progression.m_PetClasses.AppendToArray(deathsnatcherClass.ToReference<BlueprintCharacterClassReference>());
		}

		public static void SetProgression(BlueprintProgression progression)
		{
			Get().m_Progression = progression.ToReference<BlueprintProgressionReference>();
		}

		public static BlueprintCharacterClass Get()
		{
			return deathsnatcherClass ?? BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "DeathsnatcherClass");
		}

		public static BlueprintCharacterClassReference GetReference()
		{
			return Get().ToReference<BlueprintCharacterClassReference>();
		}
	}
}
