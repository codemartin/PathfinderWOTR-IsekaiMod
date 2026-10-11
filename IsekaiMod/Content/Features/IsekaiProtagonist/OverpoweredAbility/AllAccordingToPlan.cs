using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class AllAccordingToPlan
{
	private static readonly Sprite Icon_Mastermind = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("2c38da66e5a599347ac95b3294acbe00"))?.m_Icon;

	internal const int UsesPerDay = 3;

	public static void Add()
	{
		BlueprintAbilityResource resource = Helpers.CreateBlueprint(Main.IsekaiContext, "AllAccordingToPlanResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		BlueprintBuff buff = TTCoreExtensions.CreateBuff("AllAccordingToPlanOverpoweredBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Keikaku Dori (Everything According to Plan)");
			bp.SetDescription(Main.IsekaiContext, "Every enemy motion has been foreseen. For 1 round, your attack rolls gain a +10 insight bonus, critical threats are automatically confirmed, all your spells gain a +4 bonus to save DCs, and your weapon attacks ignore damage reduction.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Mastermind;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 10;
			});
			bp.AddComponent(delegate(CriticalConfirmationBonus c)
			{
				c.Bonus = 20;
				c.Value = 0;
			});
			bp.AddComponent<InitiatorCritAutoconfirm>();
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 4;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.SpellsOnly = true;
			});
			bp.AddComponent<IgnoreDamageReductionOnAttack>();
		});
		BlueprintAbility ability = Helpers.CreateBlueprint(Main.IsekaiContext, "AllAccordingToPlanOverpoweredAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "All According to Plan");
			bp.SetDescription(Main.IsekaiContext, "As a swift action (3/day), execute your overarching tactical scheme. For 1 round, your attacks gain a +10 insight bonus, critical threats automatically confirm, your spells gain a +4 DC bonus, and all your weapon attacks ignore damage reduction.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Mastermind;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.ActionType = UnitCommand.CommandType.Swift;
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
				{
					m_Buff = buff.ToReference<BlueprintBuffReference>(),
					DurationValue = Values.Duration.OneRound
				});
			});
		});
		OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "AllAccordingToPlanOverpoweredFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Overpowered Ability - All According to Plan");
			bp.SetDescription(Main.IsekaiContext, "Exclusive to the Mastermind archetype. You have mapped every possibility, trap, and adversary into an infallible grand calculation from step zero.\nBenefit: You gain a +4 insight bonus to Armor Class, attack rolls, damage rolls, and saving throws, a +8 insight bonus to Initiative, your weapon critical threat range increases by 1, and your spells ignore spell resistance. In addition, you gain the 'All According to Plan' swift action ability (3/day) to guarantee lethal tactical execution for 1 round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Mastermind;
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { ability.ToReference<BlueprintUnitFactReference>() };
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = resource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			StatType[] acAttackDamageSaves = PortedAuras.AcAttackDamageSaves;
			foreach (StatType stat in acAttackDamageSaves)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = stat;
					c.Value = 4;
				});
			}
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.Initiative;
				c.Value = 8;
			});
			bp.AddComponent(delegate(WeaponCriticalEdgeIncreaseStackable c)
			{
				c.Value = 1;
			});
			bp.AddComponent(delegate(IgnoreSpellResistanceForSpells c)
			{
				c.AllSpells = true;
			});
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "MastermindArchetype");
				c.Level = 1;
			});
		}));
	}
}
