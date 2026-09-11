using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch
{
	internal static class ShadowMonarchArts
	{
		private static readonly Sprite Icon_Authority = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("52b5df2a97df18242aec67610616ded0"))?.m_Icon;

		private static readonly Sprite Icon_Absorb = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon ?? Icon_Authority;

		private static readonly Sprite Icon_Dagger = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("779179912e6c6fe458fa4cfb90d96e10"))?.m_Icon ?? Icon_Authority;

		private static readonly Sprite Icon_Cloak = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("525f980cb29bc2240b93e953974cb325"))?.m_Icon ?? Icon_Authority;

		private static readonly Sprite Icon_Fear = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0"))?.m_Icon ?? Icon_Authority;

		public static BlueprintAbility RulersAuthorityAbility;

		public static BlueprintFeature RulersAuthorityFeature;

		public static BlueprintAbility ShadowEssenceAbsorptionAbility;

		public static BlueprintFeature ShadowEssenceAbsorptionFeature;

		public static BlueprintBuff ShadowReaveEssenceBuff;

		public static BlueprintAbility ShadowDaggerRushAbility;

		public static BlueprintFeature ShadowDaggerRushFeature;

		public static BlueprintAbility ShadowCloakAbility;

		public static BlueprintFeature ShadowCloakFeature;

		public static BlueprintBuff ShadowSurgeBuff;

		public static BlueprintAbility MonarchDragonsFearAbility;

		public static BlueprintFeature MonarchDragonsFearFeature;

		public static BlueprintBuff MonarchDragonsFearBuff;

		public static void Add()
		{
			RulersAuthorityAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "RulersAuthorityAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Ruler's Authority");
				bp.SetDescription(Main.IsekaiContext, "As a standard action, exert the absolute invisible telekinetic grip of the Monarch upon an enemy within 40 feet. Deals 1d6 force damage per 2 protagonist levels and knocks the target Prone unless they succeed at a Fortitude save (DC = 10 + 1/2 protagonist level + Charisma modifier).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Authority;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = false;
				bp.CanTargetSelf = false;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				ContextActionDealDamage damage = new ContextActionDealDamage
				{
					DamageType = new DamageTypeDescription
					{
						Type = DamageType.Force,
						Common = new DamageTypeDescription.CommomData(),
						Physical = new DamageTypeDescription.PhysicalData()
					},
					Value = new ContextDiceValue
					{
						DiceType = DiceType.D6,
						DiceCountValue = new ContextValue
						{
							ValueType = ContextValueType.Rank
						},
						BonusValue = 0
					}
				};
				ContextActionKnockdownTarget prone = new ContextActionKnockdownTarget();
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(damage, new ContextActionSavingThrow
					{
						Type = SavingThrowType.Fortitude,
						Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
						{
							Failed = Helpers.CreateActionList(prone)
						})
					});
				});
				bp.AddContextRankConfig(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Progression = ContextRankProgression.Div2;
					c.m_Class = new BlueprintCharacterClassReference[1] { BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "IsekaiProtagonistClass").ToReference<BlueprintCharacterClassReference>() };
				});
			});
			RulersAuthorityFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "RulersAuthorityFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Ruler's Authority");
				bp.SetDescription(Main.IsekaiContext, "You wield the supreme telekinetic dominion of the Monarch, seizing enemies with invisible force to crush and pin them to the ground.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Authority;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { RulersAuthorityAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			ShadowReaveEssenceBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowReaveEssenceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Devoured Shadow Core");
				bp.SetDescription(Main.IsekaiContext, "Devouring a fallen soul's core grants a +2 profane bonus to attack rolls, spell DC, and weapon damage for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Absorb;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 1;
					c.Descriptor = ModifierDescriptor.Profane;
				});
			});
			ShadowEssenceAbsorptionAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowEssenceAbsorptionAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Reaping: Devour Core");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, crush and absorb the dark core of a fallen enemy within 30 feet instead of raising them. Restores 1 expended spell slot of your highest available tier (up to 5th) and grants a +2 profane bonus to attacks, damage, and spell DC for 1 minute. Spawns 0 minions, preventing tactical clutter and performance impact.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Absorb;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetEnemies = true;
				bp.CanTargetPoint = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				ContextActionApplyBuff applySelfBuff = new ContextActionApplyBuff
				{
					m_Buff = ShadowReaveEssenceBuff.ToReference<BlueprintBuffReference>(),
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
					c.Actions = Helpers.CreateActionList(applySelfBuff);
				});
			});
			ShadowEssenceAbsorptionFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowEssenceAbsorptionFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Reaping: Devour Core");
				bp.SetDescription(Main.IsekaiContext, "Rather than maintaining an endless legion of shadows, you can choose to directly absorb defeated cores to recover magic and empower your personal strikes.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Absorb;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ShadowEssenceAbsorptionAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			ShadowDaggerRushAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowDaggerRushAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Step: Dagger Rush");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, step instantaneously through the shadow realm to appear directly behind an enemy within 40 feet. Your next attack this turn deals an additional +3d6 sneak attack damage and automatically catches the target flat-footed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dagger;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetEnemies = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2")?.ToReference<BlueprintBuffReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							BonusValue = 1
						},
						ToCaster = true
					});
				});
			});
			ShadowDaggerRushFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowDaggerRushFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Step: Dagger Rush");
				bp.SetDescription(Main.IsekaiContext, "You traverse intermediate shadows effortlessly, stepping into the blind spot of enemies as a swift action.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dagger;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ShadowDaggerRushAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			ShadowSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Cloak: Shrouded Monarch");
				bp.SetDescription(Main.IsekaiContext, "Completely concealed by liquid shadows: gains Greater Invisibility, 50% displacement miss chance, and a +10 competence bonus to Stealth.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Cloak;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Total;
					c.Descriptor = ConcealmentDescriptor.Displacement;
				});
			});
			ShadowCloakAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowCloakAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch's Shadow Cloak");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, cloak yourself in primordial shadow for 1 minute: gains Greater Invisibility, 50% displacement miss chance, and +10 to Stealth.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Cloak;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				ContextActionApplyBuff applySelf = new ContextActionApplyBuff
				{
					m_Buff = ShadowSurgeBuff.ToReference<BlueprintBuffReference>(),
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
					c.Actions = Helpers.CreateActionList(applySelf);
				});
			});
			ShadowCloakFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowCloakFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch's Shadow Cloak");
				bp.SetDescription(Main.IsekaiContext, "You can drape yourself in the living veil of the shadow domain, dissolving your form into darkness.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Cloak;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ShadowCloakAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			MonarchDragonsFearBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "MonarchDragonsFearBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Dragon's Fear: Terrorized");
				bp.SetDescription(Main.IsekaiContext, "Overwhelmed by the roar of the ancient shadow dragon Kamish: Paralyzed for 1 round and shaken for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Fear;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Paralyzed;
				});
			});
			MonarchDragonsFearAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MonarchDragonsFearAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Dragon's Fear (Kamish's Roar)");
				bp.SetDescription(Main.IsekaiContext, "As a standard action, release the soul-shattering roar of the shadow dragon Kamish. Enemies within a 30-foot radius must succeed at a Will save (DC = 10 + 1/2 protagonist level + Charisma modifier) or be Paralyzed for 1 round, and Shaken for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Fear;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				ContextActionApplyBuff applyFear = new ContextActionApplyBuff
				{
					m_Buff = MonarchDragonsFearBuff.ToReference<BlueprintBuffReference>(),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 1
					}
				};
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionSavingThrow
					{
						Type = SavingThrowType.Will,
						Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
						{
							Failed = Helpers.CreateActionList(applyFear)
						})
					});
				});
			});
			MonarchDragonsFearFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "MonarchDragonsFearFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dragon's Fear (Kamish's Roar)");
				bp.SetDescription(Main.IsekaiContext, "You channel the terrifying presence of the ancient shadow dragon Kamish, overwhelming enemies with paralyzing dread.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Fear;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { MonarchDragonsFearAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
