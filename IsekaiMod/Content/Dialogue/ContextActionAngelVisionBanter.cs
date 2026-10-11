using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("172e4272cb9f43cd8d3b5d28ebd2ae72")]
public class ContextActionAngelVisionBanter : IsekaiContextAction
{
	public bool Devoured;

	private const string AngelVisionScene = "Caves: The Angel's Last Memory";

	public override string GetCaption()
	{
		return "Awards 250 Cosmic Coins and triggers Constellation reaction to the angel's last memory";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("AngelVisionMemory"))
		{
			DivineTokens.AddClaimedCoins(250, "The Key and the Gate");
		}
		ConstellationChatManager.PostLines(ConstellationDialogueBanter.GetAngelVisionBanter(Devoured), "The Key and the Gate", ConstellationCategory.MetaLoop, "Caves: The Angel's Last Memory");
	}
}
