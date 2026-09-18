using System;
using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations
{
	public static class ConstellationBounties
	{
		public static BlueprintBuff GorumBloodlustBuff;

		public static BlueprintBuff DesnaStarlightGraceBuff;

		public static BlueprintBuff CaydenGoldenDraughtBuff;

		public static BlueprintBuff HellfireAcumenBuff;

		public static BlueprintBuff VelvetShadowCloakBuff;

		public static BlueprintBuff CausalRuptureInsightBuff;

		private static readonly Dictionary<string, ConstellationBounty> s_Bounties = new Dictionary<string, ConstellationBounty>(StringComparer.OrdinalIgnoreCase);

		private static bool s_Initialized = false;

		public static void Init()
		{
			if (!s_Initialized)
			{
				s_Initialized = true;
				CreateBuffs();
				RegisterBounties();
			}
		}

		private static void CreateBuffs()
		{
			Sprite iconCoin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			GorumBloodlustBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "GorumBloodlustBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Gorum's Bloodlust");
				bp.SetDescription(Main.IsekaiContext, "Infused with the unyielding fury of Our Lord in Iron, you gain a +2 morale bonus on attack and weapon damage rolls.");
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
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			DesnaStarlightGraceBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "DesnaStarlightGraceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Desna's Starlight Grace");
				bp.SetDescription(Main.IsekaiContext, "Bathed in the radiant benevolence of the Song of the Spheres, your base speed increases by 10 feet and you gain a +2 luck bonus on all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
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
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
			CaydenGoldenDraughtBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "CaydenGoldenDraughtBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cayden's Golden Draught");
				bp.SetDescription(Main.IsekaiContext, "Imbued with the hearty joviality of the Laughing King, you gain a +2 morale bonus on saving throws and immunity to poison and sickening effects.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
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
					c.Condition = UnitCondition.Sickened;
				});
			});
			HellfireAcumenBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "HellfireAcumenBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Hellfire Acumen");
				bp.SetDescription(Main.IsekaiContext, "Sealed under the exacting scrutiny of the Prince of Darkness, you gain a +2 profane bonus to Armor Class and a +2 bonus to initiative rolls.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
			});
			VelvetShadowCloakBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "VelvetShadowCloakBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Velvet Shadow Cloak");
				bp.SetDescription(Main.IsekaiContext, "Veiled by the seductive shadows of Lady Nocticula, you gain a +4 competence bonus on Stealth checks and a +2 dodge bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			CausalRuptureInsightBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "CausalRuptureInsightBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Causal Rupture Insight");
				bp.SetDescription(Main.IsekaiContext, "Having severed the deterministic cycle of Yog-Sothoth, you perceive all timelines at once, gaining a +4 insight bonus on all saving throws and initiative rolls.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 4;
				});
			});
		}

		private static void RegisterBounties()
		{
			AddBounty(new ConstellationBounty
			{
				Id = "Gorum_Minagho_Defeat",
				Title = "The Iron Anvil of Kenabres",
				Sponsor = "Our Lord in Iron",
				AnnouncementText = "<color=#DC143C><b>[Patron Challenge: Our Lord in Iron]</b></color>: <i>\"THE CRUSADE RUNS ON BLOOD AND STEEL! BREAK MINAGHO UPON THE ANVIL AND CLAIM MY FAVOR!\"</i>",
				SuperChatText = "THAT IS HOW REAL WARRIORS FIGHT! SMASH THEIR BONES INTO THE STONES!",
				CoinReward = 500,
				BuffReward = GorumBloodlustBuff,
				MilestoneKey = "Bounty_Gorum_Minagho"
			});
			AddBounty(new ConstellationBounty
			{
				Id = "Desna_MarketSquare_Mercy",
				Title = "Starlight Reconciliation",
				Sponsor = "The Song of the Spheres",
				AnnouncementText = "<color=#00FFFF><b>[Patron Challenge: The Song of the Spheres]</b></color>: <i>\"Suspicions sow darkness in Kenabres. Unravel the mimic's deception and restore holy harmony between Hulrun and Ramien!\"</i>",
				SuperChatText = "Starlight shines brightest when compassion dispels suspicion! Receive the blessing of the spheres!",
				CoinReward = 400,
				BuffReward = DesnaStarlightGraceBuff,
				MilestoneKey = "Bounty_Desna_MarketSquare"
			});
			AddBounty(new ConstellationBounty
			{
				Id = "Cayden_DefendersHeart_Tavern",
				Title = "The Tavern Keg Stand",
				Sponsor = "The Laughing King",
				AnnouncementText = "<color=#FFD700><b>[Patron Challenge: The Laughing King]</b></color>: <i>\"The demons are knocking at the tavern gates! Hold the cellar, rally the recruits, and turn this siege into a legendary brawl!\"</i>",
				SuperChatText = "Bwahaha! Now THAT is what I call tavern defense! Drinks on me across the planes!",
				CoinReward = 500,
				BuffReward = CaydenGoldenDraughtBuff,
				MilestoneKey = "Bounty_Cayden_DefendersHeart"
			});
			AddBounty(new ConstellationBounty
			{
				Id = "Asmodeus_Regill_Alliance",
				Title = "The Sovereign Contract",
				Sponsor = "The Prince of Darkness",
				AnnouncementText = "<color=#FF4500><b>[Patron Challenge: The Prince of Darkness]</b></color>: <i>\"A Hellknight commander values precision and unyielding order. Secure this alliance through cold tactical deduction.\"</i>",
				SuperChatText = "A masterstroke of contractual leverage. The Godclaw bows not to weakness, but to supreme intellect.",
				CoinReward = 450,
				BuffReward = HellfireAcumenBuff,
				MilestoneKey = "Bounty_Asmodeus_Regill"
			});
			AddBounty(new ConstellationBounty
			{
				Id = "Nocticula_Palace_Peer",
				Title = "The Midnight Sovereign Gambit",
				Sponsor = "The Lady of Shadows",
				AnnouncementText = "<color=#9370DB><b>[Patron Challenge: Lady of Shadows]</b></color>: <i>\"Petty mortals grovel before my velvet throne. Let us see if an Otherworlder has the spine to speak to me as a peer.\"</i>",
				SuperChatText = "Finally, an Otherworlder with the audacity to look beyond my abyssal mask. Take my shadow's embrace.",
				CoinReward = 600,
				BuffReward = VelvetShadowCloakBuff,
				MilestoneKey = "Bounty_Nocticula_Palace"
			});
			AddBounty(new ConstellationBounty
			{
				Id = "Yog_Threshold_LoopBreaker",
				Title = "The Causal Rupture",
				Sponsor = "The Key and the Gate",
				AnnouncementText = "<color=#00FFFF><b>[Patron Challenge: The Key and the Gate]</b></color>: <i>\"THE CYCLICAL SCRIPT REACHES ITS APEX. WILL THE TRAVELER REPLAY THE TAPE, OR SHATTER THE GLASS?\"</i>",
				SuperChatText = "THE TAPE HAS RUN OUT. THE FINAL SCRIPT BELONGS TO YOUR OWN WILL. THE COSMOS APPLAUDS YOUR UNCHAINED ASCENT.",
				CoinReward = 1500,
				BuffReward = CausalRuptureInsightBuff,
				MilestoneKey = "Bounty_Yog_Threshold"
			});
		}

		private static void AddBounty(ConstellationBounty bounty)
		{
			if (bounty != null && !string.IsNullOrEmpty(bounty.Id))
			{
				s_Bounties[bounty.Id] = bounty;
			}
		}

		public static void AnnounceBounty(string bountyId)
		{
			try
			{
				if (!string.IsNullOrEmpty(bountyId) && s_Bounties.TryGetValue(bountyId, out var value) && !ConstellationChatManager.HasTriggeredMilestone(value.MilestoneKey))
				{
					ConstellationChatManager.PostLog(value.AnnouncementText, value.Sponsor, ConstellationCategory.Quest, 0, value.Title);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationBounties.AnnounceBounty: " + ex);
			}
		}

		public static void CompleteBounty(string bountyId)
		{
			try
			{
				if (string.IsNullOrEmpty(bountyId) || !s_Bounties.TryGetValue(bountyId, out var value) || ConstellationChatManager.HasTriggeredMilestone(value.MilestoneKey))
				{
					return;
				}
				ConstellationChatManager.MarkMilestoneTriggered(value.MilestoneKey);
				if (value.CoinReward > 0)
				{
					DivineTokens.AddCoins(value.CoinReward, value.Sponsor);
				}
				if (value.BuffReward != null)
				{
					UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
					if (unitEntityData != null)
					{
						unitEntityData.Descriptor.Buffs.AddBuff(value.BuffReward, unitEntityData, TimeSpan.FromHours(24.0));
					}
				}
				ConstellationChatManager.PostSuperChat(value.Sponsor, value.CoinReward, value.Title, value.SuperChatText);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationBounties.CompleteBounty: " + ex);
			}
		}
	}
}
