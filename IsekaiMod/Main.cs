using System;
using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using IsekaiMod.ModLogic;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityModManagerNet;

namespace IsekaiMod
{
	internal static class Main
	{
		public class Message
		{
			public string Text { get; set; }

			public DateTime Timestamp { get; set; }

			public Color Color { get; set; }
		}

		public static ModContextTTTBase IsekaiContext;

		public static bool Load(UnityModManager.ModEntry modEntry)
		{
			IsekaiContext = new ModContextTTTBase(modEntry);
			try
			{
				Harmony harmony = new Harmony(modEntry.Info.Id);
				IsekaiContext.ModEntry.OnSaveGUI = OnSaveGUI;
				IsekaiContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
				harmony.PatchAll();
				Utilities.CombatPerformanceDiagnostics.Install(harmony);
				PostPatchInitializer.Initialize(IsekaiContext);
				return true;
			}
			catch (Exception ex)
			{
				Log(ex.ToString());
				throw ex;
			}
		}

		public static void Log(string msg)
		{
			IsekaiContext.Logger.Log(msg);
		}

		public static void LogToGeneral(string message)
		{
			MessageLogThread messageLogThread = LogThreadService.Instance.GetThreadsByChannelType(default(LogChannelType)).OfType<MessageLogThread>().FirstOrDefault();
			if (messageLogThread != null)
			{
				CombatLogMessage newMessage = new CombatLogMessage(message, Color.white, PrefixIcon.None, new TooltipTemplateSimple(null, message));
				((LogThreadBase)messageLogThread).AddMessage(newMessage);
			}
		}

		[Conditional("DEBUG")]
		public static void LogDebug(string msg)
		{
			IsekaiContext.Logger.Log(msg);
		}

		private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
		{
			IsekaiContext.SaveAllSettings();
		}
	}
}
