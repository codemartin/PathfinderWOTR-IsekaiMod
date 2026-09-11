using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class AbyssalCorsair
	{
		private static readonly Sprite Icon_Corsair = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_ENERGY_DARK.png");

		private static readonly BlueprintBuff FreedomOfMovementBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1533e782fca42b84ea370fc1dcbf4fc1");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "AbyssalCorsairFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Abyssal Corsair");
				bp.SetDescription(Main.IsekaiContext, "Master of the Midnight Isles and sovereign navigator of the Abyssal Ocean. You plunder planar riches with impunity.\nBenefit: You gain permanent Freedom of Movement, +30 feet to base movement speed, a +4 dodge bonus to Armor Class, and a +3 luck bonus to attack and damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Corsair;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				if (FreedomOfMovementBuff != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { FreedomOfMovementBuff.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 30;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 3;
				});
			}));
		}
	}
}
