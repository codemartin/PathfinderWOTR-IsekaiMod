using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features;
using IsekaiMod.Content.Quests;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;
using UnityEngine;

namespace IsekaiMod.Content.Constellations;

public class TimelineManager : IGameOverHandler, ISubscriber, IGlobalSubscriber, IAreaActivationHandler, IAreaHandler
{
	[HarmonyPatch(typeof(SaveManager), "PrepareSave")]
	private static class SaveManager_PrepareSave_Patch
	{
		[HarmonyPrefix]
		public static void Prefix()
		{
			try
			{
				Flush();
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Failed to flush TimelineHistory before a game save: " + ex);
			}
		}
	}

	private static TimelineData _data;

	private static string _filePath;

	private static bool _dirty;

	private static int _unsavedChanges;

	private const int MaxUnsavedChanges = 25;

	internal const string FileName = "TimelineHistory.json";

	internal const string HistoryFolderName = "IsekaiMod";

	internal const string MigratedSuffix = ".migrated";

	internal const int MaxLoopEndReasons = 50;

	internal const double MaxCoinLoopMultiplier = 3.0;

	public static TimelineData Data
	{
		get
		{
			if (_data == null)
			{
				Load();
			}
			return _data;
		}
	}

	public static int TotalRuns => Data.TotalRuns;

	public static bool LoopBreakerAchieved => Data.LoopBreakerAchieved;

	public static bool LoopShattered => Data.LoopShattered;

	public static List<PastCycleRecord> PastCycles => Data.PastCycles;

	internal static string HistoryPath(string persistentDataPath, string applicationDataPath)
	{
		string text = ((!string.IsNullOrEmpty(persistentDataPath)) ? persistentDataPath : applicationDataPath);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return Path.Combine(text, "IsekaiMod", "TimelineHistory.json");
	}

