using Kingmaker.Kingdom.Settlements;

namespace IsekaiMod.Content.Crusade;

internal sealed class BuildingTerms
{
	public int CostFinances;

	public int CostMaterials;

	public int BuildDays;

	public int SlotSizeX = 1;

	public int SlotSizeY = 1;

	public SettlementState.LevelType MinLevel;

	public int FinancesPerDay;

	public int MaterialsPerDay;

	public int GlobalFinancePercent;

	public int GlobalMaterialsPercent;

	public int Logistics;

	public string CompletedPrefabLink;

	public string UnfinishedPrefabLink;

	public string ModelSource;
}
