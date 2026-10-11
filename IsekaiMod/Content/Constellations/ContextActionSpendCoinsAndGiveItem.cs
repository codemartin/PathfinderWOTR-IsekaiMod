using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;

namespace IsekaiMod.Content.Constellations;

[TypeId("78cce67a040040c2b47a4c7bc83fbcea")]
public class ContextActionSpendCoinsAndGiveItem : IsekaiContextAction
{
	public int Cost;

	public BlueprintItemReference m_Item;

	public override string GetCaption()
	{
		return $"Spends {Cost} Cosmic Coins and gives an item";
	}

	protected override void RunSafely()
	{
		BlueprintItem blueprintItem = m_Item?.Get();
		ItemsCollection itemsCollection = Game.Instance?.Player?.Inventory;
		if (blueprintItem != null && itemsCollection != null && DivineTokens.SpendCoins(Cost))
		{
			itemsCollection.Add(blueprintItem, 1);
			DivineTokens.Log(CosmicStoreText.ItemBought, blueprintItem.Name);
		}
	}
}
