using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;

internal class TranscendentSovereignArchetypes
{
	public static void Add()
	{
		BlueprintFeature TranscendentDomainFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TranscendentDomainFeature");
		BlueprintFeature SovereignOathFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SovereignOathFeature");
		BlueprintFeature HeavenlyImperialMandateFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeavenlyImperialMandateFeature");
		BlueprintFeature GodEmperorProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies");
		TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "CelestialMonarchArchetype", delegate(BlueprintArchetype bp)
		{
			bp.SetName(Main.IsekaiContext, "Celestial Monarch");
			bp.SetDescription(Main.IsekaiContext, "The God Emperor's oath. The domain becomes heaven's own mandate and the crown becomes a throne. Heavenly Imperial Mandate replaces Transcendent Domain at 5th level: allies within 40 feet gain +6 to attack rolls, AC and saving throws. Heavenly Throne replaces Crown of Stories at 9th level: +4 Charisma and +2 to your spell save DCs. Only a God Emperor can swear it; everything else is the Transcendent Sovereign's.");
			bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
			bp.AddFeatures = new LevelEntry[2]
			{
				Helpers.CreateLevelEntry(1, SovereignOathFeature),
				Helpers.CreateLevelEntry(5, HeavenlyImperialMandateFeature)
			};
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
			bp.SetDescription(Main.IsekaiContext, "The Slime's oath. The domain is a stomach, and whatever it takes in, the sovereign keeps. Gluttonous Singularity replaces Transcendent Domain at 5th level: an acid vortex deals 4d6 acid damage each round to enemies within 30 feet and lowers their AC and spell resistance by 4. Endless Appetite replaces Crown of Stories at 9th level: fast healing 10 and damage reduction 10/-. Only a Slime can swear it; everything else is the Transcendent Sovereign's.");
			bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
			bp.AddFeatures = new LevelEntry[2]
			{
				Helpers.CreateLevelEntry(1, SovereignOathFeature),
				Helpers.CreateLevelEntry(5, GluttonousSingularityFeature)
			};
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
			bp.SetDescription(Main.IsekaiContext, "The Shadow Monarch's oath. The domain is the dark the army marches in, and the crown is the army itself. Absolute Shadow Domain replaces Transcendent Domain at 5th level: allies within 40 feet gain total concealment and an extra attack. Sovereign of the Night Legion replaces Crown of Stories at 9th level: every creature you summon gains +4 to attack rolls and AC, and attacks against you have a 20% miss chance. Only a Shadow Monarch can swear it; everything else is the Transcendent Sovereign's.");
			bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
			bp.AddFeatures = new LevelEntry[2]
			{
				Helpers.CreateLevelEntry(1, SovereignOathFeature),
				Helpers.CreateLevelEntry(5, AbsoluteShadowDomainFeature)
			};
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
			bp.SetDescription(Main.IsekaiContext, "The Mastermind's oath. Everything is going according to plan, and the plan has a seat for every ally. All According to Plan replaces Transcendent Domain at 5th level: allies within the aura gain +4 insight to attack rolls and saving throws and +20 to confirm critical hits. Checkmate Foresight replaces Crown of Stories at 9th level: +4 initiative, you cannot be flanked, and +2 to your spell save DCs. Only a Mastermind can swear it; everything else is the Transcendent Sovereign's.");
			bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
			bp.AddFeatures = new LevelEntry[2]
			{
				Helpers.CreateLevelEntry(1, SovereignOathFeature),
				Helpers.CreateLevelEntry(5, AllAccordingToPlanFeature)
			};
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
			bp.SetDescription(Main.IsekaiContext, "The Overlord's oath. The domain is the Great Tomb's throne room, and death has already had its turn. Tomb of the Supreme Being replaces Transcendent Domain at 5th level: allies within the aura gain +6 Strength and Charisma, fast healing 10 and immunity to negative levels. Dominion Over Death replaces Crown of Stories at 9th level: immunity to death effects and negative levels, and +2 to your spell save DCs. Only an Overlord can swear it; everything else is the Transcendent Sovereign's.");
			bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
			bp.AddFeatures = new LevelEntry[2]
			{
				Helpers.CreateLevelEntry(1, SovereignOathFeature),
				Helpers.CreateLevelEntry(5, TombOfTheSupremeBeingFeature)
			};
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
			bp.SetDescription(Main.IsekaiContext, "The Hero's oath. The domain holds the line, and the Hero never stays down. Miracle of Unyielding Hope replaces Transcendent Domain at 5th level: allies within the aura gain damage reduction 10/-, 50% fortification and heal 20 hit points each round. Hope Undying replaces Crown of Stories at 9th level: immunity to fear, +2 to all saving throws and 50% fortification. Only a Hero can swear it; everything else is the Transcendent Sovereign's.");
			bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
			bp.AddFeatures = new LevelEntry[2]
			{
				Helpers.CreateLevelEntry(1, SovereignOathFeature),
				Helpers.CreateLevelEntry(5, MiracleOfUnyieldingHopeFeature)
			};
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
		BlueprintFeature PhantomBladeProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PhantomBladeProficiencies");
		TranscendentSovereignClass.RegisterArchetype(Helpers.CreateBlueprint(Main.IsekaiContext, "AstralSwordmasterArchetype", delegate(BlueprintArchetype bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Sovereign");
			bp.SetDescription(Main.IsekaiContext, "The Martial God's oath. The domain is a storm of phantom blades, and the sovereign never swings alone. Infinite Blade Symphony replaces Transcendent Domain at 5th level: orbiting phantom blades deal 6d6 force damage each round to enemies within 25 feet. Ten Thousand Blades replaces Crown of Stories at 9th level: +2d6 force damage on weapon attacks and +10 to confirm critical hits. Only a Martial God can swear it; everything else is the Transcendent Sovereign's.");
			bp.RemoveFeatures = new LevelEntry[1] { Helpers.CreateLevelEntry(5, TranscendentDomainFeature) };
			bp.AddFeatures = new LevelEntry[2]
			{
				Helpers.CreateLevelEntry(1, SovereignOathFeature),
				Helpers.CreateLevelEntry(5, InfiniteBladeSymphonyFeature)
			};
			if (PhantomBladeProficiencies != null)
			{
				bp.AddComponent(delegate(PrerequisiteFeature c)
				{
					c.m_Feature = PhantomBladeProficiencies.ToReference<BlueprintFeatureReference>();
					c.Group = Prerequisite.GroupType.All;
				});
			}
		}));
		AddArchetypeApexes();
	}

	private static void AddArchetypeApexes()
	{
		BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SovereignCrownFeature");
		string[][] sovereignArchetypes = PrestigeLevelPlan.SovereignArchetypes;
		foreach (string[] array in sovereignArchetypes)
		{
			BlueprintArchetype modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, array[0]);
			BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, array[3]);
			if (modBlueprint2 == null || modBlueprint3 == null || modBlueprint == null)
			{
				Main.IsekaiContext.Logger.LogError("[TranscendentSovereignArchetypes] Could not give '" + array[0] + "' its level 9 apex '" + array[3] + "'.");
			}
			else
			{
				modBlueprint2.RemoveFeatures = modBlueprint2.RemoveFeatures.AppendToArray(Helpers.CreateLevelEntry(9, modBlueprint));
				modBlueprint2.AddFeatures = modBlueprint2.AddFeatures.AppendToArray(Helpers.CreateLevelEntry(9, modBlueprint3));
			}
		}
	}
}
