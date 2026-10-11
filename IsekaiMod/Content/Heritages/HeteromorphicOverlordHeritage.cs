using IsekaiMod.Components;
using IsekaiMod.Config;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class HeteromorphicOverlordHeritage
{
	public static void Add()
	{
		Sprite Icon_Overlord = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bd9a5db280284025ab01474b0a4cd434"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0093fc0098c984c43ac239d1211d1311"))?.m_Icon;
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "HeteromorphicOverlordHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Heteromorphic Overlord Heritage");
			bp.SetDescription(Main.IsekaiContext, "Reincarnated into Golarion as an ancient heteromorphic undead sovereign, your physical shell is merely an external projection concealing an immortal, supreme skeletal monarch. You possess unnatural arcane mastery, overwhelming presence, and innate dominance over death and undeath.\nGrants a +4 racial bonus to Intelligence, a +2 racial bonus to Charisma, a +1 bonus to Caster Level and DC of Necromancy spells, a +2 racial bonus on Spell Penetration checks, and a +2 racial bonus on Knowledge (Arcana) and Lore (Religion) checks.\nDeathless Sovereign: immunity to fear, and negative energy resistance 10.\nThe character preview shows your true form beneath the mortal guise. In play you wear your mortal guise, and an Overlord archetype gains Shift Form: Overlord / Mortal Guise to change shape at will.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Overlord;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Intelligence;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
			bp.AddComponent(delegate(IncreaseSpellSchoolCasterLevel c)
			{
				c.School = SpellSchool.Necromancy;
				c.BonusLevel = 1;
				c.Descriptor = ModifierDescriptor.Racial;
			});
			bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
			{
				c.School = SpellSchool.Necromancy;
				c.BonusDC = 1;
				c.Descriptor = ModifierDescriptor.Racial;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Value = 2;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fear;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fear;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Frightened;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Shaken;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.NegativeEnergy;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillKnowledgeArcana;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillLoreReligion;
				c.Value = 2;
			});
			bp.AddComponent<PrerequisiteIsMainCharacter>();
			bp.AddComponent(delegate(PrerequisiteNoArchetype c)
			{
				c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "DevourerArchetype");
			});
			bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
			bp.ReapplyOnLevelUp = true;
		});
		ContentOffering.OfferInSelection(FeatTools.Selections.DhampirHeritageSelection, feature, OfferedContent.IsekaiHeritages);
		HumanHeritageSelection.Register(feature);
	}

	public static BlueprintFeature Get()
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeteromorphicOverlordHeritage");
	}

	public static BlueprintFeatureReference GetReference()
	{
		return Get().ToReference<BlueprintFeatureReference>();
	}
}
