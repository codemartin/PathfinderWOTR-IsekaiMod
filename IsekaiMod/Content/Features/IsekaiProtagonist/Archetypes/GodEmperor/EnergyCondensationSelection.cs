using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class EnergyCondensationSelection
	{
		private static readonly Sprite Icon_LightEnergyCondensation = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_ENERGY_LIGHT.png");

		private static readonly Sprite Icon_DarkEnergyCondensation = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_ENERGY_DARK.png");

		public static void Add()
		{
			BlueprintFeature LightEnergyCondensation = Helpers.CreateBlueprint(Main.IsekaiContext, "LightEnergyCondensation", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Light Energy Condensation");
				bp.SetDescription(Main.IsekaiContext, "You gain resistance against all elements (acid, cold, electricity, fire, and sonic) equal to 10 + 2 times your character level. Your physical attacks are treated as good for the purpose of overcoming {g|Encyclopedia:Damage_Reduction}damage reduction{/g}.");
				((BlueprintUnitFact)bp).m_Icon = Icon_LightEnergyCondensation;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Sonic;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.DoublePlusBonusValue;
					c.m_StepLevel = 10;
				});
				bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
				{
					c.AddAlignment = true;
					c.Alignment = DamageAlignment.Good;
				});
				// Recalculate the rank context without rebuilding five energy
				// resistances and the fixed weapon-alignment property.
				bp.ReapplyOnLevelUp = false;
			});
			BlueprintFeature DarkEnergyCondensation = Helpers.CreateBlueprint(Main.IsekaiContext, "DarkEnergyCondensation", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dark Energy Condensation");
				bp.SetDescription(Main.IsekaiContext, "You gain resistance against all elements (acid, cold, electricity, fire, and sonic) equal to 10 + 2 times your character level. Your physical attacks are treated as evil for the purpose of overcoming {g|Encyclopedia:Damage_Reduction}damage reduction{/g}.");
				((BlueprintUnitFact)bp).m_Icon = Icon_DarkEnergyCondensation;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Sonic;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.DoublePlusBonusValue;
					c.m_StepLevel = 10;
				});
				bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
				{
					c.AddAlignment = true;
					c.Alignment = DamageAlignment.Evil;
				});
				// Recalculate the rank context without rebuilding five energy
				// resistances and the fixed weapon-alignment property.
				bp.ReapplyOnLevelUp = false;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "EnergyCondensationSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Energy Condensation");
				bp.SetDescription(Main.IsekaiContext, "At 5th level, you are able to condense your channelled energy to form a protective layer around you, giving you resistance against elemental damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_LightEnergyCondensation;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					LightEnergyCondensation.ToReference<BlueprintFeatureReference>(),
					DarkEnergyCondensation.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}
	}
}
