using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero
{
	internal class RetributionMiracle
	{
		private static readonly Sprite Icon_Miracle = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff HeroicInvulnerabilityBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "HeroicInvulnerabilityBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Hero's Retribution: Aegis");
				bp.SetDescription(Main.IsekaiContext, "Shielded by the Hero's selfless sacrifice, this ally is temporarily invulnerable to physical damage and negative conditions for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Miracle;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 100;
				});
			});
			BlueprintBuff HeroRetributionBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "HeroRetributionBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Hero's Awakening: Righteous Fury");
				bp.SetDescription(Main.IsekaiContext, "Fueled by the vow to protect companions, the Hero deals guaranteed critical hits and maximum damage on weapon attacks for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Miracle;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 10;
				});
			});
			BlueprintAbilityResource RetributionMiracleResource = Helpers.CreateBlueprint(Main.IsekaiContext, "RetributionMiracleResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintAbility RetributionMiracleAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "RetributionMiracleAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Heroic Miracle: Retribution");
				bp.SetDescription(Main.IsekaiContext, "Once per day as a swift action, the Hero rushes to protect a wounded companion. Bestows temporary invulnerability on the targeted ally for 1 round, and enters an awakened state granting +6 Sacred attack and +10 Sacred damage for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Miracle;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetFriends = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = RetributionMiracleResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = HeroicInvulnerabilityBuff.ToReference<BlueprintBuffReference>(),
						Permanent = false,
						DurationValue = Values.Duration.OneRound,
						IsNotDispelable = true
					}, new ContextActionApplyBuff
					{
						m_Buff = HeroRetributionBuff.ToReference<BlueprintBuffReference>(),
						Permanent = false,
						DurationValue = Values.Duration.OneRound,
						IsNotDispelable = true,
						ToCaster = true
					});
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "RetributionMiracleFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Heroic Miracle: Retribution");
				bp.SetDescription(Main.IsekaiContext, "At 12th level, the Hero's resolve reaches mythic proportions. Once per day as a swift action, you can shield a companion with complete invulnerability while entering an awakened state of righteous fury.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Miracle;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = RetributionMiracleResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { RetributionMiracleAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
