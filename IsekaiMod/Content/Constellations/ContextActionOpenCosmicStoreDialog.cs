using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionOpenCosmicStoreDialog : ContextAction
	{
		public BlueprintDialogReference m_Dialog;

		public override string GetCaption()
		{
			return "Opens Cosmic Store Dialog";
		}

		public override void RunAction()
		{
			BlueprintDialog blueprintDialog = m_Dialog?.Get() ?? BlueprintTools.GetModBlueprint<BlueprintDialog>(Main.IsekaiContext, "CosmicStoreDialog");
			UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
			if (blueprintDialog != null && unitEntityData != null && Game.Instance?.DialogController != null)
			{
				Game.Instance.DialogController.StartDialogWithoutTarget(blueprintDialog, null, unitEntityData);
				return;
			}
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#F5C542><b>[Cosmic Sponsorship]</b></color> The Cosmic Constellation Exchange can be accessed through the Astral Coin Envoy or Herald at the Planar Coliseum.");
			});
		}
	}
}
