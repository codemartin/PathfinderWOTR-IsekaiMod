using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class EnergyImmunitySelection
	{
		private static readonly Sprite Icon_ProtectionFromAcid = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("3d77ee3fc4913c44b9df7c5bbcdc4906")).m_Icon;

		private static readonly Sprite Icon_ProtectionFromCold = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("021d39c8e0eec384ba69140f4875e166")).m_Icon;

		private static readonly Sprite Icon_ProtectionFromFire = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("3f9605134d34e1243b096e1f6cb4c148")).m_Icon;

		private static readonly Sprite Icon_ProtectionFromElectricity = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("e24ce0c3e8eaaaf498d3656b534093df")).m_Icon;

		private static readonly Sprite Icon_ProtectionFromSonic = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0cee375b4e5265a46a13fc269beb8763")).m_Icon;

		private static readonly Sprite Icon_ProtectionFromEnergy = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d2f116cfe05fcdd4a94e80143b67046f")).m_Icon;

		public static void Add()
		{
			BlueprintFeatureReference[] EnergyImmunityList = new BlueprintFeatureReference[5]
			{
				CreateImmunity("AcidImmunity", "You gain immunity to Acid.", Icon_ProtectionFromAcid, DamageEnergyType.Acid, SpellDescriptor.Acid),
				CreateImmunity("ColdImmunity", "You gain immunity to Cold.", Icon_ProtectionFromCold, DamageEnergyType.Cold, SpellDescriptor.Cold),
				CreateImmunity("FireImmunity", "You gain immunity to Fire.", Icon_ProtectionFromFire, DamageEnergyType.Fire, SpellDescriptor.Fire),
				CreateImmunity("ElectricityImmunity", "You gain immunity to Electricity.", Icon_ProtectionFromElectricity, DamageEnergyType.Electricity, SpellDescriptor.Electricity),
				CreateImmunity("SonicImmunity", "You gain immunity to Sonic.", Icon_ProtectionFromSonic, DamageEnergyType.Sonic, SpellDescriptor.Sonic)
			};
			SpecialPowerSelection.AddToDefenseSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "EnergyImmunitySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Energy Immunity");
				bp.SetDescription(Main.IsekaiContext, "You gain energy immunity of a particular type.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ProtectionFromEnergy;
				bp.Ranks = 5;
				bp.IsClassFeature = true;
				bp.m_Features = EnergyImmunityList;
				bp.m_AllFeatures = EnergyImmunityList;
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}

		private static BlueprintFeatureReference CreateImmunity(string name, string description, Sprite icon, DamageEnergyType energy, SpellDescriptor descriptor)
		{
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, string.Concat(name.Select((char x) => (!char.IsUpper(x)) ? x.ToString() : (" " + x))).TrimStart(' '));
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = energy;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = descriptor;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = descriptor;
				});
			}).ToReference<BlueprintFeatureReference>();
		}
	}
}
