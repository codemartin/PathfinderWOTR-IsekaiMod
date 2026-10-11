using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Utilities.SelfTest;

public static class SaveHarnessSelection
{
	public sealed class Candidate
	{
		public string Name { get; }

		public string FileName { get; }

		public string FullPath { get; }

		public string AreaName { get; }

		public Candidate(string name, string fileName, string fullPath, string areaName)
		{
			Name = name ?? "";
			FileName = fileName ?? "";
			FullPath = fullPath ?? "";
			AreaName = areaName ?? "";
		}

		public override string ToString()
		{
			if (AreaName.Length != 0)
			{
				return "'" + Name + "' (" + AreaName + ")";
			}
			return "'" + Name + "'";
		}
	}

	public enum Outcome
	{
		NotRequested,
		NoSaves,
		Matched,
		NotFound,
		Ambiguous
	}

	public sealed class Choice
	{
		public Outcome Result { get; }

		public int Index { get; }

		public string Reason { get; }

		public bool Found => Result == Outcome.Matched;

		public Choice(Outcome outcome, int index, string reason)
		{
			Result = outcome;
			Index = index;
			Reason = reason ?? "";
		}
	}

	public static Choice Select(string request, IReadOnlyList<Candidate> candidates)
	{
		string wanted = SaveHarnessSwitch.Unquote(request);
		if (string.IsNullOrEmpty(wanted))
		{
			return new Choice(Outcome.NotRequested, -1, "No save was named, so the live-world checks did not run.");
		}
		IReadOnlyList<Candidate> readOnlyList = candidates ?? new Candidate[0];
		if (readOnlyList.Count == 0)
		{
			return new Choice(Outcome.NoSaves, -1, "'" + wanted + "' was asked for, but this machine has no saves at all, so the live-world checks did not run.");
		}
		string leaf = LeafOf(wanted);
		Func<Candidate, bool>[] array = new Func<Candidate, bool>[6]
		{
			(Candidate c) => SamePath(c.FullPath, wanted),
			(Candidate c) => Same(c.FileName, wanted) || Same(StripExtension(c.FileName), StripExtension(wanted)),
			(Candidate c) => Same(c.Name, wanted),
			(Candidate c) => Same(c.FileName, leaf) || Same(StripExtension(c.FileName), StripExtension(leaf)),
			(Candidate c) => Same(c.Name, leaf),
			(Candidate c) => Contains(c.Name, wanted)
		};
		foreach (Func<Candidate, bool> rule in array)
		{
			List<int> list = Matches(readOnlyList, rule);
			if (list.Count == 1)
			{
				return new Choice(Outcome.Matched, list[0], "Loading the save " + readOnlyList[list[0]]?.ToString() + ".");
			}
			if (list.Count > 1)
			{
				return new Choice(Outcome.Ambiguous, -1, "'" + wanted + "' names " + list.Count + " saves (" + Listed(readOnlyList, list) + "), so the live-world checks did not run. Give the exact save name or its folder name.");
			}
		}
		return new Choice(Outcome.NotFound, -1, "No save called '" + wanted + "' was found among the " + readOnlyList.Count + " on this machine (" + Listed(readOnlyList, AllIndexes(readOnlyList)) + "), so the live-world checks did not run.");
	}

	private static List<int> Matches(IReadOnlyList<Candidate> saves, Func<Candidate, bool> rule)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < saves.Count; i++)
		{
			if (saves[i] != null && rule(saves[i]))
			{
				list.Add(i);
			}
		}
		return list;
	}

	private static List<int> AllIndexes(IReadOnlyList<Candidate> saves)
	{
		return Enumerable.Range(0, saves.Count).ToList();
	}

	private static string Listed(IReadOnlyList<Candidate> saves, IReadOnlyList<int> indexes)
	{
		List<string> list = (from i in indexes.Take(8)
			select "'" + saves[i]?.Name + "'").ToList();
		if (indexes.Count > list.Count)
		{
			list.Add("and " + (indexes.Count - list.Count) + " more");
		}
		return string.Join(", ", list);
	}

	private static bool SamePath(string a, string b)
	{
		return Same(NormalizePath(a), NormalizePath(b));
	}

	public static string NormalizePath(string path)
	{
		return (path ?? "").Replace('/', '\\').TrimEnd('\\');
	}

	private static bool Same(string a, string b)
	{
		if (a != null && b != null && a.Length > 0)
		{
			return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static bool Contains(string haystack, string needle)
	{
		if (!string.IsNullOrEmpty(haystack) && !string.IsNullOrEmpty(needle))
		{
			return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
		}
		return false;
	}

	public static string LeafOf(string request)
	{
		string text = (request ?? "").TrimEnd('\\', '/');
		int num = text.LastIndexOfAny(new char[2] { '\\', '/' });
		if (num >= 0)
		{
			return text.Substring(num + 1);
		}
		return text;
	}

	public static string StripExtension(string name)
	{
		string text = name ?? "";
		if (!text.EndsWith(".zks", StringComparison.OrdinalIgnoreCase))
		{
			return text;
		}
		return text.Substring(0, text.Length - 4);
	}
}
