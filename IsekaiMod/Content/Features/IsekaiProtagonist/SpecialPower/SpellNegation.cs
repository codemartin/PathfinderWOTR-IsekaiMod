using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;

internal class SpellNegation
{
	private static readonly Sprite Icon_SpellResistance = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0a5ddfbcfb3989543ac7c936fc256889"))?.m_Icon;

	public static void Add()
	{
		BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("SpellNegation", "You project an arcane disruption field, gaining a +4 resistance bonus to all saving throws against spells and spell-like abilities. Unlike spell resistance, this never turns aside your allies' magic.", Icon_SpellResistance, delegate(BlueprintBuff bp)
		{
			AbilityType[] array = new AbilityType[2]
			{
				AbilityType.Spell,
				AbilityType.SpellLike
			};
			foreach (AbilityType abilityType in array)
			{
				AbilityType type = abilityType;
				bp.AddComponent(delegate(SavingThrowBonusAgainstAbilityType c)
				{
					c.AbilityType = type;
					c.ModifierDescriptor = ModifierDescriptor.Resistance;
					c.Value = 4;
					c.Bonus = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 0
					};
				});
			}
		});
		blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
		{
			c.Level = 7;
		});
		SpecialPowerSelection.AddToSelection(blueprintFeature);
	}
}
