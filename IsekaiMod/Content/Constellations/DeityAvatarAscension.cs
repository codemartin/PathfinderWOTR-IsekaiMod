using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations
{
	internal static class DeityAvatarAscension
	{
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

		public static BlueprintFeature GodDefierFeature;

		public static void Add()
		{
			Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			AvatarCaydenFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarCaydenFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the Lucky Drunk");
				bp.SetDescription(Main.IsekaiContext, "You have attained the pinnacle of favor with Cayden Cailean. \nBenefit: Grants a +4 Sacred bonus to attack rolls, saving throws, and AC. You gain Fast Healing 5, and immunity to fear, nausea, and compulsion.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
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
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Nauseated;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Nauseated;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion;
				});
			});
			AvatarIomedaeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarIomedaeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the Inheritor");
				bp.SetDescription(Main.IsekaiContext, "You stand as the living standard of Iomedae's holy crusade. \nBenefit: All weapon attacks deal an additional 2d6 Holy damage. You gain a +4 Sacred bonus to AC, attack rolls, and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
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
				bp.SetDescription(Main.IsekaiContext, "You have bound your soul to the supreme contract of Hell. \nBenefit: Grants a +4 Profane bonus to all spell DCs, attack rolls, and AC. You are immune to mind-affecting effects and fire damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
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
					c.Descriptor = SpellDescriptor.MindAffecting;
				});
			});
			AvatarDesnaFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarDesnaFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the Song of the Spheres");
				bp.SetDescription(Main.IsekaiContext, "The starlight of Desna guides your every step. \nBenefit: Grants a +30 foot bonus to base speed, +4 Sacred bonus to Reflex saves and AC, and complete immunity to curses and movement-impairing effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Speed;
					c.Value = 30;
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
					c.Stat = StatType.AC;
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
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Paralysis;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Paralysis;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.MovementImpairing;
				});
			});
			AvatarPharasmaFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarPharasmaFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the Lady of Graves");
				bp.SetDescription(Main.IsekaiContext, "The final arbiter has appointed you as her sovereign agent. \nBenefit: You are immune to death effects, energy drain, and ability score drain. Weapon attacks deal +3d6 positive damage against undead and fiends.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
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
					c.Stat = StatType.AdditionalDamage;
					c.Value = 6;
				});
			});
			AvatarCalistriaFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarCalistriaFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the Savored Sting");
				bp.SetDescription(Main.IsekaiContext, "The intoxicating vengeance of Calistria flows in your blood. \nBenefit: +4 Insight bonus to Initiative and Sneak Attack damage. Whenever you strike a critical hit, the foe is sickened with venom.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
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
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			});
			AvatarNethysFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarNethysFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the All-Seeing Eye");
				bp.SetDescription(Main.IsekaiContext, "The dual cosmic currents of absolute destruction and preservation converge upon your intellect. \nBenefit: Grants +4 Spell Penetration and a +3 bonus to the caster level and DC of all spells.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
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
			});
			AvatarGorumFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarGorumFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of Our Lord in Iron");
				bp.SetDescription(Main.IsekaiContext, "The insatiable thunder of war fuels your sinews. \nBenefit: Grants DR 10/-, immunity to fatigue and exhaustion, and a +4 Untyped bonus to melee damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Fatigued;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Exhausted;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
			});
			AvatarBesmaraFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarBesmaraFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the Pirate Queen");
				bp.SetDescription(Main.IsekaiContext, "The untamed plunder of the oceans is yours to command. \nBenefit: Grants a +4 Luck bonus to AC, attack rolls, and saving throws, along with Fast Healing 3.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
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
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 3;
					c.Bonus = 0;
				});
			});
			AvatarLanternKingFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarLanternKingFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the Laughing King");
				bp.SetDescription(Main.IsekaiContext, "The shapeshifting spark of the First World dances around your silhouette. \nBenefit: You gain permanent Freedom of Movement, +4 Dodge bonus to AC, and enemies suffer a 20% miss chance against you.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
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
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Paralysis;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Paralysis;
				});
			});
			AvatarChaldiraFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AvatarChaldiraFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of the Mischievous Friend");
				bp.SetDescription(Main.IsekaiContext, "The boundless good fortune of Chaldira shields your comrades. \nBenefit: Grants a +4 Luck bonus to all saving throws and +3 Luck bonus to attack rolls and AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
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
			GodDefierFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "GodDefierFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendence: The God Defier");
				bp.SetDescription(Main.IsekaiContext, "You have rejected every celestial throne, broken every divine chain, and stood tall as an unyielding mortal. The Constellations watch in stunned awe as your sheer will carves a sovereign sanctuary in reality.\nBenefit: Grants a +4 Untyped bonus to all ability scores, immunity to divine and profane curses and death magic, +6 Spell Resistance against all divine spells, and all attacks deal +4d6 pure untyped damage against outsiders.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
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
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 8;
				});
			});
		}

		public static bool HasAnyAvatar(UnitEntityData player)
		{
			if (player == null)
			{
				return false;
			}
			FeatureCollection features = player.Descriptor.Progression.Features;
			if ((AvatarCaydenFeature == null || !features.HasFact(AvatarCaydenFeature)) && (AvatarIomedaeFeature == null || !features.HasFact(AvatarIomedaeFeature)) && (AvatarAsmodeusFeature == null || !features.HasFact(AvatarAsmodeusFeature)) && (AvatarDesnaFeature == null || !features.HasFact(AvatarDesnaFeature)) && (AvatarPharasmaFeature == null || !features.HasFact(AvatarPharasmaFeature)) && (AvatarCalistriaFeature == null || !features.HasFact(AvatarCalistriaFeature)) && (AvatarNethysFeature == null || !features.HasFact(AvatarNethysFeature)) && (AvatarGorumFeature == null || !features.HasFact(AvatarGorumFeature)) && (AvatarBesmaraFeature == null || !features.HasFact(AvatarBesmaraFeature)) && (AvatarLanternKingFeature == null || !features.HasFact(AvatarLanternKingFeature)) && (AvatarChaldiraFeature == null || !features.HasFact(AvatarChaldiraFeature)))
			{
				if (GodDefierFeature != null)
				{
					return features.HasFact(GodDefierFeature);
				}
				return false;
			}
			return true;
		}
	}
}
