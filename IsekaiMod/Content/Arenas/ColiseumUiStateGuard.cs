using System;
using System.Reflection;
using Kingmaker;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;

namespace IsekaiMod.Content.Arenas;

public static class ColiseumUiStateGuard
{
	private class AreaLoadGuard : IAreaHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnAreaDidLoad()
		{
			try
			{
				Game instance = Game.Instance;
				if (ShouldEndTradeOnAreaLoad(instance != null && instance.Vendor?.IsTrading == true))
				{
					EndStuckTrade("The area finished loading");
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ColiseumUiStateGuard.OnAreaDidLoad: " + ex);
			}
		}

		public void OnAreaBeginUnloading()
		{
		}
	}

	private static readonly FieldInfo VendorField = typeof(VendorLogic).GetField("m_Vendor", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

	internal static bool ShouldEndTradeOnAreaLoad(bool isTrading)
	{
		return isTrading;
	}

	internal static bool ShouldEndTradeBeforeUnload(bool isTrading, string vendorBlueprintName)
	{
		if (isTrading)
		{
			return PlanarColiseumHubManager.IsHubUnit(vendorBlueprintName);
		}
		return false;
	}

	internal static bool HudIsUsable(bool isTrading, bool fullScreenUiMode)
	{
		if (!isTrading)
		{
			return !fullScreenUiMode;
		}
		return false;
	}

	internal static string CurrentVendorName()
	{
		try
		{
			return Game.Instance?.Vendor?.VendorUnit?.Blueprint?.name ?? string.Empty;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not read the current vendor: " + ex.Message);
			return string.Empty;
		}
	}

	internal static bool EndStuckTrade(string reason)
	{
		VendorLogic vendorLogic = Game.Instance?.Vendor;
		if (vendorLogic == null || !vendorLogic.IsTrading)
		{
			return false;
		}
		string text = CurrentVendorName();
		try
		{
			EventBus.RaiseEvent(delegate(IVendorUIHandler h)
			{
				h.HandleTradeEnding();
			});
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Closing the vendor window threw: " + ex);
		}
		if (vendorLogic.IsTrading)
		{
			try
			{
				vendorLogic.EndTraiding();
			}
			catch (Exception ex2)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Ending the trade threw: " + ex2);
			}
		}
		if (vendorLogic.IsTrading && VendorField != null)
		{
			try
			{
				VendorField.SetValue(vendorLogic, null);
			}
			catch (Exception ex3)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not clear the stuck vendor: " + ex3);
			}
		}
		bool flag = !vendorLogic.IsTrading;
		Main.IsekaiContext.Logger.Log("[PlanarColiseum] " + reason + ": a trade with '" + text + "' was still open; " + (flag ? "it was ended, so the HUD stays usable." : "it could NOT be ended."));
		return flag;
	}

	public static void Init()
	{
		EventBus.Subscribe(new AreaLoadGuard());
	}
}
