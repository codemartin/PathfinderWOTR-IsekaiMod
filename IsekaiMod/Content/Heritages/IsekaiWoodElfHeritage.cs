using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiWoodElfHeritage
	{
		private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

		public static void Add()
		{
			Sprite Icon_Wood_Elf = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_WOOD_ELF.png");
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiWoodElfHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Wood Elf");
				bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a Wood Elf have both extreme beauty and power. They are a nimble and natural reflection of the elven race.\nThe Isekai Wood Elf has a +4 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g}, a +2 racial bonus to {g|Encyclopedia:Wisdom}Wisdom{/g} and {g|Encyclopedia:Intelligence}Intelligence{/g}, and a -2 penalty to Constitution. They have spell resistance equal to 10 + their character level. They can also move faster other elves and have a +10 bonus to speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Wood_Elf;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Intelligence;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Wisdom;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonusIfHasFact c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = -2;
					c.InvertCondition = true;
					c.m_CheckedFacts = new BlueprintUnitFactReference[1] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				bp.Groups = new FeatureGroup[0];
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.ElvenHeritageSelection.AddToSelection(feature);
		}
	}
}
