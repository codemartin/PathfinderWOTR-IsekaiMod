using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;

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
			bp.SetDescription(Main.IsekaiContext, "Empowered by the Overlord's benevolent majesty. Gains a +4 deflection bonus to AC and weapon attacks deal an additional 1d6 holy damage.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Deflection;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Holy
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 1,
					BonusValue = 0
				};
			});
		});
		BlueprintBuff AuraOfRighteousMajestyEffectBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "AuraOfRighteousMajestyEffectBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Aura of Righteous Majesty");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet of the benevolent sovereign gain a +4 sacred bonus to saving throws against fear and mind-affecting effects, and Fast Healing 3.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear;
				c.ModifierDescriptor = ModifierDescriptor.Sacred;
				c.Value = 4;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 3;
				c.Bonus = 0;
			});
		});
		BlueprintAbilityAreaEffect AuraOfRighteousMajestyArea = Helpers.CreateBlueprint(Main.IsekaiContext, "AuraOfRighteousMajestyArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(AuraOfRighteousMajestyEffectBuff.ToReference<BlueprintBuffReference>());
		});
		AuraOfRighteousMajestyBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "AuraOfRighteousMajestyBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Aura of Righteous Majesty");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet of the benevolent sovereign gain a +4 sacred bonus to saving throws against fear and mind-affecting effects, and Fast Healing 3.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = AuraOfRighteousMajestyArea.ToReference<BlueprintAbilityAreaEffectReference>();
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
		});
	}

	public static GameAction GrantGuardianBlessing()
	{
		return new Conditional
		{
			ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterHasFact c)
			{
				c.m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "AuraOfRighteousMajestyFeature");
			}),
			IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
			{
				b.m_Buff = BlueprintTools.GetModBlueprintReference<BlueprintBuffReference>(Main.IsekaiContext, "HallowedTombGuardianBuff");
				b.Permanent = true;
				b.DurationValue = Values.Duration.Zero;
				b.IsNotDispelable = true;
			}),
			IfFalse = ActionFlow.DoNothing()
		};
	}
}
