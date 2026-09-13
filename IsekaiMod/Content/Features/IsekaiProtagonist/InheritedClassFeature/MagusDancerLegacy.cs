using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
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
	internal class MagusDancerLegacy
	{
		private static string BaseArchetypeId = "1125145639129cf45b6b9b674cbd62b1";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "MagusDancerLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Magus Legacy - Spell Dancer");
				bp.SetDescription(Main.IsekaiContext, "So you were not really into martial arts or physical fighting of any kind. \nMore like hundrets of hours of ballet lessons now serve as he basis of some really good dodging skills. \nNow with magical powers running through your veins you are even more flexible and thus have an even easier time dodging. \nAccording to some elf you met this is actually a special elven form of magic called spell dancing.");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
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
				// Spell Dancer retains the normal Magus arcane pool. Owlcat keeps that feature
				// outside the archetype additions copied above in some game versions, which left
				// inherited Spell Dancers without the pool and unable to qualify for related feats.
				BlueprintFeature arcanePool = BlueprintTools.GetBlueprint<BlueprintFeature>("a2fe27d00ece46a5a109d2d1f0bfafa7");
				if (arcanePool != null)
				{
					bool hasArcanePool = false;
					LevelEntry levelOne = null;
					LevelEntry[] levelEntries = prog.LevelEntries;
					foreach (LevelEntry levelEntry in levelEntries)
					{
						if (levelEntry.Level == 1)
						{
							levelOne = levelEntry;
						}
						foreach (BlueprintFeatureBaseReference feature4 in levelEntry.m_Features)
						{
							if (feature4 != null && feature4.Guid == arcanePool.AssetGuid)
							{
								hasArcanePool = true;
							}
						}
					}
					if (!hasArcanePool)
					{
						if (levelOne == null)
						{
							prog.LevelEntries = prog.LevelEntries.AppendToArray(Helpers.CreateLevelEntry(1, arcanePool));
						}
						else
						{
							levelOne.m_Features.Add(arcanePool.ToReference<BlueprintFeatureBaseReference>());
						}
					}
					PatchTools.PatchClassIntoFeatureOfReferenceClass(arcanePool, reference, ClassTools.ClassReferences.MagusClass);
				}
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
					c.m_Feature = MagusArcherLegacy.Get().ToReference<BlueprintFeatureReference>();
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
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "MagusDancerLegacy");
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
