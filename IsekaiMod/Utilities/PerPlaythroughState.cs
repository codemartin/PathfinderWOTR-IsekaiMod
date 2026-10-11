using System;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.KingmakerIntegration;
using Kingmaker.PubSubSystem;

namespace IsekaiMod.Utilities;

public class PerPlaythroughState : IAreaActivationHandler, ISubscriber, IGlobalSubscriber, IAreaHandler
{
	private static bool _initialized;

	private static string _lastSeenGameId;

	internal static string LastSeenGameId => _lastSeenGameId ?? string.Empty;

	public static void Init()
	{
		if (!_initialized)
		{
			_initialized = true;
			EventBus.Subscribe(new PerPlaythroughState());
		}
	}

	public void OnAreaActivated()
	{
		CheckLoadedGame();
	}

	public void OnAreaDidLoad()
	{
		CheckLoadedGame();
	}

	public void OnAreaBeginUnloading()
	{
	}

	internal static bool IsDifferentPlaythrough(string lastSeenGameId, string currentGameId)
	{
		if (string.IsNullOrEmpty(currentGameId))
		{
			return false;
		}
		if (string.IsNullOrEmpty(lastSeenGameId))
		{
			return true;
		}
		return !string.Equals(lastSeenGameId, currentGameId, StringComparison.Ordinal);
	}

	internal static void ForgetLastSeenGame()
	{
		_lastSeenGameId = null;
	}

	public static void CheckLoadedGame()
	{
		try
		{
			string text = TimelineManager.CurrentSaveId();
			if (IsDifferentPlaythrough(_lastSeenGameId, text))
			{
				string.IsNullOrEmpty(_lastSeenGameId);
				_lastSeenGameId = text;
				ResetAll();
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error resetting per-playthrough state: " + ex);
		}
	}

	public static void ResetAll()
	{
		ConstellationChatManager.ResetPerPlaythroughState();
		ConstellationCombatReactions.ResetPerPlaythroughState();
		ConstellationEventHistory.Clear();
		SubclassRespecManager.ResetPerPlaythroughState();
		KingmakerCrossSaveManager.ResetSelectedRuler();
	}
}
