using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;

internal static class PrestigeLevelPlan
{
	internal sealed class EpicTrack
	{
		internal string Mastery;

		internal string Third;

		internal string Fifth;

		internal string Seventh;

		internal string Signature;

		internal string Capstone;
	}

	internal const int PrestigeLevels = 10;

	internal const string BonusFeat = "IsekaiBonusFeatSelection";

	internal const string SpecialPower = "SpecialPowerSelection";

	internal const string OverpoweredAbility = "OverpoweredAbilitySelection";

	internal const string IsekaiSpellbookSelection = "EpicPrestigeSpellbookSelection";

	internal const string RetinueSpellbookSelection = "RetinueSpellbookSelection";

	internal static readonly string[] GenericPicks = new string[3] { "IsekaiBonusFeatSelection", "SpecialPowerSelection", "OverpoweredAbilitySelection" };

	internal static readonly string[] ClassNames = new string[8] { "TranscendentSovereignClass", "DemigodPrestigeClass", "GodhunterPrestigeClass", "WarmasterPrestigeClass", "EternalSeekerPrestigeClass", "ArchmagePrestigeClass", "ShadowSovereignPrestigeClass", "OtherworldRetinueClass" };

	internal static readonly string[] EpicClassNames = new string[6] { "DemigodPrestigeClass", "GodhunterPrestigeClass", "WarmasterPrestigeClass", "EternalSeekerPrestigeClass", "ArchmagePrestigeClass", "ShadowSovereignPrestigeClass" };

	internal static readonly int[] EpicMasteryLevels = new int[5] { 2, 4, 6, 8, 9 };

	internal const string SovereignDecree = "SovereignDecreeFeature";

	internal const string SovereignCrown = "SovereignCrownFeature";

	internal static readonly int[] SovereignDecreeLevels = new int[4] { 2, 4, 6, 8 };

	internal static readonly string[][] SovereignArchetypes = new string[7][]
	{
		new string[4] { "CelestialMonarchArchetype", "GodEmperorArchetype", "HeavenlyImperialMandateFeature", "CelestialMonarchThroneFeature" },
		new string[4] { "DemonSlimeSovereignArchetype", "DevourerArchetype", "GluttonousSingularityFeature", "DemonSlimeAppetiteFeature" },
		new string[4] { "ShadowSovereignArchetype", "ShadowMonarchArchetype", "AbsoluteShadowDomainFeature", "ShadowSovereignNightLegionFeature" },
		new string[4] { "GrandStrategistArchetype", "MastermindArchetype", "AllAccordingToPlanFeature", "GrandStrategistCheckmateFeature" },
		new string[4] { "SupremeRulerArchetype", "OverlordArchetype", "TombOfTheSupremeBeingFeature", "SupremeOverlordDominionFeature" },
		new string[4] { "ParagonOfHopeArchetype", "HeroArchetype", "MiracleOfUnyieldingHopeFeature", "ParagonOfHopeUndyingFeature" },
		new string[4] { "AstralSwordmasterArchetype", "MartialGodArchetype", "InfiniteBladeSymphonyFeature", "PhantomSovereignBladesFeature" }
	};

	internal static readonly string[] NewSupportBlueprints = new string[2] { "ShadowSovereignLegionBuff", "ShadowSovereignNightLegionBuff" };

