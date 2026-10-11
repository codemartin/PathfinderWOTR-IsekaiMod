using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Content.Arenas;

internal static class ColiseumCups
{
	internal const string FaeGroveId = "Act1.FaeGrove";

	internal const string LostLegionsId = "Act2.LostLegions";

	internal const string MidnightColosseumId = "Act3.MidnightColosseum";

	internal const string StolenLandsId = "Act3.StolenLands";

	internal const string VoidRiftId = "Act4.VoidRift";

	internal const string MultiverseThresholdId = "Act5.MultiverseThreshold";

	internal const string AstralId = "Act5.AstralCup";

	internal const string GenesisId = "Act5.GenesisCup";

	internal const string CosmicId = "Act5.CosmicChallenge";

	internal static readonly ColiseumCup[] All = new ColiseumCup[9]
	{
		new ColiseumCup
		{
			Id = "Act1.FaeGrove",
			Act = 1,
			Kind = ColiseumCupKind.Act,
			DisplayName = "Fae Grove Cup",
			Waves = ArenaType.Act1FaeGrove,
			MaxWaves = 3,
			Experience = 8000,
			Gold = 3000,
			Coins = 200
		},
		new ColiseumCup
		{
			Id = "Act2.LostLegions",
			Act = 2,
			Kind = ColiseumCupKind.Act,
			DisplayName = "Lost Legions Cup",
			Waves = ArenaType.Act2LostLegions,
			MaxWaves = 3,
			Experience = 20000,
			Gold = 8000,
			Coins = 400
		},
		new ColiseumCup
		{
			Id = "Act3.MidnightColosseum",
			Act = 3,
			Kind = ColiseumCupKind.Act,
			DisplayName = "Midnight Colosseum Cup",
			Waves = ArenaType.Act3MidnightColosseum,
			MaxWaves = 3,
			Experience = 45000,
			Gold = 15000,
			Coins = 750
		},
		new ColiseumCup
		{
			Id = "Act3.StolenLands",
			Act = 3,
			Kind = ColiseumCupKind.StolenLands,
			DisplayName = "Stolen Lands Legacy Cup",
			Waves = ArenaType.StolenLandsLegacyCup,
			MaxWaves = 3,
			Experience = 60000,
			Gold = 25000,
			Coins = 1500
		},
		new ColiseumCup
		{
			Id = "Act4.VoidRift",
			Act = 4,
			Kind = ColiseumCupKind.Act,
			DisplayName = "Void Rift Cup",
			Waves = ArenaType.Act4VoidRift,
			MaxWaves = 3,
			Experience = 90000,
			Gold = 30000,
			Coins = 1200
		},
		new ColiseumCup
		{
			Id = "Act5.MultiverseThreshold",
			Act = 5,
			Kind = ColiseumCupKind.Act,
			DisplayName = "Multiverse Threshold Cup",
			Waves = ArenaType.Act5MultiverseThreshold,
			MaxWaves = 4,
			Experience = 150000,
			Gold = 50000,
			Coins = 2000
		},
		new ColiseumCup
		{
			Id = "Act5.AstralCup",
			Act = 5,
			Kind = ColiseumCupKind.Pinnacle,
			DisplayName = "Astral Cup",
			Waves = ArenaType.AstralCup,
			MaxWaves = 4,
			Experience = 250000,
			Gold = 80000,
			Coins = 3000
		},
		new ColiseumCup
		{
			Id = "Act5.GenesisCup",
			Act = 5,
			Kind = ColiseumCupKind.Pinnacle,
			DisplayName = "Genesis Cup",
			Waves = ArenaType.GenesisCup,
			MaxWaves = 4,
			Experience = 400000,
			Gold = 120000,
			Coins = 5000
		},
		new ColiseumCup
		{
			Id = "Act5.CosmicChallenge",
			Act = 5,
			Kind = ColiseumCupKind.Pinnacle,
			DisplayName = "The Cosmic Challenge",
			Waves = ArenaType.TheCosmicChallenge,
			MaxWaves = 5,
			Experience = 1000000,
			Gold = 250000,
			Coins = 10000
		}
	};

