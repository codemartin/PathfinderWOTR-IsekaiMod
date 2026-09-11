using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components {
    /// <summary>
    /// Hax - Serious Strike (beta 2025-12-19 description): every weapon attack is an
    /// automatic critical threat that automatically confirms, and the resulting
    /// weapon damage is doubled.
    /// </summary>
    [AllowMultipleComponents]
    [TypeId("3f0a6c1e9b7d4e2a8c5f1d7b2a9e4c61")]
    public class SeriousStrikeLogic : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleAttackRoll>,
        IInitiatorRulebookHandler<RuleDealDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
            if (evt.Weapon == null) return;
            evt.AutoCriticalThreat = true;
            evt.AutoCriticalConfirmation = true;
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
        }

        public void OnEventAboutToTrigger(RuleDealDamage evt) {
            // Only weapon strikes; spells and area effects are untouched.
            if (evt.AttackRoll == null || evt.DamageBundle?.Weapon == null) return;
            // ModifierBonus is applied as (1 + ModifierBonus) x damage in RuleCalculateDamage.
            evt.ModifierBonus = (evt.ModifierBonus ?? 0f) + 1f;
        }

        public void OnEventDidTrigger(RuleDealDamage evt) {
        }
    }
}
