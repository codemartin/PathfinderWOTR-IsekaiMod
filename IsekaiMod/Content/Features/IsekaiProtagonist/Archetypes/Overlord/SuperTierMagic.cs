using IsekaiMod.Components;
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
using Kingmaker.UnitLogic.Buffs.Blueprints;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;

internal class SuperTierMagic
{
	private static readonly BlueprintUnit DarkYoungUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("143bf780bdf425a4ca14ab0e5df20232");

	private static readonly Sprite Icon_FallenDown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("5e36df08c71748f7936bce310181fb71"))?.m_Icon;

	private static readonly Sprite Icon_IaShubNiggurath = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("954f1469ed62843409783c9fa7472998"))?.m_Icon;

	private static readonly BlueprintAbility Fireball = BlueprintTools.GetBlueprint<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");

	public static void Add()
	{
		BlueprintSummonPool DarkYoungSummonPool = Helpers.CreateBlueprint(Main.IsekaiContext, "DarkYoungSummonPool", delegate(BlueprintSummonPool bp)
		{
			bp.Limit = 0;
			bp.DoNotRemoveDeadUnits = false;
		});
		BlueprintAbilityResource SuperTierMagicResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SuperTierMagicResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		BlueprintAbility FallenDownAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "FallenDownAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Super-Tier Magic: Fallen Down");
			bp.SetDescription(Main.IsekaiContext, "The Overlord's ultimate ritual incantation. Summons an apocalyptic pillar of radiant-unholy fire descending from the heavens.\nDeals 1d6 unholy damage and 1d6 fire damage per character level to all enemies across a 50-foot radius (Reflex half, DC 19 + Charisma modifier).");
			((BlueprintUnitFact)bp).m_Icon = Icon_FallenDown;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Long;
			bp.CanTargetPoint = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.LocalizedSavingThrow = Fireball?.LocalizedSavingThrow;
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
			bp.AddComponent(delegate(ContextCalculateAbilityParams c)
			{
				c.StatType = StatType.Charisma;
				c.ReplaceCasterLevel = false;
				c.ReplaceSpellLevel = false;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.SavingThrowType = SavingThrowType.Reflex;
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
			bp.SetDescription(Main.IsekaiContext, "The infamous soul-harvesting ritual of the Black Goat of the Woods. Sacrifices all enemies across a 60-foot radius: each must make a Fortitude save or be instantly destroyed if its Hit Dice do not exceed your character level, and those that succeed or have more Hit Dice take 20d6 unholy damage instead. This is a death effect, and creatures immune to death effects are never destroyed by it. From the pool of fallen souls, summons one colossal Lovecraftian Dark Young behemoth for 1 minute, once per casting and never more than one at a time, to trample the remaining enemy forces.");
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
						Succeed = Helpers.CreateActionList(DarkHarvestDamage()),
						Failed = Helpers.CreateActionList(new Conditional
						{
							ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionHitDice h)
							{
								h.HitDice = 0;
								h.AddSharedValue = true;
								h.SharedValue = AbilitySharedValue.StatBonus;
							}),
							IfTrue = ActionFlow.DoSingle<ContextActionKill>(),
							IfFalse = Helpers.CreateActionList(DarkHarvestDamage())
						})
					})
				});
			});
			bp.AddComponent(delegate(AbilityExecuteActionOnCast c)
			{
				c.Conditions = ActionFlow.EmptyCondition();
				c.Actions = Helpers.CreateActionList(new ContextActionSpawnMonster
				{
					m_Blueprint = DarkYoungUnit?.ToReference<BlueprintUnitReference>(),
					m_SummonPool = DarkYoungSummonPool.ToReference<BlueprintSummonPoolReference>(),
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
					AfterSpawn = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						Permanent = true,
						m_Buff = DarkYoungBuff?.ToReference<BlueprintBuffReference>(),
						DurationValue = Values.Duration.Zero,
						IsNotDispelable = true
					}, OverlordBenevolentFeatures.GrantGuardianBlessing())
				}, new ContextActionEnforceShadowExtractionCap
				{
					m_SummonPool = DarkYoungSummonPool.ToReference<BlueprintSummonPoolReference>(),
					m_ShadowUnits = new BlueprintUnitReference[1] { DarkYoungUnit.ToReference<BlueprintUnitReference>() },
					FixedCap = 1,
					AfterSpawn = true
				});
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "SuperTierMagicFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Super-Tier Magic");
			bp.SetDescription(Main.IsekaiContext, "At 20th level, the Overlord awakens the ultimate pinnacle of YGGDRASIL magic: Super-Tier Magic rituals. Three times per day, you may cast either {g|Encyclopedia:Spell}Fallen Down{/g} or {g|Encyclopedia:Spell}Ia Shub-Niggurath{/g}.");
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

	private static ContextActionDealDamage DarkHarvestDamage()
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
