using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero
{
	internal class HandsOfSalvation
	{
		private static readonly Sprite Icon_BlessingOfLuckAndResolveMass = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("462c21cebf7820c40a87f5e4d03e17cf")).m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "HandsOfSalvation", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Hands of Salvation");
				bp.SetDescription(Main.IsekaiContext, "The Hero heals allies for double the amount.");
				((BlueprintUnitFact)bp).m_Icon = Icon_BlessingOfLuckAndResolveMass;
				bp.AddComponent(delegate(ModifyOutgoingHealAmount c)
				{
					c.m_Facts = new BlueprintUnitFactReference[0];
					c.MultiplierIfHasNoFacts = 2f;
				});
			});
		}
	}
}
