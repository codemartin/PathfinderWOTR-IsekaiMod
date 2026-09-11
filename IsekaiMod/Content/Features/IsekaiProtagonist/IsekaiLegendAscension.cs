using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class IsekaiLegendAscension
	{
		private static readonly Sprite Icon_Legend = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintProgression>("905383229aaf79e4b8d7e2d316b68715"))?.m_Icon;

		public static void Add()
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "UnboundLegendFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Unbound Legend");
				bp.SetDescription(Main.IsekaiContext, "Awakening as an Otherworlder past the bounds of mortality and mythic strictures, the Isekai Protagonist refuses to be constrained by ordinary planar laws. You gain a +8 inherent bonus to all ability scores, retain all your Overpowered Abilities and Special Powers, gain 2 extra spell slots per spell level, and enhance Plot Armor with an additional +5 luck bonus to AC and saves, alongside immunity to death effects and energy drain.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Legend;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 5;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(AddSpellsPerDay c)
				{
					c.Amount = 2;
					c.Levels = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 20;
			});
			BlueprintProgression blueprint = BlueprintTools.GetBlueprint<BlueprintProgression>("905383229aaf79e4b8d7e2d316b68715");
			if (blueprint != null)
			{
				LevelEntry[] levelEntries = blueprint.LevelEntries;
				if (levelEntries != null && levelEntries.Length != 0)
				{
					levelEntries[0].m_Features.Add(blueprintFeature.ToReference<BlueprintFeatureBaseReference>());
				}
			}
		}

		public static BlueprintFeature Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "UnboundLegendFeature");
		}
	}
}
