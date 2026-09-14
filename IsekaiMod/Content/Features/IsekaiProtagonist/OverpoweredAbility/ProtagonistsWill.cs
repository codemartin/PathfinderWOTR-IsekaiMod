using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class ProtagonistsWill
	{
		private static readonly Sprite Icon_Will = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_GODHOOD.png");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ProtagonistsWillFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Protagonist's Will");
				bp.SetDescription(Main.IsekaiContext, "No matter how dire the crisis or insurmountable the enemy, your indomitable willpower refuses to yield.\nBenefit: You gain a +4 luck bonus to Armor Class, attack rolls, damage rolls, and saving throws, Damage Reduction 10/-, Fast Healing 10, and complete immunity to fear, compulsion, and mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Will;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
					c.Bonus = 0;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Shaken;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Frightened;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Shaken | SpellDescriptor.Frightened;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Shaken | SpellDescriptor.Frightened;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear | SpellDescriptor.Compulsion;
				});
			}));
		}
	}
}
