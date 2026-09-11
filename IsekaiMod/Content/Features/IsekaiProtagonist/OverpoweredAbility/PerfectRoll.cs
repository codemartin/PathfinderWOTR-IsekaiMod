using IsekaiMod.Components;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class PerfectRoll
	{
		public static void Add()
		{
			Sprite Icon_TrickFate = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd")).m_Icon;
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PerfectRollFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Perfect Roll");
				bp.SetDescription(Main.IsekaiContext, "You navigate every fight with perfect accuracy. Every word is predicted; every action is foreseen. Your premonitions guide you on your quest, as if you have experienced this before...\nBenefit: You gain a +5 bonus to all d20 rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrickFate;
				// ModifyD20 with RuleType.All ran a per-roll handler on every d20 (including the
				// engine's simulated preview/AI rolls) and caused in-combat stutter. Apply the
				// flat bonus to the consuming rule instead (see PerfectRollRuleBonus).
				bp.AddComponent(delegate(PerfectRollRuleBonus c)
				{
					c.Bonus = 5;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
