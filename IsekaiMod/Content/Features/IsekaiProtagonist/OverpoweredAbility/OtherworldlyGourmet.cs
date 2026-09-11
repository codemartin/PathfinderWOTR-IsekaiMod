using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class OtherworldlyGourmet
	{
		private static readonly Sprite Icon_Feast = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_REGENERATION.png");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldlyGourmetFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Otherworldly Gourmet");
				bp.SetDescription(Main.IsekaiContext, "Infusing cooking with interdimensional spices and metaphysical culinary techniques, your meals grant legendary nourishment.\nBenefit: You and your party receive a +4 morale bonus to all six ability scores (Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma), Fast Healing 5, 50 temporary hit points, and complete immunity to poison, disease, sickened, and nauseated conditions.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Feast;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.Wisdom;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
					c.Bonus = 0;
				});
				bp.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
				{
					c.Value = 50;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Sickened;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Nauseated;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Poison | SpellDescriptor.Disease;
				});
			}));
		}
	}
}
