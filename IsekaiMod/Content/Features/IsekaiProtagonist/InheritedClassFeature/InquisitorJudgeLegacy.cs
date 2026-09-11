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
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class InquisitorJudgeLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "InquisitorJudgeLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Inquisitor Legacy - Judge");
				bp.SetDescription(Main.IsekaiContext, "You were a judge in your previous world, but you were killed by a corrupt system that twisted the law to serve its own interests. \nYou have been reborn in a world where law is often ignored or perverted, and you have sworn to uphold the law and punish the lawbreakers. \nYou can use your judgements more often and more effectively than normal inquisitors, enhancing your abilities and weakening your foes. \nYou are a champion of law and order, and a scourge of chaos and anarchy.");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddPrerequisite(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.Lawful;
				});
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				LevelEntry[] array = new LevelEntry[0];
				LevelEntry[] array2 = new LevelEntry[0];
				array2 = array2.AppendToArray(Helpers.CreateLevelEntry(1, FeatTools.Selections.DomainsSelection));
				array = array.AppendToArray(Helpers.CreateLevelEntry(2, InquisitorTacticianLegacy.GetDomains()));
				BlueprintArchetype blueprint = BlueprintTools.GetBlueprint<BlueprintArchetype>("0e5e91c17f114d358910e0da4ae29b50");
				array2 = array2.AppendToArray(blueprint.RemoveFeatures);
				array = array.AppendToArray(blueprint.AddFeatures);
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.InquisitorClass, array, array2);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				LevelEntry[] addFeatures = blueprint.AddFeatures;
				for (int i = 0; i < addFeatures.Length; i++)
				{
					foreach (BlueprintFeatureBase feature3 in addFeatures[i].Features)
					{
						if (feature3 != null && feature3 is BlueprintFeatureSelection feature)
						{
							PatchTools.PatchClassIntoFeatureOfReferenceClass(feature, reference, ClassTools.ClassReferences.InquisitorClass);
						}
						else if (feature3 != null && feature3 is BlueprintFeature feature2)
						{
							PatchTools.PatchClassIntoFeatureOfReferenceClass(feature2, reference, ClassTools.ClassReferences.InquisitorClass);
						}
					}
				}
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = InquisitorTacticianLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = InquisitorDomainLordLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
			}
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.InquisitorClass.ToReference<BlueprintCharacterClassReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "InquisitorJudgeLegacy");
		}
	}
}
