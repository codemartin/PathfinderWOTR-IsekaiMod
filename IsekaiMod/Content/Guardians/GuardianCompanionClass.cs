using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians
{
	internal class GuardianCompanionClass
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "GuardianCompanionClass.Name", "Guardian");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "GuardianCompanionClass.Description", "An otherworldly guardian bound to the Isekai Protagonist, ascending in planar authority as their master gains levels.");

		private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

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
			BlueprintFeature GuardianPlanarToughness = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianPlanarToughness", delegate(BlueprintFeature blueprintFeature)
			{
				blueprintFeature.SetName(Main.IsekaiContext, "Planar Toughness");
				blueprintFeature.SetDescription(Main.IsekaiContext, "The guardian gains 80 maximum hit points, damage reduction 5/-, and a +2 bonus on Fortitude saves.");
				((BlueprintUnitFact)blueprintFeature).m_Icon = Icon_Pet;
				blueprintFeature.IsClassFeature = true;
				blueprintFeature.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 80;
				});
				blueprintFeature.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
				});
				blueprintFeature.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
			});
			BlueprintFeature GuardianStalwart = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianStalwart", delegate(BlueprintFeature blueprintFeature)
			{
				blueprintFeature.SetName(Main.IsekaiContext, "Guardian Stalwart");
				blueprintFeature.SetDescription(Main.IsekaiContext, "The guardian stands resolute against adverse magic, gaining a +4 bonus on Fortitude and Will saving throws, and immunity to shaken, frightened, and cowering.");
				((BlueprintUnitFact)blueprintFeature).m_Icon = Icon_Pet;
				blueprintFeature.IsClassFeature = true;
				blueprintFeature.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				blueprintFeature.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				blueprintFeature.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Shaken;
				});
				blueprintFeature.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Frightened;
				});
			});
			BlueprintFeature GuardianEpicResilience = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianEpicResilience", delegate(BlueprintFeature blueprintFeature)
			{
				blueprintFeature.SetName(Main.IsekaiContext, "Epic Guardian Resilience");
				blueprintFeature.SetDescription(Main.IsekaiContext, "The guardian achieves planar immunity to death effects, energy drain, petrification, and curses.");
				((BlueprintUnitFact)blueprintFeature).m_Icon = Icon_Pet;
				blueprintFeature.IsClassFeature = true;
				blueprintFeature.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Death | SpellDescriptor.Petrified;
				});
				blueprintFeature.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Death | SpellDescriptor.Petrified;
				});
			});
			BlueprintFeature GuardianSupremePlanarBound = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianSupremePlanarBound", delegate(BlueprintFeature blueprintFeature)
			{
				blueprintFeature.SetName(Main.IsekaiContext, "Supreme Planar Bond");
				blueprintFeature.SetDescription(Main.IsekaiContext, "The guardian channels the master's transcendent cheat power, gaining a +4 bonus to AC and all saving throws, and Fast Healing 10.");
				((BlueprintUnitFact)blueprintFeature).m_Icon = Icon_Pet;
				blueprintFeature.IsClassFeature = true;
				blueprintFeature.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				blueprintFeature.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				blueprintFeature.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				blueprintFeature.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				blueprintFeature.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
				});
			});
			BlueprintFeature GuardianTranscendentalAegis = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianTranscendentalAegis", delegate(BlueprintFeature blueprintFeature)
			{
				blueprintFeature.SetName(Main.IsekaiContext, "Transcendental Guardian Aegis");
				blueprintFeature.SetDescription(Main.IsekaiContext, "At 40th level, the guardian achieves complete transcendental sovereignty. Gains permanent Freedom of Movement, True Seeing, and a 50% displacement miss chance against all incoming attacks.");
				((BlueprintUnitFact)blueprintFeature).m_Icon = Icon_Pet;
				blueprintFeature.IsClassFeature = true;
				blueprintFeature.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Paralyzed;
				});
				blueprintFeature.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Paralysis | SpellDescriptor.MovementImpairing;
				});
				blueprintFeature.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Total;
					c.Descriptor = ConcealmentDescriptor.Displacement;
				});
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
				blueprintProgression.LevelEntries = new LevelEntry[7]
				{
					Helpers.CreateLevelEntry(2, Evasion),
					Helpers.CreateLevelEntry(15, ImprovedEvasion),
					Helpers.CreateLevelEntry(22, GuardianPlanarToughness),
					Helpers.CreateLevelEntry(26, GuardianStalwart),
					Helpers.CreateLevelEntry(30, GuardianEpicResilience),
					Helpers.CreateLevelEntry(35, GuardianSupremePlanarBound),
					Helpers.CreateLevelEntry(40, GuardianTranscendentalAegis)
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
