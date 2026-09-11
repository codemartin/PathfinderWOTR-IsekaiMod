using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class Afterimage
	{
		private static readonly Sprite Icon_Invisibility = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("89940cde01689fb46946b2f8cd7b66b7")).m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "Afterimage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Afterimage");
				bp.SetDescription(Main.IsekaiContext, "At 8th level, your movements are so fast that they leave an afterimage for your enemies.\nYou gain a +10 {g|Encyclopedia:Bonus}bonus{/g} to your {g|Encyclopedia:Speed}speed{/g} and any {g|Encyclopedia:Attack}attack{/g} targeting you has a 10% chance to miss.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Invisibility;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				bp.AddComponent(delegate(SetAttackerMissChance c)
				{
					c.m_Type = SetAttackerMissChance.Type.All;
					c.Value = 10;
					c.Conditions = ActionFlow.EmptyCondition();
				});
			});
		}
	}
}
