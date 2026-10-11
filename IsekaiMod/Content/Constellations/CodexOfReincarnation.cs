using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Quests;
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

namespace IsekaiMod.Content.Constellations;

internal static class CodexOfReincarnation
{
	private sealed class DeliveryHandler : IAreaHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnAreaDidLoad()
		{
			try
			{
				DeliverOnce();
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("CodexOfReincarnation delivery failed: " + ex);
			}
		}

		public void OnAreaBeginUnloading()
		{
		}
	}

	internal const string ItemName = "ItemCodexOfReincarnation";

	internal const string AbilityName = "CodexOfReincarnationAbility";

	internal const string BuffName = "CodexKnowledgeBuff";

	internal const string DeliveredKey = "Port62:CodexDelivered";

	internal const string TipKey = "Port62:CodexFirstReadTip";

	internal const int TipCoins = 500;

	internal const string TipSixTwoSponsor = "The Grand Arbiter";

	private static bool s_Subscribed;

	internal static BlueprintItemEquipmentUsable Item { get; private set; }

	internal static BlueprintBuff KnowledgeBuff { get; private set; }

	public static void Add()
	{
		Sprite coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		KnowledgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "CodexKnowledgeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Chronicle Insight");
			bp.SetDescription(Main.IsekaiContext, "Attunement with the Codex of the Reincarnated Otherworlder grants a +2 insight bonus to all Knowledge and Lore checks, as well as a +1 sacred bonus on Will saving throws.");
			((BlueprintUnitFact)bp).m_Icon = coin;
			StatType[] array = new StatType[4]
			{
				StatType.SkillKnowledgeWorld,
				StatType.SkillKnowledgeArcana,
				StatType.SkillLoreReligion,
				StatType.SkillLoreNature
			};
			foreach (StatType skill in array)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = skill;
					c.Value = 2;
				});
			}
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = 1;
			});
		});
		BlueprintAbility ability = Helpers.CreateBlueprint(Main.IsekaiContext, "CodexOfReincarnationAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Read Codex of the Reincarnated Otherworlder");
			bp.SetDescription(Main.IsekaiContext, "Read the chronicle of the stars to attune your soul to the multiverse, refreshing your insight and receiving your first-time streamer tip of 500 Cosmic Coins from The All-Seeing Eye.");
			((BlueprintUnitFact)bp).m_Icon = coin;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.CanTargetPoint = false;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new IsekaiActionReadCodex());
			});
		});
		Item = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemCodexOfReincarnation", delegate(BlueprintItemEquipmentUsable bp)
		{
			bp.SetName(Main.IsekaiContext, "Codex of the Reincarnated Otherworlder: The 13 Constellations & The Cosmic Stream");
			bp.SetDescription(Main.IsekaiContext, "A weighty, star-embossed grimoire bound in shimmering astral leather. Its parchment radiates faint multiversal warmth.\n\n--- PROLOGUE: THE OTHERWORLDER ARRIVAL ---\nYou were torn from your previous life on Earth across the multiversal threshold by an unprecedented planar convergence, awakening amid the Worldwound crisis on Golarion with fully unsealed potential.\n\n--- CHAPTER I: THE 13 WATCHING CONSTELLATIONS ---\nThirteen supreme entities of the Great Beyond hold a seat in the gallery and observe your journey as celestial sponsors:\n1. The Lucky Drunk: Loves daring wagers, taverns, and bold, impossible gambles.\n2. The Laughing King: Revels in chaos, mockery of arrogant tyrants, and outrageous stunts.\n3. The Song of the Spheres: Watches over travelers, dreamers, and joyous liberation.\n4. The Inheritor: Demands unyielding righteousness, chivalry, and crusade honor.\n5. The Prince of Darkness: Respects cold pragmatism, discipline, and uncompromising will.\n6. Our Lord in Iron: Favors unbreakable defenses, battle, and ancient oaths.\n7. The Everbloom: Champions hope, redemption, and healing mercy.\n8. The Lady of Graves: Oversees the cycle of souls and the integrity of destiny.\n9. The All-Seeing Eye: Craves arcane secrets and the bending of physical laws.\n10. The Savored Sting: Delights in passion, cunning, and sweet revenge.\n11. The Pirate Queen: Toasts freedom of the open sea and a bold plunder.\n12. The Mischievous Friend: Cheers on kindness, luck, and harmless mischief.\n13. The Silence Between the Stars: Keeps the quiet spaces between worlds.\nApart from the gallery sits the Outer Court: The Key and the Gate, enigmatic keeper of dimensional doors, and The Pallid Princess, who hungers for appetite and undeath. They pay for what you speak of, never for how you behave.\n\n--- CHAPTER II: THE LIVE BROADCAST & COSMIC COINS ---\nWhenever you take bold Otherworlder actions, resolve dilemmas creatively, or unleash Overpowered Abilities, the watching Constellations react in the live chat log and tip you Cosmic Coins.\nAll coins pool into a single universal wallet accepted at the Divine Sponsorship Store, where you can purchase mythic relics, stat tomes, and divine aids.\n\n--- CHAPTER III: THE KARMIC THREAD ---\nEvery incarnation you finish is written into the karmic timeline and carried into the next. Reading this tome recites the incarnations you have already lived.");
			((BlueprintItem)bp).m_Icon = coin;
			((BlueprintItem)bp).m_Cost = 5000;
			((BlueprintItem)bp).m_Weight = 1f;
			((BlueprintItem)bp).m_IsNotable = true;
			((BlueprintItem)bp).m_Destructible = false;
			bp.Type = UsableItemType.Other;
			bp.SpendCharges = true;
			bp.Charges = 1;
			bp.RestoreChargesOnRest = true;
			((BlueprintItemEquipment)bp).m_Ability = ability.ToReference<BlueprintAbilityReference>();
		});
	}

	public static void Init()
	{
		if (!s_Subscribed)
		{
			s_Subscribed = true;
			EventBus.Subscribe(new DeliveryHandler());
		}
	}

	internal static void DeliverOnce()
	{
		Player player = Game.Instance?.Player;
		if (player?.Inventory != null && Item != null && ShouldDeliver(ConstellationSettings.Enabled, IsekaiTransmigrationQuest.PlayerCanStartTransmigration(), ConstellationChatManager.RewardLedgerAvailable()) && ConstellationChatManager.TryClaimSceneReward("Port62:CodexDelivered"))
		{
			player.Inventory.Add(Item, 1);
			ConstellationChatManager.PostCombatLog(ConstellationText.Localized("Constellation.Codex.Onboarding", "<color=#5B2A86><b>[OTHERWORLDER ONBOARDING INITIALIZED]</b></color>\nYou have received the <b>Codex of the Reincarnated Otherworlder</b>! Inspect this tome in your inventory to understand the watching Constellations, your celestial audience, and the rules of Cosmic Coins."));
		}
	}

	internal static bool ShouldDeliver(bool constellationsEnabled, bool isTransmigrant, bool ledgerAvailable)
	{
		return constellationsEnabled & isTransmigrant & ledgerAvailable;
	}

	internal static List<string> PastLegacies(IEnumerable<PastCycleRecord> cycles)
	{
		List<string> list = new List<string>();
		foreach (PastCycleRecord item in cycles ?? Enumerable.Empty<PastCycleRecord>())
		{
			string[] array = new string[2]
			{
				item?.PrimaryLegacy,
				item?.SecondaryLegacy
			};
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text) && !list.Contains(text))
				{
					list.Add(text);
				}
			}
		}
		return list;
	}
}
