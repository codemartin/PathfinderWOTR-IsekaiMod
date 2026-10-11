using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;

internal class BeelzebubLordOfDevourers
{
	private static readonly Sprite Icon_Beelzebub = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("b974af13e45639a41a04843ce1c9aa12"))?.m_Icon;

	public static void Add()
	{
		BlueprintAbilityResource BeelzebubResource = Helpers.CreateBlueprint(Main.IsekaiContext, "BeelzebubResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		BlueprintAbility BeelzebubAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "BeelzebubAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Void Maw: Beelzebub");
			bp.SetDescription(Main.IsekaiContext, "Three times per day as a standard action, unleash the ultimate black hole stomach of Beelzebub at a point within close range, engulfing every enemy in a 30-foot radius. Enemies with no more Hit Dice than your character level must make a Fortitude save or be swallowed whole into the void and instantly destroyed. Enemies that succeed on the save, or that are too mighty to swallow, take 20d6 unholy damage instead. This is a death effect, and creatures immune to death effects are never destroyed by it. Once per casting, however many enemies are caught, the Slime heals 100 hit points.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Beelzebub;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Close;
			bp.CanTargetPoint = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = BeelzebubResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
			bp.AddComponent(delegate(AbilityTargetsAround c)
			{
				c.m_Radius = new Feet(30f);
				c.m_TargetType = TargetType.Enemy;
			});
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
			bp.AddContextRankConfig(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
			});
			bp.AddComponent(delegate(ContextCalculateSharedValue c)
			{
				c.ValueType = AbilitySharedValue.StatBonus;
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
				};
				c.Modifier = 1.0;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionSavingThrow
				{
					Type = SavingThrowType.Fortitude,
					Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
					{
						Succeed = Helpers.CreateActionList(VoidMawDamage()),
						Failed = Helpers.CreateActionList(new Conditional
						{
							ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionHitDice h)
							{
								h.HitDice = 0;
								h.AddSharedValue = true;
								h.SharedValue = AbilitySharedValue.StatBonus;
							}),
							IfTrue = ActionFlow.DoSingle<ContextActionKill>(),
							IfFalse = Helpers.CreateActionList(VoidMawDamage())
						})
					})
				});
			});
			bp.AddComponent(delegate(AbilityExecuteActionOnCast c)
			{
				c.Conditions = ActionFlow.EmptyCondition();
				c.Actions = Helpers.CreateActionList(new ContextActionOnContextCaster
				{
					Actions = Helpers.CreateActionList(new ContextActionHealTarget
					{
						Value = new ContextDiceValue
						{
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = 100
						}
					})
				});
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "BeelzebubLordOfDevourersFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Beelzebub: Lord of Devourers");
			bp.SetDescription(Main.IsekaiContext, "At 20th level, the Slime awakens as the ultimate apex predator of the multiverse. You gain Damage Reduction 15/-, a +4 size bonus to Constitution, and gain the active ability {g|Encyclopedia:Spell}Void Maw: Beelzebub{/g} three times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Beelzebub;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 15;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Size;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = BeelzebubResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { BeelzebubAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}

	private static ContextActionDealDamage VoidMawDamage()
	{
		return new ContextActionDealDamage
		{
			DamageType = new DamageTypeDescription
			{
				Type = DamageType.Energy,
				Energy = DamageEnergyType.Unholy
			},
			Duration = Values.Duration.Zero,
			Value = new ContextDiceValue
			{
				DiceType = DiceType.D6,
				DiceCountValue = 20,
				BonusValue = 0
			},
			IsAoE = true
		};
	}
}
