using System;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
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
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal static class Epic21Abilities
{
	private static readonly Sprite Icon_EpicCrown = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e6e1a24ce33454342b7053889391642c"))?.m_Icon ?? BlueprintTools.GetBlueprint<BlueprintItem>("d5f207009a651094f83b6057043e1431")?.m_Icon;

	private static readonly Sprite Icon_Time = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("5d28f75db5d3cc141ba5783a1a139f66"))?.m_Icon;

	private static readonly Sprite Icon_Vortex = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ab167fd8203c1314bac6568932f1752f"))?.m_Icon;

	private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_EpicCrown;

	private static readonly Sprite Icon_Sword = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("f5f500d6a2a39fc4181af32ad79af488"))?.m_Icon ?? Icon_EpicCrown;

	private static readonly Sprite Icon_Magic = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_EpicCrown;

	private static readonly BlueprintBuff StunnedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4397f91671007c5488e9787f5c0e6541");

	private static readonly BlueprintFeature OutflankFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11");

	private static readonly BlueprintFeature SeizeTheMomentFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760");

	private static readonly DeferredOffers s_EpicOffers = new DeferredOffers();

	internal static bool ShouldRegisterEpicAbilities()
	{
		return Main.IsekaiContext?.AddedContent?.EnableLevelUncapping ?? true;
	}

	internal static void OfferEpicContent(Action addToSelections)
	{
		s_EpicOffers.Offer(addToSelections, ShouldRegisterEpicAbilities());
	}

	internal static int OfferPendingEpicContent()
	{
		int num = s_EpicOffers.Release(ShouldRegisterEpicAbilities(), delegate(Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error offering level 21+ content: " + ex);
		});
		if (num > 0)
		{
			ExtraSpecialPowerSelection.Resync();
			Main.IsekaiContext.Logger.Log($"Level uncapping switched on: {num} level 21+ entries added to the Isekai pickers.");
		}
		return num;
	}

	private static void RegisterEpicFeature(Action<BlueprintFeature> addToSelection, BlueprintFeature feature)
	{
		if (feature != null)
		{
			OfferEpicContent(delegate
			{
				addToSelection(feature);
			});
		}
	}

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
				c.ContextBonus = new ContextValue();
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
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "TimelineDivergenceFeature", delegate(BlueprintFeature bp)
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
			});
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, feature);
		BlueprintFeature feature2 = Helpers.CreateBlueprint(Main.IsekaiContext, "AkashicOmniscienceFeature", delegate(BlueprintFeature bp)
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
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.SpellsOnly = true;
			});
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, feature2);
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
		BlueprintUnitProperty SingularityVortexDCLevelProperty = Values.CreateHalfLevelProperty("SingularityVortexDCLevelProperty", protagonistLevel: false, 5);
		BlueprintAbility SingularityVortexAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SingularityVortexAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Overpowered Ability - Singularity Vortex");
			bp.SetDescription(Main.IsekaiContext, "Manifest a localized black hole that crushes all enemies within 40 feet for 1d6 force damage per character level. A Fortitude save (DC = 15 + 1/2 Character Level + Cha) halves the damage and avoids being Stunned for 1 round. Usable 3 times per day as a swift action.");
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
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.DamageDice;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
			});
			bp.AddComponent(delegate(ContextCalculateAbilityParams c)
			{
				c.UseKineticistMainStat = false;
				c.StatType = StatType.Charisma;
				c.ReplaceSpellLevel = true;
				c.SpellLevel = Values.CreateContextCasterCustomPropertyValue(SingularityVortexDCLevelProperty);
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionSavingThrow save)
				{
					save.Type = SavingThrowType.Fortitude;
					save.Actions = Helpers.CreateActionList(new ContextActionDealDamage
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
							DiceCountValue = Values.CreateContextRankValue(AbilityRankType.DamageDice),
							BonusValue = 0
						},
						HalfIfSaved = true,
						IsAoE = true
					}, new ContextActionConditionalSaved
					{
						Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
						{
							b.m_Buff = ((StunnedBuff != null) ? StunnedBuff.ToReference<BlueprintBuffReference>() : null);
							b.DurationValue = Values.Duration.OneRound;
							b.Permanent = false;
						})
					});
				});
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = SingularityVortexResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
		});
		BlueprintFeature feature3 = Helpers.CreateBlueprint(Main.IsekaiContext, "SingularityVortexFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Overpowered Ability - Singularity Vortex");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Swift action 40 ft gravitational vortex dealing 1d6 force damage per character level and stunning enemies for 1 round (3/day).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Vortex;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { SingularityVortexAbility.ToReference<BlueprintUnitFactReference>() };
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = SingularityVortexResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, feature3);
		BlueprintBuff CosmicAegisAllyBuff = TTCoreExtensions.CreateBuff("CosmicAegisAllyBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Aegis Sanctuary");
			bp.SetDescription(Main.IsekaiContext, "Protected by an otherworldly cosmic sanctuary: gain damage reduction equal to half the character level of the Isekai who radiates the aura and spell resistance equal to 12 + that level. The damage reduction does not stack with other damage reduction.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.Div2;
			});
			bp.AddComponent(delegate(AddSpellResistance c)
			{
				c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.StatBonus;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.BonusValue;
				c.m_StepLevel = 12;
			});
		});
		BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("CosmicAegisFeature", Helpers.CreateString(Main.IsekaiContext, "CosmicAegisFeature.Name", "Epic Overpowered Ability - Cosmic Aegis"), Helpers.CreateString(Main.IsekaiContext, "CosmicAegisFeature.Desc", "Requires Level 21+. You radiate an impenetrable cosmic aegis in a 30-foot aura. Allies within this aura gain damage reduction equal to half your character level and spell resistance equal to 12 + your character level (the aura owner's level, not the ally's). The damage reduction does not stack with other damage reduction, and the spell resistance applies to friendly magic as well."), Icon_Shield, delegate(BlueprintAbilityAreaEffect bp)
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
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, blueprintFeature);
		BlueprintFeature feature4 = Helpers.CreateBlueprint(Main.IsekaiContext, "TranscendentBABFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Superpower - Transcendent Mastery");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your martial prowess transcends standard mortal limitations, granting an untyped bonus to all attack rolls equal to a quarter of your character level (+5 at level 21, rising to +10 at level 40).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.DivStep;
				c.m_StepLevel = 4;
			});
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(SpecialPowerSelection.AddToSelection, feature4);
		BlueprintFeature feature5 = Helpers.CreateBlueprint(Main.IsekaiContext, "LimitlessVigorFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Superpower - Limitless Vigor");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your life force expands to cosmic proportions, granting maximum Hit Points equal to 10 times your character level (+210 at level 21, rising to +400 at level 40) and Fast Healing equal to your character level.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.MultiplyByModifier;
				c.m_StepLevel = 10;
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
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(SpecialPowerSelection.AddToSelection, feature5);
		BlueprintFeature feature6 = Helpers.CreateBlueprint(Main.IsekaiContext, "PlanarTranscendenceFeature", delegate(BlueprintFeature bp)
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
		});
		RegisterEpicFeature(SpecialPowerSelection.AddToSelection, feature6);
		BlueprintFeature feature7 = Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteSpellLatticeFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Superpower - Infinite Spell Lattice");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your magical conduits expand infinitely, granting +2 additional spell slots per day for all spell tiers from 1st through 9th circle, in every spellbook you have.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Magic;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddSpellsPerDay c)
			{
				c.Amount = 2;
				c.Levels = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			});
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(SpecialPowerSelection.AddToSelection, feature7);
		BlueprintFeature feature8 = Helpers.CreateBlueprint(Main.IsekaiContext, "BaneOfDeitiesFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Bane of Deities");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your weapons are consecrated with divine-nullifying runes. Weapon attacks deal an additional 1d6 holy damage per six character levels, and you gain a +4 sacred bonus to attack rolls. The attack bonus is sacred, so it does not stack with other sacred bonuses to attack.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Holy
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = Values.CreateContextRankValue(AbilityRankType.DamageDice),
					BonusValue = 0
				};
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.DamageDice;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.DivStep;
				c.m_StepLevel = 6;
			});
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, feature8);
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
		BlueprintFeature blueprintFeature2 = TTCoreExtensions.CreateToggleAuraFeature("PhalanxStanceFeature", Helpers.CreateString(Main.IsekaiContext, "PhalanxStanceFeature.Name", "Prestige OP Ability - Phalanx Stance"), Helpers.CreateString(Main.IsekaiContext, "PhalanxStanceFeature.Desc", "Requires Level 21+. Allies within 30 feet share your defensive bulwark: gaining +6 shield AC, +4 to all saves, and Outflank + Seize the Moment."), Icon_Shield, delegate(BlueprintAbilityAreaEffect bp)
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
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, blueprintFeature2);
		BlueprintFeature feature9 = Helpers.CreateBlueprint(Main.IsekaiContext, "MultiversalGlimpseFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Multiversal Glimpse");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. You perceive all branching multiversal futures simultaneously. Roll twice on all Initiative checks and Saving Throws, taking the better result.");
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
		});
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, feature9);
		BlueprintFeature ArcaneOverchargeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ArcaneOverchargeFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Arcane Overcharge");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your spells tap into pure transcendent leylines. Spell damage dice smaller than a d10 are promoted to d10s, and spell damage dice that are already d10s or larger deal an additional +2 damage per die. This supersedes Magical Amplification and cannot be combined with it.");
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
		});
		BlueprintFeature magicalAmplification = MagicalAmplification.MagicalAmplificationFeature;
		if (magicalAmplification != null)
		{
			ArcaneOverchargeFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = magicalAmplification.ToReference<BlueprintFeatureReference>();
				c.Group = Prerequisite.GroupType.All;
			});
			magicalAmplification.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = ArcaneOverchargeFeature.ToReference<BlueprintFeatureReference>();
				c.Group = Prerequisite.GroupType.All;
			});
		}
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, ArcaneOverchargeFeature);
		BlueprintFeature feature10 = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowInfusionFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Shadow Infusion");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. You are saturated with abyssal planar shadow. Your weapon attacks deal an additional 1d6 negative energy damage per seven character levels.");
			((BlueprintUnitFact)bp).m_Icon = Icon_EpicCrown;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.NegativeEnergy
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = Values.CreateContextRankValue(AbilityRankType.DamageDice),
					BonusValue = 0
				};
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.DamageDice;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.DivStep;
				c.m_StepLevel = 7;
			});
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, feature10);
		BlueprintFeature feature11 = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicDominionFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Prestige OP Ability - Cosmic Dominion");
			bp.SetDescription(Main.IsekaiContext, "Requires Level 21+. Your presence bends planar authority. You gain a +4 untyped bonus to all spell save DCs, attack rolls, Armor Class, and saving throws.");
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
				c.SpellsOnly = true;
			});
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
		});
		RegisterEpicFeature(OverpoweredAbilitySelection.AddToSelection, feature11);
	}
}
