using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class Reflect
	{
		private static readonly Sprite Icon_ShieldOfDawn = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("62888999171921e4dafb46de83f4d67d")).m_Icon;

		public static void Add()
		{
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("Reflect", "You reflect 50% of the damage you receive back to attackers as direct damage (increases to 75% at level 15).", Icon_ShieldOfDawn, delegate(BlueprintBuff bp)
			{
				bp.AddComponent<ReflectDamage>();
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 7;
			});
			SpecialPowerSelection.AddToSelection(blueprintFeature);
		}
	}
}
