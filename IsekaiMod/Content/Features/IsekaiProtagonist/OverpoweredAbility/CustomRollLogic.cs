using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

[TypeId("25d702ec830848f296fa5f7c98357d1f")]
public class CustomRollLogic : IsekaiContextAction
{
	private static readonly Dictionary<string, string> SeatFanfare = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "The Pirate Queen", "<color=#5B2A86>Besmara:</color> \"AHAHAHA! Look at that glorious haul! You took the interdimensional casino for everything it's worth! Here's a pirate bounty to celebrate!\"" },
		{ "The Mischievous Friend", "<color=#5B2A86>Chaldira:</color> \"Did you see that?! Nobody is that lucky by accident! Keep pulling, keep winning, and let me hold the winnings for a moment!\"" },
		{ "The Laughing King", "<color=#5B2A86>The Lantern King:</color> \"Ohoho! Probability shattered into glittering stardust! Cheating the cosmic house is an absolute masterpiece of comedy!\"" },
		{ "The All-Seeing Eye", "<color=#133E71>Nethys:</color> \"The odds and their ruin are equally beautiful. I have recorded this branch of probability exactly as it collapsed.\"" },
		{ "Our Lord in Iron", "<color=#7A1010>Gorum:</color> \"A MOUNTAIN OF GOLD AND SACRED WEAPONS! FORGE THEM INTO TOOLS OF SLAUGHTER AND CRUSH THE ABYSSAL HORDES!\"" },
		{ "The Song of the Spheres", "<color=#133E71>Desna:</color> \"The Song of the Spheres echoes softly for you, brave dreamer. May lucky starlight illuminate your path through the darkness.\"" },
		{ "The Lucky Drunk", "<color=#5B2A86>Cayden Cailean:</color> \"Now THAT is a haul worth drinking to! First round is on you, and the second, and probably the third!\"" },
		{ "The Inheritor", "<color=#5B2A86>Iomedae:</color> \"Fortune has favored you. See that it is spent on the crusade and not on yourself, and it will have been earned.\"" },
		{ "The Prince of Darkness", "<color=#7A1010>Asmodeus:</color> \"Wealth without a contract behind it is merely luck. Still: an impressive sum. We shall discuss the terms later.\"" },
		{ "The Lady of Graves", "<color=#5B2A86>Pharasma:</color> \"Gold is a loan the living take from the dead. Use it well; the ledger closes either way.\"" },
		{ "The Savored Sting", "<color=#5B2A86>Calistria:</color> \"Extravagant, indulgent and entirely undeserved. I approve of all three. Spend it where it will be noticed.\"" },
		{ "The Everbloom", "<color=#004604>Milani:</color> \"Put it in the hands of those with nothing, and this fortune becomes a rebellion instead of a hoard.\"" },
		{ "The Silence Between the Stars", "<color=#133E71>Black Butterfly:</color> \"A silence, and then a fortune. The interval between the two is the only part worth remembering.\"" },
		{ "The Key and the Gate", "<color=#5B2A86>Yog-Sothoth:</color> \"This pull has resolved identically in four hundred and six prior iterations. You are not lucky. You are scheduled. Proceed.\"" },
		{ "The Pallid Princess", "<color=#7A1010>Urgathoa:</color> \"Spend it on wine, on meat, on whatever rots fastest. Hoarded wealth is a corpse nobody had the decency to eat.\"" }
	};

	internal const string FanfareLineName = "MerchantsGamble.Fanfare";

	public BlueprintItemReference GoldCoins { get; set; }

	public BlueprintFeatureReference OmnipotentDie { get; set; }

	public override string GetCaption()
	{
		return "Performs an Otherworldly Gacha pull with pity and tiered rewards.";
	}

	protected override void RunSafely()
	{
		UnitEntityData unitEntityData = base.Context?.MaybeCaster ?? Game.Instance?.Player?.MainCharacter.Value;
		if (unitEntityData == null)
		{
			return;
		}
		ItemsCollection itemsCollection = Game.Instance?.Player?.Inventory;
		int characterLevel = unitEntityData.Descriptor.Progression.CharacterLevel;
		Dictionary<string, string> storage = GachaPity.Ensure();
		int num = GachaPity.Read(storage, "Pity") + 1;
		double num2 = UnityEngine.Random.Range(0f, 100f);
		bool flag = GachaPity.PityDue(num);
		bool flag2 = flag || num2 < 0.2;
		bool flag3 = !flag2 && num2 < 3.2;
		bool flag4 = !flag2 && !flag3 && num2 < 18.2;
		GachaPity.Write(storage, "Pity", GachaPity.Next(num - 1, flag2));
		if (flag2)
		{
			int num3 = GachaPity.ScaleGold(100000, characterLevel);
			Game.Instance?.Player?.GainMoney(num3);
			DivineTokens.AddCoins(5000, ConstellationText.Localized("CosmicCoins.Sponsor", "Transcendent Gacha Core"));
			BlueprintAbilityResource modBlueprint = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.IsekaiContext, "CosmicWishResource");
			BlueprintAbility modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "ModularCosmicWishAbility");
			if (modBlueprint2 != null && !unitEntityData.Descriptor.HasFact(modBlueprint2))
			{
				unitEntityData.Descriptor.AddFact(modBlueprint2);
			}
			if (modBlueprint != null)
			{
				if (unitEntityData.Descriptor.Resources.GetResource((BlueprintScriptableObject)modBlueprint) == null)
				{
					unitEntityData.Descriptor.Resources.Add(modBlueprint, restoreAmount: true);
				}
				unitEntityData.Descriptor.Resources.Restore(modBlueprint, 5);
			}
			BlueprintFeature blueprintFeature = OmnipotentDie?.Get();
			if (blueprintFeature != null)
			{
				if (!unitEntityData.Descriptor.Progression.Features.HasFact(blueprintFeature))
				{
					unitEntityData.Descriptor.Progression.Features.AddFeature(blueprintFeature);
				}
				else
				{
					int num4 = GachaPity.Read(storage, "DuplicateJackpots");
					int num5 = GachaPity.DuplicateBonus(num4);
					int num6 = DuplicateJackpotsAfter(num4, num5, ConstellationSettings.Enabled);
					if (num6 != num4)
					{
						GachaPity.Write(storage, "DuplicateJackpots", num6);
						DivineTokens.AddCoins(num5, ConstellationText.Localized("CosmicCoins.Sponsor", "Omnipotent Sovereign's Die (Duplicate Bonus)"));
					}
				}
			}
			LogToCombatLog(flag ? ConstellationText.LocalizedFormat("Constellation.MerchantsGamble.Pull", "<color=#5B2A86><b>[6★ TRANSCENDENT UR JACKPOT!] [100-PULL PITY ACTIVATED!]</b></color> You pulled <b>The Omnipotent Sovereign's Die</b>, <b>{0} Gold</b>, {1}and <b>5 Cosmic Wish Charges</b>!", FormatGold(num3), CoinPart(5000)) : ConstellationText.LocalizedFormat("Constellation.MerchantsGamble.Pull", "<color=#5B2A86><b>[6★ TRANSCENDENT UR JACKPOT!]</b></color> You pulled <b>The Omnipotent Sovereign's Die</b>, <b>{0} Gold</b>, {1}and <b>5 Cosmic Wish Charges</b>!", FormatGold(num3), CoinPart(5000)));
			TriggerSupporterReactivity(6);
		}
		else if (flag3)
		{
			int num7 = GachaPity.ScaleGold(UnityEngine.Random.Range(100000, 250001), characterLevel);
			Game.Instance?.Player?.GainMoney(num7);
			int num8 = UnityEngine.Random.Range(500, 1001);
			DivineTokens.AddCoins(num8, ConstellationText.Localized("CosmicCoins.Sponsor", "SSR Gacha Blessing"));
			string[] array = new string[3] { "2e63277fbcdb469384efc97d910a12c4", "039fcbf4887047e8977102c23dd8b56b", "81d504243708f504dbfe3f8f72efdeda" };
			BlueprintItem blueprint = BlueprintTools.GetBlueprint<BlueprintItem>(array[UnityEngine.Random.Range(0, array.Length)]);
			if (blueprint != null)
			{
				itemsCollection?.Add(blueprint, 1);
			}
			LogToCombatLog(ConstellationText.LocalizedFormat("Constellation.MerchantsGamble.Pull", "<color=#5B2A86><b>[5★ SSR PULL!]</b></color> Rainbow light floods the dimensional rift! You received <b>{0} Gold</b>, {1}and a legendary relic (Pity: {2}/100)!", FormatGold(num7), CoinPart(num8), num));
			TriggerSupporterReactivity(5);
		}
		else if (flag4)
		{
			int num9 = GachaPity.ScaleGold(UnityEngine.Random.Range(15000, 35001), characterLevel);
			Game.Instance?.Player?.GainMoney(num9);
			int num10 = UnityEngine.Random.Range(50, 151);
			DivineTokens.AddCoins(num10, ConstellationText.Localized("CosmicCoins.Sponsor", "SR Gacha Cache"));
			string[] array2 = new string[3]
			{
				"1bf7ae3382d3472e956f691da66598cf",
				IsekaiDlcManager.HasDlc6() ? "a35044c76afa45c69f4baedf13ad5ac9" : "9a511d3b04f08944eb3db4462f88c2c0",
				"92752bbbf04dfa1439af186f48aee0e9"
			};
			int num11 = UnityEngine.Random.Range(0, array2.Length);
			BlueprintItem blueprint2 = BlueprintTools.GetBlueprint<BlueprintItem>(array2[num11]);
			if (blueprint2 != null && itemsCollection != null)
			{
				int count = ((num11 != 2) ? 1 : 50);
				itemsCollection.Add(blueprint2, count);
			}
			LogToCombatLog(ConstellationText.LocalizedFormat("Constellation.MerchantsGamble.Pull", "<color=#5B2A86><b>[4★ SR PULL]</b></color> A golden beam illuminates your cache! You obtained <b>{0} Gold</b>, {1}and tactical armaments (Pity: {2}/100).", FormatGold(num9), CoinPart(num10), num));
			TriggerSupporterReactivity(4);
		}
		else
		{
			int num12 = GachaPity.ScaleGold(UnityEngine.Random.Range(1500, 4001), characterLevel);
			Game.Instance?.Player?.GainMoney(num12);
			string[] array3 = new string[3] { "92752bbbf04dfa1439af186f48aee0e9", "fb822a8d451d91b438514541a9a986de", "d441dfae9c6b21e47ae24eb13d8b4c4b" };
			int num13 = UnityEngine.Random.Range(0, array3.Length);
			BlueprintItem blueprint3 = BlueprintTools.GetBlueprint<BlueprintItem>(array3[num13]);
			if (blueprint3 != null && itemsCollection != null)
			{
				int count2 = ((num13 != 0) ? 1 : 15);
				itemsCollection.Add(blueprint3, count2);
			}
			LogToCombatLog(ConstellationText.LocalizedFormat("Constellation.MerchantsGamble.Pull", "<color=#133E71><b>[3★ Common Pull]</b></color> You pulled <b>{0} Gold</b> and essential expedition supplies (Pity: {1}/100).", FormatGold(num12), num));
		}
	}

	private static string FormatGold(int gold)
	{
		return gold.ToString("N0", CultureInfo.InvariantCulture);
	}

	private void TriggerSupporterReactivity(int tier)
	{
		string text = ConstellationChatManager.SponsorForPlayerDeity();
		int amount = tier switch
		{
			5 => 250, 
			6 => 1000, 
			_ => 25, 
		};
		if (text != null && SeatFanfare.TryGetValue(text, out var value))
		{
			ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, ConstellationText.LocalizedFormat("Constellation.MerchantsGamble.FanfareFrame", "<color=#5B2A86><b>[Patron Fanfare]</b></color> {0}", ConstellationText.Localized("MerchantsGamble.Fanfare", value)), text, ConstellationCategory.DeityPatron, "Merchant's Gamble");
			DivineTokens.AddCoins(amount, text);
		}
		else
		{
			ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, ConstellationText.Localized("MerchantsGamble.FanfareObservant", "<color=#5B2A86><b>[Patron Fanfare]</b></color> <color=#5B2A86>The Observant Deities:</color> \"The cosmic constellation gazes in awe upon your spectacular fortune! Additional heavenly favor descends upon you.\""), null, ConstellationCategory.DeityPatron, "Merchant's Gamble");
			DivineTokens.AddCoins(amount, ConstellationText.Localized("CosmicCoins.Sponsor", "Observant Deities Favor"));
		}
	}

	private static string CoinPart(int coins)
	{
		if (!AnnouncesCoins(coins, ConstellationSettings.Enabled))
		{
			return "";
		}
		return ConstellationText.LocalizedFormat("Constellation.MerchantsGamble.CoinPart", "<b>{0} Cosmic Coins</b>, ", FormatGold(coins));
	}

	internal static bool AnnouncesCoins(int coins, bool constellationsEnabled)
	{
		return (coins > 0) & constellationsEnabled;
	}

	internal static int DuplicateJackpotsAfter(int alreadyPaid, int bonus, bool constellationsEnabled)
	{
		if (!((bonus > 0) & constellationsEnabled))
		{
			return alreadyPaid;
		}
		return alreadyPaid + 1;
	}

	private static Color LogTextColor()
	{
		try
		{
			LogColors logColors = Game.Instance?.BlueprintRoot?.UIRoot?.LogColors;
			if (logColors != null)
			{
				return logColors.WarningLogColor;
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error reading the game's log colours: " + ex);
		}
		if (!ColorUtility.TryParseHtmlString("#3F3F3F", out var color))
		{
			return Color.black;
		}
		return color;
	}

	private void LogToCombatLog(string message)
	{
		MessageLogThread messageLogThread = LogThreadService.Instance.GetThreadsByChannelType(default(LogChannelType)).OfType<MessageLogThread>().FirstOrDefault();
		if (messageLogThread != null)
		{
			CombatLogMessage newMessage = new CombatLogMessage(message, LogTextColor(), PrefixIcon.None, new TooltipTemplateSimple(null, message));
			((LogThreadBase)messageLogThread).AddMessage(newMessage);
		}
		else
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(message);
			});
		}
	}
}
