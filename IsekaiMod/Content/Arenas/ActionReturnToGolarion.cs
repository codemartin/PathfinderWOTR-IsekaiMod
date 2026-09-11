using Kingmaker;
using Kingmaker.Blueprints.Area;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Persistence;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas
{
	public class ActionReturnToGolarion : GameAction
	{
		public override string GetCaption()
		{
			return "Return to Golarion";
		}

		public override void RunAction()
		{
			BlueprintAreaEnterPoint blueprintAreaEnterPoint = BlueprintTools.GetBlueprint<BlueprintAreaEnterPoint>(PlanarColiseumHubManager.ColiseumReturnEnterPointGuid) ?? BlueprintTools.GetBlueprint<BlueprintAreaEnterPoint>("cdaba43118a82ff429b803ae846b35b0");
			if (blueprintAreaEnterPoint != null)
			{
				Game.Instance.LoadArea(blueprintAreaEnterPoint, AutoSaveMode.None, delegate
				{
				});
			}
		}
	}
}
