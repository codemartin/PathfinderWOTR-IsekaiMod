using IsekaiMod.Components;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class FullCounter
	{
		private static readonly Sprite Icon_FullCounter = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_BARRIER_GOLD.png");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "FullCounterFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Full Counter");
				bp.SetDescription(Main.IsekaiContext, "Drawing upon legendary anime counter techniques, you deflect, reflect, and punish any hostility directed at your person.\nBenefit: You reflect 50% of all incoming damage back to the attacker as direct damage (increasing to 75% at level 15). You gain a +4 shield bonus to Armor Class, Damage Reduction 10/-, and +10 additional Attacks of Opportunity per round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_FullCounter;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent<ReflectDamage>();
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AttackOfOpportunityCount;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			}));
		}
	}
}
