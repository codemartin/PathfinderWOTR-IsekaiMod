using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class ParadoxSovereign
	{
		private static readonly Sprite Icon_Paradox = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SECOND_REINCARNATION.png");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ParadoxSovereignFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Paradox Sovereign");
				bp.SetDescription(Main.IsekaiContext, "Commanding the axiomatic mechanics of Axis and the Inevitable paradox, you manipulate temporal loops and causality.\nBenefit: You gain a +4 insight bonus to Armor Class, attack rolls, and saving throws, Spell Resistance equal to 15 + character level, Damage Reduction 10/-, and complete immunity to confusion, insanity, and feeblemind.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Paradox;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Confusion;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Confusion;
				});
			}));
		}
	}
}
