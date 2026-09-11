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
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class DruidBaseLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "DruidBaseLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Druid Legacy - Nature Mage");
				bp.SetDescription(Main.IsekaiContext, "You were a nature lover in your previous world, but you lived in a polluted and crowded city that stifled your connection to the natural world. \nYou dreamed of escaping to a place where you could be free and wild, and one day you got your wish. \nYou have been reincarnated in a world where nature is abundant and diverse, and you have learned how to tap into its primal magic. \nYou can cast spells that manipulate the elements, summon creatures, and enhance your own abilities. \nYou are a protector of nature and a friend to all living.");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.NeutralGood | AlignmentMaskType.LawfulNeutral | AlignmentMaskType.TrueNeutral | AlignmentMaskType.ChaoticNeutral | AlignmentMaskType.NeutralEvil;
				});
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Prohibit(prog);
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				LevelEntry[] additionalReference = new LevelEntry[0];
				LevelEntry[] array = new LevelEntry[0];
				array = array.AppendToArray(Helpers.CreateLevelEntry(1, FeatTools.Selections.DruidBondSelection, BlueprintTools.GetBlueprint<BlueprintFeature>("b296531ffe013c8499ad712f8ae97f6b"), BlueprintTools.GetBlueprint<BlueprintFeature>("d00ff3791359311449c481126fbf71ce")));
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.DruidClass, additionalReference, array);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.DruidClass);
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.DruidClass.ToReference<BlueprintCharacterClassReference>();
				});
			}
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "DruidBaseLegacy");
		}
	}
}
