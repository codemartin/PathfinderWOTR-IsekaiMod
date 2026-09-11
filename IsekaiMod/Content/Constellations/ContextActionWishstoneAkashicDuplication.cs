using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionWishstoneAkashicDuplication : ContextAction
	{
		public override string GetCaption()
		{
			return "Miracle of Akashic Duplication: Manifests high-tier 9th-level scrolls";
		}

		public override void RunAction()
		{
			Player player = Game.Instance?.Player;
			if (player == null)
			{
				return;
			}
			string[] array = new string[6] { "e19e85bfe282a1145a6075567a71970f", "5954d6c5bc2640d458423ff2ef8612f4", "a1bc9bf104a3c614391eccdcdf37d4ad", "449bf6d894ed8de4bbf148023cdf36f5", "33ac9588ff822a64c92167bdcec7df65", "28968352ed826da4e9c2af856aad7096" };
			for (int i = 0; i < array.Length; i++)
			{
				BlueprintItem blueprintItem = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintItem>(array[i]);
				if (blueprintItem != null)
				{
					player.Inventory.Add(blueprintItem, 1);
				}
			}
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#00FFFF><b>[Cosmic Wishstone] Miracle of Akashic Duplication!</b></color> Reality bends around your hands, crystallizing scrolls of Mass Heal, Heroic Invocation, Overwhelming Presence, Tsunami, Weird, and Foresight into your pack!");
			});
		}
	}
}
