using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("532252c52ebf42aa98512e95ea1bec07")]
public class IsekaiActionIzTriadMultiFrontDefense : IsekaiGameAction
{
	internal const string ClaimKey = "Port62:IzTriadMultiFrontDefense";

	internal const int Coins = 500;

	internal const string Sponsor = "The Inheritor";

	public override string GetCaption()
	{
		return "Holds Queen Galfrey, Irabeth, and the Sword of Valor together at Iz";
	}

	protected override void RunSafely()
	{
		Port62StoryEtudes.Start("edb31f5ddae9420f8355d7e34faf6522");
		if (ConstellationChatManager.TryClaimSceneReward("Port62:IzTriadMultiFrontDefense"))
		{
			DivineTokens.AddClaimedCoins(500, "The Inheritor");
			ConstellationChatManager.PostLog(ConstellationText.Localized("Constellation.Port62.StoryLine", "<color=#5B2A86><b>[The Golden Vow of Iz]</b></color> The Commander vows to hold Queen Galfrey, Irabeth, and the Sword of Valor together at Iz!"), "The Inheritor", ConstellationCategory.Quest, DivineTokens.ShownCoins(500), "Port62:IzTriadMultiFrontDefense");
		}
	}
}
