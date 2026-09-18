using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class SlimeReincarnateHeritage
	{
		public static void Add()
		{
			Sprite Icon_Slime = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("4e83f1e0e52b4613982b14ee2796928f"))?.m_Icon;
			HumanHeritageSelection.Register(Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeReincarnateHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Slime Reincarnate Heritage");
				bp.SetDescription(Main.IsekaiContext, "Reincarnated into Golarion not as a humanoid mortal, but as a fluid, sentient Slime, your amorphous biology provides extraordinary resilience and adaptive capabilities.\nGrants a +2 racial bonus to Constitution and Dexterity, a +2 racial bonus on Knowledge (World), Perception, and Initiative checks, a +2 racial bonus on saving throws against poison, acid, and paralysis effects, and a +1 racial bonus on Combat Maneuver Checks made with natural attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Slime;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.SkillPerception;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
				bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
				{
					c.SpellDescriptor = SpellDescriptor.Acid | SpellDescriptor.Poison | SpellDescriptor.Paralysis;
					c.ModifierDescriptor = ModifierDescriptor.Racial;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.AdditionalCMB;
					c.Value = 1;
				});
				BlueprintFeature slimeForm = SlimeForm.Get();
				if (slimeForm != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { slimeForm.ToReference<BlueprintUnitFactReference>() };
					});
				}
				BlueprintFeature overlordHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeteromorphicOverlordHeritage");
				if (overlordHeritage != null)
				{
					bp.AddComponent(delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = overlordHeritage.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
				string[] array = new string[6] { "OverlordArchetype", "GodEmperorArchetype", "HeroArchetype", "MartialGodArchetype", "MastermindArchetype", "ShadowMonarchArchetype" };
				foreach (string name in array)
				{
					BlueprintArchetype arch = BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, name);
					if (arch != null)
					{
						bp.AddComponent(delegate(PrerequisiteNoArchetype c)
						{
							c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
							c.m_Archetype = arch.ToReference<BlueprintArchetypeReference>();
							c.Group = Prerequisite.GroupType.All;
						});
					}
				}
				bp.HideNotAvailibleInUI = true;
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.ReapplyOnLevelUp = true;
			}));
		}

		public static void Patch()
		{
			BlueprintFeature blueprintFeature = Get();
			if (blueprintFeature == null)
			{
				return;
			}
			blueprintFeature.HideNotAvailibleInUI = true;
			BlueprintFeature overlordHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeteromorphicOverlordHeritage");
			if (overlordHeritage != null && !blueprintFeature.GetComponents<PrerequisiteNoFeature>().Any((PrerequisiteNoFeature p) => p.Feature == overlordHeritage))
			{
				blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = overlordHeritage.ToReference<BlueprintFeatureReference>();
					c.Group = Prerequisite.GroupType.All;
				});
			}
			string[] array = new string[6] { "OverlordArchetype", "GodEmperorArchetype", "HeroArchetype", "MartialGodArchetype", "MastermindArchetype", "ShadowMonarchArchetype" };
			foreach (string name in array)
			{
				BlueprintArchetype arch = BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, name);
				if (arch != null && !blueprintFeature.GetComponents<PrerequisiteNoArchetype>().Any((PrerequisiteNoArchetype p) => p.Archetype == arch))
				{
					blueprintFeature.AddComponent(delegate(PrerequisiteNoArchetype c)
					{
						c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
						c.m_Archetype = arch.ToReference<BlueprintArchetypeReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
			}
		}

		public static BlueprintFeature Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeReincarnateHeritage");
		}

		public static BlueprintFeatureReference GetReference()
		{
			return Get().ToReference<BlueprintFeatureReference>();
		}
	}
}
