using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("f87d6bfdf3154f9e9f2fd08927746451")]
public class ContextActionAwardCosmicCoins : IsekaiContextAction
{
	public int Amount = 250;

	public string Sponsor = "The Laughing King";

	public string SceneKey;

	public override string GetCaption()
	{
		return $"Awards {Amount} Cosmic Coins from {Sponsor}";
	}

	protected override void RunSafely()
	{
		if (string.IsNullOrEmpty(SceneKey) || ConstellationChatManager.TryClaimSceneReward(SceneKey))
		{
			DivineTokens.AddClaimedCoins(Amount, Sponsor);
		}
	}
}
