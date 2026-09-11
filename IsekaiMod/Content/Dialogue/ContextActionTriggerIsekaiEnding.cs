using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Quests;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionTriggerIsekaiEnding : ContextAction
	{
		public string EndingId;

		public string EndingTitle;

		public bool CheckLoopBreakerKeys;

		public bool ForceLoopShatter;

		public override string GetCaption()
		{
			return "Triggers Isekai Ending: " + EndingTitle;
		}

		public override void RunAction()
		{
			bool shatterLoop = ForceLoopShatter;
			if (CheckLoopBreakerKeys)
			{
				shatterLoop = TimelineManager.Data.LoopShattered || IsekaiTransmigrationQuest.AreAllSideQuestsCompleted(out var _);
			}
			IsekaiTransmigrationQuest.TriggerEnding(EndingId, EndingTitle, shatterLoop);
		}
	}
}
