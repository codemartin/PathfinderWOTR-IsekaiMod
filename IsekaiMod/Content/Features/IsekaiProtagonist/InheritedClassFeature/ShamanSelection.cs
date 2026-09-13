using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class ShamanSelection
	{
		private static BlueprintFeatureSelection myfeat;

		private static BlueprintFeatureSelection isekaiHex;

		private static BlueprintFeatureSelection isekaiSpirit;

		public static void Configure()
		{
			isekaiHex = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHexSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Hex");
				bp.SetDescription(Main.IsekaiContext, "Gain an additional Hex.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)ShamanLegacy.shamanHex).m_Icon;
				bp.Ranks = 8;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = ShamanLegacy.shamanHex.m_AllFeatures;
				bp.m_Features = bp.m_AllFeatures;
			});
			MirroredSelections.Register(isekaiHex, ShamanLegacy.shamanHex);
			isekaiSpirit = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiSpiritSelection", delegate(BlueprintFeatureSelection bp)
			{
				((BlueprintUnitFact)bp).m_DisplayName = ((BlueprintUnitFact)ShamanLegacy.shamanSpirit).m_DisplayName;
				((BlueprintUnitFact)bp).m_Description = ((BlueprintUnitFact)ShamanLegacy.shamanSpirit).m_Description;
				bp.IgnorePrerequisites = true;
				bp.Ranks = 4;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = ShamanLegacy.shamanSpirit.m_AllFeatures;
				bp.m_Features = bp.m_AllFeatures;
			});
			MirroredSelections.Register(isekaiSpirit, ShamanLegacy.shamanSpirit);
			myfeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiShamanSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Spirit Blessing");
				bp.SetDescription(Main.IsekaiContext, "As you grow so does your connection to the spirits and the power you derive from them. \nAllowing you to connect with more spirits or gain more powers from them.");
				bp.Ranks = 8;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					isekaiHex.ToReference<BlueprintFeatureReference>(),
					isekaiSpirit.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}

		public static BlueprintFeatureSelection Get()
		{
			if (myfeat != null)
			{
				return myfeat;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiShamanSelection");
		}

		public static BlueprintFeatureSelection GetHex()
		{
			if (isekaiHex != null)
			{
				return isekaiHex;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiHexSelection");
		}

		public static BlueprintFeatureSelection GetSpirit()
		{
			if (isekaiSpirit != null)
			{
				return isekaiSpirit;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiSpiritSelection");
		}

		public static void PatchPrerequisiteCompatibility()
		{
			PrerequisiteAlternatives.Add(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("08d9f686b2944ba6b3f7763882c0ded4"), ShamanLegacy.shamanHex, GetHex());
			PrerequisiteAlternatives.Add(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("2faa80662a56ab644aec2f875a68597f"), ShamanLegacy.shamanSpirit, GetSpirit());
		}
	}
}
