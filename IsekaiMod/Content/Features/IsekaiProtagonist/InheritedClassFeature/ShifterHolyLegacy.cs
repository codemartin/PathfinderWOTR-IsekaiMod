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
	internal class ShifterHolyLegacy
	{
		private static string BaseArchetypeId = "1cdfc7d306d1430eac19427539b62091";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			if (ClassTools.Classes.ShifterClass == null)
			{
				return;
			}
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ShifterHolyLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Shifter Legacy - Divine Beast");
				bp.SetDescription(Main.IsekaiContext, "Your reincarnation was the action of a god. \nThey send you to this world to be a hero, so while you feel a connection to nature this connection is in a way \"tainted\" by that divine connection. \nYour kin are not the normal animals roaming the woods but the divine beasts of your deities realm.");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.Good;
				});
			});
		}

		public static void PatchProgression()
		{
			if (ClassTools.Classes.ShifterClass == null)
			{
				return;
			}
			if (BaseArchetype == null)
			{
				BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);
				if (BaseArchetype == null)
				{
					return;
				}
			}
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
			prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.ShifterClass, BaseArchetype, null);
			BlueprintCharacterClassReference shifterClass = ClassTools.ClassReferences.ShifterClass;
			PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(IsekaiProtagonistClass.GetReference(), shifterClass, BaseArchetype);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.ShifterClass.ToReference<BlueprintCharacterClassReference>();
			});
			prog.AddPrerequisiteNoFeature(ShifterLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterBaseLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterBaseLegacy.GetEvilAlternate());
			prog.AddPrerequisiteNoFeature(ShifterStingerLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterGriffonLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterDragonLegacy.Get());
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ShifterHolyLegacy");
		}
	}
}
