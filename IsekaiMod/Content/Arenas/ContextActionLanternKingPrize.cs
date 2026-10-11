using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("b4c6e8a0d2f14b3e95a7c9e1f3b5d7a2")]
public class ContextActionLanternKingPrize : IsekaiContextAction
{
	public bool Gold;

	public override string GetCaption()
	{
		if (!Gold)
		{
			return $"The Lantern King's prize: {1500} Cosmic Coins, once per save";
		}
		return $"The Lantern King's prize: {25000} gold, once per save";
	}

	protected override void RunSafely()
	{
		if (!LanternKingReveal.PrizeCanBePaid(Gold, ConstellationSettings.Enabled))
		{
			DivineTokens.Log(CosmicStoreText.ConstellationsDisabled);
		}
		else if (LanternKingReveal.TryClaimPrize(ColiseumProgress.Ensure()))
		{
			if (Gold)
			{
				Game.Instance?.Player?.GainMoney(25000L);
			}
			else
			{
				DivineTokens.AddCoinsWithoutLoopMultiplier(1500, "The Laughing King");
			}
		}
	}
}
