namespace IsekaiMod.Content.Crusade;

internal sealed class DecreeTerms
{
	public int Finances;

	public int Materials;

	public int CosmicCoins;

	public int PersonalGold;

	public int StartFinances;

	public int StartMaterials;

	public int Days;

	public int Cooldown;

	public int MaxRuns;

	public DecreeGate Gate;

	public bool PaysOncePerSave;

	public bool Repeatable => MaxRuns > 0;

	public int LedgerCap
	{
		get
		{
			if (MaxRuns <= 0)
			{
				return PaysOncePerSave ? 1 : 0;
			}
			return MaxRuns;
		}
	}
}
