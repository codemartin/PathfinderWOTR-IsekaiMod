using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class SlimeReincarnateHeritage
{
	public static void Add()
	{
		Sprite Icon_Slime = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("4e83f1e0e52b4613982b14ee2796928f"))?.m_Icon;
		HumanHeritageSelection.Register(Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeReincarnateHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Slime Reincarnate Heritage");
			bp.SetDescription(Main.IsekaiContext, "Reincarnated into Golarion not as a humanoid mortal, but as a fluid, sentient Slime, your amorphous biology provides extraordinary resilience and adaptive capabilities.\nGrants a +4 racial bonus to Constitution, a +2 racial bonus to Dexterity, a +2 racial bonus on Knowledge (World), Perception, and Initiative checks, a +4 racial bonus on saving throws against poison, acid, and paralysis effects, and a +2 racial bonus on combat maneuver checks.\nAmorphous Body: immunity to acid damage.\nThe character preview shows your true slime body. In play you hold a humanoid shape, and a Devourer archetype gains Shift Form: Slime / Humanoid to change shape at will.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Slime;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Constitution;
				c.Value = 4;
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
				c.Value = 4;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddEnergyImmunity c)
			{
				c.Type = DamageEnergyType.Acid;
			});
			bp.AddComponent<PrerequisiteIsMainCharacter>();
			bp.AddComponent(delegate(PrerequisiteNoArchetype c)
			{
				c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "OverlordArchetype");
			});
			bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
			bp.ReapplyOnLevelUp = true;
		}));
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
