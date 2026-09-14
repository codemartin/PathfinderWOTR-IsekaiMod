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
using Kingmaker.UnitLogic;
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
	internal class MandateOfHeaven
	{
		private static readonly Sprite Icon_Mandate = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_DIVINE.png");

		public static void Add()
		{
			BlueprintBuff MandateOfHeavenBuff = TTCoreExtensions.CreateBuff("MandateOfHeavenBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Imperial Grace of Heaven");
				bp.SetDescription(Main.IsekaiContext, "Under the sovereign mandate of heaven, you receive a +4 sacred bonus to Armor Class, attack rolls, damage rolls, and saving throws, Fast Healing 5, and immunity to fear and mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mandate;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
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
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear;
				});
			});
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("MandateOfHeaven", Helpers.CreateString(Main.IsekaiContext, "MandateOfHeaven.Name", "Overpowered Ability - Mandate of Heaven"), Helpers.CreateString(Main.IsekaiContext, "MandateOfHeaven.Description", "Exclusive to the God Emperor archetype. By divine cosmic decree, you radiate absolute imperial authority that uplifts all who serve beneath your banner.\nBenefit: You emit an aura within 50 feet. Allies inside the aura gain a +4 sacred bonus to Armor Class, attack rolls, damage rolls, and all saving throws, Fast Healing 5, and total immunity to fear and mind-affecting effects."), Icon_Mandate, delegate(BlueprintAbilityAreaEffect bp)
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
						a.m_Buff = MandateOfHeavenBuff.ToReference<BlueprintBuffReference>();
						a.Permanent = true;
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff r)
					{
						r.m_Buff = MandateOfHeavenBuff.ToReference<BlueprintBuffReference>();
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = ActionFlow.DoNothing();
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = GodEmperorArchetype.GetReference();
				c.Level = 1;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
