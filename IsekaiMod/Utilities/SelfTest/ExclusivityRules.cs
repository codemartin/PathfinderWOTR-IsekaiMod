using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Utilities.SelfTest;

public static class ExclusivityRules
{
	public sealed class DlcRequirement
	{
		public string DlcGuid { get; }

		public string Label { get; }

		public DlcRequirement(string dlcGuid, string label)
		{
			DlcGuid = dlcGuid;
			Label = label;
		}
	}

	public const string PetSelectionName = "IsekaiPetSelection";

	public const string HeritageSelectionName = "IsekaiHumanHeritageSelection";

	public const string LegacySelectionMarker = "Legacy";

	public static readonly IReadOnlyList<string[]> RequiredPairs = new List<string[]>
	{
		new string[3] { "DevourerArchetype", "HeteromorphicOverlordHeritage", "a Slime cannot be a Heteromorphic Overlord" },
		new string[3] { "DevourerArchetype", "IsekaiVampireHeritage", "the Devourer excludes the Vampire heritage" },
		new string[3] { "DevourerArchetype", "IsekaiDragonLordHeritage", "the Devourer excludes the Dragon Lord heritage" },
		new string[3] { "DevourerArchetype", "IsekaiWerewolfHeritage", "the Devourer excludes the Werewolf heritage" },
		new string[3] { "OverlordArchetype", "SlimeReincarnateHeritage", "an Overlord cannot be a Slime Reincarnate" },
		new string[3] { "OverlordArchetype", "IsekaiVampireHeritage", "the Overlord excludes the Vampire heritage" },
		new string[3] { "OverlordArchetype", "IsekaiDragonLordHeritage", "the Overlord excludes the Dragon Lord heritage" },
		new string[3] { "OverlordArchetype", "IsekaiWerewolfHeritage", "the Overlord excludes the Werewolf heritage" },
		new string[3] { "MartialGodArchetype", "KineticLegacy", "Martial God cannot take the Kinetic legacy" },
		new string[3] { "BarbarianLegacy", "SkaldBaseLegacy", "Barbarian and Skald share the rage power pool" },
		new string[3] { "BarbarianLegacy", "SkaldVoiceLegacy", "Barbarian and Skald share the rage power pool" },
		new string[3] { "BarbarianLegacy", "SkaldSilverTongueLegacy", "Barbarian and Skald share the rage power pool" },
		new string[3] { "BarbarianLegacy", "BloodragerChimeraLegacy", "Barbarian and Bloodrager share the rage power pool" },
		new string[3] { "ShifterLegacy", "DruidBaseLegacy", "Druid and Shifter share wild shape" },
		new string[3] { "ShifterLegacy", "ShifterEvilLegacy", "only one Shifter legacy" },
		new string[3] { "ShifterLegacy", "ShifterDragonLegacy", "only one Shifter legacy" },
		new string[3] { "ShifterLegacy", "ShifterGriffonLegacy", "only one Shifter legacy" },
		new string[3] { "ShifterLegacy", "ShifterHolyLegacy", "only one Shifter legacy" },
		new string[3] { "ShifterLegacy", "ShifterStingerLegacy", "only one Shifter legacy" }
	};

	public static readonly IReadOnlyDictionary<string, DlcRequirement> DlcOnlyBlueprints = new Dictionary<string, DlcRequirement>(StringComparer.Ordinal);

	public static string SelectionCategory(string selectionName)
	{
		if (selectionName == null)
		{
			return null;
		}
		if (selectionName == "IsekaiPetSelection")
		{
			return "pet";
		}
		if (selectionName == "IsekaiHumanHeritageSelection")
		{
			return "heritage";
		}
		if (selectionName.IndexOf("Legacy", StringComparison.Ordinal) >= 0)
		{
			return "legacy";
		}
		return null;
	}

	public static string DlcSkipReason(string[] pair, Func<string, bool> created, Func<string, bool> dlcAvailable)
	{
		return DlcSkipReason(pair, created, dlcAvailable, DlcOnlyBlueprints);
	}

	public static string DlcSkipReason(string[] pair, Func<string, bool> created, Func<string, bool> dlcAvailable, IReadOnlyDictionary<string, DlcRequirement> dlcOnly)
	{
		if (pair == null || pair.Length < 2 || created == null || dlcAvailable == null || dlcOnly == null)
		{
			return null;
		}
		List<string> list = new string[2]
		{
			pair[0],
			pair[1]
		}.Where((string n) => !created(n)).ToList();
		if (list.Count == 0)
		{
			return null;
		}
		List<string> list2 = new List<string>();
		foreach (string item in list)
		{
			if (item == null || !dlcOnly.TryGetValue(item, out var value) || dlcAvailable(value.DlcGuid))
			{
				return null;
			}
			list2.Add(item + " is only created with " + value.Label + ", which is not available");
		}
		return string.Join("; ", list2);
	}

	public static List<string[]> OneSidedLocks(IEnumerable<string[]> edges, ICollection<string> scope)
	{
		HashSet<Tuple<string, string>> hashSet = EdgeSet(edges);
		List<string[]> list = new List<string[]>();
		foreach (Tuple<string, string> item in hashSet.OrderBy((Tuple<string, string> e) => e.Item1, StringComparer.Ordinal).ThenBy((Tuple<string, string> e) => e.Item2, StringComparer.Ordinal))
		{
			if ((scope == null || (scope.Contains(item.Item1) && scope.Contains(item.Item2))) && !hashSet.Contains(Tuple.Create(item.Item2, item.Item1)))
			{
				list.Add(new string[2] { item.Item1, item.Item2 });
			}
		}
		return list;
	}

	public static List<string[]> MissingRequiredLocks(IEnumerable<string[]> edges, IEnumerable<string[]> required)
	{
		HashSet<Tuple<string, string>> hashSet = EdgeSet(edges);
		List<string[]> list = new List<string[]>();
		foreach (string[] item in required ?? Enumerable.Empty<string[]>())
		{
			if (item != null && item.Length >= 2)
			{
				if (!hashSet.Contains(Tuple.Create(item[0], item[1])))
				{
					list.Add(new string[2]
					{
						item[0],
						item[1]
					});
				}
				if (!hashSet.Contains(Tuple.Create(item[1], item[0])))
				{
					list.Add(new string[2]
					{
						item[1],
						item[0]
					});
				}
			}
		}
		return list;
	}

	private static HashSet<Tuple<string, string>> EdgeSet(IEnumerable<string[]> edges)
	{
		HashSet<Tuple<string, string>> hashSet = new HashSet<Tuple<string, string>>();
		foreach (string[] item in edges ?? Enumerable.Empty<string[]>())
		{
			if (item != null && item.Length >= 2 && item[0] != null && item[1] != null && !string.Equals(item[0], item[1], StringComparison.Ordinal))
			{
				hashSet.Add(Tuple.Create(item[0], item[1]));
			}
		}
		return hashSet;
	}
}
