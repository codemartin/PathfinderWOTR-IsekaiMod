using IsekaiMod.Utilities;
using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class PowerLeveling {

        public static void Add() {
            // Icon
            Sprite Icon_DimensionalAnchor = BlueprintTools.GetBlueprint<BlueprintAbility>("c0aa77246b26433fa79c8ac09b1e70d9").m_Icon;

            // Name and Description
            var PowerLevelingName = Helpers.CreateString(IsekaiContext, "PowerLeveling.Name", "Overpowered Ability — Power Leveling");
            var PowerLevelingDesc = Helpers.CreateString(IsekaiContext, "PowerLeveling.Description",
                "You are Overpowered but overly cautious. You use overwhelming force to ensure all enemies you kill are thoroughly defeated."
                + "\nBenefit: When enemies are defeated by you or an ally within 120 feet, your party gains a +2 bonus to attack rolls and all saving throws for 1 minute.");

            // Temporary Buff
            var PowerLevelingTempBuff = Helpers.CreateBlueprint<BlueprintBuff>(IsekaiContext, "PowerLevelingTempBuff", bp => {
                bp.SetName(IsekaiContext, "Overpowered Surge");
                bp.SetDescription(IsekaiContext, "The party gains a +2 bonus to attack rolls and all saving throws for 1 minute.");
                bp.m_Icon = Icon_DimensionalAnchor;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                    c.Value = 2; // +2 attack bonus
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveFortitude;
                    c.Value = 2; // +2 Fortitude save bonus
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveReflex;
                    c.Value = 2;
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveWill;
                    c.Value = 2;
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.Stacking = StackingType.Replace;
            });

            var PowerLevelingBuff = TTCoreExtensions.CreateBuff("PowerLevelingBuff", bp => {
                bp.SetName(PowerLevelingName);
                bp.SetDescription(PowerLevelingDesc);
                bp.m_Icon = Icon_DimensionalAnchor;
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<ApplyPartyBuffOnKill>(c => {
                    c.m_Buff = PowerLevelingTempBuff.ToReference<BlueprintBuffReference>();
                });
            });

            // Area Effect
            var PowerLevelingAura = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(IsekaiContext, "PowerLevelingArea", bp => {
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
                bp.SpellResistance = false;
                bp.AggroEnemies = false;
                bp.AffectEnemies = false;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = new Feet(120);
                bp.Fx = new PrefabLink();
                bp.AddComponent<AbilityAreaEffectBuff>(c => {
                    c.m_Buff = PowerLevelingBuff.ToReference<BlueprintBuffReference>();
                    c.Condition = new ConditionsChecker { Conditions = new Condition[0] }; // Apply unconditionally
                });
            });

            // Aura Buff
            var PowerLevelingAreaBuff = Helpers.CreateBlueprint<BlueprintBuff>(IsekaiContext, "PowerLevelingAreaBuff", bp => {
                bp.SetName(PowerLevelingName);
                bp.SetDescription(PowerLevelingDesc);
                bp.m_Icon = Icon_DimensionalAnchor;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = PowerLevelingAura.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            // Final Feature
            var PowerLevelingFeature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "PowerLevelingFeature", bp => {
                bp.SetName(PowerLevelingName);
                bp.SetDescription(PowerLevelingDesc);
                bp.m_Icon = Icon_DimensionalAnchor;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { PowerLevelingAreaBuff.ToReference<BlueprintUnitFactReference>() };
                });
            });

            OverpoweredAbilitySelection.AddToSelection(PowerLevelingFeature);
        }
    }
}
