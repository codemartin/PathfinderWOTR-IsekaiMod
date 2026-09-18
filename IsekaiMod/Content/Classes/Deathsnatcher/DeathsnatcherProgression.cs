using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using System.Collections.Generic;
using IsekaiMod.Content.Features.IsekaiProtagonist;
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
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Classes.Deathsnatcher
{
	internal class DeathsnatcherProgression
	{
		private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

		public static void Add()
		{
			BlueprintFeature Pounce = BlueprintTools.GetBlueprint<BlueprintFeature>("1a8149c09e0bdfc48a305ee6ac3729a8");
			BlueprintFeature DeathsnatcherSoulRendFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c8b468508a76c5140a9a2af00077753d");
			BlueprintFeature Evasion = BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");
			BlueprintFeature ImprovedEvasion = BlueprintTools.GetBlueprint<BlueprintFeature>("ce96af454a6137d47b9c6a1e02e66803");
			BlueprintFeature DeathsnatcherPoisonSting = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherPoisonSting");
			BlueprintFeature DeathsnatcherResistances = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherResistances");
			BlueprintFeature DeathsnatcherFastHealing = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherFastHealing");
			BlueprintFeature DeathsnatcherSizeBabyFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherSizeBabyFeature");
			BlueprintFeature DeathsnatcherCommandUndeadFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherCommandUndeadFeature");
			BlueprintFeature DeathsnatcherAnimateDeadFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherAnimateDeadFeature");
			BlueprintFeature DeathsnatcherCreateUndeadFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherCreateUndeadFeature");
			BlueprintFeature DeathsnatcherFingerOfDeathFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherFingerOfDeathFeature");
			BlueprintFeature DeathsnatcherAnimateDeadAdditionalUse = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherAnimateDeadAdditionalUse");
			BlueprintFeature DeathsnatcherUndeadMaster = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherUndeadMaster");
			Sprite icon = ((BlueprintUnitFact)DeathsnatcherSoulRendFeature)?.m_Icon;
			BlueprintFeature DeathsnatcherApexRend = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherApexRend", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Apex Soul Rend");
				bp.SetDescription(Main.IsekaiContext, "The Deathsnatcher's rending claws tear at the very life essence of its victims, dealing an additional 3d6 unholy damage on all claw and bite attacks.");
				((BlueprintUnitFact)bp).m_Icon = icon;
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
			BlueprintFeature DeathsnatcherEpicFastHealing = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherEpicFastHealing", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Deathsnatcher Abyssal Regeneration");
				bp.SetDescription(Main.IsekaiContext, "The Deathsnatcher's unholy vitality surges, granting Fast Healing 15 and immunity to bleed and poison.");
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 15;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Poison | SpellDescriptor.Bleed;
				});
			});
			BlueprintFeature DeathsnatcherMonstrousSovereign = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherMonstrousSovereign", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Monstrous Sovereign of the Dead");
				bp.SetDescription(Main.IsekaiContext, "Ascending as a dread predator of the void, the Deathsnatcher gains Damage Reduction 15/- and a +6 deflection bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 15;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
			});
			BlueprintFeature DeathsnatcherGreaterPoisonSting = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherGreaterPoisonSting", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Venom of Absolute Oblivion");
				bp.SetDescription(Main.IsekaiContext, "The scorpion stinger secretes apocalyptic venom. Natural attacks gain a +4 enhancement bonus to attack rolls and an additional 2d6 acid damage.");
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Acid };
				});
			});
			BlueprintFeature DeathsnatcherPrimevalApexCataclysm = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherPrimevalApexCataclysm", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Primeval Apex Cataclysm");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, the Deathsnatcher becomes a mythological apex catastrophe. Gains +10 Strength, +10 Constitution, and 2 extra attacks when making a full attack.");
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 10;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
				});
			});
			IsekaiPetProgression.GetCompanionProgression();
			DeathsnatcherClass.SetProgression(Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherClassProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(StaticReferences.Strings.Null);
				bp.SetDescription(Main.IsekaiContext, "This bipedal jackal has vulture wings and a rat tail ending in a scorpion's stinger. Each of its four arms ends in a clawed hand.");
				bp.IsClassFeature = true;
				bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
				bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
				bp.m_AlternateProgressionClasses = new BlueprintProgression.ClassWithLevel[0];
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = DeathsnatcherClass.GetReference(),
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[15]
				{
					Helpers.CreateLevelEntry(1, DeathsnatcherResistances, DeathsnatcherCommandUndeadFeature, DeathsnatcherSizeBabyFeature),
					Helpers.CreateLevelEntry(2, Evasion),
					Helpers.CreateLevelEntry(4, Pounce),
					Helpers.CreateLevelEntry(7, DeathsnatcherAnimateDeadFeature),
					Helpers.CreateLevelEntry(10, DeathsnatcherAnimateDeadAdditionalUse, DeathsnatcherPoisonSting),
					Helpers.CreateLevelEntry(13, DeathsnatcherCreateUndeadFeature),
					Helpers.CreateLevelEntry(15, DeathsnatcherSoulRendFeature, ImprovedEvasion),
					Helpers.CreateLevelEntry(16, DeathsnatcherFingerOfDeathFeature),
					Helpers.CreateLevelEntry(18, DeathsnatcherFastHealing),
					Helpers.CreateLevelEntry(20, DeathsnatcherUndeadMaster),
					Helpers.CreateLevelEntry(22, DeathsnatcherApexRend),
					Helpers.CreateLevelEntry(26, DeathsnatcherEpicFastHealing),
					Helpers.CreateLevelEntry(30, DeathsnatcherMonstrousSovereign),
					Helpers.CreateLevelEntry(35, DeathsnatcherGreaterPoisonSting),
					Helpers.CreateLevelEntry(40, DeathsnatcherPrimevalApexCataclysm)
				};
				bp.UIGroups = new UIGroup[2]
				{
					Helpers.CreateUIGroup(DeathsnatcherCommandUndeadFeature, DeathsnatcherAnimateDeadFeature, DeathsnatcherAnimateDeadAdditionalUse, DeathsnatcherCreateUndeadFeature, DeathsnatcherFingerOfDeathFeature, DeathsnatcherUndeadMaster),
					Helpers.CreateUIGroup(DeathsnatcherSizeBabyFeature, Pounce, DeathsnatcherPoisonSting, DeathsnatcherSoulRendFeature, DeathsnatcherFastHealing, DeathsnatcherApexRend, DeathsnatcherEpicFastHealing, DeathsnatcherMonstrousSovereign, DeathsnatcherGreaterPoisonSting, DeathsnatcherPrimevalApexCataclysm)
				};
				bp.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[1] { DeathsnatcherResistances.ToReference<BlueprintFeatureBaseReference>() };
			}));
		}

		public static BlueprintProgression GetCompanionProgression()
		{
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "DeathsnatcherCompanionProgression");
		}
	}
}
