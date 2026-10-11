using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

internal class IsekaiLegendAscension
{
	private static readonly Sprite Icon_Legend = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintProgression>("905383229aaf79e4b8d7e2d316b68715"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("e15e5e7045fda2244b98c8f010adfe31"))?.m_Icon;

	public static void Add()
	{
		BlueprintFeature UnboundLegendFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "UnboundLegendFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Unbound Legend");
			bp.SetDescription(Main.IsekaiContext, "Transmigrating past the bounds of mortality and mythic strictures, the Isekai Protagonist refuses to be constrained by ordinary planar laws. You gain a +8 inherent bonus to all ability scores that stacks with your other inherent bonuses, retain all your Overpowered Abilities and Special Powers, gain 2 extra spell slots per spell level, and enhance Plot Armor with an additional +5 luck bonus to AC and saves that stacks with Plot Armor's own luck bonus, alongside immunity to death effects and energy drain.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Legend;
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			StatType[] array = new StatType[6]
			{
				StatType.Strength,
				StatType.Dexterity,
				StatType.Constitution,
				StatType.Intelligence,
				StatType.Wisdom,
				StatType.Charisma
			};
			foreach (StatType stat in array)
			{
				bp.AddComponent(delegate(AddStackingStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = stat;
					c.Value = 8;
				});
			}
			array = new StatType[4]
			{
				StatType.AC,
				StatType.SaveFortitude,
				StatType.SaveReflex,
				StatType.SaveWill
			};
			foreach (StatType stat2 in array)
			{
				bp.AddComponent(delegate(AddStackingStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = stat2;
					c.Value = 5;
				});
			}
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
			});
			bp.AddComponent<AddImmunityToEnergyDrain>();
			bp.AddComponent(delegate(AddSpellsPerDay c)
			{
				c.Amount = 2;
				c.Levels = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			});
		});
		UnboundLegendFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
		{
			c.Level = 20;
			c.CheckInProgression = true;
		});
		UnboundLegendFeature.AddComponent(delegate(PrerequisiteClassLevel c)
		{
			c.m_CharacterClass = IsekaiProtagonistClass.Get().ToReference<BlueprintCharacterClassReference>();
			c.Level = 1;
			c.CheckInProgression = true;
		});
		BlueprintFeature LegendFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("7f99961610390044391f300c7ef5e0c8");
		if (LegendFeature != null)
		{
			UnboundLegendFeature.AddComponent(delegate(PrerequisiteFeature c)
			{
				c.m_Feature = LegendFeature.ToReference<BlueprintFeatureReference>();
				c.CheckInProgression = true;
			});
		}
		Helpers.CreateBlueprint(Main.IsekaiContext, "UnboundLegendWatcherFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Unbound Legend Watcher");
			bp.SetDescription(Main.IsekaiContext, "Watches for the Isekai Protagonist's ascension past the bounds of mortality.");
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			bp.HideInCharacterSheetAndLevelUp = true;
			bp.AddComponent(delegate(AddFeatureOnLegendAscension c)
			{
				c.m_Feature = UnboundLegendFeature.ToReference<BlueprintFeatureReference>();
				c.m_RequiredFact = LegendFeature?.ToReference<BlueprintUnitFactReference>();
				c.m_RequiredClass = IsekaiProtagonistClass.Get().ToReference<BlueprintCharacterClassReference>();
				c.RequiredCharacterLevel = 20;
			});
		});
	}

	public static BlueprintFeature Get()
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "UnboundLegendFeature");
	}

	public static BlueprintFeature GetWatcher()
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "UnboundLegendWatcherFeature");
	}
}
