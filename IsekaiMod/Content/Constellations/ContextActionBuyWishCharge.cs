using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Constellations;

[TypeId("4c52d6d390d04b8aa219e2a3b4236f96")]
public class ContextActionBuyWishCharge : IsekaiContextAction
{
	public int Cost = 1000;

	public BlueprintAbilityResourceReference m_WishResource;

	public override string GetCaption()
	{
		return $"Spends {Cost} Cosmic Coins and adds 1 Wish charge";
	}

	protected override void RunSafely()
	{
		UnitEntityData unitEntityData = DivineTokens.ResolveDialogTarget(base.Target, DivineTokens.GetPlayer);
		BlueprintAbilityResource blueprintAbilityResource = m_WishResource?.Get();
		if (!(unitEntityData == null) && blueprintAbilityResource != null)
		{
			UnitAbilityResourceCollection resources = unitEntityData.Descriptor.Resources;
			if (!resources.ContainsResource(blueprintAbilityResource))
			{
				DivineTokens.Log(CosmicStoreText.WishNoPool);
			}
			else if (resources.HasMaxAmount(blueprintAbilityResource))
			{
				DivineTokens.Log(CosmicStoreText.WishFull, 10);
			}
			else if (DivineTokens.SpendCoins(Cost))
			{
				unitEntityData.Descriptor.Resources.Restore(blueprintAbilityResource, 1);
				DivineTokens.Log(CosmicStoreText.WishBought);
			}
		}
	}
}
