using IsekaiMod.Utilities;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Dialogue;

internal sealed class Port62Graft
{
	public string Answer;

	public string Scene;

	public string ListGuid;

	public GraftBeat Beat;

	public string ReplySpeaker;

	public bool CompanionSpeaker;

	public bool Narrated;

	public string ContinueCueGuid;

	public string ContinueFallbackGuid;

	public bool FallBackToList;

	public string Gate;

	public string[] ExclusiveWith = new string[0];

	public string[] ExclusiveWithPorted = new string[0];

	public string[] VanillaActionsFrom = new string[0];

	public string[] WhilePlaying = new string[0];

	public string[] UnlessPlaying = new string[0];

	public bool ReconcileHulrunAndRamien;

	public AlignmentShiftDirection Shift = AlignmentShiftDirection.TrueNeutral;

	public int ExpCr = 2;

	public string ClaimKey;

	public int Coins;

	public string SixTwoSponsor;

	public int Gold;

	public string[] VanillaItems = new string[0];

	public string[] ModItems = new string[0];

	public string Buff;

	public int BuffMinutes;

	public string Feature;

	public string Sponsor => Port62Rewards.Seat(SixTwoSponsor);

	public bool HasReward => ClaimKey != null;
}
