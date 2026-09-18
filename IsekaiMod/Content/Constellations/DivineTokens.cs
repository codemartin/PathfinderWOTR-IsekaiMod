using System;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations
{
	public static class DivineTokens
	{
		public static BlueprintFeature CosmicTokensFeature;

		public static BlueprintItemEquipmentUsable ItemCosmicCoin;

		public static void Add()
		{
			Sprite Icon_CosmicCoin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			CosmicTokensFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicTokensFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Coins");
				bp.SetDescription(Main.IsekaiContext, "Tokens of cosmic favor bestowed upon you by observing deities and higher planar entities.\nThese tokens are awarded when deities celebrate your choices, audacious swagger, or heroic triumphs.\nThey can be redeemed through the Cosmic Sponsorship Exchange for divine boons, sacred elixirs, and otherworldly relics.");
				((BlueprintUnitFact)bp).m_Icon = Icon_CosmicCoin;
				bp.Ranks = 999999;
				bp.HideInCharacterSheetAndLevelUp = false;
				bp.IsClassFeature = true;
			});
		}

		public static UnitEntityData GetPlayer()
		{
			return BlueprintSafetyExtensions.SafeGetMainCharacter();
		}

		public static int GetBalance()
		{
			UnitEntityData player = GetPlayer();
			int cosmicCoins = TimelineManager.Data.CosmicCoins;
			if (player == null || CosmicTokensFeature == null)
			{
				return cosmicCoins;
			}
			int rank = player.Descriptor.Progression.Features.GetRank(CosmicTokensFeature);
			int num = Math.Max(rank, cosmicCoins);
			if (rank < num)
			{
				FeatureCollection features = player.Descriptor.Progression.Features;
				Feature fact = features.GetFact(CosmicTokensFeature);
				if (fact == null)
				{
					fact = features.AddFeature(CosmicTokensFeature);
					if (fact != null && num > 1)
					{
						for (int i = 1; i < num; i++)
						{
							fact.AddRank();
						}
					}
				}
				else
				{
					for (int j = rank; j < num; j++)
					{
						fact.AddRank();
					}
				}
			}
			else if (rank > cosmicCoins)
			{
				TimelineManager.Data.CosmicCoins = rank;
				TimelineManager.Save();
			}
			SyncInventoryCoins(num);
			return num;
		}

		public static int GetCoins()
		{
			return GetBalance();
		}

		public static bool SpendCoins(int amount, string reason)
		{
			bool applyHagglerDiscount = reason != "Cosmic Vendor Purchase";
			return SpendCoins(amount, applyHagglerDiscount);
		}

		public static void SyncInventoryCoins(int targetCount)
		{
			try
			{
				ItemsCollection itemsCollection = Game.Instance?.Player?.Inventory;
				if (itemsCollection != null && ItemCosmicCoin != null)
				{
					int num = itemsCollection.Count(ItemCosmicCoin);
					if (num < targetCount)
					{
						itemsCollection.Add(ItemCosmicCoin, targetCount - num);
					}
					else if (num > targetCount)
					{
						itemsCollection.Remove(ItemCosmicCoin, num - targetCount);
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in SyncInventoryCoins: " + ex);
			}
		}

		public static bool IsPlayerPatron(UnitEntityData player, string sponsor)
		{
			if (string.IsNullOrEmpty(sponsor))
			{
				return false;
			}
			string playerDeityName = ConstellationChatManager.GetPlayerDeityName();
			if (string.IsNullOrEmpty(playerDeityName))
			{
				return false;
			}
			string text = sponsor.ToLower();
			string text2 = playerDeityName.ToLower();
			if (text2.Contains("cayden") && (text.Contains("cayden") || text.Contains("lucky drunk")))
			{
				return true;
			}
			if (text2.Contains("iomedae") && (text.Contains("iomedae") || text.Contains("inheritor")))
			{
				return true;
			}
			if (text2.Contains("asmodeus") && (text.Contains("asmodeus") || text.Contains("archfiend") || text.Contains("lord of the pit")))
			{
				return true;
			}
			if (text2.Contains("desna") && (text.Contains("desna") || text.Contains("song of the spheres") || text.Contains("starwatcher")))
			{
				return true;
			}
			if (text2.Contains("pharasma") && (text.Contains("pharasma") || text.Contains("lady of graves") || text.Contains("astral court")))
			{
				return true;
			}
			if (text2.Contains("calistria") && (text.Contains("calistria") || text.Contains("savored sting") || text.Contains("wasp")))
			{
				return true;
			}
			if (text2.Contains("nethys") && (text.Contains("nethys") || text.Contains("all-seeing eye") || text.Contains("two-faced")))
			{
				return true;
			}
			if (text2.Contains("gorum") && (text.Contains("gorum") || text.Contains("lord in iron") || text.Contains("warlord")))
			{
				return true;
			}
			if (text2.Contains("besmara") && (text.Contains("besmara") || text.Contains("pirate queen") || text.Contains("corsair")))
			{
				return true;
			}
			if ((text2.Contains("lantern") || text2.Contains("laughing")) && (text.Contains("lantern") || text.Contains("laughing king")))
			{
				return true;
			}
			if (text2.Contains("chaldira") && (text.Contains("chaldira") || text.Contains("calamity star") || text.Contains("little spark")))
			{
				return true;
			}
			if (text2.Contains("milani") && (text.Contains("milani") || text.Contains("everbloom") || text.Contains("rose of devotion")))
			{
				return true;
			}
			if (text2.Contains("butterfly") && (text.Contains("butterfly") || text.Contains("pale shadow")))
			{
				return true;
			}
			if (text2.Contains("sarenrae") && (text.Contains("sarenrae") || text.Contains("dawnflower") || text.Contains("healing flame")))
			{
				return true;
			}
			if (text2.Contains("torag") && (text.Contains("torag") || text.Contains("father of creation") || text.Contains("iron shaper")))
			{
				return true;
			}
			if (text2.Contains("urgathoa") && (text.Contains("urgathoa") || text.Contains("pallid princess") || text.Contains("feast giver")))
			{
				return true;
			}
			if (text2.Contains("pulura") && (text.Contains("pulura") || text.Contains("shimmering maiden") || text.Contains("northern lights")))
			{
				return true;
			}
			if (text2.Contains("shelyn") && (text.Contains("shelyn") || text.Contains("eternal rose") || text.Contains("songbird")))
			{
				return true;
			}
			if (text2.Contains("abadar") && (text.Contains("abadar") || text.Contains("first vault") || text.Contains("golden scale")))
			{
				return true;
			}
			if (text2.Contains("rovagug") && (text.Contains("rovagug") || text.Contains("rough beast") || text.Contains("destroyer")))
			{
				return true;
			}
			if (text2.Contains("irori") && (text.Contains("irori") || text.Contains("master of masters") || text.Contains("enlightened")))
			{
				return true;
			}
			if (text2.Contains("erastil") && (text.Contains("erastil") || text.Contains("old deadeye") || text.Contains("huntsman")))
			{
				return true;
			}
			if (text2.Contains("lamashtu") && (text.Contains("lamashtu") || text.Contains("mother of monsters") || text.Contains("mother of beasts")))
			{
				return true;
			}
			if (text2.Contains("nocticula") && (text.Contains("nocticula") || text.Contains("lady in shadow")))
			{
				return true;
			}
			if (text2.Contains("baphomet") && (text.Contains("baphomet") || text.Contains("lord of the labyrinth")))
			{
				return true;
			}
			if (text2.Contains("deskari") && (text.Contains("deskari") || text.Contains("locust host")))
			{
				return true;
			}
			if (text2.Contains("yog-sothoth") && (text.Contains("yog-sothoth") || text.Contains("key and the gate")))
			{
				return true;
			}
			if (!text.Contains(text2))
			{
				return text2.Contains(text);
			}
			return true;
		}

		public static void AddCoins(int amount, string sponsor = null)
		{
			if (amount <= 0)
			{
				return;
			}
			UnitEntityData player = GetPlayer();
			if (player == null || CosmicTokensFeature == null)
			{
				return;
			}
			double coinLoopMultiplier = TimelineManager.GetCoinLoopMultiplier();
			bool flag = IsPlayerPatron(player, sponsor);
			double num = (flag ? 1.25 : 1.0);
			int num2 = (int)Math.Round((double)amount * coinLoopMultiplier * num);
			if (num2 <= 0)
			{
				num2 = amount;
			}
			FeatureCollection features = player.Descriptor.Progression.Features;
			Feature fact = features.GetFact(CosmicTokensFeature);
			if (fact == null)
			{
				fact = features.AddFeature(CosmicTokensFeature);
				if (fact != null && num2 > 1)
				{
					for (int i = 1; i < num2; i++)
					{
						fact.AddRank();
					}
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					fact.AddRank();
				}
			}
			int rank = features.GetRank(CosmicTokensFeature);
			TimelineManager.Data.CosmicCoins = rank;
			TimelineManager.Save();
			SyncInventoryCoins(rank);
			string text = ((coinLoopMultiplier > 1.0) ? $" <color=#00FFFF>[NG+ x{coinLoopMultiplier:0.##}]</color>" : "");
			string text2 = (flag ? " <color=#FFD700><b>[Patron Favor +25%]</b></color>" : "");
			string msg = ((sponsor != null) ? $"<color=#F5C542><b>[Cosmic Sponsorship]</b></color> <b>{sponsor}</b> has sponsored you <b><color=#F5C542>+{num2} Cosmic Coins</color></b>{text}{text2}! (Balance: <b>{rank}</b>)" : $"<color=#F5C542><b>[Cosmic Sponsorship]</b></color> You received <b><color=#F5C542>+{num2} Cosmic Coins</color></b>{text}{text2}! (Balance: <b>{rank}</b>)");
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(msg);
			});
			ConstellationChatOverlay.AddMessage(msg, sponsor, num2);
			try
			{
				EventBus.RaiseEvent(delegate(IWarningNotificationUIHandler h)
				{
					h.HandleWarning(msg, addToLog: false);
				});
			}
			catch
			{
			}
		}

		public static bool SpendCoins(int amount)
		{
			return SpendCoins(amount, applyHagglerDiscount: true);
		}

		public static bool SpendCoins(int amount, bool applyHagglerDiscount)
		{
			if (amount <= 0)
			{
				return true;
			}
			UnitEntityData player = GetPlayer();
			if (player == null || CosmicTokensFeature == null)
			{
				return false;
			}
			int finalAmount = amount;
			if (applyHagglerDiscount)
			{
				int discountPercent = Haggler.GetDiscountPercent(player);
				if (discountPercent > 0)
				{
					finalAmount = Math.Max(1, (int)Math.Round((double)amount * (1.0 - (double)discountPercent / 100.0)));
					int saved = amount - finalAmount;
					if (saved > 0)
					{
						EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
						{
							h.HandleLogMessage($"<color=#00FF7F><b>[Haggler Discount]</b></color> Haggled price reduced from <b>{amount}</b> to <b>{finalAmount} Cosmic Coins</b> (-{discountPercent}%, Saved: {saved})!");
						});
					}
				}
			}
			FeatureCollection features = player.Descriptor.Progression.Features;
			int rank = features.GetRank(CosmicTokensFeature);
			if (rank < finalAmount)
			{
				string failMsg = $"<color=#DC143C><b>[Cosmic Sponsorship]</b></color> Insufficient Cosmic Coins! Required: <b>{finalAmount}</b>, Current: <b>{rank}</b>";
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(failMsg);
				});
				ConstellationChatOverlay.AddMessage(failMsg, "Cosmic Exchange");
				try
				{
					EventBus.RaiseEvent(delegate(IWarningNotificationUIHandler h)
					{
						h.HandleWarning(failMsg, addToLog: false);
					});
				}
				catch
				{
				}
				return false;
			}
			Feature fact = features.GetFact(CosmicTokensFeature);
			if (fact != null)
			{
				for (int num = 0; num < finalAmount; num++)
				{
					fact.RemoveRank();
				}
			}
			int rank2 = features.GetRank(CosmicTokensFeature);
			TimelineManager.Data.CosmicCoins = rank2;
			TimelineManager.Save();
			SyncInventoryCoins(rank2);
			string spendMsg = $"<color=#F5C542><b>[Cosmic Sponsorship]</b></color> Spent <b>{finalAmount} Cosmic Coins</b>. (Remaining Balance: <b>{rank2}</b>)";
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(spendMsg);
			});
			ConstellationChatOverlay.AddMessage(spendMsg, "Cosmic Exchange");
			try
			{
				EventBus.RaiseEvent(delegate(IWarningNotificationUIHandler h)
				{
					h.HandleWarning(spendMsg, addToLog: false);
				});
			}
			catch
			{
			}
			return true;
		}
	}
}
