using IsekaiMod.Utilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal static class SummonerEvolutionSelection
	{
		private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		private static readonly Sprite Icon_Pounce = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("1a8149c09e0bdfc48a305ee6ac3729a8"))?.m_Icon;

		private static BlueprintFeatureSelection Selection;

		public static void Configure()
		{
			if (Selection != null)
			{
				return;
			}
			BlueprintFeature EvolutionPounce = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionPounce", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Pounce");
				bp.SetDescription(Main.IsekaiContext, "Through primal predatory evolution, you and your companion can make a full attack when executing a charge.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pounce ?? Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddMechanicsFeature c)
				{
					c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.Pounce;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionCarapace = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionCarapace", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Reinforced Carapace");
				bp.SetDescription(Main.IsekaiContext, "Dense chitin and armored scales grant a +4 natural armor bonus to Armor Class and Damage Reduction 5/adamantine.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.Adamantite;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionEnergyInfusion = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionEnergyInfusion", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Elemental Infusion");
				bp.SetDescription(Main.IsekaiContext, "Elemental fire and lightning course through your strikes. All attacks deal an additional 1d6 fire and 1d6 electricity damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Fire };
				});
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Electricity };
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionBreath = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionBreath", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Primal Caustic Breath");
				bp.SetDescription(Main.IsekaiContext, "Caustic venom and arctic frost saturate your attacks, dealing an additional 1d6 acid and 1d6 cold damage on all strikes.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Acid };
				});
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Cold };
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionClawsRend = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionClawsRend", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Rending Talons");
				bp.SetDescription(Main.IsekaiContext, "Viciously honed talons and weapons pierce enemy defenses, granting a +3 enhancement bonus on attack and damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionColossalGrowth = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionColossalGrowth", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Colossal Might");
				bp.SetDescription(Main.IsekaiContext, "Titanic growth surges through your muscles, granting a +4 size bonus to Strength and Constitution, and a +2 natural armor bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionPlanarWings = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionPlanarWings", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Planar Wings");
				bp.SetDescription(Main.IsekaiContext, "Majestic planar wings manifest, granting a +3 dodge bonus to AC and a +10 foot bonus to base movement speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionSpellResistance = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionSpellResistance", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Spell Ward Carapace");
				bp.SetDescription(Main.IsekaiContext, "Mystic runes harden across your skin, granting Spell Resistance equal to 11 + your character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
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
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionRapidRegen = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionRapidRegen", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Rapid Cellular Regeneration");
				bp.SetDescription(Main.IsekaiContext, "Surging primordial vitality rapidly mends flesh and bone, granting Fast Healing 10.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature EvolutionIncorporealShift = Helpers.CreateBlueprint(Main.IsekaiContext, "EvolutionIncorporealShift", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Evolution: Spatiotemporal Phase Shift");
				bp.SetDescription(Main.IsekaiContext, "Phases partially beyond material reality, gaining a +4 dodge bonus to AC, and immunity to bleed, poison, and paralysis.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Poison | SpellDescriptor.Paralysis | SpellDescriptor.Bleed;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			Selection = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerEvolutionSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Eidolon Evolution Pool");
				bp.SetDescription(Main.IsekaiContext, "Select modular evolutions to mutate and enhance your physical and planar abilities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.Ranks = 11;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = false;
				bp.m_AllFeatures = new BlueprintFeatureReference[10]
				{
					EvolutionPounce.ToReference<BlueprintFeatureReference>(),
					EvolutionCarapace.ToReference<BlueprintFeatureReference>(),
					EvolutionEnergyInfusion.ToReference<BlueprintFeatureReference>(),
					EvolutionBreath.ToReference<BlueprintFeatureReference>(),
					EvolutionClawsRend.ToReference<BlueprintFeatureReference>(),
					EvolutionColossalGrowth.ToReference<BlueprintFeatureReference>(),
					EvolutionPlanarWings.ToReference<BlueprintFeatureReference>(),
					EvolutionSpellResistance.ToReference<BlueprintFeatureReference>(),
					EvolutionRapidRegen.ToReference<BlueprintFeatureReference>(),
					EvolutionIncorporealShift.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}

		public static BlueprintFeatureSelection Get()
		{
			if (Selection != null)
			{
				return Selection;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SummonerEvolutionSelection");
		}
	}
}
