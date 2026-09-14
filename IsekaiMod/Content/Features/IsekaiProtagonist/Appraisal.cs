using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal static class Appraisal
	{
		private static readonly Sprite Icon_Foretell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon;

		private static readonly Sprite Icon_TrueSeeing = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("b3da3fbee6a751d4197e446c7e852bcb"))?.m_Icon;

		private static readonly Sprite Icon_Haste = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280"))?.m_Icon;

		private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon;

		private static readonly Sprite Icon_Book = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e45ab30f49215054e83b4ea12165409f"))?.m_Icon ?? Icon_Foretell;

		public static BlueprintFeature AppraisalFeature;

		public static BlueprintAbility AppraiseFoeAbility;

		public static BlueprintBuff AppraiseFoeBuff;

		public static BlueprintBuff AppraiseTargetDebuff;

		public static BlueprintItemEquipmentUsable ItemSkillBookGrandScholar;

		public static BlueprintItemEquipmentUsable ItemSkillBookPhantomInfiltrator;

		public static BlueprintItemEquipmentUsable ItemSkillBookAbsoluteCharisma;

		public static BlueprintItemEquipmentUsable ItemSkillBookWindWalker;

		public static BlueprintItemEquipmentUsable ItemSkillBookEyeOfProvidence;

		public static BlueprintItemEquipmentUsable ItemSkillBookFirstCrusaders;

		public static BlueprintItemEquipmentUsable ItemSkillBookRadiantWard;

		public static BlueprintItemEquipmentUsable ItemSkillBookUnbrokenSoul;

		public static BlueprintItemEquipmentUsable ItemSkillBookPrimalBeastmaster;

		public static BlueprintItemEquipmentUsable ItemSkillBookSwarmTransmutation;

		public static BlueprintItemEquipmentUsable ItemSkillBookTechnicLeague;

		public static BlueprintItemEquipmentUsable ItemSkillBookPlanarInfiltration;

		public static BlueprintItemEquipmentUsable ItemSkillBookAreeluAxiom;

		public static BlueprintItemEquipmentUsable ItemSkillBookMythicMineralogy;

		public static BlueprintItemEquipmentUsable ItemSkillBookRunelordLexicon;

		public static void Add()
		{
			AppraiseFoeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "AppraiseFoeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Appraise: Tactical Analysis");
				bp.SetDescription(Main.IsekaiContext, "Insight gleaned from Appraisal grants a +2 insight bonus to attack rolls, armor class, and saving throws against the analyzed foe.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrueSeeing;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			AppraiseTargetDebuff = Helpers.CreateBlueprint(Main.IsekaiContext, "AppraiseTargetDebuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Appraised: Vulnerability Exposed");
				bp.SetDescription(Main.IsekaiContext, "The protagonist's analytical gaze has identified weaknesses in your defenses, imposing a -2 penalty to armor class and saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foretell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AC;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveFortitude;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveReflex;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -2;
				});
			});
			AppraiseFoeAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "AppraiseFoeAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Appraise Foe");
				bp.SetDescription(Main.IsekaiContext, "As a free action, focus your otherworld analytical gaze upon an enemy within 30 feet. Analyzes their defensive structure for 1 minute: you gain a +2 insight bonus to attack rolls, AC, and saving throws, while the target suffers a -2 penalty to AC and saving throws against your abilities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrueSeeing;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = false;
				bp.CanTargetSelf = false;
				bp.CanTargetPoint = false;
				bp.ActionType = UnitCommand.CommandType.Free;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				ContextActionApplyBuff applyDebuff = new ContextActionApplyBuff
				{
					m_Buff = AppraiseTargetDebuff.ToReference<BlueprintBuffReference>(),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Minutes,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 1
					},
					IsNotDispelable = true
				};
				ContextActionApplyBuff applyCasterBuff = new ContextActionApplyBuff
				{
					m_Buff = AppraiseFoeBuff.ToReference<BlueprintBuffReference>(),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Minutes,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 1
					},
					ToCaster = true,
					IsNotDispelable = true
				};
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(applyDebuff, applyCasterBuff);
				});
			});
			AppraisalFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AppraisalFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Appraisal");
				bp.SetDescription(Main.IsekaiContext, "As an otherworlder, you possess an intrinsic HUD-like vision that deciphers the essence and statistics of beings in Golarion. You gain a +2 insight bonus to all Knowledge (Arcana, World) and Lore (Religion, Nature) skill checks, and gain the Appraise Foe active ability.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrueSeeing;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillKnowledgeArcana;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillLoreReligion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillLoreNature;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { AppraiseFoeAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			BlueprintFeature featureToGrant = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookGrandScholarFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grimoire of the Grand Scholar");
				bp.SetDescription(Main.IsekaiContext, "Grants a +4 competence bonus to Knowledge (Arcana, World) and Lore (Religion, Nature), and increases your caster level for all spells by +1.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Book;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeArcana;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillLoreReligion;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillLoreNature;
					c.Value = 4;
				});
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 1;
				});
			});
			ItemSkillBookGrandScholar = CreateSkillBookItem("ItemSkillBookGrandScholar", "Grimoire of the Grand Scholar", "A weighty grimoire bound in cosmic silk containing comprehensive treatises on the arcane patterns of the multiverse. Reading this book permanently grants a +4 competence bonus to Knowledge (Arcana, World) and Lore (Religion, Nature), and a +1 bonus to caster level for all spells.", featureToGrant, Icon_Book);
			BlueprintFeature featureToGrant2 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookPhantomInfiltratorFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Manual of the Phantom Infiltrator");
				bp.SetDescription(Main.IsekaiContext, "Grants a +4 competence bonus to Stealth and Trickery, and deals an additional +1d6 Sneak Attack damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foretell;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillThievery;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SneakAttack;
					c.Value = 1;
				});
			});
			ItemSkillBookPhantomInfiltrator = CreateSkillBookItem("ItemSkillBookPhantomInfiltrator", "Manual of the Phantom Infiltrator", "A shadowy manual bound in nightshade leather detailing spatial bypasses and pressure-point assassinations. Reading this book permanently grants a +4 competence bonus to Stealth and Trickery, and +1d6 Sneak Attack damage.", featureToGrant2, Icon_Foretell);
			BlueprintFeature featureToGrant3 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookAbsoluteCharismaFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chronicle of Absolute Charisma");
				bp.SetDescription(Main.IsekaiContext, "Grants a +4 competence bonus to Persuasion and Use Magic Device, and increases the DC of all Enchantment and Illusion spells by +2.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foretell;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillUseMagicDevice;
					c.Value = 4;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Enchantment;
					c.BonusDC = 2;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Illusion;
					c.BonusDC = 2;
				});
			});
			ItemSkillBookAbsoluteCharisma = CreateSkillBookItem("ItemSkillBookAbsoluteCharisma", "Chronicle of Absolute Charisma", "A gilded chronicle radiating irresistible presence and diplomatic authority. Reading this book permanently grants a +4 competence bonus to Persuasion and Use Magic Device, and increases the DC of Enchantment and Illusion spells by +2.", featureToGrant3, Icon_Foretell);
			BlueprintFeature featureToGrant4 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookWindWalkerFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Codex of the Wind Walker");
				bp.SetDescription(Main.IsekaiContext, "Grants a +4 competence bonus to Athletics and Mobility, and increases base land speed by +10 feet.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Haste;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillAthletics;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillMobility;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 10;
					c.Descriptor = ModifierDescriptor.Enhancement;
				});
			});
			ItemSkillBookWindWalker = CreateSkillBookItem("ItemSkillBookWindWalker", "Codex of the Wind Walker", "A lightweight codex woven from wind-spirit vellum. Reading this book permanently grants a +4 competence bonus to Athletics and Mobility, and increases base land speed by +10 feet.", featureToGrant4, Icon_Haste);
			BlueprintFeature featureToGrant5 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookEyeOfProvidenceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tractate of the Eye of Providence");
				bp.SetDescription(Main.IsekaiContext, "Grants a +4 competence bonus to Perception, and permanent immunity to blindness and the flat-footed condition.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrueSeeing;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Blindness;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Blindness;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Blindness;
				});
				bp.AddComponent(delegate(FlatFootedIgnore c)
				{
					c.Type = FlatFootedIgnoreType.UncannyDodge;
				});
			});
			ItemSkillBookEyeOfProvidence = CreateSkillBookItem("ItemSkillBookEyeOfProvidence", "Tractate of the Eye of Providence", "An all-seeing tractate inscribed with mystic sigils of constant spatial awareness. Reading this book permanently grants a +4 competence bonus to Perception, and grants permanent immunity to blindness and the flat-footed condition.", featureToGrant5, Icon_TrueSeeing);
			BlueprintFeature featureToGrant6 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookFirstCrusadersFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chronicles of the First Crusaders");
				bp.SetDescription(Main.IsekaiContext, "Recovered from the depths of the Shield Maze. Grants a +2 morale bonus to attack rolls and Armor Class against demonic outsiders.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			ItemSkillBookFirstCrusaders = CreateSkillBookItem("ItemSkillBookFirstCrusaders", "Chronicles of the First Crusaders", "Ancient field diaries of the first crusaders buried in the Shield Maze (Lore: Religion DC 20). Reading this grants a +2 morale bonus to attack rolls and AC against demonic enemies.", featureToGrant6, Icon_Shield);
			BlueprintFeature featureToGrant7 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookRadiantWardFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Treatise on Radiant Wards");
				bp.SetDescription(Main.IsekaiContext, "Decoded from the archives of the Gray Garrison. Grants spell resistance equal to 12 plus character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank
					};
				});
				bp.AddContextRankConfig(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 12;
				});
			});
			ItemSkillBookRadiantWard = CreateSkillBookItem("ItemSkillBookRadiantWard", "Treatise on Radiant Wards", "A holy defense treatise recovered from the Gray Garrison (Knowledge: Arcana DC 22). Reading this grants Spell Resistance equal to 12 plus character level.", featureToGrant7, Icon_Shield);
			BlueprintFeature featureToGrant8 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookUnbrokenSoulFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "The Unbroken Soul of Sarkoris");
				bp.SetDescription(Main.IsekaiContext, "Consecrated prayer-scrolls salvaged from the Lost Chapel. Grants permanent immunity to fear, despair, and confusion effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Confusion;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;
				});
			});
			ItemSkillBookUnbrokenSoul = CreateSkillBookItem("ItemSkillBookUnbrokenSoul", "The Unbroken Soul of Sarkoris", "Prayer tablets consecrated amidst the slaughter of the Lost Chapel (Lore: Religion DC 25). Reading this grants permanent immunity to fear, despair, and confusion.", featureToGrant8, Icon_Shield);
			BlueprintBuff PrimalBeastmasterCompanionBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "PrimalBeastmasterCompanionBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Primal Beastmaster");
				bp.SetDescription(Main.IsekaiContext, "Animal companions and summoned allies gain +4 Strength, +4 Constitution, and DR 5/-.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Book;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
				});
			});
			BlueprintFeature featureToGrant9 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookPrimalBeastmasterFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Vespers of the Primal Beastmaster");
				bp.SetDescription(Main.IsekaiContext, "Uncovered in the foul caverns of Leper's Smile. Animal companions and summoned allies gain +4 Strength, +4 Constitution, and DR 5/-.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Book;
				bp.AddComponent(delegate(BuffExtraEffects c)
				{
					c.m_CheckedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("706c182e86d9be848b59ddccca73d13e")?.ToReference<BlueprintBuffReference>();
					c.m_ExtraEffectBuff = PrimalBeastmasterCompanionBuff.ToReference<BlueprintBuffReference>();
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
			});
			ItemSkillBookPrimalBeastmaster = CreateSkillBookItem("ItemSkillBookPrimalBeastmaster", "Vespers of the Primal Beastmaster", "Sarkorian shamanistic runes retrieved from Leper's Smile (Lore: Nature DC 25). Imparts primeval vitality, bolstering companions and physical resilience.", featureToGrant9, Icon_Book);
			BlueprintFeature featureToGrant10 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookSwarmTransmutationFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tome of Swarm Transmutation");
				bp.SetDescription(Main.IsekaiContext, "Recovered from Drezen Citadel's deepest vaults. Grants immunity to swarm damage, poison, and the nauseated and sickened conditions.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Nauseated;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Sickened;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Poison | SpellDescriptor.Sickened | SpellDescriptor.Nauseated;
				});
			});
			ItemSkillBookSwarmTransmutation = CreateSkillBookItem("ItemSkillBookSwarmTransmutation", "Tome of Swarm Transmutation", "A biological dissertation recovered from the Drezen Citadel (Knowledge: World DC 28). Grants permanent immunity to poison, nausea, and swarms.", featureToGrant10, Icon_Shield);
			BlueprintFeature featureToGrant11 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookTechnicLeagueFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Lexicon of the Technic League");
				bp.SetDescription(Main.IsekaiContext, "Extracted from the biomechanical terminals of Blackwater. Your weapons bypass adamantine damage reduction and deal an additional 2d6 electricity damage on hit.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foretell;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula
					{
						m_Dice = DiceType.D6,
						m_Rolls = 2
					};
					c.Element = DamageEnergyType.Electricity;
				});
			});
			ItemSkillBookTechnicLeague = CreateSkillBookItem("ItemSkillBookTechnicLeague", "Lexicon of the Technic League", "Cybernetic data crystals retrieved from Blackwater (Knowledge: Arcana DC 32). Imbues weapon strikes with shocking circuitry and adamantine penetration.", featureToGrant11, Icon_Foretell);
			BlueprintFeature featureToGrant12 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookPlanarInfiltrationFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Guide to Planar Infiltration");
				bp.SetDescription(Main.IsekaiContext, "Discovered in the hidden sanctums of the Ivory Sanctum. Grants a +5 competence bonus to Stealth checks and 20% concealment against ranged attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foretell;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
					c.Descriptor = ConcealmentDescriptor.Blur;
					c.CheckWeaponRangeType = true;
					c.RangeType = WeaponRangeType.Ranged;
				});
			});
			ItemSkillBookPlanarInfiltration = CreateSkillBookItem("ItemSkillBookPlanarInfiltration", "Guide to Planar Infiltration", "Espionage maps and concealment techniques from the Ivory Sanctum (Trickery DC 30). Grants +5 Stealth and 20% ranged concealment.", featureToGrant12, Icon_Foretell);
			BlueprintFeature featureToGrant13 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookAreeluAxiomFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Areelu's Forbidden Axioms");
				bp.SetDescription(Main.IsekaiContext, "Deciphered from Areelu Vorlesh's laboratory in the Midnight Fane. Your spells gain a +2 bonus to DC and bypass all elemental damage resistances.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Book;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(AscendantElement c)
				{
					c.Element = DamageEnergyType.Acid;
				});
				bp.AddComponent(delegate(AscendantElement c)
				{
					c.Element = DamageEnergyType.Cold;
				});
				bp.AddComponent(delegate(AscendantElement c)
				{
					c.Element = DamageEnergyType.Electricity;
				});
				bp.AddComponent(delegate(AscendantElement c)
				{
					c.Element = DamageEnergyType.Fire;
				});
			});
			ItemSkillBookAreeluAxiom = CreateSkillBookItem("ItemSkillBookAreeluAxiom", "Areelu's Forbidden Axioms", "Theoretical planar geometry notes found in the Midnight Fane (Knowledge: Arcana DC 35). Grants +2 spell DC and ignores elemental immunities.", featureToGrant13, Icon_Book);
			BlueprintFeature featureToGrant14 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookMythicMineralogyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Secrets of Mythic Mineralogy");
				bp.SetDescription(Main.IsekaiContext, "Excavated from the Nahyndrian crystal veins in Colyphyr Mines. Grants a +3 natural armor enhancement bonus, DR 5/Epic, and resistance 10 to acid and fire.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmorEnhancement;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
					c.BypassedByEpic = true;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 10;
				});
			});
			ItemSkillBookMythicMineralogy = CreateSkillBookItem("ItemSkillBookMythicMineralogy", "Secrets of Mythic Mineralogy", "Nahyndrian crystalline studies from Colyphyr Mines (Knowledge: World DC 38). Grants +3 natural armor, DR 5/Epic, and elemental resilience.", featureToGrant14, Icon_Shield);
			BlueprintFeature featureToGrant15 = Helpers.CreateBlueprint(Main.IsekaiContext, "SkillBookRunelordLexiconFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Runelord's Imperial Lexicon");
				bp.SetDescription(Main.IsekaiContext, "Decoded from the deepest sanctum of the Ineluctable Prison. Grants a +4 bonus to all saving throws and increases spell penetration by +4.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Book;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 4;
				});
			});
			ItemSkillBookRunelordLexicon = CreateSkillBookItem("ItemSkillBookRunelordLexicon", "Runelord's Imperial Lexicon", "Ancient Thassilonian runic codices retrieved from the Ineluctable Prison (Knowledge: Arcana DC 42). Grants +4 to all saves and +4 Spell Penetration.", featureToGrant15, Icon_Book);
		}

		private static BlueprintItemEquipmentUsable CreateSkillBookItem(string name, string displayName, string description, BlueprintFeature featureToGrant, Sprite icon)
		{
			BlueprintAbility consumeAbility = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Ability", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.CanTargetPoint = false;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionConsumeSkillBook
					{
						m_Feature = featureToGrant.ToReference<BlueprintFeatureReference>(),
						BookName = displayName
					});
				});
			});
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintItemEquipmentUsable bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintItem)bp).m_Icon = icon;
				((BlueprintItem)bp).m_Cost = 10000;
				((BlueprintItem)bp).m_Weight = 1f;
				((BlueprintItem)bp).m_IsNotable = true;
				((BlueprintItem)bp).m_Destructible = false;
				bp.Type = UsableItemType.Other;
				bp.SpendCharges = true;
				bp.Charges = 1;
				bp.RestoreChargesOnRest = false;
				((BlueprintItemEquipment)bp).m_Ability = consumeAbility.ToReference<BlueprintAbilityReference>();
			});
		}
	}
}
