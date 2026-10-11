using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Constellations;

[TypeId("14f55e9e86a4447685908914ae7e75c7")]
public class ContextActionWishWealth : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Grants 100,000 gold";
	}

	protected override void RunSafely()
	{
		Game.Instance?.Player?.GainMoney(100000L);
		DivineTokens.Log(CosmicStoreText.WishWealth);
	}
}
