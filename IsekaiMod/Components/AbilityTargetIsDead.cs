using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;

namespace IsekaiMod.Components {
    /// <summary>
    /// Target restriction: only a dead unit is a valid target. The game only ships dead-companion checkers,
    /// which is why Shadow Reaping: Devour Core could be pointed at anything.
    /// </summary>
    [AllowedOn(typeof(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbility), false)]
    [TypeId("2d7b4f1e6a9c4c3e8b5d0a1f7e6c9b2d")]
    public class AbilityTargetIsDead : BlueprintComponent, IAbilityTargetRestriction {
        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target) {
            return target?.Unit != null && target.Unit.Descriptor.State.IsDead;
        }

        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target) {
            return "Target must be dead";
        }
    }
}
