using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class GodlyVessel
	{
		public static void Add()
		{
			Sprite Icon_PureForm = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("33e53b74891b4c34ba6ee3baa322beeb")).m_Icon;
			Helpers.CreateBlueprint(Main.IsekaiContext, "GodlyVessel", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Godly Vessel");
				bp.SetDescription(Main.IsekaiContext, "At 17th level, the God Emperor gains immunity to all elements (acid, cold, electricity, fire, and sonic).");
				((BlueprintUnitFact)bp).m_Icon = Icon_PureForm;
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Acid;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Cold;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Electricity;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Fire;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Sonic;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Acid;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Cold;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Electricity;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fire;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Sonic;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Acid;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Cold;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Electricity;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fire;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Sonic;
				});
			});
		}
	}
}
