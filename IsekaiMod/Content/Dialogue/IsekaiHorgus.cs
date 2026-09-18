using IsekaiMod.Utilities;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiHorgus
	{
		public static void Add()
		{
			BlueprintAnswer blueprint = BlueprintTools.GetBlueprint<BlueprintAnswer>("3ab564082485b034a9d0a7b550e1a3e2");
			if (blueprint != null)
			{
				BlueprintAnswer blueprintAnswer = blueprint;
				if (blueprintAnswer.ShowConditions == null)
				{
					blueprintAnswer.ShowConditions = ActionFlow.EmptyCondition();
				}
			}
		}
	}
}
