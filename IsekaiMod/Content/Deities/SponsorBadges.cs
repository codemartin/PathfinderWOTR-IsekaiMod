using System;
using System.Collections.Generic;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Localization;
using TabletopTweaks.Core.NewEvents;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Deities;

internal static class SponsorBadges
{
	private sealed class BadgedDeity
	{
		public BlueprintFeature Deity;

		public string CanonicalName;

		public LocalizedString Badge;

		public LocalizedString OriginalName;

		public LocalizedString OriginalDescription;

		public LocalizedString BadgedName;

		public LocalizedString BadgedDescription;
	}

	private class LocaleHandler : ILocaleChangedHandler, IGlobalSubscriber, ISubscriber
	{
		public void HandleLocaleChanged()
		{
			try
			{
				Refresh();
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in SponsorBadges.HandleLocaleChanged: " + ex);
			}
		}
	}

	private static readonly List<BadgedDeity> Badged = new List<BadgedDeity>();

	private static bool Subscribed;

	internal static void Apply(BlueprintFeature deity, string canonicalName, LocalizedString badge)
	{
		if (deity == null)
		{
			return;
		}
		foreach (BadgedDeity item in Badged)
		{
			if (item.Deity == deity)
			{
				return;
			}
		}
		BadgedDeity badgedDeity = new BadgedDeity
		{
			Deity = deity,
			CanonicalName = canonicalName,
			Badge = badge,
			OriginalName = ((BlueprintUnitFact)deity).m_DisplayName,
			OriginalDescription = ((BlueprintUnitFact)deity).m_Description
		};
		Badged.Add(badgedDeity);
		badgedDeity.BadgedName = MakeRichString(deity.name + ".SponsorName", BadgedNameText(badgedDeity));
		badgedDeity.BadgedDescription = MakeRichString(deity.name + ".SponsorDescription", BadgedDescriptionText(badgedDeity));
		((BlueprintUnitFact)deity).m_DisplayName = badgedDeity.BadgedName;
		((BlueprintUnitFact)deity).m_Description = badgedDeity.BadgedDescription;
		if (!Subscribed)
		{
			Subscribed = true;
			EventBus.Subscribe(new LocaleHandler());
		}
	}

	private static string BadgedNameText(BadgedDeity rec)
	{
		return "★ " + TextInCurrentLocale(rec.OriginalName, rec.CanonicalName);
	}

	private static string BadgedDescriptionText(BadgedDeity rec)
	{
		return TextInCurrentLocale(rec.Badge, "") + TextInCurrentLocale(rec.OriginalDescription, "");
	}

	internal static LocalizedString BadgeParagraph(string simpleName, string text)
	{
		return MakeRichString(simpleName, text);
	}

	private static LocalizedString MakeRichString(string simpleName, string text)
	{
		return Helpers.CreateString(Main.IsekaiContext, simpleName, text, shouldProcess: false, stripTags: false);
	}

	private static string TextInCurrentLocale(LocalizedString localized, string fallback)
	{
		if (localized == null)
		{
			return fallback;
		}
		string key = localized.Key;
		if (string.IsNullOrEmpty(key))
		{
			return fallback;
		}
		MultiLocalizationPack.MultiLocaleString multiLocaleString = FindModString(key);
		if (multiLocaleString != null)
		{
			string text = TextFor(multiLocaleString, LocalizationManager.CurrentLocale);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		LocalizationPack currentPack = LocalizationManager.CurrentPack;
		if (currentPack != null)
		{
			string text2 = currentPack.GetText(key, reportUnknown: false);
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
		}
		return fallback;
	}

	private static MultiLocalizationPack.MultiLocaleString FindModString(string key)
	{
		try
		{
			MultiLocalizationPack multiLocalizationPack = Main.IsekaiContext?.ModLocalizationPack;
			if (multiLocalizationPack == null)
			{
				return null;
			}
			MultiLocalizationPack.MultiLocaleString value;
			return multiLocalizationPack.Ids.TryGetValue(key, out value) ? value : null;
		}
		catch
		{
			return null;
		}
	}

	internal static string TextFor(MultiLocalizationPack.MultiLocaleString entry, Locale locale)
	{
		if (entry == null)
		{
			return "";
		}
		string text = locale switch
		{
			Locale.ruRU => entry.ruRU, 
			Locale.deDE => entry.deDE, 
			Locale.frFR => entry.frFR, 
			Locale.zhCN => entry.zhCN, 
			Locale.esES => entry.esES, 
			_ => entry.enGB, 
		};
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return entry.enGB ?? "";
	}

	internal static void Refresh()
	{
		foreach (BadgedDeity item in Badged)
		{
			PushText(item.BadgedName, BadgedNameText(item));
			PushText(item.BadgedDescription, BadgedDescriptionText(item));
		}
	}

	private static void PushText(LocalizedString localized, string text)
	{
		if (localized != null)
		{
			string key = localized.Key;
			if (!string.IsNullOrEmpty(key))
			{
				FindModString(key)?.SetText(LocalizationManager.CurrentLocale, text);
				LocalizationManager.CurrentPack?.PutString(key, text);
			}
		}
	}
}
