using System;
using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.PhantomBlade;
using IsekaiMod.ModLogic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
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
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;

internal class TwinSummonerLegacy
{
	private static BlueprintProgression prog;

	private static BlueprintFeatureSelection evolutionSelection;

	private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

	private static readonly BlueprintItemWeapon Claw1d6 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("65eb73689b94d894080d33a768cdf645");

	private static readonly BlueprintItemWeapon Bite2d6 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("2abc1dc6172759c42971bd04b8c115cb");

	private static readonly BlueprintFeature Airborne = BlueprintTools.GetBlueprint<BlueprintFeature>("70cffb448c132fa409e49156d013b175");

	internal static int[] EvolutionLevels => TwinAvatarShape.EvolutionLevels;

	internal static int EvolutionRanks => EvolutionLevels.Length;

	public static void Configure()
	{
		ModContextTTTBase IsekaiContext = Main.IsekaiContext;
		BlueprintFeature TwinAvatarCompanion = BlueprintTools.GetModBlueprint<BlueprintFeature>(IsekaiContext, "TwinAvatarCompanion");
		BlueprintUnit modBlueprint = BlueprintTools.GetModBlueprint<BlueprintUnit>(IsekaiContext, "TwinAvatarCompanionUnit");
		if (TwinAvatarCompanion != null)
		{
			evolutionSelection = BuildEvolutionSelection(modBlueprint?.ToReference<BlueprintUnitReference>());
			BlueprintFeature blueprintFeature = BuildResummon();
			List<LevelEntry> entries = new List<LevelEntry>();
			int[] evolutionLevels = EvolutionLevels;
			foreach (int num in evolutionLevels)
			{
				entries.Add((num == EvolutionLevels[0] && blueprintFeature != null) ? Helpers.CreateLevelEntry(num, evolutionSelection, blueprintFeature) : Helpers.CreateLevelEntry(num, evolutionSelection));
			}
			prog = Helpers.CreateBlueprint(IsekaiContext, "TwinSummonerLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(IsekaiContext, "Twin Summoner Legacy");
				bp.SetDescription(IsekaiContext, "Your legacy is not a life you might have lived but a second body you actually have. Instead of another class's training you spend the slot on the Twin Avatar itself, reshaping the double condensed out of your mana until it fights like nothing else in the party.\nRequires the Twin Avatar from your pet selection. You gain an evolution for the twin at 1st, 5th, 10th, 15th and 20th level, and the ability to recondense it in the middle of a fight.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)TwinAvatarCompanion)?.m_Icon;
				bp.IsClassFeature = true;
				bp.GiveFeaturesForPreviousLevels = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = IsekaiProtagonistClass.GetReference(),
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = entries.ToArray();
				bp.UIGroups = new UIGroup[0];
				bp.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[0];
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
			PhantomBladeLegacySelection.Register(prog);
			prog.AddComponent(delegate(PrerequisiteFeature c)
			{
				c.m_Feature = TwinAvatarCompanion.ToReference<BlueprintFeatureReference>();
				c.Group = Prerequisite.GroupType.All;
			});
			LegacySelectionOrderPatch.RegisterLeading(BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(IsekaiContext, "IsekaiPetSelection"));
		}
	}

	public static void PatchProgression()
	{
	}

	public static BlueprintProgression Get()
	{
		if (prog != null)
		{
			return prog;
		}
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "TwinSummonerLegacy");
	}

	public static BlueprintFeatureSelection GetEvolutionSelection()
	{
		if (evolutionSelection != null)
		{
			return evolutionSelection;
		}
		return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "TwinEvolutionSelection");
	}

