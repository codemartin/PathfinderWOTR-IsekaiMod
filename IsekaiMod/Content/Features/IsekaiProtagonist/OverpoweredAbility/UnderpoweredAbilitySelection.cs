using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class UnderpoweredAbilitySelection
	{
		public static void Add()
		{
			Sprite Icon_Haste = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98")).m_Icon;
			Sprite Icon_BelieveInYourselfStrength = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("9f476da1aa712284d8e652fc387ce5bb")).m_Icon;
			Sprite Icon_CrystalMind = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("4733d8dd549ff544395f1684ec73c392")).m_Icon;
			_ = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("cbf3bafa8375340498b86a3313a11e2f")).m_Icon;
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "DodgeMaster", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power - Dodge Master");
				bp.SetDescription(Main.IsekaiContext, "Blessed by the winds of fate in your new life, you can effortlessly dodge even the most well-aimed strikes.\nBenefit: You gain a +4 dodge bonus to AC and can move freely without provoking attacks of opportunity.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Haste;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddMechanicsFeature c)
				{
					c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.DisengageWithoutAttackOfOpportunity;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { BlueprintTools.GetBlueprint<BlueprintFeature>("2a6091b97ad940943b46262600eaeaeb").ToReference<BlueprintUnitFactReference>() };
				});
			});
			BlueprintFeature feature2 = Helpers.CreateBlueprint(Main.IsekaiContext, "SuperStrength", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power - Super Strength");
				bp.SetDescription(Main.IsekaiContext, "Infused with the strength of a titan, your physical might is a testament to your rebirth in this new world.\nBenefit: You gain +50 hit points and +4 Strength.");
				((BlueprintUnitFact)bp).m_Icon = Icon_BelieveInYourselfStrength;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 50;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
			});
			BlueprintFeature feature3 = Helpers.CreateBlueprint(Main.IsekaiContext, "ThoughtMaster", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power - Thought Master");
				bp.SetDescription(Main.IsekaiContext, "Your newfound mind is a gift from the cosmos, giving you unparalleled clarity and understanding.\nBenefit: You gain a +4 bonus to Intelligence and Wisdom, and a +2 bonus to Knowledge (Arcana), Knowledge (World), and Perception checks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_CrystalMind;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Wisdom;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeArcana;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 2;
				});
			});
			SpecialPowerSelection.AddToSelection(feature);
			SpecialPowerSelection.AddToSelection(feature2);
			SpecialPowerSelection.AddToSelection(feature3);
		}
	}
}
