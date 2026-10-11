using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace IsekaiMod.Content.KingmakerIntegration;

public static class KingmakerCrossSaveManager
{
	private static bool? _savesFound;

	private static bool _loggedNotDetected;

	private static List<string> _availableRulerNames;

	private static string _selectedRulerName;

	internal const string DefaultRulerName = "the Sovereign of the Stolen Lands";

	public static bool IsKingmakerInstalled
	{
		get
		{
			bool valueOrDefault = Main.IsekaiContext?.AddedContent?.IOwnKingmaker == true;
			if (!_savesFound.HasValue)
			{
				_savesFound = KingmakerSavesFound();
			}
			bool num = IsKingmakerDetected(valueOrDefault, _savesFound.Value);
			if (!num && !_loggedNotDetected)
			{
				_loggedNotDetected = true;
				Main.IsekaiContext.Logger.Log("[KingmakerCrossSave] Pathfinder: Kingmaker not detected: no .zks save in " + (GetKingmakerSaveDirectory() ?? DefaultSaveDirectory() ?? "the Kingmaker save folder") + " and the \"I own Pathfinder: Kingmaker\" setting is off. The Sylvan Fey Alliance quest will not start and the Treaty of the Stolen Lands is offered only once that quest has started. Switch the setting on if you own Kingmaker and your saves are elsewhere.");
			}
			return num;
		}
	}

	public static string SelectedRulerName => GetRulerName();

	internal static bool SavesScanned => _availableRulerNames != null;

	internal static bool IsKingmakerDetected(bool manualOverride, bool savesFound)
	{
		return manualOverride | savesFound;
	}

	internal static string NewestSaveFile(IEnumerable<KeyValuePair<string, DateTime>> files)
	{
		if (files == null)
		{
			return null;
		}
		string text = null;
		DateTime dateTime = DateTime.MinValue;
		foreach (KeyValuePair<string, DateTime> file in files)
		{
			if (!string.IsNullOrEmpty(file.Key) && file.Key.EndsWith(".zks", StringComparison.OrdinalIgnoreCase) && (text == null || file.Value > dateTime))
			{
				text = file.Key;
				dateTime = file.Value;
			}
		}
		return text;
	}

	public static void ResetSelectedRuler()
	{
		_selectedRulerName = null;
	}

	public static string GetRulerNameForBlueprint()
	{
		if (!string.IsNullOrEmpty(_selectedRulerName))
		{
			return _selectedRulerName;
		}
		return "the Sovereign of the Stolen Lands";
	}

	public static string GetRulerName()
	{
		if (!string.IsNullOrEmpty(_selectedRulerName))
		{
			return _selectedRulerName;
		}
		List<string> availableRulerNames = GetAvailableRulerNames();
		if (availableRulerNames.Count > 0)
		{
			_selectedRulerName = availableRulerNames[0];
			return _selectedRulerName;
		}
		return "the Sovereign of the Stolen Lands";
	}

	public static List<string> GetAvailableRulerNames()
	{
		if (_availableRulerNames != null)
		{
			return _availableRulerNames;
		}
		_availableRulerNames = new List<string>();
		try
		{
			string kingmakerSaveDirectory = GetKingmakerSaveDirectory();
			if (string.IsNullOrEmpty(kingmakerSaveDirectory))
			{
				return _availableRulerNames;
			}
			string text = NewestSaveFile(from f in new DirectoryInfo(kingmakerSaveDirectory).GetFiles("*.zks")
				select new KeyValuePair<string, DateTime>(f.FullName, f.LastWriteTimeUtc));
			if (text == null)
			{
				return _availableRulerNames;
			}
			try
			{
				using ZipArchive zipArchive = ZipFile.OpenRead(text);
				ZipArchiveEntry entry = zipArchive.GetEntry("header.json");
				if (entry != null)
				{
					using Stream stream = entry.Open();
					using StreamReader streamReader = new StreamReader(stream);
					string text2 = (string)JObject.Parse(streamReader.ReadToEnd())["GameName"];
					if (!string.IsNullOrEmpty(text2))
					{
						_availableRulerNames.Add(text2);
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.Log("[KingmakerCrossSave] Notice: could not read the ruler's name from " + Path.GetFileName(text) + ": " + ex.Message);
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[KingmakerCrossSave] Error reading the Kingmaker save folder: {arg}");
		}
		return _availableRulerNames;
	}

	private static bool KingmakerSavesFound()
	{
		try
		{
			string kingmakerSaveDirectory = GetKingmakerSaveDirectory();
			if (!string.IsNullOrEmpty(kingmakerSaveDirectory) && Directory.EnumerateFiles(kingmakerSaveDirectory, "*.zks").Any())
			{
				Main.IsekaiContext.Logger.Log("[KingmakerCrossSave] Detected Pathfinder: Kingmaker saves in " + kingmakerSaveDirectory);
				return true;
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[KingmakerCrossSave] Checking the Kingmaker save folder failed: " + ex.Message);
		}
		return false;
	}

	private static string DefaultSaveDirectory()
	{
		try
		{
			return SaveDirectoryUnder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
		}
		catch
		{
			return null;
		}
	}

	internal static string SaveDirectoryUnder(string userProfile)
	{
		if (string.IsNullOrEmpty(userProfile))
		{
			return null;
		}
		return Path.Combine(userProfile, "AppData", "LocalLow", "Owlcat Games", "Pathfinder Kingmaker", "Saved Games");
	}

	internal static string SaveDirectoryBeside(string wrathPersistentDataPath)
	{
		if (string.IsNullOrEmpty(wrathPersistentDataPath))
		{
			return null;
		}
		string directoryName = Path.GetDirectoryName(wrathPersistentDataPath.TrimEnd('/', '\\'));
		if (string.IsNullOrEmpty(directoryName))
		{
			return null;
		}
		return Path.Combine(directoryName, "Pathfinder Kingmaker", "Saved Games");
	}

	public static string GetKingmakerSaveDirectory()
	{
		List<string> list = new List<string> { DefaultSaveDirectory() };
		try
		{
			list.Add(SaveDirectoryBeside(Application.persistentDataPath));
		}
		catch
		{
		}
		foreach (string item in list)
		{
			try
			{
				if (!string.IsNullOrEmpty(item) && Directory.Exists(item))
				{
					return item;
				}
			}
			catch
			{
			}
		}
		return null;
	}
}
