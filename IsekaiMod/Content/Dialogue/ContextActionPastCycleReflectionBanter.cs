using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionPastCycleReflectionBanter : ContextAction
	{
		public override string GetCaption()
		{
			return "Triggers Constellation Banter reflecting upon past cycles";
		}

		public override void RunAction()
		{
			DivineTokens.AddCoins(250, "The Key and the Gate");
			List<string> pastCycleRecognitionBanter = ConstellationDialogueBanter.GetPastCycleRecognitionBanter();
			if (pastCycleRecognitionBanter == null)
			{
				return;
			}
			foreach (string item in pastCycleRecognitionBanter)
			{
				ConstellationChatManager.PostLog(item, "The Key and the Gate", ConstellationCategory.MetaLoop, 0, "Past Cycle Reflection");
			}
		}
	}
}
