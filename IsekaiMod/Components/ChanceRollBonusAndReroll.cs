using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using System;

namespace IsekaiMod.Components {

    /// <summary>
    /// With the configured chance, a d20 roll is rerolled (taking the better result) and gains a flat bonus.
    ///
    /// This replaces ModifyD20 with AddBonus for Merchant's Gamble. That component applies the bonus by
    /// temporarily modifying every stat on the character for every roll it triggers on, and each stat change
    /// cascades into dependent recalculations, which is enough to make combat stutter. Here the bonus is
    /// added to the rule that consumes the d20 instead, the same way PerfectRollRuleBonus does.
    ///
    /// The chance is rolled once per parent rule so the reroll and the bonus always land together.
    /// </summary>
    [TypeId("6f3d2c1b9a8e4d7fb0c5e4a3d2b1f0e9")]
    public class ChanceRollBonusAndReroll : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateAttackBonus>,
        IRulebookHandler<RuleCalculateAttackBonus>,
        IInitiatorRulebookHandler<RuleSkillCheck>,
        IRulebookHandler<RuleSkillCheck>,
        IInitiatorRulebookHandler<RuleInitiativeRoll>,
        IRulebookHandler<RuleInitiativeRoll>,
        IInitiatorRulebookHandler<RuleRollD20>,
        IRulebookHandler<RuleRollD20>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public int Chance = 20;
        public int Bonus = 4;
        public ModifierDescriptor Descriptor = ModifierDescriptor.Sacred;
        public bool RerollTakeBest = true;

        [NonSerialized]
        private RulebookEvent m_DecidedFor;
        [NonSerialized]
        private bool m_Decision;

        private bool Decide(RulebookEvent parent) {
            if (parent == null) return UnityEngine.Random.Range(1, 101) <= Chance;
            if (!ReferenceEquals(m_DecidedFor, parent)) {
                m_DecidedFor = parent;
                m_Decision = UnityEngine.Random.Range(1, 101) <= Chance;
            }
            return m_Decision;
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt) {
            if (Rulebook.CurrentContext?.PreviousEvent is RuleAttackRoll attackRoll && Decide(attackRoll)) {
                evt.AddModifier(Bonus, Fact, Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonus evt) {
        }

        public void OnEventAboutToTrigger(RuleSkillCheck evt) {
            if (Decide(evt)) {
                evt.AddModifier(Bonus, Fact, Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleSkillCheck evt) {
        }

        public void OnEventAboutToTrigger(RuleInitiativeRoll evt) {
            if (Decide(evt)) {
                evt.AddTemporaryModifier(evt.Initiator.Stats.Initiative.AddModifier(Bonus, Fact, Descriptor));
            }
        }

        public void OnEventDidTrigger(RuleInitiativeRoll evt) {
        }

        public void OnEventAboutToTrigger(RuleRollD20 evt) {
            if (evt.IsFake) return;
            RulebookEvent parent = Rulebook.CurrentContext?.PreviousEvent;
            if (!Decide(parent)) return;
            if (parent is RuleCheckConcentration concentration) {
                concentration.Concentration += Bonus;
            } else if (parent is RuleDispelMagic dispelMagic) {
                dispelMagic.Bonus += Bonus;
            }
            if (RerollTakeBest) {
                evt.AddReroll(1, true, Fact);
            }
        }

        public void OnEventDidTrigger(RuleRollD20 evt) {
        }
    }
}
