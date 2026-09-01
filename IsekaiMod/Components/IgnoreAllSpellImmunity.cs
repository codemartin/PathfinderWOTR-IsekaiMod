using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using System.Linq;

namespace IsekaiMod.Components {

    /// <summary>
    /// Marks a caster whose spells ignore the target's spell immunities.
    ///
    /// The game's IgnoreSpellImmunity component is target-side: it makes its owner ignore its own
    /// immunities to a descriptor. Nothing in the game lets a caster bypass a target's immunity, so
    /// this marker is consulted from a postfix on UnitPartSpellResistance.IsImmune, the single check
    /// that RuleSpellResistanceCheck and area effects use. Self-targeted effects are already exempt
    /// inside that method, so the caster's own immunities are unaffected.
    /// </summary>
    [TypeId("7d53b46163704613978bdba6365a395f")]
    [AllowedOn(typeof(BlueprintFeature), false)]
    public class IgnoreAllSpellImmunity : UnitFactComponentDelegate {

        internal static bool Applies(UnitEntityData caster) {
            if (caster == null) return false;
            return caster.Descriptor.Facts.List.Any(fact => fact.Blueprint.GetComponent<IgnoreAllSpellImmunity>() != null);
        }

        [HarmonyPatch(typeof(UnitPartSpellResistance), nameof(UnitPartSpellResistance.IsImmune), typeof(MechanicsContext), typeof(bool))]
        private static class IsImmunePatcher {
            [HarmonyPostfix]
            private static void Postfix(MechanicsContext context, ref bool __result) {
                if (!__result) return;
                if (Applies(context?.MaybeCaster)) {
                    __result = false;
                }
            }
        }
    }
}
