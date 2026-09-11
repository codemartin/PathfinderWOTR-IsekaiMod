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
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class SorcererLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			ExtraBloodlineSelection.Configure();
			BlueprintFeatureSelection ExtraSelection = ExtraBloodlineSelection.Get();
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "SorcererLegacyProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Sorcerer Legacy - Chimera");
				bp.SetDescription(Main.IsekaiContext, "Their otherworldly knowledge and point of view allow Chimeras to imbue themselves with different bloodlines in order to gain power and strength. \nChimeras are constantly seeking out new sources of power, and their ability to absorb and incorporate these different bloodlines allows them to become truly formidable foes. \nHowever, their constant experimentation with bloodlines can also lead to confusion and uncertainty about their own heritage and identity, as their original ancestry becomes harder to discern over time.\n\nPlease note that you do not get previous Bloodline Evolutions when picking this legacy at higher levels.");
				bp.GiveFeaturesForPreviousLevels = false;
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = IsekaiProtagonistClass.GetReference(),
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[7]
				{
					Helpers.CreateLevelEntry(1, ExtraSelection),
					Helpers.CreateLevelEntry(3, ExtraSelection),
					Helpers.CreateLevelEntry(6, ExtraSelection),
					Helpers.CreateLevelEntry(9, ExtraSelection),
					Helpers.CreateLevelEntry(12, ExtraSelection),
					Helpers.CreateLevelEntry(15, ExtraSelection),
					Helpers.CreateLevelEntry(18, ExtraSelection)
				};
				bp.UIGroups = new UIGroup[1] { Helpers.CreateUIGroup(ExtraSelection) };
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
			BlueprintCharacterClassReference referenceClass = ClassTools.Classes.SorcererClass.ToReference<BlueprintCharacterClassReference>();
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			PatchTools.PatchClassIntoFeatureOfReferenceClass(StaticReferences.SorcererBloodlineSelection, reference, referenceClass);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.SorcererClass.ToReference<BlueprintCharacterClassReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "SorcererLegacyProgression");
		}
	}
}
