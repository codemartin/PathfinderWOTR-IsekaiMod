using Kingmaker.RuleSystem.Rules.Damage;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
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
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Electricity };
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
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
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
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Electricity };
				});
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
				});
			});
			BlueprintFeature TempestStarWolfEpicStaticFangs = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfEpicStaticFangs", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tempest Fang Sovereign");
				bp.SetDescription(Main.IsekaiContext, "The Tempest Star Wolf's bite deals an additional 2d6 electricity damage and grants a +4 enhancement bonus on attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Electricity };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			});
			BlueprintFeature TempestStarWolfEpicStormCloak = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfEpicStormCloak", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cloak of the Thunder Sovereign");
				bp.SetDescription(Main.IsekaiContext, "The wolf becomes one with primal storm elements, gaining immunity to electricity and a +6 dodge bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Electricity;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
			});
			BlueprintBuff TempestStarWolfEpicHowlBuff = TTCoreExtensions.CreateBuff("TempestStarWolfEpicHowlBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Alpha Storm Howl");
				bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain an extra attack when making a full attack and a +30 foot bonus to movement speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.Speed;
					c.Value = 30;
				});
			});
			BlueprintFeature TempestStarWolfEpicHowl = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfEpicHowl", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Apex Alpha Storm Howl");
				bp.SetDescription(Main.IsekaiContext, "The wolf radiates a thunderous alpha aura. Allies within 30 feet gain an extra attack when making a full attack and a +30 foot bonus to movement speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = TempestStarWolfEpicHowlBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature TempestStarWolfEpicStormAlphaApex = Helpers.CreateBlueprint(Main.IsekaiContext, "TempestStarWolfEpicStormAlphaApex", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grand Alpha Star Tempest");
				bp.SetDescription(Main.IsekaiContext, "Reaching absolute primal apex, the Star Wolf gains a +10 inherent bonus to Strength and Dexterity, and all attacks deal an additional 3d6 electricity and 3d6 unholy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
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
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 3, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Electricity };
				});
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 3, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
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
				bp.LevelEntries = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, TempestStarWolfHeritage),
					Helpers.CreateLevelEntry(7, TempestStarWolfShadowLightning),
					Helpers.CreateLevelEntry(13, TempestStarWolfShadowPack),
					Helpers.CreateLevelEntry(20, TempestStarWolfApexStorm),
					Helpers.CreateLevelEntry(25, TempestStarWolfEpicStaticFangs),
					Helpers.CreateLevelEntry(30, TempestStarWolfEpicStormCloak),
					Helpers.CreateLevelEntry(35, TempestStarWolfEpicHowl),
					Helpers.CreateLevelEntry(40, TempestStarWolfEpicStormAlphaApex)
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
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion;
				});
			});
			BlueprintFeature ShadowMarshallDominion = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallDominion", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Sovereign Strike");
				bp.SetDescription(Main.IsekaiContext, "Igris's greatsword sweeps cleave through multiple foes, dealing an additional 2d6 negative energy damage on all attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
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
			BlueprintFeature ShadowMarshallEpicShadowCleave = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallEpicShadowCleave", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Abyssal Shadow Cleave");
				bp.SetDescription(Main.IsekaiContext, "Igris's greatsword radiates necrotic force, dealing an additional 3d6 unholy damage on all strikes and gaining a +4 enhancement bonus to attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 3, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			});
			BlueprintBuff ShadowMarshallEpicMonarchPresenceBuff = TTCoreExtensions.CreateBuff("ShadowMarshallEpicMonarchPresenceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Aura of the Shadow Monarch");
				bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +4 morale bonus to damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
			});
			BlueprintFeature ShadowMarshallEpicMonarchPresence = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallEpicMonarchPresence", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Presence of the Shadow Monarch");
				bp.SetDescription(Main.IsekaiContext, "Igris channels the dark domain of the monarch, gaining DR 15/- and granting allies within 30 feet a +4 morale bonus to damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 15;
				});
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = ShadowMarshallEpicMonarchPresenceBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature ShadowMarshallEpicUndyingLegionnaire = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallEpicUndyingLegionnaire", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Undying Legionnaire Supreme");
				bp.SetDescription(Main.IsekaiContext, "As long as the Shadow Monarch stands, Igris cannot be quelled. Gains Fast Healing 20 and immunity to death effects and negative energy.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 20;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.NegativeEnergy;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
			});
			BlueprintFeature ShadowMarshallEpicGrandMarshallAscension = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMarshallEpicGrandMarshallAscension", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grand Marshall Ascension");
				bp.SetDescription(Main.IsekaiContext, "Igris attains absolute grand marshall sovereignty, gaining +10 Strength, +8 natural armor to AC, and 3 additional attacks of opportunity per round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AttackOfOpportunityCount;
					c.Value = 3;
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
				bp.LevelEntries = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, ShadowMarshallHeritage),
					Helpers.CreateLevelEntry(7, ShadowMarshallDominion),
					Helpers.CreateLevelEntry(13, ShadowMarshallCommanderAura),
					Helpers.CreateLevelEntry(20, ShadowMarshallAbsoluteLoyalty),
					Helpers.CreateLevelEntry(25, ShadowMarshallEpicShadowCleave),
					Helpers.CreateLevelEntry(30, ShadowMarshallEpicMonarchPresence),
					Helpers.CreateLevelEntry(35, ShadowMarshallEpicUndyingLegionnaire),
					Helpers.CreateLevelEntry(40, ShadowMarshallEpicGrandMarshallAscension)
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
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
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
			BlueprintFeature OverlordGuardianEpicUnholyPierce = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianEpicUnholyPierce", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Piercing Abyssal Lance");
				bp.SetDescription(Main.IsekaiContext, "The Dark Valkyrie's strikes bypass all damage reduction and deal an additional 3d6 unholy damage, gaining a +4 enhancement bonus to attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 3, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			});
			BlueprintFeature OverlordGuardianEpicOverseerAura = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianEpicOverseerAura", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Supreme Overseer Domain");
				bp.SetDescription(Main.IsekaiContext, "The Dark Valkyrie commands absolute obedience from the elements, gaining immunity to fire, cold, and acid.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Fire;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Cold;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Acid;
				});
			});
			BlueprintFeature OverlordGuardianEpicAbsoluteDevotion = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianEpicAbsoluteDevotion", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Floor Guardian Aegis");
				bp.SetDescription(Main.IsekaiContext, "The Dark Valkyrie gains Fast Healing 20 and an +8 deflection bonus to Armor Class.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 8;
				});
			});
			BlueprintFeature OverlordGuardianEpicApexFloorOverseer = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordGuardianEpicApexFloorOverseer", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Supreme Floor Overseer Apex");
				bp.SetDescription(Main.IsekaiContext, "Reaching the absolute zenith of the Great Tomb's guardians, she gains +10 Constitution and Charisma, and a +6 resistance bonus to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Demon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
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
				bp.LevelEntries = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, OverlordGuardianHeritage),
					Helpers.CreateLevelEntry(7, OverlordGuardianUnholySpear),
					Helpers.CreateLevelEntry(13, OverlordGuardianFallenWings),
					Helpers.CreateLevelEntry(20, OverlordGuardianSacrifice),
					Helpers.CreateLevelEntry(25, OverlordGuardianEpicUnholyPierce),
					Helpers.CreateLevelEntry(30, OverlordGuardianEpicOverseerAura),
					Helpers.CreateLevelEntry(35, OverlordGuardianEpicAbsoluteDevotion),
					Helpers.CreateLevelEntry(40, OverlordGuardianEpicApexFloorOverseer)
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
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Holy };
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
			BlueprintFeature DivineHeraldEpicSolarSmite = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldEpicSolarSmite", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Solar Retribution Strike");
				bp.SetDescription(Main.IsekaiContext, "The First Apostle strikes with blinding holy light, dealing an additional 3d6 holy damage and gaining a +4 sacred bonus to attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 3, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Holy };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			});
			BlueprintBuff DivineHeraldEpicApostleHaloBuff = TTCoreExtensions.CreateBuff("DivineHeraldEpicApostleHaloBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Halo of the Golden Throne");
				bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet gain a +4 sacred bonus to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
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
			BlueprintFeature DivineHeraldEpicApostleHalo = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldEpicApostleHalo", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Apostle Halo of the Golden Throne");
				bp.SetDescription(Main.IsekaiContext, "The apostle radiates sovereign warmth, gaining Fast Healing 15 and granting allies within 30 feet a +4 sacred bonus to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 15;
				});
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = DivineHeraldEpicApostleHaloBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature DivineHeraldEpicWingsOfApotheosis = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldEpicWingsOfApotheosis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Wings of Imperial Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "The apostle gains Damage Reduction 15/- and complete immunity to mind-affecting effects and paralysis.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 15;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Paralysis;
				});
			});
			BlueprintFeature DivineHeraldEpicFirstApostleSolarApex = Helpers.CreateBlueprint(Main.IsekaiContext, "DivineHeraldEpicFirstApostleSolarApex", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "First Apostle of the Solar Throne");
				bp.SetDescription(Main.IsekaiContext, "Attaining the peak of imperial divinity, the apostle gains a +10 sacred bonus to all ability scores and a +10 sacred bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Strength;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Dexterity;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Constitution;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Intelligence;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Wisdom;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Charisma;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 10;
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
				bp.LevelEntries = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, DivineHeraldHeritage),
					Helpers.CreateLevelEntry(7, DivineHeraldSanctifiedAura),
					Helpers.CreateLevelEntry(13, DivineHeraldSolarSpear),
					Helpers.CreateLevelEntry(20, DivineHeraldApotheosis),
					Helpers.CreateLevelEntry(25, DivineHeraldEpicSolarSmite),
					Helpers.CreateLevelEntry(30, DivineHeraldEpicApostleHalo),
					Helpers.CreateLevelEntry(35, DivineHeraldEpicWingsOfApotheosis),
					Helpers.CreateLevelEntry(40, DivineHeraldEpicFirstApostleSolarApex)
				};
			});
			BlueprintFeature ChronoSpriteHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Hey! Listen! Temporal Guidance");
				bp.SetDescription(Main.IsekaiContext, "The Chrono Sprite flits ahead of danger, providing tactical alerts. Gains a +4 dodge bonus to AC, +4 bonus on Initiative, and immunity to ground hazards.");
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
			BlueprintFeature ChronoSpriteEpicTemporalAcceleration = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteEpicTemporalAcceleration", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Temporal Chrono Acceleration");
				bp.SetDescription(Main.IsekaiContext, "The sprite accelerates local time ribbons, gaining +4 dodge AC, +4 initiative, and granting +20 foot bonus to movement speed.");
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
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Speed;
					c.Value = 20;
				});
			});
			BlueprintFeature ChronoSpriteEpicFairyProtection = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteEpicFairyProtection", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Fairy Queen's Eternal Ward");
				bp.SetDescription(Main.IsekaiContext, "The Chrono Sprite gains a +8 luck bonus to AC and a +6 luck bonus on all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 8;
				});
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
			BlueprintFeature ChronoSpriteEpicTimeStopResonance = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteEpicTimeStopResonance", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Spatiotemporal Phase Slip");
				bp.SetDescription(Main.IsekaiContext, "The sprite permanently phases between alternate timelines, gaining Fast Healing 15 and immunity to movement impairment, paralysis, and petrification.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 15;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Paralysis | SpellDescriptor.Petrified | SpellDescriptor.MovementImpairing;
				});
			});
			BlueprintFeature ChronoSpriteEpicMasterOfTimelines = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteEpicMasterOfTimelines", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Master of Temporal Weft");
				bp.SetDescription(Main.IsekaiContext, "Reaching absolute mastery of the heroic timeline, the sprite gains Fast Healing 25, +10 Dexterity and Charisma, and an extra attack on full attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 25;
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
					c.Stat = StatType.Charisma;
					c.Value = 10;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
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
				bp.LevelEntries = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, ChronoSpriteHeritage),
					Helpers.CreateLevelEntry(7, ChronoSpriteHasteAura),
					Helpers.CreateLevelEntry(13, ChronoSpriteTemporalSlip),
					Helpers.CreateLevelEntry(20, ChronoSpriteTemporalRewind),
					Helpers.CreateLevelEntry(25, ChronoSpriteEpicTemporalAcceleration),
					Helpers.CreateLevelEntry(30, ChronoSpriteEpicFairyProtection),
					Helpers.CreateLevelEntry(35, ChronoSpriteEpicTimeStopResonance),
					Helpers.CreateLevelEntry(40, ChronoSpriteEpicMasterOfTimelines)
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
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion;
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
			BlueprintFeature EnigmaticCoConspiratorEpicMindLink = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorEpicMindLink", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grand Strategist Telepathic Network");
				bp.SetDescription(Main.IsekaiContext, "The conspirator weaves an unbreakable cognitive web, gaining a +4 insight bonus to AC and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
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
			});
			BlueprintBuff EnigmaticCoConspiratorEpicOverwhelmingGeasBuff = TTCoreExtensions.CreateBuff("EnigmaticCoConspiratorEpicOverwhelmingGeasBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Aura of Overwhelming Geas");
				bp.SetDescription(Main.IsekaiContext, "Enemies within 30 feet suffer a -4 penalty on Will saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -4;
				});
			});
			BlueprintFeature EnigmaticCoConspiratorEpicOverwhelmingGeas = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorEpicOverwhelmingGeas", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overwhelming Geas Domain");
				bp.SetDescription(Main.IsekaiContext, "An insidious mental pressure radiates outward, reducing the Will saving throws of all enemies within 30 feet by -4.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = EnigmaticCoConspiratorEpicOverwhelmingGeasBuff.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintFeature EnigmaticCoConspiratorEpicImmortalityPact = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorEpicImmortalityPact", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Pact of the Eternal Accomplice");
				bp.SetDescription(Main.IsekaiContext, "The immortal contract solidifies into absolute law. Gains Fast Healing 20, Spell Resistance 38, and immunity to ability score drain.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 20;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = 38;
				});
			});
			BlueprintFeature EnigmaticCoConspiratorEpicApexPuppetmaster = Helpers.CreateBlueprint(Main.IsekaiContext, "EnigmaticCoConspiratorEpicApexPuppetmaster", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grand Stratagem Puppetmaster");
				bp.SetDescription(Main.IsekaiContext, "The conspirator steps into the light as the unseen ruler of fate. Gains +10 Intelligence, Wisdom, and Charisma, and a +10 insight bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
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
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 10;
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
				bp.LevelEntries = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, EnigmaticCoConspiratorHeritage),
					Helpers.CreateLevelEntry(7, EnigmaticCoConspiratorCodeContact),
					Helpers.CreateLevelEntry(13, EnigmaticCoConspiratorZeroCommand),
					Helpers.CreateLevelEntry(20, EnigmaticCoConspiratorImmortalPact),
					Helpers.CreateLevelEntry(25, EnigmaticCoConspiratorEpicMindLink),
					Helpers.CreateLevelEntry(30, EnigmaticCoConspiratorEpicOverwhelmingGeas),
					Helpers.CreateLevelEntry(35, EnigmaticCoConspiratorEpicImmortalityPact),
					Helpers.CreateLevelEntry(40, EnigmaticCoConspiratorEpicApexPuppetmaster)
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
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
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
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
				});
			});
			BlueprintFeature ManifestedMartialSpiritEpicTwinFlurry = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritEpicTwinFlurry", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Endless Dao Flurry");
				bp.SetDescription(Main.IsekaiContext, "The martial spirit gains an additional extra attack when making a full attack and a +4 dodge bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
			});
			BlueprintFeature ManifestedMartialSpiritEpicUnbreakableForm = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritEpicUnbreakableForm", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Indomitable Adamantine Body");
				bp.SetDescription(Main.IsekaiContext, "Hardened by countless battles, the spirit gains Damage Reduction 15/- and immunity to bleed and stun.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 15;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Stun | SpellDescriptor.Bleed;
				});
			});
			BlueprintFeature ManifestedMartialSpiritEpicGrandmasterParry = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritEpicGrandmasterParry", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grandmaster Parry Barrier");
				bp.SetDescription(Main.IsekaiContext, "The spirit fluidly deflects lethal strikes, gaining a +8 dodge bonus to AC and Fast Healing 15.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 15;
				});
			});
			BlueprintFeature ManifestedMartialSpiritEpicTwinDaoApex = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestedMartialSpiritEpicTwinDaoApex", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Dao Apex Manifestation");
				bp.SetDescription(Main.IsekaiContext, "Reaching the absolute pinnacle of martial perfection, the spirit gains +10 Strength and Dexterity, and all strikes deal an additional 4d6 magic damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
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
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 4, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
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
				bp.LevelEntries = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, ManifestedMartialSpiritHeritage),
					Helpers.CreateLevelEntry(7, ManifestedMartialSpiritTwinFlow),
					Helpers.CreateLevelEntry(13, ManifestedMartialSpiritPerfectParry),
					Helpers.CreateLevelEntry(20, ManifestedMartialSpiritResonance),
					Helpers.CreateLevelEntry(25, ManifestedMartialSpiritEpicTwinFlurry),
					Helpers.CreateLevelEntry(30, ManifestedMartialSpiritEpicUnbreakableForm),
					Helpers.CreateLevelEntry(35, ManifestedMartialSpiritEpicGrandmasterParry),
					Helpers.CreateLevelEntry(40, ManifestedMartialSpiritEpicTwinDaoApex)
				};
			});
		}
	}
}
