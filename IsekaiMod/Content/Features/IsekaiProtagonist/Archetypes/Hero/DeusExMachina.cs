using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;

internal class DeusExMachina
{
	public const int KillHitDiceCeiling = 20;

	public const int KillSaveDC = 30;

	private static ContextActionDealDamage SmiteDamage()
	{
		return new ContextActionDealDamage
		{
			DamageType = new DamageTypeDescription
			{
				Type = DamageType.Energy,
				Energy = DamageEnergyType.Divine
			},
			Duration = Values.Duration.Zero,
			Value = new ContextDiceValue
			{
				DiceType = DiceType.D6,
				DiceCountValue = 20,
				BonusValue = 0
			},
			IsAoE = true
		};
	}

	public static void Add()
	{
		LocalizedString DeusExMachinaDescription = Helpers.CreateString(Main.IsekaiContext, "DeusExMachina.Description", "Your channel energy, cure spells, and restore HP abilities heal the maximum amount.\nOnce per day, when your {g|Encyclopedia:HP}HP{/g} drops to 0, you are resurrected to full health. Allies within 40 feet are then restored to full health. Enemies within 40 feet with 20 Hit Dice or fewer that are not immune to death effects must make a DC 30 Fortitude save or die; every other enemy, and any that save, takes 20d6 divine damage.");
		Sprite Icon_DeusExMachina = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_DEUS_EX_MACHINA.png");
		BlueprintAbilityAreaEffect DeusExMachinaArea = Helpers.CreateBlueprint(Main.IsekaiContext, "DeusExMachinaArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Any;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = true;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(40f);
			bp.Fx = new PrefabLink
			{
				AssetId = "c152c5cb0af124a40bc94087f9e2bb29"
			};
			bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
			{
				c.UnitEnter = ActionFlow.DoSingle(delegate(Conditional conditional)
				{
					conditional.ConditionsChecker = ActionFlow.IfSingle<ContextConditionIsEnemy>();
					conditional.IfTrue = ActionFlow.DoSingle(delegate(Conditional guard)
					{
						guard.ConditionsChecker = ActionFlow.IfSingle<ContextConditionImmuneToDeathEffect>();
						guard.IfTrue = Helpers.CreateActionList(SmiteDamage());
						guard.IfFalse = ActionFlow.DoSingle(delegate(Conditional ceiling)
						{
							ceiling.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionHitDice h)
							{
								h.HitDice = 20;
								h.AddSharedValue = false;
							});
							ceiling.IfFalse = Helpers.CreateActionList(SmiteDamage());
							ceiling.IfTrue = ActionFlow.DoSingle(delegate(ContextActionSavingThrow save)
							{
								save.Type = SavingThrowType.Fortitude;
								save.HasCustomDC = true;
								save.CustomDC = 30;
								save.Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved saved)
								{
									saved.Succeed = Helpers.CreateActionList(SmiteDamage());
									saved.Failed = ActionFlow.DoSingle<ContextActionKill>();
								});
							});
						});
					});
					conditional.IfFalse = ActionFlow.DoSingle(delegate(ContextActionHealTarget contextActionHealTarget)
					{
						contextActionHealTarget.Value = new ContextDiceValue
						{
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = Values.CreateContextTargetPropertyValue(UnitProperty.MaxHP)
						};
					});
				});
				c.UnitExit = ActionFlow.DoNothing();
				c.UnitMove = ActionFlow.DoNothing();
				c.Round = ActionFlow.DoNothing();
			});
		});
		BlueprintBuff DeusExMachinaBuff = TTCoreExtensions.CreateBuff("DeusExMachinaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Deus Ex Machina");
			bp.SetDescription(DeusExMachinaDescription);
			((BlueprintUnitFact)bp).m_Icon = Icon_DeusExMachina;
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(DeathActions c)
			{
				c.CheckResource = false;
				c.Actions = Helpers.CreateActionList(new ContextActionResurrect
				{
					FullRestore = true
				}, new ContextActionSpawnFx
				{
					PrefabLink = new PrefabLink
					{
						AssetId = "749ad3759dc93d64dba70a84d48135b5"
					}
				}, new ContextActionSpawnAreaEffect
				{
					m_AreaEffect = DeusExMachinaArea.ToReference<BlueprintAbilityAreaEffectReference>(),
					DurationValue = Values.Duration.OneRound
				}, new ContextActionRemoveSelf());
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "DeusExMachinaFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Deus Ex Machina");
			bp.SetDescription(DeusExMachinaDescription);
			((BlueprintUnitFact)bp).m_Icon = Icon_DeusExMachina;
			bp.AddComponent(delegate(AddRestTrigger c)
			{
				c.Action = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
				{
					contextActionApplyBuff.m_Buff = DeusExMachinaBuff.ToReference<BlueprintBuffReference>();
					contextActionApplyBuff.Permanent = true;
					contextActionApplyBuff.IsFromSpell = false;
				});
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { DeusExMachinaBuff.ToReference<BlueprintUnitFactReference>() };
				c.DoNotRestoreMissingFacts = true;
			});
			bp.AddComponent(delegate(AutoMetamagic c)
			{
				c.m_AllowedAbilities = AutoMetamagic.AllowedType.Any;
				c.Metamagic = Metamagic.Maximize;
				c.Descriptor = SpellDescriptor.Cure | SpellDescriptor.RestoreHP | SpellDescriptor.ChannelPositiveHeal | SpellDescriptor.ChannelNegativeHeal | SpellDescriptor.ChannelPositiveHarm | SpellDescriptor.ChannelNegativeHarm;
			});
		});
	}
}
