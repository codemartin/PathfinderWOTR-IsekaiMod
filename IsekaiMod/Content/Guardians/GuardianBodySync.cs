using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic.FactLogic;

namespace IsekaiMod.Content.Guardians;

internal static class GuardianBodySync
{
	internal const string GhostCorruptingTouchGuid = "61c6ea1402f174b4fa599691bcec4382";

	internal static bool NeedsHandsEnabled(bool blueprintDisablesHands, bool handsEnabled, bool polymorphed)
	{
		if (!blueprintDisablesHands && !handsEnabled)
		{
			return !polymorphed;
		}
		return false;
	}

	internal static bool NeedsEmptyHand(bool handsEnabled, bool hasEmptyHand, bool blueprintHasEmptyHand)
	{
		return (handsEnabled && !hasEmptyHand) & blueprintHasEmptyHand;
	}

	internal static void Apply(UnitEntityData unit)
	{
		if (unit?.Body == null || unit.OriginalBlueprint?.Body == null)
		{
			return;
		}
		try
		{
			BlueprintUnit.UnitBody body = unit.OriginalBlueprint.Body;
			if (NeedsHandsEnabled(body.DisableHands, unit.Body.HandsAreEnabled, unit.Body.IsPolymorphed))
			{
				EnableHands(unit);
			}
			BlueprintItemWeapon emptyHandWeapon = body.EmptyHandWeapon;
			if (NeedsEmptyHand(unit.Body.HandsAreEnabled, unit.Body.EmptyHandWeapon != null, emptyHandWeapon != null) && !unit.Body.IsPolymorphed)
			{
				unit.Body.SetEmptyHandWeapon(emptyHandWeapon, isMonkUnarmedStrike: false, isPermanent: true);
			}
			RestoreNaturalHandWeapons(unit);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[Guardians] Could not bring " + unit.Blueprint?.name + "'s body in line with its blueprint: " + ex);
		}
	}

	private static void EnableHands(UnitEntityData unit)
	{
		using (ContextData<ItemSlot.IgnoreLock>.Request())
		{
			foreach (HandsEquipmentSet handsEquipmentSet in unit.Body.HandsEquipmentSets)
			{
				ItemSlot[] array = new ItemSlot[2] { handsEquipmentSet.PrimaryHand, handsEquipmentSet.SecondaryHand };
				foreach (ItemSlot itemSlot in array)
				{
					ItemEntity itemEntity = itemSlot?.MaybeItem;
					if (itemEntity?.Blueprint != null && string.Equals(itemEntity.Blueprint.AssetGuid.ToString(), "61c6ea1402f174b4fa599691bcec4382", StringComparison.OrdinalIgnoreCase))
					{
						itemSlot.RemoveItem();
						itemEntity.Collection?.Remove(itemEntity);
					}
				}
			}
		}
		unit.Body.HandsAreEnabled = true;
		unit.Body.EnsureActiveHands();
	}

	private static void RestoreNaturalHandWeapons(UnitEntityData unit)
	{
		if (unit.Body.IsPolymorphed)
		{
			return;
		}
		BlueprintUnit.UnitBody body = unit.OriginalBlueprint.Body;
		if (!unit.Body.HandsAreEnabled)
		{
			BlueprintItemWeapon blueprintItemWeapon = body.m_AdditionalLimbs?.FirstOrDefault()?.Get();
			HandSlot handSlot = ((unit.Body.HandsEquipmentSets.Count > 0) ? unit.Body.HandsEquipmentSets[0].PrimaryHand : null);
			if (blueprintItemWeapon == null || handSlot == null || handSlot.HasItem)
			{
				return;
			}
			ItemEntity item = blueprintItemWeapon.CreateEntity();
			if (!handSlot.CanInsertItem(item))
			{
				return;
			}
			using (ContextData<ItemSlot.IgnoreLock>.Request())
			{
				handSlot.InsertItem(item);
				return;
			}
		}
		for (int i = 0; i < unit.Body.HandsEquipmentSets.Count && i < 4; i++)
		{
			if (!(body.GetHandEqupment(i, main: true) is BlueprintItemWeapon { IsNatural: not false } blueprintItemWeapon2))
			{
				continue;
			}
			HandSlot primaryHand = unit.Body.HandsEquipmentSets[i].PrimaryHand;
			if (primaryHand == null || primaryHand.HasItem)
			{
				continue;
			}
			ItemEntity item2 = blueprintItemWeapon2.CreateEntity();
			if (primaryHand.CanInsertItem(item2))
			{
				using (ContextData<ItemSlot.IgnoreLock>.Request())
				{
					primaryHand.InsertItem(item2);
				}
			}
		}
	}

