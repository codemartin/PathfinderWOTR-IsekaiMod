using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	internal class SupremeBuffRoutine
	{
		private static readonly Sprite Icon_PreBuff = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("90e59f4a4ada87243b7b3535a06d0638"))?.m_Icon;

		private static readonly BlueprintBuff TrueSeeingBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("09b4b69169304474296484c74aa12027");

		public static void Add()
		{
			BlueprintBuff bp = Helpers.CreateBlueprint(Main.IsekaiContext, "SupremeOverlordWardsBuff", delegate(BlueprintBuff blueprintBuff)
			{
				blueprintBuff.SetName(Main.IsekaiContext, "Absolute Sovereign Wards (Tier 1)");
				blueprintBuff.SetDescription(Main.IsekaiContext, "The Overlord's defensive pre-buff sequence:\n• +4 Armor bonus to AC\n• +4 Shield bonus to AC\n• +30 Temporary Hit Points\n• Immunity to death effects and negative energy\n• Resistance 20 to Acid, Cold, Electricity, and Fire.");
				((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_PreBuff;
				blueprintBuff.IsClassFeature = true;
				blueprintBuff.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Armor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				blueprintBuff.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				blueprintBuff.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
				{
					c.Value = 30;
				});
				blueprintBuff.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.NegativeEnergy;
				});
				blueprintBuff.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				blueprintBuff.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 20;
				});
				blueprintBuff.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 20;
				});
				blueprintBuff.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 20;
				});
				blueprintBuff.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 20;
				});
			});
			BlueprintBuff bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "SupremeOverlordWardsBuffT2", delegate(BlueprintBuff blueprintBuff)
			{
				blueprintBuff.SetName(Main.IsekaiContext, "Absolute Sovereign Wards (Tier 2)");
				blueprintBuff.SetDescription(Main.IsekaiContext, "The Overlord's advanced defensive pre-buff sequence:\n• +6 Armor bonus to AC\n• +6 Shield bonus to AC\n• +60 Temporary Hit Points\n• Immunity to death effects and negative energy\n• Resistance 30 to Acid, Cold, Electricity, and Fire\n• True Seeing (bypasses all concealment, blur, and invisibility).");
				((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_PreBuff;
				blueprintBuff.IsClassFeature = true;
				blueprintBuff.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Armor;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				blueprintBuff.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				blueprintBuff.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
				{
					c.Value = 60;
				});
				blueprintBuff.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.NegativeEnergy;
				});
				blueprintBuff.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				blueprintBuff.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 30;
				});
				blueprintBuff.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 30;
				});
				blueprintBuff.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 30;
				});
				blueprintBuff.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 30;
				});
				if (TrueSeeingBuff != null)
				{
					blueprintBuff.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { TrueSeeingBuff.ToReference<BlueprintUnitFactReference>() };
					});
				}
			});
			BlueprintBuff bp3 = Helpers.CreateBlueprint(Main.IsekaiContext, "SupremeOverlordWardsBuffT3", delegate(BlueprintBuff blueprintBuff)
			{
				blueprintBuff.SetName(Main.IsekaiContext, "Absolute Sovereign Wards (Apotheosis)");
				blueprintBuff.SetDescription(Main.IsekaiContext, "The Overlord's ultimate apotheosis pre-buff sequence:\n• +8 Armor bonus to AC\n• +8 Shield bonus to AC\n• +100 Temporary Hit Points\n• Complete immunity to Acid, Cold, Electricity, Fire, and Negative Energy\n• Complete immunity to death effects\n• True Seeing (bypasses all concealment, blur, and invisibility).");
				((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_PreBuff;
				blueprintBuff.IsClassFeature = true;
				blueprintBuff.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Armor;
					c.Stat = StatType.AC;
					c.Value = 8;
				});
				blueprintBuff.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 8;
				});
				blueprintBuff.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
				{
					c.Value = 100;
				});
				blueprintBuff.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.NegativeEnergy;
				});
				blueprintBuff.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.Fire;
				});
				blueprintBuff.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.Cold;
				});
				blueprintBuff.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.Acid;
				});
				blueprintBuff.AddComponent(delegate(AddEnergyDamageImmunity c)
				{
					c.EnergyType = DamageEnergyType.Electricity;
				});
				blueprintBuff.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				if (TrueSeeingBuff != null)
				{
					blueprintBuff.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { TrueSeeingBuff.ToReference<BlueprintUnitFactReference>() };
					});
				}
			});
			ContextActionApplyBuff contextActionApplyBuff = new ContextActionApplyBuff
			{
				m_Buff = bp.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Hours,
					DiceType = DiceType.Zero,
					BonusValue = 2
				},
				IsNotDispelable = true
			};
			ContextActionApplyBuff contextActionApplyBuff2 = new ContextActionApplyBuff
			{
				m_Buff = bp2.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Hours,
					DiceType = DiceType.Zero,
					BonusValue = 8
				},
				IsNotDispelable = true
			};
			ContextActionApplyBuff contextActionApplyBuff3 = new ContextActionApplyBuff
			{
				m_Buff = bp3.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Hours,
					DiceType = DiceType.Zero,
					BonusValue = 24
				},
				IsNotDispelable = true
			};
			Conditional conditional = new Conditional();
			conditional.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterLevel c)
			{
				c.MinLevel = 13;
			});
			conditional.IfTrue = new ActionList
			{
				Actions = new GameAction[1] { contextActionApplyBuff2 }
			};
			conditional.IfFalse = new ActionList
			{
				Actions = new GameAction[1] { contextActionApplyBuff }
			};
			Conditional conditional2 = conditional;
			Conditional RootScalingAction = new Conditional
			{
				ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterLevel c)
				{
					c.MinLevel = 17;
				}),
				IfTrue = new ActionList
				{
					Actions = new GameAction[1] { contextActionApplyBuff3 }
				},
				IfFalse = new ActionList
				{
					Actions = new GameAction[1] { conditional2 }
				}
			};
			BlueprintAbility SupremeBuffRoutineAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SupremeBuffRoutineAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Supreme Pre-Buff Routine");
				blueprintAbility.SetDescription(Main.IsekaiContext, "As a standard action, the Overlord unleashes their supreme defensive pre-buff sequence in a single dramatic declaration, instantly shrouding themselves in absolute defensive wards. Scales across three tiers at levels 7, 13, and 17, with duration expanding from 2 hours to 8 hours and ultimately 24 hours.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_PreBuff;
				blueprintAbility.Type = AbilityType.SpellLike;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				blueprintAbility.LocalizedDuration = StaticReferences.Strings.Duration.OneHour;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(RootScalingAction);
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "SupremeBuffRoutineFeature", delegate(BlueprintFeature blueprintFeature)
			{
				blueprintFeature.SetName(Main.IsekaiContext, "Supreme Pre-Buff Routine");
				blueprintFeature.SetDescription(Main.IsekaiContext, "At 7th level, the Overlord gains the iconic ability to cast their entire suite of defensive wards in a single action, scaling in potency and duration as they ascend.");
				((BlueprintUnitFact)blueprintFeature).m_Icon = Icon_PreBuff;
				blueprintFeature.IsClassFeature = true;
				blueprintFeature.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SupremeBuffRoutineAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
