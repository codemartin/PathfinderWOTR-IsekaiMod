using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class GammaStrike
	{
		private static readonly Sprite Icon_BladeSense = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("112bf4c6943097942b24eadfa750215f")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToMartialSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "GammaStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Gamma Strike");
				bp.SetDescription(Main.IsekaiContext, "Your attacks ignore concealment and are treated as adamantite for the purpose of overcoming {g|Encyclopedia:Damage_Reduction}damage reduction{/g}.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_BladeSense;
				bp.AddComponent<IgnoreConcealment>();
				bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
				{
					c.AddMaterial = true;
					c.Material = PhysicalDamageMaterial.Adamantite;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
