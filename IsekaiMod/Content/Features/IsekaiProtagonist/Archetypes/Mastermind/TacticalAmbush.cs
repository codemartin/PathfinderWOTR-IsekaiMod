using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind
{
	internal class TacticalAmbush
	{
		private static readonly BlueprintFeature SneakAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");

		private static readonly Sprite Icon_Tactical = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87"))?.m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "TacticalAmbushFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tactical Ambush");
				bp.SetDescription(Main.IsekaiContext, "Through meticulous calculation and weakness exploitation, the Mastermind strikes where enemies are least prepared. You gain +1d6 sneak attack damage against flanked or flat-footed foes, and add your Intelligence modifier to all weapon damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Tactical;
				bp.IsClassFeature = true;
				bp.Ranks = 5;
				if (SneakAttack != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { SneakAttack.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.BaseStat = StatType.Intelligence;
					c.DerivativeStat = StatType.AdditionalDamage;
					c.Descriptor = ModifierDescriptor.Insight;
				});
			});
		}
	}
}
