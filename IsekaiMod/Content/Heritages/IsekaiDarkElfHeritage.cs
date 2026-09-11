using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
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

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiDarkElfHeritage
	{
		private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

		private static readonly BlueprintBuff Unconsious = BlueprintTools.GetBlueprint<BlueprintBuff>("31a468926d0f3ab439b714f15d794a8b");

		private static readonly Sprite Icon_AcidBomb = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("fd101fbc4aacf5d48b76a65e3aa5db6d")).m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource DrowPoisonResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DrowPoisonResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 0,
					IncreasedByLevel = false,
					LevelIncrease = 0,
					IncreasedByLevelStartPlusDivStep = false,
					StartingLevel = 0,
					StartingIncrease = 0,
					LevelStep = 0,
					PerStepIncrease = 0,
					MinClassLevelIncrease = 0,
					OtherClassesModifier = 0f,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Intelligence
				};
			});
			BlueprintUnitProperty DrowPoisonUnitProperty = Helpers.CreateBlueprint(Main.IsekaiContext, "DrowPoisonUnitProperty", delegate(BlueprintUnitProperty bp)
			{
				bp.name = "DrowPoisonUnitProperty";
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.Level;
				});
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.StatBonusIntelligence;
				});
				bp.BaseValue = 10;
				bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
			});
			BlueprintBuff DrowPoisonBuff = TTCoreExtensions.CreateBuff("DrowPoisonBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Drow Poison");
				bp.SetDescription(Main.IsekaiContext, "Drow Poison causes their target to become unconsious on a failed fortitude save.");
				((BlueprintUnitFact)bp).m_Icon = Icon_AcidBomb;
				bp.IsClassFeature = true;
				bp.Stacking = StackingType.Replace;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
				{
					c.OnlyHit = true;
					c.Action = Helpers.CreateActionList(new ContextActionSavingThrow
					{
						Type = SavingThrowType.Fortitude,
						m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
						HasCustomDC = true,
						CustomDC = Values.CreateContextCasterCustomPropertyValue(DrowPoisonUnitProperty),
						FromBuff = false,
						Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved contextActionConditionalSaved)
						{
							contextActionConditionalSaved.Succeed = ActionFlow.DoNothing();
							contextActionConditionalSaved.Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
							{
								contextActionApplyBuff.m_Buff = Unconsious.ToReference<BlueprintBuffReference>();
								contextActionApplyBuff.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Minutes,
									m_IsExtendable = true,
									DiceType = DiceType.Zero,
									DiceCountValue = 0,
									BonusValue = 1
								};
							});
						})
					}, new ContextActionRemoveSelf());
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.DC = Values.CreateContextCasterCustomPropertyValue(DrowPoisonUnitProperty);
				});
			});
			BlueprintAbility DrowPoisonAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DrowPoisonAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Drow Poison");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, you can coat your weapon with a special drow poison. Enemies hit by the poisoned weapon will need to make a Fortitude save or become unconscious for 1 minute. This fortitude save is equal to 10 + your character level + your Intelligence modifier.");
				((BlueprintUnitFact)bp).m_Icon = Icon_AcidBomb;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.SavingThrowType = SavingThrowType.Fortitude;
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = DrowPoisonBuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.Permanent = true;
						contextActionApplyBuff.DurationValue = Values.Duration.Zero;
					});
				});
				bp.AddComponent(delegate(AbilitySpawnFx c)
				{
					c.PrefabLink = new PrefabLink
					{
						AssetId = "8de64fbe047abc243a9b4715f643739f"
					};
					c.Time = AbilitySpawnFxTime.OnApplyEffect;
					c.Anchor = AbilitySpawnFxAnchor.Caster;
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.DC = Values.CreateContextCasterCustomPropertyValue(DrowPoisonUnitProperty);
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = DrowPoisonResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
				});
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AvailableMetamagic = Metamagic.Heighten;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneMinute;
				bp.LocalizedSavingThrow = StaticReferences.Strings.SavingThrow.FortitudeNegates;
			});
			Sprite Icon_Dark_Elf = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_DARK_ELF.png");
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiDarkElfHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Dark Elf");
				bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a Dark Elf have both extreme beauty and power. They are a cruel and cunning dark reflection of the elven race.\nThe Isekai Dark Elf has a +4 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Intelligence}Intelligence{/g}, a +2 racial bonus to {g|Encyclopedia:Dexterity}Dexterity{/g} and {g|Encyclopedia:Wisdom}Wisdom{/g}, and a -2 penalty to Constitution. They have spell resistance equal to 10 + their character level. They can also use the Drow Poison ability as a swift action a number of times per day equal to their Intelligence modifier.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark_Elf;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Wisdom;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonusIfHasFact c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = -2;
					c.InvertCondition = true;
					c.m_CheckedFacts = new BlueprintUnitFactReference[1] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 10;
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = DrowPoisonResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { DrowPoisonAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.Groups = new FeatureGroup[0];
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.ElvenHeritageSelection.AddToSelection(feature);
		}
	}
}
