using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
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
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class SovereignOfTheGreatTomb
	{
		private static readonly Sprite Icon_TombSovereign = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_DARK.png");

		public static void Add()
		{
			BlueprintBuff SovereignOfTheGreatTombBuff = TTCoreExtensions.CreateBuff("SovereignOfTheGreatTombBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Dominion of the Great Tomb");
				bp.SetDescription(Main.IsekaiContext, "Empowered by the Supreme Ruler of the Great Tomb, you gain a +4 profane bonus to Strength, Dexterity, Constitution, Armor Class, attack rolls, and damage rolls, DR 10/Good, and Fast Healing 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TombSovereign;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
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
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("SovereignOfTheGreatTomb", Helpers.CreateString(Main.IsekaiContext, "SovereignOfTheGreatTomb.Name", "Overpowered Ability - Sovereign of the Great Tomb"), Helpers.CreateString(Main.IsekaiContext, "SovereignOfTheGreatTomb.Description", "Exclusive to the Overlord archetype. As absolute ruler of the supreme tomb and master of necrotic majesty, your necromantic authority is unmatched.\nBenefit: You emit a necrotic sovereign aura within 50 feet. Allies within the aura gain a +4 profane bonus to Strength, Dexterity, Constitution, Armor Class, attack rolls, and damage rolls, DR 10/Good, and Fast Healing 5. Furthermore, you personally gain complete immunity to negative energy, ability drain, energy drain, and death effects, and a +4 profane bonus to Intelligence and Charisma."), Icon_TombSovereign, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.SpellResistance = false;
				bp.AggroEnemies = false;
				bp.AffectEnemies = false;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(50f);
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
					{
						a.m_Buff = SovereignOfTheGreatTombBuff.ToReference<BlueprintBuffReference>();
						a.Permanent = true;
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff r)
					{
						r.m_Buff = SovereignOfTheGreatTombBuff.ToReference<BlueprintBuffReference>();
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = ActionFlow.DoNothing();
				});
			});
			blueprintFeature.AddComponent(delegate(AddEnergyDamageImmunity c)
			{
				c.EnergyType = DamageEnergyType.NegativeEnergy;
			});
			blueprintFeature.AddComponent(delegate(AddImmunityToAbilityScoreDamage c)
			{
				c.Drain = true;
			});
			blueprintFeature.AddComponent<AddImmunityToEnergyDrain>();
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
				c.m_Archetype = OverlordArchetype.GetReference();
				c.Level = 1;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
