using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class OmegaStrike
	{
		private static readonly Sprite Icon_LethalStance = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e4450dd9c06dc034fb7c0c08abcc202b")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "OmegaStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Omega Strike");
				bp.SetDescription(Main.IsekaiContext, "Any {g|Encyclopedia:Attack}attacks{/g} you make have their {g|Encyclopedia:Damage}damage{/g} multiplier increased by 1 (×2 becomes ×3, for example).");
				((BlueprintUnitFact)bp).m_Icon = Icon_LethalStance;
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Melee;
					c.AdditionalMultiplier = 1;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Ranged;
					c.AdditionalMultiplier = 1;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Touch;
					c.AdditionalMultiplier = 1;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 7;
				});
			}));
		}
	}
}
