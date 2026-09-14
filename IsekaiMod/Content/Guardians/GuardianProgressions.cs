using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians
{
	internal static class GuardianProgressions
	{
		private static readonly Sprite Icon_Angel = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bdddaa78f8795024081f2d1eb8b4bd78"))?.m_Icon;

		private static readonly Sprite Icon_Demon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("6a8af3f208a0fa747a465b70b7043019"))?.m_Icon;

		private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		public static BlueprintProgression AngelProgression { get; private set; }

		public static BlueprintProgression ShinigamiProgression { get; private set; }

		public static BlueprintProgression DemonProgression { get; private set; }

		public static BlueprintProgression DevourerProgression { get; private set; }

		public static BlueprintProgression DragonProgression { get; private set; }

		public static void Add()
		{
			BlueprintCharacterClassReference guardianClassRef = GuardianCompanionClass.GetReference();
			BlueprintFeature AngelHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianAngelHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Celestial Resilience");
				bp.SetDescription(Main.IsekaiContext, "Gains resistance 5 to acid, cold, and electricity, and damage reduction 5/evil.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.BypassedByAlignment = true;
					c.Alignment = DamageAlignment.Evil;
					c.Value = 5;
				});
			});
			BlueprintFeature AngelSmite = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianAngelSmite", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Smite Evil");
				bp.SetDescription(Main.IsekaiContext, "Once per day, the Guardian Angel can call upon celestial judgment against an evil foe, adding its Charisma bonus to attack rolls and class level to damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>("3a6db57fce75b0244a6a5819528ddf26");
				if (blueprint != null)
				{
					BlueprintComponent[] componentsArray = blueprint.ComponentsArray;
					foreach (BlueprintComponent value in componentsArray)
					{
						bp.ComponentsArray = bp.ComponentsArray.AppendToArray(value);
					}
				}
			});
			BlueprintFeature AngelWings = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianAngelWings", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Angelic Wings");
				bp.SetDescription(Main.IsekaiContext, "Radiant feathered wings grant a +3 dodge bonus to AC against melee attacks and immunity to ground hazards.");
				// Immunities named in the description.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Ground;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Ground;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.DifficultTerrain;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
			});
			BlueprintFeature AngelHolyStrike = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianAngelHolyStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Holy Strike");
				bp.SetDescription(Main.IsekaiContext, "The angel's weapon and natural strikes gleam with sacred light, dealing an additional 1d6 holy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(1, DiceType.D6);
					c.Element = DamageEnergyType.Holy;
				});
			});
			// (a stray duplicate GuardianAngelAuraOfMenaceBuff was created here; CreateToggleAuraBuffFeature below builds the real one)
			BlueprintFeature AngelAuraOfMenace = TTCoreExtensions.CreateToggleAuraBuffFeature("GuardianAngelAuraOfMenace", "Enemies within 20 feet of the Guardian Angel suffer a -2 penalty on attack rolls and saving throws.", "This creature suffers a -2 penalty on attack rolls and saving throws from celestial majesty.", Icon_Angel, BlueprintAbilityAreaEffect.TargetType.Enemy, new Feet(20f), affectEnemies: true, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveFortitude;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveReflex;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -2;
				});
			});
			BlueprintFeature AngelFastHealing = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianAngelFastHealing", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Angelic Fast Healing");
				bp.SetDescription(Main.IsekaiContext, "The angel gains Fast Healing 5 and its damage reduction improves to 10/evil.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
					c.Bonus = 0;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.BypassedByAlignment = true;
					c.Alignment = DamageAlignment.Evil;
					c.Value = 10;
				});
			});
			BlueprintFeature AngelArchangel = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianAngelArchangel", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Archangel Ascension");
				bp.SetDescription(Main.IsekaiContext, "The angel achieves full archangel sovereignty, gaining immunity to cold, acid, and petrification, and permanent Holy Aura.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Cold;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Acid;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Petrified;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
			});
			AngelProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianAngelProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Guardian Angel Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Guardian Angel companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[7]
				{
					Helpers.CreateLevelEntry(1, AngelHeritage),
					Helpers.CreateLevelEntry(3, AngelSmite),
					Helpers.CreateLevelEntry(5, AngelWings),
					Helpers.CreateLevelEntry(8, AngelHolyStrike),
					Helpers.CreateLevelEntry(11, AngelAuraOfMenace),
					Helpers.CreateLevelEntry(17, AngelFastHealing),
					Helpers.CreateLevelEntry(20, AngelArchangel)
				};
			});
			BlueprintFeature ShinigamiHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "ShinigamiHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Reaper's Shroud");
				bp.SetDescription(Main.IsekaiContext, "Gains resistance 5 to cold and negative energy, and damage reduction 5/good.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.NegativeEnergy;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.BypassedByAlignment = true;
					c.Alignment = DamageAlignment.Good;
					c.Value = 5;
				});
			});
			BlueprintFeature ShinigamiGhostTouch = Helpers.CreateBlueprint(Main.IsekaiContext, "ShinigamiGhostTouch", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Ghost Touch Scythe");
				bp.SetDescription(Main.IsekaiContext, "The Shinigami's scythe strikes across the material and ethereal boundaries, bypassing incorporeal armor.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
				{
					c.AffectAnyPhysicalDamage = true;
					c.AddReality = true;
					c.Reality = DamageRealityType.Ghost;
				});
			});
			BlueprintFeature ShinigamiSoulSever = Helpers.CreateBlueprint(Main.IsekaiContext, "ShinigamiSoulSever", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Soul-Severing Strike");
				bp.SetDescription(Main.IsekaiContext, "Critical hits with the Shinigami's scythe inflict 1 negative level upon the struck foe.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
				{
					c.CheckWeaponType = false;
				});
			});
			BlueprintFeature ShinigamiEthereal = Helpers.CreateBlueprint(Main.IsekaiContext, "ShinigamiEthereal", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Ethereal Form");
				bp.SetDescription(Main.IsekaiContext, "Veiled between realities, the Shinigami gains 20% concealment against all attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
					c.Descriptor = ConcealmentDescriptor.Displacement;
				});
			});
			BlueprintFeature ShinigamiSovereignReaper = Helpers.CreateBlueprint(Main.IsekaiContext, "ShinigamiSovereignReaper", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Sovereign of the Dead");
				bp.SetDescription(Main.IsekaiContext, "The Shinigami reaches the pinnacle of reaper authority. Natural 20 critical hits instantly slay non-boss adversaries.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
			});
			ShinigamiProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "ShinigamiProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Shinigami Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Shinigami companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[5]
				{
					Helpers.CreateLevelEntry(1, ShinigamiHeritage),
					Helpers.CreateLevelEntry(5, ShinigamiGhostTouch),
					Helpers.CreateLevelEntry(11, ShinigamiSoulSever),
					Helpers.CreateLevelEntry(17, ShinigamiEthereal),
					Helpers.CreateLevelEntry(20, ShinigamiSovereignReaper)
				};
			});
			BlueprintFeature DemonHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "LoyalDemonHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Abyssal Resilience");
				bp.SetDescription(Main.IsekaiContext, "Gains resistance 5 to fire, acid, and electricity, and damage reduction 5/cold iron.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.ColdIron;
					c.Value = 5;
				});
			});
			BlueprintFeature DemonWings = Helpers.CreateBlueprint(Main.IsekaiContext, "LoyalDemonWings", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Bat Wings");
				bp.SetDescription(Main.IsekaiContext, "Leathery demon wings grant a +3 dodge bonus to AC against melee attacks and ground hazard immunity.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
			});
			BlueprintFeature DemonProfaneStrike = Helpers.CreateBlueprint(Main.IsekaiContext, "LoyalDemonProfaneStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Profane Strike");
				bp.SetDescription(Main.IsekaiContext, "Attacks deal an additional 1d6 unholy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(1, DiceType.D6);
					c.Element = DamageEnergyType.Unholy;
				});
			});
			BlueprintFeature DemonAllure = Helpers.CreateBlueprint(Main.IsekaiContext, "LoyalDemonAllure", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Bewitching Allure");
				bp.SetDescription(Main.IsekaiContext, "Enemies within 20 feet are unsettled by the demon's presence, taking a -2 penalty to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
			});
			BlueprintFeature DemonQueenConsort = Helpers.CreateBlueprint(Main.IsekaiContext, "LoyalDemonQueenConsort", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Queen's Devotion");
				bp.SetDescription(Main.IsekaiContext, "Wholly attuned to her partner, the Loyal Demon gains a +4 inherent bonus to all attributes and permanent True Seeing.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
			});
			DemonProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "LoyalDemonProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Loyal Demon Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Loyal Demon companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[5]
				{
					Helpers.CreateLevelEntry(1, DemonHeritage),
					Helpers.CreateLevelEntry(3, DemonWings),
					Helpers.CreateLevelEntry(8, DemonProfaneStrike),
					Helpers.CreateLevelEntry(14, DemonAllure),
					Helpers.CreateLevelEntry(20, DemonQueenConsort)
				};
			});
			BlueprintFeature DevourerHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "AstralDevourerHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Void Carapace");
				bp.SetDescription(Main.IsekaiContext, "Gains damage reduction 5/magic and spell resistance 11 + class level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.BypassedByMagic = true;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Class = new BlueprintCharacterClassReference[1] { guardianClassRef };
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 11;
				});
			});
			BlueprintFeature DevourerForce = Helpers.CreateBlueprint(Main.IsekaiContext, "AstralDevourerForce", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Ethereal Claws");
				bp.SetDescription(Main.IsekaiContext, "Claw attacks inflict an additional 1d6 force damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(1, DiceType.D6);
					c.Element = DamageEnergyType.Magic;
				});
			});
			BlueprintFeature DevourerSingularity = Helpers.CreateBlueprint(Main.IsekaiContext, "AstralDevourerSingularity", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Void Singularity");
				bp.SetDescription(Main.IsekaiContext, "The Astral Devourer reaches total planar consumption, gaining +10 spell resistance and permanent displacement.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
					c.Descriptor = ConcealmentDescriptor.Displacement;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
			});
			DevourerProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "AstralDevourerProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Astral Devourer Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Astral Devourer companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[3]
				{
					Helpers.CreateLevelEntry(1, DevourerHeritage),
					Helpers.CreateLevelEntry(8, DevourerForce),
					Helpers.CreateLevelEntry(20, DevourerSingularity)
				};
			});
			BlueprintFeature DragonHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "HavocDragonHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Havoc Drake Essence");
				bp.SetDescription(Main.IsekaiContext, "Tiny floating chaos dragon. Cannot be mounted, but flies over ground effects and gains +4 dodge AC against attacks of opportunity.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
			});
			BlueprintFeature DragonSlip = Helpers.CreateBlueprint(Main.IsekaiContext, "HavocDragonSlip", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Prankster's Slip");
				bp.SetDescription(Main.IsekaiContext, "Cannot be flanked and gains permanent 20% concealment.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
					c.Descriptor = ConcealmentDescriptor.Displacement;
				});
			});
			BlueprintFeature DragonChaosLord = Helpers.CreateBlueprint(Main.IsekaiContext, "HavocDragonChaosLord", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of Pandemonium");
				bp.SetDescription(Main.IsekaiContext, "Attaining absolute trickster freedom, the Havoc Dragon gains permanent Greater Invisibility and +6 luck bonus to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 6;
				});
			});
			DragonProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "HavocDragonProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Havoc Dragon Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Trickster Havoc Dragon companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[3]
				{
					Helpers.CreateLevelEntry(1, DragonHeritage),
					Helpers.CreateLevelEntry(7, DragonSlip),
					Helpers.CreateLevelEntry(20, DragonChaosLord)
				};
			});
		}
	}
}
