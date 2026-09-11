using IsekaiMod.Content.Classes.IsekaiProtagonist;
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
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class Instakill
	{
		private static readonly BlueprintBuff Stunned = BlueprintTools.GetBlueprint<BlueprintBuff>("09d39b38bb7c6014394b6daced9bacd3");

		private static readonly Sprite Icon_TwoHandedFighterDevastatingBlow = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("687aa977ef0d3f849af8bee2f40930df")).m_Icon;

		public static void Add()
		{
			LocalizedString InstaKillDesc = Helpers.CreateString(Main.IsekaiContext, "Instakill.Description", "Who will live and who will die? This ultimate power to alter fate lies within your grasp. Will you wield it wisely, or will you assert your dominion as a god of the new world?\nBenefit: Instantly kills the targeted creature if they fail a Fortitude saving throw. The DC starts at 15 and scales with your level and Charisma modifier, making it increasingly difficult to resist. Creatures immune to death effects are not spared, but instead suffer significant unholy damage.\nCreatures that succeed on the saving throw are instead stunned for 1 round, giving you the opportunity to turn the tide in your favor.");
			BlueprintUnitProperty InstakillDCProperty = Helpers.CreateBlueprint(Main.IsekaiContext, "InstakillDCProperty", delegate(BlueprintUnitProperty bp)
			{
				bp.name = "InstakillDCProperty";
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.Level;
				});
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.StatBonusCharisma;
				});
				bp.BaseValue = 15;
				bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
			});
			BlueprintAbilityResource InstakillResource = Helpers.CreateBlueprint(Main.IsekaiContext, "InstakillResource", delegate(BlueprintAbilityResource bp)
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
			BlueprintAbility InstakillAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "InstakillAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Instakill");
				bp.SetDescription(InstaKillDesc);
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSavingThrow contextActionSavingThrow)
					{
						contextActionSavingThrow.Type = SavingThrowType.Fortitude;
						contextActionSavingThrow.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0];
						contextActionSavingThrow.HasCustomDC = false;
						contextActionSavingThrow.CustomDC = 0;
						contextActionSavingThrow.Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved contextActionConditionalSaved)
						{
							contextActionConditionalSaved.Succeed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
							{
								contextActionApplyBuff.m_Buff = Stunned.ToReference<BlueprintBuffReference>();
								contextActionApplyBuff.DurationValue = Values.Duration.OneRound;
							});
							contextActionConditionalSaved.Failed = Helpers.CreateActionList(new ContextActionKill
							{
								Dismember = UnitState.DismemberType.Normal
							}, new ContextActionDealDamage
							{
								DamageType = new DamageTypeDescription
								{
									Type = DamageType.Energy,
									Energy = DamageEnergyType.Unholy
								},
								Value = new ContextDiceValue
								{
									DiceType = DiceType.D6,
									DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
									BonusValue = 100
								},
								HalfIfSaved = false,
								IsAoE = false,
								IgnoreCritical = true
							});
						});
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.MultiplyByModifier;
					c.m_StepLevel = 2;
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Necromancy;
				});
				bp.AddComponent(delegate(AbilitySpawnFx c)
				{
					c.PrefabLink = new PrefabLink
					{
						AssetId = "cc9120c1d1e64e1478b859f14a200404"
					};
					c.Time = AbilitySpawnFxTime.OnApplyEffect;
					c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.Add10ToDC = false;
					c.DC = Values.CreateContextCasterCustomPropertyValue(InstakillDCProperty);
					c.CasterLevel = -1;
					c.Concentration = -1;
					c.SpellLevel = 10;
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = InstakillResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_TwoHandedFighterDevastatingBlow;
				bp.Type = AbilityType.SpellLike;
				bp.Range = AbilityRange.Medium;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = true;
				bp.CanTargetSelf = true;
				bp.SpellResistance = true;
				bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
				bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Reach;
				bp.LocalizedDuration = StaticReferences.Strings.Null;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "InstakillFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Instakill");
				bp.SetDescription(InstaKillDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_TwoHandedFighterDevastatingBlow;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { InstakillAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = InstakillResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
