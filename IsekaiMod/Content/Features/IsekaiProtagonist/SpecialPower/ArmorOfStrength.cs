using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class ArmorOfStrength
	{
		private static readonly Sprite Icon_ArmoredHulkIndomitableStance = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("74c59090138e28f4687c8a3400030763")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToDefenseSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ArmorOfStrength", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Armor of Strength");
				bp.SetDescription(Main.IsekaiContext, "You gain a natural armor bonus to AC equal to your strength modifier.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ArmoredHulkIndomitableStance;
				bp.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.BaseStat = StatType.Strength;
					c.DerivativeStat = StatType.AC;
				});
				bp.AddComponent(delegate(RecalculateOnStatChange c)
				{
					c.Stat = StatType.Strength;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
