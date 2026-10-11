using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

internal class SubclassCantrips
{
	private static readonly Sprite Icon_AcidSplash = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0c852a2405dd9f14a8bbcfaf245ff823"))?.m_Icon;

	private static readonly Sprite Icon_DisruptUndead = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("652739779aa05504a9ad5db1db6d02ae"))?.m_Icon;

	private static readonly Sprite Icon_RayOfFrost = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("9af2ab69df6538f4793b2f9c3cc85603"))?.m_Icon;

	private static readonly Sprite Icon_DivineZap = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("8a1992f59e06dd64ab9ba52337bf8cb5"))?.m_Icon;

	private static readonly Sprite Icon_EarPiercingScream = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("8e7cfa5f213a90549aadd18f8f6f4664"))?.m_Icon;

	internal static readonly string[] AbilityNames = new string[5] { "ShadowDaggerAbility", "HeavenlyRayAbility", "CorrosiveSlimeAbility", "MindSpikeAbility", "GraveRayAbility" };

	internal static readonly ContextRankConfig.CustomProgressionItem[] HeavenlyRayDoubledProgression = new ContextRankConfig.CustomProgressionItem[10]
	{
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 7,
			ProgressionValue = 2
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 11,
			ProgressionValue = 4
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 15,
			ProgressionValue = 6
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 19,
			ProgressionValue = 8
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 23,
			ProgressionValue = 10
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 27,
			ProgressionValue = 12
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 31,
			ProgressionValue = 14
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 35,
			ProgressionValue = 16
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 39,
			ProgressionValue = 18
		},
		new ContextRankConfig.CustomProgressionItem
		{
			BaseValue = 1000,
			ProgressionValue = 20
		}
	};

	public static void Add()
	{
		BlueprintBuff ShadowDaggerDebuff = TTCoreExtensions.CreateBuff("ShadowDaggerDebuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Vulnerability");
			bp.SetDescription(Main.IsekaiContext, "Target suffers a -2 penalty to Armor Class for 1 round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_RayOfFrost;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Penalty;
				c.Stat = StatType.AC;
				c.Value = -2;
			});
		});
		ContextRankConfig ShadowDaggerRankConfig = Helpers.CreateContextRankConfig(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.Default;
			c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
			c.m_Progression = ContextRankProgression.DivStep;
			c.m_StepLevel = 4;
			c.m_UseMin = true;
			c.m_Min = 1;
			c.m_UseMax = true;
			c.m_Max = 10;
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowDaggerAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Dagger");
			bp.SetDescription(Main.IsekaiContext, "You throw a dagger woven from absolute darkness as a ranged touch attack. Deals 1d4 piercing damage plus 1d4 cold damage per 4 caster levels (up to 10d4 at level 40). Targets struck suffer a -2 penalty to AC for 1 round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_RayOfFrost;
			bp.Type = AbilityType.Spell;
			bp.Range = AbilityRange.Close;
			bp.CanTargetEnemies = true;
			bp.CanTargetSelf = false;
			bp.SpellResistance = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Necromancy;
			});
			bp.AddComponent<CantripComponent>();
			bp.AddComponent(delegate(AbilityDeliverProjectile c)
			{
				c.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("d6c9daec1256561408a7a72a6979359e") };
				c.Type = AbilityProjectileType.Simple;
				c.IsHandOfTheApprentice = false;
				c.NeedAttackRoll = true;
				c.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("f6ef95b1f7bb52b408a5b345a330ffe8");
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.SavingThrowType = SavingThrowType.Unknown;
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionDealDamage d)
				{
					d.DamageType = new DamageTypeDescription
					{
						Type = DamageType.Physical,
						Physical = new DamageTypeDescription.PhysicalData
						{
							Form = PhysicalDamageForm.Piercing
						}
					};
					d.Value = new ContextDiceValue
					{
						DiceType = DiceType.D4,
						DiceCountValue = 1,
						BonusValue = 0
					};
				});
				c.Actions.Actions = c.Actions.Actions.AppendToArray(Helpers.Create(delegate(ContextActionDealDamage d)
				{
					d.DamageType = new DamageTypeDescription
					{
						Type = DamageType.Energy,
						Energy = DamageEnergyType.Cold
					};
					d.Value = new ContextDiceValue
					{
						DiceType = DiceType.D4,
						DiceCountValue = new ContextValue
						{
							ValueType = ContextValueType.Rank,
							ValueRank = AbilityRankType.Default
						},
						BonusValue = 0
					};
				}), Helpers.Create(delegate(ContextActionApplyBuff a)
				{
					a.m_Buff = ShadowDaggerDebuff.ToReference<BlueprintBuffReference>();
					a.DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = 1
					};
				}));
			});
			bp.AddComponent(ShadowDaggerRankConfig);
		});
		ContextRankConfig HeavenlyRayRankConfig = Helpers.CreateContextRankConfig(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.Default;
			c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
			c.m_Progression = ContextRankProgression.DivStep;
			c.m_StepLevel = 4;
			c.m_UseMin = true;
			c.m_Min = 1;
			c.m_UseMax = true;
			c.m_Max = 10;
		});
		ContextRankConfig HeavenlyRayDoubledRankConfig = Helpers.CreateContextRankConfig(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.DamageDiceAlternative;
			c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
			c.m_Progression = ContextRankProgression.Custom;
			c.m_CustomProgression = HeavenlyRayDoubledProgression;
		});
		BlueprintUnitFactReference HeavenlyRayUndeadType = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33");
		BlueprintUnitFactReference HeavenlyRayOutsiderType = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("9054d3988d491d944ac144e27b6bc318");
		Helpers.CreateBlueprint(Main.IsekaiContext, "HeavenlyRayAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Heavenly Ray");
			bp.SetDescription(Main.IsekaiContext, "You fire a focused beam of imperial holy radiance as a ranged touch attack. Deals 1d6 holy damage per 4 caster levels (up to 10d6 at level 40). Damage is doubled against undead and evil outsiders.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DivineZap;
			bp.Type = AbilityType.Spell;
			bp.Range = AbilityRange.Close;
			bp.CanTargetEnemies = true;
			bp.CanTargetSelf = false;
			bp.SpellResistance = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Evocation;
			});
			bp.AddComponent<CantripComponent>();
			bp.AddComponent(delegate(AbilityDeliverProjectile c)
			{
				c.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("2511627d593387d4d89004bec111ba31") };
				c.Type = AbilityProjectileType.Simple;
				c.NeedAttackRoll = true;
				c.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("f6ef95b1f7bb52b408a5b345a330ffe8");
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.SavingThrowType = SavingThrowType.Unknown;
				c.Actions = ActionFlow.DoSingle(delegate(Conditional undead)
				{
					undead.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionHasFact f)
					{
						f.m_Fact = HeavenlyRayUndeadType;
					});
					undead.IfTrue = new ActionList
					{
						Actions = new GameAction[1] { HolyDamage(AbilityRankType.DamageDiceAlternative) }
					};
					undead.IfFalse = ActionFlow.DoSingle(delegate(Conditional fiend)
					{
						fiend.ConditionsChecker = new ConditionsChecker
						{
							Operation = Operation.And,
							Conditions = new Condition[2]
							{
								new ContextConditionHasFact
								{
									m_Fact = HeavenlyRayOutsiderType
								},
								new ContextConditionAlignment
								{
									Alignment = AlignmentComponent.Evil
								}
							}
						};
						fiend.IfTrue = new ActionList
						{
							Actions = new GameAction[1] { HolyDamage(AbilityRankType.DamageDiceAlternative) }
						};
						fiend.IfFalse = new ActionList
						{
							Actions = new GameAction[1] { HolyDamage(AbilityRankType.Default) }
						};
					});
				});
			});
			bp.AddComponent(HeavenlyRayRankConfig);
			bp.AddComponent(HeavenlyRayDoubledRankConfig);
		});
		BlueprintBuff CorrosiveSlimeDebuff = TTCoreExtensions.CreateBuff("CorrosiveSlimeDebuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Slime Corrosion");
			bp.SetDescription(Main.IsekaiContext, "Target suffers a -2 penalty to spell resistance for 2 rounds.");
			((BlueprintUnitFact)bp).m_Icon = Icon_AcidSplash;
			bp.AddComponent(delegate(AddSpellResistance c)
			{
				c.AllSpellResistancePenaltyDoNotUse = true;
				c.Value = new ContextValue
				{
					ValueType = ContextValueType.Simple,
					Value = 2
				};
			});
		});
		ContextRankConfig CorrosiveSlimeRankConfig = Helpers.CreateContextRankConfig(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.Default;
			c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
			c.m_Progression = ContextRankProgression.DivStep;
			c.m_StepLevel = 4;
			c.m_UseMin = true;
			c.m_Min = 1;
			c.m_UseMax = true;
			c.m_Max = 10;
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "CorrosiveSlimeAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Corrosive Slime");
			bp.SetDescription(Main.IsekaiContext, "You spit a globule of living acidic slime as a ranged touch attack. Deals 1d6 acid damage per 4 caster levels (up to 10d6 at level 40), and reduces the target's spell resistance by 2 for 2 rounds.");
			((BlueprintUnitFact)bp).m_Icon = Icon_AcidSplash;
			bp.Type = AbilityType.Spell;
			bp.Range = AbilityRange.Close;
			bp.CanTargetEnemies = true;
			bp.CanTargetSelf = false;
			bp.SpellResistance = false;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Conjuration;
			});
			bp.AddComponent<CantripComponent>();
			bp.AddComponent(delegate(AbilityDeliverProjectile c)
			{
				c.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("d8abd128c02331a45a4f250a62722e8b") };
				c.Type = AbilityProjectileType.Simple;
				c.NeedAttackRoll = true;
				c.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("f6ef95b1f7bb52b408a5b345a330ffe8");
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.SavingThrowType = SavingThrowType.Unknown;
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionDealDamage d)
				{
					d.DamageType = new DamageTypeDescription
					{
						Type = DamageType.Energy,
						Energy = DamageEnergyType.Acid
					};
					d.Value = new ContextDiceValue
					{
						DiceType = DiceType.D6,
						DiceCountValue = new ContextValue
						{
							ValueType = ContextValueType.Rank,
							ValueRank = AbilityRankType.Default
						},
						BonusValue = 0
					};
				});
				c.Actions.Actions = c.Actions.Actions.AppendToArray(Helpers.Create(delegate(ContextActionApplyBuff a)
				{
					a.m_Buff = CorrosiveSlimeDebuff.ToReference<BlueprintBuffReference>();
					a.DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = 2
					};
				}));
			});
			bp.AddComponent(CorrosiveSlimeRankConfig);
		});
		ContextRankConfig MindSpikeRankConfig = Helpers.CreateContextRankConfig(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.Default;
			c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
			c.m_Progression = ContextRankProgression.DivStep;
			c.m_StepLevel = 4;
			c.m_UseMin = true;
			c.m_Min = 1;
			c.m_UseMax = true;
			c.m_Max = 10;
		});
		BlueprintBuff ShakenBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("25ec6cb6ab1845c48a95f9c20b034220");
		Helpers.CreateBlueprint(Main.IsekaiContext, "MindSpikeAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Mind Spike");
			bp.SetDescription(Main.IsekaiContext, "You mentally overload an enemy's cognition. Target must succeed on a Will saving throw or suffer 1d4 untyped psychic damage per 4 caster levels (up to 10d4 at level 40) and become shaken for 1 round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_EarPiercingScream;
			bp.Type = AbilityType.Spell;
			bp.Range = AbilityRange.Close;
			bp.CanTargetEnemies = true;
			bp.CanTargetSelf = false;
			bp.SpellResistance = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Enchantment;
			});
			bp.AddComponent<CantripComponent>();
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionSavingThrow s)
				{
					s.Type = SavingThrowType.Will;
					s.Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved cond)
					{
						cond.Succeed = ActionFlow.DoNothing();
						cond.Failed = Helpers.CreateActionList(new ContextActionDealDamage
						{
							DamageType = new DamageTypeDescription
							{
								Type = DamageType.Direct
							},
							Value = new ContextDiceValue
							{
								DiceType = DiceType.D4,
								DiceCountValue = new ContextValue
								{
									ValueType = ContextValueType.Rank,
									ValueRank = AbilityRankType.Default
								},
								BonusValue = 0
							}
						});
						if (ShakenBuff != null)
						{
							cond.Failed.Actions = cond.Failed.Actions.AppendToArray(Helpers.Create(delegate(ContextActionApplyBuff a)
							{
								a.m_Buff = ShakenBuff.ToReference<BlueprintBuffReference>();
								a.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Rounds,
									DiceType = DiceType.Zero,
									BonusValue = 1
								};
							}));
						}
					});
				});
			});
			bp.AddComponent(MindSpikeRankConfig);
		});
		ContextRankConfig GraveRayRankConfig = Helpers.CreateContextRankConfig(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.Default;
			c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
			c.m_Progression = ContextRankProgression.DivStep;
			c.m_StepLevel = 4;
			c.m_UseMin = true;
			c.m_Min = 1;
			c.m_UseMax = true;
			c.m_Max = 10;
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "GraveRayAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Grave Ray");
			bp.SetDescription(Main.IsekaiContext, "You channel a ray of negative necromantic energy as a ranged touch attack. Deals 1d6 negative energy damage per 4 caster levels (up to 10d6 at level 40) to living targets, or heals undead targets for the same amount.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DisruptUndead;
			bp.Type = AbilityType.Spell;
			bp.Range = AbilityRange.Close;
			bp.CanTargetEnemies = true;
			bp.CanTargetFriends = true;
			bp.SpellResistance = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Necromancy;
			});
			bp.AddComponent<CantripComponent>();
			bp.AddComponent(delegate(AbilityDeliverProjectile c)
			{
				c.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("d543d55f7fdb60340af40ea8fc5e686d") };
				c.Type = AbilityProjectileType.Simple;
				c.NeedAttackRoll = true;
				c.m_Weapon = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("f6ef95b1f7bb52b408a5b345a330ffe8");
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.SavingThrowType = SavingThrowType.Unknown;
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionDealDamage d)
				{
					d.DamageType = new DamageTypeDescription
					{
						Type = DamageType.Energy,
						Energy = DamageEnergyType.NegativeEnergy
					};
					d.Value = new ContextDiceValue
					{
						DiceType = DiceType.D6,
						DiceCountValue = new ContextValue
						{
							ValueType = ContextValueType.Rank,
							ValueRank = AbilityRankType.Default
						},
						BonusValue = 0
					};
				});
			});
			bp.AddComponent(GraveRayRankConfig);
		});
		static ContextActionDealDamage HolyDamage(AbilityRankType rank)
		{
			return new ContextActionDealDamage
			{
				DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Divine
				},
				Duration = new ContextDurationValue
				{
					DiceCountValue = 0,
					BonusValue = 0
				},
				Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = rank
					},
					BonusValue = 0
				}
			};
		}
	}
}
