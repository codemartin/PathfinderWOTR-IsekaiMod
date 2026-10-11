using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Loot;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

public static class CosmicRelicsVendorTable
{
	private static readonly Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");

	private static readonly Sprite Icon_Pact = BlueprintTools.GetBlueprint<BlueprintItem>("0fc18f15c5ec15042a972778cefee17c")?.m_Icon ?? Icon_Coin;

	private static readonly Sprite Icon_DragonHorn = BlueprintTools.GetBlueprint<BlueprintItem>("3bf029b9fdce9cb40b62d888db5dbadb")?.m_Icon ?? Icon_Coin;

	public static BlueprintSharedVendorTable Table;

	public static BlueprintItemEquipmentHead CosmicCrownOfApotheosisItem;

	public static BlueprintItemEquipmentRing CosmicRingOfOmnipresenceItem;

	public static BlueprintItemEquipmentUsable CosmicMythicPactItem;

	public static BlueprintItemEquipmentUsable CosmicAstralDragonHornItem;

	public static void Add()
	{
		CreateRelicItems();
		Table = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicRelicsVendorTable", delegate(BlueprintSharedVendorTable bp)
		{
			bp.AutoIdentifyAllItems = true;
			bp.ComponentsArray = new BlueprintComponent[0];
		});
		Dictionary<string, BlueprintItem> dictionary = new Dictionary<string, BlueprintItem>
		{
			{ "CosmicCrownOfApotheosisItem", CosmicCrownOfApotheosisItem },
			{ "CosmicRingOfOmnipresenceItem", CosmicRingOfOmnipresenceItem },
			{ "CosmicMythicPactItem", CosmicMythicPactItem },
			{ "CosmicAstralDragonHornItem", CosmicAstralDragonHornItem },
			{
				"ItemSkillBookGrandScholar",
				Appraisal.ItemSkillBookGrandScholar
			},
			{
				"ItemSkillBookPhantomInfiltrator",
				Appraisal.ItemSkillBookPhantomInfiltrator
			},
			{
				"ItemSkillBookAbsoluteCharisma",
				Appraisal.ItemSkillBookAbsoluteCharisma
			},
			{
				"ItemSkillBookWindWalker",
				Appraisal.ItemSkillBookWindWalker
			},
			{
				"ItemSkillBookEyeOfProvidence",
				Appraisal.ItemSkillBookEyeOfProvidence
			},
			{
				"ItemSkillBookTechnicLeague",
				Appraisal.ItemSkillBookTechnicLeague
			}
		};
		foreach (KeyValuePair<string, BlueprintItem> item in CosmicConsumables.Items)
		{
			dictionary[item.Key] = item.Value;
		}
		CosmicEnvoyPrices.Entry[] table = CosmicEnvoyPrices.Table;
		foreach (CosmicEnvoyPrices.Entry entry in table)
		{
			BlueprintItem value;
			if (entry.Guid != null)
			{
				AddPackByGuid(Table, entry.Guid, entry.Stock);
			}
			else if (dictionary.TryGetValue(entry.Name, out value))
			{
				AddPack(Table, value, entry.Stock);
			}
			else
			{
				Main.IsekaiContext.Logger.LogError("[CosmicRelicsVendorTable] Price table names an unknown item: " + entry.Name);
			}
		}
		Main.IsekaiContext.Logger.Log("[CosmicRelicsVendorTable] Cosmic Relics vendor table successfully assembled.");
	}

	private static void CreateRelicItems()
	{
		BlueprintFeature crownItemFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicCrownOfApotheosisItemFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Crown of Apotheosis");
			bp.SetDescription(Main.IsekaiContext, "An astral crown forged from condensed stellar supernovas.\nGrants a +6 enhancement bonus to Intelligence, Wisdom, and Charisma, a +2 insight bonus to all spell save DCs, and spell resistance 10.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			CosmicRelics.AddCrownComponents(bp);
		});
		BlueprintFeature ringItemFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicRingOfOmnipresenceItemFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Ring of Omnipresence");
			bp.SetDescription(Main.IsekaiContext, "A cosmic ring that allows its wearer to slip through the seams of physical reality.\nGrants permanent immunity to paralysis and to being held in place, a +5 deflection bonus to Armor Class, and a +5 sacred bonus to all saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = false;
			CosmicRelics.AddRingComponents(bp);
		});
		BlueprintItemEquipmentHead baseHead = BlueprintTools.GetBlueprint<BlueprintItemEquipmentHead>("a3e8e907908ca7a40ac6da78e70bf33d");
		CosmicCrownOfApotheosisItem = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicCrownOfApotheosisItem", delegate(BlueprintItemEquipmentHead bp)
		{
			bp.SetName(Main.IsekaiContext, "Crown of Apotheosis");
			bp.SetDescription(Main.IsekaiContext, "An astral crown forged from condensed stellar supernovas.\nGrants a +6 enhancement bonus to Intelligence, Wisdom, and Charisma, a +2 insight bonus to all spell save DCs, and spell resistance 10.");
			if (baseHead != null)
			{
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseHead).m_Icon;
			}
			((BlueprintItem)bp).m_Cost = CosmicEnvoyPrices.GoldValue(CosmicEnvoyPrices.Lookup("CosmicCrownOfApotheosisItem", null).GetValueOrDefault());
			((BlueprintItem)bp).m_Weight = 1f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.AddComponent(delegate(AddFactToEquipmentWielder c)
			{
				c.m_Fact = crownItemFeature.ToReference<BlueprintUnitFactReference>();
			});
		});
		BlueprintItemEquipmentRing baseRing = BlueprintTools.GetBlueprint<BlueprintItemEquipmentRing>("f333bf86cd122974792162cbcd27c9ed");
		CosmicRingOfOmnipresenceItem = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicRingOfOmnipresenceItem", delegate(BlueprintItemEquipmentRing bp)
		{
			bp.SetName(Main.IsekaiContext, "Ring of Omnipresence");
			bp.SetDescription(Main.IsekaiContext, "A cosmic ring that allows its wearer to slip through the seams of physical reality.\nGrants permanent immunity to paralysis and to being held in place, a +5 deflection bonus to Armor Class, and a +5 sacred bonus to all saving throws.");
			if (baseRing != null)
			{
				((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseRing).m_Icon;
			}
			((BlueprintItem)bp).m_Cost = CosmicEnvoyPrices.GoldValue(CosmicEnvoyPrices.Lookup("CosmicRingOfOmnipresenceItem", null).GetValueOrDefault());
			((BlueprintItem)bp).m_Weight = 0.1f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.AddComponent(delegate(AddFactToEquipmentWielder c)
			{
				c.m_Fact = ringItemFeature.ToReference<BlueprintUnitFactReference>();
			});
		});
		BlueprintAbility pactAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicMythicPactAbility", delegate(BlueprintAbility ab)
		{
			ab.SetName(Main.IsekaiContext, "Rulebreaker Mythic Pact");
			ab.SetDescription(Main.IsekaiContext, "Consumes the cosmic pact to permanently awaken absolute caster and martial authority.");
			((BlueprintUnitFact)ab).m_Icon = Icon_Pact;
			ab.Type = AbilityType.Special;
			ab.Range = AbilityRange.Personal;
			ab.CanTargetSelf = true;
			ab.ActionType = UnitCommand.CommandType.Standard;
			ab.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			ab.AddComponent(delegate(AbilityCasterHasNoFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { CosmicRelics.CosmicMythicPactFeature.ToReference<BlueprintUnitFactReference>() };
			});
			ab.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionConsumeSkillBook
				{
					m_Feature = CosmicRelics.CosmicMythicPactFeature.ToReference<BlueprintFeatureReference>(),
					BookName = "Rulebreaker Mythic Pact"
				});
			});
		});
		CosmicMythicPactItem = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicMythicPactItem", delegate(BlueprintItemEquipmentUsable bp)
		{
			bp.SetName(Main.IsekaiContext, "Rulebreaker Mythic Pact");
			bp.SetDescription(Main.IsekaiContext, "You have purchased the absolute authority of the cosmos to dictate your own destiny.\nPermanently grants a +3 bonus to your Caster Level for all spells, a +4 sacred bonus to all attack rolls, and +4 sacred bonus to all damage rolls.");
			((BlueprintItem)bp).m_Icon = Icon_Pact;
			((BlueprintItem)bp).m_Cost = CosmicEnvoyPrices.GoldValue(CosmicEnvoyPrices.Lookup("CosmicMythicPactItem", null).GetValueOrDefault());
			((BlueprintItem)bp).m_Weight = 1f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.Type = UsableItemType.Other;
			bp.SpendCharges = true;
			bp.Charges = 1;
			bp.RestoreChargesOnRest = false;
			((BlueprintItemEquipment)bp).m_Ability = pactAbility.ToReference<BlueprintAbilityReference>();
		});
		BlueprintAbility dragonHornAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicAstralDragonHornAbility", delegate(BlueprintAbility ab)
		{
			ab.SetName(Main.IsekaiContext, "Astral Sovereign Dragon Horn");
			ab.SetDescription(Main.IsekaiContext, "Blow the horn to permanently unlock the daily summon of the Astral Sovereign Dragon, an ancient silver dragon.");
			((BlueprintUnitFact)ab).m_Icon = Icon_DragonHorn;
			ab.Type = AbilityType.Special;
			ab.Range = AbilityRange.Personal;
			ab.CanTargetSelf = true;
			ab.ActionType = UnitCommand.CommandType.Standard;
			ab.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			ab.AddComponent(delegate(AbilityCasterHasNoFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { CosmicRelics.CosmicAstralDragonFeature.ToReference<BlueprintUnitFactReference>() };
			});
			ab.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionConsumeSkillBook
				{
					m_Feature = CosmicRelics.CosmicAstralDragonFeature.ToReference<BlueprintFeatureReference>(),
					BookName = "Astral Sovereign Dragon Horn"
				});
			});
		});
		CosmicAstralDragonHornItem = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicAstralDragonHornItem", delegate(BlueprintItemEquipmentUsable bp)
		{
			bp.SetName(Main.IsekaiContext, "Astral Sovereign Dragon Horn");
			bp.SetDescription(Main.IsekaiContext, "A carved iridescent horn from an astral leviathan.\nPermanently unlocks the daily summoning of the Astral Sovereign Dragon, an ancient silver dragon, for 1 hour.");
			((BlueprintItem)bp).m_Icon = Icon_DragonHorn;
			((BlueprintItem)bp).m_Cost = CosmicEnvoyPrices.GoldValue(CosmicEnvoyPrices.Lookup("CosmicAstralDragonHornItem", null).GetValueOrDefault());
			((BlueprintItem)bp).m_Weight = 2f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.Type = UsableItemType.Other;
			bp.SpendCharges = true;
			bp.Charges = 1;
			bp.RestoreChargesOnRest = false;
			((BlueprintItemEquipment)bp).m_Ability = dragonHornAbility.ToReference<BlueprintAbilityReference>();
		});
	}

	private static void AddPack(BlueprintSharedVendorTable table, BlueprintItem item, int count)
	{
		if (table != null && item != null)
		{
			table.AddComponent(delegate(LootItemsPackFixed c)
			{
				c.m_Item = new LootItem
				{
					m_Type = LootItemType.Item,
					m_Item = item.ToReference<BlueprintItemReference>()
				};
				c.m_Count = count;
			});
		}
	}

	private static void AddPackByGuid(BlueprintSharedVendorTable table, string itemGuid, int count)
	{
		if (table != null)
		{
			BlueprintItem blueprint = BlueprintTools.GetBlueprint<BlueprintItem>(itemGuid);
			if (blueprint != null)
			{
				AddPack(table, blueprint, count);
			}
		}
	}
}
