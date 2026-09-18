using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	// Restored from the pre-6.2 fork. The 6.2 rewrite dropped this legacy; it keeps its original GUID
	// so characters that took it before still resolve it.
	internal class PlayerPartTimeWorkerLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "PlayerPartTimeWorkerLegacy", delegate(BlueprintProgression bp)
			{
				BlueprintFeatureSelection legacy = LegacySelection.GetClassFeature();
				bp.SetName(Main.IsekaiContext, "Player Legacy - Part Timer");
				bp.SetDescription(Main.IsekaiContext, "You were a part time worker bouncing from job to job.\nYou know how to learn the basics of a new job, and ultimately all these classes are just a different type of job.\nSure, picking up the next one takes a while, but ultimately you will be the master of all.\nYou gain an additional Legacy selection at levels 5, 10 and 15.");
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
				bp.LevelEntries = new LevelEntry[3]
				{
					Helpers.CreateLevelEntry(5, legacy),
					Helpers.CreateLevelEntry(10, legacy),
					Helpers.CreateLevelEntry(15, legacy)
				};
				bp.UIGroups = new UIGroup[1] { Helpers.CreateUIGroup(legacy) };
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "PlayerPartTimeWorkerLegacy");
		}
	}
}
