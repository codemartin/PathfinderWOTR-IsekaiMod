using Kingmaker.Blueprints;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.Kingdom.Settlements;
using Kingmaker.Kingdom.Settlements.BuildingComponents;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Crusade
{
	internal class IsekaiSettlementBuildings
	{
		public static BlueprintSettlementBuilding OtherworldMegamall;

		public static BlueprintSettlementBuilding CosmicTransmutationMint;

		public static BlueprintSettlementBuilding DimensionalLogisticsBank;

		public static void Add()
		{
			BlueprintSettlementBuilding lodge = BlueprintTools.GetBlueprint<BlueprintSettlementBuilding>("bb8a811660d94094a7cf0a4d0ea0f34b");
			BlueprintSettlementBuilding alchemist = BlueprintTools.GetBlueprint<BlueprintSettlementBuilding>("6ecb3570375b4ff899731851bc6eb676");
			BlueprintSettlementBuilding arsenal = BlueprintTools.GetBlueprint<BlueprintSettlementBuilding>("409d6d11edb04d08bd57f5db56001fb5");
			OtherworldMegamall = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldMegamall", delegate(BlueprintSettlementBuilding bp)
			{
				bp.Name = Helpers.CreateString(Main.IsekaiContext, "OtherworldMegamall.Name", "Otherworld Megamall & Commerce Hub");
				bp.Description = Helpers.CreateString(Main.IsekaiContext, "OtherworldMegamall.Description", "A sprawling multidimensional marketplace combining modern retail corridors, exotic stalls, and planar mercantile exchanges. Infuses the settlement with unprecedented commerce, yielding massive weekly Finances and commercial dividends.");
				bp.MechanicalDescription = Helpers.CreateString(Main.IsekaiContext, "OtherworldMegamall.MechanicalDesc", "Generates +750 Finances and +100 Materials per week, grants +25% global Finance and Materials growth, +5 Morale, and unlocks the repeatable 'Collect Commercial Dividends' decree.");
				bp.SlotSizeX = 2;
				bp.SlotSizeY = 2;
				bp.BuildTime = 15;
				bp.MinLevel = SettlementState.LevelType.Town;
				bp.BuildCost = new KingdomResourcesAmount
				{
					m_Finances = 200,
					m_Materials = 100
				};
				if (lodge != null)
				{
					bp.CompletedPrefab = lodge.CompletedPrefab;
					bp.UnfinishedPrefab = lodge.UnfinishedPrefab;
				}
				bp.AddComponent(delegate(BuildingResourceGrowthIncrease c)
				{
					c.ResourcesAmount = new KingdomResourcesAmount
					{
						m_Finances = 750,
						m_Materials = 100
					};
				});
				bp.AddComponent(delegate(BuildingResourceGrowthGlobalIncrease c)
				{
					c.FinanceModifier = 25;
					c.BasicsModifier = 25;
				});
				bp.AddComponent(delegate(BuildingMoraleChangeBonus c)
				{
					c.Positive = true;
					c.FlatBonus = 5;
				});
			});
			CosmicTransmutationMint = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicTransmutationMint", delegate(BlueprintSettlementBuilding bp)
			{
				bp.Name = Helpers.CreateString(Main.IsekaiContext, "CosmicTransmutationMint.Name", "Cosmic Transmutation Mint");
				bp.Description = Helpers.CreateString(Main.IsekaiContext, "CosmicTransmutationMint.Description", "An alchemical coinage foundry powered by otherworldly transmutative magic. Converts base ores into celestial currency and mints authenticated Cosmic Coins directly into the crusade's accounts.");
				bp.MechanicalDescription = Helpers.CreateString(Main.IsekaiContext, "CosmicTransmutationMint.MechanicalDesc", "Generates +500 Finances per week, grants +15% global Finance growth, and produces Cosmic Coins.");
				bp.SlotSizeX = 1;
				bp.SlotSizeY = 1;
				bp.BuildTime = 10;
				bp.MinLevel = SettlementState.LevelType.Village;
				bp.BuildCost = new KingdomResourcesAmount
				{
					m_Finances = 150,
					m_Materials = 50
				};
				if (alchemist != null)
				{
					bp.CompletedPrefab = alchemist.CompletedPrefab;
					bp.UnfinishedPrefab = alchemist.UnfinishedPrefab;
				}
				bp.AddComponent(delegate(BuildingResourceGrowthIncrease c)
				{
					c.ResourcesAmount = new KingdomResourcesAmount
					{
						m_Finances = 500
					};
				});
				bp.AddComponent(delegate(BuildingResourceGrowthGlobalIncrease c)
				{
					c.FinanceModifier = 15;
				});
			});
			DimensionalLogisticsBank = Helpers.CreateBlueprint(Main.IsekaiContext, "DimensionalLogisticsBank", delegate(BlueprintSettlementBuilding bp)
			{
				bp.Name = Helpers.CreateString(Main.IsekaiContext, "DimensionalLogisticsBank.Name", "Dimensional Logistics Bank");
				bp.Description = Helpers.CreateString(Main.IsekaiContext, "DimensionalLogisticsBank.Description", "An interplanar banking institution utilizing spatial distortion vaults to manage crusade finances, loans, and treasury storage across dimensions.");
				bp.MechanicalDescription = Helpers.CreateString(Main.IsekaiContext, "DimensionalLogisticsBank.MechanicalDesc", "Grants +100% adjacency resource bonuses to all neighboring buildings, increases global resource growth by +15%, and unlocks repeatable capital reinvestment decrees.");
				bp.SlotSizeX = 1;
				bp.SlotSizeY = 2;
				bp.BuildTime = 12;
				bp.MinLevel = SettlementState.LevelType.Village;
				bp.BuildCost = new KingdomResourcesAmount
				{
					m_Finances = 175,
					m_Materials = 75
				};
				if (arsenal != null)
				{
					bp.CompletedPrefab = arsenal.CompletedPrefab;
					bp.UnfinishedPrefab = arsenal.UnfinishedPrefab;
				}
				bp.AddComponent(delegate(BuildingAdjacentResourceIncrease c)
				{
					c.Distance = BuildingAdjacencyBonus.DistanceRequirementType.Adjacent;
					c.BonusPercent = 100;
					c.FinanceModifier = 25;
					c.BasicsModifier = 25;
				});
				bp.AddComponent(delegate(BuildingResourceGrowthGlobalIncrease c)
				{
					c.FinanceModifier = 15;
					c.BasicsModifier = 15;
					c.FavorsModifier = 10;
				});
			});
			KingdomRoot blueprint = BlueprintTools.GetBlueprint<KingdomRoot>("f6bd33651fb0ad64d8fa659d3df6e7df");
			if (blueprint != null && blueprint.m_Buildings != null)
			{
				blueprint.m_Buildings = blueprint.m_Buildings.AppendToArray(OtherworldMegamall.ToReference<BlueprintSettlementBuildingReference>(), CosmicTransmutationMint.ToReference<BlueprintSettlementBuildingReference>(), DimensionalLogisticsBank.ToReference<BlueprintSettlementBuildingReference>());
			}
		}
	}
}
