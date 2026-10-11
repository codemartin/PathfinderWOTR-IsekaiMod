using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Constellations;

[TypeId("6128566934f248fcaa6c0bade4eac5e3")]
public class ConditionConstellationsEnabled : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return ConstellationSettings.Enabled;
	}

	public override string GetConditionCaption()
	{
		return "The Constellations and Cosmic Sponsorships are enabled";
	}
}
