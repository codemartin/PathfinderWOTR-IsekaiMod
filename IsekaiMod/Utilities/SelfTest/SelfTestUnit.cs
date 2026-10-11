using System;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal sealed class SelfTestUnit : IDisposable
{
	internal UnitEntityData Unit { get; }

	private SelfTestUnit(UnitEntityData unit)
	{
		Unit = unit;
	}

	internal static SelfTestUnit CreateBlank(out string problem)
	{
		problem = null;
		try
		{
			BlueprintUnit blueprintUnit = BlueprintRoot.Instance?.DefaultPlayerCharacter;
			if (blueprintUnit == null || Game.Instance == null)
			{
				problem = "the game has no default player character to build a blank unit from";
				return null;
			}
			UnitEntityData unitEntityData;
			using (ContextData<UnitEntityData.ChargenUnit>.Request())
			{
				unitEntityData = Game.Instance.CreateUnitVacuum(blueprintUnit);
			}
			if (unitEntityData == null)
			{
				problem = "CreateUnitVacuum returned nothing";
				return null;
			}
			EnsureArmor(unitEntityData);
			return new SelfTestUnit(unitEntityData);
		}
		catch (Exception ex)
		{
			problem = "a blank unit could not be created (" + ex.GetType().Name + ": " + ex.Message + ")";
			return null;
		}
	}

	internal static void EnsureArmor(UnitEntityData unit)
	{
		try
		{
			ArmorSlot armorSlot = unit?.Body?.Armor;
			if (armorSlot != null && !armorSlot.HasItem)
			{
				BlueprintItemArmor blueprint = BlueprintTools.GetBlueprint<BlueprintItemArmor>("02e9f83be5d1c5d4e927b5c44ed34840");
				if (blueprint != null)
				{
					armorSlot.InsertItem(new ItemEntityArmor(blueprint));
				}
			}
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		try
		{
			Unit?.Dispose();
		}
		catch
		{
		}
	}
}
