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
			if (lastCycle != null)
			{
				return string.Equals(lastCycle.EndingId, EndingId, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		public override string GetConditionCaption()
		{
			return "Checks if player triggered Isekai ending: " + EndingId;
		}
	}
}
