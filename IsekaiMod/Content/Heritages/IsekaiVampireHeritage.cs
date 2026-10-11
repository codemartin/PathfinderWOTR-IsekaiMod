using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiVampireHeritage
{
	public static void Add()
	{
		Sprite Icon_Vampire = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_VAMPIRE.png");
		Sprite Icon_Mist = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bed2f82519a10214ca023c5961e0c436"))?.m_Icon;
		BlueprintAbilityResource VampiricMistResource = Helpers.CreateBlueprint(Main.IsekaiContext, "VampiricMistResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = HeritageResources.ByCharacterLevel(1, 5, 1, 5, 1);
		});
		BlueprintBuff VampiricMistBuff = TTCoreExtensions.CreateBuff("VampiricMistBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Vampiric Mist Form");
			bp.SetDescription(Main.IsekaiContext, "You disperse your physical form into a swirling cloud of mist. You gain DR 10/-, a +30 foot bonus to speed and immunity to difficult terrain, and cannot cast spells.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Mist;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Speed;
				c.Value = 30;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.DifficultTerrain;
			});
			bp.AddComponent(delegate(ForbidSpellCasting c)
			{
				c.m_IgnoreFeature = new BlueprintFeatureReference();
			});
		});
		BlueprintAbility VampiricMistAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "VampiricMistAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Vampiric Mist Form");
			bp.SetDescription(Main.IsekaiContext, "As a swift action, dissolve into mist for 2 rounds: DR 10/-, a +30 foot bonus to speed and immunity to difficult terrain, but you cannot cast spells. Usable once per day, plus once more per day for every 5 character levels.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Mist;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			bp.ActionType = UnitCommand.CommandType.Swift;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
				{
					a.m_Buff = VampiricMistBuff.ToReference<BlueprintBuffReference>();
					a.DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = new ContextValue
						{
							ValueType = ContextValueType.Simple,
							Value = 2
						}
					};
				});
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = VampiricMistResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
		});
		BlueprintAbilityResource BloodDrainResource = Helpers.CreateBlueprint(Main.IsekaiContext, "BloodDrainResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = HeritageResources.Fixed(3);
		});
		BlueprintAbility BloodDrainBiteAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "BloodDrainBiteAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Blood Drain");
			bp.SetDescription(Main.IsekaiContext, "Make a melee touch attack against an adjacent living creature. On a hit it takes 1d6 unholy damage plus 1d6 per 2 character levels and 1 point of Constitution damage, and you heal hit points equal to the unholy damage dealt. Usable 3 times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Vampire;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Touch;
			bp.CanTargetEnemies = true;
			bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(AbilityDeliverTouch c)
			{
				c.m_TouchWeapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("bb337517547de1a4189518d404ec49d4");
			});
			bp.AddComponent(delegate(AbilityTargetHasFact c)
			{
				c.m_CheckedFacts = new BlueprintUnitFactReference[2]
				{
					BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33"),
					BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f")
				};
				c.Inverted = true;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
				{
					DamageType = new DamageTypeDescription
					{
						Type = DamageType.Energy,
						Energy = DamageEnergyType.Unholy
					},
					Duration = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero
					},
					Value = new ContextDiceValue
					{
						DiceType = DiceType.D6,
						DiceCountValue = Values.CreateContextRankValue(AbilityRankType.DamageDice),
						BonusValue = 0
					},
					WriteResultToSharedValue = true,
					ResultSharedValue = AbilitySharedValue.Damage
				}, new ContextActionDealDamage
				{
					m_Type = ContextActionDealDamage.Type.AbilityDamage,
					AbilityType = StatType.Constitution,
					Duration = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero
					},
					Value = new ContextDiceValue
					{
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 1
					}
				}, new ContextActionOnContextCaster
				{
					Actions = Helpers.CreateActionList(new ContextActionHealTarget
					{
						Value = new ContextDiceValue
						{
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Shared,
								ValueShared = AbilitySharedValue.Damage
							}
						}
					})
				});
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.DamageDice;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.OnePlusDiv2;
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = BloodDrainResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
		});
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiVampireHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Isekai Vampire Lord");
			bp.SetDescription(Main.IsekaiContext, "Reincarnated into Golarion as a true Vampire Lord, you possess otherworldly beauty, undying regeneration, and dominion over blood and mist. Unlike ordinary dhampirs, you suffer no penalty to Constitution.\nThe Isekai Vampire Lord has a +2 racial bonus to Dexterity and Charisma and a +2 racial bonus on Persuasion and Perception checks. They have DR 5/magic and silver, fast healing equal to half their character level (at least 1), and immunity to poison, disease, and harmful mind effects (charm, compulsion, confusion, daze, fear, sleep and despair).\nBlood Drain: 3 times per day, a melee touch attack against a living creature deals 1d6 unholy damage plus 1d6 per 2 character levels and 1 Constitution damage, and heals you by the unholy damage dealt.\nVampiric Mist Form: once per day, plus once more per 5 character levels, as a swift action you gain DR 10/-, +30 feet of speed and immunity to difficult terrain for 2 rounds, but cannot cast spells.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Vampire;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Dexterity;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillPerception;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillPersuasion;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Or = false;
				c.Value = 5;
				c.BypassedByMaterial = true;
				c.Material = PhysicalDamageMaterial.Silver;
				c.BypassedByMagic = true;
				c.MinEnhancementBonus = 1;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 0;
				c.Bonus = Values.CreateContextRankValue(AbilityRankType.StatBonus);
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.StatBonus;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.Div2;
				c.m_UseMin = true;
				c.m_Min = 1;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion;
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = VampiricMistResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = BloodDrainResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					VampiricMistAbility.ToReference<BlueprintUnitFactReference>(),
					BloodDrainBiteAbility.ToReference<BlueprintUnitFactReference>()
				};
			});
			string[] array = new string[2] { "DevourerArchetype", "OverlordArchetype" };
			foreach (string archetypeName in array)
			{
				bp.AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
					c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, archetypeName);
				});
			}
			bp.Groups = new FeatureGroup[2]
			{
				FeatureGroup.Racial,
				FeatureGroup.DhampirHeritage
			};
			bp.ReapplyOnLevelUp = true;
		});
		ContentOffering.OfferInSelection(FeatTools.Selections.DhampirHeritageSelection, feature, OfferedContent.IsekaiHeritages);
	}
}
