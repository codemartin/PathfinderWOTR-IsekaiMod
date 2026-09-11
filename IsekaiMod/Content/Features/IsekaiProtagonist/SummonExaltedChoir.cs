using System;
using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class SummonExaltedChoir
	{
		private static readonly BlueprintUnit CR20_SuccubusAdvancedFighter = BlueprintTools.GetBlueprint<BlueprintUnit>("2db556136eac2544fa9744314c2a5713");

		private static readonly BlueprintUnit CR14_AstralDeva = BlueprintTools.GetBlueprint<BlueprintUnit>("8f3bd0ecea704277a9f2b09296a7b01e");

		private static readonly BlueprintUnit CR22_ErinyesDevilStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("b576f3eb0aa94af44a985f51eda9db7b");

		private static readonly BlueprintUnit ShadowSoldierUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("7121303d0f344a5abb3b43b0c9cef8e4");

		private static readonly BlueprintUnit HamadryadQueenUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("b8972cfe36e3cd945bbd2c4c320d5237") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("0cc7a2526e4557945b1d8eb277d1fb3a");

		private static readonly BlueprintSummonPool SummonMonsterPool = BlueprintTools.GetBlueprint<BlueprintSummonPool>("d94c93e7240f10e41ae41db4c83d1cbe");

		private static readonly BlueprintBuff SummonedCreatureSpawnMonsterVI_IX = BlueprintTools.GetBlueprint<BlueprintBuff>("0dff842f06edace43baf8a2f44207045");

		private static readonly Sprite Icon_SummonMonsterIX = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("52b5df2a97df18242aec67610616ded0")).m_Icon;

		public static BlueprintAbilityResource SummonPlanarVanguardResource;

		public static void Add()
		{
			SummonPlanarVanguardResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonPlanarVanguardResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 2,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintAbility SummonExaltedChoirAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonExaltedChoirAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Summon Exalted Choir");
				bp.SetSummonDescription(Main.IsekaiContext, "This spell summons an Astral Deva and an Erinyes vanguard to aid you in battle.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionOnNearbyPoint contextActionOnNearbyPoint)
					{
						contextActionOnNearbyPoint.Actions = Helpers.CreateActionList(SpawnMonster(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
						{
							contextActionSpawnMonster.m_Blueprint = CR14_AstralDeva.ToReference<BlueprintUnitReference>();
						}), SpawnMonster(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
						{
							contextActionSpawnMonster.m_Blueprint = CR22_ErinyesDevilStandard.ToReference<BlueprintUnitReference>();
						}));
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Conjuration;
				});
				bp.AddComponent(delegate(SpellDescriptorComponent c)
				{
					c.Descriptor = SpellDescriptor.Summoning;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_SummonMonsterIX;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.m_IsFullRoundAction = true;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = SummonPlanarVanguardResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "SummonHaremFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Exalted Choir");
				bp.SetDescription(Main.IsekaiContext, "As a full-round action, summon celestial and planar vanguards to turn the tide of battle (2 uses per day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_SummonMonsterIX;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SummonPlanarVanguardResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SummonExaltedChoirAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			BlueprintSummonPool customPool = Helpers.CreateBlueprint<BlueprintSummonPool>(Main.IsekaiContext, "TempestFaeSummonPool", delegate
			{
			});
			CreateApexLieutenant("SummonImperialSeraphim", "Summon Imperial Seraphim", "Summons a radiant Imperial Seraphim celestial lieutenant to fight alongside the God Emperor (2 uses per day).", CR14_AstralDeva);
			CreateApexLieutenant("SummonGreatTombVanguard", "Summon Great Tomb Vanguard", "Summons a Dread Tomb Vanguard lieutenant to crush the enemies of the Overlord (2 uses per day).", CR22_ErinyesDevilStandard);
			CreateApexLieutenant("SummonShadowVanguard", "Summon Shadow Commander", "Summons an Apex Shadow Commander lieutenant from the sovereign monarch's domain (2 uses per day).", ShadowSoldierUnit);
			CreateApexLieutenant("SummonTempestVanguard", "Summon Tempest Fae Sovereign", "Summons a majestic Tempest Fae Sovereign spirit from the primordial forest to aid the Slime in battle (2 uses per day). The Fae Sovereign casts nature enchantments, heals allies, and commands the winds.", HamadryadQueenUnit, customPool);
			CreateApexLieutenant("SummonFellowshipOfHeroes", "Summon Fellowship Paragon", "Summons a legendary Paragon of Fellowship to fight by the Hero's side (2 uses per day).", CR14_AstralDeva);
			CreateApexLieutenant("SummonClockworkMatrix", "Summon Clockwork Matrix", "Summons an advanced Clockwork Matrix Golem lieutenant to execute tactical engagements (2 uses per day).", CR22_ErinyesDevilStandard);
			CreateApexLieutenant("SummonPhantomRetinue", "Summon Phantom Vanguard", "Summons an Astral Phantom Blade-Spirit lieutenant to dance across the battlefield (2 uses per day).", CR14_AstralDeva);
		}

		private static void CreateApexLieutenant(string prefix, string name, string description, BlueprintUnit unit, BlueprintSummonPool customPool = null)
		{
			BlueprintSummonPoolReference poolRef = (customPool ?? SummonMonsterPool).ToReference<BlueprintSummonPoolReference>();
			BlueprintAbility ability = Helpers.CreateBlueprint(Main.IsekaiContext, prefix + "Ability", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, name);
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintUnitFact)bp).m_Icon = Icon_SummonMonsterIX;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.m_IsFullRoundAction = false;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = SummonPlanarVanguardResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					List<GameAction> list = new List<GameAction>();
					if (customPool != null)
					{
						list.Add(new ContextActionClearSummonPool
						{
							m_SummonPool = poolRef
						});
					}
					list.Add(new ContextActionOnNearbyPoint
					{
						Actions = Helpers.CreateActionList(SpawnMonster(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
						{
							contextActionSpawnMonster.m_Blueprint = unit.ToReference<BlueprintUnitReference>();
							contextActionSpawnMonster.m_SummonPool = poolRef;
						}))
					});
					c.Actions = Helpers.CreateActionList(list.ToArray());
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Conjuration;
				});
				bp.AddComponent(delegate(SpellDescriptorComponent c)
				{
					c.Descriptor = SpellDescriptor.Summoning;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, prefix + "Feature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, name);
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintUnitFact)bp).m_Icon = Icon_SummonMonsterIX;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SummonPlanarVanguardResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ability.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}

		private static ContextActionSpawnMonster SpawnMonster(Action<ContextActionSpawnMonster> init = null)
		{
			ContextActionSpawnMonster contextActionSpawnMonster = new ContextActionSpawnMonster
			{
				m_SummonPool = SummonMonsterPool.ToReference<BlueprintSummonPoolReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Rounds,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = Values.CreateContextRankValue(AbilityRankType.Default),
					m_IsExtendable = true
				},
				CountValue = Values.Dice.One,
				LevelValue = 0,
				AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff c)
				{
					c.Permanent = true;
					c.m_Buff = SummonedCreatureSpawnMonsterVI_IX.ToReference<BlueprintBuffReference>();
					c.DurationValue = Values.Duration.Zero;
					c.IsNotDispelable = true;
				})
			};
			init?.Invoke(contextActionSpawnMonster);
			return contextActionSpawnMonster;
		}
	}
}
