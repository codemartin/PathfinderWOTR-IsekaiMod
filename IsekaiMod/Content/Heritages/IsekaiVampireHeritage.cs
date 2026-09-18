using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
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
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiVampireHeritage
	{
		public static void Add()
		{
			Sprite Icon_Vampire = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_VAMPIRE.png");
			Sprite Icon_Mist = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98")).m_Icon;
			BlueprintAbilityResource VampiricMistResource = Helpers.CreateBlueprint(Main.IsekaiContext, "VampiricMistResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintBuff VampiricMistBuff = TTCoreExtensions.CreateBuff("VampiricMistBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Vampiric Mist Form");
				bp.SetDescription(Main.IsekaiContext, "You disperse your physical form into a swirling cloud of ethereal mist. You gain DR 50/-, a +30 ft bonus to movement speed, and immunity to ground hazards, but cannot make physical attacks or cast spells for 2 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mist;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 50;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Speed;
					c.Value = 30;
				});
				bp.AddComponent<ForbidSpellCasting>();
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.CanNotAttack;
				});
			});
			BlueprintAbility VampiricMistAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "VampiricMistAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Vampiric Mist Form");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, dissolve into mist for 2 rounds. Grants DR 50/- and +30 ft movement speed.");
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
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Constitution
				};
			});
			BlueprintAbility BloodDrainBiteAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "BloodDrainBiteAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Blood Drain");
				bp.SetDescription(Main.IsekaiContext, "Sink your fangs into an adjacent living creature. Deals 2d6 unholy damage, drains 2 Constitution, and heals you for 15 hit points.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Vampire;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Touch;
				bp.CanTargetEnemies = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Energy,
							Energy = DamageEnergyType.Unholy
						},
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = 2,
							BonusValue = 0
						}
					}, new DealStatDamage
					{
						Stat = StatType.Constitution,
						IsDrain = true,
						DamageBonus = 2,
						DamageDice = new DiceFormula(0, DiceType.Zero),
						Target = new ContextTargetUnit()
					}, new ContextActionOnContextCaster
					{
						Actions = Helpers.CreateActionList(new ContextActionHealTarget
						{
							Value = new ContextDiceValue
							{
								DiceType = DiceType.Zero,
								DiceCountValue = 0,
								BonusValue = 15
							}
						})
					});
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
				bp.SetDescription(Main.IsekaiContext, "Reincarnated into Golarion as a true Vampire Lord, you possess otherworldly beauty, immortal regeneration, and dominion over blood and mist. Unlike ordinary vampires, you suffer no penalties to Constitution and are a Daywalker uninhibited by the light.\nThe Isekai Vampire Lord has a +4 racial bonus to Dexterity and Charisma, a +2 racial bonus to Constitution and Intelligence, and a +2 racial bonus on Persuasion and Perception checks.\nThey possess DR 10/Magic and Silver, Fast Healing equal to their character level, immunity to poison, disease, mind-affecting, and death effects, cold and electricity resistance 20, the Blood Drain bite, and the Vampiric Mist Form ability.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Vampire;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Intelligence;
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
					c.Value = 10;
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
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 20;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Charm | SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Charm | SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[2]
					{
						VampiricMistAbility.ToReference<BlueprintUnitFactReference>(),
						BloodDrainBiteAbility.ToReference<BlueprintUnitFactReference>()
					};
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
				bp.Groups = new FeatureGroup[2]
				{
					FeatureGroup.Racial,
					FeatureGroup.DhampirHeritage
				};
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.DhampirHeritageSelection.AddToSelection(feature);
			HumanHeritageSelection.Register(feature);
		}
	}
}
