using System;
using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	internal static class EpicPrestigeClasses
	{
		private static readonly BlueprintStatProgression BABFull = BlueprintTools.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");

		private static readonly BlueprintStatProgression BABMedium = BlueprintTools.GetBlueprint<BlueprintStatProgression>("4c936de4249b61e419a3fb775b9f2581");

		private static readonly BlueprintStatProgression BABLow = BlueprintTools.GetBlueprint<BlueprintStatProgression>("0538081888b2d8c41893d25d098dee99");

		private static readonly BlueprintStatProgression SavesHigh = BlueprintTools.GetBlueprint<BlueprintStatProgression>("ff4662bde9e75f145853417313842751");

		private static readonly BlueprintStatProgression SavesLow = BlueprintTools.GetBlueprint<BlueprintStatProgression>("dc0c7c1aba755c54f96c089cdf7d14a3");

		private static readonly Sprite Icon_Crown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bdddaa78f8795024081f2d1eb8b4bd78"))?.m_Icon;

		private static readonly Sprite Icon_Sword = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("a38824af2ce2ee845b3592f9533a6056"))?.m_Icon ?? Icon_Crown;

		private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Crown;

		private static readonly Sprite Icon_Spell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Crown;

		private static readonly Sprite Icon_Dark = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9eda82a1f78558747a03c17e0e9a1a68"))?.m_Icon ?? Icon_Crown;

		public static BlueprintFeature TranscendentProtagonistFeature;

		public static BlueprintFeature AnimeFinalFormFeature;

		public static BlueprintAbility AnimeFinalFormAbility;

		public static BlueprintBuff AnimeFinalFormBuff;

		public static void Add()
		{
			BlueprintFeatureSelection isekaiBonus = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
			BlueprintFeatureSelection specialPower = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
			BlueprintFeatureSelection opAbilitySelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection");
			TranscendentProtagonistFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TranscendentProtagonistFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendent Protagonist");
				bp.SetDescription(Main.IsekaiContext, "Upon reaching 30th level, your otherworldly existence transcends the mortal realm entirely. You gain a +4 untyped bonus to all ability scores, a +5 luck bonus to Armor Class and all saving throws, and 2 additional spell slots per day for every spell tier you can cast.");
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
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 5;
				});
			});
			AnimeFinalFormBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "AnimeFinalFormBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Anime Final Form: Absolute Awakening");
				bp.SetDescription(Main.IsekaiContext, "You have ascended into your ultimate, unchained anime form: completely immune to all damage, attacks automatically score critical hits, base speed increases by +100 feet, and all spells are cast as free actions.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 100;
					c.Descriptor = ModifierDescriptor.Enhancement;
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
			});
			AnimeFinalFormFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AnimeFinalFormFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Anime Final Form (Level 40 Capstone)");
				bp.SetDescription(Main.IsekaiContext, "The pinnacle of otherworld supremacy. Once per day, unleash your complete final form, shattering the boundary between mortal hero and cosmic sovereign.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Crown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { AnimeFinalFormAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			CreateEpicPrestige("DemigodPrestigeClass", "DemigodProgression", "The Demigod", "A mortal vessel ascending to divine status. Channeling direct celestial or abyssal divinity, the Demigod wields transcendent divine authority, unyielding immortal durability, and absolute presence.", DiceType.D12, BABFull, SavesHigh, SavesHigh, SavesHigh, Icon_Crown, delegate(int level)
			{
				List<BlueprintFeature> list = new List<BlueprintFeature>();
				switch (level)
				{
				case 1:
				{
					BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "DemigodSparkFeature", delegate(BlueprintFeature b)
					{
						b.SetName(Main.IsekaiContext, "Divine Spark");
						b.SetDescription(Main.IsekaiContext, "You possess a burgeoning divine spark, gaining a +2 inherent bonus to all ability scores and spell resistance 15 + level.");
						((BlueprintUnitFact)b).m_Icon = Icon_Crown;
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Strength;
							c.Value = 2;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Dexterity;
							c.Value = 2;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Constitution;
							c.Value = 2;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Intelligence;
							c.Value = 2;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Wisdom;
							c.Value = 2;
						});
						b.AddComponent(delegate(AddStatBonus c)
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
					else
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
						b.SetDescription(Main.IsekaiContext, "You achieve full apotheosis: permanent immunity to death, bleed, petrification, and mind-affecting effects, and an additional +4 to all ability scores.");
						// Immunities named in the description.
						b.AddComponent(delegate(BuffDescriptorImmunity c)
						{
							c.Descriptor = SpellDescriptor.Death | SpellDescriptor.Bleed | SpellDescriptor.Petrified | SpellDescriptor.MindAffecting;
						});
						b.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
						{
							c.Descriptor = SpellDescriptor.Death | SpellDescriptor.Bleed | SpellDescriptor.Petrified | SpellDescriptor.MindAffecting;
						});
						((BlueprintUnitFact)b).m_Icon = Icon_Crown;
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Strength;
							c.Value = 4;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Dexterity;
							c.Value = 4;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Constitution;
							c.Value = 4;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Intelligence;
							c.Value = 4;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Wisdom;
							c.Value = 4;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Inherent;
							c.Stat = StatType.Charisma;
							c.Value = 4;
						});
					});
					list.Add(item2);
				}
				return list.ToArray();
			});
			CreateEpicPrestige("GodhunterPrestigeClass", "GodhunterProgression", "The Godhunter", "A relentless slayer whose entire martial discipline is engineered to tear down divine entities, demon lords, and planar deities. Their strikes sever divine conduits and unravel protective wards.", DiceType.D10, BABFull, SavesHigh, SavesHigh, SavesLow, Icon_Sword, delegate(int level)
			{
				List<BlueprintFeature> list = new List<BlueprintFeature>();
				switch (level)
				{
				case 1:
				{
					BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "GodhunterDeicideFeature", delegate(BlueprintFeature b)
					{
						b.SetName(Main.IsekaiContext, "Deicide Strikes");
						b.SetDescription(Main.IsekaiContext, "Your weapons are consecrated to tear through celestial and fiendish wards. Attacks deal an additional +3d6 direct damage against outsiders and dragons, and bypass all alignment-based damage reduction.");
						((BlueprintUnitFact)b).m_Icon = Icon_Sword;
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.UntypedStackable;
							c.Stat = StatType.AdditionalDamage;
							c.Value = 6;
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
					else
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
						b.SetDescription(Main.IsekaiContext, "Your critical hits automatically dispel the highest-level buff active on the target and inflict Stun for 1 round (no save).");
						((BlueprintUnitFact)b).m_Icon = Icon_Sword;
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.AdditionalAttackBonus;
							c.Value = 4;
						});
					});
					list.Add(item2);
				}
				return list.ToArray();
			});
			CreateEpicPrestige("WarmasterPrestigeClass", "WarmasterProgression", "The Warmaster", "The ultimate tactical sovereign. Mastering the vanguard of planar armies and front-line annihilation, the Warmaster doubles critical threat ranges, commands unshakeable morale, and crushes enemy formations.", DiceType.D12, BABFull, SavesHigh, SavesLow, SavesHigh, Icon_Shield, delegate(int level)
			{
				List<BlueprintFeature> list = new List<BlueprintFeature>();
				switch (level)
				{
				case 1:
				{
					BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "WarmasterSupremacyFeature", delegate(BlueprintFeature b)
					{
						b.SetName(Main.IsekaiContext, "Tactical Supremacy");
						b.SetDescription(Main.IsekaiContext, "You and all allies within 30 feet gain a +4 competence bonus to attack rolls, weapon damage, and AC.");
						((BlueprintUnitFact)b).m_Icon = Icon_Shield;
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.AdditionalAttackBonus;
							c.Value = 4;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.AdditionalDamage;
							c.Value = 4;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.AC;
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
					else
					{
						list.Add(specialPower);
					}
					break;
				}
				if (level == 10)
				{
					BlueprintFeature item2 = Helpers.CreateBlueprint(Main.IsekaiContext, "WarmasterApexConquerorFeature", delegate(BlueprintFeature b)
					{
						b.SetName(Main.IsekaiContext, "Apex Conqueror");
						b.SetDescription(Main.IsekaiContext, "You gain 2 additional attacks during a full attack, your attacks cannot miss on a roll of 1, and your critical strikes deal double damage.");
						((BlueprintUnitFact)b).m_Icon = Icon_Shield;
						b.AddComponent(delegate(BuffExtraAttack c)
						{
							c.Number = 2;
							c.Haste = true;
						});
					});
					list.Add(item2);
				}
				return list.ToArray();
			});
			CreateEpicPrestige("EternalSeekerPrestigeClass", "EternalSeekerProgression", "The Eternal Seeker", "A wandering scholar of the infinite multiverse who has glimpsed countless timelines and disciplines. Capable of borrowing destinies and feats from any path, they possess omniscient perspective.", DiceType.D8, BABMedium, SavesHigh, SavesHigh, SavesHigh, Icon_Spell, delegate(int level)
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
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.SkillKnowledgeArcana;
							c.Value = 10;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.SkillKnowledgeWorld;
							c.Value = 10;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.SkillLoreReligion;
							c.Value = 10;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.SkillLoreNature;
							c.Value = 10;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Competence;
							c.Stat = StatType.SkillPerception;
							c.Value = 10;
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
					else
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
						b.SetDescription(Main.IsekaiContext, "You perceive all timelines simultaneously: you cannot be surprised or flat-footed, roll twice on all d20 rolls taking the better result, and gain +4 to all spell save DCs.");
						((BlueprintUnitFact)b).m_Icon = Icon_Spell;
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
			CreateEpicPrestige("ArchmagePrestigeClass", "ArchmageProgression", "The Archmage", "The absolute sovereign of the arcane arts. Pushing the boundaries of Ninth-Circle magic, the Archmage casts metamagic spells with effortless grace, tearing planar rifts with pure arcane will.", DiceType.D6, BABLow, SavesLow, SavesLow, SavesHigh, Icon_Spell, delegate(int level)
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
						b.AddComponent(delegate(IncreaseAllSpellsDC c)
						{
							c.Value = 2;
							c.Descriptor = ModifierDescriptor.UntypedStackable;
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
					else
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
						b.SetDescription(Main.IsekaiContext, "All spells you cast are automatically Empowered and Maximize Metamagic is applied without increasing their casting time or spell slot level.");
						((BlueprintUnitFact)b).m_Icon = Icon_Spell;
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
			CreateEpicPrestige("ShadowSovereignPrestigeClass", "ShadowSovereignProgression", "The Shadow Sovereign", "The ultimate evolution of the Monarch. Elevates the shadow army beyond mortal limits, expanding the summon pool to 5 elite commanders, infusing minions with profane stat scaling, and commanding the void.", DiceType.D10, BABFull, SavesLow, SavesHigh, SavesHigh, Icon_Dark, delegate(int level)
			{
				List<BlueprintFeature> list = new List<BlueprintFeature>();
				switch (level)
				{
				case 1:
				{
					BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowSovereignDomainFeature", delegate(BlueprintFeature b)
					{
						b.SetName(Main.IsekaiContext, "Monarch's Expanded Territory");
						b.SetDescription(Main.IsekaiContext, "Your shadow domain expands, raising your active shadow soldier limit to 5, and granting all summoned shadows a +4 profane bonus to all physical ability scores.");
						((BlueprintUnitFact)b).m_Icon = Icon_Dark;
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Profane;
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
					else
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
						b.SetDescription(Main.IsekaiContext, "You become the living personification of the Shadow Realm: immunity to all physical status effects, critical hits, sneak attacks, and death magic, dealing +4d6 shadow damage on all attacks.");
						((BlueprintUnitFact)b).m_Icon = Icon_Dark;
						b.AddComponent<AddImmunityToCriticalHits>();
						b.AddComponent(delegate(BuffDescriptorImmunity c)
						{
							c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
						});
						b.AddComponent(delegate(AddStatBonus c)
						{
							c.Descriptor = ModifierDescriptor.Profane;
							c.Stat = StatType.AdditionalDamage;
							c.Value = 8;
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

		private static void CreateEpicPrestige(string className, string progressionName, string displayName, string description, DiceType hitDie, BlueprintStatProgression bab, BlueprintStatProgression fort, BlueprintStatProgression @ref, BlueprintStatProgression will, Sprite icon, Func<int, BlueprintFeature[]> getLevelFeatures, Action<BlueprintCharacterClass> extraConfig = null)
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
			BlueprintCharacterClass cls = Helpers.CreateBlueprint(Main.IsekaiContext, className, delegate(BlueprintCharacterClass bp)
			{
				bp.LocalizedName = Helpers.CreateString(Main.IsekaiContext, className + ".Name", displayName);
				bp.LocalizedDescription = Helpers.CreateString(Main.IsekaiContext, className + ".Description", description);
				bp.LocalizedDescriptionShort = bp.LocalizedDescription;
				bp.HitDie = hitDie;
				bp.PrestigeClass = true;
				bp.m_BaseAttackBonus = bab.ToReference<BlueprintStatProgressionReference>();
				bp.m_FortitudeSave = fort.ToReference<BlueprintStatProgressionReference>();
				bp.m_ReflexSave = @ref.ToReference<BlueprintStatProgressionReference>();
				bp.m_WillSave = will.ToReference<BlueprintStatProgressionReference>();
				bp.m_Difficulty = 1;
				bp.m_Spellbook = IsekaiProtagonistSpellbook.GetReference();
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
				bp.IsDivineCaster = true;
				bp.IsArcaneCaster = true;
				bp.PrimaryColor = 9;
				bp.SecondaryColor = 9;
				if (clothesClass != null)
				{
					bp.MaleEquipmentEntities = clothesClass.MaleEquipmentEntities;
					bp.FemaleEquipmentEntities = clothesClass.FemaleEquipmentEntities;
				}
				bp.m_Archetypes = new BlueprintArchetypeReference[0];
				bp.AddComponent(delegate(PrerequisiteStatValue c)
				{
					c.Stat = StatType.BaseAttackBonus;
					c.Value = 7;
				});
				bp.AddComponent(delegate(PrerequisiteClassLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.Level = 10;
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
			LevelEntry[] array = new LevelEntry[10];
			for (int num3 = 1; num3 <= 10; num3++)
			{
				BlueprintFeature[] array2 = getLevelFeatures(num3);
				int num4 = num3 - 1;
				int level = num3;
				BlueprintFeatureBase[] features = array2;
				array[num4] = Helpers.CreateLevelEntry(level, features);
			}
			blueprintProgression.LevelEntries = array;
			cls.m_Progression = blueprintProgression.ToReference<BlueprintProgressionReference>();
			TTCoreExtensions.RegisterClass(cls);
		}
	}
}
