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
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class KineticKnightLegacy
	{
		private static BlueprintArchetype BaseKnight = BlueprintTools.GetBlueprint<BlueprintArchetype>("7d61d9b2250260a45b18c5634524a8fb");

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "KineticKnightLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Kinetic Legacy - Kinetic Knight");
				bp.SetDescription(Main.IsekaiContext, "Just like all Kinetic Knights your sanity is somewhat questionable, after all you willingly choose to forego fighting at range and instead choose to use energy blasts as melee weapons...\nAnd why? Because the burning blade looked cool.");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(AddProficiencies c)
				{
					c.WeaponProficiencies = new WeaponCategory[1] { WeaponCategory.KineticBlast };
				});
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
			BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>("fb9fe27f13934807bcd62dfeec477758");
			LevelEntry[] additionalReference = null;
			if (ModSupport.IsExpandedElementEnabled && blueprint != null)
			{
				additionalReference = new LevelEntry[1] { Helpers.CreateLevelEntry(1, blueprint) };
			}
			prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.KineticistClass, BaseKnight, additionalReference);
			BlueprintCharacterClassReference kineticistClass = ClassTools.ClassReferences.KineticistClass;
			PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(IsekaiProtagonistClass.GetReference(), kineticistClass, BaseKnight);
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
				c.m_Feature = KineticLegacy.Get().ToReference<BlueprintFeatureReference>();
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

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "KineticKnightLegacy");
		}
	}
}
