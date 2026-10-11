using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

internal static class IsekaiAscendantArchetypes
{
	public static void Add()
	{
		BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
		BlueprintFeatureSelection modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
		BlueprintFeatureSelection modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
		BlueprintFeatureSelection modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
		AscendantMirrorLock.CreateStrings();
		MirrorArchetype("MartialGod", "IsekaiAscendantArchetypeMartialGod", "Martial God Ascendant", "The martial path carried past the mortal ceiling. Your discipline keeps its shape: no borrowed secret, no loophole, only the technique you have been sharpening for twenty levels.", MartialGodArchetype.GetReference(), new LevelEntry[2]
		{
			Helpers.CreateLevelEntry(10, modBlueprint3),
			Helpers.CreateLevelEntry(20, modBlueprint4)
		});
		MirrorArchetype("Hero", "IsekaiAscendantArchetypeHero", "Hero Ascendant", "The hero's path carried past the mortal ceiling. What saves the world from here is still the thing that saved it before, and it is still not a cheat.", HeroArchetype.GetReference(), new LevelEntry[2]
		{
			Helpers.CreateLevelEntry(10, modBlueprint3),
			Helpers.CreateLevelEntry(20, modBlueprint4)
		});
		MirrorArchetype("Overlord", "IsekaiAscendantArchetypeOverlord", "Overlord Ascendant", "The overlord's path carried past the mortal ceiling. The guild master's arsenal was always complete; these levels only widen the throne room.", OverlordArchetype.GetReference(), new LevelEntry[2]
		{
			Helpers.CreateLevelEntry(10, modBlueprint3),
			Helpers.CreateLevelEntry(20, modBlueprint4)
		});
		MirrorArchetype("Devourer", "IsekaiAscendantArchetypeDevourer", "Slime Ascendant", "The devourer's path carried past the mortal ceiling. Everything you become from here you still have to eat first.", DevourerArchetype.GetReference(), new LevelEntry[2]
		{
			Helpers.CreateLevelEntry(10, modBlueprint3),
			Helpers.CreateLevelEntry(20, modBlueprint4)
		});
		MirrorArchetype("ShadowMonarch", "IsekaiAscendantArchetypeShadowMonarch", "Shadow Monarch Ascendant", "The monarch's path carried past the mortal ceiling. The army grows; the bargain that raised it does not change.", ShadowMonarchArchetype.GetReference(), new LevelEntry[2]
		{
			Helpers.CreateLevelEntry(10, modBlueprint3),
			Helpers.CreateLevelEntry(20, modBlueprint4)
		});
		MirrorArchetype("GodEmperor", "IsekaiAscendantArchetypeGodEmperor", "God Emperor Ascendant", "The imperial path carried past the mortal ceiling. Divinity is still not a grab bag of tricks: you give up the Special Power pick at ascension levels 5 and 15, because you are simply more of what you already are.", GodEmperorArchetype.GetReference(), new LevelEntry[4]
		{
			Helpers.CreateLevelEntry(5, modBlueprint2),
			Helpers.CreateLevelEntry(10, modBlueprint3),
			Helpers.CreateLevelEntry(15, modBlueprint2),
			Helpers.CreateLevelEntry(20, modBlueprint4)
		});
		MirrorArchetype("Mastermind", "IsekaiAscendantArchetypeMastermind", "Mastermind Ascendant", "The strategist's path carried past the mortal ceiling. You never won by collecting feats, so you give up the bonus feat at ascension levels 10 and 20.", MastermindArchetype.GetReference(), new LevelEntry[2]
		{
			Helpers.CreateLevelEntry(10, modBlueprint, modBlueprint3),
			Helpers.CreateLevelEntry(20, modBlueprint, modBlueprint4)
		});
	}

	private static void MirrorArchetype(string mirroredArchetype, string blueprintName, string displayName, string description, BlueprintArchetypeReference baseArchetype, LevelEntry[] removeFeatures)
	{
		if (baseArchetype == null || baseArchetype.Guid == BlueprintGuid.Empty)
		{
			Main.IsekaiContext.Logger.LogError("[IsekaiAscendantArchetypes] The " + mirroredArchetype + " archetype could not be referenced, so its ascension mirror was not built. That archetype would be handed back the features it gives up.");
			return;
		}
		IsekaiAscendantClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, blueprintName, delegate(BlueprintArchetype bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			bp.LocalizedDescriptionShort = bp.LocalizedDescription;
			bp.RemoveFeatures = removeFeatures;
			bp.AddFeatures = new LevelEntry[0];
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = baseArchetype;
				c.Level = 1;
			});
		}));
	}
}