	internal static readonly ColiseumCup[] Trials = new ColiseumCup[7]
	{
		new ColiseumCup
		{
			Id = "Trial.PhantomBlade.Act2",
			Act = 2,
			Kind = ColiseumCupKind.PersonalTrial,
			DisplayName = "Trial of the Phantom Sword-Saint",
			Waves = ArenaType.TrialPhantomSwordSaint,
			MaxWaves = 1
		},
		new ColiseumCup
		{
			Id = "Trial.PhantomBlade.Act3",
			Act = 3,
			Kind = ColiseumCupKind.PersonalTrial,
			DisplayName = "Trial of the Blade Spirit",
			Waves = ArenaType.TrialBladeSpirit,
			MaxWaves = 1
		},
		new ColiseumCup
		{
			Id = "Trial.Overlord.Act2",
			Act = 2,
			Kind = ColiseumCupKind.PersonalTrial,
			DisplayName = "Trial of the Bone Court",
			Waves = ArenaType.TrialBoneCourt,
			MaxWaves = 1
		},
		new ColiseumCup
		{
			Id = "Trial.Devourer.Act1",
			Act = 1,
			Kind = ColiseumCupKind.PersonalTrial,
			DisplayName = "Trial of the Corrosive Wyrm",
			Waves = ArenaType.TrialCorrosiveWyrm,
			MaxWaves = 1
		},
		new ColiseumCup
		{
			Id = "Trial.Devourer.Act3",
			Act = 3,
			Kind = ColiseumCupKind.PersonalTrial,
			DisplayName = "Trial of the Primordial Behemoth",
			Waves = ArenaType.TrialPrimordialBehemoth,
			MaxWaves = 1
		},
		new ColiseumCup
		{
			Id = "Trial.ShadowMonarch.Act2",
			Act = 2,
			Kind = ColiseumCupKind.PersonalTrial,
			DisplayName = "Trial of the Fallen Vanguard",
			Waves = ArenaType.TrialFallenVanguard,
			MaxWaves = 1
		},
		new ColiseumCup
		{
			Id = "Trial.ShadowMonarch.Act3",
			Act = 3,
			Kind = ColiseumCupKind.PersonalTrial,
			DisplayName = "Trial of the Throne of Shadows",
			Waves = ArenaType.TrialThroneOfShadows,
			MaxWaves = 1
		}
	};

	internal static IEnumerable<ColiseumCup> ActCups => from c in All
		where c.Kind == ColiseumCupKind.Act
		orderby c.Act
		select c;

	internal static ColiseumCup ById(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		return All.FirstOrDefault((ColiseumCup c) => string.Equals(c.Id, id, StringComparison.Ordinal)) ?? Trials.FirstOrDefault((ColiseumCup c) => string.Equals(c.Id, id, StringComparison.Ordinal));
	}

	internal static ColiseumCup ForAct(int act)
	{
		return All.FirstOrDefault((ColiseumCup c) => c.Kind == ColiseumCupKind.Act && c.Act == act);
	}

	internal static bool IsOffered(ColiseumCup cup, int arenaAct, bool stolenLandsAvailable, bool multiverseThresholdCleared)
	{
		if (cup == null || arenaAct < 1 || arenaAct > 5)
		{
			return false;
		}
		switch (cup.Kind)
		{
		case ColiseumCupKind.Act:
			return cup.Act <= arenaAct;
		case ColiseumCupKind.StolenLands:
			if (stolenLandsAvailable)
			{
				return arenaAct >= cup.Act;
			}
			return false;
		case ColiseumCupKind.Pinnacle:
			return (arenaAct == 5) & multiverseThresholdCleared;
		case ColiseumCupKind.PersonalTrial:
			return false;
		default:
			return false;
		}
	}

	internal static List<string> OfferedInArena(int arenaAct, bool stolenLandsAvailable, bool multiverseThresholdCleared)
	{
		return (from c in All
			where IsOffered(c, arenaAct, stolenLandsAvailable, multiverseThresholdCleared)
			select c.Id).ToList();
	}
}
