using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class MagusArcherLegacy
	{
		private static string BaseArchetypeId = "44388c01eb4a29d4d90a25cc0574320d";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "MagusArcherLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Magus Legacy - Eldritch Archer");
				bp.SetDescription(Main.IsekaiContext, "Now listen, if you can imbue into and cast your magic through a sword or axe, what exactly prevents you from doing the same with an arrow? \nLet me tell you, the answer is nothing. \nSo watch me as I remain savely at range and give my enemy a nasty suprise when hiding behind his nice large metal shield only made him a bigger more conductive target for the lighting spell I hid in my arrow...");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
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
				array2 = array2.AppendToArray(BaseArchetype.RemoveFeatures);
				array = array.AppendToArray(BaseArchetype.AddFeatures);
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.MagusClass, array, array2);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				LevelEntry[] addFeatures = BaseArchetype.AddFeatures;
				for (int i = 0; i < addFeatures.Length; i++)
				{
					foreach (BlueprintFeatureBase feature3 in addFeatures[i].Features)
					{
						if (feature3 != null && feature3 is BlueprintFeatureSelection feature)
						{
							PatchTools.PatchClassIntoFeatureOfReferenceClass(feature, reference, ClassTools.ClassReferences.MagusClass);
						}
						else if (feature3 != null && feature3 is BlueprintFeature feature2)
						{
							PatchTools.PatchClassIntoFeatureOfReferenceClass(feature2, reference, ClassTools.ClassReferences.MagusClass);
						}
					}
				}
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = MagusBasicLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = MagusDancerLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = MagusSpellbladeLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
			}
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.MagusClass.ToReference<BlueprintCharacterClassReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "MagusArcherLegacy");
		}

		public static void PatchForBroadStudy()
		{
			if (prog != null)
			{
				prog.AddComponent(delegate(BroadStudyComponent c)
				{
					c.CharacterClass = IsekaiProtagonistClass.GetReference();
				});
			}
		}
	}
}
