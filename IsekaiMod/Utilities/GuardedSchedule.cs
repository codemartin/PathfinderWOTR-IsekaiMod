using System;
using Kingmaker;

namespace IsekaiMod.Utilities;

internal static class GuardedSchedule
{
	internal static Action Wrap(string what, Action body, Action<string> report = null)
	{
		return delegate
		{
			if (body == null)
			{
				return;
			}
			try
			{
				body();
			}
			catch (Exception ex)
			{
				try
				{
					(report ?? new Action<string>(Main.LogError))(FailureLine(what, ex));
				}
				catch
				{
				}
			}
		};
	}

	internal static void Next(string what, Action body)
	{
		Game instance = Game.Instance;
		if (instance != null && body != null)
		{
			instance.ScheduleAction(Wrap(what, body));
		}
	}

	internal static string FailureLine(string what, Exception ex)
	{
		return "[Scheduled] " + (string.IsNullOrEmpty(what) ? "A scheduled action" : what) + " failed and was skipped; the game carries on: " + ex;
	}
}
