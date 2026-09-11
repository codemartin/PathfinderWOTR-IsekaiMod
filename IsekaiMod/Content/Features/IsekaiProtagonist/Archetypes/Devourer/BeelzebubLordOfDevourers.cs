using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	internal class BeelzebubLordOfDevourers
	{
		private static readonly Sprite Icon_Beelzebub = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource BeelzebubResource = Helpers.CreateBlueprint(Main.IsekaiContext, "BeelzebubResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintAbility BeelzebubAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "BeelzebubAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Void Maw: Beelzebub");
				bp.SetDescription(Main.IsekaiContext, "Once per day as a standard action, unleash the ultimate black hole stomach of Beelzebub in a 30-foot cone. All non-boss enemies must make a Fortitude save or be swallowed whole into the void and instantly destroyed. Enemies that succeed on the save take 20d6 unholy damage. The Slime heals for 50% of their maximum hit points.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Beelzebub;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = BeelzebubResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = new Feet(30f);
					c.m_TargetType = TargetType.Enemy;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionSavingThrow
					{
						Type = SavingThrowType.Fortitude,
						Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
						{
							Succeed = Helpers.CreateActionList(new ContextActionDealDamage
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
								}
							}),
							Failed = Helpers.CreateActionList(new ContextActionKill())
						})
					}, new ContextActionOnContextCaster
					{
						Actions = Helpers.CreateActionList(new ContextActionHealTarget
						{
							Value = new ContextDiceValue
							{
								DiceType = DiceType.Zero,
								DiceCountValue = 0,
								BonusValue = 100
							}
						})
					});
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "BeelzebubLordOfDevourersFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Beelzebub: Lord of Devourers");
				bp.SetDescription(Main.IsekaiContext, "At 20th level, the Slime awakens as the ultimate apex predator of the multiverse. You gain Damage Reduction 15/-, a +4 size bonus to Constitution, and gain the active ability {g|Encyclopedia:Spell}Void Maw: Beelzebub{/g} once per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Beelzebub;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 15;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = BeelzebubResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { BeelzebubAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
