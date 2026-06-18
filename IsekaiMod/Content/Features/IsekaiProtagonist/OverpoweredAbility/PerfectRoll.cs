using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class PerfectRoll {
        public static void Add() {
            const string PerfectRollDesc = "You navigate every conversation and fight with perfect accuracy. "
                + "Every word is predicted; every action is foreseen. "
                + "Your premonitions guide you on your quest, as if you have experienced this before..."
                + "\nBenefit: You gain a +5 bonus to all d20 rolls.";

            var Icon_TrickFate = BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd").m_Icon;

            // Create the feature using CreateBlueprint
            var PerfectRollFeature = Helpers.CreateBlueprint<BlueprintFeature>(
                IsekaiMod.Main.IsekaiContext,
                "PerfectRollFeature",
                bp => {
                    bp.SetName(IsekaiMod.Main.IsekaiContext, "Overpowered Ability — Perfect Roll");
                    bp.SetDescription(IsekaiMod.Main.IsekaiContext, PerfectRollDesc);
                    bp.m_Icon = Icon_TrickFate;

                    // PERFORMANCE FIX: the previous implementation used a single
                    // ModifyD20 { Rule = RuleType.All, AddBonus = +5 }. ModifyD20 registers a
                    // per-roll rule handler that runs on EVERY d20 in the game, including the
                    // many simulated rolls the engine fires each frame for hit-chance previews
                    // and AI decision-making. That is what caused the stutter/freeze on attack
                    // and even when buffing, and the sluggish combat log.
                    //
                    // The same flat +5 is now granted as static stat modifiers instead. These
                    // are summed once into the relevant stat (no per-roll handler), so they are
                    // effectively free at runtime while still applying to all the d20 rolls a
                    // character actually makes: attack rolls, all saving throws, initiative, and
                    // every skill check.
                    const int PerfectRollBonus = 5;

                    // Attack rolls (also feeds combat maneuvers).
                    bp.AddComponent<AddStatBonus>(c => {
                        c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                        c.Value = PerfectRollBonus;
                        c.Descriptor = ModifierDescriptor.UntypedStackable;
                    });

                    // Saving throws.
                    foreach (var saveStat in new[] {
                        Kingmaker.EntitySystem.Stats.StatType.SaveFortitude,
                        Kingmaker.EntitySystem.Stats.StatType.SaveReflex,
                        Kingmaker.EntitySystem.Stats.StatType.SaveWill,
                    }) {
                        var stat = saveStat;
                        bp.AddComponent<AddStatBonus>(c => {
                            c.Stat = stat;
                            c.Value = PerfectRollBonus;
                            c.Descriptor = ModifierDescriptor.UntypedStackable;
                        });
                    }

                    // Initiative.
                    bp.AddComponent<AddStatBonus>(c => {
                        c.Stat = Kingmaker.EntitySystem.Stats.StatType.Initiative;
                        c.Value = PerfectRollBonus;
                        c.Descriptor = ModifierDescriptor.UntypedStackable;
                    });

                    // All skill checks.
                    foreach (var skillStat in new[] {
                        Kingmaker.EntitySystem.Stats.StatType.SkillAthletics,
                        Kingmaker.EntitySystem.Stats.StatType.SkillMobility,
                        Kingmaker.EntitySystem.Stats.StatType.SkillThievery,
                        Kingmaker.EntitySystem.Stats.StatType.SkillStealth,
                        Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana,
                        Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeWorld,
                        Kingmaker.EntitySystem.Stats.StatType.SkillLoreNature,
                        Kingmaker.EntitySystem.Stats.StatType.SkillLoreReligion,
                        Kingmaker.EntitySystem.Stats.StatType.SkillPerception,
                        Kingmaker.EntitySystem.Stats.StatType.SkillPersuasion,
                        Kingmaker.EntitySystem.Stats.StatType.SkillUseMagicDevice,
                    }) {
                        var stat = skillStat;
                        bp.AddComponent<AddStatBonus>(c => {
                            c.Stat = stat;
                            c.Value = PerfectRollBonus;
                            c.Descriptor = ModifierDescriptor.UntypedStackable;
                        });
                    }
                });

            // Add the feature to the Overpowered Ability selection
            OverpoweredAbilitySelection.AddToSelection(PerfectRollFeature);
        }
    }
}

