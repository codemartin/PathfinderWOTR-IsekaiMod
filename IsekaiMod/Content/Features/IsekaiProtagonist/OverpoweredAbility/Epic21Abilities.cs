using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal static class Epic21Abilities
	{
		private static readonly Sprite Icon_EpicCrown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e6e1a24ce33454342b7053889391642c"))?.m_Icon;

		private static readonly Sprite Icon_Time = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd"))?.m_Icon;

		private static readonly Sprite Icon_Vortex = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ab167fd8203c1314bac6568932f1752f"))?.m_Icon;

		private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_EpicCrown;

		private static readonly Sprite Icon_Sword = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("779179912e6c6fe458fa4cfb90d96e10"))?.m_Icon ?? Icon_EpicCrown;

		private static readonly Sprite Icon_Magic = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_EpicCrown;

		private static readonly BlueprintBuff StunnedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("09d39b38bb7c6014394b6daced9bacd3");

		private static readonly BlueprintFeature OutflankFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11");

		private static readonly BlueprintFeature SeizeTheMomentFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760");

		public static void Add()
		{
			BlueprintAbilityResource TimelineDivergenceResource = Helpers.CreateBlueprint(Main.IsekaiContext, "TimelineDivergenceResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevelStartPlusDivStep = false,
					StartingLevel = 0,
					StartingIncrease = 0,
					LevelStep = 0,
					PerStepIncrease = 0,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[0],
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 3;
			});
			BlueprintBuff TimelineDivergenceBuff = TTCoreExtensions.CreateBuff("TimelineDivergenceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Timeline Divergence");
				bp.SetDescription(Main.IsekaiContext, "Operating outside standard spacetime. For 1 round, your movement speed is increased by +50 feet, you gain 2 additional attacks at full BAB, and all spells are automatically Quickened.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Time;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 50;
					c.Descriptor = ModifierDescriptor.Enhancement;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
					c.Haste = false;
				});
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = Metamagic.Quicken;
				});
			});
			BlueprintAbility TimelineDivergenceAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "TimelineDivergenceAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Overpowered Ability - Timeline Divergence");
				bp.SetDescription(Main.IsekaiContext, "By splitting the flow of causality, you step into a parallel temporal stream for 1 round. \nBenefit: As a free action, gain +50 ft movement speed, 2 extra full-BAB attacks, and all spells are automatically Quickened for 1 round. Usable 3 times per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Time;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Free;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRound;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = TimelineDivergenceBuff.ToReference<BlueprintBuffReference>();
						b.DurationValue = Values.Duration.OneRound;
						b.Permanent = false;
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = TimelineDivergenceResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "TimelineDivergenceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Overpowered Ability - Timeline Divergence");
				bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Step into parallel timelines to take multiple standard actions and auto-quickened spells in a single turn.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Time;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { TimelineDivergenceAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = TimelineDivergenceResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "AkashicOmniscienceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Overpowered Ability - Akashic Omniscience");
				bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. You establish a direct link to the Akashic Records, the omniscient library containing all knowledge in the multiverse. \nBenefit: You gain a +10 insight bonus to all skill checks and a +4 bonus to all spell save DCs.");
				((BlueprintUnitFact)bp).m_Icon = Icon_EpicCrown;
				bp.IsClassFeature = true;
				StatType[] array = new StatType[11]
				{
					StatType.SkillAthletics,
					StatType.SkillMobility,
					StatType.SkillThievery,
					StatType.SkillStealth,
					StatType.SkillKnowledgeArcana,
					StatType.SkillKnowledgeWorld,
					StatType.SkillLoreNature,
					StatType.SkillLoreReligion,
					StatType.SkillPerception,
					StatType.SkillPersuasion,
					StatType.SkillUseMagicDevice
				};
				foreach (StatType skill in array)
				{
					bp.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Insight;
						c.Stat = skill;
						c.Value = 10;
					});
				}
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.Insight;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			BlueprintAbilityResource SingularityVortexResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SingularityVortexResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevelStartPlusDivStep = false,
					StartingLevel = 0,
					StartingIncrease = 0,
					LevelStep = 0,
					PerStepIncrease = 0,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[0],
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 3;
			});
			BlueprintAbility SingularityVortexAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SingularityVortexAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Overpowered Ability - Singularity Vortex");
				bp.SetDescription(Main.IsekaiContext, "Manifest a localized black hole that crushes all enemies within 40 feet for 20d6 force damage and forces a Fortitude save (DC = 15 + 1/2 Character Level + Cha) or become Stunned for 1 round. Usable 3 times per day as a swift action.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Vortex;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Medium;
				bp.CanTargetPoint = true;
				bp.CanTargetEnemies = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Reach;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.Instantaneous;
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = new Feet(40f);
					c.m_TargetType = TargetType.Enemy;
				});
				bp.AddComponent(delegate(ContextCalculateAbilityParams c)
				{
					c.UseKineticistMainStat = false;
					c.StatType = StatType.Charisma;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Force,
							Common = new DamageTypeDescription.CommomData(),
							Physical = new DamageTypeDescription.PhysicalData()
						},
						Duration = Values.Duration.Zero,
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = 20,
							BonusValue = 0
						}
					}, new ContextActionSavingThrow
					{
						Type = SavingThrowType.Fortitude,
						Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved s)
						{
							s.Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
							{
								b.m_Buff = ((StunnedBuff != null) ? StunnedBuff.ToReference<BlueprintBuffReference>() : null);
								b.DurationValue = Values.Duration.OneRound;
								b.Permanent = false;
							});
						})
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = SingularityVortexResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "SingularityVortexFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Overpowered Ability - Singularity Vortex");
				bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Swift action 40 ft gravitational vortex dealing 20d6 force damage and stunning enemies for 1 round (3/day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Vortex;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SingularityVortexAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SingularityVortexResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			BlueprintBuff CosmicAegisAllyBuff = TTCoreExtensions.CreateBuff("CosmicAegisAllyBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Aegis Sanctuary");
				bp.SetDescription(Main.IsekaiContext, "Protected by an otherworldly cosmic sanctuary: gain DR 20/- and Spell Resistance 35.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 35
					};
				});
			});
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("CosmicAegisFeature", Helpers.CreateString(Main.IsekaiContext, "CosmicAegisFeature.Name", "Epic Overpowered Ability - Cosmic Aegis"), Helpers.CreateString(Main.IsekaiContext, "CosmicAegisFeature.Desc", "Requires Level 21+. You radiate an impenetrable cosmic aegis in a 30-foot aura. Allies within this aura gain Damage Reduction 20/- and Spell Resistance 35."), Icon_Shield, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(30f);
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = CosmicAegisAllyBuff.ToReference<BlueprintBuffReference>();
						b.Permanent = true;
						b.DurationValue = Values.Duration.Zero;
						b.AsChild = true;
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff b)
					{
						b.m_Buff = CosmicAegisAllyBuff.ToReference<BlueprintBuffReference>();
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = ActionFlow.DoNothing();
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "TranscendentBABFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Superpower - Transcendent Mastery");
				bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your martial prowess transcends standard mortal limitations, granting a +10 untyped bonus to all attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 10;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "LimitlessVigorFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Superpower - Limitless Vigor");
				bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your life force expands to cosmic proportions, granting +200 maximum Hit Points and Fast Healing 20.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 200;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 20;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "PlanarTranscendenceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Superpower - Planar Transcendence");
				bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your physical vessel transcends planar physics, granting complete immunity to Acid, Cold, Electricity, Fire, and Sonic energy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				DamageEnergyType[] array = new DamageEnergyType[5]
				{
					DamageEnergyType.Acid,
					DamageEnergyType.Cold,
					DamageEnergyType.Electricity,
					DamageEnergyType.Fire,
					DamageEnergyType.Sonic
				};
				foreach (DamageEnergyType energy in array)
				{
					bp.AddComponent(delegate(AddEnergyImmunity c)
					{
						c.Type = energy;
					});
				}
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteSpellLatticeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Superpower - Infinite Spell Lattice");
				bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your magical conduits expand infinitely, granting +4 additional spell slots per day for all spell tiers from 1st through 9th circle.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Magic;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddSpellsPerDay c)
				{
					c.Amount = 4;
					c.Levels = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BaneOfDeitiesFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Bane of Deities");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to The Godhunter (or Level 21+). Your weapons are consecrated with divine-nullifying runes. All weapon attacks deal an additional +4d6 divine damage and gain a +4 sacred attack bonus against Outsiders and Dragons.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 14;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			BlueprintBuff PhalanxStanceBuff = TTCoreExtensions.CreateBuff("PhalanxStanceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Phalanx Stance Formation");
				bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +6 shield bonus to Armor Class, a +4 bonus to all saving throws, and benefit from Outflank and Seize the Moment teamwork feats.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 6;
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
				if (OutflankFeat != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { OutflankFeat.ToReference<BlueprintUnitFactReference>() };
					});
				}
				if (SeizeTheMomentFeat != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { SeizeTheMomentFeat.ToReference<BlueprintUnitFactReference>() };
					});
				}
			});
			BlueprintFeature blueprintFeature2 = TTCoreExtensions.CreateToggleAuraFeature("PhalanxStanceFeature", Helpers.CreateString(Main.IsekaiContext, "PhalanxStanceFeature.Name", "Prestige OP Ability - Phalanx Stance"), Helpers.CreateString(Main.IsekaiContext, "PhalanxStanceFeature.Desc", "Exclusive to The Warmaster (or Level 21+). Allies within 30 feet share your defensive bulwark: gaining +6 shield AC, +4 to all saves, and Outflank + Seize the Moment."), Icon_Shield, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(30f);
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = PhalanxStanceBuff.ToReference<BlueprintBuffReference>();
						b.Permanent = true;
						b.DurationValue = Values.Duration.Zero;
						b.AsChild = true;
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff b)
					{
						b.m_Buff = PhalanxStanceBuff.ToReference<BlueprintBuffReference>();
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = ActionFlow.DoNothing();
				});
			});
			blueprintFeature2.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature2);
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "MultiversalGlimpseFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Multiversal Glimpse");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to The Eternal Seeker (or Level 21+). You perceive all branching multiversal futures simultaneously. Roll twice on all Initiative checks and Saving Throws, taking the better result.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Time;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(ModifyD20 c)
				{
					c.Rule = RuleType.SavingThrow;
					c.RollsAmount = 1;
					c.TakeBest = true;
				});
				bp.AddComponent(delegate(ModifyD20 c)
				{
					c.Rule = RuleType.Initiative;
					c.RollsAmount = 1;
					c.TakeBest = true;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ArcaneOverchargeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Arcane Overcharge");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to The Archmage (or Level 21+). Your spells tap into pure transcendent leylines. Damaging spells deal an additional +2 damage per caster level and all spell damage dice are promoted to d10s.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Magic;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(PromoteSpellDices c)
				{
					c.MinDice = DiceType.D10;
					c.Bonus = 2;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowInfusionFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Shadow Infusion");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to The Shadow Sovereign (or Level 21+). You and your summoned shadow soldiers are saturated with abyssal planar shadow. All attacks deal an additional +3d6 negative energy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_EpicCrown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 10;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicDominionFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Cosmic Dominion");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to The Transcendent Sovereign (or Level 21+). Your presence bends planar authority. You gain a +4 untyped bonus to all spell save DCs, attack rolls, Armor Class, and saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_EpicCrown;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
			}));
		}
	}
}
