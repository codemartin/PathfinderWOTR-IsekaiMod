using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.ActivatableAbilities;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class BetaStrike
	{
		private static readonly Sprite Icon_ArcaneWeaponSpeed = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("85742dd6788c6914f96ddc4628b23932")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToMartialSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BetaStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Beta Strike");
				bp.SetDescription(Main.IsekaiContext, "You get an additional {g|Encyclopedia:Attack}attack{/g} per {g|Encyclopedia:Combat_Round}round{/g}.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ArcaneWeaponSpeed;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = false;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
