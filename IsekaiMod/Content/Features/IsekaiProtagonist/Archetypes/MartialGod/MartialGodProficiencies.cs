using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod
{
	internal class MartialGodProficiencies
	{
		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodProficiencies", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial God Proficiencies");
				bp.SetDescription(Main.IsekaiContext, "The Martial God is proficient with all simple, martial, and exotic weapons as well as light and medium armor. They can cast spells from this class while wearing armor without incurring the normal arcane spell failure chance.");
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[5]
					{
						StaticReferences.Proficiencies.LightArmorProficiency.ToReference<BlueprintUnitFactReference>(),
						StaticReferences.Proficiencies.MediumArmorProficiency.ToReference<BlueprintUnitFactReference>(),
						StaticReferences.Proficiencies.SimpleWeaponProficiency.ToReference<BlueprintUnitFactReference>(),
						StaticReferences.Proficiencies.MartialWeaponProficiency.ToReference<BlueprintUnitFactReference>(),
						StaticReferences.Proficiencies.ExoticWeaponProficiency.ToReference<BlueprintUnitFactReference>()
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
			});
		}
	}
}
