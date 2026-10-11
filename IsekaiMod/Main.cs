using System;
using System.Diagnostics;
using HarmonyLib;
using IsekaiMod.Content.Guardians;
using IsekaiMod.ModLogic;
using IsekaiMod.Utilities;
using IsekaiMod.Utilities.SelfTest;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityModManagerNet;

namespace IsekaiMod;

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
		try
		{
			LogSession.Write();
			IsekaiContext = new ModContextTTTBase(modEntry);
			Harmony harmony = new Harmony(modEntry.Info.Id);
			IsekaiContext.ModEntry.OnSaveGUI = OnSaveGUI;
			IsekaiContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
			IsekaiSelfTest.Arm();
			HarmonyStartup.PatchModAssembly(harmony);
			GuardianPetLevel.ApplyTabletopTweaksGate(harmony);
			PostPatchInitializer.Initialize(IsekaiContext);
			return true;
		}
		catch (Exception ex)
		{
			Log(ex.ToString());
			throw;
		}
	}

	public static void Log(string msg)
	{
		ModContextTTTBase isekaiContext = IsekaiContext;
		if (isekaiContext != null)
		{
			isekaiContext.Logger.Log(msg);
			return;
		}
		try
		{
			UnityModManager.Logger.Log("[IsekaiMod] " + msg);
		}
		catch
		{
		}
	}

	public static void LogError(string msg)
	{
		ModContextTTTBase isekaiContext = IsekaiContext;
		if (isekaiContext != null)
		{
			try
			{
				isekaiContext.Logger.LogError(msg);
				return;
			}
			catch
			{
			}
		}
		Log("ERROR: " + msg);
	}

	[Conditional("DEBUG")]
	public static void LogDebug(string msg)
	{
		Log(msg);
	}

	private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
	{
		IsekaiContext.SaveAllSettings();
	}
}
