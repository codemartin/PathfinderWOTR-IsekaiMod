using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class BodyMindAlterSelection
	{
		private static readonly Sprite Icon_MajesticAura = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_MAJESTIC.png");

		private static readonly Sprite Icon_SiphoningAura = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_SIPHONING.png");

		public static void Add()
		{
			BlueprintFeature MajesticAuraFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("MajesticAura", "Allies within 40 feet of you gain a sacred bonus to all attributes equal to 1/2 your character level.", "This character has a sacred bonus to all attributes equal to 1/2 your character level.", Icon_MajesticAura, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Strength;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Dexterity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Constitution;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Intelligence;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Wisdom;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Charisma;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
			});
			BlueprintFeature SiphoningAuraFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("SiphoningAura", "Enemies within 40 feet of you take a penalty on all attributes equal to 1/2 your character level.", "This creature has a penalty on all attributes equal to 1/2 your character level.", Icon_SiphoningAura, BlueprintAbilityAreaEffect.TargetType.Enemy, new Feet(40f), affectEnemies: true, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Strength;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Dexterity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Constitution;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Intelligence;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Wisdom;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Charisma;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.DivStep;
					c.m_StepLevel = -2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "BodyMindAlterSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Alteration of Body and Mind");
				bp.SetDescription(Main.IsekaiContext, "At 10th level, you gain the ability to alter the bodies and minds of those around you.");
				((BlueprintUnitFact)bp).m_Icon = Icon_MajesticAura;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					MajesticAuraFeature.ToReference<BlueprintFeatureReference>(),
					SiphoningAuraFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
			SecretPowerSelection.AddToSelection(MajesticAuraFeature);
			SecretPowerSelection.AddToSelection(SiphoningAuraFeature);
		}
	}
}
