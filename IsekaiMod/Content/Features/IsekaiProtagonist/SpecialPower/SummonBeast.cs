using System;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class SummonBeast
	{
		private const string StandardSummonDescription = "Summoned monsters appear where you designate and act according to their {g|Encyclopedia:Initiative}initiative{/g} {g|Encyclopedia:Check}check{/g} results. They {g|Encyclopedia:Attack}attack{/g} your opponents to the best of their ability.";

		private static readonly BlueprintUnit CR7_HydraAdvanced = BlueprintTools.GetBlueprint<BlueprintUnit>("e135463f804adb541b402bae3f657af4");

		private static readonly BlueprintUnit CR9_OwlbearAdvanced = BlueprintTools.GetBlueprint<BlueprintUnit>("7d3bd11169778c845b2631d22d27d465");

		private static readonly BlueprintUnit CR9_RocStandard = BlueprintTools.GetBlueprint<BlueprintUnit>("12d33b211fa5b394fb214e202a2db300");

		private static readonly BlueprintUnit CR10_FiendishMinotaur_Guard = BlueprintTools.GetBlueprint<BlueprintUnit>("962427b9ddb354947ac92655dc637e0c");

		private static readonly BlueprintSummonPool SummonMonsterPool = BlueprintTools.GetBlueprint<BlueprintSummonPool>("d94c93e7240f10e41ae41db4c83d1cbe");

		private static readonly BlueprintBuff SummonedCreatureSpawnMonsterVI_IX = BlueprintTools.GetBlueprint<BlueprintBuff>("0dff842f06edace43baf8a2f44207045");

		private static readonly Sprite Icon_SummonMonsterVII = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ab167fd8203c1314bac6568932f1752f")).m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource SummonBeastResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonBeastResource", delegate(BlueprintAbilityResource bp)
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
			BlueprintAbility SummonBeastAbility = CreateSummonAbility("SummonBeastAbility", SummonBeastResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Summon Beast");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons a Hydra, an Owlbear, a Roc, or a Minotaur.");
			});
			BlueprintAbility SummonHydra = CreateSummonAbility("SummonHydra", SummonBeastResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Summon Beast (Hydra)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons a Hydra.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnBeast(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = CR7_HydraAdvanced?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintAbility SummonOwlbear = CreateSummonAbility("SummonOwlbear", SummonBeastResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Summon Beast (Owlbear)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons an Owlbear.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnBeast(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = CR9_OwlbearAdvanced?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintAbility SummonRoc = CreateSummonAbility("SummonRoc", SummonBeastResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Summon Beast (Roc)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons a Roc (requires level 12+, summons Owlbear otherwise).");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new Conditional
					{
						ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterLevel cond)
						{
							cond.MinLevel = 12;
						}),
						IfTrue = SpawnBeast(delegate(ContextActionSpawnMonster s)
						{
							s.m_Blueprint = CR9_RocStandard?.ToReference<BlueprintUnitReference>();
						}),
						IfFalse = SpawnBeast(delegate(ContextActionSpawnMonster s)
						{
							s.m_Blueprint = CR9_OwlbearAdvanced?.ToReference<BlueprintUnitReference>();
						})
					});
				});
			});
			BlueprintAbility SummonMinotaur = CreateSummonAbility("SummonMinotaur", SummonBeastResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Summon Beast (Minotaur)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons a Minotaur (requires level 12+, summons Hydra otherwise).");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new Conditional
					{
						ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterLevel cond)
						{
							cond.MinLevel = 12;
						}),
						IfTrue = SpawnBeast(delegate(ContextActionSpawnMonster s)
						{
							s.m_Blueprint = CR10_FiendishMinotaur_Guard?.ToReference<BlueprintUnitReference>();
						}),
						IfFalse = SpawnBeast(delegate(ContextActionSpawnMonster s)
						{
							s.m_Blueprint = CR7_HydraAdvanced?.ToReference<BlueprintUnitReference>();
						})
					});
				});
			});
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonBeastFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Summon Beast");
				bp.SetDescription(Main.IsekaiContext, "As a full-round action, you summon a powerful beast to fight by your side. At level 7, you can summon an Advanced Hydra or Advanced Owlbear. At level 12, you unlock the Roc and Fiendish Minotaur Guard. Usable 3 times per day (scaling up to 6 times per day by level 20). Duration: 1 round per level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_SummonMonsterVII;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SummonBeastAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SummonBeastResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			});
			SummonBeastAbility.AddComponent(delegate(AbilityVariants c)
			{
				c.m_Variants = new BlueprintAbilityReference[4]
				{
					SummonHydra.ToReference<BlueprintAbilityReference>(),
					SummonOwlbear.ToReference<BlueprintAbilityReference>(),
					SummonRoc.ToReference<BlueprintAbilityReference>(),
					SummonMinotaur.ToReference<BlueprintAbilityReference>()
				};
			});
			SpecialPowerSelection.AddToAuthoritySelection(feature);
		}

		private static BlueprintAbility CreateSummonAbility(string name, BlueprintAbilityResource resource, Action<BlueprintAbility> init = null)
		{
			BlueprintAbility blueprintAbility = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility bp)
			{
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
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
				((BlueprintUnitFact)bp).m_Icon = Icon_SummonMonsterVII;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.m_IsFullRoundAction = true;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			init?.Invoke(blueprintAbility);
			return blueprintAbility;
		}

		private static ActionList SpawnBeast(Action<ContextActionSpawnMonster> init = null)
		{
			ContextActionSpawnMonster contextActionSpawnMonster = new ContextActionSpawnMonster
			{
				m_SummonPool = SummonMonsterPool?.ToReference<BlueprintSummonPoolReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Rounds,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = Values.CreateContextRankValue(AbilityRankType.Default),
					m_IsExtendable = true
				},
				CountValue = new ContextDiceValue
				{
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = 1
				},
				LevelValue = 0,
				AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff c)
				{
					c.Permanent = true;
					c.m_Buff = SummonedCreatureSpawnMonsterVI_IX?.ToReference<BlueprintBuffReference>();
					c.DurationValue = Values.Duration.Zero;
					c.IsNotDispelable = true;
				})
			};
			init?.Invoke(contextActionSpawnMonster);
			return Helpers.CreateActionList(contextActionSpawnMonster);
		}
	}
}
