using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("c7275917e2e243d3be4051988328c1c8")]
public class ConditionHasPastCycles : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		TimelineData data = TimelineManager.Data;
		return HasPastCycleOutsideGame(data?.ClosedCycles, data?.TotalRuns ?? 1, (data?.PastCycles?.Count).GetValueOrDefault(), TimelineManager.CurrentSaveId());
	}

	public override string GetConditionCaption()
	{
		return "Checks if another playthrough has closed a cycle of the loop";
	}

	internal static bool HasPastCycleOutsideGame(IList<string> closedCycles, int totalRuns, int pastCycleCount, string gameId)
	{
		if (closedCycles != null && closedCycles.Count > 0)
		{
			string value = gameId + "|";
			foreach (string closedCycle in closedCycles)
			{
				if (closedCycle != null && (string.IsNullOrEmpty(gameId) || !closedCycle.StartsWith(value, StringComparison.Ordinal)))
				{
					return true;
				}
			}
			return false;
		}
		if (totalRuns <= 1)
		{
			return pastCycleCount > 0;
		}
		return true;
	}
}
