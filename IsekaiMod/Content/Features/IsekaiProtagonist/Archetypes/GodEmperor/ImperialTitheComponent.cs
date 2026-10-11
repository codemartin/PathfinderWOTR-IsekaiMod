using System;
using System.Collections.Generic;
using System.Globalization;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Rest;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Kingdom;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;

[TypeId("5301a1aed0ca49698ae8a8bd7c2c534c")]
public class ImperialTitheComponent : UnitFactComponentDelegate, IRestFinishedHandler, ISubscriber, IGlobalSubscriber, IKingdomDayHandler
{
	internal const string StorageName = "IsekaiMod.ImperialTithe";

	internal const string LastPaidDayKey = "LastPaidDay";

	public void HandleRestFinished(RestStatus status)
	{
		if (status != null && status.RestSucceeded)
		{
			CollectTithe();
		}
	}

	public void OnNewDay()
	{
	}

	internal static int CalendarDay(DateTime startDate, TimeSpan gameTime)
	{
		if (gameTime < TimeSpan.Zero)
		{
			gameTime = TimeSpan.Zero;
		}
		return (int)((startDate + gameTime).Date - startDate.Date).TotalDays;
	}

	internal static bool TryClaimTitheDay(IDictionary<string, string> storage, int day)
	{
		if (storage == null)
		{
			return true;
		}
		if (int.TryParse(SaveScopedStorage.Get(storage, "LastPaidDay"), out var result) && day <= result)
		{
			return false;
		}
		storage["LastPaidDay"] = day.ToString(CultureInfo.InvariantCulture);
		return true;
	}

	internal static int? LegacySeedDay(IList<string> legacyLedger, string gameId, int today)
	{
		if (legacyLedger == null || string.IsNullOrEmpty(gameId))
		{
			return null;
		}
		string text = gameId + "|";
		foreach (string item in legacyLedger)
		{
			if (item != null && item.StartsWith(text, StringComparison.Ordinal))
			{
				if (int.TryParse(item.Substring(text.Length), out var result) && result == today)
				{
					return today;
				}
				return null;
			}
		}
		return null;
	}

	private static IDictionary<string, string> Ledger(int today)
	{
		EntityDataBase entity = SaveScopedStorage.CurrentPlayer();
		Dictionary<string, string> dictionary = SaveScopedStorage.Find(entity, "IsekaiMod.ImperialTithe");
		if (dictionary != null)
		{
			return dictionary;
		}
		dictionary = SaveScopedStorage.Ensure(entity, "IsekaiMod.ImperialTithe");
		int? num = LegacySeedDay(TimelineManager.Data?.ImperialTitheDays, TimelineManager.CurrentSaveId(), today);
		if (dictionary != null && num.HasValue)
		{
			dictionary["LastPaidDay"] = num.Value.ToString(CultureInfo.InvariantCulture);
		}
		return dictionary;
	}

	private static int CurrentCalendarDay()
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

	internal static int[] TitheFor(int level)
	{
		if (level < 20)
		{
			if (level < 15)
			{
				if (level < 10)
				{
					if (level < 5)
					{
						return new int[4] { 2500, 10, 0, 0 };
					}
					return new int[4] { 10000, 50, 10, 5 };
				}
				return new int[4] { 25000, 150, 50, 20 };
			}
			return new int[4] { 50000, 300, 100, 50 };
		}
		return new int[4] { 100000, 1000, 200, 100 };
	}

	private void CollectTithe()
	{
		UnitEntityData owner = base.Owner;
		if (owner == null || !owner.IsMainCharacter)
		{
			return;
		}
		int num = CurrentCalendarDay();
		if (!TryClaimTitheDay(Ledger(num), num))
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(ConstellationText.Localized("ImperialTithe.AlreadyCollected", "<color=#5B2A86>[Imperial Tithe]</color> Today's tribute has already been collected."));
			});
			return;
		}
		int num2 = owner.Progression.GetClassLevel(IsekaiProtagonistClass.Get());
		if (num2 <= 0)
		{
			num2 = owner.Progression.CharacterLevel;
		}
		int[] array = TitheFor(num2);
		int gold = array[0];
		int finance = array[1];
		int materials = array[2];
		int favors = array[3];
		Game.Instance.Player.GainMoney(gold);
		try
		{
			Game.Instance.Player.Kingdom?.GainResource(KingdomResourcesAmount.Create(finance, materials, favors));
		}
		catch
		{
		}
		EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
		{
			h.HandleLogMessage(ConstellationText.LocalizedFormat("Constellation.ImperialTithe.Collected", "<color=#5B2A86>[Imperial Tithe]</color> Mortals pay devotional tribute to the living God-Emperor! Received {0} gold and {1} Crusade finances.", gold.ToString("N0", CultureInfo.InvariantCulture), finance.ToString("N0", CultureInfo.InvariantCulture)));
		});
	}
}
