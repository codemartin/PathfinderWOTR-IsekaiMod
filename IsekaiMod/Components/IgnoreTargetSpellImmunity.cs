using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Parts;
using System.Runtime.CompilerServices;

namespace IsekaiMod.Components {
    /// <summary>
    /// Makes the owner's spells and abilities ignore the target's spell immunities (Spell Immunity, Globe of
    /// Invulnerability, golem-style immunity to spells, immunity to a spell descriptor or school).
    ///
    /// The game's own IgnoreSpellImmunity component does the opposite of what its name suggests for this
    /// purpose: it makes the OWNER stop being immune to a descriptor (used for liches accepting negative
    /// energy healing). There is no vanilla caster-side "my spells bypass immunity" component; the closest
    /// thing is the per-immunity CasterIgnoreImmunityFact, which each immunity has to name explicitly. So
    /// this component marks the caster and a small patch on UnitPartSpellResistance.CanApply answers "not
    /// immune" whenever the caster is marked. Energy resistance and immunity (a demon's immunity to
    /// electricity) are damage-side rules and are not touched here.
    /// </summary>
    [TypeId("3f5a2c1e9b7d4e0a8c6f1b2d3e4a5c6f")]
    public class IgnoreTargetSpellImmunity : UnitFactComponentDelegate {
        private sealed class Marker { public int Count; }

        // Not a UnitPart on purpose: parts are serialised into saves, and a save that names a mod type breaks
        // when the mod is absent. OnTurnOn runs again on load, so a weak table keyed by the unit is enough.
        private static readonly ConditionalWeakTable<UnitDescriptor, Marker> marked = new ConditionalWeakTable<UnitDescriptor, Marker>();

        public override void OnTurnOn() {
            base.OnTurnOn();
            if (Owner == null) return;
            marked.GetOrCreateValue(Owner).Count++;
        }

        public override void OnTurnOff() {
            if (Owner != null && marked.TryGetValue(Owner, out Marker m) && m.Count > 0) m.Count--;
            base.OnTurnOff();
        }

        internal static bool IsMarked(UnitEntityData unit) {
            return unit?.Descriptor != null && marked.TryGetValue(unit.Descriptor, out Marker m) && m.Count > 0;
        }

        [HarmonyPatch(typeof(UnitPartSpellResistance), "CanApply",
            typeof(UnitPartSpellResistance.SpellImmunity), typeof(AbilityData), typeof(UnitEntityData), typeof(Kingmaker.Blueprints.Classes.Spells.SpellDescriptor?), typeof(BlueprintAbility))]
        private static class CanApplyPatch {
            // Prefix so the immunity's own exceptions and alignment checks are simply skipped for a marked caster.
            private static bool Prefix(UnitEntityData caster, ref bool __result) {
                if (!IsMarked(caster)) return true;
                __result = false;
                return false;
            }
        }
    }
}
