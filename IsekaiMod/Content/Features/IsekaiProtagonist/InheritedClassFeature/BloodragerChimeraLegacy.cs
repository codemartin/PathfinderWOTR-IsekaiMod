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
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class BloodragerChimeraLegacy
	{
		private static BlueprintProgression prog;

		private static BlueprintFeatureSelection bloodlines;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "BloodragerChimeraLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Bloodrager Legacy - Chimeric Rager");
				bp.SetDescription(Main.IsekaiContext, "Much like the Chimera you draw upon the power of inhuman bloodlines and learned how to slowly either awaken or fuse more of them into yourself. \nHowever, you would rather use that to empower your melee attacks rather than your spells.\nAfter all, what is the point of draconic claws or a phoenixes burning wings if you only use them as a fallback?");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			bloodlines = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiBloodragerSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(((BlueprintUnitFact)FeatTools.Selections.BloodragerBloodlineSelection).m_DisplayName);
				bp.SetDescription(((BlueprintUnitFact)FeatTools.Selections.BloodragerBloodlineSelection).m_Description);
				bp.Ranks = 4;
				bp.IgnorePrerequisites = true;
				bp.IsClassFeature = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			HeroLegacySelection.Prohibit(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				LevelEntry[] array = new LevelEntry[0];
				LevelEntry[] array2 = new LevelEntry[0];
				array2 = array2.AppendToArray(Helpers.CreateLevelEntry(1, FeatTools.Selections.BloodragerBloodlineSelection));
				bloodlines.SetFeatures(FeatTools.Selections.BloodragerBloodlineSelection.m_AllFeatures);
				array = array.AppendToArray(Helpers.CreateLevelEntry(1, bloodlines));
				array = array.AppendToArray(Helpers.CreateLevelEntry(5, bloodlines));
				array = array.AppendToArray(Helpers.CreateLevelEntry(10, bloodlines));
				array = array.AppendToArray(Helpers.CreateLevelEntry(15, bloodlines));
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.BloodragerClass, array, array2);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.BloodragerClass);
				PatchBloodragerResistanceBuffs(reference);
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.BloodragerClass.ToReference<BlueprintCharacterClassReference>();
				});
			}
		}

		// Bloodline resistance features apply separate buffs during Bloodrage. Those buffs are not
		// children of the bloodline progressions, so the inherited-feature traversal never adds the
		// Isekai class to their Bloodrager-only ContextRankConfig and they showed resistance 0.
		private static void PatchBloodragerResistanceBuffs(BlueprintCharacterClassReference myClass)
		{
			string[] resistanceBuffGuids = new string[14]
			{
				"982a14da9e8a4714a739e45111a1ae5a", // BloodragerAberrantResistanceBuff
				"f2ad59f72a9544738a20dfb4e3d34ddd", // BloodragerAbyssalResistanceBuff
				"f1ceaeea9311e0f4884f069ebfa00b3b", // BloodragerCelestialResistancesBuff
				"89ac2964ef12ddb468d5c3ade461eb68", // BloodragerDraconicResistanceACBuff
				"2ac2345547bf3674e931d2b87ebc555d", // BloodragerDraconicResistanceBuffAcid
				"87282f7914eea69498fa8513108cd573", // BloodragerDraconicResistanceBuffCold
				"d70a60fa4ae9b3245b86da2ef2b618d1", // BloodragerDraconicResistanceBuffElectricity
				"54d7b792a931f20459bb983a063aa534", // BloodragerDraconicResistanceBuffFire
				"641d5bdb776d58548ae889102a2d330c", // BloodragerElementalAcidResistanceBuff
				"f8a35d21218171f43ab36deaf4c5c441", // BloodragerElementalColdResistanceBuff
				"2892fe41538768141b5838e4a9fecfa0", // BloodragerElementalElectricityResistanceBuff
				"bd823e94454ab7f4599f846ca4a3bec1", // BloodragerElementalFireResistanceBuff
				"66c973ccad1293345ae6c5bfeced4a7b", // BloodragerInfernalResistanceBuff
				"ef2d9784fcd404d4f9bd97840541b2e5"  // BloodragerSerpentineResistanesBuff
			};
			foreach (string buffGuid in resistanceBuffGuids)
			{
				BlueprintBuff resistanceBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(buffGuid);
				if (resistanceBuff != null)
				{
					PatchTools.PatchClassIntoFeatureOfReferenceClass(resistanceBuff, myClass, ClassTools.ClassReferences.BloodragerClass);
				}
			}
		}

		public static void PatchPrerequisiteCompatibility()
		{
			BlueprintFeatureSelection secondBloodline = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b7f62628915bdb14d8888c25da3fac56");
			PrerequisiteAlternatives.Add(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19"), FeatTools.Selections.BloodragerBloodlineSelection, bloodlines);
			PrerequisiteAlternatives.Add(secondBloodline, FeatTools.Selections.BloodragerBloodlineSelection, bloodlines);
			PrerequisiteAlternatives.RequireUnownedChoices(secondBloodline);
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "BloodragerChimeraLegacy");
		}
	}
}
