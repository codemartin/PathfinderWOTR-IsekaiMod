using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class DomainExpansion
	{
		private static readonly Sprite Icon_Domain = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("dd3dacafcf40a0145a5824c838e2698d"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0a5ddfbcfb3989543ac7c936fc256889"))?.m_Icon;

		public static BlueprintFeature DomainExpansionFeature;

		public static void Add()
		{
			BlueprintBuff AllyBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "DomainExpansionAllyBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Domain: Sovereign Dominion");
				bp.SetDescription(Main.IsekaiContext, "While inside the Domain of the Infinite Singularity, reality bends to your sovereign will. Allies gain a +4 sacred bonus to attack rolls, weapon damage rolls, critical confirmation rolls, and spell save DCs.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Domain;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Bonus = 4;
					c.Value = 0; // the component reads Value unconditionally, so a missing one throws on every attack roll
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.Sacred;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.SeeInvisibility;
				});
			});
			BlueprintBuff EnemyBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "DomainExpansionEnemyBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Domain: Crushing Singularity");
				bp.SetDescription(Main.IsekaiContext, "Inside the domain, gravitational and conceptual force crushes your soul. Enemies suffer a -4 penalty to all saving throws, cannot benefit from concealment or invisibility, and suffer continuous force damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Domain;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveFortitude;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveReflex;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -4;
				});
				bp.AddComponent<DoNotBenefitFromConcealment>();
			});
			DomainExpansionFeature = TTCoreExtensions.CreateToggleAuraFeature("DomainExpansion", Helpers.CreateString(Main.IsekaiContext, "DomainExpansionFeature.Name", "Special Power: Domain Expansion - Infinite Singularity"), Helpers.CreateString(Main.IsekaiContext, "DomainExpansionFeature.Desc", "You impose the inner landscape of your soul onto the fabric of reality itself, creating a 30-foot conceptual domain.\nAllies within your domain gain a +4 sacred bonus to attack rolls, damage rolls, critical confirmation, and all spell save DCs.\nEnemies within your domain suffer a -4 penalty to all saving throws, lose all benefits of concealment, and suffer 2d6 pure force damage every round."), Icon_Domain, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Any;
				bp.SpellResistance = false;
				bp.AggroEnemies = true;
				bp.AffectEnemies = true;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(30f);
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = ActionFlow.DoSingle(delegate(Conditional cond)
					{
						cond.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionIsAlly chk)
						{
							chk.Not = false;
						});
						cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
						{
							b.m_Buff = AllyBuff.ToReference<BlueprintBuffReference>();
							b.Permanent = true;
							b.DurationValue = Values.Duration.Zero;
							b.AsChild = true;
						});
						cond.IfFalse = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
						{
							b.m_Buff = EnemyBuff.ToReference<BlueprintBuffReference>();
							b.Permanent = true;
							b.DurationValue = Values.Duration.Zero;
							b.AsChild = true;
						});
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(Conditional cond)
					{
						cond.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionIsAlly chk)
						{
							chk.Not = false;
						});
						cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff b)
						{
							b.m_Buff = AllyBuff.ToReference<BlueprintBuffReference>();
						});
						cond.IfFalse = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff b)
						{
							b.m_Buff = EnemyBuff.ToReference<BlueprintBuffReference>();
						});
					});
					c.Round = ActionFlow.DoSingle(delegate(Conditional cond)
					{
						cond.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionIsAlly chk)
						{
							chk.Not = false;
						});
						cond.IfTrue = ActionFlow.DoNothing();
						cond.IfFalse = ActionFlow.DoSingle(delegate(ContextActionDealDamage dmg)
						{
							dmg.DamageType = new DamageTypeDescription
							{
								Type = DamageType.Force,
								Common = new DamageTypeDescription.CommomData(),
								Physical = new DamageTypeDescription.PhysicalData()
							};
							dmg.Duration = Values.Duration.Zero;
							dmg.Value = new ContextDiceValue
							{
								DiceType = DiceType.D6,
								DiceCountValue = 2,
								BonusValue = 0
							};
							dmg.IsAoE = true;
						});
					});
				});
			});
			DomainExpansionFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 13;
			});
			SpecialPowerSelection.AddToMagicSelection(DomainExpansionFeature);
		}
	}
}
