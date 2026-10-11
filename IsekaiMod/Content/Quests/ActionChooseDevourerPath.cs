using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Quests;

[TypeId("d7aaddbcb79f45b7b07b074857e516d8")]
public class ActionChooseDevourerPath : IsekaiGameAction
{
	public string Path;

	public override string GetCaption()
	{
		return "Choose the Devourer's path: " + Path;
	}

	protected override void RunSafely()
	{
		PersonalQuestDirector.OnDivergenceChosen(Path);
	}
}
