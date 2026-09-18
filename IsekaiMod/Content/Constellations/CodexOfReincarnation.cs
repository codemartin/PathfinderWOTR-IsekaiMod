using System;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations
{
	public static class CodexOfReincarnation
	{
		public static BlueprintItemEquipmentUsable ItemCodexOfReincarnation;

		public static BlueprintBuff CodexKnowledgeBuff;

		public static BlueprintAbility CodexAbility;

		public static void Add()
		{
			Sprite sprite = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			Sprite iconBook = ((BlueprintItem)BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("289842de01a049249f0e921c18bd91a4"))?.m_Icon ?? sprite;
			CodexKnowledgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "CodexKnowledgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Chronicle Insight");
				bp.SetDescription(Main.IsekaiContext, "Attunement with the Codex of the Reincarnated Otherworlder grants a +2 insight bonus to all Knowledge and Lore checks, as well as a +1 sacred bonus on Will saving throws.");
				((BlueprintUnitFact)bp).m_Icon = iconBook;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillKnowledgeArcana;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillLoreReligion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillLoreNature;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 1;
				});
			});
			CodexAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "CodexOfReincarnationAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Read Codex of the Reincarnated Otherworlder");
				bp.SetDescription(Main.IsekaiContext, "Read the chronicle of the stars to attune your soul to the multiverse, refreshing your insight and receiving your initial celestial stipend of 500 Cosmic Coins from The Grand Arbiter.");
				((BlueprintUnitFact)bp).m_Icon = iconBook;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.CanTargetPoint = false;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle<ContextActionReadCodex>();
				});
			});
			string codexTitle = "Codex of the Reincarnated Otherworlder: The 13 Constellations & The Cosmic Tapestry";
			string codexDescription = "A magnificent starlight-bound grimoire radiating multiversal resonance.\nWhen used, attunes your soul to the cosmos, permanently unlocking celestial chronicle lore and granting +2 to all Knowledge and Lore skills and +1 sacred bonus to Will saves for 24 hours.\nUpon first reading, The Grand Arbiter awards you 500 Cosmic Coins as an initial celestial stipend!\n\n--- CHAPTER I: THE 13 CONSTELLATIONS & THEIR DOMAINS ---\n1. The Laughing King (The Lantern King): Chaos, jokes, meta humor, twists.\n2. The Lucky Drunk (Cayden Cailean): Daring gambles, brawls, toasts, swagger.\n3. The Inheritor (Iomedae): Righteous honor, duty, crusade discipline.\n4. The Prince of Darkness (Asmodeus): Tyranny, contracts, sovereign order.\n5. The Song of the Spheres (Desna): Freedom, dreams, travel, hope.\n6. The Lady of Graves (Pharasma): Fate, death, soul cycles, solemn judgment.\n7. The Savored Sting (Calistria): Lust, revenge, audacity, intrigue.\n8. The All-Seeing Eye (Nethys): Arcane supremacy, reckless magic, knowledge.\n9. Our Lord in Iron (Gorum): Pure martial carnage, brute strength, warfare.\n10. The Pirate Queen (Besmara): Plunder, freedom on the high seas, defiance.\n11. The Roseguard Sentinel (Milani): Revolution, uprisings against tyrants.\n12. The Grand Arbiter: Maintains the fair balance and order of the celestial gallery.\n13. The Lurker at the Threshold: The enigmatic keeper of dimensional doors.\n\n--- CHAPTER II: THE CELESTIAL BROADCAST & COSMIC COINS ---\nWhenever you take bold Otherworlder actions, resolve dilemmas creatively, or unleash Overpowered Abilities, the watching Constellations react in the celestial log and sponsor you with Cosmic Coins.\nAll coins pool into a single universal wallet accepted at the Divine Sponsorship Store, where you can purchase mythic relics, stat tomes, and divine aids.\n\n--- CHAPTER III: SEVERING THE KARMIC THREAD ---\nShould you ever desire a clean slate to begin an untainted Loop 1 journey, you may sever the karmic timeline to reset all cycle records and reincarnate anew.";
			ItemCodexOfReincarnation = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemCodexOfReincarnation", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, codexTitle);
				bp.SetDescription(Main.IsekaiContext, codexDescription);
				((BlueprintItem)bp).m_Icon = iconBook;
				((BlueprintItem)bp).m_Cost = 5000;
				((BlueprintItem)bp).m_Weight = 1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.Type = UsableItemType.Other;
				bp.SpendCharges = false;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = true;
				((BlueprintItemEquipment)bp).m_Ability = CodexAbility.ToReference<BlueprintAbilityReference>();
			});
		}

		public static void SeverKarmicTimeline()
		{
			TimelineManager.ResetTimeline();
		}

		public static void EnsureCodexDelivered()
		{
			try
			{
				Player player = Game.Instance?.Player;
				if (player?.Inventory != null && ItemCodexOfReincarnation != null && !player.Inventory.Contains(ItemCodexOfReincarnation))
				{
					player.Inventory.Add(ItemCodexOfReincarnation, 1);
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#FFD700><b>================================================================================</b></color>\n<color=#00FFFF><b>[OTHERWORLDER ONBOARDING INITIALIZED]</b></color>\nYou have received the <b>Codex of the Reincarnated Otherworlder</b>! Inspect this tome in your inventory to understand the 13 Constellations, your celestial audience, and the rules of Cosmic Coins.\n<color=#FFD700><b>================================================================================</b></color>");
					});
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in CodexOfReincarnation.EnsureCodexDelivered: " + ex);
			}
		}
	}
}
