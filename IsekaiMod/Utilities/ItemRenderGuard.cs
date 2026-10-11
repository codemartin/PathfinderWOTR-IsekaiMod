using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

public static class ItemRenderGuard
{
	internal enum SweepAction
	{
		Keep,
		Replace,
		ReportOnly
	}

	private class AreaLoadSweep : IAreaHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnAreaDidLoad()
		{
			try
			{
				SweepPartyInventory("The area finished loading");
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ItemRenderGuard.OnAreaDidLoad: " + ex);
			}
		}

		public void OnAreaBeginUnloading()
		{
		}
	}

	internal const string ArmagsBladeGuid = "ce0842546b73aa34b8fcf40a970ede68";

	internal const string GreatswordTypeGuid = "5f824fbb0766a3543bbd6ae50248688f";

	internal const string ArmagsBladeReplacementGuid = "5f6b606de240fcf4d941343e6c055179";

	private static readonly HashSet<string> s_Reported = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	internal static bool CanRender(bool isWeapon, bool hasWeaponType)
	{
		return !isWeapon | hasWeaponType;
	}

	internal static string ReplacementFor(string blueprintGuid)
	{
		if (!string.Equals(blueprintGuid, "ce0842546b73aa34b8fcf40a970ede68", StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		return "5f6b606de240fcf4d941343e6c055179";
	}

	internal static SweepAction Classify(string blueprintGuid, bool isWeapon, bool hasWeaponType)
	{
		if (ReplacementFor(blueprintGuid) != null)
		{
			return SweepAction.Replace;
		}
		if (!CanRender(isWeapon, hasWeaponType))
		{
			return SweepAction.ReportOnly;
		}
		return SweepAction.Keep;
	}

	internal static bool ShouldSweep(string blueprintGuid, bool isWeapon, bool hasWeaponType)
	{
		return Classify(blueprintGuid, isWeapon, hasWeaponType) == SweepAction.Replace;
	}

	internal static string UnrenderableLine(string reason, string blueprintName, string blueprintGuid, int count)
	{
		return string.Format("[ItemRenderGuard] {0}: the party holds {1} x {2} ({3}), a weapon whose weapon type does not resolve.", reason, count, blueprintName ?? "(unnamed)", blueprintGuid) + " The inventory and vendor screens cannot draw it. It is not one of the items this mod repairs, so it was left in the inventory; the mod that added it (or one that was switched off) is the place to fix it.";
	}

	internal static string RenderProblem(BlueprintItem blueprint)
	{
		if (blueprint == null)
		{
			return "the item has no blueprint";
		}
		try
		{
			if (blueprint is BlueprintItemWeapon { Type: null })
			{
				return "a weapon whose weapon type does not resolve";
			}
			_ = blueprint.Name;
			_ = blueprint.NonIdentifiedName;
			_ = blueprint.SubtypeName;
			_ = blueprint.Icon;
			return null;
		}
		catch (Exception ex)
		{
			return ex.GetType().Name + ": " + ex.Message;
		}
	}

	public static void Init()
	{
		RepairBlueprints();
		EventBus.Subscribe(new AreaLoadSweep());
	}

	internal static void RepairBlueprints()
	{
		BlueprintItemWeapon blueprint = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("ce0842546b73aa34b8fcf40a970ede68");
		if (blueprint != null && blueprint.Type == null)
		{
			blueprint.m_Type = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("5f824fbb0766a3543bbd6ae50248688f");
			Main.IsekaiContext.Logger.Log("[ItemRenderGuard] ArmagsBladeItem named a weapon type the game does not have; it is now a greatsword, so the inventory can draw it.");
		}
	}

	internal static int SweepPartyInventory(string reason)
	{
		ItemsCollection itemsCollection = Game.Instance?.Player?.Inventory;
		if (itemsCollection == null)
		{
			return 0;
		}
		Dictionary<BlueprintItem, int> dictionary = new Dictionary<BlueprintItem, int>();
		Dictionary<BlueprintItem, int> dictionary2 = new Dictionary<BlueprintItem, int>();
		foreach (ItemEntity item in itemsCollection.Items.ToList())
		{
			BlueprintItem blueprintItem = item?.Blueprint;
			if (blueprintItem != null)
			{
				bool flag = blueprintItem is BlueprintItemWeapon;
				bool hasWeaponType = flag && ((BlueprintItemWeapon)blueprintItem).Type != null;
				SweepAction sweepAction = Classify(blueprintItem.AssetGuid.ToString(), flag, hasWeaponType);
				if (sweepAction != SweepAction.Keep)
				{
					Dictionary<BlueprintItem, int> obj = ((sweepAction == SweepAction.Replace) ? dictionary : dictionary2);
					obj.TryGetValue(blueprintItem, out var value);
					obj[blueprintItem] = value + Math.Max(1, item.Count);
				}
			}
		}
		foreach (KeyValuePair<BlueprintItem, int> item2 in dictionary2)
		{
			string text = item2.Key.AssetGuid.ToString();
			bool flag2;
			lock (s_Reported)
			{
				flag2 = s_Reported.Add(text);
			}
			if (flag2)
			{
				Main.IsekaiContext.Logger.LogError(UnrenderableLine(reason, item2.Key.name, text, item2.Value));
			}
		}
		int num = 0;
		foreach (KeyValuePair<BlueprintItem, int> item3 in dictionary)
		{
			try
			{
				int num2 = itemsCollection.Remove(item3.Key, item3.Value);
				num += num2;
				string text2 = ReplacementFor(item3.Key.AssetGuid.ToString());
				BlueprintItem blueprintItem2 = ((text2 == null) ? null : BlueprintTools.GetBlueprint<BlueprintItem>(text2));
				if (blueprintItem2 != null && num2 > 0)
				{
					itemsCollection.Add(blueprintItem2, num2);
				}
				Main.IsekaiContext.Logger.Log($"[ItemRenderGuard] {reason}: took {num2} x {item3.Key.name} out of the inventory because the UI cannot draw it" + ((blueprintItem2 != null) ? ("; replaced with " + blueprintItem2.name + ".") : "."));
			}
			catch (Exception arg)
			{
				Main.IsekaiContext.Logger.LogError($"[ItemRenderGuard] Could not take {item3.Key.name} out of the inventory: {arg}");
			}
		}
		return num;
	}
}
