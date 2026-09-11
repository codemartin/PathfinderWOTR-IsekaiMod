using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class MagicalAmplification
	{
		public static void Add()
		{
			string description = "Any {g|Encyclopedia:Spell}spells{/g} dealing {g|Encyclopedia:Damage}damage{/g} now change their dice to d10, if it was not d10 or greater. If it already was d10 or greater, instead the spell deals one additional point of damage per dice rolled.";
			Sprite icon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("1a33199538c31a14db23318fdb6e10cb")).m_Icon;
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("MagicalAmplification", description, icon, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(PromoteSpellDices c)
				{
					c.MinDice = DiceType.D10;
					c.Bonus = 1;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 5;
			});
			SpecialPowerSelection.AddToSelection(blueprintFeature);
		}
	}
}
