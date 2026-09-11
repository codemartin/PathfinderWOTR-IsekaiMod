using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians
{
	internal static class SubclassGuardianProgressions
	{
		private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		private static readonly Sprite Icon_Angel = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bdddaa78f8795024081f2d1eb8b4bd78"))?.m_Icon;

		private static readonly Sprite Icon_Demon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("6a8af3f208a0fa747a465b70b7043019"))?.m_Icon;

		public static BlueprintProgression TempestStarWolfProgression { get; private set; }

		public static BlueprintProgression ShadowMarshallProgression { get; private set; }

		public static BlueprintProgression OverlordGuardianProgression { get; private set; }

		public static BlueprintProgression DivineHeraldProgression { get; private set; }

		public static BlueprintProgression ChronoSpriteProgression { get; private set; }

		public static BlueprintProgression EnigmaticCoConspiratorProgression { get; private set; }

		public static BlueprintProgression ManifestedMartialSpiritProgression { get; private set; }

		public static void Add()
		{
			BlueprintCharacterClassReference guardianClassRef = GuardianCompanionClass.GetReference();
			BlueprintFeature TempestStarWolfHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tempest Fang & Lightning Strike");
				bp.SetDescription(Main.IsekaiContext, "The Tempest Star Wolf's natural bite attacks deal an extra 1d6 electricity damage and gain the Trip combat maneuver upon landing a successful hit.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(1, DiceType.D6);
					c.Element = DamageEnergyType.Electricity;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
			BlueprintFeature TempestStarWolfShadowLightning = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfShadowLightning", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Lightning Burst");
				bp.SetDescription(Main.IsekaiContext, "The Star Wolf cloaks itself in crackling shadows, gaining +4 dodge bonus to AC and dealing an additional 1d6 negative energy damage on all attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(1, DiceType.D6);
					c.Element = DamageEnergyType.Unholy;
				});
			});
			BlueprintBuff TempestStarWolfShadowPackBuff = TTCoreExtensions.CreateBuff("TempestStarWolfShadowPackBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Pack Resonance");
				bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +4 morale bonus on attack and damage rolls against flanked targets.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
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
			BlueprintFeature TempestStarWolfShadowPack = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfShadowPack", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Pack Alpha");
				bp.SetDescription(Main.IsekaiContext, "Emitting an alpha howl, the Star Wolf coordinates strikes with its master. Allies within 30 feet gain +4 morale bonus on attack and damage rolls against flanked targets.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = TempestStarWolfShadowPackBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature TempestStarWolfApexStorm = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfApexStorm", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Apex Storm Sovereign");
				bp.SetDescription(Main.IsekaiContext, "The wolf reaches apex storm maturity, gaining permanent Freedom of Movement, +6 dodge AC, and its lightning strikes deal an additional 2d6 electricity and 2d6 unholy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(2, DiceType.D6);
					c.Element = DamageEnergyType.Electricity;
				});
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(2, DiceType.D6);
					c.Element = DamageEnergyType.Unholy;
				});
			});
			TempestStarWolfProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Tempest Star Wolf Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Slime's Tempest Star Wolf companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[4]
				{
					Helpers.CreateLevelEntry(1, TempestStarWolfHeritage),
					Helpers.CreateLevelEntry(7, TempestStarWolfShadowLightning),
					Helpers.CreateLevelEntry(13, TempestStarWolfShadowPack),
					Helpers.CreateLevelEntry(20, TempestStarWolfApexStorm)
				};
			});
			BlueprintFeature ShadowMarshallHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Knight Armor & Blade");
				bp.SetDescription(Main.IsekaiContext, "Igris wields a greatsword with deadly grace, clad in impenetrable shadow plate. Gains Damage Reduction 5/- and immunity to mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Shaken;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Frightened;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Confusion;
				});
			});
			BlueprintFeature ShadowMarshallDominion = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallDominion", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Sovereign Strike");
				bp.SetDescription(Main.IsekaiContext, "Igris's greatsword sweeps cleave through multiple foes, dealing an additional 2d6 negative energy damage on all attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(2, DiceType.D6);
					c.Element = DamageEnergyType.Unholy;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
			});
			BlueprintBuff ShadowMarshallCommanderAuraBuff = TTCoreExtensions.CreateBuff("ShadowMarshallCommanderAuraBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Marshall's Discipline");
				bp.SetDescription(Main.IsekaiContext, "Allies and summoned shadow soldiers within 30 feet gain a +4 enhancement bonus on attack rolls and a +2 bonus on all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			BlueprintFeature ShadowMarshallCommanderAura = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallCommanderAura", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Vanguard Commander");
				bp.SetDescription(Main.IsekaiContext, "As the vanguard commander of the shadow army, Igris inspires all allies within 30 feet with +4 attack bonus and +2 to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = ShadowMarshallCommanderAuraBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature ShadowMarshallAbsoluteLoyalty = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallAbsoluteLoyalty", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Apex Shadow Marshall: Undying Fealty");
				bp.SetDescription(Main.IsekaiContext, "Igris achieves apex shadow ascension. Gains +6 natural armor, +4 Strength, and Fast Healing 10.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
				});
			});
			ShadowMarshallProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Marshall Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Shadow Monarch's loyal Marshall Igris.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[4]
				{
					Helpers.CreateLevelEntry(1, ShadowMarshallHeritage),
					Helpers.CreateLevelEntry(7, ShadowMarshallDominion),
					Helpers.CreateLevelEntry(13, ShadowMarshallCommanderAura),
					Helpers.CreateLevelEntry(20, ShadowMarshallAbsoluteLoyalty)
				};
			});
			BlueprintFeature OverlordGuardianHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dark Valkyrie Demonic Aegis");
				bp.SetDescription(Main.IsekaiContext, "Clad in raven-feathered armor, the Dark Valkyrie gains a +3 dodge bonus to AC against melee attacks, DR 5/good, and resistance 10 to cold and fire.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.BypassedByAlignment = true;
					c.Alignment = DamageAlignment.Good;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 10;
				});
			});
			BlueprintFeature OverlordGuardianUnholySpear = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianUnholySpear", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Unholy Black Spear");
				bp.SetDescription(Main.IsekaiContext, "The Dark Valkyrie's spear pierces through divine wards, dealing an additional 2d6 unholy damage and bypassing DR/good and DR/silver.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(2, DiceType.D6);
					c.Element = DamageEnergyType.Unholy;
				});
			});
			BlueprintBuff OverlordGuardianDespairAuraBuff = TTCoreExtensions.CreateBuff("OverlordGuardianDespairAuraBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Dark Valkyrie's Dreadful Presence");
				bp.SetDescription(Main.IsekaiContext, "Enemies within 30 feet suffer a -2 penalty on attack rolls and -2 on saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
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
			BlueprintFeature OverlordGuardianFallenWings = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianFallenWings", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Wings of the Fallen Overseer");
				bp.SetDescription(Main.IsekaiContext, "The Dark Valkyrie radiates an aura of dreadful despair, penalizing enemies within 30 feet by -2 on attacks and saving throws, while gaining Spell Resistance 11 + Level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = OverlordGuardianDespairAuraBuff.ToReference<BlueprintBuffReference>();
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 11;
				});
			});
			BlueprintFeature OverlordGuardianSacrifice = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianSacrifice", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Supreme Tomb Guardian Overseer");
				bp.SetDescription(Main.IsekaiContext, "Reaching the apex of devotion to the Overlord, the Dark Valkyrie gains +6 deflection bonus to AC, +4 to all saving throws, and immunity to death effects and negative levels.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.NegativeEnergy;
				});
			});
			OverlordGuardianProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Overlord Guardian Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Overlord's Dark Valkyrie companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[4]
				{
					Helpers.CreateLevelEntry(1, OverlordGuardianHeritage),
					Helpers.CreateLevelEntry(7, OverlordGuardianUnholySpear),
					Helpers.CreateLevelEntry(13, OverlordGuardianFallenWings),
					Helpers.CreateLevelEntry(20, OverlordGuardianSacrifice)
				};
			});
			BlueprintFeature DivineHeraldHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Celestial Herald Aura");
				bp.SetDescription(Main.IsekaiContext, "The First Apostle shines with golden radiance, gaining +4 sacred bonus to AC and DR 5/evil.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.BypassedByAlignment = true;
					c.Alignment = DamageAlignment.Evil;
					c.Value = 5;
				});
			});
			BlueprintBuff DivineHeraldSanctifiedAuraBuff = TTCoreExtensions.CreateBuff("DivineHeraldSanctifiedAuraBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Sanctified Radiance");
				bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +3 sacred bonus on attack rolls and saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 3;
				});
			});
			BlueprintFeature DivineHeraldSanctifiedAura = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldSanctifiedAura", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Sanctified Radiance Aura");
				bp.SetDescription(Main.IsekaiContext, "The First Apostle radiates sacred light, granting allies within 30 feet +3 sacred bonus to attack rolls and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = DivineHeraldSanctifiedAuraBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature DivineHeraldSolarSpear = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldSolarSpear", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solar Radiance Strike");
				bp.SetDescription(Main.IsekaiContext, "Strikes burst with blinding solar brilliance, dealing an additional 2d6 holy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(2, DiceType.D6);
					c.Element = DamageEnergyType.Holy;
				});
			});
			BlueprintFeature DivineHeraldApotheosis = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldApotheosis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Seraph of Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "Ascending as the ultimate apostle of the God Emperor, the herald gains permanent True Seeing, +6 sacred bonus to all ability scores, and immunity to energy drain.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Strength;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Dexterity;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Constitution;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Intelligence;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Wisdom;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Holy;
				});
			});
			DivineHeraldProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Divine Herald Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the God Emperor's First Apostle companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[4]
				{
					Helpers.CreateLevelEntry(1, DivineHeraldHeritage),
					Helpers.CreateLevelEntry(7, DivineHeraldSanctifiedAura),
					Helpers.CreateLevelEntry(13, DivineHeraldSolarSpear),
					Helpers.CreateLevelEntry(20, DivineHeraldApotheosis)
				};
			});
			BlueprintFeature ChronoSpriteHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Hey! Listen! Temporal Guidance");
				bp.SetDescription(Main.IsekaiContext, "The Chrono Sprite flits ahead of danger, providing tactical alerts. Gains a +4 dodge bonus to AC, +4 bonus on Initiative, and immunity to ground hazards.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 4;
				});
			});
			BlueprintBuff ChronoSpriteHasteAuraBuff = TTCoreExtensions.CreateBuff("ChronoSpriteHasteAuraBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Temporal Haste Field");
				bp.SetDescription(Main.IsekaiContext, "Allies within 20 feet gain the benefits of Haste: +1 extra attack on full attack, +30 ft movement speed, and +1 dodge AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
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
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 1;
				});
			});
			BlueprintFeature ChronoSpriteHasteAura = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteHasteAura", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chrono Pulse Field");
				bp.SetDescription(Main.IsekaiContext, "The fairy emits a rhythmic pulse of accelerated time, providing a continuous Haste effect to all allies within 20 feet.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = ChronoSpriteHasteAuraBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature ChronoSpriteTemporalSlip = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteTemporalSlip", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Timeline Evasion");
				bp.SetDescription(Main.IsekaiContext, "The sprite shifts partially out of phase with present reality, gaining 50% concealment against all attacks and immunity to flanking.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
			});
			BlueprintFeature ChronoSpriteTemporalRewind = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteTemporalRewind", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Avatar of Heroic Timelines");
				bp.SetDescription(Main.IsekaiContext, "Mastering the flow of time, the Chrono Sprite grants Fast Healing 10, +6 luck bonus to AC, and ensures the Hero cannot be surprised or flat-footed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
			});
			ChronoSpriteProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Chrono Sprite Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Hero of Time's Chrono Sprite familiar.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[4]
				{
					Helpers.CreateLevelEntry(1, ChronoSpriteHeritage),
					Helpers.CreateLevelEntry(7, ChronoSpriteHasteAura),
					Helpers.CreateLevelEntry(13, ChronoSpriteTemporalSlip),
					Helpers.CreateLevelEntry(20, ChronoSpriteTemporalRewind)
				};
			});
			BlueprintFeature EnigmaticCoConspiratorHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Code of the Immortal Conspirator");
				bp.SetDescription(Main.IsekaiContext, "Bound by an otherworldly immortal contract, the conspirator gains Fast Healing 5, immunity to mind-affecting effects, and immunity to death effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Shaken;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Frightened;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Confusion;
				});
			});
			BlueprintFeature EnigmaticCoConspiratorCodeContact = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorCodeContact", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cognitive Contact");
				bp.SetDescription(Main.IsekaiContext, "Sharing a direct telepathic wavelength with the Mastermind, the conspirator gains an insight bonus to AC and all saving throws equal to the Mastermind's Intelligence modifier (minimum +3).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 3;
				});
			});
			BlueprintBuff EnigmaticCoConspiratorZeroCommandBuff = TTCoreExtensions.CreateBuff("EnigmaticCoConspiratorZeroCommandBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Strategic Dominance");
				bp.SetDescription(Main.IsekaiContext, "Enemies within 30 feet suffer a -4 penalty on Will saving throws against mental enchantments and mind-affecting abilities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -4;
				});
			});
			BlueprintFeature EnigmaticCoConspiratorZeroCommand = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorZeroCommand", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Command Protocol Zero");
				bp.SetDescription(Main.IsekaiContext, "The conspirator disrupts enemy mental coherence, imposing a -4 penalty on Will saving throws against enchantments on all foes within 30 feet.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = EnigmaticCoConspiratorZeroCommandBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature EnigmaticCoConspiratorImmortalPact = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorImmortalPact", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Immortal Accomplice Sovereignty");
				bp.SetDescription(Main.IsekaiContext, "Reaching true immortal partnership, the conspirator gains +6 insight bonus to AC, Spell Resistance 32, and Fast Healing 10.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = 32;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
				});
			});
			EnigmaticCoConspiratorProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Enigmatic Co-Conspirator Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Mastermind's enigmatic partner.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[4]
				{
					Helpers.CreateLevelEntry(1, EnigmaticCoConspiratorHeritage),
					Helpers.CreateLevelEntry(7, EnigmaticCoConspiratorCodeContact),
					Helpers.CreateLevelEntry(13, EnigmaticCoConspiratorZeroCommand),
					Helpers.CreateLevelEntry(20, EnigmaticCoConspiratorImmortalPact)
				};
			});
			BlueprintFeature ManifestedMartialSpiritHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Soul Resonance");
				bp.SetDescription(Main.IsekaiContext, "The Manifested Martial Spirit mirrors every combat movement. When fighting adjacent to its summoner, it grants a +3 shield bonus to the summoner's AC and gains an unarmed strike dealing 1d8 damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			BlueprintFeature ManifestedMartialSpiritTwinFlow = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritTwinFlow", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Strike Flurry");
				bp.SetDescription(Main.IsekaiContext, "The martial spirit gains an extra attack when making a full attack, and all its natural and melee strikes bypass Damage Reduction as adamantine and magic.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(1, DiceType.D6);
					c.Element = DamageEnergyType.Magic;
				});
			});
			BlueprintFeature ManifestedMartialSpiritPerfectParry = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritPerfectParry", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Reactive Parry Barrier");
				bp.SetDescription(Main.IsekaiContext, "Fluid defensive stances grant the spirit a +4 dodge bonus to AC and 2 additional attacks of opportunity per round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AttackOfOpportunityCount;
					c.Value = 2;
				});
			});
			BlueprintFeature ManifestedMartialSpiritResonance = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritResonance", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Supreme Martial Transmutation");
				bp.SetDescription(Main.IsekaiContext, "Reaching absolute harmony with the Martial God, the spirit gains +6 dodge AC, +4 to all saving throws, and deals an extra 2d6 magic damage on every strike.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula(2, DiceType.D6);
					c.Element = DamageEnergyType.Magic;
				});
			});
			ManifestedMartialSpiritProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Manifested Martial Spirit Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Martial God's Manifested Martial Spirit companion.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[4]
				{
					Helpers.CreateLevelEntry(1, ManifestedMartialSpiritHeritage),
					Helpers.CreateLevelEntry(7, ManifestedMartialSpiritTwinFlow),
					Helpers.CreateLevelEntry(13, ManifestedMartialSpiritPerfectParry),
					Helpers.CreateLevelEntry(20, ManifestedMartialSpiritResonance)
				};
			});
		}
	}
}
