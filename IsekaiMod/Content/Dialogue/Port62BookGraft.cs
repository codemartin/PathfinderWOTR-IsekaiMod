namespace IsekaiMod.Content.Dialogue;

internal sealed class Port62BookGraft
{
	public string Answer;

	public string Scene;

	public string PageGuid;

	public string NextPageGuid;

	public string MirrorsAnswer;

	public int ExpCr = 4;

	public string ClaimKey;

	public int Coins;

	public string SixTwoSponsor;

	public string[] ModItems = new string[0];

	public string Buff;

	public int BuffMinutes;

	public string Feature;

	public string Sponsor => Port62Rewards.Seat(SixTwoSponsor);
}
