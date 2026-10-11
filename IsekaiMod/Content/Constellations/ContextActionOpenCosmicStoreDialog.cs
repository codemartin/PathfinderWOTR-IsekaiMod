using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;

namespace IsekaiMod.Content.Constellations;

[TypeId("507b97acfb5742e783566df2fbfe313e")]
public class ContextActionOpenCosmicStoreDialog : IsekaiContextAction
{
	public BlueprintDialogReference m_Dialog;

	public override string GetCaption()
	{
		return "Opens Cosmic Store Dialog";
	}

	protected override void RunSafely()
	{
		if (!ConstellationSettings.Enabled)
		{
			DivineTokens.Log(CosmicStoreText.ConstellationsDisabled);
			return;
		}
		BlueprintDialog blueprintDialog = m_Dialog?.Get();
		UnitEntityData player = DivineTokens.GetPlayer();
		if (blueprintDialog != null && player != null)
		{
			if (Game.Instance.Player.IsInCombat)
			{
				DivineTokens.Log(CosmicStoreText.InCombat);
			}
			else
			{
				Game.Instance.DialogController.StartDialogWithoutTarget(blueprintDialog, null, player);
			}
		}
	}
}
