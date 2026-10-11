using System;

namespace IsekaiMod.Content.Arenas;

internal static class ColiseumRewards
{
	internal const int ReplayExperienceAndGoldDivisor = 10;

	internal const int ReplayCoinPercent = 25;

	internal const int ReplayCoinFloor = 25;

	internal const int RepeatReplayCoins = 25;

	internal const int RepeatReplayDivisor = 100;

	internal static ColiseumPurse For(int experience, int gold, int coins, bool firstClear, bool paidReplay = true)
	{
		if (firstClear)
		{
			return new ColiseumPurse
			{
				Experience = Math.Max(0, experience),
				Gold = Math.Max(0, gold),
				Coins = Math.Max(0, coins)
			};
		}
		if (!paidReplay)
		{
			return new ColiseumPurse
			{
				Experience = Share(experience, 100),
				Gold = Share(gold, 100),
				Coins = ((coins > 0) ? Math.Min(25, coins) : 0)
			};
		}
		return new ColiseumPurse
		{
			Experience = Tenth(experience),
			Gold = Tenth(gold),
			Coins = ((coins > 0) ? Math.Max(25, (int)((long)coins * 25L / 100)) : 0)
		};
	}

	internal static ColiseumPurse For(ColiseumCup cup, bool firstClear, bool paidReplay = true)
	{
		if (cup != null)
		{
			return For(cup.Experience, cup.Gold, cup.Coins, firstClear, paidReplay);
		}
		return default(ColiseumPurse);
	}

	private static int Tenth(int full)
	{
		return Share(full, 10);
	}

	private static int Share(int full, int divisor)
	{
		if (full <= 0 || divisor <= 0)
		{
			return 0;
		}
		return Math.Max(1, full / divisor);
	}
}
