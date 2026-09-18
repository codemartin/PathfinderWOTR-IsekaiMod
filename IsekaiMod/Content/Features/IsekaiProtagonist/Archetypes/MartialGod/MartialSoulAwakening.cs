using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.RuleSystem.Rules.Damage;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod
{
	internal static class MartialSoulAwakening
	{
		private static bool Added = false;

		private static readonly Sprite Icon_InnerSoul = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("b89373001e05f1f4aa9b9bb4f420c40f"))?.m_Icon;

		private static readonly Sprite Icon_Surge = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd"))?.m_Icon;

		public static BlueprintFeature Stage1 { get; private set; }

		public static BlueprintFeature Stage2 { get; private set; }

		public static BlueprintFeature Stage3 { get; private set; }

		public static BlueprintFeature Stage4 { get; private set; }

		public static BlueprintFeature Stage5 { get; private set; }

		public static BlueprintFeature Stage6 { get; private set; }

		public static BlueprintFeature Stage7 { get; private set; }

		public static void Add()
		{
			if (Added)
			{
				return;
			}
			Added = true;
			Stage1 = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningStage1", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Inner Soul Awakening I: Iron Focus");
				bp.SetDescription(Main.IsekaiContext, "Awakening the foundational resonance of their soul, the Martial God gains a +1 enhancement bonus on all attack and damage rolls with manufactured weapons, natural attacks, and unarmed strikes, as well as a +2 dodge bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_InnerSoul;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			Stage2 = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningStage2", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Inner Soul Awakening II: Flow State");
				bp.SetDescription(Main.IsekaiContext, "Entering an effortless combat trance, the Martial God gains an additional attack at their highest base attack bonus when making a full attack. Furthermore, all their attacks bypass Damage Reduction as magic.");
				((BlueprintUnitFact)bp).m_Icon = Icon_InnerSoul;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
			Stage3 = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningStage3", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Inner Soul Awakening III: Domain of the Fist and Blade");
				bp.SetDescription(Main.IsekaiContext, "The Martial God's spirit extends into a lethal perimeter around their body. Their critical threat range with all weapons and unarmed strikes increases by 1 (which stacks with other critical range modifiers), and all attacks bypass Damage Reduction as adamantine.");
				((BlueprintUnitFact)bp).m_Icon = Icon_InnerSoul;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			BlueprintBuff SurgeBuff = TTCoreExtensions.CreateBuff("MartialSoulAwakeningSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Limit Breaker Surge");
				bp.SetDescription(Main.IsekaiContext, "Temporarily unlocking all spiritual limiters, you gain a +4 enhancement bonus to Strength and Dexterity, and +30 ft movement speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Surge;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Speed;
					c.Value = 30;
				});
			});
			BlueprintAbilityResource SurgeResource = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningSurgeResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Charisma
				};
			});
			BlueprintAbility SurgeAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningSurgeAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Limit Breaker Combat Surge");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, temporarily shatter all bodily limiters for 1 minute. You gain +4 enhancement bonus to Strength and Dexterity, and +30 ft movement speed (uses per day equal to 3 + Charisma modifier).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Surge;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.CanTargetSelf = true;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneMinute;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = SurgeResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = SurgeBuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.DurationValue = Values.Duration.OneMinute;
					});
				});
			});
			Stage4 = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningStage4", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Inner Soul Awakening IV: Divine Combat Ascendance");
				bp.SetDescription(Main.IsekaiContext, "Attaining absolute divine combat mastery, all your attacks deal an additional 2d6 force damage and you gain a +4 dodge bonus to AC. Furthermore, you can unleash a Limit Breaker Combat Surge as a swift action.");
				((BlueprintUnitFact)bp).m_Icon = Icon_InnerSoul;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SurgeResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SurgeAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			Stage5 = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningStage5", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Inner Soul Awakening V: Transcendent Flow");
				bp.SetDescription(Main.IsekaiContext, "Your spirit flows seamlessly through the fabric of combat. You gain a +2 enhancement bonus on attack and damage rolls, an additional attack at your highest BAB when making a full attack, and all your attacks bypass Damage Reduction as epic.");
				((BlueprintUnitFact)bp).m_Icon = Icon_InnerSoul;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
			});
			Stage6 = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningStage6", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Inner Soul Awakening VI: Spirit Severing Strike");
				bp.SetDescription(Main.IsekaiContext, "Your attacks sever spiritual threads and rend reality. You gain an additional 2d6 force damage on all attacks, a +4 dodge bonus to AC, and a +4 enhancement bonus to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_InnerSoul;
				bp.IsClassFeature = true;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
			});
			Stage7 = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialSoulAwakeningStage7", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Inner Soul Awakening VII: Apex Transcendent Body");
				bp.SetDescription(Main.IsekaiContext, "Reaching the absolute pinnacle of martial physiology, you gain permanent immunity to paralysis, trip, disarm, and stun, and your base land speed increases by +30 feet.");
				((BlueprintUnitFact)bp).m_Icon = Icon_InnerSoul;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 30;
					c.Descriptor = ModifierDescriptor.Enhancement;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Stun | SpellDescriptor.Paralysis;
				});
			});
		}
	}
}
