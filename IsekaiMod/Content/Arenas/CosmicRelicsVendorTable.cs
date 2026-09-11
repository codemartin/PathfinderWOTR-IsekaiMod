using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Loot;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Arenas
{
	public static class CosmicRelicsVendorTable
	{
		private static readonly Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");

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
			AddPack(Table, CosmicCrownOfApotheosisItem, 1);
			AddPack(Table, CosmicRingOfOmnipresenceItem, 1);
			AddPack(Table, CosmicMythicPactItem, 1);
			AddPack(Table, CosmicAstralDragonHornItem, 1);
			if (Appraisal.ItemSkillBookGrandScholar != null)
			{
				AddPack(Table, Appraisal.ItemSkillBookGrandScholar, 1);
			}
			if (Appraisal.ItemSkillBookPhantomInfiltrator != null)
			{
				AddPack(Table, Appraisal.ItemSkillBookPhantomInfiltrator, 1);
			}
			if (Appraisal.ItemSkillBookAbsoluteCharisma != null)
			{
				AddPack(Table, Appraisal.ItemSkillBookAbsoluteCharisma, 1);
			}
			if (Appraisal.ItemSkillBookWindWalker != null)
			{
				AddPack(Table, Appraisal.ItemSkillBookWindWalker, 1);
			}
			if (Appraisal.ItemSkillBookEyeOfProvidence != null)
			{
				AddPack(Table, Appraisal.ItemSkillBookEyeOfProvidence, 1);
			}
			if (Appraisal.ItemSkillBookTechnicLeague != null)
			{
				AddPack(Table, Appraisal.ItemSkillBookTechnicLeague, 1);
			}
			AddPackByGuid(Table, "7a7b1dfa67aa4544aa468d0558b1f667", 1);
			AddPackByGuid(Table, "b8ea6d2f787c7004c9cab9f519f687f8", 1);
			AddPackByGuid(Table, "2d4d510a69da09c48893f945ec197210", 1);
			AddPackByGuid(Table, "3584c2a2f8b5b1b43ae11128f0ff1583", 1);
			AddPackByGuid(Table, "419a486154514594c99193da785d4302", 1);
			AddPackByGuid(Table, "37e2f09923a96234ca486bc9db0b6ad6", 1);
			AddPackByGuid(Table, "d0b7d29c9bea99d4bb25f8f6a29261c5", 1);
			AddPackByGuid(Table, "843ae85d505be8441b9fbb47b04e19e0", 1);
			AddPackByGuid(Table, "f7887d35eab0a8144b7e6c29794b0865", 1);
			AddPackByGuid(Table, "1bf7ae3382d3472e956f691da66598cf", 1);
			AddPackByGuid(Table, "81d504243708f504dbfe3f8f72efdeda", 1);
			AddPackByGuid(Table, "9bab0e37c72be78418516e57a5e78a99", 1);
			Main.IsekaiContext.Logger.Log("[CosmicRelicsVendorTable] Cosmic Relics vendor table successfully assembled.");
		}

		private static void CreateRelicItems()
		{
			BlueprintItemEquipmentHead baseHead = BlueprintTools.GetBlueprint<BlueprintItemEquipmentHead>("a3e8e907908ca7a40ac6da78e70bf33d");
			CosmicCrownOfApotheosisItem = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicCrownOfApotheosisItem", delegate(BlueprintItemEquipmentHead bp)
			{
				bp.SetName(Main.IsekaiContext, "Crown of Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "An astral crown forged from condensed stellar supernovas.\nGrants a +6 enhancement bonus to Intelligence, Wisdom, and Charisma, a +4 sacred bonus to all spell save DCs, and +10 Spell Resistance.");
				if (baseHead != null)
				{
					((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseHead).m_Icon;
				}
				((BlueprintItem)bp).m_Cost = 4000;
				((BlueprintItem)bp).m_Weight = 1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = CosmicRelics.CosmicCrownOfApotheosisFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
			BlueprintItemEquipmentRing baseRing = BlueprintTools.GetBlueprint<BlueprintItemEquipmentRing>("f333bf86cd122974792162cbcd27c9ed");
			CosmicRingOfOmnipresenceItem = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicRingOfOmnipresenceItem", delegate(BlueprintItemEquipmentRing bp)
			{
				bp.SetName(Main.IsekaiContext, "Ring of Omnipresence");
				bp.SetDescription(Main.IsekaiContext, "A cosmic ring that allows its wearer to slip through the seams of physical reality.\nGrants permanent Freedom of Movement, a +5 deflection bonus to Armor Class, and a +5 sacred bonus to all saving throws.");
				if (baseRing != null)
				{
					((BlueprintItem)bp).m_Icon = ((BlueprintItem)baseRing).m_Icon;
				}
				((BlueprintItem)bp).m_Cost = 3500;
				((BlueprintItem)bp).m_Weight = 0.1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.AddComponent(delegate(AddFactToEquipmentWielder c)
				{
					c.m_Fact = CosmicRelics.CosmicRingOfOmnipresenceFeature.ToReference<BlueprintUnitFactReference>();
				});
			});
			BlueprintAbility pactAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicMythicPactAbility", delegate(BlueprintAbility ab)
			{
				ab.SetName(Main.IsekaiContext, "Rulebreaker Mythic Pact");
				ab.SetDescription(Main.IsekaiContext, "Consumes the cosmic pact to permanently awaken absolute caster and martial authority.");
				((BlueprintUnitFact)ab).m_Icon = Icon_Coin;
				ab.Type = AbilityType.Special;
				ab.Range = AbilityRange.Personal;
				ab.CanTargetSelf = true;
				ab.ActionType = UnitCommand.CommandType.Standard;
				ab.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
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
				((BlueprintItem)bp).m_Icon = Icon_Coin;
				((BlueprintItem)bp).m_Cost = 4500;
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
				ab.SetDescription(Main.IsekaiContext, "Blow the horn to permanently unlock the daily summon of an Ancient Silver Sovereign Dragon.");
				((BlueprintUnitFact)ab).m_Icon = Icon_Coin;
				ab.Type = AbilityType.Special;
				ab.Range = AbilityRange.Personal;
				ab.CanTargetSelf = true;
				ab.ActionType = UnitCommand.CommandType.Standard;
				ab.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
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
				bp.SetDescription(Main.IsekaiContext, "A carved iridescent horn from an astral leviathan.\nPermanently unlocks the daily summoning of an Ancient Silver Sovereign Dragon for 1 hour.");
				((BlueprintItem)bp).m_Icon = Icon_Coin;
				((BlueprintItem)bp).m_Cost = 5500;
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
}
