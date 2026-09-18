using IsekaiMod.Utilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.RuleSystem.Rules.Damage;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	internal static class OverlordBenevolentFeatures
	{
		private static readonly Sprite Icon_Aura = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon;

		private static readonly Sprite Icon_Crown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e6e1a24ce33454342b7053889391642c"))?.m_Icon ?? Icon_Aura;

		private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Aura;

		public static BlueprintFeature OverlordChannelEnergyFeature;

		public static BlueprintFeature AuraOfRighteousMajestyFeature;

		public static BlueprintBuff AuraOfRighteousMajestyBuff;

		public static BlueprintBuff HallowedTombGuardianBuff;

		public static void Add()
		{
			BlueprintFeature PositiveChannel = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiChannelPositiveEnergyFeature");
			BlueprintFeature NegativeChannel = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiChannelNegativeEnergyFeature");
			OverlordChannelEnergyFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordChannelEnergyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overlord Sovereignty Channeling");
				bp.SetDescription(Main.IsekaiContext, "As an absolute sovereign, your command extends over life and death alike. You gain the ability to channel both positive and negative energy, allowing you to heal living followers, bolster or create undead, and annihilate foes.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					List<BlueprintUnitFactReference> list = new List<BlueprintUnitFactReference>();
					if (PositiveChannel != null)
					{
						list.Add(PositiveChannel.ToReference<BlueprintUnitFactReference>());
					}
					if (NegativeChannel != null)
					{
						list.Add(NegativeChannel.ToReference<BlueprintUnitFactReference>());
					}
					c.m_Facts = list.ToArray();
				});
			});
			HallowedTombGuardianBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "HallowedTombGuardianBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Hallowed Tomb Guardian");
				bp.SetDescription(Main.IsekaiContext, "Empowered by the Overlord's benevolent majesty. Gains a +4 deflection bonus to AC and weapon attacks deal an additional 1d6 holy and unholy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Holy };
				});
			});
			AuraOfRighteousMajestyBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "AuraOfRighteousMajestyBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Aura of Righteous Majesty");
				bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet of the benevolent sovereign gain a +4 sacred or profane bonus to saving throws against fear and mind-affecting effects, and Fast Healing 3.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 3;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;
				});
			});
			AuraOfRighteousMajestyFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AuraOfRighteousMajestyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Aura of Righteous Majesty");
				bp.SetDescription(Main.IsekaiContext, "You govern your domain with regal benevolence and awe-inspiring presence. You and all allies within 30 feet gain a +4 bonus to saving throws against fear and mind-affecting effects, and Fast Healing 3. Furthermore, your summoned guardians gain a +4 deflection bonus to AC and deal +1d6 extra damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { AuraOfRighteousMajestyBuff.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(BuffExtraEffects c)
				{
					c.m_CheckedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("706c182e86d9be848b59ddccca73d13e")?.ToReference<BlueprintBuffReference>();
					c.m_ExtraEffectBuff = HallowedTombGuardianBuff.ToReference<BlueprintBuffReference>();
				});
			});
		}
	}
}
