using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Quests;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.View;
using Kingmaker.Visual.CharacterSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestCompanionChecks
{
	private static Dictionary<BlueprintUnit, string> PetUnits()
	{
		Dictionary<BlueprintUnit, string> dictionary = new Dictionary<BlueprintUnit, string>();
		foreach (SelfTestRecorder.CreatedBlueprint item in SelfTestRecorder.CreatedBlueprints())
		{
			if (!(item.Blueprint is BlueprintUnitFact blueprintUnitFact))
			{
				continue;
			}
			BlueprintComponent[] array = blueprintUnitFact.ComponentsArray ?? new BlueprintComponent[0];
			for (int i = 0; i < array.Length; i++)
			{
				BlueprintUnit blueprintUnit = (array[i] as AddPet)?.Pet;
				if (blueprintUnit != null)
				{
					dictionary[blueprintUnit] = item.Name;
				}
			}
		}
		return dictionary;
	}

	private static bool HasCharacterAvatar(BlueprintUnit unit)
	{
		UnitViewLink prefab = unit.Prefab;
		if (prefab != null && prefab.Exists())
		{
			try
			{
				UnitEntityView unitEntityView = prefab.Load();
				if (unitEntityView != null && unitEntityView.GetComponentInChildren<Character>(includeInactive: true) != null)
				{
					return true;
				}
			}
			catch
			{
			}
		}
		if (unit.CustomizationPreset == null)
		{
			return unit.Body?.Armor != null;
		}
		return true;
	}

	internal static SelfTestChecks.Outcome CompanionCharacterSheets()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<BlueprintUnit, string> item in PetUnits().OrderBy((KeyValuePair<BlueprintUnit, string> p) => p.Value, StringComparer.Ordinal))
		{
			BlueprintUnit key = item.Key;
			num++;
			List<string> list2 = new List<string>();
			BlueprintUnit.UnitBody body = key.Body;
			if (body == null)
			{
				list2.Add("it has no Body at all");
			}
			else
			{
				BlueprintItemEquipmentUsableReference[] quickSlots = body.m_QuickSlots;
				int num3 = ((quickSlots != null) ? quickSlots.Length : 0);
				if (num3 < 5)
				{
					list2.Add($"its Body has {num3} quick slots; InventoryDollVM indexes five and throws below that");
				}
				if (HasCharacterAvatar(key) && key.Race == null)
				{
					list2.Add("its view is a Character avatar and it has no race, so DollRoom reads Progression.Race.RaceId on null");
				}
				if (body.EmptyHandWeapon == null && body.PrimaryHand == null)
				{
					BlueprintItemWeaponReference[] additionalLimbs = body.m_AdditionalLimbs;
					if (additionalLimbs == null || !additionalLimbs.Any((BlueprintItemWeaponReference w) => w != null))
					{
						list2.Add("its Body has no empty-hand weapon, no primary hand and no limbs, so it has nothing to attack with");
					}
				}
			}
			if (key.PortraitSafe == null)
			{
				list2.Add("it has no portrait and no placeholder");
			}
			if (list2.Count > 0)
			{
				num2++;
				list.Add(key.name + " (summoned by " + item.Value + "): " + string.Join("; ", list2) + ".");
			}
		}
		return SelfTestChecks.Outcome.Of(num2 == 0, $"{num} summoned companions; {num2} whose character sheet or body the engine cannot handle.", list);
	}

	internal static SelfTestChecks.Outcome GrantedItemsRender()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		string[] grantedRelicGuids = SylvanFeyAllianceQuest.GrantedRelicGuids;
		foreach (string text in grantedRelicGuids)
		{
			num++;
			BlueprintItem blueprint = BlueprintTools.GetBlueprint<BlueprintItem>(text);
			string text2 = ((blueprint == null) ? "the blueprint does not resolve" : ItemRenderGuard.RenderProblem(blueprint));
			if (text2 == null && blueprint is BlueprintItemEquipment { IsNonRemovable: not false })
			{
				text2 = "IsNonRemovable";
			}
			if (text2 != null)
			{
				num2++;
				list.Add(text + ": " + text2);
			}
		}
		return SelfTestChecks.Outcome.Of(num2 == 0, $"{num} Stolen Lands grants checked; {num2} the inventory could not draw or remove.", list);
	}

	internal static SelfTestChecks.Outcome GrantedItemsAreRemovable()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (SelfTestRecorder.CreatedBlueprint item in SelfTestRecorder.CreatedBlueprints())
		{
			if (!(item.Blueprint is BlueprintUnitFact blueprintUnitFact))
			{
				continue;
			}
			BlueprintComponent[] array = blueprintUnitFact.ComponentsArray ?? new BlueprintComponent[0];
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] is AddStartingEquipment { BasicItems: var basicItems }))
				{
					continue;
				}
				foreach (BlueprintItem item2 in basicItems)
				{
					if (item2 != null)
					{
						num++;
						if (item2 is BlueprintItemEquipment { IsNonRemovable: not false })
						{
							num2++;
							list.Add(item.Name + " grants " + item2.name + ", which is IsNonRemovable: once it is equipped the player can never take it off.");
						}
					}
				}
			}
		}
		return SelfTestChecks.Outcome.Of(num2 == 0, $"{num} items granted as starting equipment; {num2} the player could not unequip.", list);
	}
}
