using System;
using Kingmaker;
using Kingmaker.Blueprints.Root;

namespace IsekaiMod.Utilities;

internal static class GameCalendar
{
	internal static int CalendarDay(DateTime startDate, TimeSpan gameTime)
	{
		if (gameTime < TimeSpan.Zero)
		{
			gameTime = TimeSpan.Zero;
		}
		return (int)((startDate + gameTime).Date - startDate.Date).TotalDays;
	}

	internal static int CurrentDay()
	{
		try
		{
			TimeSpan gameTime = Game.Instance.TimeController.GameTime;
			try
			{
				return CalendarDay(BlueprintRoot.Instance.Calendar.GetStartDate(), gameTime);
			}
			catch
			{
				return (int)gameTime.TotalDays;
			}
		}
		catch
		{
			return 0;
		}
	}
}
