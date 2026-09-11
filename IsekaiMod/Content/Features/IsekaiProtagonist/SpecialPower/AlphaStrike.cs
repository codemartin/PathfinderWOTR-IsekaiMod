using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class AlphaStrike
	{
		private static readonly Sprite Icon_SneakStab = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("df4f34f7cac73ab40986bc33f87b1a3c")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "AlphaStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Alpha Strike");
				bp.SetDescription(Main.IsekaiContext, "You have become an alpha. Your critical threats are automatically confirmed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_SneakStab;
				bp.AddComponent<InitiatorCritAutoconfirm>();
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
