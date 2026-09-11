using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class IsekaiAuraSelection
	{
		private static readonly Sprite Icon_FriendlyAura = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_FRIENDLY.png");

		private static readonly Sprite Icon_DarkAura = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_DARK.png");

		private static readonly Sprite Icon_DivineAura = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_DIVINE.png");

		public static void Add()
		{
			BlueprintFeature FriendlyAuraFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("FriendlyAura", "You emit an aura of friendship that cause enemies to subconsciously hold back.\nEnemies within 40 feet take a -4 penalty on attack and damage rolls.", "This creature has a -4 penalty on attack {g|Encyclopedia:Dice}rolls{/g}.", Icon_FriendlyAura, BlueprintAbilityAreaEffect.TargetType.Enemy, new Feet(40f), affectEnemies: true, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AdditionalDamage;
					c.Value = -4;
				});
			});
			BlueprintFeature DarkAuraFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("DarkAura", "You emit an aura of darkness that cause enemies to become uneasy and vulnerable.\nEnemies within 40 feet take a -4 penalty on AC and saving throws.", "This creature has a -4 penalty on AC and saving throws.", Icon_DarkAura, BlueprintAbilityAreaEffect.TargetType.Enemy, new Feet(40f), affectEnemies: true, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AC;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveFortitude;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveReflex;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -4;
				});
			});
			BlueprintFeature DivineAuraFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("DivineAura", "You emit an aura of divinity that cause enemies to be overcome with feelings of futility.\nEnemies within 40 feet take a -4 penalty on all attributes.", "This creature has a -4 penalty on all attributes.", Icon_DivineAura, BlueprintAbilityAreaEffect.TargetType.Enemy, new Feet(40f), affectEnemies: true, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Strength;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Dexterity;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Constitution;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Intelligence;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Wisdom;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Charisma;
					c.Value = -4;
				});
			});
			LocalizedString auraDescription = Helpers.CreateString(Main.IsekaiContext, "AuraSelection.Description", "At 10th level, you are able to choose an aura.");
			Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiAuraSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Otherworldly Aura");
				bp.SetDescription(auraDescription);
				((BlueprintUnitFact)bp).m_Icon = Icon_FriendlyAura;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[3]
				{
					FriendlyAuraFeature.ToReference<BlueprintFeatureReference>(),
					DarkAuraFeature.ToReference<BlueprintFeatureReference>(),
					DivineAuraFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "GodEmperorAuraSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Regal Aura");
				bp.SetDescription(auraDescription);
				((BlueprintUnitFact)bp).m_Icon = Icon_DivineAura;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					DivineAuraFeature.ToReference<BlueprintFeatureReference>(),
					DarkAuraFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "HeroAuraSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Heroic Aura");
				bp.SetDescription(auraDescription);
				((BlueprintUnitFact)bp).m_Icon = Icon_DivineAura;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					FriendlyAuraFeature.ToReference<BlueprintFeatureReference>(),
					DivineAuraFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}
	}
}
