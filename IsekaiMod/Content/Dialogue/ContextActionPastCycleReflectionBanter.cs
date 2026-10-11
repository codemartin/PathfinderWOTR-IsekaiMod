using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("c3135aa9bf2444aa940de494c4692090")]
public class ContextActionPastCycleReflectionBanter : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Triggers Constellation Banter reflecting upon past cycles";
	}

	protected override void RunSafely()
	{
		if (ConstellationChatManager.TryClaimSceneReward("PastCycleReflection"))
		{
			DivineTokens.AddClaimedCoins(250, "The Key and the Gate");
		}
		ConstellationChatManager.PostLines(ConstellationDialogueBanter.GetPastCycleRecognitionBanter(), "The Key and the Gate", ConstellationCategory.MetaLoop, "Kenabres Square: Deja Vu");
	}
}
