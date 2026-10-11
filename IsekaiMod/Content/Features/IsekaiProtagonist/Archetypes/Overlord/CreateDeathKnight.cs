using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;

internal class CreateDeathKnight
{
	private static readonly BlueprintUnit DeathKnightUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("7237a32613fe55e479d1141682f2bbd4");

	private static readonly BlueprintBuff SummonedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0dff842f06edace43baf8a2f44207045");

	private static readonly Sprite Icon_DeathKnight = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("9b75cb3bd3108a24c81329a3734f2248"))?.m_Icon;

	public const int MaxDeathKnights = 2;

	public static void Add()
	{
		BlueprintSummonPool DeathKnightPool = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathKnightSummonPool", delegate(BlueprintSummonPool bp)
		{
			bp.Limit = 0;
			bp.DoNotRemoveDeadUnits = false;
		});
		BlueprintAbilityResource DeathKnightResource = Helpers.CreateBlueprint(Main.IsekaiContext, "CreateDeathKnightResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = DailyUses.Amount(1, StatType.Charisma);
		});
		BlueprintBuff DeathKnightWallBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathKnightWallBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Indomitable Wall");
			bp.SetDescription(Main.IsekaiContext, "The Death Knight is an immovable vanguard of the Supreme Being. Its heavy plate grants a +6 natural armor bonus.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DeathKnight;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 6;
			});
		});
		BlueprintAbility CreateDeathKnightAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "CreateDeathKnightAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Create High Tier Undead: Death Knight");
			bp.SetDescription(Main.IsekaiContext, "The Overlord's iconic frontline creation. Summons a heavy-armored Death Knight vanguard for 1 minute per character level. The Death Knight is an armored front-liner with a +6 natural armor bonus from {g|Encyclopedia:Spell}Indomitable Wall{/g}. Usable 1 + Charisma modifier times per day; at most 2 of your Death Knights stand at once, and a new one dismisses the oldest.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DeathKnight;
			bp.Type = AbilityType.SpellLike;
			bp.Range = AbilityRange.Close;
			bp.CanTargetPoint = true;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend;
			bp.LocalizedDuration = StaticReferences.Strings.Duration.OneMinutePerLevel;
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Necromancy;
			});
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.Summoning;
			});
			DailyUses.Limit(bp, DeathKnightResource);
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionOnNearbyPoint
				{
					Actions = Helpers.CreateActionList(new ContextActionSpawnMonster
					{
						m_Blueprint = DeathKnightUnit?.ToReference<BlueprintUnitReference>(),
						m_SummonPool = DeathKnightPool.ToReference<BlueprintSummonPoolReference>(),
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
						AfterSpawn = Helpers.CreateActionList(new ContextActionApplyBuff
						{
							Permanent = true,
							m_Buff = DeathKnightWallBuff.ToReference<BlueprintBuffReference>(),
							DurationValue = Values.Duration.Zero,
							IsNotDispelable = true
						}, new ContextActionApplyBuff
						{
							Permanent = true,
							m_Buff = SummonedBuff.ToReference<BlueprintBuffReference>(),
							DurationValue = Values.Duration.Zero,
							IsNotDispelable = true
						}, OverlordBenevolentFeatures.GrantGuardianBlessing())
					})
				}, new ContextActionEnforceShadowExtractionCap
				{
					m_SummonPool = DeathKnightPool.ToReference<BlueprintSummonPoolReference>(),
					m_ShadowUnits = new BlueprintUnitReference[1] { DeathKnightUnit.ToReference<BlueprintUnitReference>() },
					FixedCap = 2,
					AfterSpawn = true
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
			bp.SetDescription(Main.IsekaiContext, "At 3rd level, the Overlord can summon a loyal Death Knight vanguard into battle, 1 + Charisma modifier times per day, with at most 2 standing at once.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DeathKnight;
			bp.IsClassFeature = true;
			DailyUses.Grant(bp, DeathKnightResource);
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { CreateDeathKnightAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
