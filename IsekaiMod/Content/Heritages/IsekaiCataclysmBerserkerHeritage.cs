using IsekaiMod.Config;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiCataclysmBerserkerHeritage
{
	public static void Add()
	{
		Sprite Icon_Frenzy = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("29e2f51e6dd7427099b015de88718990"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("97b991256e43bb140b263c326f690ce2"))?.m_Icon;
		BlueprintFeature OrcFerocity = BlueprintTools.GetBlueprint<BlueprintFeature>("c99f3405d1ef79049bd90678a666e1d7");
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiCataclysmBerserkerHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Cataclysm Berserker");
			bp.SetDescription(Main.IsekaiContext, "Reincarnated carrying the apocalyptic battle-rage of cataclysm warlords who shatter armies bare-handed. Their frenzy defies mortality itself.\nThe Cataclysm Berserker gains a +4 racial bonus to Strength and Constitution, DR 5/-, a +4 bonus on attack rolls to confirm critical hits, a +2 racial bonus on damage rolls, and a +4 racial bonus on Persuasion checks.\nDeathless Ferocity: they have orc ferocity, remaining conscious and fighting on when brought below 0 hit points.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Frenzy;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 5;
			});
			bp.AddComponent(delegate(CriticalConfirmationBonus c)
			{
				c.Value = 0;
				c.Bonus = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillPersuasion;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = OrcFerocity.ToReference<BlueprintUnitFactReference>();
				c.m_Feature = OrcFerocity.ToReference<BlueprintUnitFactReference>();
				c.Not = true;
			});
			bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
			bp.ReapplyOnLevelUp = true;
		});
		BlueprintFeatureSelection blueprint = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8c3244440e0b4d1d9d9b182685cbacbd");
		if (blueprint != null)
		{
			ContentOffering.OfferInSelection(blueprint, feature, OfferedContent.IsekaiHeritages);
		}
		HumanHeritageSelection.Register(feature);
	}
}
