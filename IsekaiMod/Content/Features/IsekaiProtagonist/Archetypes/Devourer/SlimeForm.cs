using System.Collections.Generic;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
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
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;

internal class SlimeForm
{
	private static bool Added = false;

	private static BlueprintFeature ClassFeature;

	private static readonly Sprite Icon_Gelatinous = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("4e83f1e0e52b4613982b14ee2796928f"))?.m_Icon;

	private static readonly Sprite Icon_GelatinousSustain = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("2d46a6f1dbef51f4c9e14fc163ee4124"))?.m_Icon ?? Icon_Gelatinous;

	internal const string SmallOozePolymorphBuffGuid = "81bf22b0af505554f806b18822da197a";

	private static readonly BlueprintBuff SmallOozePolymorphBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("81bf22b0af505554f806b18822da197a");

	public static void Add()
	{
		if (Added)
		{
			return;
		}
		Added = true;
		BlueprintBuff SlimeBodyBuff = VisualPolymorph.CreateBodyBuff("SlimeFormBodyBuff", SmallOozePolymorphBuff);
		BlueprintBuff SlimeFormSizeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeFormSizeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Slime Form: Small Body");
			bp.SetDescription(Main.IsekaiContext, "Your slime body is Small.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Gelatinous;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
			bp.AddComponent(delegate(ChangeUnitSize c)
			{
				c.m_Type = ChangeUnitSize.ChangeType.Value;
				c.Size = Size.Small;
			});
		});
		BlueprintBuff SlimeFormBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeFormBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Slime Form");
			bp.SetDescription(Main.IsekaiContext, "In your true amorphous slime state, your fluid body lacks internal organs or fixed anatomy. You gain immunity to critical hits, precision damage, flanking, paralysis, sleep, poison, and disease. Your gelatinous composition provides a +4 dodge bonus to AC, acid immunity, and damage resistance.\nYour slime body is Small: you take up less space and gain the size bonus to AC and attack rolls, at a small penalty to combat maneuvers.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Gelatinous;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.Polymorph;
			});
			bp.AddComponent(delegate(AddFactContextActions c)
			{
				c.Activated = Helpers.CreateActionList(new ContextActionRemoveBuffsByDescriptor
				{
					SpellDescriptor = SpellDescriptor.Polymorph,
					NotSelf = true
				});
				c.Deactivated = Helpers.CreateActionList();
				c.NewRound = Helpers.CreateActionList();
			});
			List<BlueprintUnitFactReference> carried = new List<BlueprintUnitFactReference>();
			if (SlimeBodyBuff != null)
			{
				carried.Add(SlimeBodyBuff.ToReference<BlueprintUnitFactReference>());
			}
			carried.Add(SlimeFormSizeBuff.ToReference<BlueprintUnitFactReference>());
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = carried.ToArray();
			});
			bp.AddComponent<AddImmunityToCriticalHits>();
			bp.AddComponent<AddImmunityToPrecisionDamage>();
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Paralysis | SpellDescriptor.Sleep;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Paralysis | SpellDescriptor.Sleep;
			});
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked;
			});
			bp.AddComponent(delegate(AddEnergyImmunity c)
			{
				c.Type = DamageEnergyType.Acid;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
				c.m_Class = IsekaiAscendantClass.ClassLevelScalingClasses();
				c.m_Progression = ContextRankProgression.Custom;
				c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
				{
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 9,
						ProgressionValue = 5
					},
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 19,
						ProgressionValue = 10
					},
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 100,
						ProgressionValue = 15
					}
				};
			});
		});
		BlueprintAbility SlimeFormToggleAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeFormToggleAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Shift Form: Slime / Humanoid");
			bp.SetDescription(Main.IsekaiContext, "As a free action, you instantly shift between your humanoid guise and your true amorphous slime body.\nIn Slime Form, you gain complete immunity to critical hits, precision damage, flanking, paralysis, sleep, poison, and disease, plus a +4 dodge bonus to AC, acid immunity, and damage resistance scaling from DR 5/- up to DR 15/-.\nActivating this ability while in Slime Form reverts you to your humanoid shape.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Gelatinous;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.ActionType = UnitCommand.CommandType.Free;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new Conditional
				{
					ConditionsChecker = new ConditionsChecker
					{
						Conditions = new Condition[1]
						{
							new ContextConditionHasBuff
							{
								m_Buff = SlimeFormBuff.ToReference<BlueprintBuffReference>()
							}
						}
					},
					IfTrue = Helpers.CreateActionList(new ContextActionRemoveBuff
					{
						m_Buff = SlimeFormBuff.ToReference<BlueprintBuffReference>()
					}),
					IfFalse = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = SlimeFormBuff.ToReference<BlueprintBuffReference>(),
						Permanent = true,
						DurationValue = Values.Duration.Zero,
						AsChild = false
					})
				});
			});
		});
		BlueprintActivatableAbility SlimeFormAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeFormAbility", delegate(BlueprintActivatableAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Slime Form (Auto-Maintain)");
			bp.SetDescription(Main.IsekaiContext, "Continuously maintain your true amorphous slime body. In Slime Form, you gain complete immunity to critical hits, precision damage, flanking, paralysis, sleep, poison, and disease, plus a +4 dodge bonus to AC, acid immunity, and damage resistance scaling from DR 5/- up to DR 15/-.");
			((BlueprintUnitFact)bp).m_Icon = Icon_GelatinousSustain;
			bp.m_Buff = SlimeFormBuff.ToReference<BlueprintBuffReference>();
			bp.Group = ActivatableAbilityGroup.None;
			bp.WeightInGroup = 1;
			bp.IsOnByDefault = false;
			bp.DeactivateImmediately = true;
			bp.ActivationType = AbilityActivationType.Immediately;
		});
		ClassFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeFormFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "True Slime Reincarnation");
			bp.SetDescription(Main.IsekaiContext, "You were reincarnated into Golarion not as a standard mortal, but as a fluid slime entity. While you have learned to hold a stable humanoid shape, you can shift into your true Slime Form at will, gaining comprehensive ooze immunities, acid mastery, and fluid physical damage resistance.\n\nYou begin and travel in your humanoid shape. Use Shift Form: Slime / Humanoid, a free action, to change shape, or switch on Slime Form (Auto-Maintain) to stay in slime form.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Gelatinous;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					SlimeFormToggleAbility.ToReference<BlueprintUnitFactReference>(),
					SlimeFormAbility.ToReference<BlueprintUnitFactReference>()
				};
			});
		});
	}

	public static BlueprintFeature Get()
	{
		if (!Added)
		{
			Add();
		}
		return ClassFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeFormFeature");
	}
}
