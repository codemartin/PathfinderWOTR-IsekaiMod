using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	internal class CreateDeathKnight
	{
		private static readonly BlueprintUnit CR19_CavalierPrestigeHellknight = BlueprintTools.GetBlueprint<BlueprintUnit>("a2c16111a4369be4b9e69ee80bfbe98b");

		private static readonly BlueprintSummonPool SummonMonsterPool = BlueprintTools.GetBlueprint<BlueprintSummonPool>("d94c93e7240f10e41ae41db4c83d1cbe");

		private static readonly BlueprintBuff SummonedCreatureSpawnMonsterVI_IX = BlueprintTools.GetBlueprint<BlueprintBuff>("0dff842f06edace43baf8a2f44207045");

		private static readonly Sprite Icon_DeathKnight = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("32881a5411648a449bf455fae21ea2b6"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource DeathKnightResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathKnightResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Charisma
				};
			});
			BlueprintSummonPool DeathKnightSummonPool = Helpers.CreateBlueprint<BlueprintSummonPool>(Main.IsekaiContext, "DeathKnightSummonPool", delegate
			{
			});
			BlueprintAbility CreateDeathKnightAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "CreateDeathKnightAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Create High Tier Undead: Death Knight");
				bp.SetDescription(Main.IsekaiContext, "As a standard action, the Overlord animates an unholy Death Knight to fight at their command. The Death Knight is a terrifying martial combatant equipped with full plate, unholy blade, and aura of fear. Active vanguard is capped at 1 Death Knight (3 + Charisma modifier uses per day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_DeathKnight;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.m_IsFullRoundAction = false;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = DeathKnightResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionClearSummonPool
					{
						m_SummonPool = DeathKnightSummonPool.ToReference<BlueprintSummonPoolReference>()
					}, new ContextActionOnNearbyPoint
					{
						Actions = Helpers.CreateActionList(new ContextActionSpawnMonster
						{
							m_Blueprint = CR19_CavalierPrestigeHellknight?.ToReference<BlueprintUnitReference>(),
							m_SummonPool = DeathKnightSummonPool.ToReference<BlueprintSummonPoolReference>(),
							DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Minutes,
								DiceType = DiceType.Zero,
								DiceCountValue = 0,
								BonusValue = Values.CreateContextRankValue(AbilityRankType.Default),
								m_IsExtendable = true
							},
							CountValue = Values.Dice.One,
							LevelValue = 0,
							AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff buffAction)
							{
								buffAction.Permanent = true;
								buffAction.m_Buff = SummonedCreatureSpawnMonsterVI_IX.ToReference<BlueprintBuffReference>();
								buffAction.DurationValue = Values.Duration.Zero;
								buffAction.IsNotDispelable = true;
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
			Helpers.CreateBlueprint(Main.IsekaiContext, "CreateDeathKnightFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Create High Tier Undead: Death Knight");
				bp.SetDescription(Main.IsekaiContext, "At 3rd level, the Overlord can summon a loyal Death Knight vanguard into battle, acting as an indomitable shield against enemy hordes. Active vanguard is capped at 1 Death Knight (3 + Charisma modifier uses per day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_DeathKnight;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = DeathKnightResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { CreateDeathKnightAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
