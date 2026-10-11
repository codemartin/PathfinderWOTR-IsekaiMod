using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas;

[TypeId("613c68dd568941f7ba4b0731c8372737")]
public class ActionOpenCosmicStoreFromColiseum : IsekaiGameAction
{
	public override string GetCaption()
	{
		return "Open the Cosmic Constellation Exchange";
	}

	internal static bool CanOpenStore(bool inCombat, bool trialActive)
	{
		if (!inCombat)
		{
			return !trialActive;
		}
		return false;
	}

	internal static bool StoreAvailable(bool constellationsEnabled)
	{
		return constellationsEnabled;
	}

	protected override void RunSafely()
	{
		if (!StoreAvailable(ConstellationSettings.Enabled))
		{
			PlanarColiseumHubManager.PostLog("<color=#3F3F3F><b>[Planar Coliseum]</b></color> The Cosmic Constellation Exchange is closed: the Constellations and Cosmic Sponsorships are switched off in the mod settings.");
			return;
		}
		Game instance = Game.Instance;
		if (!CanOpenStore(instance != null && instance.Player?.IsInCombat == true, DimensionalArenaManager.Instance.IsActive))
		{
			PlanarColiseumHubManager.PostLog("<color=#7A1010><b>[Planar Coliseum]</b></color> The Cosmic Constellation Exchange does not open while blood is being spilled on the sands.");
			return;
		}
		BlueprintDialog modBlueprint = BlueprintTools.GetModBlueprint<BlueprintDialog>(Main.IsekaiContext, "CosmicStoreDialog");
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (modBlueprint != null && unitEntityData != null)
		{
			Game.Instance.DialogController.StartDialogWithoutTarget(modBlueprint, null, unitEntityData);
		}
	}
}
