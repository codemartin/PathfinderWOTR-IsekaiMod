using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas
{
	public class ActionOpenCosmicStoreFromColiseum : GameAction
	{
		public override string GetCaption()
		{
			return "Open Cosmic Store";
		}

		public override void RunAction()
		{
			BlueprintDialog modBlueprint = BlueprintTools.GetModBlueprint<BlueprintDialog>(Main.IsekaiContext, "CosmicStoreDialog");
			UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
			if (modBlueprint != null && unitEntityData != null && Game.Instance?.DialogController != null)
			{
				Game.Instance.DialogController.StartDialogWithoutTarget(modBlueprint, null, unitEntityData);
			}
		}
	}
}
