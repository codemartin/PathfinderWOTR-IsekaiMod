using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Quests;

[TypeId("9b10d609e24d453eb2b20504cb685626")]
public class ConditionDevourerDivergenceOpen : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return PersonalQuestDirector.IsDivergenceOfferedNow();
	}

	public override string GetConditionCaption()
	{
		return "The Sovereign Divergence is still open";
	}
}
