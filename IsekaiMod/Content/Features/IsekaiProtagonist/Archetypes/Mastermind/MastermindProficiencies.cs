using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind
{
	internal class MastermindProficiencies
	{
		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindProficiencies", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Mastermind Proficiences");
				bp.SetDescription(Main.IsekaiContext, "The Mastermind is proficient with all simple and martial weapons and with all armor (heavy, light, and medium). They can cast {g|Encyclopedia:Spell}spells{/g} from this class while wearing armor without incurring the normal {g|Encyclopedia:Spell_Fail_Chance}arcane spell failure chance{/g}, but they incur the normal arcane spell failure chance for arcane spells received from other classes.");
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
				// Count Mastermind levels as Arcanist levels for prerequisites, so exploits such as Swift Consume qualify.
				bp.AddComponent(delegate(ClassLevelsForPrerequisites c)
				{
					c.m_FakeClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("52dbfd8505e22f84fad8d702611f60b7");
					c.m_ActualClass = IsekaiProtagonistClass.GetReference();
					c.Modifier = 1.0;
				});
			});
		}
	}
}
