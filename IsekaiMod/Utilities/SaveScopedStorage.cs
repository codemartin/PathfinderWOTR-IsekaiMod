using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;

namespace IsekaiMod.Utilities;

internal static class SaveScopedStorage
{
	internal const string SpawnMarkerStorage = "IsekaiMod.Spawn";

	internal const string SpawnKindKey = "Kind";

	internal const string KindColiseumContestant = "ColiseumContestant";

	internal const string KindColiseumCameo = "ColiseumCameo";

	internal const string KindBarrenBoss = "BarrenBoss";

	internal const string KindReinforcement = "DimensionalReinforcement";

	internal const string KindRiftIncursionBoss = "RiftIncursionBoss";

	private const char ListSeparator = ',';

	internal static Dictionary<string, string> Find(EntityDataBase entity, string storageName)
	{
		if (entity == null || string.IsNullOrEmpty(storageName))
		{
			return null;
		}
		try
		{
			EntityPartKeyValueStorage entityPartKeyValueStorage = entity.Get<EntityPartKeyValueStorage>();
			if (entityPartKeyValueStorage == null || entityPartKeyValueStorage.m_Data == null)
			{
				return null;
			}
			Dictionary<string, string> value;
			return entityPartKeyValueStorage.m_Data.TryGetValue(storageName, out value) ? value : null;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error reading save-scoped storage " + storageName + ": " + ex);
			return null;
		}
	}

	internal static Dictionary<string, string> Ensure(EntityDataBase entity, string storageName)
	{
		if (entity == null || string.IsNullOrEmpty(storageName))
		{
			return null;
		}
		try
		{
			return entity.Ensure<EntityPartKeyValueStorage>().GetStorage(storageName);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error creating save-scoped storage " + storageName + ": " + ex);
			return null;
		}
	}

	internal static void Drop(EntityDataBase entity, string storageName)
	{
		if (entity == null || string.IsNullOrEmpty(storageName))
		{
			return;
		}
		try
		{
			entity.Get<EntityPartKeyValueStorage>()?.m_Data?.Remove(storageName);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error clearing save-scoped storage " + storageName + ": " + ex);
		}
	}

	internal static EntityDataBase CurrentPlayer()
	{
		try
		{
			return Game.Instance?.Player;
		}
		catch
		{
			return null;
		}
	}

	internal static void MarkSpawned(UnitEntityData unit, string kind)
	{
		Dictionary<string, string> dictionary = Ensure(unit, "IsekaiMod.Spawn");
		if (dictionary != null)
		{
			dictionary["Kind"] = kind ?? "";
		}
	}

	internal static string SpawnKindOf(UnitEntityData unit)
	{
		return Get(Find(unit, "IsekaiMod.Spawn"), "Kind");
	}

	internal static Dictionary<string, string> SpawnMarker(UnitEntityData unit)
	{
		return Find(unit, "IsekaiMod.Spawn");
	}

	internal static string Get(IDictionary<string, string> storage, string key)
	{
		if (storage == null || key == null)
		{
			return null;
		}
		if (!storage.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}

	internal static int GetInt(IDictionary<string, string> storage, string key, int fallback = 0)
	{
		if (!int.TryParse(Get(storage, key), out var result))
		{
			return fallback;
		}
		return result;
	}

	internal static bool GetFlag(IDictionary<string, string> storage, string key)
	{
		return Get(storage, key) == "1";
	}

	internal static void SetFlag(IDictionary<string, string> storage, string key, bool value)
	{
		if (storage != null && key != null)
		{
			if (value)
			{
				storage[key] = "1";
			}
			else
			{
				storage.Remove(key);
			}
		}
	}

	internal static List<string> GetList(IDictionary<string, string> storage, string key)
	{
		string text = Get(storage, key);
		if (string.IsNullOrEmpty(text))
		{
			return new List<string>();
		}
		return text.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	}

	internal static void SetList(IDictionary<string, string> storage, string key, IEnumerable<string> values)
	{
		if (storage != null && key != null)
		{
			List<string> list = ((values == null) ? new List<string>() : values.Where((string v) => !string.IsNullOrEmpty(v) && v.IndexOf(',') < 0).Distinct().ToList());
			if (list.Count == 0)
			{
				storage.Remove(key);
			}
			else
			{
				storage[key] = string.Join(','.ToString(), list.ToArray());
			}
		}
	}
}
