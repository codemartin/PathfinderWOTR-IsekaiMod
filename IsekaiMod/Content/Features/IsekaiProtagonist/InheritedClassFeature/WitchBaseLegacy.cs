using System.Collections.Generic;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;

internal class WitchBaseLegacy
{
	private static BlueprintProgression prog;

	public static void Configure()
	{
		prog = Helpers.CreateBlueprint(Main.IsekaiContext, "WitchBaseLegacy", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Witch Legacy - Pactmaker");
			bp.SetDescription(Main.IsekaiContext, "You were once an ordinary person in your original world, until you were summoned by a mysterious entity to another world full of magic and monsters. \nThere, you learned how to make pacts with various beings, from spirits and demons to dragons and gods. \nYou use your pacts to gain power, knowledge, and allies in this strange new world. \nBut beware, for every pact has a price, and some beings may not be so willing to cooperate with you.");
			bp.GiveFeaturesForPreviousLevels = true;
		});
		WitchPatronSelection.Configure();
		LegacySelection.RegisterForFeat(prog);
		LegacySelection.Register(prog);
		ShadowMonarchLegacySelection.Register(prog);
		DevourerLegacySelection.Register(prog);
		MartialGodLegacySelection.Prohibit(prog);
		GodEmperorLegacySelection.Prohibit(prog);
		HeroLegacySelection.Register(prog);
		MastermindLegacySelection.Register(prog);
		OverlordLegacySelection.Prohibit(prog);
	}

	public static void PatchProgression()
	{
		if (prog != null)
		{
			prog = PatchTools.PatchClassProgressionBasedOnRefClass(prog, ClassTools.Classes.WitchClass);
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.WitchClass);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.WitchClass.ToReference<BlueprintCharacterClassReference>();
			});
			AddOrMergeLevelEntry(5, WitchPatronSelection.Get());
			AddOrMergeLevelEntry(10, WitchPatronSelection.Get());
			AddOrMergeLevelEntry(15, WitchPatronSelection.Get());
			AddOrMergeLevelEntry(20, WitchPatronSelection.Get());
			prog.AddPrerequisiteNoFeature(ShamanLegacy.Get());
			WitchPatronSelection.Get()?.AddFeatures(FeatTools.Selections.WitchPatronSelection.m_AllFeatures);
		}
	}

	private static void AddOrMergeLevelEntry(int level, BlueprintFeature feature)
	{
		if (prog != null && feature != null)
		{
			prog.LevelEntries = MergeLevelEntry(prog.LevelEntries, level, feature.ToReference<BlueprintFeatureBaseReference>());
		}
	}

	internal static LevelEntry[] MergeLevelEntry(LevelEntry[] entries, int level, BlueprintFeatureBaseReference featureRef)
	{
		if (featureRef == null)
		{
			return entries;
		}
		entries = entries ?? new LevelEntry[0];
		LevelEntry[] array = entries;
		foreach (LevelEntry levelEntry in array)
		{
			if (levelEntry != null && levelEntry.Level == level)
			{
				if (levelEntry.m_Features == null)
				{
					levelEntry.m_Features = new List<BlueprintFeatureBaseReference>();
				}
				if (!levelEntry.m_Features.Contains(featureRef))
				{
					levelEntry.m_Features.Add(featureRef);
				}
				return entries;
			}
		}
		return entries.AddToArray(new LevelEntry
		{
			Level = level,
			m_Features = new List<BlueprintFeatureBaseReference> { featureRef }
		});
	}

	public static BlueprintProgression Get()
	{
		if (prog != null)
		{
			return prog;
		}
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "WitchBaseLegacy");
	}
}
