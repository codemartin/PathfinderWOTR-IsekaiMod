using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal static class Port62ForbiddenCheats
{
	internal const int ManaWellspringPrice = 5000;

	internal const int RealityPiercerPrice = 10000;

	internal const int AegisUndyingPrice = 6000;

	internal const int DimensionalSanctuaryPrice = 4000;

	internal const int ManaWellspringSlots = 4;

	internal static readonly int[] ManaWellspringLevels = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

	internal const int RealityPiercerSpellPenetration = 6;

	internal const int AegisHeal = 150;

	internal const int AegisThresholdPercent = 25;

	internal const int AegisRounds = 3;

	internal const int SanctuaryEncumbrance = 1000000;

	private const string InvisibilityGreaterBuff = "e6b35473a237a6045969253beb09777c";

	public static void Add()
	{
		Sprite coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		Helpers.CreateBlueprint(Main.IsekaiContext, "ManaWellspringFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Forbidden Cheat: Mana Wellspring");
			bp.SetDescription(Main.IsekaiContext, "An inexhaustible fountain of otherworld mana courses through your soul. Grants +4 additional spell slots per day for all spell tiers from 1st through 9th circle. The 10th circle of a mythic spellbook gains 4 slots as well.");
			((BlueprintUnitFact)bp).m_Icon = coin;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddSpellsPerDay c)
			{
				c.Amount = 4;
				c.Levels = (int[])ManaWellspringLevels.Clone();
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "RealityPiercerFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Forbidden Cheat: Reality Piercer");
			bp.SetDescription(Main.IsekaiContext, "Your attacks shear through reality itself. All physical and magical attacks completely ignore Damage Reduction and Concealment, and you gain a +6 untyped bonus to Spell Penetration.");
			((BlueprintUnitFact)bp).m_Icon = coin;
			bp.IsClassFeature = true;
			bp.AddComponent<IgnoreAllDamageReduction>();
			bp.AddComponent<IgnoreConcealment>();
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 6;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
		});
		BlueprintBuff invisibility = BlueprintTools.GetBlueprint<BlueprintBuff>("e6b35473a237a6045969253beb09777c");
		BlueprintBuff aegisBuff = TTCoreExtensions.CreateBuff("AegisUndyingBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Aegis of the Undying Awakening");
			bp.SetDescription(Main.IsekaiContext, "Protected by the impenetrable aegis of destiny. Restores 150 Hit Points and grants Greater Invisibility for 3 rounds.");
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)invisibility)?.m_Icon ?? coin;
			if (invisibility != null)
			{
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { invisibility.ToReference<BlueprintUnitFactReference>() };
				});
			}
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "AegisUndyingFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Forbidden Cheat: Aegis of the Undying");
			bp.SetDescription(Main.IsekaiContext, "Whenever a hit that deals damage leaves you alive below 25% of your hit points, the cosmic aegis triggers, instantly healing you for 150 hit points and shrouding you in Greater Invisibility for 3 rounds. It can trigger at most once every 6 seconds (one round). Stacks seamlessly with all other survival powers.");
			((BlueprintUnitFact)bp).m_Icon = coin;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AegisUndyingTrigger c)
			{
				c.Heal = 150;
				c.ThresholdPercent = 25;
				c.Rounds = 3;
				c.m_Buff = aegisBuff.ToReference<BlueprintBuffReference>();
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "DimensionalSanctuaryFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Forbidden Cheat: Dimensional Sanctuary");
			bp.SetDescription(Main.IsekaiContext, "An otherworldly pocket sanctuary extends around you and your company. Party encumbrance capacity is increased by 1,000,000 lbs, and you gain permanent immunity to Fatigue and Exhaustion.");
			((BlueprintUnitFact)bp).m_Icon = coin;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddPartyEncumbrance c)
			{
				c.Value = 1000000;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Fatigued;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Exhausted;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
			});
		});
	}
}
