using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace IsekaiMod.Components {

    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("e17809fff3724ec6a528a0c40078232a")]
    public class ApplyPartyBuffOnKill : UnitFactComponentDelegate<ApplyPartyBuffOnKill.ComponentData>,
        IInitiatorRulebookHandler<RuleDealDamage>,
        IInitiatorRulebookHandler<RuleDealStatDamage>,
        IInitiatorRulebookHandler<RuleDrainEnergy>,
        IRulebookHandler<RuleDealDamage>,
        IRulebookHandler<RuleDealStatDamage>,
        IRulebookHandler<RuleDrainEnergy>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public class ComponentData {
            public bool WasTargetAlive;
        }

        public BlueprintBuffReference m_Buff;

        public void OnEventAboutToTrigger(RuleDealDamage evt) => RememberTargetState(evt);
        public void OnEventAboutToTrigger(RuleDealStatDamage evt) => RememberTargetState(evt);
        public void OnEventAboutToTrigger(RuleDrainEnergy evt) => RememberTargetState(evt);

        public void OnEventDidTrigger(RuleDealDamage evt) => ApplyBuffIfKilled(evt);
        public void OnEventDidTrigger(RuleDealStatDamage evt) => ApplyBuffIfKilled(evt);
        public void OnEventDidTrigger(RuleDrainEnergy evt) => ApplyBuffIfKilled(evt);

        private void RememberTargetState(RulebookTargetEvent evt) {
            Data.WasTargetAlive = evt.Target != null && !evt.Target.Descriptor.State.IsDead;
        }

        private void ApplyBuffIfKilled(RulebookTargetEvent evt) {
            bool isDead = evt.Target != null
                && (evt.Target.Descriptor.State.IsDead || evt.Target.Stats.HitPoints <= evt.Target.Damage);
            if (!Data.WasTargetAlive || !isDead || !Owner.Group.IsEnemy(evt.Target.Group)
                || Fact is not IFactContextOwner factContextOwner) {
                return;
            }
            var actions = IsekaiMod.Utilities.ActionFlow.DoSingle<ContextActionPartyMembers>(party => {
                party.Action = IsekaiMod.Utilities.ActionFlow.DoSingle<ContextActionApplyBuff>(apply => {
                    apply.m_Buff = m_Buff;
                    apply.DurationValue = new ContextDurationValue() {
                        Rate = DurationRate.Minutes,
                        DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                        DiceCountValue = 0,
                        BonusValue = 1
                    };
                    apply.IsFromSpell = false;
                });
            });
            factContextOwner.RunActionInContext(actions, Owner);
        }
    }
}
