using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components {
    /// <summary>
    /// The owner's energy damage ignores the target's energy resistance and energy immunity, for every energy
    /// type at once. This is the game's own Ascendant Element mythic ability (which does it for one element)
    /// applied to all of them: the same DamageDecline flag and IgnoreReduction bit, so the combat log shows it
    /// the same way. Used by Second Reincarnation so "your spells ignore immunity" covers a demon's immunity
    /// to electricity and not only spell immunity proper.
    ///
    /// Applies to damage dealt by an ability (spells, kineticist blasts, bomb splashes); weapon energy dice
    /// are left alone because the feature text talks about spells.
    /// </summary>
    [TypeId("6b1d0e7a4c2f4a8e9d3b5c7f1e2a4d6c")]
    public class IgnoreEnergyResistanceAndImmunity : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber {
        public bool OnlyAbilities = true;

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            if (OnlyAbilities && evt.Reason?.Ability == null) return;
            foreach (BaseDamage item in evt.DamageBundle) {
                if (item is EnergyDamage energyDamage) {
                    item.AddDecline(new DamageDecline(DamageDeclineType.None, Fact) { AscendantElement = true });
                    energyDamage.IgnoreReduction = true;
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) { }
    }
}
