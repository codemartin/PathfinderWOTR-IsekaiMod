using IsekaiMod.Content.Classes.IsekaiProtagonist;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SoloMastery
{
	public static class SoloSovereign
	{
		private static bool Added = false;

		private static readonly Sprite Icon_Solo = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		private static BlueprintFeatureSelection BasicFeatSelection => FeatTools.Selections.BasicFeatSelection;

		public static BlueprintFeature SoloSovereignBaseFeature { get; private set; }

		public static BlueprintFeatureSelection SoloSovereignFeatSelection { get; private set; }

		public static BlueprintFeature SoloAwakeningSlime { get; private set; }

		public static BlueprintFeature SoloAwakeningShadowMonarch { get; private set; }

		public static BlueprintFeature SoloAwakeningOverlord { get; private set; }

		public static BlueprintFeature SoloAwakeningGodEmperor { get; private set; }

		public static BlueprintFeature SoloAwakeningHero { get; private set; }

		public static BlueprintFeature SoloAwakeningMastermind { get; private set; }

		public static BlueprintFeature SoloAwakeningMartialGod { get; private set; }

		private static BlueprintArchetypeReference GetArchetypeReference(string name)
		{
			BlueprintGuid gUID = Main.IsekaiContext.Blueprints.GetGUID(name);
			return new BlueprintArchetypeReference
			{
				deserializedGuid = gUID
			};
		}

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
				bp.m_Features = bp.m_AllFeatures;
			});
			SoloAwakeningSlime = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningSlime", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solo Awakening: Primeval Density");
				bp.SetDescription(Main.IsekaiContext, "Walking alone as an apex amorphous entity, your mass becomes extraordinarily dense. You gain a +2 inherent bonus to Constitution and a +2 natural armor bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
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
					c.m_Archetype = GetArchetypeReference("DevourerArchetype");
					c.Level = 1;
				});
			});
			SoloAwakeningShadowMonarch = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningShadowMonarch", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solo Awakening: Lone Monarch Dominance");
				bp.SetDescription(Main.IsekaiContext, "Refusing a permanent marshall at your side, you embody the entire shadow army within yourself. You gain a +2 inherent bonus to Strength and a +2 bonus to CMB.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
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
					c.m_Archetype = GetArchetypeReference("ShadowMonarchArchetype");
					c.Level = 1;
				});
			});
			SoloAwakeningOverlord = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningOverlord", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solo Awakening: Solitary Supreme Ruler");
				bp.SetDescription(Main.IsekaiContext, "Governing without floor overseers, the Supreme Being commands absolute reverence. You gain a +2 inherent bonus to Charisma and Damage Reduction 3/-.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
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
					c.m_Archetype = GetArchetypeReference("OverlordArchetype");
					c.Level = 1;
				});
			});
			SoloAwakeningGodEmperor = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningGodEmperor", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solo Awakening: Solitary Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "Ascending to godhood through purely personal divinity, you gain a +2 inherent bonus to Charisma and a +2 sacred bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
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
					c.m_Archetype = GetArchetypeReference("GodEmperorArchetype");
					c.Level = 1;
				});
			});
			SoloAwakeningHero = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningHero", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solo Awakening: Solitary Champion");
				bp.SetDescription(Main.IsekaiContext, "Carrying the weight of the timeline on your own shoulders, your reflexes sharpen beyond mortal limits. You gain a +2 inherent bonus to Dexterity and a +4 insight bonus on Initiative rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
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
					c.m_Archetype = GetArchetypeReference("HeroArchetype");
					c.Level = 1;
				});
			});
			SoloAwakeningMastermind = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningMastermind", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solo Awakening: Singular Grandmaster");
				bp.SetDescription(Main.IsekaiContext, "Relying on no co-conspirator, your intellect operates at flawless calculation speed. You gain a +2 inherent bonus to Intelligence and a +1 bonus to the Difficulty Class of all your spells and abilities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
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
					c.m_Archetype = GetArchetypeReference("MastermindArchetype");
					c.Level = 1;
				});
			});
			SoloAwakeningMartialGod = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloAwakeningMartialGod", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solo Awakening: Unbound Martial Will");
				bp.SetDescription(Main.IsekaiContext, "Channeling all spiritual resonance directly into your own flesh and weapons, you gain a +2 inherent bonus to Strength and 2 additional attacks of opportunity per round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
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
					c.m_Archetype = GetArchetypeReference("MartialGodArchetype");
					c.Level = 1;
				});
			});
			SoloSovereignBaseFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SoloSovereignFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solo Sovereign (No Companion)");
				bp.SetDescription(Main.IsekaiContext, "You choose to walk the path of absolute singularity. Forgoing any companion, familiar, or retinue partner, your solitary will tempers your mind and body.\n\nGrants a +2 bonus to all saving throws, a bonus combat or teamwork feat, and unlocks an exclusive Solo Sovereign Awakening based on your archetype at 1st level.\n\nNote: If you later decide to form a bond with a companion or guardian, you may adopt one via an Otherworldly Guardian Pact without losing any of your Solo Sovereign powers.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Solo;
				bp.IsClassFeature = true;
				bp.Ranks = 1;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = SoloSovereignFeatSelection.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = SoloAwakeningSlime.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = SoloAwakeningShadowMonarch.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = SoloAwakeningOverlord.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = SoloAwakeningGodEmperor.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = SoloAwakeningHero.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = SoloAwakeningMastermind.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = SoloAwakeningMartialGod.ToReference<BlueprintFeatureReference>();
				});
			});
		}
	}
}
