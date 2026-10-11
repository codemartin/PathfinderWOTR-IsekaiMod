using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Guardians;

internal class GuardianCompanionClass
{
	private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "GuardianCompanionClass.Name", "Guardian");

	private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "GuardianCompanionClass.Description", "An otherworldly guardian bound to the Isekai Protagonist, ascending in planar authority as its master gains levels. The Guardian is a full base attack bonus class with good saves on every side. It grows stronger, quicker and harder to hit at 1st and 3rd level and every three levels after, gains Evasion at 2nd, Multiattack at 9th and Improved Evasion at 15th, picks a bonus combat feat at 1st level and every four levels, and shares its master's mythic power through its Mythic Bond. Each guardian brings its own milestones, and its archetype names it and trains it in the weapons it fights with.");

	private static readonly BlueprintStatProgression BABFull = StaticReferences.StatProgressions.BABFull;

	private static readonly BlueprintStatProgression SavesHigh = StaticReferences.StatProgressions.SavesHigh;

	private static readonly BlueprintFeature Evasion = BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");

	private static readonly BlueprintFeature ImprovedEvasion = BlueprintTools.GetBlueprint<BlueprintFeature>("ce96af454a6137d47b9c6a1e02e66803");

	private static readonly BlueprintFeature Multiattack = BlueprintTools.GetBlueprint<BlueprintFeature>("8ac319e47057e2741b42229210eb43ed");

	public static BlueprintFeature CreateNaturalArmor(string blueprintName, string displayName, int bonus)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, blueprintName, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, $"The guardian's hide, plate or planar carapace grants a +{bonus} natural armor bonus to AC.");
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			bp.HideInCharacterSheetAndLevelUp = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = bonus;
			});
		});
	}

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
		BlueprintFeature GuardianStatFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianStatFeature", delegate(BlueprintFeature blueprintFeature)
		{
			blueprintFeature.SetName(Main.IsekaiContext, "Otherworldly Growth");
			blueprintFeature.SetDescription(Main.IsekaiContext, "The guardian grows into its bond. Each time this is gained it adds +1 to Strength, +1 to Dexterity and +1 to its natural armor bonus to AC.");
			blueprintFeature.IsClassFeature = true;
			blueprintFeature.Ranks = 10;
			blueprintFeature.HideInCharacterSheetAndLevelUp = true;
			blueprintFeature.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
				c.m_Feature = blueprintFeature.ToReference<BlueprintFeatureReference>();
				c.m_Progression = ContextRankProgression.AsIs;
			});
			blueprintFeature.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Strength;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			blueprintFeature.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Dexterity;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			blueprintFeature.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
		});
		BlueprintFeatureSelection fighterFeats = FeatTools.Selections.FighterFeatSelection;
		BlueprintFeatureSelection GuardianBonusFeatSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianBonusFeatSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
		{
			blueprintFeatureSelection.SetName(Main.IsekaiContext, "Guardian Combat Feat");
			blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "At 1st level and every four levels thereafter (4th, 8th, 12th, 16th and 20th), the guardian gains a bonus combat feat, chosen from the same list as a Fighter's bonus feats. It must meet the feat's prerequisites.");
			((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = ((BlueprintUnitFact)fighterFeats)?.m_Icon;
			blueprintFeatureSelection.IsClassFeature = true;
			blueprintFeatureSelection.Group = FeatureGroup.CombatFeat;
			blueprintFeatureSelection.m_AllFeatures = (fighterFeats?.m_AllFeatures ?? new BlueprintFeatureReference[0]).Where((BlueprintFeatureReference f) => f != null && !f.IsEmpty()).ToArray();
		});
		BlueprintFeature GuardianMythicBond = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianMythicBondFeature", delegate(BlueprintFeature blueprintFeature)
		{
			blueprintFeature.SetName(Main.IsekaiContext, "Mythic Bond");
			blueprintFeature.SetDescription(Main.IsekaiContext, "The guardian draws on its master's mythic power: it gains a mythic bonus to AC, attack rolls and all saving throws equal to half its master's mythic rank, up to +5 at mythic rank 10. The bonus follows the master's rank as the master levels up.");
			blueprintFeature.IsClassFeature = true;
			blueprintFeature.AddComponent(delegate(GuardianMythicBondBonus c)
			{
				c.MaxBonus = 5;
			});
		});
		BlueprintProgression bp = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianBaseProgression", delegate(BlueprintProgression blueprintProgression)
		{
			blueprintProgression.SetName(Main.IsekaiContext, "Guardian Base Progression");
			blueprintProgression.SetDescription(Main.IsekaiContext, "Base companion progression for otherworldly guardians.");
			blueprintProgression.IsClassFeature = true;
			blueprintProgression.HideInUI = true;
			blueprintProgression.HideInCharacterSheetAndLevelUp = true;
			blueprintProgression.m_Classes = new BlueprintProgression.ClassWithLevel[1]
			{
				new BlueprintProgression.ClassWithLevel
				{
					m_Class = GuardianClass.ToReference<BlueprintCharacterClassReference>(),
					AdditionalLevel = 0
				}
			};
			blueprintProgression.LevelEntries = new LevelEntry[8]
			{
				Helpers.CreateLevelEntry(1, GuardianStatFeature, GuardianMythicBond),
				Helpers.CreateLevelEntry(2, Evasion),
				Helpers.CreateLevelEntry(3, GuardianStatFeature),
				Helpers.CreateLevelEntry(6, GuardianStatFeature),
				Helpers.CreateLevelEntry(9, GuardianStatFeature, Multiattack),
				Helpers.CreateLevelEntry(12, GuardianStatFeature),
				Helpers.CreateLevelEntry(15, GuardianStatFeature, ImprovedEvasion),
				Helpers.CreateLevelEntry(18, GuardianStatFeature)
			};
			List<LevelEntry> list = blueprintProgression.LevelEntries.ToList();
			int[] bonusFeatLevels = GuardianRoles.BonusFeatLevels;
			foreach (int level in bonusFeatLevels)
			{
				LevelEntry levelEntry = list.FirstOrDefault((LevelEntry e) => e.Level == level);
				if (levelEntry == null)
				{
					list.Add(new LevelEntry
					{
						Level = level,
						m_Features = new List<BlueprintFeatureBaseReference> { GuardianBonusFeatSelection.ToReference<BlueprintFeatureBaseReference>() }
					});
				}
				else
				{
					levelEntry.m_Features.Add(GuardianBonusFeatSelection.ToReference<BlueprintFeatureBaseReference>());
				}
			}
			blueprintProgression.LevelEntries = list.OrderBy((LevelEntry e) => e.Level).ToArray();
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
