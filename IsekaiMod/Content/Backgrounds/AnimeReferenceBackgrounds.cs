using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	public static class AnimeReferenceBackgrounds
	{
		private static bool Added;

		public static void Add()
		{
			if (Added)
			{
				return;
			}
			Added = true;
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundSpiritDetective", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Spirit Detective");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Appointed as a liaison between the living world and the spirit realm, you spent your past life solving supernatural disturbances and firing spiritual energy from your fingertips. You gain a +2 competence bonus on Perception and Lore (Religion) checks, a +1 sacred bonus on ranged touch attack rolls, and treat Perception as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillLoreReligion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundHollowedBladeVessel", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Hollowed Blade Vessel");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Housing an untamed inner spirit and carrying a blade that resonates with your soul's true name, your spiritual pressure commands respect. You gain a +1 competence bonus on attack rolls with swords, a +2 competence bonus on Intimidate checks, a +2 bonus on saving throws against fear, and treat Persuasion as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPersuasion;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillPersuasion;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundKiMartialProdigy", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Ki Martial Prodigy");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Having endured hundred-fold gravity chamber conditioning and mountain-shattering martial drills, your body is a fortress of raw life-force. You gain a +2 competence bonus on Athletics checks, a +2 competence bonus on Fortitude saving throws, a +5 ft bonus to base movement speed, and treat Athletics as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillAthletics;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillAthletics;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillAthletics;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundNenAuraSpecialist", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Nen Aura Specialist");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Awakening your micropores to harness personal life energy through Ten and Ren, an invisible protective shroud surrounds your body. You gain a +1 dodge bonus to AC, a +2 competence bonus on Perception checks, a +2 bonus on Will saving throws against mind-affecting effects, and treat Perception as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundSubstitutionShinobi", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Substitution Shinobi");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Trained from childhood in covert ninjutsu, chakra tree-walking, and instantaneous substitution techniques, you can vanish into a cloud of smoke and logs at a moment's notice. You gain a +2 competence bonus on Stealth and Mobility checks, a +2 bonus on Reflex saving throws, and treat Stealth and Mobility as class skills.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillMobility;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillStealth;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillMobility;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillStealth;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillMobility;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundGourmetHunter", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Gourmet Hunter");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Seeking out the ultimate ingredients across prehistoric continents and forbidden biomes, your cells have evolved to thrive on culinary delicacies. You gain +10 maximum hit points, a +2 competence bonus on Lore (Nature) checks, a +2 bonus on Fortitude saving throws against poison and disease, and treat Lore (Nature) as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillLoreNature;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillLoreNature;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillLoreNature;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundVRGuildMaster", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "VR Guild Master");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Having led a top-tier guild through hundreds of raid dungeon bosses, min-maxing hotkeys, and managing treasury logistics, full-dive VR instincts carry over flawlessly. You gain a +2 competence bonus on Use Magic Device and Knowledge (World) checks, a +2 bonus on Initiative checks, and treat Use Magic Device as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillUseMagicDevice;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillUseMagicDevice;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillUseMagicDevice;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundCardDuelistKing", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Card Duelist King");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Believing unyieldingly in the Heart of the Cards, you have stood at the apex of tabletop tournaments and sealed ancient shadow games. You gain a +2 competence bonus on Persuasion checks, a +2 luck bonus on all saving throws against fear and despair, and treat Persuasion as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPersuasion;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillPersuasion;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundTimeLeapingMadScientist", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Time-Leaping Mad Scientist");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Operating out of a cramped laboratory with a modified microwave and a passionate laugh, your Reading Steiner ability retains memories across divergent worldlines. You gain a +2 competence bonus on Knowledge (World) checks, a +2 bonus on saving throws against confusion and mind-affecting effects, a +2 bonus on Initiative, and treat Knowledge (World) as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeWorld;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeWorld;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundShrunkHighSchoolSleuth", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shrunk High School Sleuth");
				bp.SetBackgroundDescription(Main.IsekaiContext, "There is always only one truth! Despite mysterious toxins compressing your mortal body, your analytical intellect dissects crime scenes and ambush trajectories instantly. You gain a +3 competence bonus on Perception checks, cannot be caught flat-footed at the start of combat, and treat Perception as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundUndergroundGambler", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Underground Gambler");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Pushed to the precipice of ruin in steel-beam crossings and high-stakes pachinko parlors, your instincts shine brightest in life-or-death desperation. You gain a +2 competence bonus on Trickery checks, a +2 luck bonus on saving throws against mind-affecting effects, and treat Trickery as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillThievery;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillThievery;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillThievery;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundStateAlchemist", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "State Alchemist");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Understanding the sacred laws of Equivalent Exchange and having peered beyond the Gate of Truth, you manipulate the molecular composition of the world with ease. You gain a +2 competence bonus on Knowledge (Arcana) checks, a +1 bonus to the DC of all Transmutation spells, and treat Knowledge (Arcana) as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeArcana;
					c.Value = 2;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Transmutation;
					c.BonusDC = 1;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeArcana;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeArcana;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundLimitBreakerTrainee", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Limit Breaker Trainee");
				bp.SetBackgroundDescription(Main.IsekaiContext, "100 pushups, 100 situps, 100 squats, and a 10-kilometer run every single day, with no air conditioning in summer or heating in winter! You gain a +2 competence bonus on Athletics checks, a +2 competence bonus on Fortitude saving throws, a +1 natural armor bonus to AC, and treat Athletics as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillAthletics;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillAthletics;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillAthletics;
				});
			}));
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundReincarnatedMobCharacter", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Reincarnated Mob Character");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Convinced you were merely a nameless background extra, you practiced walking unnoticed in the shadows while letting dramatic events unfold around you. You gain a +2 competence bonus on Stealth checks, a +1 luck bonus on all saving throws, and treat Stealth as a class skill.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillStealth;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillStealth;
				});
			}));
		}
	}
}
