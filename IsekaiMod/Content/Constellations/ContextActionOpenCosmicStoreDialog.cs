using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

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
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#F5C542><b>[Cosmic Sponsorship]</b></color> The Cosmic Constellation Exchange can be accessed through the Astral Coin Envoy or Herald at the Planar Coliseum.");
			});
		}
	}
}
