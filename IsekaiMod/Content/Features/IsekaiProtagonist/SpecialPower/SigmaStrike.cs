using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class SigmaStrike
	{
		private static readonly Sprite Icon_OracleRevelationMightyPebbleAbility = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("2ae123c190625644e889517e15e8f640")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToMartialSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "SigmaStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Sigma Strike");
				bp.SetDescription(Main.IsekaiContext, "Your critical threat range is increased by 2.");
				((BlueprintUnitFact)bp).m_Icon = Icon_OracleRevelationMightyPebbleAbility;
				bp.AddComponent(delegate(WeaponCriticalEdgeIncreaseStackable c)
				{
					c.Value = 2;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			}));
		}
	}
}
