using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch
{
	internal class ShadowMonarchProficiencies
	{
		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchProficiencies", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Monarch Proficiencies");
				bp.SetDescription(Main.IsekaiContext, "The Shadow Monarch is proficient with all simple and martial weapons, scythes, dueling swords, daggers, as well as light and medium armor without incurring arcane spell failure.");
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
			});
		}
	}
}
