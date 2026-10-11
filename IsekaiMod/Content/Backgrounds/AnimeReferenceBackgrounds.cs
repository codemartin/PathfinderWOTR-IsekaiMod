using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Appointed as a liaison between the living world and the spirit realm, you spent your past life solving supernatural disturbances and firing spiritual energy from your fingertips. Spirit Gun: you gain a +2 sacred bonus on ranged touch attack rolls. You also gain a +4 competence bonus on Initiative, a +2 competence bonus on Will saves, a +2 bonus on saving throws against death effects, a +2 competence bonus on Perception and Lore (Religion) checks, and treat Perception and Lore (Religion) as class skills.");
			bp.AddComponent(delegate(AttackTypeAttackBonus c)
			{
				c.Type = WeaponRangeType.RangedTouch;
				c.AttackBonus = 1;
				c.Value = 2;
				c.Descriptor = ModifierDescriptor.Sacred;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.Death;
				c.ModifierDescriptor = ModifierDescriptor.Competence;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
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
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillLoreReligion;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillLoreReligion;
			});
		}));
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundHollowedBladeVessel", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Hollowed Blade Vessel");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Housing an untamed inner spirit and carrying a blade that resonates with your soul's true name, your spiritual pressure commands respect. Soul Blade: you gain a +2 competence bonus on attack rolls with swords (light and heavy blades). You also gain +10 maximum hit points, a +2 competence bonus on Will and Fortitude saves and to Combat Maneuver Bonus, a +2 bonus on saving throws against fear, a +2 competence bonus on Persuasion checks, and treat Persuasion as a class skill.");
			bp.AddComponent(delegate(WeaponGroupAttackBonus c)
			{
				c.WeaponGroup = WeaponFighterGroup.BladesLight;
				c.AttackBonus = 2;
				c.Descriptor = ModifierDescriptor.Competence;
			});
			bp.AddComponent(delegate(WeaponGroupAttackBonus c)
			{
				c.WeaponGroup = WeaponFighterGroup.BladesHeavy;
				c.AttackBonus = 2;
				c.Descriptor = ModifierDescriptor.Competence;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.Fear;
				c.ModifierDescriptor = ModifierDescriptor.Morale;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
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
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPersuasion;
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Having endured hundred-fold gravity chamber conditioning and mountain-shattering martial drills, your body is a fortress of raw life-force. Gravity Training: you are immune to being staggered or slowed. You also gain +20 maximum hit points, a +4 competence bonus to Combat Maneuver Bonus, a +2 competence bonus on Fortitude and Reflex saves, a +5 ft bonus to base movement speed, a +2 competence bonus on Athletics checks, and treat Athletics as a class skill.");
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Staggered;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Slowed;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Staggered;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Staggered;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 20;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Speed;
				c.Value = 5;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillAthletics;
				c.Value = 2;
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Awakening your micropores to harness personal life energy through Ten and Ren, an invisible protective shroud surrounds your body. Ten: your aura gives you a +2 deflection bonus to AC. You also gain a +1 dodge bonus to AC, a +2 competence bonus on Will saves, a +2 bonus on saving throws against mind-affecting effects, a +2 competence bonus on Perception checks, and treat Perception as a class skill.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Deflection;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPerception;
				c.Value = 2;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.MindAffecting;
				c.ModifierDescriptor = ModifierDescriptor.Insight;
				c.Value = 2;
				c.Bonus = 0;
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Trained from childhood in covert ninjutsu, chakra tree-walking, and instantaneous substitution techniques, you can vanish into a cloud of smoke and logs at a moment's notice. Substitution Jutsu: attacks against you have a 20% concealment miss chance. You also gain a +1 dodge bonus to AC, a +2 competence bonus on Initiative and Reflex saves, a +2 competence bonus on Stealth and Mobility checks, and treat Stealth and Mobility as class skills.");
			bp.AddComponent(delegate(AddConcealment c)
			{
				c.Descriptor = ConcealmentDescriptor.Blur;
				c.Concealment = Concealment.Partial;
				c.OnlyForAttacks = true;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Seeking out the ultimate ingredients across prehistoric continents and forbidden biomes, your cells have evolved to thrive on culinary delicacies. Gourmet Cells: you are immune to disease. You also gain +30 maximum hit points, fast healing 2, a +2 competence bonus on Fortitude and Reflex saves, a +2 bonus on saving throws against poison, a +2 competence bonus on Lore (Nature) checks, and treat Lore (Nature) as a class skill.");
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Disease;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Disease;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.Poison;
				c.ModifierDescriptor = ModifierDescriptor.Competence;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 30;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillLoreNature;
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Having led a top-tier guild through hundreds of raid dungeon bosses, min-maxing hotkeys, and managing treasury logistics, full-dive VR instincts carry over flawlessly. Cooldown Management: you gain a +1 bonus to the DC of all your spells. You also gain +10 maximum hit points, a +1 dodge bonus to AC, a +2 competence bonus on Initiative, Will and Reflex saves, a +2 competence bonus on Use Magic Device and Knowledge (World) checks, and treat Use Magic Device as a class skill.");
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 1;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.SpellsOnly = true;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Believing unyieldingly in the Heart of the Cards, you have stood at the apex of tabletop tournaments and sealed ancient shadow games. Heart of the Cards: you always draw first - you roll initiative twice and take the better result. You also gain +10 maximum hit points, a +2 competence bonus on Initiative, Will and Reflex saves and to CMD, a +2 luck bonus on saving throws against fear and other emotion effects, a +2 competence bonus on Persuasion checks, and treat Persuasion as a class skill.");
			bp.AddComponent(delegate(ModifyD20 c)
			{
				c.Rule = RuleType.Initiative;
				c.RollsAmount = 1;
				c.TakeBest = true;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.Fear | SpellDescriptor.Emotion;
				c.ModifierDescriptor = ModifierDescriptor.Luck;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalCMD;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPersuasion;
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Operating out of a cramped laboratory with a modified microwave and a passionate laugh, your Reading Steiner ability retains memories across divergent worldlines. Reading Steiner: whenever you fail a Will save, you roll it again and keep the better result. You also gain a +2 insight bonus on Initiative, a +2 competence bonus on Fortitude and Reflex saves, a +2 bonus on saving throws against confusion and mind-affecting effects, a +2 competence bonus on Knowledge (World) and Knowledge (Arcana) checks, and treat Knowledge (World) and Knowledge (Arcana) as class skills.");
			bp.AddComponent(delegate(ModifyD20 c)
			{
				c.Rule = RuleType.SavingThrow;
				c.m_SavingThrowType = FlaggedSavingThrowType.Will;
				c.RollsAmount = 1;
				c.TakeBest = true;
				c.RerollOnlyIfFailed = true;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Confusion;
				c.ModifierDescriptor = ModifierDescriptor.Insight;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.Initiative;
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
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
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
				c.Stat = StatType.SkillKnowledgeArcana;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeWorld;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeArcana;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeWorld;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeArcana;
			});
		}));
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundShrunkHighSchoolSleuth", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Shrunk High School Sleuth");
			bp.SetBackgroundDescription(Main.IsekaiContext, "There is always only one truth! Despite mysterious toxins compressing your mortal body, your analytical intellect dissects crime scenes and ambush trajectories instantly. Only One Truth: you see invisible creatures, and gain a +4 bonus on saving throws against illusions. You also gain a +2 insight bonus on Initiative, a +2 competence bonus on Will and Reflex saves, a +3 competence bonus on Perception checks and a +2 competence bonus on Knowledge (World) checks, and treat Perception and Knowledge (World) as class skills.");
			bp.AddComponent(delegate(AddCondition c)
			{
				c.Condition = UnitCondition.SeeInvisibility;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstSchool c)
			{
				c.School = SpellSchool.Illusion;
				c.ModifierDescriptor = ModifierDescriptor.Competence;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPerception;
				c.Value = 3;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillKnowledgeWorld;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeWorld;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeWorld;
			});
		}));
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundUndergroundGambler", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Underground Gambler");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Pushed to the precipice of ruin in steel-beam crossings and high-stakes pachinko parlors, your instincts shine brightest in life-or-death desperation. All In: you gain a +2 luck bonus on attack rolls. You also gain +10 maximum hit points, a +2 competence bonus on Initiative and Reflex saves, a +2 luck bonus on saving throws against mind-affecting effects, a +2 competence bonus on Trickery checks, and treat Trickery as a class skill.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.MindAffecting;
				c.ModifierDescriptor = ModifierDescriptor.Luck;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillThievery;
				c.Value = 2;
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Understanding the sacred laws of Equivalent Exchange and having peered beyond the Gate of Truth, you manipulate the molecular composition of the world with ease. Equivalent Exchange: you gain a +2 bonus to the DC of all Transmutation spells. You also gain acid and cold resistance 10, a +2 competence bonus on Fortitude saves, Initiative and to Combat Maneuver Bonus, a +2 competence bonus on Knowledge (Arcana) checks, and treat Knowledge (Arcana) as a class skill.");
			bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
			{
				c.School = SpellSchool.Transmutation;
				c.BonusDC = 2;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Acid;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Cold;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillKnowledgeArcana;
				c.Value = 2;
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "100 pushups, 100 situps, 100 squats, and a 10-kilometer run every single day, with no air conditioning in summer or heating in winter! One Punch: you gain a +2 bonus on damage rolls. You also gain +10 maximum hit points, a +1 natural armor bonus to AC, a +2 competence bonus on Fortitude and Will saves and to Combat Maneuver Bonus, a +2 competence bonus on Athletics checks, and treat Athletics as a class skill.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillAthletics;
				c.Value = 2;
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
			bp.SetBackgroundDescription(Main.IsekaiContext, "Convinced you were merely a nameless background extra, you practiced walking unnoticed in the shadows while letting dramatic events unfold around you. Shadow Garden: you gain +1d6 sneak attack. You also gain +10 maximum hit points, a +1 dodge bonus to AC, a +2 competence bonus on Initiative, Reflex and Will saves, a +4 competence bonus on Stealth checks, and treat Stealth and Mobility as class skills.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SneakAttack;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillStealth;
				c.Value = 4;
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
	}
}
