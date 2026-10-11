using System;
using System.Collections.Generic;
using System.Globalization;
using IsekaiMod.Utilities;

namespace IsekaiMod.Content.Constellations;

internal static class CosmicPatronSwitch
{
	internal const string StorageName = "IsekaiMod.Patron";

	internal const string LastSwitchDayKey = "LastSwitchDay";

	internal const int SwitchCost = 500;

	internal const int CooldownDays = 7;

	internal static bool CooldownElapsed(int? lastSwitchDay, int currentDay)
	{
		if (!lastSwitchDay.HasValue)
		{
			return true;
		}
		return currentDay - lastSwitchDay.Value >= 7;
	}

	internal static int DaysRemaining(int? lastSwitchDay, int currentDay)
	{
		if (CooldownElapsed(lastSwitchDay, currentDay))
		{
			return 0;
		}
		return Math.Max(1, 7 - (currentDay - lastSwitchDay.Value));
	}

	internal static int? ReadLastSwitchDay(IDictionary<string, string> storage)
	{
		if (!int.TryParse(SaveScopedStorage.Get(storage, "LastSwitchDay"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return null;
		}
		return result;
	}

	internal static void WriteLastSwitchDay(IDictionary<string, string> storage, int day)
	{
		if (storage != null)
		{
			storage["LastSwitchDay"] = day.ToString(CultureInfo.InvariantCulture);
		}
	}

	internal static bool AllowedNow(out int daysRemaining)
	{
		Dictionary<string, string> storage = SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.Patron");
		daysRemaining = DaysRemaining(ReadLastSwitchDay(storage), GameCalendar.CurrentDay());
		return daysRemaining == 0;
	}

	internal static void RecordSwitch()
	{
		WriteLastSwitchDay(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.Patron"), GameCalendar.CurrentDay());
	}
}
