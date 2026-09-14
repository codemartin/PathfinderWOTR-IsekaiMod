using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	internal class EssenceAssimilation
	{
		private static readonly Sprite Icon_Assimilation = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "EssenceAssimilationFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Essence Assimilation");
				bp.SetDescription(Main.IsekaiContext, "At 3rd level, the Slime's biology continuously assimilates traits from consumed lifeforms:\n• Draconic Scales: Fire resistance 30 and a +3 Natural Armor bonus.\n• Demonic Blood: Poison immunity and +2 unholy damage on all weapon and natural attacks.\n• Undead Core: Complete immunity to death effects, negative levels, and energy drain.\n• Beastial Pounce: Gain the Pounce ability, making full attacks at the end of a charge.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Assimilation;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 30;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Poison;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Poison;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Sickened;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Sickened;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Sickened;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.NegativeEnergy;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent<AddImmunityToEnergyDrain>();
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { BlueprintTools.GetBlueprint<BlueprintFeature>("1a8149c09e0bdfc48a305ee6ac3729a8").ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
