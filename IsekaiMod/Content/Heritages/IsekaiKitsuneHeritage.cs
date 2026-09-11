using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiKitsuneHeritage
	{
		private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

		public static void Add()
		{
			Sprite Icon_Furry = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_FURRY.png");
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiKitsuneHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Kitsune");
				bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a Kitsune have both extreme beauty and power. They are very clever and powerful as you are born partially divine.\nThe Isekai Kitsune has a +4 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g}, a +2 racial bonus to {g|Encyclopedia:Intelligence}Intelligence{/g} and {g|Encyclopedia:Charisma}Charisma{/g}, and a -2 penalty to Strength. They have a +10 bonus to speed and have fast healing equal to their character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Furry;
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
					c.Stat = StatType.Charisma;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonusIfHasFact c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Strength;
					c.Value = -2;
					c.InvertCondition = true;
					c.m_CheckedFacts = new BlueprintUnitFactReference[1] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 0;
					c.Bonus = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
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
			FeatTools.Selections.KitsuneHeritageSelection.AddToSelection(feature);
		}
	}
}
