using System;
using IsekaiMod.Content.Constellations;
using Kingmaker.ElementsSystem;

namespace IsekaiMod.Content.Dialogue
{
	public class ConditionCheckIsekaiEnding : Condition
	{
		public string EndingId;

		public override bool CheckCondition()
		{
			PastCycleRecord lastCycle = TimelineManager.GetLastCycle();
			if (lastCycle == null || string.IsNullOrEmpty(lastCycle.EndingId) || string.IsNullOrEmpty(EndingId))
			{
				return false;
			}
			if (!string.Equals(lastCycle.EndingId, EndingId, StringComparison.OrdinalIgnoreCase) && !string.Equals("IsekaiEnding" + lastCycle.EndingId, EndingId, StringComparison.OrdinalIgnoreCase) && !string.Equals(lastCycle.EndingId, "IsekaiEnding" + EndingId, StringComparison.OrdinalIgnoreCase))
			{
				if (EndingId.StartsWith("IsekaiEnding", StringComparison.OrdinalIgnoreCase))
				{
					return string.Equals(lastCycle.EndingId, EndingId.Substring("IsekaiEnding".Length), StringComparison.OrdinalIgnoreCase);
				}
				return false;
			}
			return true;
		}

		public override string GetConditionCaption()
		{
			return "Checks if player triggered Isekai ending: " + EndingId;
		}
	}
}
