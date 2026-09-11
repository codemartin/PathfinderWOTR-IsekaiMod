using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class Unreactable
	{
		private static readonly Sprite Icon_TieflingHeritageDiv = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e3d324eb309cdf44a87d666c7a27715c")).m_Icon;

		public static void Add()
		{
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("Unreactable", "Enemies you attack are treated as flat-footed.", Icon_TieflingHeritageDiv, delegate(BlueprintBuff bp)
			{
				bp.AddComponent<SetFlatFootedOnAttack>();
			});
			blueprintFeature.AddPrerequisite(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 15;
			});
			SpecialPowerSelection.AddToSelection(blueprintFeature);
		}
	}
}
