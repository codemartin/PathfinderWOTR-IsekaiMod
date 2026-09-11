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
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class SkaldSilverTongueLegacy
	{
		private static readonly string BaseArchetypeId = "5e63586bebd229649bdafe0fde4caaec";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "SkaldSilverTongueLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Skald Legacy - Silver Tongue");
				bp.SetDescription(Main.IsekaiContext, "You were a flatterer in your past life, and learned to hone your skills to a ridiculous level. \nNo Queen can resist you, and that's a real advantage in these dark times.");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Prohibit(prog);
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
			prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.SkaldClass, BaseArchetype, null);
			BlueprintCharacterClassReference skaldClass = ClassTools.ClassReferences.SkaldClass;
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(reference, skaldClass, BaseArchetype);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.SkaldClass.ToReference<BlueprintCharacterClassReference>();
			});
			PatchTools.PatchClassIntoFeatureOfReferenceClass(BlueprintTools.GetBlueprint<BlueprintBuff>("70b5a320c87e5f34191caea053a3a1b8"), reference, skaldClass);
			prog.AddPrerequisiteNoFeature(SkaldVoiceLegacy.Get());
			prog.AddPrerequisiteNoFeature(SkaldBaseLegacy.Get());
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "SkaldSilverTongueLegacy");
		}
	}
}
