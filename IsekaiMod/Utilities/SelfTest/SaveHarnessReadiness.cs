using System;
using System.Collections.Generic;

namespace IsekaiMod.Utilities.SelfTest;

public static class SaveHarnessReadiness
{
	public sealed class Sample
	{
		public bool HasLoadedArea;

		public bool LoadingInProcess;

		public bool HasMainCharacter;

		public int PartyCount;

		public string ModeName;
	}

	public sealed class StabilityTracker
	{
		private readonly int required;

		private int streak;

		public int Streak => streak;

		public StabilityTracker(int requiredSamples = 30)
		{
			required = Math.Max(1, requiredSamples);
		}

		public bool Observe(Sample sample)
		{
			if (IsReady(sample))
			{
				if (streak < int.MaxValue)
				{
					streak++;
				}
			}
			else
			{
				streak = 0;
			}
			return streak >= required;
		}

		public void Reset()
		{
			streak = 0;
		}
	}

	public const int RequiredStableSamples = 30;

	public static readonly string[] SettledModes = new string[3] { "Default", "GlobalMap", "Pause" };

	public static bool IsReady(Sample sample)
	{
		if (sample != null && sample.HasLoadedArea && !sample.LoadingInProcess && sample.HasMainCharacter && sample.PartyCount > 0)
		{
			return IsSettledMode(sample.ModeName);
		}
		return false;
	}

	public static bool IsSettledMode(string modeName)
	{
		if (string.IsNullOrEmpty(modeName))
		{
			return false;
		}
		string[] settledModes = SettledModes;
		foreach (string b in settledModes)
		{
			if (string.Equals(modeName, b, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	public static List<string> MissingSigns(Sample sample)
	{
		List<string> list = new List<string>();
		if (sample == null)
		{
			list.Add("nothing could be read from the game");
			return list;
		}
		if (!sample.HasLoadedArea)
		{
			list.Add("no area is loaded");
		}
		if (sample.LoadingInProcess)
		{
			list.Add("a loading process is still running");
		}
		if (!sample.HasMainCharacter)
		{
			list.Add("there is no main character");
		}
		if (sample.PartyCount <= 0)
		{
			list.Add("the party is empty");
		}
		if (!IsSettledMode(sample.ModeName))
		{
			list.Add("the game mode is '" + (string.IsNullOrEmpty(sample.ModeName) ? "none" : sample.ModeName) + "' rather than " + string.Join(" or ", SettledModes));
		}
		return list;
	}
}
