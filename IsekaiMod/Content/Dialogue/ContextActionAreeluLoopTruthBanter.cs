using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("a94afb0deda44f6cbf0f8c2ff4e6ce96")]
public class ContextActionAreeluLoopTruthBanter : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Awards 500 Cosmic Coins and triggers Constellation reaction to the Loop Truth reveal";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("AreeluLoopTruth"))
		{
			DivineTokens.AddClaimedCoins(500, "The Key and the Gate");
		}
		ConstellationChatManager.PostLog(ConstellationText.Localized("Constellation.SubclassReaction", "<color=#5B2A86><b>[The Key and the Gate]</b></color>: <i>\"The veil of causality is pierced. Even the architect's servant now glimpses the infinite recurring corridors of the All-in-One.\"</i>"), "The Key and the Gate", ConstellationCategory.MetaLoop, 0, "Areelu's Laboratory");
	}
}
