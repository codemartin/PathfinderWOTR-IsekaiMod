using IsekaiMod.Components;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class ArmorSaint
	{
		private static readonly Sprite Icon_ArmorSaint = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("ae177f17cfb45264291d4d7c2cb64671")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToDefenseSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ArmorSaint", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Armor Saint");
				bp.SetDescription(Main.IsekaiContext, "You can move at normal speed while wearing armor. You also reduce your armor check penalty to zero and increase your max dexterity bonus by 20.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ArmorSaint;
				bp.AddComponent<ArmorNoSpeedPenalty>();
				bp.AddComponent(delegate(ArmorCheckPenaltyIncrease c)
				{
					c.Bonus = 100;
				});
				bp.AddComponent(delegate(MaxDexBonusIncrease c)
				{
					c.Bonus = 20;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			}));
		}
	}
}
