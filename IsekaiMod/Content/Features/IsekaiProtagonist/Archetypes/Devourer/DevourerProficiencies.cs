using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;

internal class DevourerProficiencies
{
	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "DevourerProficiencies", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Slime Proficiencies");
			bp.SetDescription(Main.IsekaiContext, "The Slime relies on instinct and biological weapons, gaining proficiency with simple weapons, natural weapons, and light armor without incurring arcane spell failure. The Slime also knows the Corrosive Slime cantrip.");
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					StaticReferences.Proficiencies.LightArmorProficiency.ToReference<BlueprintUnitFactReference>(),
					StaticReferences.Proficiencies.SimpleWeaponProficiency.ToReference<BlueprintUnitFactReference>()
				};
			});
			bp.AddComponent(delegate(ArcaneArmorProficiency c)
			{
				c.Armor = new ArmorProficiencyGroup[1] { ArmorProficiencyGroup.Light };
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "CorrosiveSlimeAbility") };
			});
			bp.AddComponent(delegate(BindAbilitiesToClass c)
			{
				c.m_Abilites = new BlueprintAbilityReference[1] { BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(Main.IsekaiContext, "CorrosiveSlimeAbility") };
				c.Cantrip = true;
				c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
				c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
				c.m_Archetypes = new BlueprintArchetypeReference[0];
				c.Stat = StatType.Charisma;
			});
			bp.AddComponent(delegate(LearnSignatureCantrip c)
			{
				c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
				c.m_Cantrip = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(Main.IsekaiContext, "CorrosiveSlimeAbility");
			});
		});
	}
}
