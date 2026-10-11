using System;
using System.Collections.Generic;
using IsekaiMod.Utilities;

namespace IsekaiMod.Content.Arenas;

internal sealed class ColiseumTournamentRecord
{
	internal const string StorageName = "IsekaiMod.ColiseumTournament";

	private const string CupKey = "Cup";

	private const string WaveKey = "Wave";

	private const string EnemiesKey = "Enemies";

	private const string SpawnedKey = "Spawned";

	internal const string CameoKey = "Cameo";

	public string Cup = "";

	public int Wave;

	public List<string> Enemies = new List<string>();

	public List<string> Spawned = new List<string>();

	internal static ColiseumTournamentRecord Read(IDictionary<string, string> storage)
	{
		string text = SaveScopedStorage.Get(storage, "Cup");
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return new ColiseumTournamentRecord
		{
			Cup = text,
			Wave = Math.Max(0, SaveScopedStorage.GetInt(storage, "Wave")),
			Enemies = SaveScopedStorage.GetList(storage, "Enemies"),
			Spawned = SaveScopedStorage.GetList(storage, "Spawned")
		};
	}

	internal void Write(IDictionary<string, string> storage)
	{
		if (storage != null)
		{
			storage["Cup"] = Cup ?? "";
			storage["Wave"] = Wave.ToString();
			SaveScopedStorage.SetList(storage, "Enemies", Enemies);
			SaveScopedStorage.SetList(storage, "Spawned", Spawned);
		}
	}

	internal static void Clear(IDictionary<string, string> storage)
	{
		if (storage != null)
		{
			storage.Remove("Cup");
			storage.Remove("Wave");
			storage.Remove("Enemies");
			storage.Remove("Spawned");
		}
	}
}
