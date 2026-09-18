using System;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class AbsoluteDefense
	{
		private static readonly Sprite Icon_AbsoluteDefense = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("62888999171921e4dafb46de83f4d67d"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource AbsoluteDefenseResource = Helpers.CreateBlueprint(Main.IsekaiContext, "AbsoluteDefenseResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 10,
					StartingIncrease = 1,
					LevelStep = 5,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Constitution
				};
				bp.m_UseMax = false;
			});
			BlueprintBuff AbsoluteDefenseBuff = TTCoreExtensions.CreateBuff("AbsoluteDefenseBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Defense");
				bp.SetDescription(Main.IsekaiContext, "You are shielded by an adamantine barrier. Grants Damage Reduction and energy resistance (20 at levels 1-9, 40 at levels 10-14, 75 at levels 15-20, and 100 at level 21+) and a shield bonus to Armor Class (+10 at levels 1-9, +15 at levels 10-14, and +20 at level 15+).");
				((BlueprintUnitFact)bp).m_Icon = Icon_AbsoluteDefense;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[4]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 20
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 40
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 20,
							ProgressionValue = 75
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 100
						}
					};
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				foreach (DamageEnergyType energyType in Enum.GetValues(typeof(DamageEnergyType)))
				{
					bp.AddComponent(delegate(AddDamageResistanceEnergy c)
					{
						c.Type = energyType;
						c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					});
				}
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 10
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 15
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 20
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
			});
			BlueprintAbility AbsoluteDefenseAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "AbsoluteDefenseAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Absolute Defense");
				bp.SetDescription(Main.IsekaiContext, "Channeling the ultimate defensive technique from another realm, you create an impenetrable barrier around yourself. \nBenefit: As a swift action, grant yourself scalable Damage Reduction and energy resistance (20 at levels 1-9, 40 at levels 10-14, 75 at levels 15-20, and 100 at level 21+) and shield AC (+10 at levels 1-9, +15 at levels 10-14, and +20 at level 15+). The barrier lasts 1 round at levels 1-9, 2 rounds at levels 10-14, 3 rounds at levels 15-20, and 4 rounds at level 21+. Usable 3 + Constitution modifier times per day (scaling up to 6 + Constitution modifier times per day by level 20).\nNote: Mutually exclusive with Full Counter (Calistrian Retribution).");
				((BlueprintUnitFact)bp).m_Icon = Icon_AbsoluteDefense;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRound;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[4]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 1
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 20,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 4
						}
					};
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = AbsoluteDefenseBuff.ToReference<BlueprintBuffReference>();
						b.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
						};
						b.Permanent = false;
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = AbsoluteDefenseResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AbsoluteDefenseFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Absolute Defense");
				bp.SetDescription(Main.IsekaiContext, "Like the Legendary Shield Hero or an adamantine bastion, your defenses cannot be breached by conventional attacks. \nBenefit: Gain the swift-action Absolute Defense ability, granting scalable DR, energy resistance, and shield AC for up to 4 rounds. Usable 3 + Constitution modifier times per day (scaling up to 6 + Constitution modifier times per day by level 20).\nNote: Mutually exclusive with Full Counter (Calistrian Retribution).");
				((BlueprintUnitFact)bp).m_Icon = Icon_AbsoluteDefense;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { AbsoluteDefenseAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = AbsoluteDefenseResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "FullCounterFeature");
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
