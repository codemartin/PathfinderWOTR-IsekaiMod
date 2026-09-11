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
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class CavalierBasicLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "CavalierBasicLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Cavalier Legacy - Frontline Leader");
				bp.SetDescription(Main.IsekaiContext, "You charge into the thick of things, but no matter how hectic the chaos around you gets you never forget your team. \nAfter all teamwork is at its most important if you and the team are surrounded by enemies on all sides.");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				LevelEntry[] array = new LevelEntry[0];
				LevelEntry[] array2 = new LevelEntry[0];
				array2 = array2.AppendToArray(Helpers.CreateLevelEntry(1, FeatTools.Selections.CavalierMountSelection));
				array = array.AppendToArray(Helpers.CreateLevelEntry(3, BlueprintTools.GetBlueprint<BlueprintFeature>("1b9916f7675d6ef4fb427081250d49de"), BlueprintTools.GetBlueprint<BlueprintFeature>("a318fa1af8424638ab10c4f98c11ee6a")));
				array = array.AppendToArray(Helpers.CreateLevelEntry(5, BlueprintTools.GetBlueprint<BlueprintFeature>("c3abcce19f9f80640a867c9e75f880b2"), BlueprintTools.GetBlueprint<BlueprintFeature>("7bc55b5e381358c45b42153b8b2603a6")));
				array = array.AppendToArray(Helpers.CreateLevelEntry(15, BlueprintTools.GetBlueprint<BlueprintFeature>("7bc55b5e381358c45b42153b8b2603a6")));
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.CavalierClass, array, array2);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.CavalierClass);
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.CavalierClass.ToReference<BlueprintCharacterClassReference>();
				});
				PatchTools.PatchResource(BlueprintTools.GetBlueprint<BlueprintAbilityResource>("672e8c9c98db1df4aa66676a66036e71"), reference);
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = CavalierKnightOfWall.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = CavalierStandardBearerLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = InquisitorTacticianLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
			}
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "CavalierBasicLegacy");
		}
	}
}
