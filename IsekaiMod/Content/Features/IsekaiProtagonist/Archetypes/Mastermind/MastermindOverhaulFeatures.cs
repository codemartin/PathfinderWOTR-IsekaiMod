using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind
{
	public static class MastermindOverhaulFeatures
	{
		private static bool Added = false;

		private static readonly Sprite Icon_Foresight = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("1f01a098d737ec6419aedc4e7ad61fdd"))?.m_Icon;

		private static readonly Sprite Icon_Dominate = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d7cbd2004ce66a042aeab2e95a3c5c61"))?.m_Icon;

		private static readonly Sprite Icon_TrueSeeing = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("b3da3fbee6a751d4197e446c7e852bcb"))?.m_Icon;

		public static BlueprintFeature GrandmasterForesight { get; private set; }

		public static BlueprintFeature PersuasionTake20 { get; private set; }

		public static BlueprintFeature CheckmateGambit { get; private set; }

		public static BlueprintFeature Zugzwang { get; private set; }

		public static BlueprintFeature CalculatedSacrifice { get; private set; }

		public static BlueprintFeature GeassOfAbsoluteCommand { get; private set; }

		public static void Add()
		{
			if (Added)
			{
				return;
			}
			Added = true;
			GrandmasterForesight = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindGrandmasterForesight", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grandmaster's Foresight");
				bp.SetDescription(Main.IsekaiContext, "Through absolute strategic calculation, you anticipate incoming attacks and tactical engagements well before they manifest.\n\nYou gain an insight bonus to your {g|Encyclopedia:Armor_Class}Armor Class{/g} and {g|Encyclopedia:Initiative}Initiative{/g} checks equal to your {g|Encyclopedia:Intelligence}Intelligence{/g} modifier.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.BaseStat = StatType.Intelligence;
					c.DerivativeStat = StatType.AC;
				});
				bp.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.BaseStat = StatType.Intelligence;
					c.DerivativeStat = StatType.Initiative;
				});
				bp.AddComponent(delegate(RecalculateOnStatChange c)
				{
					c.Stat = StatType.Intelligence;
				});
			});
			PersuasionTake20 = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindPersuasionTake20", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Psychological Mastery (Take 20 Persuasion)");
				bp.SetDescription(Main.IsekaiContext, "Understanding human cognition, body language, and diplomatic pressure points, you never falter in negotiation.\n\nYou automatically roll a natural 20 on all {g|Encyclopedia:Persuasion}Persuasion{/g} skill checks (Diplomacy, Bluff, and Intimidate).");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrueSeeing;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(ModifyD20 c)
				{
					c.Rule = RuleType.SkillCheck;
					c.SpecificSkill = true;
					c.Skill = new StatType[1] { StatType.SkillPersuasion };
					c.Replace = true;
					c.RollResult = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 20
					};
				});
			});
			BlueprintBuff CheckmateGambitBuff = TTCoreExtensions.CreateBuff("MastermindCheckmateGambitBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Exposed Flaw (Checkmate Gambit)");
				bp.SetDescription(Main.IsekaiContext, "The target's defenses have been completely disassembled by the Mastermind. All attacks against this creature bypass Damage Reduction, and critical threats are automatically confirmed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AC;
					c.Value = -4;
				});
				bp.AddComponent<TargetCritAutoconfirm>();
				bp.AddComponent<IgnoreTargetCritImmunity>();
			});
			BlueprintAbilityResource CheckmateGambitResource = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindCheckmateGambitResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Intelligence
				};
			});
			BlueprintAbility CheckmateGambitAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindCheckmateGambitAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Checkmate Gambit");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, pinpoint a critical flaw in an enemy's tactical formation. For 1 round, the target suffers a -4 penalty to AC, all attacks against it bypass Damage Reduction, and critical threats against it automatically confirm.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = false;
				bp.CanTargetSelf = false;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRound;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = CheckmateGambitResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = CheckmateGambitBuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.DurationValue = Values.Duration.OneRound;
					});
				});
			});
			CheckmateGambit = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindCheckmateGambit", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Checkmate Gambit");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, expose an enemy's defenses. For 1 round, the target suffers a -4 penalty to AC, all attacks against it bypass Damage Reduction, and critical threats against it automatically confirm (uses per day equal to 3 + Intelligence modifier).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = CheckmateGambitResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { CheckmateGambitAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			BlueprintBuff ZugzwangBuff = TTCoreExtensions.CreateBuff("MastermindZugzwangBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Zugzwang Trap");
				bp.SetDescription(Main.IsekaiContext, "Trapped in a zero-sum tactical dilemma, every move the target makes worsens its position. The creature rolls twice and takes the worse result on all saving throws against party spells and abilities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(ModifyD20 c)
				{
					c.Rule = RuleType.SavingThrow;
					c.RollsAmount = 1;
					c.TakeBest = false;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Speed;
					c.Value = -15;
				});
			});
			BlueprintAbility ZugzwangAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindZugzwangAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Zugzwang");
				bp.SetDescription(Main.IsekaiContext, "As a standard action, force an enemy into a tactical trap for 3 rounds. The creature suffers a -15 penalty to movement speed and must roll twice, taking the worse result, on all saving throws against your party's abilities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Medium;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = false;
				bp.CanTargetSelf = false;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.ThreeRounds;
				bp.LocalizedSavingThrow = StaticReferences.Strings.SavingThrow.WillNegates;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = ZugzwangBuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.DurationValue = Values.Duration.ThreeRounds;
					});
				});
			});
			Zugzwang = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindZugzwang", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Zugzwang");
				bp.SetDescription(Main.IsekaiContext, "As a standard action, force an enemy into an inescapable dilemma. For 3 rounds, the target's movement speed is reduced by 15 feet and it must roll twice and take the worse result on all saving throws against your party.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ZugzwangAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			BlueprintBuff CalculatedSacrificeBuff = TTCoreExtensions.CreateBuff("MastermindCalculatedSacrificeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Calculated Sacrifice: Contingency");
				bp.SetDescription(Main.IsekaiContext, "Your analytical foresight activates upon near-fatal damage. You gain Fast Healing 20 and a +6 deflection bonus to AC for 3 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 20;
					c.Bonus = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 0
					};
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
			});
			CalculatedSacrifice = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindCalculatedSacrifice", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Calculated Sacrifice");
				bp.SetDescription(Main.IsekaiContext, "Every pawn has a price, and no true grandmaster perishes before the board is clear. Upon taking damage that would reduce you below 0 hit points, your analytical foresight instantly triggers: you gain Fast Healing 20 for 3 rounds and a +6 deflection bonus to AC. Additionally, you gain a permanent +3 bonus to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Foresight;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddIncomingDamageTrigger c)
				{
					c.ReduceBelowZero = true;
					c.Actions = Helpers.CreateActionList(new Conditional
					{
						ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionHasFact cond)
						{
							cond.m_Fact = CalculatedSacrificeBuff.ToReference<BlueprintUnitFactReference>();
							cond.Not = true;
						}),
						IfTrue = Helpers.CreateActionList(new ContextActionHealTarget
						{
							Value = new ContextDiceValue
							{
								DiceType = DiceType.Zero,
								DiceCountValue = 0,
								BonusValue = new ContextValue
								{
									ValueType = ContextValueType.Simple,
									Value = 50
								}
							}
						}, new ContextActionApplyBuff
						{
							m_Buff = CalculatedSacrificeBuff.ToReference<BlueprintBuffReference>(),
							DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Rounds,
								DiceType = DiceType.Zero,
								BonusValue = 3
							}
						})
					});
				});
			});
			BlueprintBuff GeassBuff = TTCoreExtensions.CreateBuff("MastermindGeassBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Command (Geass)");
				bp.SetDescription(Main.IsekaiContext, "Compelled by the Mastermind's absolute gaze, this creature is paralyzed in helpless submission or commanded to fight for the sovereign.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dominate;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Paralyzed;
				});
			});
			BlueprintAbility GeassAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindGeassAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Geass of Absolute Command");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, unlock the crimson sigil of absolute obedience. Targets an enemy within 50 feet: paralyzes the foe for 1d4 rounds with no saving throw allowed (once per day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dominate;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Medium;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Special;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = false;
				bp.CanTargetSelf = false;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRound;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				BlueprintAbilityResource geassResource = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindGeassResource", delegate(BlueprintAbilityResource blueprintAbilityResource)
				{
					blueprintAbilityResource.m_MaxAmount = new BlueprintAbilityResource.Amount
					{
						BaseValue = 1,
						IncreasedByLevel = false,
						IncreasedByStat = false
					};
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = geassResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = GeassBuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.DurationValue = Values.Duration.OneRound;
					});
				});
			});
			GeassOfAbsoluteCommand = Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindGeassOfAbsoluteCommand", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Geass of Absolute Command");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, issue an absolute psychological decree to an enemy within 50 feet, paralyzing them for 1 round with no saving throw allowed (1 use per day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dominate;
				bp.IsClassFeature = true;
				BlueprintAbilityResource res = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.IsekaiContext, "MastermindGeassResource");
				if (res != null)
				{
					bp.AddComponent(delegate(AddAbilityResources c)
					{
						c.m_Resource = res.ToReference<BlueprintAbilityResourceReference>();
						c.RestoreAmount = true;
					});
				}
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { GeassAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
