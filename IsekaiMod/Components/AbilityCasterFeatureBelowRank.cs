using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Components;

[AllowedOn(typeof(BlueprintAbility), false)]
[TypeId("5e0a9d3c7b6f4d18a2c4e9f1d7b83a06")]
public class AbilityCasterFeatureBelowRank : BlueprintComponent, IAbilityCasterRestriction
{
	public BlueprintFeatureReference m_Feature;

	public int MaxRank = 10;

	private static LocalizedString s_AtMaxRank;

	internal static bool Passes(int currentRank, int maxRank)
	{
		return currentRank < maxRank;
	}

	public bool IsCasterRestrictionPassed(UnitEntityData caster)
	{
		BlueprintFeature blueprintFeature = m_Feature?.Get();
		if (caster == null || blueprintFeature == null)
		{
			return true;
		}
		return Passes(caster.Descriptor.Progression.Features.GetRank(blueprintFeature), MaxRank);
	}

	public string GetAbilityCasterRestrictionUIText()
	{
		if (s_AtMaxRank == null)
		{
			s_AtMaxRank = Helpers.CreateString(Main.IsekaiContext, "AbilityCasterFeatureBelowRank.AtMaxRank", "This boost is already at its maximum rank");
		}
		return s_AtMaxRank.ToString();
	}
}
