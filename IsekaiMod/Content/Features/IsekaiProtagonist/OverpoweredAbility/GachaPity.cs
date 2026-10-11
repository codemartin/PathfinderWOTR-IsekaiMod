using System;
using System.Collections.Generic;
using System.Globalization;
using IsekaiMod.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal static class GachaPity
{
	internal const string StorageName = "IsekaiMod.Gacha";

	internal const string PityKey = "Pity";

	internal const string DuplicateJackpotsKey = "DuplicateJackpots";

	internal const int PityThreshold = 100;

	internal const int DuplicateJackpotCoins = 2000;

	internal const int MaxPaidDuplicates = 3;

	internal const int FullGoldLevel = 20;

	internal const int MinGoldPercent = 10;

	internal static int ScaleGold(int listedGold, int characterLevel)
	{
		if (listedGold <= 0)
		{
			return 0;
		}
		int num = Math.Max(10, Math.Min(100, characterLevel * 100 / 20));
		return (int)((long)listedGold * (long)num / 100);
	}

	internal static int Next(int current, bool wasUR)
	{
		if (current < 0)
		{
			current = 0;
		}
		if (!wasUR)
		{
			return current + 1;
		}
		return 0;
	}

	internal static bool PityDue(int pityAfterIncrement)
	{
		return pityAfterIncrement >= 100;
	}

	internal static int DuplicateBonus(int alreadyPaid)
	{
		if (alreadyPaid < 3)
		{
			return 2000;
		}
		return 0;
	}

	internal static Dictionary<string, string> Ensure()
	{
		return SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.Gacha");
	}

	internal static int Read(IDictionary<string, string> storage, string key)
	{
		return Math.Max(0, SaveScopedStorage.GetInt(storage, key));
	}

	internal static void Write(IDictionary<string, string> storage, string key, int value)
	{
		if (storage != null)
		{
			storage[key] = Math.Max(0, value).ToString(CultureInfo.InvariantCulture);
		}
	}
}
