using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("72e38df13af14d91951b8e7fd5684308")]
public class ContextActionBaphometDefianceBanter : IsekaiContextAction
{
	public string Line;

	public override string GetCaption()
	{
		return "Awards 500 Cosmic Coins for humiliating Baphomet";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("BaphometDefiance"))
		{
			DivineTokens.AddClaimedCoins(500, "The Laughing King");
		}
		string text = (string.IsNullOrEmpty(Line) ? ConstellationText.Localized("Constellation.SubclassReaction", "Did the Commander just insult the Lord of the Labyrinth to his face?! Absolute legendary performance!") : ConstellationText.LocalizeIfKnown(Line));
		ConstellationChatManager.PostLog(ConstellationText.LocalizedFormat("Constellation.SubclassReaction", "<color=#5B2A86><b>[The Laughing King]</b></color>: <i>\"{0}\"</i>", text), "The Laughing King", ConstellationCategory.Subclass, 0, "Colyphyr: Baphomet");
	}
}
