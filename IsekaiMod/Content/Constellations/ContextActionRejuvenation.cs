using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Constellations;

[TypeId("b30aa28bdaf542e78577a51653dd2952")]
public class ContextActionRejuvenation : IsekaiContextAction
{
	public int Cost = 400;

	internal const string GachaResourceName = "MerchantsGambleResource";

	internal static List<BlueprintScriptableObject> RejuvenationExcludedResources()
	{
		List<BlueprintScriptableObject> list = new List<BlueprintScriptableObject>();
		BlueprintAbilityResource modBlueprint = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.IsekaiContext, "MerchantsGambleResource");
		if (modBlueprint != null)
		{
			list.Add(modBlueprint);
		}
		return list;
	}

	public override string GetCaption()
	{
		return "Restores hit points, spell slots and resources";
	}

	protected override void RunSafely()
	{
		UnitEntityData unitEntityData = DivineTokens.ResolveDialogTarget(base.Target, DivineTokens.GetPlayer);
		if (unitEntityData == null || !DivineTokens.SpendCoins(Cost))
		{
			return;
		}
		foreach (Spellbook spellbook in unitEntityData.Descriptor.Spellbooks)
		{
			spellbook.Rest();
		}
		unitEntityData.Descriptor.Resources.FullRestoreAll(RejuvenationExcludedResources());
		unitEntityData.Descriptor.Damage = 0;
		DivineTokens.Log(CosmicStoreText.Rejuvenated);
	}
}
