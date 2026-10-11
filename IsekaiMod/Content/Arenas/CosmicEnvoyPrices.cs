using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;

namespace IsekaiMod.Content.Arenas;

public static class CosmicEnvoyPrices
{
	public sealed class Entry
	{
		public readonly string Name;

		public readonly string Guid;

		public readonly int Coins;

		public readonly int Stock;

		public Entry(string name, string guid, int coins, int stock = 1)
		{
			Name = name;
			Guid = guid;
			Coins = coins;
			Stock = stock;
		}
	}

	public const double GoldPerCoin = 13.75;

	public const double EnvoyItemPremium = 1.25;

	public static readonly Entry[] Table = new Entry[41]
	{
		new Entry("CosmicCrownOfApotheosisItem", null, Premium(4500)),
		new Entry("CosmicRingOfOmnipresenceItem", null, Premium(4500)),
		new Entry("CosmicMythicPactItem", null, Premium(4000)),
		new Entry("CosmicAstralDragonHornItem", null, Premium(5000)),
		new Entry("ItemSkillBookGrandScholar", null, Premium(800)),
		new Entry("ItemSkillBookPhantomInfiltrator", null, Premium(800)),
		new Entry("ItemSkillBookAbsoluteCharisma", null, Premium(800)),
		new Entry("ItemSkillBookWindWalker", null, Premium(800)),
		new Entry("ItemSkillBookEyeOfProvidence", null, Premium(800)),
		new Entry("ItemSkillBookTechnicLeague", null, Premium(800)),
		new Entry("ManualOfGainfulExercisePlus2", "7a7b1dfa67aa4544aa468d0558b1f667", FromGold(27500)),
		new Entry("ManualOfQuicknessOfActionPlus2", "b8ea6d2f787c7004c9cab9f519f687f8", FromGold(27500)),
		new Entry("ManualOfBodilyHealthPlus2", "2d4d510a69da09c48893f945ec197210", FromGold(27500)),
		new Entry("TomeOfClearThoughtPlus2", "3584c2a2f8b5b1b43ae11128f0ff1583", FromGold(27500)),
		new Entry("TomeOfUnderstandingPlus2", "419a486154514594c99193da785d4302", FromGold(27500)),
		new Entry("TomeOfLeadershipAndInfluencePlus2", "37e2f09923a96234ca486bc9db0b6ad6", FromGold(27500)),
		new Entry("MetamagicRodGreaterMaximize", "d0b7d29c9bea99d4bb25f8f6a29261c5", FromGold(125000)),
		new Entry("MetamagicRodGreaterQuicken", "843ae85d505be8441b9fbb47b04e19e0", FromGold(185000)),
		new Entry("MetamagicRodMasterpiece", "f7887d35eab0a8144b7e6c29794b0865", FromGold(121500)),
		new Entry("MetamagicRodGreaterBolster", "1bf7ae3382d3472e956f691da66598cf", FromGold(76000)),
		new Entry("MetamagicRodGreaterEmpower", "81d504243708f504dbfe3f8f72efdeda", FromGold(85000)),
		new Entry("MetamagicRodGreaterExtend", "9bab0e37c72be78418516e57a5e78a99", FromGold(22000)),
		new Entry("ItemPrimordialAmbrosiaMight", null, 3000, 99),
		new Entry("ItemPrimordialAmbrosiaGrace", null, 3000, 99),
		new Entry("ItemPrimordialAmbrosiaEndurance", null, 3000, 99),
		new Entry("ItemPrimordialAmbrosiaIntellect", null, 3000, 99),
		new Entry("ItemPrimordialAmbrosiaInsight", null, 3000, 99),
		new Entry("ItemPrimordialAmbrosiaMajesty", null, 3000, 99),
		new Entry("ItemPrimordialAmbrosiaCelerity", null, 3000, 99),
		new Entry("ItemPrimordialAmbrosiaEternalRenewal", null, 10000, 99),
		new Entry("ItemNectarEndlessLife", null, 1500, 99),
		new Entry("ItemNectarVoidStalker", null, 4000, 99),
		new Entry("ItemNectarWorldBreaker", null, 4000, 99),
		new Entry("ItemNectarDiamondSoul", null, 3500, 99),
		new Entry("ItemNectarUnbrokenAegis", null, 3500, 99),
		new Entry("ItemDraughtCosmicVelocity", null, 2500, 99),
		new Entry("ItemDraughtImmortalTitan", null, 2500, 99),
		new Entry("ItemDraughtArcaneOmniscience", null, 3000, 99),
		new Entry("ItemDraughtApexPredator", null, 3000, 99),
		new Entry("ItemDraughtAbsoluteSoulAnchor", null, 500, 99),
		new Entry("ItemCosmicWishstone", null, 4000, 99)
	};

	private static Dictionary<string, int> s_ByKey;

	private static Dictionary<string, int> ByKey
	{
		get
		{
			if (s_ByKey == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
				Entry[] table = Table;
				foreach (Entry entry in table)
				{
					dictionary[entry.Guid ?? entry.Name] = entry.Coins;
				}
				s_ByKey = dictionary;
			}
			return s_ByKey;
		}
	}

	private static int Premium(int storePrice)
	{
		return (int)Math.Round((double)storePrice * 1.25);
	}

	public static int FromGold(int gold)
	{
		return Math.Max(100, (int)Math.Round((double)gold / 13.75 / 100.0) * 100);
	}

	public static int GoldValue(int coins)
	{
		return (int)Math.Round((double)coins * 13.75 / 100.0) * 100;
	}

	public static int? Lookup(string blueprintName, string assetGuid)
	{
		if (!string.IsNullOrEmpty(assetGuid) && ByKey.TryGetValue(assetGuid, out var value))
		{
			return value;
		}
		if (!string.IsNullOrEmpty(blueprintName) && ByKey.TryGetValue(blueprintName, out var value2))
		{
			return value2;
		}
		return null;
	}

	public static int ListedCoins(string blueprintName, string assetGuid, long goldCost)
	{
		int? num = Lookup(blueprintName, assetGuid);
		if (num.HasValue)
		{
			return num.Value;
		}
		if (goldCost <= 0)
		{
			return 0;
		}
		return FromGold((int)Math.Min(2147483647L, goldCost));
	}

	public static int DiscountedUnitPrice(int listedCoins, int discountPercent)
	{
		return DivineTokens.DiscountedCost(listedCoins, discountPercent);
	}
}