	internal static EpicTrack Epic(string className)
	{
		return className switch
		{
			"DemigodPrestigeClass" => new EpicTrack
			{
				Signature = "DemigodSparkFeature",
				Capstone = "DemigodImmortalFormFeature",
				Mastery = "DemigodPortfolioFeature",
				Third = "DemigodMantleFeature",
				Fifth = "DemigodMajestyFeature",
				Seventh = "DemigodSalientFeature"
			}, 
			"GodhunterPrestigeClass" => new EpicTrack
			{
				Signature = "GodhunterDeicideFeature",
				Capstone = "GodhunterSeverDivinityFeature",
				Mastery = "GodhunterResolveFeature",
				Third = "GodhunterWardBreakerFeature",
				Fifth = "GodhunterGodslayerEdgeFeature",
				Seventh = "GodhunterUnbowedFeature"
			}, 
			"WarmasterPrestigeClass" => new EpicTrack
			{
				Signature = "WarmasterSupremacyFeature",
				Capstone = "WarmasterApexConquerorFeature",
				Mastery = "WarmasterCommandFeature",
				Third = "WarmasterGritFeature",
				Fifth = "WarmasterFormationFeature",
				Seventh = "WarmasterUnbreakableLineFeature"
			}, 
			"EternalSeekerPrestigeClass" => new EpicTrack
			{
				Signature = "EternalSeekerMultiverseFeature",
				Capstone = "EternalSeekerOmniscientFeature",
				Mastery = "EternalSeekerDestinyFeature",
				Third = "EternalSeekerGlimpseFeature",
				Fifth = "EternalSeekerGraspFeature",
				Seventh = "EternalSeekerEchoesFeature"
			}, 
			"ArchmagePrestigeClass" => new EpicTrack
			{
				Signature = "ArchmageSpellfireFeature",
				Capstone = "ArchmageOverloadFeature",
				Mastery = "ArchmageReservoirFeature",
				Third = "ArchmageArcaneReservesFeature",
				Fifth = "ArchmageShapingFeature",
				Seventh = "ArchmageArcaneFireFeature"
			}, 
			"ShadowSovereignPrestigeClass" => new EpicTrack
			{
				Signature = "ShadowSovereignDomainFeature",
				Capstone = "ShadowSovereignIncarnateFeature",
				Mastery = "ShadowSovereignArmyFeature",
				Third = "ShadowSovereignVeilFeature",
				Fifth = "ShadowSovereignLegionFeature",
				Seventh = "ShadowSovereignVoidFeature"
			}, 
			_ => null, 
		};
	}

	internal static string[] NewEpicIdentityAt(string className, int level)
	{
		EpicTrack epicTrack = Epic(className);
		if (epicTrack == null)
		{
			return new string[0];
		}
		List<string> list = new List<string>();
		if (EpicMasteryLevels.Contains(level))
		{
			list.Add(epicTrack.Mastery);
		}
		if (level == 3)
		{
			list.Add(epicTrack.Third);
		}
		if (level == 5)
		{
			list.Add(epicTrack.Fifth);
		}
		if (level == 7)
		{
			list.Add(epicTrack.Seventh);
		}
		return list.ToArray();
	}

	internal static string[] IdentityAt(string className, int level)
	{
		if (level < 1 || level > 10)
		{
			return new string[0];
		}
		if (className == "TranscendentSovereignClass")
		{
			List<string> list = new List<string>();
			switch (level)
			{
			case 1:
				list.Add("TranscendentAuthority");
				break;
			case 3:
				list.Add("PlotTwistFeature");
				break;
			case 5:
				list.Add("TranscendentDomainFeature");
				break;
			case 7:
				list.Add("AbsoluteCounterFeature");
				break;
			case 9:
				list.Add("SovereignCrownFeature");
				break;
			case 10:
				list.Add("RealityRewriteFeature");
				break;
			}
			if (SovereignDecreeLevels.Contains(level))
			{
				list.Add("SovereignDecreeFeature");
			}
			return list.ToArray();
		}
		if (className == "OtherworldRetinueClass")
		{
			switch (level)
			{
			case 1:
				return new string[2] { "RetinuePactSelection", "RetinueProtagonistResonanceFeature" };
			case 2:
				return new string[1] { "RetinueLesserPlotArmorFeature" };
			case 3:
				return new string[1] { "RetinueBorrowedPowerSelection" };
			case 4:
				return new string[1] { "RetinueAuraRelayFeature" };
			case 5:
				return new string[2] { "RetinueHypeEngineFeature", "RetinueSignatureBondSelection" };
			case 6:
				return new string[1] { "RetinueBorrowedPowerSelection" };
			case 7:
				return new string[1] { "RetinueSharedLimitBreakFeature" };
			case 8:
				return new string[1] { "RetinueTeamworkHarmonyFeature" };
			case 9:
				return new string[1] { "RetinueBorrowedPowerSelection" };
			case 10:
				return new string[1] { "RetinueSovereignsRightHandFeature" };
			}
		}
		EpicTrack epicTrack = Epic(className);
		if (epicTrack == null)
		{
			return new string[0];
		}
		List<string> list2 = new List<string>();
		if (level == 1)
		{
			list2.Add(epicTrack.Signature);
		}
		list2.AddRange(NewEpicIdentityAt(className, level));
		if (level == 10)
		{
			list2.Add(epicTrack.Capstone);
		}
		return list2.ToArray();
	}

