using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class SignatureMoveSelection
	{
		private static readonly Sprite Icon_MutagenResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("3b163587f010382408142fc8a97852b6")?.m_Icon;

		private static readonly Sprite Icon_SwordSaintWeaponMastery = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("5b31af13868166d4c9bb452f19277f19"))?.m_Icon;

		private static readonly BlueprintFeature SneakAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");

		public static void Add()
		{
			BlueprintFeatureReference SneakAttackRef = SneakAttack?.ToReference<BlueprintFeatureReference>();
			BlueprintFeature SignatureAttack = Helpers.CreateBlueprint(Main.IsekaiContext, "SignatureAttack", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Signature Attack");
				bp.SetDescription(Main.IsekaiContext, "You gain a luck bonus to {g|Encyclopedia:BAB}attack{/g} and damage rolls equal to 1/2 your character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_SwordSaintWeaponMastery;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalDamage;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.OnePlusDiv2;
				});
				bp.ReapplyOnLevelUp = true;
			});
			BlueprintFeature SignatureAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SignatureAbility", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Signature Ability");
				bp.SetDescription(Main.IsekaiContext, "You gain a bonus to spell DC and spell hit point damage equal to 1/2 your character level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_MutagenResource;
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
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.OnePlusDiv2;
				});
				bp.ReapplyOnLevelUp = true;
			});
			BlueprintFeature SignatureStrike = Helpers.CreateBlueprint(Main.IsekaiContext, "SignatureStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Signature Strike");
				bp.SetDescription(Main.IsekaiContext, "You gain an additional 1d6 sneak attack damage equal to 1/2 your Isekai Protagonist class level.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)SneakAttack)?.m_Icon ?? Icon_SwordSaintWeaponMastery;
				bp.ReapplyOnLevelUp = true;
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 2;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 4;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 6;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 8;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 10;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 12;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 14;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 16;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 18;
					c.m_Feature = SneakAttackRef;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 20;
					c.m_Feature = SneakAttackRef;
				});
			});
			LocalizedString SignatureMoveSelectionDesc = Helpers.CreateString(Main.IsekaiContext, "SignatureMoveSelection.Description", "At 6th level, you choose a signature move.");
			Helpers.CreateBlueprint(Main.IsekaiContext, "SignatureMoveSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Signature Move");
				bp.SetDescription(SignatureMoveSelectionDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_SwordSaintWeaponMastery;
				bp.IgnorePrerequisites = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[3]
				{
					SignatureAttack.ToReference<BlueprintFeatureReference>(),
					SignatureAbility.ToReference<BlueprintFeatureReference>(),
					SignatureStrike.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "SignatureMoveBonusSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Another Signature Move");
				bp.SetDescription(SignatureMoveSelectionDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_SwordSaintWeaponMastery;
				bp.IgnorePrerequisites = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[3]
				{
					SignatureAttack.ToReference<BlueprintFeatureReference>(),
					SignatureAbility.ToReference<BlueprintFeatureReference>(),
					SignatureStrike.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}
	}
}
