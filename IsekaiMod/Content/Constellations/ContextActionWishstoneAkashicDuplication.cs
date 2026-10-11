using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Constellations;

[TypeId("6e8a0c2e4a6c4e8a0c2e4a6c8e0a2c4e")]
public class ContextActionWishstoneAkashicDuplication : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Miracle of Akashic Duplication: manifests six 9th-level scrolls";
	}

	protected override void RunSafely()
	{
		Player player = Game.Instance?.Player;
		if (player == null)
		{
			return;
		}
		string[] akashicScrolls = CosmicConsumableCatalog.AkashicScrolls;
		for (int i = 0; i < akashicScrolls.Length; i++)
		{
			BlueprintItem blueprint = BlueprintTools.GetBlueprint<BlueprintItem>(akashicScrolls[i]);
			if (blueprint != null)
			{
				player.Inventory.Add(blueprint, 1);
			}
		}
		EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
		{
			h.HandleLogMessage(ConstellationText.Localized("CosmicWishstone.Miracle", "<color=#133E71><b>[Cosmic Wishstone] Miracle of Akashic Duplication!</b></color> Reality bends around your hands, crystallizing scrolls of Mass Heal, Heroic Invocation, Overwhelming Presence, Tsunami, Weird, and Foresight into your pack!"));
		});
	}
}
