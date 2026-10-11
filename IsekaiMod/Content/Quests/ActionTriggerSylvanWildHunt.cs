using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Quests;

[TypeId("7bb4af73c22f4d6eb40065388c005506")]
public class ActionTriggerSylvanWildHunt : IsekaiGameAction
{
	public override string GetCaption()
	{
		return "Trigger Sylvan Wild Hunt Encounter";
	}

	protected override void RunSafely()
	{
		SylvanFeyAllianceQuest.TriggerWildHuntEncounter();
	}
}
