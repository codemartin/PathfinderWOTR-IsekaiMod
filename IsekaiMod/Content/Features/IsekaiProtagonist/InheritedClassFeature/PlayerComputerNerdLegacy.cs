using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class PlayerComputerNerdLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "PlayerComputerNerdLegacy", delegate(BlueprintProgression bp)
			{
				BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>("cad1b9175e8c0e64583432a22134d33c");
				bp.SetName(Main.IsekaiContext, "Player Legacy - Computer Nerd");
				bp.SetDescription(Main.IsekaiContext, "You were a computer nerd in your last life, you get a +10 Bonus to Knowledge(Programming) and +5 to your MMORPG Skill.\nWait what do you mean there are no computers in this world and all that knowledge and skill is useless?\nWell at least some of the theoretical knowledge about magic is applicable in this world...");
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
				bp.LevelEntries = new LevelEntry[1] { Helpers.CreateLevelEntry(1, blueprint) };
				bp.UIGroups = new UIGroup[1] { Helpers.CreateUIGroup(blueprint) };
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
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "PlayerComputerNerdLegacy");
		}
	}
}
