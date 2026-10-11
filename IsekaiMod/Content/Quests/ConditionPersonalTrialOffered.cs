using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Quests;

[TypeId("439a6b8435f04cf08c1f741258818d0b")]
public class ConditionPersonalTrialOffered : IsekaiCondition
{
	public string TrialId;

	protected override bool CheckSafely()
	{
		return PersonalQuestDirector.IsTrialOfferedNow(TrialId);
	}

	public override string GetConditionCaption()
	{
		return "The personal-quest trial " + TrialId + " is offered";
	}
}
