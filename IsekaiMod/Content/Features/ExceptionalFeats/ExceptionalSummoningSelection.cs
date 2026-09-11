using System;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.ExceptionalFeats
{
	internal class ExceptionalSummoningSelection
	{
		public static void Add()
		{
			Sprite Icon_ExceptionalSummoning = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_EXCEPTIONAL_SUMMONING.png");
			Sprite icon = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_FORBIDDEN_SUMMONING.png");
			Sprite icon2 = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_FEROCIOUS_SUMMONING.png");
			BlueprintFeature MightySummoningFeature = CreateSummonBuffToggleFeature("MightySummoning", "Your summoned creatures get a +5 bonus to maximum Hit Points per character level and a +1 bonus to Attack, damage, and AC per character level.", "This creature gets a +5 bonus to maximum Hit Points per character level and a +1 bonus to Attack, damage, and AC per character level.", Icon_ExceptionalSummoning, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.HitPoints;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
					c.Multiplier = 5;
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.AdditionalDamage;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.AsIs;
				});
			});
			BlueprintFeature MagicalSummoningFeature = CreateSummonBuffToggleFeature("MagicalSummoning", "Your summoned creatures get a +5 bonus to maximum Hit Points per character level and a +1 bonus to spell penetration, spell DC, spell damage, and saving throws per character level.", "This creature gets a +5 bonus to maximum Hit Points per character level and a +1 bonus to spell penetration, spell DC, spell damage, and saving throws per character level.", Icon_ExceptionalSummoning, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.HitPoints;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
					c.Multiplier = 5;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.SpellsOnly = true;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(IncreaseSpellDamage c)
				{
					c.DamageBonus = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.SaveFortitude;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.SaveReflex;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.SaveWill;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.AsIs;
				});
			});
			BlueprintFeature ForbiddenSummoningFeature = CreateSummonBuffToggleFeature("ForbiddenSummoning", "Your summoned creatures gain a +10 bonus to hit points per character level and a +1 bonus to all attributes per character level.", "This creature gains a +10 bonus to maximum Hit Points per character level and a +1 bonus to all attributes per character level.", icon, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.HitPoints;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
					c.Multiplier = 10;
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.Strength;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.Dexterity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.Constitution;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.Intelligence;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.Wisdom;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.Charisma;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.AsIs;
				});
			});
			ForbiddenSummoningFeature.AddComponent(delegate(PrerequisiteFeature c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_Feature = MightySummoningFeature.ToReference<BlueprintFeatureReference>();
			});
			ForbiddenSummoningFeature.AddComponent(delegate(PrerequisiteFeature c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_Feature = MagicalSummoningFeature.ToReference<BlueprintFeatureReference>();
			});
			BlueprintFeature blueprintFeature = CreateSummonBuffToggleFeature("FerociousSummoning", "Your summoned creatures have 2 additional attacks and gain a +10 bonus to speed. It also gains 1d6 sneak attack per character level.", "This creature has 2 additional attacks and gains a +10 bonus to speed. It also gains 1d6 sneak attack per character level.", icon2, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
					c.Haste = false;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Stat = StatType.SneakAttack;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.AsIs;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteFeature c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_Feature = ForbiddenSummoningFeature.ToReference<BlueprintFeatureReference>();
			});
			BlueprintFeatureReference[] ExceptionalSummoingFeatures = new BlueprintFeatureReference[4]
			{
				MightySummoningFeature.ToReference<BlueprintFeatureReference>(),
				MagicalSummoningFeature.ToReference<BlueprintFeatureReference>(),
				ForbiddenSummoningFeature.ToReference<BlueprintFeatureReference>(),
				blueprintFeature.ToReference<BlueprintFeatureReference>()
			};
			LocalizedString ExceptionalSummoningDesc = Helpers.CreateString(Main.IsekaiContext, "ExceptionalSummoningSelection.Description", "Your summons become more powerful as you increase your level.");
			BlueprintFeatureSelection selection = Helpers.CreateBlueprint(Main.IsekaiContext, "ExceptionalSummoningSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Exceptional Summoning");
				bp.SetDescription(ExceptionalSummoningDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_ExceptionalSummoning;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = ExceptionalSummoingFeatures;
				bp.m_Features = ExceptionalSummoingFeatures;
			});
			BlueprintFeatureSelection bonusSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "ExceptionalSummoningBonusSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Exceptional Summoning");
				bp.SetDescription(ExceptionalSummoningDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_ExceptionalSummoning;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = ExceptionalSummoingFeatures;
				bp.m_Features = ExceptionalSummoingFeatures;
			});
			ExceptionalFeatSelection.AddToSelection(selection, bonusSelection);
		}

		private static BlueprintFeature CreateSummonBuffToggleFeature(string name, string description, string descriptionBuff, Sprite icon, Action<BlueprintBuff> summonBuffEffect = null)
		{
			string displayName = string.Concat(name.Select((char x) => (!char.IsUpper(x)) ? x.ToString() : (" " + x))).TrimStart(' ');
			LocalizedString displayDesc = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			LocalizedString buffDesc = Helpers.CreateString(Main.IsekaiContext, name + "SummonBuff.Description", descriptionBuff);
			BlueprintBuff summonBuff = TTCoreExtensions.CreateBuff(name + "SummonBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(buffDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.Stacking = StackingType.Replace;
				bp.IsClassFeature = true;
			});
			summonBuffEffect?.Invoke(summonBuff);
			BlueprintBuff buff = TTCoreExtensions.CreateBuff(name + "Buff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				((BlueprintUnitFact)bp).m_Description = StaticReferences.Strings.Null;
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
			});
			BlueprintActivatableAbility ability = TTCoreExtensions.CreateActivatableAbility(name + "Ability", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(displayDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
			});
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(displayDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ability.ToReference<BlueprintUnitFactReference>() };
				});
				bp.ReapplyOnLevelUp = true;
			});
			buff.AddComponent(delegate(OnSpawnBuff c)
			{
				c.m_IfHaveFact = feature.ToReference<BlueprintFeatureReference>();
				c.m_buff = summonBuff.ToReference<BlueprintBuffReference>();
				c.IsInfinity = true;
			});
			return feature;
		}
	}
}
