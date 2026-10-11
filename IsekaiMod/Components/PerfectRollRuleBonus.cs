using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components {

    /// <summary>
    /// Applies Perfect Roll's flat bonus to the rule that consumes the d20.
    /// The base-game ModifyD20 AddBonus path temporarily modifies almost every
    /// character stat for every roll, which causes excessive recalculation.
    /// </summary>
    [TypeId("0c716724b2a344fb83ff778c58064c55")]
    public class PerfectRollRuleBonus : UnitFactComponentDelegate,
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

        public int Bonus = 5;
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;

        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt) {
            // Combat maneuvers can also calculate attack bonus, but their final
            // d20 is handled by RuleSkillCheck. Only modify a true attack roll.
            if (Rulebook.CurrentContext?.PreviousEvent is RuleAttackRoll) {
                evt.AddModifier(Bonus, Fact, Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonus evt) {
        }

        public void OnEventAboutToTrigger(RuleSkillCheck evt) {
            // Saving throws, combat maneuvers, and spell-resistance checks all
            // resolve through RuleSkillCheck as well as ordinary skill checks.
            evt.AddModifier(Bonus, Fact, Descriptor);
        }

        public void OnEventDidTrigger(RuleSkillCheck evt) {
        }

        public void OnEventAboutToTrigger(RuleInitiativeRoll evt) {
            // Initiative reads its stat during OnTrigger, so modify that one
            // stat temporarily instead of every stat on the character.
            evt.AddTemporaryModifier(
                evt.Initiator.Stats.Initiative.AddModifier(Bonus, Fact, Descriptor));
        }

        public void OnEventDidTrigger(RuleInitiativeRoll evt) {
        }

        public void OnEventAboutToTrigger(RuleRollD20 evt) {
            // Concentration and dispel checks use a raw RuleRollD20 rather than
            // RuleSkillCheck. At this point their calculated bonus is available.
            RulebookEvent previousEvent = Rulebook.CurrentContext?.PreviousEvent;
            if (previousEvent is RuleCheckConcentration concentration) {
                concentration.Concentration += Bonus;
            } else if (previousEvent is RuleDispelMagic dispelMagic) {
                dispelMagic.Bonus += Bonus;
            }
        }

        public void OnEventDidTrigger(RuleRollD20 evt) {
        }
    }
}
