using HarmonyLib;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    internal static class PatchTools {
        // Spellbooks to patch
        private static BlueprintSpellbookReference[] patchableSpellBooks = new BlueprintSpellbookReference[0];

        // Features to ignore during patching
        public static readonly BlueprintFeatureBase[] FeaturesIgnoredWhenPatching = new BlueprintFeatureBase[] {
            FeatTools.Selections.BasicFeatSelection,
            FeatTools.Selections.FighterFeatSelection,
            FeatTools.Selections.CombatTrick,
            FeatTools.Selections.SkaldFeatSelection,
            FeatTools.Selections.AnimalCompanionSelectionDomain,
            FeatTools.Selections.WarDomainGreaterFeatSelection,
            FeatTools.Selections.MagusFeatSelection,
            FeatTools.Selections.CavalierBonusFeatSelection,
            BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("f1add10c87fa4563ad5f71779eecde19") // GriffonheartShifterFeatSelection
        };

        public static void RegisterSpellbook(BlueprintSpellbook spellbook) {
            if (spellbook == null) return;
            BlueprintSpellbookReference spellbookRef = spellbook.ToReference<BlueprintSpellbookReference>();
            if (patchableSpellBooks.Contains(spellbookRef)) return;
            patchableSpellBooks = patchableSpellBooks.AddToArray(spellbookRef);

            // Allow Spellbook to be merged with angel and lich
            var AngelIncorporateSpellBook = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("e1fbb0e0e610a3a4d91e5e5284587939");
            var LichIncorporateSpellBook = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("3f16e9caf7c683c40884c7c455ed26af");
            TTCoreExtensions.RegisterForMythicSpellbook(AngelIncorporateSpellBook, spellbook);
            TTCoreExtensions.RegisterForMythicSpellbook(LichIncorporateSpellBook, spellbook);
        }

        private static BlueprintProgression PatchPatchClassProgressionBasedOnRefClassStep1(BlueprintProgression prog, BlueprintCharacterClass refClass) {
            prog.IsClassFeature = true;
            prog.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                new BlueprintProgression.ClassWithLevel {
                    m_Class = IsekaiProtagonistClass.GetReference(),
                    AdditionalLevel = 0
                }
            };
            prog.AddComponent<ClassLevelsForPrerequisites>(c => {
                c.m_FakeClass = refClass.ToReference<BlueprintCharacterClassReference>();
                c.m_ActualClass = IsekaiProtagonistClass.GetReference();
                c.Modifier = 1.0;
            });
            prog.LevelEntries = new LevelEntry[0];
            var referenceUIGroups = refClass.Progression.UIGroups;
            prog.UIGroups = new UIGroup[0];
            foreach (var referenceUIGroup in referenceUIGroups) {
                prog.UIGroups = prog.UIGroups.AddToArray(referenceUIGroup);
            }
            prog.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[0];
            var referenceUIDeterminators = refClass.Progression.UIDeterminatorsGroup;
            foreach (var UIDetermin in referenceUIDeterminators) {
                prog.m_UIDeterminatorsGroup = prog.m_UIDeterminatorsGroup.AddToArray(UIDetermin.ToReference<BlueprintFeatureBaseReference>());
            }
            return prog;
        }

        public static BlueprintProgression PatchClassProgressionBasedOnRefClass(BlueprintProgression prog, BlueprintCharacterClass refClass) {
            prog = PatchPatchClassProgressionBasedOnRefClassStep1(prog, refClass);
            var referenceLevels = refClass.Progression.LevelEntries;
            foreach (var referenceLevel in referenceLevels) {
                BlueprintFeatureBaseReference[] features = referenceLevel.m_Features.ToArray();
                prog.LevelEntries = prog.LevelEntries.AddToArray(Helpers.CreateLevelEntry(referenceLevel.Level, features));
            };
            return prog;
        }

        public static BlueprintProgression PatchClassProgressionBasedOnSeparateLists(BlueprintProgression prog, BlueprintCharacterClass refClass, LevelEntry[] additionalReference, LevelEntry[] removedReference) {
            BlueprintArchetype archetype = new BlueprintArchetype {
                RemoveFeatures = removedReference,
                AddFeatures = additionalReference
            };
            return PatchClassProgressionBasedonRefArchetype(prog, refClass, archetype, null);
        }

        public static BlueprintProgression PatchClassProgressionBasedonRefArchetype(BlueprintProgression prog, BlueprintCharacterClass refClass, BlueprintArchetype refArchetype, LevelEntry[] additionalReference) {
            prog = PatchPatchClassProgressionBasedOnRefClassStep1(prog, refClass);
            LevelEntry[] referenceLevels = refClass.Progression.LevelEntries;
            BlueprintFeatureBase[] MissingUIGroup = new BlueprintFeatureBase[0];
            foreach (LevelEntry referenceLevel in referenceLevels) {
                BlueprintFeatureBaseReference[] features = new BlueprintFeatureBaseReference[0];
                BlueprintFeatureBaseReference[] addItems = referenceLevel.m_Features.ToArray();
                BlueprintFeatureBaseReference[] removed = new BlueprintFeatureBaseReference[0];
                foreach (LevelEntry candidate in refArchetype.RemoveFeatures) {
                    if (candidate.Level == referenceLevel.Level) {
                        removed = removed.AddRangeToArray(candidate.m_Features.ToArray());
                    }
                }
                foreach (BlueprintFeatureBaseReference feature in addItems) {
                    if (!removed.Contains(feature)) {
                        features = features.AddToArray(feature);
                    }
                }
                BlueprintFeatureBaseReference[] added = new BlueprintFeatureBaseReference[0];
                foreach (LevelEntry candidate in refArchetype.AddFeatures) {
                    if (candidate.Level == referenceLevel.Level) {
                        added = added.AddRangeToArray(candidate.m_Features.ToArray());
                    }
                }
                if (added != null && added.Length > 0) {
                    foreach (var feature in added) {
                        features = features.AddToArray(feature);
                        if (!MissingUIGroup.Contains(feature)) { MissingUIGroup = MissingUIGroup.AddToArray(feature); }
                    }
                }
                if (additionalReference != null) {
                    LevelEntry additionalFeatures = null;
                    foreach (LevelEntry candidate in additionalReference) {
                        if (candidate.Level == referenceLevel.Level) {
                            additionalFeatures = candidate;
                        }
                    }
                    if (additionalFeatures != null) {
                        foreach (BlueprintFeatureBaseReference feature in additionalFeatures.m_Features) {
                            features = features.AddToArray(feature);
                            if (!MissingUIGroup.Contains(feature)) { MissingUIGroup = MissingUIGroup.AddToArray(feature); }
                        }
                    }
                }
                prog.LevelEntries = prog.LevelEntries.AddToArray(Helpers.CreateLevelEntry(referenceLevel.Level, features));
            };
            //run through them again to get references to levels that had no features previously
            if (additionalReference != null) {
                foreach (LevelEntry level in additionalReference) {
                    bool found = false;
                    foreach (LevelEntry refLevel in prog.LevelEntries) {
                        if (refLevel.Level == level.Level) {
                            found = true;
                        }
                    }
                    if (!found) {
                        prog.LevelEntries = prog.LevelEntries.AddToArray(level);
                        foreach (BlueprintFeatureBaseReference feature in level.m_Features) {
                            if (!MissingUIGroup.Contains(feature)) {
                                MissingUIGroup = MissingUIGroup.AddToArray(feature);
                            }
                        }
                    }
                }
            }
            if (MissingUIGroup.Length > 0) {
                prog.UIGroups = prog.UIGroups.AddToArray(Helpers.CreateUIGroup(MissingUIGroup));
            }
            return prog;
        }

        public static void PatchProgressionFeaturesBasedOnReferenceClass(BlueprintProgression prog, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass) {
            var features = new HashSet<BlueprintFeatureBase>();
            foreach (LevelEntry levelEntry in prog.LevelEntries) {
                foreach (BlueprintFeatureBaseReference levelitem in levelEntry.m_Features) {
                    if (!features.Contains(levelitem)) { features.Add(levelitem); }
                }
            }
            foreach (BlueprintFeatureBase levelitem in features) {
                if (levelitem is BlueprintProgression progression) {
                    PatchClassIntoFeatureOfReferenceClass(progression, myClass, referenceClass);
                } else if (levelitem is BlueprintFeature feature) {
                    PatchClassIntoFeatureOfReferenceClass(feature, myClass, referenceClass);
                }
            }
        }

        private const int MaxPatchTraversalDepth = 256;
        private const int MaxMechanicsTraversalDepth = 16;
        private static readonly Dictionary<Type, FieldInfo[]> MechanicsFieldCache = new();
        private static readonly Dictionary<Type, bool> PatchableReferenceTypeCache = new();

        public static void PatchClassIntoFeatureOfReferenceClass(BlueprintFact feature, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int level = 0, HashSet<BlueprintFact> loopPrevention = null) {
            loopPrevention ??= new();
            if (feature == null || myClass == null || referenceClass == null) {
                IsekaiContext.Logger.LogError("Call to add feature but one of the three parameters is null");
                return;
            }
            int mylevel = level + 1;
            if (mylevel > MaxPatchTraversalDepth) {
                IsekaiContext.Logger.LogError($"reference class={referenceClass.Guid} stopped patching feature={feature.AssetGuid} name={feature.name} after exceeding traversal depth {MaxPatchTraversalDepth}");
                return;
            }
            if (FeaturesIgnoredWhenPatching.Contains(feature)) {
                //these lists are to be ignored because they are known to be massive but are mostly subsets of the basic feat list or other things that should never contain something class specific that needs patching
                return;
            }
            if (!loopPrevention.Add(feature)) {
                return;
            }
            try {
                if (feature is BlueprintProgression progression) {
                    PatchClassProgression(progression, myClass, referenceClass, mylevel, loopPrevention);
                }
                if (feature is BlueprintFeatureSelection selection) {
                    PatchClassSelection(selection, myClass, referenceClass, mylevel, loopPrevention);
                }
                //components is null for BlueprintProgressions despite the fact that they implement Blueprintfeature, that will cause a nullpointer,
                //and since the cast to Blueptintfeature will work since it "supposedly" implements it checking if the field is null is the safest solution
                if (feature.Components != null && feature.Components.Length > 0) {
                    HashSet<SpellReference> mySpellSet = new HashSet<SpellReference>();
                    List<SpontaneousSpellConversion> conversions = new List<SpontaneousSpellConversion>();
                    List<CannyDefensePermanent> cannyDefenses = new List<CannyDefensePermanent>();
                    HashSet<object> visitedMechanicsObjects = new HashSet<object>(ReferenceObjectComparer.Instance);
                    for (int componentIndex = 0; componentIndex < feature.ComponentsArray.Length; componentIndex++) {
                        BlueprintComponent component = feature.ComponentsArray[componentIndex];
                        if (component == null) continue;
                        if (component is ContextCalculateAbilityParamsBasedOnClass abilityParams &&
                            abilityParams.m_CharacterClass != null &&
                            abilityParams.m_CharacterClass.Equals(referenceClass)) {
                            component = new ContextCalculateAbilityParamsBasedOnClasses() {
                                m_CharacterClasses = new[] { abilityParams.m_CharacterClass, myClass },
                                StatType = abilityParams.StatType,
                                UseKineticistMainStat = abilityParams.UseKineticistMainStat
                            };
                            feature.ComponentsArray[componentIndex] = component;
                        } else if (component is ContextCalculateAbilityParamsBasedOnClasses abilityParamsByClasses &&
                            abilityParamsByClasses.m_CharacterClasses != null &&
                            abilityParamsByClasses.m_CharacterClasses.Contains(referenceClass) &&
                            !abilityParamsByClasses.m_CharacterClasses.Contains(myClass)) {
                            abilityParamsByClasses.m_CharacterClasses = abilityParamsByClasses.m_CharacterClasses.AddToArray(myClass);
                        }
                        if (component is SpellLevelByClassLevel spellLevel &&
                            spellLevel.m_Class != null &&
                            spellLevel.m_Class.Equals(referenceClass)) {
                            component = new SpellLevelByClassLevels() {
                                m_Ability = spellLevel.m_Ability,
                                m_Classes = new[] { spellLevel.m_Class, myClass }
                            };
                            feature.ComponentsArray[componentIndex] = component;
                        } else if (component is SpellLevelByClassLevels spellLevelByClasses &&
                            spellLevelByClasses.m_Classes != null &&
                            spellLevelByClasses.m_Classes.Contains(referenceClass) &&
                            !spellLevelByClasses.m_Classes.Contains(myClass)) {
                            spellLevelByClasses.m_Classes = spellLevelByClasses.m_Classes.AddToArray(myClass);
                        }
                        if (component is AddClassLevelToSummonDuration summonDuration &&
                            summonDuration.m_CharacterClass != null &&
                            summonDuration.m_CharacterClass.Equals(referenceClass)) {
                            component = new AddClassLevelsToSummonDuration() {
                                Half = summonDuration.Half,
                                m_CharacterClasses = new[] { summonDuration.m_CharacterClass, myClass }
                            };
                            feature.ComponentsArray[componentIndex] = component;
                        } else if (component is AddClassLevelsToSummonDuration summonDurationByClasses &&
                            summonDurationByClasses.m_CharacterClasses != null &&
                            summonDurationByClasses.m_CharacterClasses.Contains(referenceClass) &&
                            !summonDurationByClasses.m_CharacterClasses.Contains(myClass)) {
                            summonDurationByClasses.m_CharacterClasses = summonDurationByClasses.m_CharacterClasses.AddToArray(myClass);
                        }
                        //check if component is addSpell or addFeat
                        HandleComponent(feature.AssetGuid, myClass, referenceClass, mylevel, mySpellSet, component, loopPrevention, visitedMechanicsObjects);
                        if (component is ContextRankConfig rankConfig && (
                            rankConfig.m_BaseValueType == ContextRankBaseValueType.ClassLevel ||
                            rankConfig.m_BaseValueType == ContextRankBaseValueType.MaxClassLevelWithArchetype ||
                            rankConfig.m_BaseValueType == ContextRankBaseValueType.SummClassLevelWithArchetype ||
                            rankConfig.m_BaseValueType == ContextRankBaseValueType.OwnerSummClassLevelWithArchetype ||
                            rankConfig.m_BaseValueType == ContextRankBaseValueType.Bombs)) {
                            if (rankConfig.m_Class != null &&
                                !rankConfig.m_Class.Contains(myClass) &&
                                rankConfig.m_Class.Contains(referenceClass)) {
                                rankConfig.m_Class = rankConfig.m_Class.AddToArray(myClass);
                                //test at level 20 if needed
                                //rankConfig.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                            }
                            PatchReferencedUnitProperties(rankConfig, myClass, referenceClass);
                        }
                        else if (component is SpontaneousSpellConversion conversion && conversion.m_CharacterClass != null && conversion.m_CharacterClass.Equals(referenceClass)) {
                            conversions.Add(conversion);
                        }
                        else if (component is CannyDefensePermanent cannyDefense && cannyDefense.m_CharacterClass != null && cannyDefense.m_CharacterClass.Equals(referenceClass)) {
                            cannyDefenses.Add(cannyDefense);
                        }
                    }
                    foreach (var spellReference in mySpellSet) {
                        feature.AddComponent<AddKnownSpell>(c => {
                            c.m_CharacterClass = myClass;
                            c.m_Spell = spellReference.value;
                            c.SpellLevel = spellReference.level;

                        });
                    }
                    foreach (SpontaneousSpellConversion conversion in conversions) {
                        feature.AddComponent<SpontaneousSpellConversion>(c => {
                            c.m_CharacterClass = myClass;
                            c.m_SpellsByLevel = conversion.m_SpellsByLevel;
                        });
                    }
                    foreach (CannyDefensePermanent cannyDefense in cannyDefenses) {
                        feature.AddComponent<CannyDefensePermanent>(c => {
                            c.m_CharacterClass = myClass;
                            c.RequiresKensai = cannyDefense.RequiresKensai;
                            c.m_ChosenWeaponBlueprint = cannyDefense.m_ChosenWeaponBlueprint;
                        });
                    }
                }
            } catch (NullReferenceException e) {
                if (feature.name != null) {
                    IsekaiContext.Logger.LogError($"Unpatachable Feature={feature.AssetGuid} name={feature.name} at level={mylevel} reason={e.Message}");
                } else {
                    IsekaiContext.Logger.LogError($"Unpatachable Feature={feature.AssetGuid} at level={mylevel} reason={e.Message}");
                }
            }
        }

        private static void PatchClassProgression(BlueprintProgression progression, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int mylevel, HashSet<BlueprintFact> loopPrevention) {
            progression.GiveFeaturesForPreviousLevels = true;
            if (progression.m_Classes != null && progression.m_Classes.Length > 0) {
                bool alreadyPatched = false;
                foreach (var refClass in progression.m_Classes) {
                    if (refClass != null && myClass.Equals(refClass.m_Class)) {
                        alreadyPatched = true;
                        break;
                    }
                }
                if (!alreadyPatched) {
                    progression.AddClass(myClass);
                }
            }
            HashSet<BlueprintFeatureBase> features = new();
            foreach (LevelEntry levelEntry in progression.LevelEntries ?? Array.Empty<LevelEntry>()) {
                foreach (BlueprintFeatureBase levelitem in levelEntry.Features ?? Array.Empty<BlueprintFeatureBase>()) {
                    if (levelitem != null && !features.Contains(levelitem)) {
                        features.Add(levelitem);
                    }
                }
            }
            foreach (BlueprintFeatureBase feature in features) {
                PatchClassIntoFeatureOfReferenceClass(feature, myClass, referenceClass, mylevel, loopPrevention);
            }
        }

        private static void PatchClassSelection(BlueprintFeatureSelection selection, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int mylevel, HashSet<BlueprintFact> loopPrevention) {
            //don't trust selections past a certain size to actually contain class features rather than just a selection of basic feats unless they are selections that are known to be that size for a valid reason(revelations, hexes, rage powers)
            string selectionGuid = selection.AssetGuid.ToString();
            if (selection.m_AllFeatures == null) {
                return;
            }
            if (selection.m_AllFeatures.Length > 30 && !(
                selectionGuid.Equals("60008a10ad7ad6543b1f63016741a5d2") // OracleRevelationSelection
                || selectionGuid.Equals("c074a5d615200494b8f2a9c845799d93") // RogueTalentSelection
                || selectionGuid.Equals("4223fe18c75d4d14787af196a04e14e7") // ShamanHexSelection
                || selectionGuid.Equals("28710502f46848d48b3f0d6132817c4e") // RagePowerSelection
                || selectionGuid.Equals("2476514e31791394fa140f1a07941c96") // SkaldRagePowerSelection
                || selectionGuid.Equals("9846043cf51251a4897728ed6e24e76f") // WitchHexSelection
                || selectionGuid.Equals("99999999000900000009000000000001") // InquisitorDomains (In our mod)
                || selectionGuid.Equals("58d6f8e9eea63f6418b107ce64f315ea") // InfusionSelection
                || selectionGuid.Equals("5c883ae0cd6d7d5448b7a420f51f8459") // WildTalentSelection
                || selectionGuid.Equals("6b894365217f47049765067a303ed5a6") // ArcanistFaithMagic
                || selectionGuid.Equals("94e2cd84bf3a8e04f8609fe502892f4f") // BardTalentSelection
                || selectionGuid.Equals("ad6b9cecb5286d841a66e23cea3ef7bf") // HexcrafterMagusHexArcanaSelection
                || selectionGuid.Equals("b78d146cea711a84598f0acef69462ea") // FinesseTrainingSelection
                )) {
                IsekaiContext.Logger.LogError($"reference class={referenceClass.Guid} Stop Feature={selectionGuid} name={selection.name} reason=selection contains too many features and thus likely is a basic feat variation");
                return;
            }
            foreach (BlueprintFeatureReference featureRef in selection.m_AllFeatures) {
                PatchClassIntoFeatureOfReferenceClass(featureRef?.Get(), myClass, referenceClass, mylevel, loopPrevention);
            }
        }

        private static void HandleComponent(BlueprintGuid featureGuid, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int level, HashSet<SpellReference> mySpellSet, BlueprintComponent component, HashSet<BlueprintFact> loopPrevention, HashSet<object> visitedMechanicsObjects) {
            var mylevel = level + 1;
            if (mylevel > MaxPatchTraversalDepth) {
                IsekaiContext.Logger.LogError($"stopped patching components for feature={featureGuid} after exceeding traversal depth {MaxPatchTraversalDepth}");
                return;
            }
            if (component == null) { return; }

            try {
                if (component is AddKnownSpell asSpell && asSpell.m_CharacterClass != null && asSpell.m_CharacterClass.Equals(referenceClass)) {
                    mySpellSet.Add(new SpellReference(asSpell.SpellLevel, asSpell.m_Spell));
                }
            } catch (NullReferenceException) {
                IsekaiContext.Logger.LogError($"{featureGuid} component cast asSpell failed due to Nullpointer");
            }

            try {
                // we do not have a special spell list, so just add all such spells to spells known
                if (component is AddSpecialSpellList asSpellList && asSpellList.m_CharacterClass.Equals(referenceClass)) {
                    foreach (var level2 in asSpellList.SpellList.SpellsByLevel) {
                        foreach (var spell in level2.m_Spells) {
                            mySpellSet.Add(new SpellReference(level2.SpellLevel, spell));
                        }
                    }
                }
            } catch (NullReferenceException) {
                IsekaiContext.Logger.LogError($"{featureGuid} component cast AddSpecialSpellList failed due to Nullpointer");
            }
            if (component is AddAbilityUseTrigger trigger && trigger.m_Spellbooks != null && trigger.m_Spellbooks.Length > 0) {
                trigger.m_Spellbooks = trigger.m_Spellbooks.AddRangeToArray(patchableSpellBooks);
            }
            if (component is AddCasterLevelForSpellbook cl && cl.m_Spellbooks != null && cl.m_Spellbooks.Length > 0) {
                cl.m_Spellbooks = cl.m_Spellbooks.AddRangeToArray(patchableSpellBooks);
            }
            if (component is IncreaseSpellSpellbookDC cdc && cdc.m_Spellbooks != null && cdc.m_Spellbooks.Length > 0) {
                cdc.m_Spellbooks = cdc.m_Spellbooks.AddRangeToArray(patchableSpellBooks);
            }
            if (component is IncreaseSpellDamageByClassLevel spellDamage &&
                spellDamage.m_CharacterClass != null &&
                (spellDamage.m_CharacterClass.Equals(referenceClass) ||
                 (spellDamage.m_AdditionalClasses?.Contains(referenceClass) ?? false)) &&
                !(spellDamage.m_AdditionalClasses?.Contains(myClass) ?? false)) {
                spellDamage.m_AdditionalClasses = (spellDamage.m_AdditionalClasses ?? Array.Empty<BlueprintCharacterClassReference>())
                    .AddToArray(myClass);
            }
            if (component is BindAbilitiesToClass bindAbilities) {
                PatchPrimaryAndAdditionalClassFields(bindAbilities, "m_CharacterClass", "m_AdditionalClasses", myClass, referenceClass);
            }
            if (component is ReplaceCasterLevelOfAbility replaceCasterLevel) {
                PatchPrimaryAndAdditionalClassFields(replaceCasterLevel, "m_Class", "m_AdditionalClasses", myClass, referenceClass);
            }
            if (component is AutoMetamagic autoMetamagic) {
                PatchClassArrayField(autoMetamagic, "m_IncludeClasses", myClass, referenceClass);
                PatchClassArrayField(autoMetamagic, "m_ExcludeClasses", myClass, referenceClass);
            }
            if (component is EnhancePotion enhancePotion) {
                PatchClassArrayField(enhancePotion, "m_Classes", myClass, referenceClass);
            }
            if (component is AddFeatureOnClassLevel addFeatureOnLevel) {
                PatchClassIntoFeatureOfReferenceClass(addFeatureOnLevel.m_Feature.Get(), myClass, referenceClass, mylevel, loopPrevention);
                if (addFeatureOnLevel.m_Class != null && addFeatureOnLevel.m_Class.Equals(referenceClass)) {
                    addFeatureOnLevel.m_AdditionalClasses = addFeatureOnLevel.m_AdditionalClasses.AddToArray(myClass);
                }
            }
            if (component is MonkNoArmorFeatureUnlock addUnarmedFact) {
                var fact = addUnarmedFact.m_NewFact.Get();
                if (fact is BlueprintFact nestedFact) {
                    PatchClassIntoFeatureOfReferenceClass(nestedFact, myClass, referenceClass, mylevel, loopPrevention);
                }
            }
            if (component is AddFeatureIfHasFact addIfFact) {
                PatchClassIntoFeatureOfReferenceClass(addIfFact.m_Feature, myClass, referenceClass, mylevel, loopPrevention);
            }
            if (component is AbilityVariants variants) {
                foreach (BlueprintAbilityReference abilityReference in variants.m_Variants ?? Array.Empty<BlueprintAbilityReference>()) {
                    PatchClassIntoFeatureOfReferenceClass(abilityReference?.Get(), myClass, referenceClass, mylevel, loopPrevention);
                }
            }

            try {
                // check if component is add facts because features could also be added as facts rather than on level...
                if (component is AddFacts addFact) {
                    BlueprintUnitFactReference[] factReferences = addFact.m_Facts ?? Array.Empty<BlueprintUnitFactReference>();
                    bool hasMissingFact = false;
                    foreach (BlueprintUnitFactReference factReference in factReferences) {
                        BlueprintUnitFact fact = factReference?.Get();
                        if (fact is BlueprintFact nestedFact) {
                            PatchClassIntoFeatureOfReferenceClass(nestedFact, myClass, referenceClass, mylevel, loopPrevention);
                        } else {
                            hasMissingFact = true;
                        }
                    }

                    // Expanded Content 0.13.68 references CrueltyFact even though that
                    // version never creates the blueprint. Remove its unusable null entry.
                    if (hasMissingFact && featureGuid.ToString().Equals("3910a52a11134219ad17ed7a9f0e353e")) {
                        addFact.m_Facts = factReferences
                            .Where(factReference => factReference?.Get() != null)
                            .ToArray();
                        IsekaiContext.Logger.Log($"Removed unresolved CrueltyFact reference from feature={featureGuid}");
                    } else if (hasMissingFact) {
                        IsekaiContext.Logger.LogError($"{featureGuid} component AddFacts contains an unresolved reference");
                    }
                }
            } catch (NullReferenceException) {
                IsekaiContext.Logger.LogError($"{featureGuid} component cast AddFacts failed due to Nullpointer");
            }
            if (component is AddAbilityResources addResource) {
                BlueprintAbilityResourceReference resRef = addResource.m_Resource;
                if (resRef != null) {
                    BlueprintAbilityResource res = resRef.Get();
                    bool classlocked = false;
                    bool alreadyPatched = false;
                    if (res.m_MaxAmount.m_ClassDiv != null && res.m_MaxAmount.m_ClassDiv.Length > 0) {
                        if (res.m_MaxAmount.m_ClassDiv.Contains(referenceClass)) {
                            classlocked = true;
                        }
                        if (res.m_MaxAmount.m_ClassDiv.Contains(myClass)) {
                            alreadyPatched = true;
                        }
                        if (classlocked && !alreadyPatched) {
                            res.m_MaxAmount.m_ClassDiv = res.m_MaxAmount.m_ClassDiv.AddToArray(myClass);
                            //Main.Log("class resource patched= " + resRef.Guid);
                        }
                    }
                    if (!classlocked && res.m_MaxAmount.m_Class != null && res.m_MaxAmount.m_Class.Length > 0) {
                        if (res.m_MaxAmount.m_Class.Contains(referenceClass)) {
                            classlocked = true;
                        }
                        if (res.m_MaxAmount.m_Class.Contains(myClass)) {
                            alreadyPatched = true;
                        }
                        if (classlocked && !alreadyPatched) {
                            res.m_MaxAmount.m_Class = res.m_MaxAmount.m_Class.AddToArray(myClass);
                            //Main.Log("class resource patched= " + resRef.Guid);
                        }
                    }
                }
            }

            PatchNestedMechanicsReferences(
                component,
                myClass,
                referenceClass,
                mylevel,
                loopPrevention,
                0,
                visitedMechanicsObjects);
        }

        private static void PatchNestedMechanicsReferences(
            object value,
            BlueprintCharacterClassReference myClass,
            BlueprintCharacterClassReference referenceClass,
            int level,
            HashSet<BlueprintFact> loopPrevention,
            int mechanicsDepth,
            HashSet<object> visitedObjects) {
            if (value == null || mechanicsDepth > MaxMechanicsTraversalDepth) return;

            if (value is BlueprintReferenceBase blueprintReference) {
                if (!IsPatchableBlueprintReference(blueprintReference.GetType())) return;

                SimpleBlueprint referencedBlueprint = blueprintReference.GetBlueprint();
                // Only follow runtime ability and buff facts discovered through reflected
                // mechanics containers. Following feature, selection, or progression
                // references here can expand into entire class and bloodline graphs.
                // Known feature-bearing components are handled explicitly in
                // HandleComponent above.
                if (referencedBlueprint is BlueprintAbility nestedAbility) {
                    PatchClassIntoFeatureOfReferenceClass(nestedAbility, myClass, referenceClass, level, loopPrevention);
                } else if (referencedBlueprint is BlueprintBuff nestedBuff) {
                    PatchClassIntoFeatureOfReferenceClass(nestedBuff, myClass, referenceClass, level, loopPrevention);
                } else if (referencedBlueprint is BlueprintAbilityResource resource) {
                    PatchResourceBasedOnReferenceClass(resource, myClass, referenceClass);
                } else if (referencedBlueprint is BlueprintUnitProperty property) {
                    PatchUnitProperty(property, myClass, referenceClass);
                }
                return;
            }

            if (value is SimpleBlueprint || value is string || value is Delegate || value is Type) return;

            Type valueType = value.GetType();
            if (valueType.IsPrimitive || valueType.IsEnum || valueType == typeof(decimal)) return;

            if (value is IEnumerable enumerable && (valueType.IsArray || value is IList)) {
                foreach (object item in enumerable) {
                    PatchNestedMechanicsReferences(item, myClass, referenceClass, level, loopPrevention, mechanicsDepth + 1, visitedObjects);
                }
                return;
            }

            if (!IsMechanicsContainer(valueType)) return;
            if (!valueType.IsValueType && !visitedObjects.Add(value)) return;

            foreach (FieldInfo field in GetMechanicsFields(valueType)) {
                object fieldValue;
                try {
                    fieldValue = field.GetValue(value);
                } catch (Exception) {
                    continue;
                }
                PatchNestedMechanicsReferences(fieldValue, myClass, referenceClass, level, loopPrevention, mechanicsDepth + 1, visitedObjects);
            }
        }

        private static bool IsMechanicsContainer(Type valueType) {
            if (typeof(BlueprintComponent).IsAssignableFrom(valueType)
                || typeof(GameAction).IsAssignableFrom(valueType)
                || typeof(Condition).IsAssignableFrom(valueType)
                || valueType == typeof(ActionList)
                || valueType == typeof(ConditionsChecker)) {
                return true;
            }

            return valueType.IsValueType
                && valueType.Namespace != null
                && valueType.Namespace.StartsWith("Kingmaker.UnitLogic.Mechanics", StringComparison.Ordinal);
        }

        private static bool IsPatchableBlueprintReference(Type referenceType) {
            if (PatchableReferenceTypeCache.TryGetValue(referenceType, out bool isPatchable)) return isPatchable;

            Type currentType = referenceType;
            while (currentType != null && currentType != typeof(object)) {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(BlueprintReference<>)) {
                    Type blueprintType = currentType.GetGenericArguments()[0];
                    isPatchable = typeof(BlueprintFact).IsAssignableFrom(blueprintType)
                        || typeof(BlueprintAbilityResource).IsAssignableFrom(blueprintType)
                        || typeof(BlueprintUnitProperty).IsAssignableFrom(blueprintType);
                    break;
                }
                currentType = currentType.BaseType;
            }

            PatchableReferenceTypeCache[referenceType] = isPatchable;
            return isPatchable;
        }

        private static FieldInfo[] GetMechanicsFields(Type valueType) {
            if (MechanicsFieldCache.TryGetValue(valueType, out FieldInfo[] cachedFields)) return cachedFields;

            List<FieldInfo> fields = new();
            for (Type currentType = valueType; currentType != null && currentType != typeof(object); currentType = currentType.BaseType) {
                fields.AddRange(currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                    .Where(field => !field.IsStatic && !field.IsNotSerialized));
            }

            cachedFields = fields.ToArray();
            MechanicsFieldCache[valueType] = cachedFields;
            return cachedFields;
        }

        private static void PatchResourceBasedOnReferenceClass(
            BlueprintAbilityResource resource,
            BlueprintCharacterClassReference myClass,
            BlueprintCharacterClassReference referenceClass) {
            if (resource == null) return;

            if (resource.m_MaxAmount.m_ClassDiv != null
                && resource.m_MaxAmount.m_ClassDiv.Contains(referenceClass)
                && !resource.m_MaxAmount.m_ClassDiv.Contains(myClass)) {
                resource.m_MaxAmount.m_ClassDiv = resource.m_MaxAmount.m_ClassDiv.AddToArray(myClass);
            }

            if (resource.m_MaxAmount.m_Class != null
                && resource.m_MaxAmount.m_Class.Contains(referenceClass)
                && !resource.m_MaxAmount.m_Class.Contains(myClass)) {
                resource.m_MaxAmount.m_Class = resource.m_MaxAmount.m_Class.AddToArray(myClass);
            }
        }

        private static void PatchPrimaryAndAdditionalClassFields(
            object component,
            string primaryFieldName,
            string additionalFieldName,
            BlueprintCharacterClassReference myClass,
            BlueprintCharacterClassReference referenceClass) {
            Traverse componentFields = Traverse.Create(component);
            BlueprintCharacterClassReference primaryClass = componentFields.Field<BlueprintCharacterClassReference>(primaryFieldName).Value;
            BlueprintCharacterClassReference[] additionalClasses = componentFields.Field<BlueprintCharacterClassReference[]>(additionalFieldName).Value
                ?? Array.Empty<BlueprintCharacterClassReference>();

            if ((primaryClass?.Equals(referenceClass) ?? false) || additionalClasses.Contains(referenceClass)) {
                if (!additionalClasses.Contains(myClass)) {
                    componentFields.Field<BlueprintCharacterClassReference[]>(additionalFieldName).Value = additionalClasses.AddToArray(myClass);
                }
            }
        }

        private static void PatchClassArrayField(
            object component,
            string fieldName,
            BlueprintCharacterClassReference myClass,
            BlueprintCharacterClassReference referenceClass) {
            Traverse<BlueprintCharacterClassReference[]> field = Traverse.Create(component).Field<BlueprintCharacterClassReference[]>(fieldName);
            BlueprintCharacterClassReference[] classes = field.Value ?? Array.Empty<BlueprintCharacterClassReference>();
            if (classes.Contains(referenceClass) && !classes.Contains(myClass)) {
                field.Value = classes.AddToArray(myClass);
            }
        }

        private sealed class ReferenceObjectComparer : IEqualityComparer<object> {
            public static readonly ReferenceObjectComparer Instance = new();

            public new bool Equals(object left, object right) {
                return ReferenceEquals(left, right);
            }

            public int GetHashCode(object value) {
                return RuntimeHelpers.GetHashCode(value);
            }
        }

        internal class SpellReference {
            public int level;
            public BlueprintAbilityReference value;
            public SpellReference(int inLevel, BlueprintAbilityReference inValue) {
                level = inLevel;
                value = inValue;
            }
            public override bool Equals(object p) {
                if (ReferenceEquals(this, p)) return true;
                if (p is not SpellReference other) return false;
                return level == other.level && Equals(value?.Guid, other.value?.Guid);
            }
            public static bool operator ==(SpellReference left, SpellReference right) {
                if (left is null && right is null) return true;
                if (left is not null) {
                    return left.Equals(right);
                }
                return false;
            }
            public static bool operator !=(SpellReference left, SpellReference right) { return !(left == right); }
            public override int GetHashCode() {
                unchecked {
                    return (level * 397) ^ (value?.Guid.GetHashCode() ?? 0);
                }
            }
        }

        internal static void PatchResource(BlueprintAbilityResource resource, BlueprintCharacterClassReference classRef) {
            if (resource.m_MaxAmount.m_Class != null && resource.m_MaxAmount.m_Class.Length != 0 &&
                !resource.m_MaxAmount.m_Class.Contains(classRef)) {
                resource.m_MaxAmount.m_Class = resource.m_MaxAmount.m_Class.AppendToArray(classRef);
            }
            if (resource.m_MaxAmount.m_ClassDiv != null && resource.m_MaxAmount.m_ClassDiv.Length != 0 &&
                !resource.m_MaxAmount.m_ClassDiv.Contains(classRef)) {
                resource.m_MaxAmount.m_ClassDiv = resource.m_MaxAmount.m_ClassDiv.AppendToArray(classRef);
            }
        }

        private static void PatchReferencedUnitProperties(ContextRankConfig rankConfig, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass) {
            if (rankConfig.m_CustomProperty != null) {
                PatchUnitProperty(rankConfig.m_CustomProperty.Get(), myClass, referenceClass);
            }
            foreach (BlueprintUnitPropertyReference propertyReference in rankConfig.m_CustomPropertyList ?? Array.Empty<BlueprintUnitPropertyReference>()) {
                PatchUnitProperty(propertyReference?.Get(), myClass, referenceClass);
            }
        }

        private static void PatchUnitProperty(BlueprintUnitProperty property, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass) {
            if (property?.ComponentsArray == null) return;

            for (int componentIndex = 0; componentIndex < property.ComponentsArray.Length; componentIndex++) {
                BlueprintComponent component = property.ComponentsArray[componentIndex];
                if (component is ClassLevelGetter classLevel &&
                    classLevel.m_Class != null &&
                    classLevel.m_Class.Equals(referenceClass)) {
                    property.ComponentsArray[componentIndex] = new ClassLevelGetterWithAlternatives {
                        m_Class = classLevel.m_Class,
                        m_Archetype = classLevel.m_Archetype,
                        m_AlternativeClasses = new[] { myClass }
                    };
                } else if (component is ClassLevelGetterWithAlternatives classLevelWithAlternatives &&
                    classLevelWithAlternatives.m_Class != null &&
                    classLevelWithAlternatives.m_Class.Equals(referenceClass) &&
                    !(classLevelWithAlternatives.m_AlternativeClasses?.Contains(myClass) ?? false)) {
                    classLevelWithAlternatives.m_AlternativeClasses = (classLevelWithAlternatives.m_AlternativeClasses ?? Array.Empty<BlueprintCharacterClassReference>()).AddToArray(myClass);
                } else if (component is SummClassLevelGetter summClassLevel &&
                    summClassLevel.m_Class != null &&
                    summClassLevel.m_Class.Contains(referenceClass)) {
                    property.ComponentsArray[componentIndex] = new SummClassLevelGetterWithAlternatives {
                        m_Classes = summClassLevel.m_Class,
                        Archetype = summClassLevel.Archetype,
                        m_Archetypes = summClassLevel.m_Archetypes,
                        m_AlternativeClasses = new[] { myClass }
                    };
                } else if (component is SummClassLevelGetterWithAlternatives summClassLevelWithAlternatives &&
                    summClassLevelWithAlternatives.m_Classes != null &&
                    summClassLevelWithAlternatives.m_Classes.Contains(referenceClass) &&
                    !(summClassLevelWithAlternatives.m_AlternativeClasses?.Contains(myClass) ?? false)) {
                    summClassLevelWithAlternatives.m_AlternativeClasses = (summClassLevelWithAlternatives.m_AlternativeClasses ?? Array.Empty<BlueprintCharacterClassReference>()).AddToArray(myClass);
                }
            }
        }
        internal static void PatchAbility(BlueprintAbility ability, BlueprintCharacterClassReference classRef) {
            foreach (BlueprintComponent comp in ability.Components) {
                if (comp is ContextRankConfig rankConfig && rankConfig.m_Class != null && rankConfig.m_Class.Length > 0 &&
                    !rankConfig.m_Class.Contains(classRef)) {
                    rankConfig.m_Class = rankConfig.m_Class.AppendToArray(classRef);
                }
            }
        }
        private static void PatchBuff(BlueprintBuff buff, BlueprintSpellbookReference spellbookRef) {
            foreach (BlueprintComponent comp in buff.Components) {
                if (comp is AddAbilityUseTrigger triggerComp &&
                    (triggerComp.m_Spellbooks == null || !triggerComp.m_Spellbooks.Contains(spellbookRef))) {
                    triggerComp.m_Spellbooks = triggerComp.m_Spellbooks.AppendToArray(spellbookRef);
                } else if (comp is AddCasterLevelForSpellbook casterLevelComp &&
                    (casterLevelComp.m_Spellbooks == null || !casterLevelComp.m_Spellbooks.Contains(spellbookRef))) {
                    casterLevelComp.m_Spellbooks = casterLevelComp.m_Spellbooks.AppendToArray(spellbookRef);
                } else if (comp is IncreaseSpellSpellbookDC increaseSpellComp &&
                    (increaseSpellComp.m_Spellbooks == null || !increaseSpellComp.m_Spellbooks.Contains(spellbookRef))) {
                    increaseSpellComp.m_Spellbooks = increaseSpellComp.m_Spellbooks.AppendToArray(spellbookRef);
                }
            }
        }
        private static void PatchBuff(BlueprintBuff buff, BlueprintCharacterClassReference classRef) {
            foreach (BlueprintComponent comp in buff.Components) {
                if (comp is ContextRankConfig rankConfig && rankConfig.m_Class != null && rankConfig.m_Class.Length > 0 &&
                    !rankConfig.m_Class.Contains(classRef)) {
                    rankConfig.m_Class = rankConfig.m_Class.AppendToArray(classRef);
                }
            }
        }

        internal static class ArcanistPatcher {
            public static void Patch(BlueprintCharacterClassReference classRef, BlueprintSpellbookReference spellbookRef) {
                PatchArcaneResrvoir(classRef, spellbookRef);
                PatchConsumeSpells(classRef);
                PatchArcanistExploits(classRef);
            }
            private static void PatchArcaneResrvoir(BlueprintCharacterClassReference classRef, BlueprintSpellbookReference spellbookRef) {
                var ArcanistArcaneReservoirResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("cac948cbbe79b55459459dd6a8fe44ce");
                var ArcanistArcaneReservoirResourceBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1dd776b7b27dcd54ab3cedbbaf440cf3");
                PatchBuff(ArcanistArcaneReservoirResourceBuff, classRef);
                PatchResource(ArcanistArcaneReservoirResource, classRef);
                PatchArcaneReservoirBuffs(spellbookRef);
            }
            private static void PatchConsumeSpells(BlueprintCharacterClassReference classRef) {
                var ArcanistConsumeSpellsResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("d67ddd98ad019854d926f3d6a4e681c5");
                PatchResource(ArcanistConsumeSpellsResource, classRef);
            }
            private static void PatchArcanistExploits(BlueprintCharacterClassReference classRef) {
                var ArcanistExploitSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b8bf3d5023f2d8c428fdf6438cecaea7");
                foreach (BlueprintFeature feature in ArcanistExploitSelection.AllFeatures) {
                    AddFacts addFacts = feature.GetComponent<AddFacts>();
                    if (addFacts == null) continue;
                    foreach (BlueprintUnitFact fact in addFacts.Facts) {
                        if (fact is BlueprintAbility ability) {
                            PatchAbility(ability, classRef);
                        }
                    }
                }
                BlueprintBuff[] buffs = new BlueprintBuff[] {
                    BlueprintTools.GetBlueprint<BlueprintBuff>("d3361a1d65825aa4a952476639248792"), // ArcanistExploitLingeringAcidBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("d3a217dba1100f9449e7f249d2916f86"), // ArcanistExploitSpellResistanceBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("7b392a6348fe3ef4d907ce168a4c7773"), // ArcanistExploitSpellResistanceGreaterBuff
                };
                foreach (BlueprintBuff buff in buffs) {
                    PatchBuff(buff, classRef);
                }
            }
            private static void PatchArcaneReservoirBuffs(BlueprintSpellbookReference spellbookRef) {
                BlueprintBuff[] buffs = new BlueprintBuff[] {
                    BlueprintTools.GetBlueprint<BlueprintBuff>("33e0c3a2a54c0e7489fa4ec4d79a581b"), // ArcanistArcaneReservoirCLBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("db4b91a8a297c4247b13cfb6ea228bf3"), // ArcanistArcaneReservoirDCBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("ea01ddf2c1878354990000d1c7fc5ce4"), // ArcanistArcaneReservoirCLPotentBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("6fea993ed5782054a88fa54037a3e6dd"), // ArcanistArcaneReservoirDCPotentBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("a27a3c5e45f9416428ce983e0d4bd2d2"), // EldritchFontEldritchSurgeCLBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("91b2762997f0d8044baeeef0871eac6f"), // EldritchFontEldritchSurgeDCBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("9aab299fb44ff3c49af5b8527a23fcf7"), // EldritchFontImprovedEldritchSurgeAttackBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("4425d831546249647b8c9ad06d7ed0e7"), // EldritchFontGreaterSurgeBuff
                };
                foreach (BlueprintBuff buff in buffs) {
                    PatchBuff(buff, spellbookRef);
                }
            }
        }

        internal static class KineticistPatcher {
            public static void Patch(BlueprintCharacterClassReference classRef) {
                // TODO: base blasts added by mods would need to be patched here
                BlueprintAbility[] baseAbilities = new BlueprintAbility[] {
                    BlueprintTools.GetBlueprint<BlueprintAbility>("0ab1552e2ebdacf44bb7b20f5393366d"), // AirBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("403bcf42f08ca70498432cf62abee434"), // IceBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("e2610c88664e07343b4f3fb6336f210c"), // MudBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("83d5873f306ac954cad95b6aeeeb2d8c"), // FireBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("7980e876b0749fc47ac49b9552e259c1"), // ColdBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("e53f34fb268a7964caf1566afb82dadd"), // EarthBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("3baf01649a92ae640927b0f633db7c11"), // SteamBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("8c25f52fce5113a4491229fd1265fc3c"), // MagmaBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("6276881783962284ea93298c1fe54c48"), // MetalBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("d663a8d40be1e57478f34d6477a67270"), // WaterBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("9afdc3eeca49c594aa7bf00e8e9803ac"), // PlasmaBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("45eb571be891c4c4581b6fcddda72bcd"), // ElectricBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("b93e1f0540a4fa3478a6b47ae3816f32"), // SandstormBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("4e2e066dd4dc8de4d8281ed5b3f4acb6"),  // ChargedWaterBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("b813ceb82d97eed4486ddd86d3f7771b"),  // ThunderstormBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("ba2113cfed0c2c14b93c20e7625a4c74"),  // BloodBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("16617b8c20688e4438a803effeeee8a6"),  // BlizzardBlastBase
                    BlueprintTools.GetBlueprint<BlueprintAbility>("d29186edb20be6449b23660b39435398"),  // BlueFlameBlastBase
                };
                foreach (BlueprintAbility baseAbility in baseAbilities) {
                    foreach (BlueprintAbilityReference abilityRef in baseAbility.GetComponent<AbilityVariants>().m_Variants) {
                        var ability = abilityRef.Get();
                        for (int i = 0; i < ability.ComponentsArray.Length; i++) {
                            BlueprintComponent component = ability.ComponentsArray[i];
                            if (component is ContextCalculateAbilityParamsBasedOnClass paramsComponent) {
                                ability.ComponentsArray[i] = new ContextCalculateAbilityParamsBasedOnClasses() {
                                    m_CharacterClasses = new BlueprintCharacterClassReference[] { paramsComponent.m_CharacterClass, classRef },
                                    StatType = paramsComponent.StatType,
                                    UseKineticistMainStat = paramsComponent.UseKineticistMainStat
                                };
                            }
                        }
                    }
                }
            }
        }

        internal static class ShifterPatcher {
            public static void Patch(BlueprintCharacterClassReference classRef) {
                var shifterClassRef = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("a406d6ebea5c46bba3160246be03e96f");
                BlueprintFeature[] clawFeatures = new BlueprintFeature[] {
                    BlueprintTools.GetBlueprint<BlueprintFeature>("08a8cfba6ae34505a64d6ba00225c4d2"), // ShifterClawsFeatureAddLevel
                    BlueprintTools.GetBlueprint<BlueprintFeature>("f7996c5b51e348fc9277480d9cc0a88c"), // ShifterClawsFeatureAddLevel1
                    BlueprintTools.GetBlueprint<BlueprintFeature>("19b7335626b3434cbe2af01fb33582ff"), // ShifterClawsFeatureAddLevel2
                    BlueprintTools.GetBlueprint<BlueprintFeature>("8d6b338131764a4fb68eaf5b5c6cfe47"), // ShifterClawsFeatureAddLevel3
                    BlueprintTools.GetBlueprint<BlueprintFeature>("024b8248f85d412cb7c520a9f746c547"), // ShifterClawsFeatureAddLevel4
                    BlueprintTools.GetBlueprint<BlueprintFeature>("76b8314a83ff4825a145ac8f7b59d6e4"), // ShifterClawsFeatureAddLevel5
                    BlueprintTools.GetBlueprint<BlueprintFeature>("28991899db1948d9bdd5f958b4add2d8"), // ShifterClawsFeatureAddLevel6
                };
                foreach (BlueprintFeature clawFeature in clawFeatures) {
                    PatchClassIntoFeatureOfReferenceClass(clawFeature, classRef, shifterClassRef);
                }

                BlueprintBuff[] aspectBuffs = new BlueprintBuff[] {
                    BlueprintTools.GetBlueprint<BlueprintBuff>("a237792fc2644a4ebc6eefa2d325f181"), // ShifterAspectBearBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("0fdc579eafbf4fceae649beed8188a5c"), // ShifterAspectBoarBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("4a981c46dc60474cad17549a2b9f7f65"), // ShifterAspectDinosaurBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("0bdfd34b7e5a4639bf6b716fa2ac0098"), // ShifterAspectHorseBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("29c0e16558b043278f061b128b9d180c"), // ShifterAspectLizardBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("c4c39e7078224b6caaf3c8a02032b5cb"), // ShifterAspectElephantBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("49e196492d2c4aa588bafffea6db8c43"), // ShifterAspectSpiderBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("0555960c7ab8431d86de4e7db0c22160"), // ShifterAspectTigerBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("d61bd26255d4425d929d73e57ef0e6dd"), // ShifterAspectWolfBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("e0b35a32cf234381ab13c831106937f1"), // ShifterAspectWolverineBuff
                };
                foreach (BlueprintBuff buff in aspectBuffs) {
                    foreach(ContextRankConfig rankConfig in buff.GetComponents<ContextRankConfig>()) {
                        if (rankConfig.m_BaseValueType != ContextRankBaseValueType.ClassLevel) continue;
                        rankConfig.m_Class = rankConfig.m_Class.AddToArray(classRef);
                    }
                }

                BlueprintAbilityResource[] resources = new BlueprintAbilityResource[] {
                    BlueprintTools.GetBlueprint<BlueprintAbilityResource>("2210cea8cc94431a911dc5d4b6d72cbd"), // ShifterWildShapeResource
                    BlueprintTools.GetBlueprint<BlueprintAbilityResource>("80923bd575dc48f5813c2343517414cf"), // DragonbloodShifterResource
                    BlueprintTools.GetBlueprint<BlueprintAbilityResource>("72e7ec0822604f7da75c3dd32e93d5ea"), // DragonbloodShifterBreathResource
                };
                foreach (BlueprintAbilityResource resource in resources) {
                    PatchResource(resource, classRef);
                }

                BlueprintAbility[] abilities = new BlueprintAbility[] {
                    BlueprintTools.GetBlueprint<BlueprintAbility>("c35a9830c7684f76a704aff424128851"), // ShifterDragonForm20Neutral
                    BlueprintTools.GetBlueprint<BlueprintAbility>("2d52be9832e542bc88b1959be2f3b2e2"), // ShifterDragonForm20Good
                    BlueprintTools.GetBlueprint<BlueprintAbility>("f619bb36520a48478f900431b20d50c4"), // ShifterDragonForm20Evil
                };
                foreach (BlueprintAbility ability in abilities) {
                    PatchClassIntoFeatureOfReferenceClass(ability, classRef, shifterClassRef);
                }

                BlueprintBuff[] dragonAspectBuffs = new BlueprintBuff[] {
                    BlueprintTools.GetBlueprint<BlueprintBuff>("ace35704bf744216b142adcbd5c58d13"), // DragonbloodShifterBlackBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("627c3f3256494000b3cba6f461b2c44c"), // DragonbloodShifterBlueBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("e850db860994442b83e964d3a43e9358"), // DragonbloodShifterGreenBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("fb71be7bcbc648539d57171b0d0baf79"), // DragonbloodShifterRedBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("cd34e74f0d2844e3ab5580c1dbe3ff7d"), // DragonbloodShifterWhiteBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("ed9ac87aaac7419095395304c7b5d813"), // DragonbloodShifterBrassBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("233617f6876a42ef973de8af502d4fed"), // DragonbloodShifterBronzeBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("566474477be14eaeb860086b82e5e9cf"), // DragonbloodShifterCopperBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("e36d67f4b25a4d5ebf9add1e2bc52e74"), // DragonbloodShifterGoldBuff
                    BlueprintTools.GetBlueprint<BlueprintBuff>("d9de1c4f6e68416196c193ac87962993"), // DragonbloodShifterSilverBuff
                };
                foreach (BlueprintBuff dragonBuff in dragonAspectBuffs) {
                    PatchClassIntoFeatureOfReferenceClass(dragonBuff, classRef, shifterClassRef);
                }
            }
        }

        [HarmonyPatch(typeof(UnitPartMagus), "get_Spellbook")]
        public static class UnitPartMagusPatcher {
            public static bool Prefix(UnitPartMagus __instance, ref Spellbook __result) {
                ClassData classData = __instance.Owner.Progression.GetClassData(IsekaiProtagonistClass.Get());
                if (classData == null) {
                    // Not an Isekai Protagonist: run the unmodified game getter.
                    return true;
                }
                BlueprintSpellbook blueprintSpellbook;
                if ((blueprintSpellbook = (classData?.Spellbook.Or(null))) == null) {
                    BlueprintCharacterClass blueprintCharacterClass = __instance.Class.Or(null);
                    blueprintSpellbook = (blueprintCharacterClass?.Spellbook);
                }
                BlueprintSpellbook blueprintSpellbook2 = blueprintSpellbook;
                __instance.m_Spellbook = ((blueprintSpellbook2 != null) ? __instance.Owner.GetSpellbook(blueprintSpellbook2) : null);
                if (__instance.m_Spellbook == null) {
                    __instance.m_Spellbook = __result;
                    return false;
                } else {
                    __result = __instance.m_Spellbook;
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(AddVendorDiscount), "OnTurnOn")]
        public static class AddVendorDiscountPatcher {
            public static void Prefix(AddVendorDiscount __instance) {
                __instance.OnInitialize();
            }
        }
    }
}
