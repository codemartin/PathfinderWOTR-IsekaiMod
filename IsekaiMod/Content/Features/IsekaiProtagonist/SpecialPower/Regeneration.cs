using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Buffs.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;

internal class Regeneration
{
	public static void Add()
	{
		Sprite Icon_Regeneration = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_REGENERATION.png");
		SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "RegenerationFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Regeneration");
			bp.SetDescription(Main.IsekaiContext, "Your otherworldly flesh knits itself back together without pause: you regain 10 hit points per round. This healing is relentless, but it cannot hold back death itself.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Regeneration;
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 10;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 5;
			});
		}));
	}
}
