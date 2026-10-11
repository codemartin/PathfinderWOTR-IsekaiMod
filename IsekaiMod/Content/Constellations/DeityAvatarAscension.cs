using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations;

internal static class DeityAvatarAscension
{
	internal enum AscendRefusal
	{
		None,
		AlreadyEmbodied,
		OtherAvatarHeld
	}

	public static BlueprintFeature AvatarCaydenFeature;

	public static BlueprintFeature AvatarIomedaeFeature;

	public static BlueprintFeature AvatarAsmodeusFeature;

	public static BlueprintFeature AvatarDesnaFeature;

	public static BlueprintFeature AvatarPharasmaFeature;

	public static BlueprintFeature AvatarCalistriaFeature;

	public static BlueprintFeature AvatarNethysFeature;

	public static BlueprintFeature AvatarGorumFeature;

	public static BlueprintFeature AvatarBesmaraFeature;

	public static BlueprintFeature AvatarLanternKingFeature;

	public static BlueprintFeature AvatarChaldiraFeature;

	public static BlueprintFeature AvatarMilaniFeature;

	public static BlueprintFeature AvatarBlackButterflyFeature;

	public static BlueprintFeature GodDefierFeature;

	internal static readonly string[] AvatarBlueprintNames = new string[14]
	{
		"AvatarCaydenFeature", "AvatarIomedaeFeature", "AvatarAsmodeusFeature", "AvatarDesnaFeature", "AvatarPharasmaFeature", "AvatarCalistriaFeature", "AvatarNethysFeature", "AvatarGorumFeature", "AvatarBesmaraFeature", "AvatarLanternKingFeature",
		"AvatarChaldiraFeature", "AvatarMilaniFeature", "AvatarBlackButterflyFeature", "GodDefierFeature"
	};

	internal static bool IsAvatar(string blueprintName)
	{
		if (blueprintName != null)
		{
			return Array.IndexOf(AvatarBlueprintNames, blueprintName) >= 0;
		}
		return false;
	}

	internal static AscendRefusal CheckAscend(IEnumerable<string> ownedFeatureNames, string wanted, out string heldAvatar)
	{
		heldAvatar = null;
		foreach (string item in ownedFeatureNames ?? Enumerable.Empty<string>())
		{
			if (IsAvatar(item))
			{
				heldAvatar = item;
				if (item == wanted)
				{
					return AscendRefusal.AlreadyEmbodied;
				}
			}
		}
		if (heldAvatar != null)
		{
			return AscendRefusal.OtherAvatarHeld;
		}
		return AscendRefusal.None;
	}

	internal static List<string> AvatarsLostOnPatronSwitch(IEnumerable<string> ownedFeatureNames)
	{
		return (ownedFeatureNames ?? Enumerable.Empty<string>()).Where(IsAvatar).Distinct().ToList();
	}

