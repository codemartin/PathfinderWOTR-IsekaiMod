using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("06cba280b3054e91ac5e9b879f66874d")]
public class ContextActionDesnaNatureCommunion : IsekaiContextAction
{
	public int Amount = 250;

	public override string GetCaption()
	{
		return "Awards 250 Cosmic Coins and triggers Elysian Nature Communion";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("DesnaNatureCommunion"))
		{
			DivineTokens.AddClaimedCoins(Amount, "The Song of the Spheres");
		}
		ConstellationChatManager.PostLog(ConstellationText.Localized("Constellation.SubclassReaction", "<color=#133E71><b>[The Song of the Spheres]</b></color>: <i>\"The spirits of ancient Sarkoris and the beasts of Elysium heed the gentle rhythm of your living jelly core.\"</i>"), "The Song of the Spheres", ConstellationCategory.Subclass, 0, "Elysian Nature Communion");
	}
}
