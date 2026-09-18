using Kingmaker.UnitLogic.Mechanics.Components;
using IsekaiMod.Utilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
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

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal static class EpicArchetypeFeatures
	{
		private static readonly Sprite Icon_Crown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bdddaa78f8795024081f2d1eb8b4bd78"))?.m_Icon;

		private static readonly Sprite Icon_Sword = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("a38824af2ce2ee845b3592f9533a6056"))?.m_Icon ?? Icon_Crown;

		private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Crown;

		private static readonly Sprite Icon_Spell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Crown;

		private static readonly Sprite Icon_Dark = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9eda82a1f78558747a03c17e0e9a1a68"))?.m_Icon ?? Icon_Crown;

		private static bool Added = false;

		public static BlueprintFeature GodEmperorApotheosis;

		public static BlueprintFeature GodEmperorTrueSolarGodhood;

		public static BlueprintFeature OverlordSupremeRuler;

		public static BlueprintFeature OverlordTrueDominion;

		public static BlueprintFeature HeroOfLegendTranscendentTriad;

		public static BlueprintFeature HeroMasterSwordAwakened;

		public static BlueprintFeature MartialGodDaoPerfection;

		public static BlueprintFeature MartialGodApexOmnipresence;

		public static BlueprintFeature MastermindGrandmasterForesightEpic;

		public static BlueprintFeature MastermindAbsoluteCheckmate;

		public static BlueprintFeature ShadowMonarchCosmicLegion;

		public static BlueprintFeature ShadowMonarchTrueVoidSovereign;

		public static BlueprintFeature SlimeInfiniteSpatialStomach;

		public static BlueprintFeature SlimeBeelzebubCosmicDevourer;

		public static void Add()
		{
			if (Added)
			{
				return;
			}
			Added = true;
			GodEmperorApotheosis = Helpers.CreateBlueprint(Main.IsekaiContext, "GodEmperorApotheosis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Divine Sovereign Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "At 30th level, the God Emperor's spiritual cultivation transcends worldly deities. You gain a +6 inherent bonus to Intelligence, Wisdom, and Charisma, and a +5 sacred bonus to Armor Class and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 5;
				});
			});
			GodEmperorTrueSolarGodhood = Helpers.CreateBlueprint(Main.IsekaiContext, "GodEmperorTrueSolarGodhood", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "True Solar Godhood");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, the God Emperor achieves absolute solar divinity. You are permanently immune to death effects, negative levels, ability drain, energy drain, petrification, and bleed. You gain an additional +10 sacred bonus to Armor Class, and your damaging spells bypass all elemental resistances and immunities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.Bleed | SpellDescriptor.Petrified | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.Bleed | SpellDescriptor.Petrified | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
				bp.AddComponent(delegate(IgnoreSpellImmunity c)
				{
					c.SpellDescriptor = SpellDescriptor.None;
				});
			});
			OverlordSupremeRuler = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordSupremeRuler", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Supreme Ruler of the Great Tomb");
				bp.SetDescription(Main.IsekaiContext, "At 30th level, your absolute dominion over the grave and dark arts surpasses all mortal necromancers. You gain a +6 profane bonus to Strength, Constitution, Intelligence, and Charisma, and a +4 profane bonus to the DC of all your spells and abilities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Constitution;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Intelligence;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.Profane;
				});
			});
			OverlordTrueDominion = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordTrueDominion", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Death and Dominion");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, the Overlord achieves ultimate dark apotheosis. You are permanently immune to critical hits, sneak attacks, mind-affecting effects, and death magic. All physical and spell attacks deal an additional +30 direct unholy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent<AddImmunityToCriticalHits>();
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 30;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 30;
				});
			});
			HeroOfLegendTranscendentTriad = Helpers.CreateBlueprint(Main.IsekaiContext, "HeroOfLegendTranscendentTriad", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Hero of Legend: Transcendent Triad");
				bp.SetDescription(Main.IsekaiContext, "At 30th level, the Triforce of Courage, Wisdom, and Power achieve complete cosmic harmony within you. You gain a +6 inherent bonus to all ability scores, permanent immunity to fear, compulsion, and curse effects, and a +5 luck bonus to Armor Class and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 5;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Curse;
				});
			});
			HeroMasterSwordAwakened = Helpers.CreateBlueprint(Main.IsekaiContext, "HeroMasterSwordAwakened", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Master Sword: Blade of Evil's Bane");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, the legendary sacred blade awakens its full transcendent power. Your weapon attacks ignore concealment and physical damage reduction, and deal an additional 10d6 sacred damage against evil foes. You gain an additional +10 sacred bonus to Armor Class and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent<IgnoreConcealment>();
				bp.AddComponent<IgnoreDamageReductionOnAttack>();
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 10, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Divine };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 10;
				});
			});
			MartialGodDaoPerfection = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodDaoPerfection", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dao of the Infinite Strike");
				bp.SetDescription(Main.IsekaiContext, "At 30th level, your martial mastery transcends the boundary of physical form and spiritual perfection. You gain a +8 competence bonus to attack and weapon damage rolls, all your attacks bypass all forms of physical damage reduction, and you gain a +5 dodge bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 8;
				});
				bp.AddComponent<IgnoreDamageReductionOnAttack>();
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
			});
			MartialGodApexOmnipresence = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodApexOmnipresence", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Apex Martial Omnipresence");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, you embody the ultimate apex of combat speed and lethality. Your base movement speed increases by +50 feet, you gain 2 additional attacks during a full attack, all your attacks deal an additional +20 damage, and you gain permanent immunity to paralysis, petrification, and stun.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 50;
					c.Descriptor = ModifierDescriptor.Enhancement;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
					c.Haste = true;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 20;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Stun | SpellDescriptor.Paralysis | SpellDescriptor.Petrified;
				});
			});
			MastermindGrandmasterForesightEpic = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindGrandmasterForesightEpic", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grandmaster Calculation: 100 Moves Ahead");
				bp.SetDescription(Main.IsekaiContext, "At 30th level, your intellectual foresight predicts every variable of combat across infinite dimensions. You gain a +6 insight bonus to Armor Class, saving throws, initiative, and attack rolls, a +4 insight bonus to all spell DCs, and +6 spell penetration.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 6;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.Insight;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 6;
				});
			});
			MastermindAbsoluteCheckmate = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindAbsoluteCheckmate", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Checkmate: Masterplan Realized");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, your Masterplan is absolute and inescapable. You gain 2 additional attacks during a full attack, all spells you cast gain a +6 DC increase, and you gain permanent immunity to mind-affecting effects and a +10 insight bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
					c.Haste = true;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 6;
					c.Descriptor = ModifierDescriptor.Insight;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
			});
			ShadowMonarchCosmicLegion = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchCosmicLegion", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Shadow Legion");
				bp.SetDescription(Main.IsekaiContext, "At 30th level, your shadow army reaches cosmic scale. You gain a +6 profane bonus to Dexterity, Constitution, and Charisma, a +5 profane bonus to Armor Class and attack rolls, and Damage Reduction 20/-.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Dexterity;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Constitution;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 20;
				});
			});
			ShadowMonarchTrueVoidSovereign = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchTrueVoidSovereign", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "True Sovereign of Shadows");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, you become the living avatar of the Shadow Monarch. You gain permanent immunity to death effects, negative levels, ability drain, and critical hits. All attacks deal an additional 6d6 unholy damage, and you gain +20 damage bonus and +10 profane bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent<AddImmunityToCriticalHits>();
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 6, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
			});
			SlimeInfiniteSpatialStomach = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeInfiniteSpatialStomach", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Infinite Void Gluttony");
				bp.SetDescription(Main.IsekaiContext, "At 30th level, your spatial stomach expands into an infinite pocket universe. You gain Damage Reduction 30/-, immunity to acid, poison, and petrification, and a +6 inherent bonus to Constitution and Strength.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 30;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Acid;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Acid | SpellDescriptor.Poison | SpellDescriptor.Petrified;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
			});
			SlimeBeelzebubCosmicDevourer = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeBeelzebubCosmicDevourer", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Beelzebub: Cosmic Consumption");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, you achieve the final divine form of the Gluttonous Predator. You gain Fast Healing 25, immunity to paralysis, stun, bleed, and death effects, all attacks deal an additional 10d6 consumption damage, and you gain +8 inherent bonus to Constitution and Strength.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 25;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Stun | SpellDescriptor.Paralysis | SpellDescriptor.Death | SpellDescriptor.Bleed | SpellDescriptor.NegativeLevel;
				});
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 10, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
			});
		}
	}
}
