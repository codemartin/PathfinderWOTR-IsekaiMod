using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;

internal class GraspHeart
{
	private static readonly Sprite Icon_GraspHeart = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("a89dcbbab8f40e44e920cc60636097cf"))?.m_Icon;

	private static readonly BlueprintBuff StunnedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("09d39b38bb7c6014394b6daced9bacd3");

	public static void Add()
	{
		BlueprintAbilityResource GraspHeartResource = Helpers.CreateBlueprint(Main.IsekaiContext, "GraspHeartResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = true,
				ResourceBonusStat = StatType.Charisma
			};
		});
		BlueprintAbility GraspHeartAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "GraspHeartAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Grasp Heart");
			bp.SetDescription(Main.IsekaiContext, "The Overlord's signature 9th-tier spell. Clenches the target's heart in an invisible fist of pure negative energy.\nThe target must make a Fortitude save. On a failed save it takes 1d6 unholy damage per character level and is stunned for 1 round.\nOn a successful save it takes half damage and is not stunned. This is a death effect: creatures immune to death effects are unaffected.");
			((BlueprintUnitFact)bp).m_Icon = Icon_GraspHeart;
			bp.Type = AbilityType.SpellLike;
			bp.Range = AbilityRange.Medium;
			bp.CanTargetEnemies = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			bp.SpellResistance = true;
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Necromancy;
			});
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.Evil | SpellDescriptor.Death;
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = GraspHeartResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionSavingThrow
				{
					Type = SavingThrowType.Fortitude,
					Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
					{
						Succeed = Helpers.CreateActionList(new ContextActionDealDamage
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
								DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
								BonusValue = 0
							},
							HalfIfSaved = true
						}),
						Failed = Helpers.CreateActionList(new ContextActionDealDamage
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
								DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
								BonusValue = 0
							}
						}, new ContextActionApplyBuff
						{
							m_Buff = StunnedBuff?.ToReference<BlueprintBuffReference>(),
							Permanent = false,
							DurationValue = Values.Duration.OneRound,
							IsNotDispelable = false
						})
					})
				});
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "GraspHeartFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Grasp Heart");
			bp.SetDescription(Main.IsekaiContext, "At 1st level, the Overlord commands the iconic necromantic spell Grasp Heart. Crushes the target's heart, dealing 1d6 unholy damage per character level and stunning it for 1 round on a failed Fortitude save. A successful save halves the damage and avoids the stun.");
			((BlueprintUnitFact)bp).m_Icon = Icon_GraspHeart;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = GraspHeartResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { GraspHeartAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
