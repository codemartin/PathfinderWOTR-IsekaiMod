using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class ImperialSovereignty
	{
		private static readonly Sprite Icon_Aura = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff ImperialStandardBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialStandardBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Imperial Standard");
				bp.SetDescription(Main.IsekaiContext, "The radiant presence of the God Emperor inspires divine devotion. Allies receive a +3 Sacred bonus to attack rolls, weapon damage, Armor Class, and saving throws, and are immune to fear and shaken effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 3;
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
			});
			BlueprintAbilityAreaEffect ImperialStandardArea = Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialStandardArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.SpellResistance = false;
				bp.AggroEnemies = false;
				bp.AffectEnemies = false;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(40f);
				bp.Fx = new PrefabLink();
				bp.AddUnconditionalAuraEffect(ImperialStandardBuff.ToReference<BlueprintBuffReference>());
			});
			BlueprintBuff ImperialStandardAreaBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialStandardAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Imperial Standard Aura");
				bp.SetDescription(Main.IsekaiContext, "Emits a 40-foot aura that grants allies a +3 Sacred bonus to attack, damage, AC, saves, and immunity to fear.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = ImperialStandardArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialSovereigntyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Imperial Sovereignty & Armies");
				bp.SetDescription(Main.IsekaiContext, "As an otherworldly sovereign, the God Emperor commands imperial authority over military forces and crusader legions:\n• +20 Permanent Morale to all Crusade armies.\n• +25% Weekly recruit growth across all military units.\n• All Crusade military squads receive enchanted weapons (+15% damage and magical damage bypass).\n• Military squads gain mental discipline, making them immune to fear and confusion.\n• Emits a 40-foot Imperial Standard Aura granting all companions, allies, and summons a +3 Sacred bonus to attack, damage, AC, saves, and immunity to fear and shaken effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.AddComponent<ImperialSovereigntyComponent>();
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ImperialStandardAreaBuff.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
