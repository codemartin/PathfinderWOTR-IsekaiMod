using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiHighElfHeritage
{
	private static readonly Sprite Icon_TricksterCharmWhatever = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("943328ac5bc8a734e85b5b2af3ae2bf7"))?.m_Icon;

	public static void Add()
	{
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHighElfHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Isekai High Elf");
			bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a High Elf have both extreme beauty and power. They are a refined and cultured reflection of the elven race.\nThe Isekai High Elf has a +4 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Wisdom}Wisdom{/g} and a +2 racial bonus to {g|Encyclopedia:Dexterity}Dexterity{/g} and {g|Encyclopedia:Intelligence}Intelligence{/g}, which replace the elf's ability score adjustments, and a +2 racial bonus on Knowledge (Arcana) and Use Magic Device checks. They have spell resistance equal to 10 + their character level.\nArcane Reserves: they can cast 1 more spell per day of each spell level from 1st to 9th.");
			((BlueprintUnitFact)bp).m_Icon = Icon_TricksterCharmWhatever;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Dexterity;
				c.Value = 2;
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
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillKnowledgeArcana;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillUseMagicDevice;
				c.Value = 2;
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
			bp.AddComponent(delegate(AddSpellsPerDay c)
			{
				c.Amount = 1;
				c.Levels = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			});
			bp.Groups = new FeatureGroup[0];
			bp.ReapplyOnLevelUp = true;
		});
		ContentOffering.OfferInSelection(FeatTools.Selections.ElvenHeritageSelection, feature, OfferedContent.IsekaiHeritages);
	}
}
