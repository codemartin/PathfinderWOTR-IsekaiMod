using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class PrimalChimera
	{
		private static readonly Sprite Icon_Chimera = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_FEROCIOUS_SUMMONING.png");

		private static readonly BlueprintFeature Pounce = BlueprintTools.GetBlueprint<BlueprintFeature>("1a8149c09e0bdfc48a305ee6ac3729a8");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "PrimalChimeraFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Primal Chimera");
				bp.SetDescription(Main.IsekaiContext, "Infused with the primeval animal spirits of lost Sarkoris, your physical form achieves primordial predatory perfection.\nBenefit: You gain Pounce (can make a full attack on a charge), +4 natural armor bonus to AC, +4 enhancement bonus to attack and damage rolls, and DR 10/Cold Iron.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Chimera;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				if (Pounce != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { Pounce.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.ColdIron;
				});
			}));
		}
	}
}
