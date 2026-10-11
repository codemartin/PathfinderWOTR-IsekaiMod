using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Dialogue;

[TypeId("22f035bfa8aa49c6818230a12039df9f")]
public class ContextActionTriggerIsekaiEnding : IsekaiContextAction
{
	public string EndingId;

	public string EndingTitle;

	public override string GetCaption()
	{
		return "Triggers Isekai Ending: " + EndingTitle;
	}

	protected override void RunSafely()
	{
		IsekaiTransmigrationQuest.TriggerEnding(EndingId, EndingTitle);
	}
}
