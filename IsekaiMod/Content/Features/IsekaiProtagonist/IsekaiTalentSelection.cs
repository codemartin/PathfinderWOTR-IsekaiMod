using System.Collections.Generic;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class IsekaiTalentSelection
	{
		private static readonly BlueprintFeature CripplingStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("b696bd7cb38da194fa3404032483d1db");

		private static readonly BlueprintFeature DispellingAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("1b92146b8a9830d4bb97ab694335fa7c");

		private static readonly BlueprintFeature ConfoundingBlades = BlueprintTools.GetBlueprint<BlueprintFeature>("ce72662a812b1f242849417b2c784b5e");

		private static readonly BlueprintFeature Opportunist = BlueprintTools.GetBlueprint<BlueprintFeature>("5bb6dc5ce00550441880a6ff8ad4c968");

		private static readonly BlueprintFeature FastStealth = BlueprintTools.GetBlueprint<BlueprintFeature>("97a6aa2b64dd21a4fac67658a91067d7");

		private static readonly BlueprintFeature FocusingAttackConfused = BlueprintTools.GetBlueprint<BlueprintFeature>("955ff81c596c1c3489406d03e81e6087");

		private static readonly BlueprintFeature FocusingAttackShaken = BlueprintTools.GetBlueprint<BlueprintFeature>("791f50e199d069d4f8e933996a2ce054");

		private static readonly BlueprintFeature FocusingAttackSickened = BlueprintTools.GetBlueprint<BlueprintFeature>("79475c263e538c94f8e23907bd570a35");

		public static void Add()
		{
			List<BlueprintFeatureReference> talents = new List<BlueprintFeatureReference>();
			BlueprintFeature[] array = new BlueprintFeature[8] { CripplingStrike, DispellingAttack, ConfoundingBlades, Opportunist, FastStealth, FocusingAttackConfused, FocusingAttackShaken, FocusingAttackSickened };
			foreach (BlueprintFeature blueprintFeature in array)
			{
				if (blueprintFeature != null)
				{
					talents.Add(blueprintFeature.ToReference<BlueprintFeatureReference>());
				}
			}
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiTalentSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Rogue Talents");
				bp.SetDescription(Main.IsekaiContext, "You can select a rogue talent from a limited set of rogue talents, ignoring any prerequisites for these talents.");
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.Group = FeatureGroup.RogueTalent;
				bp.Group2 = FeatureGroup.None;
				bp.IgnorePrerequisites = true;
				bp.m_AllFeatures = talents.ToArray();
				bp.m_Features = bp.m_AllFeatures;
			}));
		}

		public static BlueprintFeatureSelection Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiTalentSelection");
		}

		public static BlueprintFeatureReference GetReference()
		{
			return Get().ToReference<BlueprintFeatureReference>();
		}
	}
}
