using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
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
	internal class PowerOfFriendship
	{
		private static readonly Sprite Icon_Friendship = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("90e59f4a4ada87243b7b3535a06d0638"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff PowerOfFriendshipBuff = TTCoreExtensions.CreateBuff("PowerOfFriendshipBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Unbreakable Bond of Friendship");
				bp.SetDescription(Main.IsekaiContext, "Fueled by the power of friendship, you receive a +4 sacred bonus to Armor Class, attack rolls, damage rolls, and saving throws, an extra attack on a full attack, Fast Healing 10, and immunity to fatigue, exhaustion, and fear.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Friendship;
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
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = false;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
					c.Bonus = 0;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Fatigued;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Exhausted;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Emotion;
				});
			});
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("PowerOfFriendship", Helpers.CreateString(Main.IsekaiContext, "PowerOfFriendship.Name", "Overpowered Ability - Power of Friendship"), Helpers.CreateString(Main.IsekaiContext, "PowerOfFriendship.Description", "Exclusive to the Hero archetype. Drawing upon the ultimate heroic anime trope, the bonds you forge with your companions empower everyone with miraculous might.\nBenefit: You emit an aura within 50 feet. Allies within the aura gain a +4 sacred bonus to Armor Class, attack rolls, damage rolls, and saving throws, one extra attack per round, Fast Healing 10, and immunity to fatigue, exhaustion, and fear effects."), Icon_Friendship, delegate(BlueprintAbilityAreaEffect bp)
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
						a.m_Buff = PowerOfFriendshipBuff.ToReference<BlueprintBuffReference>();
						a.Permanent = true;
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff r)
					{
						r.m_Buff = PowerOfFriendshipBuff.ToReference<BlueprintBuffReference>();
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = ActionFlow.DoNothing();
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = HeroArchetype.GetReference();
				c.Level = 1;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
