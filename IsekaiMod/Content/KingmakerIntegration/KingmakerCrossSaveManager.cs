using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace IsekaiMod.Content.KingmakerIntegration
{
	public static class KingmakerCrossSaveManager
	{
		private static bool? _isKingmakerInstalled;

		private static List<string> _availableRulerNames;

		private static string _selectedRulerName;

		private static readonly string DefaultRulerName = "the Sovereign of the Stolen Lands";

		public static bool IsKingmakerInstalled
		{
			get
			{
				if (!_isKingmakerInstalled.HasValue)
				{
					_isKingmakerInstalled = CheckKingmakerInstalled();
				}
				return _isKingmakerInstalled.Value;
			}
		}

		public static string SelectedRulerName
		{
			get
			{
				return GetRulerName();
			}
			set
			{
				SetRulerName(value);
			}
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
			return DefaultRulerName;
		}

		public static void SetRulerName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				_selectedRulerName = name;
				Main.IsekaiContext.Logger.Log("[KingmakerCrossSave] Selected canon Stolen Lands ruler: " + _selectedRulerName);
			}
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
				if (string.IsNullOrEmpty(kingmakerSaveDirectory) || !Directory.Exists(kingmakerSaveDirectory))
				{
					return _availableRulerNames;
				}
				List<FileInfo> list = (from f in new DirectoryInfo(kingmakerSaveDirectory).GetFiles("*.zks")
					orderby f.LastWriteTimeUtc descending
					select f).ToList();
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (FileInfo item in list)
				{
					try
					{
						using ZipArchive zipArchive = ZipFile.OpenRead(item.FullName);
						ZipArchiveEntry entry = zipArchive.GetEntry("header.json");
						if (entry == null)
						{
							continue;
						}
						using Stream stream = entry.Open();
						using StreamReader streamReader = new StreamReader(stream);
						string text = (string)JObject.Parse(streamReader.ReadToEnd())["GameName"];
						if (!string.IsNullOrEmpty(text) && !hashSet.Contains(text))
						{
							hashSet.Add(text);
							_availableRulerNames.Add(text);
						}
					}
					catch (Exception ex)
					{
						Main.IsekaiContext.Logger.Log("[KingmakerCrossSave] Notice: skipping save " + item.Name + ": " + ex.Message);
					}
				}
			}
			catch (Exception arg)
			{
				Main.IsekaiContext.Logger.LogError($"[KingmakerCrossSave] Error scanning saves: {arg}");
			}
			return _availableRulerNames;
		}

		private static bool CheckKingmakerInstalled()
		{
			try
			{
				string[] array = new string[6] { "Z:\\SteamLibrary\\steamapps\\common\\Pathfinder Kingmaker", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Pathfinder Kingmaker", "D:\\SteamLibrary\\steamapps\\common\\Pathfinder Kingmaker", "E:\\SteamLibrary\\steamapps\\common\\Pathfinder Kingmaker", "C:\\GOG Games\\Pathfinder Kingmaker", "D:\\GOG Games\\Pathfinder Kingmaker" };
				foreach (string text in array)
				{
					if (Directory.Exists(text) && (File.Exists(Path.Combine(text, "Kingmaker.exe")) || Directory.Exists(Path.Combine(text, "Kingmaker_Data"))))
					{
						Main.IsekaiContext.Logger.Log("[KingmakerCrossSave] Detected Pathfinder Kingmaker installation at: " + text);
						return true;
					}
				}
				try
				{
					using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 640820");
					if (registryKey != null)
					{
						string text2 = registryKey.GetValue("InstallLocation") as string;
						if (!string.IsNullOrEmpty(text2) && Directory.Exists(text2))
						{
							Main.IsekaiContext.Logger.Log("[KingmakerCrossSave] Detected Kingmaker via Steam registry at: " + text2);
							return true;
						}
					}
				}
				catch
				{
				}
				string kingmakerSaveDirectory = GetKingmakerSaveDirectory();
				if (!string.IsNullOrEmpty(kingmakerSaveDirectory) && Directory.Exists(kingmakerSaveDirectory) && Directory.EnumerateFiles(kingmakerSaveDirectory, "*.zks").Any())
				{
					Main.IsekaiContext.Logger.Log("[KingmakerCrossSave] Detected Kingmaker save files in user profile.");
					return true;
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[KingmakerCrossSave] Safe check failed: " + ex.Message);
			}
			return false;
		}

		public static string GetKingmakerSaveDirectory()
		{
			try
			{
				string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData\\LocalLow\\Owlcat Games\\Pathfinder Kingmaker\\Saved Games");
				if (Directory.Exists(text))
				{
					return text;
				}
			}
			catch
			{
			}
			return null;
		}
	}
}
