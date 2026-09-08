using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist {
    static class HaxSelection {
        private static readonly Sprite Icon_Hax = AssetLoader.LoadInternal(IsekaiContext, "Features", "ICON_HAX.png");
        public static void Add() {
            var SeriousStrike = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "SeriousStrike", bp => {
                bp.SetName(IsekaiContext, "Serious Strike");
                bp.SetDescription(IsekaiContext, "Your strikes always critically hit and deal double damage!");
                bp.m_Icon = Icon_Hax;
                // Beta 2025-12-19 description; the beta binary still used ContextActionKill.
                // This implements what the description says instead of the one-hit kill.
                bp.AddComponent<SeriousStrikeLogic>();
            });
            var Invincibility = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "Invincibility", bp => {
                bp.SetName(IsekaiContext, "Invincibility");
                bp.SetDescription(IsekaiContext, "You have damage reduction 100/—.");
                bp.m_Icon = Icon_Hax;
                // Beta 2025-12-19: DR 100/— instead of full damage immunity
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Value = 100;
                });
            });
            var FasterThanLight = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "FasterThanLight", bp => {
                bp.SetName(IsekaiContext, "Super Speed");
                bp.SetDescription(IsekaiContext, "Attacks have a 75% chance to automatically miss you.");
                bp.m_Icon = Icon_Hax;
                // Beta 2025-12-19: 75% miss chance instead of guaranteed auto-miss
                bp.AddComponent<SetAttackerMissChance>(c => {
                    c.m_Type = SetAttackerMissChance.Type.All;
                    c.Value = 75;
                    c.Conditions = ActionFlow.EmptyCondition();
                });
            });
            var NoHax = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "NoHax", bp => {
                bp.SetName(IsekaiContext, "None");
                bp.SetDescription(IsekaiContext, "You decide not to abuse your power.");
                bp.m_Icon = null;
            });
            var Ascension = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "Ascension", bp => {
                bp.SetName(IsekaiContext, "Ascension");
                bp.SetDescription(IsekaiContext, "You begin ascension to godhood, you are upgraded to Deity Status increasing all stats by 15.");
                bp.m_Icon = Icon_Hax;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Other;
                    c.Stat = StatType.Strength;
                    c.Value = 15;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Other;
                    c.Stat = StatType.Dexterity;
                    c.Value = 15;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Other;
                    c.Stat = StatType.Constitution;
                    c.Value = 15;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Other;
                    c.Stat = StatType.Intelligence;
                    c.Value = 15;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Other;
                    c.Stat = StatType.Wisdom;
                    c.Value = 15;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Other;
                    c.Stat = StatType.Charisma;
                    c.Value = 15;
                });
            });
            var HaxSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(IsekaiContext, "HaxSelection", bp => {
                bp.SetName(IsekaiContext, "Hax");
                bp.SetDescription(IsekaiContext, "So this is the result of 100 push-ups, 100 sit-ups, 100 squats and a 10km run...");
                bp.m_Icon = Icon_Hax;
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                    SeriousStrike.ToReference<BlueprintFeatureReference>(),
                    Invincibility.ToReference<BlueprintFeatureReference>(),
                    FasterThanLight.ToReference<BlueprintFeatureReference>(),
                    NoHax.ToReference<BlueprintFeatureReference>(),
                    Ascension.ToReference<BlueprintFeatureReference>(),
                };
            });
        }
    }
}
