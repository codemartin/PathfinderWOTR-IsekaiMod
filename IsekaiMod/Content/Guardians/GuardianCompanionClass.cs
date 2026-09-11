using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Guardians
{
	internal class GuardianCompanionClass
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "GuardianCompanionClass.Name", "Guardian");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "GuardianCompanionClass.Description", "An otherworldly guardian bound to the Isekai Protagonist, ascending in planar authority as their master gains levels.");

		private static readonly BlueprintStatProgression BABFull = BlueprintTools.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");

		private static readonly BlueprintStatProgression SavesHigh = BlueprintTools.GetBlueprint<BlueprintStatProgression>("ff4662bde9e75f145853417313842751");

		private static readonly BlueprintFeature Evasion = BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");

		private static readonly BlueprintFeature ImprovedEvasion = BlueprintTools.GetBlueprint<BlueprintFeature>("ce96af454a6137d47b9c6a1e02e66803");

		public static void Add()
		{
			BlueprintCharacterClass GuardianClass = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianCompanionClass", delegate(BlueprintCharacterClass blueprintCharacterClass)
			{
				blueprintCharacterClass.LocalizedName = Name;
				blueprintCharacterClass.LocalizedDescription = Description;
				blueprintCharacterClass.LocalizedDescriptionShort = Description;
				blueprintCharacterClass.HitDie = DiceType.D10;
				blueprintCharacterClass.m_BaseAttackBonus = BABFull.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass.m_FortitudeSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass.m_ReflexSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass.m_WillSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass.m_Difficulty = 1;
				blueprintCharacterClass.HideIfRestricted = true;
				blueprintCharacterClass.m_Archetypes = new BlueprintArchetypeReference[0];
				blueprintCharacterClass.RecommendedAttributes = new StatType[0];
				blueprintCharacterClass.NotRecommendedAttributes = new StatType[0];
				blueprintCharacterClass.m_EquipmentEntities = new KingmakerEquipmentEntityReference[0];
				blueprintCharacterClass.m_StartingItems = new BlueprintItemReference[0];
				blueprintCharacterClass.SkillPoints = 1;
				blueprintCharacterClass.ClassSkills = new StatType[5]
				{
					StatType.SkillAthletics,
					StatType.SkillMobility,
					StatType.SkillPerception,
					StatType.SkillPersuasion,
					StatType.SkillLoreReligion
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
				blueprintCharacterClass.m_Archetypes = new BlueprintArchetypeReference[0];
				blueprintCharacterClass.m_Progression = null;
				blueprintCharacterClass.m_DefaultBuild = null;
			});
			BlueprintProgression bp = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianBaseProgression", delegate(BlueprintProgression blueprintProgression)
			{
				blueprintProgression.SetName(Main.IsekaiContext, "Guardian Base Progression");
				blueprintProgression.SetDescription(Main.IsekaiContext, "Base companion progression for otherworldly guardians.");
				blueprintProgression.IsClassFeature = true;
				blueprintProgression.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = GuardianClass.ToReference<BlueprintCharacterClassReference>(),
						AdditionalLevel = 0
					}
				};
				blueprintProgression.LevelEntries = new LevelEntry[2]
				{
					Helpers.CreateLevelEntry(2, Evasion),
					Helpers.CreateLevelEntry(15, ImprovedEvasion)
				};
			});
			GuardianClass.m_Progression = bp.ToReference<BlueprintProgressionReference>();
			if (BlueprintRoot.Instance?.Progression?.m_PetClasses != null)
			{
				BlueprintRoot.Instance.Progression.m_PetClasses = BlueprintRoot.Instance.Progression.m_PetClasses.AppendToArray(GuardianClass.ToReference<BlueprintCharacterClassReference>());
			}
		}

		public static BlueprintCharacterClass Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "GuardianCompanionClass");
		}

		public static BlueprintCharacterClassReference GetReference()
		{
			return Get().ToReference<BlueprintCharacterClassReference>();
		}
	}
}
