using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class SecondaryAwakening
	{
		private static readonly Sprite Icon_Awakening = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("27203d62eb3d4184c9aced94f22e1806"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0a5ddfbcfb3989543ac7c936fc256889"))?.m_Icon;

		public static BlueprintFeature SecondaryAwakeningFeature;

		public static void Add()
		{
			BlueprintAbilityResource Resource = Helpers.CreateBlueprint(Main.IsekaiContext, "SecondaryAwakeningResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 15,
					StartingIncrease = 1,
					LevelStep = 99,
					PerStepIncrease = 0,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 2;
			});
			BlueprintBuff Buff = Helpers.CreateBlueprint(Main.IsekaiContext, "SecondaryAwakeningBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Secondary Awakening");
				bp.SetDescription(Main.IsekaiContext, "Your soul has shattered its mortal limiters. For 5 rounds, you gain a +6 inherent bonus to all six ability scores, +20 feet base speed, one additional attack at full base attack bonus during full attacks, and total immunity to fatigue, exhaustion, and mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Awakening;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.Speed;
					c.Value = 20;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Fatigued;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Exhausted;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting;
				});
			});
			BlueprintAbility Ability = Helpers.CreateBlueprint(Main.IsekaiContext, "SecondaryAwakeningAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power: Secondary Awakening");
				bp.SetDescription(Main.IsekaiContext, "Trigger an emergency soul awakening as a swift action. For 5 rounds, you gain a +6 inherent bonus to all ability scores, +20 feet base speed, an extra attack at full BAB during full attacks, and total immunity to fatigue, exhaustion, and mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Awakening;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.LocalizedDuration = Helpers.CreateString(Main.IsekaiContext, "SecondaryAwakeningAbility.Duration", "5 rounds");
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = Buff.ToReference<BlueprintBuffReference>();
						b.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = 5
						};
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = Resource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			SecondaryAwakeningFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SecondaryAwakeningFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power: Secondary Awakening");
				bp.SetDescription(Main.IsekaiContext, "You possess a secondary awakening reserve that can be tapped during desperate battles.\nBenefit: As a swift action, you can activate Secondary Awakening once per day (twice per day at level 15). For 5 rounds, you gain a +6 inherent bonus to all six ability scores, +20 ft movement speed, an extra attack at full BAB, and immunity to fatigue, exhaustion, and mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Awakening;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { Ability.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = Resource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
			SecondaryAwakeningFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 7;
			});
			SpecialPowerSelection.AddToSelection(SecondaryAwakeningFeature);
		}
	}
}