	internal static string[] GenericAt(string className, int level)
	{
		if (className == "TranscendentSovereignClass")
		{
			switch (level)
			{
			case 2:
			case 4:
			case 6:
			case 8:
				return new string[1] { "IsekaiBonusFeatSelection" };
			case 9:
				return new string[1] { "SpecialPowerSelection" };
			default:
				return new string[0];
			}
		}
		if (className == "OtherworldRetinueClass")
		{
			if (level % 2 != 0)
			{
				return new string[0];
			}
			return new string[1] { "IsekaiBonusFeatSelection" };
		}
		if (Epic(className) == null)
		{
			return new string[0];
		}
		switch (level)
		{
		case 5:
			return new string[1] { "OverpoweredAbilitySelection" };
		case 9:
			return new string[1] { "SpecialPowerSelection" };
		default:
			if (level % 2 == 0)
			{
				return new string[1] { "IsekaiBonusFeatSelection" };
			}
			return new string[0];
		}
	}

	internal static string SpellbookSelectionOf(string className)
	{
		if (!(className == "OtherworldRetinueClass"))
		{
			return "EpicPrestigeSpellbookSelection";
		}
		return "RetinueSpellbookSelection";
	}

	internal static string[] AllAt(string className, int level)
	{
		List<string> list = new List<string>(IdentityAt(className, level));
		list.AddRange(GenericAt(className, level));
		if (level == 1)
		{
			list.Add(SpellbookSelectionOf(className));
		}
		return list.ToArray();
	}

	internal static string ExemptionReason(string className, int level)
	{
		return null;
	}

