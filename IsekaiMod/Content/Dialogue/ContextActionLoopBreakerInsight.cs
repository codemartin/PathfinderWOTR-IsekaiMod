using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("2d42cbbadeca4aff80c4458f350102e1")]
public class ContextActionLoopBreakerInsight : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Awards 1,000 Cosmic Coins and weakens Yog-Sothoth's causal anchors";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("LoopBreakerInsight"))
		{
			DivineTokens.AddClaimedCoins(1000, "The Laughing King");
		}
		ConstellationChatManager.PostLog(ConstellationText.Localized("Constellation.SubclassReaction", "<color=#5B2A86><b>[Loop-Breaker Conviction]</b></color> <i>You have defied both celestial dogma and abyssal temptation! The cosmic anchors of Yog-Sothoth tremble before your mortal resolve!</i>"), "The Laughing King", ConstellationCategory.MetaLoop, 0, "Drezen: The Goddesses Summit");
		ConstellationChatManager.RecordLoopDefiance();
	}
}
