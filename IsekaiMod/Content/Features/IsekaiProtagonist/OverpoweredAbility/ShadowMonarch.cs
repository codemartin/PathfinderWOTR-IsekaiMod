using System;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class ShadowMonarch
	{
		private static readonly BlueprintUnit ShadowDemonTemplate = BlueprintTools.GetBlueprint<BlueprintUnit>("791269525695a3b428ec2822427766e5");

		private static readonly BlueprintUnit ShadowKnightTemplate = BlueprintTools.GetBlueprint<BlueprintUnit>("46acfb6a54f3f2a44bd3fd9b9403cd84");

		private static readonly BlueprintUnit ShadowMythicTemplate = BlueprintTools.GetBlueprint<BlueprintUnit>("e31ec23f6a6e4c028e01fb2f7bd0d523");

		private static readonly BlueprintSummonPool ShadowMonarchPool = BlueprintTools.GetBlueprint<BlueprintSummonPool>("d94c93e7240f10e41ae41db4c83d1cbe");

		private static readonly BlueprintBuff SummonedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0dff842f06edace43baf8a2f44207045");

		private static readonly Sprite Icon_ShadowMonarch = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("52b5df2a97df18242aec67610616ded0")).m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource ShadowMonarchResource = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 5,
					StartingIncrease = 1,
					LevelStep = 5,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 6;
			});
			BlueprintAbility ShadowMonarchAbility = CreateSummonAbility("ShadowMonarchAbility", ShadowMonarchResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Shadow Monarch");
				bp.SetSummonDescription(Main.IsekaiContext, "Arise! Command the shadows of the fallen. Summons an elite Shadow Soldier to fight by your side. You can have at most 1 active Shadow Soldier at a time.");
			});
			BlueprintAbility SummonShadowKnight = CreateSummonAbility("SummonShadowKnight", ShadowMonarchResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Monarch - Extract Shadow Knight");
				bp.SetSummonDescription(Main.IsekaiContext, "Summons an armored Shadow Knight imbued with dark resilience and heavy melee strikes.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnShadow(ShadowKnightTemplate);
				});
			});
			BlueprintAbility SummonShadowDemon = CreateSummonAbility("SummonShadowDemon", ShadowMonarchResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Monarch - Extract Shadow Infiltrator");
				bp.SetSummonDescription(Main.IsekaiContext, "Summons an incorporeal Shadow Infiltrator capable of bypassing physical barriers.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnShadow(ShadowDemonTemplate);
				});
			});
			BlueprintAbility SummonShadowSovereign = CreateSummonAbility("SummonShadowSovereign", ShadowMonarchResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Monarch - Extract Shadow Sovereign");
				bp.SetSummonDescription(Main.IsekaiContext, "Summons a devastating Mythic Shadow Sovereign wielding apocalyptic shadowy magic.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnShadow(ShadowMythicTemplate);
				});
			});
			ShadowMonarchAbility.AddComponent(delegate(AbilityVariants c)
			{
				c.m_Variants = new BlueprintAbilityReference[3]
				{
					SummonShadowKnight.ToReference<BlueprintAbilityReference>(),
					SummonShadowDemon.ToReference<BlueprintAbilityReference>(),
					SummonShadowSovereign.ToReference<BlueprintAbilityReference>()
				};
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Shadow Monarch");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to the Shadow Monarch archetype. Death is not the end of service for your foes. As the Shadow Monarch, you can extract the shadows of slain elites to serve as eternal guardians. \nBenefit: As a standard action, summon an elite Shadow Soldier. You maintain a hard limit of 1 active shadow to prevent planar disruption. Extracting a new shadow replaces the previous one.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ShadowMonarch;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ShadowMonarchAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = ShadowMonarchResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
				bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.m_Archetype = ShadowMonarchArchetype.GetReference();
					c.Level = 1;
				});
			}));
		}

		private static BlueprintAbility CreateSummonAbility(string name, BlueprintAbilityResource resource, Action<BlueprintAbility> init = null)
		{
			BlueprintAbility blueprintAbility = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility bp)
			{
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Conjuration;
				});
				bp.AddComponent(delegate(SpellDescriptorComponent c)
				{
					c.Descriptor = SpellDescriptor.Summoning;
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_ShadowMonarch;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			init?.Invoke(blueprintAbility);
			return blueprintAbility;
		}

		private static ActionList SpawnShadow(BlueprintUnit template)
		{
			ContextActionSpawnMonster contextActionSpawnMonster = new ContextActionSpawnMonster
			{
				m_Blueprint = template.ToReference<BlueprintUnitReference>(),
				m_SummonPool = ShadowMonarchPool.ToReference<BlueprintSummonPoolReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Hours,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 1
					},
					m_IsExtendable = true
				},
				CountValue = Values.Dice.One,
				LevelValue = 0,
				AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff c)
				{
					c.Permanent = true;
					c.m_Buff = SummonedBuff.ToReference<BlueprintBuffReference>();
					c.DurationValue = Values.Duration.Zero;
					c.IsNotDispelable = true;
				})
			};
			return Helpers.CreateActionList(new ContextActionClearSummonPool
			{
				m_SummonPool = ShadowMonarchPool.ToReference<BlueprintSummonPoolReference>()
			}, contextActionSpawnMonster);
		}
	}
}
