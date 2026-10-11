using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("ec81f98c5e3f4e1f99000da7f58a5860")]
public class ContextActionOverlordRevealBanter : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Awards 250 Cosmic Coins and triggers Overlord Reveal Banter";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("OverlordReveal"))
		{
			DivineTokens.AddClaimedCoins(250, "The Pallid Princess");
		}
		ConstellationChatManager.PostLines(ConstellationDialogueBanter.GetOverlordSkeletonRevealBanter(), "The Pallid Princess", ConstellationCategory.Subclass, "Caves: Rescue Anevia");
	}
}
