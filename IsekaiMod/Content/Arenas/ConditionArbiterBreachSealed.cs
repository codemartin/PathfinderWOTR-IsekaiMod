using System.Collections.Generic;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("a265448fa69443df84c3b33999de4402")]
public class ConditionArbiterBreachSealed : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		List<string> missingQuests;
		if (BossSites.CurrentAccessibleRiftAct() == 6)
		{
			return !IsekaiTransmigrationQuest.AreAllSideQuestsCompleted(out missingQuests);
		}
		return false;
	}

	public override string GetConditionCaption()
	{
		return "The Grand Arbiter's breach is still sealed";
	}
}
