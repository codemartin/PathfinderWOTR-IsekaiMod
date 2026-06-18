using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower {

    internal class TrainingMontage {
        private static readonly Sprite Icon_LegendaryProportions = BlueprintTools.GetBlueprint<BlueprintAbility>("da1b292d91ba37948893cdbe9ea89e28").m_Icon;

        public static void Add() {
            // Create the Training Montage feature
            var TrainingMontage = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "TrainingMontage", bp => {
                bp.SetName(IsekaiContext, "Training Montage");
                bp.SetDescription(IsekaiContext, "Through relentless training and an unwavering desire to better yourself, you gain a +2 bonus to all attributes at level 1, increasing by +1 every 4 levels (to a maximum of +8 at level 20).");
                bp.m_Icon = Icon_LegendaryProportions;

                // Scaling bonus based on character level. ScalingStatBonus already includes the
                // initial +2 (InitialValue), so the previous separate flat +2 AddStatBonus loop was
                // removed — it double-counted, granting +4 at level 1 instead of the intended +2.
                foreach (StatType stat in new[] {
                    StatType.Strength, StatType.Dexterity, StatType.Constitution,
                    StatType.Intelligence, StatType.Wisdom, StatType.Charisma
                }) {
                    bp.AddComponent<ScalingStatBonus>(c => {
                        c.Stat = stat;
                        c.LevelDivisor = 4; // Gain +1 every 4 levels
                        c.InitialValue = 2; // Initial +2
                        c.MaxBonus = 8; // Max bonus at level 20
                    });
                }

                // Recalculate the level-scaled bonus when the character levels up; without this the
                // bonus is computed once when the feature is gained and stays frozen at that level.
                bp.ReapplyOnLevelUp = true;
            });

            // Add Training Montage to the Special Power selection
            SpecialPowerSelection.AddToSelection(TrainingMontage);
        }
    }

    // Custom component for scaling bonuses.
    // TypeId is REQUIRED for the blueprint component to bind/serialize correctly (every other custom
    // component in this mod has one); without it the component could be dropped, so Training Montage
    // would grant no bonus at all.
    [TypeId("b838cfc5d0c04a5497a60a8a40e37eaa")]
    public class ScalingStatBonus : UnitFactComponentDelegate {
        public StatType Stat;
        public int LevelDivisor = 4; // Divisor for character level scaling
        public int InitialValue = 2; // Initial bonus value
        public int MaxBonus = 8; // Maximum bonus

        public override void OnActivate() {
            ApplyScalingBonus();
        }

        public override void OnDeactivate() {
            RemoveScalingBonus();
        }

        private void ApplyScalingBonus() {
            int levelBonus = Owner.Progression.CharacterLevel / LevelDivisor;
            int totalBonus = Mathf.Min(InitialValue + levelBonus, MaxBonus);

            Owner.Stats.GetStat(Stat).AddModifier(totalBonus, Runtime, ModifierDescriptor.UntypedStackable);
        }

        private void RemoveScalingBonus() {
            Owner.Stats.GetStat(Stat).RemoveModifiersFrom(Runtime);
        }
    }
}
