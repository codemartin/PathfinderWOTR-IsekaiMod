using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class BarrierSelection
	{
		private static readonly Sprite Icon_GoldBarrier = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_BARRIER_GOLD.png");

		private static readonly Sprite Icon_VoidBarrier = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_BARRIER_VOID.png");

		private static readonly BlueprintBuff HeroismGreaterBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b8da3ec045ec04845a126948e1f4fc1a");

		public static void Add()
		{
			BlueprintFeature GoldBarrierHeroism = Helpers.CreateBlueprint(Main.IsekaiContext, "GoldBarrierHeroism", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Improved Gold Barrier");
				bp.SetDescription(Main.IsekaiContext, "At 10th level, Gold Barrier now also grants Greater Heroism to your allies.");
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
			});
			BlueprintFeature GoldBarrierFastHealing = Helpers.CreateBlueprint(Main.IsekaiContext, "GoldBarrierFastHealing", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Greater Gold Barrier");
				bp.SetDescription(Main.IsekaiContext, "At 12th level, Gold Barrier now also grants fast healing equal to your character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
			});
			BlueprintBuff GoldBarrierFastHealingBuff = TTCoreExtensions.CreateBuff("GoldBarrierFastHealingBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Greater Gold Barrier");
				bp.SetDescription(Main.IsekaiContext, "This character has fast healing equal to your character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 0;
					c.Bonus = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
			});
			BlueprintFeature GoldBarrierResistance = Helpers.CreateBlueprint(Main.IsekaiContext, "GoldBarrierResistance", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Grand Gold Barrier");
				bp.SetDescription(Main.IsekaiContext, "At 15th level, Gold Barrier now also grants DR/- and resistance against all elements (acid, cold, electricity, fire, and sonic) equal to 1/2 your character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
			});
			BlueprintBuff GoldBarrierResistanceBuff = TTCoreExtensions.CreateBuff("GoldBarrierResistanceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Grand Gold Barrier");
				bp.SetDescription(Main.IsekaiContext, "This character has DR/- and resistance against all elements (acid, cold, electricity, fire, and sonic) equal to 1/2 your character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Sonic;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
			});
			LocalizedString GoldBarrierDescription = Helpers.CreateString(Main.IsekaiContext, "GoldBarrier.Description", "Allies within 40 feet of you gain a sacred bonus to AC and saving throws equal to 1/2 your character level.");
			BlueprintBuff GoldBarrierBuff = TTCoreExtensions.CreateBuff("GoldBarrierBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Gold Barrier");
				bp.SetDescription(Main.IsekaiContext, "This character has a sacred bonus to AC and saving throws equal to 1/2 your character level.");
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
			});
			BlueprintAbilityAreaEffect GoldBarrierArea = Helpers.CreateBlueprint(Main.IsekaiContext, "GoldBarrierArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(40f);
				bp.Fx = new PrefabLink();
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = GoldBarrierBuff.ToReference<BlueprintBuffReference>(),
						Permanent = true,
						DurationValue = Values.Duration.Zero
					}, new Conditional
					{
						ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterHasFact contextConditionCasterHasFact)
						{
							contextConditionCasterHasFact.m_Fact = GoldBarrierHeroism.ToReference<BlueprintUnitFactReference>();
							contextConditionCasterHasFact.Not = false;
						}),
						IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
						{
							contextActionApplyBuff.m_Buff = HeroismGreaterBuff.ToReference<BlueprintBuffReference>();
							contextActionApplyBuff.Permanent = true;
							contextActionApplyBuff.DurationValue = Values.Duration.Zero;
						}),
						IfFalse = ActionFlow.DoNothing()
					}, new Conditional
					{
						ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterHasFact contextConditionCasterHasFact)
						{
							contextConditionCasterHasFact.m_Fact = GoldBarrierFastHealing.ToReference<BlueprintUnitFactReference>();
							contextConditionCasterHasFact.Not = false;
						}),
						IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
						{
							contextActionApplyBuff.m_Buff = GoldBarrierFastHealingBuff.ToReference<BlueprintBuffReference>();
							contextActionApplyBuff.Permanent = true;
							contextActionApplyBuff.DurationValue = Values.Duration.Zero;
						}),
						IfFalse = ActionFlow.DoNothing()
					}, new Conditional
					{
						ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterHasFact contextConditionCasterHasFact)
						{
							contextConditionCasterHasFact.m_Fact = GoldBarrierResistance.ToReference<BlueprintUnitFactReference>();
							contextConditionCasterHasFact.Not = false;
						}),
						IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
						{
							contextActionApplyBuff.m_Buff = GoldBarrierResistanceBuff.ToReference<BlueprintBuffReference>();
							contextActionApplyBuff.Permanent = true;
							contextActionApplyBuff.DurationValue = Values.Duration.Zero;
						}),
						IfFalse = ActionFlow.DoNothing()
					});
					c.UnitExit = Helpers.CreateActionList(new ContextActionRemoveBuff
					{
						m_Buff = GoldBarrierBuff.ToReference<BlueprintBuffReference>(),
						RemoveRank = false,
						ToCaster = false,
						OnlyFromCaster = true
					}, new ContextActionRemoveBuff
					{
						m_Buff = HeroismGreaterBuff.ToReference<BlueprintBuffReference>(),
						RemoveRank = false,
						ToCaster = false,
						OnlyFromCaster = true
					}, new ContextActionRemoveBuff
					{
						m_Buff = GoldBarrierFastHealingBuff.ToReference<BlueprintBuffReference>(),
						RemoveRank = false,
						ToCaster = false,
						OnlyFromCaster = true
					}, new ContextActionRemoveBuff
					{
						m_Buff = GoldBarrierResistanceBuff.ToReference<BlueprintBuffReference>(),
						RemoveRank = false,
						ToCaster = false,
						OnlyFromCaster = true
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = ActionFlow.DoNothing();
				});
			});
			BlueprintBuff GoldBarrierAreaBuff = TTCoreExtensions.CreateBuff("GoldBarrierAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Gold Barrier");
				bp.SetDescription(GoldBarrierDescription);
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = GoldBarrierArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			BlueprintActivatableAbility GoldBarrierAbility = TTCoreExtensions.CreateActivatableAbility("GoldBarrierAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Gold Barrier");
				bp.SetDescription(GoldBarrierDescription);
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
				bp.m_Buff = GoldBarrierAreaBuff.ToReference<BlueprintBuffReference>();
				bp.DoNotTurnOffOnRest = true;
			});
			BlueprintFeature GoldBarrierFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "GoldBarrierFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Gold Barrier");
				bp.SetDescription(GoldBarrierDescription);
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { GoldBarrierAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			BlueprintFeature VoidBarrierFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("VoidBarrier", "Allies within 40 feet of you gain a profane bonus to AC and saving throws equal to 1/2 your character level.", "This character has a profane bonus to AC and saving throws equal to 1/2 your character level.", Icon_VoidBarrier, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveFortitude;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveReflex;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveWill;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "BarrierSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Energy Barrier");
				bp.SetDescription(Main.IsekaiContext, "At 7th level, you are able to channel your energy to form a solid barrier to shield allies from physical and magical attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_GoldBarrier;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					GoldBarrierFeature.ToReference<BlueprintFeatureReference>(),
					VoidBarrierFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
			SecretPowerSelection.AddToSelection(GoldBarrierFeature);
			SecretPowerSelection.AddToSelection(VoidBarrierFeature);
		}
	}
}
