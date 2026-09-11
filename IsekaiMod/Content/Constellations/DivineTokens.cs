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
			if (player == null || CosmicTokensFeature == null)
			{
				return 0;
			}
			return player.Descriptor.Progression.Features.GetRank(CosmicTokensFeature);
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
			int num = (int)Math.Round((double)amount * coinLoopMultiplier);
			if (num <= 0)
			{
				num = amount;
			}
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
				for (int j = 0; j < num; j++)
				{
					fact.AddRank();
				}
			}
			int rank = features.GetRank(CosmicTokensFeature);
			SyncInventoryCoins(rank);
			string text = ((coinLoopMultiplier > 1.0) ? $" <color=#00FFFF>[NG+ x{coinLoopMultiplier:0.##}]</color>" : "");
			string msg = ((sponsor != null) ? $"<color=#F5C542><b>[Cosmic Sponsorship]</b></color> <b>{sponsor}</b> has sponsored you <b><color=#F5C542>+{num} Cosmic Coins</color></b>{text}! (Balance: <b>{rank}</b>)" : $"<color=#F5C542><b>[Cosmic Sponsorship]</b></color> You received <b><color=#F5C542>+{num} Cosmic Coins</color></b>{text}! (Balance: <b>{rank}</b>)");
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(msg);
			});
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
			int current = features.GetRank(CosmicTokensFeature);
			if (current < finalAmount)
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#DC143C><b>[Cosmic Sponsorship]</b></color> Insufficient Cosmic Coins! Required: <b>{finalAmount}</b>, Current: <b>{current}</b>");
				});
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
			int remaining = features.GetRank(CosmicTokensFeature);
			SyncInventoryCoins(remaining);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage($"<color=#F5C542><b>[Cosmic Sponsorship]</b></color> Spent <b>{finalAmount} Cosmic Coins</b>. (Remaining Balance: <b>{remaining}</b>)");
			});
			return true;
		}
	}
}
