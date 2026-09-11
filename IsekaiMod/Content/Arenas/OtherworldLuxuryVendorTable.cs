using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas
{
	public static class OtherworldLuxuryVendorTable
	{
		public static BlueprintSharedVendorTable Table;

		public static void Add()
		{
			Table = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldLuxuryVendorTable", delegate(BlueprintSharedVendorTable bp)
			{
				bp.AutoIdentifyAllItems = true;
				bp.ComponentsArray = new BlueprintComponent[0];
			});
			AddPackByGuid(Table, "dc2f2613634c2f947b306fa3379232c2", 1);
			AddPackByGuid(Table, "88945c4d1bd44de59905ac20898d05d5", 1);
			AddPackByGuid(Table, "039fcbf4887047e8977102c23dd8b56b", 1);
			AddPackByGuid(Table, "fd8122d61cc34463a615878dd59aee30", 1);
			AddPackByGuid(Table, "5db66b14642642eaa1c623ed0daa5813", 1);
			AddPackByGuid(Table, "81d504243708f504dbfe3f8f72efdeda", 1);
			AddPackByGuid(Table, "a1c498e8e3e640deb5eb6d888e229565", 1);
			AddPackByGuid(Table, "372aae7b04ff4dd438ef3a8f881d5b17", 1);
			AddPackByGuid(Table, "75790eb681ee60344b1a2223e9c8c3a6", 1);
			AddPackByGuid(Table, "34fb9e0a9c674bd082d61e1c6404bdb9", 1);
			AddPackByGuid(Table, "0c97bace75036234f833dc03e5175d8d", 1);
			AddPackByGuid(Table, "92752bbbf04dfa1439af186f48aee0e9", 5000);
			AddPackByGuid(Table, "66e2ab66e3e0c514f977cd22b3d7a0cd", 200);
			AddPackByGuid(Table, "6169a9e10d32c524ea44edb47ebd93cf", 25);
			Main.IsekaiContext.Logger.Log("[OtherworldLuxuryVendorTable] Void Market Smuggler luxury table successfully assembled.");
		}

		private static void AddPackByGuid(BlueprintSharedVendorTable table, string itemGuid, int count)
		{
			if (table == null)
			{
				return;
			}
			BlueprintItem item = BlueprintTools.GetBlueprint<BlueprintItem>(itemGuid);
			if (item != null)
			{
				table.AddComponent(delegate(LootItemsPackFixed c)
				{
					c.m_Item = new LootItem
					{
						m_Type = LootItemType.Item,
						m_Item = item.ToReference<BlueprintItemReference>()
					};
					c.m_Count = count;
				});
			}
		}
	}
}
