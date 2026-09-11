using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class KineticDarkElementalistLegacy
	{
		private static string BaseArchetypeId = "f12f18ae8842425489d29f302e69134c";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "KineticDarkElementalistLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Kinetic Legacy - Soulbinder");
				bp.SetDescription(Main.IsekaiContext, "You have discovered a dark secret of elemental manipulation: you can use the souls of your enemies as fuel for your powers. \nInstead of straining your own body, you can use souls of those who oppose you and unleash devastating blasts of elemental energy. \nYou are a master of soulbinding, a technique that some consider to be unnatural and morbid. \nBut you don't care about such moral qualms; you only care about your own survival and dominance.");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(AddProficiencies c)
				{
					c.WeaponProficiencies = new WeaponCategory[1] { WeaponCategory.KineticBlast };
				});
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.Evil;
				});
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			HeroLegacySelection.Prohibit(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			if (prog == null)
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
			BlueprintCharacterClassReference kineticistClass = ClassTools.ClassReferences.KineticistClass;
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.KineticistClass, BaseArchetype, null);
			PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(reference, kineticistClass, BaseArchetype);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.KineticistClass.ToReference<BlueprintCharacterClassReference>();
			});
			prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = KineticKnightLegacy.Get().ToReference<BlueprintFeatureReference>();
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
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "KineticDarkElementalistLegacy");
		}
	}
}
