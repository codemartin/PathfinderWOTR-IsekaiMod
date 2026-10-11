using System.Collections.Generic;
using System.Globalization;
using IsekaiMod.Utilities;

namespace IsekaiMod.Content.Quests;

internal static class PersonalQuestRecord
{
	internal const string StorageName = "IsekaiMod.PersonalQuests";

	internal const string ArchetypeRelicClaimedKey = "ArchetypeRelicClaimed";

	internal const string DevourerPathKey = "Devourer.Path";

	internal static string RelicKey(string itemName)
	{
		return "Relic." + itemName;
	}

	internal static string CoinsKey(string questId, int act)
	{
		return "Coins." + questId + "." + act.ToString(CultureInfo.InvariantCulture);
	}

	internal static string TrialKey(string trialId)
	{
		return "Trial." + trialId;
	}

	internal static string DecreeKey(string decreeId)
	{
		return "Decree." + decreeId;
	}

	internal static bool RelicPaid(IDictionary<string, string> storage, string itemName)
	{
		if (!string.IsNullOrEmpty(itemName))
		{
			return SaveScopedStorage.GetFlag(storage, RelicKey(itemName));
		}
		return false;
	}

	internal static void MarkRelicPaid(IDictionary<string, string> storage, string itemName, bool countsAgainstCap)
	{
		if (storage != null && !string.IsNullOrEmpty(itemName))
		{
			SaveScopedStorage.SetFlag(storage, RelicKey(itemName), value: true);
			if (countsAgainstCap)
			{
				SaveScopedStorage.SetFlag(storage, "ArchetypeRelicClaimed", value: true);
			}
		}
	}

	internal static bool ArchetypeRelicClaimed(IDictionary<string, string> storage)
	{
		return SaveScopedStorage.GetFlag(storage, "ArchetypeRelicClaimed");
	}

	internal static bool CoinsPaid(IDictionary<string, string> storage, string questId, int act)
	{
		if (!string.IsNullOrEmpty(questId))
		{
			return SaveScopedStorage.GetFlag(storage, CoinsKey(questId, act));
		}
		return false;
	}

	internal static void MarkCoinsPaid(IDictionary<string, string> storage, string questId, int act)
	{
		if (storage != null && !string.IsNullOrEmpty(questId))
		{
			SaveScopedStorage.SetFlag(storage, CoinsKey(questId, act), value: true);
		}
	}

	internal static bool TrialWon(IDictionary<string, string> storage, string trialId)
	{
		if (!string.IsNullOrEmpty(trialId))
		{
			return SaveScopedStorage.GetFlag(storage, TrialKey(trialId));
		}
		return false;
	}

	internal static void MarkTrialWon(IDictionary<string, string> storage, string trialId)
	{
		if (storage != null && !string.IsNullOrEmpty(trialId))
		{
			SaveScopedStorage.SetFlag(storage, TrialKey(trialId), value: true);
		}
	}

	internal static bool DecreeDone(IDictionary<string, string> storage, string decreeId)
	{
		if (!string.IsNullOrEmpty(decreeId))
		{
			return SaveScopedStorage.GetFlag(storage, DecreeKey(decreeId));
		}
		return false;
	}

	internal static void MarkDecreeDone(IDictionary<string, string> storage, string decreeId)
	{
		if (storage != null && !string.IsNullOrEmpty(decreeId))
		{
			SaveScopedStorage.SetFlag(storage, DecreeKey(decreeId), value: true);
		}
	}

	internal static string DevourerPath(IDictionary<string, string> storage)
	{
		string text = SaveScopedStorage.Get(storage, "Devourer.Path");
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return null;
	}

	internal static void SetDevourerPath(IDictionary<string, string> storage, string path)
	{
		if (storage != null)
		{
			if (string.IsNullOrEmpty(path))
			{
				storage.Remove("Devourer.Path");
			}
			else
			{
				storage["Devourer.Path"] = path;
			}
		}
	}

	internal static Dictionary<string, string> Find()
	{
		return SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.PersonalQuests");
	}

	internal static Dictionary<string, string> Ensure()
	{
		return SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.PersonalQuests");
	}
}
