using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("f242cafc062d4c17b2d57e7b15762a88")]
public class ContextActionWardstoneMythicAwakening : IsekaiContextAction
{
	private const string WardstoneScene = "Gray Garrison: The Wardstone";

	public override string GetCaption()
	{
		return "Awards 500 Cosmic Coins and triggers Wardstone Mythic Banter";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("WardstoneMythicAwakening"))
		{
			double num = ConstellationDialogueBanter.RollMultiplier("The Laughing King", out var shoutoutMsg);
			int amount = (int)(500.0 * num);
			if (!string.IsNullOrEmpty(shoutoutMsg))
			{
				ConstellationChatManager.PostLog(shoutoutMsg, "The Laughing King", ConstellationCategory.Subclass, 0, "Gray Garrison: The Wardstone");
			}
			DivineTokens.AddClaimedCoins(amount, "The Laughing King");
		}
		ConstellationChatManager.PostLines(ConstellationDialogueBanter.GetWardstoneMythicAwakeningBanter("The Laughing King"), "The Laughing King", ConstellationCategory.Subclass, "Gray Garrison: The Wardstone");
	}
}
