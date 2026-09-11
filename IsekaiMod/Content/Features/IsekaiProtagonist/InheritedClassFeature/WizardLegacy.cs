using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Utilities;
using System.Linq;
using Kingmaker.Blueprints.Classes.Selection;
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
				ReplaceSchoolSelection();
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.WizardClass.ToReference<BlueprintCharacterClassReference>();
				});
			}
		}

		// The base-game Specialist School selection (FeatureGroup.SpecialistSchool) cannot be
		// picked from inside the legacy progression, so swap in an Isekai wrapper the same way the
		// Sorcerer legacy wraps the bloodline selection.
		private static void ReplaceSchoolSelection()
		{
			BlueprintFeatureSelection school = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("5f838049069f1ac4d804ce0862ab5110");
			if (school == null || prog == null)
			{
				return;
			}
			BlueprintFeatureSelection wrapper = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiWizardSchoolSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.m_DisplayName = school.m_DisplayName;
				bp.m_Description = school.m_Description;
				bp.m_DescriptionShort = school.m_DescriptionShort;
				bp.m_Icon = school.m_Icon;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = true;
				bp.Group = FeatureGroup.None;
				bp.m_AllFeatures = (school.m_AllFeatures ?? new BlueprintFeatureReference[0]).ToArray();
				bp.m_Features = bp.m_AllFeatures;
			});
			MirroredSelections.Register(wrapper, school);
			BlueprintFeatureBaseReference wrapperRef = wrapper.ToReference<BlueprintFeatureBaseReference>();
			int replaced = 0;
			foreach (LevelEntry entry in prog.LevelEntries ?? new LevelEntry[0])
			{
				if (entry?.m_Features == null)
				{
					continue;
				}
				for (int i = 0; i < entry.m_Features.Count; i++)
				{
					if (entry.m_Features[i] != null && entry.m_Features[i].Guid == school.AssetGuid)
					{
						entry.m_Features[i] = wrapperRef;
						replaced++;
					}
				}
			}
			foreach (UIGroup group in prog.UIGroups ?? new UIGroup[0])
			{
				if (group?.m_Features == null)
				{
					continue;
				}
				for (int i = 0; i < group.m_Features.Count; i++)
				{
					if (group.m_Features[i] != null && group.m_Features[i].Guid == school.AssetGuid)
					{
						group.m_Features[i] = wrapperRef;
					}
				}
			}
			Main.IsekaiContext.Logger.Log("WizardLegacy: replaced " + replaced + " Specialist School selection entries with IsekaiWizardSchoolSelection");
		}
	}
}
