using System;
using System.Collections.Generic;
using IsekaiMod.Config;
using Kingmaker.Blueprints;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.Kingdom.Settlements;
using Kingmaker.Kingdom.Settlements.BuildingComponents;
using Kingmaker.ResourceLinks;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Crusade;

internal class IsekaiSettlementBuildings
{
	public static BlueprintSettlementBuilding OtherworldMegamall;

	public static BlueprintSettlementBuilding CosmicTransmutationMint;

	public static BlueprintSettlementBuilding DimensionalLogisticsBank;

	internal static readonly Dictionary<string, BuildingTerms> Terms = new Dictionary<string, BuildingTerms>(StringComparer.Ordinal)
	{
		["OtherworldMegamall"] = new BuildingTerms
		{
			CostFinances = 5000,
			CostMaterials = 500,
			BuildDays = 5,
			SlotSizeX = 2,
			SlotSizeY = 2,
			MinLevel = SettlementState.LevelType.Town,
			FinancesPerDay = 150,
			MaterialsPerDay = 10,
			GlobalFinancePercent = 10,
			Logistics = 16000,
			ModelSource = "Palace",
			CompletedPrefabLink = "d92b1e9ef7aad204490e469287fcfc32",
			UnfinishedPrefabLink = "96b9cc0160a898d478165dcd590746fe"
		},
		["CosmicTransmutationMint"] = new BuildingTerms
		{
			CostFinances = 1500,
			CostMaterials = 150,
			BuildDays = 3,
			MinLevel = SettlementState.LevelType.Village,
			FinancesPerDay = 75,
			Logistics = 4500,
			ModelSource = "EngineersWorkshop",
			CompletedPrefabLink = "bca1bdb9032a70b4488529b6978976ac",
			UnfinishedPrefabLink = "294452b45ee6a9848a72ce20f2205856"
		},
		["DimensionalLogisticsBank"] = new BuildingTerms
		{
			CostFinances = 2500,
			CostMaterials = 250,
			BuildDays = 3,
			MinLevel = SettlementState.LevelType.Town,
			GlobalFinancePercent = 10,
			GlobalMaterialsPercent = 10,
			Logistics = 4500,
			ModelSource = "FightersPitCapital",
			CompletedPrefabLink = "3472b3aed89edfc41a52f93ecb0b8c15",
			UnfinishedPrefabLink = "294452b45ee6a9848a72ce20f2205856"
		}
	};

	public static void Add()
	{
		OtherworldMegamall = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldMegamall", delegate(BlueprintSettlementBuilding bp)
		{
			bp.Name = Helpers.CreateString(Main.IsekaiContext, "OtherworldMegamall.Name", "Otherworld Megamall & Commerce Hub");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "OtherworldMegamall.Description", "A sprawling multidimensional marketplace combining modern retail corridors, exotic stalls, and planar mercantile exchanges. Its trade brings the crusade a steady daily income.");
			bp.MechanicalDescription = Helpers.CreateString(Main.IsekaiContext, "OtherworldMegamall.MechanicalDesc", "Generates +150 Finances and +10 Materials per day and raises the crusade's Finance income by 10%. Only one can be built in the whole crusade.");
			Configure(bp, "OtherworldMegamall");
		});
		CosmicTransmutationMint = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicTransmutationMint", delegate(BlueprintSettlementBuilding bp)
		{
			bp.Name = Helpers.CreateString(Main.IsekaiContext, "CosmicTransmutationMint.Name", "Cosmic Transmutation Mint");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "CosmicTransmutationMint.Description", "An alchemical coinage foundry powered by otherworldly transmutative magic. It turns base ores into coin for the crusade's treasury.");
			bp.MechanicalDescription = Helpers.CreateString(Main.IsekaiContext, "CosmicTransmutationMint.MechanicalDesc", "Generates +75 Finances per day. Only one can be built in the whole crusade.");
			Configure(bp, "CosmicTransmutationMint");
		});
		DimensionalLogisticsBank = Helpers.CreateBlueprint(Main.IsekaiContext, "DimensionalLogisticsBank", delegate(BlueprintSettlementBuilding bp)
		{
			bp.Name = Helpers.CreateString(Main.IsekaiContext, "DimensionalLogisticsBank.Name", "Dimensional Logistics Bank");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, "DimensionalLogisticsBank.Description", "An interplanar banking institution utilizing spatial distortion vaults to manage crusade finances, loans, and treasury storage across dimensions.");
			bp.MechanicalDescription = Helpers.CreateString(Main.IsekaiContext, "DimensionalLogisticsBank.MechanicalDesc", "Raises the crusade's Finance and Materials income by 10%. Only one can be built in the whole crusade.");
			Configure(bp, "DimensionalLogisticsBank");
		});
		KingdomRoot blueprint = BlueprintTools.GetBlueprint<KingdomRoot>("f6bd33651fb0ad64d8fa659d3df6e7df");
		if (blueprint != null && blueprint.m_Buildings != null)
		{
			blueprint.m_Buildings = blueprint.m_Buildings.AppendToArray(OtherworldMegamall.ToReference<BlueprintSettlementBuildingReference>(), CosmicTransmutationMint.ToReference<BlueprintSettlementBuildingReference>(), DimensionalLogisticsBank.ToReference<BlueprintSettlementBuildingReference>());
		}
	}

	private static void Configure(BlueprintSettlementBuilding bp, string name)
	{
		BuildingTerms terms = Terms[name];
		bp.SlotSizeX = terms.SlotSizeX;
		bp.SlotSizeY = terms.SlotSizeY;
		bp.BuildTime = terms.BuildDays;
		bp.MinLevel = terms.MinLevel;
		bp.BuildCost = new KingdomResourcesAmount
		{
			m_Finances = terms.CostFinances,
			m_Materials = terms.CostMaterials
		};
		bp.StatChanges = new KingdomStats.Changes();
		bp.StatChanges.m_Changes[1] = terms.Logistics;
		bp.CompletedPrefab = new PrefabLink
		{
			AssetId = terms.CompletedPrefabLink
		};
		bp.UnfinishedPrefab = new PrefabLink
		{
			AssetId = terms.UnfinishedPrefabLink
		};
		bp.AddComponent(delegate(OncePerCrusadeOfferedRestriction c)
		{
			c.Content = OfferedContent.IsekaiProtagonist;
		});
		if (terms.FinancesPerDay > 0 || terms.MaterialsPerDay > 0)
		{
			bp.AddComponent(delegate(BuildingResourceGrowthIncrease c)
			{
				c.ResourcesAmount = new KingdomResourcesAmount
				{
					m_Finances = terms.FinancesPerDay,
					m_Materials = terms.MaterialsPerDay
				};
			});
		}
		if (terms.GlobalFinancePercent > 0 || terms.GlobalMaterialsPercent > 0)
		{
			bp.AddComponent(delegate(BuildingResourceGrowthGlobalIncrease c)
			{
				c.FinanceModifier = terms.GlobalFinancePercent;
				c.BasicsModifier = terms.GlobalMaterialsPercent;
			});
		}
	}
}
