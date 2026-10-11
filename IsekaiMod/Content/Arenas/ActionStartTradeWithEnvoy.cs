using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("7b00e877e1aa4861a4b920db3e5d9c2d")]
public class ActionStartTradeWithEnvoy : IsekaiGameAction
{
	public override string GetCaption()
	{
		return "Start trade with Astral Coin Envoy";
	}

	protected override void RunSafely()
	{
		HubTrade.Start("AstralCoinEnvoyUnit", CosmicRelicsVendorTable.Table);
	}
}
