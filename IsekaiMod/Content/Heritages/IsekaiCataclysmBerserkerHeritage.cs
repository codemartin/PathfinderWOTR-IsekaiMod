using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiCataclysmBerserkerHeritage
	{
		public static void Add()
		{
			Sprite Icon_Frenzy = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("29e2f51e6dd7427099b015de88718990")).m_Icon;
			BlueprintFeature OrcFerocity = BlueprintTools.GetBlueprint<BlueprintFeature>("c99f3405d1ef79049bd90678a666e1d7");
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiCataclysmBerserkerHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cataclysm Berserker");
				bp.SetDescription(Main.IsekaiContext, "Reincarnated carrying the apocalyptic battle-rage of cataclysm warlords who shatter armies bare-handed. Their frenzy defies mortality itself.\nThe Cataclysm Berserker gains a +4 racial bonus to Strength and Constitution, DR 5/-, a +4 bonus on attack rolls to confirm critical hits, a +4 racial bonus to all weapon damage rolls, a +4 racial bonus on Persuasion (Intimidation) checks, and the Deathless Ferocity trait allowing them to remain conscious and fight on even when brought below 0 hit points.");
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
					c.Bonus = 4;
					c.Value = 0; // the component reads Value unconditionally, so a missing one throws on every attack roll
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { OrcFerocity.ToReference<BlueprintUnitFactReference>() };
				});
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.ReapplyOnLevelUp = true;
			});
			BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8c3244440e0b4d1d9d9b182685cbacbd")?.AddToSelection(feature);
			HumanHeritageSelection.Register(feature);
		}
	}
}
