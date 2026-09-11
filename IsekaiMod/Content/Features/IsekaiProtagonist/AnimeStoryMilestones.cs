using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal static class AnimeStoryMilestones
	{
		private static readonly Sprite Icon_Log = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e45ab30f49215054e83b4ea12165409f"))?.m_Icon;

		private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Log;

		private static readonly Sprite Icon_Crown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Log;

		public static BlueprintFeature ChronicleOtherworldFeature { get; private set; }

		public static BlueprintFeature HuntersLogFeature { get; private set; }

		public static BlueprintFeature GluttonyCompendiumFeature { get; private set; }

		public static BlueprintFeature TombAnnalsFeature { get; private set; }

		public static BlueprintFeature HeroChronicleFeature { get; private set; }

		public static BlueprintFeature ImperialEdictsFeature { get; private set; }

		public static BlueprintFeature GrandStrategyFeature { get; private set; }

		public static BlueprintFeature SoulArchiveFeature { get; private set; }

		public static void Add()
		{
			ChronicleOtherworldFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronicleOtherworldFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chronicle of the Otherworld");
				bp.SetDescription(Main.IsekaiContext, "You maintain an otherworldly journal recording your landmark victories across Golarion. As you conquer pivotal story dungeons and chapter climaxes, you unlock passive otherworlder adventuring bonuses and ancient skill books.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Log;
				bp.IsClassFeature = true;
			});
			HuntersLogFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "HuntersLogFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Hunter's Log: Chronicle of Dominance");
				bp.SetDescription(Main.IsekaiContext, "You maintain an otherworld hunter's log that records your landmark victories across Golarion. As you conquer pivotal story dungeons and chapter climaxes, you permanently unlock passive shadow dominance bonuses and combat skill books.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Log;
				bp.IsClassFeature = true;
			});
			GluttonyCompendiumFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "GluttonyCompendiumFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator's Compendium: Record of Gluttony");
				bp.SetDescription(Main.IsekaiContext, "You maintain a primordial predator's compendium of consumed prey and assimilated territories across Golarion, unlocking massive biological resilience, fast healing, and acid mastery upon conquering landmark milestones.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Log;
				bp.IsClassFeature = true;
			});
			TombAnnalsFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TombAnnalsFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grimoire of Conquest: Annals of the Tomb");
				bp.SetDescription(Main.IsekaiContext, "You record the unstoppable expansion of the Great Tomb across Golarion, permanently expanding your dark necromantic authority, profane defenses, and cold dominion upon conquering landmark milestones.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Log;
				bp.IsClassFeature = true;
			});
			HeroChronicleFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "HeroChronicleFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chronicle of Legends: The Hero's Odyssey");
				bp.SetDescription(Main.IsekaiContext, "You chronicle the epic saga of fellowship and righteous triumph across Golarion, permanently empowering yourself and your allies with sacred morale bonuses, temporal luck, and radiant destiny upon conquering landmark milestones.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Log;
				bp.IsClassFeature = true;
			});
			ImperialEdictsFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialEdictsFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Imperial Edicts: Testament of Dominion");
				bp.SetDescription(Main.IsekaiContext, "You issue supreme celestial edicts claiming divine dominion over Golarion, permanently amplifying your radiant holy power, sacred authority, and charismatic awe upon conquering landmark milestones.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Log;
				bp.IsClassFeature = true;
			});
			GrandStrategyFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "GrandStrategyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grand Strategy: Archive of Calculations");
				bp.SetDescription(Main.IsekaiContext, "You compile a meticulous tactical archive analyzing the weaknesses of every opposing faction across Golarion, unlocking devastating precision, initiative dominance, and tactical mastery upon conquering landmark milestones.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Log;
				bp.IsClassFeature = true;
			});
			SoulArchiveFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SoulArchiveFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Soul Archive: Ledger of Slain Foes");
				bp.SetDescription(Main.IsekaiContext, "You preserve the spiritual imprints of defeated entities within your bonded phantom weapon, granting ethereal phase-shifting, ghost touch precision, and raw spiritual force upon conquering landmark milestones.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Log;
				bp.IsClassFeature = true;
			});
			CreateBaseMilestones();
			CreateSlimeMilestones();
			CreateOverlordMilestones();
			CreateShadowMilestones();
			CreateHeroMilestones();
			CreateGodMilestones();
			CreateMastermindMilestones();
			CreatePhantomMilestones();
			EventBus.Subscribe(new AnimeStoryMilestonesHandler());
		}

		private static void CreateBaseMilestones()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneShieldMazeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Shield Maze Conquered");
				bp.SetDescription(Main.IsekaiContext, "Victory over the Shield Maze cultists. Grants a +1 morale bonus to attack rolls and saving throws against poison.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneWaterElementalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Optional Apex Predator Slayed");
				bp.SetDescription(Main.IsekaiContext, "Defeated the optional primal water elemental in the Shield Maze. Grants resistance 5 to cold and acid, and +1 dodge AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneGrayGarrisonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Gray Garrison Liberated");
				bp.SetDescription(Main.IsekaiContext, "Awakening of the Wardstone in Kenabres. Increases your Caster Level for all spells by +1.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneVescavorQueenFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Leper's Smile Purged");
				bp.SetDescription(Main.IsekaiContext, "Purged the subterranean swarm matriarch. Grants immunity to confusion effects caused by swarms and +2 Fortitude saves.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneLostChapelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Lost Chapel Consecrated");
				bp.SetDescription(Main.IsekaiContext, "Rescued your companions on the frozen slopes. Grants a +2 morale bonus to all Will saves.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneDrezenCitadelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Drezen Citadel Reclaimed");
				bp.SetDescription(Main.IsekaiContext, "Raised the Sword of Valor over the fortress city. Grants a +2 morale bonus to attack rolls and AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneRedDragonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Ancient Dragon Hunt");
				bp.SetDescription(Main.IsekaiContext, "Slayed the red dragon Devarra. Grants fire resistance 20 and +2 bonus to weapon damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneIvorySanctumFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Ivory Sanctum Cleared");
				bp.SetDescription(Main.IsekaiContext, "Defeated the Swarm-That-Walks Xanthir Vang. Grants permanent immunity to bleed and nauseated conditions.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Nauseated;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Bleed;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneMidnightFaneFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Midnight Fane Breached");
				bp.SetDescription(Main.IsekaiContext, "Breached the abyssal threshold. Grants +2 bonus to Caster Level checks to overcome spell resistance.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneColyphyrFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Colyphyr Mines Subjugated");
				bp.SetDescription(Main.IsekaiContext, "Harvested the Nahyndrian crystal veins. Grants a +2 natural armor enhancement bonus.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmorEnhancement;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneBaphometFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Demon Lord Vanquished");
				bp.SetDescription(Main.IsekaiContext, "Stood victorious against the Lord of the Minotaurs. Grants a +2 untyped bonus to all ability scores.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Intelligence;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Wisdom;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Charisma;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneThresholdFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Record: Threshold of the Multiverse");
				bp.SetDescription(Main.IsekaiContext, "Stood at the epicenter of planar collapse. Grants immunity to death effects, negative levels, and a +4 insight bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
			});
		}

		private static void CreateSlimeMilestones()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneSlime_ShieldMazeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Gluttony Record: Shield Maze Digested");
				bp.SetDescription(Main.IsekaiContext, "Assimilated the subterranean biomass of the Shield Maze. Grants +10 maximum hit points and a +2 bonus to saving throws against poison and acid.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneSlime_WaterElementalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Gluttony Record: Primal Fluid Absorbed");
				bp.SetDescription(Main.IsekaiContext, "Absorbed the elemental fluid core. Grants cold and acid resistance 15 and Fast Healing 1.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 15;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 15;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneSlime_GrayGarrisonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Gluttony Record: Wardstone Core Digested");
				bp.SetDescription(Main.IsekaiContext, "Metabolized pure celestial ward energy. Grants +15 maximum hit points and a +2 natural armor enhancement bonus.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 15;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmorEnhancement;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneSlime_DrezenCitadelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Gluttony Record: Fortress Demonic Meat Assimilated");
				bp.SetDescription(Main.IsekaiContext, "Feasted upon the conquered demonic garrison. Grants +25 maximum hit points, +2 attack rolls, and DR 5/-.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 25;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
				});
			});
		}

		private static void CreateOverlordMilestones()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneOverlord_ShieldMazeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tomb Record: Subterranean Labyrinth Claimed");
				bp.SetDescription(Main.IsekaiContext, "Converted the Shield Maze into an outpost of the Tomb. Grants +1 DC to all Necromancy spells and a +2 profane bonus to Fortitude saves.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Necromancy;
					c.BonusDC = 1;
					c.Descriptor = ModifierDescriptor.Profane;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneOverlord_WaterElementalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tomb Record: Frozen Elemental Subjugated");
				bp.SetDescription(Main.IsekaiContext, "Harnessed the elemental chill. Grants cold resistance 20 and a +1 profane bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneOverlord_GrayGarrisonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tomb Record: Wardstone Necrotized");
				bp.SetDescription(Main.IsekaiContext, "Corrupted and conquered the Kenabres nexus. Increases Caster Level for Necromancy spells by +2 and grants +2 profane Spell Penetration.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseSpellSchoolCasterLevel c)
				{
					c.School = SpellSchool.Necromancy;
					c.BonusLevel = 2;
					c.Descriptor = ModifierDescriptor.Profane;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.Profane;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneOverlord_DrezenCitadelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tomb Record: Citadel Sovereign Domain");
				bp.SetDescription(Main.IsekaiContext, "Raised the banners of supreme death over Drezen. Grants a +2 profane bonus to attack rolls, +2 profane AC, and DR 5/bludgeoning.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
					c.BypassedByForm = true;
					c.Form = PhysicalDamageForm.Bludgeoning;
				});
			});
		}

		private static void CreateShadowMilestones()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneShadow_ShieldMazeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch Record: Shadows in the Maze");
				bp.SetDescription(Main.IsekaiContext, "Harvested the first fallen souls in darkness. Grants +1d6 Sneak Attack damage and +2 Stealth bonus.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneShadow_WaterElementalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch Record: Shadow of the Deep");
				bp.SetDescription(Main.IsekaiContext, "Extinguished the primal elemental. Grants cold resistance 10 and a +2 bonus to Initiative.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneShadow_GrayGarrisonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch Record: Kenabres Shadow Harvest");
				bp.SetDescription(Main.IsekaiContext, "Conducted an immense shadow extraction in Kenabres. Grants an additional +1d6 Sneak Attack and +2 bonus to weapon critical confirmation.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Bonus = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneShadow_DrezenCitadelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch Record: Fortress of Shadows");
				bp.SetDescription(Main.IsekaiContext, "Subjugated the fortress under an eternal shroud. Grants a +2 competence bonus to attack rolls, +2d6 Sneak Attack, and +4 Stealth.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 4;
				});
			});
		}

		private static void CreateHeroMilestones()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneHero_ShieldMazeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Odyssey: Fellowship in the Deep");
				bp.SetDescription(Main.IsekaiContext, "Led your allies through the trial of the maze. Grants a +2 morale bonus to attack rolls and +10 ft base movement speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneHero_WaterElementalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Odyssey: Primal Beast Overcome");
				bp.SetDescription(Main.IsekaiContext, "Protected your party from ancient elemental fury. Grants cold and acid resistance 10 and a +1 morale bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneHero_GrayGarrisonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Odyssey: Beacon of Radiant Hope");
				bp.SetDescription(Main.IsekaiContext, "Awakened the spirit of the crusade. Increases Caster Level by +1 and grants all allies a +1 morale bonus to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneHero_DrezenCitadelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Odyssey: Triumphant Liberation");
				bp.SetDescription(Main.IsekaiContext, "Restored Drezen to the civilized world. Grants a +3 morale bonus to attack rolls, +2 morale AC, and +10 ft speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
		}

		private static void CreateGodMilestones()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneGod_ShieldMazeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Edict: Radiant Judgment in the Labyrinth");
				bp.SetDescription(Main.IsekaiContext, "Sanctified the cultist maze with holy wrath. Grants a +2 sacred bonus to weapon damage and +2 sacred saves against fear.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneGod_WaterElementalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Edict: Subjugation of Primal Chaos");
				bp.SetDescription(Main.IsekaiContext, "Commanded the unruly elements to bow. Grants resistance 5 to all elemental energy types and a +1 sacred bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneGod_GrayGarrisonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Edict: Kenabres Ascendancy");
				bp.SetDescription(Main.IsekaiContext, "Proclaimed divine authority over the Worldwound. Grants +1 Caster Level, +2 sacred bonus to Charisma checks, and +2 sacred spell DC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 1;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.Sacred;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneGod_DrezenCitadelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Edict: Throne of Holy Sovereignty");
				bp.SetDescription(Main.IsekaiContext, "Consecrated Drezen as your holy seat of power. Grants a +2 sacred bonus to attack rolls, +2 sacred AC, and +4 holy damage on weapon attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
			});
		}

		private static void CreateMastermindMilestones()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneMastermind_ShieldMazeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Strategy: Labyrinth Dissection");
				bp.SetDescription(Main.IsekaiContext, "Analyzed the tactical layout of the subterranean maze. Grants a +2 insight bonus to Initiative and +1 insight bonus to attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneMastermind_WaterElementalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Strategy: Fluid Mechanics Exploited");
				bp.SetDescription(Main.IsekaiContext, "Anticipated every elemental movement. Grants cold and acid resistance 5 and a +2 insight bonus to Reflex saves.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneMastermind_GrayGarrisonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Strategy: Kenabres Defensive Analysis");
				bp.SetDescription(Main.IsekaiContext, "Re-engineered the magical wards of the city. Grants +1 Caster Level and +2 DC to all Illusion and Enchantment spells.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 1;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Illusion;
					c.BonusDC = 2;
					c.Descriptor = ModifierDescriptor.Insight;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Enchantment;
					c.BonusDC = 2;
					c.Descriptor = ModifierDescriptor.Insight;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestoneMastermind_DrezenCitadelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Strategy: Checkmate in Drezen");
				bp.SetDescription(Main.IsekaiContext, "Executed a flawless siege. Grants a +3 insight bonus to attack rolls, +2 insight AC, and +4 Initiative.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 4;
				});
			});
		}

		private static void CreatePhantomMilestones()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestonePhantom_ShieldMazeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Soul Ledger: Ethereal Inscription");
				bp.SetDescription(Main.IsekaiContext, "Inscribed the fallen souls into your living weapon. Weapon strikes deal an additional 2 pure force damage, and you gain a +1 dodge bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestonePhantom_WaterElementalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Soul Ledger: Aqueous Phantasm");
				bp.SetDescription(Main.IsekaiContext, "Phase-shifted through elemental currents. Grants cold and acid resistance 10 and a +1 deflection bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestonePhantom_GrayGarrisonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Soul Ledger: Wardstone Resonance");
				bp.SetDescription(Main.IsekaiContext, "Attuned your phantom blade to celestial resonance. Increases Caster Level by +1 and increases spellstrike critical multiplier by 1.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MilestonePhantom_DrezenCitadelFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Soul Ledger: Citadel Phantom March");
				bp.SetDescription(Main.IsekaiContext, "Phase-shifted your weapon through the thick walls of Drezen. Grants a +2 bonus to attack rolls, +4 force weapon damage, and a +2 dodge bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
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
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
		}
	}
}
