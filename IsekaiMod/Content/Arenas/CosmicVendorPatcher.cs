using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Constellations;
using Kingmaker;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.QA.Statistics;
using Kingmaker.UI.MVVM._VM.ServiceWindows.Inventory;
using Kingmaker.UI.MVVM._VM.Vendor;
using Kingmaker.UI.Vendor;
using UnityEngine;

namespace IsekaiMod.Content.Arenas
{
	[HarmonyPatch]
	public static class CosmicVendorPatcher
	{
		public const string CosmicEnvoyBlueprintName = "AstralCoinEnvoyUnit";

		private static bool IsCosmicEnvoy(VendorLogic logic)
		{
			return logic?.VendorUnit?.Blueprint?.name == "AstralCoinEnvoyUnit";
		}

		[HarmonyPatch(typeof(VendorLogic), "PlayerMoneyAfterDeal", MethodType.Getter)]
		[HarmonyPrefix]
		public static bool PlayerMoneyAfterDeal_Prefix(VendorLogic __instance, ref long __result)
		{
			try
			{
				if (IsCosmicEnvoy(__instance))
				{
					__result = DivineTokens.GetCoins() + __instance.DealPrice;
					return false;
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in PlayerMoneyAfterDeal_Prefix: " + ex);
			}
			return true;
		}

		[HarmonyPatch(typeof(VendorLogic), "IsDealPossible", MethodType.Getter)]
		[HarmonyPrefix]
		public static bool IsDealPossible_Prefix(VendorLogic __instance, ref bool __result)
		{
			try
			{
				if (IsCosmicEnvoy(__instance))
				{
					long num = -__instance.DealPrice;
					if (num <= 0)
					{
						num = __instance.ItemsForBuyPrice;
					}
					__result = __instance.ItemsForBuy != null && __instance.ItemsForBuy.Any() && DivineTokens.GetCoins() >= num;
					return false;
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in IsDealPossible_Prefix: " + ex);
			}
			return true;
		}

		[HarmonyPatch(typeof(VendorLogic), "AddForSell")]
		[HarmonyPrefix]
		public static bool AddForSell_Prefix(VendorLogic __instance, ItemEntity item, ref ItemEntity __result)
		{
			try
			{
				if (IsCosmicEnvoy(__instance))
				{
					__result = null;
					return false;
				}
				if (item?.Blueprint == DivineTokens.ItemCosmicCoin || item?.Blueprint?.name == "ItemCosmicCoin")
				{
					__result = null;
					return false;
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in AddForSell_Prefix: " + ex);
			}
			return true;
		}

		[HarmonyPatch(typeof(VendorLogic), "Deal")]
		[HarmonyPrefix]
		public static bool Deal_Prefix(VendorLogic __instance)
		{
			try
			{
				if (!IsCosmicEnvoy(__instance))
				{
					return true;
				}
				long num = -__instance.DealPrice;
				if (num <= 0)
				{
					num = __instance.ItemsForBuyPrice;
				}
				if (num <= 0 || DivineTokens.GetCoins() < num)
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
				DivineTokens.SpendCoins((int)num, "Cosmic Vendor Purchase");
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
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in Deal_Prefix: " + ex);
				return true;
			}
		}

		[HarmonyPatch(typeof(VendorUI), "UpdatePlayerMoneyAndInventoryWeight")]
		[HarmonyPostfix]
		public static void UpdatePlayerMoneyAndInventoryWeight_Postfix(VendorUI __instance)
		{
			try
			{
				if ((UnityEngine.Object)(object)__instance?.PlayerMoneyNow != null && IsCosmicEnvoy(VendorUI.Vendor))
				{
					__instance.PlayerMoneyNow.text = $"{DivineTokens.GetCoins():N0} Cosmic Coins";
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
}
