using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class ArcanistEldritchFontLegacy
	{
		private static string BaseArchetypeId = "2f35608fc073c04448a9c162a20852ce";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ArcanistEldritchFontLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Arcanist Legacy - Eldritch Font");
				bp.SetDescription(Main.IsekaiContext, "TBD");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
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
				prog.SetDescription(BaseArchetype.LocalizedDescription);
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.ArcanistClass, array, array2);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				LevelEntry[] addFeatures = BaseArchetype.AddFeatures;
				for (int i = 0; i < addFeatures.Length; i++)
				{
					foreach (BlueprintFeatureBase feature3 in addFeatures[i].Features)
					{
						if (feature3 != null && feature3 is BlueprintFeatureSelection feature)
						{
							PatchTools.PatchClassIntoFeatureOfReferenceClass(feature, reference, ClassTools.ClassReferences.ArcanistClass);
						}
						else if (feature3 != null && feature3 is BlueprintFeature feature2)
						{
							PatchTools.PatchClassIntoFeatureOfReferenceClass(feature2, reference, ClassTools.ClassReferences.ArcanistClass);
						}
					}
				}
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = ArcanistBasicLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = ArcanistBrownFurLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
			}
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.ArcanistClass.ToReference<BlueprintCharacterClassReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ArcanistEldritchFontLegacy");
		}
	}
}
