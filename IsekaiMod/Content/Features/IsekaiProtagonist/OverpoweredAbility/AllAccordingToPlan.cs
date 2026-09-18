using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
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
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class AllAccordingToPlan
	{
		private static readonly Sprite Icon_Mastermind = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("90e59f4a4ada87243b7b3535a06d0638"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource AllAccordingToPlanResource = Helpers.CreateBlueprint(Main.IsekaiContext, "AllAccordingToPlanResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintBuff AllAccordingToPlanBuff = TTCoreExtensions.CreateBuff("AllAccordingToPlanBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Keikaku Dori (Everything According to Plan)");
				bp.SetDescription(Main.IsekaiContext, "Every enemy motion has been foreseen. For 1 round, your attack rolls gain an insight bonus (+5 at levels 1--9, +8 at levels 10--14, and +10 at level 15+), critical threats gain a confirmation bonus (+10 at levels 1--9, +15 at levels 10--14, and +20 at level 15+), your spells gain a DC bonus (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+), and all your weapon attacks ignore damage reduction.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mastermind;
				bp.IsClassFeature = true;
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
							ProgressionValue = 5
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 8
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 10
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.ProjectilesCount;
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
				bp.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.ProjectilesCount);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 4
						}
					};
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					c.Descriptor = ModifierDescriptor.Insight;
				});
				bp.AddComponent<IgnoreDamageReductionOnAttack>();
			});
			BlueprintAbility AllAccordingToPlanAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "AllAccordingToPlanAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "All According to Plan");
				bp.SetDescription(Main.IsekaiContext, "As a swift action (3/day), execute your overarching tactical scheme. For 1 round, your attacks gain an insight bonus (+5 at levels 1--9, +8 at levels 10--14, and +10 at level 15+), critical threats gain a confirmation bonus (+10 at levels 1--9, +15 at levels 10--14, and +20 at level 15+), your spells gain a DC bonus (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+), and all your weapon attacks ignore damage reduction.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mastermind;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = AllAccordingToPlanResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = AllAccordingToPlanBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = Values.Duration.OneRound
					});
				});
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "AllAccordingToPlanFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - All According to Plan");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to the Mastermind archetype. You have mapped every possibility, trap, and adversary into an infallible grand calculation from step zero.\nBenefit: You gain an insight bonus to Armor Class, attack rolls, damage rolls, and saving throws (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+), an insight bonus to Initiative (+4 at levels 1--9, +6 at levels 10--14, and +8 at level 15+), your weapon critical threat range increases by 1, and your spells ignore spell resistance. In addition, you gain the 'All According to Plan' swift action ability (3/day) to guarantee lethal tactical execution for 1 round.\nNote: Mutually exclusive with the Perception insight trio (Status Window, Omniscient Mimicry, and Paradox Sovereign).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mastermind;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { AllAccordingToPlanAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = AllAccordingToPlanResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
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
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 4
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalDamage;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 4
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 6
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 8
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				bp.AddComponent(delegate(WeaponCriticalEdgeIncreaseStackable c)
				{
					c.Value = 1;
				});
				bp.AddComponent(delegate(IgnoreSpellImmunity c)
				{
					c.SpellDescriptor = SpellDescriptor.None;
				});
				bp.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "StatusWindowFeature");
				});
				bp.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "OmniscientMimicryFeature");
				});
				bp.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "ParadoxSovereignFeature");
				});
				bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.m_Archetype = MastermindArchetype.GetReference();
					c.Level = 1;
				});
			}));
		}
	}
}
