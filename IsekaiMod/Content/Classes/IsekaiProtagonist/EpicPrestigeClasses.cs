using System;
using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Config;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

internal static class EpicPrestigeClasses
{
	private static readonly BlueprintStatProgression BABFull = StaticReferences.StatProgressions.BABFull;

	private static readonly BlueprintStatProgression BABMedium = StaticReferences.StatProgressions.BABMedium;

	private static readonly BlueprintStatProgression BABLow = StaticReferences.StatProgressions.BABLow;

	private static readonly BlueprintStatProgression SavesPrestigeHigh = StaticReferences.StatProgressions.SavesPrestigeHigh;

	private static readonly BlueprintStatProgression SavesPrestigeLow = StaticReferences.StatProgressions.SavesPrestigeLow;

	private static readonly Sprite Icon_Crown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e6e1a24ce33454342b7053889391642c"))?.m_Icon ?? BlueprintTools.GetBlueprint<BlueprintItem>("2bffd039eb8f54847a2e0642db6c9080")?.m_Icon;

	private static readonly Sprite Icon_Sword = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("f5f500d6a2a39fc4181af32ad79af488"))?.m_Icon ?? Icon_Crown;

	private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Crown;

	private static readonly Sprite Icon_Spell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Crown;

	private static readonly Sprite Icon_Dark = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("b8fdf9d9e84c490d8a57d49ed344571e"))?.m_Icon ?? Icon_Crown;

	private const string StunnedBuff = "4397f91671007c5488e9787f5c0e6541";

	public static BlueprintFeature TranscendentProtagonistFeature;

	public static BlueprintFeature AnimeFinalFormFeature;

	public static BlueprintAbility AnimeFinalFormAbility;

	public static BlueprintBuff AnimeFinalFormBuff;

	public static BlueprintProgression IsekaiCapstoneProgression;

	public const string CapstoneProgressionName = "IsekaiCapstoneProgression";

	public static string[] CapstoneCountingClassNames()
	{
		return new string[8] { "IsekaiAscendantClass", "TranscendentSovereignClass", "DemigodPrestigeClass", "GodhunterPrestigeClass", "WarmasterPrestigeClass", "EternalSeekerPrestigeClass", "ArchmagePrestigeClass", "ShadowSovereignPrestigeClass" };
	}

	public static (StatType Stat, int Value) DistinguishingPrerequisite(string className)
	{
		return className switch
		{
			"DemigodPrestigeClass" => (Stat: StatType.Charisma, Value: 19), 
			"GodhunterPrestigeClass" => (Stat: StatType.SkillLoreReligion, Value: 10), 
			"WarmasterPrestigeClass" => (Stat: StatType.BaseAttackBonus, Value: 12), 
			"EternalSeekerPrestigeClass" => (Stat: StatType.SkillKnowledgeWorld, Value: 10), 
			"ArchmagePrestigeClass" => (Stat: StatType.SkillKnowledgeArcana, Value: 12), 
			"ShadowSovereignPrestigeClass" => (Stat: StatType.SkillStealth, Value: 10), 
			_ => (Stat: StatType.BaseAttackBonus, Value: 7), 
		};
	}

	public static bool CapstoneReached(int isekaiClassLevel, int modPrestigeLevels, int capstoneLevel)
	{
		if (isekaiClassLevel <= 0)
		{
			return false;
		}
		return isekaiClassLevel + modPrestigeLevels >= capstoneLevel;
	}

	public static void CreateBlueprints()
	{
		TranscendentProtagonistFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TranscendentProtagonistFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Transcendent Protagonist");
			bp.SetDescription(Main.IsekaiContext, "Upon reaching 30th level, counting your Isekai Protagonist levels together with your levels in any Isekai prestige class, your otherworldly existence transcends the mortal realm entirely. You gain a +4 untyped bonus to all ability scores, a +5 untyped bonus to Armor Class and all saving throws, and 2 additional spell slots per day for every spell level you can cast.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Dexterity;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Intelligence;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Wisdom;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Charisma;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 5;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 5;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 5;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 5;
			});
			bp.AddComponent(delegate(AddSpellsPerDay c)
			{
				c.Amount = 2;
				c.Levels = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			});
		});
		AnimeFinalFormBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "AnimeFinalFormBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Anime Final Form: Absolute Awakening");
			bp.SetDescription(Main.IsekaiContext, "You have ascended into your ultimate, unchained anime form: +10 to attack rolls, +20 to damage, +10 to AC, damage reduction 50/- against physical attacks, and +100 ft movement speed.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(BuffMovementSpeed c)
			{
				c.Value = 100;
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.ContextBonus = new ContextValue();
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 20;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 50;
			});
		});
		BlueprintAbilityResource AnimeFinalFormResource = Helpers.CreateBlueprint(Main.IsekaiContext, "AnimeFinalFormResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 1,
				IncreasedByLevel = false,
				IncreasedByStat = false,
				OtherClassesModifier = 0f
			};
		});
		AnimeFinalFormAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "AnimeFinalFormAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Anime Final Form");
			bp.SetDescription(Main.IsekaiContext, "Once per day as a free action, ascend into your unchained Anime Final Form for 1 minute: gain +10 to attack rolls, +20 to damage, +10 to AC, DR 50/-, and +100 ft movement speed.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.ActionType = UnitCommand.CommandType.Free;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			ContextActionApplyBuff apply = new ContextActionApplyBuff
			{
				m_Buff = AnimeFinalFormBuff.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Minutes,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = 1
				},
				ToCaster = true
			};
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(apply);
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = AnimeFinalFormResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
		});
		AnimeFinalFormFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AnimeFinalFormFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Anime Final Form (Level 40 Capstone)");
			bp.SetDescription(Main.IsekaiContext, "The pinnacle of otherworld supremacy, reached at 40th level counting your Isekai Protagonist levels together with your levels in any Isekai prestige class. Once per day, unleash your complete final form, shattering the boundary between mortal hero and cosmic sovereign.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { AnimeFinalFormAbility.ToReference<BlueprintUnitFactReference>() };
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = AnimeFinalFormResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
		});
		CreateCapstoneProgression();
	}

	private static void CreateCapstoneProgression()
	{
		List<BlueprintProgression.ClassWithLevel> classes = new List<BlueprintProgression.ClassWithLevel>
		{
			new BlueprintProgression.ClassWithLevel
			{
				m_Class = IsekaiProtagonistClass.GetReference(),
				AdditionalLevel = 0
			}
		};
		string[] array = CapstoneCountingClassNames();
		foreach (string text in array)
		{
			BlueprintCharacterClassReference modBlueprintReference = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, text);
			if (modBlueprintReference == null || modBlueprintReference.Guid == BlueprintGuid.Empty)
			{
				Main.IsekaiContext.Logger.LogError("[EpicPrestigeClasses] '" + text + "' has no registered GUID; its levels will not count towards the capstones.");
				continue;
			}
			classes.Add(new BlueprintProgression.ClassWithLevel
			{
				m_Class = modBlueprintReference,
				AdditionalLevel = 0
			});
		}
		IsekaiCapstoneProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiCapstoneProgression", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Otherworld Ascension");
			bp.SetDescription(Main.IsekaiContext, "The two capstones of the otherworldly path arrive at 30th and 40th level, counting your Isekai Protagonist levels together with your levels in any Isekai prestige class.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			((BlueprintUnitFact)bp).m_AllowNonContextActions = false;
			bp.m_Classes = classes.ToArray();
			bp.LevelEntries = new LevelEntry[2]
			{
				Helpers.CreateLevelEntry(30, TranscendentProtagonistFeature),
				Helpers.CreateLevelEntry(40, AnimeFinalFormFeature)
			};
		});
	}

	public static void Add()
	{
		BlueprintFeatureSelection isekaiBonus = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
		BlueprintFeatureSelection specialPower = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
		BlueprintFeatureSelection opAbilitySelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection");
		PrestigeIdentityFeatures.CreateEpic(Icon_Crown, Icon_Sword, Icon_Shield, Icon_Spell, Icon_Dark);
		CreateEpicPrestige("DemigodPrestigeClass", "DemigodProgression", "The Demigod", "Somewhere between the first cheat skill and the tenth, people started praying to you. The Demigod stops pretending to be mortal and lets the world decide what to worship. Every level is another step of the apotheosis: wounds close on their own, fear and stunning slide off, mortals feel the weight of your regard, and at the end the spark becomes a god that will not die.\n\nRequires 10 levels of Isekai Protagonist and Charisma 19. Every level advances the Isekai spellbook you choose at 1st level. Divine Spark (1st): +2 to every ability score and spell resistance 15 + your character level. Divine Portfolio (2nd, 4th, 6th, 8th and 9th): +1 to AC and all saving throws per rank, up to +5. Mantle of Divinity (3rd): damage reduction 10/- and fast healing 10. Divine Majesty (5th): +4 Charisma and +10 Persuasion. Salient Divine Ability (7th): immunity to fear, paralysis and stunning, and +10 spell resistance. Immortal Ascension (10th): immunity to death, bleed, petrification and harmful mind-affecting effects, and a further +4 to every ability score. You also gain an Isekai bonus feat at every even level, an Overpowered Ability at 5th and a Special Power at 9th.", "Isekai prestige class. Your plot armor becomes divinity: defenses that stack with every rank, fast healing, immunity to fear and death, and +6 to every ability score by 10th level.", DiceType.D12, BABFull, SavesPrestigeHigh, SavesPrestigeHigh, SavesPrestigeHigh, Icon_Crown, delegate(int level)
		{
			List<BlueprintFeature> list = new List<BlueprintFeature>();
			switch (level)
			{
			case 1:
			{
				BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "DemigodSparkFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Divine Spark");
					b.SetDescription(Main.IsekaiContext, "You possess a burgeoning divine spark, gaining a +2 inherent bonus to all ability scores and spell resistance equal to 15 + your character level.");
					((BlueprintUnitFact)b).m_Icon = Icon_Crown;
					b.AddComponent(delegate(ContextRankConfig c)
					{
						c.m_Type = AbilityRankType.Default;
						c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
						c.m_Progression = ContextRankProgression.BonusValue;
						c.m_StepLevel = 15;
					});
					b.AddComponent(delegate(AddSpellResistance c)
					{
						c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					});
					b.AddComponent(delegate(AddStackingStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Inherent;
						c.Stat = StatType.Strength;
						c.Value = 2;
					});
					b.AddComponent(delegate(AddStackingStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Inherent;
						c.Stat = StatType.Dexterity;
						c.Value = 2;
					});
					b.AddComponent(delegate(AddStackingStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Inherent;
						c.Stat = StatType.Constitution;
						c.Value = 2;
					});
					b.AddComponent(delegate(AddStackingStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Inherent;
						c.Stat = StatType.Intelligence;
						c.Value = 2;
					});
					b.AddComponent(delegate(AddStackingStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Inherent;
						c.Stat = StatType.Wisdom;
						c.Value = 2;
					});
					b.AddComponent(delegate(AddStackingStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Inherent;
						c.Stat = StatType.Charisma;
						c.Value = 2;
					});
				});
				list.Add(item);
				break;
			}
			case 5:
				if (opAbilitySelection != null)
				{
					list.Add(opAbilitySelection);
				}
				else
				{
					list.Add(specialPower);
				}
				break;
			default:
				if (level % 2 == 0)
				{
					list.Add(isekaiBonus);
				}
				else if (level == 9)
				{
					list.Add(specialPower);
				}
				break;
			}
			if (level == 10)
			{
				BlueprintFeature item2 = Helpers.CreateBlueprint(Main.IsekaiContext, "DemigodImmortalFormFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Immortal Ascension");
					b.SetDescription(Main.IsekaiContext, "You achieve full apotheosis: permanent immunity to death, bleed, petrification, and harmful mind-affecting effects, and a further +4 untyped bonus to all ability scores that stacks with your Divine Spark.");
					((BlueprintUnitFact)b).m_Icon = Icon_Crown;
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.Strength;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.Dexterity;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.Constitution;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.Intelligence;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.Wisdom;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.Charisma;
						c.Value = 4;
					});
					SpellDescriptor apotheosisDescriptors = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion | SpellDescriptor.Death | SpellDescriptor.Sleep | SpellDescriptor.Bleed | SpellDescriptor.Petrified | SpellDescriptor.NegativeEmotion;
					b.AddComponent(delegate(BuffDescriptorImmunity c)
					{
						c.Descriptor = apotheosisDescriptors;
					});
					b.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
					{
						c.Descriptor = apotheosisDescriptors;
					});
				});
				list.Add(item2);
			}
			return list.ToArray();
		});
		CreateEpicPrestige("GodhunterPrestigeClass", "GodhunterProgression", "The Godhunter", "Some protagonists are summoned by a god. Some notice afterwards who did the summoning and decide to have a word. The Godhunter studies the divine the way a butcher studies cattle: every hunt hardens the will, every level teaches the blade where heavenly flesh parts, and the road ends with a strike that tears the blessings off a demon lord mid-sentence.\n\nRequires 10 levels of Isekai Protagonist and 10 ranks in Lore (Religion). Every level advances the Isekai spellbook you choose at 1st level. Deicide Strikes (1st): +3d6 damage on weapon attacks, which also count as good, evil, lawful and chaotic against damage reduction. Heretic's Resolve (2nd, 4th, 6th, 8th and 9th): +1 to attack rolls and Will saves per rank, up to +5. Faithless Ward (3rd): +10 spell resistance and +4 to Will saves. Godslayer's Edge (5th): a further +2d6 weapon damage and +10 to confirm critical hits. Unbowed Before Heaven (7th): immunity to charm, compulsion and fear. Sever Divinity (10th): +4 to attack rolls, and your critical hits strip the target's strongest dispellable effect and stun it for 1 round. You also gain an Isekai bonus feat at every even level, an Overpowered Ability at 5th and a Special Power at 9th.", "Isekai prestige class for hunting gods and demon lords: heavy bonus weapon damage that pierces every alignment, spell resistance, and a will no divinity can bend.", DiceType.D10, BABFull, SavesPrestigeHigh, SavesPrestigeHigh, SavesPrestigeLow, Icon_Sword, delegate(int level)
		{
			List<BlueprintFeature> list = new List<BlueprintFeature>();
			switch (level)
			{
			case 1:
			{
				BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "GodhunterDeicideFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Deicide Strikes");
					b.SetDescription(Main.IsekaiContext, "Your weapons are consecrated to tear through celestial and fiendish wards. Your weapon attacks deal an additional 3d6 untyped damage and count as good-, evil-, lawful- and chaotic-aligned for the purpose of overcoming damage reduction.");
					((BlueprintUnitFact)b).m_Icon = Icon_Sword;
					b.AddComponent(delegate(AddAdditionalWeaponDamage c)
					{
						c.DamageType = new DamageTypeDescription
						{
							Type = DamageType.Direct
						};
						c.Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = 3,
							BonusValue = 0
						};
					});
					b.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
					{
						c.AddAlignment = true;
						c.Alignment = DamageAlignment.Good | DamageAlignment.Evil | DamageAlignment.Chaotic | DamageAlignment.Lawful;
					});
				});
				list.Add(item);
				break;
			}
			case 5:
				if (opAbilitySelection != null)
				{
					list.Add(opAbilitySelection);
				}
				else
				{
					list.Add(specialPower);
				}
				break;
			default:
				if (level % 2 == 0)
				{
					list.Add(isekaiBonus);
				}
				else if (level == 9)
				{
					list.Add(specialPower);
				}
				break;
			}
			if (level == 10)
			{
				BlueprintFeature item2 = Helpers.CreateBlueprint(Main.IsekaiContext, "GodhunterSeverDivinityFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Sever Divinity");
					b.SetDescription(Main.IsekaiContext, "You gain a +4 untyped bonus to attack rolls, and your critical hits strip the highest-level dispellable effect from the target and stun it for 1 round with no save.");
					((BlueprintUnitFact)b).m_Icon = Icon_Sword;
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.AdditionalAttackBonus;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
					{
						c.OnlyHit = true;
						c.CriticalHit = true;
						c.Action = Helpers.CreateActionList(new ContextActionDispelMagic
						{
							m_BuffType = ContextActionDispelMagic.BuffType.All,
							m_CheckType = RuleDispelMagic.CheckType.CasterLevel,
							m_StopAfterCountRemoved = true,
							m_CountToRemove = 1,
							m_MaxSpellLevel = 10,
							OnlyTargetEnemyBuffs = true,
							OnlyEnemyAreaEffects = true,
							OnSuccess = ActionFlow.DoNothing(),
							OnFail = ActionFlow.DoNothing(),
							ContextBonus = 0
						}, new ContextActionApplyBuff
						{
							m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4397f91671007c5488e9787f5c0e6541"),
							DurationValue = Values.Duration.OneRound
						});
					});
				});
				list.Add(item2);
			}
			return list.ToArray();
		});
		CreateEpicPrestige("WarmasterPrestigeClass", "WarmasterProgression", "The Warmaster", "Every isekai hero ends up leading an army sooner or later. The Warmaster is the one who actually read the manual. A general who fights in the front rank, the Warmaster lends the whole line nerve and edge, shrugs off wounds that would drop a company, breaks formations with main strength, and in the end strikes as often as two soldiers put together.\n\nRequires 10 levels of Isekai Protagonist and a base attack bonus of +12; a pure Isekai Protagonist reaches +12 at character level 12, so that is the earliest it can be taken. Every level advances the Isekai spellbook you choose at 1st level. Tactical Supremacy (1st): a 30-foot aura you can switch on that gives you and your allies +4 to attack rolls, weapon damage and AC. Battlefield Command (2nd, 4th, 6th, 8th and 9th): +1 to AC and damage rolls per rank, up to +5. Veteran's Grit (3rd): damage reduction 5/- and +4 Constitution. Crushing Formation (5th): +4 to combat maneuver bonus and defense. Unbreakable Line (7th): immunity to fear and 50% fortification. Apex Conqueror (10th): two extra attacks in a full attack, stacking with haste. You also gain an Isekai bonus feat at every even level, an Overpowered Ability at 5th and a Special Power at 9th.", "Isekai prestige class for front-line commanders: an aura that raises the whole party's attack, damage and AC, a body that refuses to fall, and two extra attacks at 10th level.", DiceType.D12, BABFull, SavesPrestigeHigh, SavesPrestigeLow, SavesPrestigeHigh, Icon_Shield, delegate(int level)
		{
			List<BlueprintFeature> list = new List<BlueprintFeature>();
			switch (level)
			{
			case 1:
			{
				BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("WarmasterSupremacy", "You project a 30-foot aura of command. You and all allies within it gain a +4 untyped bonus to attack rolls, weapon damage, and AC.", "Allies within the Warmaster's aura gain a +4 untyped bonus to attack rolls, weapon damage, and AC.", Icon_Shield, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(30f), affectEnemies: false, delegate(BlueprintBuff b)
				{
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.AdditionalAttackBonus;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.AdditionalDamage;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.AC;
						c.Value = 4;
					});
				});
				blueprintFeature.SetName(Main.IsekaiContext, "Tactical Supremacy");
				list.Add(blueprintFeature);
				break;
			}
			case 5:
				if (opAbilitySelection != null)
				{
					list.Add(opAbilitySelection);
				}
				else
				{
					list.Add(specialPower);
				}
				break;
			default:
				if (level % 2 == 0)
				{
					list.Add(isekaiBonus);
				}
				else if (level == 9)
				{
					list.Add(specialPower);
				}
				break;
			}
			if (level == 10)
			{
				BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "WarmasterApexConquerorFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Apex Conqueror");
					b.SetDescription(Main.IsekaiContext, "You gain 2 additional attacks during a full attack. These stack with haste.");
					((BlueprintUnitFact)b).m_Icon = Icon_Shield;
					b.AddComponent(delegate(BuffExtraAttack c)
					{
						c.Number = 2;
						c.Haste = false;
					});
				});
				list.Add(item);
			}
			return list.ToArray();
		});
		CreateEpicPrestige("EternalSeekerPrestigeClass", "EternalSeekerProgression", "The Eternal Seeker", "Being reincarnated once is an accident. The Eternal Seeker has lost count. Every life glimpsed in passing left something behind: a skill, a reflex, a spell that half remembers being cast. The Seeker borrows from all of them, sees the blow that lands in the timeline next door before it lands in this one, and in the end perceives every timeline at once.\n\nRequires 10 levels of Isekai Protagonist and 10 ranks in Knowledge (World). Every level advances the Isekai spellbook you choose at 1st level. Multiverse Scholar (1st): +10 to all skill checks, and your Intelligence modifier is added to all saving throws. Borrowed Destiny (2nd, 4th, 6th, 8th and 9th): +1 to all skill checks and initiative per rank, up to +5. Glimpse of Timelines (3rd): +2 to AC and Reflex saves and +4 to initiative. Scholar's Grasp (5th): +1 caster level and +2 to spell penetration. Echoes of Every Path (7th): +2 to your spell save DCs and +2 to attack rolls. Omniscient Transcendent (10th): you cannot be flanked, you roll every d20 twice and keep the better result, and your spell save DCs rise by 4. You also gain an Isekai bonus feat at every even level, an Overpowered Ability at 5th and a Special Power at 9th.", "Isekai prestige class for the reincarnated polymath: huge skill bonuses, your Intelligence added to every save, faster initiative, and at 10th level every d20 rolled twice.", DiceType.D8, BABMedium, SavesPrestigeHigh, SavesPrestigeHigh, SavesPrestigeHigh, Icon_Spell, delegate(int level)
		{
			List<BlueprintFeature> list = new List<BlueprintFeature>();
			switch (level)
			{
			case 1:
			{
				BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "EternalSeekerMultiverseFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Multiverse Scholar");
					b.SetDescription(Main.IsekaiContext, "You gain a +10 competence bonus to all skill checks, and add your Intelligence modifier to all saving throws.");
					((BlueprintUnitFact)b).m_Icon = Icon_Spell;
					StatType[] array = new StatType[11]
					{
						StatType.SkillAthletics,
						StatType.SkillMobility,
						StatType.SkillThievery,
						StatType.SkillStealth,
						StatType.SkillKnowledgeArcana,
						StatType.SkillKnowledgeWorld,
						StatType.SkillLoreNature,
						StatType.SkillLoreReligion,
						StatType.SkillPerception,
						StatType.SkillPersuasion,
						StatType.SkillUseMagicDevice
					};
					foreach (StatType skill in array)
					{
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = skill;
							c.Value = 10;
						});
					}
					array = new StatType[3]
					{
						StatType.SaveFortitude,
						StatType.SaveReflex,
						StatType.SaveWill
					};
					foreach (StatType save in array)
					{
						b.AddComponent(delegate(DerivativeStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.UntypedStackable;
							c.BaseStat = StatType.Intelligence;
							c.DerivativeStat = save;
						});
					}
					b.AddComponent(delegate(RecalculateOnStatChange c)
					{
						c.Stat = StatType.Intelligence;
					});
				});
				list.Add(item);
				break;
			}
			case 5:
				if (opAbilitySelection != null)
				{
					list.Add(opAbilitySelection);
				}
				else
				{
					list.Add(specialPower);
				}
				break;
			default:
				if (level % 2 == 0)
				{
					list.Add(isekaiBonus);
				}
				else if (level == 9)
				{
					list.Add(specialPower);
				}
				break;
			}
			if (level == 10)
			{
				BlueprintFeature item2 = Helpers.CreateBlueprint(Main.IsekaiContext, "EternalSeekerOmniscientFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Omniscient Transcendent");
					b.SetDescription(Main.IsekaiContext, "You perceive all timelines simultaneously: you cannot be flanked, you roll every d20 twice and take the better result, and you gain a +4 bonus to all spell save DCs.");
					((BlueprintUnitFact)b).m_Icon = Icon_Spell;
					b.AddComponent(delegate(ModifyD20 c)
					{
						c.Rule = RuleType.All;
						c.RollsAmount = 1;
						c.TakeBest = true;
					});
					b.AddComponent(delegate(AddMechanicsFeature c)
					{
						c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked;
					});
					b.AddComponent(delegate(IncreaseAllSpellsDC c)
					{
						c.Value = 4;
						c.Descriptor = ModifierDescriptor.UntypedStackable;
					});
				});
				list.Add(item2);
			}
			return list.ToArray();
		});
		CreateEpicPrestige("ArchmagePrestigeClass", "ArchmageProgression", "The Archmage", "Magic was the first thing in this world that made sense, and the Archmage never stopped studying it. Each level deepens the reservoir the spells are drawn from and sharpens how they are shaped, until at the summit every spell leaves the hand empowered and maximized at no cost at all.\n\nRequires 10 levels of Isekai Protagonist and 12 ranks in Knowledge (Arcana), which cannot be reached before character level 12. Every level advances the Isekai spellbook you choose at 1st level. Spellfire Supremacy (1st): your damaging spells deal +2 damage per damage die, and +4 to spell penetration. Arcane Reservoir (2nd, 4th, 6th, 8th and 9th): +1 to Intelligence, Wisdom and Charisma per rank, up to +5. Arcane Reserves (3rd): one extra spell slot per day at every spell level from 1st to 9th. Mastery of Shaping (5th): +2 caster level. Arcane Fire (7th): a further +4 to spell penetration and +2 to your spell save DCs. Ninth-Circle Overload (10th): every spell you cast is Empowered and Maximized for free, and your spell save DCs rise by 4. You also gain an Isekai bonus feat at every even level, an Overpowered Ability at 5th and a Special Power at 9th.", "Isekai prestige class for pure spellcasters: extra spell slots, caster level, penetration and save DCs, and at 10th level every spell Empowered and Maximized for free.", DiceType.D6, BABLow, SavesPrestigeLow, SavesPrestigeLow, SavesPrestigeHigh, Icon_Spell, delegate(int level)
		{
			List<BlueprintFeature> list = new List<BlueprintFeature>();
			switch (level)
			{
			case 1:
			{
				BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "ArchmageSpellfireFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Spellfire Supremacy");
					b.SetDescription(Main.IsekaiContext, "Your damaging spells deal an additional +2 damage per damage die rolled, and your spell penetration increases by +4.");
					((BlueprintUnitFact)b).m_Icon = Icon_Spell;
					b.AddComponent(delegate(SpellPenetrationBonus c)
					{
						c.Value = 4;
					});
					b.AddComponent(delegate(PromoteSpellDices c)
					{
						c.MinDice = DiceType.Zero;
						c.Bonus = 2;
					});
				});
				list.Add(item);
				break;
			}
			case 5:
				if (opAbilitySelection != null)
				{
					list.Add(opAbilitySelection);
				}
				else
				{
					list.Add(specialPower);
				}
				break;
			default:
				if (level % 2 == 0)
				{
					list.Add(isekaiBonus);
				}
				else if (level == 9)
				{
					list.Add(specialPower);
				}
				break;
			}
			if (level == 10)
			{
				BlueprintFeature item2 = Helpers.CreateBlueprint(Main.IsekaiContext, "ArchmageOverloadFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Ninth-Circle Overload");
					b.SetDescription(Main.IsekaiContext, "Every spell you cast is automatically Empowered and Maximized, with no increase to its casting time or to the spell slot it is cast from. You also gain a +4 bonus to all spell save DCs.");
					((BlueprintUnitFact)b).m_Icon = Icon_Spell;
					b.AddComponent(delegate(AutoMetamagic c)
					{
						c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
						c.Metamagic = Metamagic.Empower | Metamagic.Maximize;
					});
					b.AddComponent(delegate(IncreaseAllSpellsDC c)
					{
						c.Value = 4;
						c.Descriptor = ModifierDescriptor.UntypedStackable;
					});
				});
				list.Add(item2);
			}
			return list.ToArray();
		});
		CreateEpicPrestige("ShadowSovereignPrestigeClass", "ShadowSovereignProgression", "The Shadow Sovereign", "Only a Shadow Monarch can walk this path. The army that rose from the fallen was the beginning; the Shadow Sovereign drills it into a legion. The monarch's domain spreads to every soldier summoned, the shadows cling close enough to turn blades aside, the cold of the void becomes home, and in the end the monarch is the Shadow Realm made flesh.\n\nRequires 10 levels of Isekai Protagonist as a Shadow Monarch and 10 ranks in Stealth. Every level advances the Isekai spellbook you choose at 1st level. Monarch's Expanded Territory (1st): every creature you summon gains +4 Strength, Dexterity and Constitution, and you gain +4 to attack rolls. Army of Shadows (2nd, 4th, 6th, 8th and 9th): +1 to attack rolls and AC per rank, up to +5. Veil of the Monarch (3rd): attacks against you have a 20% miss chance, and +10 to Stealth. Monarch's Legion (5th): your summons gain +4 to attack rolls and AC. Void Resilience (7th): cold and negative energy resistance 30. Monarch Incarnate (10th): immunity to critical hits, sneak attacks, death effects and negative levels, and +4d6 negative energy damage on weapon attacks. You also gain an Isekai bonus feat at every even level, an Overpowered Ability at 5th and a Special Power at 9th.", "The Shadow Monarch's own prestige class: a stronger, drilled shadow army, concealment and void resistances, and at 10th level immunity to critical hits and death.", DiceType.D10, BABFull, SavesPrestigeLow, SavesPrestigeHigh, SavesPrestigeHigh, Icon_Dark, delegate(int level)
		{
			List<BlueprintFeature> list = new List<BlueprintFeature>();
			switch (level)
			{
			case 1:
			{
				BlueprintBuff summonBuff = TTCoreExtensions.CreateBuff("ShadowSovereignSummonBuff", delegate(BlueprintBuff b)
				{
					b.SetName(Main.IsekaiContext, "Monarch's Expanded Territory");
					b.SetDescription(Main.IsekaiContext, "Raised under an expanded shadow domain: +4 profane bonus to Strength, Dexterity and Constitution.");
					((BlueprintUnitFact)b).m_Icon = Icon_Dark;
					b.IsClassFeature = true;
					b.Stacking = StackingType.Replace;
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Profane;
						c.Stat = StatType.Strength;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Profane;
						c.Stat = StatType.Dexterity;
						c.Value = 4;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Profane;
						c.Stat = StatType.Constitution;
						c.Value = 4;
					});
				});
				BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowSovereignDomainFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Monarch's Expanded Territory");
					b.SetDescription(Main.IsekaiContext, "Your shadow domain expands. Every creature you summon gains a +4 profane bonus to Strength, Dexterity and Constitution, and you gain a +4 untyped bonus to attack rolls.");
					((BlueprintUnitFact)b).m_Icon = Icon_Dark;
					b.AddComponent(delegate(OnSpawnBuff c)
					{
						c.m_buff = summonBuff.ToReference<BlueprintBuffReference>();
						c.IsInfinity = true;
					});
					b.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = StatType.AdditionalAttackBonus;
						c.Value = 4;
					});
				});
				list.Add(item);
				break;
			}
			case 5:
				if (opAbilitySelection != null)
				{
					list.Add(opAbilitySelection);
				}
				else
				{
					list.Add(specialPower);
				}
				break;
			default:
				if (level % 2 == 0)
				{
					list.Add(isekaiBonus);
				}
				else if (level == 9)
				{
					list.Add(specialPower);
				}
				break;
			}
			if (level == 10)
			{
				BlueprintFeature item2 = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowSovereignIncarnateFeature", delegate(BlueprintFeature b)
				{
					b.SetName(Main.IsekaiContext, "Monarch Incarnate");
					b.SetDescription(Main.IsekaiContext, "You become the living personification of the Shadow Realm: immunity to critical hits and sneak attacks, to death effects and to negative levels, and your weapon attacks deal an additional 4d6 negative energy damage.");
					((BlueprintUnitFact)b).m_Icon = Icon_Dark;
					b.AddComponent<AddImmunityToCriticalHits>();
					b.AddComponent(delegate(BuffDescriptorImmunity c)
					{
						c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
					});
					b.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
					{
						c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
					});
					b.AddComponent<AddImmunityToEnergyDrain>();
					b.AddComponent(delegate(AddAdditionalWeaponDamage c)
					{
						c.DamageType = new DamageTypeDescription
						{
							Type = DamageType.Energy,
							Energy = DamageEnergyType.NegativeEnergy
						};
						c.Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = 4,
							BonusValue = 0
						};
					});
				});
				list.Add(item2);
			}
			return list.ToArray();
		}, delegate(BlueprintCharacterClass bp)
		{
			BlueprintFeature ShadowMonarchProficiencies = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies");
			if (ShadowMonarchProficiencies != null)
			{
				bp.AddComponent(delegate(PrerequisiteFeature c)
				{
					c.m_Feature = ShadowMonarchProficiencies.ToReference<BlueprintFeatureReference>();
					c.Group = Prerequisite.GroupType.All;
				});
			}
		});
	}

	private static void CreateEpicPrestige(string className, string progressionName, string displayName, string description, string descriptionShort, DiceType hitDie, BlueprintStatProgression bab, BlueprintStatProgression fort, BlueprintStatProgression @ref, BlueprintStatProgression will, Sprite icon, Func<int, BlueprintFeature[]> getLevelFeatures, Action<BlueprintCharacterClass> extraConfig = null)
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
		BlueprintCharacterClass cls = Helpers.CreateBlueprint(Main.IsekaiContext, className, delegate(BlueprintCharacterClass bp)
		{
			bp.LocalizedName = Helpers.CreateString(Main.IsekaiContext, className + ".Name", displayName);
			bp.LocalizedDescription = Helpers.CreateString(Main.IsekaiContext, className + ".Description", description);
			bp.LocalizedDescriptionShort = Helpers.CreateString(Main.IsekaiContext, className + ".DescriptionShort", descriptionShort);
			bp.HitDie = hitDie;
			bp.PrestigeClass = true;
			bp.m_BaseAttackBonus = bab.ToReference<BlueprintStatProgressionReference>();
			bp.m_FortitudeSave = fort.ToReference<BlueprintStatProgressionReference>();
			bp.m_ReflexSave = @ref.ToReference<BlueprintStatProgressionReference>();
			bp.m_WillSave = will.ToReference<BlueprintStatProgressionReference>();
			bp.m_Difficulty = 1;
			bp.SkillPoints = 4;
			bp.ClassSkills = new StatType[11]
			{
				StatType.SkillAthletics,
				StatType.SkillMobility,
				StatType.SkillThievery,
				StatType.SkillStealth,
				StatType.SkillKnowledgeArcana,
				StatType.SkillKnowledgeWorld,
				StatType.SkillLoreNature,
				StatType.SkillLoreReligion,
				StatType.SkillPerception,
				StatType.SkillPersuasion,
				StatType.SkillUseMagicDevice
			};
			bp.IsDivineCaster = false;
			bp.IsArcaneCaster = false;
			bp.PrimaryColor = 9;
			bp.SecondaryColor = 9;
			if (clothesClass != null)
			{
				bp.MaleEquipmentEntities = clothesClass.MaleEquipmentEntities;
				bp.FemaleEquipmentEntities = clothesClass.FemaleEquipmentEntities;
			}
			bp.m_Archetypes = new BlueprintArchetypeReference[0];
			bp.AddComponent(delegate(PrerequisiteClassLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.Level = 10;
			});
			(StatType Stat, int Value) gate = DistinguishingPrerequisite(className);
			bp.AddComponent(delegate(PrerequisiteStatValue c)
			{
				c.Stat = gate.Stat;
				c.Value = gate.Value;
			});
			extraConfig?.Invoke(bp);
		});
		BlueprintProgression blueprintProgression = Helpers.CreateBlueprint(Main.IsekaiContext, progressionName, delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			bp.IsClassFeature = true;
			bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
			{
				new BlueprintProgression.ClassWithLevel
				{
					m_Class = cls.ToReference<BlueprintCharacterClassReference>(),
					AdditionalLevel = 0
				}
			};
		});
		BlueprintFeatureSelection isekaiPrestigeSpellbookSelection = PrestigeClassReplaceSpellbook.GetIsekaiPrestigeSpellbookSelection();
		LevelEntry[] array = new LevelEntry[10];
		for (int num3 = 1; num3 <= 10; num3++)
		{
			BlueprintFeature[] array2 = getLevelFeatures(num3);
			List<BlueprintFeature> list = new List<BlueprintFeature>(PrestigeIdentityFeatures.EpicIdentityAt(className, num3));
			if (list.Count > 0)
			{
				list.InsertRange(0, array2);
				array2 = list.ToArray();
			}
			if (num3 == 1 && isekaiPrestigeSpellbookSelection != null)
			{
				BlueprintFeature[] array3 = new BlueprintFeature[array2.Length + 1];
				Array.Copy(array2, array3, array2.Length);
				array3[array2.Length] = isekaiPrestigeSpellbookSelection;
				array2 = array3;
			}
			int num4 = num3 - 1;
			int level = num3;
			BlueprintFeatureBase[] features = array2;
			array[num4] = Helpers.CreateLevelEntry(level, features);
		}
		blueprintProgression.LevelEntries = array;
		cls.m_Progression = blueprintProgression.ToReference<BlueprintProgressionReference>();
		ContentOffering.RegisterClass(cls, OfferedContent.IsekaiProtagonist);
	}
}
