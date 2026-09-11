using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige
{
	internal class TranscendentSovereignFeatures
	{
		private static readonly Sprite Icon_Authority = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("f3bc6f9c855b2fb4e9aea364b8163aca"))?.m_Icon;

		private static readonly Sprite Icon_PlotTwist = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd"))?.m_Icon;

		private static readonly Sprite Icon_Domain = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("247a4068296e8be42890143f451b4b45"))?.m_Icon;

		private static readonly Sprite Icon_Counter = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d7741c08ccf699e4a8a8f8ab2ed345f8"))?.m_Icon;

		private static readonly Sprite Icon_Rewrite = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("80a1a388ee938aa4e90d427ce9a7a3e9"))?.m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "TranscendentAuthority", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendent Authority");
				bp.SetDescription(Main.IsekaiContext, "Your mastery over planar realities grants an unquestioned dominion. You gain a +2 bonus to the difficulty class of all your spells and abilities, as well as a +2 bonus to your caster level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Authority;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseSpellDC c)
				{
					c.BonusDC = 2;
				});
				bp.AddComponent(delegate(AddCasterLevel c)
				{
					c.Bonus = 2;
				});
			});
			BlueprintAbilityResource PlotTwistResource = Helpers.CreateBlueprint(Main.IsekaiContext, "PlotTwistResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = false,
					IncreasedByStat = false,
					OtherClassesModifier = 0f
				};
			});
			BlueprintBuff TemporalStasisBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "TemporalStasisBuff");
			BlueprintBuff PlotTwistBuff = TTCoreExtensions.CreateBuff("PlotTwistBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Temporal Inversion");
				bp.SetDescription(Main.IsekaiContext, "Local causality bends to your sovereign authority. You gain an extra attack, True Seeing, increased speed, and a +4 dodge bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_PlotTwist;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Speed;
					c.Value = 30;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.TrueSeeing;
				});
			});
			BlueprintAbility PlotTwistAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PlotTwistAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Temporal Inversion - Sovereign Domain");
				bp.SetDescription(Main.IsekaiContext, "You declare your absolute authority over local space-time. As a swift action, you invert temporal flow: all enemies within 60 feet are frozen in temporal stasis for 2 rounds, while you gain an extra attack, True Seeing, increased speed, and a +4 dodge bonus to AC. Usable once per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_PlotTwist;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = 60.Feet();
					c.m_TargetType = TargetType.Enemy;
					c.m_IncludeDead = false;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
					{
						if (TemporalStasisBuff != null)
						{
							a.m_Buff = TemporalStasisBuff.ToReference<BlueprintBuffReference>();
						}
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
						a.IsNotDispelable = true;
					});
				});
				bp.AddComponent(delegate(AbilityExecuteActionOnCast c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
					{
						a.m_Buff = PlotTwistBuff.ToReference<BlueprintBuffReference>();
						a.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Rounds,
							DiceType = DiceType.Zero,
							BonusValue = new ContextValue
							{
								ValueType = ContextValueType.Simple,
								Value = 3
							}
						};
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = PlotTwistResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "PlotTwistFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Temporal Inversion - Sovereign Domain");
				bp.SetDescription(Main.IsekaiContext, "At 3rd level, you gain the ability to halt time and manifest sovereign authority over local space-time. Once per day as a swift action, freeze all enemies within 60 feet for 2 rounds while gaining an extra attack, True Seeing, increased speed, and a +4 dodge bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_PlotTwist;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { PlotTwistAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = PlotTwistResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
			TTCoreExtensions.CreateToggleAuraBuffFeature("TranscendentDomain", "At 5th level, you can project an aura out to 40 feet. Allies within gain a +4 morale bonus to attack rolls and saving throws, alongside immunity to fear and mind-affecting effects.", "Allies in your domain gain a +4 morale bonus to attack rolls and saving throws, alongside immunity to fear and mind-affecting effects.", Icon_Domain, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Fear;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "AbsoluteCounterFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Counter");
				bp.SetDescription(Main.IsekaiContext, "At 7th level, your defensive instincts reach perfection. You gain +10 spell resistance and a +4 dodge bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Counter;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 10
					};
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
			});
			BlueprintAbilityResource RealityRewriteResource = Helpers.CreateBlueprint(Main.IsekaiContext, "RealityRewriteResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = false,
					IncreasedByStat = false,
					OtherClassesModifier = 0f
				};
			});
			BlueprintAbility RealityRewriteAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "RealityRewriteAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Reality Rewrite");
				bp.SetDescription(Main.IsekaiContext, "You rewrite the fabric of reality itself. Once per day as a standard action, all fallen allies within 60 feet are restored to life with full hit points and vigor, while all enemies within 60 feet suffer 200 divine damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Rewrite;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = 60.Feet();
					c.m_TargetType = TargetType.Any;
					c.m_IncludeDead = true;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(Conditional cond)
					{
						cond.ConditionsChecker = ActionFlow.IfSingle<ContextConditionIsEnemy>();
						cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionDealDamage d)
						{
							d.DamageType = new DamageTypeDescription
							{
								Type = DamageType.Direct
							};
							d.Value = new ContextDiceValue
							{
								DiceType = DiceType.Zero,
								BonusValue = 200
							};
						});
						cond.IfFalse = ActionFlow.DoSingle(delegate(ContextActionResurrect r)
						{
							r.FullRestore = true;
						});
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = RealityRewriteResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "RealityRewriteFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Reality Rewrite");
				bp.SetDescription(Main.IsekaiContext, "At 10th level, you gain the supreme power to rewrite reality. Once per day as a standard action, resurrect and fully restore all fallen allies within 60 feet, while dealing 200 direct divine damage to all enemies in range.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Rewrite;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { RealityRewriteAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = RealityRewriteResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
			CreateSubclassPrestigeFeatures();
		}

		private static void CreateSubclassPrestigeFeatures()
		{
			TTCoreExtensions.CreateToggleAuraBuffFeature("HeavenlyImperialMandate", "At 5th level, replace Transcendent Domain with an aura granting allies within 40 feet a +6 sacred bonus to attack rolls, AC, and saving throws.", "Allies receive a +6 sacred bonus to attack rolls, AC, and saving throws vs evil creatures.", Icon_Domain, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 6;
				});
			});
			BlueprintBuff SingularityBuff = TTCoreExtensions.CreateBuff("GluttonousSingularityBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Gluttonous Singularity Target");
				bp.SetDescription(Main.IsekaiContext, "Enemies in the acid aura suffer a -4 penalty to AC and spell resistance.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Domain;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AC;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = -4
					};
				});
			});
			TTCoreExtensions.CreateToggleAuraFeature("GluttonousSingularity", Helpers.CreateString(Main.IsekaiContext, "GluttonousSingularity.Name", "Gluttonous Singularity"), Helpers.CreateString(Main.IsekaiContext, "GluttonousSingularity.Description", "At 5th level, replace Transcendent Domain with an acidic vortex dealing 4d6 acid damage each round to enemies within 30 feet, while reducing their AC and spell resistance by 4."), Icon_Domain, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.AffectEnemies = true;
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
				bp.Size = new Feet(30f);
				bp.AddUnconditionalAuraEffect(SingularityBuff.ToReference<BlueprintBuffReference>());
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.Round = ActionFlow.DoSingle(delegate(ContextActionDealDamage d)
					{
						d.DamageType = new DamageTypeDescription
						{
							Type = DamageType.Energy,
							Energy = DamageEnergyType.Acid
						};
						d.Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = 4,
							BonusValue = 0
						};
					});
				});
			});
			TTCoreExtensions.CreateToggleAuraBuffFeature("AbsoluteShadowDomain", "At 5th level, replace Transcendent Domain with an aura cloaking all allies in shadows for total concealment and extra weapon attacks.", "Allies and shadows gain total concealment and an additional attack.", Icon_Domain, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Total;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
			});
			TTCoreExtensions.CreateToggleAuraBuffFeature("AllAccordingToPlan", "At 5th level, replace Transcendent Domain with an aura granting allies a +4 insight bonus to attack rolls and saving throws, plus a +20 bonus to critical confirmation rolls.", "Allies receive a +4 insight bonus to attack rolls and saving throws, and a +20 bonus to critical confirmation rolls.", Icon_Domain, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Bonus = 20;
					c.Value = 20;
				});
			});
			TTCoreExtensions.CreateToggleAuraBuffFeature("TombOfTheSupremeBeing", "At 5th level, replace Transcendent Domain with an aura granting allies a +6 profane bonus to Strength and Charisma, fast healing 10, and immunity to negative levels.", "Allies gain a +6 profane bonus to Strength and Charisma, fast healing 10, and immunity to negative levels.", Icon_Domain, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.NegativeLevel;
				});
			});
			TTCoreExtensions.CreateToggleAuraBuffFeature("MiracleOfUnyieldingHope", "At 5th level, replace Transcendent Domain with an aura granting allies DR 10/-, 50% fortification, and healing 20 hit points each round.", "Allies gain DR 10/-, 50% fortification against critical hits and sneak attacks, and heal 20 HP at the start of each combat round.", Icon_Domain, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddFortification c)
				{
					c.Bonus = 50;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 20;
				});
			});
			TTCoreExtensions.CreateToggleAuraFeature("InfiniteBladeSymphony", Helpers.CreateString(Main.IsekaiContext, "InfiniteBladeSymphony.Name", "Infinite Blade Symphony"), Helpers.CreateString(Main.IsekaiContext, "InfiniteBladeSymphony.Description", "At 5th level, replace Transcendent Domain with an aura of orbiting phantom blades dealing 6d6 force damage each round to enemies within 25 feet."), Icon_Domain, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.AffectEnemies = true;
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
				bp.Size = new Feet(25f);
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.Round = ActionFlow.DoSingle(delegate(ContextActionDealDamage d)
					{
						d.DamageType = new DamageTypeDescription
						{
							Type = DamageType.Force
						};
						d.Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = 6,
							BonusValue = 0
						};
					});
				});
			});
		}
	}
}
