using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem.Rules.Damage;
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
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class Instakill {
        private static readonly BlueprintBuff Stunned = BlueprintTools.GetBlueprint<BlueprintBuff>("09d39b38bb7c6014394b6daced9bacd3");
        private static readonly Sprite Icon_TwoHandedFighterDevastatingBlow = BlueprintTools.GetBlueprint<BlueprintFeature>("687aa977ef0d3f849af8bee2f40930df").m_Icon;

        public static void Add() {
            LocalizedString InstaKillDesc = Helpers.CreateString(IsekaiContext, "Instakill.Description",
        "Who will live and who will die? This ultimate power to alter fate lies within your grasp. Will you wield it wisely, or will you "
        + "assert your dominion as a god of the new world?"
        + "\nBenefit: Instantly kills the targeted creature if they fail a Fortitude saving throw. The DC starts at 25 and scales with "
        + "your level and Charisma modifier, making it increasingly difficult to resist. Creatures immune to death effects are not spared, "
        + "but instead suffer significant unholy damage."
        + "\nFailure: Creatures that succeed on the saving throw are stunned for 1 round, giving you the opportunity to turn the tide in your favor."
    );

            var InstakillUnitProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(IsekaiContext, "InstakillUnitProperty", bp => {
                bp.name = "InstakillUnitProperty";
                bp.AddComponent<SimplePropertyGetter>(c => {
                    c.Property = UnitProperty.Level;
                });
                bp.AddComponent<SimplePropertyGetter>(c => {
                    c.Property = UnitProperty.StatBonusCharisma;
                });
                bp.BaseValue = 25;
                bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
            });

            var InstakillAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, "InstakillAbility", bp => {
                bp.SetName(IsekaiContext, "Overpowered Ability — Instakill");
                bp.SetDescription(InstaKillDesc);
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = ActionFlow.DoSingle<ContextActionSavingThrow>(c => {
                        c.Type = SavingThrowType.Fortitude;
                        c.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0];
                        c.HasCustomDC = false;
                        c.CustomDC = 0;
                        c.Actions = ActionFlow.DoSingle<ContextActionConditionalSaved>(c => {
                            c.Succeed = ActionFlow.DoSingle<ContextActionApplyBuff>(c => {
                                c.m_Buff = Stunned.ToReference<BlueprintBuffReference>();
                                c.DurationValue = Values.Duration.OneRound;
                            });
                            c.Failed = ActionFlow.DoSingle<Conditional>(c => {
                                c.ConditionsChecker = ActionFlow.IfSingle<ContextConditionHasBuffImmunityWithDescriptor>(c => {
                                    c.CheckBuffDescriptorComponent = true;
                                    c.CheckSpellDescriptorComponent = true;
                                    c.SpellDescriptor = SpellDescriptor.Death;
                                });
                                c.IfTrue = ActionFlow.DoSingle<ContextActionDealDamage>(c => {
                                    c.DamageType = new DamageTypeDescription() {
                                        Type = DamageType.Energy,
                                        Energy = Kingmaker.Enums.Damage.DamageEnergyType.Unholy
                                    };
                                    c.Value = new ContextDiceValue() {
                                        DiceType = DiceType.Zero,
                                        DiceCountValue = 0,
                                        BonusValue = Values.CreateContextRankValue(AbilityRankType.DamageBonus)
                                    };
                                    c.HalfIfSaved = false;
                                    c.IsAoE = false;
                                    c.IgnoreCritical = true;
                                });
                                c.IfFalse = ActionFlow.DoSingle<ContextActionKill>();
                            });
                        });
                    });
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Necromancy;
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = new PrefabLink() { AssetId = "cc9120c1d1e64e1478b859f14a200404" };
                    c.Time = AbilitySpawnFxTime.OnApplyEffect;
                    c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
                });
                bp.AddComponent<ContextSetAbilityParams>(c => {
                    c.DC = Values.CreateContextCasterCustomPropertyValue(InstakillUnitProperty);
                    c.CasterLevel = -1;
                    c.Concentration = -1;
                    c.SpellLevel = 10;
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Progression = ContextRankProgression.MultiplyByModifier;
                    c.m_StepLevel = 10;
                });
                bp.m_Icon = Icon_TwoHandedFighterDevastatingBlow;
                bp.Type = AbilityType.SpellLike;
                bp.Range = AbilityRange.Medium;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.SpellResistance = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.AvailableMetamagic = Metamagic.Reach | Metamagic.Quicken;
                bp.LocalizedDuration = StaticReferences.Strings.Null;
                bp.LocalizedSavingThrow = StaticReferences.Strings.SavingThrow.FortitudeNegates;
            });

            var InstakillFeature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "InstakillFeature", bp => {
                bp.SetName(IsekaiContext, "Overpowered Ability — Instakill");
                bp.SetDescription(InstaKillDesc);
                bp.m_Icon = Icon_TwoHandedFighterDevastatingBlow;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { InstakillAbility.ToReference<BlueprintUnitFactReference>() };
                });
            });

            OverpoweredAbilitySelection.AddToSelection(InstakillFeature);
        }
    }
}

