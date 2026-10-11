using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Items;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.QA.Statistics;
using Kingmaker.UI.MVVM._VM.ServiceWindows.Inventory;
using Kingmaker.UI.MVVM._VM.Vendor;
using Kingmaker.UI.Vendor;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

[HarmonyPatch]
public static class CosmicVendorPatcher
{
	public const string CosmicEnvoyBlueprintName = "AstralCoinEnvoyUnit";

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.ResolvesAll(typeof(CosmicVendorPatcher), (typeof(VendorUI), "UpdatePlayerMoneyAndInventoryWeight"));
	}

	private static bool IsCosmicEnvoy(VendorLogic logic)
	{
		return logic?.VendorUnit?.Blueprint?.name == "AstralCoinEnvoyUnit";
	}

	private static long EnvoyUnitPrice(BlueprintItem blueprint)
	{
		if (blueprint == null)
		{
			return 0L;
		}
		return DivineTokens.PurchasePrice(CosmicEnvoyPrices.ListedCoins(blueprint.name, blueprint.AssetGuid.ToString(), blueprint.Cost));
	}

	[HarmonyPatch(typeof(VendorLogic), "GetItemBuyPrice", new Type[] { typeof(ItemEntity) })]
	[HarmonyBefore(new string[] { "0ToyBox0" })]
	[HarmonyPrefix]
	public static bool GetItemBuyPrice_Entity_Prefix(VendorLogic __instance, ItemEntity item, ref long __result)
	{
		if (!IsCosmicEnvoy(__instance) || item == null)
		{
			return true;
		}
		__result = EnvoyUnitPrice(item.Blueprint);
		return false;
	}

	[HarmonyPatch(typeof(VendorLogic), "GetItemBuyPrice", new Type[] { typeof(BlueprintItem) })]
	[HarmonyBefore(new string[] { "0ToyBox0" })]
	[HarmonyPrefix]
	public static bool GetItemBuyPrice_Blueprint_Prefix(VendorLogic __instance, BlueprintItem blueprintItem, ref long __result)
	{
		if (!IsCosmicEnvoy(__instance) || blueprintItem == null)
		{
			return true;
		}
		__result = EnvoyUnitPrice(blueprintItem);
		return false;
	}

	private static long BasketPrice(VendorLogic logic)
	{
		if (logic?.ItemsForBuy == null)
		{
			return 0L;
		}
		long num = 0L;
		foreach (ItemEntity item in logic.ItemsForBuy)
		{
			if (item != null)
			{
				num += logic.GetItemBuyPrice(item) * item.Count;
			}
		}
		return num;
	}

	[HarmonyPatch(typeof(VendorLogic), "PlayerMoneyAfterDeal", MethodType.Getter)]
	[HarmonyPrefix]
	public static bool PlayerMoneyAfterDeal_Prefix(VendorLogic __instance, ref long __result)
	{
		if (IsCosmicEnvoy(__instance))
		{
			__result = DivineTokens.GetCoins() - BasketPrice(__instance);
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(VendorLogic), "IsDealPossible", MethodType.Getter)]
	[HarmonyPrefix]
	public static bool IsDealPossible_Prefix(VendorLogic __instance, ref bool __result)
	{
		if (IsCosmicEnvoy(__instance))
		{
			long num = BasketPrice(__instance);
			__result = __instance.ItemsForBuy != null && __instance.ItemsForBuy.Any() && DivineTokens.GetCoins() >= num;
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(VendorLogic), "AddForSell")]
	[HarmonyPrefix]
	public static bool AddForSell_Prefix(VendorLogic __instance, ref ItemEntity __result)
	{
		if (IsCosmicEnvoy(__instance))
		{
			__result = null;
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(VendorLogic), "Deal")]
	[HarmonyPrefix]
	public static bool Deal_Prefix(VendorLogic __instance)
	{
		if (!IsCosmicEnvoy(__instance))
		{
			return true;
		}
		long num = BasketPrice(__instance);
		if (num < 0 || num > int.MaxValue || DivineTokens.GetCoins() < num)
		{
			return false;
		}
		ItemsCollection itemsCollection = Game.Instance?.Player?.Inventory;
		if (itemsCollection == null)
		{
			return false;
		}
		if (__instance.ItemsForBuy == null)
		{
			return false;
		}
		if (num > 0 && !DivineTokens.SpendCoins((int)num, applyHagglerDiscount: false))
		{
			return false;
		}
		List<ItemEntity> itemsToBuy = __instance.ItemsForBuy.ToList();
		foreach (ItemEntity item in itemsToBuy)
		{
			item.SellTime = null;
			item.SetVendorIfNull(__instance.VendorUnit);
			__instance.ItemsForBuy.Transfer(item, itemsCollection);
		}
		EventBus.RaiseEvent(delegate(IVendorDealHandler h)
		{
			h.HandleVendorDeal(__instance.VendorUnit, MoneyFlowStatistic.ActionType.Buy, itemsToBuy);
		});
		__instance.ReturnItems();
		__instance.UpdateDeal();
		return false;
	}

	[HarmonyPatch(typeof(VendorUI), "UpdatePlayerMoneyAndInventoryWeight")]
	[HarmonyPostfix]
	public static void UpdatePlayerMoneyAndInventoryWeight_Postfix(VendorUI __instance)
	{
		try
		{
			if ((UnityEngine.Object)(object)__instance?.PlayerMoneyNow != null && IsCosmicEnvoy(VendorUI.Vendor))
			{
				__instance.PlayerMoneyNow.text = ConstellationText.LocalizedFormat("Constellation.CosmicEnvoyMoneyLabel.Text", "{0} Cosmic Coins", DivineSponsorshipStore.FormatCoins(DivineTokens.GetCoins()));
			}
		}
		catch
		{
		}
	}

	[HarmonyPatch(typeof(InventoryStashVM), "UpdateGoldCoins")]
	[HarmonyPostfix]
	public static void UpdateGoldCoins_Postfix(InventoryStashVM __instance)
	{
		try
		{
			if (__instance?.Money != null && VendorHelper.Vendor?.VendorUnit?.Blueprint?.name == "AstralCoinEnvoyUnit")
			{
				__instance.Money.Value = DivineTokens.GetCoins();
			}
		}
		catch
		{
		}
	}
}
