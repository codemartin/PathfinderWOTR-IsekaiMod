using Kingmaker.ElementsSystem;

namespace IsekaiMod.Content.Quests
{
	public class ActionTriggerSylvanWildHunt : GameAction
	{
		public override string GetCaption()
		{
			return "Trigger Sylvan Wild Hunt Encounter";
		}

		public override void RunAction()
		{
			SylvanFeyAllianceQuest.TriggerWildHuntEncounter();
		}
	}
}
