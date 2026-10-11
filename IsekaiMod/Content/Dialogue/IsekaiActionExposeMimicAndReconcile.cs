using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("781a2357bbbf42019d44a5fc083a4581")]
public class IsekaiActionExposeMimicAndReconcile : IsekaiGameAction
{
	internal const string ClaimKey = "Port62:ExposeMimicAndReconcile";

	internal const int Coins = 400;

	internal const string Sponsor = "The Song of the Spheres";

	public override string GetCaption()
	{
		return "Reconciles Hulrun and Ramien through exposure of demonic mimicry";
	}

	protected override void RunSafely()
	{
		Port62StoryEtudes.Start("eb5658cc38d4432a9288cf7a0f273321");
		if (ConstellationChatManager.TryClaimSceneReward("Port62:ExposeMimicAndReconcile"))
		{
			DivineTokens.AddClaimedCoins(400, "The Song of the Spheres");
			ConstellationChatManager.PostLog(ConstellationText.Localized("Constellation.Port62.StoryLine", "<color=#133E71><b>[Harmony of Kenabres]</b></color> Prelate Hulrun and High Priest Ramien join forces under your authority!"), "The Song of the Spheres", ConstellationCategory.Quest, DivineTokens.ShownCoins(400), "Port62:ExposeMimicAndReconcile");
		}
	}
}
