using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class ShifterLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			if (ClassTools.Classes.ShifterClass != null)
			{
				prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ShifterLegacy", delegate(BlueprintProgression bp)
				{
					bp.SetName(Main.IsekaiContext, "Shifter Legacy - Primal Chimera");
					bp.SetDescription(Main.IsekaiContext, "While lesser shifters bound themselves to single beasts or druidic oaths, you awakened the dormant primal chimera within your soul, allowing you to morph seamlessly between aspects and wild shapes without natural constraints.");
					bp.GiveFeaturesForPreviousLevels = true;
				});
			}
		}

		public static void PatchProgression()
		{
			if (prog == null)
			{
				return;
			}
			if (ClassTools.Classes.ShifterClass == null)
			{
				Main.Log("Shifter Class not present. ShifterLegacy skipped.");
				return;
			}
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			prog = PatchTools.PatchClassProgressionBasedOnRefClass(prog, ClassTools.Classes.ShifterClass);
			PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.ShifterClass);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.ShifterClass.ToReference<BlueprintCharacterClassReference>();
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			DevourerLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			BlueprintProgression baseLeg = ShifterBaseLegacy.Get();
			if (baseLeg != null)
			{
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = baseLeg.ToReference<BlueprintFeatureReference>();
				});
			}
			BlueprintProgression baseEvil = ShifterBaseLegacy.GetEvilAlternate();
			if (baseEvil != null)
			{
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = baseEvil.ToReference<BlueprintFeatureReference>();
				});
			}
			BlueprintProgression dragonLeg = ShifterDragonLegacy.Get();
			if (dragonLeg != null)
			{
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = dragonLeg.ToReference<BlueprintFeatureReference>();
				});
			}
			BlueprintProgression griffonLeg = ShifterGriffonLegacy.Get();
			if (griffonLeg != null)
			{
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = griffonLeg.ToReference<BlueprintFeatureReference>();
				});
			}
			BlueprintProgression holyLeg = ShifterHolyLegacy.Get();
			if (holyLeg != null)
			{
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = holyLeg.ToReference<BlueprintFeatureReference>();
				});
			}
			BlueprintProgression stingerLeg = ShifterStingerLegacy.Get();
			if (stingerLeg != null)
			{
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = stingerLeg.ToReference<BlueprintFeatureReference>();
				});
			}
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ShifterLegacy");
		}
	}
}
