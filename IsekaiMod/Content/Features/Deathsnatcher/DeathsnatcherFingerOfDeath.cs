using IsekaiMod.Content.Classes.Deathsnatcher;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.Deathsnatcher
{
	internal class DeathsnatcherFingerOfDeath
	{
		private static readonly BlueprintAbility FingerOfDeathAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("6f1dcf6cfa92d1948a740195707c0dbe");

		public static void Add()
		{
			BlueprintAbilityResource DeathsnatcherFingerOfDeathResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherFingerOfDeathResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = false,
					LevelIncrease = 1,
					IncreasedByLevelStartPlusDivStep = false,
					StartingLevel = 0,
					StartingIncrease = 0,
					LevelStep = 0,
					PerStepIncrease = 0,
					MinClassLevelIncrease = 0,
					OtherClassesModifier = 0f,
					IncreasedByStat = false,
					ResourceBonusStat = StatType.Unknown
				};
			});
			BlueprintAbility DeathsnatcherFingerOfDeathAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherFingerOfDeathAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(((BlueprintUnitFact)FingerOfDeathAbility).m_DisplayName);
				bp.SetDescription(((BlueprintUnitFact)FingerOfDeathAbility).m_Description);
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)FingerOfDeathAbility).m_Icon;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.SavingThrowType = SavingThrowType.Fortitude;
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved contextActionConditionalSaved)
					{
						contextActionConditionalSaved.Succeed = ActionFlow.DoSingle(delegate(ContextActionDealDamage contextActionDealDamage)
						{
							contextActionDealDamage.m_Type = ContextActionDealDamage.Type.Damage;
							contextActionDealDamage.DamageType = new DamageTypeDescription
							{
								Type = DamageType.Energy,
								Energy = DamageEnergyType.Unholy
							};
							contextActionDealDamage.AbilityType = StatType.Unknown;
							contextActionDealDamage.Duration = Values.Duration.Zero;
							contextActionDealDamage.Value = new ContextDiceValue
							{
								DiceType = DiceType.D6,
								DiceCountValue = 3,
								BonusValue = Values.CreateContextRankValue(AbilityRankType.DamageBonus)
							};
						});
						contextActionConditionalSaved.Failed = ActionFlow.DoSingle(delegate(ContextActionDealDamage contextActionDealDamage)
						{
							contextActionDealDamage.m_Type = ContextActionDealDamage.Type.Damage;
							contextActionDealDamage.DamageType = new DamageTypeDescription
							{
								Type = DamageType.Energy,
								Energy = DamageEnergyType.Unholy
							};
							contextActionDealDamage.AbilityType = StatType.Unknown;
							contextActionDealDamage.Duration = Values.Duration.Zero;
							contextActionDealDamage.Value = new ContextDiceValue
							{
								DiceType = DiceType.Zero,
								DiceCountValue = 0,
								BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
							};
						});
					});
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Necromancy;
				});
				bp.AddComponent(delegate(SpellDescriptorComponent c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(AbilitySpawnFx c)
				{
					c.PrefabLink = new PrefabLink
					{
						AssetId = "e8569f4442d66ee42ad10502a100c1df"
					};
					c.Time = AbilitySpawnFxTime.OnStart;
					c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
				});
				bp.AddComponent(delegate(AbilityDeliverDelay c)
				{
					c.DelaySeconds = 0.65f;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Progression = ContextRankProgression.MultiplyByModifier;
					c.m_StepLevel = 10;
					c.m_Class = new BlueprintCharacterClassReference[1] { DeathsnatcherClass.GetReference() };
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.DamageBonus;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Progression = ContextRankProgression.AsIs;
					c.m_StepLevel = 1;
					c.m_Class = new BlueprintCharacterClassReference[1] { DeathsnatcherClass.GetReference() };
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.DC = Values.CreateContextCasterCustomPropertyValue(DeathsnatcherSpellLikeDC.Get());
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = DeathsnatcherFingerOfDeathResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
				});
				bp.Type = AbilityType.SpellLike;
				bp.Range = AbilityRange.Close;
				bp.CanTargetEnemies = true;
				bp.CanTargetSelf = true;
				bp.SpellResistance = true;
				bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
				bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = FingerOfDeathAbility.AvailableMetamagic;
				bp.LocalizedDuration = StaticReferences.Strings.Null;
				bp.LocalizedSavingThrow = StaticReferences.Strings.SavingThrow.FortitudePartial;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherFingerOfDeathFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(((BlueprintUnitFact)FingerOfDeathAbility).m_DisplayName);
				bp.SetDescription(Main.IsekaiContext, "At 16th level, the Deathsnatcher gains Finger of Death as a spell-like ability once per day.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)FingerOfDeathAbility).m_Icon;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = DeathsnatcherFingerOfDeathResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { DeathsnatcherFingerOfDeathAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
