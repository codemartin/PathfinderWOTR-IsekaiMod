using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class MundaneAura
	{
		private static readonly Sprite Icon_BardLoreMaster = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("4bea694e79a87cd4d8c14fb91578059e")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToAuthoritySelection(Helpers.CreateBlueprint(Main.IsekaiContext, "MundaneAura", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Mundane Aura");
				bp.SetDescription(Main.IsekaiContext, "You emit a subtle aura of complete mundanity, granting you immunity to precision damage, sneak attacks, and {g|Encyclopedia:Critical}critical hits{/g}.");
				((BlueprintUnitFact)bp).m_Icon = Icon_BardLoreMaster;
				bp.AddComponent<AddImmunityToCriticalHits>();
				bp.AddComponent<AddImmunityToPrecisionDamage>();
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			}));
		}
	}
}
