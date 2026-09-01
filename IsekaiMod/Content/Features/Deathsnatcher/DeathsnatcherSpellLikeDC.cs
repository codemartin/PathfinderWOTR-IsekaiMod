using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.Deathsnatcher {

    /// <summary>
    /// Save DC for the Deathsnatcher's spell-like abilities: 10 + half its level + its Charisma modifier.
    /// Without an explicit DC source the game falls back to 10 + spell level 0 + Charisma, which never scales.
    /// </summary>
    internal class DeathsnatcherSpellLikeDC {

        public static void Add() {
            Helpers.CreateBlueprint<BlueprintUnitProperty>(IsekaiContext, "DeathsnatcherSpellLikeDCProperty", bp => {
                bp.name = "DeathsnatcherSpellLikeDCProperty";
                bp.AddComponent<SimplePropertyGetter>(c => {
                    c.Property = UnitProperty.Level;
                    c.Settings = new PropertySettings() {
                        m_Progression = PropertySettings.Progression.Div2
                    };
                });
                bp.AddComponent<SimplePropertyGetter>(c => {
                    c.Property = UnitProperty.StatBonusCharisma;
                });
                bp.BaseValue = 10;
                bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
            });
        }

        public static BlueprintUnitProperty Get() {
            return BlueprintTools.GetModBlueprint<BlueprintUnitProperty>(IsekaiContext, "DeathsnatcherSpellLikeDCProperty");
        }
    }
}
