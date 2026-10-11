using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;

namespace IsekaiMod.Content.Constellations;

[TypeId("2a6c8e0f1b3d4f5a7c9e1a3b5d7f9b12")]
public class ContextActionWishstoneWealth : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Miracle of Wealth: grants 500,000 gold";
	}

	protected override void RunSafely()
	{
		Player player = Game.Instance?.Player;
		if (player != null)
		{
			player.GainMoney(500000L);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(ConstellationText.Localized("CosmicWishstone.Miracle", "<color=#004604><b>[Cosmic Wishstone] Miracle of Wealth!</b></color> 500,000 Gold has manifested directly into your party treasury from the outer realms!"));
			});
		}
	}
}
