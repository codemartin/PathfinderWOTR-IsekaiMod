using System;
using System.Collections.Generic;
using System.Globalization;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations;

public static class DivineTokens
{
	internal enum LedgerAction
	{
		Nothing,
		Seed,
		Mirror
	}

	private sealed class OwedCoinsHandler : IAreaActivationHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnAreaActivated()
		{
			PayOwed();
		}
	}

	private sealed class CoinLedgerHandler : IAreaActivationHandler, ISubscriber, IGlobalSubscriber, IUnitChangedAfterRespecHandler
	{
		public void OnAreaActivated()
		{
			RefreshLedger();
		}

		public void HandleUnitChangedAfterRespec(UnitEntityData unit)
		{
			if (unit != null && unit == GetPlayer())
			{
				RefreshLedger();
			}
		}
	}

	public static BlueprintFeature CosmicTokensFeature;

	public static BlueprintItemEquipmentUsable ItemCosmicCoin;

	internal static LocalizedString SponsoredText;

	internal static LocalizedString ReceivedText;

	internal static LocalizedString LoopMultiplierText;

	internal static LocalizedString InsufficientText;

	internal static LocalizedString HaggledText;

	internal static LocalizedString SpentText;

	internal static LocalizedString OwedArrivedText;

	internal const string CoinStorageName = "IsekaiMod.CosmicCoins";

	internal const string BalanceKey = "Balance";

	private static bool s_ledgerSubscribed;

	private static bool s_owedSubscribed;

	internal const string OwedStorageName = "IsekaiMod.OwedCoins";

	internal const string OwedWithLoopKey = "WithLoopMultiplier";

	internal const string OwedExactKey = "Exact";

	public static void CreateBlueprints()
	{
		Sprite Icon_CosmicCoin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		SponsoredText = Helpers.CreateString(Main.IsekaiContext, "CosmicCoins.Log.Sponsored", "<color=#5B2A86><b>[Cosmic Sponsorship]</b></color> <b>{0}</b> has sponsored you <b><color=#5B2A86>+{1} Cosmic Coins</color></b>{2}! (Balance: <b>{3}</b>)");
		ReceivedText = Helpers.CreateString(Main.IsekaiContext, "CosmicCoins.Log.Received", "<color=#5B2A86><b>[Cosmic Sponsorship]</b></color> You received <b><color=#5B2A86>+{0} Cosmic Coins</color></b>{1}! (Balance: <b>{2}</b>)");
		LoopMultiplierText = Helpers.CreateString(Main.IsekaiContext, "CosmicCoins.Log.LoopMultiplier", " <color=#133E71>[NG+ x{0}]</color>");
		InsufficientText = Helpers.CreateString(Main.IsekaiContext, "CosmicCoins.Log.Insufficient", "<color=#7A1010><b>[Cosmic Sponsorship]</b></color> Insufficient Cosmic Coins! Required: <b>{0}</b>, Current: <b>{1}</b>");
		HaggledText = Helpers.CreateString(Main.IsekaiContext, "CosmicCoins.Log.Haggled", "<color=#004604><b>[Haggler Discount]</b></color> Haggled price reduced from <b>{0}</b> to <b>{1} Cosmic Coins</b> (-{2}%, Saved: {3})!");
		SpentText = Helpers.CreateString(Main.IsekaiContext, "CosmicCoins.Log.Spent", "<color=#5B2A86><b>[Cosmic Sponsorship]</b></color> Spent <b>{0} Cosmic Coins</b>. (Remaining Balance: <b>{1}</b>)");
		OwedArrivedText = Helpers.CreateString(Main.IsekaiContext, "CosmicCoins.Log.OwedArrived", "<color=#5B2A86><b>[Cosmic Sponsorship]</b></color> The Cosmic Coins you earned while the Constellations were switched off arrive at last.");
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

	internal static string FormatSafely(string template, params object[] args)
	{
		if (string.IsNullOrEmpty(template))
		{
			return template ?? string.Empty;
		}
		if (args == null || args.Length == 0)
		{
			return template;
		}
		try
		{
			return string.Format(CultureInfo.InvariantCulture, template, args);
		}
		catch (FormatException)
		{
			return template;
		}
	}

	internal static string Say(LocalizedString text, params object[] args)
	{
		if (text == null)
		{
			return string.Empty;
		}
		return FormatSafely(text, args);
	}

	internal static void Log(LocalizedString text, params object[] args)
	{
		string message = Say(text, args);
		if (!string.IsNullOrEmpty(message))
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(message);
			});
		}
	}

	internal static UnitEntityData ResolveDialogTarget(TargetWrapper target, Func<UnitEntityData> fallback)
	{
		UnitEntityData unitEntityData = target?.Unit;
		if (unitEntityData != null)
		{
			return unitEntityData;
		}
		return fallback?.Invoke();
	}

	public static UnitEntityData GetPlayer()
	{
		return (Game.Instance?.Player)?.MainCharacter.Value;
	}

	internal static int? RecordedBalance(IDictionary<string, string> storage)
	{
		if (!int.TryParse(SaveScopedStorage.Get(storage, "Balance"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) || result < 0)
		{
			return null;
		}
		return result;
	}

	internal static int ReadBalance(IDictionary<string, string> storage, int featureRank)
	{
		return RecordedBalance(storage) ?? Math.Max(0, featureRank);
	}

	internal static void WriteBalance(IDictionary<string, string> storage, int balance)
	{
		if (storage != null)
		{
			storage["Balance"] = Math.Max(0, balance).ToString(CultureInfo.InvariantCulture);
		}
	}

	internal static LedgerAction DecideLedger(int? recordedBalance, int featureRank)
	{
		if (!recordedBalance.HasValue)
		{
			if (featureRank <= 0)
			{
				return LedgerAction.Nothing;
			}
			return LedgerAction.Seed;
		}
		if (recordedBalance.Value != Math.Max(0, featureRank))
		{
			return LedgerAction.Mirror;
		}
		return LedgerAction.Nothing;
	}

	public static int GetBalance()
	{
		UnitEntityData player = GetPlayer();
		if (player == null || CosmicTokensFeature == null)
		{
			return 0;
		}
		return ReadBalance(SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.CosmicCoins"), player.Descriptor.Progression.Features.GetRank(CosmicTokensFeature));
	}

	private static void SetBalance(UnitEntityData player, int balance)
	{
		WriteBalance(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.CosmicCoins"), balance);
		MirrorFeature(player, balance);
		SyncInventoryCoins(balance);
	}

	private static void MirrorFeature(UnitEntityData player, int balance)
	{
		FeatureCollection features = player.Descriptor.Progression.Features;
		Feature feature = features.GetFact(CosmicTokensFeature);
		if (feature == null)
		{
			if (balance <= 0)
			{
				return;
			}
			feature = features.AddFeature(CosmicTokensFeature);
		}
		SetCoinRank(feature, ClampRank(0, balance, CosmicTokensFeature.Ranks));
	}

	internal static void RefreshLedger()
	{
		try
		{
			UnitEntityData player = GetPlayer();
			if (!(player == null) && CosmicTokensFeature != null)
			{
				int rank = player.Descriptor.Progression.Features.GetRank(CosmicTokensFeature);
				int? recordedBalance = RecordedBalance(SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.CosmicCoins"));
				switch (DecideLedger(recordedBalance, rank))
				{
				case LedgerAction.Seed:
					WriteBalance(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.CosmicCoins"), rank);
					break;
				case LedgerAction.Mirror:
					MirrorFeature(player, recordedBalance.Value);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error refreshing the Cosmic Coin record: " + ex);
		}
	}

	public static void Init()
	{
		if (!s_owedSubscribed)
		{
			s_owedSubscribed = true;
			EventBus.Subscribe(new OwedCoinsHandler());
		}
		if (ConstellationSettings.Enabled && !s_ledgerSubscribed)
		{
			s_ledgerSubscribed = true;
			EventBus.Subscribe(new CoinLedgerHandler());
		}
	}

	internal static int OwedIn(IDictionary<string, string> storage, bool withLoopMultiplier)
	{
		if (!int.TryParse(SaveScopedStorage.Get(storage, withLoopMultiplier ? "WithLoopMultiplier" : "Exact"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) || result <= 0)
		{
			return 0;
		}
		return result;
	}

	internal static void Owe(IDictionary<string, string> storage, int amount, bool withLoopMultiplier)
	{
		if (storage != null && amount > 0)
		{
			long num = (long)OwedIn(storage, withLoopMultiplier) + (long)amount;
			if (num > int.MaxValue)
			{
				num = 2147483647L;
			}
			storage[withLoopMultiplier ? "WithLoopMultiplier" : "Exact"] = ((int)num).ToString(CultureInfo.InvariantCulture);
		}
	}

	public static void AddClaimedCoins(int amount, string sponsor)
	{
		if (amount > 0)
		{
			if (ConstellationSettings.Enabled)
			{
				AddCoins(amount, sponsor);
			}
			else
			{
				Owe(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.OwedCoins"), amount, withLoopMultiplier: true);
			}
		}
	}

	public static void AddClaimedCoinsWithoutLoopMultiplier(int amount, string sponsor)
	{
		if (amount > 0)
		{
			if (ConstellationSettings.Enabled)
			{
				AddCoinsWithoutLoopMultiplier(amount, sponsor);
			}
			else
			{
				Owe(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.OwedCoins"), amount, withLoopMultiplier: false);
			}
		}
	}

	internal static void PayOwed()
	{
		try
		{
			if (!ConstellationSettings.Enabled)
			{
				return;
			}
			EntityDataBase entity = SaveScopedStorage.CurrentPlayer();
			Dictionary<string, string> dictionary = SaveScopedStorage.Find(entity, "IsekaiMod.OwedCoins");
			if (dictionary == null)
			{
				return;
			}
			int num = OwedIn(dictionary, withLoopMultiplier: true);
			int num2 = OwedIn(dictionary, withLoopMultiplier: false);
			if (num <= 0 && num2 <= 0)
			{
				SaveScopedStorage.Drop(entity, "IsekaiMod.OwedCoins");
			}
			else if (!(GetPlayer() == null) && CosmicTokensFeature != null)
			{
				SaveScopedStorage.Drop(entity, "IsekaiMod.OwedCoins");
				Log(OwedArrivedText);
				if (num > 0)
				{
					AddCoins(num);
				}
				if (num2 > 0)
				{
					AddCoinsWithoutLoopMultiplier(num2, null);
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error paying the Cosmic Coins owed from while the Constellations were off: " + ex);
		}
	}

	public static int GetCoins()
	{
		return GetBalance();
	}

	public static bool SpendCoins(int amount, string reason)
	{
		return SpendCoins(amount, applyHagglerDiscount: true);
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

	internal static int ShownCoins(int baseCoins)
	{
		if (baseCoins > 0)
		{
			return ApplyLoopMultiplier(baseCoins, TimelineManager.GetCoinLoopMultiplier());
		}
		return 0;
	}

	internal static int ApplyLoopMultiplier(int amount, double loopMultiplier)
	{
		if (amount <= 0)
		{
			return 0;
		}
		if (loopMultiplier <= 0.0)
		{
			return amount;
		}
		int num = (int)Math.Round((double)amount * loopMultiplier);
		if (num > 0)
		{
			return num;
		}
		return amount;
	}

	internal static int ClampRank(int currentRank, int delta, int maxRanks)
	{
		long num = (long)currentRank + (long)delta;
		if (num < 0)
		{
			num = 0L;
		}
		if (maxRanks > 0 && num > maxRanks)
		{
			num = maxRanks;
		}
		return (int)num;
	}

	private static void SetCoinRank(Feature fact, int target)
	{
		if (fact != null)
		{
			if (target <= 0)
			{
				fact.SetRank(1);
				fact.RemoveRank();
			}
			else
			{
				fact.SetRank(target);
			}
		}
	}

	public static void AddCoinsWithoutLoopMultiplier(int amount, string sponsor)
	{
		if (amount <= 0 || !ConstellationSettings.Enabled)
		{
			return;
		}
		UnitEntityData player = GetPlayer();
		if (!(player == null) && CosmicTokensFeature != null)
		{
			int balance = GetBalance();
			int num = ClampRank(balance, amount, CosmicTokensFeature.Ranks);
			SetBalance(player, num);
			int num2 = Math.Max(0, num - balance);
			if (sponsor != null)
			{
				Log(SponsoredText, sponsor, num2, "", num);
			}
			else
			{
				Log(ReceivedText, num2, "", num);
			}
		}
	}

	public static void AddCoins(int amount, string sponsor = null)
	{
		if (amount <= 0 || !ConstellationSettings.Enabled)
		{
			return;
		}
		UnitEntityData player = GetPlayer();
		if (player == null || CosmicTokensFeature == null)
		{
			return;
		}
		double coinLoopMultiplier = TimelineManager.GetCoinLoopMultiplier();
		int num = ApplyLoopMultiplier(amount, coinLoopMultiplier);
		if (num > 0)
		{
			int balance = GetBalance();
			int num2 = ClampRank(balance, num, CosmicTokensFeature.Ranks);
			SetBalance(player, num2);
			int num3 = Math.Max(0, num2 - balance);
			string text = ((coinLoopMultiplier > 1.0) ? Say(LoopMultiplierText, coinLoopMultiplier.ToString("0.##", CultureInfo.InvariantCulture)) : "");
			if (sponsor != null)
			{
				Log(SponsoredText, sponsor, num3, text, num2);
			}
			else
			{
				Log(ReceivedText, num3, text, num2);
			}
		}
	}

	public static bool SpendCoins(int amount)
	{
		return SpendCoins(amount, applyHagglerDiscount: true);
	}

	internal static int DiscountedCost(int amount, int discountPercent)
	{
		if (amount <= 0)
		{
			return 0;
		}
		if (discountPercent <= 0)
		{
			return amount;
		}
		return Math.Max(1, (int)Math.Round((double)amount * (1.0 - (double)discountPercent / 100.0)));
	}

	internal static bool CanAfford(int balance, int amount, int discountPercent)
	{
		return DiscountedCost(amount, discountPercent) <= Math.Max(0, balance);
	}

	public static int PurchasePrice(int amount)
	{
		UnitEntityData player = GetPlayer();
		if (player == null || amount <= 0)
		{
			return Math.Max(0, amount);
		}
		return DiscountedCost(amount, Haggler.GetDiscountPercent(player));
	}

	public static bool CanAffordPurchase(int amount)
	{
		UnitEntityData player = GetPlayer();
		if (player == null || CosmicTokensFeature == null)
		{
			return amount <= 0;
		}
		int discountPercent = Haggler.GetDiscountPercent(player);
		return CanAfford(GetBalance(), amount, discountPercent);
	}

	public static bool SpendCoins(int amount, bool applyHagglerDiscount)
	{
		if (amount <= 0)
		{
			return true;
		}
		if (!ConstellationSettings.Enabled)
		{
			return false;
		}
		UnitEntityData player = GetPlayer();
		if (player == null || CosmicTokensFeature == null)
		{
			return false;
		}
		int num = (applyHagglerDiscount ? Haggler.GetDiscountPercent(player) : 0);
		int num2 = DiscountedCost(amount, num);
		int balance = GetBalance();
		if (balance < num2)
		{
			Log(InsufficientText, num2, balance);
			return false;
		}
		int num3 = ClampRank(balance, -num2, CosmicTokensFeature.Ranks);
		SetBalance(player, num3);
		int num4 = amount - num2;
		if (num4 > 0)
		{
			Log(HaggledText, amount, num2, num, num4);
		}
		Log(SpentText, num2, num3);
		return true;
	}
}
