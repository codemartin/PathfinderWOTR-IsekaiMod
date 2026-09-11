using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	internal class DespairAura
	{
		private static readonly Sprite Icon_Aura = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff DespairAuraDebuff = Helpers.CreateBlueprint(Main.IsekaiContext, "DespairAuraDebuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Despair Aura");
				bp.SetDescription(Main.IsekaiContext, "The suffocating presence of the Supreme Being crushes enemy resolve. Enemies take a -2 penalty to saving throws against fear and negative energy, become Shaken (-2 penalty on attack rolls, saving throws, skill checks, and ability checks), and take 2d6 unholy damage each round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AC;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Shaken;
				});
				bp.AddComponent(delegate(AddFactContextActions c)
				{
					c.NewRound = Helpers.CreateActionList(new ContextActionDealDamage
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
							DiceCountValue = 2,
							BonusValue = 0
						}
					});
				});
			});
			BlueprintAbilityAreaEffect DespairAuraArea = Helpers.CreateBlueprint(Main.IsekaiContext, "DespairAuraArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
				bp.SpellResistance = false;
				bp.AggroEnemies = true;
				bp.AffectEnemies = true;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(30f);
				bp.Fx = new PrefabLink();
				bp.AddUnconditionalAuraEffect(DespairAuraDebuff.ToReference<BlueprintBuffReference>());
			});
			BlueprintBuff DespairAuraAreaBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "DespairAuraAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Despair Aura");
				bp.SetDescription(Main.IsekaiContext, "Emits a 30-foot aura of tyrannical dread, demoralizing and eroding all enemies.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = DespairAuraArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "DespairAuraFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Despair Aura (I - V)");
				bp.SetDescription(Main.IsekaiContext, "The signature passive presence of the Overlord. Enemies within 30 feet suffer suffocating dread: taking a -2 penalty to attack rolls, Armor Class, and Will saves, becoming Shaken, and taking 2d6 unholy damage each round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { DespairAuraAreaBuff.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
