using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class ImperialWorship
	{
		private static readonly Sprite Icon_Bless = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("90e59f4a4ada87243b7b3535a06d0638"))?.m_Icon;

		private static readonly Sprite Icon_HolySmite = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		private static readonly Sprite Icon_Heal = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ff8f1534f66559c478448723e16b6624"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource ImperialDevotionResource = Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialDevotionResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = true,
					LevelIncrease = 1,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Wisdom
				};
			});
			BlueprintAbility DivineInterventionAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineInterventionAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Miracle: Divine Intervention");
				bp.SetDescription(Main.IsekaiContext, "Expends 1 Devotion point to heal a target ally for 10 hit points per character level, removing all negative conditions, curses, and poisons.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Heal;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetFriends = true;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ImperialDevotionResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionHealTarget
					{
						Value = new ContextDiceValue
						{
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
						}
					}, new ContextActionDispelMagic
					{
						m_BuffType = ContextActionDispelMagic.BuffType.All,
						m_CheckType = RuleDispelMagic.CheckType.CasterLevel,
						m_StopAfterCountRemoved = true,
						m_CountToRemove = 1,
						OnlyTargetEnemyBuffs = false
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.MultiplyByModifier;
					c.m_StepLevel = 10;
				});
			});
			BlueprintBuff DeityMandateBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "DeityMandateBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Living Deity's Mandate");
				bp.SetDescription(Main.IsekaiContext, "Allies receive a +4 Sacred bonus to attack rolls, Armor Class, and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Bless;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
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
			});
			BlueprintAbility DeityMandateAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DeityMandateAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Miracle: Living Deity's Mandate");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, expends 1 Devotion point to proclaim imperial authority, granting all allies within 40 feet a +4 Sacred bonus to attack, AC, and saving throws for 3 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Bless;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetFriends = true;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ImperialDevotionResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = new Feet(40f);
					c.m_TargetType = TargetType.Ally;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = DeityMandateBuff.ToReference<BlueprintBuffReference>(),
						Permanent = false,
						DurationValue = Values.Duration.ThreeRounds,
						IsNotDispelable = true
					});
				});
			});
			BlueprintAbility WrathOfHeavenAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "WrathOfHeavenAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Miracle: Wrath of Heaven");
				bp.SetDescription(Main.IsekaiContext, "Expends 1 Devotion point to unleash a 30-foot burst of divine retribution, dealing 1d8 radiant holy damage per character level to all enemies (Reflex half).");
				((BlueprintUnitFact)bp).m_Icon = Icon_HolySmite;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Medium;
				bp.CanTargetPoint = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ImperialDevotionResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = new Feet(30f);
					c.m_TargetType = TargetType.Enemy;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Energy,
							Energy = DamageEnergyType.Holy
						},
						Duration = Values.Duration.Zero,
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D8,
							DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
							BonusValue = 0
						},
						IsAoE = true,
						HalfIfSaved = true
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialWorshipFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Imperial Worship: Living Deity");
				bp.SetDescription(Main.IsekaiContext, "As you walk the path towards godhood, mortals revere you as a living deity. You gain a Devotion Pool equal to 3 + your character level + your Wisdom modifier, allowing you to invoke divine miracles in battle: {g|Encyclopedia:Spell}Divine Intervention{/g}, {g|Encyclopedia:Spell}Living Deity's Mandate{/g}, and {g|Encyclopedia:Spell}Wrath of Heaven{/g}.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Bless;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = ImperialDevotionResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[3]
					{
						DivineInterventionAbility.ToReference<BlueprintUnitFactReference>(),
						DeityMandateAbility.ToReference<BlueprintUnitFactReference>(),
						WrathOfHeavenAbility.ToReference<BlueprintUnitFactReference>()
					};
				});
			});
		}
	}
}
