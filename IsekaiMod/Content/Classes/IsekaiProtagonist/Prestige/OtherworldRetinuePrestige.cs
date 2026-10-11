using System;
using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.GenericSlot;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;

internal class OtherworldRetinuePrestige
{
	private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "OtherworldRetinueClass.Name", "Otherworld Retinue");

	private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "OtherworldRetinueClass.Description", "Chosen companions and devoted allies who walk in the radiance of an otherworldly protagonist. They were born in this world, but the soul bond lets them borrow fragments of the protagonist's authority, plot armor and reality-defying luck without giving up the magic or the fighting style they already had. Every protagonist needs a party; this is what standing in one does to you.\n\nRequires character level 6 and either a base attack bonus of +5 or the ability to cast 2nd-level spells; it is a companion's class, and a character with Isekai Protagonist levels cannot take it. Retinue Discipline (1st): a spellcaster's own class spellbook keeps advancing at every level of this class, and a character with no spells gains martial prowess instead. Pact of the Retinue (1st): choose a mantle, the Aegis (defense and damage reduction), the Vanguard (sneak attack and critical hits), the Conduit (caster level and spell penetration) or the Tactician (an aura for the party). Protagonist Resonance (1st): a morale bonus to attack rolls, saving throws and AC that grows to +4. Lesser Plot Armor (2nd): immunity to death effects and +2 luck to AC and saves. Borrowed Special Power (3rd, 6th and 9th), Aura Relay (4th), Hype Engine and a Signature Bond (5th), Shared Limit Break (7th), Teamwork Harmony (8th) and The Sovereign's Right Hand (10th) follow, with an Isekai bonus feat at every even level.");

	private static readonly LocalizedString DescriptionShort = Helpers.CreateString(Main.IsekaiContext, "OtherworldRetinueClass.DescriptionShort", "Prestige class for the protagonist's companions: it keeps their own spellcasting advancing while granting plot armor, borrowed special powers, party auras and a signature bond.");

	private static BlueprintCharacterClass otherworldRetinueClass;

	private static readonly BlueprintStatProgression BABMedium = StaticReferences.StatProgressions.BABMedium;

	private static readonly BlueprintStatProgression SavesPrestigeHigh = StaticReferences.StatProgressions.SavesPrestigeHigh;

	private static readonly BlueprintStatProgression SavesPrestigeLow = StaticReferences.StatProgressions.SavesPrestigeLow;

	public static void Add()
	{
		BlueprintCharacterClass blueprintCharacterClass = ClassTools.Classes.SlayerClass ?? ClassTools.Classes.FighterClass;
		int num = LinqExtensions.IndexOf(StaticReferences.BaseClasses, blueprintCharacterClass);
		if (num < 0 && StaticReferences.BaseClasses.Length != 0)
		{
			num = 0;
		}
		int isekaiDefaultClothes = Main.IsekaiContext.AddedContent.IsekaiDefaultClothes;
		int num2 = StaticReferences.BaseClasses.Length;
		BlueprintCharacterClass clothesClass = ((isekaiDefaultClothes >= 0 && isekaiDefaultClothes < num2) ? StaticReferences.BaseClasses[isekaiDefaultClothes] : null);
		if (clothesClass == null)
		{
			clothesClass = ((num >= 0 && num < num2) ? StaticReferences.BaseClasses[num] : blueprintCharacterClass);
		}
		otherworldRetinueClass = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldRetinueClass", delegate(BlueprintCharacterClass blueprintCharacterClass2)
		{
			blueprintCharacterClass2.LocalizedName = Name;
			blueprintCharacterClass2.LocalizedDescription = Description;
			blueprintCharacterClass2.LocalizedDescriptionShort = DescriptionShort;
			blueprintCharacterClass2.HitDie = DiceType.D10;
			blueprintCharacterClass2.PrestigeClass = true;
			blueprintCharacterClass2.m_BaseAttackBonus = BABMedium.ToReference<BlueprintStatProgressionReference>();
			blueprintCharacterClass2.m_FortitudeSave = SavesPrestigeHigh.ToReference<BlueprintStatProgressionReference>();
			blueprintCharacterClass2.m_ReflexSave = SavesPrestigeLow.ToReference<BlueprintStatProgressionReference>();
			blueprintCharacterClass2.m_WillSave = SavesPrestigeLow.ToReference<BlueprintStatProgressionReference>();
			blueprintCharacterClass2.m_Difficulty = 1;
			blueprintCharacterClass2.RecommendedAttributes = new StatType[3]
			{
				StatType.Strength,
				StatType.Dexterity,
				StatType.Charisma
			};
			blueprintCharacterClass2.NotRecommendedAttributes = new StatType[0];
			blueprintCharacterClass2.SkillPoints = 4;
			blueprintCharacterClass2.ClassSkills = new StatType[10]
			{
				StatType.SkillAthletics,
				StatType.SkillMobility,
				StatType.SkillStealth,
				StatType.SkillKnowledgeArcana,
				StatType.SkillKnowledgeWorld,
				StatType.SkillLoreNature,
				StatType.SkillLoreReligion,
				StatType.SkillPerception,
				StatType.SkillPersuasion,
				StatType.SkillUseMagicDevice
			};
			blueprintCharacterClass2.IsDivineCaster = false;
			blueprintCharacterClass2.IsArcaneCaster = false;
			blueprintCharacterClass2.PrimaryColor = 11;
			blueprintCharacterClass2.SecondaryColor = 11;
			if (clothesClass != null)
			{
				blueprintCharacterClass2.MaleEquipmentEntities = clothesClass.MaleEquipmentEntities;
				blueprintCharacterClass2.FemaleEquipmentEntities = clothesClass.FemaleEquipmentEntities;
			}
			blueprintCharacterClass2.m_Archetypes = new BlueprintArchetypeReference[0];
			blueprintCharacterClass2.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.Level = 6;
			});
			blueprintCharacterClass2.AddComponent(delegate(PrerequisiteStatValue c)
			{
				c.Group = Prerequisite.GroupType.Any;
				c.Stat = StatType.BaseAttackBonus;
				c.Value = 5;
			});
			blueprintCharacterClass2.AddComponent(delegate(PrerequisiteCasterTypeSpellLevel c)
			{
				c.Group = Prerequisite.GroupType.Any;
				c.IsArcane = true;
				c.RequiredSpellLevel = 2;
			});
			blueprintCharacterClass2.AddComponent(delegate(PrerequisiteCasterTypeSpellLevel c)
			{
				c.Group = Prerequisite.GroupType.Any;
				c.IsArcane = false;
				c.RequiredSpellLevel = 2;
			});
			blueprintCharacterClass2.AddComponent(delegate(PrerequisiteIsMainCharacter c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.CompanionOnly = true;
				c.RespectsIsekaiSettings = true;
			});
			blueprintCharacterClass2.AddComponent(delegate(PrerequisiteNoClassLevel c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
			});
		});
		BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueMartialProwessFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Retinue Martial Prowess");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "For allies who do not practice spellcasting, service in the Retinue sharpens martial instinct. You gain a +1 bonus to attack rolls and a +2 bonus to physical damage rolls.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 1;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 2;
			});
		});
		(string, string, BlueprintCharacterClass)[] array = new(string, string, BlueprintCharacterClass)[19]
		{
			("Witch", "Witch", ClassTools.Classes.WitchClass),
			("Oracle", "Oracle", ClassTools.Classes.OracleClass),
			("Shaman", "Shaman", ClassTools.Classes.ShamanClass),
			("Cleric", "Cleric", ClassTools.Classes.ClericClass),
			("Paladin", "Paladin", ClassTools.Classes.PaladinClass),
			("Wizard", "Wizard", ClassTools.Classes.WizardClass),
			("Sorcerer", "Sorcerer", ClassTools.Classes.SorcererClass),
			("Arcanist", "Arcanist", ClassTools.Classes.ArcanistClass),
			("Bard", "Bard", ClassTools.Classes.BardClass),
			("Skald", "Skald", ClassTools.Classes.SkaldClass),
			("Magus", "Magus", ClassTools.Classes.MagusClass),
			("Bloodrager", "Bloodrager", ClassTools.Classes.BloodragerClass),
			("Inquisitor", "Inquisitor", ClassTools.Classes.InquisitorClass),
			("Ranger", "Ranger", ClassTools.Classes.RangerClass),
			("Warpriest", "Warpriest", ClassTools.Classes.WarpriestClass),
			("Druid", "Druid", ClassTools.Classes.DruidClass),
			("Alchemist", "Alchemist", ClassTools.Classes.AlchemistClass),
			("Hunter", "Hunter", ClassTools.Classes.HunterClass),
			("Rogue", "Rogue", ClassTools.Classes.RogueClass)
		};
		List<BlueprintFeatureReference> spellbookFeatures = new List<BlueprintFeatureReference>();
		(string, string, BlueprintCharacterClass)[] array2 = array;
		for (int num3 = 0; num3 < array2.Length; num3++)
		{
			(string, string, BlueprintCharacterClass) tuple = array2[num3];
			if (tuple.Item3 != null && tuple.Item3.Spellbook != null)
			{
				BlueprintFeatureReplaceSpellbook bp = CreateRetinueSpellbook("RetinueSpellbook" + tuple.Item1, tuple.Item2, tuple.Item3, ArchetypesReplacingBookOf(tuple.Item3));
				spellbookFeatures.Add(bp.ToReference<BlueprintFeatureReference>());
			}
		}
		string[][] array3 = ArchetypeReplacedBookIds();
		foreach (string[] array4 in array3)
		{
			BlueprintFeatureReplaceSpellbook blueprintFeatureReplaceSpellbook = CreateRetinueArchetypeSpellbook(array4[0], array4[1], array4[2]);
			if (blueprintFeatureReplaceSpellbook != null)
			{
				spellbookFeatures.Add(blueprintFeatureReplaceSpellbook.ToReference<BlueprintFeatureReference>());
			}
		}
		List<BlueprintCharacterClassReference> coveredClasses = new List<BlueprintCharacterClassReference>();
		array2 = array;
		for (int num3 = 0; num3 < array2.Length; num3++)
		{
			(string, string, BlueprintCharacterClass) tuple2 = array2[num3];
			if (tuple2.Item3 != null && tuple2.Item3.Spellbook != null)
			{
				coveredClasses.Add(tuple2.Item3.ToReference<BlueprintCharacterClassReference>());
			}
		}
		array3 = ArchetypeReplacedBookIds();
		for (int num3 = 0; num3 < array3.Length; num3++)
		{
			BlueprintCharacterClass blueprint = BlueprintTools.GetBlueprint<BlueprintCharacterClass>(array3[num3][1]);
			if (blueprint != null)
			{
				BlueprintCharacterClassReference item = blueprint.ToReference<BlueprintCharacterClassReference>();
				if (!coveredClasses.Contains(item))
				{
					coveredClasses.Add(item);
				}
			}
		}
		blueprintFeature.AddComponent(delegate(PrerequisiteHasNoSpellbookOfClasses c)
		{
			c.Group = Prerequisite.GroupType.All;
			c.m_CharacterClasses = coveredClasses.ToArray();
		});
		spellbookFeatures.Add(blueprintFeature.ToReference<BlueprintFeatureReference>());
		BlueprintFeatureSelection blueprintFeatureSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueSpellbookSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection5)
		{
			blueprintFeatureSelection5.SetName(Main.IsekaiContext, "Retinue Discipline Selection");
			blueprintFeatureSelection5.SetDescription(Main.IsekaiContext, "Select your primary discipline. Spellcasters advance their existing class spellbook at every level of Otherworld Retinue, gaining spells known and spells per day as if advancing in that class. Non-spellcasters gain martial prowess bonuses.");
			blueprintFeatureSelection5.Group = FeatureGroup.ReplaceSpellbook;
			blueprintFeatureSelection5.Ranks = 1;
			blueprintFeatureSelection5.IsClassFeature = true;
			blueprintFeatureSelection5.m_AllFeatures = spellbookFeatures.ToArray();
		});
		BlueprintFeature RetinuePactAegisFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinuePactAegisFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Bestowed Title: The Aegis");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "You stand as the protagonist's unbreakable shield. You gain a +2 shield bonus to AC, a +2 natural armor bonus to AC, and Damage Reduction 5/-.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Shield;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Simple,
					Value = 5
				};
			});
		});
		BlueprintFeature RetinuePactVanguardFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinuePactVanguardFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Bestowed Title: The Vanguard");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "You spearhead the offensive at the protagonist's flank. You gain +2d6 Sneak Attack damage and a +4 bonus on attack rolls to confirm critical hits.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SneakAttack;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(CriticalConfirmationBonus c)
			{
				c.Value = 0;
				c.Bonus = 4;
			});
		});
		BlueprintFeature RetinuePactConduitFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinuePactConduitFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Bestowed Title: The Conduit");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "You channel the supernatural energies swirling around the protagonist. You gain a +1 bonus to your caster level and a +2 bonus to Spell Penetration.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddCasterLevel c)
			{
				c.Bonus = 1;
			});
			blueprintFeature9.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 2;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
		});
		BlueprintFeature RetinuePactTacticianFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("RetinuePactTactician", "You orchestrate battlefield positioning to complement the protagonist's tactics. You and all allies within 30 feet gain a +2 competence bonus to attack rolls and saving throws.", "Allies within 30 feet gain a +2 competence bonus to attack rolls and saving throws.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11"))?.m_Icon, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(30f), affectEnemies: false, delegate(BlueprintBuff obj)
		{
			obj.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			obj.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			obj.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			obj.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
		});
		RetinuePactTacticianFeature.SetName(Main.IsekaiContext, "Bestowed Title: The Tactician");
		BlueprintFeatureSelection blueprintFeatureSelection2 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinuePactSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection5)
		{
			blueprintFeatureSelection5.SetName(Main.IsekaiContext, "Pact of the Retinue");
			blueprintFeatureSelection5.SetDescription(Main.IsekaiContext, "Upon bonding with the otherworldly protagonist, choose your bestowed mantle: The Aegis (defensive protection and DR), The Vanguard (sneak attack and critical confirmation), The Conduit (caster level and spell penetration), or The Tactician (group competence aura).");
			blueprintFeatureSelection5.Ranks = 1;
			blueprintFeatureSelection5.IsClassFeature = true;
			blueprintFeatureSelection5.m_AllFeatures = new BlueprintFeatureReference[4]
			{
				RetinuePactAegisFeature.ToReference<BlueprintFeatureReference>(),
				RetinuePactVanguardFeature.ToReference<BlueprintFeatureReference>(),
				RetinuePactConduitFeature.ToReference<BlueprintFeatureReference>(),
				RetinuePactTacticianFeature.ToReference<BlueprintFeatureReference>()
			};
		});
		BlueprintFeature blueprintFeature2 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueProtagonistResonanceFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Protagonist Resonance");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Standing in the presence of the protagonist bolsters your resolve. You gain a +1 morale bonus to attack rolls, saving throws, and Armor Class per 3 Otherworld Retinue levels (+1 at 1st, +2 at 4th, +3 at 7th, and +4 at 10th level).");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
				c.m_Class = new BlueprintCharacterClassReference[1] { otherworldRetinueClass.ToReference<BlueprintCharacterClassReference>() };
				c.m_Progression = ContextRankProgression.StartPlusDivStep;
				c.m_StartLevel = 1;
				c.m_StepLevel = 3;
			});
			blueprintFeature9.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Rank,
					ValueRank = AbilityRankType.Default
				};
			});
			blueprintFeature9.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveFortitude;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Rank,
					ValueRank = AbilityRankType.Default
				};
			});
			blueprintFeature9.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveReflex;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Rank,
					ValueRank = AbilityRankType.Default
				};
			});
			blueprintFeature9.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveWill;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Rank,
					ValueRank = AbilityRankType.Default
				};
			});
			blueprintFeature9.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AC;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Rank,
					ValueRank = AbilityRankType.Default
				};
			});
		});
		BlueprintFeature blueprintFeature3 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueLesserPlotArmorFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Lesser Plot Armor");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "The narrative refuses to let the protagonist's cherished companions fall trivially. You gain immunity to death effects, a +2 luck bonus to Armor Class, and a +2 luck bonus to all saving throws.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
			blueprintFeature9.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
		});
		BlueprintWeaponEnchantment HolyEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140");
		BlueprintFeature RetinueBorrowedExcaliburFeature = TTCoreExtensions.CreateToggleBuffFeature("RetinueBorrowedExcalibur", "Your primary weapon gains the holy enchantment, dealing an extra 2d6 holy damage, and your melee reach increases by 5 feet.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("808ab74c12df8784ab4eeaf6a107dbea"))?.m_Icon, delegate(BlueprintBuff obj)
		{
			obj.AddComponent(delegate(BuffEnchantAnyWeapon c)
			{
				c.m_EnchantmentBlueprint = HolyEnchantment.ToReference<BlueprintItemEnchantmentReference>();
				c.Slot = EquipSlotBase.SlotType.PrimaryHand;
			});
			obj.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Reach;
				c.Value = 5;
			});
		});
		RetinueBorrowedExcaliburFeature.SetName(Main.IsekaiContext, "Borrowed Special Power: Excalibur");
		BlueprintFeature RetinueBorrowedRegenerationFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedRegenerationFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Borrowed Special Power: Regeneration");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "You channel the protagonist's biological cheat code, regaining 5 hit points every round.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
		});
		BlueprintFeature RetinueBorrowedSneakyMagicFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedSneakyMagicFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Borrowed Special Power: Sneaky Magic");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "You can add your sneak attack damage to any spell that deals damage against flat-footed targets.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent<SurpriseSpells>();
		});
		BlueprintFeature RetinueBorrowedArmorOfKingsFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedArmorOfKingsFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Borrowed Special Power: Armor of Kings");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "A defensive ward covers you, granting a +3 deflection bonus to Armor Class and a +3 resistance bonus to all saving throws.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Deflection;
				c.Stat = StatType.AC;
				c.Value = 3;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Resistance;
				c.Stat = StatType.SaveFortitude;
				c.Value = 3;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Resistance;
				c.Stat = StatType.SaveReflex;
				c.Value = 3;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Resistance;
				c.Stat = StatType.SaveWill;
				c.Value = 3;
			});
		});
		BlueprintFeature RetinueBorrowedUnreactableFeature = TTCoreExtensions.CreateToggleBuffFeature("RetinueBorrowedUnreactable", "Enemies you attack are treated as flat-footed.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e3d324eb309cdf44a87d666c7a27715c"))?.m_Icon, delegate(BlueprintBuff obj)
		{
			obj.AddComponent<SetFlatFootedOnAttack>();
		});
		RetinueBorrowedUnreactableFeature.SetName(Main.IsekaiContext, "Borrowed Special Power: Unreactable");
		BlueprintFeature RetinueBorrowedChannelingFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedChannelingFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Borrowed Special Power: Vitality Surge");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Supernatural vitality floods your essence. You gain +20 maximum hit points and a +2 bonus to Constitution.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 20;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Constitution;
				c.Value = 2;
			});
		});
		BlueprintFeatureSelection blueprintFeatureSelection3 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedPowerSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection5)
		{
			blueprintFeatureSelection5.SetName(Main.IsekaiContext, "Borrowed Special Power");
			blueprintFeatureSelection5.SetDescription(Main.IsekaiContext, "Select an otherworldly special power borrowed through your soul bond with the protagonist.");
			blueprintFeatureSelection5.Ranks = 3;
			blueprintFeatureSelection5.IsClassFeature = true;
			blueprintFeatureSelection5.m_AllFeatures = new BlueprintFeatureReference[6]
			{
				RetinueBorrowedExcaliburFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBorrowedRegenerationFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBorrowedSneakyMagicFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBorrowedArmorOfKingsFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBorrowedUnreactableFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBorrowedChannelingFeature.ToReference<BlueprintFeatureReference>()
			};
		});
		BlueprintFeature blueprintFeature4 = TTCoreExtensions.CreateToggleAuraBuffFeature("RetinueAuraRelay", "The companion acts as an otherworldly amplifier. All allies within 30 feet gain a +2 insight bonus to Armor Class and attack rolls.", "Allies within 30 feet gain a +2 insight bonus to Armor Class and attack rolls.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("247a4068296e8be42890143f451b4b45"))?.m_Icon, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(30f), affectEnemies: false, delegate(BlueprintBuff obj)
		{
			obj.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			obj.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
		});
		blueprintFeature4.SetName(Main.IsekaiContext, "Aura Relay");
		BlueprintBuff RetinueHypeEngineBuff = TTCoreExtensions.CreateBuff("RetinueHypeEngineBuff", delegate(BlueprintBuff blueprintBuff)
		{
			blueprintBuff.SetName(Main.IsekaiContext, "Hype Surge");
			blueprintBuff.SetDescription(Main.IsekaiContext, "Fueled by the excitement of battle, you gain +1 extra attack per round and a +10 foot bonus to movement speed.");
			blueprintBuff.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
			});
			blueprintBuff.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
		});
		BlueprintFeature blueprintFeature5 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueHypeEngineFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Hype Engine");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Whenever you confirm a critical hit, narrative momentum surges through you, granting an additional attack and +10 foot speed for 2 rounds.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
			{
				c.OnlyHit = true;
				c.CriticalHit = true;
				c.Action = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
				{
					b.m_Buff = RetinueHypeEngineBuff.ToReference<BlueprintBuffReference>();
					b.DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = 2
					};
				});
			});
		});
		BlueprintFeature RetinueBondHellknightIronWillFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondHellknightIronWillFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Signature Bond: Hellknight Iron Will");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Emulating the unbending discipline of Hellknight commanders, you stand as an unyielding fortress beside the protagonist. You gain a +4 shield bonus to Armor Class, weapon attacks are treated as Good- and Lawful-aligned for overcoming damage reduction, and you become completely immune to fear effects.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Shield;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			blueprintFeature9.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
			{
				c.AddAlignment = true;
				c.Alignment = DamageAlignment.Good | DamageAlignment.Lawful;
			});
			blueprintFeature9.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fear;
			});
			blueprintFeature9.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fear;
			});
		});
		BlueprintBuff BlindnessBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("187f88d96a0ef464280706b63635f2af");
		BlueprintFeature RetinueBondElysianDreamwalkerFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondElysianDreamwalkerFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Signature Bond: Elysian Dream-Walker");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Attuned to the song of the stars and the boundless realm of Elysian dreams, your magic flourishes while your strikes disrupt enemy perception. You gain a +2 bonus to your caster level, and your sneak attacks inflict blindness on the target for 1 round.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddCasterLevel c)
			{
				c.Bonus = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
			{
				c.OnlyHit = true;
				c.OnlySneakAttack = true;
				c.Action = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
				{
					b.m_Buff = BlindnessBuff.ToReference<BlueprintBuffReference>();
					b.DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = 1
					};
				});
			});
		});
		BlueprintFeature RetinueBondUntouchableHedonistFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondUntouchableHedonistFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Signature Bond: Untouchable Hedonist");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Channeling a scandalous indifference to peril, fortune guides your movements while your excess turns into life-affirming vitality. You add your Charisma modifier as a dodge bonus to Armor Class and Reflex saving throws. Furthermore, whenever you confirm a critical hit with a weapon attack, a burst of positive energy heals you for 3d8 hit points.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(DerivativeStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.BaseStat = StatType.Charisma;
				c.DerivativeStat = StatType.AC;
			});
			blueprintFeature9.AddComponent(delegate(DerivativeStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.BaseStat = StatType.Charisma;
				c.DerivativeStat = StatType.SaveReflex;
			});
			blueprintFeature9.AddComponent(delegate(RecalculateOnStatChange c)
			{
				c.Stat = StatType.Charisma;
			});
			blueprintFeature9.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
			{
				c.OnlyHit = true;
				c.CriticalHit = true;
				c.ActionsOnInitiator = true;
				c.Action = ActionFlow.DoSingle(delegate(ContextActionHealTarget h)
				{
					h.Value = new ContextDiceValue
					{
						DiceType = DiceType.D8,
						DiceCountValue = 3,
						BonusValue = 0
					};
				});
			});
		});
		BlueprintBuff AbyssalApexHunterACDebuff = TTCoreExtensions.CreateBuff("AbyssalApexHunterACDebuff", delegate(BlueprintBuff blueprintBuff)
		{
			blueprintBuff.SetName(Main.IsekaiContext, "Prey Exposed");
			blueprintBuff.SetDescription(Main.IsekaiContext, "This creature's defenses have been torn open by predatory marksmanship, suffering a -2 penalty to Armor Class.");
			blueprintBuff.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Penalty;
				c.Stat = StatType.AC;
				c.Value = -2;
			});
		});
		BlueprintFeature RetinueBondAbyssalApexHunterFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondAbyssalApexHunterFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Signature Bond: Abyssal Apex Hunter");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Rooted in the brutal instincts of subterranean predators, your marksmanship pierces defenses with predatory precision. You gain a +3 bonus on attack and damage rolls with ranged weapons. In addition, scoring a critical hit with a ranged weapon reduces the target's Armor Class by 2 for 2 rounds.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AttackTypeAttackBonus c)
			{
				c.Type = WeaponRangeType.Ranged;
				c.AttackBonus = 3;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Value = 1;
			});
			blueprintFeature9.AddComponent(delegate(WeaponParametersDamageBonus c)
			{
				c.Ranged = true;
				c.DamageBonus = 3;
			});
			blueprintFeature9.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
			{
				c.OnlyHit = true;
				c.CriticalHit = true;
				c.CheckWeaponRangeType = true;
				c.RangeType = WeaponRangeType.Ranged;
				c.Action = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
				{
					b.m_Buff = AbyssalApexHunterACDebuff.ToReference<BlueprintBuffReference>();
					b.DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = 2
					};
				});
			});
		});
		BlueprintFeature RetinueBondRadiantChampionFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("RetinueBondRadiantChampion", "The radiant conviction of a stalwart paladin projects an aura of steadfast courage. You and all allies within 30 feet gain a sacred bonus to saving throws equal to half your Charisma modifier.", "Allies within 30 feet gain a sacred bonus to saving throws equal to half your Charisma modifier.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11"))?.m_Icon, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(30f), affectEnemies: false, delegate(BlueprintBuff obj)
		{
			obj.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
				c.m_Stat = StatType.Charisma;
				c.m_Progression = ContextRankProgression.Div2;
			});
			obj.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveFortitude;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Rank,
					ValueRank = AbilityRankType.Default
				};
			});
			obj.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveReflex;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Rank,
					ValueRank = AbilityRankType.Default
				};
			});
			obj.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Rank,
					ValueRank = AbilityRankType.Default
				};
			});
		});
		RetinueBondRadiantChampionFeature.SetName(Main.IsekaiContext, "Signature Bond: Radiant Champion");
		BlueprintBuff Bleed2d6Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("16249b8075ab8684ca105a78a047a5ef");
		BlueprintFeature RetinueBondCrimsonCourtFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondCrimsonCourtFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Signature Bond: Spirits of the Crimson Court");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Communing with ancient spirits of blood and sacrifice, your strikes open weeping lacerations that siphon life back into your flesh. Weapon attacks inflict 2d6 bleed damage and immediately heal you for 2d6 hit points.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
			{
				c.OnlyHit = true;
				c.Action = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
				{
					b.m_Buff = Bleed2d6Buff.ToReference<BlueprintBuffReference>();
					b.DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = 1
					};
				});
			});
			blueprintFeature9.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
			{
				c.OnlyHit = true;
				c.ActionsOnInitiator = true;
				c.Action = ActionFlow.DoSingle(delegate(ContextActionHealTarget h)
				{
					h.Value = new ContextDiceValue
					{
						DiceType = DiceType.D6,
						DiceCountValue = 2,
						BonusValue = 0
					};
				});
			});
		});
		BlueprintFeature RetinueBondZenSkywatcherFeature = TTCoreExtensions.CreateToggleBuffFeature("RetinueBondZenSkywatcher", "Mastery over breath, balance, and the sky grants effortless archery. You gain +2 additional attacks per round with your primary weapon during a full attack, and your ranged attacks ignore partial concealment.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9c928dc570bb9e54a9649b3ebfe47a41"))?.m_Icon, delegate(BlueprintBuff obj)
		{
			obj.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 2;
			});
			obj.AddComponent<IgnorePartialConcealmentOnRangedAttacks>();
		});
		RetinueBondZenSkywatcherFeature.SetName(Main.IsekaiContext, "Signature Bond: Zen Skywatcher");
		BlueprintFeature RetinueBondBoundlessResonanceFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondBoundlessResonanceFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Signature Bond: Boundless Resonance");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "A versatile resonance with the otherworldly protagonist, ideal for mercenary companions and unsworn allies. You gain a +2 bonus to all saving throws, a +2 bonus to all attack rolls, and +15 maximum hit points.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			blueprintFeature9.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 15;
			});
		});
		BlueprintFeatureSelection blueprintFeatureSelection4 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueSignatureBondSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection5)
		{
			blueprintFeatureSelection5.SetName(Main.IsekaiContext, "Retinue Signature Bond");
			blueprintFeatureSelection5.SetDescription(Main.IsekaiContext, "At 5th level, an Otherworld Retinue companion cements a profound signature bond tailored to their nature, disciplines, and personality. Choose one signature bond to permanently amplify your combat role.");
			blueprintFeatureSelection5.Ranks = 1;
			blueprintFeatureSelection5.IsClassFeature = true;
			blueprintFeatureSelection5.m_AllFeatures = new BlueprintFeatureReference[8]
			{
				RetinueBondHellknightIronWillFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBondElysianDreamwalkerFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBondUntouchableHedonistFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBondAbyssalApexHunterFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBondRadiantChampionFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBondCrimsonCourtFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBondZenSkywatcherFeature.ToReference<BlueprintFeatureReference>(),
				RetinueBondBoundlessResonanceFeature.ToReference<BlueprintFeatureReference>()
			};
		});
		BlueprintBuff RetinueSharedLimitBreakBuff = TTCoreExtensions.CreateBuff("RetinueSharedLimitBreakBuff", delegate(BlueprintBuff blueprintBuff)
		{
			blueprintBuff.SetName(Main.IsekaiContext, "Shared Limit Break");
			blueprintBuff.SetDescription(Main.IsekaiContext, "Desperation awakens deep otherworldly reserves! You gain Fast Healing 10, a +4 morale bonus to attack rolls, and a +4 morale bonus to saving throws.");
			blueprintBuff.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 10;
				c.Bonus = 0;
			});
			blueprintBuff.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			blueprintBuff.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			blueprintBuff.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			blueprintBuff.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
		});
		BlueprintFeature blueprintFeature6 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueSharedLimitBreakFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Shared Limit Break");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "When pushed to the brink, the soul bond ignites. When reduced below 50% hit points, you gain Fast Healing 10, a +4 morale bonus to attack rolls, and a +4 morale bonus to saving throws for 3 rounds.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddIncomingDamageTrigger c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(Conditional cond)
				{
					cond.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionTargetHPLessThanPercent p)
					{
						p.Percent = 50;
					});
					cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = RetinueSharedLimitBreakBuff.ToReference<BlueprintBuffReference>();
						b.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							BonusValue = 3
						};
					});
					cond.IfFalse = ActionFlow.DoNothing();
				});
			});
		});
		BlueprintFeature blueprintFeature7 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueTeamworkHarmonyFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "Teamwork Harmony");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "Instinctive synchronization with the protagonist grants you Outflank, Precise Strike, and Allied Spellcaster teamwork feats.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[3]
				{
					BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("422dab7309e1ad343935f33a4d6e9f11"),
					BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("5662d1b793db90c4b9ba68037fd2a768"),
					BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("9093ceeefe9b84746a5993d619d7c86f")
				};
			});
		});
		BlueprintFeature blueprintFeature8 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueSovereignsRightHandFeature", delegate(BlueprintFeature blueprintFeature9)
		{
			blueprintFeature9.SetName(Main.IsekaiContext, "The Sovereign's Right Hand");
			blueprintFeature9.SetDescription(Main.IsekaiContext, "You have ascended to become the recognized right hand of an otherworldly sovereign. You gain permanent True Seeing, a +4 inherent bonus to all ability scores, and your weapon attacks deal an additional 3d6 Holy damage.");
			blueprintFeature9.Ranks = 1;
			blueprintFeature9.IsClassFeature = true;
			blueprintFeature9.AddComponent(delegate(AddCondition c)
			{
				c.Condition = UnitCondition.TrueSeeing;
			});
			blueprintFeature9.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			blueprintFeature9.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Dexterity;
				c.Value = 4;
			});
			blueprintFeature9.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			blueprintFeature9.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Intelligence;
				c.Value = 4;
			});
			blueprintFeature9.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Wisdom;
				c.Value = 4;
			});
			blueprintFeature9.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Charisma;
				c.Value = 4;
			});
			blueprintFeature9.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
			{
				c.OnlyHit = true;
				c.Action = ActionFlow.DoSingle(delegate(ContextActionDealDamage d)
				{
					d.DamageType = new DamageTypeDescription
					{
						Type = DamageType.Energy,
						Energy = DamageEnergyType.Holy
					};
					d.Value = new ContextDiceValue
					{
						DiceType = DiceType.D6,
						DiceCountValue = 3,
						BonusValue = 0
					};
				});
			});
		});
		BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
		BlueprintProgression blueprintProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldRetinueProgression", delegate(BlueprintProgression blueprintProgression2)
		{
			blueprintProgression2.SetName(Main.IsekaiContext, "Otherworld Retinue");
			((BlueprintUnitFact)blueprintProgression2).m_Description = Description;
			((BlueprintUnitFact)blueprintProgression2).m_AllowNonContextActions = false;
			blueprintProgression2.IsClassFeature = true;
			blueprintProgression2.m_Classes = new BlueprintProgression.ClassWithLevel[1]
			{
				new BlueprintProgression.ClassWithLevel
				{
					m_Class = otherworldRetinueClass.ToReference<BlueprintCharacterClassReference>(),
					AdditionalLevel = 0
				}
			};
		});
		blueprintProgression.LevelEntries = new LevelEntry[10]
		{
			Helpers.CreateLevelEntry(1, blueprintFeatureSelection2, blueprintFeature2, blueprintFeatureSelection),
			Helpers.CreateLevelEntry(2, blueprintFeature3, modBlueprint),
			Helpers.CreateLevelEntry(3, blueprintFeatureSelection3),
			Helpers.CreateLevelEntry(4, blueprintFeature4, modBlueprint),
			Helpers.CreateLevelEntry(5, blueprintFeature5, blueprintFeatureSelection4),
			Helpers.CreateLevelEntry(6, blueprintFeatureSelection3, modBlueprint),
			Helpers.CreateLevelEntry(7, blueprintFeature6),
			Helpers.CreateLevelEntry(8, blueprintFeature7, modBlueprint),
			Helpers.CreateLevelEntry(9, blueprintFeatureSelection3),
			Helpers.CreateLevelEntry(10, blueprintFeature8, modBlueprint)
		};
		blueprintProgression.UIGroups = new UIGroup[3]
		{
			Helpers.CreateUIGroup(blueprintFeatureSelection2, blueprintFeature4, blueprintFeature7, blueprintFeature8),
			Helpers.CreateUIGroup(blueprintFeature2, blueprintFeature3, blueprintFeature5, blueprintFeatureSelection4, blueprintFeature6),
			Helpers.CreateUIGroup(blueprintFeatureSelection3)
		};
		otherworldRetinueClass.m_Progression = blueprintProgression.ToReference<BlueprintProgressionReference>();
		ContentOffering.RegisterClass(otherworldRetinueClass, OfferedContent.IsekaiProtagonist);
	}

	internal static string[][] ArchetypeReplacedBookIds()
	{
		return new string[19][]
		{
			new string[3] { "RetinueSpellbookEldritchScoundrel", "299aa766dee3cbf4790da4efb8c72484", "57f93dd8423c97c49989501281296c4a" },
			new string[3] { "RetinueSpellbookLeyLineGuardianWitch", "1b9873f1e7bfe5449bc84d03e9c8e3cc", "ee92a9813c04b1440a3af3c766b6b02d" },
			new string[3] { "RetinueSpellbookAccursedWitch", "1b9873f1e7bfe5449bc84d03e9c8e3cc", "c5f6e53e71059fb4d802ce81a277a12d" },
			new string[3] { "RetinueSpellbookEldritchScion", "45a4607686d96a1498891b3286121780", "d078b2ef073f2814c9e338a789d97b73" },
			new string[3] { "RetinueSpellbookEldritchFont", "52dbfd8505e22f84fad8d702611f60b7", "2f35608fc073c04448a9c162a20852ce" },
			new string[3] { "RetinueSpellbookMagicDeceiver", "52dbfd8505e22f84fad8d702611f60b7", "5c77110cd0414e7eb4c2e485659c9a46" },
			new string[3] { "RetinueSpellbookNatureMage", "52dbfd8505e22f84fad8d702611f60b7", "26185cfb81b34e778ad370407300de9a" },
			new string[3] { "RetinueSpellbookUnletteredArcanist", "52dbfd8505e22f84fad8d702611f60b7", "44f3ba33839a87f48a66b2b9b2f7c69b" },
			new string[3] { "RetinueSpellbookAngelfireApostle", "67819271767a9dd4fbfd4ae700befea0", "857bc9fadf70f294795a9cba974a48b8" },
			new string[3] { "RetinueSpellbookCrusader", "67819271767a9dd4fbfd4ae700befea0", "6bfb7e74b530f3749b590286dd2b9b30" },
			new string[3] { "RetinueSpellbookFeyspeaker", "610d836f3a3a9ed42a4349b62f002e96", "da69747aa3dd0044ebff5f3d701cdde3" },
			new string[3] { "RetinueSpellbookLivingGrimoire", "f1a70d9e1b0b41e49874e1fa9052a1ce", "b68b52e5e802426d989a3d4f8c986761" },
			new string[3] { "RetinueSpellbookSwordSaint", "45a4607686d96a1498891b3286121780", "7d6678f2160018049814838af2ab4236" },
			new string[3] { "RetinueSpellbookTorturedCrusader", "bfa11238e7ae3544bbeb4d0b92e897ec", "f91ea9596e6f42f8a23803c1b3e976b4" },
			new string[3] { "RetinueSpellbookMasterSpy", "cda0615668a6df14eb36ba19ee881af6", "7888187acfd26b7499cf255b0d6dcb0e" },
			new string[3] { "RetinueSpellbookCrossblooded", "b3a505fb61437dc4097f43c3f8f9a4cf", "e66a112ab57f69e418f1f11d5d2f3248" },
			new string[3] { "RetinueSpellbookEmpyrealSorcerer", "b3a505fb61437dc4097f43c3f8f9a4cf", "aa00d945f7cf6c34c909a29a25f2df38" },
			new string[3] { "RetinueSpellbookSageSorcerer", "b3a505fb61437dc4097f43c3f8f9a4cf", "00b990c8be2117e45ae6514ee4ef561c" },
			new string[3] { "RetinueSpellbookExploiterWizard", "ba34257984f4c41408ce1dc2004e342e", "6d2027b4eec590c41adb5caeb7bb026d" }
		};
	}

	private static string ArchetypeBookDisplayName(string bpName)
	{
		return bpName switch
		{
			"RetinueSpellbookEldritchScoundrel" => "Eldritch Scoundrel", 
			"RetinueSpellbookLeyLineGuardianWitch" => "Ley Line Guardian", 
			"RetinueSpellbookAccursedWitch" => "Accursed Witch", 
			"RetinueSpellbookEldritchScion" => "Eldritch Scion", 
			"RetinueSpellbookEldritchFont" => "Eldritch Font", 
			"RetinueSpellbookMagicDeceiver" => "Magic Deceiver", 
			"RetinueSpellbookNatureMage" => "Nature Mage", 
			"RetinueSpellbookUnletteredArcanist" => "Unlettered Arcanist", 
			"RetinueSpellbookAngelfireApostle" => "Angelfire Apostle", 
			"RetinueSpellbookCrusader" => "Crusader", 
			"RetinueSpellbookFeyspeaker" => "Feyspeaker", 
			"RetinueSpellbookLivingGrimoire" => "Living Grimoire", 
			"RetinueSpellbookSwordSaint" => "Sword Saint", 
			"RetinueSpellbookTorturedCrusader" => "Tortured Crusader", 
			"RetinueSpellbookMasterSpy" => "Master Spy", 
			"RetinueSpellbookCrossblooded" => "Crossblooded", 
			"RetinueSpellbookEmpyrealSorcerer" => "Empyreal Sorcerer", 
			"RetinueSpellbookSageSorcerer" => "Sage Sorcerer", 
			"RetinueSpellbookExploiterWizard" => "Exploiter Wizard", 
			_ => bpName.Replace("RetinueSpellbook", ""), 
		};
	}

	private static BlueprintArchetypeReference[] ArchetypesReplacingBookOf(BlueprintCharacterClass characterClass)
	{
		List<BlueprintArchetypeReference> list = new List<BlueprintArchetypeReference>();
		string[][] array = ArchetypeReplacedBookIds();
		foreach (string[] array2 in array)
		{
			if (string.Equals(array2[1], characterClass.AssetGuid.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				BlueprintArchetype blueprint = BlueprintTools.GetBlueprint<BlueprintArchetype>(array2[2]);
				if (blueprint != null)
				{
					list.Add(blueprint.ToReference<BlueprintArchetypeReference>());
				}
			}
		}
		return list.ToArray();
	}

	private static BlueprintFeatureReplaceSpellbook CreateRetinueArchetypeSpellbook(string bpName, string classId, string archetypeId)
	{
		BlueprintCharacterClass characterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>(classId);
		BlueprintArchetype archetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(archetypeId);
		BlueprintSpellbookReference spellbook = archetype?.m_ReplaceSpellbook;
		if (characterClass == null || archetype == null || spellbook == null || spellbook.Guid == BlueprintGuid.Empty)
		{
			Main.IsekaiContext.Logger.LogError("[OtherworldRetinuePrestige] Could not build '" + bpName + "': class, archetype or its replacement spellbook is missing.");
			return null;
		}
		string displayName = ArchetypeBookDisplayName(bpName);
		return Helpers.CreateBlueprint(Main.IsekaiContext, bpName, delegate(BlueprintFeatureReplaceSpellbook bp)
		{
			bp.SetName(Main.IsekaiContext, "Spellbook: " + displayName);
			bp.SetDescription(Main.IsekaiContext, "Advances spellcasting progression for " + displayName + " at every level of Otherworld Retinue.");
			bp.HideInUI = true;
			bp.HideNotAvailibleInUI = true;
			bp.HideInCharacterSheetAndLevelUp = false;
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.m_Spellbook = spellbook;
			bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_CharacterClass = characterClass.ToReference<BlueprintCharacterClassReference>();
				c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
				c.Level = 1;
			});
		});
	}

	private static BlueprintFeatureReplaceSpellbook CreateRetinueSpellbook(string bpName, string displayName, BlueprintCharacterClass characterClass, BlueprintArchetypeReference[] excludedArchetypes = null)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, bpName, delegate(BlueprintFeatureReplaceSpellbook bp)
		{
			bp.SetName(Main.IsekaiContext, "Spellbook: " + displayName);
			bp.SetDescription(Main.IsekaiContext, "Advances spellcasting progression for " + displayName + " at every level of Otherworld Retinue.");
			bp.HideInUI = true;
			bp.HideNotAvailibleInUI = true;
			bp.HideInCharacterSheetAndLevelUp = false;
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.m_Spellbook = characterClass.Spellbook.ToReference<BlueprintSpellbookReference>();
			bp.AddComponent(delegate(PrerequisiteClassSpellLevel c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_CharacterClass = characterClass.ToReference<BlueprintCharacterClassReference>();
				c.RequiredSpellLevel = 1;
			});
			if (excludedArchetypes != null)
			{
				BlueprintArchetypeReference[] array = excludedArchetypes;
				foreach (BlueprintArchetypeReference archetypeReference in array)
				{
					if (archetypeReference != null)
					{
						bp.AddComponent(delegate(PrerequisiteNoArchetype c)
						{
							c.Group = Prerequisite.GroupType.All;
							c.m_CharacterClass = characterClass.ToReference<BlueprintCharacterClassReference>();
							c.m_Archetype = archetypeReference;
						});
					}
				}
			}
		});
	}

	public static BlueprintCharacterClass Get()
	{
		if (otherworldRetinueClass != null)
		{
			return otherworldRetinueClass;
		}
		return BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "OtherworldRetinueClass");
	}

	public static BlueprintCharacterClassReference GetReference()
	{
		return BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "OtherworldRetinueClass");
	}
}
