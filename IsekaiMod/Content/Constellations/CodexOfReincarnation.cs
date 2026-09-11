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
			Sprite iconCoin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			CodexKnowledgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "CodexKnowledgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Chronicle Insight");
				bp.SetDescription(Main.IsekaiContext, "Attunement with the Codex of the Reincarnated Otherworlder grants a +2 insight bonus to all Knowledge and Lore checks, as well as a +1 sacred bonus on Will saving throws.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
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
				bp.SetDescription(Main.IsekaiContext, "Read the chronicle of the stars to attune your soul to the multiverse, refreshing your insight and receiving your first-time streamer tip of 500 Cosmic Coins from The Grand Arbiter.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
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
			string codexTitle = "Codex of the Reincarnated Otherworlder: The 13 Constellations & The Cosmic Stream";
			string codexDescription = "A weighty, star-embossed grimoire bound in shimmering astral leather. Its parchment radiates faint multiversal warmth.\n\n--- PROLOGUE: THE OTHERWORLDER ARRIVAL ---\nYou were torn from your previous life on Earth across the multiversal threshold by an unprecedented planar convergence, awakening amid the Worldwound crisis on Golarion with fully unsealed potential.\n\n--- CHAPTER I: THE 13 WATCHING CONSTELLATIONS ---\nThirteen supreme entities and deities across the Great Beyond observe your journey as celestial sponsors:\n1. The Lucky Drunk: Loves daring wagers, taverns, and bold, impossible gambles.\n2. The Laughing King: Revels in chaos, mockery of arrogant tyrants, and outrageous stunts.\n3. The Song of the Spheres: Watches over travelers, dreamers, and joyous liberation.\n4. The Inheritor: Demands unyielding righteousness, chivalry, and crusade honor.\n5. The Dark Prince: Respects cold pragmatism, discipline, and uncompromising will.\n6. The Iron Warden: Favors unbreakable defenses, craftsmanship, and ancient oaths.\n7. The Dawnflower: Champions redemption, blazing sunlight, and healing mercy.\n8. The Silver Maiden: Oversees the cycle of souls and the integrity of destiny.\n9. The World Sovereign: Admits the imperial ambition and boundless potential of mortals.\n10. The Omniscient Hermit: Craves arcane secrets and the bending of physical laws.\n11. The Fading Dragon: Honors ancient draconic lineage and noble self-sacrifice.\n12. The Grand Arbiter: Maintains the fair rules of the celestial live broadcast.\n13. The Lurker at the Threshold: The enigmatic keeper of dimensional doors.\n\n--- CHAPTER II: THE LIVE BROADCAST & COSMIC COINS ---\nWhenever you take bold Otherworlder actions, resolve dilemmas creatively, or unleash Overpowered Abilities, the watching Constellations react in the live chat log and tip you Cosmic Coins.\nAll coins pool into a single universal wallet accepted at the Divine Sponsorship Store, where you can purchase mythic relics, stat tomes, and divine aids.\n\n--- CHAPTER III: SEVERING THE KARMIC THREAD ---\nShould you ever desire a clean slate to begin an untainted Loop 1 journey, you may sever the karmic timeline to reset all cycle records and reincarnate anew.";
			ItemCodexOfReincarnation = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemCodexOfReincarnation", delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, codexTitle);
				bp.SetDescription(Main.IsekaiContext, codexDescription);
				((BlueprintItem)bp).m_Icon = iconCoin;
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
