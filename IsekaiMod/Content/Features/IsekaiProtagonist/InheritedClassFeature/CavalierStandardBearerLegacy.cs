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
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class CavalierStandardBearerLegacy
	{
		private static string BaseArchetypeId = "07db3fec753209546a792a7942288998";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "CavalierStandardBearerLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Cavalier Legacy - Backline Leader");
				bp.SetDescription(Main.IsekaiContext, "Some lead from the front, but while you can charge ahead you prefer to lead from the back.\nAfter all the backline grants you a much better strategic position to evaluate where the problems in the formation are. \nAlso, at your core you are a caster, so why should you charge into the thick of things?");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				if (BaseArchetype == null)
				{
					BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);
					if (BaseArchetype == null)
					{
						return;
					}
				}
				LevelEntry[] array = new LevelEntry[0];
				LevelEntry[] array2 = new LevelEntry[0];
				array2 = array2.AppendToArray(Helpers.CreateLevelEntry(5, FeatTools.Selections.CavalierMountSelection));
				array2 = array2.AppendToArray(BaseArchetype.RemoveFeatures);
				array = array.AppendToArray(BaseArchetype.AddFeatures);
				array = array.AppendToArray(Helpers.CreateLevelEntry(3, BlueprintTools.GetBlueprint<BlueprintFeature>("1b9916f7675d6ef4fb427081250d49de"), BlueprintTools.GetBlueprint<BlueprintFeature>("a318fa1af8424638ab10c4f98c11ee6a")));
				array = array.AppendToArray(Helpers.CreateLevelEntry(5, BlueprintTools.GetBlueprint<BlueprintFeature>("c3abcce19f9f80640a867c9e75f880b2"), BlueprintTools.GetBlueprint<BlueprintFeature>("7bc55b5e381358c45b42153b8b2603a6")));
				array = array.AppendToArray(Helpers.CreateLevelEntry(15, BlueprintTools.GetBlueprint<BlueprintFeature>("7bc55b5e381358c45b42153b8b2603a6")));
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.CavalierClass, array, array2);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				LevelEntry[] addFeatures = BaseArchetype.AddFeatures;
				for (int i = 0; i < addFeatures.Length; i++)
				{
					foreach (BlueprintFeatureBase feature3 in addFeatures[i].Features)
					{
						if (feature3 != null && feature3 is BlueprintFeatureSelection feature)
						{
							PatchTools.PatchClassIntoFeatureOfReferenceClass(feature, reference, ClassTools.ClassReferences.CavalierClass);
						}
						else if (feature3 != null && feature3 is BlueprintFeature feature2)
						{
							PatchTools.PatchClassIntoFeatureOfReferenceClass(feature2, reference, ClassTools.ClassReferences.CavalierClass);
						}
					}
				}
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = InquisitorTacticianLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = CavalierBasicLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = CavalierKnightOfWall.Get().ToReference<BlueprintFeatureReference>();
				});
			}
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.CavalierClass.ToReference<BlueprintCharacterClassReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "CavalierStandardBearerLegacy");
		}
	}
}
