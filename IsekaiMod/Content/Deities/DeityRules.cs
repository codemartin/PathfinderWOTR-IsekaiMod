using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Deities;

internal static class DeityRules
{
	internal const string MythicIgnoreAlignmentRestrictionsGuid = "24e78475f0a243e1a810452d14d0a1bd";

	internal const string WarpriestDeitySacredWeaponFeatureGuid = "bae328694ee21bc4587c5ffd0035b6a2";

	internal const string SacredWeaponRapier = "a8b41c73bfca4b94c935f45f6fce3601";

	internal const string SacredWeaponDagger = "0bca569c95613264d844f1fd00937968";

	internal const string SacredWeaponShortsword = "6f7f3a0cbeba784469b17b3d8f8260ea";

	internal const string SacredWeaponHeavyMace = "2eacfd16f6bc0b445813f28473d3e6ba";

	internal const string SacredWeaponStarknife = "bb445cdb72b543f429ef375a1ccaba11";

	internal static readonly Dictionary<string, string> SeparatistDomainGuids = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "AirDomain", "fa67965152634a16bf619ad0e72d57ff" },
		{ "AnimalDomain", "617b8a3d83bc45e391b53ebc13f9e5b1" },
		{ "ArtificeDomain", "2ab5269971064d37bb26a89ab0dd7c08" },
		{ "ChaosDomain", "da3be0f595ab46e29a010afdc8f6c858" },
		{ "CharmDomain", "473a037df8ed4919ab4ccb605f884f54" },
		{ "CommunityDomain", "c91daec2470c4e87a2833631d2eee347" },
		{ "DarknessDomain", "94d955787f7248e99504bb2f22297d41" },
		{ "DeathDomain", "2a97721a11434058a530c70ff73b640c" },
		{ "MurderSubdomain", "662cf0a1c4654e44b9dad4d8c2faefd3" },
		{ "UndeadSubdomain", "b8ea7061fb744142a6c76bc70b5ff74d" },
		{ "DestructionDomain", "0a53b9d715b54242bf84fe105d6f0401" },
		{ "EarthDomain", "0ceca50b20394281af0a1245b2007dec" },
		{ "EvilDomain", "33d69c06ca4c4afd94df5bdda511c508" },
		{ "FireDomain", "10e69991b2c043b69287c3b6a07fec94" },
		{ "GloryDomain", "f803ea6e161d4a789187d44f8d09fd90" },
		{ "GoodDomain", "7fd23a5b7fc84e59a71b69289e468294" },
		{ "HealingDomain", "4a352fc8440544f3b58b4e91f6e0f551" },
		{ "KnowledgeDomain", "259a3e560c1b48ab899d878f3332f8ba" },
		{ "LawDomain", "c0f4e109353b4ddc875cf8a42faa63cd" },
		{ "LiberationDomain", "31a283dacb484afc9ef8d69979c24d0c" },
		{ "LuckDomain", "b99c38f89c014eaeab5543c00d795721" },
		{ "MadnessDomain", "bfbc42a8c94a4d4b9f424565ec97c755" },
		{ "MagicDomain", "0cdeca405da848d49f922a9422fefa3c" },
		{ "NobilityDomain", "c3543d3da9774e81808a2bbe721a2174" },
		{ "PlantDomain", "71dcf73a8ca0457da97e66ac0d663c42" },
		{ "ProtectionDomain", "5d8dc80eb8714ad2a14353a0e3cbd20c" },
		{ "ReposeDomain", "928ec21e21fd471cac25bec3ea3dfaa9" },
		{ "RuneDomain", "5cf9bc2be3ba4a56a1e17c10f1b5d693" },
		{ "ScalykindDomain", "4448c8005c854a72a0853319171f29e6" },
		{ "StrengthDomain", "f07500d237ac4ac9b10860b72a25e8f1" },
		{ "SunDomain", "10958fa969f84b6486271b9559016058" },
		{ "TravelDomain", "deb547decbf845b6aaa176c2717723c8" },
		{ "TrickeryDomain", "085b645950fa492c85b3a3a2fffd6fd6" },
		{ "WarDomain", "0535f677358f4f3bb7cd8c5c4b1a58f5" },
		{ "IceSubdomain", "76e69d9915454f238c6b0eea0db62703" },
		{ "WaterDomain", "87b27e3d011f4caea0e2679cbd90e7f7" },
		{ "WeatherDomain", "8bc3629d677f48798235b94cc9ba0f36" }
	};

	private static readonly Dictionary<string, string[]> Subdomains = new Dictionary<string, string[]>(StringComparer.Ordinal)
	{
		{
			"DeathDomain",
			new string[2] { "MurderSubdomain", "UndeadSubdomain" }
		},
		{
			"WaterDomain",
			new string[1] { "IceSubdomain" }
		}
	};

	internal static BlueprintUnitFactReference IgnoreAlignmentRestrictions()
	{
		return BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("24e78475f0a243e1a810452d14d0a1bd");
	}

	internal static BlueprintSpellbookReference[] DivineSpellbooks()
	{
		return new BlueprintSpellbookReference[3]
		{
			SpellTools.Spellbook.CrusaderSpellbook.ToReference<BlueprintSpellbookReference>(),
			SpellTools.Spellbook.ClericSpellbook.ToReference<BlueprintSpellbookReference>(),
			SpellTools.Spellbook.InquisitorSpellbook.ToReference<BlueprintSpellbookReference>()
		};
	}

	internal static void WireSacredWeapon(BlueprintFeature deity, string sacredWeaponFeatureGuid)
	{
		if (deity == null)
		{
			return;
		}
		BlueprintUnitFactReference sacredWeapon = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(sacredWeaponFeatureGuid);
		deity.AddComponent(delegate(AddFeatureIfHasFact c)
		{
			c.m_CheckedFact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("bae328694ee21bc4587c5ffd0035b6a2");
			c.m_Feature = sacredWeapon;
			c.Not = false;
		});
		BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>("bae328694ee21bc4587c5ffd0035b6a2");
		if (blueprint == null)
		{
			Main.IsekaiContext.Logger.LogError("[Deity] WarpriestDeitySacredWeaponFeature not found; " + deity.name + " is wired one way only.");
			return;
		}
		blueprint.AddComponent(delegate(AddFeatureIfHasFact c)
		{
			c.m_CheckedFact = deity.ToReference<BlueprintUnitFactReference>();
			c.m_Feature = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(sacredWeaponFeatureGuid);
			c.Not = false;
		});
	}

	internal static List<string> SeparatistDomains(AlignmentMaskType deityAlignment, params string[] ownDomains)
	{
		HashSet<string> excluded = new HashSet<string>(ownDomains ?? new string[0], StringComparer.Ordinal);
		string[] array = ownDomains ?? new string[0];
		foreach (string key in array)
		{
			if (Subdomains.TryGetValue(key, out var value))
			{
				excluded.UnionWith(value);
			}
		}
		if ((deityAlignment & AlignmentMaskType.Good) != AlignmentMaskType.None && (deityAlignment & AlignmentMaskType.Evil) == 0)
		{
			excluded.Add("EvilDomain");
		}
		if ((deityAlignment & AlignmentMaskType.Evil) != AlignmentMaskType.None && (deityAlignment & AlignmentMaskType.Good) == 0)
		{
			excluded.Add("GoodDomain");
		}
		if ((deityAlignment & AlignmentMaskType.Lawful) != AlignmentMaskType.None && (deityAlignment & AlignmentMaskType.Chaotic) == 0)
		{
			excluded.Add("ChaosDomain");
		}
		if ((deityAlignment & AlignmentMaskType.Chaotic) != AlignmentMaskType.None && (deityAlignment & AlignmentMaskType.Lawful) == 0)
		{
			excluded.Add("LawDomain");
		}
		return SeparatistDomainGuids.Keys.Where((string d) => !excluded.Contains(d)).ToList();
	}

	internal static BlueprintUnitFactReference[] SeparatistDomainFacts(AlignmentMaskType deityAlignment, params string[] ownDomains)
	{
		return (from d in SeparatistDomains(deityAlignment, ownDomains)
			select BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(SeparatistDomainGuids[d])).ToArray();
	}
}
