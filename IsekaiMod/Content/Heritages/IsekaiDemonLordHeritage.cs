using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Heritages {

    /// <summary>
    /// Ported from the 2025-12-19 beta binary (DemonLordHeritage). The beta shipped the class
    /// but never called it; here it is registered on the Tiefling heritage selection next to
    /// the Lust Demon. Must run after IsekaiSuccubusHeritage (shares its charm ability) and
    /// after ExtraWings (wing abilities).
    /// </summary>
    internal class IsekaiDemonLordHeritage {
        private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

        public static void Add() {
            var DevilWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(IsekaiContext, "DevilWingsAbility");
            var DemonWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(IsekaiContext, "DemonWingsAbility");
            var BlackWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(IsekaiContext, "BlackWingsAbility");
            var SuccubusCharmAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(IsekaiContext, "SuccubusCharmAbility");

            // Demon Lord Heritage
            var Icon_DemonLord = AssetLoader.LoadInternal(IsekaiContext, "Heritages", "ICON_SUCCUBUS.png");
            var IsekaiDemonLordHeritage = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "IsekaiDemonLordHeritage", bp => {
                bp.SetName(IsekaiContext, "Isekai Demon Lord");
                bp.SetDescription(IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a Demon Lord have both extreme intelligence and power, and often "
                    + "have a propensity towards devilish plans.\n"
                    + "The Isekai Demon Lord has a +2 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} and {g|Encyclopedia:Intelligence}Intelligence{/g}, "
                    + "a +4 racial bonus to {g|Encyclopedia:Charisma}Charisma{/g}, "
                    + "a -2 {g|Encyclopedia:Penalty}penalty{/g} to {g|Encyclopedia:Strength}Strength{/g}, and a +2 racial bonus on {g|Encyclopedia:Persuasion}Persuasion{/g} and "
                    + "{g|Encyclopedia:Perception}Perception checks{/g}. "
                    + "They have DR 10/Cold Iron or Good, and have spell resistance equal to 10 + their character level. "
                    + "They have immunity to fire, electricity, and poisons as well as acid and cold resistance 20. "
                    + "They can also use the Charm spell once per day.");
                bp.m_Icon = Icon_DemonLord;

                // Attributes
                bp.AddComponent<AddStatBonusIfHasFact>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Strength;
                    c.Value = -2;
                    c.InvertCondition = true;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Dexterity;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Intelligence;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.Charisma;
                    c.Value = 4;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.SkillPerception;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Racial;
                    c.Stat = StatType.SkillPersuasion;
                    c.Value = 2;
                });

                // DR 10/cold iron or good
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Or = true;
                    c.Value = 10;
                    c.BypassedByMaterial = true;
                    c.BypassedByAlignment = true;
                    c.Material = PhysicalDamageMaterial.ColdIron;
                    c.Alignment = DamageAlignment.Good;
                });

                // Spell Resistance 10 + character level
                bp.AddComponent<AddSpellResistance>(c => {
                    c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Progression = ContextRankProgression.BonusValue;
                    c.m_StepLevel = 10;
                });

                // Resistances and Immunities
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Acid;
                    c.Value = 20;
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Cold;
                    c.Value = 20;
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Fire;
                    c.Value = 20;
                });
                bp.AddComponent<AddDamageResistanceEnergy>(c => {
                    c.Type = DamageEnergyType.Electricity;
                    c.Value = 20;
                });
                bp.AddComponent<AddEnergyImmunity>(c => {
                    c.Type = DamageEnergyType.Electricity;
                });
                bp.AddComponent<AddEnergyImmunity>(c => {
                    c.Type = DamageEnergyType.Fire;
                });
                bp.AddComponent<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.Poison
                    | SpellDescriptor.Electricity
                    | SpellDescriptor.Fire;
                });
                bp.AddComponent<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.Poison
                    | SpellDescriptor.Electricity
                    | SpellDescriptor.Fire;
                });

                // Abilities (the beta description promises Charm but its component list omitted it; granted here)
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        SuccubusCharmAbility.ToReference<BlueprintUnitFactReference>(),
                        DevilWingsAbility.ToReference<BlueprintUnitFactReference>(),
                        DemonWingsAbility.ToReference<BlueprintUnitFactReference>(),
                        BlackWingsAbility.ToReference<BlueprintUnitFactReference>()
                    };
                });

                bp.Groups = new FeatureGroup[] { FeatureGroup.Racial, FeatureGroup.TieflingHeritage };
                bp.ReapplyOnLevelUp = true;
            });

            FeatTools.Selections.TieflingHeritageSelection.AddToSelection(IsekaiDemonLordHeritage);
        }
    }
}
