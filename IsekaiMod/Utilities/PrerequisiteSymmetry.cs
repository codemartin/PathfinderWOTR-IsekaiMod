using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal static class PrerequisiteSymmetry
{
	private static readonly string[] ExtraSourceSelections = new string[9] { "2dda67424ee8e0b4d83ef01a73ca6bff", "b0a5118b4fb793241bc7042464b23fab", "cc6fda79e8c340b88c84689414a9abbe", "4e7265c0ae1345db90d3375f4ced94cc", "48525e5da45c9c243a343fc6545dbdb9", "43281c3d7fe18cc4d91928395837cd1e", "213a8480d22206b45acbfa0619ca5aaf", "42b781e4375d499383b2602d90661283", "72909a37a1ed5344f88ec9d1d31f5c5b" };

	public static void Apply(IEnumerable<BlueprintFeatureSelection> selections)
	{
		Dictionary<BlueprintGuid, BlueprintProgression> dictionary = new Dictionary<BlueprintGuid, BlueprintProgression>();
		foreach (BlueprintFeatureSelection item in selections.Concat(((IEnumerable<string>)ExtraSourceSelections).Select((Func<string, BlueprintFeatureSelection>)BlueprintTools.GetBlueprint<BlueprintFeatureSelection>)))
		{
			if (item?.m_AllFeatures == null)
			{
				continue;
			}
			BlueprintFeatureReference[] allFeatures = item.m_AllFeatures;
			for (int i = 0; i < allFeatures.Length; i++)
			{
				if (allFeatures[i]?.Get() is BlueprintProgression blueprintProgression)
				{
					dictionary[blueprintProgression.AssetGuid] = blueprintProgression;
				}
			}
		}
		int num = 0;
		foreach (BlueprintProgression progression in dictionary.Values.ToList())
		{
			foreach (PrerequisiteNoFeature requirement in progression.GetComponents<PrerequisiteNoFeature>().ToList())
			{
				if (requirement == null || requirement.Group != Prerequisite.GroupType.All)
				{
					continue;
				}
				BlueprintFeature feature = requirement.Feature;
				if (feature != null && feature != progression && dictionary.TryGetValue(feature.AssetGuid, out var value) && !value.GetComponents<PrerequisiteNoFeature>().Any((PrerequisiteNoFeature existing) => existing?.Feature == progression))
				{
					BlueprintExtentions.AddComponent<PrerequisiteNoFeature>((BlueprintScriptableObject)value, (Action<PrerequisiteNoFeature>)delegate(PrerequisiteNoFeature c)
					{
						c.m_Feature = progression.ToReference<BlueprintFeatureReference>();
						c.Group = Prerequisite.GroupType.All;
						c.CheckInProgression = requirement.CheckInProgression;
					});
					num++;
				}
			}
		}
		if (num > 0)
		{
			((ModContextBase)Main.IsekaiContext).Logger.Log($"PrerequisiteSymmetry: added {num} reverse exclusion(s) between progressions offered to the Isekai Protagonist.");
		}
	}
}
