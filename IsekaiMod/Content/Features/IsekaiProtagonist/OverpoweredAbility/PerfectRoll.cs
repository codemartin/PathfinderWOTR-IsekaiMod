using IsekaiMod.Utilities;
using IsekaiMod.Components;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
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

                    bp.AddComponent<PerfectRollRuleBonus>(c => {
                        c.Bonus = 5;
                        c.Descriptor = ModifierDescriptor.UntypedStackable;
                    });
                });

            // Add the feature to the Overpowered Ability selection
            OverpoweredAbilitySelection.AddToSelection(PerfectRollFeature);
        }
    }
}