	internal static IEnumerable<KeyValuePair<string, string[]>> NewFeatureClaims()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>(StringComparer.Ordinal);
		dictionary["DemigodPortfolioFeature"] = Of(new string[2] { "AddContextStatBonus", "ContextRankConfig" });
		dictionary["DemigodMantleFeature"] = Of(new string[2] { "AddDamageResistancePhysical", "AddEffectFastHealing" });
		dictionary["DemigodMajestyFeature"] = Of(new string[1] { "AddStatBonus" });
		dictionary["DemigodSalientFeature"] = Of(new string[3] { "BuffDescriptorImmunity", "SpellImmunityToSpellDescriptor", "AddSpellResistance" });
		dictionary["GodhunterResolveFeature"] = Of(new string[2] { "AddContextStatBonus", "ContextRankConfig" });
		dictionary["GodhunterWardBreakerFeature"] = Of(new string[2] { "AddSpellResistance", "AddStatBonus" });
		dictionary["GodhunterGodslayerEdgeFeature"] = Of(new string[2] { "AddAdditionalWeaponDamage", "CriticalConfirmationBonus" });
		dictionary["GodhunterUnbowedFeature"] = Of(new string[2] { "BuffDescriptorImmunity", "SpellImmunityToSpellDescriptor" });
		dictionary["WarmasterCommandFeature"] = Of(new string[2] { "AddContextStatBonus", "ContextRankConfig" });
		dictionary["WarmasterGritFeature"] = Of(new string[2] { "AddDamageResistancePhysical", "AddStatBonus" });
		dictionary["WarmasterFormationFeature"] = Of(new string[1] { "AddStatBonus" });
		dictionary["WarmasterUnbreakableLineFeature"] = Of(new string[3] { "AddFortification", "BuffDescriptorImmunity", "SpellImmunityToSpellDescriptor" });
		dictionary["EternalSeekerDestinyFeature"] = Of(new string[3] { "BuffAllSkillsBonus", "AddContextStatBonus", "ContextRankConfig" });
		dictionary["EternalSeekerGlimpseFeature"] = Of(new string[1] { "AddStatBonus" });
		dictionary["EternalSeekerGraspFeature"] = Of(new string[2] { "AddCasterLevel", "SpellPenetrationBonus" });
		dictionary["EternalSeekerEchoesFeature"] = Of(new string[2] { "IncreaseAllSpellsDC", "AddStatBonus" });
		dictionary["ArchmageReservoirFeature"] = Of(new string[2] { "AddContextStatBonus", "ContextRankConfig" });
		dictionary["ArchmageArcaneReservesFeature"] = Of(new string[1] { "AddSpellsPerDay" });
		dictionary["ArchmageShapingFeature"] = Of(new string[1] { "AddCasterLevel" });
		dictionary["ArchmageArcaneFireFeature"] = Of(new string[2] { "SpellPenetrationBonus", "IncreaseAllSpellsDC" });
		dictionary["ShadowSovereignArmyFeature"] = Of(new string[2] { "AddContextStatBonus", "ContextRankConfig" });
		dictionary["ShadowSovereignVeilFeature"] = Of(new string[2] { "AddConcealment", "AddStatBonus" });
		dictionary["ShadowSovereignLegionFeature"] = Of(new string[1] { "OnSpawnBuff" });
		dictionary["ShadowSovereignVoidFeature"] = Of(new string[1] { "AddDamageResistanceEnergy" });
		dictionary["SovereignDecreeFeature"] = Of(new string[2] { "AddContextStatBonus", "ContextRankConfig" });
		dictionary["SovereignCrownFeature"] = Of(new string[2] { "IncreaseAllSpellsDC", "AddStatBonus" });
		dictionary["CelestialMonarchThroneFeature"] = Of(new string[2] { "AddStatBonus", "IncreaseAllSpellsDC" });
		dictionary["DemonSlimeAppetiteFeature"] = Of(new string[2] { "AddEffectFastHealing", "AddDamageResistancePhysical" });
		dictionary["ShadowSovereignNightLegionFeature"] = Of(new string[2] { "OnSpawnBuff", "AddConcealment" });
		dictionary["GrandStrategistCheckmateFeature"] = Of(new string[3] { "AddStatBonus", "AddMechanicsFeature", "IncreaseAllSpellsDC" });
		dictionary["SupremeOverlordDominionFeature"] = Of(new string[3] { "BuffDescriptorImmunity", "SpellImmunityToSpellDescriptor", "IncreaseAllSpellsDC" });
		dictionary["ParagonOfHopeUndyingFeature"] = Of(new string[4] { "BuffDescriptorImmunity", "SpellImmunityToSpellDescriptor", "AddStatBonus", "AddFortification" });
		dictionary["PhantomSovereignBladesFeature"] = Of(new string[2] { "AddAdditionalWeaponDamage", "CriticalConfirmationBonus" });
		return dictionary;
		static string[] Of(params string[] components)
		{
			return components;
		}
	}

	internal static int MasteryRanks(string featureName)
	{
		if (featureName == "SovereignDecreeFeature")
		{
			return SovereignDecreeLevels.Length;
		}
		if (!EpicClassNames.Any((string c) => Epic(c).Mastery == featureName))
		{
			return 1;
		}
		return EpicMasteryLevels.Length;
	}
}
