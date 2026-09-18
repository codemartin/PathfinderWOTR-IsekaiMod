using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Dialogue
{
	internal static class IsekaiCampaignExpansions
	{
		public static BlueprintBuff SilverDragonAegisBuff;

		public static BlueprintBuff MindOfTheOtherworlderBuff;

		public static BlueprintBuff StitchWardBuff;

		public static BlueprintBuff EmberSparkBuff;

		public static BlueprintFeature LarielsVowFeature;

		public static BlueprintFeature ResonantMythicSparkFeature;

		public static BlueprintItem ItemSilverDragonShard;

		public static BlueprintItemEquipmentNeck ItemSilverDragonShardPendant;

		public static BlueprintItemEquipmentHead ItemLarielFeatherCirclet;

		public static BlueprintItemEquipmentWrist ItemThieflingCharmBracelet;

		public static BlueprintItemEquipmentRing ItemRoyalMendevianSignetRing;

		public static BlueprintItemEquipmentShoulders ItemHellknightOfficerCloak;

		public static BlueprintItemEquipmentBelt ItemThieflingMasterLockpicksBelt;

		public static BlueprintItemEquipmentGloves ItemNenioInscribedJournalGloves;

		public static BlueprintItemEquipmentUsable ItemTearOfSilverDragon;

		public static BlueprintItem ItemLarielFeather;

		public static BlueprintItem ItemThieflingCharm;

		public static BlueprintItem ItemRoyalMendevianSignet;

		public static BlueprintItem ItemHellknightOfficerRegalia;

		public static BlueprintItemEquipmentUsable ItemVescavorPheromoneFlask;

		public static BlueprintBuff CrusaderLiberatorBuff;

		public static BlueprintFeature SwordOfValorResonantFeature;

		public static BlueprintItemWeapon ItemGwermAncestralRapier;

		public static BlueprintItemEquipmentRing ItemRingOfTheAncientChronicler;

		public static BlueprintItem ItemThieflingMasterLockpicks;

		public static BlueprintItem ItemNenioInscribedJournal;

		public static BlueprintBuff TavernDefianceMoraleBuff;

		public static BlueprintItemEquipmentUsable ItemOtherworldEspresso;

		public static BlueprintBuff OtherworldEspressoBuff;

		public static BlueprintItemEquipmentNeck ItemTalismanOfTheCosmicTourist;

		public static BlueprintItemEquipmentNeck ItemAmuletOfOtherworldEquilibrium;

		public static BlueprintItemEquipmentRing ItemRingOfTheGrandAuditor;

		public static BlueprintItemEquipmentHead ItemMartialGodFocusBand;

		public static BlueprintItemEquipmentRing ItemSovereignsImperialSignet;

		private static readonly Dictionary<string, BlueprintFeature> s_FactCache = new Dictionary<string, BlueprintFeature>();

		private static readonly Dictionary<string, BlueprintCharacterClass> s_MythicClassCache = new Dictionary<string, BlueprintCharacterClass>();

		public const string MythicClassAngel = "a5a9fe8f663d701488bd1db8ea40484e";

		public const string MythicClassDemon = "8e19495ea576a8641964102d177e34b7";

		public const string MythicClassAeon = "15a85e67b7d69554cab9ed5830d0268e";

		public const string MythicClassAzata = "9a3b2c63afa79744cbca46bea0da9a16";

		public const string MythicClassLich = "5d501618a28bdc24c80007a5c937dcb7";

		public const string MythicClassTrickster = "8df873a8c6e48294abdb78c45834aa0a";

		public const string MythicClassLegend = "3d420403f3e7340499931324640efe96";

		public const string MythicClassGoldDragon = "daf1235b6217787499c14e4e32142523";

		public const string MythicClassSwarm = "5295b8e13c2303f4c88bdb3d7760a757";

		public const string MythicClassDevil = "211f49705f478b3468db6daa802452a2";

		private static BlueprintFeature GetRequiredDialogueFact(string factName)
		{
			if (s_FactCache.TryGetValue(factName, out var value))
			{
				return value;
			}
			BlueprintFeature blueprintFeature = factName switch
			{
				"IsekaiProficiencies" => BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PlotArmor") ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies"), 
				"MasterAtArmsProficiencies" => BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialGodProficiencies"), 
				"VillainProficiencies" => BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies"), 
				_ => BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, factName), 
			};
			if (blueprintFeature != null)
			{
				s_FactCache[factName] = blueprintFeature;
			}
			return blueprintFeature;
		}

		public static void Add()
		{
			CreateBuffsAndFeatures();
			InjectCompanionRootHubEncounters();
			InjectEarlyPrologueEncounters();
			InjectAct1Encounters();
			InjectAct2Encounters();
			InjectAct3Encounters();
			InjectAct4Encounters();
			InjectAct5Encounters();
			InjectAct6Encounters();
			InjectDlcEncounters();
		}

		private static void CreateBuffsAndFeatures()
		{
			Sprite iconCoin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			Sprite iconScale = BlueprintTools.GetBlueprint<BlueprintItem>("816f244523b5455a85ae06db452d4330")?.m_Icon ?? iconCoin;
			Sprite iconPearl = BlueprintTools.GetBlueprint<BlueprintItem>("f682126f69da1ea479bf1ddf1d775d97")?.m_Icon ?? iconCoin;
			Sprite iconFeather = BlueprintTools.GetBlueprint<BlueprintItem>("9ba0194ebcbeb8a428926c1ad9911967")?.m_Icon ?? iconCoin;
			Sprite iconCharm = ((BlueprintItem)BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("235d859d7b7c9c240a1cec89eefb1f7a"))?.m_Icon ?? iconCoin;
			Sprite iconMedallion = BlueprintTools.GetBlueprint<BlueprintItem>("06846b7beaca5444d8eebfebc320adca")?.m_Icon ?? iconCoin;
			Sprite iconFlask = ((BlueprintItem)BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("c76deb732d037024a98fb5aa549df478"))?.m_Icon ?? iconCoin;
			Sprite iconLockpick = BlueprintTools.GetBlueprint<BlueprintItem>("f00d1a227450e3b49af4b9cc38145c89")?.m_Icon ?? iconCoin;
			Sprite iconTome = ((BlueprintItem)BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("3584c2a2f8b5b1b43ae11128f0ff1583"))?.m_Icon ?? iconCoin;
			Sprite iconPotion = ((BlueprintItem)BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("5219d5846529ae949b88c87858c1bb9e"))?.m_Icon ?? iconCoin;
			Sprite iconSignet = ((BlueprintItem)BlueprintTools.GetBlueprint<BlueprintItemEquipmentRing>("e0986c3e091b2a14a91d2257979b39b6"))?.m_Icon ?? iconCoin;
			SilverDragonAegisBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SilverDragonAegisBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Silver Dragon's Aegis");
				bp.SetDescription(Main.IsekaiContext, "Terendelev's draconic starlight grants a +2 sacred bonus on all saving throws and absolute immunity to fear effects.");
				((BlueprintUnitFact)bp).m_Icon = iconScale;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Frightened;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Frightened;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Frightened;
				});
			});
			MindOfTheOtherworlderBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "MindOfTheOtherworlderBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Mind of the Otherworlder");
				bp.SetDescription(Main.IsekaiContext, "Having traversed the cosmic void between dimensions, your mind is anchored beyond mortal terror, granting a permanent +2 sacred bonus on Will saving throws against fear and mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_MIND_CONTROL_IMMUNE.png") ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			StitchWardBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "StitchWardBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Stitch's Scavenged Ward");
				bp.SetDescription(Main.IsekaiContext, "A hastily woven protective ward scavenged from Kenabres rubble, granting a +2 dodge bonus to Armor Class for 1 hour.");
				((BlueprintUnitFact)bp).m_Icon = iconCharm;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			EmberSparkBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "EmberSparkBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Ember's Saintly Spark");
				bp.SetDescription(Main.IsekaiContext, "Resonating with Ember's pure-hearted innocent flame, your fire spells and healing abilities receive a +2 sacred bonus.");
				((BlueprintUnitFact)bp).m_Icon = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_ENERGY_LIGHT.png") ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 1;
				});
			});
			LarielsVowFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "LarielsVowFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Lariel's Vow");
				bp.SetDescription(Main.IsekaiContext, "The fallen angel Lariel honors your dual covenant of mercy and retribution, granting a +1 sacred bonus on attack rolls and +1 to caster level against chaotic evil outsiders.");
				((BlueprintUnitFact)bp).m_Icon = iconFeather;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 1;
				});
			});
			ResonantMythicSparkFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ResonantMythicSparkFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Resonant Mythic Spark");
				bp.SetDescription(Main.IsekaiContext, "Cleansing and harmonizing the Kenabres Wardstone with otherworldly mana awakens your dormant mythic potential, granting a +1 sacred bonus to attack rolls, AC, and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_GODHOOD.png") ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 1;
				});
			});
			ItemSilverDragonShard = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemSilverDragonShard", delegate(BlueprintItem bp)
			{
				bp.SetName(Main.IsekaiContext, "Silver Dragon's Shard");
				bp.SetDescription(Main.IsekaiContext, "A radiant silver crystal fragment imbued with Terendelev's sacred starlight. It pulses with gentle warmth, shielding its bearer from dark sorceries.");
				bp.m_Icon = iconScale;
				bp.m_Cost = 1000;
				bp.m_Weight = 0.5f;
				bp.m_IsNotable = true;
				bp.m_Destructible = false;
			});
			// The original shard is a plain inventory item with no effect; it stays defined so saves that already
			// hold one keep loading. The pendant below is what the healing scene hands out now.
			BlueprintItemEquipmentNeck shardBaseNeck = BlueprintTools.GetBlueprint<BlueprintItemEquipmentNeck>("afd04948b8f211c448f1cc4bd9a67e3e");
			BlueprintFeature shardFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemSilverDragonShardPendantFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Silver Dragon's Shard");
				bp.SetDescription(Main.IsekaiContext, "Terendelev's starlight shields the wearer from dark sorceries: a +2 sacred bonus on saving throws against necromancy spells and against effects with the evil descriptor, and a +1 sacred bonus on all other saving throws.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)shardBaseNeck)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(SavingThrowBonusAgainstSchool c)
				{
					c.ModifierDescriptor = ModifierDescriptor.Sacred;
					c.School = SpellSchool.Necromancy;
					c.Value = 2;
				});
				bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
				{
					c.ModifierDescriptor = ModifierDescriptor.Sacred;
					c.SpellDescriptor = SpellDescriptor.Evil;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 1;
				});
			});
			ItemSilverDragonShardPendant = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemSilverDragonShardPendant", delegate(BlueprintItemEquipmentNeck bp)
			{
				bp.SetName(Main.IsekaiContext, "Silver Dragon's Shard");
				bp.SetDescription(Main.IsekaiContext, "A radiant silver crystal fragment imbued with Terendelev's sacred starlight, strung on a simple cord. It pulses with gentle warmth, shielding its wearer from dark sorceries: a +2 sacred bonus on saving throws against necromancy spells and evil effects, and a +1 sacred bonus on all other saving throws.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)shardBaseNeck)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 4000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = shardFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
			ItemTearOfSilverDragon = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemTearOfSilverDragon", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Tear of the Silver Dragon");
				bp.SetDescription(Main.IsekaiContext, "A pearlescent silver teardrop gifted directly by Terendelev. Radiates ancient draconic grace, granting its bearer supernatural endurance against the freezing dark.");
				((BlueprintItem)bp).m_Icon = iconPearl;
				((BlueprintItem)bp).m_Cost = 3000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.Type = UsableItemType.Other;
				bp.SpendCharges = false;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = true;
			});
			ItemLarielFeather = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemLarielFeather", delegate(BlueprintItem bp)
			{
				bp.SetName(Main.IsekaiContext, "Lariel's Radiant Feather");
				bp.SetDescription(Main.IsekaiContext, "A pristine celestial feather plucked from the memory of the angel Lariel. Glows with unyielding righteousness against the demonic hordes.");
				bp.m_Icon = iconFeather;
				bp.m_Cost = 2500;
				bp.m_Weight = 0.1f;
				bp.m_IsNotable = true;
				bp.m_Destructible = false;
			});
			ItemThieflingCharm = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemThieflingCharm", delegate(BlueprintItem bp)
			{
				bp.SetName(Main.IsekaiContext, "Thiefling's Luck Charm");
				bp.SetDescription(Main.IsekaiContext, "A brass coin stamped with the mark of the Kenabres Thieflings, pledged by Woljif Jefto under an Otherworlder retainer contract.");
				bp.m_Icon = iconCharm;
				bp.m_Cost = 1500;
				bp.m_Weight = 0.1f;
				bp.m_IsNotable = true;
				bp.m_Destructible = false;
			});
			ItemRoyalMendevianSignet = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRoyalMendevianSignet", delegate(BlueprintItem bp)
			{
				bp.SetName(Main.IsekaiContext, "Royal Mendevian Command Signet");
				bp.SetDescription(Main.IsekaiContext, "A gold signet ring engraved with the royal crest of Mendev, gifted directly by Queen Galfrey. Radiates sovereign authority, inspiring all companion vanguard forces.");
				bp.m_Icon = iconSignet;
				bp.m_Cost = 3500;
				bp.m_Weight = 0.1f;
				bp.m_IsNotable = true;
				bp.m_Destructible = false;
			});
			ItemHellknightOfficerRegalia = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemHellknightOfficerRegalia", delegate(BlueprintItem bp)
			{
				bp.SetName(Main.IsekaiContext, "Hellknight Officer's Field Regalia");
				bp.SetDescription(Main.IsekaiContext, "A polished cold iron medallion stamped with the symbol of the Order of the Godclaw, conferred by Paralictor Regill Derenge in recognition of impeccable field triage and strategic logic.");
				bp.m_Icon = iconMedallion;
				bp.m_Cost = 4000;
				bp.m_Weight = 0.5f;
				bp.m_IsNotable = true;
				bp.m_Destructible = false;
			});
			ItemVescavorPheromoneFlask = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemVescavorPheromoneFlask", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Vescavor Pheromone Flask");
				bp.SetDescription(Main.IsekaiContext, "A sealed crystal flask filled with concentrated Vescavor Queen pheromones confiscated from the saboteur Nurah. Its pungent scent can confound insectoid swarms and draw vermin away from friendly ranks.");
				((BlueprintItem)bp).m_Icon = iconFlask;
				((BlueprintItem)bp).m_Cost = 2000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.Type = UsableItemType.Other;
				bp.SpendCharges = false;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = true;
			});
			CrusaderLiberatorBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "CrusaderLiberatorBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Crusader Liberator's Aegis");
				bp.SetDescription(Main.IsekaiContext, "Sovereign starlight channeled at the Lost Chapel cleanses all corruption, granting a +2 morale bonus on attack rolls and saving throws, and total immunity to fear and disease.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Frightened;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Frightened;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Frightened;
				});
			});
			SwordOfValorResonantFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SwordOfValorResonantFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Resonant Sword of Valor");
				bp.SetDescription(Main.IsekaiContext, "The liberated Sword of Valor resonates with otherworldly starlight, granting a +2 sacred bonus to attack rolls, armor class, and saving throws against evil outsiders.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
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
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			BlueprintItemWeapon blueprint = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("ec731c55e657cf0408fd89c648ccc536");
			if (blueprint != null)
			{
				ItemGwermAncestralRapier = blueprint.CreateCopy(Main.IsekaiContext, "ItemGwermAncestralRapier", delegate(BlueprintItemWeapon bp)
				{
					bp.SetName(Main.IsekaiContext, "Gwerm Ancestral Rapier");
					bp.SetDescription(Main.IsekaiContext, "An heirloom cold iron rapier passed down through the Gwerm bloodline, sharpened to a lethal edge. Deals +1 enhancement bonus and possesses the Keen property.");
					((BlueprintItem)bp).m_Cost = 4500;
				});
			}
			BlueprintItemEquipmentRing baseRing = BlueprintTools.GetBlueprint<BlueprintItemEquipmentRing>("f333bf86cd122974792162cbcd27c9ed");
			ItemRingOfTheAncientChronicler = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRingOfTheAncientChronicler", delegate(BlueprintItemEquipmentRing bp)
			{
				bp.SetName(Main.IsekaiContext, "Ring of the Ancient Chronicler");
				bp.SetDescription(Main.IsekaiContext, "A star-sapphire band engraved with the annals of forgotten realities, gifted by the Storyteller. Grants a +3 competence bonus to Lore (World) and Lore (Religion) checks.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseRing)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 3500;
				((BlueprintItem)bp).m_Weight = 0.1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
			});
			ItemThieflingMasterLockpicks = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemThieflingMasterLockpicks", delegate(BlueprintItem bp)
			{
				bp.SetName(Main.IsekaiContext, "Thiefling Master Lockpicks");
				bp.SetDescription(Main.IsekaiContext, "A set of finely-tempered cold iron tension wrenches and skeleton picks gifted by Sister Kerisme. Grants a +3 competence bonus to Trickery checks.");
				bp.m_Icon = iconLockpick;
				bp.m_Cost = 2500;
				bp.m_Weight = 0.5f;
				bp.m_IsNotable = true;
				bp.m_Destructible = false;
			});
			ItemNenioInscribedJournal = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNenioInscribedJournal", delegate(BlueprintItem bp)
			{
				bp.SetName(Main.IsekaiContext, "Nenio's Inscribed Research Journal");
				bp.SetDescription(Main.IsekaiContext, "A thick leather-bound compendium filled with Nenio's frantic diagrams, multiversal theorems, and cellular taxonomy notes. Inspires profound intellectual curiosity.");
				bp.m_Icon = iconTome;
				bp.m_Cost = 3000;
				bp.m_Weight = 1f;
				bp.m_IsNotable = true;
				bp.m_Destructible = false;
			});
			// Wearable versions of the story keepsakes. The plain items above stay defined so saves that already
			// hold one keep loading; the answers hand out these from now on.
			BlueprintItemEquipmentHead keepsakeBaseHead = BlueprintTools.GetBlueprint<BlueprintItemEquipmentHead>("a3e8e907908ca7a40ac6da78e70bf33d");
			BlueprintItemEquipmentWrist keepsakeBaseWrist = BlueprintTools.GetBlueprint<BlueprintItemEquipmentWrist>("9482c62934be44044918c3aac3730232");
			BlueprintItemEquipmentRing keepsakeBaseRing = BlueprintTools.GetBlueprint<BlueprintItemEquipmentRing>("f333bf86cd122974792162cbcd27c9ed");
			BlueprintItemEquipmentShoulders keepsakeBaseCloak = BlueprintTools.GetBlueprint<BlueprintItemEquipmentShoulders>("4aea6773c1da01c42bd382c1b7c384bc");
			BlueprintItemEquipmentBelt keepsakeBaseBelt = BlueprintTools.GetBlueprint<BlueprintItemEquipmentBelt>("b9cc5d85d4a5032458ef4491f5fed251");
			BlueprintItemEquipmentGloves keepsakeBaseGloves = BlueprintTools.GetBlueprint<BlueprintItemEquipmentGloves>("6555965e6540c3b48b9a352214ecba41");
			BlueprintFeature subtypeDemon = BlueprintTools.GetBlueprint<BlueprintFeature>("dc960a234d365cb4f905bdc5937e623a");

			BlueprintFeature featherFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemLarielFeatherCircletFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Lariel's Radiant Feather");
				bp.SetDescription(Main.IsekaiContext, "Lariel's memory burns against the Abyss: a +2 sacred bonus on attack and damage rolls against demons.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)keepsakeBaseHead)?.m_Icon ?? iconCoin;
				if (subtypeDemon != null)
				{
					bp.AddComponent(delegate(AttackBonusAgainstFactOwner c)
					{
						c.m_CheckedFact = subtypeDemon.ToReference<BlueprintUnitFactReference>();
						c.AttackBonus = 2;
						c.Bonus = 0;
						c.Descriptor = ModifierDescriptor.Sacred;
					});
					bp.AddComponent(delegate(DamageBonusAgainstFactOwner c)
					{
						c.m_CheckedFact = subtypeDemon.ToReference<BlueprintUnitFactReference>();
						c.DamageBonus = 2;
						c.Bonus = 0;
						c.Descriptor = ModifierDescriptor.Sacred;
					});
				}
			});
			ItemLarielFeatherCirclet = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemLarielFeatherCirclet", delegate(BlueprintItemEquipmentHead bp)
			{
				bp.SetName(Main.IsekaiContext, "Lariel's Radiant Feather");
				bp.SetDescription(Main.IsekaiContext, "A pristine celestial feather plucked from the memory of the angel Lariel, bound into a simple circlet. Glows with unyielding righteousness against the demonic hordes: +2 sacred bonus on attack and damage rolls against demons.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)keepsakeBaseHead)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 6000;
				((BlueprintItem)bp).m_Weight = 0.1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = featherFeature.ToReference<BlueprintUnitFactReference>();
				});
			});

			BlueprintFeature charmFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemThieflingCharmBraceletFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Thiefling's Luck Charm");
				bp.SetDescription(Main.IsekaiContext, "The Thieflings' luck rubs off: a +1 luck bonus on all saving throws and a +2 competence bonus to Trickery checks.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)keepsakeBaseWrist)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Luck; c.Stat = StatType.SaveFortitude; c.Value = 1; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Luck; c.Stat = StatType.SaveReflex; c.Value = 1; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Luck; c.Stat = StatType.SaveWill; c.Value = 1; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Competence; c.Stat = StatType.SkillThievery; c.Value = 2; });
			});
			ItemThieflingCharmBracelet = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemThieflingCharmBracelet", delegate(BlueprintItemEquipmentWrist bp)
			{
				bp.SetName(Main.IsekaiContext, "Thiefling's Luck Charm");
				bp.SetDescription(Main.IsekaiContext, "A brass coin stamped with the mark of the Kenabres Thieflings, pledged by Woljif Jefto under an Otherworlder retainer contract and worn on a leather cord. +1 luck bonus on all saving throws and +2 competence bonus to Trickery checks.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)keepsakeBaseWrist)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 3000;
				((BlueprintItem)bp).m_Weight = 0.1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = charmFeature.ToReference<BlueprintUnitFactReference>();
				});
			});

			BlueprintFeature signetAura = TTCoreExtensions.CreateToggleAuraBuffFeature("RoyalMendevianCommandAura", "The signet's authority rallies nearby allies: allies within 30 feet gain a +1 morale bonus on attack rolls and saving throws.", "Rallied by the Royal Mendevian Command Signet: +1 morale bonus on attack rolls and saving throws.", ((BlueprintItem)keepsakeBaseRing)?.m_Icon ?? iconCoin, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(30f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Morale; c.Stat = StatType.AdditionalAttackBonus; c.Value = 1; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Morale; c.Stat = StatType.SaveFortitude; c.Value = 1; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Morale; c.Stat = StatType.SaveReflex; c.Value = 1; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Morale; c.Stat = StatType.SaveWill; c.Value = 1; });
			});
			ItemRoyalMendevianSignetRing = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRoyalMendevianSignetRing", delegate(BlueprintItemEquipmentRing bp)
			{
				bp.SetName(Main.IsekaiContext, "Royal Mendevian Command Signet");
				bp.SetDescription(Main.IsekaiContext, "A gold signet ring engraved with the royal crest of Mendev, gifted directly by Queen Galfrey. Radiates sovereign authority: the wearer can project a Command Aura that grants allies within 30 feet a +1 morale bonus on attack rolls and saving throws.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)keepsakeBaseRing)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 8000;
				((BlueprintItem)bp).m_Weight = 0.1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = signetAura.ToReference<BlueprintUnitFactReference>();
				});
			});

			BlueprintFeature regaliaFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemHellknightOfficerCloakFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Hellknight Officer's Field Regalia");
				bp.SetDescription(Main.IsekaiContext, "Godclaw discipline: a +2 competence bonus to Persuasion checks and a +2 resistance bonus on Fortitude and Will saving throws.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)keepsakeBaseCloak)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Competence; c.Stat = StatType.SkillPersuasion; c.Value = 2; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Resistance; c.Stat = StatType.SaveFortitude; c.Value = 2; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Resistance; c.Stat = StatType.SaveWill; c.Value = 2; });
			});
			ItemHellknightOfficerCloak = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemHellknightOfficerCloak", delegate(BlueprintItemEquipmentShoulders bp)
			{
				bp.SetName(Main.IsekaiContext, "Hellknight Officer's Field Regalia");
				bp.SetDescription(Main.IsekaiContext, "A polished cold iron medallion on an officer's half-cloak, stamped with the symbol of the Order of the Godclaw and conferred by Paralictor Regill Derenge in recognition of impeccable field triage and strategic logic. +2 competence bonus to Persuasion and +2 resistance bonus on Fortitude and Will saves.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)keepsakeBaseCloak)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 5000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = regaliaFeature.ToReference<BlueprintUnitFactReference>();
				});
			});

			BlueprintFeature lockpicksFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemThieflingMasterLockpicksBeltFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Thiefling Master Lockpicks");
				bp.SetDescription(Main.IsekaiContext, "Grants a +3 competence bonus to Trickery checks.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)keepsakeBaseBelt)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Competence; c.Stat = StatType.SkillThievery; c.Value = 3; });
			});
			ItemThieflingMasterLockpicksBelt = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemThieflingMasterLockpicksBelt", delegate(BlueprintItemEquipmentBelt bp)
			{
				bp.SetName(Main.IsekaiContext, "Thiefling Master Lockpicks");
				bp.SetDescription(Main.IsekaiContext, "A set of finely-tempered cold iron tension wrenches and skeleton picks gifted by Sister Kerisme, carried on a tool belt. Grants a +3 competence bonus to Trickery checks.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)keepsakeBaseBelt)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 2500;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = lockpicksFeature.ToReference<BlueprintUnitFactReference>();
				});
			});

			BlueprintFeature journalFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNenioInscribedJournalGlovesFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Nenio's Inscribed Research Journal");
				bp.SetDescription(Main.IsekaiContext, "Nenio's notes inspire profound intellectual curiosity: a +2 competence bonus to Knowledge (Arcana), Knowledge (World), Lore (Nature) and Lore (Religion) checks.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)keepsakeBaseGloves)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Competence; c.Stat = StatType.SkillKnowledgeArcana; c.Value = 2; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Competence; c.Stat = StatType.SkillKnowledgeWorld; c.Value = 2; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Competence; c.Stat = StatType.SkillLoreNature; c.Value = 2; });
				bp.AddComponent(delegate(AddStatBonus c) { c.Descriptor = ModifierDescriptor.Competence; c.Stat = StatType.SkillLoreReligion; c.Value = 2; });
			});
			ItemNenioInscribedJournalGloves = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemNenioInscribedJournalGloves", delegate(BlueprintItemEquipmentGloves bp)
			{
				bp.SetName(Main.IsekaiContext, "Nenio's Inscribed Research Journal");
				bp.SetDescription(Main.IsekaiContext, "A thick leather-bound compendium filled with Nenio's frantic diagrams, multiversal theorems, and cellular taxonomy notes, strapped to the forearm for reading on the move. Inspires profound intellectual curiosity: +2 competence bonus to Knowledge (Arcana), Knowledge (World), Lore (Nature) and Lore (Religion) checks.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)keepsakeBaseGloves)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 4000;
				((BlueprintItem)bp).m_Weight = 1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = journalFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
			TavernDefianceMoraleBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TavernDefianceMoraleBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Tavern Defiance Morale");
				bp.SetDescription(Main.IsekaiContext, "Inspired by the Otherworlder Vanguard Doctrine during the defense of Defender's Heart, this unit gains a +2 morale bonus on attack rolls and Will saving throws.");
				((BlueprintUnitFact)bp).m_Icon = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_FRIENDLY.png") ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			OtherworldEspressoBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldEspressoBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Otherworld Espresso Rush");
				bp.SetDescription(Main.IsekaiContext, "A concentrated jolt of dimensional caffeine and alchemical sugar, granting Haste, a +2 morale bonus to attack rolls, saving throws, and skill checks, and absolute immunity to sleep and fatigue for 10 minutes.");
				((BlueprintUnitFact)bp).m_Icon = iconPotion;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Value = 30;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Fatigued;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Sleeping;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Sleep;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Sleep;
				});
			});
			ItemOtherworldEspresso = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemOtherworldEspresso", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, "Otherworld Triple-Shot Espresso");
				bp.SetDescription(Main.IsekaiContext, "A steaming porcelain thermos filled with concentrated Otherworld espresso brewed from celestial roasted beans. Grants the Otherworld Espresso Rush buff for 10 minutes.");
				((BlueprintItem)bp).m_Icon = iconPotion;
				((BlueprintItem)bp).m_Cost = 750;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.Type = UsableItemType.Potion;
				bp.SpendCharges = true;
				bp.Charges = 1;
			});
			BlueprintItemEquipmentNeck baseNeck = BlueprintTools.GetBlueprint<BlueprintItemEquipmentNeck>("afd04948b8f211c448f1cc4bd9a67e3e");
			BlueprintFeature touristFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemTalismanOfTheCosmicTouristFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Talisman of the Cosmic Tourist");
				bp.SetDescription(Main.IsekaiContext, "An otherworldly silver pendant shaped like a compass rose. Grants a +2 luck bonus to Armor Class and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)baseNeck)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			ItemTalismanOfTheCosmicTourist = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemTalismanOfTheCosmicTourist", delegate(BlueprintItemEquipmentNeck bp)
			{
				bp.SetName(Main.IsekaiContext, "Talisman of the Cosmic Tourist");
				bp.SetDescription(Main.IsekaiContext, "An otherworldly silver pendant shaped like a compass rose. Grants a +2 luck bonus to Armor Class and all saving throws.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseNeck)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 10000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = touristFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
			BlueprintFeature equilibriumFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemAmuletOfOtherworldEquilibriumFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Amulet of Otherworld Equilibrium");
				bp.SetDescription(Main.IsekaiContext, "A pristine crystal talisman that stabilizes the wearer's biological rhythms, granting a +4 enhancement bonus to Constitution and absolute immunity to fatigue and exhaustion.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)baseNeck)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Constitution;
					c.Value = 4;
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
			});
			ItemAmuletOfOtherworldEquilibrium = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemAmuletOfOtherworldEquilibrium", delegate(BlueprintItemEquipmentNeck bp)
			{
				bp.SetName(Main.IsekaiContext, "Amulet of Otherworld Equilibrium");
				bp.SetDescription(Main.IsekaiContext, "A pristine crystal talisman that stabilizes the wearer's biological rhythms, granting a +4 enhancement bonus to Constitution and absolute immunity to fatigue and exhaustion.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseNeck)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 15000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = equilibriumFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
			BlueprintFeature auditorFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRingOfTheGrandAuditorFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Ring of the Grand Auditor");
				bp.SetDescription(Main.IsekaiContext, "A platinum signet ring engraved with analytical runes. Grants a +5 competence bonus to Persuasion, Lore (Religion), and Knowledge (World) checks.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)baseRing)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillLoreReligion;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 5;
				});
			});
			ItemRingOfTheGrandAuditor = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRingOfTheGrandAuditor", delegate(BlueprintItemEquipmentRing bp)
			{
				bp.SetName(Main.IsekaiContext, "Ring of the Grand Auditor");
				bp.SetDescription(Main.IsekaiContext, "A platinum signet ring engraved with analytical runes. Grants a +5 competence bonus to Persuasion, Lore (Religion), and Knowledge (World) checks.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseRing)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 14000;
				((BlueprintItem)bp).m_Weight = 0.1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = auditorFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
			BlueprintItemEquipmentHead baseHead = BlueprintTools.GetBlueprint<BlueprintItemEquipmentHead>("a3e8e907908ca7a40ac6da78e70bf33d");
			BlueprintFeature martialBandFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemMartialGodFocusBandFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial God's Focus Band");
				bp.SetDescription(Main.IsekaiContext, "A woven headband imbued with focused spiritual prana. Grants a +2 sacred bonus to unarmed and natural attack and damage rolls and a +2 sacred bonus to Reflex saving throws.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)baseHead)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
			});
			ItemMartialGodFocusBand = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemMartialGodFocusBand", delegate(BlueprintItemEquipmentHead bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial God's Focus Band");
				bp.SetDescription(Main.IsekaiContext, "A woven headband imbued with focused spiritual prana. Grants a +2 sacred bonus to unarmed and natural attack and damage rolls and a +2 sacred bonus to Reflex saving throws.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseHead)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 12000;
				((BlueprintItem)bp).m_Weight = 0.5f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = martialBandFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
			BlueprintFeature sovereignSignetFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemSovereignsImperialSignetFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Sovereign's Imperial Signet");
				bp.SetDescription(Main.IsekaiContext, "A gold signet ring radiating imperial authority. Grants a +2 sacred bonus to the Difficulty Class (DC) of all mind-affecting and enchantment spells and abilities.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintItem)baseRing)?.m_Icon ?? iconCoin;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			ItemSovereignsImperialSignet = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemSovereignsImperialSignet", delegate(BlueprintItemEquipmentRing bp)
			{
				bp.SetName(Main.IsekaiContext, "Sovereign's Imperial Signet");
				bp.SetDescription(Main.IsekaiContext, "A gold signet ring radiating imperial authority. Grants a +2 sacred bonus to the Difficulty Class (DC) of all mind-affecting and enchantment spells and abilities.");
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseRing)?.m_Icon ?? iconCoin;
				((BlueprintItem)bp).m_Cost = 16000;
				((BlueprintItem)bp).m_Weight = 0.1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = sovereignSignetFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
		}

		private static void AddBookPageAnswer(BlueprintBookPage page, string id, string answerText, string nextCueGuid, int expCr = 2, Action<BlueprintAnswer> configureAnswer = null)
		{
			if (page == null)
			{
				return;
			}
			BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(id, delegate(BlueprintAnswer bp)
			{
				bp.SetText(Main.IsekaiContext, answerText);
				if (!string.IsNullOrEmpty(nextCueGuid))
				{
					BlueprintCueBase blueprint = BlueprintTools.GetBlueprint<BlueprintCueBase>(nextCueGuid);
					if (blueprint != null)
					{
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { blueprint.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
					}
				}
				bp.ShowOnce = true;
				if (expCr > 0)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(GainExp c)
					{
						c.Encounter = EncounterType.SkillCheck;
						c.CR = expCr;
						c.Modifier = 1f;
					});
				}
				bp.RequirePlotArmor();
				configureAnswer?.Invoke(bp);
			});
			page.InsertAnswer(answer);
		}

		private static void AddUniversalAnswer(BlueprintAnswersList answersList, string id, string answerText, string replyText, int expCr = 2, Action<BlueprintCue> configureReply = null, Action<BlueprintAnswer> configureAnswer = null)
		{
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue(id + "Reply", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, replyText);
				configureReply?.Invoke(bp);
				List<BlueprintAnswerBaseReference> answers = bp.Answers;
				if ((answers == null || answers.Count == 0) && (bp.Continue?.Cues?.Count).GetValueOrDefault() == 0)
				{
					bp.SetAnswersList(answersList);
				}
			});
			BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(id, delegate(BlueprintAnswer bp)
			{
				bp.SetText(Main.IsekaiContext, answerText);
				bp.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				bp.ShowOnce = true;
				if (expCr > 0)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(GainExp c)
					{
						c.Encounter = EncounterType.SkillCheck;
						c.CR = expCr;
						c.Modifier = 1f;
					});
				}
				bp.RequirePlotArmor();
				configureAnswer?.Invoke(bp);
			});
			answersList.InsertAnswer(answer);
		}

		private static void AddSubclassAnswer(BlueprintAnswersList answersList, string id, string answerText, string replyText, string proficiencyFactName, int expCr = 2, Action<BlueprintCue> configureReply = null, Action<BlueprintAnswer> configureAnswer = null)
		{
			if (answersList == null)
			{
				return;
			}
			BlueprintFeature feat = GetRequiredDialogueFact(proficiencyFactName);
			if (feat == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue(id + "Reply", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, replyText);
				configureReply?.Invoke(bp);
				List<BlueprintAnswerBaseReference> answers = bp.Answers;
				if ((answers == null || answers.Count == 0) && (bp.Continue?.Cues?.Count).GetValueOrDefault() == 0)
				{
					bp.SetAnswersList(answersList);
				}
			});
			BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(id, delegate(BlueprintAnswer bp)
			{
				bp.SetText(Main.IsekaiContext, answerText);
				bp.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				bp.ShowOnce = true;
				if (expCr > 0)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(GainExp c)
					{
						c.Encounter = EncounterType.SkillCheck;
						c.CR = expCr;
						c.Modifier = 1f;
					});
				}
				bp.AddShowCondition(delegate(HasFact c)
				{
					c.Unit = new PlayerCharacter();
					c.m_Fact = feat.ToReference<BlueprintUnitFactReference>();
				});
				configureAnswer?.Invoke(bp);
			});
			answersList.InsertAnswer(answer);
		}

		private static void AddAlignedAnswer(BlueprintAnswersList answersList, string id, string answerText, string replyText, AlignmentShiftDirection alignmentShift, string proficiencyFactName = null, int expCr = 2, Action<BlueprintCue> configureReply = null, Action<BlueprintAnswer> configureAnswer = null)
		{
			if (answersList == null)
			{
				return;
			}
			BlueprintFeature feat = null;
			if (!string.IsNullOrEmpty(proficiencyFactName))
			{
				feat = GetRequiredDialogueFact(proficiencyFactName);
				if (feat == null)
				{
					return;
				}
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue(id + "Reply", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, replyText);
				configureReply?.Invoke(bp);
				List<BlueprintAnswerBaseReference> answers = bp.Answers;
				if ((answers == null || answers.Count == 0) && (bp.Continue?.Cues?.Count).GetValueOrDefault() == 0)
				{
					bp.SetAnswersList(answersList);
				}
			});
			BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(id, delegate(BlueprintAnswer bp)
			{
				bp.SetText(Main.IsekaiContext, answerText);
				bp.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				bp.ShowOnce = true;
				if (expCr > 0)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(GainExp c)
					{
						c.Encounter = EncounterType.SkillCheck;
						c.CR = expCr;
						c.Modifier = 1f;
					});
				}
				if (feat != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new PlayerCharacter();
						c.m_Fact = feat.ToReference<BlueprintUnitFactReference>();
					});
				}
				else
				{
					bp.RequirePlotArmor();
				}
				if (alignmentShift != AlignmentShiftDirection.TrueNeutral)
				{
					bp.AlignmentShift = new AlignmentShift
					{
						Direction = alignmentShift,
						Value = 1,
						Description = new LocalizedString()
					};
				}
				configureAnswer?.Invoke(bp);
			});
			answersList.InsertAnswer(answer);
		}

		private static void AddMythicAnswer(BlueprintAnswersList answersList, string id, string answerText, string replyText, string mythicClassGuid, string proficiencyFactName = null, AlignmentShiftDirection alignmentShift = AlignmentShiftDirection.TrueNeutral, int expCr = 5, Action<BlueprintCue> configureReply = null, Action<BlueprintAnswer> configureAnswer = null)
		{
			if (answersList == null)
			{
				return;
			}
			if (!s_MythicClassCache.TryGetValue(mythicClassGuid, out var mythicClass))
			{
				mythicClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>(mythicClassGuid);
				if (mythicClass != null)
				{
					s_MythicClassCache[mythicClassGuid] = mythicClass;
				}
			}
			if (mythicClass == null)
			{
				return;
			}
			BlueprintFeature feat = null;
			if (!string.IsNullOrEmpty(proficiencyFactName))
			{
				feat = GetRequiredDialogueFact(proficiencyFactName);
				if (feat == null)
				{
					return;
				}
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue(id + "Reply", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, replyText);
				configureReply?.Invoke(bp);
				List<BlueprintAnswerBaseReference> answers = bp.Answers;
				if ((answers == null || answers.Count == 0) && (bp.Continue?.Cues?.Count).GetValueOrDefault() == 0)
				{
					bp.SetAnswersList(answersList);
				}
			});
			BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(id, delegate(BlueprintAnswer bp)
			{
				bp.SetText(Main.IsekaiContext, answerText);
				bp.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				bp.ShowOnce = true;
				if (expCr > 0)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(GainExp c)
					{
						c.Encounter = EncounterType.SkillCheck;
						c.CR = expCr;
						c.Modifier = 1f;
					});
				}
				bp.AddShowCondition(delegate(UnitClass c)
				{
					c.Unit = new PlayerCharacter();
					c.m_Class = mythicClass.ToReference<BlueprintCharacterClassReference>();
				});
				if (feat != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new PlayerCharacter();
						c.m_Fact = feat.ToReference<BlueprintUnitFactReference>();
					});
				}
				else
				{
					bp.RequirePlotArmor();
				}
				if (alignmentShift != AlignmentShiftDirection.TrueNeutral)
				{
					bp.AlignmentShift = new AlignmentShift
					{
						Direction = alignmentShift,
						Value = 1,
						Description = new LocalizedString()
					};
				}
				configureAnswer?.Invoke(bp);
			});
			answersList.InsertAnswer(answer);
		}

		private static void InjectCompanionRootHubEncounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("417fa384f3250634bb71859fbc913453");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiSeelahCampFestival", "(Isekai Protagonist) [Otherworld Festivals & Street Food] \"Seelah, back in my world, we had midsummer festivals where folks dressed in full armor just to eat greasy fried street snacks, drink icy cider, and watch fireworks light up the night sky.\"", "{n}Seelah's eyes light up with unvarnished joy, a wide, radiant grin spreading across her face as she leans forward with hearty laughter.{/n} \"Fried street snacks and sky-fire?! Now that sounds like my kind of holiday! Here I thought your realm was all metal carriages and solemn glass towers! Promise me this, Commander: once we kick the demons back into the Abyss and reclaim Drezen, you're teaching the citadel cooks how to make those fried treats!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 250;
						c.Sponsor = "The Lucky Drunk";
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"Fried food and festival fireworks! Cayden approves this doctrine with flying colors!\"</i>";
					});
				});
				AddAlignedAnswer(blueprint, "IsekaiSeelahCampHero", "(Hero) [Good] [Radiance of Hope] \"A paladin's smile is brighter than any sun, Seelah. When fear chills the camp, seeing your laughter reminds everyone why we fight.\"", "{n}Seelah blushes slightly beneath her armor, rubbing the back of her neck with an embarrassed but deeply touched chuckle.{/n} \"Aw, stop it, you're making me blush in broad daylight! But thank you, Commander. Having someone like you watching our backs makes it a whole lot easier to keep smiling.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 3);
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("9497b573bde03724c8e08090509a1774");
			if (blueprint2 != null)
			{
				AddUniversalAnswer(blueprint2, "IsekaiCamelliaCampTalisman", "(Isekai Protagonist) [Otherworlder's Appraisal: Cursed Relics] \"You keep caressing that necklace, Lady Camellia. In my world, antique jewelry that radiates spiritual static usually comes with an exorcism hotline.\"", "{n}Camellia's aristocratic smile freezes for a fraction of a second, her fingers lingering over the polished stone with delicate, practiced elegance.{/n} \"How remarkably fanciful. An 'exorcism hotline'? My spirits require no such vulgar meddling, stranger. They are refined, ancient, and quite... particular. Do mind your own curiosities, lest you uncover matters far beyond your delicate sensibilities.\"", 3);
				AddSubclassAnswer(blueprint2, "IsekaiCamelliaCampOverlord", "(Overlord) [Gaze of Dominion] \"You clutch that trinket like a thief concealing stolen sweets. Remember whose shadow covers this camp, girl.\"", "{n}A shiver runs down Camellia's spine, her breath catching slightly as a flicker of genuine apprehension darkens her eyes before she dips into a stiff, obedient curtsy.{/n} \"Of course... my lord. Your authority is unquestioned.\"", "OverlordProficiencies", 4);
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("66385ad77fa743e4bb1234078dbd804c");
			if (blueprint3 != null)
			{
				AddUniversalAnswer(blueprint3, "IsekaiLannCampComedy", "(Isekai Protagonist) [Deadpan Appreciation] \"Lann, your dry humor would kill at an Otherworld comedy cellar. We call that 'deadpan self-deprecating satire'--people pay good money for sets like yours.\"", "{n}Lann blinks in mock astonishment, carefully checking both hands before letting out a dry, raspy chuckle.{/n} \"People pay money for someone complaining about their awful lifespan and scaly skin? Truly, your world is a paradise of questionable tastes. Maybe I picked the wrong profession--Chief Executive Stand-Up Mongrel has a nice ring to it.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 250;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"The mongrel's comedic timing is second to none! Five stars!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint3, "IsekaiLannCampMartialKata", "(Martial God) [Low-Stance Archery Kata] \"Your reptilian half lowers your center of gravity significantly. Let me show you an archer's root-stance that channels earth ki directly into your draw weight.\"", "{n}Lann mirrors your footing, his yellow eyes widening as his composite bow bends with startling ease and stability.{/n} \"Whoa... the tension on my shoulder blade just dropped by half. That's bizarrely effective. Remind me never to challenge you to an arm-wrestling contest, Commander.\"", "MartialGodProficiencies", 4);
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("ced27e744d2dded40bbb5adf17816dbb");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiWenduagCampGourmet", "(Isekai Protagonist) [Apex Predator's Cuisine] \"Wenduag, in my world, the greatest hunters don't just chew raw gristle; they roast monster meat with crushed herbs and smoked fat. True apex predators savor the feast.\"", "{n}Wenduag tilts her head, her slit pupils narrowing with predatory intrigue as she inhales the savory spice aroma drifting from your mess kit.{/n} \"A hunter who masters fire and seasoning as well as the bow... Interesting. I thought surface-dwellers ate only weak, soft mush. If your spices sharpen my claws and fortify my blood, I will accept your culinary instruction, master.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Dark Prince";
						c.BannerMessage = "<color=#FF4500><b>[The Dark Prince]</b></color>: <i>\"Refining primal savagery into aristocratic indulgence. Excellent form.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint4, "IsekaiWenduagCampMastermind", "(Mastermind) [Calculated Predation] \"Ruthlessness without tactical foresight is merely wasted kinetic force, Wenduag. An apex predator plans the ambush three moves ahead.\"", "{n}Wenduag bows her head, her fangs clicking in respectful agreement.{/n} \"Your intellect is as sharp as a poisoned arrowhead. Teach me how to see through their stratagems.\"", "MastermindProficiencies", 4);
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e41585da330233143b34ef64d7d62d69");
			if (blueprint5 != null)
			{
				AddUniversalAnswer(blueprint5, "IsekaiWoljifCampFranchise", "(Isekai Protagonist) [Corporate Syndicate Franchising] \"Woljif, street thievery has razor-thin margins and high mortality. In my world, syndicates use supply chain logistics and franchise licensing. Ever heard of wholesale distribution?\"", "{n}Woljif's jaw drops, his tail swishing like a metronome as he leans in, his amber eyes sparkling with raw entrepreneurial greed.{/n} \"Franchise licensing?! Wholesale distribution?! Boss, every time you open your mouth, gold coins practically rain from the ceiling! You gotta write this down! If we turn the Thieflings into a legitimate logistics cartel, Irabeth won't even be able to tax us!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Gold = 250;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Teaching the street rogue corporate monopolies! Absolutely scandalous!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint5, "IsekaiWoljifCampSlime", "(Slime) [Acidic Vault Cracking] \"A micro-secretion of acidic fluid dissolves iron lock pins in four seconds without making a sound. Observe.\"", "{n}Woljif stares in sheer awe as a rusted lock dissolves into vapor with a faint hiss.{/n} \"No lockpicks?! Just living acid on demand?! Boss, you are a walking master thief miracle!\"", "DevourerProficiencies", 4);
			}
			BlueprintAnswersList blueprint6 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("4d978cbd2aa780d46874255282039f3f");
			if (blueprint6 != null)
			{
				AddUniversalAnswer(blueprint6, "IsekaiDaeranCampLuxury", "(Isekai Protagonist) [Decadence in War Zones] \"Daeran, your dedication to drinking fine chilled vintage while dodging catapult boulders is almost inspiring. Do you also critique the mud texture while fleeing gargoyles?\"", "{n}Daeran bursts into delighted, theatrical applause, taking an elegant sip from his silver chalice before raising it in a mock toast.{/n} \"Naturally! If one must perish in this wretched crusade, doing so covered in inferior mud while drinking vinegar is a tragedy beyond forgiveness. I see you appreciate the finer nuances of battlefield aesthetics, my dear Commander!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Lucky Drunk";
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"Wine tastes sweetest when the world is burning! Cheers!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint6, "IsekaiDaeranCampGodEmperor", "(God Emperor) [Imperial Aristocracy] \"A true sovereign commands realities, Count Arendae; you merely purchase amusements. Yet your sharp tongue pleases the court.\"", "{n}Daeran smirks, executing a sweeping, exaggerated bow.{/n} \"At your service, Your Imperial Magnificence. Do continue conquering; it makes for splendid dinner conversation.\"", "GodEmperorProficiencies", 4);
			}
			BlueprintAnswersList blueprint7 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("f2a35965e9bc601449498bd022b04d9d");
			if (blueprint7 != null)
			{
				AddUniversalAnswer(blueprint7, "IsekaiEmberCampSweetCake", "(Isekai Protagonist) [Honey-Glazed Pastry] \"Here, Ember. I baked this honey-glazed cake using an Otherworld recipe. In the realm I hail from, people who hold onto kindness through hardship are revered as saints.\"", "{n}Ember's soot-streaked face breaks into a radiant, childlike smile as she takes the pastry in her small, scarred hands, a soft tear tracing down her cheek.{/n} \"It smells like warm mornings and peaceful dreams... Thank you! You're so kind to everyone, even when the world is angry and hurt. I'll share half with Soot!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Dawnflower";
						c.BuffToApply = EmberSparkBuff;
						c.BuffDuration = new TimeSpan(24, 0, 0);
						c.BannerMessage = "<color=#FFD700><b>[The Dawnflower]</b></color>: <i>\"A gentle hand extended to an innocent soul. The sacred starlight shines upon you.\"</i>";
					});
				});
				AddAlignedAnswer(blueprint7, "IsekaiEmberCampHero", "(Hero) [Good] [Vow of Protection] \"Your warmth reaches places iron swords could never touch, Ember. I will make sure no demon or inquisitor ever snuffs out your light.\"", "{n}Ember hugs your arm tightly, her quiet flame flickering with joyful golden warmth.{/n} \"I'm not afraid. As long as you're here, the dark doesn't feel so big anymore.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 3);
			}
			BlueprintAnswersList blueprint8 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("9b38499627b7eda4f86d959f3f222834");
			if (blueprint8 != null)
			{
				AddUniversalAnswer(blueprint8, "IsekaiNenioCampPeerReview", "(Isekai Protagonist) [Academic Peer Review Protocol] \"Nenio, have you considered submitting your encyclopedia for peer review? Back home, if you publish bold theorems without citations, twelve angry department heads duel you with red ink.\"", "{n}Nenio gasps in pure intellectual scandal, her ears flattening back in utter indignation before her quill begins furiously scratching across parchment.{/n} \"Peer review?! By lesser intellects with inferior cranial capacity?! Preposterous! My axioms are self-evident empirical absolute truths! However... the concept of an organized academic duel using colored ink warrants extensive taxonomic documentation! Proceed with your testimony, specimen!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Hermit";
						c.BannerMessage = "<color=#9370DB><b>[The Hermit]</b></color>: <i>\"Academic bureaucracy crossing dimensional thresholds. Fascinating scholarly friction.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint8, "IsekaiNenioCampMastermind", "(Mastermind) [Cognitive Defragmentation] \"Your empirical observations are encyclopedic, Nenio, but your cognitive memory cache suffers from severe indexing fragmentation. Adopt my hexadecimal filing matrix.\"", "{n}Nenio freezes, staring at your diagram with bulging eyes before letting out a high-pitched squeak of comprehension.{/n} \"Hexadecimal categorization matrix?! My data retrieval latency will drop by sixty-four percent! Extraordinary!\"", "MastermindProficiencies", 4);
			}
			BlueprintAnswersList blueprint9 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("129b55b8b5d50974f84f7c607d894fd0");
			if (blueprint9 != null)
			{
				AddUniversalAnswer(blueprint9, "IsekaiSosielCampPerspective", "(Isekai Protagonist) [Linear Perspective & Optical Realism] \"Sosiel, capturing light requires understanding optical geometry. In my world, Renaissance painters revolutionized art by using vanishing points and chromatic gradients.\"", "{n}Sosiel lowers his brush, his gentle eyes widening as he studies the geometric sketch you draw in the corner of his easel.{/n} \"A vanishing point on the horizon line... so that every angle recedes toward infinity? Sweet Shelyn, the illusion of three-dimensional depth is breathtaking! You see the world not merely as pigment and canvas, but as living geometry. Thank you, Commander; this changes everything.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"Art and celestial geometry intertwined. A beautiful stroke of inspiration!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint9, "IsekaiSosielCampMartial", "(Martial God) [Breathing of the Brush] \"The brush stroke and the glaive thrust require the exact same foundation: absolute stillness of breath before instantaneous motion.\"", "{n}Sosiel nods in quiet realization, steadying his grip as his brush touches the canvas with newfound precision.{/n} \"Stillness before motion... Yes. I can feel the tension leave my shoulder. Remarkable advice, Commander.\"", "MartialGodProficiencies", 4);
			}
			BlueprintAnswersList blueprint10 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("2366a8db6481070439fee222c0c52e45");
			if (blueprint10 != null)
			{
				AddUniversalAnswer(blueprint10, "IsekaiRegillCampCorporate", "(Isekai Protagonist) [Corporate Supply Chain Efficiency] \"Regill, have you ever considered corporate executive logistics? In my world, directors who can ruthlessly optimize shift schedules and eliminate supply waste make seven figures.\"", "{n}Regill slowly turns his slate, his cold amber eyes piercing yours with unblinking scrutiny.{/n} \"Financial compensation is irrelevant. Waste is treason against operational victory. If your world recognizes that sentiment and disciplines inefficiency without sentimental hesitation, then its governance possesses merit. Return to your duties, Commander; our frontline rotation begins in eight minutes.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"A mutual appreciation for unyielding discipline and zero wasted motion.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint10, "IsekaiRegillCampOverlord", "(Overlord) [Absolute Order] \"Your Order of the Godclaw builds a sturdy foundation of obedience, Paralictor. Keep your ranks sharp; sovereign standards demand perfection.\"", "{n}Regill renders a crisp, immaculate Chelish salute.{/n} \"The Order does not tolerate deficiency, my lord. We stand ready.\"", "OverlordProficiencies", 4);
			}
			BlueprintAnswersList blueprint11 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("03ebad9587cbea0438d901a0f8df44f1");
			if (blueprint11 != null)
			{
				AddUniversalAnswer(blueprint11, "IsekaiArueshalaeCampLucid", "(Isekai Protagonist) [Psychology of Lucid Dreaming] \"Arueshalae, dreams are proof of a soul's autonomy. In my world, scholars write entire libraries about lucid dreaming: the realization that within your mindscape, you have absolute power to rewrite your nature.\"", "{n}Arueshalae's eyes glisten like starlight on water, a soft gasp escaping her lips as she presses both hands against her heart.{/n} \"'Absolute power to rewrite your nature'... You speak of dreams not as fleeting illusions, but as sanctuaries of true self-determination. Whenever darkness threatens to drown me, I will remember your words across the stars.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"Desna's starlight dances in her heart. Dreams transcend all darkness!\"</i>";
					});
				});
				AddAlignedAnswer(blueprint11, "IsekaiArueshalaeCampHero", "(Hero) [Good] [Every Step is a Triumph] \"Every single step you take away from the Abyss is greater than slaying a demon lord. Keep walking forward; I'll always have your back.\"", "{n}Arueshalae smiles through a shimmer of grateful tears, nodding with quiet, unwavering resolve.{/n} \"With you walking beside me, Commander, I know I will never falter.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 3);
			}
			BlueprintAnswersList blueprint12 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("174d6c94b6725f44aad1d2a76993a926");
			if (blueprint12 == null)
			{
				return;
			}
			AddUniversalAnswer(blueprint12, "IsekaiGreyborCampContract", "(Isekai Protagonist) [Security Contractor Retainers & Hazard Multipliers] \"Greybor, what are your standard freelance consulting rates? In my world, top-tier private defense contractors charge monthly retainers plus hazardous assignment multipliers.\"", "{n}Greybor puffs slowly on his pipe, an appreciative glint appearing in his heavy-lidded eyes as he blows a neat smoke ring.{/n} \"Hazard multipliers and monthly retainers? Now you're speaking the language of true professionals, Commander. Most crusaders try to pay in holy blessings and patriotic speeches. You understand that when axes swing, coin talks. Keep those retainers coming, and my blades are yours to the end.\"", 3, null, delegate(BlueprintAnswer bp)
			{
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 300;
					c.Gold = 250;
					c.Sponsor = "The Dark Prince";
					c.BannerMessage = "<color=#FF4500><b>[The Dark Prince]</b></color>: <i>\"Gold and iron seal the pact. Professional excellence rewarded.\"</i>";
				});
			});
			AddSubclassAnswer(blueprint12, "IsekaiGreyborCampMartial", "(Martial God) [Dual-Axe Rotational Torque] \"Dual-wielding heavy handaxes requires exceptional wrist tendon conditioning and continuous rotational momentum. Respectable balance, dwarf.\"", "{n}Greybor grunts in genuine professional respect, testing the weight of his left axe with a crisp, fluid snap.{/n} \"You know your weaponry. Few commanders recognize the torque behind a short backswing. Honored to stand in your company.\"", "MartialGodProficiencies", 4);
		}

		private static void InjectEarlyPrologueEncounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("87997a477e6a58d4aae46e26a3712825");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiPrologueSquareAwakening", "(Isekai Protagonist) [Otherworlder's Instinct] \"Alright, deep breaths... let's test the classic protocol. 'Status Window, open!' ...Menu? Character sheet? System inventory? ...Nothing. No floating blue display, no cheerful tutorial fairy. Just cold cobblestones, church bells, and a terrifyingly real wound in my chest.\"", "{n}The attending priestess gasps in alarm, frantically pressing a cool, glowing palm against your brow while looking anxiously toward her fellow clerics.{/n} \"Merciful gods, lie still! He is delirious--muttering strange incantations about floating windows and invisible menus while clutching his heart! Peace, traveler, you are safe in Kenabres. By the grace of the Inheritor and our noble dragon protector, the wound is closing. Take this blessed relic shard; let its sacred starlight steady your fevered spirit until the festival concludes.\"", 2, delegate(BlueprintCue bp)
				{
					BlueprintCue blueprint19 = BlueprintTools.GetBlueprint<BlueprintCue>("76c6a0e7880db144c9138c6b39d386d9");
					if (blueprint19 != null)
					{
						bp.Continue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { blueprint19.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
					}
				}, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Fading Dragon";
						c.DirectItems = new BlueprintItem[1] { ItemSilverDragonShardPendant };
						c.BannerMessage = "<color=#FFD700><b>[The Fading Dragon]</b></color>: <i>\"A brave soul from beyond the threshold stirs! Accept this blessing, traveler.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e27807b731f3b1a4eb19c1a04fdfcf53");
			if (blueprint2 != null)
			{
				AddUniversalAnswer(blueprint2, "IsekaiPrologueSquareHulrun", "(Isekai Protagonist) [Otherworlder's Frank Confession] \"I didn't sneak through your checkpoints, Prelate. The truth is far stranger: I am an Otherworlder. In the realm I came from, there are spires of glass and steel that scrape the clouds, horseless carriages of metal, and nights illuminated by harnessed lightning. I went to sleep in my world and woke up bleeding on your cobblestones.\"", "{n}Hulrun's weathered face twitches with severe suspicion, his knuckles tightening on his halberd until the metal creaks. He stares at you in complete, dumbfounded disbelief before letting out a harsh, rasping snort of exasperation.{/n} \"'Towers of glass'? 'Harnessed lightning'? By the Inheritor's shield, the blow to your skull must have scrambled your brains entirely! You are no demonic saboteur--even the most half-witted cultist of Baphomet would invent a less preposterous tale. Take Terendelev's festival stipend and report to the cathedral's infirmary before you collapse and make a spectacle of yourself on a holy day.\"", 3, delegate(BlueprintCue bp)
				{
					BlueprintCue blueprint19 = BlueprintTools.GetBlueprint<BlueprintCue>("ba9c82193a32275408973a8aebdb3a6d");
					if (blueprint19 != null)
					{
						bp.Continue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { blueprint19.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
					}
				}, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 150;
						c.Coins = 350;
						c.Sponsor = "The Grand Arbiter";
						c.ItemGuids = new string[3] { "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91" };
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Honesty so absurd it completely disarms inquisitorial paranoia. Superb opening performance.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("0f7dddeb3f77a4f408e7dc843b9c66fb");
			if (blueprint3 != null)
			{
				AddUniversalAnswer(blueprint3, "IsekaiPrologueSquareTerendelev", "(Isekai Protagonist) [Awe of the Reincarnated] \"In the world I came from, beings of your legendary grace existed only in bedtime chronicles and faded tapestries. Even standing before me in mortal guise, the ancient nobility of your presence is unmistakable. It is an honor, Lady Terendelev.\"", "{n}The silver-haired noblewoman smiles with gentle warmth, her luminous, liquid-silver eyes shimmering with ancient draconic wisdom as she studies your face.{/n} \"You speak with the quiet wonder of a traveler who has crossed unfathomable oceans of stars, child of another sky. Even through your injuries, I can feel it--an extraordinary spark slumbering deep within your soul, foreign to this world yet pure and untainted by the Abyss. Take this silver tear as my keepsake. Whatever trials await you beneath these heavens, let my light shield you from the dark.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Fading Dragon";
						c.DirectItems = new BlueprintItem[1] { ItemTearOfSilverDragon };
						c.BuffToApply = SilverDragonAegisBuff;
						c.BuffDuration = new TimeSpan(24, 0, 0);
						c.BannerMessage = "<color=#FFD700><b>[The Fading Dragon]</b></color>: <i>\"My guardian flame shall watch over your footsteps, Otherworlder.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a3e8939fe8354cb4698403e4256fe0d9");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiPrologueStitchGreedyHoard", "(Isekai Protagonist) [Pragmatic Survivalist: Take Both] \"Listen to me, little one! When the sky tears open and demonic locusts rain from the heavens, you don't choose between weapons and medicine--you take both or you die! Hand over the crossbow, the cold iron bolts, and every healing draught in that satchel! Right now!\"", "{n}Stitch shrieks in utter panic as demonic rubble crashes down across the square, frantically shoving his entire leather satchel into your arms with trembling claws.{/n} \"Take it! Take the bow, take the bolts, take Stitch's lucky draughts! Just don't let those winged bug monsters rip my skin off!\"", 2, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Lucky Drunk";
						c.ItemGuids = new string[7] { "511c97c1ea111444aa186b1a58496664", "464ecede228b0f745a578f69a968226d", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "bc93a78d71bef084fa155e529660ed0d", "bc93a78d71bef084fa155e529660ed0d" };
						c.DirectItems = new BlueprintItem[1] { CodexOfReincarnation.ItemCodexOfReincarnation };
						c.BuffToApply = StitchWardBuff;
						c.BuffDuration = new TimeSpan(1, 0, 0);
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"Hahaha! That's the spirit! Squeeze every coin and arrow out of the universe!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a55fc20c6f0ff56439b40d6ba53cb8d7");
			if (blueprint5 != null)
			{
				AddUniversalAnswer(blueprint5, "IsekaiPrologueWakingUp", "(Isekai Protagonist) [Steeled Otherworlder] \"One moment I was falling between dimensions, and the next I was tumbling down a subterranean fissure with a dragon battling a demon lord. My head is pounding, but my pulse is steady and I'm still breathing. Check your gear, paladin; we're climbing out of this dark pit together.\"", "{n}Seelah blinks through the limestone dust coating her brow, then lets out a hearty, surprised laugh as she extends a gauntleted hand to haul you upright.{/n} \"Another world? Well, whatever distant realm you tumbled out of, they certainly breed folk with backbone! Name's Seelah. Grab some steel, friend--let's see what's waiting for us in the dark.\"");
				AddSubclassAnswer(blueprint5, "IsekaiPrologueMartialGodWaking", "(Martial God) [Ki Circulation] \"My meridians are completely clear and circulating smoothly. Tend to Anevia's wounds; I will secure the perimeter.\"", "{n}Seelah nods respectfully at your effortless stance.{/n} \"Now that's what I call crusader discipline! Lead on, warrior!\"", "MartialGodProficiencies", 3);
			}
			BlueprintAnswersList blueprint6 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("1ca6cf08fceeac141a0df689cecc784a");
			if (blueprint6 != null)
			{
				AddUniversalAnswer(blueprint6, "IsekaiPrologueCamelliaAppraisal", "(Isekai Protagonist) [Discerning Gaze] \"You can lower the delicate aristocratic mask, Lady Camellia. The tremble in your fingers isn't horror at this corpse--it's an intoxicating thrill. I know a hidden hunger when I look into someone's eyes. Direct that bloodlust toward the demons trying to devour this city, and we won't have an issue.\"", "{n}Camellia's aristocratic composure stiffens, her knuckles whitening around her silver rapier as a dark, dangerous spark flares in her emerald eyes. For a heartbeat, the mask slips entirely before she recovers with a tight, breathy chuckle.{/n} \"How remarkably perceptive... and how delightfully insolent. You speak as though you can read my very soul, stranger. Very well. As long as our blades point at the same fiends, I suppose I can tolerate your company.\"", 3);
				AddSubclassAnswer(blueprint6, "IsekaiPrologueCamelliaOverlord", "(Overlord) [Gaze of the Sovereign] \"You tremble with amateur bloodlust, girl. Learn to master your hunger before a true sovereign of death, or be erased.\"", "{n}Camellia shivers uncontrollably as a chilling darkness grips her throat.{/n} \"Understood... my lord. I shall obey.\"", "OverlordProficiencies", 4);
			}
			BlueprintAnswersList blueprint7 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("da6ca50574b6c714f9166f37b64b7b59");
			if (blueprint7 != null)
			{
				AddUniversalAnswer(blueprint7, "IsekaiPrologueAneviaBoneMend", "(Isekai Protagonist) [Otherworlder's Field Medicine] \"Bite on this leather strap and take a deep swallow of this tonic while I realign the splintered bone. Where I come from, proper traction and focused mana can knit a compound fracture cleanly before shock sets in.\"", "{n}With a firm, practiced twist and a soothing pulse of channeled celestial mana, the jagged bone snaps cleanly back into alignment. Anevia gasps, trembling as the searing agony subsides into a cool, steady numbness.{/n} \"Sweet Desna... the burning fire in my shin is gone. How did you--who taught you hands like that?! Thank you, friend. You just saved me from being dead weight in this grave.\"", 3);
				AddSubclassAnswer(blueprint7, "IsekaiPrologueAneviaSlimeCast", "(Slime) [Biomechanical Secretion Cast] \"My fluid secretion can form a semi-rigid biomechanical cast around your fracture in five seconds, stabilizing the marrow.\"", "{n}Anevia stares in awe as the painless translucent splint locks her leg safely in place.{/n} \"A living splint?! That's bizarre, but I can actually walk on it! You're a lifesaver!\"", "DevourerProficiencies", 4);
			}
			BlueprintAnswersList blueprint8 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("61ba231b3ad6b144a918c88ca49cb92a");
			if (blueprint8 != null)
			{
				AddUniversalAnswer(blueprint8, "IsekaiPrologueMeetLannDiplomat", "(Isekai Protagonist) [Otherworlder's Diplomatic Hand] \"Lower your bows, scouts. I am an Otherworlder who just survived a fall from the surface alongside your crusader kin. We seek an escape route, not a quarrel. Share your path, and I will share our provisions and stand with you against whatever prowls these tunnels.\"", "{n}Lann and Wenduag exchange wary glances, their tensed bowstrings gradually easing as they take in your calm, dignified bearing and open hands.{/n} \"An Otherworlder? Surface-dwellers usually scream at our scales or draw steel without asking questions. If you've got guts and good intentions, we can talk. I'm Lann, and this is Wenduag. Welcome to the undercity.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Song of the Spheres";
						c.ItemGuids = new string[3] { "adc1a081ba26d2a439a62d702437a40d", "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"A peaceful hand extended in the darkness. The stars illuminate your path!\"</i>";
					});
				});
			}
			BlueprintBookPage blueprint9 = BlueprintTools.GetBlueprint<BlueprintBookPage>("ee776f7d205f3484e9def0e007c5623f");
			if (blueprint9 != null)
			{
				AddBookPageAnswer(blueprint9, "IsekaiStorybookPage1DualAction", "(Isekai Protagonist) [Dual Manifestation of Dawn] Draw upon the boundless wellspring of your otherworld soul, dividing your mana into twin radiant currents: weave a shimmering cocoon of starlight around the dying mongrel maiden while unleashing a searing wave of celestial dawnfire to incinerate the chanting cultists.", "2212f276d642d1d4fb8f93a0b74cb320", 3, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Dawnflower";
						c.DirectItems = new BlueprintItem[1] { ItemLarielFeatherCirclet };
						c.BannerMessage = "<color=#FFD700><b>[The Dawnflower]</b></color>: <i>\"Mercy and vigilance intertwined. The golden flame burns brightest in your hands.\"</i>";
					});
				});
			}
			BlueprintBookPage blueprint10 = BlueprintTools.GetBlueprint<BlueprintBookPage>("2212f276d642d1d4fb8f93a0b74cb320");
			if (blueprint10 != null)
			{
				AddBookPageAnswer(blueprint10, "IsekaiStorybookPage12SoulAegis", "(Isekai Protagonist) [Dimensional Aegis of the Soul] \"A demon's psychic phantoms? My consciousness has crossed the unfathomable abyss between dimensions and gazed into the infinite void. Your petty illusions carry no authority against an anchored Otherworlder.\" Shatter Savameleh's vision with an effortless burst of mental clarity.", "77ec7207718bf02409322455b1fbbf6c", 3, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Grand Arbiter";
						c.BuffToApply = MindOfTheOtherworlderBuff;
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Demon psychic manipulation completely neutralized by dimensional displacement.\"</i>";
					});
				});
			}
			BlueprintBookPage blueprint11 = BlueprintTools.GetBlueprint<BlueprintBookPage>("a6df6ed440dce0e499eb66b832243229");
			if (blueprint11 != null)
			{
				AddBookPageAnswer(blueprint11, "IsekaiStorybookPage33DualCovenant", "(Isekai Protagonist) [Dual Covenant: Sovereign of Heaven] \"True righteousness is never a choice between compassion and wrath. The same hand that shields the helpless must wield the blade that strikes down their tormentors. I accept both covenants under the standard of the Otherworlder!\"", "d1da445d30718914a8b7794864af8039", 4, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Inheritor";
						c.FeatureToGrant = LarielsVowFeature;
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"A commander who refuses compromise between mercy and justice. Well chosen!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint12 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("c4d71c5f339b4564da1f4a86d323249e");
			if (blueprint12 != null)
			{
				AddUniversalAnswer(blueprint12, "IsekaiVendougMasterStrategist", "(Isekai Protagonist) [Master Strategist of the Otherworld] \"Lower your bow, Wenduag. I am neither a reckless zealot nor a blind fool. I will reveal the angelic light to Chief Sull so the tribe rallies behind our cause and bestows their ancestral blessings, but I will order the young hunters to fortify Neathholm's barricades rather than march into a meat grinder. Your hunters survive, the tribe unites, and you and I take the Shield Maze together.\"", "{n}Wenduag freezes, her amber eyes widening as she scrutinizes your calm, unflinching expression. A slow, predatory smile parts her fangs.{/n} \"You... you aren't an idealistic crusader puppy at all. You think like a true chieftain playing three moves ahead. Very well. Take this stash of ambush gear and my scouting maps of the maze. If you can truly pull this off, my bow belongs to you.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 350;
						c.Coins = 300;
						c.Sponsor = "The Laughing King";
						c.ItemGuids = new string[5] { "adc1a081ba26d2a439a62d702437a40d", "2983c97b71ba60443b1c749854ef53a1", "2983c97b71ba60443b1c749854ef53a1", "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Playing both sides like a seasoned guild grandmaster! Exquisite diplomacy!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint13 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("5233c72929cda7c48b92ec6292a730ac");
			if (blueprint13 != null)
			{
				AddUniversalAnswer(blueprint13, "IsekaiSulHarmoniousRevelation", "(Isekai Protagonist) [The Harmonious Revelation] \"Look upon the sacred light of the First Crusaders, Chief Sull, and let hope return to the children of the stone! But heed my words: do not send your youth to die unprepared in the dark. Wenduag's hunters shall stand guard over Neathholm's walls, while my companions and I carve a path straight through Hosilla's lair!\"", "{n}Chief Sull falls to his knees upon the cavern floor, thick tears rolling over his weathered scales. Behind him, the entire cavern gasps in religious reverie, while Wenduag watches you from the shadows with newfound reverence.{/n} \"The holy light... the sword of the angels has returned to us! And your wisdom preserves our sons and daughters from a pointless massacre. Noble traveler, take this ancestral amulet of our elders. May the spirits of the deep stone shield your chest in battle!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The Song of the Spheres";
						c.ItemGuids = new string[1] { "209960f8d60e23a4c8e75d14e2261263" };
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"Hope rekindled beneath the stones. The stars smile upon Neathholm!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint14 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a6b5ef97dca04894a8cc7984bff5a592");
			if (blueprint14 != null)
			{
				AddUniversalAnswer(blueprint14, "IsekaiHorgusExecutiveRetainer", "(Isekai Protagonist) [Shrewd Executive Retainer] \"Master Gwerm, highborn titles and family crests won't parry a fiend's claws in these tunnels, but gold can certainly buy the finest protection in Kenabres. If you desire my personal vanguard to escort you safely to your mansion's doorstep, you will pay five hundred pieces of gold upfront as an executive retainer, and the remaining bounty once you are sitting comfortably in your parlor.\"", "{n}Horgus's face flushes a violent shade of purple, sputtering with wounded aristocratic pride before casting a terrified glance at the cavern shadows and furiously slapping a heavy purse into your palm.{/n} \"Five hundred gold pieces upfront?! You are an unprincipled brigand in crusader armor! ...Take it! Take the coin, you extortionist, but if a single mongrel or demon lays a finger on my velvet doublet, I'll have you thrown into the deepest dungeon in Mendev!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 500;
						c.Coins = 300;
						c.Sponsor = "The Dark Prince";
						c.BannerMessage = "<color=#FF4500><b>[The Dark Prince]</b></color>: <i>\"A contract sealed in gold before the blade is drawn. Commendable pragmatism.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint15 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e02baf0076ae8e9499fe1a1f8e89f05c");
			if (blueprint15 != null)
			{
				AddUniversalAnswer(blueprint15, "IsekaiShieldMazeDualRecruitment", "(Isekai Protagonist) [The Absolute Command: Dual Allegiance] \"Lower your bows, both of you. I reject this petty, tragic feud. Wenduag, your thirst for strength will never be quenched by bowing to a pathetic demon lackey; follow me, and I will show you power that bends the heavens. Lann, your duty to Neathholm demands that the mongrel race stands united, not torn apart by fratricide. Both of you sheath your weapons and march under my banner together right now!\"", "{n}Lann and Wenduag exchange stunned, wide-eyed glances, completely disarmed by the sheer gravitational weight of your command. Wenduag slowly lowers her composite bow, bowing her head in fierce submission, while Lann lets out a breathless, disbelieving laugh as he shakes his head.{/n} \"Both of us...? Under your standard? You crazy, magnificent bastard... you're actually serious. Alright, Commander. If you're crazy enough to lead us both, we'll follow you to the gates of the Abyss itself.\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle<ContextActionLannWenduagDualRecruit>();
				});
			}
			BlueprintAnswersList blueprint16 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("65395d8277d3b9b4f82f068616de8a56");
			if (blueprint16 != null)
			{
				AddUniversalAnswer(blueprint16, "IsekaiPrologueCaveBickeringMediation", "(Isekai Protagonist) [Diplomatic De-escalation Protocol] \"Cease your bickering, both of you. Master Gwerm, shouting at an injured crusader will not mend the ceiling or hasten our ascent. Anevia, save your breath for the climb. We survive together or perish individually--conserve your energy for the fiends ahead.\"", "{n}Horgus shuts his mouth with an indignant snap, smoothing his velvet lapels in embarrassed silence, while Anevia lets out a weary, appreciative chuckle.{/n} \"You heard the commander, nobleman. Let's keep moving before more rocks drop on our skulls.\"", 2, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The World Sovereign";
						c.BannerMessage = "<color=#FFD700><b>[The World Sovereign]</b></color>: <i>\"A true commander brings immediate order to panic and discord.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint17 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("4b2c98a24f2bb304f8ee50286eec3f90");
			if (blueprint17 != null)
			{
				AddUniversalAnswer(blueprint17, "IsekaiPrologueDyraAppraisal", "(Isekai Protagonist) [Appraisal of the Otherworld] \"Let me look over those salvage bundles, Dyra. In my world, distinguishing rare alchemical reagents from common scrap was second nature. There are preserved restorative tinctures hidden under that tarpaulin.\"", "{n}Dyra blinks in astonishment, her trembling hands sifting through the grime-caked satchels until she uncovers three perfectly preserved crystal phials.{/n} \"By the spirits... you spotted these through the ash and leather?! Most surface folk call all of this junk! Here, keep these draughts, sharp-eyed traveler, and may fortune follow your footsteps!\"", 2, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Lucky Drunk";
						c.ItemGuids = new string[3] { "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613", "bc93a78d71bef084fa155e529660ed0d" };
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"A keen eye turns subterranean rubbish into liquid gold! Bottoms up!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint18 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("d35d5c76186af32468dc71952eb339ec");
			if (blueprint18 == null)
			{
				return;
			}
			AddUniversalAnswer(blueprint18, "IsekaiPrologueHosillaInvalidation", "(Isekai Protagonist) [Psychological Disruption] \"You pledge your soul to a cowardly demon lord hiding in the dark, Hosilla? In the universe I hail from, petty cultists like you are merely the introductory warmup encounter. Your dark god cannot shield you from what I bring.\"", "{n}Hosilla's sneer twists into fury, though a flicker of cold dread passes behind her eyes as she raises her weapon.{/n} \"Blasphemous worm! Baphomet will strip the flesh from your bones! Slay them all!\"", 3, null, delegate(BlueprintAnswer bp)
			{
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 350;
					c.Sponsor = "The Inheritor";
					c.ItemGuids = new string[2] { "2983c97b71ba60443b1c749854ef53a1", "2983c97b71ba60443b1c749854ef53a1" };
					c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"Righteous mockery breaks the cultist's resolve before steel even clashes!\"</i>";
				});
			});
		}

		private static void InjectAct1Encounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a9e697b9ad6fa1c4d9c2ae0895bd56d2");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiWoljifSyndicateContract", "(Isekai Protagonist) [Otherworlder's Syndicate Contract] \"I know exceptional talent when I see it, Woljif. A gifted scoundrel with fiendish arcane cunning is far too valuable to waste on a damp dungeon floor. Sign a personal retainer contract with me right now: lend your daggers and lockpicks to my vanguard, and I will personally guarantee your freedom from Irabeth's gallows and the Thieflings' blades.\"", "{n}Woljif's eyes light up with unbridled joy, his tail swishing wildly as he eagerly thrusts his clawed hand through the iron bars to seal the pact.{/n} \"A real contract?! With personal protection from the top brass?! Deal, boss! A thousand times deal! You've got an eye for diamond-grade talent! Irabeth's jaw is gonna hit the tavern floor when she sees you walking me out!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Laughing King";
						c.DirectItems = new BlueprintItem[1] { ItemThieflingCharmBracelet };
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Recruiting the trickster right from under the inquisitors' noses! Glorious!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("871af36f2ab2b1f40b5de77976c54276");
			if (blueprint2 != null)
			{
				AddUniversalAnswer(blueprint2, "IsekaiDefendersHeartCouncilDoctrine", "(Isekai Protagonist) [Otherworlder's Urban Defense Doctrine] \"Irabeth, despair will cost us this city faster than any demon blade. Where I come from, we defend fortified positions through interlocking barricades, reserve strike teams, and controlled choke points. Take this four hundred gold to reinforce the barricades and supply the militia; we will hold Defender's Heart together.\"", "{n}Irabeth's tired shoulders straighten, a spark of genuine hope kindling in her heavy eyes as she takes the coin and studies your defensive layout.{/n} \"Interlocking barricades and reserve runners... by the gods, you speak with the authority of a seasoned fortress commander! With your gold and this doctrine, Defender's Heart will not break! Thank you, friend.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 400;
						c.Coins = 400;
						c.Sponsor = "The Inheritor";
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"A commander who turns panic into ironclad discipline. Defender's Heart shall stand!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("0637eb6a25ff1f34c8cc800e53604705");
			if (blueprint3 != null)
			{
				AddUniversalAnswer(blueprint3, "IsekaiFaxonPreemptiveAmbush", "(Isekai Protagonist) [Pre-emptive Tactical Deconstruction] \"Save your theatrical chanting, Faxon. Your clandestine timetable for the assault on Defender's Heart was leaked hours ago. We've already inverted your defensive perimeter and sealed the cellar escapes. Relinquish your orders and surrender the tower, or face immediate annihilation.\"", "{n}Faxon's fanatical grin evaporates into sheer, blood-chilling terror. He frantically glances toward the shadowed rafters as his cultist acolytes begin to murmur in panicked paranoia.{/n} \"Our timetable?! The sealed cellars?! How could you know our exact deployment?! Traitors! There are heretics in our ranks!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 500;
						c.Coins = 400;
						c.Sponsor = "The Inheritor";
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"The fortress of righteousness strikes before the serpent can strike!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("583314478921a19419304d5f83b72109");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiDaeranAristocraticWit", "(Isekai Protagonist) [Aristocratic Meta-Wit] \"Hosting an extravagant masquerade banquet in the midst of a demonic siege while cutthroats linger behind the velvet curtains? Count Arendae, your flair for theatrical decadence is eclipsed only by your spectacular indifference to mortal peril.\"", "{n}Daeran swirls the vintage wine in his golden goblet, a dazzling, amused smile breaking across his handsome features as he looks you up and down.{/n} \"Oh, delightful! An uninvited guest who speaks fluent insolence rather than suffocating crusader righteousness! Tell me, my sudden savior, did you kick down my door to lecture me on civic duty, or simply to admire the exquisite irony of my final vintage?\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Two masters of insolence meeting in the ruins. Pure entertainment!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("4bba85261e5f2064989ebdc878c0228a");
			if (blueprint5 != null)
			{
				AddUniversalAnswer(blueprint5, "IsekaiMeetEmberGuardianAegis", "(Isekai Protagonist) [Otherworlder's Guardian Intervention] \"Stand down and lower your halberds, crusaders, before you stain what remains of your honor! Murdering a defenseless, barefoot child in the name of the Inheritor is the swiftest road to becoming the very monsters you fight!\"", "{n}The crusaders blanch beneath the sheer, suffocating authority radiating from your voice. Shamed and shaken, they slowly lower their weapons and retreat into the rubble. Ember looks up with enormous, luminous eyes, clutching her soot-stained ragdoll to her chest with a wondrous smile.{/n} \"Your heart... it glows with such a warm, golden fire, stranger. Like sunshine after a long winter night. Can I walk with you? The street was getting so cold...\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Dawnflower";
						c.BuffToApply = EmberSparkBuff;
						c.BannerMessage = "<color=#FFD700><b>[The Dawnflower]</b></color>: <i>\"Blessed are the guardians of the innocent. Her spark is safe in your warmth.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint6 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("16d9e0a48d5746a4fbb1497e4ee4cf81");
			if (blueprint6 != null)
			{
				AddUniversalAnswer(blueprint6, "IsekaiDesnaTempleSanctuaryHarmony", "(Isekai Protagonist) [Resonance of the Starlit Void] \"The music of the Song of the Spheres carries across all worlds and dimensions. Be at peace, faithful of Desna; your sanctuary shall not fall to paranoia or demon claws while I draw breath.\"", "{n}A celestial bell rings through the ruined shrine, a chorus of spectral starlight swirling around your shoulders as the nervous acolytes bow in profound gratitude.{/n} \"The stars have brought us a protector who hears the heavenly song! Take our blessings and these draughts, starlit traveler!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Song of the Spheres";
						c.ItemGuids = new string[2] { "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"The celestial chimes resonate with your otherworldly presence!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint7 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("497b79c111a66034c920842d33b9ce71");
			if (blueprint7 != null)
			{
				AddUniversalAnswer(blueprint7, "IsekaiBlackWingLibraryInquisitorExpose", "(Isekai Protagonist) [Inquisitorial Jurisdiction Expose] \"You can drop the pious facade, Chaleb. Prelate Hulrun's official inquisitorial warrants bear the crimson wax seal of the Third Crusade, which your forged parchment noticeably lacks. Step away from the scholars and put down that torch before I remove your head from your shoulders.\"", "{n}Chaleb's self-righteous sneer freezes in terror, the torch trembling in his grasp as he realizes his ruse has been completely dismantled.{/n} \"The wax seal...?! How could a wretched vagabond know the internal seals of the Inquisition?! Kill them! Kill them before they ruin everything!\"", 3);
				AddSubclassAnswer(blueprint7, "IsekaiBlackWingHeroRighteousness", "(Hero) [Voice of Righteousness] \"By the light of Heaven, stand down, false inquisitor! No true servant of justice burns books and slaughters defenseless scholars!\"", "{n}The terrified cultists falter under your radiant conviction, several dropping their torches in panic.{/n}", "HeroProficiencies", 4);
			}
			BlueprintAnswersList blueprint8 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("833a8b5997b48b645b386b4fcb518134");
			if (blueprint8 != null)
			{
				AddUniversalAnswer(blueprint8, "IsekaiTopazChemicalProtocol", "(Isekai Protagonist) [Alchemical Threat Deconstruction] \"Hold your step. Notice the hairline groove beneath that third floorboard and the volatile phosphor flasks suspended on the tripwire. Step aside while I neutralize the reaction with a pinch of alkaline salt.\"", "{n}With a steady hand and a quick dusting of reagent powder, the deadly alchemical trap hisses softly and goes inert, preserving the entire cache safely intact.{/n}", 3);
				AddSubclassAnswer(blueprint8, "IsekaiTopazSlimeAbsorption", "(Slime) [Chemical Toxin Absorption] \"Poisonous acidic vapors? My slime body absorbs and converts chemical toxins into harmless mineral crystals.\"", "{n}You engulf the noxious vapors in seconds, leaving the cellar sparkling clean.{/n}", "DevourerProficiencies", 4);
			}
			BlueprintAnswersList blueprint9 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("833a8b5997b48b645b386b4fcb518134");
			if (blueprint9 != null)
			{
				AddUniversalAnswer(blueprint9, "IsekaiSilkenThreadEvacuation", "(Isekai Protagonist) [Calculated Alley Evacuation] \"Gather your tools, stay low, and follow my lead. The demonic prowlers in the adjacent plaza operate on a forty-second patrol loop; we have a clear window through the eastern courtyard right now. Move!\"", "{n}The terrified artisans hustle quietly along your guided route, slipping through the ruined alleys without alerting a single demon scout.{/n}", 3);
			}
			BlueprintAnswersList blueprint10 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("cc0bf28eab0e60b429dac7b53ec8e4d2");
			if (blueprint10 != null)
			{
				AddUniversalAnswer(blueprint10, "IsekaiMarketSquareReconcile", "(Isekai Protagonist) [Sovereign Reconciliation] \"Lower your steel, both of you! Hulrun, your obsession with hidden demons is blinding you to real allies. Ramien, your secretiveness is feeding his paranoia. Kenabres is on fire, and you two are squabbling like children in the ashes! You will lay down your grudges and unite your faiths under my command right now, or I will remove you both from this city's defense!\"", "{n}Both the iron prelate and the Desnan priest freeze in stunned disbelief under the crushing authority of your voice. Hulrun's jaw clenches tightly before he slowly grounds his halberd, while Ramien lets out a long, trembling breath of relief.{/n} \"Under your command... such audacity, yet your conviction cuts deeper than any demon's blade. Very well, stranger. For the survival of Kenabres, the Inquisition and the clergy shall stand together.\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle<ContextActionExposeMimicAndReconcile>();
				});
				AddSubclassAnswer(blueprint10, "IsekaiMarketSquareHeroUnity", "(Hero) [Unifier of the Despairing] \"Crusaders of Iomedae and worshippers of Desna! Our enemy is the Abyss, not each other! Lay down your pride and unite for Kenabres!\"", "{n}A warm, celestial luminescence envelops the square. Hulrun grunts in begrudging respect, while Ramien smiles through tears.{/n}", "HeroProficiencies", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle<ContextActionExposeMimicAndReconcile>();
				});
			}
			BlueprintAnswersList blueprint11 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("1a17d8053a3be7f47a7908eb6706f2fe");
			if (blueprint11 != null)
			{
				AddUniversalAnswer(blueprint11, "IsekaiThaberdineHangoverPanacea", "(Isekai Protagonist) [Otherworlder's Hangover Panacea] \"Drink this effervescent blend of activated charcoal, willow bark, and electrolyte tonic, your majesty. It will flush the toxic spirits from your bloodstream and banish your headache in ten heartbeats.\"", "{n}Thaberdine gags, chugs the fizzing draught, and gasps as color rushes back into his sallow face. His rheumy eyes widen in absolute clarity.{/n} \"By Torag's heavy anvil! The pounding inside my skull... it vanished! My head feels lighter than a newly polished crown! What sorcery is this glorious nectar?!\"", 3);
				AddSubclassAnswer(blueprint11, "IsekaiThaberdineSlimeGastricFilter", "(Slime) [Gastric Fermentation Filter] \"Allow my slime catalyst to consume the toxic fusel oils in your blood. In five seconds, you will be sober enough to recite ancient history.\"", "{n}A cool blue droplet touches Thaberdine's tongue, absorbing all alcohol toxicity in seconds.{/n} \"Incredible! My head is as sharp as a newly honed waraxe!\"", "DevourerProficiencies", 4);
			}
			BlueprintAnswersList blueprint12 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("38adb0a43a5ac3043ad0fe59fdcba6b8");
			if (blueprint12 != null)
			{
				AddUniversalAnswer(blueprint12, "IsekaiJoranColdIronMetallurgy", "(Isekai Protagonist) [Advanced Metallurgy - Cold Iron Cryo-Quenching] \"Master Vhane, standard oil-quenching leaves microscopic fractures in cold iron. If you rapidly quench the heated blade in glacial mineral brine, the crystal lattice stabilizes, doubling its cutting edge against fiendish hide.\"", "{n}Joran Vhane pauses his hammer mid-swing, examining your sketches with burgeoning smithing fervor.{/n} \"Glacial brine quenching... to prevent lattice stress?! By the Great Smith's beard, you've solved the brittle-grain dilemma! Any weapon you bring to my forge will receive the finest craftsmanship I have to give!\"", 3);
			}
			BlueprintAnswersList blueprint13 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("5acd8001d9f7d2443bd57fb1291a03e4");
			if (blueprint13 != null)
			{
				AddUniversalAnswer(blueprint13, "IsekaiMinaghoShieldMazeDefiance", "(Isekai Protagonist) [Otherworlder's Defiance] \"You boast of slaughtering Kenabres, Minagho, but your reign ends right here. I am an Otherworlder who crossed the cosmic void; your abyssal masters hold no terror for me. Ready your scythe--you're facing someone whose script you cannot predict.\"", "{n}Minagho's cruel smile falters as an inexplicable, icy dread washes over her demonic flesh. She bares her fangs in a frantic snarl.{/n} \"Another world?! What meaningless drivel! I will carve your heart out and offer your soul to Deskari!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Defying the general of the Abyss with unflinching insolence! Magnifique!\"</i>";
					});
				});
			}
			BlueprintBookPage blueprint14 = BlueprintTools.GetBlueprint<BlueprintBookPage>("c297ea885ef96b8408b9bc236a192c63");
			if (blueprint14 != null)
			{
				AddBookPageAnswer(blueprint14, "IsekaiWardstoneResonantHarmony", "(Isekai Protagonist) [Resonant Harmonization: Cleanse and Preserve] \"I will not shatter the crusaders' ancient aegis, nor let the Abyss corrupt it! I channel the boundless mana of the Otherworlder directly into the Wardstone's beating heart! Purge the demonic rot from its veins and unleash a tide of cleansing fire across Kenabres while anchoring the celestial barrier for eternity!\"", "144c3a2bb925a754fa49fb9022b6b885", 6, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 1000;
						c.Sponsor = "The Grand Arbiter";
						c.FeatureToGrant = ResonantMythicSparkFeature;
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"EPIC TRIUMPH! The Wardstone is cleansed without loss of celestial barrier integrity! +1000 Cosmic Coins awarded!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint15 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("2e4be005071707640b4b5cac95c23d06");
			if (blueprint15 != null)
			{
				AddUniversalAnswer(blueprint15, "IsekaiAct1NenioBiologyExam", "(Isekai Protagonist) [Multiversal Scientific Taxonomy] \"Allow me to contribute to your encyclopedic inquiry, scholar. In my home dimension, cellular biology, genetic recombination, and extradimensional physics operate on rigorous empirical frameworks far beyond primitive cult dogma.\"", "{n}Nenio's eyes dilate in breathless academic ecstasy. Her quill scratches across parchment with blurring speed, sending flecks of ink flying.{/n} \"Empirical frameworks?! Extradimensional cellular genetics?! Fascinating! Utterly magnificent! A completely unique specimen with unprecedented biological taxonomy! You must accompany me immediately for further cranial measurements! Take this inscribed journal of my foundational hypotheses!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The Hermit";
						c.DirectItems = new BlueprintItem[1] { ItemNenioInscribedJournalGloves };
						c.BannerMessage = "<color=#9370DB><b>[The Hermit]</b></color>: <i>\"A meeting of unfathomable intellects. Knowledge transcends dimensional boundaries!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint15, "IsekaiAct1NenioScholarMeta", "(Hero) [Unwavering Axiom of Truth] \"Demonic falsehoods collapse beneath objective inquiry. Observe how the demonic humors decompose when subjected to celestial resonance.\"", "{n}Nenio nods frantically, scribbling notes without looking.{/n} \"Celestial resonance as an antiseptic agent! Extraordinary empirical verification!\"", "HeroProficiencies", 4);
			}
			BlueprintAnswersList blueprint16 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("28a0d7c556d73e84c89211458a9adbc5");
			if (blueprint16 != null)
			{
				AddUniversalAnswer(blueprint16, "IsekaiAct1NenioMeetCollaborator", "(Isekai Protagonist) [Academic Collaboration] \"We can debate extradimensional thermodynamics once we reach safety, Nenio. Travel with my company; you will have front-row observations of phenomena no Golarion text has ever documented.\"", "{n}Nenio snaps her notebook shut with decisive authority.{/n} \"A sound proposal. An itinerant field expedition with an anomalous specimen provides superior data density. Lead the way!\"", 3);
			}
			BlueprintAnswersList blueprint17 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("3a9fc611d9dcde349bdf614f6f66a568");
			if (blueprint17 != null)
			{
				AddUniversalAnswer(blueprint17, "IsekaiAct1WoljifCurioGolem", "(Isekai Protagonist) [Golem Logic Override] \"Standard runic automatons operate on binary heuristic directives. Observe: by inverting the polarity of the command matrix on its breastplate, the guardian's combat subroutine safely shuts down.\"", "{n}With a calm gesture and a hum of celestial energy, the ancient stone construct groans, its glowing sapphire eyes fading to a peaceful slumber.{/n} \"Runic polarity inversion?! Boss, you didn't just pick a lock--you hypnotized a walking mountain! Look at the shiny salvage it was sitting on!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 400;
						c.Coins = 400;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Outsmarting ancient magic with modern logic! Simply delightful!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint18 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a586818461e82c64c9a2f9249795b676");
			if (blueprint18 != null)
			{
				AddUniversalAnswer(blueprint18, "IsekaiAct1WoljifTraitorExpose", "(Isekai Protagonist) [Forensic Syndicate Audit] \"Sister Kerisme, save the veiled threats. I have already cross-referenced the ledger discrepancies with street informant reports. The traitor isn't Woljif; it's the compromised middlemen selling tips to the cultists. Woljif operates under my personal retainer now--clear his name and compensate him properly, or I'll audit every illicit operation under this tavern.\"", "{n}Sister Kerisme's sly smile freezes, her eyes widening as she sizes up your unwavering stance and razor-sharp deduction. She exhales softly, tossing a pouch of coins and a velvet roll of tools onto the table.{/n} \"An auditor with teeth... I like that. Very well, boy. Your boss cleared your slate. Take these masterwork picks as a token of Thiefling goodwill, and keep your horns out of trouble.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 800;
						c.Coins = 450;
						c.Sponsor = "The Laughing King";
						c.DirectItems = new BlueprintItem[1] { ItemThieflingMasterLockpicksBelt };
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"A forensic audit that leaves the underworld speechless! Outstanding work!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint19 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("1f1985a501be18a409aa22c31b268150");
			if (blueprint19 != null)
			{
				AddUniversalAnswer(blueprint19, "IsekaiAct1WoljifSisterKerismeAudit", "(Isekai Protagonist) [Syndicate Retainer Settlement] \"Our business here is concluded, Sister Kerisme. Woljif's talents are contracted to the crusade now. See that your Thieflings stay out of our vanguard's way, and we won't need to return.\"", "{n}Kerisme smirks respectfully and tips two fingers to her temple.{/n} \"Understood, Commander. The Thieflings know who holds the winning hand.\"", 3);
			}
			BlueprintAnswersList blueprint20 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("82da27c4efd945bab5ec9c3cb977dcb2");
			if (blueprint20 != null)
			{
				AddUniversalAnswer(blueprint20, "IsekaiAct1GwermSecretCryptDiscretion", "(Isekai Protagonist) [Corporate Confidentiality & Bloodline Discretion] \"Master Gwerm, your private crypt and family secrets remain strictly confidential under our executive retainer agreement. Whatever shadows lie in this cellar are between you and your conscience; my concern is securing your estate and honoring our pact.\"", "{n}Horgus Gwerm exhales a shaky sigh of immense relief, his trembling hands lowering his lantern. For the first time, his gaze holds genuine, profound respect.{/n} \"Confidentiality... without sanctimonious preaching or extortionate threats. You are truly unlike any crusader I have ever known. Take this ancestral rapier from my family vault, and the full bounty I promised. You have earned every coin.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 1000;
						c.Coins = 450;
						c.Sponsor = "The Dark Prince";
						c.DirectItems = new BlueprintItem[1] { ItemGwermAncestralRapier };
						c.BannerMessage = "<color=#FF4500><b>[The Dark Prince]</b></color>: <i>\"A gentleman's discretion rewarded in cold steel and gold. Exquisite contract adherence.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint21 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("2373bef62ea65d741a8f54ba95f9a8b2");
			if (blueprint21 != null)
			{
				AddUniversalAnswer(blueprint21, "IsekaiAct1ArankaSongOfTheStars", "(Isekai Protagonist) [Harmonic Resonance of the Spheres] \"Sing on, Aranka. The melodies of the cosmos echo through every realm and world. Hulrun's inquisitors will not touch you; my vanguard will guide your song safely past the checkpoints.\"", "{n}Aranka's fingers dance across her harp strings, a celestial glow illuminating her tear-streaked face.{/n} \"You hear it too... the song that dreams of freedom beneath the night sky! Take these traveler's draughts, noble protector. May the stars forever watch over your path!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Song of the Spheres";
						c.ItemGuids = new string[2] { "5f9fe6030695f48489b59887536748e8", "5f9fe6030695f48489b59887536748e8" };
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"The song of Elysium swells in harmony with your otherworldly soul!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint22 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("58bfbb095cc338a4cb79612cf5acf380");
			if (blueprint22 != null)
			{
				AddUniversalAnswer(blueprint22, "IsekaiAct1TrappedThieflingsLeverage", "(Isekai Protagonist) [Structural Physics: Safe Extraction] \"Hold still under there! Don't push against the keystone beam or the entire ceiling will collapse. Stand clear while we lever the fulcrum from this angle and clear the debris safely.\"", "{n}With precise leverage and a surge of otherworldly strength, the heavy masonry is shifted aside, freeing the trapped scoundrels without a single scratch.{/n} \"Whoa! You moved that boulder like it was made of balsa wood! Thanks, friend! Here--take our stash of shiny trinkets and potions before the guards show up!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 350;
						c.Coins = 350;
						c.Sponsor = "The Laughing King";
						c.ItemGuids = new string[2] { "5f9fe6030695f48489b59887536748e8", "f991f3051c3b9e64fabc87891077b613" };
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Clever leverage saves the day and fills your pockets!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint23 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("7789c0a35cd866b41896f74f6f6b3c82");
			if (blueprint23 != null)
			{
				AddUniversalAnswer(blueprint23, "IsekaiAct1StorytellerMultiversalChronicles", "(Isekai Protagonist) [Multiversal Chronicles] \"Greetings, Storyteller. The memories you read from relics are not confined to Golarion alone. In my soul linger echoes of other skies, distant stars, and forgotten civilizations that traversed the cosmic void. When Kenabres is secure, I would share these tales with you.\"", "{n}The blind elf tilts his head, his milky eyes widening as he reaches out a trembling hand toward your aura. A look of ancient, reverent awe settles upon his weathered face.{/n} \"Echoes... of other skies? Yes... I can feel the boundless resonance radiating from you, child of distant cosmoses. The threads of reality bend around your journey. Wear this ring, chronicler of the stars, and may your tales endure when all else turns to dust.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 500;
						c.Sponsor = "The Song of the Spheres";
						c.DirectItems = new BlueprintItem[1] { ItemRingOfTheAncientChronicler };
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"A bond forged between chroniclers of infinite worlds! +500 Cosmic Coins!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint24 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("1c39502dd11dbd046a7fc212702ad3fb");
			if (blueprint24 != null)
			{
				AddUniversalAnswer(blueprint24, "IsekaiAct1StauntonVhaneRedemptionSeed", "(Isekai Protagonist) [Insight into Guilt and Damnation] \"Drinking yourself to numbness won't wash seventy years of shame from your hands, Staunton. In the world I hail from, true redemption isn't granted by a forgiving crowd--it is carved out of the future with your own blood and steel. When we march on Drezen, don't throw your life away in despair; look to the horizon and choose who you want to be.\"", "{n}Staunton Vhane freezes, his tankard gripped so tightly the tin bends under his calloused fingers. He lifts bloodshot eyes to meet your gaze, a tremor running through his scarred dwarf frame.{/n} \"Carve it out of the future...? You talk like a madman who hasn't felt the weight of seventy winters of curses. ...Yet no one has looked at me without disgust in half a century. I won't forget what you said, Otherworlder.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Inheritor";
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"A seed of true redemption planted in hardened soil. The Inheritor watches with hope.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint25 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6705408dd6027424a96e018cdd04ead6");
			if (blueprint25 != null)
			{
				AddUniversalAnswer(blueprint25, "IsekaiAct1KaylessaCellularPurification", "(Isekai Protagonist) [Cellular Toxin Neutralization] \"Rest easy, Kaylessa. I recognize the necrotizing venom coursing through your veins. Drink this concentrated antitoxin infusion while I neutralize the dark humors in your bloodstream.\"", "{n}Kaylessa drinks the tonic with a ragged gasp. As a soothing warmth courses through her veins, the gray pallor of deadly drow venom fades from her cheeks.{/n} \"The burning... it's fading. You treated drow venom like a common fever. Who are you, stranger? Whatever you are, I owe you my breath. Take these antidotes--you may need them against those who hunt me.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Song of the Spheres";
						c.ItemGuids = new string[2] { "b46b711ee354a134496c080b720bbb16", "b46b711ee354a134496c080b720bbb16" };
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"Mercy shown to the hunted wanderer beneath the shadows.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint26 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("65e2ec6ff2d50884b818df2a35700e2c");
			if (blueprint26 != null)
			{
				AddUniversalAnswer(blueprint26, "IsekaiAct1DefendersHeartPreBattleSpeech", "(Isekai Protagonist) [Otherworlder Vanguard Rally] \"Crusaders of Kenabres! The demons believe we are trapped rats in this tavern, waiting to be butchered in the dark! But they do not know what kind of hunter is waiting inside! Steel your hearts, hold your barricades, and fight for every inch of stone! Tonight, we show them that Kenabres will NEVER kneel!\"", "{n}A deafening roar of defiance erupts from every throat in the tavern, weapons clashing against shields in a thunderous rhythm of indomitable courage. Irabeth raises her sword high, tears of fierce pride shining in her eyes.{/n} \"To the barricades! FOR KENABRES! FOR THE COMMANDER!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The World Sovereign";
						c.BuffToApply = TavernDefianceMoraleBuff;
						c.BuffDuration = new TimeSpan(2, 0, 0);
						c.BannerMessage = "<color=#FFD700><b>[The World Sovereign]</b></color>: <i>\"An inspiring rally that ignites the flame of victory in mortal hearts!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint27 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("aab7657377a11d847a5ad73bf633e8e8");
			if (blueprint27 != null)
			{
				AddUniversalAnswer(blueprint27, "IsekaiAct1DefendersHeartPostBattleToast", "(Isekai Protagonist) [The Victor's Toast] \"Raise your cups, defenders of Kenabres! Tonight we stood against the fury of the Abyss, and it was the demons who broke against our wall! To fallen comrades, to enduring hope, and to the dawn that will see our city reclaimed!\"", "{n}Mugs clink and cheer after cheer shakes the tavern rafters as soldiers drink deep in joyous relief. Irabeth claps you on the shoulder with a tired, glowing smile.{/n} \"You turned what should have been our slaughter into our finest hour. Take this victory purse collected from the grateful merchants--you've earned every single copper.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 500;
						c.Coins = 450;
						c.Sponsor = "The Lucky Drunk";
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"A victory toast that will be sung of for centuries! Cheers, hero!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint28 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("f6572cb37f42aa841808b2bbc293f3b0");
			if (blueprint28 != null)
			{
				AddUniversalAnswer(blueprint28, "IsekaiAct1GrayGarrisonSuccubusShatter", "(Isekai Protagonist) [Dimensional Resonance: Charm Shatter] \"Your cheap pheromones and seductive enchantments might ensnare mortal senses, demon, but against an Otherworlder anchored across realities, they are as brittle as glass. Crusaders of Mendev, AWAKEN!\"", "{n}A piercing chord of celestial starlight reverberates through the hall. The charmed crusaders gasp, violently shaking their heads as the succubus's psychic webs shatter into iridescent motes.{/n} \"My enchantment?! How dare you break my playthings, filthy worm! Cut them to pieces!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Demonic psychic enthrallment cleanly severed by dimensional disruption!\"</i>";
					});
				});
			}
			BlueprintBookPage blueprint29 = BlueprintTools.GetBlueprint<BlueprintBookPage>("ffd65d469961ff74c822be943a521775");
			if (blueprint29 != null)
			{
				AddBookPageAnswer(blueprint29, "IsekaiAct1MarketSquareAeonRiftBookEvent", "(Isekai Protagonist) [Cosmic Equilibrium: Planar Rift Stabilization] Step before the tearing planar anomaly. As an entity whose very existence spans disparate dimensions, reach out with focused will and weave cosmic threads of equilibrium across the rift, sealing the abyssal fracture with the absolute authority of the cosmos.", "e5069fb8a9fbc7d47a6ff74139f72a36" /* BookPage_0025, the present vision; Cue_0002 was the page narration itself and ended the event */, 4, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 500;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Planar tear stabilized through primordial multiversal equilibrium! +500 Cosmic Coins!\"</i>";
					});
				});
			}
			BlueprintBookPage blueprint30 = BlueprintTools.GetBlueprint<BlueprintBookPage>("b3f4b3f4de3494e4fb60d82d3f7c187e");
			if (blueprint30 != null)
			{
				AddBookPageAnswer(blueprint30, "IsekaiAct1MarketSquareDesnaHymnBookEvent", "(Isekai Protagonist) [Harmonic Resonance: Celestial Hymn of the Spheres] Touch the ancient altar of the Song of the Spheres, blending your otherworldly mana with the sacred melody of Elysium. Chime the bells in perfect octave resonance, suffusing the ruined square with celestial starlight.", "7c54ae222f65b6a4e8af0e318fe5dfed" /* BookPage_0009, where every vanilla answer leads; Cue_0002 was the page narration itself and ended the event */, 4, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"The celestial chimes echo across Kenabres, heralding the triumph of dreams!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint31 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("ab362e5b1af4b354ea59d583376cf661");
			if (blueprint31 != null)
			{
				AddUniversalAnswer(blueprint31, "IsekaiAct1DefendersHeartMessengerAlarm", "(Isekai Protagonist) [Emergency Defense Deployment] \"Calm yourself, runner! Panic will break our lines before the first demon reaches the barricades. Irabeth, seal the cellar hatches and position the archers on the upper gallery. Where I come from, ambushes are turned into kill zones by disciplined choke points. Kenabres stands ready!\"", "{n}The frantic messenger catches his breath, his panicked eyes steadying as Irabeth immediately bellows your orders to the garrison.{/n} \"Understood, Commander! Archery squads to the gallery! Reinforce the barricades! Defender's Heart shall not fall!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Inheritor";
						c.ItemGuids = new string[1] { "2983c97b71ba60443b1c749854ef53a1" };
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"Calm leadership under sudden alarm turns chaos into resolute defiance.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint32 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("07d811c04bc82a34ba54c253e3ca2455");
			if (blueprint32 != null)
			{
				AddUniversalAnswer(blueprint32, "IsekaiAct1ChiefSullAuxiliaryIntegration", "(Isekai Protagonist) [Tactical Auxiliary Integration: The Subterranean Pioneers] \"Stand tall, Chief Sull. In my world, elite subterranean shock troops with night-vision and poison resistance are invaluable command assets, not monsters. Your hunters form our stealth vanguard beneath Kenabres's shattered streets; any crusader who questions your bloodline answers directly to me.\"", "{n}Chief Sull stares in disbelief, his weathered eyes glistening as he straightens his curved back with pride. Behind him, the mongrel hunters let out a fierce, guttural cheer.{/n} \"Shock troops... vanguard?! You hear that, children of the stone?! The surface commander sees us as warriors, not vermin! We will scour the alleys and flay every demon that dares cross your path!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The World Sovereign";
						c.ItemGuids = new string[4] { "5f9fe6030695f48489b59887536748e8", "5f9fe6030695f48489b59887536748e8", "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
						c.BannerMessage = "<color=#FFD700><b>[The World Sovereign]</b></color>: <i>\"A true sovereign sees strength and loyalty where petty zealots see only fear.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint33 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("b837a26d7444dd442a45ee54da358cc9");
			if (blueprint33 != null)
			{
				AddUniversalAnswer(blueprint33, "IsekaiAct1KlaemTraumaDecompression", "(Isekai Protagonist) [Combat Trauma Decompression & Forward Re-Armament] \"Drink this warm restorative tonic, Klaem. Survivor's guilt is a heavy weight, but your fallen squad did not die so you could wither away in a tavern corner. Rest your hands, re-align your bowstring, and honor their memory when the demon horns sound on our barricades.\"", "{n}Klaem wipes tears and ash from his cheeks, his trembling hands steadying as he drinks the tonic and grips his bow with renewed purpose.{/n} \"You're right, Commander. My squad held the line so I could carry the warning back. I won't dishonor their sacrifice by rotting in the dark. Give me a quiver of cold iron and point me at the gate!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Inheritor";
						c.ItemGuids = new string[2] { "464ecede228b0f745a578f69a968226d", "2983c97b71ba60443b1c749854ef53a1" };
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"Honor for the fallen, strength for the living. The scout returns to the fight!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint34 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("8c7d0b0b6a1701d49835740d65d3d922");
			if (blueprint34 != null)
			{
				AddUniversalAnswer(blueprint34, "IsekaiAct1QueenGalfreyAfterActionReview", "(Isekai Protagonist) [Comprehensive After-Action Review & Fifth Crusade Strategic Doctrine] \"Welcome to reclaimed Kenabres, Your Majesty. Review these detailed battle logs: the Wardstone's corrupting frequency has been harmonized, the demonic supply lines are severed, and our vanguard has maintained full squad cohesion. Kenabres was merely the defensive phase; give me command of the Fifth Crusade, and we march on Drezen.\"", "{n}Queen Galfrey pores over your concise, impeccably organized battle logs with rising astonishment. The high prelates and generals exchange wide-eyed glances.{/n} \"Harmonized resonance... strategic supply interdiction... and not a single lost regiment in the garrison assault. In a hundred years of crusade warfare, I have never seen a military mind grasp planar battlefield logistics with such clarity. Commander, Mendev is in your debt. The sword of the Fifth Crusade is yours to wield.\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 1000;
						c.Coins = 600;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Exemplary strategic doctrine commands immediate imperial recognition! +600 Cosmic Coins!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint34, "IsekaiAct1QueenGalfreyGodEmperorHegemony", "(God Emperor) [Imperial Hegemony Briefing] \"Queen Galfrey, behold a city that did not merely survive, but triumphed under an otherworldly sovereign. The Fifth Crusade requires neither pious hesitation nor bureaucratic doubt. Grant me the mandate, and the Worldwound will bend.\"", "{n}Queen Galfrey's royal breath hitches as a majestic, undeniable aura of cosmic sovereignty radiates from your presence, leaving the gathered court in stunned silence.{/n} \"Such overwhelming authority... It feels as though an emperor of the heavens stands before me. Lead the Fifth Crusade, Sovereign. May the Worldwound shatter beneath your heel.\"", "GodEmperorProficiencies", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 1500;
						c.Coins = 700;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Imperial majesty acknowledged by mortal royalty. The crusade marches under your sovereign will!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint35 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6c6ca2d78b121504ca4bedf0a1c6f07f");
			if (blueprint35 == null)
			{
				return;
			}
			AddUniversalAnswer(blueprint35, "IsekaiAct1BartenderOtherworldDistillation", "(Isekai Protagonist) [High-Proof Fractional Distillation & Carbonation Technique] \"Master Elan, your ale is watered down by panic. Let me demonstrate a simple fractional freeze-distillation and effervescent herb carbonation. One barrel of this 'Otherworld IPA' will double soldier morale and clean their field wounds in a pinch.\"", "{n}Elan takes a cautious sip from the effervescent wooden tankard. His eyes widen in pure ecstasy as the crisp, aromatic hops and sparkling bubbles hit his tongue.{/n} \"By Cayden's blessed beard! It's crisp, it's fizzing like lightning, and the aftertaste warms you right down to your boots! You're a genius, friend! Take these cure draughts from behind the bar--any drink you ever order in my tavern is on the house!\"", 2, null, delegate(BlueprintAnswer bp)
			{
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 350;
					c.Sponsor = "The Lucky Drunk";
					c.ItemGuids = new string[5] { "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91" };
					c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"A master brewmaster from another reality! Cayden Cailean raises his mug in celestial salute!\"</i>";
				});
			});
		}

		private static void InjectAct2Encounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("eae6ac8c30bb31249aa266b2e0a87fb9");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiQueenGalfreyLogisticsDoctrine", "(Isekai Protagonist) [Modern Military Logistics & Strategic Echelon Doctrine] \"Your Majesty, study this forward echelon scouting blueprint and modular supply schedule. In my home world, campaigns of attrition are won not through heroic martyrdom, but through redundant supply lines, rotating perimeter watches, and concentrated force projection. Under my command, the Fifth Crusade will breach Drezen's outer walls in twelve days without starving a single soldier.\"", "{n}Queen Galfrey pauses in genuine astonishment, her royal gaze sweeping across your immaculate tactical charts with growing reverence. A breathless murmur ripples through the attending high officers and paladins.{/n} \"Redundant supply lines and echelon rotations... by the Inheritor, this is not mere theoretical strategy; it is the doctrine of a true sovereign commander. Mendev has bled for a century under obsolete siege manuals, yet you distill the art of war into pure mathematical triumph. Wear this signet, Commander; let every officer know your word is absolute royal law.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 600;
						c.Coins = 400;
						c.Sponsor = "The World Sovereign";
						c.DirectItems = new BlueprintItem[1] { ItemRoyalMendevianSignetRing };
						c.BannerMessage = "<color=#FFD700><b>[The World Sovereign]</b></color>: <i>\"A true commander does not rely on miracles when logistics can conquer kingdoms!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint, "IsekaiQueenGalfreyImperialEdict", "(God Emperor) [Imperial Edict of the Fifth Crusade] \"Queen Galfrey, step down from the war dais in peace. Mendev's century of despair ends today under the sovereign banner of humanity. Drezen shall not merely be recaptured; the Worldwound itself shall be crushed beneath my imperial standard.\"", "{n}An overwhelming aura of celestial majesty radiates from your form, compelling even the Queen's elite paladins to instinctively bow their heads in awe.{/n}", "GodEmperorProficiencies", 5);
				AddAlignedAnswer(blueprint, "IsekaiQueenGalfreyMastermindAudit", "(Mastermind) [Lawful] [Forensic Quartermaster Audit] \"Your Majesty, study these quartermaster margins. 18% of Mendevian grain is lost to mold due to non-ventilated carts. Adopt my compartmentalized storage schematics, and our march stamina doubles.\"", "{n}Queen Galfrey stares at the crisp numerical breakdown, then hands it directly to her herald.{/n} \"Unbelievable... You exposed a century-old supply defect with a single sheet of parchment. Implement this protocol across all battalions immediately.\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 4);
				AddAlignedAnswer(blueprint, "IsekaiQueenGalfreyMartialChallenge", "(Martial God) [Chaotic] [Vanguard Sparring Challenge] \"Ceremonial speeches will not pierce gargoyle hide, Your Majesty. Grant me ten minutes in the sparring ring against your royal champions; I will show your vanguard how to redirect demon momentum before we cross the march line.\"", "{n}The royal paladins bristled, but Galfrey holds up a hand, her lips curling into a rare, intrigued smile.{/n} \"A commander who leads with their own bare knuckles... Show them, Commander. Let our knights witness what true otherworldly martial mastery looks like.\"", AlignmentShiftDirection.Chaotic, "MartialGodProficiencies", 4);
				AddAlignedAnswer(blueprint, "IsekaiQueenGalfreyHeroVow", "(Hero) [Good] [Vow of the Undying Vanguard] \"Your Majesty, look at these young recruits from Kenabres. They answered the call because they believed in hope. I give you my sacred oath: under my command, no rear guard will be abandoned as expendable. We all return together.\"", "{n}Tears glisten in Galfrey's eyes as cheers erupt from the surrounding soldiers and squires.{/n} \"A vow of true chivalry... May your light guide their path, Commander. Mendev marches with you.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 4);
				AddAlignedAnswer(blueprint, "IsekaiQueenGalfreyOverlordTithe", "(Overlord) [Evil] [Unforgiving March Mandate] \"Queen Galfrey, spare me moral platitudes. Those who collapse on the march are discarded. Drezen requires ruthless efficiency, not sentimental baggage. Let the weak fall where they stand.\"", "{n}Galfrey's expression hardens into icy royal resolve, yet she does not challenge your terrifying authority.{/n} \"A chilling decree, Commander. Pray your cold iron will does not shatter before we reach the fortress.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 4);
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("34822d5daf196244db224703cef5d1d4");
			if (blueprint2 != null)
			{
				AddUniversalAnswer(blueprint2, "IsekaiYakerRapidMobilization", "(Isekai Protagonist) [Rapid Reactionary Vanguard Deployment] \"Stand at ease, Armiger. Panic will not save your paralictor. We are already mobilizing a light cavalry strike force equipped with cold iron ballistas and incendiary charges. Lead the way to Reliable Redoubt; we will break the gargoyle siege before your lines collapse.\"", "{n}Yaker's panicked breathing hitches in shock. He stiffens into an ironclad Hellknight salute, his eyes wide with profound disbelief and newfound hope.{/n} \"Rapid mobile cavalry and incendiary ballistas?! By the Measure and the Chain, I expected bureaucratic delays from crusaders, not an immediate mechanized counter-offensive! Lead on, Commander! The Godclaw will witness your fury!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 400;
						c.Coins = 400;
						c.Sponsor = "The Iron Warden";
						c.BannerMessage = "<color=#FFD700><b>[The Iron Warden]</b></color>: <i>\"Iron discipline meets lightning speed! The vanguard marches!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint2, "IsekaiYakerFlashStepVanguard", "(Martial God) [Flash-Step Reconnaissance] \"No need to waste horses on rocky terrain. Point me toward the ridge; my stride outpaces the wind.\"", "{n}A blast of focused ki whips around your boots as Yaker stares slack-jawed at your unearthly velocity.{/n}", "MasterAtArmsProficiencies", 4);
				AddAlignedAnswer(blueprint2, "IsekaiYakerMastermindChokepoint", "(Mastermind) [Lawful] [Topographical Defile Strategy] \"Armiger, catch your breath. The redoubt is situated between two basalt ridges. If we advance through the eastern ravine, the gargoyles lose altitude clearance and become funnel targets for our arbalests.\"", "{n}Yaker quickly sketches your route on his gauntlet, nodding in disbelief.{/n} \"The Measure dictates terrain exploitation, yet you solved our encirclement in five seconds! Proceeding immediately!\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 4);
				AddAlignedAnswer(blueprint2, "IsekaiYakerHeroCharge", "(Hero) [Good] [Unwavering Rescue Charge] \"Hold on, Armiger! Tell your brothers in arms to form a defensive shell! We are charging right into the gorge to break the siege!\"", "{n}Yaker's stoic armor trembles with renewed hope.{/n} \"Understood, Commander! We will hold until your standard arrives!\"", AlignmentShiftDirection.Good, "HeroProficiencies", 4);
				AddAlignedAnswer(blueprint2, "IsekaiYakerOverlordContract", "(Overlord) [Evil] [Subjugation of the Besieged] \"Your Order prides itself on merciless order, yet here you crawl. Tell Paralictor Derenge that if I break this siege, his Hellknights owe their lives and swords to my private court.\"", "{n}Yaker gulps, bowing low despite his trembling knees.{/n} \"The Paralictor values survival and order above all else. Your terms will be delivered, Sovereign.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 4);
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("ab4c009b7ff77da4fad0abe34ad79c08");
			if (blueprint3 != null)
			{
				AddUniversalAnswer(blueprint3, "IsekaiRegillFieldTriageDoctrine", "(Isekai Protagonist) [Tactical Efficiency Counter-Proposal: Otherworlder Field Triage] \"Sheathe your hook-hammer, Paralictor. Your execution arithmetic is dangerously primitive. In my world, combat triage prioritizes rapid stabilized clotting, tourniquets, and concentrated antiseptic poultices. These soldiers are seasoned veterans whose tactical value vastly exceeds the rations they consume. Apply my medical supplies right now; they will be back in formation within forty minutes.\"", "{n}Regill Derenge lowers his gnome hook-hammer by a fraction of an inch, his crimson eyes narrowing as he methodically scrutinizes your modern medical doctrine with razor-sharp analytical precision. A tense, dead silence grips the blood-soaked gorge.{/n} \"Antiseptic clotting agents and stabilized tourniquets... to preserve hardened combat personnel rather than squandering resources on replenishment. Highly unorthodox... yet mathematically impeccable. Very well, Commander. Your logic supersedes standard field protocol. Administer the treatment; if they hold the line, their lives belong to your vanguard.\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 500;
						c.Coins = 450;
						c.Sponsor = "The Iron Warden";
						c.DirectItems = new BlueprintItem[1] { ItemHellknightOfficerCloak };
						c.BannerMessage = "<color=#FFD700><b>[The Iron Warden]</b></color>: <i>\"Cold logic yields to superior otherworldly doctrine! Zero executions!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint3, "IsekaiRegillOverlordReclamation", "(Overlord) [Sovereign Battlefield Reclamation] \"A foolish waste of mortal thralls, gnome. If their flesh breaks, my sovereign necromancy re-knits their marrow. Under my reign, none are discarded.\"", "{n}Dark sovereign filaments pulse from your fingertips, sealing the wounded soldiers' flesh with terrifying efficiency. Regill stares with cold fascination.{/n}", "OverlordProficiencies", 5);
				AddAlignedAnswer(blueprint3, "IsekaiRegillMartialStanceCritique", "(Martial God) [Chaotic] [Critique of Godclaw Rigidity] \"Paralictor, your hooked hammer routines are textbook, but your footwork is far too rigid on uneven scree. If your soldiers had learned evasive flow instead of standing in stagnant lines, half these wounds wouldn't exist.\"", "{n}Regill's jaw tightens by a millimeter, observing your relaxed, perfectly balanced combat stance.{/n} \"Unorthodox footwork... yet undeniably effective against aerial dive angles. Your criticism is noted, Commander. Proceed with your tactics.\"", AlignmentShiftDirection.Chaotic, "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint3, "IsekaiRegillHeroMercyRebuke", "(Hero) [Good] [Righteous Rebuke of Field Executions] \"Lower that hammer, Regill! Executing wounded soldiers who bled holding the line for you is cowardice masked as discipline! Step aside; I will heal them myself!\"", "{n}Regill steps back, his face completely expressionless, yet his eyes scrutinize your unwavering conviction.{/n} \"Sentimental defiance. If their wounds heal, they fight. If they fail, the responsibility rests upon your conscience, Commander.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
				AddAlignedAnswer(blueprint3, "IsekaiRegillGodEmperorImperialPardon", "(God Emperor) [Lawful] [Imperial Sovereign Requisition] \"Stay your execution hammer, Paralictor. By sovereign decree of the Radiant Throne, these soldiers are pardoned and transferred to the imperial guard. Their lives belong to my empire, not your executioner block.\"", "{n}Regill slowly sheathes his weapon, bowing with rigid formal protocol.{/n} \"A sovereign claim backed by absolute authority supersedes standard military summary execution. They are yours, Commander.\"", AlignmentShiftDirection.Lawful, "GodEmperorProficiencies", 5);
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("280221f20f506b9409275949d588eb1e");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiGargoyleNightPerimeterStarlight", "(Isekai Protagonist) [Night Perimeter Starlight Flare & Decoy Ambush] \"Sound the defensive drums and ignite the high-altitude starlight pyres! Gargoyles hunt by thermal contrast against the cold night sky. Blank the camp with illuminated smoke, form phalanx shields around the hospital tents, and target the wings with cold iron nets! Drive these flying stone vultures into the mud!\"", "{n}Anevia's tense grimace transforms into a fierce grin as your tactical directives instantly cut through the midnight panic. Starlight flares burst high above the tents, turning the pitch-black sky blindingly bright and blinding the diving gargoyles as crusader arbalests shoot them from the air.{/n} \"They're blinded by the flares! Form ranks, third company! Box them in! By the gods, Commander, you turned an ambush into a turkey shoot!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 400;
						c.Coins = 450;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"Spectral starlight illuminates the dark! The night ambush is shattered!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint4, "IsekaiGargoyleMidnightShadowSeverance", "(Martial God) [Heaven-Splitting Sky Strike] \"Hunting from the clouds? You picked the wrong sky, gargoyles. Come down and face true martial mastery!\"", "{n}A shockwave of concentrated kinetic ki detonates upward into the mist, shattering gargoyle wings and sending stones plummeting into the dirt.{/n}", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint4, "IsekaiGargoyleMastermindAcousticTrap", "(Mastermind) [Lawful] [Acoustic Triangulation Crossfire] \"Archers, hold fire until the count of three! Gargoyles rely on wind sheer between pavilion ropes to target tent openings. Loose at grid coordinates Alpha-Four on my whistle!\"", "{n}A concentrated volley of cold iron bolts rips through the mist right as three gargoyles dive, pinning them to the dirt before they can snatch a single soldier.{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 5);
				AddAlignedAnswer(blueprint4, "IsekaiGargoyleHeroHospitalShield", "(Hero) [Good] [Human Fortress of the Infirmary] \"Form a circle around the wounded! As long as my pulse beats, no flying beast will drag a helpless crusader into the night! Fight with me!\"", "{n}A radiant beacon of protective courage blazes from your shield, inspiring the panicking recruits to stand their ground with drawn steel!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
				AddAlignedAnswer(blueprint4, "IsekaiGargoyleOverlordNecroticGrasp", "(Overlord) [Evil] [Necrotic Gravity Snare] \"Stone-winged vermin dare interrupt an Overlord's sleep? Let the earth drag your hollow bones into the mud! [Black necrotic chains erupt from the soil, snapping gargoyle wings mid-dive].\"", "{n}Terrified screeching echoes through the mist as the gargoyles are violently crushed against the gravel, their stone bodies pulverizing under dark pressure.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("8e28d75caf0ddd14995c923ff5fc4760");
			if (blueprint5 != null)
			{
				AddUniversalAnswer(blueprint5, "IsekaiNurahPheromoneDecomposition", "(Isekai Protagonist) [Appraisal: Alchemical Pheromone Decomposition] \"Drop the charming scholar facade, Nurah. That scent lingering on your cloak isn't lavender oil; it's a hyper-concentrated extract of Vescavor queen pheromones mixed with fire-resistant alchemical accelerant. Hand over the vial, detail your communication channels with Staunton Vhane, and sit down before I freeze your hands to your sleeves.\"", "{n}Nurah's bubbly, scholarly smile instantly withers into a mask of pure, frantic malice and bitter despair. Her small hands shake violently as she realizes every single component of her clandestine sabotage has been cataloged and exposed.{/n} \"The pheromone scent...?! You can distinguish alchemical distillation bases with a single sniff?! Damn you! Damn you and your meddling, all-seeing eyes! You crusaders were supposed to burn in screaming agony!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.DirectItems = new BlueprintItem[1] { ItemVescavorPheromoneFlask };
						c.Coins = 450;
						c.Sponsor = "The Hermit";
						c.BannerMessage = "<color=#00FFFF><b>[The Hermit]</b></color>: <i>\"The hidden serpent is unmasked before its venom could poison the camp!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint5, "IsekaiNurahSlimePheromoneDrain", "(Slime) [Chemical Scent Neutralization] \"Your pheromone vial is already empty, Nurah. My slime pseudopod siphoned and metabolized every volatile hydrocarbon while you were speaking.\"", "{n}Nurah frantically reaches into her satchel only to pull out a dry, glass shell, thoroughly dissolved clean. She stumbles back in terrified silence.{/n}", "DevourerProficiencies", 5);
				AddAlignedAnswer(blueprint5, "IsekaiNurahMastermindHandwritingAudit", "(Mastermind) [Lawful] [Forensic Handwriting Discrepancy] \"Nurah, the handwriting on the camp's false scouting reports has the exact 12-degree leftward slant as your personal historical monographs. You made three grammatical errors common to Chelish slave ciphers. Your game is over.\"", "{n}Nurah freezes, her ink-stained fingers trembling as her eyes dart between your cold gaze and her confiscated notes.{/n} \"Chelish slave ciphers... You dissect my entire life from a margin note?! You monster!\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 5);
				AddAlignedAnswer(blueprint5, "IsekaiNurahMartialPulseLock", "(Martial God) [Chaotic] [Pressure Point Meridian Lock] \"Your pulse spiked to 150 the moment I mentioned the supply wagons. Don't reach for that dagger; I've already locked your shoulder meridians.\"", "{n}Nurah gasps as her right arm goes completely numb, the concealed stiletto clattering harmlessly to the carpet.{/n} \"What did you do to my arm?! You witch-doctor!\"", AlignmentShiftDirection.Chaotic, "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint5, "IsekaiNurahGodEmperorImperialCondemnation", "(God Emperor) [Lawful] [Imperial Condemnation of Treason] \"Nurah, by sovereign imperial mandate, your petty conspiracies are laid bare before celestial light. Kneel and confess your treason, or face summary imperial judgment.\"", "{n}A crushing wave of regal authority forces Nurah to her knees, breaking through her spiteful sneer until tears of humiliation stream down her face.{/n} \"Curse your light... curse your crown!\"", AlignmentShiftDirection.Lawful, "GodEmperorProficiencies", 5);
			}
			BlueprintBookPage blueprint6 = BlueprintTools.GetBlueprint<BlueprintBookPage>("7be206b993f3dea49a89a88ce5e6ce6f");
			if (blueprint6 != null)
			{
				AddBookPageAnswer(blueprint6, "IsekaiLepersSmileSpatialStasis", "(Isekai Protagonist) [Spatial Stasis Extraction: The Zero-Casualty Protocol] \"Hold the army on the high ridge and form a thermal barricade of cold iron and Greek fire! I will plunge into the ravine alone, enveloped in an Otherworlder spatial stasis barrier. While the swarm fruitlessly breaks against my impenetrable aegis, my dimensional storage will retrieve every submerged supply wagon, relic, and trapped scout without sacrificing a single crusader's life!\"", "802031c7fc1bceb49b4ade6f06aa138c" /* BookPage_0011, the field council the vanilla plan answer leads to; Cue_0002 was the page narration itself and ended the event */, 5, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 4000;
						c.Coins = 500;
						c.Sponsor = "The Lucky Drunk";
						c.ItemGuids = new string[2] { "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"Retrieving the sunken treasure without losing a single drop of soldier blood! Legendary!\"</i>";
					});
				});
				AddBookPageAnswer(blueprint6, "IsekaiLepersSmileMartialSoloSweep", "(Martial God) [Chaotic] [Ki-Barrier Solo Cultivation] \"A boiling canyon of flesh-eating insects? The ultimate trial for my kinetic ki shield! Hold the army back; I will carve a clean path to the queen with bare hands!\"", "802031c7fc1bceb49b4ade6f06aa138c" /* BookPage_0011, the field council the vanilla plan answer leads to; Cue_0002 was the page narration itself and ended the event */, 5);
				AddBookPageAnswer(blueprint6, "IsekaiLepersSmileMastermindAcousticRupture", "(Mastermind) [Lawful] [Resonant Ultrasonic Decoy] \"Tune our war horns to high pitch G-sharp and blast the canyon walls. The ultrasonic resonance will scramble the Vescavors' antennal sensors and force them to turn upon each other!\"", "802031c7fc1bceb49b4ade6f06aa138c" /* BookPage_0011, the field council the vanilla plan answer leads to; Cue_0002 was the page narration itself and ended the event */, 5);
				AddBookPageAnswer(blueprint6, "IsekaiLepersSmileHeroRearguardAegis", "(Hero) [Good] [Vanguard Aegis for the Baggage Train] \"Form ranks! I will lead the front row with my shield raised, intercepting every swarm cloud so the baggage handlers can withdraw safely!\"", "802031c7fc1bceb49b4ade6f06aa138c" /* BookPage_0011, the field council the vanilla plan answer leads to; Cue_0002 was the page narration itself and ended the event */, 5);
				AddBookPageAnswer(blueprint6, "IsekaiLepersSmileOverlordExpendableMarch", "(Overlord) [Evil] [Calculated Vanguard Sacrifice] \"Order the forward ranks to march straight through the canyon. The insects will exhaust their mandibles on the vanguard while our primary forces advance unhindered. Move!\"", "802031c7fc1bceb49b4ade6f06aa138c" /* BookPage_0011, the field council the vanilla plan answer leads to; Cue_0002 was the page narration itself and ended the event */, 5);
			}
			BlueprintAnswersList blueprint7 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6856a7d6e123a0c46b666e8052008405");
			if (blueprint7 != null)
			{
				AddUniversalAnswer(blueprint7, "IsekaiLostChapelFlashRescue", "(Isekai Protagonist) [Sovereign Cleansing Aura: The Unbroken Vanguard] \"Abyssal carrion and desecrated dead, hear my decree! You have no jurisdiction over living souls! I unleash the untainted mana of an Otherworlder across this desecrated shrine--let sacred solar fire shatter your necrotic bonds, sear the flesh of the devourers, and restore these fallen crusaders to unbroken valor!\"", "{n}A tidal shockwave of blinding solar radiance erupts from your outstretched hand, incinerating the necrotic binding chains in a shower of golden sparks. The snarling ghouls shriek as holy light dissolves their rotting flesh, while the rescued crusaders weep in ecstatic relief, leaping to their feet with drawn weapons.{/n} \"The chains... they melted like morning frost! The Commander has brought Heaven's own wrath down upon this desecrated mountain! For the Commander! No retreat!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The Inheritor";
						c.BuffToApply = CrusaderLiberatorBuff;
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"Righteous fury breaks every profane chain! The lost are reclaimed!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint7, "IsekaiLostChapelBeaconOfTheLost", "(Hero) [Beacon of the Lost] \"Hold fast, soldiers! As long as I draw breath, no evil shall claim you! Break your chains and fight!\"", "{n}Radiant angelic feathers swirl through the hall, restoring the captives' health and courage instantly.{/n}", "HeroProficiencies", 5);
				AddAlignedAnswer(blueprint7, "IsekaiLostChapelMartialGhoulSeverance", "(Martial God) [Chaotic] [Severing Ghoul Claws at Mach Speed] \"Rotting nails against transcendent martial technique? You fiends are too slow to graze my shadow! [A storm of kinetic palms shatters the ghouls into bone fragments before they can strike].\"", "{n}The captive crusaders cheer in astonishment as the ghouls disintegrate into dry powder in less than two seconds!{/n}", AlignmentShiftDirection.Chaotic, "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint7, "IsekaiLostChapelGodEmperorSolarPurge", "(God Emperor) [Lawful] [Solar Consecration of the Desecrated Mount] \"Profane undead, return to the dust! This ancient chapel is reclaimed in the name of the Solar Throne! Blinding dawnfire, consume the dark!\"", "{n}Blinding solar radiance incinerates the ghoul tormentors, leaving the stone altar pure and warm under celestial glory.{/n}", AlignmentShiftDirection.Lawful, "GodEmperorProficiencies", 5);
				AddAlignedAnswer(blueprint7, "IsekaiLostChapelMastermindAltarDecryption", "(Mastermind) [Lawful] [Deconstructing Necrotic Containment Glyphs] \"Notice the glyph symmetry on the sacrificial altars. Disconnect the corner keystone first; the necrotic feedback loop collapses and frees the prisoners without triggering the corpse traps.\"", "{n}Anevia and the scouts marvel as the glowing necrotic runes fizzle out harmlessly like damp wicks.{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 5);
			}
			BlueprintAnswersList blueprint8 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("7fd900cf92974d69afe518511368991d");
			if (blueprint8 != null)
			{
				AddUniversalAnswer(blueprint8, "IsekaiWilcerSupplyChain", "(Isekai Protagonist) [Standardized Quartermaster Protocol: High-Efficiency Supply Lines] \"Master Garms, review this standardized requisition matrix. If we package field rations into vacuum-sealed caloric bricks and standardize ammunition calibers across all auxiliary squads, wagon weight drops by 30% while march endurance increases twofold. Here is five hundred gold to fund the supply chain overhaul.\"", "{n}Wilcer Garms gasps, frantically adjusting his glasses as his ink-stained fingers trace your diagrams with boundless excitement.{/n} \"Caloric brick rations... standardized fletching calibers?! Merciful gods, Commander, this solves our entire transport bottleneck across the rocky wastes! With this overhaul, our vanguards will march twice as far with full bellies! You truly are a gift from beyond the stars!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 500;
						c.Coins = 350;
						c.Sponsor = "The World Sovereign";
						c.BannerMessage = "<color=#FFD700><b>[The World Sovereign]</b></color>: <i>\"Organized logistics feed the fires of victory!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint9 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("c2691373bfd094544a2d96578aae4f7c");
			if (blueprint9 != null)
			{
				AddUniversalAnswer(blueprint9, "IsekaiConundrumMatrixSolver", "(Isekai Protagonist) [Meta-Logic: Runic Slab Topological Inversion] \"Spare me the cryptic pantomime, spirit. This runic grid is a straightforward 4x4 matrix permutation based on dual-axis diagonal parity. The dominoes align along rotational symmetry, unlocking the inner chamber mechanisms instantly.\"", "{n}The Masked Ghost's ethereal posture stiffens in utter, stunned incredulity. The spectral glow behind the golden mask wavers violently before the spirit bows with solemn reverence as ancient stone gears deep within the floor rumble to life.{/n} \"Diagonal... parity?! You solve the primordial puzzle of forgotten ages as though reading nursery rhymes... Stranger from afar, you perceive patterns that eluded Sarkorian archmages for millennia. The vault yields to your boundless intellect.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 600;
						c.Coins = 450;
						c.Sponsor = "The Hermit";
						c.BannerMessage = "<color=#00FFFF><b>[The Hermit]</b></color>: <i>\"Ancient riddles unravel before otherworldly logic!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint10 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("8b3bfe8703cffc44cb09cea58d8acc2c");
			if (blueprint10 != null)
			{
				AddUniversalAnswer(blueprint10, "IsekaiChorussinaPortalSeverance", "(Isekai Protagonist) [Dimensional Phalanx Breach] \"Crusaders, lock shields and brace for spatial redirection! I fold the kinetic impact of their boiling pitch and ballistas into the astral void! Charge the gatehouse now!\"", "{n}A glittering planar mirror absorbs the enemy catapult fire harmlessly into empty space, leaving the gateway wide open for your shock troops.{/n}", 5);
			}
			BlueprintAnswersList blueprint11 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("0c390d22c44f76c4aaa053ee987836d1");
			if (blueprint11 != null)
			{
				AddUniversalAnswer(blueprint11, "IsekaiDrezenWarCouncilThreePronged", "(Isekai Protagonist) [Three-Pronged Synchronized Assault: The Masterstroke] \"We will not choose between half-measures. We execute a synchronized three-pronged assault! Regill leads heavy infantry to smash the temple distractors; Wenduag and Lann infiltrate the prison dungeons from below to liberate the captive smiths; while our main vanguard breaches the southern gates with heavy siege engines! We crush Drezen's garrison on every front at once!\"", "{n}The war tent erupts in stunned, electrifying energy. Regill nods with rare, unvarnished approval, Anevia slams her fist on the table in enthusiastic agreement, and Irabeth's eyes flare with fiery determination.{/n} \"Synchronized multi-front pressure! The demons won't know which breach to reinforce until our steel is at their throats! By the gods, Commander, we aren't just taking Drezen--we are sweeping it clean!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 800;
						c.Coins = 500;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Flawless multi-front coordination! Drezen's walls tremble!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint12 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("c0a6d32e56866274bb86b0adcfab1dbe");
			if (blueprint12 != null)
			{
				AddUniversalAnswer(blueprint12, "IsekaiStauntonMinaghoRedemption", "(Isekai Protagonist) [Redemption of the Fallen Warden & Cosmic Defiance] \"Minagho, your parasitic dominion over Drezen ends today! And Staunton--seventy years of agonizing shame and self-hatred have blinded you to the truth! You were betrayed by a succubus, but your crime was naivety, not malice! The demons used you as a pawn and discarded you into the dirt! Turn your hammer against the monster that ruined your life and reclaim your honor before the Inheritor!\"", "{n}Staunton Vhane's hands tremble violently on the grip of his heavy hammer, his haggard, grief-ravaged face contorting in an agonizing tempest of realization and burning fury. Minagho shrieks in panicked fury as the dwarf's bloodshot eyes turn toward her with terrifying vengeance.{/n} \"Seventy years... she whispered lies in the dark while mocking my disgrace! No more! By Torag's hammer, no more! Demon witch, you will pay for every tear Sarkoris has wept!\"", 6, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 1000;
						c.Coins = 600;
						c.Sponsor = "The Inheritor";
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"The broken oath is reforged in righteous fury! Minagho shall fall!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint12, "IsekaiStauntonMinaghoVillainDissection", "(Villain) [Calculated Humiliation] \"Look at the two of you--a pathetic traitor and a disgraced lilitu clinging to stolen stone. Neither of you is worthy to rule this fortress. Die together in the dust.\"", "{n}Minagho's snarl freezes as cold terror grips her black heart, while Staunton roars in frantic desperation.{/n}", "VillainProficiencies", 6);
				AddAlignedAnswer(blueprint12, "IsekaiStauntonMartialDuelOfShame", "(Martial God) [Chaotic] [Duel of Seventy Years' Regret] \"Staunton! Your footwork is weighed down by seventy years of self-loathing! Lift your hammer and face me like a warrior, or die as Minagho's pet in the dirt!\"", "{n}Staunton roars, tears burning tracks through the grime on his face as he heaves his hammer with desperate, suicidal fury!{/n}", AlignmentShiftDirection.Chaotic, "MartialGodProficiencies", 6);
				AddAlignedAnswer(blueprint12, "IsekaiStauntonHeroRedemptionPlea", "(Hero) [Good] [Plea for a Fallen Soul] \"Staunton, don't let your story end as an abyssal footnote! Die defending Drezen, not serving the fiends who ruined your kin! You can still choose honor!\"", "{n}Staunton's knees tremble, his grip wavering as he stares at the banner of the Inheritor above the hall.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 6);
				AddAlignedAnswer(blueprint12, "IsekaiStauntonGodEmperorTreasonExecution", "(God Emperor) [Lawful] [Imperial Sentence of Treason] \"Staunton Vhane, your treason cost Mendev seventy years of blood. By celestial law, the sentence is death. Minagho, you shall be purged alongside your puppet.\"", "{n}Incandescent holy light flares along your blade, casting long shadows across the trembling traitors.{/n}", AlignmentShiftDirection.Lawful, "GodEmperorProficiencies", 6);
			}
			BlueprintAnswersList blueprint13 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("44704bddb6223b84989dd26bcf20b601");
			if (blueprint13 != null)
			{
				AddUniversalAnswer(blueprint13, "IsekaiDrezenVictoryProclamation", "(Isekai Protagonist) [The Sovereign's Triumphant Proclamation: The Unconquered Citadel] \"Welcome to liberated Drezen, Your Majesty. The Sword of Valor flies above the citadel once more, cleansed of abyssal corruption and burning with otherworldly starlight. What Mendev lost seventy years ago has been reclaimed in blood and glory. Drezen is now our fortress, and from here, we will push the demonic hordes all the way back to the Abyss!\"", "{n}Queen Galfrey looks up at the radiant Sword of Valor fluttering proudly against the northern sky, tears of overwhelming emotion welling in her eyes. Around her, thousands of crusaders raise their weapons, cheering your name until the very mountain cliffs shake.{/n} \"Drezen is ours... the banner is raised! In seventy years, no mortal army has achieved such an impossible triumph! You have not merely fulfilled your duty, Commander--you have rekindled the dying heart of the entire crusader cause!\"", 6, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Gold = 1500;
						c.Coins = 1000;
						c.Sponsor = "The Grand Arbiter";
						c.FeatureToGrant = SwordOfValorResonantFeature;
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"MAGNIFICENT VICTORY! Drezen is reclaimed and the Sword of Valor resonates across the multiverse! +1000 Cosmic Coins!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint14 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("366eb018f3667a6479526a9416c3b412");
			if (blueprint14 != null)
			{
				AddUniversalAnswer(blueprint14, "IsekaiAct2SosielPortraitPose", "(Isekai Protagonist) [Iconic Cover Pose & Optical Lenses] \"If you're going to sketch my likeness, Sosiel, wait until I strike the classic light novel cover pose: one hand on my hilt, cloak billowing in an unfelt breeze, looking thoughtfully toward the horizon. In my world, portraits like this sell millions of volumes!\"", "{n}Sosiel blinks in stunned silence before breaking into soft, melodic laughter, his charcoal hovering over the parchment with genuine delight.{/n} \"A 'light novel cover pose'? You are truly an extraordinary commander! Most warriors demand to look stern and terrifying, but you treat destiny with infectious joy. Hold that stance--the morning light catching your cloak is truly magnificent! Take this lucky silver talisman as a token of my gratitude for being such an inspired subject!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Song of the Spheres";
						c.DirectItems = new BlueprintItem[1] { ItemTalismanOfTheCosmicTourist };
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"The heroic pose captured for all eternity! Starlight guides your legend!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint14, "IsekaiAct2SosielMartialStillness", "(Martial God) [Cultivation Stillness] \"Capture the flow of internal ki. Notice how the breath roots into the earth, circulating without disturbance.\"", "{n}Sosiel nods in quiet awe, sketching the immaculate balance of your stance.{/n} \"The stillness in your center... It's like watching a mountain against the wind.\"", "MartialGodProficiencies", 4);
				AddSubclassAnswer(blueprint14, "IsekaiAct2SosielOverlordMajesty", "(Overlord) [Imperial Portraiture] \"Paint a portrait suited for a sovereign throne room, priest. The canvas should compel all who gaze upon it to kneel.\"", "{n}Sosiel's fingers tremble slightly against his easel.{/n} \"The sheer weight of your presence is unmistakable, Commander. I will do my finest.\"", "OverlordProficiencies", 4);
			}
			BlueprintAnswersList blueprint15 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("3fafdb348338f5e4ebac24a8970494e7");
			if (blueprint15 != null)
			{
				AddUniversalAnswer(blueprint15, "IsekaiAct2SeelahRationBanter", "(Isekai Protagonist) [Convenience Store Delicacies vs Iron Hardtack] \"Seelah, looking at these rock-hard crusader biscuits makes me miss convenience store instant noodles and fizzy cola so much I could weep. If I had five minutes with an electric kettle, I would revolutionize camp morale overnight!\"", "{n}Seelah laughs until her breastplate rattles, slapping her thigh in shared culinary agony.{/n} \"'Fizzy cola' and 'instant noodles'?! By the Inheritor, whatever miraculous delicacies your realm invented, they sound ten times better than chewing on dried mutton and hardtack! Let's strike a deal: the moment we liberate Drezen, you're the executive chef of our celebration banquet!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 250;
						c.Sponsor = "The Lucky Drunk";
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"Hardtack is an insult to mortal palates! Raise a cup to better feasts ahead!\"</i>";
					});
				});
				AddAlignedAnswer(blueprint15, "IsekaiAct2SeelahHeroCheer", "(Hero) [Good] [High-Spirited Vanguard] \"We march into the teeth of the Worldwound together, Seelah. Keep that radiant laugh loud and proud; it keeps the darkness at bay.\"", "{n}Seelah gives a jaunty salute with a glowing grin.{/n} \"Right beside you, Commander! Let's show these fiends what true crusaders can do!\"", AlignmentShiftDirection.Good, "HeroProficiencies", 3);
			}
			BlueprintAnswersList blueprint16 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e9355a3b814dbc24ea94193b093ba8e1");
			if (blueprint16 != null)
			{
				AddUniversalAnswer(blueprint16, "IsekaiAct2JernaughGermTheory", "(Isekai Protagonist) [Modern Germ Theory & Sterilization Protocols] \"Brother Jernaugh, before your clerics exhaust their healing spells on minor infections, boil your surgical scalpels, wash your hands in clean alcohol, and use clean boiled bandages. Invisible microscopic pathogens cause wound fever, not bad air.\"", "{n}Brother Jernaugh stares at you in stunned contemplation, rubbing his chin as the sheer revolutionary logic of antiseptic medicine dawns upon him.{/n} \"Invisible contagions on unwashed blades and hands?! Sweet Sarenrae... that explains why fever spreads even when demonic magic is absent! Washing in alcohol and boiling water costs mere pennies compared to divine restorations! You have just saved hundreds of frontline soldiers with simple wisdom, Commander! Take these blessed draughts!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Grand Arbiter";
						c.ItemGuids = new string[2] { "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Empirical hygiene reduces field mortality exponentially. Superior medical governance.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint16, "IsekaiAct2JernaughSlimeSterilize", "(Slime) [Bio-Antiseptic Coating] \"A thin coating of purified digestive fluid neutralizes all bacterial decay on medical instruments instantly without dulling the edge.\"", "{n}Brother Jernaugh watches in astonishment as a corroded needle emerges sparkling clean.{/n} \"Remarkable! A natural antiseptic fluid! This will save countless limbs!\"", "DevourerProficiencies", 4);
			}
			BlueprintAnswersList blueprint17 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("34822d5daf196244db224703cef5d1d4");
			if (blueprint17 != null)
			{
				AddUniversalAnswer(blueprint17, "IsekaiAct2YakerPunctuality", "(Isekai Protagonist) [Corporate Punctuality Commendation] \"Arriving in full plate armor through enemy territory three minutes ahead of estimated schedule? Yaker, back in my world, operational efficiency like that earns you a rapid promotion to Senior Logistics Director. Catch your breath and take this focus band; we're marching to relieve the Paralictor right now.\"", "{n}Yaker stammers in disbelief, stiffening into an even more rigid salute as he accepts the headband with trembling gauntlets.{/n} \"Senior... Logistics Director?! Sir, the Order of the Godclaw trains us never to yield to physical exhaustion, but your appreciation of operational tempo honors our discipline. I will guide your vanguard to the redoubt without delay!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Inheritor";
						c.DirectItems = new BlueprintItem[1] { ItemMartialGodFocusBand };
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"Valorous punctuality rewarded! March forth and crush the gargoyle brood!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint17, "IsekaiAct2YakerMartialDiscipline", "(Martial God) [Iron Meridian Check] \"Your breathing is labored, but your core meridian remains unbent. Maintain that rhythm; we strike like lightning.\"", "{n}Yaker exhales sharply, steadying his stride.{/n} \"Understood, Commander! Lead on!\"", "MartialGodProficiencies", 4);
			}
			BlueprintAnswersList blueprint18 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("16ccf8f56019e2c4f92b1caec3864b5d");
			if (blueprint18 != null)
			{
				AddUniversalAnswer(blueprint18, "IsekaiAct2NurahHistoricalTrivia", "(Isekai Protagonist) [Comparative Multiversal Historiography] \"Nurah, comparing the First Crusade's annals to the legendary siege wars of my home world is fascinating. Back home, whenever a fortress fell from within, it was almost always a disgruntled court intellectual sabotaging the gates. Good thing Mendevian scholars are purely devoted to the cause, right?\"", "{n}Nurah's hand twitches violently over her parchment, ink splattering across her notes. She forces a high-pitched, strained giggle, quickly brushing away the blot.{/n} \"Hahaha! What an... extraordinarily dark historical observation, Commander! A disgruntled court scholar? Perish the thought! We historians only record history; we certainly don't manufacture catastrophic disasters! Excuse me, I must... organize my index cards!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Hermit";
						c.BannerMessage = "<color=#9370DB><b>[The Hermit]</b></color>: <i>\"The saboteur sweats under the gaze of an Otherworlder. Delicious psychological pressure.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint18, "IsekaiAct2NurahMastermindDetect", "(Mastermind) [Forensic Micro-Expression Analysis] \"Your pulse elevated by twenty beats when I mentioned sabotaged gates, Nurah. Continue writing; I enjoy observing compromised pawns.\"", "{n}Nurah goes pale as ash, burying her nose in her notes in terrified silence.{/n}", "MastermindProficiencies", 4);
			}
			BlueprintAnswersList blueprint19 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6856a7d6e123a0c46b666e8052008405");
			if (blueprint19 != null)
			{
				AddUniversalAnswer(blueprint19, "IsekaiAct2LostChapelTeaRations", "(Isekai Protagonist) [Warm Provisions & Morale Triage] \"Drink this piping hot spiced broth and wrap yourselves in these wool blankets. The gargoyle swarms are broken and the vanguard holds the hillside. Nobody else is dying on this mountain today.\"", "{n}The shivering, bloodstained crusaders weep with profound relief as the warm broth brings color back to their pale lips. Behind you, holy starlight pushes the mountain chill back into the gloom.{/n} \"The Commander... you came for us in the dark! Praise the Inheritor and praise the Otherworlder! We are saved!\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Dawnflower";
						c.BannerMessage = "<color=#FFD700><b>[The Dawnflower]</b></color>: <i>\"Mercy warms the freezing night. Your compassion guides lost souls back to life.\"</i>";
					});
				});
				AddAlignedAnswer(blueprint19, "IsekaiAct2LostChapelHeroWarmth", "(Hero) [Good] [Beacon of Deliverance] \"Stand tall, soldiers of Mendev! The nightmare is over! As long as my blade draws breath, you will never be forsaken in the dark!\"", "{n}A renewed cheer erupts from the rescued soldiers as hope rekindles in their eyes.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 3);
			}
			BlueprintAnswersList blueprint20 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("2f4e364807b094a4c8fdd1f9f8cfb1dd");
			if (blueprint20 != null)
			{
				AddUniversalAnswer(blueprint20, "IsekaiAct2LostChapelCommandCalm", "(Isekai Protagonist) [De-escalation of Command Friction] \"Cease the recriminations, both of you. Irabeth, surviving an ambush with your wits intact under torture is a victory, not a failure. Regill, your cold iron logic kept the perimeter from collapsing. We regroup our vanguards and take the summit together.\"", "{n}Irabeth releases a ragged, grateful breath, while Regill inclines his head in clinical acknowledgment.{/n} \"Understood, Commander. Tactical cohesion restored.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Sovereign leadership instantly resolves ideological division on the battlefield.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint21 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("364877496cda83640a9663b45ed0fc10");
			if (blueprint21 != null)
			{
				AddUniversalAnswer(blueprint21, "IsekaiAct2LostChapelAneviaCheck", "(Isekai Protagonist) [Orthopedic Check-In] \"How is that leg holding up in this freezing mud, Anevia? If those splints are slipping, I have fresh alchemical wraps. Irabeth is safe, and the path to camp is secure.\"", "{n}Anevia wipes cold sweat from her brow, a weary but deeply affectionate smile breaking through her exhaustion.{/n} \"Leg's holding together thanks to your handiwork, Commander. And hearing that Beth is alive... that's all the medicine I need. Thank you for never giving up on us.\"", 3, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"Love and loyalty endure the darkest mountain night.\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint22 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e47ef9ec640a1b4488ffb0c6a9983348");
			if (blueprint22 == null)
			{
				return;
			}
			AddUniversalAnswer(blueprint22, "IsekaiAct2DrezenYanielTheatrics", "(Isekai Protagonist) [Calling Out Theatrical Overacting] \"You lay the tragic martyr act on a little thick, 'Yaniel'. In my world, villains disguised as heroic martyrs usually get exposed in episode two by their over-rehearsed hand-wringing. Minagho's demonic perfume isn't completely masked by that holy aura.\"", "{n}The figure freezes mid-sob, her posture stiffening as her eyes flash with momentary fury before she hastily recomposes herself.{/n} \"Overacting?! What blasphemy is this?! I am Yaniel, chosen of the Inheritor! ...Do not doubt my sorrow, stranger!\"", 3, null, delegate(BlueprintAnswer bp)
			{
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 350;
					c.Sponsor = "The Laughing King";
					c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Seeing through the demon's B-tier melodrama instantly! Magnifique!\"</i>";
				});
			});
		}

		private static void InjectAct3Encounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("143d77815c045dd4abf5e81e948393b6");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiDrezenIntroTricksterBypass", "(Isekai Protagonist) [Tactical Subversion] \"Rebuilding Drezen? Leave the tedious clerical paperwork to the bureaucrats; my scouts and I will construct secret subterranean supply depots and teleportation relays across the perimeter.\"", "{n}Irabeth rubs her temples, half-exasperated and half-impressed.{/n} \"I do not understand half of your logistics jargon, Commander, but if it keeps our supply trains safe from gargoyle raids, make it so.\"", 5);
				AddAlignedAnswer(blueprint, "IsekaiDrezenIntroImperialMandate", "(God Emperor) [Imperial Mandate] \"Drezen is no longer merely a border outpost. By imperial decree, this citadel is consecrated as the eternal bastion of mortal sovereignty, where demonkind will learn divine judgment!\"", "{n}A stunned hush falls over the council chamber. The crusader captains straighten their postures, struck by an overwhelming wave of regal authority.{/n}", AlignmentShiftDirection.Lawful, "GodEmperorProficiencies", 5);
				AddSubclassAnswer(blueprint, "IsekaiDrezenIntroMartialCrucible", "(Martial God) [The Martial Crucible] \"Walls do not defend a city; warriors do. From this sunrise forth, every crusader in Drezen undergoes otherworlder combat drills until their reflexes outpace abyssal fiends.\"", "{n}The veteran sergeants exchange wide-eyed glances as your aura of pure martial mastery washes over the hall.{/n} \"Sir! We will double the training regimens immediately!\"", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint, "IsekaiDrezenIntroOverlordDominion", "(Overlord) [Iron Dominion] \"Drezen answers to my absolute authority. Any soldier who falters will find my displeasure far more terrifying than whatever lurks in the Worldwound.\"", "{n}The temperature in the throne room plummets as shadows dance obediently at your command. Irabeth swallows hard, silently nodding in grim compliance.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
				AddAlignedAnswer(blueprint, "IsekaiDrezenIntroMastermindLogistics", "(Mastermind) [Grand Logistics Calculation] \"Factoring regional mana leylines and abyssal weather cycles, standard supply attrition drops by 74% if we route through the southern ridges. Here is the operational blueprint.\"", "{n}Irabeth examines your crystalline analytical charts in sheer disbelief.{/n} \"This is... mathematically flawless. How did you compute weeks of logistics in moments?\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 5);
				AddAlignedAnswer(blueprint, "IsekaiDrezenIntroHeroLiberationCry", "(Hero) [Beacon of Dawn] \"We took Drezen for everyone who suffered under the Worldwound's shadow. As long as I draw breath, no demon will ever breach these gates again!\"", "{n}Cheers erupt from the doorway as junior crusaders and volunteers raise their swords in fervent salute!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("b9c56617dec2b7d42aea7b1c33fe5cbd");
			if (blueprint2 != null)
			{
				AddUniversalAnswer(blueprint2, "IsekaiHeraldPlanarCosmology", "(Isekai Protagonist) [Dimensional Comparative Analysis] \"I have seen cosmic pantheons across different realities, Herald. Heaven's order is admirable, but mortal determination is the true pivot of this war.\"", "{n}The Hand of the Inheritor tilts his golden helm thoughtfully.{/n} \"A perspective as profound as it is foreign to Golarion. I shall watch your deeds with keen interest, Commander.\"", 5);
				AddMythicAnswer(blueprint2, "IsekaiHeraldAngelSovereignty", "(God Emperor) [Angel] [Radiant Sovereignty] \"Heaven and the mortal realm stand as sovereign allies, Herald. Together, our golden light shall burn away the Worldwound's corruption down to the bedrock.\"", "{n}Golden halos interweave above you and the Herald as divine resonance echoes through the sanctum.{/n} \"Well spoken, divine sibling. May justice guide your sword!\"", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 6);
				AddSubclassAnswer(blueprint2, "IsekaiHeraldMartialSparChallenge", "(Martial God) [Blade Evaluation] \"You have fought fiends across ten thousand celestial wars, Herald. Sometime soon, cross blades with me; I wish to see how heaven's finest measures against otherworlder sword arts.\"", "{n}A spark of genuine warrior's fire ignites in the celestial's golden gaze.{/n} \"An extraordinary proposition, mortal warrior. I accept; when our duties permit, let us test our metal.\"", "MartialGodProficiencies", 5);
				AddMythicAnswer(blueprint2, "IsekaiHeraldAeonJurisdiction", "(Mastermind) [Aeon] [Cosmic Jurisdiction] \"The Worldwound violates the prime cosmic axis. My duty is to restore universal balance, with or without celestial dispensation.\"", "{n}The Herald observes the geometric ribbons of cosmic law swirling around your hands.{/n} \"The scales of eternity move through you, Commander. Walk the path of balance without wavering.\"", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 6);
				AddAlignedAnswer(blueprint2, "IsekaiHeraldOverlordPragmatism", "(Overlord) [Ruthless Necessity] \"Prayers and righteousness failed Kenabres for a century, Herald. Only overwhelming force and cold dominion will cleanse this continent.\"", "{n}The Herald's expression hardens with sorrow and warning.{/n} \"Take care, Commander. In hunting monsters with ruthless zeal, do not forge your own abyssal chains.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
				AddMythicAnswer(blueprint2, "IsekaiHeraldAzataJoyousAffirmation", "(Hero) [Azata] [Song of the Free Hearts] \"We don't need rigid dogmas to save this world, Herald. As long as freedom, kindness, and fellowship guide us, the demons don't stand a chance!\"", "{n}The Herald chuckles softly, his stern celestial demeanor softening.{/n} \"Your innocence and boundless courage bring warmth to this scarred earth, young hero.\"", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 6);
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("cd9c9facc3a8ded4e9683cde8958295e");
			if (blueprint3 != null)
			{
				AddUniversalAnswer(blueprint3, "IsekaiAreeluLabCheatCodeMockery", "(Isekai Protagonist) [Reality Architecture Analysis] \"Vorlesh thought she was running an unprecedented experiment. She didn't realize she wrote developer cheat codes directly into the spatial matrix for an Otherworlder to claim!\"", "{n}The laboratory's ambient arcane machinery hums erratically as your fingers effortlessly trace the dimensional seams, disarming latent warding traps instantly.{/n}", 6);
				AddMythicAnswer(blueprint3, "IsekaiAreeluLabAeonEquationSunder", "(Mastermind) [Aeon] [Dimensional Axiomatic Sunder] \"This planar suture is fundamentally illegal under cosmic geometry. I will excise Vorlesh's parasitic equations and restore the baseline reality coordinate.\"", "{n}Glowing mathematical runes of crystalline law circle your fingertips, suppressing the toxic abyssal seepage into inert vapor.{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 6);
				AddMythicAnswer(blueprint3, "IsekaiAreeluLabLegendMartialStrike", "(Martial God) [Legend] [Spatial Void Severance] \"I need neither demon flesh nor celestial favor to dismantle this rift. My blade cuts through the concept of the portal itself!\"", "{n}With a thunderous strike that rattles your teeth, your weapon cleaves the metaphysical seam, leaving the abyssal vortex shuddering and silenced!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 6);
				AddMythicAnswer(blueprint3, "IsekaiAreeluLabLichNahyndrianDrain", "(Overlord) [Lich] [Nahyndrian Necrotic Harvest] \"Vorlesh refined abyssal power for her petty schemes. I shall harvest these raw Nahyndrian currents into my own immortal soul crucible.\"", "{n}Pale necrotic vortexes swirl from your outstretched palm, siphoning the agonizingly potent rift energies directly into your dark core!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 6);
				AddMythicAnswer(blueprint3, "IsekaiAreeluLabAngelSolarPurge", "(God Emperor) [Angel] [Solar Consecration] \"Every inch of this foul laboratory reeks of violated mortal souls. In the name of heaven and earth, let holy fire cleanse this blasphemy!\"", "{n}Blinding solar flames cascade across the lab tables, incinerating horrific flesh-vats into clean, fragrant ash.{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 6);
				AddAlignedAnswer(blueprint3, "IsekaiAreeluLabHeroPurityVow", "(Hero) [Inviolate Mortal Heart] \"No matter what experiments Vorlesh performed, my heart belongs to those who trust me. I will wield this power to protect, never to dominate.\"", "{n}A comforting, warm radiance emanates from your chest, reassuring your companions and repelling the oppressive dread of the laboratory.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 6);
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("f240afdd8595d2340aa18ecf5ed233a3");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiWintersunIllusionMockery", "(Isekai Protagonist) [Phantasm Compression Disruption] \"A psychic projection this cheap wouldn't even pass for a budget virtual simulation back home. Drop the glamour, demon; your puppet strings are visible to anyone with eyes.\"", "{n}Jerribeth's porcelain smile twitches in momentary astonishment as your mana pulse ripples across the village square, momentarily revealing rotting carcasses beneath the feast.{/n}", 5);
				AddMythicAnswer(blueprint4, "IsekaiWintersunMastermindAeonDispersion", "(Mastermind) [Aeon] [Axiomatic Deconstruction] \"Your illusion violates cognitive symmetry. By introducing an inverse harmonic frequency, your enchantment grid collapses into inert noise.\"", "{n}With a crisp snap of your fingers, a lattice of blue starlight flashes across the settlement, tearing through Jerribeth's phantasms instantly!{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 6);
				AddAlignedAnswer(blueprint4, "IsekaiWintersunGodEmperorRadiantTruth", "(God Emperor) [The Emperor's Sun] \"You dare pose as a deity before a true sovereign? Begone, vermin, before the blinding dawn of righteous order!\"", "{n}A solar pillar erupts behind you, casting blinding rays that sear Jerribeth's demonic flesh and send her recoiling in agony!{/n}", AlignmentShiftDirection.Good, "GodEmperorProficiencies", 6);
				AddSubclassAnswer(blueprint4, "IsekaiWintersunMartialGodKillingIntent", "(Martial God) [Sunder Glamour: Apex Bloodlust] \"Your illusions reek of cheap perfume and rotten meat. Stop talking before my blade cleaves your voice from your throat.\"", "{n}A localized hurricane of sheer, suffocating killing intent pins Jerribeth in place, wiping the smirk cleanly from her face.{/n}", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint4, "IsekaiWintersunOverlordDominationDemand", "(Overlord) [Subjugation or Annihilation] \"You played with mortals like cattle, Jerribeth. Now kneel before a true master of shadows, or be ground into dust under my boot.\"", "{n}Your shadow engulfs the stone terrace, demonic whispers turning into shrill whimpers beneath your domineering pressure.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
				AddMythicAnswer(blueprint4, "IsekaiWintersunHeroAzataAwakening", "(Hero) [Azata] [Song of Awakening] \"People of Wintersun, hear my voice! The nightmare is over! Break free from her whispers and reclaim your freedom!\"", "{n}Your passionate cry echoes across the valley like spring rain, causing the blinded villagers to blink and weep as the trance dissolves!{/n}", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 6);
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("feac1861da9b5ae4b8e2629b84cac14e");
			if (blueprint5 != null)
			{
				AddUniversalAnswer(blueprint5, "IsekaiDragonhuntFlightTrajectory", "(Isekai Protagonist) [Monster Hunter Meta-Appraisal] \"Devarra's injured wing forces a 30-degree banking turn near the cliffside. Station our ballistas there; the dragon will fly directly into our trap.\"", "{n}Greybor pauses, squints at your tactical map, and nods with genuine professional respect.{/n} \"Flawless deduction, Commander. The beast won't know what hit it.\"", 5);
				AddSubclassAnswer(blueprint5, "IsekaiDragonhuntMartialGodSingleStroke", "(Martial God) [Dragon-Slaying Stance] \"Let the beast fly wherever it wishes. One clean vertical draw from my blade will sever its fiery breath at the larynx.\"", "{n}Greybor raises an appreciative eyebrow.{/n} \"Ambitious. Reckless, even. But looking at your stance, I have little doubt you can deliver on that boast.\"", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint5, "IsekaiDragonhuntOverlordTrophyClaim", "(Overlord) [Predatory Harvest] \"The red dragon's heart and scales will be harvested for my personal armory. Greybor, see to it that the corpse is not ruined by clumsy hands.\"", "{n}Greybor gives a curt, pragmatic nod.{/n} \"As long as my contract fee clears, Commander, the dragon's carcass is entirely yours to harvest.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
			}
			BlueprintAnswersList blueprint6 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("d7aadaa261a8023489bdba49216cc334");
			if (blueprint6 != null)
			{
				AddUniversalAnswer(blueprint6, "IsekaiKyadoTruthExtraction", "(Isekai Protagonist) [Spiritual Scrutiny] \"Kyado, your prayers ring hollow and your hands tremble with sulfur residue. Speak the truth of your demonic pact before your soul is forfeit.\"", "{n}Kyado collapses to his knees, sobbing hysterically as he confesses the demon pact.{/n} \"Forgive me! They threatened to tear my soul apart! I will tell you everything!\"", 4);
				AddAlignedAnswer(blueprint6, "IsekaiKyadoMastermindContractDissection", "(Mastermind) [Abyssal Contract Invalidation] \"You signed your pact out of duress and existential panic, Kyado. Under fundamental planar jurisprudence, a contract signed without true informed consent has four fatal loopholes. Hand me the parchment.\"", "{n}Kyado stares at you in slack-jawed bewilderment as you systematically tear the magical clauses of his demonic bondage to shreds!{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 5);
				AddAlignedAnswer(blueprint6, "IsekaiKyadoHeroRedemptionOffer", "(Hero) [Compassionate Redemption] \"Fear makes cowards of us all, Kyado. But redemption begins with confession. Stand behind us; we will protect you from the demons you feared.\"", "{n}Tears stream down Kyado's cheeks as he hands over the keys to the temple vaults, weeping in profound relief.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
			}
			BlueprintAnswersList blueprint7 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6278129d95bb73744a60b07fc6612e45");
			if (blueprint7 != null)
			{
				AddUniversalAnswer(blueprint7, "IsekaiPuluraGoldenSave", "(Isekai Protagonist) [The Golden Save: Multi-Dimensional Action] \"I refuse to choose. A dimensional barrier shields the priestesses from the fire while my spatial tether anchors Mutasafen's research carriage in place!\"", "{n}Mutasafen's jaw drops in horror as your dual-phased spell protects the crying priestesses while hauling the research cart safely into your hands!{/n} \"No! My life's work! Impossible!\"", 6);
				AddSubclassAnswer(blueprint7, "IsekaiPuluraMartialGodInterception", "(Martial God) [Supersonic Step: Dual Target Intercept] \"A binary dilemma is an illusion of the slow. While my afterimage shields the altar, my actual blade pins your carriage to the stone!\"", "{n}A sonic boom detonates across the canyon as your supersonic dash cleaves Mutasafen's axle in twain before he can ignite his firebombs!{/n}", "MartialGodProficiencies", 6);
			}
			BlueprintAnswersList blueprint8 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6e5d9e9370306194ca15420193bae14d");
			if (blueprint8 != null)
			{
				AddUniversalAnswer(blueprint8, "IsekaiGreengatesArueshalaeAffirmation", "(Isekai Protagonist) [Soul Affirmation] \"Your past in the Abyss does not define your future, Arueshalae. Every dream of freedom is proof that your soul belongs to you, not the demons.\"", "{n}Arueshalae's eyes fill with tears of profound gratitude as the holy bell chimes with resonant starlight, banishing all shadow fiends.{/n} \"Thank you... with you by my side, I will never look back!\"", 5);
				AddMythicAnswer(blueprint8, "IsekaiGreengatesHeroAzataSong", "(Hero) [Azata] [Chime of Elysium] \"Let the bells ring with the joy of tomorrow, Arueshalae! Even in the deepest shadows, the seeds of Elysium will always blossom!\"", "{n}Vibrant rainbow petals swirl around the belltower as celestial melodies dissolve every trace of demonic taint in the grove.{/n}", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 6);
			}
			BlueprintAnswersList blueprint9 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a01c59afcc9038c46a8bee2de45427cf");
			if (blueprint9 != null)
			{
				AddUniversalAnswer(blueprint9, "IsekaiMoltenScarVrockMockery", "(Isekai Protagonist) [Vocal Frequency Jamming] \"Save your shrieking dance, Vorimeraak. In my home world, we have audio equalizers that cancel out dissonant bird screeches entirely.\"", "{n}Vorimeraak gags as an acoustic dampening field chokes her throat mid-screech, leaving her flapping her wings in comical indignity.{/n}", 5);
				AddSubclassAnswer(blueprint9, "IsekaiMoltenScarMartialGodDecapitation", "(Martial God) [Feather-Severing Draw] \"Flap your wings all you like; my blade moves faster than the sound of your screams.\"", "{n}A flash of steel leaves Vorimeraak's plumes fluttering to the ash before she can even raise her talons.{/n}", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint9, "IsekaiMoltenScarGodEmperorExecution", "(God Emperor) [Imperial Condemnation] \"You have preyed on crusader scouts for the last time. As sovereign of Drezen, I sentence you to immediate purification!\"", "{n}Solar brilliance erupts from your hand, burning the vrock's plumage into glowing cinders!{/n}", AlignmentShiftDirection.Lawful, "GodEmperorProficiencies", 5);
				AddAlignedAnswer(blueprint9, "IsekaiMoltenScarHeroRescueVow", "(Hero) [No Soldier Left Behind] \"Release the captured crusaders immediately! Not another soul will die in your sulfur pits today!\"", "{n}The captive soldiers behind Vorimeraak shout in renewed hope at your fearless arrival!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
				AddAlignedAnswer(blueprint9, "IsekaiMoltenScarOverlordTorment", "(Overlord) [Subservience or Ashes] \"Vrock, you may either bow and deliver the prisoners unharmed, or your wings will decorate my fortress gates.\"", "{n}The monstrous bird shivers in genuine terror beneath your suffocating aura of malevolent authority.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
			}
			BlueprintAnswersList blueprint10 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("cbe3b0c3ea8b47045b66c085c1a1b354");
			if (blueprint10 != null)
			{
				AddUniversalAnswer(blueprint10, "IsekaiXanthirSwarmVacuum", "(Isekai Protagonist) [Atmospheric Vacuum] \"A swarm body relies on atmospheric air pressure to maintain cohesion. An inverted mana vortex scatters your locusts to the winds.\"", "{n}Xanthir Vang shrieks as your mana vortex tears his insectoid matrix apart, leaving him helpless.{/n} \"Curse you! My swarm! My glorious ascension!\"", 6);
				AddSubclassAnswer(blueprint10, "IsekaiXanthirSlimeDigestion", "(Slime) [Apex Swarm Digestion] \"A swarm of insects? You are merely a buffet of protein. My digestive enzymes consume your swarm faster than you can regenerate.\"", "{n}Your slime pseudopods engulf whole swarms in seconds, digesting them into inert biomass.{/n}", "DevourerProficiencies", 6);
				AddMythicAnswer(blueprint10, "IsekaiXanthirAngelSolarFire", "(God Emperor) [Angel] [Solar Incineration] \"Your defiled insects will not darken this world another day. Heaven's righteous light reduces your hive to ashes!\"", "{n}A torrent of pure solar fire engulfs the room, burning tens of thousands of locusts into white ash in a heartbeat!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 6);
				AddMythicAnswer(blueprint10, "IsekaiXanthirLegendMartialCleave", "(Martial God) [Legend] [Thousand-Blade Whirlwind] \"You think safety lies in numbers? My blade executes a thousand slashes in a single breath!\"", "{n}Blades of pressurized air crisscross the chamber, slicing Xanthir's swarm into millions of lifeless fragments!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 6);
				AddAlignedAnswer(blueprint10, "IsekaiXanthirMastermindPheromoneDisruption", "(Mastermind) [Pheromone Matrix Jamming] \"Your hive consciousness relies on high-frequency chemical signaling. By dispersing an inverted sulfur-enzyme, I turn your own swarm into cannibalistic chaos.\"", "{n}Xanthir howls in utter despair as the locusts forming his flesh begin viciously devouring each other!{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 6);
				AddMythicAnswer(blueprint10, "IsekaiXanthirLichResearchHarvest", "(Overlord) [Lich] [Entomological Soul Enslavement] \"Your bio-alchemy is crude, Xanthir, but your locust matrices will make excellent necrotic drones for my undead armies.\"", "{n}Black necrotic frost encases the swarm bodies, subjugating their lingering souls into your dark reservoir!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 6);
				AddAlignedAnswer(blueprint10, "IsekaiXanthirHeroAvengerVow", "(Hero) [Vow for the Lost] \"For every scout you tortured in these vile labs, your monstrous experiments end right here, right now!\"", "{n}Your battle-cry echoes through the stone corridors, invigorating your companions with relentless courage!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 6);
			}
			BlueprintAnswersList blueprint11 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("44704bddb6223b84989dd26bcf20b601");
			if (blueprint11 != null)
			{
				AddUniversalAnswer(blueprint11, "IsekaiGalfreyArrivesTricksterHumor", "(Isekai Protagonist) [Sovereignty Protocol Bypass] \"Welcome to Drezen, Your Majesty! I took the liberty of skipping royal formalities and reallocating the throne room budget to defensive enchantments.\"", "{n}Galfrey pauses, blinking in surprise before offering a tired, genuine smile.{/n} \"I would expect nothing less from you, Commander. Results speak louder than courtiers' etiquette.\"", 5);
				AddAlignedAnswer(blueprint11, "IsekaiGalfreyArrivesGodEmperorPeers", "(God Emperor) [Equal Sovereigns] \"Welcome, Queen of Mendev. You stand in Drezen not as my monarch, but as an honored fellow sovereign in our shared crusade against the Abyss.\"", "{n}Galfrey's royal guard stiffens, but the Queen holds up a hand, looking upon your majestic imperial aura with profound solemnity.{/n}", AlignmentShiftDirection.Lawful, "GodEmperorProficiencies", 6);
				AddSubclassAnswer(blueprint11, "IsekaiGalfreyArrivesMartialGodAssurance", "(Martial God) [The Unconquered Blade] \"Rest easy, Galfrey. The Midnight Fane and whatever demon lords lurk beneath will break upon my blade just like the rest.\"", "{n}Galfrey looks at your confident, relaxed martial posture and lets out a long breath of relief.{/n} \"May Iomedae grant your blade swift passage, warrior.\"", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint11, "IsekaiGalfreyArrivesOverlordWarning", "(Overlord) [Sovereign Ultimatum] \"Drezen and the Crusade answer to me now, Galfrey. Do not mistake my alliance for subservience to Nerosyan's distant crown.\"", "{n}Galfrey meets your icy gaze without flinching, though tension fills the air like drawn bowstrings.{/n} \"We shall see where your ambition leads you, Commander.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
				AddAlignedAnswer(blueprint11, "IsekaiGalfreyArrivesMastermindReport", "(Mastermind) [Comprehensive Strategic Audit] \"Here are the logistical metrics, casualty estimates, and tactical projections for breaching the Midnight Fane. We have minimized allied casualties to under 2.3%.\"", "{n}Galfrey reviews the precise tactical scrolls in amazement.{/n} \"A masterpiece of operational planning. You have transformed the crusade into an unyielding machine.\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 5);
				AddAlignedAnswer(blueprint11, "IsekaiGalfreyArrivesHeroGratitude", "(Hero) [Heart of the Crusade] \"Thank you for trusting me with Drezen, Your Majesty. We will finish what the crusaders started and bring peace back to Mendev!\"", "{n}Galfrey smiles warmly and places a gentle hand on your shoulder.{/n} \"The people believe in you, Commander. And so do I.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
			}
			BlueprintAnswersList blueprint12 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("d8d8512aa3c48a342a68e9873f92e88e");
			if (blueprint12 != null)
			{
				AddUniversalAnswer(blueprint12, "IsekaiFaneBreachTricksterDive", "(Isekai Protagonist) [The Otherworlder Dive] \"The demons have been invading our world for a century. It's about time someone invaded the Abyss right back and showed them how an Otherworlder plays the game!\"", "{n}Your bold cheer spreads through the crusader ranks like wildfire, turning grim anxiety into exhilarating battle fervor!{/n}", 6);
				AddMythicAnswer(blueprint12, "IsekaiFaneBreachAngelImperialConquest", "(God Emperor) [Angel] [Holy Conqueror's Decree] \"We do not enter the Abyss as prey or martyrs! We march as divine conquerors to deliver holy vengeance to the demon lords' doorsteps!\"", "{n}Blinding golden wings flare behind your silhouette as the entire army roars with unstoppable holy zeal!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Lawful, 6);
				AddMythicAnswer(blueprint12, "IsekaiFaneBreachLegendMartialPinnacle", "(Martial God) [Legend] [Apex Challenger] \"An entire dimension crawling with the strongest fiends in existence? This is the ultimate crucible. Every demon in the Abyss will learn to fear my blade!\"", "{n}You draw your weapon with a single sharp ring that echoes down into the endless dark of the rift, cutting through the abyssal chill!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 6);
				AddMythicAnswer(blueprint12, "IsekaiFaneBreachLichDreadVanguard", "(Overlord) [Lich] [The Abyss Shall Bow] \"The Abyss prides itself on dread and agony. When my shadow falls upon the Midnight Isles, even demon lords will know true fear.\"", "{n}Dark frost creeps along the edge of the portal as your dread aura pulses, chilling the demonic flames into ash.{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 6);
				AddMythicAnswer(blueprint12, "IsekaiFaneBreachMastermindAeonCalculus", "(Mastermind) [Aeon] [Planar Ingress Synchronization] \"Portal coordinates locked. Ingress vector synchronized with prime reality anchor. Let us execute the operation.\"", "{n}Geometric arrays of pure cosmic law stabilize the portal threshold, turning the chaotic vortex into a smooth transit gate.{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 6);
				AddMythicAnswer(blueprint12, "IsekaiFaneBreachHeroAzataDawnMarch", "(Hero) [Azata] [Marching toward the Dawn] \"For everyone waiting for us back home, we march into hell and bring back the dawn! Follow me, everyone!\"", "{n}A chorus of hopeful song swells among your companions and soldiers, banishing all shadow from their hearts!{/n}", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 6);
			}
			BlueprintAnswersList blueprint13 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("f4308e946d8562f4c9ebb611aff91529");
			if (blueprint13 != null)
			{
				AddUniversalAnswer(blueprint13, "IsekaiAct3FyeBrewingMasterclass", "(Isekai Protagonist) [Otherworld Coffee & Carbonated Cocktail Masterclass] \"Fye, sour ale and murky water won't keep the crusade sharp during long midnight watches. Let me teach you how to roast imported beans into steaming espresso and brew fizzy iced cocktails. One cup of this, and your patrons will have the energy of a raging barbarian!\"", "{n}Fye takes a tentative sip of the foaming, jet-black brew, his eyes bulging wide as a jolt of pure caffeinated lightning electrifies his senses. He lets out a breathless whoop, vigorously wiping down his bar counter with blinding speed.{/n} \"Sweet Cayden's tankard! My heart is pounding like a war drum and I feel like I could sprint straight to Kenabres and back! What is this sorcery?! You're telling me soldiers will pay double coin for this 'espresso' every morning?! Commander, you just made this tavern the most popular establishment in the entire Mendevian north! Take these freshly brewed thermoses!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Gold = 500;
						c.Sponsor = "The Lucky Drunk";
						c.DirectItems = new BlueprintItem[5] { ItemOtherworldEspresso, ItemOtherworldEspresso, ItemOtherworldEspresso, ItemOtherworldEspresso, ItemOtherworldEspresso };
						c.BuffToApply = OtherworldEspressoBuff;
						c.BuffDuration = new TimeSpan(0, 10, 0);
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"The sacred nectar of vigilance! Fye's tavern is now an official holy site of Cayden Cailean!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint13, "IsekaiAct3FyeOverlordVintage", "(Overlord) [Imperial Tavern Decree] \"This tavern now operates under imperial sovereignty, barkeep. Stock the finest vintages in the realm for my officers, or find your taps frozen.\"", "{n}Fye bows nervously, hurriedly setting aside his private reserve.{/n} \"At once, my lord! Only the finest cellar casks for your court!\"", "OverlordProficiencies", 4);
			}
			BlueprintAnswersList blueprint14 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("88cfebc7c46549aba284036a26e9eade");
			if (blueprint14 != null)
			{
				AddUniversalAnswer(blueprint14, "IsekaiAct3StorytellerEpicNovels", "(Isekai Protagonist) [Multiversal Narrative Parallels & Epic Literature] \"Storyteller, hearing your accounts of ancient Earthfall and the Age of Anguish reminds me of the serialized epic fantasy chronicles from my home dimension. We had storytellers who spent decades worldbuilding grand sagas with magic academies, chosen ones, and apocalyptic worldwounds.\"", "{n}The blind ancient elf smiles with profound, gentle nostalgia, his weathered fingers lightly tracing the cover of an ancient grimoire.{/n} \"Ah... across the ocean of stars, mortal hearts still yearn for the same light amidst the dark. We weave chronicles of suffering and triumph so that those who follow will remember that even after Earthfall, the sun eventually rises. Take this ring, child of another sky; let the annals of forgotten times guide your wisdom.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Hermit";
						c.DirectItems = new BlueprintItem[1] { ItemRingOfTheGrandAuditor };
						c.BannerMessage = "<color=#9370DB><b>[The Hermit]</b></color>: <i>\"The echoes of distant universes resonate within the Storyteller's tower. Wisdom rewarded.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint14, "IsekaiAct3StorytellerMastermindTropes", "(Mastermind) [Deconstructing Historical Tragedies] \"Ancient civilizations consistently collapsed due to pride, lack of strategic redundancy, and poor communication protocols. Observe how the Starstone crisis could have been mitigated.\"", "{n}The Storyteller chuckles softly, thoroughly intrigued by your clinical breakdown of ancient history.{/n} \"A remarkably pragmatic mind. If only the emperors of ancient Azlant possessed your analytical foresight.\"", "MastermindProficiencies", 5);
			}
			BlueprintAnswersList blueprint15 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("76d61bf7eb5ba0c4087cf98ab106385c");
			if (blueprint15 != null)
			{
				AddUniversalAnswer(blueprint15, "IsekaiAct3CiarTrafficLaws", "(Isekai Protagonist) [Cavalry Traffic Regulation & Courtyard Speed Limits] \"Sir Ciar, your Everbright knights are charging through the lower courtyard like they're trying to win an Otherworld street race. We need pedestrian crosswalks, designated mount parking stalls, and a fifteen-mile-per-hour speed limit before someone tramples an alchemist's cart!\"", "{n}Sir Ciar's stern, weathered features twitch with dry, exasperated amusement, though he crosses his gauntleted arms with reluctant approval.{/n} \"'Designated mount parking stalls' and 'speed limits'?! Hmph. Most commanders demand our knights ride faster, not obey courtyard traffic regulations! But... you aren't wrong. Sir Morveg nearly clipped a crate of alchemical fire yesterday. I will instruct our squires to establish designated cavalry lanes immediately.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Imposing municipal traffic laws on proud knights! Hilarious civic order!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint15, "IsekaiAct3CiarMartialStretches", "(Martial God) [Saddle Fatigue Breathing Kata] \"Your cavaliers suffer from pelvic meridian constriction after twelve hours in heavy stirrups. Teach them this core-alignment stretch before evening muster.\"", "{n}Sir Ciar tests the stretch, his spine popping with immediate relief.{/n} \"By the sun... the lower back stiffness is gone. You have my gratitude, Commander; this will keep our riders combat-effective.\"", "MartialGodProficiencies", 4);
			}
			BlueprintAnswersList blueprint16 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("ecaf5cfe8087a4f45a2269974f4885c9");
			if (blueprint16 != null)
			{
				AddUniversalAnswer(blueprint16, "IsekaiAct3ArsinoeTaxTithes", "(Isekai Protagonist) [Temple Tithe Optimization & Scroll Subscriptions] \"Priestess Arsinoe, selling single scrolls over a dusty counter is terribly inefficient. In my world, temples offer seasonal subscription packages for restorative potions and tax-deductible crusade donations. Standardize your pricing, and cathedral revenue will triple!\"", "{n}Arsinoe's eyes widen as she hurriedly pulls out an abacus and inkpot, muttering prayers of astonishment as the numbers line up.{/n} \"Seasonal scroll subscriptions and tax-deductible crusade donations?! Merciful Abadar... that is sheer clerical brilliance! We can fund three new field hospices with those margins! Take this blessed talisman of equilibrium as a donation receipt, Commander!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Gold = 400;
						c.Sponsor = "The Dark Prince";
						c.DirectItems = new BlueprintItem[1] { ItemAmuletOfOtherworldEquilibrium };
						c.BannerMessage = "<color=#FF4500><b>[The Dark Prince]</b></color>: <i>\"Modern financial wizardry applied to clerical dogma. Flawless execution.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint16, "IsekaiAct3ArsinoeGodEmperorPatron", "(God Emperor) [Imperial Temple Charter] \"The cathedral now serves under the sovereign charter of humanity. Tend to the people's spirits; imperial treasury funds will cover all expenses.\"", "{n}Arsinoe bows with deep reverent grace.{/n} \"The Inheritor smiles upon such magnificent generosity, Your Majesty.\"", "GodEmperorProficiencies", 5);
			}
			BlueprintAnswersList blueprint17 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("23ab8e982ccf3d348a486db7488ddd3d");
			if (blueprint17 != null)
			{
				AddUniversalAnswer(blueprint17, "IsekaiAct3JoranBessemerMetallurgy", "(Isekai Protagonist) [Modern Metallurgy & Carbon Folding] \"Master Vhane, your dwarven tempering is legendary, but study this: by injecting pressurized air through molten pig iron to burn off excess carbon, and adding trace amounts of vanadium, we can produce high-tensile cold iron steel that never chips against demon bone.\"", "{n}Joran drops his tongs with a resounding clatter, peering into the forge coals with wild, fanatical craftsmanship.{/n} \"Pressurized oxygen blast to purge carbon impurities?! And vanadium alloys?! By Torag's beard, that's not just smithing--that's true elemental transmutation! I can forge breastplates half the weight with twice the tensile strength! Hand me that slate, Commander; the citadel armory is about to make history!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Industrial metallurgy eradicates obsolete forging techniques. Superb technological injection.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint17, "IsekaiAct3JoranMartialBalance", "(Martial God) [Center-of-Mass Hilt Tuning] \"Shave two ounces from the pommel counterweight. The blade should align seamlessly with the radial nerve, becoming an extension of personal ki.\"", "{n}Joran files the hilt, handing the weapon back with a stunned nod.{/n} \"It balances like a feather on the fingertip... Immaculate eye, warrior.\"", "MartialGodProficiencies", 4);
			}
			BlueprintAnswersList blueprint18 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("143d77815c045dd4abf5e81e948393b6");
			if (blueprint18 != null)
			{
				AddUniversalAnswer(blueprint18, "IsekaiAct3IrabethWellnessCheck", "(Isekai Protagonist) [Mental Health Check-In & Warm Pastry] \"Irabeth, put down the requisition quill for five minutes and eat this warm almond pastry. You survived the Lost Chapel, you reclaimed Drezen, and you're doing an extraordinary job holding our supply lines together. Stop tormenting yourself with self-doubt.\"", "{n}Irabeth freezes, staring at the warm pastry in your hand as a fragile, trembling sigh escapes her lips. She rubs her weary eyes, her voice thick with unvoiced gratitude.{/n} \"Commander... I... thank you. It's so easy to drown in casualty reports and convince myself I'm unworthy of this uniform. Having someone acknowledge the struggle... it means more than you know. I'll take a breath. And... the pastry is delicious.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Dawnflower";
						c.BannerMessage = "<color=#FFD700><b>[The Dawnflower]</b></color>: <i>\"A compassionate word lifts the crushing weight of command from a weary paladin's heart.\"</i>";
					});
				});
				AddAlignedAnswer(blueprint18, "IsekaiAct3IrabethHeroPraise", "(Hero) [Good] [Cornerstone of the Crusade] \"You held Kenabres when everything collapsed, Irabeth. Without your courage at Defender's Heart, none of us would be standing in Drezen today. Be proud.\"", "{n}Irabeth stands tall, her gaze steadying with renewed paladin conviction.{/n} \"Thank you, Commander. I won't let you or Mendev down.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 4);
			}
			BlueprintAnswersList blueprint19 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("b2b77a591c8808f409aa2083bd1b318a");
			if (blueprint19 != null)
			{
				AddUniversalAnswer(blueprint19, "IsekaiAct3MoltenScarDeescalation", "(Isekai Protagonist) [Combat Stress Psychology & Honorable Reprieve] \"Lower your daggers. In my world, we understand that human minds break under relentless psychological trauma--we call it combat fatigue. You aren't cowardly traitors; you are exhausted soldiers driven mad by demonic whispers. Stand down, report to the Drezen rear guard for medical rehabilitation, and your lives are spared.\"", "{n}The trembling deserters collapse to their knees in the volcanic ash, tears streaming through soot-streaked faces as they cast their weapons aside.{/n} \"Combat... fatigue? You don't think we're filthy traitors deserving the gallows?! Merciful gods... the voices in the sulfur drove us half-insane! Thank you, Commander! We will serve in the supply trains until our dying breath!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Inheritor";
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"Compassion redeems broken souls where cold iron would only foster despair.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint19, "IsekaiAct3MoltenScarOverlordReprieve", "(Overlord) [Conditional Sovereign Pardon] \"Fools who flee before my standard commit treason, yet squandering living labor is inefficient. Return to the Drezen vanguard as frontline sappers, or be erased.\"", "{n}The deserters bow frantically in terror.{/n} \"We obey! We will clear the trenches with our bare hands! Thank you, Lord Commander!\"", "OverlordProficiencies", 4);
			}
			BlueprintAnswersList blueprint20 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("cc0f2a51d6fe7e54f801ba49816db136");
			if (blueprint20 != null)
			{
				AddUniversalAnswer(blueprint20, "IsekaiAct3WintersunHorrorFlags", "(Isekai Protagonist) [Horror Film Rural Trope Analysis] \"An isolated forest hamlet where everyone smiles too cheerfully, pretends the Worldwound doesn't exist, and serves mystery stew? In my world, this place is ticking literally every single 'haunted pagan cult movie' checklist. Keep your shields raised; this idyllic village is an illusion waiting to bite.\"", "{n}Your companions glance warily around the quiet settlement, hands instinctively tightening on their weapon pommels as the uncanny, Stepford-like stillness of the villagers becomes unnervingly apparent.{/n} \"A 'horror film checklist'?! Well, whatever strange stories they tell in your world, Commander, the hairs on the back of my neck agree completely. Something is deeply rotten in Wintersun.\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Spotting the creepy village tropes before the jump-scare! A seasoned veteran!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint21 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("feac1861da9b5ae4b8e2629b84cac14e");
			if (blueprint21 == null)
			{
				return;
			}
			AddUniversalAnswer(blueprint21, "IsekaiAct3DragonhuntRaidMechanics", "(Isekai Protagonist) [MMO Dragon Raid Phase Tactics] \"Listen up! Flying dragons always cycle through three distinct combat phases: ground breath cone, aerial dive bombs, and tail cleave! Greybor, hold the flank; casters spread out in a sixty-degree arc to avoid stacking breath damage! When she lands, burn all cooldowns!\"", "{n}Greybor spits into the mountain gravel, a grim, admiring smirk crossing his dwarven features as he tightens his axe grips.{/n} \"'Aerial dive bombs' and 'avoid stacking breath cone damage'? Heh. Never heard dragon hunting described like an engineering drill, Commander, but the tactical logic is airtight. Let's ground this overgrown lizard and collect our bounty!\"", 4, null, delegate(BlueprintAnswer bp)
			{
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 350;
					c.Sponsor = "The Lucky Drunk";
					c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"Raiding the red dragon with textbook tactical positioning! Let's get that loot!\"</i>";
				});
			});
		}

		private static void InjectAct4Encounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("91097a9b90537a34fbe379d5aa4d9cbe");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiNexusCampTricksterArrival", "(Isekai Protagonist) [Planar Beachhead Meta] \"So this is the Abyss! A bit gloomy, sure, but nothing an Otherworlder can't turn into a forward operating base. Time to explore!\"", "{n}The Hand of the Inheritor looks upon your unfazed demeanor with awe and renewed hope.{/n} \"Your courage in this cursed realm is a beacon to us all, Commander.\"", 5);
				AddMythicAnswer(blueprint, "IsekaiNexusCampAngelConsecration", "(God Emperor) [Angel] [Planar Consecration] \"Heaven's light does not retreat before the Abyss! By my imperial mandate, this Nexus is consecrated as holy ground!\"", "{n}Blinding golden light suffuses the volcanic cavern, driving out abyssal shadows and creating a pure oasis of celestial peace.{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 6);
				AddSubclassAnswer(blueprint, "IsekaiNexusCampMartialGodAcclimation", "(Martial God) [Vanguard Acclimation] \"The gravity is heavy and the air tastes of brimstone. Perfect. Every swing of my blade will be forged twice as sharp in this crucible.\"", "{n}You execute a few effortless practice draws, your blade slicing the sulfuric air with melodic, lethal precision.{/n}", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint, "IsekaiNexusCampOverlordTerritoryClaim", "(Overlord) [First Territory Claim] \"The Midnight Isles do not know it yet, but this camp is merely the first stone in my new abyssal dominion.\"", "{n}Dark shadows swirl obediently across the cave entrance, warding the camp with chilling dread.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
				AddAlignedAnswer(blueprint, "IsekaiNexusCampMastermindPlanarMapping", "(Mastermind) [Planar Topology Audit] \"Abyssal gravitational drift and floating island resonance calculated. I have mapped three optimal infiltration vectors into Alushinyrra.\"", "{n}The Herald studies your geometric crystalline maps in astonishment.{/n} \"You navigate the labyrinthine Abyss as if you have lived here for centuries, Commander.\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 5);
				AddAlignedAnswer(blueprint, "IsekaiNexusCampHeroSteadfastHeart", "(Hero) [Unwavering Hope in Hell] \"No matter how dark or terrifying this realm is, we stand united. We will find the source of the Nahyndrian crystals and save our world!\"", "{n}Your companions nod with fierce, renewed resolve as your heroic warmth dispels their doubts.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("c896137fe31c15445bba592672ebfbc9");
			if (blueprint2 != null)
			{
				AddUniversalAnswer(blueprint2, "IsekaiFleshmarketTricksterLiquidation", "(Isekai Protagonist) [Hostile Commercial Liquidation] \"Selling mortals by the pound? Your markup margins are absurd and your customer service is atrocious. This market is officially under new management: bankruptcy and liberation!\"", "{n}Dyunk stutters in enraged bewilderment as your mana-pulse shatters every slave collar in the plaza simultaneously!{/n} \"What have you done?! My merchandise! The guards, seize them!\"", 6);
				AddMythicAnswer(blueprint2, "IsekaiFleshmarketAngelHolyLiberation", "(God Emperor) [Angel] [Solar Emancipation] \"Every chain forged in this wretched pit melts before heaven's righteous fury! By sovereign decree, all captives are emancipated under my aegis!\"", "{n}Blinding golden solar flares erupt across the market, incinerating iron manacles into vapor while shielding every slave from demonic claws!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 6);
				AddSubclassAnswer(blueprint2, "IsekaiFleshmarketMartialGodSunderSlavers", "(Martial God) [Apex Sunder: Slaver Cleave] \"You trade in flesh because you are too cowardly to test your steel against a real warrior. Draw your weapons, vermin; let's see how much your own meat fetches.\"", "{n}A sonic detonation of supreme killing intent drops the surrounding slaver brutes to their knees, vomiting black bile in terror.{/n}", "MartialGodProficiencies", 6);
				AddAlignedAnswer(blueprint2, "IsekaiFleshmarketOverlordTitheDominion", "(Overlord) [Sovereign Confiscation] \"The Fleshmarket now belongs to my dark dominion. Hand over the ledger keys and kneel, or your flesh will be the first item placed on the auction block.\"", "{n}Dyunk pales to the color of curdled milk as your dread aura freezes the blood in his veins, forcing him into frantic prostration.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 6);
				AddAlignedAnswer(blueprint2, "IsekaiFleshmarketMastermindCartelAudit", "(Mastermind) [Abyssal Macroeconomic Collapse] \"By manipulating Alushinyrra's soul-token arbitrage and flashing counter-trade promissory writs, your cartel's assets just vanished into thin air. You are completely insolvent, Dyunk.\"", "{n}Dyunk stares in horror at his enchanted transaction stones as the balances plummet to absolute zero in seconds!{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 6);
				AddAlignedAnswer(blueprint2, "IsekaiFleshmarketHeroAegisVow", "(Hero) [No One Left in Chains] \"We swore to protect the innocent wherever the crusade marches! Slavers, step back from those cages, or you will not survive the next ten seconds!\"", "{n}The enslaved crusaders and tieflings cheer in roaring ecstasy as your hero's barrier pushes the demonic guards back!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 6);
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6edb974ede9417542abc4f2fc7c1bade");
			if (blueprint3 != null)
			{
				AddUniversalAnswer(blueprint3, "IsekaiBattleblissTournamentArcMockery", "(Isekai Protagonist) [Anime Tournament Arc Meta] \"You call this chaotic brawl an arena? Back home, a proper tournament arc has tiered brackets, dramatic backstories, and actual choreography. Time to show this city a real championship fight!\"", "{n}Zeklex blinks his manifold eyes in bewilderment, yet the abyssal crowd roars in bloodthirsty anticipation of your promised spectacle!{/n}", 6);
				AddMythicAnswer(blueprint3, "IsekaiBattleblissMartialGodApexGladiator", "(Martial God) [Legend] [The God of the Arena] \"Gelderfang? He telegraphs his heavy swing by three whole frames. Send every gladiator in the pits at once; maybe then I won't fall asleep from boredom.\"", "{n}A shockwave of pure martial dominance reverberates through the arena stone, silencing forty thousand demonic spectators in stunned awe!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 7);
				AddMythicAnswer(blueprint3, "IsekaiBattleblissGodEmperorGladiatorLiberation", "(God Emperor) [Angel] [Sovereign of the Sands] \"Mortal blood will no longer be spilled for demonic amusements! Gladiators of Battlebliss, lay down your chains and rally behind the golden dawn!\"", "{n}Holy luminescence rains upon the arena floor, burning away demonic brand-marks and invigorating the captive champions with divine strength!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 6);
				AddAlignedAnswer(blueprint3, "IsekaiBattleblissOverlordBloodDomain", "(Overlord) [Reign of Blood and Iron] \"This arena will be consecrated in the entrails of its masters. Zeklex, prepare the arena floor: today, the blood runs until Alushinyrra drowns.\"", "{n}Shadows writhe across the sand as your demonic bloodlust chills the spectators into uneasy silence.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 6);
				AddAlignedAnswer(blueprint3, "IsekaiBattleblissMastermindTacticalAnalysis", "(Mastermind) [Predictive Combat Calculus] \"Gelderfang relies on inertial momentum for his secondary cleave. A simple side-step feint at 0.4 seconds exposes his cervical spine to instant lethality.\"", "{n}Zeklex frantically scribbles your tactical observations on his slate, trembling at your chilling precision.{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 6);
				AddMythicAnswer(blueprint3, "IsekaiBattleblissHeroAzataFreedomSong", "(Hero) [Azata] [Song of the Unbroken] \"Hear me, fighters! You are not beasts to be slaughtered! Fight for your freedom, fight for tomorrow! We break these walls together!\"", "{n}Your stirring battle-hymn echoes across the coliseum, igniting a full-scale gladiator revolt as slaves shatter their holding gates!{/n}", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 6);
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("43f93812d6216c94db356622859397f1");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiHerraxSocialDeduction", "(Isekai Protagonist) [Social Deduction - The Mastermind's Gambit] \"Herrax, your girls aren't gathering coin; they are gathering scrying frequencies for the Shadow Demon cabal. Play straight with me, or your establishment becomes rubble.\"", "{n}Herrax gasps, bowing low with frantic subservience.{/n} \"Mercy, my lord! Here is the guest ledger! Take it, take everything!\"", 5);
				AddMythicAnswer(blueprint4, "IsekaiHerraxGodEmperorSolarPurge", "(God Emperor) [Angel] [Sanctification of Shadows] \"Your den of lust and extortion is an insult to mortal dignity. Speak the truth of your masters before my golden aura burns this den to cinders!\"", "{n}Herrax covers her eyes, screaming as celestial radiance singes her succubus silks.{/n} \"I will speak! I will confess! Just douse the flames!\"", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 6);
				AddSubclassAnswer(blueprint4, "IsekaiHerraxMartialGodBladeWard", "(Martial God) [Vanish-Cut: Assassin Neutralization] \"Tell the hidden shadow demons behind the drapery to sheath their daggers. My blade has already cut their shadows from their spines.\"", "{n}Three concealed assassins collapse from behind the curtains, their weapons sliced clean in half before they could even draw breath!{/n}", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint4, "IsekaiHerraxOverlordDarkCoercion", "(Overlord) [Subjugation of the Courtesans] \"You will serve as my eyes and ears in Alushinyrra, Herrax. Every drop of information passing through these halls belongs to me, or your soul will feed my hounds.\"", "{n}Herrax prostrates herself, trembling in genuine, unfeigned terror.{/n} \"Everything, Dread Sovereign! My network is entirely yours to command!\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
				AddMythicAnswer(blueprint4, "IsekaiHerraxHeroAzataDreamLiberation", "(Hero) [Azata] [Cleansing the Stolen Dreams] \"The mortals trapped here deserve their own lives, not to be drained for demonic vices. I am setting every captive free right now!\"", "{n}A breeze of sweet wildflowers sweeps through the perfumed corridors, breaking succubus enchantments and opening every locked door!{/n}", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 6);
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e76902cd78ea1d24687eed6a97c51598");
			if (blueprint5 != null)
			{
				AddUniversalAnswer(blueprint5, "IsekaiMageTowerPortalAlignment", "(Isekai Protagonist) [Dimensional Compass] \"A rotating multidimensional tower? The gravitational axes are aligned in a 3-2-4 sequence; all doors are unlocked.\"", "{n}All mystical doors flash gold in unison, locking open without need for camera rotation.{/n}", 5);
				AddMythicAnswer(blueprint5, "IsekaiMageTowerAeonGravitationalOrder", "(Mastermind) [Aeon] [Axiomatic Space Stabilization] \"This chaotic spatial rotation is an affront to universal physics. Freezing the rotational matrix at the absolute Prime Material harmonic.\"", "{n}Crystalline geometric grids snap across the architecture, locking all twisting hallways into solid, predictable corridors!{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 6);
				AddMythicAnswer(blueprint5, "IsekaiMageTowerLegendMartialSunder", "(Martial God) [Legend] [Dimensional Cleave Bypass] \"Who has time for rotating doors? A single vertical strike cuts through the spatial barrier between this floor and the roof!\"", "{n}With a thunderous crack, your blade tears open a direct rift in space leading straight to the wizard's inner sanctum!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 6);
				AddMythicAnswer(blueprint5, "IsekaiMageTowerLichLeylineDrain", "(Overlord) [Lich] [Abyssal Leyline Siphon] \"The arcane power circulating this tower will fuel my own soul forge. Siphoning the conduits into my personal focus!\"", "{n}Necrotic tendrils drain the glowing runes along the walls, granting you dark empowerment while forcing the doors wide open!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 6);
				AddAlignedAnswer(blueprint5, "IsekaiMageTowerHeroPathfinderGuide", "(Hero) [Safe Passage for the Party] \"Stay close to me, everyone! I will disarm the teleportation hazards so none of our companions are lost to the void!\"", "{n}Your protective aura envelops your friends, nullifying disorienting gravitational shifts and guiding them safely through the tower.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
			}
			BlueprintAnswersList blueprint6 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("75678bafb5e33854baaee17ee6eaf69c");
			if (blueprint6 != null)
			{
				AddUniversalAnswer(blueprint6, "IsekaiNocticulaExitStrategyPsychology", "(Isekai Protagonist) [Narrative Psychology - The Demon Lord's Exit] \"Lady Nocticula, let's skip the theatrical seductions. You are exhausted by the petty savagery of the Abyss and looking for an exit strategy into true divinity. Am I warm?\"", "{n}Nocticula's seductive smile freezes for an imperceptible fraction of a second, her dark eyes narrowing with razor-sharp fascination.{/n} \"How dreadfully perceptive of you, mortal... Perhaps you are far more than another disposable champion of the crusade.\"", 7);
				AddMythicAnswer(blueprint6, "IsekaiNocticulaGodEmperorSovereignParity", "(God Emperor) [Angel] [Sovereign Parity] \"I stand in your palace not as a supplicant, Lady of Shadows, but as the supreme sovereign of the Crusade. We dictate terms to the Abyss, not the reverse!\"", "{n}Golden imperial halos pulse behind you, resisting Nocticula's intoxicating aura with effortless majesty. She leans forward on her throne, genuinely intrigued.{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Lawful, 7);
				AddSubclassAnswer(blueprint6, "IsekaiNocticulaMartialGodBladeWarning", "(Martial God) [Apex Stance: God-Slaying Intent] \"Keep your shadows where they belong, Nocticula. My blade has cut through concepts and dimensions; do not make the mistake of thinking your demon flesh is immortal.\"", "{n}A razor-thin line of vacuum chills the royal chamber, causing Nocticula's shadow-guards to gasp in sudden, frantic alarm.{/n}", "MartialGodProficiencies", 6);
				AddAlignedAnswer(blueprint6, "IsekaiNocticulaOverlordPlanarPact", "(Overlord) [Equal Exploitation] \"You rule Alushinyrra, Nocticula, but my reach expands across realities. Assist my crusade against Baphomet and Deskari, and I shall allow your realm to survive my ascension.\"", "{n}Nocticula laughs, a dark, melodic sound that echoes with mutual ambition.{/n} \"Such delightful, brazen audacity! I do appreciate a creature with grand appetites.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 6);
				AddMythicAnswer(blueprint6, "IsekaiNocticulaMastermindAeonForensics", "(Mastermind) [Aeon] [Nahyndrian Forensics Delivery] \"Here is the actuarial balance of Vorlesh's crystalline harvests and Baphomet's planned betrayal. My calculations reveal you have 48 hours before his vanguard attempts your assassination.\"", "{n}Nocticula reviews your crystalline data projections with chilling intensity, nodding slowly.{/n} \"Invaluable intelligence, Commander. The horned beast will find his ambitions severely punished.\"", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 7);
				AddMythicAnswer(blueprint6, "IsekaiNocticulaHeroAzataRedemptionWhisper", "(Hero) [Azata] [The Whisper of Elysium] \"Even in the darkest abyss, a soul is never permanently chained to evil. When you are ready to seek the stars and leave this shadow behind, remember this conversation.\"", "{n}For a fleeting heartbeat, the Lady of Shadows gazes into your eyes with profound, melancholic silence before turning away with a faint smile.{/n}", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 7);
			}
			BlueprintAnswersList blueprint7 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("abf7287a88afc554da91b3cd5d7c5bd1");
			if (blueprint7 != null)
			{
				AddUniversalAnswer(blueprint7, "IsekaiChivarroVanityDeconstruction", "(Isekai Protagonist) [Ego Shatter] \"Your entire hierarchy is built on pathetic vanity and mutual deceit. In the grand multiversal scale, your petty squabbles wouldn't even warrant a B-plot.\"", "{n}Chivarro shrieks in wounded pride as your casual dismissal stings far worse than any holy water.{/n}", 5);
				AddSubclassAnswer(blueprint7, "IsekaiChivarroMartialGodInstantSever", "(Martial God) [Zero-Frame Disarm] \"Your vanity charms won't save you. Draw your steel, or surrender the keys to the portal immediately.\"", "{n}A single flicker of your scabbard sends Chivarro's enchanted whip flying across the chamber, embedded deep into the stone wall.{/n}", "MartialGodProficiencies", 5);
				AddAlignedAnswer(blueprint7, "IsekaiChivarroGodEmperorImperialExile", "(God Emperor) [Imperial Condemnation] \"Your reign over these corrupted pleasure dens is terminated. Begone from my sight before divine sunlight reduces you to ash!\"", "{n}Solar brilliance flares from your silhouette, blinding the succubus guards and driving them into panicked retreat!{/n}", AlignmentShiftDirection.Lawful, "GodEmperorProficiencies", 5);
				AddAlignedAnswer(blueprint7, "IsekaiChivarroOverlordEnslavement", "(Overlord) [Subservience Under Penalty of Death] \"You will crawl before my throne and serve as an informant, Chivarro. Cross me once, and I will feed your soul to the outer void.\"", "{n}Chivarro falls to her knees, trembling under the weight of your suffocating malice.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 5);
				AddAlignedAnswer(blueprint7, "IsekaiChivarroHeroMercyChoice", "(Hero) [The Choice of Mercy] \"Leave Alushinyrra and never harm another mortal again, Chivarro. This is the only warning you will receive.\"", "{n}Stunned by your unexpected mercy, Chivarro scurries into the shadows without looking back.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 5);
				AddAlignedAnswer(blueprint7, "IsekaiChivarroMastermindNetworkSubversion", "(Mastermind) [Espionage Grid Hijack] \"Your entire communications network has been compromised. Hand over the cipher ring, and you might survive the night.\"", "{n}Faced with your undeniable proof of her network's compromise, Chivarro tosses her signet ring in bitter surrender.{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 5);
			}
			BlueprintAnswersList blueprint8 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("995aaa29e772b594a966a2f133be702c");
			if (blueprint8 != null)
			{
				AddUniversalAnswer(blueprint8, "IsekaiHepzamirahAntagonistDeconstruction", "(Isekai Protagonist) [Antagonist Deconstruction] \"Hepzamirah, you think yourself Baphomet's cherished daughter. You are merely his sacrificial goat to pave his planar incursion.\"", "{n}Hepzamirah's fanatical fury turns to bewildered horror as your words strike the hollow core of her devotion.{/n}", 6);
				AddMythicAnswer(blueprint8, "IsekaiHepzamirahAngelHolySmite", "(God Emperor) [Angel] [Heaven's Execution] \"Your defiled mines end here, daughter of Baphomet! Consecrating these caverns with heaven's wrath!\"", "{n}A catastrophic pillar of celestial fire crushes Hepzamirah's cultist bodyguards into ash!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 7);
				AddSubclassAnswer(blueprint8, "IsekaiHepzamirahMartialGodHornSever", "(Martial God) [Horn-Severing Horizon] \"Your giant blood and demonic lineage are nothing before true mastery. One strike to sever your prized horn!\"", "{n}With blinding speed, your blade cleaves Hepzamirah's demonic horn clean off, sending it tumbling across the stone!{/n}", "MartialGodProficiencies", 6);
				AddMythicAnswer(blueprint8, "IsekaiHepzamirahLichNahyndrianDrain", "(Overlord) [Lich] [The True Nahyndrian Heir] \"These Nahyndrian crystal veins belong to me now, Hepzamirah. Your soul will serve as the keystone of my abyssal throne!\"", "{n}Black soul-frost surges from your hand, draining the crystalline power directly from the mine shafts into your phylactery!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 7);
				AddAlignedAnswer(blueprint8, "IsekaiHepzamirahMastermindStructuralDisruption", "(Mastermind) [Mine Shaft Demolition Matrix] \"The support struts of your crystal refinery are under extreme tectonic tension. A single targeted shockwave collapses your escape route.\"", "{n}A calculated concussive burst detonates behind Hepzamirah, burying her reinforcement vanguard under tons of granite!{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 6);
				AddAlignedAnswer(blueprint8, "IsekaiHepzamirahHeroSlaveEvacuation", "(Hero) [Miners' Emancipation] \"Crusaders! Free the miners and secure the skiffs! We hold the line against Hepzamirah while you escape!\"", "{n}Cheering miners pour out of the pits as your defensive aegis shields them from the cultists' arrows!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 6);
			}
			BlueprintAnswersList blueprint9 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("9ab9ad8f6e11d67499b48fd595e50972");
			if (blueprint9 != null)
			{
				AddUniversalAnswer(blueprint9, "IsekaiBaphometDimensionalWard", "(Isekai Protagonist) [Dimensional Ward] \"A Demon Lord arrives? We stand our ground! Multiversal ward deployed!\"", "{n}A crystalline barrier of multiversal mana negates Baphomet's suffocating fear aura across your entire party!{/n}", 7);
				AddMythicAnswer(blueprint9, "IsekaiBaphometAngelDivineChallenge", "(God Emperor) [Angel] [The Dawn of Judgment] \"Baphomet! The Lord of the Ivory Labyrinth cowers before the righteous dawn! You will answer for a century of crusader blood!\"", "{n}Blinding solar wings unfurl from your back, forcing the goat-headed demon lord to shield his glowing red eyes in rage and disgust!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 7);
				AddMythicAnswer(blueprint9, "IsekaiBaphometLegendMartialChallenger", "(Martial God) [Legend] [The God-Slayer's Roar] \"A Demon Lord in the flesh! Finally, a neck thick enough to test the full power of my blade! Come, beast, show me your labyrinth!\"", "{n}Your weapon roars with a hurricane of pressurized air, cutting deep gouges into the cavern walls before you even take a step forward!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 7);
				AddAlignedAnswer(blueprint9, "IsekaiBaphometOverlordLabyrinthClaim", "(Overlord) [Sovereign of Nightmares] \"You rule a maze of cattle, Baphomet. When I tear your horns from your skull, the Ivory Labyrinth will be annexed into my eternal empire!\"", "{n}Suffocating dread rolls off your form, meeting Baphomet's abyssal aura in a shrieking clash of dark titans!{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 7);
				AddMythicAnswer(blueprint9, "IsekaiBaphometMastermindAeonGeometrySunder", "(Mastermind) [Aeon] [Axiomatic Sunder of the Maze] \"The geometric topology of your labyrinth is an obsolete fractal. Every corridor in your realm now leads directly to your inevitable doom!\"", "{n}Ribbons of crystalline cosmic law fracture Baphomet's planar halo, exposing his metaphysical core to reality's judgment!{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 7);
				AddMythicAnswer(blueprint9, "IsekaiBaphometHeroAzataHeartOfFreedom", "(Hero) [Azata] [The Song That Breaks All Mazes] \"No labyrinth can trap a heart that flies free! For everyone you butchered in the crusade, we stand unbroken and we will never bow!\"", "{n}Celestial song cascades through the volcanic caverns, shattering Baphomet's despair curse across your entire retinue!{/n}", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 7);
			}
			BlueprintAnswersList blueprint10 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("772496e11e1b6804da503d2883417f97");
			if (blueprint10 != null)
			{
				AddUniversalAnswer(blueprint10, "IsekaiAct4BadLuckHealthViolation", "(Isekai Protagonist) [Municipal Health Code Violations in the Abyss] \"Barkeep, I've conducted health inspections in some sketchy dive bars, but this establishment fails on fourteen consecutive health code violations: raw demon viscera on the cutting board, acidic sludge leaking into the grog cask, and whatever is breathing inside that mug. Wipe the counter down before the health department condemns the whole block!\"", "{n}The demonic innkeeper blinks all three of his bloodshot yellow eyes, scratching his bulbous horn in utter, dumbfounded incomprehension.{/n} \"'Health code violations'?! 'Municipal department'?! Mortal, this is Alushinyrra! We serve pure distilled misery and poison! But... if you're tossing around shiny otherworld coin like that, I suppose I can wipe the skull off the counter with a relatively clean rag! Take this lucky pendant from the last fool who choked on the house brew!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Gold = 600;
						c.Sponsor = "The Dark Prince";
						c.DirectItems = new BlueprintItem[1] { ItemAmuletOfOtherworldEquilibrium };
						c.BannerMessage = "<color=#FF4500><b>[The Dark Prince]</b></color>: <i>\"Demanding sanitary standards from abyssal dive bars! Superbly insolent pragmatism!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint10, "IsekaiAct4BadLuckGodEmperorTip", "(God Emperor) [Imperial Coin of Cleanliness] \"Clean this table, fiend. The sovereign vanguard does not rest where filth festers.\"", "{n}The demon innkeeper scurries like a frightened rat, violently polishing the dark stone with both sleeves.{/n} \"Right away, glorious majesty! Immediately!\"", "GodEmperorProficiencies", 5);
			}
			BlueprintAnswersList blueprint11 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("b45c22db4bfe8ad4ab106941ede45b7d");
			if (blueprint11 != null)
			{
				AddUniversalAnswer(blueprint11, "IsekaiAct4BadLuckHalflingTurnaround", "(Isekai Protagonist) [Underdog Protagonist Reversal Charm] \"Take this pouch of Otherworld coin, friend. In my world, stories of blind wanderers surviving in demon cities always lead to the ultimate third-act underdog comeback. Keep your head down and stay alive; the tide in Alushinyrra is about to turn.\"", "{n}The blind halfling gasps, his trembling fingers clutching the heavy pouch of coin to his chest. Behind his cloudy eyes, a sudden spark of long-dead hope flares to life.{/n} \"Otherworld coin... real gold?! Not abyssal lead or cursed slag?! Stranger, you... you speak like someone who actually intends to burn this city to the ground. May whatever lucky star brought you here shield your back from their daggers!\"", 4, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Lucky Drunk";
						c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"A generous hand tossed to a battered soul in the pit of the Abyss. Cayden salutes you!\"</i>";
					});
				});
				AddAlignedAnswer(blueprint11, "IsekaiAct4BadLuckHalflingHero", "(Hero) [Good] [No One is Forgotten] \"Even in the deepest shadow of the Abyss, you are not forgotten. Hold on; light is returning to these broken streets.\"", "{n}The halfling weeps quietly into his cloak, bowing his head in heartfelt prayer.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 4);
			}
			BlueprintAnswersList blueprint12 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("43f93812d6216c94db356622859397f1");
			if (blueprint12 != null)
			{
				AddUniversalAnswer(blueprint12, "IsekaiAct4HerraxHotSprings", "(Isekai Protagonist) [Luxury Hospitality & Bathhouse Inquiries] \"Madame Herrax, you advertise ten thousand delights, but do you have an authentic volcanic hot spring bathhouse, an aromatic cedar sauna, or a karaoke soundstage? True relaxation requires steam and song, not just brooding incubus guards in velvet shadows.\"", "{n}Herrax arches a dark, painted eyebrow, a throaty, purring laugh escaping her lips as she fans herself with jeweled feathers.{/n} \"'Volcanic hot spring bathhouses' and 'cedar saunas'?! What delightfully decadent and refined tastes you possess, mortal traveler! Perhaps the upper planes have far more imaginative pleasures than we credit them for. I may have to commission our incubus artisans to construct a steam bathhouse just for you!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"Demanding luxury bathhouse amenities in the City of Screams! A true vacation connoisseur!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint12, "IsekaiAct4HerraxMastermindIntel", "(Mastermind) [Brothel Information Broker Network] \"I see through the silk veils, madame. Every courtesan is an informational listening post routing secrets to the Upper City. Let's discuss pricing for your archives.\"", "{n}Herrax's smile tightens with professional respect.{/n} \"Sharp eyes, traveler. You understand how true power flows through Alushinyrra.\"", "MastermindProficiencies", 5);
			}
			BlueprintAnswersList blueprint13 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("5ce68965a500a3142a864c7009b56799");
			if (blueprint13 != null)
			{
				AddUniversalAnswer(blueprint13, "IsekaiAct4ChivarroRetention", "(Isekai Protagonist) [Customer Retention Metrics in Alushinyrra] \"Chivarro, murdering high-paying patrons every second Tuesday is terrible for customer lifetime value. In my world, luxury entertainment venues focus on repeat subscriptions, VIP reward tiers, and non-fatal hospitality. Think of the compounding quarterly revenue!\"", "{n}Chivarro stares at you in complete, speechless bewilderment before letting out a sharp, amused hiss.{/n} \"'Quarterly compounding revenue'?! You speak of the flesh trade as though it were an account ledger in a merchant guild! How wonderfully twisted you are, Otherworlder! But I suppose there is logic in not devouring the cows that give the richest milk...\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Dark Prince";
						c.BannerMessage = "<color=#FF4500><b>[The Dark Prince]</b></color>: <i>\"Optimizing customer lifetime value in an abyssal pleasure palace! Exquisite capitalism!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint14 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("c67f95deb80bb324281796413b5d28ae");
			if (blueprint14 != null)
			{
				AddUniversalAnswer(blueprint14, "IsekaiAct4BatteSlaverAutomation", "(Isekai Protagonist) [Economic Invalidation of Chattel Slavery] \"You boast of your slave pens, demon, but your business model is hopelessly obsolete. In advanced civilizations, automated steam engines, hydraulic cranes, and magical assembly lines produce ten thousand times more output at zero feeding and security costs. You aren't a fearsome kingpin; you're an economically illiterate savage trading in meat.\"", "{n}Batte the slaver turns a violent shade of crimson, his tusks clattering in apoplectic fury as he stomps the stone dais.{/n} \"'Economically illiterate'?! 'Automated assembly lines'?! How dare you insult the proud, ancient tradition of abyssal flesh trading, mortal scum! I'll have you chained to the slave pens and sold to the highest bidder for that insolence!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Demolishing the abyssal slave trade through basic macroeconomic theory. Absolutely devastating.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint14, "IsekaiAct4BatteSlaverOverlordScorn", "(Overlord) [Insects in the Dust] \"You trade in screaming cattle and call yourself powerful. Before a true sovereign, your entire bazaar is merely kindling.\"", "{n}Batte recoils instinctively as a crushing dark aura presses down on his horns.{/n} \"What... what kind of monster are you?!\"", "OverlordProficiencies", 5);
			}
			BlueprintAnswersList blueprint15 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("642c54b135c66dd448373875b677a25a");
			if (blueprint15 != null)
			{
				AddUniversalAnswer(blueprint15, "IsekaiAct4MeatSlavesDisgust", "(Isekai Protagonist) [Deadpan Health Inspection Horror] \"Selling humanoid meat in an open-air market under sulfuric ash without refrigeration or health seals? Even for the Abyss, this is disgusting. I'm going to cleanse this whole plaza just so I don't have to smell this stall anymore.\"", "{n}The butcher demon sneers, brandishing his cleaver.{/n} \"Cleanse it?! Come closer and I'll hang you from the meat hooks, stranger!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 300;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Health inspector protocol: purify with righteous steel!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint16 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("67823011274209e4882b60f4b95d103f");
			if (blueprint16 != null)
			{
				AddUniversalAnswer(blueprint16, "IsekaiAct4AasimarSlavesPromise", "(Isekai Protagonist) [Otherworlder's Promise of Liberation] \"Hold on to hope, celestial kin. I crossed the void between dimensions to tear this city's masters from their thrones. Your chains will not hold you for much longer.\"", "{n}The chained Aasimar weep in quiet reverence, their golden halos flickering back to life as your otherworldly presence cuts through the demonic miasma.{/n} \"An Otherworlder... We can feel the starlight in your soul! May the heavens guide your strike!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Dawnflower";
						c.BannerMessage = "<color=#FFD700><b>[The Dawnflower]</b></color>: <i>\"The promise of redemption and freedom shines through the slave pens!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint17 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("dd04ec68821f0ac4db58a756d323d213");
			if (blueprint17 != null)
			{
				AddUniversalAnswer(blueprint17, "IsekaiAct4ArenaHealerSterile", "(Isekai Protagonist) [Field Medicine vs Rusty Bone-Saws] \"Put that rusty serrated bone-saw away, butcher! Where I come from, compound fractures are stabilized with traction splints and sterilized sutures, not hacking off limbs like cheap firewood. Hand me the bandages before you give this gladiator sepsis!\"", "{n}The demon surgeon snorts in bafflement, but steps back as your practiced Otherworld hands reset the wounded gladiator's bone with clean celestial precision.{/n} \"A living splint without an amputation?! Bah! You ruin all the blood and gore, mortal! But the gladiator can fight again tomorrow... Zeklex will be pleased. Take this combat draught!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Modern orthopedic triage saves another combatant from crude butcher surgery.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint17, "IsekaiAct4ArenaHealerSlime", "(Slime) [Living Suture Matrix] \"Biomechanical cellular secretion knits torn muscle tissue in twelve seconds. Observe.\"", "{n}The demon healer stares in utter awe as the gaping wound seals seamlessly.{/n} \"A living biological suture?! Unheard of in the pits!\"", "DevourerProficiencies", 5);
			}
			BlueprintAnswersList blueprint18 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("da29db4239d1c9945ba3440bd1a01141");
			if (blueprint18 != null)
			{
				AddUniversalAnswer(blueprint18, "IsekaiAct4BattleblissEsports", "(Isekai Protagonist) [Esports Tournament Seeding & Bracket Hype] \"Zeklex, your tournament formatting is hopelessly chaotic. In my world, championship arenas use double-elimination brackets, hype intro packages, and broadcast commentary. If you let me seed the grand finals, pay-per-view ticket sales will crash the city gates!\"", "{n}Zeklex rubs his claws together with demonic greed, his eyes glowing with raw spectacle lust.{/n} \"'Double-elimination brackets' and 'hype broadcast commentary'?! Oh, magnificent! Truly the mortal realms produce geniuses of violent commercial entertainment! If you survive the arena, you can produce the next championship season!\"", 5, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Bringing esports tournament production to the blood-pits of the Abyss! Superb!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint19 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("d138954fd7cdb2d4e90bb28cbd76235e");
			if (blueprint19 != null)
			{
				AddUniversalAnswer(blueprint19, "IsekaiAct4ShamiraVillainessMelodrama", "(Isekai Protagonist) [Calling Out Anime Villainess Melodrama] \"Lady Shamira, the heavy velvet drapes, the smoky pipe, the calculated sultry languor... In my home world, you are basically the textbook archetype of the dramatic anime villainess who plots in the shadows. It's a fantastic aesthetic, but let's skip the seduction monologue and talk real terms.\"", "{n}Shamira pauses mid-drag on her long ivory pipe, a stunned, crystalline laugh escaping her lips before she leans back against her silk cushions with genuine, predatory fascination.{/n} \"'Dramatic anime villainess'?! How delightfully insolent! No mortal has ever entered my court and reduced the sublime art of abyssal seduction to a theatrical archetype! You possess an extraordinary, insolent spark, Otherworlder. Wear this imperial signet; anyone with the audacity to mock Lady Shamira to her face deserves to walk Alushinyrra as my personal guest!\"", 6, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 500;
						c.Gold = 1000;
						c.Sponsor = "The World Sovereign";
						c.DirectItems = new BlueprintItem[1] { ItemSovereignsImperialSignet };
						c.BannerMessage = "<color=#FFD700><b>[The World Sovereign]</b></color>: <i>\"Disarming the seductress of the Abyss with unflinching Otherworlder swagger! Outstanding!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint19, "IsekaiAct4ShamiraGodEmperorPoise", "(God Emperor) [Sovereign Radiance] \"Your perfumed court is merely a gilded cage, Shamira. Before true imperial majesty, demonic illusions wither into dust.\"", "{n}Shamira shivers as your golden aura washes over her court, dimming her seductive glamers.{/n} \"What... what kind of power radiates from you?!\"", "GodEmperorProficiencies", 6);
			}
			BlueprintAnswersList blueprint20 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("91097a9b90537a34fbe379d5aa4d9cbe");
			if (blueprint20 == null)
			{
				return;
			}
			AddUniversalAnswer(blueprint20, "IsekaiAct4NexusCyberpunk", "(Isekai Protagonist) [Alushinyrra as Neon Cyberpunk Dystopia] \"Herald, look at those floating islands, neon portals, and cutthroat corporate-style syndicates. This city looks like a dark cyberpunk metropolis where everyone traded their smartphones for poisoned daggers. Don't worry--I've hacked tougher systems than this.\"", "{n}The Hand of the Inheritor tilts his golden helmet, his celestial eyes glowing with calm resolve.{/n} \"A 'cyberpunk metropolis'? Your homeland must be a strange and perilous realm, Commander. But if you have navigated such treacherous labyrinths before, my sword is honored to follow your lead through these shadows.\"", 5, null, delegate(BlueprintAnswer bp)
			{
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 350;
					c.Sponsor = "The Inheritor";
					c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"Unshakable confidence in the belly of the demon realm! Lead on, Commander!\"</i>";
				});
			});
		}

		private static void InjectAct5Encounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("234a43b05125963409b93aded03a5125");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiDrezenReturnRumorMockery", "(Isekai Protagonist) [Protagonist Survival Trope] \"The rumors of my death were greatly exaggerated. It takes far more than the entire Abyss to kill an Otherworlder! Let's get back to winning this war.\"", "{n}Stunned gasps turn into roaring cheers as the citizens and crusaders realize their invincible Commander has returned!{/n}", 6);
				AddMythicAnswer(blueprint, "IsekaiDrezenReturnGodEmperorMajesty", "(God Emperor) [Angel] [The Emperor's Sun Returns] \"I have walked through the depths of hell and brought back victory. Drezen, rise and bask in the radiant dawn of mortal triumph!\"", "{n}Golden banners flutter across the battlements as a massive aura of holy peace descends upon the entire citadel!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Lawful, 7);
				AddSubclassAnswer(blueprint, "IsekaiDrezenReturnMartialGodPoise", "(Martial God) [The Return of the Unconquered] \"The demons of Alushinyrra tested my blade and broke. Anyone who thought Drezen would fall in my absence will now see true mastery.\"", "{n}Crusader veterans stand at rigid attention, awe-struck by your serene, terrifying combat aura.{/n}", "MartialGodProficiencies", 6);
				AddAlignedAnswer(blueprint, "IsekaiDrezenReturnOverlordDominion", "(Overlord) [Iron Dominion Reasserted] \"My absence was merely a brief recess. Drezen belongs to me, and any traitor who wavered will answer for their treason.\"", "{n}The citadel gates tremble as your suffocating aura sweeps through the streets, silencing all dissent.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 6);
				AddAlignedAnswer(blueprint, "IsekaiDrezenReturnMastermindReorganization", "(Mastermind) [Crusade Grid Reboot] \"Logistical deficit: 31%. Morale attrition: 19%. By executing emergency reallocation protocol Alpha-7, Drezen will be at 100% operational readiness in 48 hours.\"", "{n}The quartermasters blink in disbelief as your calculations instantly solve weeks of supply grid chaos!{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 6);
				AddAlignedAnswer(blueprint, "IsekaiDrezenReturnHeroTriumphantEmbrace", "(Hero) [We Survived Together] \"I swore I would return for all of you! Hold your heads high, crusaders; the end of the Worldwound is within our grasp!\"", "{n}Tears of joyful relief flow openly as soldiers rush forward to celebrate your safe return!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 6);
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a53f40ba02af23444b63de6b9337702d");
			if (blueprint2 != null)
			{
				AddUniversalAnswer(blueprint2, "IsekaiCoronationProtagonistSovereignty", "(Isekai Protagonist) [Narrative Climax - The True Sovereign] \"Depose me, Galfrey? You forget that I am the protagonist of this tale. Mortal crowns have no jurisdiction over someone who rewrote the rules of this world!\"", "{n}The royal courtiers exchange frightened, bewildered glances as your words resonate with multiversal certainty!{/n}", 7);
				AddMythicAnswer(blueprint2, "IsekaiCoronationGodEmperorSelfCoronation", "(God Emperor) [Angel] [The Imperial Mandate] \"By divine authority and the unanimous will of the mortal realm, I crown myself Supreme Sovereign of the Holy Mendevian Empire!\"", "{n}A celestial halo descends from the heavens, crowning your brow in blinding solar brilliance as the entire throne room kneels!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Lawful, 8);
				AddSubclassAnswer(blueprint2, "IsekaiCoronationMartialGodEdictSunder", "(Martial God) [Sunder Royal Edict] \"My power was never granted by your crown, Galfrey, and no piece of parchment can strip it from my hands.\"", "{n}With a flick of your fingers, the royal decree is cleanly sliced into microscopic confetti before it even touches the velvet table!{/n}", "MartialGodProficiencies", 7);
				AddMythicAnswer(blueprint2, "IsekaiCoronationOverlordAbsoluteSubmission", "(Overlord) [Lich] [The Living Shall Obey] \"Your reign was weak and indecisive, Galfrey. Kneel before the master of life and death, or your skeleton will serve as my throne's footstool.\"", "{n}Chilling frost coats the walls as the dead whisper your dread name, freezing the royal guards in place!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 8);
				AddAlignedAnswer(blueprint2, "IsekaiCoronationMastermindConstitutionalAudit", "(Mastermind) [Constitutional Nullification] \"Under Article IV of the First Crusade Charter, royal prerogative to dismiss a Knight-Commander during active demonic hostilities requires ratification by three high prelates. Your edict is legally void, Galfrey.\"", "{n}Galfrey stares at the historical statutes in stunned silence, unable to refute your ironclad legal logic!{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 7);
				AddAlignedAnswer(blueprint2, "IsekaiCoronationHeroShieldOfThePeople", "(Hero) [Champion of the People] \"Take the title if you wish, Your Majesty. I never fought for glory or crowns. I fight for the people of Golarion, and I will protect them until the very end!\"", "{n}A murmur of deep respect ripples through the crusader bodyguards, many of them bowing their heads in reverence.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("ec740ac6a039d8c4b83a076cee6c1ff0");
			if (blueprint3 != null)
			{
				AddUniversalAnswer(blueprint3, "IsekaiIzPlagueBombsTriadSalvation", "(Isekai Protagonist) [Dimensional Sprint - Triad Salvation] \"A flash-step faster than sight disarms the primary fuse and teleports all hostages out of the blast radius!\"", "{n}In a blur of supersonic speed, the plague bombs are disarmed while Queen Galfrey and Irabeth are teleported into safety!{/n}", 7, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle<ContextActionIzTriadMultiFrontDefense>();
				});
				AddSubclassAnswer(blueprint3, "IsekaiIzMartialGodKiFlash", "(Martial God) [Supersonic Ki Flash] \"A supersonic step disarms the detonator before sound can travel, slicing Mutasafen's concoction to dust!\"", "{n}Mutasafen blinks, holding a severed fuse in his trembling claws as your party stands victorious.{/n}", "MartialGodProficiencies", 7, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle<ContextActionIzTriadMultiFrontDefense>();
				});
				AddMythicAnswer(blueprint3, "IsekaiIzAngelSolarAegisSave", "(God Emperor) [Angel] [Solar Aegis of the Triad] \"Heaven's divine barrier envelops the Sword of Valor, the Queen, and the library simultaneously! Your foul poisons have no power here!\"", "{n}Blinding golden shields absorb the alchemical detonations without a single spark reaching the hostages!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 8, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle<ContextActionIzTriadMultiFrontDefense>();
				});
				AddAlignedAnswer(blueprint3, "IsekaiIzMastermindTemporalDisplacement", "(Mastermind) [Temporal Phase Displacement] \"Triggering calibrated chronomantic delay. The detonator fuses are shifted 30 seconds into the future, allowing flawless triage evacuation.\"", "{n}The ticking timers freeze in mid-turn as you calmly escort Galfrey and Irabeth to defensive positions!{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 7, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle<ContextActionIzTriadMultiFrontDefense>();
				});
				AddAlignedAnswer(blueprint3, "IsekaiIzHeroUltimateShield", "(Hero) [The Indomitable Shield] \"I will tank the explosion myself! Get the Queen and Irabeth behind me, NOW!\"", "{n}Your heroic barrier absorbs the full brunt of the alchemical shockwave, protecting every soldier without giving an inch!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 7, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle<ContextActionIzTriadMultiFrontDefense>();
				});
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("09b8d5eb5dae3634d9dc3d8c7bdd6e9d");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiTerendelevMemoryResonance", "(Isekai Protagonist) [Soul Resonance: The Scale of Kenabres] \"Terendelev, look upon this radiant scale from Kenabres! You protected this world for centuries; do not let Deskari make you his butcher!\"", "{n}The undead silver dragon pauses in mid-strike, her hollow draconic eyes flaring with ancient sorrow as her soul temporarily reclaims control!{/n}", 7);
				AddMythicAnswer(blueprint4, "IsekaiTerendelevAngelSolarRedemption", "(God Emperor) [Angel] [Heaven's Dragon Rest] \"Guardian of Mendev, your long watch is ended. Return to Heaven's golden skies with honor and dignity!\"", "{n}Golden light dissolves the defiled necromantic stitches, allowing Terendelev's spirit to ascend peacefully toward the heavens.{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 8);
				AddSubclassAnswer(blueprint4, "IsekaiTerendelevMartialGodMercifulRelease", "(Martial God) [The Warrior's Requiem] \"You fought bravely to the bitter end, noble dragon. My blade will grant you a swift, honorable release worthy of a warrior.\"", "{n}A single immaculate cut of pure silver ki severs the abyssal control anchor without desecrating her noble form.{/n}", "MartialGodProficiencies", 7);
				AddMythicAnswer(blueprint4, "IsekaiTerendelevLichDominionClaim", "(Overlord) [Lich] [The Dragon Kneels to the Sovereign] \"Deskari's crude puppetry ends. Terendelev, bow before the true master of undeath and take your rightful place as my personal vanguard!\"", "{n}Pale soul-fire flares in the dragon's eyes as she bows her massive skull in eternal servitude to your dark will!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 8);
				AddAlignedAnswer(blueprint4, "IsekaiTerendelevMastermindRuneSever", "(Mastermind) [Necromantic Matrix Severance] \"Deskari's control frequency is localized in the cranial necro-gem. A targeted disruption dart shatters the crystal instantly.\"", "{n}With pinpoint accuracy, your spell shatters the control jewel, instantly collapsing the demonic reanimation!{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 7);
				AddAlignedAnswer(blueprint4, "IsekaiTerendelevHeroSacredVow", "(Hero) [We Will Avenge You] \"Rest now, Terendelev. We will defeat Deskari and save the world you gave your life to protect. That is my sacred promise!\"", "{n}A gentle silver light bathes the dragon's remains as she rests in eternal peace.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("c6d5321c350bf7c4fa364e34c0ee30a4");
			if (blueprint5 != null)
			{
				AddUniversalAnswer(blueprint5, "IsekaiStoneManuscriptsTemporalStasis", "(Isekai Protagonist) [Temporal Stasis - Rapid Acquisition] \"Time slows as my dimensional grasp snatches the Glass Key and Stone Manuscripts before the flames touch them.\"", "{n}The key and ancient texts phase into your pack in pristine condition, frustrating Anemora's sabotage completely.{/n}", 6);
				AddMythicAnswer(blueprint5, "IsekaiStoneManuscriptsAeonArchivalPreservation", "(Mastermind) [Aeon] [Axiomatic Data Reconstruction] \"Information cannot be destroyed under fundamental cosmic conservation laws. Reconstructing burned historical manuscripts from quantum echo traces.\"", "{n}Burnt ash reverses through the air, reassembling into pristine ancient stone tablets in your hands!{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 7);
				AddSubclassAnswer(blueprint5, "IsekaiStoneManuscriptsMartialGodSpeedSnatch", "(Martial God) [Zero-Point Draw: Archive Retrieval] \"Faster than the ignition of oil, my blade-wind extinguishes the torches while my offhand secures the documents.\"", "{n}A localized vacuum snuffs out every ember in the temple in a single millisecond!{/n}", "MartialGodProficiencies", 6);
				AddAlignedAnswer(blueprint5, "IsekaiStoneManuscriptsGodEmperorSanctum", "(God Emperor) [Solar Preservation] \"No profane flame of the Abyss shall touch this sacred history. By imperial will, this archive is preserved for eternity!\"", "{n}Solar barriers encase every scroll and manuscript, repelling all cultist sabotage.{/n}", AlignmentShiftDirection.Good, "GodEmperorProficiencies", 6);
				AddAlignedAnswer(blueprint5, "IsekaiStoneManuscriptsOverlordTomeHarvest", "(Overlord) [Forbidden Archive Acquisition] \"These ancient secrets belong to my personal library. Anemora, surrender the records or be flayed alive where you stand.\"", "{n}The cultist priestess shivers in terror, clutching the burning books before dropping them frantically at your feet.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 6);
			}
			BlueprintAnswersList blueprint6 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("7d877541fed521c489e6b33845647ad3");
			if (blueprint6 != null)
			{
				AddUniversalAnswer(blueprint6, "IsekaiDeskariPesticideMockery", "(Isekai Protagonist) [Pesticide Protocol: Apex Bug-Zapper] \"Deskari! You took Kenabres with a sneak attack, but today you face an Otherworlder prepared for a raid boss! Time for the ultimate bug zapper!\"", "{n}Deskari shrieks in ear-splitting fury as an electrical grid of multiversal mana fries his forward swarms into charred popcorn!{/n}", 8);
				AddMythicAnswer(blueprint6, "IsekaiDeskariGodEmperorSolarJudgment", "(God Emperor) [Angel] [The Dawn That Burns the Swarm] \"Lord of Locusts! For every drop of innocent blood shed on Golarion, heaven's eternal dawn sentences you to total obliteration!\"", "{n}A thermonuclear pillar of pure solar wrath crashes into Deskari's torso, blinding the battlefield with righteous illumination!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 8);
				AddMythicAnswer(blueprint6, "IsekaiDeskariMartialGodScytheSunder", "(Martial God) [Legend] [The God-Slaying Counter] \"You cleaved Kenabres with your scythe, insect. Now witness a blade that cuts through gods!\"", "{n}A sonic blast of peerless martial force clashes against Deskari's scythe, shearing off its jagged teeth in a storm of black sparks!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 8);
				AddMythicAnswer(blueprint6, "IsekaiDeskariOverlordSwarmUsurpation", "(Overlord) [Demon] [Swarm Usurper] \"You called yourself a Demon Lord, Deskari? You are merely vermin. Your locusts will feast upon your own rotting flesh at my command!\"", "{n}Deskari roars in agony as thousands of his own insects turn upon his carapace, obeying your dominant abyssal will!{/n}", "8e19495ea576a8641964102d177e34b7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 8);
				AddMythicAnswer(blueprint6, "IsekaiDeskariMastermindAeonChitinShatter", "(Mastermind) [Aeon] [Axiomatic Resonant Frequency] \"Your chitin matrix has a resonant shear frequency of 14,200 Hertz. Emitting counter-wave: your carapace dissolves in 3... 2... 1.\"", "{n}Deskari's massive armored exoskeleton shatters into brittle shards under the devastating sonic vibration of cosmic law!{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 8);
				AddMythicAnswer(blueprint6, "IsekaiDeskariHeroAzataDawnOfMankind", "(Hero) [Azata] [The Song of Kenabres Avenged] \"For everyone who lost someone in Kenabres! For every family torn apart! We stand together, and we bring back the dawn!\"", "{n}A glorious chorus of celestial harmonies rings across Iz, filling your companions with unstoppable courage as Deskari recoils in fear!{/n}", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 8);
			}
			BlueprintAnswersList blueprint7 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("20a1be61aa7680e45890656b204e6fe8");
			if (blueprint7 != null)
			{
				AddUniversalAnswer(blueprint7, "IsekaiIzBannerResonantAnchors", "(Isekai Protagonist) [Multiversal Banner Leyline Anchor] \"The Sword of Valor flew high over Drezen, and it will not fall here in Iz! Multiversal mana anchors deployed across the perimeter!\"", "{n}A brilliant resonant pulse surges through the fabric of the banner, repelling abyssal smoke and holding the flag unyielding against the gale!{/n}", 7);
				AddMythicAnswer(blueprint7, "IsekaiIzBannerGodEmperorSolarBastion", "(God Emperor) [Angel] [The Emperor's Sun Banner] \"By holy decree and the mandate of heaven, this sacred standard radiates the invincible dawn of mortal victory!\"", "{n}Blinding golden flames leap along the standard, searing any demon that steps within fifty paces!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Lawful, 8);
				AddSubclassAnswer(blueprint7, "IsekaiIzBannerMartialGodUnbreakableLine", "(Martial God) [The Unbreakable Sentinel] \"Any fiend that wishes to touch this banner must first step through the storm of my blade.\"", "{n}You plant your weapon before the banner, the ground fracturing under your supreme martial pressure.{/n}", "MartialGodProficiencies", 7);
				AddAlignedAnswer(blueprint7, "IsekaiIzBannerOverlordDarkConsecration", "(Overlord) [Consecration of the Dread Banner] \"The martyrs who fell for this flag will not rest. Rise, fallen crusaders, and defend my banner for eternity!\"", "{n}Spectral knights rise from the ash, their ethereal blades saluting your dread majesty as they form an unbreakable perimeter!{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 7);
				AddAlignedAnswer(blueprint7, "IsekaiIzBannerMastermindDefensiveCalculus", "(Mastermind) [Automated Warding Grid] \"Perimeter defensive triangulation complete. Deploying automated abjuration pylons at key choke points.\"", "{n}Interlocking geometric barrier fields snap into place around the banner, deflecting demonic charges with flawless efficiency.{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 7);
				AddAlignedAnswer(blueprint7, "IsekaiIzBannerHeroEternalOath", "(Hero) [The Banner of the Living] \"For everyone who bled for Mendev, we hold this ground! We will not take a single step backward!\"", "{n}A warm, invincible light envelopes the defenders as they shout in roaring defiance against the demon horde!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
			}
			BlueprintAnswersList blueprint8 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a120eec4aaca4d2398cd863e6245a551");
			if (blueprint8 != null)
			{
				AddUniversalAnswer(blueprint8, "IsekaiAlderpashThassilonianLoophole", "(Isekai Protagonist) [Meta-Arcana - Thassilonian Loophole] \"Lord Alderpash, the curse binding you to this sphere relies on a localized spatial contract. With otherworldly mana, I can unweave the anchor while leaving Baphomet's seal intact.\"", "{n}Runelord Alderpash gasps in ecstasy as the planar chains dissolve from his ancient limbs.{/n} \"Free! Free after ten thousand years! Here, reincarnated archmage, take my ancient runic treasures!\"", 8);
				AddSubclassAnswer(blueprint8, "IsekaiAlderpashOverlordTreaty", "(Overlord) [Sovereign Planar Treaty] \"Runelord of Wrath, swear fealty to the Great Tomb, and I shall shatter this celestial cage and grant you dominion over ancient secrets.\"", "{n}Alderpash kneels before your ivory majesty with fanatical obedience.{/n}", "OverlordProficiencies", 8);
				AddMythicAnswer(blueprint8, "IsekaiAlderpashAeonContractAxiom", "(Mastermind) [Aeon] [Axiomatic Sentence Commutation] \"Ten thousand years exceeds the statutory maximum sentence for planar wrath transgressions. By the authority of the cosmic scales, your confinement is officially commuted.\"", "{n}The glowing runes binding the sphere turn crystalline blue and gently dissolve into cosmic starlight.{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 8);
				AddMythicAnswer(blueprint8, "IsekaiAlderpashLegendMartialCageShatter", "(Martial God) [Legend] [Transcendent Sunder: Prison Breaker] \"An unbreakable magical prison? There is no concept in existence that my blade cannot sever. Stand back, old wizard.\"", "{n}With a thunderous strike that shakes the entire labyrinth, the ancient sphere of force shatters into harmless glass shards!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 8);
				AddAlignedAnswer(blueprint8, "IsekaiAlderpashHeroRedemptionCondition", "(Hero) [Freedom with a Conscience] \"I will break this cage, Alderpash, but only if you swear on your ancient honor never to bring war or tyranny to the mortal realms again.\"", "{n}The ancient Runelord bows his head with genuine solemnity.{/n} \"You have my word, young champion. Ten thousand years has taught me the futility of mortal tyrannies.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 8);
			}
			BlueprintAnswersList blueprint9 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6278129d95bb73744a60b07fc6612e45");
			if (blueprint9 != null)
			{
				AddUniversalAnswer(blueprint9, "IsekaiPuluraAct5FinalDisruption", "(Isekai Protagonist) [Checkmate: The Final Save] \"Mutasafen, how many times must I humiliate your research before you realize you cannot outmaneuver an Otherworlder? It's over!\"", "{n}Mutasafen screeches in panicked hysteria as your telekinetic ward snatches his final batch of nahyndrian syringes cleanly from his claws!{/n}", 7);
				AddSubclassAnswer(blueprint9, "IsekaiPuluraAct5MartialGodDecisiveStrike", "(Martial God) [Instant Severance: Escape Intercept] \"You will not crawl into another escape tunnel. One step to close the distance, one stroke to end your experiments forever.\"", "{n}A blinding flash of steel pins Mutasafen to the cavern wall before he can even reach for his escape flask.{/n}", "MartialGodProficiencies", 7);
				AddMythicAnswer(blueprint9, "IsekaiPuluraAct5AngelSolarIncineration", "(God Emperor) [Angel] [Solar Purification of the Blight] \"Your grotesque experiments have desecrated the stars for long enough. Burn in heaven's cleansing dawn!\"", "{n}Golden celestial fire incinerates Mutasafen's research notes and poison vials into harmless white dust!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 7);
				AddAlignedAnswer(blueprint9, "IsekaiPuluraAct5OverlordSoulDissection", "(Overlord) [Soul Extraction] \"Your twisted intellect will serve my grand library. Yield your soul, alchemist, or suffer eternal torment in my soul crucible.\"", "{n}Mutasafen drops to his knees, his soul shivering in dark despair as your shadow claims his essence.{/n}", AlignmentShiftDirection.Evil, "OverlordProficiencies", 7);
				AddAlignedAnswer(blueprint9, "IsekaiPuluraAct5MastermindTriage", "(Mastermind) [Predictive Starlight Lockdown] \"Your escape vector was mapped before we entered the canyon. All subterranean escape tunnels are already collapsed.\"", "{n}Mutasafen frantically claws at the sealed cavern exits, realizing his retreat was doomed from the start.{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 7);
				AddAlignedAnswer(blueprint9, "IsekaiPuluraAct5HeroAegis", "(Hero) [Sanctuary of the Stars] \"Priestesses of Pulura, the nightmare is over! As long as we stand here, no demon will ever harm this sanctuary again!\"", "{n}Tears of profound gratitude fill the priestesses' eyes as your heroic aura restores the peace of the ancient shrine.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
			}
			BlueprintAnswersList blueprint10 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6b8b71866da1eee44bdd1d5d93a2a0f3");
			if (blueprint10 != null)
			{
				AddUniversalAnswer(blueprint10, "IsekaiAct5ChunPaperworkBacklog", "(Isekai Protagonist) [Six Months of Overdue Citadel Invoices] \"Chun, I survived the Abyss, but staring at this towering mountain of six months of backlogged supply requisitions and unpaid blacksmith invoices might actually kill me. Where is the 'mark all as read' button?!\"", "{n}Chun lets out a dry, exhausted chuckle, frantically pushing stacks of parchment aside to uncover an inkwell.{/n} \"A 'mark all as read button'?! Ha! If only the heavens were so merciful, Commander! Half the merchant cartels in Mendev thought you were dead and tried to double-charge for rations! But now that you're back... we'll balance these books in three days flat!\"", 6, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Gold = 1500;
						c.Sponsor = "The Grand Arbiter";
						c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"The true final boss of any crusade: catastrophic bureaucratic backlogs!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint10, "IsekaiAct5ChunMastermindMatrix", "(Mastermind) [Quad-Quadrant Organizational Matrix] \"Color-code the requisitions: green for immediate grain, blue for cold iron, red for royal taxes to be disputed. This backlog clears by nightfall.\"", "{n}Chun stares at the quadrant system in religious awe.{/n} \"It's... it's pure mathematical genius!\"", "MastermindProficiencies", 6);
			}
			BlueprintAnswersList blueprint11 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("234a43b05125963409b93aded03a5125");
			if (blueprint11 != null)
			{
				AddUniversalAnswer(blueprint11, "IsekaiAct5CiarHoldingDrezen", "(Isekai Protagonist) [Commending the Wall Defenders] \"Sir Ciar, you and the Everbright knights held these walls while the Queen took the army to Iz and I was lost in the Abyss. Whatever anyone says, you saved this city from becoming a smoking crater. Outstanding work, knight.\"", "{n}Sir Ciar stands rigid, his scarred jaw clenching as he swallows a surge of hard-won pride, executing a slow, solemn bow of absolute respect.{/n} \"We held our ground because we believed you would return, Commander. When the demons pounded on the lower gate, we reminded each other that as long as the Otherworlder lives, Drezen will not fall. Welcome home.\"", 6, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Inheritor";
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"Honor and unyielding steadfastness rewarded! The bastion stands!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint11, "IsekaiAct5CiarMartialKata", "(Martial God) [Trans-Dimensional Heavy Kata] \"Your defense held, but your footwork is heavy from siege fatigue. Let me show you the seven-fold sweeping counter-cleave.\"", "{n}Sir Ciar practices the fluid motion, his heavy greatsword carving the air with terrifying speed.{/n} \"A devastating counter-strike... Honored by your instruction, Commander.\"", "MartialGodProficiencies", 6);
			}
			BlueprintAnswersList blueprint12 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("46f6b0263c6992f43b33a2fda5518983");
			if (blueprint12 != null)
			{
				AddUniversalAnswer(blueprint12, "IsekaiAct5AneviaDemonManhattan", "(Isekai Protagonist) [Catching Up on Gossip from Demon Manhattan] \"Anevia, give me the debrief: while I was dodging demon kingpins in the Abyss, who made a mess of my city? And more importantly, are you and Irabeth holding up?\"", "{n}Anevia laughs--a warm, tired, deeply relieved laugh as she leans against the map table.{/n} \"'Demon Manhattan'?! Only you could survive the Midnight Isles and treat it like a bad business trip! Beth and I are still breathing, thanks to you. And now that you're back... let's clean up the Queen's mess and finish this war.\"", 6, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 350;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"The reunion of faithful friends brings light back to Drezen!\"</i>";
					});
				});
				AddAlignedAnswer(blueprint12, "IsekaiAct5AneviaHeroRelief", "(Hero) [Good] [Unbroken Bond] \"Seeing you alive and well is the greatest victory I could ask for, Anevia. We'll bring Irabeth back safely from Iz; I swear it.\"", "{n}Anevia nods with tight, grateful determination.{/n} \"I know you will, Commander. Bring her home.\"", AlignmentShiftDirection.Good, "HeroProficiencies", 6);
			}
			BlueprintAnswersList blueprint13 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("c6d5321c350bf7c4fa364e34c0ee30a4");
			if (blueprint13 != null)
			{
				AddUniversalAnswer(blueprint13, "IsekaiAct5AnemoraCurator", "(Isekai Protagonist) [Museum Curator Appraisal of Ancient Sarkoris] \"Anemora, scribbling demonic graffiti over ten-thousand-year-old Sarkorian stone tablets is an unforgivable crime against multiversal archaeology. In my world, museum curators would have you jailed for defacing historical artifacts, even before we execute you for treason!\"", "{n}Anemora shrieks in furious derision, her desiccated hands clawing the stone manuscripts.{/n} \"'Museum curators'?! 'Defacing artifacts'?! Foolish Otherworlder, the glory of Deskari drowns all your petty history in swarms and ash!\"", 6, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Hermit";
						c.BannerMessage = "<color=#9370DB><b>[The Hermit]</b></color>: <i>\"Defending ancient historical relics against abyssal vandalism! A true scholar!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint14 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a120eec4aaca4d2398cd863e6245a551");
			if (blueprint14 != null)
			{
				AddUniversalAnswer(blueprint14, "IsekaiAct5AlderpashAcademic", "(Isekai Protagonist) [Ancient Academic Boredom & Prison Hobbies] \"Runelord Alderpash, spending ten millennia trapped in a demonic maze would drive anyone mad. Back home, long-term prisoners take up wood carving, write ten-volume autobiographies, or do calisthenics. Did you at least invent a new branch of mathematics to pass the time?\"", "{n}The ancient Runelord of Wrath stares at you, his withered face cracking into an incredulous, cackling laugh that echoes off the iron bars.{/n} \"'Wood carving' and 'calisthenics'?! Hahaha! Mortal, you are speaking to the sovereign master of Thassilonian wrath magic, not an indolent convict! But... I did indeed derive forty-seven new runic theorems while waiting for Baphomet to rot! Tell me of your world's magic, stranger!\"", 7, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The Hermit";
						c.BannerMessage = "<color=#9370DB><b>[The Hermit]</b></color>: <i>\"Debating high arcane theory with an ancient Runelord. Superb intellectual parley.\"</i>";
					});
				});
				AddSubclassAnswer(blueprint14, "IsekaiAct5AlderpashMastermind", "(Mastermind) [Thassilonian Runic Axiom Audit] \"Your containment matrix has a fundamental entropy flaw in its eighth anchor. I can collapse this entire cage in three equations.\"", "{n}Alderpash's eyes widen with frantic, desperate greed.{/n} \"Show me! Show me the equations, master of logic!\"", "MastermindProficiencies", 7);
			}
			BlueprintAnswersList blueprint15 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("15abc4d1f76f3cc418b593c33c41dda1");
			if (blueprint15 == null)
			{
				return;
			}
			AddUniversalAnswer(blueprint15, "IsekaiAct5TavernSiegeComfort", "(Isekai Protagonist) [Tavern Defense Reassurance] \"Keep behind the barricade, barkeep. I didn't return from the Abyss just to let demon locusts drink our cold brew. The vanguard is here; stay low and pour the celebration ale once the dust settles.\"", "{n}The terrified barkeep nods frantically, clutching a heavy iron ladle like a warhammer.{/n} \"You heard the Commander! Hold the doors! Pour the victory ale!\"", 6, null, delegate(BlueprintAnswer bp)
			{
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 350;
					c.Sponsor = "The Lucky Drunk";
					c.BannerMessage = "<color=#FFD700><b>[The Lucky Drunk]</b></color>: <i>\"Protecting the sacred taps from abyssal defilement! Charge!\"</i>";
				});
			});
		}

		private static void InjectAct6Encounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("bb3528754c7e741438acef95ec3b6430");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiThresholdArmySeriesFinale", "(Isekai Protagonist) [Series Finale Trope - The Final Dungeon] \"This is it, team: the grand finale! Every arc, every battle, and every choice brought us to this threshold. Let's make this last dungeon run unforgettable!\"", "{n}A roaring cheer shakes the volcanic crags as your companions and the entire allied army draw steel with exhilarating fervor!{/n}", 8);
				AddMythicAnswer(blueprint, "IsekaiThresholdArmyGodEmperorAscent", "(God Emperor) [Angel] [The Dawn of the Holy Empire] \"Soldiers of the Crusade! We stand at the precipice of destiny! By our holy blades and unbreakable resolve, the Worldwound closes today under the righteous sun!\"", "{n}A thermonuclear corona of blinding solar light illuminates the corrupted sky, giving hope to the thousands gathered below!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Lawful, 8);
				AddSubclassAnswer(blueprint, "IsekaiThresholdArmyMartialGodPinnacle", "(Martial God) [The Apex Crucible] \"Beyond this gate lies the ultimate confrontation between dimensions. My blade has trained across realities for this single hour. Follow me into legend!\"", "{n}Your weapon emits a peerless resonant hum that vibrates through the armor of every crusader in the army.{/n}", "MartialGodProficiencies", 8);
				AddMythicAnswer(blueprint, "IsekaiThresholdArmyOverlordGrandDominion", "(Overlord) [Lich] [The Living and the Dead Shall March] \"Let the Abyss tremble. We do not march to defend Golarion; we march to claim sovereign dominion over the rift and bend eternity to our will!\"", "{n}Legions of undead knights and shadow vanguard raise their halberds in chilling, rhythmic salute!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 8);
				AddAlignedAnswer(blueprint, "IsekaiThresholdArmyMastermindPlanarCollapse", "(Mastermind) [Ontological Inversion Calculus] \"The rift coordinates are primed for irreversible collapse. Upon defeating Vorlesh, the Worldwound's geometry will fold into zero dimensional volume.\"", "{n}Your tactical lieutenants nod in grim, admiring confidence at your flawless predictive model.{/n}", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 8);
				AddAlignedAnswer(blueprint, "IsekaiThresholdArmyHeroDawnOfTomorrow", "(Hero) [For Everyone Waiting at Home] \"For everyone we loved and lost, and for the bright dawn that will rise tomorrow: together, we finish this right now!\"", "{n}A wave of tearful, unbreakable courage washes across the allied ranks as tears of hope glisten on every face!{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 8);
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("2b1cc60873a86314c9b81b93635fa430");
			if (blueprint2 != null)
			{
				AddAlignedAnswer(blueprint2, "IsekaiSeelahFarewellHeroBond", "(Hero) [Comrades of the Shield] \"Seelah, your laughter and faith carried us through the darkest trenches of this war. Stand with me one last time, my friend!\"", "{n}Seelah grins through tears, strapping her shield tighter with fierce pride.{/n} \"To the very end, Commander! Drinks on me in Drezen when we walk out of here!\"", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
				AddSubclassAnswer(blueprint2, "IsekaiSeelahFarewellMartialGodRespect", "(Martial God) [Vanguard Respect] \"Keep your shield high, Seelah. When the dust settles, no blade in any world will match the valor of your guard.\"", "{n}Seelah nods firmly, her paladin aura burning steady and bright.{/n} \"Right behind you, Commander. Let's make every strike count!\"", "MartialGodProficiencies", 7);
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("bc23c9f44b93ab9428f62db47ac84b3f");
			if (blueprint3 != null)
			{
				AddAlignedAnswer(blueprint3, "IsekaiRegillFarewellMastermindDiscipline", "(Mastermind) [Exemplary Operational Precision] \"Paralictor Regill, your discipline was the cornerstone of our tactical victory. It has been an honor orchestrating this war alongside you.\"", "{n}Regill salutes with immaculate Hellknight precision, a rare flicker of profound professional respect in his eyes.{/n} \"The Order of the Godclaw could not have engineered a more flawless campaign, Commander. Dismissed to the battlefield.\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 7);
				AddAlignedAnswer(blueprint3, "IsekaiRegillFarewellOverlordIronPact", "(Overlord) [Iron Recognizes Iron] \"You understand what weak hearts deny, Regill: victory demands unyielding resolve. Your service will be honored in my new world order.\"", "{n}Regill meets your dark gaze without flinching.{/n} \"Order is order, Commander. Deliver victory first; history will judge the rest.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 7);
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("234f289beeac48e458189a331295669f");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiWoljifFarewellProtagonistRogue", "(Isekai Protagonist) [From Cell to Sovereign Vanguard] \"From a cellar cage in Kenabres to the final dungeon of the Worldwound! You're the finest shadow rogue an Otherworlder could ever recruit, Woljif.\"", "{n}Woljif wipes his nose on his sleeve, his daggers gleaming with magical shadows.{/n} \"Aw, chief... don't make me get all sentimental! I've got your back, always!\"", 7);
				AddAlignedAnswer(blueprint4, "IsekaiWoljifFarewellHeroFamily", "(Hero) [You Are Family Now] \"You're not that frightened kid alone in the cellar anymore, Woljif. You are a hero of the crusade, and we are family.\"", "{n}Woljif blinks back genuine tears, grinning with newfound, unshakable confidence.{/n} \"Family, huh? Yeah... yeah, I like the sound of that!\"", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("c170936600403124291df84fa2e9e9fa");
			if (blueprint5 != null)
			{
				AddAlignedAnswer(blueprint5, "IsekaiLannFarewellHeroBrotherhood", "(Hero) [Brothers of the Sun] \"Lann, you gave the mongrels a future and gave me an unbreakable brother. We walk out of Threshold under the open sky together.\"", "{n}Lann chuckles, testing his bowstring with steady hands.{/n} \"Under the open sky, chief. Not a bad place for a couple of cavern rats to make history!\"", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
				AddSubclassAnswer(blueprint5, "IsekaiLannFarewellMartialGodArchery", "(Martial God) [The Apex Archer] \"Your arrows have never missed a mark when my back was turned. Keep one shaft nocked for Vorlesh's heart.\"", "{n}Lann smiles with grim warrior satisfaction.{/n} \"Already done, Commander. She won't know which eye to close first.\"", "MartialGodProficiencies", 7);
			}
			BlueprintAnswersList blueprint6 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("945033166e2524a46b3282f15a925139");
			if (blueprint6 != null)
			{
				AddAlignedAnswer(blueprint6, "IsekaiWenduagFarewellOverlordEmpress", "(Overlord) [Consort of the Shadow Sovereign] \"You recognized true power when all others wavered, Wenduag. Rule at my side when this continent is remade in our image.\"", "{n}Wenduag bows low, her arachnid limbs trembling with fierce devotion.{/n} \"My master... my god. Every throat in Threshold will be severed for your glory.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 7);
				AddSubclassAnswer(blueprint6, "IsekaiWenduagFarewellMartialGodApexHunt", "(Martial God) [The Apex Hunt] \"Your predatory instincts are sharpened to perfection, Wenduag. Strike from my shadow and feast upon the hearts of our foes.\"", "{n}Wenduag bares her sharp fangs in a bloodthirsty grin.{/n} \"The hunt ends with their extinction, master!\"", "MartialGodProficiencies", 7);
			}
			BlueprintAnswersList blueprint7 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("8bc697f29600c9446a5d531a2128f4b2");
			if (blueprint7 != null)
			{
				AddAlignedAnswer(blueprint7, "IsekaiArueFarewellHeroDreamBloom", "(Hero) [The Blossom of Desna] \"Arueshalae, you showed the multiverse that a soul is defined by its choices, not its origins. Your wings will carry the dawn into Threshold!\"", "{n}Tears of pure happiness sparkle in Arueshalae's eyes as starlight flutters around her wings.{/n} \"Thank you... with you, I am whole. I will never be afraid again!\"", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
				AddUniversalAnswer(blueprint7, "IsekaiArueFarewellCharacterArcPraise", "(Isekai Protagonist) [Masterpiece Character Arc] \"Look at how far you've come, Arueshalae! From a succubus in the Abyss to the savior of Golarion! That is a 10 out of 10 character arc!\"", "{n}Arueshalae laughs melodically, nocking an arrow of pure starlight.{/n} \"Then let us write the greatest ending this world has ever seen, Commander!\"", 7);
			}
			BlueprintAnswersList blueprint8 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("979f96a0b504daf4297e9a91016ab446");
			if (blueprint8 != null)
			{
				AddAlignedAnswer(blueprint8, "IsekaiEmberFarewellHeroGentleSun", "(Hero) [The Gentle Saint] \"Ember, your kindness made even demons weep. We will end this war so that every child can grow up in a world filled with warmth and love.\"", "{n}Ember smiles her gentle, radiant smile, holding your hand.{/n} \"They are just sad and hurt, but you showed them the way. I'm not afraid at all with you!\"", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
				AddMythicAnswer(blueprint8, "IsekaiEmberFarewellGodEmperorAegis", "(God Emperor) [Angel] [The Saint's Eternal Aegis] \"Little saint, your compassion shines brighter than all the heavens. Walk in safety behind my imperial shield; no evil will touch you.\"", "{n}A golden crown of celestial warmth rests upon Ember's head, shielding her in divine tranquility.{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 8);
			}
			BlueprintAnswersList blueprint9 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("727ea1dcd38772c4089ee5c696de60eb");
			if (blueprint9 != null)
			{
				AddUniversalAnswer(blueprint9, "IsekaiDaeranFarewellMultiversalBanquet", "(Isekai Protagonist) [The Multiversal Banquet Promise] \"Daeran, when we walk out of here alive, you owe me the most decadent, ridiculous banquet in the history of the multiverse.\"", "{n}Daeran pours a goblet of fine wine, raising it in a mocking yet genuine toast.{/n} \"Consider it already funded, my dear Commander. Only the very best vintage for the savior of the world.\"", 7);
				AddAlignedAnswer(blueprint9, "IsekaiDaeranFarewellOverlordAristocracy", "(Overlord) [Privileged Courtier] \"Your sharp tongue amused me through this dull war, Daeran. In my new empire, your debauchery will remain completely unmolested.\"", "{n}Daeran offers an extravagant, theatrical bow.{/n} \"How dreadfully generous, Your Malevolence. I shall prepare the royal suites at once.\"", AlignmentShiftDirection.Evil, "OverlordProficiencies", 7);
			}
			BlueprintAnswersList blueprint10 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("98a6dde2e5e7b2a4abd97a0bc3719a96");
			if (blueprint10 != null)
			{
				AddSubclassAnswer(blueprint10, "IsekaiGreyborFarewellMartialMastery", "(Martial God) [Professional Brotherhood] \"Greybor, a true master of the contract. After this, the tales of our partnership will make your guild famous across multiple dimensions.\"", "{n}Greybor puffs his pipe, nodding with quiet, solemn pride.{/n} \"A contract worthy of a lifetime, Commander. Let's finish the job.\"", "MartialGodProficiencies", 7);
				AddAlignedAnswer(blueprint10, "IsekaiGreyborFarewellMastermindBonus", "(Mastermind) [The Ultimate Completion Bonus] \"All contract performance milestones have been exceeded by 400%, Greybor. The completion payout will dwarf the treasury of Isger.\"", "{n}Greybor grins broadly, securing his twin axes.{/n} \"Music to my ears. You point, I execute.\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 7);
			}
			BlueprintAnswersList blueprint11 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("6b068a702077f1c448d66f64afeb65f7");
			if (blueprint11 != null)
			{
				AddUniversalAnswer(blueprint11, "IsekaiNenioFarewellEncyclopediaPeak", "(Isekai Protagonist) [The Greatest Chapter in Science] \"Nenio, keep that pen moving! This final battle will be the most famous chapter in the entire Encyclopedia Golariana!\"", "{n}Nenio scribbles frantically in her notebook, tail swishing with hyperactive scientific joy!{/n} \"Fascinating! The emotional resonance of the protagonist before final planar annihilation! A landmark monograph!\"", 7);
				AddAlignedAnswer(blueprint11, "IsekaiNenioFarewellMastermindEmpiricalPeer", "(Mastermind) [Empirical Peer Review] \"Your analytical documentation provided essential ontological data, Nenio. Together, we shall publish the ultimate proof of planar mechanics.\"", "{n}Nenio adjusts her glasses with scholarly triumph.{/n} \"An empirical peer of true intellect! Let us conclude our peer review in the blood of Vorlesh!\"", AlignmentShiftDirection.Lawful, "MastermindProficiencies", 7);
			}
			BlueprintAnswersList blueprint12 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("2b6acff363cf61346b319befd6229bb9");
			if (blueprint12 != null)
			{
				AddMythicAnswer(blueprint12, "IsekaiAivuFarewellHeroAzataCookies", "(Hero) [Azata] [Cookies and Sunshine Forever] \"Aivu, my favorite little dragon in the whole multiverse! Let's close this nasty Worldwound so we can eat sweet cookies and fly in the sunshine forever!\"", "{n}Aivu bounces into the air, flapping her butterfly wings in ecstatic happiness!{/n} \"YAY! Cookies! Sunshine! Let's go zap the bad lady and save everyone!\"", "9a3b2c63afa79744cbca46bea0da9a16", "HeroProficiencies", AlignmentShiftDirection.Good, 8);
			}
			BlueprintAnswersList blueprint13 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("90861396a1375684eb5bf894bedecff3");
			if (blueprint13 != null)
			{
				AddUniversalAnswer(blueprint13, "IsekaiAreeluFinalProtagonistParadox", "(Isekai Protagonist) [The Reincarnation Paradox: Unintended Protagonist] \"Areelu Vorlesh, Architect of the Worldwound! You thought your dimensional suture would resurrect your lost child. Instead, you summoned an Otherworlder who rewrote your entire experiment!\"", "{n}Areelu Vorlesh's crystalline mask trembles, her eyes widening in profound, cosmic realization as your otherworldly soul signature outshines her spells!{/n} \"An anomaly... an impossible soul from beyond the Great Beyond! How could my calculations have summoned... YOU?!\"", 8);
				AddMythicAnswer(blueprint13, "IsekaiAreeluFinalGodEmperorSolarJudgment", "(God Emperor) [Angel] [The Final Solar Judication] \"A century of suffering, tears, and desecration ends now, Vorlesh! By the mandate of heaven and mortal sovereignty, your Worldwound is purged forever!\"", "{n}A supernal nova of solar fire engulfs the chamber, turning demonic fleshcraft and abyssal runes into brilliant white starlight!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Good, 8);
				AddMythicAnswer(blueprint13, "IsekaiAreeluFinalMartialGodWorldCleave", "(Martial God) [Legend] [The Blade That Cuts Reality] \"You sought to engineer the ultimate living weapon, Areelu. Look upon me: the Martial God who cuts through destiny itself!\"", "{n}Your blade draws a line of absolute nothingness across the chamber, slicing Areelu's abyssal wards into ribbons before she can finish an incantation!{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 8);
				AddMythicAnswer(blueprint13, "IsekaiAreeluFinalOverlordDominionClaim", "(Overlord) [Lich] [The Great Tomb Claims the Worldwound] \"Your experiments were magnificent, Areelu, but you lacked the grand vision of an Overlord. Your knowledge, your power, and this rift belong to my eternal empire!\"", "{n}Pale soul-vortices encase the altar as the dead whisper your name, subjugating Areelu's life's work into your dark crucible!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 8);
				AddMythicAnswer(blueprint13, "IsekaiAreeluFinalMastermindAeonLoopSever", "(Mastermind) [Aeon] [Axiomatic Loop Severance] \"Your grief created a temporal and planar singularity. I have solved the equation of your child's soul. By cosmic law, the loop is severed and balance is restored.\"", "{n}Geometric arrays of pure sapphire light untangle the tangled soul-threads, releasing the trapped spirits peacefully into the cosmic stream!{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 8);
				AddAlignedAnswer(blueprint13, "IsekaiAreeluFinalHeroCompassionateResolution", "(Hero) [No More Tears, No More Blood] \"I understand the unbearable grief that broke your heart, Areelu. But no love justifies destroying the lives of millions. It ends today: rest your weary heart.\"", "{n}A gentle, warm radiance touches Areelu's face, bringing a single tear of peace to the Architect's sorrowful eyes.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 8);
			}
			BlueprintAnswersList blueprint14 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("f0d89a1e468b4c6f9affc010e8d47596");
			if (blueprint14 != null)
			{
				AddUniversalAnswer(blueprint14, "IsekaiPharasmaReincarnationAuditHumor", "(Isekai Protagonist) [The Reincarnation Audit] \"Lady Pharasma, don't look at me like that! Reincarnating into Golarion wasn't my idea, but you have to admit: I cleaned up your century-old Worldwound problem rather nicely!\"", "{n}The Lady of Graves sits upon her throne of bone and stars, her stern, eternal visage softening with a ghost of an amused, cosmic smile.{/n} \"An extraordinary soul... The tapestry of fate was rewritten by your hand, Otherworlder. Your legend is etched into eternity.\"", 8);
				AddMythicAnswer(blueprint14, "IsekaiPharasmaGodEmperorMultiversalParity", "(God Emperor) [Angel] [Sovereignty Across Eternity] \"You preside over mortal souls, Lady of Graves, but the Supreme Sovereign bows to no single pantheon. Our deeds stand inviolate in the annals of creation!\"", "{n}Golden imperial light surrounds you, standing as an equal radiant power before the throne of death!{/n}", "a5a9fe8f663d701488bd1db8ea40484e", "GodEmperorProficiencies", AlignmentShiftDirection.Lawful, 8);
				AddMythicAnswer(blueprint14, "IsekaiPharasmaMartialGodBladeInviolate", "(Martial God) [Legend] [The Inviolate Blade of Fate] \"Weigh my soul however your scales desire, goddess. My blade carved peace and freedom for an entire world; no celestial judgment can diminish that truth.\"", "{n}The scales of fate tip and balance perfectly as Pharasma silently acknowledges the unconquered warrior before her.{/n}", "3d420403f3e7340499931324640efe96", "MartialGodProficiencies", AlignmentShiftDirection.TrueNeutral, 8);
				AddMythicAnswer(blueprint14, "IsekaiPharasmaOverlordTombAscension", "(Overlord) [Lich] [The New Sovereign of Death] \"Your River of Souls was stagnant, Pharasma. My Great Tomb now stands as a new pillar of eternity, where death obeys my supreme intellect!\"", "{n}The starry court shudders as your dark majesty stands defiant, carving a new kingdom into the cosmic hierarchy!{/n}", "5d501618a28bdc24c80007a5c937dcb7", "OverlordProficiencies", AlignmentShiftDirection.Evil, 8);
				AddMythicAnswer(blueprint14, "IsekaiPharasmaMastermindCosmicReconciliation", "(Mastermind) [Aeon] [Cosmic Audit Complete] \"Submitting finalized planar audit: 100-year abyssal rift closed with 0.0% residual causality error. The multiversal books are balanced, Lady Pharasma.\"", "{n}A crystal scroll of cosmic law unfurls before the throne, stamping the case as permanently resolved with celestial perfection.{/n}", "15a85e67b7d69554cab9ed5830d0268e", "MastermindProficiencies", AlignmentShiftDirection.Lawful, 8);
				AddAlignedAnswer(blueprint14, "IsekaiPharasmaHeroHeartOfKindness", "(Hero) [A Heart That Loved Golarion] \"I only wanted to protect the people who needed me. Whatever realm comes next, I would choose this adventure and these friends a thousand times over!\"", "{n}A warm, gentle light bathes the Spire as Pharasma places a hand upon your head, conferring the eternal blessing of the dawn.{/n}", AlignmentShiftDirection.Good, "HeroProficiencies", 8);
			}
			BlueprintAnswersList blueprint15 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("46e1e280571bd4249af979c38846fe04");
			if (blueprint15 != null)
			{
				AddUniversalAnswer(blueprint15, "IsekaiAct6AneviaIrabethVacation", "(Isekai Protagonist) [The Post-War Hot Spring Vacation Pact] \"Anevia, Irabeth... look how far we've come. From popping broken shinbones in the subterranean rubble of Kenabres to standing together at the gates of Threshold. Once we close this Worldwound, I'm booking you two an all-expenses-paid vacation to a peaceful seaside hot spring. No armor, no demons, no war. Just peace.\"", "{n}Anevia and Irabeth stand side by side, their fingers tightly intertwined. Tears shimmer in Irabeth's eyes as she presses her gauntleted fist against her chest, while Anevia lets out a soft, shaky laugh of overflowing happiness.{/n} \"An 'all-expenses-paid seaside vacation'... Godclaw and Inheritor, that sounds like heaven itself, Commander. We started this journey with you in the dark underground, and we will see it through to the sunrise. Thank you for giving us a future to dream of.\"", 7, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"A sacred promise of peace and love beneath the final sky. Desna's starlight smiles upon you.\"</i>";
					});
				});
				AddAlignedAnswer(blueprint15, "IsekaiAct6AneviaIrabethHeroLove", "(Hero) [Good] [Love as the Ultimate Aegis] \"Your love for each other was the true light that carried us through the dark. When history remembers this crusade, it will remember that hope never failed.\"", "{n}Irabeth nods proudly, her blade glowing with pure celestial fire.{/n} \"For Kenabres, for Mendev, and for our Commander!\"", AlignmentShiftDirection.Good, "HeroProficiencies", 7);
			}
			BlueprintAnswersList blueprint16 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("bc636f62a8c94ab4798b32bba398bebb");
			if (blueprint16 != null)
			{
				AddUniversalAnswer(blueprint16, "IsekaiAct6GalfreyBossRoom", "(Isekai Protagonist) [The Classic Boss Room Preparation] \"Your Majesty, in all the greatest chronicles of my world, the final dungeon always ends the same way: the hero and the queen march shoulder-to-shoulder, the epic finale soundtrack swells to a crescendo, and the ancient evil gets clobbered into dust. Check your potions and buffs; we're wrapping up this campaign in style.\"", "{n}Queen Galfrey lets out a soft, beautiful laugh that echoes across the frozen rocks of Threshold, her royal standard snapping fiercely in the rift wind. Her eyes shine with total, radiant confidence in your vanguard.{/n} \"A 'final dungeon' and an 'epic finale soundtrack'?! Truly, you carry the boundless spirit of another world into our darkest hour, Commander. For a hundred years, Mendev marched toward this gate in dread. But today, standing beside you... I feel only the joy of impending victory. Lead on; let us finish this chronicle together!\"", 7, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 450;
						c.Sponsor = "The Inheritor";
						c.BannerMessage = "<color=#FFD700><b>[The Inheritor]</b></color>: <i>\"The Queen and the Otherworlder march to destiny! The final hour is at hand!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint16, "IsekaiAct6GalfreyGodEmperorPeer", "(God Emperor) [Sovereigns of Humanity] \"Galfrey, you bore the crown of Mendev through a century of torment. Walk with me now not as a desperate monarch, but as a sovereign peer sealing the Abyss forever.\"", "{n}Galfrey bows her head in profound respect, drawing her holy blade beside yours.{/n} \"Under your standard, humanity shall triumph.\"", "GodEmperorProficiencies", 7);
			}
			BlueprintAnswersList blueprint17 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("422a385b41f72094783b676f54aad062");
			if (blueprint17 != null)
			{
				AddUniversalAnswer(blueprint17, "IsekaiAct6NorthernLightsAnimeEnding", "(Isekai Protagonist) [The Grand Finale Skyline] \"Look at those cosmic auroras dancing above the wound... In my home world, this is the exact moment in the anime finale where the orchestral theme reaches its peak. We're about to write the greatest ending this world has ever seen.\"", "{n}Your companions gather around you beneath the swirling emerald and violet lights, looking up into the heavens with wide, awe-struck eyes. A peaceful, solemn warmth settles over the entire crusade vanguard.{/n} \"The finale soundtrack... You really believe we're going to win, don't you? Yes. Standing under these lights with you, Commander, failure is completely unthinkable.\"", 7, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Song of the Spheres";
						c.BannerMessage = "<color=#00FFFF><b>[The Song of the Spheres]</b></color>: <i>\"The stars sing across the heavens as the final curtain rises!\"</i>";
					});
				});
				AddSubclassAnswer(blueprint17, "IsekaiAct6NorthernLightsMartialPrana", "(Martial God) [Atmospheric Prana Gathering] \"The cosmic collision between planar thresholds supercharges the prana of the air. Draw it into your dantian; let every strike carry the weight of a dying star.\"", "{n}Your companions exhale in unison, their weapons humming with intense, shimmering internal energy.{/n} \"The power... It feels limitless!\"", "MartialGodProficiencies", 7);
			}
			BlueprintAnswersList blueprint18 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("cf957e3715b7e3840aaab58a6eb372d0");
			if (blueprint18 != null)
			{
				AddUniversalAnswer(blueprint18, "IsekaiAct6ComeInsideSwagger", "(Isekai Protagonist) [Confident Final Dungeon Entry Swagger] \"Alright, Threshold. You've been the dreaded final dungeon on the map for a hundred years. Let's see what you've got. Time to wrap up season one.\"", "{n}With a confident grin and an effortless stride, you lead your vanguard across the threshold into the heart of the Worldwound, your steps echoing like thunder through the ancient corridors.{/n}", 6, null, delegate(BlueprintAnswer bp)
				{
					bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
					{
						c.Coins = 400;
						c.Sponsor = "The Laughing King";
						c.BannerMessage = "<color=#FFD700><b>[The Laughing King]</b></color>: <i>\"Entering the final dungeon like a true protagonist! Show them how it's done!\"</i>";
					});
				});
			}
			BlueprintAnswersList blueprint19 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("bb3528754c7e741438acef95ec3b6430");
			if (blueprint19 == null)
			{
				return;
			}
			AddUniversalAnswer(blueprint19, "IsekaiAct6AreeluStaging", "(Isekai Protagonist) [Critique of Dramatic Monologue Staging] \"Areelu, the dramatic shadow lighting and standing at the top of the grand marble stairs with your cape spreading out? Be honest: how many times did you rehearse this entrance in front of a scrying mirror? It's a solid nine out of ten for theatrical presence!\"", "{n}Areelu Vorlesh pauses mid-incantation, her piercing, otherworldly eyes narrowing as a faint, almost imperceptible twitch of a wry smirk touches the corner of her lips.{/n} \"'Rehearsed in a mirror'?! You never cease to amaze me, child. Even standing before the Architect of the Worldwound at the edge of oblivion, your thoughts wander to the staging of the theater. Perhaps that irrepressible spirit is why my experiment exceeded every calculation. Ascend, then; let us conclude our drama.\"", 7, null, delegate(BlueprintAnswer bp)
			{
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionGiveOtherworlderRewards c)
				{
					c.Coins = 500;
					c.Sponsor = "The Grand Arbiter";
					c.BannerMessage = "<color=#00FFFF><b>[The Grand Arbiter]</b></color>: <i>\"Mocking the Architect's final monologue staging to her face! Absolute legendary audacity!\"</i>";
				});
			});
		}

		private static void InjectDlcEncounters()
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>("b686f80a62f24ca08899e558b96c7a1b");
			if (blueprint != null)
			{
				AddUniversalAnswer(blueprint, "IsekaiDlc1AxiomaticDebug", "(Isekai Protagonist) [Debug Mode - Axiomatic Code Injection] \"Your clockwork simulation has an unhandled null pointer exception in thread 0x7F. Let me patch your axioms before your gears melt.\"", "{n}Valmallos's mechanical processors freeze in astonishment as the simulation stabilizes with zero glitches.{/n} \"Anomalous code accepted. Inevitable protocol updated.\"", 7);
			}
			BlueprintAnswersList blueprint2 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("aac49e0c7220417c89bde055dd698ebb");
			if (blueprint2 != null)
			{
				AddUniversalAnswer(blueprint2, "IsekaiDlc3PirateKingTrope", "(Isekai Protagonist) [Pirate King Trope - Charting the Outer Dark] \"Steersman, your compass points to Nahyndri's ancient curse. My coordinates point to infinite plunder across the stars. Set sail under my command!\"", "{n}The Mad Steersman slowly nods his hooded skull, turning the abyssal wheel with eager speed.{/n} \"Aye, Captain... to the treasures of the void!\"", 6);
			}
			BlueprintAnswersList blueprint3 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("57929455cd9546d28fc159656bf41eda");
			if (blueprint3 != null)
			{
				AddUniversalAnswer(blueprint3, "IsekaiDlc4SpiritCleansing", "(Isekai Protagonist) [Shamanic Otherworlder Attunement] \"Ancient spirits of Sarkoris, hear the voice of one who walked between the stars! Let the abyssal corruption slough away from your roots!\"", "{n}The ancient grove radiates pure celestial light as the demonic blight withers to dust. Ulbrig Olesk stares in reverent awe.{/n} \"By the spirits... you healed the ancient forest!\"", 6);
			}
			BlueprintAnswersList blueprint4 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("0cbb2e81134942d2aac2cf6312072c80");
			if (blueprint4 != null)
			{
				AddUniversalAnswer(blueprint4, "IsekaiDlc5AbsoluteZeroRegulation", "(Isekai Protagonist) [Thermal Mana Regulation - Absolute Zero Resistance] \"You think this frozen void chills me? In my previous world, thermodynamic entropy is merely atomic vibration at 0 Kelvin.\"", "{n}A thermal aura envelops your companions, completely shielding them from the biting frost.{/n}", 7);
			}
			BlueprintAnswersList blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("470d79f6ad25457582490fd9cddb712e");
			if (blueprint5 != null)
			{
				AddUniversalAnswer(blueprint5, "IsekaiDlc6MainCharacterRecognition", "(Isekai Protagonist) [Main Character Recognition Protocol] \"You hired cheap circus performers to impersonate ME? Their posture is atrocious, their wigs are falling off, and their battle aura is zero out of ten.\"", "{n}The festival crowd erupts in deafening laughter, throwing tomatoes at the humiliated Razmiran impostors!{/n}", 5);
			}
			BlueprintAnswersList blueprint6 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a9f9c6440ea049c8aab718a18eab74cb");
			if (blueprint6 != null)
			{
				AddUniversalAnswer(blueprint6, "IsekaiDlc6ArenaFinalFlex", "(Isekai Protagonist) [The Final Flex: Arena Sweep] \"The announcer introduced you as an invincible legend. In my world, you are the mid-season boss who exists solely to showcase my new transformation.\"", "{n}The arena explodes into thunderous cheers as you effortlessly parry the champion's ultimate technique with two fingers, sending him crashing into the wall!{/n}", 8);
			}
		}
	}
}
