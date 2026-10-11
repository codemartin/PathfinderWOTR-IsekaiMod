using IsekaiMod.Content.Features;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians;

internal static class SubclassGuardianProgressions
{
	private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

	private static readonly Sprite Icon_Angel = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("3a6db57fce75b0244a6a5819528ddf26"))?.m_Icon;

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
		BlueprintBuff TempestStarWolfShadowVeilBuff = TTCoreExtensions.CreateBuff("TempestStarWolfShadowVeilBuff", delegate(BlueprintBuff bp)
		{
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
			bp.FxOnStart = new PrefabLink
			{
				AssetId = "b6b3ca16af1fc8c47bb055d94c3a1d54"
			};
			bp.FxOnRemove = new PrefabLink
			{
				AssetId = "39802c709edd65941a3da18e3c581965"
			};
		});
		BlueprintBuff TempestStarWolfStormArcsBuff = TTCoreExtensions.CreateBuff("TempestStarWolfStormArcsBuff", delegate(BlueprintBuff bp)
		{
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
			bp.FxOnStart = new PrefabLink
			{
				AssetId = "33ff8a78376aad6439092cad0fbf37fd"
			};
		});
		BlueprintFeature TempestStarWolfHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Tempest Fang & Lightning Strike");
			bp.SetDescription(Main.IsekaiContext, "The Tempest Star Wolf's bite attacks deal an extra 1d6 electricity damage, and it gains a +10 foot racial bonus to speed.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Electricity
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 1,
					BonusValue = 0
				};
				c.CheckWeaponCatergoy = true;
				c.Category = WeaponCategory.Bite;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					TempestStarWolfShadowVeilBuff.ToReference<BlueprintUnitFactReference>(),
					TempestStarWolfStormArcsBuff.ToReference<BlueprintUnitFactReference>()
				};
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
					DiceCountValue = 1,
					BonusValue = 0
				};
			});
		});
		BlueprintBuff TempestStarWolfShadowPackBuff = TTCoreExtensions.CreateBuff("TempestStarWolfShadowPackBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Pack Resonance");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +4 morale bonus on attack and damage rolls.");
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
		BlueprintAbilityAreaEffect TempestStarWolfShadowPackArea = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfShadowPackArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(TempestStarWolfShadowPackBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff TempestStarWolfShadowPackAreaBuff = TTCoreExtensions.CreateBuff("TempestStarWolfShadowPackAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Pack Alpha");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +4 morale bonus on attack and damage rolls.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = TempestStarWolfShadowPackArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		BlueprintFeature TempestStarWolfShadowPack = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfShadowPack", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Pack Alpha");
			bp.SetDescription(Main.IsekaiContext, "Emitting an alpha howl, the Star Wolf coordinates strikes with its master. Allies within 30 feet gain a +4 morale bonus on attack and damage rolls.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = TempestStarWolfShadowPackAreaBuff.ToReference<BlueprintBuffReference>();
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
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Paralyzed;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Entangled;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.DifficultTerrain;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Slowed;
			});
			bp.AddComponent(delegate(ManeuverImmunity c)
			{
				c.Type = CombatManeuver.Grapple;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.CantMove;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Staggered | SpellDescriptor.Paralysis | SpellDescriptor.MovementImpairing;
			});
			bp.AddComponent(delegate(SpecificBuffImmunity c)
			{
				c.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("73b860ae69621ac4fb27d7bbeef914ba");
			});
			bp.AddComponent(delegate(SpecificBuffImmunity c)
			{
				c.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0bc608c3f2b548b44b7146b7530613ac");
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Staggered;
			});
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Electricity
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 2,
					BonusValue = 0
				};
			});
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Unholy
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 2,
					BonusValue = 0
				};
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
			bp.SetDescription(Main.IsekaiContext, "Igris wields a greatsword with deadly grace, clad in impenetrable shadow plate. Gains Damage Reduction 5/- and immunity to being shaken, frightened, or confused.");
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
			bp.SetDescription(Main.IsekaiContext, "Igris's greatsword strikes with sovereign authority, gaining a +3 enhancement bonus on attack rolls and dealing an additional 2d6 negative energy damage on all attacks.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
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
					DiceCountValue = 2,
					BonusValue = 0
				};
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
			bp.SetName(Main.IsekaiContext, "Shadow Marshal's Discipline");
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
		BlueprintAbilityAreaEffect ShadowMarshallCommanderAuraArea = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallCommanderAuraArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(ShadowMarshallCommanderAuraBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff ShadowMarshallCommanderAuraAreaBuff = TTCoreExtensions.CreateBuff("ShadowMarshallCommanderAuraAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Vanguard Commander");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +4 enhancement bonus on attack rolls and a +2 bonus on all saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = ShadowMarshallCommanderAuraArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		BlueprintFeature ShadowMarshallCommanderAura = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallCommanderAura", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Vanguard Commander");
			bp.SetDescription(Main.IsekaiContext, "As the vanguard commander of the shadow army, Igris inspires all allies within 30 feet with +4 attack bonus and +2 to all saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = ShadowMarshallCommanderAuraAreaBuff.ToReference<BlueprintBuffReference>();
			});
		});
		BlueprintFeature ShadowMarshallAbsoluteLoyalty = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallAbsoluteLoyalty", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Apex Shadow Marshal: Undying Fealty");
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
				c.Bonus = 0;
			});
		});
		ShadowMarshallProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallProgression", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Shadow Marshal Progression");
			bp.SetDescription(Main.IsekaiContext, "Progression for the Shadow Monarch's loyal Marshal Igris.");
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
			bp.AddComponent(delegate(ACBonusAgainstAttacks c)
			{
				c.AgainstMeleeOnly = true;
				c.ArmorClassBonus = 3;
				c.Value = 0;
				c.Descriptor = ModifierDescriptor.Dodge;
				c.NotArmorCategory = new ArmorProficiencyGroup[0];
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
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Unholy
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 2,
					BonusValue = 0
				};
			});
			bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
			{
				c.AddAlignment = true;
				c.Alignment = DamageAlignment.Good;
			});
			bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
			{
				c.AddMaterial = true;
				c.Material = PhysicalDamageMaterial.Silver;
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
		BlueprintAbilityAreaEffect OverlordGuardianDespairAuraArea = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianDespairAuraArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = true;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(OverlordGuardianDespairAuraBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff OverlordGuardianDespairAuraAreaBuff = TTCoreExtensions.CreateBuff("OverlordGuardianDespairAuraAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Wings of the Fallen Overseer");
			bp.SetDescription(Main.IsekaiContext, "Enemies within 30 feet suffer a -2 penalty on attack rolls and -2 on saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = OverlordGuardianDespairAuraArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		BlueprintFeature OverlordGuardianFallenWings = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianFallenWings", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Wings of the Fallen Overseer");
			bp.SetDescription(Main.IsekaiContext, "Jet-black wings mark the Dark Valkyrie as an overseer of the Great Tomb. She radiates dreadful despair, imposing a -2 penalty on attack rolls and saving throws on enemies within 30 feet, and gains Spell Resistance 11 + Level.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = OverlordGuardianDespairAuraAreaBuff.ToReference<BlueprintBuffReference>();
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
			bp.SetDescription(Main.IsekaiContext, "Reaching the apex of devotion to the Overlord, the Dark Valkyrie gains +6 deflection bonus to AC, +4 to all saving throws, and immunity to death effects, negative levels, and negative energy damage.");
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
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
			});
			bp.AddComponent<AddImmunityToEnergyDrain>();
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
		BlueprintAbilityAreaEffect DivineHeraldSanctifiedAuraArea = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldSanctifiedAuraArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(DivineHeraldSanctifiedAuraBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff DivineHeraldSanctifiedAuraAreaBuff = TTCoreExtensions.CreateBuff("DivineHeraldSanctifiedAuraAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Sanctified Radiance Aura");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +3 sacred bonus on attack rolls and saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = DivineHeraldSanctifiedAuraArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		BlueprintFeature DivineHeraldSanctifiedAura = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldSanctifiedAura", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Sanctified Radiance Aura");
			bp.SetDescription(Main.IsekaiContext, "The First Apostle radiates sacred light, granting allies within 30 feet +3 sacred bonus to attack rolls and all saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = DivineHeraldSanctifiedAuraAreaBuff.ToReference<BlueprintBuffReference>();
			});
		});
		BlueprintFeature DivineHeraldSolarSpear = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldSolarSpear", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Solar Radiance Strike");
			bp.SetDescription(Main.IsekaiContext, "Strikes burst with blinding solar brilliance, dealing an additional 2d6 holy damage.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
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
					DiceCountValue = 2,
					BonusValue = 0
				};
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
			bp.AddComponent(delegate(AddCondition c)
			{
				c.Condition = UnitCondition.TrueSeeing;
			});
			bp.AddComponent<AddImmunityToEnergyDrain>();
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.NegativeLevel;
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
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.DifficultTerrain;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Ground;
			});
		});
		ExtraWings.AddPitImmunity(ChronoSpriteHeritage);
		BlueprintBuff ChronoSpriteHasteAuraBuff = TTCoreExtensions.CreateBuff("ChronoSpriteHasteAuraBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Temporal Haste Field");
			bp.SetDescription(Main.IsekaiContext, "Allies within 20 feet gain the benefits of Haste: +1 extra attack on full attack, +30 ft movement speed, and +1 dodge AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = true;
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
		BlueprintAbilityAreaEffect ChronoSpriteHasteAuraArea = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteHasteAuraArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(20f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(ChronoSpriteHasteAuraBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff ChronoSpriteHasteAuraAreaBuff = TTCoreExtensions.CreateBuff("ChronoSpriteHasteAuraAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Chrono Pulse Field");
			bp.SetDescription(Main.IsekaiContext, "Allies within 20 feet gain the benefits of Haste: +1 extra attack on full attack, +30 ft movement speed, and +1 dodge AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = ChronoSpriteHasteAuraArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		BlueprintFeature ChronoSpriteHasteAura = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteHasteAura", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Chrono Pulse Field");
			bp.SetDescription(Main.IsekaiContext, "The fairy emits a rhythmic pulse of accelerated time, providing a continuous Haste effect to all allies within 20 feet.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = ChronoSpriteHasteAuraAreaBuff.ToReference<BlueprintBuffReference>();
			});
		});
		BlueprintFeature ChronoSpriteTemporalSlip = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteTemporalSlip", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Timeline Evasion");
			bp.SetDescription(Main.IsekaiContext, "The sprite shifts partially out of phase with present reality, gaining 50% concealment against all attacks, immunity to flanking, and a +4 luck bonus on Reflex and Will saving throws.");
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
			bp.AddComponent(delegate(AddConcealment c)
			{
				c.Descriptor = ConcealmentDescriptor.Displacement;
				c.Concealment = Concealment.Total;
				c.OnlyForAttacks = true;
			});
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked;
			});
		});
		BlueprintFeature ChronoSpriteTemporalRewind = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteTemporalRewind", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Avatar of Heroic Timelines");
			bp.SetDescription(Main.IsekaiContext, "Mastering the flow of time, the Chrono Sprite gains Fast Healing 10 and a +6 luck bonus to AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 10;
				c.Bonus = 0;
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
				Helpers.CreateLevelEntry(7, ChronoSpriteTemporalSlip),
				Helpers.CreateLevelEntry(13, ChronoSpriteHasteAura),
				Helpers.CreateLevelEntry(20, ChronoSpriteTemporalRewind)
			};
		});
		BlueprintFeature EnigmaticCoConspiratorHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Code of the Immortal Conspirator");
			bp.SetDescription(Main.IsekaiContext, "Bound by an otherworldly immortal contract, the conspirator gains Fast Healing 5 and immunity to being shaken, frightened, or confused.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
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
			bp.SetDescription(Main.IsekaiContext, "Sharing a direct telepathic wavelength with the Mastermind, the conspirator gains a +3 insight bonus to AC and all saving throws.");
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
			bp.SetDescription(Main.IsekaiContext, "Enemies within 30 feet suffer a -4 penalty on Will saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Penalty;
				c.Stat = StatType.SaveWill;
				c.Value = -4;
			});
		});
		BlueprintAbilityAreaEffect EnigmaticCoConspiratorZeroCommandArea = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorZeroCommandArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = true;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(EnigmaticCoConspiratorZeroCommandBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff EnigmaticCoConspiratorZeroCommandAreaBuff = TTCoreExtensions.CreateBuff("EnigmaticCoConspiratorZeroCommandAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Command Protocol Zero");
			bp.SetDescription(Main.IsekaiContext, "Enemies within 30 feet suffer a -4 penalty on Will saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = EnigmaticCoConspiratorZeroCommandArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		BlueprintFeature EnigmaticCoConspiratorZeroCommand = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorZeroCommand", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Command Protocol Zero");
			bp.SetDescription(Main.IsekaiContext, "The conspirator disrupts enemy mental coherence: enemies within 30 feet suffer a -4 penalty on Will saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = EnigmaticCoConspiratorZeroCommandAreaBuff.ToReference<BlueprintBuffReference>();
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
				c.Bonus = 0;
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
			bp.SetDescription(Main.IsekaiContext, "The Manifested Martial Spirit mirrors every combat movement, gaining a +3 dodge bonus to AC and a +2 enhancement bonus on attack rolls.");
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
			bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
			{
				c.AddMagic = true;
				c.AddMaterial = true;
				c.Material = PhysicalDamageMaterial.Adamantite;
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
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Magic
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 2,
					BonusValue = 0
				};
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
