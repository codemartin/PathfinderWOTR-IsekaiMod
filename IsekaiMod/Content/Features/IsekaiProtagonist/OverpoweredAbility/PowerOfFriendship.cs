using IsekaiMod.Content.Classes.IsekaiProtagonist;
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
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class PowerOfFriendship
{
	private static readonly Sprite Icon_Friendship = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("90e59f4a4ada87243b7b3535a06d0638"))?.m_Icon;

	public static void Add()
	{
		BlueprintBuff buff = TTCoreExtensions.CreateBuff("PowerOfFriendshipBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Unbreakable Bond of Friendship");
			bp.SetDescription(Main.IsekaiContext, "Fueled by the power of friendship, you receive a +4 sacred bonus to Armor Class, attack rolls, damage rolls, and saving throws, an extra attack on a full attack, Fast Healing 10, and immunity to fatigue, exhaustion, and hostile fear and emotion effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Friendship;
			bp.IsClassFeature = true;
			StatType[] acAttackDamageSaves = PortedAuras.AcAttackDamageSaves;
			foreach (StatType stat in acAttackDamageSaves)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = stat;
					c.Value = 4;
				});
			}
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
			PortedImmunities.AddFearImmunity(bp);
			PortedImmunities.AddHostileImmunity(bp, SpellDescriptor.Emotion);
		});
		BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("PowerOfFriendship", Helpers.CreateString(Main.IsekaiContext, "PowerOfFriendship.Name", "Overpowered Ability - Power of Friendship"), Helpers.CreateString(Main.IsekaiContext, "PowerOfFriendship.Description", "Exclusive to the Hero archetype. Drawing upon the ultimate heroic anime trope, the bonds you forge with your companions empower everyone with miraculous might.\nBenefit: You emit an aura within 50 feet. Allies within the aura gain a +4 sacred bonus to Armor Class, attack rolls, damage rolls, and saving throws, one extra attack on a full attack, Fast Healing 10, and immunity to fatigue, exhaustion, and hostile fear and emotion effects."), Icon_Friendship, delegate(BlueprintAbilityAreaEffect area)
		{
			PortedAuras.ConfigureAllyAura(area, buff, 50);
		});
		blueprintFeature.AddComponent(delegate(PrerequisiteArchetypeLevel c)
		{
			c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
			c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "HeroArchetype");
			c.Level = 1;
		});
		OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
	}
}
