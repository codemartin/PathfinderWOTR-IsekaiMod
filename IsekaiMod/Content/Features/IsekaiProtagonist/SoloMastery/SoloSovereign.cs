using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SoloMastery;

public static class SoloSovereign
{
	private static bool Added = false;

	private static readonly BlueprintFeatureSelection BasicFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");

	private static readonly Sprite Icon_Solo = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

	public static BlueprintFeatureSelection SoloSovereignFeatSelection { get; private set; }

	public static BlueprintProgression SoloSovereignBonusFeatProgression { get; private set; }

	public static BlueprintFeature SoloAwakeningSlime { get; private set; }

	public static BlueprintFeature SoloAwakeningShadowMonarch { get; private set; }

	public static BlueprintFeature SoloAwakeningOverlord { get; private set; }

	public static BlueprintFeature SoloAwakeningGodEmperor { get; private set; }

	public static BlueprintFeature SoloAwakeningHero { get; private set; }

	public static BlueprintFeature SoloAwakeningMastermind { get; private set; }

	public static BlueprintFeature SoloAwakeningMartialGod { get; private set; }

	public static void Add()
	{
		if (Added)
		{
			return;
		}
		Added = true;
		SoloSovereignFeatSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloSovereignFeatSelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Sovereign Bonus Feat");
			bp.SetDescription(Main.IsekaiContext, "Through relentless solitary training and self-reliance, you gain a bonus combat or adventuring feat.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.Ranks = 1;
			bp.m_AllFeatures = BasicFeatSelection?.m_AllFeatures ?? new BlueprintFeatureReference[0];
			bp.m_Features = (BlueprintFeatureReference[])bp.m_AllFeatures.Clone();
			BonusFeatListRefresh.Snapshot("SoloSovereignFeatSelection");
		});
		SoloSovereignBonusFeatProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloSovereignBonusFeatProgression", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Sovereign Bonus Feat");
			bp.SetDescription(Main.IsekaiContext, "Through relentless solitary training and self-reliance, you gain a bonus combat or adventuring feat.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			bp.GiveFeaturesForPreviousLevels = true;
			bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
			{
				new BlueprintProgression.ClassWithLevel
				{
					m_Class = IsekaiProtagonistClass.GetReference(),
					AdditionalLevel = 0
				}
			};
			bp.LevelEntries = new LevelEntry[1] { Helpers.CreateLevelEntry(1, SoloSovereignFeatSelection) };
		});
		SoloAwakeningSlime = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningSlime", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Awakening: Primeval Density");
			bp.SetDescription(Main.IsekaiContext, "Walking alone as an apex amorphous entity, your mass becomes extraordinarily dense. You gain a +2 inherent bonus to Constitution and a +2 natural armor bonus to AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Constitution;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "DevourerArchetype");
				c.Level = 1;
			});
		});
		SoloAwakeningShadowMonarch = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningShadowMonarch", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Awakening: Lone Monarch Dominance");
			bp.SetDescription(Main.IsekaiContext, "Refusing a permanent marshal at your side, you embody the entire shadow army within yourself. You gain a +2 inherent bonus to Strength and a +2 bonus to CMB.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Strength;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 2;
			});
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "ShadowMonarchArchetype");
				c.Level = 1;
			});
		});
		SoloAwakeningOverlord = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningOverlord", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Awakening: Solitary Supreme Ruler");
			bp.SetDescription(Main.IsekaiContext, "Governing without floor overseers, the Supreme Being commands absolute reverence. You gain a +2 inherent bonus to Charisma and Damage Reduction 3/-.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 3;
			});
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "OverlordArchetype");
				c.Level = 1;
			});
		});
		SoloAwakeningGodEmperor = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningGodEmperor", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Awakening: Solitary Apotheosis");
			bp.SetDescription(Main.IsekaiContext, "Ascending to godhood through purely personal divinity, you gain a +2 inherent bonus to Charisma and a +2 sacred bonus to AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "GodEmperorArchetype");
				c.Level = 1;
			});
		});
		SoloAwakeningHero = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningHero", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Awakening: Solitary Champion");
			bp.SetDescription(Main.IsekaiContext, "Carrying the weight of the timeline on your own shoulders, your reflexes sharpen beyond mortal limits. You gain a +2 inherent bonus to Dexterity and a +4 insight bonus on Initiative rolls.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Dexterity;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.Initiative;
				c.Value = 4;
			});
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "HeroArchetype");
				c.Level = 1;
			});
		});
		SoloAwakeningMastermind = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningMastermind", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Awakening: Singular Grandmaster");
			bp.SetDescription(Main.IsekaiContext, "Relying on no co-conspirator, your intellect operates at flawless calculation speed. You gain a +2 inherent bonus to Intelligence and a +1 bonus to the Difficulty Class of all your spells and abilities.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Intelligence;
				c.Value = 2;
			});
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 1;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "MastermindArchetype");
				c.Level = 1;
			});
		});
		SoloAwakeningMartialGod = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningMartialGod", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Solo Awakening: Unbound Martial Will");
			bp.SetDescription(Main.IsekaiContext, "Channeling all spiritual resonance directly into your own flesh and weapons, you gain a +2 inherent bonus to Strength and 2 additional attacks of opportunity per round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Strength;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AttackOfOpportunityCount;
				c.Value = 2;
			});
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "MartialGodArchetype");
				c.Level = 1;
			});
		});
	}

	public static void AddArchetypeAwakenings(BlueprintFeature bp)
	{
		foreach (KeyValuePair<string, BlueprintFeature> item in ArchetypeAwakenings())
		{
			if (item.Value != null)
			{
				string marker = item.Key;
				BlueprintFeature feature = item.Value;
				bp.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, marker);
					c.m_Feature = feature.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			}
		}
	}

	private static IEnumerable<KeyValuePair<string, BlueprintFeature>> ArchetypeAwakenings()
	{
		yield return new KeyValuePair<string, BlueprintFeature>("DevourerProficiencies", SoloAwakeningSlime);
		yield return new KeyValuePair<string, BlueprintFeature>("ShadowMonarchProficiencies", SoloAwakeningShadowMonarch);
		yield return new KeyValuePair<string, BlueprintFeature>("OverlordProficiencies", SoloAwakeningOverlord);
		yield return new KeyValuePair<string, BlueprintFeature>("GodEmperorProficiencies", SoloAwakeningGodEmperor);
		yield return new KeyValuePair<string, BlueprintFeature>("HeroProficiencies", SoloAwakeningHero);
		yield return new KeyValuePair<string, BlueprintFeature>("MastermindProficiencies", SoloAwakeningMastermind);
		yield return new KeyValuePair<string, BlueprintFeature>("PhantomBladeProficiencies", SoloAwakeningMartialGod);
	}
}
