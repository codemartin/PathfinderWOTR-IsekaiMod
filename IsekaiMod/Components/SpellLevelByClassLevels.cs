using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Linq;

namespace IsekaiMod.Components {
	[TypeId("45b3563fd3f94fe28acb66dde6dbeb88")]
	internal class SpellLevelByClassLevels : UnitFactComponentDelegate,
		IInitiatorRulebookHandler<RuleCalculateAbilityParams>,
		IRulebookHandler<RuleCalculateAbilityParams>,
		ISubscriber,
		IInitiatorRulebookSubscriber {

		public BlueprintAbility Ability => m_Ability?.Get();

		public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt) {
			if (evt.Spell != Ability) return;

			int classLevel = (m_Classes ?? Array.Empty<BlueprintCharacterClassReference>())
				.Select(characterClass => characterClass?.Get())
				.Where(characterClass => characterClass != null)
				.Select(characterClass => Owner.Progression.GetClassLevel(characterClass))
				.DefaultIfEmpty(0)
				.Max();
			evt.ReplaceCasterLevel = Math.Max(classLevel, 1);
		}

		public void OnEventDidTrigger(RuleCalculateAbilityParams evt) { }

		public BlueprintAbilityReference m_Ability;
		public BlueprintCharacterClassReference[] m_Classes = Array.Empty<BlueprintCharacterClassReference>();
	}
}