	public static void Add()
	{
		Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		AvatarCaydenFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarCaydenFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Lucky Drunk");
			bp.SetDescription(Main.IsekaiContext, "You have attained the pinnacle of favor with Cayden Cailean. \nBenefit: You gain a +4 sacred bonus to attack rolls, AC and all saving throws, fast healing 5, and immunity to nausea and to fear and compulsion effects, whether they come from a spell or any other source.\nSacred bonuses do not stack with other sacred bonuses (for example the Ring of Omnipresence, the Rulebreaker Mythic Pact or the Angel boon); only the highest applies.\nThe compulsion immunity also refuses helpful compulsions from allies, such as the Rage spell and Blessing of Luck and Resolve, as the game's own compulsion immunity does.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Nauseated;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion;
			});
		});
		AvatarIomedaeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarIomedaeFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Inheritor");
			bp.SetDescription(Main.IsekaiContext, "You stand as the living standard of Iomedae's holy crusade. \nBenefit: You gain a +4 sacred bonus to AC, attack rolls and all saving throws, and a +6 sacred bonus to damage rolls.\nSacred bonuses do not stack with other sacred bonuses (for example the Ring of Omnipresence, the Rulebreaker Mythic Pact or the Angel boon); only the highest applies. This avatar overwrites Iomedae's own Vanguard of the Inheritor blessing (+3 sacred AC and saves), which adds nothing while you embody her.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 6;
			});
		});
		AvatarAsmodeusFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarAsmodeusFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Prince of Darkness");
			bp.SetDescription(Main.IsekaiContext, "You have bound your soul to the supreme contract of Hell. \nBenefit: You gain a +4 profane bonus to AC, attack rolls and the DC of all your abilities, immunity to fire, and immunity to harmful mind-affecting effects (charm, compulsion, confusion, daze, fear, sleep and despair), whether they come from a spell or any other source. Helpful mind-affecting effects from allies still reach you, except compulsions such as the Rage spell.\nProfane bonuses do not stack with other profane bonuses (for example the Overlord's Corrupt Aura or the Shadow Monarch's domain); only the highest applies.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 4;
				c.Descriptor = ModifierDescriptor.Profane;
			});
			bp.AddComponent(delegate(AddEnergyImmunity c)
			{
				c.Type = DamageEnergyType.Fire;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion;
			});
		});
		AvatarDesnaFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarDesnaFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Song of the Spheres");
			bp.SetDescription(Main.IsekaiContext, "The starlight of Desna guides your every step. \nBenefit: You gain a +30 foot sacred bonus to speed and a +4 sacred bonus to AC and all saving throws. Whenever a saving throw roll would fail, you roll it again and keep the better result. You are immune to paralysis and entanglement, and to curses, movement-impairing effects and sleep, whether they come from a spell or any other source.\nSacred bonuses do not stack with other sacred bonuses (for example the Ring of Omnipresence or the Angel boon); only the highest applies.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.Speed;
				c.Value = 30;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(ModifyD20 c)
			{
				c.Rule = RuleType.SavingThrow;
				c.RollsAmount = 1;
				c.TakeBest = true;
				c.RerollOnlyIfFailed = true;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Paralyzed;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Entangled;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Sleep | SpellDescriptor.MovementImpairing;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Sleep | SpellDescriptor.MovementImpairing;
			});
		});
		AvatarPharasmaFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarPharasmaFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Lady of Graves");
			bp.SetDescription(Main.IsekaiContext, "The final arbiter has appointed you as her sovereign agent. \nBenefit: You are immune to death effects, whether they come from a spell or any other source, to energy drain and to negative energy damage. You gain a +4 bonus to all saving throws, a +2 bonus to attack rolls and a +6 bonus to damage rolls. These bonuses are untyped and stack with everything.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
			bp.AddComponent<AddImmunityToEnergyDrain>();
			bp.AddComponent(delegate(AddEnergyImmunity c)
			{
				c.Type = DamageEnergyType.NegativeEnergy;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 6;
			});
		});
		AvatarCalistriaFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarCalistriaFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Savored Sting");
			bp.SetDescription(Main.IsekaiContext, "The intoxicating vengeance of Calistria flows in your blood. \nBenefit: You gain a +4 insight bonus to initiative, attack rolls, AC and Reflex saves, +4d6 sneak attack damage, and immunity to poison, whether it comes from a spell or any other source.\nInsight bonuses do not stack with other insight bonuses (for example the Aeon boon, Omniscient Mimicry or Grandmaster Transcendence); only the highest applies, and the sneak attack dice replace smaller insight sneak attack bonuses.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.Initiative;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SneakAttack;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Poison;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Poison;
			});
		});
		AvatarNethysFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarNethysFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the All-Seeing Eye");
			bp.SetDescription(Main.IsekaiContext, "The dual cosmic currents of absolute destruction and preservation converge upon your intellect. \nBenefit: You gain a +3 bonus to the DC of all your abilities, a +4 bonus on caster level checks to overcome spell resistance, a +4 bonus to all saving throws, a +2 bonus to attack rolls and a +4 bonus to damage rolls. You resist 10 acid, cold, electricity, fire and sonic damage, and you see invisible creatures.\nThe numeric bonuses are untyped and stack with everything. Energy resistance does not stack with other resistance to the same energy; the highest applies.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 3;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 4;
				c.CheckFact = false;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Acid;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Cold;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Electricity;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Fire;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Sonic;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddCondition c)
			{
				c.Condition = UnitCondition.SeeInvisibility;
			});
		});
		AvatarGorumFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarGorumFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of Our Lord in Iron");
			bp.SetDescription(Main.IsekaiContext, "The insatiable thunder of war fuels your sinews. \nBenefit: You gain a +6 bonus to Strength, a +4 bonus to Constitution and a +4 bonus to damage rolls, one extra attack at your highest attack bonus when you make a full attack, damage reduction 10/- that stacks with your other damage reduction, and immunity to fatigue and exhaustion. The bonuses are untyped and stack with everything.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Strength;
				c.Value = 6;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 4;
			});
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
				c.Penalized = false;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
				c.m_IsStackable = true;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Fatigued;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Exhausted;
			});
		});
		AvatarBesmaraFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarBesmaraFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Pirate Queen");
			bp.SetDescription(Main.IsekaiContext, "The untamed plunder of the oceans is yours to command. \nBenefit: You gain a +4 bonus to AC and all saving throws that stacks with everything, including your Plot Armor, a +4 luck bonus to attack rolls, a +4 bonus to combat maneuver bonus and defense, and fast healing 5.\nThe luck bonus to attack rolls does not stack with other luck bonuses to attack (for example Signature Attack); only the highest applies.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalCMD;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
		});
		AvatarLanternKingFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarLanternKingFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Laughing King");
			bp.SetDescription(Main.IsekaiContext, "The shapeshifting spark of the First World dances around your silhouette. \nBenefit: You gain a +6 dodge bonus to AC, a +6 bonus to Dexterity and a +4 bonus to Will saves. You are immune to paralysis and entanglement, and to confusion, whether it comes from a spell or any other source.\nDodge bonuses stack, but like all dodge bonuses this one is lost while you are flat-footed. The other bonuses are untyped and stack with everything.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 6;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Dexterity;
				c.Value = 6;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Paralyzed;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Entangled;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Confusion;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Confusion;
			});
		});
		BlueprintBuff ChaldiraAuraEffectBuff = TTCoreExtensions.CreateBuff("AvatarChaldiraAuraEffectBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Chaldira's Mischievous Fortune");
			bp.SetDescription(Main.IsekaiContext, "An ally of Chaldira's avatar stands near. This character gains a +4 luck bonus to all saving throws and a +3 luck bonus to attack rolls and AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveWill;
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
				c.Stat = StatType.AC;
				c.Value = 3;
			});
		});
		BlueprintAbilityAreaEffect ChaldiraAuraArea = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarChaldiraAuraArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddComponent(delegate(AbilityAreaEffectBuff c)
			{
				c.m_Buff = ChaldiraAuraEffectBuff.ToReference<BlueprintBuffReference>();
				c.Condition = new ConditionsChecker
				{
					Conditions = new Condition[1]
					{
						new ContextConditionIsCaster
						{
							Not = true
						}
					}
				};
			});
		});
		BlueprintBuff ChaldiraAuraAreaBuff = TTCoreExtensions.CreateBuff("AvatarChaldiraAuraAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Chaldira's Mischievous Fortune");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +4 luck bonus to all saving throws and a +3 luck bonus to attack rolls and AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = ChaldiraAuraArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		AvatarChaldiraFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarChaldiraFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Mischievous Friend");
			bp.SetDescription(Main.IsekaiContext, "The boundless good fortune of Chaldira shields your comrades. \nBenefit: Your allies within 30 feet gain a +4 luck bonus to all saving throws and a +3 luck bonus to attack rolls and AC. You gain a +2 bonus to AC and all saving throws that stacks with everything.\nYou do not receive the aura's luck bonuses: your Plot Armor already gives you a luck bonus that would replace them. The aura's luck bonuses do not stack with other luck bonuses your allies have; only the highest applies.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = ChaldiraAuraAreaBuff.ToReference<BlueprintBuffReference>();
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
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
				c.Value = 2;
			});
		});
		AvatarMilaniFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarMilaniFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Everbloom");
			bp.SetDescription(Main.IsekaiContext, "Milani's rose blooms again wherever the oppressed rise, and it has taken root in you. \nBenefit: You gain a +4 sacred bonus to AC and all saving throws, a +2 bonus to attack rolls and a +4 bonus to damage rolls, fast healing 5, and immunity to disease and bleed, whether they come from a spell or any other source.\nSacred bonuses do not stack with other sacred bonuses (for example the Ring of Omnipresence or the Angel boon); only the highest applies. The attack and damage bonuses are untyped and stack with everything.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Disease | SpellDescriptor.Bleed;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Disease | SpellDescriptor.Bleed;
			});
		});
		AvatarBlackButterflyFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarBlackButterflyFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of the Silence Between");
			bp.SetDescription(Main.IsekaiContext, "The silent dark between the stars folds around you, and blows meant for you fall into it. \nBenefit: Attacks against you have a 50% miss chance, as displacement (true seeing ignores it). You gain a +4 dodge bonus to AC, a +4 bonus to Will saves and initiative, a +4 bonus to damage rolls, and immunity to cold and sonic damage.\nThe miss chance does not stack with other concealment; the highest applies. Dodge bonuses stack, but are lost while you are flat-footed. The other bonuses are untyped and stack with everything.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddConcealment c)
			{
				c.Descriptor = ConcealmentDescriptor.Displacement;
				c.Concealment = Concealment.Total;
				c.OnlyForAttacks = true;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Initiative;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddEnergyImmunity c)
			{
				c.Type = DamageEnergyType.Cold;
			});
			bp.AddComponent(delegate(AddEnergyImmunity c)
			{
				c.Type = DamageEnergyType.Sonic;
			});
		});
		GodDefierFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "GodDefierFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Transcendence: The God Defier");
			bp.SetDescription(Main.IsekaiContext, "You have rejected every celestial throne, broken every divine chain, and stood tall as an unyielding mortal. The Constellations watch in stunned awe as your sheer will carves a sovereign sanctuary in reality.\nBenefit: Grants a +4 untyped bonus to all ability scores, immunity to curses and death effects whether they come from a spell or any other source, and a +8 untyped bonus to damage rolls.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Dexterity;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Intelligence;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Wisdom;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Charisma;
				c.Value = 4;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Death;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Death;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 8;
			});
		});
	}
}
