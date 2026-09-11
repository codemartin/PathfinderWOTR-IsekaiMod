using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages
{
	internal class ElfHeritagePatcher
	{
		private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

		public static void Patch()
		{
			BlueprintUnitFactReference IsekaiDarkElfHeritageReference = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "IsekaiDarkElfHeritage");
			BlueprintUnitFactReference IsekaiHighElfHeritageReference = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "IsekaiHighElfHeritage");
			BlueprintUnitFactReference IsekaiWoodElfHeritageReference = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "IsekaiWoodElfHeritage");
			BlueprintRace blueprint = BlueprintTools.GetBlueprint<BlueprintRace>("25a5878d125338244896ebd3238226c8");
			if (blueprint != null)
			{
				blueprint.RemoveComponents<AddStatBonus>();
				blueprint.RemoveComponents<AddStatBonusIfHasFact>();
				BlueprintFeatureBase obj = blueprint.Features.FirstOrDefault((BlueprintFeatureBase f) => f != null && f.name == "ElfAbilityModifiers") ?? blueprint;
				obj.RemoveComponents<AddStatBonus>();
				obj.RemoveComponents<AddStatBonusIfHasFact>();
				obj.AddComponent(delegate(AddStatBonusIfNotHasFact c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
					c.m_CheckedFacts = new BlueprintUnitFactReference[3] { IsekaiDarkElfHeritageReference, IsekaiHighElfHeritageReference, IsekaiWoodElfHeritageReference };
				});
				obj.AddComponent(delegate(AddStatBonusIfNotHasFact c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Intelligence;
					c.Value = 2;
					c.m_CheckedFacts = new BlueprintUnitFactReference[3] { IsekaiDarkElfHeritageReference, IsekaiHighElfHeritageReference, IsekaiWoodElfHeritageReference };
				});
				List<BlueprintUnitFactReference> conCheckedFacts = new List<BlueprintUnitFactReference>();
				if (DestinyBeyondBirthMythicFeat != null)
				{
					conCheckedFacts.Add(DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>());
				}
				conCheckedFacts.Add(IsekaiDarkElfHeritageReference);
				conCheckedFacts.Add(IsekaiHighElfHeritageReference);
				conCheckedFacts.Add(IsekaiWoodElfHeritageReference);
				obj.AddComponent(delegate(AddStatBonusIfNotHasFact c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = -2;
					c.m_CheckedFacts = conCheckedFacts.ToArray();
				});
			}
		}
	}
}
