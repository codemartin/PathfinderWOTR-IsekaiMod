using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Linq;

namespace IsekaiMod.Components {
    [TypeId("1a778ad6d5c7495cb4d058ac451ce6db")]
    internal class AddClassLevelsToSummonDuration : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleSummonUnit>,
        IRulebookHandler<RuleSummonUnit>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleSummonUnit evt) {
            var ability = evt.Reason.Ability;
            if (ability?.Spellbook == null || ability.SpellSchool != SpellSchool.Conjuration) return;

            int classLevel = (m_CharacterClasses ?? Array.Empty<BlueprintCharacterClassReference>())
                .Select(characterClass => characterClass?.Get())
                .Where(characterClass => characterClass != null)
                .Select(characterClass => Owner.Progression.GetClassLevel(characterClass))
                .DefaultIfEmpty(0)
                .Max();
            if (Half) {
                classLevel = Math.Max(classLevel / 2, 1);
            }
            evt.BonusDuration += classLevel.Rounds();
        }

        public void OnEventDidTrigger(RuleSummonUnit evt) { }

        public bool Half;
        public BlueprintCharacterClassReference[] m_CharacterClasses = Array.Empty<BlueprintCharacterClassReference>();
    }
}
