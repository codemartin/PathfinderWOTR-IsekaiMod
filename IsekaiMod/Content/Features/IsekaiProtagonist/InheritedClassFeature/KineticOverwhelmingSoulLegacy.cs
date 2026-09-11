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
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class KineticOverwhelmingSoulLegacy
	{
		private static string BaseArchetypeId = "aa11888104d17f7459851e8d559ffa98";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "KineticOverwhelmingSoulLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Kinetic Legacy - Noble Soul");
				bp.SetDescription(Main.IsekaiContext, "Your soul is stronger than most, for you hail from another world.\nYou command the elements with ease and grace, while others struggle and suffer.\nYou are a noble soul, a revered and feared master of elemental power.");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(AddProficiencies c)
				{
					c.WeaponProficiencies = new WeaponCategory[1] { WeaponCategory.KineticBlast };
				});
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
			prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.KineticistClass, BaseArchetype, null);
			BlueprintCharacterClassReference kineticistClass = ClassTools.ClassReferences.KineticistClass;
			PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(IsekaiProtagonistClass.GetReference(), kineticistClass, BaseArchetype);
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
				c.m_Feature = KineticLegacy.Get().ToReference<BlueprintFeatureReference>();
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
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "KineticOverwhelmingSoulLegacy");
		}
	}
}
