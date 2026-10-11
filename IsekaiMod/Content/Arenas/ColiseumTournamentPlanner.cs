using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Content.Arenas;

internal static class ColiseumTournamentPlanner
{
	internal static void Plan(ColiseumTournamentRecord record, IEnumerable<ColiseumUnitSnapshot> present, bool inColiseum, ICollection<string> rosterGuids, ICollection<string> legacySpawnedIds, out List<string> resume, out List<string> destroy)
	{
		resume = new List<string>();
		destroy = new List<string>();
		List<ColiseumUnitSnapshot> list = (present ?? Enumerable.Empty<ColiseumUnitSnapshot>()).Where((ColiseumUnitSnapshot u) => !string.IsNullOrEmpty(u.UniqueId)).ToList();
		if (record != null)
		{
			HashSet<string> hashSet = new HashSet<string>(record.Enemies ?? new List<string>(), StringComparer.Ordinal);
			foreach (ColiseumUnitSnapshot item in list)
			{
				if (hashSet.Contains(item.UniqueId) && !item.IsDead)
				{
					resume.Add(item.UniqueId);
				}
			}
		}
		HashSet<string> hashSet2 = new HashSet<string>(resume, StringComparer.Ordinal);
		HashSet<string> hashSet3 = new HashSet<string>(StringComparer.Ordinal);
		if (record?.Spawned != null)
		{
			hashSet3.UnionWith(record.Spawned);
		}
		if (record?.Enemies != null)
		{
			hashSet3.UnionWith(record.Enemies);
		}
		if (legacySpawnedIds != null)
		{
			hashSet3.UnionWith(legacySpawnedIds);
		}
		foreach (ColiseumUnitSnapshot item2 in list)
		{
			if (!hashSet2.Contains(item2.UniqueId) && (hashSet3.Contains(item2.UniqueId) || item2.SpawnKind == "ColiseumContestant" || (inColiseum && item2.SpawnKind == null && item2.IsEnemyFaction && rosterGuids != null && !string.IsNullOrEmpty(item2.BlueprintGuid) && rosterGuids.Contains(item2.BlueprintGuid))) && !KeepForLooting(item2))
			{
				destroy.Add(item2.UniqueId);
			}
		}
	}

	internal static bool KeepForLooting(ColiseumUnitSnapshot unit)
	{
		if (unit.IsDead)
		{
			return unit.CarriesLoot;
		}
		return false;
	}

	internal static List<string> SweepSpawned(IEnumerable<ColiseumUnitSnapshot> spawned)
	{
		return (from u in spawned ?? Enumerable.Empty<ColiseumUnitSnapshot>()
			where !string.IsNullOrEmpty(u.UniqueId) && !KeepForLooting(u)
			select u.UniqueId).ToList();
	}
}
