using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
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
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;

internal static class SlimePredatorArts
{
	private static readonly Sprite Icon_Web = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("134cb6d492269aa4f8662700ef57449f"))?.m_Icon;

	private static readonly Sprite Icon_Armor = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Web;

	private static readonly Sprite Icon_Hellfire = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("700cfcbd0cb2975419bcab7dbb8c6210"))?.m_Icon ?? Icon_Web;

	private static readonly Sprite Icon_Haste = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280"))?.m_Icon ?? Icon_Web;

	private static readonly Sprite Icon_Thought = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("4733d8dd549ff544395f1684ec73c392"))?.m_Icon ?? Icon_Web;

	private static readonly Sprite Icon_DragonBreath = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("2809e762f83cbdb47b2f8b7816cc2a34"))?.m_Icon ?? Icon_Web;

	private static readonly Sprite Icon_Frenzy = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("b175001b42b1a02479881b72fe132116"))?.m_Icon ?? Icon_Web;

	public static BlueprintAbility PredatorArtStickyThreadAbility;

	public static BlueprintFeature PredatorArtStickyThreadFeature;

	public static BlueprintFeature PredatorArtBodyArmorFeature;

	public static BlueprintAbility PredatorArtBlackFlameAbility;

	public static BlueprintFeature PredatorArtBlackFlameFeature;

	public static BlueprintAbility PredatorArtThoughtAccelerationAbility;

	public static BlueprintFeature PredatorArtThoughtAccelerationFeature;

	public static BlueprintAbility PredatorArtDragonBreathAbility;

	public static BlueprintFeature PredatorArtDragonBreathFeature;

	public static BlueprintBuff PredatorFrenzyBuff;

	public static BlueprintBuff SlimeDemonSurgeBuff;

	public static BlueprintBuff SlimeDragonSurgeBuff;

	public static BlueprintBuff SlimeUndeadSurgeBuff;

	public static BlueprintBuff SlimeBeastSurgeBuff;

	public static BlueprintBuff SlimeConstructSurgeBuff;

	public static BlueprintBuff SlimeCasterSurgeBuff;

	private const string FireConeBreath30Feet = "52c3a84f628ddde4dbfb38e4a581b01a";

	private const string FireCone50Feet = "4c272644b29989a40bcf1e6003cfe708";

	private static void WireSurgesIntoDevourPrey()
	{
		AbilityEffectRunAction abilityEffectRunAction = BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "DevourPreyAbility")?.GetComponent<AbilityEffectRunAction>();
		if (abilityEffectRunAction?.Actions == null)
		{
			Main.IsekaiContext.Logger.LogError("[SlimePredatorArts] Devour Prey or its action list is missing, so no Species Surge can be granted.");
			return;
		}
		List<GameAction> list = new List<GameAction>();
		if (PredatorFrenzyBuff != null)
		{
			list.Add(OnCaster(new GameAction[1] { OneMinute(PredatorFrenzyBuff) }));
		}
		if (SlimeDemonSurgeBuff != null)
		{
			list.Add(IfTarget(SlimeDemonSurgeBuff, new Condition[1] { Is("dc960a234d365cb4f905bdc5937e623a") }));
		}
		if (SlimeDragonSurgeBuff != null)
		{
			list.Add(IfTarget(SlimeDragonSurgeBuff, new Condition[1] { Is("455ac88e22f55804ab87c2467deff1d6") }));
		}
		if (SlimeUndeadSurgeBuff != null)
		{
			list.Add(IfTarget(SlimeUndeadSurgeBuff, new Condition[1] { Is("734a29b693e9ec346ba2951b27987e33") }));
		}
		if (SlimeBeastSurgeBuff != null)
		{
			list.Add(IfTarget(SlimeBeastSurgeBuff, new Condition[2]
			{
				Is("a95311b3dc996964cbaa30ff9965aaf6"),
				Is("625827490ea69d84d8e599a33929fdc6")
			}));
		}
		if (SlimeConstructSurgeBuff != null)
		{
			list.Add(IfTarget(SlimeConstructSurgeBuff, new Condition[1] { Is("fd389783027d63343b4a5634bd81645f") }));
		}
		if (SlimeCasterSurgeBuff != null)
		{
			list.Add(IfTarget(SlimeCasterSurgeBuff, new Condition[2]
			{
				new ContextConditionTargetIsArcaneCaster(),
				new ContextConditionTargetIsDivineCaster()
			}));
		}
		abilityEffectRunAction.Actions.Actions = abilityEffectRunAction.Actions.Actions.Concat(list).ToArray();
		static Conditional IfTarget(BlueprintBuff buff, params Condition[] conditions)
		{
			return new Conditional
			{
				ConditionsChecker = new ConditionsChecker
				{
					Conditions = conditions,
					Operation = Operation.Or
				},
				IfTrue = Helpers.CreateActionList(OnCaster(new GameAction[1] { OneMinute(buff) })),
				IfFalse = Helpers.CreateActionList()
			};
		}
		static ContextConditionHasFact Is(string factId)
		{
			return new ContextConditionHasFact
			{
				m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(factId)
			};
		}
		static GameAction OnCaster(params GameAction[] actions)
		{
			return new ContextActionOnContextCaster
			{
				Actions = Helpers.CreateActionList(actions)
			};
		}
		static ContextActionApplyBuff OneMinute(BlueprintBuff buff)
		{
			return new ContextActionApplyBuff
			{
				m_Buff = buff.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Minutes,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = 1
				},
				AsChild = false
			};
		}
	}

	private static void AddConeDelivery(BlueprintAbility bp, int feet, string projectileId)
	{
		bp.AddComponent(delegate(AbilityDeliverProjectile c)
		{
			c.Type = AbilityProjectileType.Cone;
			c.m_Length = new Feet(feet);
			c.m_LineWidth = new Feet(5f);
			c.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>(projectileId) };
		});
	}

	private static void AddConstitutionSaveDC(BlueprintAbility bp, BlueprintUnitProperty halfProtagonistLevel)
	{
		bp.AddComponent(delegate(ContextCalculateAbilityParams c)
		{
			c.StatType = StatType.Constitution;
			c.UseKineticistMainStat = false;
			c.ReplaceSpellLevel = true;
			c.SpellLevel = Values.CreateContextCasterCustomPropertyValue(halfProtagonistLevel);
		});
	}

	private static ContextActionDealDamage HalfBreathDamage(DiceType dice, DamageEnergyType energy)
	{
		return new ContextActionDealDamage
		{
			DamageType = new DamageTypeDescription
			{
				Type = DamageType.Energy,
				Energy = energy,
				Common = new DamageTypeDescription.CommomData(),
				Physical = new DamageTypeDescription.PhysicalData()
			},
			Value = new ContextDiceValue
			{
				DiceType = dice,
				DiceCountValue = Values.CreateContextRankValue(AbilityRankType.DamageDice),
				BonusValue = 0
			},
			HalfIfSaved = true,
			IsAoE = true
		};
	}

	private static void AddBreathRankConfigs(BlueprintAbility bp)
	{
		bp.AddContextRankConfig(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.Default;
			c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
			c.m_Class = IsekaiAscendantClass.ClassLevelScalingClasses();
		});
		bp.AddContextRankConfig(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.DamageDice;
			c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
			c.m_Progression = ContextRankProgression.Div2;
			c.m_Class = IsekaiAscendantClass.ClassLevelScalingClasses();
		});
	}

	public static void Add()
	{
		BlueprintUnitProperty PredatorArtsDCLevelProperty = Values.CreateHalfLevelProperty("PredatorArtsDCLevelProperty", protagonistLevel: true);
		BlueprintAbilityResource BlackFlameResource = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtBlackFlameResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = DailyUses.Amount(3);
		});
		BlueprintAbilityResource DraconicBreathResource = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtDragonBreathResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = DailyUses.Amount(3);
		});
		PredatorFrenzyBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorFrenzyBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Frenzy");
			bp.SetDescription(Main.IsekaiContext, "Consumption of a worthy adversary triggers an instinctual apex predator frenzy: +4 inherent bonus to Strength and Constitution, +30 ft base movement speed, and one additional attack per round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Frenzy;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(BuffMovementSpeed c)
			{
				c.Value = 30;
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.ContextBonus = new ContextValue();
			});
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
		});
		SlimeDemonSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeDemonSurgeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Species Surge: Abyssal Titan");
			bp.SetDescription(Main.IsekaiContext, "Absorbing demonic essence grants DR 10/Good and a +4 profane bonus to Strength for 1 minute.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Frenzy;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
				c.BypassedByAlignment = true;
				c.Alignment = DamageAlignment.Good;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
		});
		SlimeDragonSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeDragonSurgeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Species Surge: Ancient Drake");
			bp.SetDescription(Main.IsekaiContext, "Absorbing draconic essence grants a +4 natural armor bonus, immunity to paralysis, and +4 Constitution for 1 minute.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = (ModifierDescriptor)1718;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Paralysis;
			});
		});
		SlimeUndeadSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeUndeadSurgeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Species Surge: Lich Core");
			bp.SetDescription(Main.IsekaiContext, "Absorbing necrotic core energy grants immunity to ability drain, energy drain, and mind-affecting effects for 1 minute.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Hellfire;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.NegativeLevel;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.NegativeLevel;
			});
			bp.AddComponent<AddImmunityToEnergyDrain>();
			bp.AddComponent(delegate(AddImmunityToAbilityScoreDamage c)
			{
				c.Drain = true;
				c.StatTypes = new StatType[0];
			});
		});
		SlimeBeastSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeBeastSurgeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Species Surge: Primal Apex");
			bp.SetDescription(Main.IsekaiContext, "Absorbing primeval beast traits grants Pounce and a +30 foot bonus to movement speed for 1 minute.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Haste;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(BuffMovementSpeed c)
			{
				c.Value = 30;
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.ContextBonus = new ContextValue();
			});
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.Pounce;
			});
		});
		SlimeConstructSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeConstructSurgeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Species Surge: Adamantine Matrix");
			bp.SetDescription(Main.IsekaiContext, "Absorbing construct plating grants DR 10/Adamantine and immunity to bleed and critical hits for 1 minute.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Armor;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
				c.BypassedByMaterial = true;
				c.Material = PhysicalDamageMaterial.Adamantite;
			});
			bp.AddComponent<AddImmunityToCriticalHits>();
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Bleed;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Bleed;
			});
		});
		SlimeCasterSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeCasterSurgeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Species Surge: Arcane Archon");
			bp.SetDescription(Main.IsekaiContext, "Absorbing high-tier spellcaster essence grants a +2 bonus to the DC of all spells and spell penetration for 1 minute.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Web;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 2;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 4;
			});
		});
		WireSurgesIntoDevourPrey();
		BlueprintBuff StickyThreadBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "StickyThreadBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Sticky Thread: Restrained");
			bp.SetDescription(Main.IsekaiContext, "Entangled in unbreakable adhesive slime filaments: cannot move, suffers a -2 penalty to attack rolls and -4 penalty to Dexterity.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Web;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddCondition c)
			{
				c.Condition = UnitCondition.Entangled;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Penalty;
				c.Stat = StatType.Dexterity;
				c.Value = -4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Penalty;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = -2;
			});
		});
		PredatorArtStickyThreadAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtStickyThreadAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Sticky Steel Thread");
			bp.SetDescription(Main.IsekaiContext, "As a standard action, launch viscous tensile threads at an enemy within 40 feet. The target is Entangled and immobilized for 1 minute (Reflex DC = 10 + 1/2 protagonist level + Constitution modifier negates).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Web;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Close;
			bp.CanTargetEnemies = true;
			bp.CanTargetFriends = false;
			bp.CanTargetSelf = false;
			bp.CanTargetPoint = false;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			AddConstitutionSaveDC(bp, PredatorArtsDCLevelProperty);
			ContextActionApplyBuff applyDebuff = new ContextActionApplyBuff
			{
				m_Buff = StickyThreadBuff.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Minutes,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = 1
				}
			};
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionSavingThrow
				{
					Type = SavingThrowType.Reflex,
					Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
					{
						Failed = Helpers.CreateActionList(applyDebuff)
					})
				});
			});
		});
		PredatorArtStickyThreadFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtStickyThreadFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Sticky Steel Thread");
			bp.SetDescription(Main.IsekaiContext, "You produce hyper-resilient steel filaments from your fluid biology. You gain the Sticky Steel Thread active ability to entangle and ground foes.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Web;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { PredatorArtStickyThreadAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
		PredatorArtBodyArmorFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtBodyArmorFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Viscous Body Armor");
			bp.SetDescription(Main.IsekaiContext, "Your slime cells continuously absorb impact and harden dynamically. You gain DR 5/Slashing, immunity to critical hits and sneak attacks, and an inherent +1 natural armor bonus per 3 protagonist levels.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Armor;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 5;
				c.BypassedByMaterial = false;
				c.BypassedByForm = true;
				c.Form = PhysicalDamageForm.Slashing;
			});
			bp.AddComponent<AddImmunityToCriticalHits>();
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = (ModifierDescriptor)1718;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
		});
		PredatorArtBlackFlameAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtBlackFlameAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Black Flame Hellfire");
			bp.SetDescription(Main.IsekaiContext, "Unleash a 30-foot cone of unquenchable black hellfire. Enemies in the area take 1d6 damage per protagonist level (half unholy, half fire). A successful Reflex save halves the damage (DC = 10 + 1/2 protagonist level + Constitution modifier). Allies in the cone are not harmed. Usable 3 times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Hellfire;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Close;
			bp.CanTargetPoint = true;
			bp.CanTargetEnemies = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
			AddConeDelivery(bp, 30, "52c3a84f628ddde4dbfb38e4a581b01a");
			AddConstitutionSaveDC(bp, PredatorArtsDCLevelProperty);
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(Conditional cond)
				{
					cond.ConditionsChecker = ActionFlow.IfSingle<ContextConditionIsEnemy>();
					cond.IfFalse = ActionFlow.DoNothing();
					cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionSavingThrow save)
					{
						save.Type = SavingThrowType.Reflex;
						save.Actions = Helpers.CreateActionList(HalfBreathDamage(DiceType.D6, DamageEnergyType.Unholy), HalfBreathDamage(DiceType.D6, DamageEnergyType.Fire));
					});
				});
			});
			DailyUses.Limit(bp, BlackFlameResource);
			AddBreathRankConfigs(bp);
		});
		PredatorArtBlackFlameFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtBlackFlameFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Black Flame Hellfire");
			bp.SetDescription(Main.IsekaiContext, "Synthesized from devoured demon lords and fire elementals, you master Black Flame, unleashing an unquenchable cone of unholy hellfire.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Hellfire;
			bp.IsClassFeature = true;
			DailyUses.Grant(bp, BlackFlameResource);
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { PredatorArtBlackFlameAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
		BlueprintBuff ThoughtAccelBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ThoughtAccelBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Thought Acceleration: Hyper-Speed");
			bp.SetDescription(Main.IsekaiContext, "Perception of time accelerated by 1,000%: +4 insight bonus to AC and attack rolls, and one extra attack when making a full attack, which stacks with Haste.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Thought;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
		});
		PredatorArtThoughtAccelerationAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtThoughtAccelerationAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Thought Acceleration");
			bp.SetDescription(Main.IsekaiContext, "As a swift action, accelerate neural processing speed by a thousandfold for 1 round. Grants a +4 insight bonus to AC, +4 to attack rolls, and one extra attack when making a full attack, which stacks with Haste.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Thought;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.ActionType = UnitCommand.CommandType.Swift;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			ContextActionApplyBuff applySelf = new ContextActionApplyBuff
			{
				m_Buff = ThoughtAccelBuff.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Rounds,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = 2
				},
				ToCaster = true
			};
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(applySelf);
			});
		});
		PredatorArtThoughtAccelerationFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtThoughtAccelerationFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Thought Acceleration");
			bp.SetDescription(Main.IsekaiContext, "Your fluid internal core functions as a multi-threaded organic supercomputer. You gain the Thought Acceleration active ability (swift action).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Thought;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { PredatorArtThoughtAccelerationAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
		PredatorArtDragonBreathAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtDragonBreathAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Draconic Plasma Breath");
			bp.SetDescription(Main.IsekaiContext, "Exhale a 50-foot cone of pure plasma synthesized from devoured ancient dragons. Deals 1d8 damage per protagonist level (half sonic, half divine). Reflex save halves (DC = 10 + 1/2 protagonist level + Constitution modifier). Allies in the cone are not harmed. Usable 3 times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Close;
			bp.CanTargetPoint = true;
			bp.CanTargetEnemies = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
			AddConeDelivery(bp, 50, "4c272644b29989a40bcf1e6003cfe708");
			AddConstitutionSaveDC(bp, PredatorArtsDCLevelProperty);
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(Conditional cond)
				{
					cond.ConditionsChecker = ActionFlow.IfSingle<ContextConditionIsEnemy>();
					cond.IfFalse = ActionFlow.DoNothing();
					cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionSavingThrow save)
					{
						save.Type = SavingThrowType.Reflex;
						save.Actions = Helpers.CreateActionList(HalfBreathDamage(DiceType.D8, DamageEnergyType.Sonic), HalfBreathDamage(DiceType.D8, DamageEnergyType.Divine));
					});
				});
			});
			DailyUses.Limit(bp, DraconicBreathResource);
			AddBreathRankConfigs(bp);
		});
		PredatorArtDragonBreathFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtDragonBreathFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Predator Art: Draconic Plasma Breath");
			bp.SetDescription(Main.IsekaiContext, "Assimilation of draconic dragon-factor enables you to exhale a catastrophic cone of divine sonic plasma.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
			bp.IsClassFeature = true;
			DailyUses.Grant(bp, DraconicBreathResource);
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { PredatorArtDragonBreathAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
