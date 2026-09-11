using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class OverpoweredAbilitySelection
	{
		private static readonly Sprite Icon_TrickFate = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd")).m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "OverpoweredAbilitySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability");
				bp.SetDescription(Main.IsekaiContext, "As a being from another world, you are granted an Overpowered Ability which no ordinary being in this world possess.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrickFate;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "OverpoweredAbilitySelectionOverlord", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Additional Overpowered Ability");
				bp.SetDescription(Main.IsekaiContext, "Overlords get to select additional Overpowered Abilities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrickFate;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
			BlueprintFeatureSelection OverpoweredAbilityMythicSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "OverpoweredAbilityMythicSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Mythic Overpowered Ability");
				bp.SetDescription(Main.IsekaiContext, "You use your mythic powers to gain an additional Overpowered Ability.\nSource: Isekai Mod");
				((BlueprintUnitFact)bp).m_Icon = Icon_TrickFate;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
			if (!Main.IsekaiContext.AddedContent.MultipleMythicOPAbility)
			{
				OverpoweredAbilityMythicSelection.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = OverpoweredAbilityMythicSelection.ToReference<BlueprintFeatureReference>();
				});
			}
			if (Main.IsekaiContext.AddedContent.RestrictMythicOPAbility)
			{
				OverpoweredAbilityMythicSelection.AddPrerequisite(delegate(PrerequisiteClassLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.Level = 1;
				});
			}
			FeatTools.Selections.MythicAbilitySelection.AddToSelection(OverpoweredAbilityMythicSelection);
			FeatTools.Selections.ExtraMythicAbilityMythicFeat.AddToSelection(OverpoweredAbilityMythicSelection);
		}

		public static void AddToSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection[] array = new BlueprintFeatureSelection[3]
			{
				BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection"),
				BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelectionOverlord"),
				BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilityMythicSelection")
			};
			foreach (BlueprintFeatureSelection obj in array)
			{
				obj.m_Features = obj.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
				obj.m_AllFeatures = obj.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			}
		}

		public static void AddToNonMythicSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection[] array = new BlueprintFeatureSelection[2]
			{
				BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection"),
				BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelectionOverlord")
			};
			foreach (BlueprintFeatureSelection obj in array)
			{
				obj.m_Features = obj.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
				obj.m_AllFeatures = obj.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			}
		}
	}
}
