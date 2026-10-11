using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("246760757a8942b6ba9f25e88e36202a")]
public class ActionStartTradeWithSmuggler : IsekaiGameAction
{
	public override string GetCaption()
	{
		return "Start trade with Void Market Smuggler";
	}

	protected override void RunSafely()
	{
		HubTrade.Start("VoidMarketSmugglerUnit", OtherworldLuxuryVendorTable.Table);
	}
}
