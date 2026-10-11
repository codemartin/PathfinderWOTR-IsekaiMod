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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;

internal class ShadowExtraction
{
	private static readonly BlueprintUnit ShadowSoldierUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("7237a32613fe55e479d1141682f2bbd4");

	private static readonly BlueprintUnitReference PreviousShadowSoldierUnit = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>("7121303d0f344a5abb3b43b0c9cef8e4");

	private static readonly Sprite Icon_Arise = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("caac251ca7601324bbe000372a0a1005"))?.m_Icon;

	public static void Add()
	{
		BlueprintBuff ShadowSoldierBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowSoldierBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Soldier: Extracted Essence");
			bp.SetDescription(Main.IsekaiContext, "A loyal warrior risen from the shadows of death. Deals additional unholy and cold damage on all attacks, gains a +10 ft speed bonus, and is immune to fear and mind-affecting effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Arise;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
		});
		BlueprintAbilityResource ShadowExtractionResource = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowExtractionResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = true,
				ResourceBonusStat = StatType.Charisma
			};
		});
		BlueprintAbility ShadowExtractionAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowExtractionAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Extraction: Arise");
			bp.SetDescription(Main.IsekaiContext, "The Shadow Monarch's supreme sovereign command: 'Arise!'\nExtracts the shadow of a fallen warrior, summoning a loyal Shadow Soldier to fight at your side for 1 minute per character level. Active shadows are capped at 1 soldier (increases to 2 at level 10, and 3 at level 20). Usable 3 + Charisma modifier times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Arise;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Medium;
			bp.CanTargetPoint = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend;
			bp.LocalizedDuration = StaticReferences.Strings.Duration.OneMinutePerLevel;
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = ShadowExtractionResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionEnforceShadowExtractionCap
				{
					m_SummonPool = ShadowMonarchSummonPool.GetReference(),
					m_LegacySummonPools = ShadowMonarchSummonPool.LegacyPools(),
					m_ShadowUnits = new BlueprintUnitReference[2]
					{
						ShadowSoldierUnit?.ToReference<BlueprintUnitReference>(),
						PreviousShadowSoldierUnit
					}
				}, new ContextActionOnNearbyPoint
				{
					Actions = Helpers.CreateActionList(new ContextActionSpawnMonster
					{
						m_Blueprint = ShadowSoldierUnit?.ToReference<BlueprintUnitReference>(),
						m_SummonPool = ShadowMonarchSummonPool.GetReference(),
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
						AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
						{
							contextActionApplyBuff.Permanent = true;
							contextActionApplyBuff.m_Buff = ShadowSoldierBuff?.ToReference<BlueprintBuffReference>();
							contextActionApplyBuff.DurationValue = Values.Duration.Zero;
							contextActionApplyBuff.IsNotDispelable = true;
						})
					})
				}, new ContextActionEnforceShadowExtractionCap
				{
					m_SummonPool = ShadowMonarchSummonPool.GetReference(),
					m_LegacySummonPools = ShadowMonarchSummonPool.LegacyPools(),
					m_ShadowUnits = new BlueprintUnitReference[2]
					{
						ShadowSoldierUnit?.ToReference<BlueprintUnitReference>(),
						PreviousShadowSoldierUnit
					},
					AfterSpawn = true
				});
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowExtractionFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Extraction: Arise");
			bp.SetDescription(Main.IsekaiContext, "At 1st level, the Shadow Monarch gains the ability to extract the souls of the fallen, commanding them: 'Arise'. Usable 3 + Charisma modifier times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Arise;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = ShadowExtractionResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { ShadowExtractionAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
