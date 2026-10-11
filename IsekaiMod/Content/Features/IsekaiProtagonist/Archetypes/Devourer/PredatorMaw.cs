using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;

internal class PredatorMaw
{
	private static readonly BlueprintItemWeapon BiteWeapon = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("dd43b44ac7a64dc1bcceb018c6630e4c");

	private static readonly Sprite Icon_Devour = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("3b646e1db3403b940bf620e01d2ce0c7"))?.m_Icon;

	public static void Add()
	{
		BlueprintBuff EssenceOfGluttonyBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "EssenceOfGluttonyBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Essence of the Slime");
			bp.SetDescription(Main.IsekaiContext, "Assimilated biological power gained from consuming prey as an apex Slime. Each stack grants a +1 untyped bonus to all ability scores, up to 5 stacks.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Devour;
			bp.IsClassFeature = true;
			bp.Ranks = 5;
			bp.Stacking = StackingType.Rank;
			StatType[] array = new StatType[6]
			{
				StatType.Strength,
				StatType.Dexterity,
				StatType.Constitution,
				StatType.Intelligence,
				StatType.Wisdom,
				StatType.Charisma
			};
			foreach (StatType statType in array)
			{
				StatType captured = statType;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = captured;
					c.Value = 1;
				});
			}
		});
		BlueprintAbilityResource DevourPreyResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DevourPreyResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = true,
				ResourceBonusStat = StatType.Constitution
			};
		});
		BlueprintAbility DevourPreyAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DevourPreyAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator: Devour Prey");
			bp.SetDescription(Main.IsekaiContext, "As a standard action, the Slime unhinges their amorphous body and consumes an adjacent weakened enemy whose Hit Dice do not exceed your character level. The prey must be weakened: below 30 + 10 per character level remaining hit points, or already down to a quarter of its own maximum hit points. Instantly executes the target, heals the Slime for twice their character level, and grants 1 stack of {g|Encyclopedia:Spell}Essence of the Slime{/g} (+1 untyped bonus to all ability scores, max 5 stacks). This is a death effect, and creatures immune to death effects are unaffected. Usable 3 + Constitution modifier times per day. Every devour also sends the Slime into Predator Frenzy for 1 minute, and devouring a demon, dragon, undead, animal or magical beast, construct or spellcaster adds that kind's Species Surge for 1 minute.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Devour;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Touch;
			bp.CanTargetEnemies = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = DevourPreyResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
			bp.AddComponent(delegate(AbilityTargetHPBelowThresholdScaling c)
			{
				c.BaseValue = 30;
				c.PerLevel = 10;
				c.PercentOfMaximum = 25;
			});
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
			bp.AddComponent(delegate(AbilityTargetMaximumHitDice c)
			{
				c.UseContextInstead = true;
				c.ContextHitDice = Values.CreateContextCasterPropertyValue(UnitProperty.Level);
			});
			bp.AddContextRankConfig(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.DamageBonus;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.MultiplyByModifier;
				c.m_StepLevel = 2;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionKill(), new ContextActionOnContextCaster
				{
					Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = EssenceOfGluttonyBuff.ToReference<BlueprintBuffReference>(),
						Permanent = true,
						DurationValue = Values.Duration.Zero,
						AsChild = false
					}, new ContextActionHealTarget
					{
						Value = new ContextDiceValue
						{
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = Values.CreateContextRankValue(AbilityRankType.DamageBonus)
						}
					})
				});
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorMawFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Slime Maw");
			bp.SetDescription(Main.IsekaiContext, "The Slime manifests a primary natural bite attack (1d8 damage with bleed). Additionally, you gain the active ability {g|Encyclopedia:Spell}Devour Prey{/g}, allowing you to consume weakened enemies to gain permanent attribute assimilation. Usable 3 + Constitution modifier times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Devour;
			bp.IsClassFeature = true;
			if (BiteWeapon != null)
			{
				bp.AddComponent(delegate(AddAdditionalLimb c)
				{
					c.m_Weapon = BiteWeapon.ToReference<BlueprintItemWeaponReference>();
				});
			}
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = DevourPreyResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { DevourPreyAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
