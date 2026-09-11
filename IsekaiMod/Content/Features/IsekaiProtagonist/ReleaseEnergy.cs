using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal static class ReleaseEnergy
	{
		private static readonly Sprite Icon_AngelfireApostleChannel = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9d30d6cc7bfcda44aab7505f7ed3f933")).m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "ReleaseEnergy", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Release Energy");
				bp.SetDescription(Main.IsekaiContext, "The Isekai Protagonist is able to channel both positive energy and negative energy.\nSo this what happens when you channel one percent of your power...");
				((BlueprintUnitFact)bp).m_Icon = Icon_AngelfireApostleChannel;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[2]
					{
						BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "IsekaiChannelPositiveEnergyFeature"),
						BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "IsekaiChannelNegativeEnergyFeature")
					};
				});
			});
		}
	}
}
