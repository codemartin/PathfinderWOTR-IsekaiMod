using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("8d9dfc64a9304484a4ff664290416bd1")]
public class ContextActionSlimeRevealBanter : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Awards 250 Cosmic Coins and triggers Slime Reveal Banter";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("SlimeReveal"))
		{
			DivineTokens.AddClaimedCoins(250, "The Laughing King");
		}
		ConstellationChatManager.PostLines(ConstellationDialogueBanter.GetSlimeRevealBanter(), "The Laughing King", ConstellationCategory.Subclass, "Caves: Rescue Anevia");
	}
}
