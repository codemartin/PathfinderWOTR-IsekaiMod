using System;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Items;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.MVVM;
using Kingmaker.UnitLogic.Parts;

namespace IsekaiMod.Content.Arenas;

internal static class HubTrade
{
	internal enum Outcome
	{
		Opened,
		NoVendor,
		Refused,
		WindowFailed
	}

	internal static Outcome Classify(bool vendorFound, bool tradingAfter, bool windowOpenAfter)
	{
		if (!vendorFound)
		{
			return Outcome.NoVendor;
		}
		if (!tradingAfter)
		{
			return Outcome.Refused;
		}
		if (!windowOpenAfter)
		{
			return Outcome.WindowFailed;
		}
		return Outcome.Opened;
	}

	internal static bool IsLiveVendor(string unitBlueprintName, string wantedName, bool destroyed, bool disposed)
	{
		if (!destroyed && !disposed && !string.IsNullOrEmpty(wantedName))
		{
			return string.Equals(unitBlueprintName, wantedName, StringComparison.Ordinal);
		}
		return false;
	}

	internal static UnitEntityData FindVendor(string unitBlueprintName)
	{
		return (Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>())?.FirstOrDefault((UnitEntityData u) => u != null && IsLiveVendor(u.Blueprint?.name, unitBlueprintName, u.Destroyed, u.IsDisposed));
	}

	internal static bool VendorWindowOpen()
	{
		try
		{
			return RootUIContext.Instance?.InGameVM?.StaticPartVM?.VendorVM?.Value != null;
		}
		catch
		{
			return false;
		}
	}

	internal static Outcome Start(string unitBlueprintName, BlueprintSharedVendorTable table)
	{
		UnitEntityData vendor = FindVendor(unitBlueprintName);
		if (vendor == null)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] No live " + unitBlueprintName + " is standing here, so there is no shop to open.");
			return Outcome.NoVendor;
		}
		Game instance = Game.Instance;
		if (instance != null && instance.Vendor?.IsTrading == true)
		{
			ColiseumUiStateGuard.EndStuckTrade("A hub vendor's shop was asked for");
		}
		try
		{
			UnitPartVendor unitPartVendor = vendor.Get<UnitPartVendor>();
			if (table != null && (unitPartVendor == null || unitPartVendor.SharedInventory == null))
			{
				vendor.Ensure<UnitPartVendor>().SetSharedInventory(table);
				Main.IsekaiContext.Logger.Log("[PlanarColiseum] " + unitBlueprintName + " had no vendor table; it was given " + table.name + ".");
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[PlanarColiseum] Could not give {unitBlueprintName} its vendor table: {arg}");
		}
		ItemRenderGuard.SweepPartyInventory("A hub vendor's shop is opening");
		EventBus.RaiseEvent(delegate(IVendorUIHandler h)
		{
			h.HandleTradeStarted(vendor);
		});
		Game instance2 = Game.Instance;
		Outcome outcome = Classify(vendorFound: true, instance2 != null && instance2.Vendor?.IsTrading == true, VendorWindowOpen());
		switch (outcome)
		{
		case Outcome.Refused:
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] The game refused a trade with " + unitBlueprintName + ".");
			break;
		case Outcome.WindowFailed:
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] The trade with " + unitBlueprintName + " opened without a window; it is ended so the HUD stays usable.");
			ColiseumUiStateGuard.EndStuckTrade("A hub vendor's window failed to build");
			break;
		}
		return outcome;
	}
}
