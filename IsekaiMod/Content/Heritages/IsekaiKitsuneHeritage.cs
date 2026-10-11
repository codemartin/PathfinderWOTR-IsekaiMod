using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiKitsuneHeritage
{
	public static void Add()
	{
		Sprite Icon_Furry = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_FURRY.png");
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiKitsuneHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Isekai Kitsune");
			bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a Kitsune have both extreme beauty and power. They are very clever and powerful as you are born partially divine.\nThe Isekai Kitsune has a +4 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} and a +2 racial bonus to {g|Encyclopedia:Intelligence}Intelligence{/g} and {g|Encyclopedia:Charisma}Charisma{/g}, a +2 racial bonus on Will saves and on Initiative, and a +10 foot bonus to speed.\nDivine Vigor: they have fast healing equal to their character level. They keep the kitsune spell-like ability.");
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
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Initiative;
				c.Value = 2;
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
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("a212fff222f57a841aa88bc49a5962d9") };
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
		ContentOffering.OfferInSelection(FeatTools.Selections.KitsuneHeritageSelection, feature, OfferedContent.IsekaiHeritages);
	}
}
