using Kingmaker.ElementsSystem;

namespace IsekaiMod.Content.Arenas
{
	public class ActionTriggerColiseumHeraldDialogue : GameAction
	{
		public override string GetCaption()
		{
			return "Trigger Coliseum Herald Dialogue";
		}

		public override void RunAction()
		{
			new ActionEnterColiseum().RunAction();
		}
	}
}
