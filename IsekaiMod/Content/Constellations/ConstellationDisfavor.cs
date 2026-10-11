using System;
using System.Collections.Generic;
using System.Globalization;
using IsekaiMod.Utilities;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Constellations;

internal static class ConstellationDisfavor
{
	internal enum OuterCourtDeed
	{
		Appetite,
		LoopDefiance,
		DeathUndone
	}

	internal const string StorageName = "IsekaiMod.Disfavor";

	internal const int OffenceStep = 15;

	internal const int PatronOffenceMultiplier = 2;

	internal const int AmendsStep = 15;

	internal const int DisfavorThreshold = 60;

	internal const int DisfavorCeiling = 90;

	internal const int DisfavorPayoutPercent = 50;

	internal static readonly IReadOnlyDictionary<string, AlignmentMaskType> SeatAlignment = new Dictionary<string, AlignmentMaskType>(StringComparer.Ordinal)
	{
		{
			"The Lucky Drunk",
			AlignmentMaskType.ChaoticGood
		},
		{
			"The Inheritor",
			AlignmentMaskType.LawfulGood
		},
		{
			"The Prince of Darkness",
			AlignmentMaskType.LawfulEvil
		},
		{
			"The Song of the Spheres",
			AlignmentMaskType.ChaoticGood
		},
		{
			"The Lady of Graves",
			AlignmentMaskType.TrueNeutral
		},
		{
			"The Savored Sting",
			AlignmentMaskType.ChaoticNeutral
		},
		{
			"The All-Seeing Eye",
			AlignmentMaskType.TrueNeutral
		},
		{
			"Our Lord in Iron",
			AlignmentMaskType.ChaoticNeutral
		},
		{
			"The Pirate Queen",
			AlignmentMaskType.ChaoticNeutral
		},
		{
			"The Laughing King",
			AlignmentMaskType.ChaoticNeutral
		},
		{
			"The Mischievous Friend",
			AlignmentMaskType.NeutralGood
		},
		{
			"The Everbloom",
			AlignmentMaskType.ChaoticGood
		},
		{
			"The Silence Between the Stars",
			AlignmentMaskType.ChaoticNeutral
		},
		{
			"The Key and the Gate",
			AlignmentMaskType.TrueNeutral
		},
		{
			"The Pallid Princess",
			AlignmentMaskType.TrueNeutral
		}
	};

	internal const string OuterCourtKey = "The Key and the Gate";

	internal const string OuterCourtPallid = "The Pallid Princess";

	internal static readonly IReadOnlyList<string> OuterCourtSeats = new string[2] { "The Key and the Gate", "The Pallid Princess" };

	internal const int InnerCourtTithePercent = 75;

	internal static bool IsOuterCourtSeat(string sponsor)
	{
		if (!(sponsor == "The Key and the Gate"))
		{
			return sponsor == "The Pallid Princess";
		}
		return true;
	}

	internal static int OuterCourtReaction(string sponsor, OuterCourtDeed deed)
	{
		if (!IsOuterCourtSeat(sponsor))
		{
			return 0;
		}
		switch (deed)
		{
		case OuterCourtDeed.Appetite:
			return 1;
		case OuterCourtDeed.LoopDefiance:
			if (!(sponsor == "The Key and the Gate"))
			{
				return 0;
			}
			return -1;
		case OuterCourtDeed.DeathUndone:
			if (!(sponsor == "The Pallid Princess"))
			{
				return 0;
			}
			return -1;
		default:
			return 0;
		}
	}

	internal static int ApplyInnerCourtTithe(int coins, bool titheApplies)
	{
		if (!titheApplies || coins <= 0)
		{
			return Math.Max(0, coins);
		}
		return Math.Max(1, coins * 75 / 100);
	}

	internal static AlignmentMaskType AlignmentOf(string sponsor)
	{
		if (string.IsNullOrEmpty(sponsor))
		{
			return AlignmentMaskType.TrueNeutral;
		}
		if (!SeatAlignment.TryGetValue(sponsor, out var value))
		{
			return AlignmentMaskType.TrueNeutral;
		}
		return value;
	}

	internal static bool IsSeat(string sponsor)
	{
		if (!string.IsNullOrEmpty(sponsor))
		{
			return SeatAlignment.ContainsKey(sponsor);
		}
		return false;
	}

	internal static AlignmentShiftDirection SeatSideOn(AlignmentMaskType seat, AlignmentShiftDirection axisMember)
	{
		switch (axisMember)
		{
		case AlignmentShiftDirection.Good:
		case AlignmentShiftDirection.Evil:
			if ((seat & AlignmentMaskType.Good) != AlignmentMaskType.None)
			{
				return AlignmentShiftDirection.Good;
			}
			if ((seat & AlignmentMaskType.Evil) != AlignmentMaskType.None)
			{
				return AlignmentShiftDirection.Evil;
			}
			return AlignmentShiftDirection.TrueNeutral;
		case AlignmentShiftDirection.Lawful:
		case AlignmentShiftDirection.Chaotic:
			if ((seat & AlignmentMaskType.Lawful) != AlignmentMaskType.None)
			{
				return AlignmentShiftDirection.Lawful;
			}
			if ((seat & AlignmentMaskType.Chaotic) != AlignmentMaskType.None)
			{
				return AlignmentShiftDirection.Chaotic;
			}
			return AlignmentShiftDirection.TrueNeutral;
		default:
			return AlignmentShiftDirection.TrueNeutral;
		}
	}

	internal static int Reaction(AlignmentMaskType seat, AlignmentShiftDirection shift)
	{
		if (shift == AlignmentShiftDirection.TrueNeutral)
		{
			return 0;
		}
		AlignmentShiftDirection alignmentShiftDirection = SeatSideOn(seat, shift);
		if (alignmentShiftDirection == AlignmentShiftDirection.TrueNeutral)
		{
			return 0;
		}
		if (alignmentShiftDirection != shift)
		{
			return -1;
		}
		return 1;
	}

	internal static int Apply(int current, int reaction, bool isPatron)
	{
		if (current < 0)
		{
			current = 0;
		}
		if (reaction == 0)
		{
			return Math.Min(current, 90);
		}
		int num = ((reaction < 0) ? (current + 15 * ((!isPatron) ? 1 : 2)) : (current - 15));
		if (num < 0)
		{
			num = 0;
		}
		if (num > 90)
		{
			num = 90;
		}
		return num;
	}

	internal static bool IsOffended(int disfavor)
	{
		return disfavor >= 60;
	}

	internal static int ReducedPayout(int coins)
	{
		if (coins <= 0)
		{
			return 0;
		}
		return Math.Max(1, coins * 50 / 100);
	}

	internal static int AdjustPayout(int coins, bool offended)
	{
		if (!offended)
		{
			return Math.Max(0, coins);
		}
		return ReducedPayout(coins);
	}

	internal static int Read(IDictionary<string, string> storage, string sponsor)
	{
		if (string.IsNullOrEmpty(sponsor))
		{
			return 0;
		}
		return Math.Max(0, SaveScopedStorage.GetInt(storage, sponsor));
	}

	internal static void Write(IDictionary<string, string> storage, string sponsor, int disfavor)
	{
		if (storage != null && !string.IsNullOrEmpty(sponsor))
		{
			if (disfavor <= 0)
			{
				storage.Remove(sponsor);
			}
			else
			{
				storage[sponsor] = disfavor.ToString(CultureInfo.InvariantCulture);
			}
		}
	}
}
