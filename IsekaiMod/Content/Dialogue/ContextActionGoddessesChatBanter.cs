using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("05dfcccabe78484e9c947aeae5b41bb9")]
public class ContextActionGoddessesChatBanter : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Triggers Constellation Live Chat Banter at the Goddesses Summit";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("GoddessesChat"))
		{
			DivineTokens.AddClaimedCoins(500, "The Laughing King");
		}
		ConstellationChatManager.PostLog(ConstellationText.Localized("Constellation.SubclassReaction", "<color=#5B2A86><b>[The Laughing King]</b></color>: <i>\"HAHAHA! Standing between the Inheritor and the Demon Queen and checking the chat poll! Maximum ratings unlocked!\"</i>"), "The Laughing King", ConstellationCategory.Subclass, 0, "Drezen: The Goddesses Summit");
		ConstellationChatManager.PostLog(ConstellationText.Localized("Constellation.SubclassReaction", "<color=#133E71><b>[The Song of the Spheres]</b></color>: <i>\"Freedom is never chosen from someone else's menu. Dance on your own stage, starlight!\"</i>"), "The Song of the Spheres", ConstellationCategory.Subclass, 0, "Drezen: The Goddesses Summit");
	}
}
