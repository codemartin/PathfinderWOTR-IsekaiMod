using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class SovereignOfTheGreatTomb
{
	private static readonly Sprite Icon_TombSovereign = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_DARK.png");

	public static void Add()
	{
		BlueprintBuff buff = TTCoreExtensions.CreateBuff("SovereignOfTheGreatTombBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Dominion of the Great Tomb");
			bp.SetDescription(Main.IsekaiContext, "Empowered by the Supreme Ruler of the Great Tomb, you gain a +4 profane bonus to Strength, Dexterity, Constitution, Armor Class, attack rolls, and damage rolls, DR 10/Good, and Fast Healing 5.");
			((BlueprintUnitFact)bp).m_Icon = Icon_TombSovereign;
			bp.IsClassFeature = true;
			StatType[] array = new StatType[6]
			{
				StatType.Strength,
				StatType.Dexterity,
				StatType.Constitution,
				StatType.AC,
				StatType.AdditionalAttackBonus,
				StatType.AdditionalDamage
			};
			foreach (StatType stat in array)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = stat;
					c.Value = 4;
				});
			}
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
				c.BypassedByAlignment = true;
				c.Alignment = DamageAlignment.Good;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
		});
		BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("SovereignOfTheGreatTomb", Helpers.CreateString(Main.IsekaiContext, "SovereignOfTheGreatTomb.Name", "Overpowered Ability - Sovereign of the Great Tomb"), Helpers.CreateString(Main.IsekaiContext, "SovereignOfTheGreatTomb.Description", "Exclusive to the Overlord archetype. As absolute ruler of the supreme tomb and master of necrotic majesty, your necromantic authority is unmatched.\nBenefit: You emit a necrotic sovereign aura within 50 feet. Allies within the aura gain a +4 profane bonus to Strength, Dexterity, Constitution, Armor Class, attack rolls, and damage rolls, DR 10/Good, and Fast Healing 5. Furthermore, you personally gain complete immunity to negative energy, ability drain, energy drain, and death effects, and a +4 profane bonus to Intelligence and Charisma."), Icon_TombSovereign, delegate(BlueprintAbilityAreaEffect area)
		{
			PortedAuras.ConfigureAllyAura(area, buff, 50);
		});
		blueprintFeature.AddComponent(delegate(AddEnergyDamageImmunity c)
		{
			c.EnergyType = DamageEnergyType.NegativeEnergy;
		});
		blueprintFeature.AddComponent(delegate(AddImmunityToAbilityScoreDamage c)
		{
			c.Drain = true;
			c.StatTypes = new StatType[0];
		});
		blueprintFeature.AddComponent<AddImmunityToEnergyDrain>();
		blueprintFeature.AddComponent(delegate(BuffDescriptorImmunity c)
		{
			c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
		});
		blueprintFeature.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
		{
			c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
		});
		blueprintFeature.AddComponent(delegate(AddStatBonus c)
		{
			c.Descriptor = ModifierDescriptor.Profane;
			c.Stat = StatType.Intelligence;
			c.Value = 4;
		});
		blueprintFeature.AddComponent(delegate(AddStatBonus c)
		{
			c.Descriptor = ModifierDescriptor.Profane;
			c.Stat = StatType.Charisma;
			c.Value = 4;
		});
		blueprintFeature.AddComponent(delegate(PrerequisiteArchetypeLevel c)
		{
			c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
			c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "OverlordArchetype");
			c.Level = 1;
		});
		OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
	}
}
