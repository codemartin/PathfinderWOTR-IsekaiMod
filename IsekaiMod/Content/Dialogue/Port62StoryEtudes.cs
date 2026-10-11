using Kingmaker;
using Kingmaker.AreaLogic.Etudes;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62StoryEtudes
{
	internal const string HulrunRedeemed = "eb5658cc38d4432a9288cf7a0f273321";

	internal const string HurlunAgreedToSpareRamien = "b5d3ae57a01a07440a3089452d91a391";

	internal const string SwordOfValorActiveIz = "edb31f5ddae9420f8355d7e34faf6522";

	internal const string PlayerFoughtAlongsideWithGalfrey = "caf51287bfd0152489d3a6fe4700060c";

	internal static readonly string[] Forced = new string[2] { "eb5658cc38d4432a9288cf7a0f273321", "edb31f5ddae9420f8355d7e34faf6522" };

	internal static readonly string[] NotForced = new string[2] { "b5d3ae57a01a07440a3089452d91a391", "caf51287bfd0152489d3a6fe4700060c" };

	internal static void Start(string etudeGuid)
	{
		BlueprintEtude blueprint = BlueprintTools.GetBlueprint<BlueprintEtude>(etudeGuid);
		EtudesSystem etudesSystem = Game.Instance?.Player?.EtudesSystem;
		if (blueprint != null && etudesSystem != null && !etudesSystem.EtudeIsStarted(blueprint))
		{
			etudesSystem.StartEtude(blueprint);
		}
	}
}
