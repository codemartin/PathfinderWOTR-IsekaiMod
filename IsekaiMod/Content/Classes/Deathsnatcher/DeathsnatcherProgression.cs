using System.Collections.Generic;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.Deathsnatcher
{
	internal class DeathsnatcherProgression
	{
		private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

		public static void Add()
		{
			BlueprintFeature Pounce = BlueprintTools.GetBlueprint<BlueprintFeature>("1a8149c09e0bdfc48a305ee6ac3729a8");
			BlueprintFeature DeathsnatcherSoulRendFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c8b468508a76c5140a9a2af00077753d");
			BlueprintFeature Evasion = BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");
			BlueprintFeature ImprovedEvasion = BlueprintTools.GetBlueprint<BlueprintFeature>("ce96af454a6137d47b9c6a1e02e66803");
			BlueprintFeature DeathsnatcherPoisonSting = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherPoisonSting");
			BlueprintFeature DeathsnatcherResistances = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherResistances");
			BlueprintFeature DeathsnatcherFastHealing = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherFastHealing");
			BlueprintFeature DeathsnatcherSizeBabyFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherSizeBabyFeature");
			BlueprintFeature DeathsnatcherCommandUndeadFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherCommandUndeadFeature");
			BlueprintFeature DeathsnatcherAnimateDeadFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherAnimateDeadFeature");
			BlueprintFeature DeathsnatcherCreateUndeadFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherCreateUndeadFeature");
			BlueprintFeature DeathsnatcherFingerOfDeathFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherFingerOfDeathFeature");
			BlueprintFeature DeathsnatcherAnimateDeadAdditionalUse = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherAnimateDeadAdditionalUse");
			BlueprintFeature DeathsnatcherUndeadMaster = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherUndeadMaster");
			IsekaiPetProgression.GetCompanionProgression();
			DeathsnatcherClass.SetProgression(Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherClassProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(StaticReferences.Strings.Null);
				bp.SetDescription(Main.IsekaiContext, "This bipedal jackal has vulture wings and a rat tail ending in a scorpion's stinger. Each of its four arms ends in a clawed hand.");
				bp.IsClassFeature = true;
				bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
				bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
				bp.m_AlternateProgressionClasses = new BlueprintProgression.ClassWithLevel[0];
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = DeathsnatcherClass.GetReference(),
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[10]
				{
					Helpers.CreateLevelEntry(1, DeathsnatcherResistances, DeathsnatcherCommandUndeadFeature, DeathsnatcherSizeBabyFeature),
					Helpers.CreateLevelEntry(2, Evasion),
					Helpers.CreateLevelEntry(4, Pounce),
					Helpers.CreateLevelEntry(7, DeathsnatcherAnimateDeadFeature),
					Helpers.CreateLevelEntry(10, DeathsnatcherAnimateDeadAdditionalUse, DeathsnatcherPoisonSting),
					Helpers.CreateLevelEntry(13, DeathsnatcherCreateUndeadFeature),
					Helpers.CreateLevelEntry(15, DeathsnatcherSoulRendFeature, ImprovedEvasion),
					Helpers.CreateLevelEntry(16, DeathsnatcherFingerOfDeathFeature),
					Helpers.CreateLevelEntry(18, DeathsnatcherFastHealing),
					Helpers.CreateLevelEntry(20, DeathsnatcherUndeadMaster)
				};
				bp.UIGroups = new UIGroup[2]
				{
					Helpers.CreateUIGroup(DeathsnatcherCommandUndeadFeature, DeathsnatcherAnimateDeadFeature, DeathsnatcherAnimateDeadAdditionalUse, DeathsnatcherCreateUndeadFeature, DeathsnatcherFingerOfDeathFeature, DeathsnatcherUndeadMaster),
					Helpers.CreateUIGroup(DeathsnatcherSizeBabyFeature, Pounce, DeathsnatcherPoisonSting, DeathsnatcherSoulRendFeature, DeathsnatcherFastHealing)
				};
				bp.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[1] { DeathsnatcherResistances.ToReference<BlueprintFeatureBaseReference>() };
			}));
		}

		public static BlueprintProgression GetCompanionProgression()
		{
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "DeathsnatcherCompanionProgression");
		}
	}
}
