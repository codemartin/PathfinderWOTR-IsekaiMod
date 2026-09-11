using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class WizardLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "WizardLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Wizard Legacy - Arcane Savant");
				bp.SetDescription(Main.IsekaiContext, "A tireless scholar of cosmic mathematics and magical formulae, you master specialized schools of arcane theory. Your deep understanding of planar weave structure unlocks profound arcane discoveries and enhanced metamagic control.");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
			ShadowMonarchLegacySelection.Register(prog);
			DevourerLegacySelection.Register(prog);
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "WizardLegacy");
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				prog = PatchTools.PatchClassProgressionBasedOnRefClass(prog, ClassTools.Classes.WizardClass);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.WizardClass);
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.WizardClass.ToReference<BlueprintCharacterClassReference>();
				});
			}
		}
	}
}
