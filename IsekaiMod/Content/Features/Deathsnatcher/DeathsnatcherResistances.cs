using IsekaiMod.Content.Classes.Deathsnatcher;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.Deathsnatcher
{
	internal class DeathsnatcherResistances
	{
		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherResistances", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Deathsnatcher Resistances");
				bp.SetDescription(Main.IsekaiContext, "The Deathsnatcher is immune to negative energy and deaths effects, and has cold and fire resistance 30. It also has spell resistance equal to 10 + the Deathsnatcher's level.");
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.ChannelNegativeHarm;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.ChannelNegativeHarm;
				});
				bp.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.NegativeEnergy;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 30;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 30;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 10;
					c.m_Class = new BlueprintCharacterClassReference[1] { DeathsnatcherClass.GetReference() };
				});
				bp.ReapplyOnLevelUp = true;
			});
		}
	}
}
