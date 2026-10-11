using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Constellations;

internal static class ForbiddenCheatRecord
{
	private sealed class CheatRestoreHandler : IAreaHandler, ISubscriber, IGlobalSubscriber, IUnitChangedAfterRespecHandler
	{
		public void OnAreaDidLoad()
		{
			RestoreMissing();
		}

		public void OnAreaBeginUnloading()
		{
		}

		public void HandleUnitChangedAfterRespec(UnitEntityData unit)
		{
			RestoreMissing();
		}
	}

	internal const string StorageName = "IsekaiMod.ForbiddenCheats";

	private static bool s_subscribed;

	internal static string CheatKey(string factGuid)
	{
		return "Cheat." + (factGuid ?? "").ToLowerInvariant();
	}

	internal static void RecordPurchase(IDictionary<string, string> storage, string factGuid, string buyerId)
	{
		if (storage != null && !string.IsNullOrEmpty(factGuid) && !string.IsNullOrEmpty(buyerId))
		{
			string key = CheatKey(factGuid);
			List<string> list = SaveScopedStorage.GetList(storage, key);
			if (!list.Contains(buyerId))
			{
				list.Add(buyerId);
			}
			SaveScopedStorage.SetList(storage, key, list);
		}
	}

	internal static bool IsRecorded(IDictionary<string, string> storage, string factGuid, string buyerId)
	{
		if (storage == null || string.IsNullOrEmpty(factGuid) || string.IsNullOrEmpty(buyerId))
		{
			return false;
		}
		return SaveScopedStorage.GetList(storage, CheatKey(factGuid)).Contains(buyerId);
	}

	internal static List<string> MissingCheats(IDictionary<string, string> storage, string buyerId, Func<string, bool> hasFact)
	{
		List<string> list = new List<string>();
		if (storage == null || string.IsNullOrEmpty(buyerId) || hasFact == null)
		{
			return list;
		}
		foreach (KeyValuePair<string, string> item in storage.ToList())
		{
			if (item.Key != null && item.Key.StartsWith("Cheat.", StringComparison.Ordinal))
			{
				string text = item.Key.Substring("Cheat.".Length);
				if (SaveScopedStorage.GetList(storage, item.Key).Contains(buyerId) && !hasFact(text))
				{
					list.Add(text);
				}
			}
		}
		return list;
	}

	internal static void RecordPurchase(UnitEntityData buyer, BlueprintUnitFact fact)
	{
		if (!(buyer == null) && fact != null)
		{
			RecordPurchase(SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.ForbiddenCheats"), fact.AssetGuid.ToString(), buyer.UniqueId);
		}
	}

	internal static void RestoreMissing()
	{
		try
		{
			Dictionary<string, string> dictionary = SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.ForbiddenCheats");
			if (dictionary == null || dictionary.Count == 0)
			{
				return;
			}
			List<UnitEntityData> list = Game.Instance?.Player?.AllCharacters;
			if (list == null)
			{
				return;
			}
			foreach (UnitEntityData unit in list)
			{
				if (unit == null)
				{
					continue;
				}
				foreach (string item in MissingCheats(dictionary, unit.UniqueId, delegate(string guid)
				{
					BlueprintFeature blueprintFeature2 = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(BlueprintGuid.Parse(guid));
					return blueprintFeature2 == null || unit.Descriptor.Progression.Features.HasFact(blueprintFeature2);
				}))
				{
					BlueprintFeature blueprintFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(BlueprintGuid.Parse(item));
					if (blueprintFeature != null)
					{
						unit.Descriptor.AddFact(blueprintFeature);
						Main.IsekaiContext.Logger.Log("Forbidden Cheats: restored " + blueprintFeature.name + " to " + unit.CharacterName + " after a respec.");
					}
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error restoring bought Forbidden Cheats: " + ex);
		}
	}

	public static void Init()
	{
		if (!s_subscribed)
		{
			s_subscribed = true;
			EventBus.Subscribe(new CheatRestoreHandler());
		}
	}
}
