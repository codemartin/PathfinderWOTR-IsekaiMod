using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UI.GenericSlot;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class Excalibur
	{
		public static void Add()
		{
			LocalizedString localizedString = Helpers.CreateString(Main.IsekaiContext, "Excalibur.Description", "Your primary weapon gains the holy and radiant enchantments, dealing an additional 2d6 holy damage. Your melee attack reach is increased by 10 feet.");
			Sprite icon = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_EXCALIBUR.png");
			BlueprintWeaponEnchantment RadiantEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("5ac5c88157f7dde48a2a5b24caf40131");
			BlueprintWeaponEnchantment HolyEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140");
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("Excalibur", localizedString, icon, delegate(BlueprintBuff bp)
			{
				if (RadiantEnchantment != null)
				{
					bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
					{
						c.m_EnchantmentBlueprint = RadiantEnchantment.ToReference<BlueprintItemEnchantmentReference>();
						c.Slot = EquipSlotBase.SlotType.PrimaryHand;
					});
				}
				if (HolyEnchantment != null)
				{
					bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
					{
						c.m_EnchantmentBlueprint = HolyEnchantment.ToReference<BlueprintItemEnchantmentReference>();
						c.Slot = EquipSlotBase.SlotType.PrimaryHand;
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Reach;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 13;
			});
			SpecialPowerSelection.AddToAuthoritySelection(blueprintFeature);
		}
	}
}
