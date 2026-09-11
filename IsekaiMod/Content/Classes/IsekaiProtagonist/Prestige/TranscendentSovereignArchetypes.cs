using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige
{
	internal class TranscendentSovereignArchetypes
	{
		public static void Add()
		{
			BlueprintFeature TranscendentDomainFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TranscendentDomainFeature");
			BlueprintFeature HeavenlyImperialMandateFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeavenlyImperialMandateFeature");
			BlueprintFeature GodEmperorProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies");
			TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "CelestialMonarchArchetype", delegate(BlueprintArchetype bp)
			{
				bp.SetName(Main.IsekaiContext, "Celestial Monarch");
				bp.SetDescription(Main.IsekaiContext, "A Transcendent Sovereign aligned with divine rulership. Their sovereign presence radiates celestial mandate, elevating allies with sacred bonuses.");
				bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
				bp.AddFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, HeavenlyImperialMandateFeature) };
				bp.m_ReplaceSpellbook = GodEmperorSpellbook.GetReference();
				if (GodEmperorProficiencies != null)
				{
					bp.AddComponent(delegate(PrerequisiteFeature c)
					{
						c.m_Feature = GodEmperorProficiencies.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
			}));
			BlueprintFeature GluttonousSingularityFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GluttonousSingularityFeature");
			BlueprintFeature DevourerProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies");
			TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "DemonSlimeSovereignArchetype", delegate(BlueprintArchetype bp)
			{
				bp.SetName(Main.IsekaiContext, "Demon Slime Sovereign");
				bp.SetDescription(Main.IsekaiContext, "A Transcendent Sovereign born from endless consumption. Their domain manifests as an acidic singularity melting enemy defenses.");
				bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
				bp.AddFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, GluttonousSingularityFeature) };
				if (DevourerProficiencies != null)
				{
					bp.AddComponent(delegate(PrerequisiteFeature c)
					{
						c.m_Feature = DevourerProficiencies.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
			}));
			BlueprintFeature AbsoluteShadowDomainFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AbsoluteShadowDomainFeature");
			BlueprintFeature ShadowMonarchProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies");
			TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowSovereignArchetype", delegate(BlueprintArchetype bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Sovereign");
				bp.SetDescription(Main.IsekaiContext, "A Transcendent Sovereign commanding the endless legions of death. Their domain cloaks the battlefield in total shadow and empowering frost.");
				bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
				bp.AddFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, AbsoluteShadowDomainFeature) };
				bp.m_ReplaceSpellbook = ShadowMonarchSpellbook.GetReference();
				if (ShadowMonarchProficiencies != null)
				{
					bp.AddComponent(delegate(PrerequisiteFeature c)
					{
						c.m_Feature = ShadowMonarchProficiencies.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
			}));
			BlueprintFeature AllAccordingToPlanFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "AllAccordingToPlanFeature");
			BlueprintFeature MastermindProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindProficiencies");
			TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "GrandStrategistArchetype", delegate(BlueprintArchetype bp)
			{
				bp.SetName(Main.IsekaiContext, "Grand Strategist of Fate");
				bp.SetDescription(Main.IsekaiContext, "A Transcendent Sovereign whose mind maps every cosmic variable. Their domain ensures every strike is critical and perfectly timed.");
				bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
				bp.AddFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, AllAccordingToPlanFeature) };
				bp.m_ReplaceSpellbook = MastermindSpellbook.GetReference();
				if (MastermindProficiencies != null)
				{
					bp.AddComponent(delegate(PrerequisiteFeature c)
					{
						c.m_Feature = MastermindProficiencies.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
			}));
			BlueprintFeature TombOfTheSupremeBeingFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TombOfTheSupremeBeingFeature");
			BlueprintFeature OverlordProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies");
			TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "SupremeRulerArchetype", delegate(BlueprintArchetype bp)
			{
				bp.SetName(Main.IsekaiContext, "Supreme Overlord");
				bp.SetDescription(Main.IsekaiContext, "A Transcendent Sovereign embodying the ultimate tier of necromantic authority and dominion. Their sovereign presence grants profane might and rapid recovery to their servants.");
				bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
				bp.AddFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TombOfTheSupremeBeingFeature) };
				bp.m_ReplaceSpellbook = OverlordSpellbook.GetReference();
				if (OverlordProficiencies != null)
				{
					bp.AddComponent(delegate(PrerequisiteFeature c)
					{
						c.m_Feature = OverlordProficiencies.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
			}));
			BlueprintFeature MiracleOfUnyieldingHopeFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MiracleOfUnyieldingHopeFeature");
			BlueprintFeature HeroProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroProficiencies");
			TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "ParagonOfHopeArchetype", delegate(BlueprintArchetype bp)
			{
				bp.SetName(Main.IsekaiContext, "Paragon of Hope");
				bp.SetDescription(Main.IsekaiContext, "A Transcendent Sovereign who will never surrender. Their domain shields allies with unyielding physical fortitude and continuous restoration.");
				bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
				bp.AddFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, MiracleOfUnyieldingHopeFeature) };
				if (HeroProficiencies != null)
				{
					bp.AddComponent(delegate(PrerequisiteFeature c)
					{
						c.m_Feature = HeroProficiencies.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
			}));
			BlueprintFeature InfiniteBladeSymphonyFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "InfiniteBladeSymphonyFeature");
			BlueprintFeature MartialGodProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialGodProficiencies");
			TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "AstralSwordmasterArchetype", delegate(BlueprintArchetype bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Sovereign");
				bp.SetDescription(Main.IsekaiContext, "A Transcendent Sovereign wielding the absolute authority of infinite martial strikes. Their sovereign domain slashes all surrounding enemies with unrelenting force.");
				bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
				bp.AddFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, InfiniteBladeSymphonyFeature) };
				if (MartialGodProficiencies != null)
				{
					bp.AddComponent(delegate(PrerequisiteFeature c)
					{
						c.m_Feature = MartialGodProficiencies.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
					});
				}
			}));
		}
	}
}