	internal static bool MigrateLegacyFile(string legacyPath, string newPath)
	{
		if (string.IsNullOrEmpty(legacyPath) || string.IsNullOrEmpty(newPath))
		{
			return false;
		}
		if (string.Equals(Path.GetFullPath(legacyPath), Path.GetFullPath(newPath), StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		if (!File.Exists(legacyPath) || File.Exists(newPath))
		{
			return false;
		}
		string directoryName = Path.GetDirectoryName(newPath);
		if (!string.IsNullOrEmpty(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		WriteAtomically(newPath, File.ReadAllText(legacyPath));
		string text = legacyPath + ".migrated";
		int num = 1;
		while (File.Exists(text))
		{
			text = legacyPath + ".migrated-" + num++;
		}
		File.Move(legacyPath, text);
		return true;
	}

	internal static void WriteAtomically(string path, string contents)
	{
		string directoryName = Path.GetDirectoryName(path);
		if (!string.IsNullOrEmpty(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		string text = path + ".tmp";
		byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false).GetBytes(contents ?? string.Empty);
		using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.None))
		{
			fileStream.Write(bytes, 0, bytes.Length);
			fileStream.Flush(flushToDisk: true);
		}
		if (!File.Exists(path))
		{
			File.Move(text, path);
			return;
		}
		try
		{
			File.Replace(text, path, null);
		}
		catch (Exception ex) when (ex is PlatformNotSupportedException || ex is NotSupportedException || ex is IOException)
		{
			File.Delete(path);
			File.Move(text, path);
		}
	}

	internal static bool RecoverInterruptedWrite(string path)
	{
		string text = path + ".tmp";
		if (string.IsNullOrEmpty(path) || File.Exists(path) || !File.Exists(text))
		{
			return false;
		}
		try
		{
			if (JsonConvert.DeserializeObject<TimelineData>(File.ReadAllText(text)) == null)
			{
				return false;
			}
		}
		catch
		{
			return false;
		}
		File.Move(text, path);
		return true;
	}

	private static void EnsureFilePath()
	{
		if (!string.IsNullOrEmpty(_filePath))
		{
			return;
		}
		string persistentDataPath = null;
		try
		{
			persistentDataPath = Application.persistentDataPath;
		}
		catch
		{
		}
		_filePath = HistoryPath(persistentDataPath, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) ?? Path.Combine(Main.IsekaiContext.ModEntry.Path, "TimelineHistory.json");
		try
		{
			string text = Main.IsekaiContext?.ModEntry?.Path;
			if (!string.IsNullOrEmpty(text) && MigrateLegacyFile(Path.Combine(text, "TimelineHistory.json"), _filePath))
			{
				Main.IsekaiContext.Logger.Log("TimelineHistory.json moved out of the mod folder to " + _filePath + " so that updating or reinstalling the mod keeps it.");
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Could not move TimelineHistory.json out of the mod folder: " + ex);
		}
	}

	public static void Init()
	{
		EnsureFilePath();
		Load();
		EventBus.Subscribe(new TimelineManager());
	}

	public static void Load()
	{
		try
		{
			EnsureFilePath();
			try
			{
				if (RecoverInterruptedWrite(_filePath))
				{
					Main.IsekaiContext.Logger.LogWarning("TimelineHistory.json was restored from the write that was interrupted last session.");
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Could not restore TimelineHistory.json from an interrupted write: " + ex);
			}
			if (File.Exists(_filePath))
			{
				string value = File.ReadAllText(_filePath);
				TimelineData timelineData = null;
				try
				{
					timelineData = JsonConvert.DeserializeObject<TimelineData>(value);
				}
				catch (Exception ex2)
				{
					Main.IsekaiContext.Logger.LogError("TimelineHistory.json could not be parsed: " + ex2);
				}
				if (timelineData == null)
				{
					BackUpCorruptFile();
					_data = new TimelineData();
					MarkDirty();
					Flush();
				}
				else
				{
					_data = timelineData;
				}
			}
			else
			{
				_data = new TimelineData();
				MarkDirty();
				Flush();
			}
		}
		catch (Exception ex3)
		{
			Main.IsekaiContext.Logger.LogError("Failed to load TimelineHistory: " + ex3);
			_data = new TimelineData();
		}
		if (Compact(_data))
		{
			MarkDirty();
			Flush();
		}
	}

	private static void BackUpCorruptFile()
	{
		try
		{
			string text = _filePath + ".bak-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
			int num = 1;
			while (File.Exists(text))
			{
				text = _filePath + ".bak-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + num++;
			}
			File.Move(_filePath, text);
			Main.IsekaiContext.Logger.LogWarning("TimelineHistory.json was unreadable and has been kept as " + Path.GetFileName(text) + "; a new history was started.");
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Could not back up the corrupt TimelineHistory.json: " + ex);
		}
	}

	internal static bool Compact(TimelineData data)
	{
		if (data == null)
		{
			return false;
		}
		bool result = false;
		if (data.PastChoices == null || data.PastChoices.Count > 0)
		{
			data.PastChoices = new List<string>();
			result = true;
		}
		if (data.LoopEndReasons != null && data.LoopEndReasons.Count > 50)
		{
			data.LoopEndReasons.RemoveRange(0, data.LoopEndReasons.Count - 50);
			result = true;
		}
		return result;
	}

	private static void MarkDirty()
	{
		_dirty = true;
		_unsavedChanges++;
		if (_unsavedChanges >= 25)
		{
			Flush();
		}
	}

	public static void Flush()
	{
		if (_dirty)
		{
			WriteFile();
		}
	}

	public static void Save()
	{
		_dirty = true;
		WriteFile();
	}

	private static void WriteFile()
	{
		try
		{
			EnsureFilePath();
			string contents = JsonConvert.SerializeObject(_data, Formatting.Indented);
			WriteAtomically(_filePath, contents);
			_dirty = false;
			_unsavedChanges = 0;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Failed to save TimelineHistory: " + ex);
		}
	}

	public static string ActiveRunArchetypeForCurrentSave()
	{
		return LookupRunArchetype(Data.RunArchetypes, CurrentSaveId());
	}

	internal static string LookupRunArchetype(IList<string> entries, string gameId)
	{
		if (entries == null || string.IsNullOrEmpty(gameId))
		{
			return string.Empty;
		}
		string text = gameId + "|";
		for (int i = 0; i < entries.Count; i++)
		{
			string text2 = entries[i];
			if (text2 != null && text2.StartsWith(text, StringComparison.Ordinal))
			{
				return ArchetypeOrigin.Normalize(text2.Substring(text.Length));
			}
		}
		return string.Empty;
	}

	internal static bool RecordRunArchetype(List<string> entries, string gameId, string archetype)
	{
		if (entries == null || string.IsNullOrEmpty(gameId))
		{
			return false;
		}
		string text = ArchetypeOrigin.Normalize(archetype);
		if (text.Length == 0)
		{
			return false;
		}
		if (LookupRunArchetype(entries, gameId).Length > 0)
		{
			return false;
		}
		entries.Add(gameId + "|" + text);
		return true;
	}

	public static void SetActiveRunArchetype(string archetype)
	{
		TimelineData data = Data;
		if (data.RunArchetypes == null)
		{
			data.RunArchetypes = new List<string>();
		}
		if (RecordRunArchetype(Data.RunArchetypes, CurrentSaveId(), archetype))
		{
			Save();
		}
	}

	public static void RecordMythicPath(string pathName)
	{
		if (!Data.PastMythicPaths.Contains(pathName))
		{
			Data.PastMythicPaths.Add(pathName);
			Save();
		}
	}

	internal static CycleClosure DecideCycleClosure(IList<string> closedCycles, string gameId, bool loopBreakingEnding, bool shatterRequirementsMet)
	{
		if (TryGetClosedCycle(closedCycles, gameId, out var _, out var _))
		{
			return CycleClosure.AlreadyClosed;
		}
		if (!(loopBreakingEnding & shatterRequirementsMet))
		{
			return CycleClosure.Resolved;
		}
		return CycleClosure.Shattered;
	}

	internal static bool TryGetClosedCycle(IList<string> closedCycles, string gameId, out string endingId, out bool shattered)
	{
		endingId = null;
		shattered = false;
		if (closedCycles == null)
		{
			return false;
		}
		string text = gameId + "|";
		for (int i = 0; i < closedCycles.Count; i++)
		{
			string text2 = closedCycles[i];
			if (text2 != null && text2.StartsWith(text, StringComparison.Ordinal))
			{
				string text3 = text2.Substring(text.Length);
				int num = text3.LastIndexOf('|');
				endingId = ((num >= 0) ? text3.Substring(0, num) : text3);
				shattered = num >= 0 && text3.Substring(num + 1) == "1";
				return true;
			}
		}
		return false;
	}

	internal static void ApplyCycleClosure(TimelineData data, string gameId, CycleClosure closure, PastCycleRecord record)
	{
		if (data == null || closure == CycleClosure.AlreadyClosed || TryGetClosedCycle(data.ClosedCycles, gameId, out var _, out var _))
		{
			return;
		}
		bool flag = closure == CycleClosure.Shattered;
		string text = record?.EndingId ?? "";
		TimelineData timelineData = data;
		if (timelineData.ClosedCycles == null)
		{
			timelineData.ClosedCycles = new List<string>();
		}
		data.ClosedCycles.Add(gameId + "|" + text + "|" + (flag ? "1" : "0"));
		timelineData = data;
		if (timelineData.ClearedEndings == null)
		{
			timelineData.ClearedEndings = new List<string>();
		}
		if (!string.IsNullOrEmpty(text) && !data.ClearedEndings.Contains(text))
		{
			data.ClearedEndings.Add(text);
		}
		if (record != null)
		{
			record.CycleNumber = data.TotalRuns;
			timelineData = data;
			if (timelineData.PastCycles == null)
			{
				timelineData.PastCycles = new List<PastCycleRecord>();
			}
			data.PastCycles.Add(record);
		}
		if (flag)
		{
			data.LoopShattered = true;
			data.LoopBreakerAchieved = true;
		}
		string text2 = ((record == null) ? text : record.EndingAchieved);
		AdvanceLoop(data, flag ? ("Loop Shattered - " + text2) : text2);
	}

	internal static void CloseCycle(string gameId, CycleClosure closure, string endingId, string endingTitle, string archetype, string mythicPath, string romancedCompanion, string characterName, string primaryLegacy = null, string secondaryLegacy = null)
	{
		if (closure != CycleClosure.AlreadyClosed)
		{
			PastCycleRecord record = new PastCycleRecord
			{
				PrimaryLegacy = (primaryLegacy ?? ""),
				SecondaryLegacy = (secondaryLegacy ?? ""),
				EndingId = (endingId ?? ""),
				EndingTitle = (endingTitle ?? ""),
				CharacterName = (string.IsNullOrEmpty(characterName) ? "The Transmigrator" : characterName),
				Archetype = (string.IsNullOrEmpty(archetype) ? ArchetypeNameForSave(gameId) : archetype),
				MythicPath = (string.IsNullOrEmpty(mythicPath) ? "Legend" : mythicPath),
				RomancedCompanion = (string.IsNullOrEmpty(romancedCompanion) ? "None" : romancedCompanion),
				CompletionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
			};
			ApplyCycleClosure(Data, gameId, closure, record);
			Save();
		}
	}

	internal static string ArchetypeNameForSave(string gameId)
	{
		string text = LookupRunArchetype(Data.RunArchetypes, gameId);
		if (text.Length != 0)
		{
			return ArchetypeOrigin.DisplayName(text);
		}
		return "Base Protagonist";
	}

	public static PastCycleRecord GetLastCycle()
	{
		if (Data.PastCycles == null || Data.PastCycles.Count <= 0)
		{
			return null;
		}
		return Data.PastCycles[Data.PastCycles.Count - 1];
	}

	public static bool HasClearedEnding(string endingId)
	{
		return Data.ClearedEndings.Contains(endingId);
	}

	public static bool HasPlayedArchetype(string archetype)
	{
		if (Data.PastCycles == null)
		{
			return false;
		}
		for (int i = 0; i < Data.PastCycles.Count; i++)
		{
			if (Data.PastCycles[i].Archetype.IndexOf(archetype, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	private static void AdvanceLoop(TimelineData data, string reason)
	{
		data.TotalRuns++;
		data.RecentDeath = false;
		data.RecentDeathGameId = "";
		data.RunArchetypes?.Clear();
		data.ShardRelicGranted = false;
		if (!string.IsNullOrEmpty(reason))
		{
			if (data.LoopEndReasons == null)
			{
				data.LoopEndReasons = new List<string>();
			}
			data.LoopEndReasons.Add($"Loop {data.TotalRuns - 1} End: {reason}");
			if (data.LoopEndReasons.Count > 50)
			{
				data.LoopEndReasons.RemoveRange(0, data.LoopEndReasons.Count - 50);
			}
		}
	}

	public static string CurrentSaveId()
	{
		try
		{
			return Game.Instance?.Player?.GameId ?? "";
		}
		catch
		{
			return "";
		}
	}

	internal static int RunNumberForSave(List<string> owners, string gameId, int currentRun, out bool added)
	{
		added = false;
		if (owners == null || string.IsNullOrEmpty(gameId))
		{
			return currentRun;
		}
		string text = gameId + "|";
		for (int i = 0; i < owners.Count; i++)
		{
			string text2 = owners[i];
			if (text2 != null && text2.StartsWith(text, StringComparison.Ordinal))
			{
				if (int.TryParse(text2.Substring(text.Length), out var result) && result > 0)
				{
					return result;
				}
				return currentRun;
			}
		}
		owners.Add(text + currentRun);
		added = true;
		return currentRun;
	}

	public static void IncrementDeath(string areaName)
	{
		Data.DeathCount++;
		Data.RecentDeath = true;
		Data.LastArea = areaName;
		Data.RecentDeathGameId = CurrentSaveId();
		Save();
	}

	public void HandleGameOver(Player.GameOverReasonType reason)
	{
		if (reason == Player.GameOverReasonType.Won)
		{
			PostVanillaEndingFarewell();
			return;
		}
		string text = null;
		try
		{
			text = Game.Instance?.CurrentlyLoadedArea?.AreaDisplayName;
		}
		catch
		{
			text = null;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = Game.Instance?.CurrentlyLoadedArea?.name ?? "UnknownArea";
		}
		IncrementDeath(text);
	}

	internal static bool VanillaFarewellDue(bool won, bool transmigrant, IList<string> closedCycles, string gameId)
	{
		string endingId;
		bool shattered;
		if (won & transmigrant)
		{
			return !TryGetClosedCycle(closedCycles, gameId, out endingId, out shattered);
		}
		return false;
	}

	private static void PostVanillaEndingFarewell()
	{
		try
		{
			if (!VanillaFarewellDue(won: true, IsekaiTransmigrationQuest.PlayerCanStartTransmigration(), Data.ClosedCycles, CurrentSaveId()))
			{
				return;
			}
			foreach (string item in ConstellationDialogueBanter.GetVanillaEndingFarewell())
			{
				ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, item, null, ConstellationCategory.MetaLoop, "The Grand Final");
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error posting the vanilla ending farewell: " + ex);
		}
	}

	public void OnAreaBeginUnloading()
	{
		Flush();
	}

	internal static bool ShardGrantDue(bool loopBreakerAchieved, int saveRun, int currentRun, ICollection<string> grantedGames, string gameId)
	{
		if (!loopBreakerAchieved)
		{
			return false;
		}
		if (saveRun <= 1 || saveRun != currentRun)
		{
			return false;
		}
		if (!string.IsNullOrEmpty(gameId) && grantedGames != null)
		{
			return !grantedGames.Contains(gameId);
		}
		return true;
	}

	public void OnAreaDidLoad()
	{
	}

	internal static int LookupRunNumberForSave(List<string> owners, string gameId, int currentRun)
	{
		if (owners == null || string.IsNullOrEmpty(gameId))
		{
			return currentRun;
		}
		string text = gameId + "|";
		for (int i = 0; i < owners.Count; i++)
		{
			string text2 = owners[i];
			if (text2 != null && text2.StartsWith(text, StringComparison.Ordinal))
			{
				if (!int.TryParse(text2.Substring(text.Length), out var result) || result <= 0)
				{
					return currentRun;
				}
				return result;
			}
		}
		return currentRun;
	}

	internal static double LoopMultiplierForRun(int runNumber)
	{
		if (runNumber <= 1)
		{
			return 1.0;
		}
		double num = 1.0 + (double)(runNumber - 1) * 0.25;
		if (!(num > 3.0))
		{
			return num;
		}
		return 3.0;
	}

	public static double GetCoinLoopMultiplier()
	{
		return LoopMultiplierForRun(LookupRunNumberForSave(Data.RunOwners, CurrentSaveId(), Data.TotalRuns));
	}

	public void OnAreaActivated()
	{
		IsekaiTransmigrationQuest.StartQuest();
		string text = CurrentSaveId();
		int num = RunNumberForSave(Data.RunOwners, text, Data.TotalRuns, out var added);
		if (added)
		{
			MarkDirty();
		}
		if (num > 1)
		{
			UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
			BlueprintFeature blueprintFeature = EchoOfPastSovereigns.Get();
			if (unitEntityData != null && blueprintFeature != null)
			{
				Feature fact = unitEntityData.Descriptor.Progression.Features.GetFact(blueprintFeature);
				int num2 = num - 1;
				if (fact == null)
				{
					fact = unitEntityData.Descriptor.Progression.Features.AddFeature(blueprintFeature);
					if (fact != null && num2 > 1)
					{
						fact.SetRank(num2);
					}
					ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, ConstellationText.LocalizedFormat("Constellation.Timeline.EchoAwakens", "<color=#133E71><b>[Continuity of the Multiverse]</b></color> The <b>Echo of Past Sovereigns</b> awakens within you (Loop {0}: +{1} competence bonus to all skills, +{2}% Cosmic Coins).", num, num2, Math.Round((GetCoinLoopMultiplier() - 1.0) * 100.0)), "The Constellations", ConstellationCategory.MetaLoop, "New Cycle Progression");
				}
				else if (fact.GetRank() < num2)
				{
					fact.SetRank(num2);
				}
			}
		}
		if (ShardGrantDue(Data.LoopBreakerAchieved, num, Data.TotalRuns, Data.ShardGrantedGames, text))
		{
			UnitEntityData unitEntityData2 = Game.Instance?.Player?.MainCharacter.Value;
			BlueprintFeature blueprintFeature2 = ShardOfTheShatteredLoop.Get();
			if (unitEntityData2 != null && blueprintFeature2 != null)
			{
				if (!unitEntityData2.Progression.Features.HasFact(blueprintFeature2))
				{
					unitEntityData2.Progression.Features.AddFeature(blueprintFeature2);
					DivineTokens.AddClaimedCoins(1000, "The Grand Arbiter");
					ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, "<color=#5B2A86><b>[Sovereign Return - NG+]</b></color> You have retained the <b>Shard of the Shattered Loop</b> and a 1,000-coin sponsorship from your victory over the causal loop!", "The Grand Arbiter", ConstellationCategory.MetaLoop, "Sovereign Return");
					ConstellationChatManager.PostLines(ConstellationDialogueBanter.GetLoopBreakerSovereignReturnBanter(), "The Constellations", ConstellationCategory.MetaLoop, "Sovereign Return");
				}
				Data.ShardRelicGranted = true;
				if (!string.IsNullOrEmpty(text))
				{
					TimelineData data = Data;
					if (data.ShardGrantedGames == null)
					{
						data.ShardGrantedGames = new List<string>();
					}
					Data.ShardGrantedGames.Add(text);
				}
				MarkDirty();
			}
		}
		if (Data.RecentDeath && string.Equals(Data.RecentDeathGameId, text, StringComparison.Ordinal))
		{
			ConstellationChatManager.OnDeathReload(Data.DeathCount, Data.LastArea);
			Data.RecentDeath = false;
			Data.RecentDeathGameId = "";
			MarkDirty();
		}
		Flush();
	}
}
