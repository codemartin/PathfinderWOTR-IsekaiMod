using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind
{
	internal class GodEmperorQuickFooted
	{
		private static readonly Sprite Icon_ExpeditiousRetreat = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("4f8181e7a7f1d904fbaea64220e83379")).m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "GodEmperorQuickFooted", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Quick-Footed");
				bp.SetDescription(Main.IsekaiContext, "At 15th level, you gain a competence {g|Encyclopedia:Bonus}bonus{/g} to your {g|Encyclopedia:Initiative}initiative{/g} {g|Encyclopedia:Check}checks{/g} equal to your {g|Encyclopedia:Wisdom}Wisdom{/g} modifier.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ExpeditiousRetreat;
				bp.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.BaseStat = StatType.Wisdom;
					c.DerivativeStat = StatType.Initiative;
				});
				bp.AddComponent(delegate(RecalculateOnStatChange c)
				{
					c.Stat = StatType.Wisdom;
				});
			});
		}
	}
}
