using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class KineticLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "KineticLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Kinetic Legacy - Kinetic Lord");
				bp.SetDescription(Main.IsekaiContext, "Most Protagonists only occasionally use kinetic blasts when they think they would look cool.\n But you really love them, and given that there is no other cantrip this overpowered why not?");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(AddProficiencies c)
				{
					c.WeaponProficiencies = new WeaponCategory[1] { WeaponCategory.KineticBlast };
				});
				bp.AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_Archetype = MartialGodArchetype.GetReference();
				});
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				prog = PatchTools.PatchClassProgressionBasedOnRefClass(prog, ClassTools.Classes.KineticistClass);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.KineticistClass);
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.KineticistClass.ToReference<BlueprintCharacterClassReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = KineticDarkElementalistLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = KineticKnightLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = KineticOverwhelmingSoulLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = KineticPsychoLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
			}
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "KineticLegacy");
		}
	}
}
