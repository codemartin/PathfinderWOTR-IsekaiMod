using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;

namespace IsekaiMod.Content.Arenas;

internal static class LanternKingReveal
{
	internal const string RevealedKey = "LanternKing.Revealed";

	internal const string PrizeClaimedKey = "LanternKing.PrizeClaimed";

	internal const string DiscountAnnouncedKey = "LanternKing.DiscountAnnounced";

	internal const string CameoReactedKey = "LanternKing.CameoReacted";

	internal const int AvatarDiscountPercent = 50;

	internal const int PrizeCoins = 1500;

	internal const int PrizeGold = 25000;

	internal static int DiscountedAvatarCost => 5000;

	internal static bool IsRevealPending(IDictionary<string, string> storage)
	{
		if (storage == null || SaveScopedStorage.GetFlag(storage, "LanternKing.Revealed"))
		{
			return false;
		}
		if (!ColiseumProgress.IsCleared(storage, "Act3.MidnightColosseum"))
		{
			return ColiseumProgress.IsCleared(storage, "Act3.StolenLands");
		}
		return true;
	}

	internal static bool IsRevealed(IDictionary<string, string> storage)
	{
		if (storage != null)
		{
			return SaveScopedStorage.GetFlag(storage, "LanternKing.Revealed");
		}
		return false;
	}

	internal static bool IsPrizeAvailable(IDictionary<string, string> storage)
	{
		if (IsRevealed(storage))
		{
			return !SaveScopedStorage.GetFlag(storage, "LanternKing.PrizeClaimed");
		}
		return false;
	}

	internal static bool TryClaimPrize(IDictionary<string, string> storage)
	{
		if (!IsPrizeAvailable(storage))
		{
			return false;
		}
		SaveScopedStorage.SetFlag(storage, "LanternKing.PrizeClaimed", value: true);
		return true;
	}

	internal static bool PrizeCanBePaid(bool gold, bool constellationsEnabled)
	{
		return gold | constellationsEnabled;
	}

	internal static IEnumerable<ColiseumCup> CupsForAllCups(bool stolenLandsAvailable)
	{
		return ColiseumCups.All.Where((ColiseumCup c) => stolenLandsAvailable || c.Kind != ColiseumCupKind.StolenLands);
	}

	internal static bool AllCupsCleared(IDictionary<string, string> storage, bool stolenLandsAvailable)
	{
		if (storage == null)
		{
			return false;
		}
		return CupsForAllCups(stolenLandsAvailable).All((ColiseumCup c) => ColiseumProgress.IsCleared(storage, c.Id));
	}

	internal static bool IsDiscountAnnouncementPending(IDictionary<string, string> storage, bool stolenLandsAvailable)
	{
		if (IsRevealed(storage) && !SaveScopedStorage.GetFlag(storage, "LanternKing.DiscountAnnounced"))
		{
			return AllCupsCleared(storage, stolenLandsAvailable);
		}
		return false;
	}

	internal static int AvatarPrice(bool allCupsCleared)
	{
		if (!allCupsCleared)
		{
			return 10000;
		}
		return DiscountedAvatarCost;
	}

	internal static bool StolenLandsAvailableSafe()
	{
		try
		{
			return DimensionalArenaManager.StolenLandsCupAvailable();
		}
		catch
		{
			return false;
		}
	}

	internal static bool DiscountEarnedInSave()
	{
		return AllCupsCleared(ColiseumProgress.Find(), StolenLandsAvailableSafe());
	}

	internal static List<string> RevealReactions(bool kingmakerDetected, string rulerName)
	{
		List<string> list = new List<string>
		{
			ConstellationText.Localized("Constellation.LanternKing", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> takes a deep bow from the celestial table: \"Surprise! Did everybody enjoy the show? Yes, I went down there myself. Yes, in person. Yes, that is against the rules. That is what makes it FUN!\""),
			ConstellationText.Localized("Constellation.LanternKing", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> rises from her seat: \"This is interference! A seat at this table does not walk onto the contestant's stage. Sit down, Lantern King, or be removed.\""),
			ConstellationText.Localized("Constellation.LanternKing", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> makes a neat note in a ledger bound in something that is not leather: \"No sponsor shall manifest in the contestant's presence. Breach of contract, recorded and dated. I will collect. I always collect.\""),
			ConstellationText.Localized("Constellation.LanternKing", "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color> slams a gauntlet on the table: \"CHEATING! If the fey can walk onto the sand, so can I! Give me an axe and five minutes!\""),
			ConstellationText.Localized("Constellation.LanternKing", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color> chuckles behind a veil of runes: \"Fascinating. The trickster has stepped through the fourth wall of a cosmos that has no walls. Do not stop him yet. I am taking notes.\"")
		};
		if (kingmakerDetected && !string.IsNullOrEmpty(rulerName))
		{
			list.Add(ConstellationText.LocalizedFormat("Constellation.LanternKing", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> adds, a little too sweetly: \"And do give my regards to {0}. We have unfinished business in the Stolen Lands, that ruler and I.\"", rulerName));
		}
		list.Add(ConstellationText.Localized("Constellation.LanternKing", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> sighs: \"The Stolen Lands have barely begun to heal from his last game. Take his prize if you must, little star, but never take his word.\""));
		return list;
	}

	internal static List<string> CameoReactions()
	{
		return new List<string>
		{
			ConstellationText.Localized("Constellation.LanternKing", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> squints at the sands: \"Hang on. Is that the Laughing King down THERE? Is that allowed? Somebody check the rules. After this drink.\""),
			ConstellationText.Localized("Constellation.LanternKing", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> frowns: \"A constellation on the contestant's stage. That is not applause, that is interference.\""),
			ConstellationText.Localized("Constellation.LanternKing", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> waves up at the sky: \"Relax, relax! I am just a fan in a very good costume!\"")
		};
	}

	internal static List<string> DiscountReactions()
	{
		return new List<string>
		{
			ConstellationText.Localized("Constellation.LanternKing", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> twirls a quill of candle flame: \"Every cup! EVERY cup! Half price on my Avatar, and I dare anyone at this table to match the offer!\""),
			ConstellationText.Localized("Constellation.LanternKing", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> does not look up from his ledger: \"A sponsor discounting his own Avatar to a contestant he has already met in person. Two breaches. The interest is compounding.\"")
		};
	}

	internal static void PostVoices(IEnumerable<string> lines)
	{
		foreach (string line in lines)
		{
			ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, line, "The Laughing King", ConstellationCategory.Combat, "Planar Coliseum");
		}
	}

	internal static void OnCameoAppeared()
	{
		Dictionary<string, string> dictionary = ColiseumProgress.Ensure();
		if (dictionary != null && !SaveScopedStorage.GetFlag(dictionary, "LanternKing.CameoReacted"))
		{
			SaveScopedStorage.SetFlag(dictionary, "LanternKing.CameoReacted", value: true);
			PostVoices(CameoReactions());
		}
	}
}
