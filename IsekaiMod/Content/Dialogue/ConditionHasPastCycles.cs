using IsekaiMod.Content.Constellations;
using Kingmaker.ElementsSystem;

namespace IsekaiMod.Content.Dialogue
{
	public class ConditionHasPastCycles : Condition
	{
		public override bool CheckCondition()
		{
			if (TimelineManager.TotalRuns <= 1)
			{
				if (TimelineManager.PastCycles != null)
				{
					return TimelineManager.PastCycles.Count > 0;
				}
				return false;
			}
			return true;
		}

		public override string GetConditionCaption()
		{
			return "Checks if player has past cycles recorded in TimelineManager";
		}
	}
}
