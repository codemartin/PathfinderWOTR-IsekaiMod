using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class BloodragerChimeraLegacy
	{
		private static BlueprintProgression prog;

		private static BlueprintFeatureSelection bloodlines;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "BloodragerChimeraLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Bloodrager Legacy - Chimeric Rager");
				bp.SetDescription(Main.IsekaiContext, "Much like the Chimera you draw upon the power of inhuman bloodlines and learned how to slowly either awaken or fuse more of them into yourself. \nHowever, you would rather use that to empower your melee attacks rather than your spells.\nAfter all, what is the point of draconic claws or a phoenixes burning wings if you only use them as a fallback?");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			bloodlines = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiBloodragerSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(((BlueprintUnitFact)FeatTools.Selections.BloodragerBloodlineSelection).m_DisplayName);
				bp.SetDescription(((BlueprintUnitFact)FeatTools.Selections.BloodragerBloodlineSelection).m_Description);
				bp.Ranks = 4;
				bp.IgnorePrerequisites = true;
				bp.IsClassFeature = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			HeroLegacySelection.Prohibit(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				LevelEntry[] array = new LevelEntry[0];
				LevelEntry[] array2 = new LevelEntry[0];
				array2 = array2.AppendToArray(Helpers.CreateLevelEntry(1, FeatTools.Selections.BloodragerBloodlineSelection));
				bloodlines.SetFeatures(FeatTools.Selections.BloodragerBloodlineSelection.m_AllFeatures);
				array = array.AppendToArray(Helpers.CreateLevelEntry(1, bloodlines));
				array = array.AppendToArray(Helpers.CreateLevelEntry(5, bloodlines));
				array = array.AppendToArray(Helpers.CreateLevelEntry(10, bloodlines));
				array = array.AppendToArray(Helpers.CreateLevelEntry(15, bloodlines));
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.BloodragerClass, array, array2);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.BloodragerClass);
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.BloodragerClass.ToReference<BlueprintCharacterClassReference>();
				});
			}
		}

		public static void PatchPrerequisiteCompatibility()
		{
			BlueprintFeatureSelection secondBloodline = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b7f62628915bdb14d8888c25da3fac56");
			PrerequisiteAlternatives.Add(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19"), FeatTools.Selections.BloodragerBloodlineSelection, bloodlines);
			PrerequisiteAlternatives.Add(secondBloodline, FeatTools.Selections.BloodragerBloodlineSelection, bloodlines);
			PrerequisiteAlternatives.RequireUnownedChoices(secondBloodline);
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "BloodragerChimeraLegacy");
		}
	}
}
