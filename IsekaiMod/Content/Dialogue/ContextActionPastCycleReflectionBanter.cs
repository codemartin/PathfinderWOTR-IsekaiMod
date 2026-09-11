using IsekaiMod.Content.Constellations;
using Kingmaker.PubSubSystem;
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
			foreach (string line in ConstellationDialogueBanter.GetPastCycleRecognitionBanter())
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(line);
				});
			}
		}
	}
}
