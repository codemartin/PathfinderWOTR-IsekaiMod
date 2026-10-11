using System;
using System.Collections.Generic;
using System.Globalization;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.KingmakerIntegration;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Kingdom;
using Kingmaker.PubSubSystem;

namespace IsekaiMod.Content.Crusade;

[TypeId("e78b189ce1cc4442bd8f82ee4afbae9b")]
public class ActionCompleteKingdomProjectRewards : IsekaiGameAction
{
	public int Finances;

	public int Materials;

	public int CosmicCoins;

	public int PersonalGold;

	public string LogMessageText;

	public bool CompletesSylvanAlliance;

	public bool CompletesImperialDais;

	internal const string StorageName = "IsekaiMod.KingdomDecrees";

	public string RunLedgerKey;

	public int MaxRuns;

	internal const string RulerToken = "{ruler}";

	internal const string TreatyRatifiedLogTemplate = "<color=#004604><b>[Treaty Ratified]</b></color> The Stolen Lands under <b>{ruler}</b> have allied with the Fifth Crusade!";

	private static string LocalizedLogText(string english)
	{
		if (!(english == "<color=#004604><b>[Treaty Ratified]</b></color> The Stolen Lands under <b>{ruler}</b> have allied with the Fifth Crusade!"))
		{
			return ConstellationText.LocalizeIfKnown(english);
		}
		return ConstellationText.Localized("ProjectTreatyStolenLands.RatifiedLog", "<color=#004604><b>[Treaty Ratified]</b></color> The Stolen Lands under <b>{ruler}</b> have allied with the Fifth Crusade!");
	}

	private static string ResolveRulerToken(string text)
	{
		if (text == null || text.IndexOf("{ruler}", StringComparison.Ordinal) < 0)
		{
			return text;
		}
		string newValue;
		try
		{
			newValue = KingmakerCrossSaveManager.GetRulerName();
		}
		catch
		{
			newValue = "the Sovereign of the Stolen Lands";
		}
		return text.Replace("{ruler}", newValue);
	}

	public override string GetCaption()
	{
		return $"Complete Project Rewards: {Finances} Fin, {Materials} Mat, {CosmicCoins} Coins, {PersonalGold} Gold";
	}

	internal static bool RunDue(int alreadyRun, int maxRuns)
	{
		if (maxRuns > 0)
		{
			return alreadyRun < maxRuns;
		}
		return true;
	}

	internal static bool MayPay(IDictionary<string, string> ledger, string key, int maxRuns)
	{
		if (string.IsNullOrEmpty(key) || maxRuns <= 0)
		{
			return true;
		}
		if (ledger == null)
		{
			return false;
		}
		return RunDue(Math.Max(0, SaveScopedStorage.GetInt(ledger, key)), maxRuns);
	}

	private bool TryClaimRun()
	{
		if (string.IsNullOrEmpty(RunLedgerKey) || MaxRuns <= 0)
		{
			return true;
		}
		Dictionary<string, string> dictionary = SaveScopedStorage.Ensure(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.KingdomDecrees");
		if (!MayPay(dictionary, RunLedgerKey, MaxRuns))
		{
			return false;
		}
		int num = Math.Max(0, SaveScopedStorage.GetInt(dictionary, RunLedgerKey));
		dictionary[RunLedgerKey] = (num + 1).ToString(CultureInfo.InvariantCulture);
		return true;
	}

	protected override void RunSafely()
	{
		try
		{
			if (!TryClaimRun())
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(ConstellationText.Localized("Decree.Exhausted", "<color=#5B2A86><b>[Decree]</b></color> This decree has already run as many times as it can, and pays nothing more."));
				});
				return;
			}
			KingdomState instance = KingdomState.Instance;
			if (instance != null && (Finances > 0 || Materials > 0))
			{
				instance.GainResource(new KingdomResourcesAmount
				{
					m_Finances = Finances,
					m_Materials = Materials
				});
			}
			if (PersonalGold > 0)
			{
				Game.Instance?.Player?.GainMoney(PersonalGold);
			}
			if (CosmicCoins > 0)
			{
				DivineTokens.AddClaimedCoins(CosmicCoins, ConstellationText.Localized("CosmicCoins.Sponsor", "Crusade Decree / Treaty Ratification"));
			}
			if (!string.IsNullOrEmpty(LogMessageText))
			{
				string text = ResolveRulerToken(LocalizedLogText(LogMessageText));
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(text);
				});
			}
			if (CompletesSylvanAlliance)
			{
				SylvanFeyAllianceQuest.OnTreatyRatified();
			}
			if (CompletesImperialDais)
			{
				PersonalQuestDirector.OnDecreeRatified("ImperialDais");
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in ActionCompleteKingdomProjectRewards: " + ex);
		}
	}
}
