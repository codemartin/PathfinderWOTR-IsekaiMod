using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class Supermassive
	{
		private static readonly Sprite Icon_TricksterMicroscopicProportions = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d6042abe75df262498b6021eb8cc07a5")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToMartialSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "Supermassive", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Supermassive");
				bp.SetDescription(Main.IsekaiContext, "You gain a size bonus to HP equal to your 10 times your Constitution modifier.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TricksterMicroscopicProportions;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.HitPoints;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
					c.Multiplier = 10;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
					c.m_Stat = StatType.Constitution;
				});
				bp.AddComponent(delegate(RecalculateOnStatChange c)
				{
					c.Stat = StatType.Constitution;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			}));
		}
	}
}
