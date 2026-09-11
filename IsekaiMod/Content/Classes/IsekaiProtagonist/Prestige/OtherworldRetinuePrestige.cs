using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
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

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige
{
	internal class OtherworldRetinuePrestige
	{
		private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "OtherworldRetinueClass.Name", "Otherworld Retinue");

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "OtherworldRetinueClass.Description", "Chosen companions and devoted allies who walk in the radiance of an otherworldly protagonist. While not hailing from another realm themselves, their soul bond with the protagonist allows them to channel fragments of otherworldly authority, plot armor, and reality-defying power without sacrificing their original spellcasting or combat disciplines.");

		private static readonly LocalizedString DescriptionShort = Helpers.CreateString(Main.IsekaiContext, "OtherworldRetinueClass.DescriptionShort", "A prestige class for companions and allies that grants otherworldly powers, spellbook continuation, and protagonist resonance.");

		private static BlueprintCharacterClass otherworldRetinueClass;

		private static readonly BlueprintStatProgression BABFull = BlueprintTools.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");

		private static readonly BlueprintStatProgression SavesHigh = BlueprintTools.GetBlueprint<BlueprintStatProgression>("ff4662bde9e75f145853417313842751");

		private static readonly BlueprintStatProgression SavesLow = BlueprintTools.GetBlueprint<BlueprintStatProgression>("dc0c7c1aba755c54f96c089cdf7d14a3");

		public static void Add()
		{
			BlueprintCharacterClass blueprintCharacterClass = ClassTools.Classes.SlayerClass ?? ClassTools.Classes.FighterClass;
			int num = StaticReferences.BaseClasses.IndexOf(blueprintCharacterClass);
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
				blueprintCharacterClass2.m_BaseAttackBonus = BABFull.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass2.m_FortitudeSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass2.m_ReflexSave = SavesLow.ToReference<BlueprintStatProgressionReference>();
				blueprintCharacterClass2.m_WillSave = SavesHigh.ToReference<BlueprintStatProgressionReference>();
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
					c.Level = 3;
				});
				if (!Main.IsekaiContext.AddedContent.AllowMainCharacterRetinue)
				{
					blueprintCharacterClass2.AddComponent(delegate(PrerequisiteIsMainCharacter c)
					{
						c.Group = Prerequisite.GroupType.All;
						c.CompanionOnly = true;
					});
				}
				blueprintCharacterClass2.AddComponent(delegate(PrerequisiteNoClassLevel c)
				{
					c.Group = Prerequisite.GroupType.All;
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				});
			});
			BlueprintFeature bp = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueMartialProwessFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Retinue Martial Prowess");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "For allies who do not practice spellcasting, service in the Retinue sharpens martial instinct. You gain a +1 bonus to attack rolls and a +2 bonus to physical damage rolls.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			(string, string, BlueprintCharacterClass)[] obj = new(string, string, BlueprintCharacterClass)[17]
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
				("Alchemist", "Alchemist", ClassTools.Classes.AlchemistClass)
			};
			List<BlueprintFeatureReference> spellbookFeatures = new List<BlueprintFeatureReference>();
			(string, string, BlueprintCharacterClass)[] array = obj;
			for (int num3 = 0; num3 < array.Length; num3++)
			{
				(string, string, BlueprintCharacterClass) tuple = array[num3];
				if (tuple.Item3 != null && tuple.Item3.Spellbook != null)
				{
					BlueprintFeatureReplaceSpellbook bp2 = CreateRetinueSpellbook("RetinueSpellbook" + tuple.Item1, tuple.Item2, tuple.Item3);
					spellbookFeatures.Add(bp2.ToReference<BlueprintFeatureReference>());
				}
			}
			spellbookFeatures.Add(bp.ToReference<BlueprintFeatureReference>());
			BlueprintFeatureSelection blueprintFeatureSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueSpellbookSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection5)
			{
				blueprintFeatureSelection5.SetName(Main.IsekaiContext, "Retinue Discipline Selection");
				blueprintFeatureSelection5.SetDescription(Main.IsekaiContext, "Select your primary discipline. Spellcasters advance their existing class spellbook at every level of Otherworld Retinue, gaining spells known and spells per day as if advancing in that class. Non-spellcasters gain martial prowess bonuses.");
				blueprintFeatureSelection5.Ranks = 1;
				blueprintFeatureSelection5.IsClassFeature = true;
				blueprintFeatureSelection5.m_AllFeatures = spellbookFeatures.ToArray();
			});
			BlueprintFeature RetinuePactAegisFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinuePactAegisFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Bestowed Title: The Aegis");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "You stand as the protagonist's unbreakable shield. You gain a +2 shield bonus to AC, a +2 natural armor bonus to AC, and Damage Reduction 5/-.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 5
					};
				});
			});
			BlueprintFeature RetinuePactVanguardFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinuePactVanguardFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Bestowed Title: The Vanguard");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "You spearhead the offensive at the protagonist's flank. You gain +2d6 Sneak Attack damage and a +4 bonus on attack rolls to confirm critical hits.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Value = 4;
					c.Bonus = 4;
				});
			});
			BlueprintFeature RetinuePactConduitFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinuePactConduitFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Bestowed Title: The Conduit");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "You channel the supernatural energies swirling around the protagonist. You gain a +1 bonus to your caster level and a +2 bonus to Spell Penetration.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddCasterLevel c)
				{
					c.Bonus = 1;
				});
				blueprintFeature8.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
			});
			BlueprintFeature RetinuePactTacticianFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("RetinuePactTactician", "You orchestrate battlefield positioning to complement the protagonist's tactics. You and all allies within 30 feet gain a +2 competence bonus to attack rolls and saving throws.", "Allies within 30 feet gain a +2 competence bonus to attack rolls and saving throws.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11"))?.m_Icon, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(30f), affectEnemies: false, delegate(BlueprintBuff obj2)
			{
				obj2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				obj2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				obj2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				obj2.AddComponent(delegate(AddStatBonus c)
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
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueProtagonistResonanceFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Protagonist Resonance");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Standing in the presence of the protagonist bolsters your resolve. You gain a +1 morale bonus to attack rolls, saving throws, and Armor Class per 3 Otherworld Retinue levels (+1 at 1st, +2 at 4th, +3 at 7th, and +4 at 10th level).");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Class = new BlueprintCharacterClassReference[1] { otherworldRetinueClass.ToReference<BlueprintCharacterClassReference>() };
					c.m_Progression = ContextRankProgression.OnePlusDivStep;
					c.m_StepLevel = 3;
				});
				blueprintFeature8.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				blueprintFeature8.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				blueprintFeature8.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				blueprintFeature8.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				blueprintFeature8.AddComponent(delegate(AddContextStatBonus c)
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
			BlueprintFeature blueprintFeature2 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueLesserPlotArmorFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Lesser Plot Armor");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "The narrative refuses to let the protagonist's cherished companions fall trivially. You gain immunity to death effects, a +2 luck bonus to Armor Class, and a +2 luck bonus to all saving throws.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				blueprintFeature8.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
			});
			BlueprintWeaponEnchantment HolyEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140");
			BlueprintFeature RetinueBorrowedExcaliburFeature = TTCoreExtensions.CreateToggleBuffFeature("RetinueBorrowedExcalibur", "Your primary weapon gains the holy enchantment, dealing an extra 2d6 holy damage, and your melee reach increases by 5 feet.", AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_EXCALIBUR.png"), delegate(BlueprintBuff obj2)
			{
				obj2.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = HolyEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.PrimaryHand;
				});
				obj2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Reach;
					c.Value = 5;
				});
			});
			RetinueBorrowedExcaliburFeature.SetName(Main.IsekaiContext, "Borrowed Special Power: Excalibur");
			BlueprintFeature RetinueBorrowedRegenerationFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedRegenerationFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Borrowed Special Power: Regeneration");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "You channel the protagonist's biological cheat code, regaining 5 hit points every round.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
				});
			});
			BlueprintFeature RetinueBorrowedSneakyMagicFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedSneakyMagicFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Borrowed Special Power: Sneaky Magic");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "You can add your sneak attack damage to any spell that deals damage against flat-footed targets.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent<SurpriseSpells>();
			});
			BlueprintFeature RetinueBorrowedArmorOfKingsFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedArmorOfKingsFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Borrowed Special Power: Armor of Kings");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "A defensive ward covers you, granting a +3 deflection bonus to Armor Class and a +3 resistance bonus to all saving throws.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveFortitude;
					c.Value = 3;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveReflex;
					c.Value = 3;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveWill;
					c.Value = 3;
				});
			});
			BlueprintFeature RetinueBorrowedUnreactableFeature = TTCoreExtensions.CreateToggleBuffFeature("RetinueBorrowedUnreactable", "Enemies you attack are treated as flat-footed.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e3d324eb309cdf44a87d666c7a27715c"))?.m_Icon, delegate(BlueprintBuff obj2)
			{
				obj2.AddComponent<SetFlatFootedOnAttack>();
			});
			RetinueBorrowedUnreactableFeature.SetName(Main.IsekaiContext, "Borrowed Special Power: Unreactable");
			BlueprintFeature RetinueBorrowedChannelingFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBorrowedChannelingFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Borrowed Special Power: Vitality Surge");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Supernatural vitality floods your essence. You gain +20 maximum hit points and a +2 bonus to Constitution.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 20;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
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
			BlueprintFeature blueprintFeature3 = TTCoreExtensions.CreateToggleAuraBuffFeature("RetinueAuraRelay", "The companion acts as an otherworldly amplifier. All allies within 30 feet gain a +2 insight bonus to Armor Class and attack rolls.", "Allies within 30 feet gain a +2 insight bonus to Armor Class and attack rolls.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("247a4068296e8be42890143f451b4b45"))?.m_Icon, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(30f), affectEnemies: false, delegate(BlueprintBuff obj2)
			{
				obj2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				obj2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			blueprintFeature3.SetName(Main.IsekaiContext, "Aura Relay");
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
			BlueprintFeature blueprintFeature4 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueHypeEngineFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Hype Engine");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Whenever you confirm a critical hit or fell an enemy, narrative momentum surges through you, granting an additional attack and +10 foot speed for 2 rounds.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
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
			BlueprintFeature RetinueBondHellknightIronWillFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondHellknightIronWillFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Signature Bond: Hellknight Iron Will");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Emulating the unbending discipline of Hellknight commanders, you stand as an unyielding fortress beside the protagonist. You gain a +4 shield bonus to Armor Class, weapon attacks are treated as Good- and Lawful-aligned for overcoming damage reduction, and you become completely immune to fear effects.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				blueprintFeature8.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
				{
					c.AddAlignment = true;
					c.Alignment = DamageAlignment.Good | DamageAlignment.Lawful;
				});
				blueprintFeature8.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fear;
				});
				blueprintFeature8.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fear;
				});
			});
			BlueprintBuff BlindnessBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("187f88d96a0ef464280706b63635f2af");
			BlueprintFeature RetinueBondElysianDreamwalkerFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondElysianDreamwalkerFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Signature Bond: Elysian Dream-Walker");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Attuned to the song of the stars and the boundless realm of Elysian dreams, your magic flourishes while your strikes disrupt enemy perception. You gain a +2 bonus to your caster level, and your sneak attacks inflict blindness on the target for 1 round.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddCasterLevel c)
				{
					c.Bonus = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
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
			BlueprintFeature RetinueBondUntouchableHedonistFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondUntouchableHedonistFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Signature Bond: Untouchable Hedonist");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Channeling a scandalous indifference to peril, fortune guides your movements while your excess turns into life-affirming vitality. You add your Charisma modifier as a dodge bonus to Armor Class and Reflex saving throws. Furthermore, whenever you confirm a critical hit with a weapon attack, a burst of positive energy heals you for 3d8 hit points.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.BaseStat = StatType.Charisma;
					c.DerivativeStat = StatType.AC;
				});
				blueprintFeature8.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.BaseStat = StatType.Charisma;
					c.DerivativeStat = StatType.SaveReflex;
				});
				blueprintFeature8.AddComponent(delegate(RecalculateOnStatChange c)
				{
					c.Stat = StatType.Charisma;
				});
				blueprintFeature8.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
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
			BlueprintFeature RetinueBondAbyssalApexHunterFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondAbyssalApexHunterFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Signature Bond: Abyssal Apex Hunter");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Rooted in the brutal instincts of subterranean predators, your marksmanship pierces defenses with predatory precision. You gain a +3 bonus on attack and damage rolls with ranged weapons. In addition, scoring a critical hit with a ranged weapon reduces the target's Armor Class by 2 for 2 rounds.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AttackTypeAttackBonus c)
				{
					c.Type = WeaponRangeType.Ranged;
					c.AttackBonus = 3;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Value = new ContextValue();
				});
				blueprintFeature8.AddComponent(delegate(WeaponParametersDamageBonus c)
				{
					c.Ranged = true;
					c.DamageBonus = 3;
				});
				blueprintFeature8.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
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
			BlueprintFeature RetinueBondRadiantChampionFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("RetinueBondRadiantChampion", "The radiant conviction of a stalwart paladin projects an aura of steadfast courage. You and all allies within 30 feet gain a sacred bonus to saving throws equal to half your Charisma modifier.", "Allies within 30 feet gain a sacred bonus to saving throws equal to half your Charisma modifier.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11"))?.m_Icon, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(30f), affectEnemies: false, delegate(BlueprintBuff obj2)
			{
				obj2.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
					c.m_Stat = StatType.Charisma;
					c.m_Progression = ContextRankProgression.Div2;
				});
				obj2.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				obj2.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				obj2.AddComponent(delegate(AddContextStatBonus c)
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
			BlueprintFeature RetinueBondCrimsonCourtFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondCrimsonCourtFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Signature Bond: Spirits of the Crimson Court");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Communing with ancient spirits of blood and sacrifice, your strikes open weeping lacerations that siphon life back into your flesh. Weapon attacks inflict 2d6 bleed damage and immediately heal you for 2d6 hit points.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
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
				blueprintFeature8.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
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
			BlueprintFeature RetinueBondZenSkywatcherFeature = TTCoreExtensions.CreateToggleBuffFeature("RetinueBondZenSkywatcher", "Mastery over breath, balance, and the sky grants effortless archery. You gain +2 additional attacks per round with ranged weapons during a full attack, and your ranged attacks ignore partial concealment.", ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9c928dc570bb9e54a9649b3ebfe47a41"))?.m_Icon, delegate(BlueprintBuff obj2)
			{
				obj2.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
				});
				obj2.AddComponent<IgnorePartialConcealmentOnRangedAttacks>();
			});
			RetinueBondZenSkywatcherFeature.SetName(Main.IsekaiContext, "Signature Bond: Zen Skywatcher");
			BlueprintFeature RetinueBondBoundlessResonanceFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueBondBoundlessResonanceFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Signature Bond: Boundless Resonance");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "A versatile resonance with the otherworldly protagonist, ideal for mercenary companions and unsworn allies. You gain a +2 bonus to all saving throws, a +2 bonus to all attack rolls, and +15 maximum hit points.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
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
			BlueprintFeature blueprintFeature5 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueSharedLimitBreakFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Shared Limit Break");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "When pushed to the brink, the soul bond ignites. When reduced below 50% hit points, you gain Fast Healing 10, a +4 morale bonus to attack rolls, and a +4 morale bonus to saving throws for 3 rounds.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddIncomingDamageTrigger c)
				{
					c.ReduceBelowZero = true;
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = RetinueSharedLimitBreakBuff.ToReference<BlueprintBuffReference>();
						b.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							BonusValue = 3
						};
					});
				});
			});
			BlueprintFeature blueprintFeature6 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueTeamworkHarmonyFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "Teamwork Harmony");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "Instinctive synchronization with the protagonist grants you Outflank, Precise Strike, and Allied Spellcaster teamwork feats.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[3]
					{
						BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11").ToReference<BlueprintUnitFactReference>(),
						BlueprintTools.GetBlueprint<BlueprintFeature>("5662d1b793db90c4b9ba68037fd2a768").ToReference<BlueprintUnitFactReference>(),
						BlueprintTools.GetBlueprint<BlueprintFeature>("9093ceeefe9b84746a5993d619d7c86f").ToReference<BlueprintUnitFactReference>()
					};
				});
			});
			BlueprintFeature blueprintFeature7 = Helpers.CreateBlueprint(Main.IsekaiContext, "RetinueSovereignsRightHandFeature", delegate(BlueprintFeature blueprintFeature8)
			{
				blueprintFeature8.SetName(Main.IsekaiContext, "The Sovereign's Right Hand");
				blueprintFeature8.SetDescription(Main.IsekaiContext, "You have ascended to become the recognized right hand of an otherworldly sovereign. You gain permanent True Seeing, a +4 inherent bonus to all ability scores, and your weapon attacks deal an additional 3d6 Holy damage.");
				blueprintFeature8.Ranks = 1;
				blueprintFeature8.IsClassFeature = true;
				blueprintFeature8.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.TrueSeeing;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 4;
				});
				blueprintFeature8.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
				blueprintFeature8.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
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
				blueprintProgression2.SetDescription(Main.IsekaiContext, "Chosen companions and devoted allies who walk in the radiance of an otherworldly protagonist.");
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
				Helpers.CreateLevelEntry(1, blueprintFeatureSelection2, blueprintFeature, blueprintFeatureSelection),
				Helpers.CreateLevelEntry(2, blueprintFeature2, modBlueprint),
				Helpers.CreateLevelEntry(3, blueprintFeatureSelection3),
				Helpers.CreateLevelEntry(4, blueprintFeature3, modBlueprint),
				Helpers.CreateLevelEntry(5, blueprintFeature4, blueprintFeatureSelection4),
				Helpers.CreateLevelEntry(6, blueprintFeatureSelection3, modBlueprint),
				Helpers.CreateLevelEntry(7, blueprintFeature5),
				Helpers.CreateLevelEntry(8, blueprintFeature6, modBlueprint),
				Helpers.CreateLevelEntry(9, blueprintFeatureSelection3),
				Helpers.CreateLevelEntry(10, blueprintFeature7, modBlueprint)
			};
			blueprintProgression.UIGroups = new UIGroup[3]
			{
				Helpers.CreateUIGroup(blueprintFeatureSelection2, blueprintFeature3, blueprintFeature6, blueprintFeature7),
				Helpers.CreateUIGroup(blueprintFeature, blueprintFeature2, blueprintFeature4, blueprintFeatureSelection4, blueprintFeature5),
				Helpers.CreateUIGroup(blueprintFeatureSelection3)
			};
			otherworldRetinueClass.m_Progression = blueprintProgression.ToReference<BlueprintProgressionReference>();
			TTCoreExtensions.RegisterClass(otherworldRetinueClass);
		}

		private static BlueprintFeatureReplaceSpellbook CreateRetinueSpellbook(string bpName, string displayName, BlueprintCharacterClass characterClass)
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
}
