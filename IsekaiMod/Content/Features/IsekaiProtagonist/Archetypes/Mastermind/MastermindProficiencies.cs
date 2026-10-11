using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;

internal class MastermindProficiencies
{
	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindProficiencies", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mastermind Proficiences");
			bp.SetDescription(Main.IsekaiContext, "The Mastermind is proficient with all simple and martial weapons and with all armor (heavy, light, and medium). They can cast {g|Encyclopedia:Spell}spells{/g} from this class while wearing armor without incurring the normal {g|Encyclopedia:Spell_Fail_Chance}arcane spell failure chance{/g}, but they incur the normal arcane spell failure chance for arcane spells received from other classes. The Mastermind also knows the Mind Spike cantrip.");
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[5]
				{
					StaticReferences.Proficiencies.LightArmorProficiency.ToReference<BlueprintUnitFactReference>(),
					StaticReferences.Proficiencies.MediumArmorProficiency.ToReference<BlueprintUnitFactReference>(),
					StaticReferences.Proficiencies.HeavyArmorProficiency.ToReference<BlueprintUnitFactReference>(),
					StaticReferences.Proficiencies.SimpleWeaponProficiency.ToReference<BlueprintUnitFactReference>(),
					StaticReferences.Proficiencies.MartialWeaponProficiency.ToReference<BlueprintUnitFactReference>()
				};
			});
			bp.AddComponent(delegate(ArcaneArmorProficiency c)
			{
				c.Armor = new ArmorProficiencyGroup[3]
				{
					ArmorProficiencyGroup.Light,
					ArmorProficiencyGroup.Medium,
					ArmorProficiencyGroup.Heavy
				};
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "MindSpikeAbility") };
			});
			bp.AddComponent(delegate(BindAbilitiesToClass c)
			{
				c.m_Abilites = new BlueprintAbilityReference[1] { BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(Main.IsekaiContext, "MindSpikeAbility") };
				c.Cantrip = true;
				c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
				c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
				c.m_Archetypes = new BlueprintArchetypeReference[0];
				c.Stat = StatType.Intelligence;
			});
			bp.AddComponent(delegate(LearnSignatureCantrip c)
			{
				c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
				c.m_Cantrip = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(Main.IsekaiContext, "MindSpikeAbility");
			});
		});
	}
}
