using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;

internal class ShadowMonarchProficiencies
{
	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchProficiencies", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Monarch Proficiencies");
			bp.SetDescription(Main.IsekaiContext, "The Shadow Monarch is proficient with all simple and martial weapons, scythes, dueling swords, daggers, as well as light and medium armor without incurring arcane spell failure. The Shadow Monarch also knows the Shadow Dagger cantrip.");
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[4]
				{
					StaticReferences.Proficiencies.LightArmorProficiency.ToReference<BlueprintUnitFactReference>(),
					StaticReferences.Proficiencies.MediumArmorProficiency.ToReference<BlueprintUnitFactReference>(),
					StaticReferences.Proficiencies.SimpleWeaponProficiency.ToReference<BlueprintUnitFactReference>(),
					StaticReferences.Proficiencies.MartialWeaponProficiency.ToReference<BlueprintUnitFactReference>()
				};
			});
			bp.AddComponent(delegate(ArcaneArmorProficiency c)
			{
				c.Armor = new ArmorProficiencyGroup[2]
				{
					ArmorProficiencyGroup.Light,
					ArmorProficiencyGroup.Medium
				};
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "ShadowDaggerAbility") };
			});
			bp.AddComponent(delegate(BindAbilitiesToClass c)
			{
				c.m_Abilites = new BlueprintAbilityReference[1] { BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(Main.IsekaiContext, "ShadowDaggerAbility") };
				c.Cantrip = true;
				c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
				c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
				c.m_Archetypes = new BlueprintArchetypeReference[0];
				c.Stat = StatType.Charisma;
			});
			bp.AddComponent(delegate(LearnSignatureCantrip c)
			{
				c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
				c.m_Cantrip = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(Main.IsekaiContext, "ShadowDaggerAbility");
			});
		});
	}
}
