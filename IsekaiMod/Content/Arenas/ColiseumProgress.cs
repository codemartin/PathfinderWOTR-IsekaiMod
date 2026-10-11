using System;
using System.Collections.Generic;
using System.Globalization;
using IsekaiMod.Utilities;

namespace IsekaiMod.Content.Arenas;

internal static class ColiseumProgress
{
	internal const string StorageName = "IsekaiMod.Coliseum";

	internal const string ReturnEnterPointKey = "ReturnEnterPoint";

	internal const string ReturnChapterKey = "ReturnChapter";

	internal const string ArrivalMapKey = "ArrivalMap";

	internal static string ClearedKey(string cupId)
	{
		return "Cleared." + cupId;
	}

	internal static string ClearsKey(string cupId)
	{
		return "Clears." + cupId;
	}

	internal static string ReplayDayKey(string cupId)
	{
		return "ReplayDay." + cupId;
	}

	internal static bool TryClaimPaidReplay(IDictionary<string, string> storage, string cupId, int day)
	{
		if (storage == null || string.IsNullOrEmpty(cupId))
		{
			return true;
		}
		string key = ReplayDayKey(cupId);
		if (int.TryParse(SaveScopedStorage.Get(storage, key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) && day <= result)
		{
			return false;
		}
		storage[key] = day.ToString(CultureInfo.InvariantCulture);
		return true;
	}

	internal static bool IsCleared(IDictionary<string, string> storage, string cupId)
	{
		if (!string.IsNullOrEmpty(cupId))
		{
			return SaveScopedStorage.GetFlag(storage, ClearedKey(cupId));
		}
		return false;
	}

	internal static int ClearCount(IDictionary<string, string> storage, string cupId)
	{
		if (!string.IsNullOrEmpty(cupId))
		{
			return Math.Max(0, SaveScopedStorage.GetInt(storage, ClearsKey(cupId)));
		}
		return 0;
	}

	internal static bool RecordClear(IDictionary<string, string> storage, string cupId)
	{
		if (storage == null || string.IsNullOrEmpty(cupId))
		{
			return false;
		}
		bool result = !IsCleared(storage, cupId);
		SaveScopedStorage.SetFlag(storage, ClearedKey(cupId), value: true);
		storage[ClearsKey(cupId)] = (ClearCount(storage, cupId) + 1).ToString(CultureInfo.InvariantCulture);
		return result;
	}

	internal static void WriteReturnPoint(IDictionary<string, string> storage, string enterPointGuid, int chapter)
	{
		if (storage != null)
		{
			if (string.IsNullOrEmpty(enterPointGuid))
			{
				storage.Remove("ReturnEnterPoint");
				storage.Remove("ReturnChapter");
			}
			else
			{
				storage["ReturnEnterPoint"] = enterPointGuid;
				storage["ReturnChapter"] = chapter.ToString(CultureInfo.InvariantCulture);
			}
		}
	}

	internal static string ResolveReturnPoint(string recordedEnterPoint, int? recordedChapter, int currentChapter)
	{
		if (string.IsNullOrEmpty(recordedEnterPoint) || !recordedChapter.HasValue || recordedChapter.Value != currentChapter)
		{
			return ActionEnterColiseum.ResolveReturnEnterPoint(null, currentChapter);
		}
		return recordedEnterPoint;
	}

	internal static void ReadRecordedReturnPoint(IDictionary<string, string> storage, out string enterPointGuid, out int? chapter)
	{
		enterPointGuid = SaveScopedStorage.Get(storage, "ReturnEnterPoint");
		string s = SaveScopedStorage.Get(storage, "ReturnChapter");
		chapter = (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? new int?(result) : ((int?)null));
	}

	internal static string ReadReturnPoint(IDictionary<string, string> storage, int currentChapter)
	{
		ReadRecordedReturnPoint(storage, out var enterPointGuid, out var chapter);
		return ResolveReturnPoint(enterPointGuid, chapter, currentChapter);
	}

	internal static Dictionary<string, string> Find()
	{
		return SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.Coliseum");
	}

	internal static Dictionary<string, string> Ensure()
	{
		return SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.Coliseum");
	}

	internal static bool IsClearedInSave(string cupId)
	{
		return IsCleared(Find(), cupId);
	}
}