	private static BlueprintFeatureSelection BuildEvolutionSelection(BlueprintUnitReference petUnitRef)
	{
		List<BlueprintFeature> picks = new List<BlueprintFeature>();
		Evolution("TwinEvolutionClaws", "Evolution: Rending Claws", "The twin's hands sharpen into claws. It gains two primary claw attacks dealing 1d6 damage each.", delegate(BlueprintFeature bp)
		{
			if (Claw1d6 != null)
			{
				bp.AddComponent(delegate(AddAdditionalLimb c)
				{
					c.m_Weapon = Claw1d6.ToReference<BlueprintItemWeaponReference>();
				});
				bp.AddComponent(delegate(AddAdditionalLimb c)
				{
					c.m_Weapon = Claw1d6.ToReference<BlueprintItemWeaponReference>();
				});
			}
		});
		Evolution("TwinEvolutionMaw", "Evolution: Gaping Maw", "Your double's jaw unhinges into something that was never a mouth. It gains a primary bite attack dealing 2d6 damage.", delegate(BlueprintFeature bp)
		{
			if (Bite2d6 != null)
			{
				bp.AddComponent(delegate(AddAdditionalLimb c)
				{
					c.m_Weapon = Bite2d6.ToReference<BlueprintItemWeaponReference>();
				});
			}
		});
		Evolution("TwinEvolutionExtraArms", "Evolution: Extra Limbs", "A second pair of arms unfolds from the twin's back. It makes one additional attack each time it attacks with its weapons.", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddExtraAttack c)
			{
				c.Number = 1;
			});
		});
		BlueprintBuff LargeBuff = TTCoreExtensions.CreateBuff("TwinEvolutionLargeBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Towering Frame");
			bp.SetDescription(Main.IsekaiContext, "The Twin Avatar has grown to Large size.");
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
			bp.AddComponent(delegate(ChangeUnitSize c)
			{
				c.m_Type = ChangeUnitSize.ChangeType.Value;
				c.Size = Size.Large;
			});
			bp.AddComponent(delegate(AddGenericStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Size;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddGenericStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Size;
				c.Stat = StatType.Dexterity;
				c.Value = -2;
			});
		});
		Evolution("TwinEvolutionLarge", "Evolution: Towering Frame", "Your mana pours in until the double stands head and shoulders over you. The twin becomes Large: +4 size bonus to Strength, -2 size penalty to Dexterity, and its natural armor bonus rises to +5.", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 5;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { LargeBuff.ToReference<BlueprintUnitFactReference>() };
			});
		});
		ExtraWings.AddPitImmunity(Evolution("TwinEvolutionFlight", "Evolution: Soaring Twin", "Wings of condensed mana carry the twin off the ground. It flies over ground hazards and gains a +3 dodge bonus to AC against melee attacks.", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(ACBonusAgainstAttacks c)
			{
				c.AgainstMeleeOnly = true;
				c.ArmorClassBonus = 3;
				c.Value = 0;
				c.Descriptor = ModifierDescriptor.Dodge;
				c.NotArmorCategory = new ArmorProficiencyGroup[0];
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.DifficultTerrain;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Ground;
			});
			if (Airborne != null)
			{
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { Airborne.ToReference<BlueprintUnitFactReference>() };
				});
			}
		}));
		EnergyEvolution("TwinEvolutionFire", "Evolution: Burning Strikes", "fire", DamageEnergyType.Fire);
		EnergyEvolution("TwinEvolutionCold", "Evolution: Freezing Strikes", "cold", DamageEnergyType.Cold);
		EnergyEvolution("TwinEvolutionShock", "Evolution: Shocking Strikes", "electricity", DamageEnergyType.Electricity);
		EnergyEvolution("TwinEvolutionAcid", "Evolution: Corroding Strikes", "acid", DamageEnergyType.Acid);
		Evolution("TwinEvolutionMight", "Evolution: Mirrored Might", "The double is built heavier than you are. It gains a +4 inherent bonus to Strength and Constitution.", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
		});
		Evolution("TwinEvolutionGrace", "Evolution: Mirrored Grace", "The double is built quicker than you are. It gains a +4 inherent bonus to Dexterity and Wisdom.", delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Dexterity;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Wisdom;
				c.Value = 4;
			});
		});
		Evolution("TwinEvolutionWards", "Evolution: Warded Form", "Your mana closes over the twin like a shell. It gains resistance 10 to fire, cold, electricity, acid and sonic damage.", delegate(BlueprintFeature bp)
		{
			DamageEnergyType[] array = new DamageEnergyType[5]
			{
				DamageEnergyType.Fire,
				DamageEnergyType.Cold,
				DamageEnergyType.Electricity,
				DamageEnergyType.Acid,
				DamageEnergyType.Sonic
			};
			foreach (DamageEnergyType damageEnergyType in array)
			{
				DamageEnergyType captured = damageEnergyType;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = captured;
					c.Value = 10;
				});
			}
		});
		return Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEvolutionSelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Twin Avatar Evolution");
			bp.SetDescription(Main.IsekaiContext, "Reshape your Twin Avatar. Each evolution is permanent and applies to the twin, not to you.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.Ranks = EvolutionRanks;
			bp.Group = FeatureGroup.AnimalCompanion;
			bp.IgnorePrerequisites = false;
			bp.m_AllFeatures = new BlueprintFeatureReference[0];
			bp.m_Features = new BlueprintFeatureReference[0];
			foreach (BlueprintFeature item2 in picks)
			{
				bp.m_AllFeatures = bp.m_AllFeatures.AppendToArray(item2.ToReference<BlueprintFeatureReference>());
				bp.m_Features = bp.m_Features.AppendToArray(item2.ToReference<BlueprintFeatureReference>());
			}
		});
		void EnergyEvolution(string name, string displayName, string energyName, DamageEnergyType energy)
		{
			Evolution(name, displayName, "The mana holding the twin together burns. Its attacks deal an additional 1d6 " + energyName + " damage.", delegate(BlueprintFeature bp)
			{
				bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
				{
					c.DamageType = new DamageTypeDescription
					{
						Type = DamageType.Energy,
						Energy = energy
					};
					c.Value = new ContextDiceValue
					{
						DiceType = DiceType.D6,
						DiceCountValue = 1,
						BonusValue = 0
					};
				});
			});
		}
		BlueprintFeature Evolution(string name, string displayName, string description, Action<BlueprintFeature> effect)
		{
			BlueprintFeature effectFeature = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Effect", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				effect(bp);
			});
			BlueprintFeature item = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFactsToPetSelfHealing c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { effectFeature.ToReference<BlueprintUnitFactReference>() };
					c.m_PetType = PetType.AnimalCompanion;
					c.m_PetBlueprint = petUnitRef;
				});
			});
			picks.Add(item);
			return effectFeature;
		}
	}

	private static BlueprintFeature BuildResummon()
	{
		BlueprintAbilityResource resource = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinResummonResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		BlueprintAbility ability = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinResummonAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Recondense Twin Avatar");
			bp.SetDescription(Main.IsekaiContext, "You pull the scattered mana of your double back together. Target your Twin Avatar: it is healed for 10 hit points per Isekai Protagonist level and freed of paralysis, stun, sleep and daze. Usable three times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.Type = AbilityType.SpellLike;
			bp.Range = AbilityRange.Medium;
			bp.CanTargetFriends = true;
			bp.CanTargetSelf = false;
			bp.CanTargetEnemies = false;
			bp.CanTargetPoint = false;
			bp.AddComponent<AbilityTargetIsAnimalCompanion>();
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.LocalizedDuration = StaticReferences.Strings.Null;
			bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionHealTarget
				{
					Value = new ContextDiceValue
					{
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
					}
				}, new ContextActionRemoveBuffsByDescriptor
				{
					SpellDescriptor = (SpellDescriptor.Daze | SpellDescriptor.Stun | SpellDescriptor.Paralysis | SpellDescriptor.Sleep)
				});
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
				c.m_Class = IsekaiAscendantClass.ClassLevelScalingClasses();
				c.m_Progression = ContextRankProgression.MultiplyByModifier;
				c.m_StepLevel = 10;
				c.m_Min = 1;
				c.m_UseMin = true;
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
		});
		return Helpers.CreateBlueprint(Main.IsekaiContext, "TwinResummonFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Recondense Twin Avatar");
			bp.SetDescription(Main.IsekaiContext, "You can pull your double back together in the middle of a fight, healing it for 10 hit points per Isekai Protagonist level and clearing paralysis, stun, sleep and daze. Usable three times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = resource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { ability.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
