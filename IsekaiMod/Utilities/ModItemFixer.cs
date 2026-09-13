using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using TabletopTweaks.Core.Config;

namespace IsekaiMod.Utilities
{
	// Items created with Helpers.CreateBlueprint start with a null m_Enchantments array. The game's
	// BlueprintItem.Enchantments (used by ItemEntity creation, IdentifyDC, tooltips) calls
	// m_Enchantments.Dereference(), which throws ArgumentNullException("source") on null. That made
	// every mod item reward (personal quest heirlooms, store items, consumables) impossible to add to
	// the inventory. Give every mod item an empty array instead of patching each creation site.
	internal static class ModItemFixer
	{
		public static void FixEnchantmentArrays()
		{
			Blueprints blueprints = Main.IsekaiContext.Blueprints;
			if (blueprints == null)
			{
				return;
			}
			int fixedCount = 0;
			foreach (string field in new string[2] { "NewBlueprints", "UsedGUIDs" })
			{
				SortedDictionary<string, Guid> guids = Traverse.Create(blueprints).Field(field).GetValue<SortedDictionary<string, Guid>>();
				if (guids == null)
				{
					continue;
				}
				foreach (KeyValuePair<string, Guid> entry in guids)
				{
					SimpleBlueprint blueprint = ResourcesLibrary.TryGetBlueprint(new BlueprintGuid(entry.Value));
					if (blueprint != null && Fix(blueprint))
					{
						fixedCount++;
					}
				}
			}
			Main.IsekaiContext.Logger.Log("ModItemFixer: initialised enchantment arrays on " + fixedCount + " item blueprint(s)");
		}

		private static bool Fix(SimpleBlueprint blueprint)
		{
			switch (blueprint)
			{
			case BlueprintItemWeapon weapon:
				if (weapon.m_Enchantments == null)
				{
					weapon.m_Enchantments = new BlueprintWeaponEnchantmentReference[0];
					return true;
				}
				return false;
			case BlueprintItemArmor armor:
				if (armor.m_Enchantments == null)
				{
					armor.m_Enchantments = new BlueprintEquipmentEnchantmentReference[0];
					return true;
				}
				return false;
			case BlueprintItemEquipmentUsable usable:
				if (usable.m_Enchantments == null)
				{
					usable.m_Enchantments = new BlueprintEquipmentEnchantmentReference[0];
					return true;
				}
				return false;
			case BlueprintItemEquipmentSimple simple:
				if (simple.m_Enchantments == null)
				{
					simple.m_Enchantments = new BlueprintEquipmentEnchantmentReference[0];
					return true;
				}
				return false;
			default:
				return false;
			}
		}
	}
}
