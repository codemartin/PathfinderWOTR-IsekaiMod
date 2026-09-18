using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class SpecialPowerSelection
	{
		private static readonly Sprite Icon_ForetellAidBuff = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2")).m_Icon;

		public static void Add()
		{
			BlueprintFeatureSelection bp = Helpers.CreateBlueprint(Main.IsekaiContext, "SpecialPowerMartialSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Martial Cheats & Physical Prowess");
				blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "Special powers focused on offensive physical dominance, impossible critical strikes, and martial supremacy.");
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_ForetellAidBuff;
				blueprintFeatureSelection.Ranks = 1;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.m_Features = new BlueprintFeatureReference[0];
				blueprintFeatureSelection.m_AllFeatures = new BlueprintFeatureReference[0];
			});
			BlueprintFeatureSelection bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "SpecialPowerDefenseSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Defensive Wards & Survival");
				blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "Special powers focused on cellular regeneration, impenetrable armor, energy immunities, and absolute defense.");
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_ForetellAidBuff;
				blueprintFeatureSelection.Ranks = 1;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.m_Features = new BlueprintFeatureReference[0];
				blueprintFeatureSelection.m_AllFeatures = new BlueprintFeatureReference[0];
			});
			BlueprintFeatureSelection bp3 = Helpers.CreateBlueprint(Main.IsekaiContext, "SpecialPowerMagicSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Arcane Supremacy & Metamagic");
				blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "Special powers focused on overwhelming magical potency, spell mastery, sneaky spellcasting, and reality-bending domains.");
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_ForetellAidBuff;
				blueprintFeatureSelection.Ranks = 1;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.m_Features = new BlueprintFeatureReference[0];
				blueprintFeatureSelection.m_AllFeatures = new BlueprintFeatureReference[0];
			});
			BlueprintFeatureSelection bp4 = Helpers.CreateBlueprint(Main.IsekaiContext, "SpecialPowerAuthoritySelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Auras, Authority & Archetypes");
				blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "Special powers focused on sovereign presence, mercantile influence, mythical summonings, and defining protagonist philosophies.");
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_ForetellAidBuff;
				blueprintFeatureSelection.Ranks = 1;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.m_Features = new BlueprintFeatureReference[0];
				blueprintFeatureSelection.m_AllFeatures = new BlueprintFeatureReference[0];
			});
			BlueprintFeatureReference[] subSelections = new BlueprintFeatureReference[4]
			{
				bp.ToReference<BlueprintFeatureReference>(),
				bp2.ToReference<BlueprintFeatureReference>(),
				bp3.ToReference<BlueprintFeatureReference>(),
				bp4.ToReference<BlueprintFeatureReference>()
			};
			Helpers.CreateBlueprint(Main.IsekaiContext, "SpecialPowerSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Special Power");
				blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "As you increase your level, you gain special powers that allow you to wreck your enemies more easily.");
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_ForetellAidBuff;
				blueprintFeatureSelection.Ranks = 1;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.m_Features = (BlueprintFeatureReference[])subSelections.Clone();
				blueprintFeatureSelection.m_AllFeatures = (BlueprintFeatureReference[])subSelections.Clone();
			});
			BlueprintFeatureSelection SpecialPowerMythicSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "SpecialPowerMythicSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Mythic Special Power");
				blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "You use your mythic powers to gain an additional special power.\nSource: Isekai Mod");
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_ForetellAidBuff;
				blueprintFeatureSelection.Ranks = 1;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.m_Features = (BlueprintFeatureReference[])subSelections.Clone();
				blueprintFeatureSelection.m_AllFeatures = (BlueprintFeatureReference[])subSelections.Clone();
			});
			if (!Main.IsekaiContext.AddedContent.MultipleMythicSpecialPower)
			{
				SpecialPowerMythicSelection.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = SpecialPowerMythicSelection.ToReference<BlueprintFeatureReference>();
				});
			}
			if (Main.IsekaiContext.AddedContent.RestrictMythicSpecialPower)
			{
				SpecialPowerMythicSelection.AddPrerequisite(delegate(PrerequisiteClassLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.Level = 1;
				});
			}
			FeatTools.Selections.MythicAbilitySelection.AddToSelection(SpecialPowerMythicSelection);
			FeatTools.Selections.ExtraMythicAbilityMythicFeat.AddToSelection(SpecialPowerMythicSelection);
		}

		public static void AddToMartialSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerMartialSelection");
			modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
		}

		public static void AddToDefenseSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerDefenseSelection");
			modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
		}

		public static void AddToMagicSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerMagicSelection");
			modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
		}

		public static void AddToAuthoritySelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerAuthoritySelection");
			modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
		}

		public static void AddToSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection[] array = new BlueprintFeatureSelection[2]
			{
				BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection"),
				BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerMythicSelection")
			};
			foreach (BlueprintFeatureSelection obj in array)
			{
				obj.m_Features = obj.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
				obj.m_AllFeatures = obj.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			}
		}

		public static void AddToNonMythicSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
			modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
			modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
		}

		public static BlueprintFeatureSelection Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
		}

		public static BlueprintFeatureSelection GetMartial()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerMartialSelection");
		}

		public static BlueprintFeatureSelection GetDefense()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerDefenseSelection");
		}

		public static BlueprintFeatureSelection GetMagic()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerMagicSelection");
		}

		public static BlueprintFeatureSelection GetAuthority()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerAuthoritySelection");
		}
	}
}
