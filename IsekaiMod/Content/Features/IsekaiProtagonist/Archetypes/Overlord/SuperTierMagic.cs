using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
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
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	internal class SuperTierMagic
	{
		private static readonly BlueprintUnit DarkYoungUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("143bf780bdf425a4ca14ab0e5df20232");

		private static readonly BlueprintSummonPool DarkYoungPool = BlueprintTools.GetBlueprint<BlueprintSummonPool>("d94c93e7240f10e41ae41db4c83d1cbe");

		private static readonly Sprite Icon_FallenDown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		private static readonly Sprite Icon_IaShubNiggurath = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("52b5df2a97df18242aec67610616ded0"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource SuperTierMagicResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SuperTierMagicResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintAbility FallenDownAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "FallenDownAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Super-Tier Magic: Fallen Down");
				bp.SetDescription(Main.IsekaiContext, "The Overlord's ultimate ritual incantation. Summons an apocalyptic pillar of radiant-unholy fire descending from the heavens.\nDeals 2d6 unholy damage and 2d6 fire damage per caster level to all enemies across a 50-foot radius. This damage ignores fire resistance and damage reduction (Reflex save for half damage).");
				((BlueprintUnitFact)bp).m_Icon = Icon_FallenDown;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Long;
				bp.CanTargetPoint = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = SuperTierMagicResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = new Feet(50f);
					c.m_TargetType = TargetType.Enemy;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
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
						IsAoE = true,
						HalfIfSaved = true
					}, new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Energy,
							Energy = DamageEnergyType.Fire
						},
						Duration = Values.Duration.Zero,
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
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
			BlueprintBuff DarkYoungBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "DarkYoungBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Dark Young: Eldritch Calamity");
				bp.SetDescription(Main.IsekaiContext, "The colossal offspring of Shub-Niggurath, summoned by the Overlord's soul sacrifice. Stomps across the battlefield dealing immense bludgeoning damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_IaShubNiggurath;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
			});
			BlueprintAbility IaShubNiggurathAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "IaShubNiggurathAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Super-Tier Magic: Ia Shub-Niggurath");
				bp.SetDescription(Main.IsekaiContext, "The infamous soul-harvesting ritual of the Black Goat of the Woods. Sacrifices all enemies across a 60-foot radius, instantly destroying non-boss creatures with fewer Hit Dice than your level (or dealing 20d6 unholy damage on a successful Fortitude save). From the pool of fallen souls, summons 1 colossal Lovecraftian Dark Young behemoth for 1 minute to trample the remaining enemy forces.");
				((BlueprintUnitFact)bp).m_Icon = Icon_IaShubNiggurath;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Long;
				bp.CanTargetPoint = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = SuperTierMagicResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = new Feet(60f);
					c.m_TargetType = TargetType.Enemy;
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
									DiceCountValue = 20,
									BonusValue = 0
								}
							}),
							Failed = Helpers.CreateActionList(new ContextActionKill())
						})
					}, new ContextActionSpawnMonster
					{
						m_Blueprint = DarkYoungUnit?.ToReference<BlueprintUnitReference>(),
						m_SummonPool = DarkYoungPool?.ToReference<BlueprintSummonPoolReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Minutes,
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = 1,
							m_IsExtendable = true
						},
						CountValue = Values.Dice.One,
						LevelValue = 0,
						AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
						{
							contextActionApplyBuff.Permanent = true;
							contextActionApplyBuff.m_Buff = DarkYoungBuff?.ToReference<BlueprintBuffReference>();
							contextActionApplyBuff.DurationValue = Values.Duration.Zero;
							contextActionApplyBuff.IsNotDispelable = true;
						})
					});
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "SuperTierMagicFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Super-Tier Magic");
				bp.SetDescription(Main.IsekaiContext, "At 20th level, the Overlord awakens the ultimate pinnacle of YGGDRASIL magic: Super-Tier Magic rituals. Once per day, you may cast either {g|Encyclopedia:Spell}Fallen Down{/g} or {g|Encyclopedia:Spell}Ia Shub-Niggurath{/g}.");
				((BlueprintUnitFact)bp).m_Icon = Icon_FallenDown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SuperTierMagicResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[2]
					{
						FallenDownAbility.ToReference<BlueprintUnitFactReference>(),
						IaShubNiggurathAbility.ToReference<BlueprintUnitFactReference>()
					};
				});
			});
		}
	}
}