	internal static void ClearSlotsBeforeLock(UnitEntityData unit, BlueprintUnitFact lockingFact)
	{
		if (unit?.Body == null || lockingFact == null)
		{
			return;
		}
		try
		{
			List<LockEquipmentSlot> list = lockingFact.ComponentsArray?.OfType<LockEquipmentSlot>().ToList() ?? new List<LockEquipmentSlot>();
			if (list.Count == 0)
			{
				return;
			}
			BlueprintUnit.UnitBody unitBody = unit.OriginalBlueprint?.Body;
			foreach (LockEquipmentSlot item2 in list)
			{
				ItemSlot itemSlot = SlotFor(unit, item2.m_SlotType, out var handSet, out var primary);
				ItemEntity item = itemSlot?.MaybeItem;
				if (item == null || (handSet >= 0 && unitBody != null && item.Blueprint == unitBody.GetHandEqupment(handSet, primary)))
				{
					continue;
				}
				if (handSet >= 0 && unitBody != null && unitBody.DisableHands)
				{
					BlueprintItemWeaponReference[] additionalLimbs = unitBody.m_AdditionalLimbs;
					if (additionalLimbs != null && additionalLimbs.Any((BlueprintItemWeaponReference l) => l?.Get() == item.Blueprint))
					{
						continue;
					}
				}
				if (!item.IsNonRemovable)
				{
					itemSlot.RemoveItem();
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[Guardians] Could not empty " + unit.Blueprint?.name + "'s slots before locking them: " + ex);
		}
	}

	private static ItemSlot SlotFor(UnitEntityData unit, LockEquipmentSlot.SlotType type, out int handSet, out bool primary)
	{
		UnitBody body = unit.Body;
		handSet = -1;
		primary = true;
		switch (type)
		{
		case LockEquipmentSlot.SlotType.Armor:
			return body.Armor;
		case LockEquipmentSlot.SlotType.Shirt:
			return body.Shirt;
		case LockEquipmentSlot.SlotType.MainHand:
			handSet = body.CurrentHandEquipmentSetIndex;
			primary = true;
			return body.PrimaryHand;
		case LockEquipmentSlot.SlotType.OffHand:
			handSet = body.CurrentHandEquipmentSetIndex;
			primary = false;
			return body.SecondaryHand;
		case LockEquipmentSlot.SlotType.Cloak:
			return body.Shoulders;
		case LockEquipmentSlot.SlotType.Bracers:
			return body.Wrist;
		case LockEquipmentSlot.SlotType.Boots:
			return body.Feet;
		case LockEquipmentSlot.SlotType.Gloves:
			return body.Gloves;
		case LockEquipmentSlot.SlotType.Ring1:
			return body.Ring1;
		case LockEquipmentSlot.SlotType.Ring2:
			return body.Ring2;
		case LockEquipmentSlot.SlotType.Necklace:
			return body.Neck;
		case LockEquipmentSlot.SlotType.Belt:
			return body.Belt;
		case LockEquipmentSlot.SlotType.Headgear:
			return body.Head;
		case LockEquipmentSlot.SlotType.Glasses:
			return body.Glasses;
		case LockEquipmentSlot.SlotType.Weapon1:
			handSet = 0;
			primary = true;
			return body.HandsEquipmentSets[0].PrimaryHand;
		case LockEquipmentSlot.SlotType.Weapon2:
			handSet = 0;
			primary = false;
			return body.HandsEquipmentSets[0].SecondaryHand;
		case LockEquipmentSlot.SlotType.Weapon3:
			handSet = 1;
			primary = true;
			return body.HandsEquipmentSets[1].PrimaryHand;
		case LockEquipmentSlot.SlotType.Weapon4:
			handSet = 1;
			primary = false;
			return body.HandsEquipmentSets[1].SecondaryHand;
		case LockEquipmentSlot.SlotType.Weapon5:
			handSet = 2;
			primary = true;
			return body.HandsEquipmentSets[2].PrimaryHand;
		case LockEquipmentSlot.SlotType.Weapon6:
			handSet = 2;
			primary = false;
			return body.HandsEquipmentSets[2].SecondaryHand;
		case LockEquipmentSlot.SlotType.Weapon7:
			handSet = 3;
			primary = true;
			return body.HandsEquipmentSets[3].PrimaryHand;
		case LockEquipmentSlot.SlotType.Weapon8:
			handSet = 3;
			primary = false;
			return body.HandsEquipmentSets[3].SecondaryHand;
		default:
			return null;
		}
	}
}
