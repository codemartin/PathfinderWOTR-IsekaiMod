using Kingmaker.UnitLogic.Mechanics.Components;
using IsekaiMod.Utilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Content.Guardians;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class TwinSummonerLegacy
	{
		private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		private static BlueprintProgression prog;

		public static void Configure()
		{
			TwinAvatarCompanionUnit.Configure();
			BlueprintFeature twinPetFeature = TwinAvatarCompanionUnit.TwinAvatarFeature;
			BlueprintFeature TwinProtagonistPlotArmor = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinProtagonistPlotArmor", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twinned Destiny: Shared Plot Armor");
				bp.SetDescription(Main.IsekaiContext, "You and your Twin Avatar share an unbroken heroic thread of destiny, granting a +2 luck bonus to Armor Class and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			BlueprintFeature TwinTeamworkFlanking = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinTeamworkFlanking", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twinned Teamwork: Flank Synthesis");
				bp.SetDescription(Main.IsekaiContext, "Synchronized movement confuses enemy guards. You gain a +2 bonus on attack rolls and deal an additional 1d6 precision sneak attack damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = 1;
				});
			});
			BlueprintFeature TwinTranspositionAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinTranspositionAbility", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twinned Transposition: Step Across Space");
				bp.SetDescription(Main.IsekaiContext, "Rapid positional phase shifts grant a +4 insight bonus to Initiative and a +10 foot bonus to movement speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
			BlueprintFeature TwinSynchronizedStrike = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinSynchronizedStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twinned Synchronized Strike");
				bp.SetDescription(Main.IsekaiContext, "Striking in absolute unison, you gain a +4 morale bonus to attack and damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
			});
			BlueprintFeature TwinSpellEcho = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinSpellEcho", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twinned Arcane Echo");
				bp.SetDescription(Main.IsekaiContext, "Your spells reverberate through the twin's planar matrix, increasing your spell difficulty class by +2 and granting a +4 bonus on caster level checks to overcome spell resistance.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 4;
				});
			});
			BlueprintFeature TwinDualityAegis = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinDualityAegis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twinned Duality Aegis");
				bp.SetDescription(Main.IsekaiContext, "Reaching full soul resonance, you gain a +6 deflection bonus to AC, Fast Healing 10, and immunity to death effects and negative energy.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.NegativeEnergy;
				});
			});
			BlueprintFeature TwinEpicResonance = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEpicResonance", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Twinned Resonance");
				bp.SetDescription(Main.IsekaiContext, "At 25th level, strikes channel the twin's magical essence, dealing an additional 3d6 magic damage and granting a +4 enhancement bonus to attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 3, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			});
			BlueprintFeature TwinEpicSharedSoul = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEpicSharedSoul", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Shared Soul Shield");
				bp.SetDescription(Main.IsekaiContext, "At 30th level, the shared soul protects both beings against hostile magic, granting a +8 deflection bonus to AC and a +6 resistance bonus on all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveFortitude;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveReflex;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveWill;
					c.Value = 6;
				});
			});
			BlueprintFeature TwinEpicDimensionalUnity = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEpicDimensionalUnity", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Dimensional Unity");
				bp.SetDescription(Main.IsekaiContext, "At 35th level, you and your twin exist simultaneously across planes, gaining Fast Healing 20, Damage Reduction 15/-, and Spell Resistance 35.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 20;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 15;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = 35;
				});
			});
			BlueprintFeature TwinEpicDualOverlordApex = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEpicDualOverlordApex", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Sovereign Dual Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, you and your Twin Avatar become supreme twin gods of the multiverse. You gain a +10 inherent bonus to all ability scores, a +10 dodge bonus to AC, and 2 extra attacks when making a full attack.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
				});
			});
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinSummonerLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Summoner Legacy - Twinned Soul");
				bp.SetDescription(Main.IsekaiContext, "Instead of summoning beasts or planar entities, you have manifested a perfect mirror reflection of your soul into physical reality. Your Twin Avatar mirrors your destiny, coordinates tactical strikes, shares damage and wards, and fights as an equal god by your side.");
				bp.GiveFeaturesForPreviousLevels = false;
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = IsekaiProtagonistClass.GetReference(),
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[10]
				{
					Helpers.CreateLevelEntry(1, twinPetFeature, TwinProtagonistPlotArmor),
					Helpers.CreateLevelEntry(4, TwinTeamworkFlanking),
					Helpers.CreateLevelEntry(8, TwinTranspositionAbility),
					Helpers.CreateLevelEntry(12, TwinSynchronizedStrike),
					Helpers.CreateLevelEntry(16, TwinSpellEcho),
					Helpers.CreateLevelEntry(20, TwinDualityAegis),
					Helpers.CreateLevelEntry(25, TwinEpicResonance),
					Helpers.CreateLevelEntry(30, TwinEpicSharedSoul),
					Helpers.CreateLevelEntry(35, TwinEpicDimensionalUnity),
					Helpers.CreateLevelEntry(40, TwinEpicDualOverlordApex)
				};
				bp.UIGroups = new UIGroup[1] { Helpers.CreateUIGroup(TwinProtagonistPlotArmor, TwinTeamworkFlanking, TwinTranspositionAbility, TwinSynchronizedStrike, TwinSpellEcho, TwinDualityAegis, TwinEpicResonance, TwinEpicSharedSoul, TwinEpicDimensionalUnity, TwinEpicDualOverlordApex) };
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
			ShadowMonarchLegacySelection.Register(prog);
			DevourerLegacySelection.Register(prog);
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
	}
}
